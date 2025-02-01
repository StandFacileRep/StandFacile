/*****************************************************************************
	NomeFile : StandCommonSrc/Printer_Windows.cs
    Data	 : 01.02.2025
	Autore   : Mauro Artuso

	Descrizione :
	Questo file contiene la classe per la gestione della stampante
	Windows generica mediante Canvas ed installata con il suo driver

  	consigliato Soft Font Lucida Console, size 12

    stampante pdf 210mm = 8,27"  * 600 dpi = 4958pz di larghezza
                  297mm = 11,69" * 600 dpi = 7016pz di lunghezza

    STAMPANTE_TM_T88            : 180dpi, carta 80mm 521 dot
    STAMPANTE_TM_L90            : 203dpi, carta 80mm 640 dot
    STAMPANTE_VRETTI_POS-80C    : 203dpi, carta 72mm 576 dot
    
    dpi = dotNum*25.4/paperWidth

****************************************************************************/

using System;
using System.IO;
using System.Drawing;
using System.Drawing.Printing;

using static StandCommonFiles.CommonCl;
using static StandCommonFiles.ComDef;
using static StandCommonFiles.LogServer;

using StandFacile;
using static StandFacile.glb;
using System.Threading;

#if STAND_CUCINA
using static StandFacile.Define; // serve a StandCucina
#else
using static StandFacile.dBaseIntf;
#endif

namespace StandCommonFiles
{
    class Printer_Windows
    {
#pragma warning disable IDE0044
#pragma warning disable IDE0059
#pragma warning disable IDE1006

        const int BARCODE_HEIGHT = 100;
        //const String WIDE_CONST_STRING = "*********_*********_********";

        static bool _bIsDati, _bIsTicket;
        static bool _bLogo;
        static bool _bCopiaCucina;
        static bool _bStampaBarcode, _bStampaBarcodePrev;
        static bool _bTicketNumFound;
        static bool _bSkipNumeroScontrino, _bLogoPrinted;

        /// <summary>se true evita la stampa dello scontrino</summary>
        static bool _bSkipTicketPrint = false;

        /// <summary>imposta l'intervallo tra le stampe</summary>
        public static int iPrint_WaitInterval = 200;

        static float _fLeftMargin, _fLogoCenter;
        static float _fCanvasVertPos;

        static int _iPageRows, _iGruppoStampa;

        static Char[] _cGruppoStampa = { '9', '9' };
        static String _sDataStr = "";
        static String _sOrdineNum = "";

        static TErrMsg _WrnMsg;

        static Font _printFont = new Font("Lucida Console", 10.25f, FontStyle.Regular);
        static Font _LogoFont = new Font("Lucida Console", 10.25f, FontStyle.Regular);

        static StreamReader _fileToPrint;

        static TWinPrinterParams _sWinPrinterParams;
        static String _sFileToPrintParam;

        // imposta i margini di stampa altrimenti il default è 1 pollice
        static readonly Margins margins = new Margins(0, 0, 20, 20);

        /// <summary>
        /// variabile per gestione zoom scontrino largo 32/28 caratteri rispetto alle copie che non hanno prezzi<br/>
        /// vale 1.0f per larghezza di 28 caratteri
        /// </summary>
        static float _fReceiptVsCopyZoom;
        static float _fHZoom, _fVZoom;
        static float _fH_px_to_gu, _fV_px_to_gu; // conversion between pixels and graph units

        static string sPrevPrinter;

        /// <summary>
        /// imposta _bSkipTicketPrint
        /// </summary>
        public static void SetSkipTicketPrint(bool bParam) { _bSkipTicketPrint = bParam; }

        /// <summary>
        /// imposta _bSkipNumeroScontrino
        /// </summary>
        public static void SetSkipNumeroScontrino(bool bParam) { _bSkipNumeroScontrino = bParam; }

        /****************************************************
            STAMPA su stampante gestita da driver windows
         ****************************************************/
        public static void PrintFile(String sFileToPrintParam)
        {
            PrintFile(sFileToPrintParam, sGlbWinPrinterParams, NUM_EDIT_GROUPS); // OK
        }

        /// <summary>
        ///  STAMPA su stampante gestita da driver windows,<br/>
        ///  diversa per le 8 copie + scontrino principale<br/>
        ///  iPrinterIndex da 0 a NUM_EDIT_GROUPS - 1 identifica stampante copie,<br/>
        ///  NUM_EDIT_GROUPS == NUM_EDIT_GROUPS  identifica stampante Receipt,<br/>
        ///  NUM_EDIT_GROUPS == NUM_EDIT_GROUPS + 1 identifica stampante Messaggi
        /// </summary>
        /// <param name="sFileToPrintParam"></param>
        /// <param name="sWinPrinterParams"></param>
        /// <param name="iPrinterIndex"></param>
        /// <param name="sPrinterNameParam"></param>
        public static void PrintFile(String sFileToPrintParam, TWinPrinterParams sWinPrinterParams, int iPrinterIndex, string sPrinterNameParam = "")
        {
            int iPos;
            string sTmp;

            _bIsDati = false;
            _bCopiaCucina = false;
            _bIsTicket = false;
            _bStampaBarcodePrev = false;
            _bLogo = true;

            _fReceiptVsCopyZoom = 1.0f;

            // init
            _sOrdineNum = "0123"; // per sampleText
            _sDataStr = GetActualDate().ToString("ddMMyy");

            _sWinPrinterParams = sWinPrinterParams;
            _sFileToPrintParam = sFileToPrintParam;

            sTmp = Path.GetFileName(_sFileToPrintParam);

            _iGruppoStampa = 0;

            if (sTmp.Contains("_TT") || sTmp.Contains("_TN"))
            {
#if STANDFACILE || STAND_MONITOR
                // gruppi in fase di emissione ordine
                if (SF_Data.bPrevendita)
                {
                    _iGruppoStampa = NUM_PRE_SALE_GRP;
                }
                else if (IsBitSet(SF_Data.iStatusReceipt, BIT_CARICATO_DA_WEB))
                {
                    _iGruppoStampa = NUM_WEB_SALE_GRP;
                }
#endif
                if (sTmp.Contains("_TT"))
                {
                    _bIsTicket = true;

                    _bTicketNumFound = false; // forza ricerca stringa

                    _fReceiptVsCopyZoom = sWinPrinterParams.bChars33 ? (28.0f / 33.0f) : 1.0f;
                }
                else
                {
                    _bTicketNumFound = true; // necessario nella copia Receipt NoPrices
                }
            }
            else if (sTmp.Contains("_CT") || sTmp.Contains("CT_")) // copia
            {
                _bTicketNumFound = false;

                iPos = sTmp.IndexOf("_G");

                _iGruppoStampa = Convert.ToInt32(sTmp[iPos + 2]) - 48;
            }
            else if (_sFileToPrintParam.Contains(NOME_FILE_SAMPLE_TEXT))
            {
                _fReceiptVsCopyZoom = sWinPrinterParams.bChars33 ? (28.0f / 33.0f) : 1.0f;
            }
            else
            {
                _bLogo = false;
            }

            _bTicketNumFound = true;

#if STANDFACILE || STAND_MONITOR
            int iDebug1, iDebug2;

            iDebug1 = DB_Data.iStatusReceipt; // 4 BIT_CARICATO_DA_PREVENDITA
            iDebug2 = SF_Data.iStatusReceipt; // 2 BIT_EMESSO_IN_PREVENDITA

            // aggiunto bUSA_NDB() per ignorare il flag del listino con SQLite
            _bStampaBarcode = IsBitSet(SF_Data.iBarcodeRichiesto, _iGruppoStampa) && bUSA_NDB();

            // OK verificato
            _bStampaBarcodePrev = SF_Data.bPrevendita || IsBitSet(DB_Data.iStatusReceipt, BIT_EMESSO_IN_PREVENDITA) && !IsBitSet(SF_Data.iStatusReceipt, BIT_CARICATO_DA_PREVENDITA);

            //numero di colonne ridotto -> font più grande
            if (_sFileToPrintParam.Contains(NOME_FILE_STAMPA_LOC_TMP) || _sFileToPrintParam.Contains(NOME_FILE_STAMPA_LOC_RID_TMP) ||
                _sFileToPrintParam.Contains("_Dati") || _sFileToPrintParam.Contains("Listino"))
            {
                _bIsDati = true;

                //numero di colonne ridotto -> font più grande
                if (_sFileToPrintParam.Contains(NOME_FILE_STAMPA_LOC_RID_TMP))
                    _fReceiptVsCopyZoom = sWinPrinterParams.bChars33 ? (30.0f / 33.0f) : 1.06f;
                else
                    _fReceiptVsCopyZoom = sWinPrinterParams.bChars33 ? (26.0f / 33.0f) : 0.86f;
            }
#else
            _bStampaBarcode = (ReadRegistry(STAMPA_BARCODE_KEY, 0) == 1);
#endif

            // String sTmpFormat = String.Format("{0,2:D2}", _iGruppoStampa);
            _cGruppoStampa[0] = String.Format("{0,2:D2}", _iGruppoStampa)[0];
            _cGruppoStampa[1] = String.Format("{0,2:D2}", _iGruppoStampa)[1];

            /*************************************
                if(bCopiaCucina) stampa BARCODE
             *************************************/
#if STANDFACILE
            if (sTmp.Contains("_CT"))
                _bCopiaCucina = true;
            else
                _bCopiaCucina = false;
#endif

#if STAND_CUCINA
            if (sTmp.Contains("CT_") || (_sFileToPrintParam == NOME_FILE_STAMPA_RECEIPT))
                _bCopiaCucina = true; //StandCucina stampa solo copie
            else
                _bCopiaCucina = false;
#endif

            try
            {
                // corretto aprire qui il file in caso di stampa su più pagine
                if (File.Exists(_sFileToPrintParam))
                    _fileToPrint = File.OpenText(_sFileToPrintParam);
                else
                    return;

                LogToFile("Printer_Windows : inizio stampa di " + _sFileToPrintParam);

                // ciclo per consentire i tagli intermedi
                while (!_fileToPrint.EndOfStream)
                {
                    PrintDocument pd = new PrintDocument();

                    // controlli sul Logo
                    if (String.IsNullOrEmpty(_sWinPrinterParams.sLogoName))
                        _bLogo = false;
                    else
                    {
                        // verifica che esista anche il nome del file Logo
                        if ((_sWinPrinterParams.iLogoWidth < 50) || (_sWinPrinterParams.iLogoWidth > (LOGO_WIDTH + 100)) ||
                           (_sWinPrinterParams.iLogoHeight < 50) || (_sWinPrinterParams.iLogoHeight > (LOGO_HEIGHT + 100)))
                            _bLogo = false;
                    }

                    pd.DocumentName = Path.GetFileName(_sFileToPrintParam);
                    pd.DefaultPageSettings.Margins = margins;

                    if (String.IsNullOrEmpty(sPrinterNameParam))
                    {
                        if (iPrinterIndex == NUM_EDIT_GROUPS + 1)  // stampa Messaggi
                            pd.PrinterSettings.PrinterName = _sWinPrinterParams.sMsgPrinterModel;
                        else if (iPrinterIndex == (NUM_EDIT_GROUPS)) // stampa Tickets
                            pd.PrinterSettings.PrinterName = _sWinPrinterParams.sTckPrinterModel;
                        else
                            pd.PrinterSettings.PrinterName = _sWinPrinterParams.sPrinterModel[iPrinterIndex];
                    }
                    else
                        pd.PrinterSettings.PrinterName = sPrinterNameParam; // serve alle prove di stampa

                    /******************************************************** 
                     *   evita errore con alcune stampanti come doPDF 10
                     ********************************************************/
                    if ((pd.PrinterSettings.DefaultPageSettings.PrinterResolution.Y == 0) && (pd.PrinterSettings.DefaultPageSettings.PrinterResolution.X > 0))
                    {
                        _fH_px_to_gu = 100.0f / pd.PrinterSettings.DefaultPageSettings.PrinterResolution.X;
                        _fV_px_to_gu = _fH_px_to_gu;

                        _fHZoom = (pd.PrinterSettings.DefaultPageSettings.PrinterResolution.X / 180.0f) * (_sWinPrinterParams.iLogoZoomValue / 100.0f);
                        _fVZoom = _fHZoom;

                        if (sPrevPrinter != pd.PrinterSettings.PrinterName)
                        {
                            sTmp = String.Format("Printer_Windows Y0: {0}", _fH_px_to_gu);
                            LogToFile(sTmp);
                        }
                    }
                    else if ((pd.PrinterSettings.DefaultPageSettings.PrinterResolution.X == 0) && (pd.PrinterSettings.DefaultPageSettings.PrinterResolution.Y > 0))
                    {
                        _fV_px_to_gu = 100.0f / pd.PrinterSettings.DefaultPageSettings.PrinterResolution.Y;
                        _fH_px_to_gu = _fV_px_to_gu;

                        _fVZoom = (pd.PrinterSettings.DefaultPageSettings.PrinterResolution.Y / 180.0f) * (_sWinPrinterParams.iLogoZoomValue / 100.0f);
                        _fHZoom = _fVZoom;

                        if (sPrevPrinter != pd.PrinterSettings.PrinterName)
                        {
                            sTmp = String.Format("Printer_Windows X0: {0}", _fV_px_to_gu);
                            LogToFile(sTmp);
                        }
                    }
                    else
                    {
                        // vanno dopo l'attribuzione del nome della stampante
                        // conversioni da pixel a graphic unit = pixel/pollice * 100 cioè centesimi di pollice
                        if (pd.PrinterSettings.DefaultPageSettings.PrinterResolution.Kind == PrinterResolutionKind.Custom)
                        {
                            _fH_px_to_gu = 100.0f / pd.PrinterSettings.DefaultPageSettings.PrinterResolution.X;
                            _fV_px_to_gu = 100.0f / pd.PrinterSettings.DefaultPageSettings.PrinterResolution.Y;

                            // Zoom da applicare ai pixels, 1 = stessa risoluzione della TM-T88IV
                            _fHZoom = (pd.PrinterSettings.DefaultPageSettings.PrinterResolution.X / 180.0f) * (_sWinPrinterParams.iLogoZoomValue / 100.0f);
                            _fVZoom = (pd.PrinterSettings.DefaultPageSettings.PrinterResolution.Y / 180.0f) * (_sWinPrinterParams.iLogoZoomValue / 100.0f);
                        }
                        else if (pd.PrinterSettings.DefaultPageSettings.PrinterResolution.Kind == PrinterResolutionKind.High)
                        {
                            _fH_px_to_gu = 100.0f / 203.0f;
                            _fV_px_to_gu = 100.0f / 203.0f;

                            _fHZoom = (203 / 180.0f) * (_sWinPrinterParams.iLogoZoomValue / 100.0f);
                            _fVZoom = (203 / 180.0f) * (_sWinPrinterParams.iLogoZoomValue / 100.0f);
                        }
                        else
                        {
                            _fH_px_to_gu = 100.0f / 180.0f;
                            _fV_px_to_gu = 100.0f / 180.0f;

                            _fHZoom = _sWinPrinterParams.iLogoZoomValue / 100.0f;
                            _fVZoom = _sWinPrinterParams.iLogoZoomValue / 100.0f;
                        }

                    }

                    // controllo sullo zoom
                    if (_fHZoom < 0.5f) _fHZoom = 0.5f;
                    if (_fVZoom < 0.5f) _fVZoom = 0.5f;

                    if (_bIsDati)
                    {
                        _printFont = new Font(_sWinPrinterParams.sRepFontType, _sWinPrinterParams.fRepFontSize * (_sWinPrinterParams.iRepZoomValue / 100.0f) * _fReceiptVsCopyZoom, _sWinPrinterParams.sRepFontStyle);
                        _fLeftMargin = _sWinPrinterParams.iRepLeftMargin * _fH_px_to_gu;

                        _LogoFont = _printFont;
                    }
                    else
                    {
                        _printFont = new Font(_sWinPrinterParams.sTckFontType, _sWinPrinterParams.fTckFontSize * (_sWinPrinterParams.iTckZoomValue / 100.0f) * _fReceiptVsCopyZoom, _sWinPrinterParams.sTckFontStyle);

                        _fLeftMargin = _sWinPrinterParams.iTckLeftMargin * _fH_px_to_gu;

#if STANDFACILE
                        // considera solo il Font per la Receipt in modo da non avere poi differenze con le copie
                        if (_bIsTicket)
                            _LogoFont = _printFont;
#else
                        _LogoFont = _printFont;
#endif
                    }

                    _fLogoCenter = _sWinPrinterParams.iLogoCenter * _fH_px_to_gu;

                    if (sPrevPrinter != pd.PrinterSettings.PrinterName)
                    {
                        sTmp = String.Format("Printer_Windows : PrinterResolution.Kind = {0}", (int)pd.PrinterSettings.DefaultPageSettings.PrinterResolution.Kind);
                        LogToFile(sTmp);

                        sTmp = String.Format("Printer_Windows : _fH_px_to_gu = {0}, _fV_px_to_gu = {1}", _fH_px_to_gu, _fV_px_to_gu);
                        LogToFile(sTmp);
                        sTmp = String.Format("Printer_Windows : iTckFontSize = {0}, _fLeftMargin = {1}, _fLogoCenter = {2}", _sWinPrinterParams.fTckFontSize, _fLeftMargin, _fLogoCenter);
                        LogToFile(sTmp);
                        sTmp = String.Format("Printer_Windows : _fHZoom = {0}, _fVZoom = {1}", _fHZoom, _fVZoom);
                        LogToFile(sTmp);

                        sTmp = String.Format("Printer_Windows nome stampante: {0}", pd.PrinterSettings.PrinterName);
                        LogToFile(sTmp);

                        sPrevPrinter = pd.PrinterSettings.PrinterName;
                    }

                    // consente impostazioni: es. _LogoFont ma non stampa
                    if (_bIsTicket && _bSkipTicketPrint)
                    {
                        break;
                    }
                    else
                    {

                        pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);

                        LogToFile("Printer_Windows : PrintPage");

                        // stampa
                        pd.Print();
                    }
                }

                _fileToPrint.Close();

#if STANDFACILE
                if (!CheckService(Define._AUTO_SEQ_TEST))
                    Thread.Sleep(iPrint_WaitInterval);
#endif

                LogToFile("Printer_Windows : file Close");
                //throw new System.InvalidOperationException("Prova null");
            }

            catch (Exception ex)
            {
                _fileToPrint.Close();

                sTmp = String.Format("Printer_Windows : {0}", ex.ToString());
                LogToFile(sTmp);

                _WrnMsg.sMsg = sPrevPrinter;
                _WrnMsg.iErrID = WRN_STF;
                WarningManager(_WrnMsg);
            }
        }

        // The PrintPage event is raised for each page to be printed.
        private static void pd_PrintPage(object sender, PrintPageEventArgs ev)
        {
            bool bLogoRequested;
            int i, iPos;

            Graphics pg = ev.Graphics;

            float linesPerPage;
            float fLogo_LeftMargin;
            float fBC_LeftMargin, fBC_Height, fBC_Zoom;
            float topMargin = ev.MarginBounds.Top;
            float fFont_HSize, fFont_VSize, fLogoFont_HSize;

            string sTmp, sInStr;
            String sBarcode;

            // Create a new pen.
            Pen blackPen = new Pen(Brushes.Black, 2.0f);
            fBC_Zoom = 1.6f; // per rendere uguale strip nere e bianche

            _iPageRows = 0; // reset all'inizio della pagina

            // Calculate the number of lines per page.
            linesPerPage = ev.MarginBounds.Height / _printFont.GetHeight(ev.Graphics);

            // fFont_VSize = pg.MeasureString(WIDE_CONST_STRING, _printFont).Width;
            // char width in pixels
            fFont_HSize = pg.MeasureString("W", _printFont).Width * 1.22f;
            fFont_VSize = pg.MeasureString("W", _printFont).Height * 1.22f;

            fLogoFont_HSize = pg.MeasureString("W", _LogoFont).Width * 1.22f;

            i = (int)pg.PageUnit;

            _fCanvasVertPos = topMargin;

            PrintCanvas(pg, "");

            while ((_iPageRows < linesPerPage) && ((sInStr = _fileToPrint.ReadLine()) != null))
            {
                if (sInStr.IndexOf(_LOGO) != -1)
                {
                    bLogoRequested = true;

                    sInStr = _fileToPrint.ReadLine();
                    sInStr = _fileToPrint.ReadLine();
                }
                else
                    bLogoRequested = false;

                /*************************************
                 * 		   Stampa del Logo
                 *************************************/
                if (_bLogo && bLogoRequested)
                {
                    Image img = WinPrinterDlg._rWinPrinterDlg.GetWinPrinterLogo();

                    if (img != null)
                    {
                        fLogo_LeftMargin = _fLogoCenter + (iMAX_RECEIPT_CHARS * fLogoFont_HSize - img.Size.Width * _fHZoom) * _fH_px_to_gu / 2.0f;

                        if (fLogo_LeftMargin < 0)
                            fLogo_LeftMargin = _fLogoCenter;

                        RectangleF imageRect = new RectangleF(fLogo_LeftMargin, _fCanvasVertPos, img.Size.Width * _fHZoom * _fH_px_to_gu,
                                                                img.Size.Height * _fVZoom * _fV_px_to_gu);

                        pg.DrawImage(img, imageRect);

                        _fCanvasVertPos += img.Size.Height * _fVZoom * _fV_px_to_gu;
                        _iPageRows = Convert.ToInt32(img.Size.Height * _fVZoom * _fV_px_to_gu / _printFont.GetHeight(ev.Graphics));

                        PrintCanvas(pg, "");

                        if (!_bLogoPrinted)
                        {
                            _bLogoPrinted = true;

                            sTmp = String.Format("Printer_Windows Logo: {0:0.00}, {1:0.00}, {2:0.00}, {3:0.00}", fLogo_LeftMargin, _fCanvasVertPos,
                                                    img.Size.Width * _fHZoom * _fH_px_to_gu, img.Size.Height * _fVZoom * _fV_px_to_gu);
                            LogToFile(sTmp);
                        }
                    }
                }

                /*************************************
                 * 		   Stampa del testo
                 *************************************/
                //PrintCanvas(ev, "#########_#########_########");

                //Console.WriteLine(String.Format("Printer Win: current position = {0}", _lFileCursor));

                iPos = sInStr.IndexOf(_TICK_NUM); // strLenght = 5 attenzione alla corrispondenza stringa !

                // costruzione Num. scontrino per Barcode
                if (iPos != -1)
                {
                    sTmp = sInStr.Substring(iPos + _TICK_NUM.Length);
                    sTmp = sTmp.Trim();

                    _sOrdineNum = String.Format("{0:d4}", Convert.ToInt32(sTmp));
                    _bTicketNumFound = true;

                    // accorcia stringa
                    if (sInStr.StartsWith("   "))
                        sInStr = sInStr.Substring(3);

                    if (!_bSkipNumeroScontrino)
                        PrintCanvas(pg, 1.32f, 1.32f, sInStr); // era 1.24f
                }
                else
                {
                    // elimina Keywords Prezzi
                    if (_bIsDati && (sInStr.Length > 2))
                    {
                        if ((sInStr[0] == '#') && (sInStr[1] == 'L') && (sInStr[2] == 'F'))// Line Feed multipli
                        {
                            _fileToPrint.ReadLine();

                            sInStr = _fileToPrint.ReadLine();
                        }
                        else if ((sInStr[0] == '#') && (sInStr[1] == 'C') && (sInStr[2] == 'K'))// Checksum
                            sInStr = _fileToPrint.ReadLine();
                    }

                    if (sInStr.Contains(_CUT))
                    {
                        sInStr = _fileToPrint.ReadLine();
                        sInStr = _fileToPrint.ReadLine();

                        break;
                    }

                    PrintCanvas(pg, sInStr);
                }
            }

            if (!_bTicketNumFound)
            {
                _WrnMsg.iErrID = WRN_TNFF;
                _WrnMsg.sNomeFile = _sFileToPrintParam;
                WarningManager(_WrnMsg);
            }

            /******************************************************
                               BAR CODE EAN13
              effettua la stampa solo se il flag bStampaBarcode
              è abilitato dal dialogo FrmImpostaTipoCassa
             ******************************************************/
            if ((_iPageRows < linesPerPage) && (_bCopiaCucina && _bStampaBarcode) || (_bIsTicket && _bStampaBarcodePrev) ||
                _sFileToPrintParam.Contains(NOME_FILE_SAMPLE_TEXT))
            {
                fBC_LeftMargin = _fLogoCenter + ((_sWinPrinterParams.bChars33 ? 33 : 28) * fFont_HSize * _fHZoom * _fH_px_to_gu - 95 * blackPen.Width) / 2;

                if (fBC_LeftMargin < 0)
                    fBC_LeftMargin = 0;

                fBC_Height = BARCODE_HEIGHT * _fVZoom * _fV_px_to_gu;

                sBarcode = String.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}",
                    _cGruppoStampa[0], _cGruppoStampa[1],                                                   // 2 gruppo di stampa
                    _sDataStr[0], _sDataStr[1], _sDataStr[2], _sDataStr[3], _sDataStr[4], _sDataStr[5],     // date
                    _sOrdineNum[0], _sOrdineNum[1], _sOrdineNum[2], _sOrdineNum[3]);                        // Scontrino num

                if (_sFileToPrintParam.Contains(NOME_FILE_SAMPLE_TEXT))
                {
                    Barcode_EAN13.BuildBarcodeID("010406220123");
                }
                else
                    Barcode_EAN13.BuildBarcodeID(sBarcode);

                for (i = 0; i <= 94; i++)
                {
                    if (Barcode_EAN13._sBuilString[i] == '1')
                        pg.DrawLine(blackPen, fBC_LeftMargin + i * blackPen.Width * _fHZoom * _fH_px_to_gu * fBC_Zoom, _fCanvasVertPos,
                            fBC_LeftMargin + i * blackPen.Width * _fHZoom * _fH_px_to_gu * fBC_Zoom, _fCanvasVertPos + fBC_Height);
                }

                _fCanvasVertPos += fBC_Height;
                _iPageRows += Convert.ToInt32(fBC_Height / fFont_VSize);

                // _sBarcodeID comprende il checksum
                sTmp = String.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10} {11} {12}",
                        Barcode_EAN13._sBarcodeID[0], Barcode_EAN13._sBarcodeID[1], Barcode_EAN13._sBarcodeID[2], Barcode_EAN13._sBarcodeID[3],
                        Barcode_EAN13._sBarcodeID[4], Barcode_EAN13._sBarcodeID[5], Barcode_EAN13._sBarcodeID[6], Barcode_EAN13._sBarcodeID[7],
                        Barcode_EAN13._sBarcodeID[8], Barcode_EAN13._sBarcodeID[9], Barcode_EAN13._sBarcodeID[10], Barcode_EAN13._sBarcodeID[11],
                        Barcode_EAN13._sBarcodeID[12]);

                Font BC_Font = new Font("Arial", 11.0f);

                pg.DrawString(sTmp, BC_Font, Brushes.Black, fBC_LeftMargin, _fCanvasVertPos, new StringFormat());

                LogToFile(String.Format("Printer Win: _sBarcodeID = {0}", sTmp));

                _iPageRows++;

                _bStampaBarcode = false;
            }

            // nel caso di carta A5 meglio evitare righe aggiuntive pre-taglio
            if (!sGlbWinPrinterParams.bA5Paper)
            {
                PrintCanvas(pg, " "); // 4 righe di fine stampa
                PrintCanvas(pg, " ");
                PrintCanvas(pg, " ");
                PrintCanvas(pg, "_");
            }

            // valuta se servono altre pagine
            ev.HasMorePages = (_iPageRows > linesPerPage);
        }

        static void PrintCanvas(Graphics pgParam, float fSize, float fVertSep, String sStrParam)
        {
            Font printFont = new Font(_sWinPrinterParams.sTckFontType, _sWinPrinterParams.fTckFontSize * fSize * (_sWinPrinterParams.iTckZoomValue / 100.0f));

            pgParam.DrawString(sStrParam, printFont, Brushes.Black, _fLeftMargin, _fCanvasVertPos);

            _fCanvasVertPos += printFont.GetHeight(pgParam) * fVertSep;
            _iPageRows++;
        }

        static void PrintCanvas(Graphics pgParam, String sStrParam)
        {
            pgParam.DrawString(sStrParam, _printFont, Brushes.Black, _fLeftMargin, _fCanvasVertPos);

            _fCanvasVertPos += _printFont.GetHeight(pgParam);
            _iPageRows++;
        }

        /***************************************
                 test di esempio di stampa
         ***************************************/
        public static void PrintSampleText(TWinPrinterParams sWinPrinterParams)
        {
            String sTmp, sFileToPrint;

            sFileToPrint = BuildSampleText();

            _sOrdineNum = "0123"; // per sampleText

            sTmp = String.Format("Printer_Windows : printWinSampleText() {0}", sFileToPrint);
            LogToFile(sTmp);

            PrintFile(sFileToPrint, sWinPrinterParams, NUM_EDIT_GROUPS); // OK
        }

    }
}

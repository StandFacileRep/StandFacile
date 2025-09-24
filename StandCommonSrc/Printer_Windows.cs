/*****************************************************************************
	NomeFile : StandCommonSrc/Printer_Windows.cs
    Data	 : 13.09.2025
	Autore   : Mauro Artuso

	Descrizione :
	Questo file contiene la classe per la gestione della stampante
	Windows generica mediante Canvas ed installata con il suo driver

    con carta A4 tutte le stampe sono incluse nella Receipt

  	consigliato Soft Font Lucida Console, size 12

    stampante pdf 210mm = 8,27"  * 600 dpi = 4958px di larghezza
                  297mm = 11,69" * 600 dpi = 7016px di lunghezza

    STAMPANTE_TM_T88            : 180dpi, carta 80mm 521 dot
    STAMPANTE_TM_L90            : 203dpi, carta 80mm 640 dot
    STAMPANTE_VRETTI_POS-80C    : 203dpi, carta 72mm 576 dot
    
    dpi = dotNum*25.4/paperWidth

    in*dpi = px;
    mm*dpi = px*25.4

    25.4mm = 100gu  1mm = 100/25.4gu = 3.937gu
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

        const float PAGE_VERT_SIZE_PERC1 = 0.55f;
        const float PAGE_VERT_SIZE_PERC2 = 0.65f;

        static bool _bIsDati, _bIsTicket;
        static bool _bLogoCheck_T, _bLogoCheck_B;
        static bool _bCopiaCucina;
        static bool _bTicketNumFound;
        static bool _bSkipNumeroScontrino, _bLogoPrinted_T, _bLogoPrinted_B;

        /// <summary>se true evita la stampa dello scontrino</summary>
        static bool _bSkipTicketPrint = false;
        static bool _bPaperIsA4 = false;

        /// <summary>imposta l'intervallo tra le stampe</summary>
        public static int iPrint_WaitInterval = 200;

        static float _fLeftMargin, _fLogoCenter, _fLeftMarginBk;
        static float _fCanvasVertPos, _fCanvasVertPosBk;

        static int _iGruppoStampa;

        static Char[] _cGruppoStampa = { '9', '9' };
        static String _sDataStr = "";
        static String _sOrdineNum = "";

        static TErrMsg _WrnMsg;

        static float _fLogo_T_LeftMargin, _fLogo_T_LeftMarginBk;
        static float _fLogo_B_LeftMargin, _fLogo_B_LeftMarginBk;
        static float _fFont_HSize, _fFont_VSize, _fLogoFont_HSize;

        static Image _img_T, _img_B;

        static Font _printFont = new Font("Lucida Console", 10.25f, FontStyle.Regular);
        static Font _LogoFont = new Font("Lucida Console", 10.25f, FontStyle.Regular);

        static StreamReader _fileToPrint;

        static TGenericPrinterParams _sGenericPrinterParams;
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
        
        const float _mm_to_gu = 0.3937f; // corrisponde a 0.1mm

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
            PrintFile(sFileToPrintParam, sGlbWinPrinterParams, NUM_SEP_PRINT_GROUPS); // OK
        }

        /// <summary>
        ///  STAMPA su stampante gestita da driver windows,<br/>
        ///  diversa per le 10 copie + scontrino principale + messaggi<br/>
        ///  iPrinterIndex da 0 a NUM_EDIT_GROUPS - 1   identifica stampante copie,<br/>
        ///  iPrinterIndex == NUM_SEP_PRINT_GROUPS      identifica stampante Receipt,<br/>
        ///  iPrinterIndex == NUM_SEP_PRINT_GROUPS + 1  identifica stampante Messaggi
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
            _bLogoCheck_T = true;
            _bLogoCheck_B = true;

            _fReceiptVsCopyZoom = 1.0f;

            _fLeftMarginBk = 0;
            _fCanvasVertPosBk = 0;

            _fLogo_T_LeftMargin = 0;
            _fLogo_B_LeftMargin = 0;

            // init
            _sOrdineNum = "0123"; // per sampleText
            _sDataStr = GetActualDate().ToString("ddMMyy");

            _sGenericPrinterParams = sGlbGenericPrinterParams;
            _sWinPrinterParams = sWinPrinterParams;
            _sFileToPrintParam = sFileToPrintParam;

            sTmp = Path.GetFileName(_sFileToPrintParam);

            _img_T = WinPrinterDlg._rWinPrinterDlg.GetWinPrinterLogo(true);
            _img_B = WinPrinterDlg._rWinPrinterDlg.GetWinPrinterLogo(false);

            _iGruppoStampa = 0;

            if (sTmp.Contains("_TT") || sTmp.Contains("_TN"))
            {
#if STANDFACILE || STAND_MONITOR
                // gruppi in fase di emissione ordine
                if (SF_Data.bPrevendita)
                {
                    _iGruppoStampa = NUM_PRE_SALE_GRP;
                }
                else if (IsBitSet(SF_Data.iStatusReceipt, (int)STATUS_FLAGS.BIT_CARICATO_DA_WEB))
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
                else // copia locale
                {
                    if (IsBitSet(SF_Data.iReceiptCopyOptions, (int)LOCAL_COPIES_OPTS.BIT_PRICE_PRINT_ON_COPIES_REQUIRED))
                        _fReceiptVsCopyZoom = sWinPrinterParams.bChars33 ? (28.0f / 33.0f) : 1.0f;

                    _bTicketNumFound = true; // necessario nella copia Receipt NoPrices
                }
            }
            else if (sTmp.Contains("_CT") || sTmp.Contains("CT_")) // copia in rete
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
                _bLogoCheck_T = false;
                _bLogoCheck_B = false;
            }

            _bTicketNumFound = true;

#if STANDFACILE || STAND_MONITOR
            int iDebug1, iDebug2;

            iDebug1 = DB_Data.iStatusReceipt; // 4 BIT_CARICATO_DA_PREVENDITA
            iDebug2 = SF_Data.iStatusReceipt; // 2 BIT_EMESSO_IN_PREVENDITA

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

                // ciclo esterno per consentire i tagli intermedi
                while (!_fileToPrint.EndOfStream)
                {
                    PrintDocument pd = new PrintDocument();

                    pd.DocumentName = Path.GetFileName(_sFileToPrintParam);
                    pd.DefaultPageSettings.Margins = margins;

                    if (String.IsNullOrEmpty(sPrinterNameParam))
                    {
                        if (iPrinterIndex == NUM_SEP_PRINT_GROUPS + 1)  // stampa Messaggi
                            pd.PrinterSettings.PrinterName = _sWinPrinterParams.sMsgPrinterModel;
                        else if (iPrinterIndex == (NUM_SEP_PRINT_GROUPS)) // stampa Tickets
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

                    // controlla se la carta è di tipo A4
                    _bPaperIsA4 = (pd.PrinterSettings.DefaultPageSettings.PaperSize.Kind == PaperKind.A4);

                    // controllo sullo zoom
                    if (_fHZoom < 0.5f) _fHZoom = 0.5f;
                    if (_fVZoom < 0.5f) _fVZoom = 0.5f;

                    if (_bIsDati)
                    {
                        _printFont = new Font(_sWinPrinterParams.sRepFontType, _sWinPrinterParams.fRepFontSize * (_sWinPrinterParams.iRepZoomValue / 100.0f) * _fReceiptVsCopyZoom, _sWinPrinterParams.sRepFontStyle);
                        _fLeftMargin = _sWinPrinterParams.iRepLeftMargin * _mm_to_gu;

                        _LogoFont = _printFont;
                    }
                    else
                    {
                        _printFont = new Font(_sWinPrinterParams.sTckFontType, _sWinPrinterParams.fTckFontSize * (_sWinPrinterParams.iTckZoomValue / 100.0f) * _fReceiptVsCopyZoom, _sWinPrinterParams.sTckFontStyle);

                        _fLeftMargin = _sWinPrinterParams.iTckLeftMargin * _mm_to_gu;

#if STANDFACILE
                        // considera solo il Font per la Receipt in modo da non avere poi differenze con le copie
                        if (_bIsTicket)
                            _LogoFont = _printFont;
#else
                        _LogoFont = _printFont;
#endif
                    }

                    _fLogoCenter = _sWinPrinterParams.iLogoCenter * _mm_to_gu;

                    // controlli sul Logo Top
                    if (String.IsNullOrEmpty(_sWinPrinterParams.sLogoName_T))
                        _bLogoCheck_T = false;
                    else
                    {
                        // verifica che esista anche il nome del file Logo
                        if ((_sWinPrinterParams.iLogoWidth_T < 50) || (_sWinPrinterParams.iLogoWidth_T > (LOGO_WIDTH + 100)) ||
                           (_sWinPrinterParams.iLogoHeight_T < 50) || (_sWinPrinterParams.iLogoHeight_T > (LOGO_HEIGHT + 100)))
                            _bLogoCheck_T = false;
                        else
                        {
                            _fLogo_T_LeftMargin = _fLogoCenter + (iMAX_RECEIPT_CHARS * _fLogoFont_HSize - _img_T.Size.Width * _fHZoom) * _fH_px_to_gu / 2.0f;

                            if (_fLogo_T_LeftMargin < 0)
                                _fLogo_T_LeftMargin = _fLogoCenter;
                        }
                    }

                    // controlli sul Logo Bottom
                    if (String.IsNullOrEmpty(_sWinPrinterParams.sLogoName_B))
                        _bLogoCheck_B = false;
                    else
                    {
                        // verifica che esista anche il nome del file Logo
                        if ((_sWinPrinterParams.iLogoWidth_B < 50) || (_sWinPrinterParams.iLogoWidth_B > (LOGO_WIDTH + 100)) ||
                           (_sWinPrinterParams.iLogoHeight_B < 50) || (_sWinPrinterParams.iLogoHeight_B > (LOGO_HEIGHT + 100)))
                            _bLogoCheck_B = false;
                        else
                        {
                            _fLogo_B_LeftMargin = _fLogoCenter + (iMAX_RECEIPT_CHARS * _fLogoFont_HSize - _img_B.Size.Width * _fHZoom) * _fH_px_to_gu / 2.0f;

                            if (_fLogo_B_LeftMargin < 0)
                                _fLogo_B_LeftMargin = _fLogoCenter;
                        }
                    }

                    if (sPrevPrinter != pd.PrinterSettings.PrinterName)
                    {
                        sTmp = String.Format("Printer_Windows : PrinterResolution.Kind = {0}", (int)pd.PrinterSettings.DefaultPageSettings.PrinterResolution.Kind);
                        LogToFile(sTmp, true);

                        sTmp = String.Format("Printer_Windows : _fH_px_to_gu = {0}, _fV_px_to_gu = {1}", _fH_px_to_gu, _fV_px_to_gu);
                        LogToFile(sTmp, true);
                        sTmp = String.Format("Printer_Windows : iTckFontSize = {0}, _fLeftMargin = {1}, _fLogoCenter = {2}", _sWinPrinterParams.fTckFontSize, _fLeftMargin, _fLogoCenter);
                        LogToFile(sTmp, true);
                        sTmp = String.Format("Printer_Windows : _fHZoom = {0}, _fVZoom = {1}", _fHZoom, _fVZoom);
                        LogToFile(sTmp, true);

                        sTmp = String.Format("Printer_Windows nome stampante: {0}", pd.PrinterSettings.PrinterName);
                        LogToFile(sTmp, true);

                        sPrevPrinter = pd.PrinterSettings.PrinterName;
                    }

                    // non stampa la Receipt
                    if (_bIsTicket && (_bSkipTicketPrint || CheckService(CFG_COMMON_STRINGS._SKIP_STAMPA_RCP)))
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
                } // end while()

                _fileToPrint.Close();

#if STANDFACILE
                if (!CheckService(Define.CFG_SERVICE_STRINGS._AUTO_SEQ_TEST))
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
            bool bLogoRequested_T, bLogoRequested_B, bBarcodeRequested;
            int i, iPos, iA4_PrintStatus;

            Graphics pg = ev.Graphics;

            float fBC_LeftMargin, fBC_Height, fBC_Zoom;
            float topMargin = ev.MarginBounds.Top;

            string sTmp, sInStr;
            String sBarcode;

            // Create a new pen.
            Pen blackPen = new Pen(Brushes.Black, 2.0f);
            fBC_Zoom = 1.6f; // per rendere uguale strip nere e bianche

            // _fFont_VSize = pg.MeasureString(WIDE_CONST_STRING, _printFont).Width;
            // char width in pixels
            _fFont_HSize = pg.MeasureString("W", _printFont).Width * 1.22f;
            _fFont_VSize = pg.MeasureString("W", _printFont).Height * 1.22f;

            _fLogoFont_HSize = pg.MeasureString("W", _LogoFont).Width * 1.22f;

            i = (int)pg.PageUnit;

            // inizializzazione posizionamento e init Bk
            _fCanvasVertPos = topMargin;

            _fCanvasVertPosBk = _fCanvasVertPos;
            _fLeftMarginBk = _fLeftMargin;

            _fLogo_T_LeftMarginBk = _fLogo_T_LeftMargin;
            _fLogo_B_LeftMarginBk = _fLogo_B_LeftMargin;

            iA4_PrintStatus = 0;
            for (i = 0; i < _sGenericPrinterParams.iRowsInitial; i++) // N righe di inizio stampa
                PrintCanvas(pg, "");

            while ((_fCanvasVertPos < ev.PageSettings.PaperSize.Height) && ((sInStr = _fileToPrint.ReadLine()) != null))
            {
                if (!String.IsNullOrEmpty(sInStr) && (sInStr.IndexOf(_LOGO_T) != -1) || (sInStr.IndexOf(_LOGO) != -1))
                {
                    bLogoRequested_T = true;

                    sInStr = _fileToPrint.ReadLine();
                }
                else
                    bLogoRequested_T = false;

                if (!String.IsNullOrEmpty(sInStr) && (sInStr.IndexOf(_LOGO_B) != -1))
                {
                    bLogoRequested_B = true;

                    sInStr = _fileToPrint.ReadLine();
                }
                else
                    bLogoRequested_B = false;

                if (!String.IsNullOrEmpty(sInStr) && (sInStr.IndexOf(_BARCODE) != -1))
                {
                    bBarcodeRequested = true;

                    sInStr = _fileToPrint.ReadLine();
                }
                else
                    bBarcodeRequested = false;

                /*************************************
                 *        Stampa del Logo Top
                 *************************************/
                if (_bLogoCheck_T && bLogoRequested_T)
                {

                    RectangleF imageRect = new RectangleF(_fLogo_T_LeftMargin, _fCanvasVertPos, _img_T.Size.Width * _fHZoom * _fH_px_to_gu,
                                                            _img_T.Size.Height * _fVZoom * _fV_px_to_gu);

                    pg.DrawImage(_img_T, imageRect);

                    _fCanvasVertPos += _img_T.Size.Height * _fVZoom * _fV_px_to_gu;

                    PrintCanvas(pg, "");

                    if (!_bLogoPrinted_T)
                    {
                        _bLogoPrinted_T = true;

                        sTmp = String.Format("Printer_Windows Logo: {0:0.00}, {1:0.00}, {2:0.00}, {3:0.00}", _fLogo_T_LeftMargin, _fCanvasVertPos,
                                                _img_T.Size.Width * _fHZoom * _fH_px_to_gu, _img_T.Size.Height * _fVZoom * _fV_px_to_gu);
                        LogToFile(sTmp);
                    }
                }


                /*************************************
                 * 		   Stampa del testo
                 *************************************/
                //PrintCanvas(ev, "#########_#########_########");

                //Console.WriteLine(String.Format("Printer Win: current position = {0}", _lFileCursor));
                if (!String.IsNullOrEmpty(sInStr))
                    iPos = sInStr.IndexOf(_TICK_NUM); // strLenght = 5 attenzione alla corrispondenza stringa !
                else
                    iPos = -1;

                // costruzione Num. scontrino per Barcode
                if (iPos != -1)
                {
                    sTmp = sInStr.Substring(iPos + _TICK_NUM.Length);
                    sTmp = sTmp.Trim();

                    _sOrdineNum = String.Format("{0:d4}", Convert.ToInt32(sTmp));
                    _bTicketNumFound = true;

                    // accorcia stringa
                    if (sInStr.StartsWith("  ") && _sGenericPrinterParams.iCassaInline)
                        sInStr = sInStr.Substring(2);
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

                    if (!String.IsNullOrEmpty(sInStr) && sInStr.Contains(_CUT) && sGlbWinPrinterParams.bA4Paper && _bPaperIsA4)
                    {
                        switch (iA4_PrintStatus)
                        {
                            // prepara 2Q Top
                            case 0:
                                _fLeftMargin = _fLeftMarginBk + ev.PageSettings.PaperSize.Width / 2;
                                _fCanvasVertPos = _fCanvasVertPosBk;

                                _fLogo_T_LeftMargin = _fLogo_T_LeftMarginBk + ev.PageSettings.PaperSize.Width / 2;
                                _fLogo_B_LeftMargin = _fLogo_B_LeftMarginBk + ev.PageSettings.PaperSize.Width / 2;

                                PrintCanvas(pg, "");

                                iA4_PrintStatus = (iA4_PrintStatus + 1) % 4;
                                break;

                            // prepara 2Q Bottom
                            case 1:
                                if (_fCanvasVertPos < ev.PageSettings.PaperSize.Height * PAGE_VERT_SIZE_PERC1)
                                    _fCanvasVertPos = ev.PageSettings.PaperSize.Height * PAGE_VERT_SIZE_PERC1;
                                else
                                    _fCanvasVertPos += _printFont.GetHeight(pg);

                                iA4_PrintStatus = (iA4_PrintStatus + 1) % 4;
                                PrintCanvas(pg, "");
                                break;

                            // prepara 2Q Bottom / 4Q
                            case 2:
                                if (_fCanvasVertPos < ev.PageSettings.PaperSize.Height * PAGE_VERT_SIZE_PERC2)
                                {
                                    _fCanvasVertPos += _printFont.GetHeight(pg);

                                    iA4_PrintStatus = (iA4_PrintStatus + 1) % 4;
                                    PrintCanvas(pg, "");
                                }
                                else
                                {
                                    // cambio pagina ed a capo
                                    _fLeftMargin = _fLeftMarginBk;
                                    _fCanvasVertPos = ev.PageSettings.PaperSize.Height;

                                    _fLogo_T_LeftMargin = _fLogo_T_LeftMarginBk;
                                    _fLogo_B_LeftMargin = _fLogo_B_LeftMarginBk;

                                    iA4_PrintStatus = 0;
                                }

                                break;

                            // prepara 1Q e cambio pagina
                            default:
                                PrintCanvas(pg, "");
                                break;
                        }

                        sInStr = _fileToPrint.ReadLine();
                        sInStr = _fileToPrint.ReadLine();

                        Console.WriteLine("Prt_Win: iA4_PrintStatus = {0}", iA4_PrintStatus);
                    }
                    else if (!String.IsNullOrEmpty(sInStr) && sInStr.Contains(_CUT))
                    {
                        sInStr = _fileToPrint.ReadLine();
                        sInStr = _fileToPrint.ReadLine();

                        break;
                    }
                    // testo normale
                    else
                    {
                        PrintCanvas(pg, sInStr);
                        Console.WriteLine("Prt_Win: {0}", sInStr);
                    }
                }

                /*************************************
                 * 		Stampa del Logo Bottom
                 *************************************/
                if (_bLogoCheck_B && bLogoRequested_B)
                {
                    RectangleF imageRect = new RectangleF(_fLogo_B_LeftMargin, _fCanvasVertPos, _img_B.Size.Width * _fHZoom * _fH_px_to_gu,
                                                            _img_B.Size.Height * _fVZoom * _fV_px_to_gu);

                    pg.DrawImage(_img_B, imageRect);

                    _fCanvasVertPos += _img_B.Size.Height * _fVZoom * _fV_px_to_gu;

                    PrintCanvas(pg, "");

                    if (!_bLogoPrinted_B)
                    {
                        _bLogoPrinted_B = true;

                        sTmp = String.Format("Printer_Windows Logo: {0:0.00}, {1:0.00}, {2:0.00}, {3:0.00}", _fLogo_B_LeftMargin, _fCanvasVertPos,
                                                _img_B.Size.Width * _fHZoom * _fH_px_to_gu, _img_B.Size.Height * _fVZoom * _fV_px_to_gu);
                        LogToFile(sTmp);
                    }
                }

                /******************************************************
                                   BAR CODE EAN13
                  effettua la stampa solo se il flag bStampaBarcode
                  è abilitato dal dialogo FrmImpostaTipoCassa
                 ******************************************************/
                if (bBarcodeRequested)
                {
                    fBC_LeftMargin = _fLeftMargin + _fLogoCenter + ((_sWinPrinterParams.bChars33 ? 33 : 28) * _fFont_HSize * _fHZoom * _fH_px_to_gu - 95 * blackPen.Width) / 2;

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

                    // _sBarcodeID comprende il checksum
                    sTmp = String.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10} {11} {12}",
                            Barcode_EAN13._sBarcodeID[0], Barcode_EAN13._sBarcodeID[1], Barcode_EAN13._sBarcodeID[2], Barcode_EAN13._sBarcodeID[3],
                            Barcode_EAN13._sBarcodeID[4], Barcode_EAN13._sBarcodeID[5], Barcode_EAN13._sBarcodeID[6], Barcode_EAN13._sBarcodeID[7],
                            Barcode_EAN13._sBarcodeID[8], Barcode_EAN13._sBarcodeID[9], Barcode_EAN13._sBarcodeID[10], Barcode_EAN13._sBarcodeID[11],
                            Barcode_EAN13._sBarcodeID[12]);

                    Font BC_Font = new Font("Arial", 11.0f);

                    pg.DrawString(sTmp, BC_Font, Brushes.Black, fBC_LeftMargin, _fCanvasVertPos, new StringFormat());

                    LogToFile(String.Format("Printer Win: _sBarcodeID = {0}", sTmp));
                }
            } // end while()


            if (!_bTicketNumFound)
            {
                _WrnMsg.iErrID = WRN_TNFF;
                _WrnMsg.sNomeFile = _sFileToPrintParam;
                WarningManager(_WrnMsg);
            }

            // nel caso di carta A4, A5 meglio evitare righe aggiuntive pre-taglio
            if (!(sGlbWinPrinterParams.bA4Paper || sGlbWinPrinterParams.bA5Paper) && _sGenericPrinterParams.iRowsFinal > 0)
            {
                for(i=0; i< _sGenericPrinterParams.iRowsFinal-1; i++) // N righe di fine stampa
                    PrintCanvas(pg, " ");
                PrintCanvas(pg, "_");
            }

            // valuta se servono altre pagine, 25.4*32/100 = 8mm
            if (_fCanvasVertPos > (ev.PageSettings.PaperSize.Height - 32))
            {
                // reset _fCanvasVertPos
                _fCanvasVertPos = _printFont.GetHeight(pg);
                ev.HasMorePages = true;
            }
            else
                ev.HasMorePages = false;  // uscita loop
        }

        static void PrintCanvas(Graphics pgParam, float fSize, float fVertSep, String sStrParam)
        {
            Font printFont = new Font(_sWinPrinterParams.sTckFontType, _sWinPrinterParams.fTckFontSize * fSize * (_sWinPrinterParams.iTckZoomValue / 100.0f));

            pgParam.DrawString(sStrParam, printFont, Brushes.Black, _fLeftMargin, _fCanvasVertPos);

            _fCanvasVertPos += printFont.GetHeight(pgParam) * fVertSep;
        }

        static void PrintCanvas(Graphics pgParam, String sStrParam)
        {
            pgParam.DrawString(sStrParam, _printFont, Brushes.Black, _fLeftMargin, _fCanvasVertPos);

            _fCanvasVertPos += _printFont.GetHeight(pgParam);
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

            PrintFile(sFileToPrint, sWinPrinterParams, NUM_SEP_PRINT_GROUPS); // OK
        }

    }
}

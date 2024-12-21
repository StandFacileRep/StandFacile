/**********************************************************************************************
    NomeFile : StandCommonSrc/Printer_LP2844.cs
    Data	 : 06.12.2024
    Autore   : Mauro Artuso

    Descrizione :
        Questo file contiene la classe per la gestione della stampante seriale ZEBRA LP2844

    Caratteristiche stampante : LP2844 : 203dpi, 8dot/mm
          Font 1 : 8x12, Font 2 : 10x16, Font 3 : 12x20,
          Font 4 : 14x24, Font 5 : 32x48,

    consigliato Soft Font Lucida Console 8x16, largh. 22pt, alt. 45pt
    con carta 80mm, si hanno 8x80:22)=29 caratteri per riga

    Attenzione : la stampante LP2844  gestisce solo il controllo di flusso HW !  ???
 *********************************************************************************************/

using System;
using System.Threading;
using System.IO;
using static System.Convert;

using static StandCommonFiles.ComDef;
using static StandCommonFiles.CommonCl;
using static StandCommonFiles.LogServer;
using static StandCommonFiles.Printer_Legacy;

using StandFacile;
using static StandFacile.glb;
using static StandFacile.Define;

#if !STAND_CUCINA
using static StandFacile.dBaseIntf;
#endif

namespace StandCommonFiles
{
    /// <summary>
    ///  classe per la gestione della stampante LP2844
    /// </summary>
    public static class TPrinter_LP2844
    {
#pragma warning disable IDE0044

        const int LP2844_LMARGIN_SCNT_57MM = 2;
        const int LP2844_LMARGIN_SCNT_80MM = 2;

        const int LP2844_LMARGIN_DATA_80MM = 5;
        const int LP2844_LMARGIN_DATARID_80MM = 5;

        const int LP2844_LMARGIN_LOGO_80MM = 60;
        const int LP2844_VPOS_LOGO_57MM = 200;
        const int LP2844_VPOS_LOGO_80MM = 220;

        const int LP2844_LMARGIN_BARCODE_57MM = 70;
        const int LP2844_LMARGIN_BARCODE_80MM = 160;

        static bool _bStampaBarcode = false;

        static int[] iZebra_FontHSize = new int[128]; // solo per testo di prova
        static int[] iZebra_FontVSize = new int[128];
        static String sArray;
        static String sFNrm, sFNSc;

        static int iChFontNormal, iChFontNumSc;
        static int iChSizeNormal, iChSizeNumSc;
        static int iHTuneNumSc, _iLMargin;
        static int iHLogoPos, iVLogoPos;
        static int _iRowAdvance;
        static int iBarcodeLMargin;

        static String _sFileToPrint;
        static bool _bIsDati, _bIsDatiRid;
        static bool bOnceDone = false;

        static TErrMsg _WrnMsg = new TErrMsg();

        /// <summary>Init chiamata solo una volta</summary>
        static void Init_once()
        {
            int i;

            bOnceDone = true;

            // inizializzazioni vettori per stampanti Zebra LP2844
            for (i = 0; i < 128; i++)
            {
                iZebra_FontHSize[i] = 14;
                iZebra_FontVSize[i] = 24;
            }

            iZebra_FontHSize['1'] = 8; iZebra_FontVSize['1'] = 12 + 4;
            iZebra_FontHSize['2'] = 10; iZebra_FontVSize['2'] = 16 + 4;
            iZebra_FontHSize['3'] = 12; iZebra_FontVSize['3'] = 20 + 4;
            iZebra_FontHSize['4'] = 14; iZebra_FontVSize['4'] = 24 + 4;
            iZebra_FontHSize['5'] = 32; iZebra_FontVSize['5'] = 48 + 4;
            iZebra_FontHSize['6'] = 14; iZebra_FontVSize['6'] = 19 + 4;
            iZebra_FontHSize['7'] = 14; iZebra_FontVSize['7'] = 19 + 4;

            iZebra_FontHSize['a'] = 19; iZebra_FontVSize['a'] = 39 + 2; // Vsize reale Lucida 7x14
            iZebra_FontHSize['b'] = 19; iZebra_FontVSize['b'] = 39 + 5;
            iZebra_FontHSize['c'] = 19; iZebra_FontVSize['c'] = 45 + 2; // Vsize reale Lucida 7x16
            iZebra_FontHSize['d'] = 19; iZebra_FontVSize['d'] = 45 + 5;
        }

        /// <summary>Init</summary>
        public static void Init()
        {
            // simula il costruttore che nelle classi statiche non esiste
            if (!bOnceDone)
                Init_once();

            // Impostazioni volatili di stampa : Font,dimensione caratteri, etc..
            PrintLine("\nN");

            _bIsDati = false;
            _bIsDatiRid = false;

#if STANDFACILE || STAND_MONITOR
            if (!String.IsNullOrEmpty(_sFileToPrint))
            {
                // verifica se è il file dati di riepilogo o file dei prezzi
                if ((_sFileToPrint.Contains(NOME_FILE_STAMPA_LOC_RID_TMP)) || (_sFileToPrint.Contains("Listino")))
                    _bIsDatiRid = true;
                else if (_sFileToPrint.Contains(NOME_FILE_STAMPA_LOC_TMP) || (_sFileToPrint.Contains("_Dati")))
                    _bIsDati = true;
            }
#else
	        _bStampaBarcode = (ReadRegistry(STAMPA_BARCODE_KEY, 0) == 1);
#endif

            if (_bIsDatiRid) // Impostazioni Font per riepilogo Dati o Prezzi con riduzione colonne
            {
                _iLMargin = LP2844_LMARGIN_DATARID_80MM;

                iChFontNormal = '3';
                iChSizeNormal = 1;
            }
            if (_bIsDati) // Impostazioni Font per riepilogo Dati
            {
                _iLMargin = LP2844_LMARGIN_DATA_80MM;

                iChFontNormal = '2';
                iChSizeNormal = 1;
            }
            else // Scontrino
            {
                if (_LegacyPrinterParams.iPaperSize == (int)LegacyPrinterDlg.PAPER_SIZE.S57MM) // Form Width
                {
                    iChFontNormal = '3'; iChSizeNormal = 1;
                    iChFontNumSc = '3'; iChSizeNumSc = 2;
                    iHTuneNumSc = 260;
                    iHLogoPos = 0;
                    _iLMargin = LP2844_LMARGIN_SCNT_57MM;
                    iVLogoPos = LP2844_VPOS_LOGO_57MM;
                    iBarcodeLMargin = LP2844_LMARGIN_BARCODE_57MM;
                }
                else // 80mm
                {
                    if (_LegacyPrinterParams.iFontType > 0)
                    {
                        // SOFT FONT
                        iChFontNormal = 'a' + _LegacyPrinterParams.iFontType - 1;
                        iChSizeNormal = 1;
                    }
                    else
                    {
                        iChFontNormal = '1';
                        iChSizeNormal = 2;
                    }

                    iChFontNumSc = '3'; iChSizeNumSc = 2;
                    iHTuneNumSc = 320;
                    iHLogoPos = LP2844_LMARGIN_LOGO_80MM;
                    iVLogoPos = LP2844_VPOS_LOGO_80MM;
                    _iLMargin = LP2844_LMARGIN_SCNT_80MM;
                    iBarcodeLMargin = LP2844_LMARGIN_BARCODE_80MM;
                }
            }

            // Impostazioni non volatili di stampa
            PrintLine("Q200,0");

            if (!_bIsDati)
            {
                switch (_LegacyPrinterParams.iPaperSize) // Form Width 8dot/mm
                {
                    case (int)LegacyPrinterDlg.PAPER_SIZE.S57MM:
                        PrintLine("q448");
                        break;
                    default: // 80mm
                        PrintLine("q632");
                        break;
                }
            }
            else
                PrintLine("q632");

            switch (_LegacyPrinterParams.iPaperSpeed) // 1..4
            {
                case (int)LegacyPrinterDlg.RANGE.LOW:
                    PrintLine("S2");   // paper speed
                    break;
                case (int)LegacyPrinterDlg.RANGE.HIGH:
                    PrintLine("S4");
                    break;
                default:
                    PrintLine("S3");
                    break;
            }
            Thread.Sleep(100);

            switch (_LegacyPrinterParams.iDensity)  // 0..15
            {
                case (int)LegacyPrinterDlg.RANGE.LOW:
                    PrintLine("D4");   // print density
                    break;
                case (int)LegacyPrinterDlg.RANGE.HIGH:
                    PrintLine("D14");
                    break;
                default:
                    PrintLine("D10");
                    break;
            }

            //      Disable Top Of Form
            // 		PrintLine("oRE"); // set Euro char (Writes memory)
            //		PrintLine("R1,1"); // alignement
            PrintLine("JC");   // disable top of form backuP

            /*************************************************************
                I comandi di cui sopra possono provocare dei ritardi nella
                loro esecuzione, quindi bisogna far seguire uno Sleep(100)
             *************************************************************/

            Thread.Sleep(200);
        }


         /// <summary>
         /// esegue un Autotest se supportato
         /// </summary>
        public static void PrintAutoTest()
        {
            PrintLine("UP");  // self print
            PrintLine("\nN"); // page clear
        }

        /// <summary>Info se supportato</summary>
        public static void PrintInfo()
        {
            // PrintLine("FI"); // Form Info
            PrintLine("GI"); // Graphic Info
            PrintLine("EI"); // Soft Font Info
            PrintLine("\nN"); // page clear
        }

        /// <summary>
        /// stampa del file di tipo: DATI, LISTINO, SCONTRINO
        /// </summary>
        public static void PrintFile(String sFileToPrintParam)
        {
            String sInStr, sTmp;
            String sNumTicket = "0123", sDataStr, sGruppoStampa = "1";
            StreamReader fTxtFile = null;
            int i, j, iGruppoStampa, iPos;

            bool bOneTimeNum = true;
            bool bLogo, bCopiaCucina;

            _sFileToPrint = sFileToPrintParam;

            sDataStr = GetActualDate().ToString("ddMMyy");

            Init();
            bCopiaCucina = false;

            /******************************************
                stampa il Logo solo se il file
                è un Receipt o il Testo di prova
             ******************************************/
            sTmp = Path.GetFileName(sFileToPrintParam);

            if (sTmp.Contains("_TT") || sFileToPrintParam.Contains(NOME_FILE_SAMPLE_TEXT))
            {
                bLogo = true;
            }
            else if (sTmp.Contains("_CT")) // copia
            {
                bLogo = false;

                iPos = sTmp.IndexOf("_G");
                sGruppoStampa = sTmp.Substring(iPos + 2, 1); // ??? controllare

                iGruppoStampa = ToInt32(sGruppoStampa);

#if STANDFACILE
                _bStampaBarcode = IsBitSet(SF_Data.iBarcodeRichiesto, iGruppoStampa);
#endif
            }
            else
                bLogo = false;

            /*************************************
                if(bCopiaCucina) stampa BARCODE
             *************************************/
#if STANDFACILE
            if (sTmp.Contains("_CT"))
                bCopiaCucina = true;
            else
                bCopiaCucina = false;
#endif

#if STAND_CUCINA
	if (sTmp.Contains("CT_") || (_sFileToPrint == NOME_FILE_STAMPA_RECEIPT))
		bCopiaCucina = true; //StandCucina stampa solo copie
	else
		bCopiaCucina = false;
#endif

            if (String.IsNullOrEmpty(_sFileToPrint))
            {
                _WrnMsg.sNomeFile = _sFileToPrint;
                _WrnMsg.iErrID = WRN_FNO;
                WarningManager(_WrnMsg);
            }
            else if (File.Exists(_sFileToPrint))
            {
                try
                {
                    // stampa il Logo solo se il file è un Receipt
                    sTmp = Path.GetFileName(sFileToPrintParam);

                    if (sTmp.Contains("ST") || sFileToPrintParam.Contains(NOME_FILE_SAMPLE_TEXT))
                        bLogo = true;
                    else
                        bLogo = false;

                    if (_bIsDati && (_LegacyPrinterParams.iPaperSize == (int)LegacyPrinterDlg.PAPER_SIZE.S57MM))
                        WarningManager(WRN_SDN);

                    if (File.Exists(_sFileToPrint))
                    {
                        fTxtFile = File.OpenText(_sFileToPrint);

                        LogToFile(String.Format("Printer_LP2844 : inizio stampa di : {0}", _sFileToPrint));
                        _iRowAdvance = 100; // RIGA DI START STAMPA

                        if ((_LegacyPrinterParams.iLogoBmp != 0) && bLogo)
                        {   // Stampa del Logo
                            sArray = String.Format("GG{0},0,\"Logo{1}\"\n", iHLogoPos, _LegacyPrinterParams.iLogoBmp);
                            PrintLine(sArray);
                            _iRowAdvance += iVLogoPos;
                        }

                        while ((sInStr = fTxtFile.ReadLine()) != null)
                        {
                            iPos = sInStr.IndexOf(_TICK_NUM); // strLenght = 5 attenzione alla corrispondenza stringa !

                            // costruzione Num. scontrino per Barcode
                            if (iPos != -1)
                            {
                                sTmp = sInStr.Substring(iPos + _TICK_NUM.Length);
                                sTmp = sTmp.Trim();

                                sNumTicket = String.Format("{0:d4}", Convert.ToInt32(sTmp));
                            }

                            for (i = 0; i < _iLMargin; i++) // centratura
                                sInStr = sInStr.Insert(0, " ");

                            // conversione in stringa
                            sFNrm = "" + (char)iChFontNormal;
                            sFNSc = "" + (char)iChFontNumSc;

                            // messa in evidenza del numero Scontrino su carta da 57mm
                            if (sInStr.Contains(_TICK_NUM) && (!_bIsDati) && bOneTimeNum &&
                                (_LegacyPrinterParams.iPaperSize == (int)LegacyPrinterDlg.PAPER_SIZE.S80MM))
                            {
                                bOneTimeNum = false;

                                sInStr = sInStr.Trim();
                                j = sInStr.IndexOf(" ");
                                sInStr = sInStr.Remove(0, j);
                                sInStr = sInStr.Trim();

                                // stampa "Num. "
                                sArray = String.Format("A160,{0},0,{1},{2},{3},N,\"{4}\"\n",
                                        _iRowAdvance + 12, sFNrm, iChSizeNormal, iChSizeNormal, _TICK_NUM);
                                PrintLine(sArray);

                                // prepara stampa numero
                                sArray = String.Format("A{0},{1},0,{2},{3},{4},N,\"{5}\"\n",
                                    iHTuneNumSc, _iRowAdvance, sFNSc, iChSizeNumSc, iChSizeNumSc, sInStr);
                                PrintLine(sArray);
                                _iRowAdvance += 20;
                            }
                            else
                                sArray = String.Format("A0,{0},0,{1},{2},{3},N,\"{4}\"\n",
                                    _iRowAdvance, sFNrm, iChSizeNormal, iChSizeNormal, sInStr);

                            PrintLine(sArray, 40);
                            // sotto i 40ms non stampa scontrini lunghi con Soft Font
                            ResetClosedelay();
                            _iRowAdvance += ((iZebra_FontVSize[iChFontNormal] + 4) * iChSizeNormal);
                        }

                        /******************************************************
                                BAR CODE type = EAN 13 (12 + checksum)
                         ******************************************************/

                        if ((bCopiaCucina && _bStampaBarcode) || sFileToPrintParam.Contains(NOME_FILE_SAMPLE_TEXT))
                        {
                            sTmp = String.Format("0{0}{1}{2}", sGruppoStampa[1], sDataStr, sNumTicket);

                            sArray = String.Format("B{0},{0},0,E30,3,3,100,B,\"{0}\"\n", iBarcodeLMargin, _iRowAdvance - 50, sTmp);
                            PrintLine(sArray, 40);
                        }

                        // fine della stampa              
                        PrintLine("P1", 200);
                        PrintLine("\nN", 100); // page clear

                        fTxtFile.Close();
                        LogToFile(String.Format("Printer_LP2844 : fine stampa di {0}", _sFileToPrint));
                        _sFileToPrint = "";
                    }
                }
                catch (TimeoutException) // COM occupata
                {
                    _WrnMsg.iErrID = WRN_STF;
                    _WrnMsg.sMsg = _LegacyPrinterParams.sPort;
                    WarningManager(_WrnMsg);
                    return;
                }
            }
        }
    } // end class
} // end namespace

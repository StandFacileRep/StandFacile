/*****************************************************************************
     NomeFile : StandCommonSrc/Printer_Epson_POS.cs
     Data	   : 20.04.2024
     Autore   : Mauro Artuso

     Caratteristiche stampante :
     STAMPANTE_TM_T88_II : 180dpi, carta 80mm 521 dot
     STAMPANTE_TM_L90    : 203dpi, carta 80mm 640 dot

     Font A : 12x24, Font B : 9x17

     con carta 80mm, Font A x1 si ha 521:12=42 caratteri per riga
     con carta 80mm, Font B x1 si ha 521: 9=56 caratteri per riga
     con carta 80mm, Font B x2 si ha 521:18=29 caratteri per riga
     NV Image buffer 256KBit
     ****************************************************************************/

using System;
using System.IO;
using static System.Convert;

using static StandCommonFiles.ComDef;
using static StandCommonFiles.CommonCl;
using static StandCommonFiles.LogServer;
using static StandCommonFiles.Printer_Legacy;

using static StandFacile.glb;
using static StandFacile.Define;

#if !STAND_CUCINA
using static StandFacile.dBaseIntf;
#endif

namespace StandCommonFiles
{
    /// <summary>
    ///  classe per la gestione della stampante seriale e/o parallela
    /// </summary>
    public static class TPrinter_TM_POS
    {
#pragma warning disable IDE0044

        // margini Printer Termica TM T88 II
        const String TM_T88_CUT = "\x1DV0";

        const int TM_T88_LMARGIN_SCNT_FSTD = 0;
        const int TM_T88_LMARGIN_SCNT_FRID = 6;
        const int TM_T88_LMARGIN_DATA_RID = 4;
        const int TM_T88_LMARGIN_DATA = 4;

        const int TM_L90_LMARGIN_SCNT_FSTD = 2;
        const int TM_L90_LMARGIN_SCNT_FRID = 9;
        const int TM_L90_LMARGIN_DATA_RID = 4;
        const int TM_L90_LMARGIN_DATA = 4;

        static int _iLMargin;
        static int _Font;
        static int iCharCnt;

        static bool _bIsDati = false;
        static bool _bIsDatiRid = false;
        static bool _bStampaBarcode = false;

        static String _sFileToPrint = "";

        static char[] cBuffer = new char[256];
        static TErrMsg _WrnMsg = new TErrMsg();

        /// <summary>
        ///  impostazione dei font, spacing, etc...
        /// </summary>
        static void Init()
        {
            int iCharCnt;

            // Printer Init
            iCharCnt = 0;
            cBuffer[iCharCnt++] = (char)0x1B;
            cBuffer[iCharCnt++] = '@';

            _bIsDati = false;
            _bIsDatiRid = false;

#if STANDFACILE || STAND_MONITOR

            // numero di colonne ridotto -> font più grande
            if (_sFileToPrint != null)
            {
                if (_sFileToPrint.Contains(NOME_FILE_STAMPA_LOC_RID_TMP) || _sFileToPrint.Contains("Listino"))
                    _bIsDatiRid = true;
                else if (_sFileToPrint.Contains(NOME_FILE_STAMPA_LOC_TMP) || _sFileToPrint.Contains("_Dati"))
                    _bIsDati = true;
            }
#else
	        _bStampaBarcode = (ReadRegistry(STAMPA_BARCODE_KEY, 0) == 1);
#endif

            if (_bIsDatiRid)
            {
                // Selezione Font A 12x24
                cBuffer[iCharCnt++] = (char)0x1B; cBuffer[iCharCnt++] = 'M'; cBuffer[iCharCnt++] = (char)0x00;
                // Selezione Size 1x1
                cBuffer[iCharCnt++] = (char)0x1D; cBuffer[iCharCnt++] = '!'; cBuffer[iCharCnt++] = (char)0x00;
                // Selezione line spacing
                cBuffer[iCharCnt++] = (char)0x1B; cBuffer[iCharCnt++] = '3'; cBuffer[iCharCnt++] = (char)0x38;

                PrintBuffer(cBuffer, iCharCnt);

                if (sGlbLegacyPrinterParams.iPrinterModel == (int)LEGACY_PRINTER_MODELS.STAMPANTE_TM_T88_SER)
                    _iLMargin = TM_T88_LMARGIN_DATA_RID;
                else
                    _iLMargin = TM_L90_LMARGIN_DATA_RID;
            }
            else if (_bIsDati)
            {
                // Selezione Font 9x17
                cBuffer[iCharCnt++] = (char)0x1B; cBuffer[iCharCnt++] = 'M'; cBuffer[iCharCnt++] = (char)0x01;
                // Selezione Size 1x1
                cBuffer[iCharCnt++] = (char)0x1D; cBuffer[iCharCnt++] = '!'; cBuffer[iCharCnt++] = (char)0x00;
                // Selezione line spacing
                cBuffer[iCharCnt++] = (char)0x1B; cBuffer[iCharCnt++] = '3'; cBuffer[iCharCnt++] = (char)0x38;

                PrintBuffer(cBuffer, iCharCnt);

                if (sGlbLegacyPrinterParams.iPrinterModel == (int)LEGACY_PRINTER_MODELS.STAMPANTE_TM_T88_SER)
                    _iLMargin = TM_T88_LMARGIN_DATA;
                else
                    _iLMargin = TM_L90_LMARGIN_DATA;
            }
            else
            {
                SetFont(_LegacyPrinterParams.iFontType);
            }

        }

        /// <summary>
        /// imposta il font
        /// </summary>
        public static void SetFont(int iFontType)
        {
            _Font = iFontType;
            iCharCnt = 0;

            if (iFontType == 0)
            {
                // Selezione Font 9x17
                cBuffer[iCharCnt++] = (char)0x1B; cBuffer[iCharCnt++] = 'M'; cBuffer[iCharCnt++] = (char)0x01;
                // Selezione Size 2x2
                cBuffer[iCharCnt++] = (char)0x1D; cBuffer[iCharCnt++] = '!'; cBuffer[iCharCnt++] = (char)0x11;
                // Selezione line spacing
                cBuffer[iCharCnt++] = (char)0x1B; cBuffer[iCharCnt++] = '3'; cBuffer[iCharCnt++] = (char)0x64;

                _iLMargin = TM_T88_LMARGIN_SCNT_FSTD;

                if (sGlbLegacyPrinterParams.iPrinterModel == (int)LEGACY_PRINTER_MODELS.STAMPANTE_TM_T88_SER)
                    _iLMargin = TM_T88_LMARGIN_SCNT_FSTD;
                else
                    _iLMargin = TM_L90_LMARGIN_SCNT_FSTD;
            }
            else
            {
                // Selezione Font 12x24
                cBuffer[iCharCnt++] = (char)0x1B; cBuffer[iCharCnt++] = 'M'; cBuffer[iCharCnt++] = (char)0x00;
                // Selezione Size 1x1
                cBuffer[iCharCnt++] = (char)0x1D; cBuffer[iCharCnt++] = '!'; cBuffer[iCharCnt++] = (char)0x00;
                // Selezione line spacing
                cBuffer[iCharCnt++] = (char)0x1B; cBuffer[iCharCnt++] = '3'; cBuffer[iCharCnt++] = (char)0x38;

                if (sGlbLegacyPrinterParams.iPrinterModel == (int)LEGACY_PRINTER_MODELS.STAMPANTE_TM_T88_SER)
                    _iLMargin = TM_T88_LMARGIN_SCNT_FRID;
                else
                    _iLMargin = TM_L90_LMARGIN_SCNT_FRID;
            }

            PrintBuffer(cBuffer, iCharCnt); // ??? file vuoto on break
        }

        /// <summary>
        /// esegue un Autotest se supportato
        /// </summary>
        public static void PrintAutoTest()
        {
            int iCharCnt = 0;

            cBuffer[iCharCnt++] = (char)0x1D; cBuffer[iCharCnt++] = '('; cBuffer[iCharCnt++] = 'A';
            cBuffer[iCharCnt++] = (char)0x00; cBuffer[iCharCnt++] = (char)0x02;
            cBuffer[iCharCnt++] = (char)0x00; cBuffer[iCharCnt++] = (char)0x02;

            LogToFile(String.Format("Printer_EPSON : PrintAutoTest"));

            PrintBuffer(cBuffer, iCharCnt);
        }

        /// <summary>
        /// stampa del file di tipo: DATI, LISTINO, SCONTRINO
        /// </summary>
        public static void PrintFile(String sFileToPrintParam)
        {
            int i, iGruppoStampa, iPos;
            bool bLogo, bCopiaCucina;

            String sInStr, sTmp;
            String sReceiptNum = "0123", sDataStr, sGruppoStampa = "0";

            StreamReader fTxtFile = null;

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
                SetFont(0); //Font grande con stampa copie

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
                    fTxtFile = File.OpenText(_sFileToPrint);

                    LogToFile(String.Format("Printer_TM_T88 : inizio stampa di : {0}", _sFileToPrint));

                    // Stampa del Logo
                    if ((_LegacyPrinterParams.iLogoBmp != 0) && bLogo)
                    {
                        // Stampa del Logo
                        iCharCnt = 0;
                        cBuffer.SetValue((char)0x1C, iCharCnt++); cBuffer.SetValue((char)0x70, iCharCnt++);
                        cBuffer.SetValue((char)_LegacyPrinterParams.iLogoBmp, iCharCnt++); cBuffer.SetValue((char)0x00, iCharCnt++);
                        PrintBuffer(cBuffer, iCharCnt, 800);
                        PrintLine("");
                    }

                    while ((sInStr = fTxtFile.ReadLine()) != null)
                    {
                        // paper CUT
                        if (sInStr.Contains(_CUT))
                        {
                            PrintLine("\n\n\n\n\n");
                            PrintLine(TM_T88_CUT);
                            
                            fTxtFile.ReadLine();
                            fTxtFile.ReadLine();
                            
                            continue;
                        }

                        iPos = sInStr.IndexOf(_TICK_NUM); // strLenght = 5 attenzione alla corrispondenza stringa !

                        // costruzione Num. scontrino per Barcode
                        if (iPos != -1)
                        {
                            sTmp = sInStr.Substring(iPos + _TICK_NUM.Length);
                            sTmp = sTmp.Trim();

                            sReceiptNum = String.Format("{0:d4}", Convert.ToInt32(sTmp));
                        }

                        for (i = 0; i < _iLMargin; i++) // centratura
                            sInStr = sInStr.Insert(0, " ");

                        PrintLine(sInStr);
                    }

                    /******************************************************
                                       BAR CODE EAN13
                      effettua la stampa solo se il flag bStampaBarcode
                      è abilitato dal dialogo FrmImpostaTipoCassa
                     ******************************************************/
                    if ((bCopiaCucina && _bStampaBarcode) || sFileToPrintParam.Contains(NOME_FILE_SAMPLE_TEXT))
                    {
                        iCharCnt = 0;

                        //Hor left margin
                        cBuffer.SetValue((char)0x1B, iCharCnt++); cBuffer[iCharCnt++] = '$'; cBuffer.SetValue((char)120, iCharCnt++); cBuffer.SetValue((char)0, iCharCnt++);
                        //barcode height
                        cBuffer.SetValue((char)0x1D, iCharCnt++); cBuffer[iCharCnt++] = 'h'; cBuffer.SetValue((char)80, iCharCnt++);
                        //barcode width
                        cBuffer.SetValue((char)0x1D, iCharCnt++); cBuffer[iCharCnt++] = 'w'; cBuffer.SetValue((char)3, iCharCnt++); // 1..6
                        //HRI position
                        cBuffer.SetValue((char)0x1D, iCharCnt++); cBuffer[iCharCnt++] = 'H'; cBuffer.SetValue((char)2, iCharCnt++);
                        //HRI font
                        cBuffer.SetValue((char)0x1D, iCharCnt++); cBuffer[iCharCnt++] = 'f'; cBuffer.SetValue((char)1, iCharCnt++);
                        //barcode type = EAN 13 (12 + checksum)
                        cBuffer.SetValue((char)0x1D, iCharCnt++); cBuffer[iCharCnt++] = 'k'; cBuffer.SetValue((char)67, iCharCnt++); cBuffer.SetValue((char)12, iCharCnt++);

                        cBuffer[iCharCnt++] = '0';
                        cBuffer[iCharCnt++] = sGruppoStampa[0]; // gruppo

                        cBuffer[iCharCnt++] = sDataStr[0]; // dd
                        cBuffer[iCharCnt++] = sDataStr[1]; // dd
                        cBuffer[iCharCnt++] = sDataStr[2]; // dd
                        cBuffer[iCharCnt++] = sDataStr[3]; // dd
                        cBuffer[iCharCnt++] = sDataStr[4]; // dd
                        cBuffer[iCharCnt++] = sDataStr[5]; // dd

                        cBuffer[iCharCnt++] = sReceiptNum[0]; // num
                        cBuffer[iCharCnt++] = sReceiptNum[1]; // num
                        cBuffer[iCharCnt++] = sReceiptNum[2]; // num
                        cBuffer[iCharCnt++] = sReceiptNum[3]; // num

                        PrintBuffer(cBuffer, iCharCnt);
                        PrintLine("");
                    }

                    // fine della stampa
                    PrintLine("\n\n");

                    if (_Font == 1)
                        PrintLine("\n\n");

                    if (_bIsDati || _bIsDatiRid)
                        PrintLine("\n\n\n\n\n");

                    // paper CUT
                    cBuffer.SetValue((char)0x1D, 0); cBuffer.SetValue('V', 1); cBuffer.SetValue('0', 2);
                    PrintBuffer(cBuffer, 3, 400);

                    fTxtFile.Close();
                    LogToFile(String.Format("Printer_EPSON : fine stampa di {0}", _sFileToPrint));
                    _sFileToPrint = null;
                }
                catch (Exception)
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

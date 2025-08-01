/*****************************************************
 	NomeFile : StandCommonSrc/CommonFunc.cs
    Data	 : 24.07.2025
 	Autore	 : Mauro Artuso

	Classi statiche di uso comune
 *****************************************************/

using System;
using System.IO;
using System.Text;
using Microsoft.Win32;
using System.Windows.Forms;
using System.Drawing;
using System.Xml.Serialization;

using System.Security.Cryptography;
using System.Runtime.Serialization.Formatters.Binary;

using static StandCommonFiles.ComDef;
using static StandCommonFiles.ComSafe;

using StandFacile;
using static StandFacile.glb;
using static StandFacile.Define;

namespace StandCommonFiles
{

    /// <summary>
    /// definizione dei codici di errore e di warning
    /// </summary>
    public static partial class CommonCl
    {
        /// <summary>data all'avvio del programma</summary>
        static DateTime actualDate;

        /// <summary>indica se c'è già una istanza del Programma in esecuzione</summary>
        public static bool bApplicationRuns;

        /// <summary>struct per gestione configurazione da File</summary>
        public static TConfig sConfig = new TConfig(0);

        /// <summary>verifica se la stringa passata è contenuta nella chiave "serviceStrings"</summary>
        public static bool CheckService(String sString)
        {
            if (!String.IsNullOrEmpty(sConfig.sService) && sConfig.sService.Contains(sString))
                return true;
            else
                return false;
        }

        /// <summary>
        /// legge dal Registry un intero ritornando il valore
        /// iDef in caso di errore come .NET
        /// </summary>
        public static int ReadRegistry(String sArray, int iDef)
        {
            try
            {
                // Apertura della chiave presente nel Registro di Windows
#if STANDFACILE
                return (int)Registry.GetValue(KEY_STAND_FACILE, sArray, iDef);
#elif STAND_CUCINA
                return (int)Registry.GetValue(KEY_STAND_CUCINA, sArray, iDef);
#elif STAND_ORDINI
                return (int)Registry.GetValue(KEY_STAND_ORDINI, sArray, iDef);
#elif STAND_MONITOR
                return (int)Registry.GetValue(KEY_STAND_MONITOR, sArray, iDef);
#else
                return 0;
#endif
            }

            catch (Exception)
            {
                return iDef;
            }
        }

        /// <summary>
        /// legge dal Registry una AnsiString
        /// </summary>
        public static String ReadRegistry(String sParam, String sDefStr)
        {
            String sDebug;

            try
            {
                // Apertura della chiave presente nel Registro di Windows
#if STANDFACILE
                sDebug = (String)Registry.GetValue(KEY_STAND_FACILE, sParam, sDefStr);
#elif STAND_CUCINA
                sDebug = (String)Registry.GetValue(KEY_STAND_CUCINA, sParam, sDefStr);
#elif STAND_ORDINI
                sDebug = (String)Registry.GetValue(KEY_STAND_ORDINI, sParam, sDefStr);
#elif STAND_MONITOR
                sDebug = (String)Registry.GetValue(KEY_STAND_MONITOR, sParam, sDefStr);
#else
                sDebug = sDefStr;
#endif
            }

            catch (Exception)
            {
                return sDefStr;
            }

            if (String.IsNullOrEmpty(sDebug))
                return sDefStr;
            else
                return sDebug;
        }


        /// <summary>
        /// Scrittura di un float nel Registro di Windows
        /// </summary>
        public static void WriteRegistry(String sArrayName, float fVal)
        {
            int iVal = (int)fVal;
            // Apertura/Creazione della chiave presente nel Registro di Windows
#if STANDFACILE
            Registry.SetValue(KEY_STAND_FACILE, sArrayName, iVal);
#elif STAND_CUCINA
            Registry.SetValue(KEY_STAND_CUCINA, sArrayName, iVal);
#elif STAND_ORDINI
            Registry.SetValue(KEY_STAND_ORDINI, sArrayName, iVal);
#elif STAND_MONITOR
            Registry.SetValue(KEY_STAND_MONITOR, sArrayName, iVal);
#endif
        }

        /// <summary>
        /// Scrittura di un int nel Registro di Windows
        /// </summary>
        public static void WriteRegistry(String sArrayName, int iVal)
        {
            // Apertura/Creazione della chiave presente nel Registro di Windows
#if STANDFACILE
            Registry.SetValue(KEY_STAND_FACILE, sArrayName, iVal);
#elif STAND_CUCINA
            Registry.SetValue(KEY_STAND_CUCINA, sArrayName, iVal);
#elif STAND_ORDINI
            Registry.SetValue(KEY_STAND_ORDINI, sArrayName, iVal);
#elif STAND_MONITOR
            Registry.SetValue(KEY_STAND_MONITOR, sArrayName, iVal);
#endif
        }

        /// <summary>
        /// Scrittura di una stringa nel Registro di Windows
        /// </summary>
        public static void WriteRegistry(String sArrayName, String sArrayVal)
        {
            // Apertura/Creazione della chiave presente nel Registro di Windows
#if STANDFACILE
            Registry.SetValue(KEY_STAND_FACILE, sArrayName, sArrayVal);
#elif STAND_CUCINA
            Registry.SetValue(KEY_STAND_CUCINA, sArrayName, sArrayVal);
#elif STAND_ORDINI
            Registry.SetValue(KEY_STAND_ORDINI, sArrayName, sArrayVal);
#elif STAND_MONITOR
            Registry.SetValue(KEY_STAND_MONITOR, sArrayName, sArrayVal);
#endif
        }

        /// <summary>
        /// Scrittura di un bool nel Registro di Windows come intero
        /// </summary>
        public static void WriteRegistry(String sArrayName, bool bVal)
        {
            // Apertura/Creazione della chiave presente nel Registro di Windows
#if STANDFACILE
            if (bVal)
                Registry.SetValue(KEY_STAND_FACILE, sArrayName, 1);
            else
                Registry.SetValue(KEY_STAND_FACILE, sArrayName, 0);
#elif STAND_CUCINA
            if (bVal)
                Registry.SetValue(KEY_STAND_CUCINA, sArrayName, 1);
            else
                Registry.SetValue(KEY_STAND_CUCINA, sArrayName, 0);
#elif STAND_ORDINI
            if (bVal)
                Registry.SetValue(KEY_STAND_ORDINI, sArrayName, 1);
            else
                Registry.SetValue(KEY_STAND_ORDINI, sArrayName, 0);
#elif STAND_MONITOR
            if (bVal)
                Registry.SetValue(KEY_STAND_MONITOR, sArrayName, 1);
            else
                Registry.SetValue(KEY_STAND_MONITOR, sArrayName, 0);
#endif
        }

        /// <summary>ottiene la data attuale</summary>
        public static DateTime GetActualDate()
        {
            return actualDate;
        }

        /// <summary>imposta la data attuale</summary>
        public static void SetActualDate(DateTime dateParam)
        {
            actualDate = dateParam;
        }

        /// <summary>
        /// impostazione della data: è necessario effettuarla prima di lanciare la form di avvio,
        /// il giorno corrente finisce alle ore 05.00
        /// </summary>
        public static void InitActualDate(int iTestHourPrm = -1, int iTestMinPrm = -1)
        {
            if ((iTestHourPrm == -1) && (iTestMinPrm == -1))
                actualDate = DateTime.Now;
            else
            {
                actualDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, iTestHourPrm, iTestMinPrm, 0);
            }

            // gli scontrini oltre la mezzanotte sono relativi al giorno precedente
            if (actualDate.Hour < 5)
                actualDate = actualDate.AddDays(-1);
        }

        /// <summary>ritorna una stringa con la Data e l'ora corrente</summary>
        public static String GetDateTimeString(bool bIsTest = false)
        {
            String sDateTime, sTime, sDate;

            sDate = actualDate.ToString("ddd dd/MM/yy");

            if (bIsTest)
                sTime = actualDate.ToString("HH.mm.ss");
            else
                sTime = DateTime.Now.ToString("HH.mm.ss");

            sDateTime = String.Format("{0} {1}", sDate, sTime);

            return sDateTime;
        }

        /// <summary>
        /// Funzione per centrare le stringhe sulla larghezza della carta termica
        /// </summary>
        public static String CenterJustify(String sText, int iWidth)
        {
            int i, iLength;
            String sTmp;

            // non centra stringhe vuote
            if (String.IsNullOrEmpty(sText))
                return "";

            sTmp = sText.Trim();
            iLength = sTmp.Length;

            for (i = 0; (i + iLength) < iWidth; i += 2)
                sTmp = sTmp.Insert(0, " ");

            return sTmp;
        }

        /// <summary>
        /// Funzione per centrare le stringhe sulla larghezza della carta termica 
        /// non aggiungendo spazi ma caratteri come *, #
        /// </summary>
        public static String CenterJustifyStars(String sText, int iWidth, char ch)
        {
            int i, iLength, iReducedWidth;
            String sTmp;

            // limita il numero di caratteri diversi dallo spazio
            iReducedWidth = iWidth - 4;

            // non centra stringhe vuote
            if (String.IsNullOrEmpty(sText))
                return "";

            sTmp = sText.Trim();
            iLength = sTmp.Length;

            // se c'è abbastanza spazio aggiungi 2 o più spazi ' '
            if (iLength < (iReducedWidth - 8))
                sTmp = sTmp.Insert(0, "   ") + "   ";
            else if (iLength < (iReducedWidth - 6))
                sTmp = sTmp.Insert(0, "  ") + "  ";
            else if (iLength < (iReducedWidth - 4))
                sTmp = sTmp.Insert(0, " ") + " ";

            // aggiornamento iLength
            iLength = sTmp.Length;

            // aggiungi ch
            for (i = 0; (i + iLength) < iReducedWidth; i += 2)
                sTmp = sTmp.Insert(0, ch.ToString()) + ch.ToString();

            // aggiornamento iLength
            iLength = sTmp.Length;

            // completa centraggio con spazi
            for (i = 0; (i + iLength) < iWidth; i += 2)
                sTmp = sTmp.Insert(0, " ");

            return sTmp;
        }

        /// <summary>funzione di calcolo checksum per (string, int)</summary>
        public static uint Hash(String sData, int iParam = 0)
        {
            uint uCkecksum;

            MD5 md5 = MD5.Create();
            sData = sData.Trim();

            if (iParam > 0)
                sData += iParam.ToString();

            byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(sData);
            byte[] hash = md5.ComputeHash(inputBytes);

            uCkecksum = BitConverter.ToUInt32(hash, 0);

            return uCkecksum;
        }

        /// <summary>overload funzione di calcolo checksum</summary>
        public static uint Hash(int iData)
        {
            String sData = iData.ToString();
            return Hash(sData);
        }

        /// <summary>costruzione file di test stampa</summary>
        public static String BuildSampleText()
        {
            int i, j, ich;
            int iLMargin = 2;
            int iMaxChars = 24;

            String sTmp;
            StreamWriter fData;
            String sDir = "";
            TErrMsg ErrMsg = new TErrMsg();

#if STANDFACILE
            sDir = DataManager.GetExeDir() + "\\";
#endif

#if STAND_MONITOR
            sDir = sRootDir + "\\";
#endif

#if !STAND_ORDINI
            iMaxChars = (sGlbWinPrinterParams.bChars33 ? 33 : 28) - 4;
#endif

            File.Delete(sDir + NOME_FILE_SAMPLE_TEXT);
            fData = File.CreateText(sDir + NOME_FILE_SAMPLE_TEXT);
            if (fData == null)
            {
                ErrMsg.sNomeFile = NOME_FILE_SAMPLE_TEXT;
                ErrMsg.iErrID = ERR_FNO;
                ErrorManager(ErrMsg);
            }
            else
            {
                fData.WriteLine("");

#if STANDFACILE || STAND_MONITOR

                sTmp = CenterJustify(_LOGO, iMaxChars);

                for (j = 0; j < iLMargin; j++)
                    sTmp = sTmp.Insert(0, " ");
                fData.WriteLine(sTmp + "\n");

                sTmp = CenterJustify(SF_Data.sHeaders[0], iMaxChars);

                for (j = 0; j < iLMargin; j++)
                    sTmp = sTmp.Insert(0, " ");

                if (!String.IsNullOrEmpty(SF_Data.sHeaders[0]))
                    fData.WriteLine(sTmp + "\n");

                sTmp = CenterJustify(SF_Data.sHeaders[1], iMaxChars);

                for (j = 0; j < iLMargin; j++)
                    sTmp = sTmp.Insert(0, " ");

                if (!String.IsNullOrEmpty(SF_Data.sHeaders[1]))
                    fData.WriteLine(sTmp + "\n");
#elif STAND_CUCINA
                sTmp = CenterJustify(dBaseIntf.DB_Data.sHeaders[0], iMaxChars);

                for (j = 0; j < iLMargin; j++)
                    sTmp = sTmp.Insert(0, " ");

                if (!String.IsNullOrEmpty(dBaseIntf.DB_Data.sHeaders[0]))
                    fData.WriteLine(sTmp + "\n");

                sTmp = CenterJustify(dBaseIntf.DB_Data.sHeaders[1], iMaxChars);

                for (j = 0; j < iLMargin; j++)
                    sTmp = sTmp.Insert(0, " ");

                if (!String.IsNullOrEmpty(dBaseIntf.DB_Data.sHeaders[1]))
                    fData.WriteLine(sTmp + "\n");
#endif

                sTmp = CenterJustify(GetDateTimeString(), iMaxChars);
                for (j = 0; j < iLMargin; j++)
                    sTmp = sTmp.Insert(0, " ");
                fData.WriteLine(sTmp + "\n");

                for (i = 0; i < iMaxChars / 2; i++)       // avanzamanto riga
                {
                    sTmp = "";
                    for (j = 0; j < iMaxChars; j++) // ripetizione riga
                    {
                        ich = '0' + (i + j);

                        if (('0' <= ich) && (ich <= '9') ||
                            ('A' <= ich) && (ich <= 'Z') ||
                            ('a' <= ich) && (ich <= 'z'))
                            sTmp += (char)ich;

                        else sTmp += '.';
                    }

                    for (j = 0; j < iLMargin; j++) // centratura
                        sTmp = sTmp.Insert(0, " ");

                    fData.WriteLine(sTmp);
                }

                sTmp = CenterJustify("########################", iMaxChars);
                for (j = 0; j < iLMargin; j++)
                    sTmp = sTmp.Insert(0, " ");

                fData.WriteLine("{0}", sTmp);
                fData.WriteLine("{0}", sTmp);

                fData.WriteLine("");

#if STANDFACILE
                sTmp = CenterJustify(SF_Data.sHeaders[2], iMaxChars);
                for (j = 0; j < iLMargin; j++)
                    sTmp = sTmp.Insert(0, " ");

                if (!String.IsNullOrEmpty(SF_Data.sHeaders[2]))
                    fData.WriteLine(sTmp + "\n");

                sTmp = CenterJustify(SF_Data.sHeaders[MAX_NUM_HEADERS - 1], iMaxChars);
                for (j = 0; j < iLMargin; j++)
                    sTmp = sTmp.Insert(0, " ");

                if (!String.IsNullOrEmpty(SF_Data.sHeaders[MAX_NUM_HEADERS - 1]))
                    fData.WriteLine(sTmp + "\n");
#elif STAND_CUCINA
                sTmp = CenterJustify(dBaseIntf.DB_Data.sHeaders[2], iMaxChars);
                for (j = 0; j < iLMargin; j++)
                    sTmp = sTmp.Insert(0, " ");

                if (!String.IsNullOrEmpty(dBaseIntf.DB_Data.sHeaders[2]))
                    fData.WriteLine(sTmp + "\n");

                sTmp = CenterJustify(dBaseIntf.DB_Data.sHeaders[MAX_NUM_HEADERS - 1], iMaxChars);
                for (j = 0; j < iLMargin; j++)
                    sTmp = sTmp.Insert(0, " ");

                if (!String.IsNullOrEmpty(dBaseIntf.DB_Data.sHeaders[MAX_NUM_HEADERS - 1]))
                    fData.WriteLine(sTmp + "\n");
#endif
                fData.WriteLine("\n");
                fData.Close();
            }

            return sDir + NOME_FILE_SAMPLE_TEXT;
        }

#if !STAND_ORDINI
        /// <summary>decide dove fare la stampa generica</summary>
        public static void GenPrintFile(String sFileToPrintParm)
        {
#if STANDFACILE
            if (PrintLocalCopiesConfigDlg.GetPrinterTypeIsWinwows())
#else
            if (PrintConfigLightDlg.GetPrinterTypeIsWinwows())
#endif
                Printer_Windows.PrintFile(sFileToPrintParm);
            else
                Printer_Legacy.PrintFile(sFileToPrintParm);
        }
#endif

        /// <summary>
        /// conversione di un int in String Euro
        /// </summary>
        public static String IntToEuro(int iEuro)
        {
            String sText;

            // posiziona il punto decimale
            sText = String.Format("{0,3:d3}", iEuro);
            sText = sText.Insert(sText.Length - 2, ",");

            return sText;
        }

        /// <summary>
        /// aggiunge la voce corrente al combo se questa non esiste già e
        /// limita il numero delle voci a iMaxComboItemsParam
        /// </summary>
        public static void AddTo_ComboList(ComboBox Combo_NameParam, String sKeyParam)
        {
            int i, iPos;
            bool bExists = false;
            String sStrTmp, sTextParam;

            sTextParam = Combo_NameParam.Text;
            sTextParam.Trim();

            iPos = 0;
            for (i = 0; ((i < MAX_COMBO_ITEMS) && (i < Combo_NameParam.Items.Count) && !bExists); i++)
            {
                if (Combo_NameParam.Items[i].ToString() == sTextParam)
                {
                    bExists = true;
                    iPos = i;
                }
            }

            // inserisce la voce in prima posizione, togliendola da quelle successive
            if (bExists && (iPos > 0)) // se (bExists && (iPos == 0)) non fare nulla è già a posto
            {
                Combo_NameParam.Items.RemoveAt(iPos);
                Combo_NameParam.Items.Insert(0, sTextParam);
            }
            else if (!bExists)
            {
                Combo_NameParam.Items.Insert(0, sTextParam);
            }

            // necessaria altrimenti può sparire con ComboServerNameAddr.Items.RemoveAt(0);
            Combo_NameParam.Text = sTextParam;

            // limita il numero
            while (Combo_NameParam.Items.Count > MAX_COMBO_ITEMS)
                Combo_NameParam.Items.RemoveAt(MAX_COMBO_ITEMS);

            for (i = 0; (i < MAX_COMBO_ITEMS); i++)
            {
                sStrTmp = String.Format(sKeyParam, i);

                if (i < Combo_NameParam.Items.Count) // i parte da 0
                    WriteRegistry(sStrTmp, Combo_NameParam.Items[i].ToString());
                else
                    WriteRegistry(sStrTmp, "");
            }
        }

        /// <summary>ottiene il nome della tabella dati</summary>
        public static String GetNomeDatiDBTable(int iNumCassaParam, DateTime dateTimeParam)
        {
            String sTmp, sDati;

            sTmp = dateTimeParam.ToString("yyMMdd");

#if STANDFACILE
            if (SF_Data.bPrevendita)
                sDati = String.Format("{0}_c{1}_{2}", _dbPreDataTablePrefix, iNumCassaParam, sTmp);
            else
#endif
                sDati = String.Format("{0}_c{1}_{2}", _dbDataTablePrefix, iNumCassaParam, sTmp);

            return sDati;
        }

        /// <summary>ottiene il nome della tabella ordini</summary>
        public static String GetNomeOrdiniDBTable(DateTime dateTimeParam)
        {
            String sTmp, sOrdini;

            sTmp = dateTimeParam.ToString("yyMMdd");

#if STANDFACILE
            if (SF_Data.bPrevendita)
                sOrdini = String.Format("{0}_{1}", _dbPreOrdersTablePrefix, sTmp);
            else
#endif
                sOrdini = String.Format("{0}_{1}", _dbOrdersTablePrefix, sTmp);

            return sOrdini;
        }

        /// <summary>ottiene il nome del file dati</summary>
        public static String GetNomeFileDati(int iNumCassaParam, DateTime dateTimeParam)
        {
            String sTmp, sDati;

            sTmp = dateTimeParam.ToString("'Dati_'MMdd'.txt'");
            sDati = String.Format("C{0}_{1}", iNumCassaParam, sTmp);

            return sDati;
        }

        /// <summary>
        /// ottiene il nome del file backup dati
        /// </summary>
        public static String GetNomeFileDatiBak(int iNumCassaParam)
        {
            String sTmpBak, sDatiBak;

            sTmpBak = GetActualDate().ToString("'Dati_'MMdd'.bak'");
            sDatiBak = String.Format("C{0}_{1}", iNumCassaParam, sTmpBak);

            return sDatiBak;
        }

#if STANDFACILE
        /// <summary>
        /// ottiene il nome del file DB SQLite, ricostruisce tutto il Path
        /// </summary>
        public static String GetNomeFileDatiDB_SQLite(DateTime dateParam)
        {
            String sTmp, sAnno;

            sAnno = ANNO_DIR + dateParam.ToString("yyyy");

            sTmp = DataManager.GetRootDir() + "\\" + sAnno + "\\" + "Dati_Standfacile.db";
            return sTmp;
        }
#endif

        /// <summary>
        /// conversione di una String Euro in int*100,<br/>
        /// il parametro standard iErrThrow determina il comportamento,<br/>
        /// vedi struct EURO_CONVERSION in caso di errori di conversione:<br/>
        /// presenza di spazi interni alla stringa da convertire,<br/>
        /// presenza del segno '-' nella stringa da convertire,<br/>
        /// in dipendenza da iErrThrow si accetta o meno il prezzo nullo<br/>
        /// ritorna -1 <br/>
        /// ErrMsg da informazioni riguardo alla riga in cui c'è l'errore
        /// </summary>
        public static int EuroToInt(String sEuro, EURO_CONVERSION iErrThrow, TErrMsg ErrMsg)
        {
            bool bErr = false;
            int p_pos;          // posizione del punto
            int iPrz;
            String sEuroInt;    // parte intera della stringa in Euro
            String sEuroDec;  // parte decimale della stringa in Euro

            ErrMsg.iErrID = ERR_ECE;

            sEuro = sEuro.Trim();
            if (sEuro.Contains(" "))
            {
                bErr = true;
                // ricerca spazi intermedi tra le cifre
                if (iErrThrow == EURO_CONVERSION.EUROCONV_ERROR || (iErrThrow == EURO_CONVERSION.EUROCONV_Z_ERROR))
                    ErrorManager(ErrMsg);
                else if (iErrThrow == EURO_CONVERSION.EUROCONV_WARN || (iErrThrow == EURO_CONVERSION.EUROCONV_Z_WARN))
                    WarningManager(WRN_ECE); // ricerca spazi intermedi tra le cifre
                else
                    return -1;
            }

            try
            {
                p_pos = sEuro.IndexOf('.');    // ricerca punto

                if (p_pos == -1)
                    p_pos = sEuro.IndexOf(',');    // ricerca virgola

                if (p_pos != -1) // è presente il separatore decimale
                {
                    // separazione parte Intera da quella Decimale
                    sEuroInt = sEuro.Substring(0, p_pos);
                    sEuroDec = sEuro;
                    sEuroDec = sEuroDec.Remove(0, p_pos + 1);
                }
                else     // non ci sono decimali
                {
                    sEuroInt = sEuro;
                    sEuroDec = "0";
                }

                // caso di parte Intera con stringa nulla
                if (String.IsNullOrEmpty(sEuroInt))
                    sEuroInt = "0";

                // solleva eccezione se il prezzo è negativo
                if (sEuro.Contains("-"))
                {
                    bErr = true;
                    if (iErrThrow == EURO_CONVERSION.EUROCONV_ERROR || (iErrThrow == EURO_CONVERSION.EUROCONV_Z_ERROR))
                        ErrorManager(ErrMsg);
                    else if (iErrThrow == EURO_CONVERSION.EUROCONV_WARN || (iErrThrow == EURO_CONVERSION.EUROCONV_Z_WARN))
                        WarningManager(WRN_ECE);
                    else
                        return -1;
                }

                // verifiche sulla parte Decimale
                if (String.IsNullOrEmpty(sEuroDec))
                    sEuroDec = "0";
                else if (sEuroDec.Length == 1)
                    sEuroDec += "0";
                else if (sEuroDec.Length > 2)
                {
                    bErr = true;
                    if (iErrThrow == EURO_CONVERSION.EUROCONV_ERROR || (iErrThrow == EURO_CONVERSION.EUROCONV_Z_ERROR))
                        ErrorManager(ErrMsg);
                    else if (iErrThrow == EURO_CONVERSION.EUROCONV_WARN || (iErrThrow == EURO_CONVERSION.EUROCONV_Z_WARN))
                        WarningManager(WRN_ECE);
                    else
                        return -1;
                }

                // .ToInt() può sollevare l'eccezione EConvertError
                if (!bErr)
                    iPrz = Convert.ToInt32(sEuroInt) * 100 + Convert.ToInt32(sEuroDec);
                else
                    iPrz = -1;

            }

            catch (Exception)
            {
                // Errore di conversione
                ErrMsg.iErrID = ERR_CNV;
                ErrMsg.sNomeFile = NOME_FILE_LISTINO;

                if (iErrThrow != EURO_CONVERSION.EUROCONV_DONT_CARE)
                    ErrorManager(ErrMsg);

                return -1;
            }

            // solleva eccezione se il prezzo è nullo. accetta però EUROCONV_DONT_CARE, EUROCONV_Z_WARN, EUROCONV_Z_ERROR
            if (iPrz == 0)
            {
                if (iErrThrow == EURO_CONVERSION.EUROCONV_ERROR)
                    ErrorManager(ErrMsg);
                else if (iErrThrow == EURO_CONVERSION.EUROCONV_WARN)
                    WarningManager(WRN_PRZ);
                else if ((iErrThrow == EURO_CONVERSION.EUROCONV_Z_WARN) || (iErrThrow == EURO_CONVERSION.EUROCONV_Z_ERROR) ||
                         (iErrThrow == EURO_CONVERSION.EUROCONV_DONT_CARE))
                    return 0;
                else
                    return -1;
            }

            return iPrz;
        }

#if ! STAND_ORDINI

        /// <summary>inizializzazione delle stringhe di formattazione</summary>
        public static void InitFormatStrings(bool bChars33Param)
        {
            iMAX_RECEIPT_CHARS = bChars33Param ? MAX_ABS_RECEIPT_CHARS : MAX_LEG_RECEIPT_CHARS;
            iMAX_ART_CHAR = bChars33Param ? MAX_ABS_ART_CHAR : MAX_LEG_ART_CHAR;

            iCenterOrderNum = bChars33Param ? MAX_ABS_RECEIPT_CHARS - 10 : MAX_LEG_RECEIPT_CHARS - 4;

            sRCP_FMT_RCPT = bChars33Param ? _RCP_FMT_33_RCPT : _RCP_FMT_28_RCPT;
            sRCP_FMT_CPY = bChars33Param ? _RCP_FMT_33_CPY : _RCP_FMT_28_CPY;
            sRCP_FMT_DSC = bChars33Param ? _RCP_FMT_33_DSC : _RCP_FMT_28_DSC;
            sRCP_FMT_DIF = bChars33Param ? _RCP_FMT_33_DIF : _RCP_FMT_28_DIF;
            sRCP_FMT_TOT = bChars33Param ? _RCP_FMT_33_TOT : _RCP_FMT_28_TOT;
            sRCP_FMT_DSH = bChars33Param ? _RCP_FMT_33_DSH : _RCP_FMT_28_DSH;
            sRCP_FMT_NOTE = bChars33Param ? _RCP_FMT_33_NOTE : _RCP_FMT_28_NOTE;

            sDAT_FMT_PRL = bChars33Param ? _DAT_FMT_33_PRL : _DAT_FMT_28_PRL;
            sDAT_FMT_DAT = bChars33Param ? _DAT_FMT_33_DAT : _DAT_FMT_28_DAT;
            sDAT_FMT_TOT = bChars33Param ? _DAT_FMT_33_TOT : _DAT_FMT_28_TOT;
            sDAT_FMT_DSH = bChars33Param ? _DAT_FMT_33_DSH : _DAT_FMT_28_DSH;
            sDAT_FMT_HED = bChars33Param ? _DAT_FMT_33_HED : _DAT_FMT_28_HED;

            sDAT_FMT_REP_RED = bChars33Param ? _DAT_FMT_33_REP_RED : _DAT_FMT_28_REP_RED;
            sDAT_FMT_DSH_RED = bChars33Param ? _DAT_FMT_33_DSH_RED : _DAT_FMT_28_DSH_RED;
            sDAT_FMT_TOT_RED = bChars33Param ? _DAT_FMT_33_TOT_RED : _DAT_FMT_28_TOT_RED;

#if STANDFACILE
            // formattazione della griglia
            sGRD_FMT_STD = bChars33Param ? _GRD_FMT_33_STD : _GRD_FMT_28_STD;
            sGRD_FMT_TCH = bChars33Param ? _GRD_FMT_33_TCH : _GRD_FMT_28_TCH;

            sGRDW_FMT_STD = bChars33Param ? _GRDW_FMT_33_STD : _GRDW_FMT_28_STD;
            sGRDZ_FMT_TCH = bChars33Param ? _GRDZ_FMT_33_TCH : _GRDZ_FMT_28_TCH;
            sGRDW_FMT_TCH = bChars33Param ? _GRDW_FMT_33_TCH : _GRDW_FMT_28_TCH;
#endif
        }

#endif

        /// <summary>funzione per l'arrotondamento ai 10c,<br/>
        /// agisce su valori * 100, sempio 10.08 è rappresentato come 1008
        /// </summary>
        public static int Arrotonda(double fParam)
        {
            int iRounded;

            iRounded = (int)Math.Round(fParam / 10.0) * 10;
            return iRounded;
        }

        /// <summary>
        /// crittografia per codifica password utente database
        /// https://www.codeproject.com/Articles/14150/Encrypt-and-Decrypt-Data-with-C
        /// </summary>
        public static string Encrypt(string toEncrypt)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(CIPHER_KEY));

            hashmd5.Clear();

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider()
            { //set the secret key for the tripleDES algorithm
                Key = keyArray,

                //mode of operation. there are other 4 modes.
                //We choose ECB(Electronic code Book)
                Mode = CipherMode.ECB,

                //padding mode(if any extra byte added)
                Padding = PaddingMode.PKCS7
            };

            ICryptoTransform cTransform = tdes.CreateEncryptor();

            //transform the specified region of bytes array to resultArray
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            tdes.Clear();

            //Return the encrypted data into unreadable string format
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        /// decripta la stringa passata
        /// </summary>
        public static string Decrypt(string cipherString)
        {
            byte[] keyArray, toEncryptArray;

            try
            {
                toEncryptArray = Convert.FromBase64String(cipherString);
            }
            catch (Exception)
            {
                return "";
            }

            //if hashing was used get the hash code with regards to your key
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(CIPHER_KEY));

            hashmd5.Clear();

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider()
            {
                //set the secret key for the tripleDES algorithm
                Key = keyArray,

                //mode of operation. there are other 4 modes. 
                //We choose ECB(Electronic code Book)
                Mode = CipherMode.ECB,

                //padding mode(if any extra byte added)
                Padding = PaddingMode.PKCS7
            };

            ICryptoTransform cTransform = tdes.CreateDecryptor();

            byte[] resultArray = { 0 };

            try
            {
                resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            }
            catch (Exception)
            { }

            tdes.Clear();

            //return the Clear decrypted TEXT
            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        /// <summary>
        /// funzione per la deep copy di oggetti <br/>
        /// per le Struct in teoria non serve, nel dubbio usare .GetType().IsValueType<br/>
        /// https://stackoverflow.com/questions/129389/how-do-you-do-a-deep-copy-of-an-object-in-net
        /// </summary>
        static public void DeepCopy2<T>(ref T object2Copy, ref T objectCopy)
        {
            using (var stream = new MemoryStream())
            {
                var serializer = new XmlSerializer(typeof(T));

                serializer.Serialize(stream, object2Copy);
                stream.Position = 0;
                objectCopy = (T)serializer.Deserialize(stream);
            }
        }

        /// <summary>
        /// funzione per la deep copy di oggetti <br/>
        /// per le Struct in teoria non serve, nel dubbio usare .GetType().IsValueType
        /// </summary>
        static public T DeepCopy<T>(T obj)
        {
            BinaryFormatter s = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                s.Serialize(ms, obj);
                ms.Position = 0;
                T t = (T)s.Deserialize(ms);

                return t;
            }
        }

        /// <summary>funzione per ricerca di una Stringa dentro alla Struct ORDER_CONST<br/>
        /// il primo parametro è per la stringa da cercare, il secondo per eventuali esclusioni</summary>
        static public bool StringBelongsTo_ORDER_CONST(String sStrParam, String sEsclusioneParam = SHMAGIC)
        {
            foreach (String sItem in ORDER_CONST.sArray)
            {
                if (sItem == sEsclusioneParam)
                    continue;

                if (sItem == sStrParam)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// ritorna il colore foregroung e background per i gruppi di stampa
        /// </summary>
        static public Color[] GetColor(int iParam)
        {
            // [0] per il BackColor, [1] per il ForeColor
            Color[] retColor = new Color[2];

            switch (iParam)
            {
                case 1:
                    retColor[0] = Color.LimeGreen;
                    retColor[1] = Color.Black;
                    break;
                case 2:
                    retColor[0] = Color.Blue;
                    retColor[1] = Color.White;
                    break;
                case 3:
                    retColor[0] = Color.Yellow;
                    retColor[1] = Color.Black;
                    break;
                case 4:
                    retColor[0] = Color.Red;
                    retColor[1] = Color.White;
                    break;
                case 0:
                default:
                    retColor[0] = SystemColors.ControlLight;
                    retColor[1] = SystemColors.ControlText;
                    break;
            }

            return retColor;
        }

        /// <summary>ottiene un intero con il bit in posizione bitPosParam che vale ad 1<br/>
        /// bitPosParam is 0 based
        /// </summary>
        public static int SetBit(int intParam, int bitPosParam)
        {
            return intParam |= (1 << bitPosParam);
        }

        /// <summary>ottiene un intero con il bit in posizione bitPosParam che vale 0<br/>
        /// bitPosParam is 0 based
        /// </summary>
        public static int ClearBit(int intParam, int bitPosParam)
        {
            return intParam & ~(1 << bitPosParam);
        }

        /// <summary>verifica se il bit in posizione bitPosParam vale 1<br/>
        /// bitPosParam is 0 based
        /// </summary>
        public static bool IsBitSet(int intParam, int bitPosParam)
        {
            if ((bitPosParam < 0) || (bitPosParam >= 32))
                return false;
            else
                return (intParam & (1 << bitPosParam)) != 0;
        }

        // da evitare verifiche di 2 tipi: positive e negative
        // <summary>verifica se il bit in posizione bitPosParam vale 0</summary>
        // public static bool IsBitClear(int intParam, int bitPosParam)
        // {
        //    return !IsBitSet(intParam, bitPosParam);
        // }

    } // end class
} // end namespace

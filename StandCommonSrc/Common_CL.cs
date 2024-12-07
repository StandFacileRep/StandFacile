/*****************************************************
 	NomeFile : StandCommonSrc/CommonFunc.cs
    Data	 : 25.09.2024
 	Autore	 : Mauro Artuso

	Classi statiche di uso comune
 *****************************************************/

using System;
using System.IO;
using System.Text;
using Microsoft.Win32;
using System.Windows.Forms;
using System.Configuration;
using System.Drawing;
using System.Xml.Serialization;

using System.Security.Cryptography;
using System.Runtime.Serialization.Formatters.Binary;

using static StandCommonFiles.ComDef;
using static StandCommonFiles.ComSafe;
using static StandCommonFiles.LogServer;

using StandFacile;
using static StandFacile.glb;
using static StandFacile.Define;

#if STANDFACILE
using static StandFacile.FrmMain;
#endif

#if STAND_CUCINA || STAND_ORDINI || STAND_MONITOR
using static StandFacile.MessageDlg;
#endif

namespace StandCommonFiles
{

    /// <summary>
    /// definizione dei codici di errore e di warning
    /// </summary>
    public static class commonCl
    {
        /// <summary>Errore generico</summary>
        public const int ERR_GEN = 50;

        /// <summary>Errore di conversione</summary>
        public const int ERR_CNV = 100;
        /// <summary> File Not Found</summary>
        public const int ERR_FNF = 110;
        /// <summary>Numero Eccessivo di Articoli</summary>
        public const int ERR_NVE = 120;
        /// <summary>Semicolon ; Not Found</summary>
        public const int ERR_SNF = 130;
        /// <summary>Stringa Troppo Lunga</summary>
        public const int ERR_STL = 140;
        /// <summary>Errore di conversione della cifra in Euro</summary>
        public const int ERR_ECE = 150;
        /// <summary>Tipo di Articolo Vuoto con Prezzo non nullo</summary>
        public const int ERR_PVP = 160;
        /// <summary> Errore nella destinazione della stampa</summary>
        public const int ERR_EDS = 170;
        /// <summary> Voci Ripetute</summary>
        public const int ERR_VRP = 180;
        /// <summary>Directory Non Aperta</summary>
        public const int ERR_DNA = 190;
        /// <summary>File Not Opened</summary>
        public const int ERR_FNO = 200;
        /// <summary> Ethernet Communication Error</summary>
        public const int ERR_ETH = 210;
        /// <summary>File non Rinominabile</summary>
        public const int ERR_FNR = 220;
        /// <summary>Problemi di scrittura nel Registry</summary>
        public const int ERR_RGT = 230;
        /// <summary>Numero scontrini non trovato</summary>
        public const int ERR_NSN = 240;

        /// <summary>Numero messaggi non trovato</summary>
        public const int ERR_NMN = 250;
        /// <summary>Azzeramento Dati</summary>
        public const int ERR_AZD = 260;
        /// <summary>Importati i Prezzi</summary>
        public const int ERR_AZP = 270;
        /// <summary>Cambio Database</summary>
        public const int ERR_CDB = 280;
        /// <summary>Chiusura cassa</summary>
        public const int ERR_CHC = 290;
        /// <summary>mancano dll</summary>
        public const int ERR_DLL = 300;

        /// <summary>File Not Found</summary>
        public const int WRN_FNF = 400;
        /// <summary>File Not Found</summary>
        public const int WRN_CNF = 410;
        /// <summary>File Not Opened</summary>
        public const int WRN_FNO = 420;
        /// <summary>Tabella non presente</summary>
        public const int WRN_TNP = 430;
        /// <summary>Errore di conversione della cifra in Euro</summary>
        public const int WRN_ECE = 440;
        /// <summary> Log File Error</summary>
        public const int WRN_LFE = 450;
        /// <summary>Match Not Found</summary>
        public const int WRN_MNF = 460;
        /// <summary>Errore nel Tipo di Articolo Vuoto</summary>
        public const int WRN_TPV = 470;
        /// <summary>Errore Prezzo nullo</summary>
        public const int WRN_PRZ = 480;

        /// <summary>Errore Tipo nullo</summary>
        public const int WRN_TPN = 490;

        /// <summary>Voce duplicata</summary>
        public const int WRN_TVD = 500;
        /// <summary>Ethernet Communication Error, usata da StandClient</summary>
        public const int WRN_ETH = 510;
        /// <summary>COM non aperta</summary>
        public const int WRN_CNA = 520;
        /// <summary>Errore di comunicazione con il Server</summary>
        public const int WRN_SRV = 530;
        /// <summary>Errore di comunicazione con il Client</summary>
        public const int WRN_CLT = 540;
        /// <summary>Stampa Scontrino fallita</summary>
        public const int WRN_STF = 550;
        /// <summary>Test di Stampa fallito</summary>
        public const int WRN_TSF = 560;
        /// <summary>Quantità Articoli maggiore della disponibilità</summary>
        public const int WRN_QMD = 570;
        /// <summary>Modifica non possibile !</summary>
        public const int WRN_MNP = 590;
        /// <summary>Test di comunicazione con il NumSc server eseguito con successo !</summary>
        public const int WRN_TTS = 600;
        /// <summary>Test di comunicazione con il Server Cucina eseguito con successo !</summary>
        public const int WRN_TSS = 610;
        /// <summary>Manca il nome del Server, usata da StandCucina</summary>
        public const int WRN_MNT = 620;
        /// <summary>Checksum loc non corretto file Prezzi</summary>
        public const int WRN_CKPL = 630;
        /// <summary>Checksum web non corretto file Prezzi</summary>
        public const int WRN_CKPW = 635;
        /// <summary>Checksum mancante file Prezzi</summary>
        public const int WRN_CKD = 640;
        /// <summary>Stampa dati non possibile</summary>
        public const int WRN_SDN = 650;

        /// <summary>Receipt con totale a zero</summary>
        public const int WRN_TZT = 660;
        /// <summary>Ricevuto Xoff</summary>
        public const int WRN_XOF = 670;
        /// <summary>Ricevuta Stringa</summary>
        public const int WRN_RXS = 680;
        /// <summary>Errore gestione coda</summary>
        public const int WRN_QEX = 690;
        /// <summary>Errore conversione quantità</summary>
        public const int WRN_ECQ = 700;
        /// <summary>Quantità nulla</summary>
        public const int WRN_CQZ = 710;
        /// <summary>Test di comunicazione con il DB server eseguito con successo !</summary>
        public const int WRN_TDS = 720;
        /// <summary>Test di comunicazione con il DB server eseguito con successo, ma non c'è la tabella !</summary>
        public const int WRN_TDQ = 730;
        /// <summary>Test di comunicazione con DB server fallito !</summary>
        public const int WRN_TDF = 740;
        /// <summary>Problema generico con DB !</summary>
        public const int WRN_TDG = 750;
        /// <summary>Test di apertura file SQLite fallito !</summary>
        public const int WRN_TCF = 760;
        /// <summary>Record trovato ma annullato!</summary>
        public const int WRN_RAN = 770;
        /// <summary>gruppo di stampa non è accettato</summary>
        public const int WRN_GNA = 775;
        /// <summary>Record non trovato !</summary>
        public const int WRN_RNF = 780;
        /// <summary>Record di Prevendita già scaricato !</summary>
        public const int WRN_RPS = 785;
        /// <summary>Database error !</summary>
        public const int WRN_DBE = 790;
        /// <summary>Annullo non eseguito !</summary>
        public const int WRN_SNE = 800;
        /// <summary>Annullo eseguito !</summary>
        public const int WRN_SEX = 810;
        /// <summary>Excel non installato !</summary>
        public const int WRN_EXN = 820;
        /// <summary>Ordini non presenti !</summary>
        public const int WRN_DNP = 825;

        /// <summary>QR code error !</summary>
        public const int WRN_QRE = 830; // * 
        /// <summary>test QR code !</summary>
        public const int WRN_TQR = 835; // *
        /// <summary>REMOTE Database error !</summary>
        public const int WRN_DBR = 840; // *
        /// <summary>Stabilita connessione con il WEB server !</summary>
        public const int WRN_WSCS = 850; // *
        /// <summary>Tabella Listino non trovata!</summary>
        public const int WRN_WPNF = 860; // *
        /// <summary>Tabella Log non trovata!</summary>
        public const int WRN_WLNF = 865; // *
        /// <summary>Test di comunicazione con il WEB server eseguito con successo !</summary>
        public const int WRN_WSTS = 870; // *
        /// <summary>Test di comunicazione con il WEB server fallito !</summary>
        public const int WRN_WSTF = 880; // *
        /// <summary>Checksum listino non corrispondente nell'ordine web</summary>
        public const int WRN_CKWO = 890;

        /// <summary>Database Listino error !</summary>
        public const int WRN_DBL = 900;

        /// <summary>Database Test error !</summary>
        public const int WRN_DBT = 910;

        /// <summary>File Prezzi Ignorato !</summary>
        public const int WRN_FPI = 920;
        /// <summary>Token presenti</summary>
        public const int WRN_TKP = 930;
        /// <summary>Date non allineate</summary>
        public const int WRN_DNA = 940;
        /// <summary>Numero eccessivo di voci nel DB</summary>
        public const int WRN_NVD = 950;
        /// <summary>File aperto: chiuderlo</summary>
        public const int WRN_FOC = 960;
        /// <summary>Numero Receipt not found in file</summary>
        public const int WRN_TNFF = 970;
        /// <summary>Numero Receipt not found</summary>
        public const int WRN_TNNF = 975;
        /// <summary>Dimensioni del Logo errate</summary>
        public const int WRN_DLE = 980;
        /// <summary>Errore del Driver di stampa</summary>
        public const int WRN_PDE = 990;

        /// <summary>Errore lunghezza del testo gruppi</summary>
        public const int WRN_LTE = 995;

        /// <summary>Test Item Match Not Found</summary>
        public const int WRN_TIANF = 1000;

        /// <summary>Pricelist Upload success</summary>
        public const int WRN_PUPS = 1010;

        /// <summary>Stringa Troppo Lunga</summary>
        public const int WRN_STL = 1020;

        /// <summary>Richiesta aggiunta commento con quantità Zero</summary>
        public const int WRN_NQZ = 1030;

        /// <summary>data all'avvio del programma</summary>
        static DateTime actualDate;

        /// <summary>indica se c'è già una istanza del Programma in esecuzione</summary>
        public static bool bApplicationRuns;

        static int iRefresh;

        /// <summary>struct per gestione configurazione da File</summary>
        public static TConfig sConfig = new TConfig();

        /// <summary>verifica se la stringa passata è contenuta nella chiave "serviceStrings"</summary>
        public static bool bCheckService(String sString)
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
        public static int iReadRegistry(String sArray, int iDef = 0)
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
        public static String sReadRegistry(String sParam, String sDefStr = "")
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
        public static DateTime getActualDate()
        {
            return actualDate;
        }

        /// <summary>imposta la data attuale</summary>
        public static void setActualDate(DateTime dateParam)
        {
            actualDate = dateParam;
        }

        /// <summary>
        /// impostazione della data: è necessario effettuarla prima di lanciare la form di avvio,
        /// il giorno corrente finisce alle ore 05.00
        /// </summary>
        public static void initActualDate(int iTestHourPrm = -1, int iTestMinPrm = -1)
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
        /// Funzione di gestione dei warning,
        /// ogni warning ha un suo codice univoco iWrnID
        /// </summary>
        public static void WarningManager(int iWrnID)
        {
            TErrMsg WrnMsg = new TErrMsg();

            WrnMsg.sMsg = "";
            WrnMsg.iErrID = iWrnID;

            WarningManager(WrnMsg);
        }

        /// <summary>
        /// funzione di generazione avviso all'utente
        /// </summary>
        public static void WarningManager(TErrMsg WrnMsg)
        {
            bool bModal = false;
            String sCaptionBuf, sWrnStr, sLogStr;

            switch (WrnMsg.iErrID)
            {
                case WRN_FNF: // File Not Found
                    sWrnStr = "File : " + WrnMsg.sNomeFile + "  non trovato !";
                    break;

                case WRN_CNF: // Command Not Found
                    sWrnStr = "Comando non trovato nel File : \n\n " + WrnMsg.sNomeFile + "\n\nalla riga: " + WrnMsg.iRiga + " !";
                    break;

                case WRN_FNO: // File Not Opened
                    sWrnStr = "File : " + WrnMsg.sNomeFile + "  non apribile !";
                    break;

                case WRN_TNP: // Tabella vuota
                    sWrnStr = "Tabella : " + WrnMsg.sMsg + "  vuota !";
                    break;

                case WRN_CNA: // COM non aperta
                    sWrnStr = "Impossibile stampare su " + WrnMsg.sMsg + " !\n";
                    break;

                case WRN_ECE: // Errore di conversione nel dialogo PrzDlg
                    sWrnStr = "Errore di conversione del Prezzo in Euro";
                    break;

                case WRN_PRZ: // Errore di conversione nel dialogo PrzDlg
                    sWrnStr = "Prezzo pari a zero !";
                    break;

                case WRN_TPN: // Tipo Articolo vuoto con prezzo non nullo
                    sWrnStr = "Manca il Tipo di Articolo nel File :\n\n " + WrnMsg.sNomeFile + "\n\n alla riga: " + WrnMsg.iRiga + " !";
                    break;

                case WRN_LFE:
                    sWrnStr = "Errore nell'apertura del LogFile !";
                    break;

                case WRN_TPV:
                    sWrnStr = "Inserire il Nome dell' Articolo e/o il Prezzo!";
                    break;

                case WRN_TVD:
                    sWrnStr = "Tipo di Articolo già esistente!";
                    break;

                case WRN_SRV:
                    sWrnStr = " Attenzione !\r\n\r\nComunicazione con il server Numero Scontrini : \'" + WrnMsg.sMsg + "\' fallita !";
                    break;

                case WRN_CLT:
                    sWrnStr = "Attenzione !\nComunicazione con il client fallita";
                    break;

                case WRN_STF:
                    sWrnStr = "Stampa dello Scontrino fallita :\n\n";
                    sWrnStr += "impossibile stampare sulla " + WrnMsg.sMsg + " !\n";
                    break;

                case WRN_TSF:
                    sWrnStr = "Attenzione : Test di Stampa fallito !";
                    break;

                case WRN_MNP:
                    sWrnStr = "Attenzione !\nModifica non possibile!";
                    break;

                case WRN_TTS:
                    sWrnStr = "Test di comunicazione con il server Numero Scontrini : '" + WrnMsg.sMsg + "'\neseguito con successo !";
                    break;

                case WRN_TSS:
                    sWrnStr = "Test di comunicazione con il Server delle copie in Cucina : '" + WrnMsg.sMsg + "'\neseguito con successo !";
                    break;

                case WRN_ETH:
                    sWrnStr = "Attenzione !\nConnessione Ethernet non possibile !";
                    break;

                case WRN_MNT:
                    sWrnStr = "Attenzione !\nManca il nome del  TCP/IP Server!";
                    break;

                case WRN_MNF: // Match Not Found
                    sWrnStr = "\nArticolo:\n\n " + WrnMsg.sMsg + " \n\nnon trovato nel database!";
                    break;

                case WRN_QMD:
                    sWrnStr = "Attenzione !\nQuantità " + WrnMsg.sMsg + "\nmaggiore della disponibilità !\r\n\r\nCorreggere!";
                    break;

                case WRN_CKPL:
                    sWrnStr = " Attenzione !\r\n\r\nChecksum locale Errato nel file : " + WrnMsg.sNomeFile + "\r\n\r\n" +
                                "Se il file è stato modificato a mano clicca su OK e prosegui pure,\n" + "altrimenti il file è corrotto!";
                    break;

                case WRN_CKPW:
                    sWrnStr = " Attenzione !\r\n\r\nChecksum web Errato nel file : " + WrnMsg.sNomeFile + "\r\n\r\n" +
                                "Se il file è stato modificato a mano clicca su OK e prosegui pure,\n" + "altrimenti il file è corrotto!";
                    break;

                case WRN_CKD:
                    sWrnStr = " Attenzione !\r\n\r\nChecksum mancante nel file : \r\n\r\n" + WrnMsg.sNomeFile +
                            "\r\n\r\n Se il file è stato modificato a mano clicca su OK e prosegui pure,\n" +
                            "altrimenti è meglio uscire dal Programma !";
                    break;

                case WRN_SDN:
                    sWrnStr = "Attenzione !\nStampa dei dati su carta stretta!";
                    break;

                case WRN_TZT:
                    sWrnStr = "Attenzione !\n\nScontrino con importo zero!";
                    break;

                case WRN_XOF:
                    sWrnStr = "Attenzione !\nRicevuto timeout Xoff, stampa da ripetere!";
                    break;

                case WRN_RXS:
                    sWrnStr = "Attenzione !\nRicevuta Stringa !";
                    break;

                case WRN_QEX:
                    sWrnStr = "Attenzione !\nErrore Coda : " + WrnMsg.sMsg + " !\n";
                    break;

                case WRN_ECQ:
                    sWrnStr = "Errore di conversione della quantità !";
                    break;

                case WRN_CQZ:
                    sWrnStr = "Inserire la quantità !";
                    break;

                case WRN_TDS:
                    sWrnStr = "Test di comunicazione con il DB server " + WrnMsg.sMsg + "\r\n\r\neseguito con successo !";
                    break;

                case WRN_TDQ:
                    sWrnStr = " Attenzione !\r\n\r\nManca collegamento con la tabella : '" + WrnMsg.sMsg + "' !\n";
                    break;

                case WRN_TDF:
                    sWrnStr = " Attenzione !\r\n\r\nTest di comunicazione con il DB server : '" + WrnMsg.sMsg + "' fallito !";
                    break;

                case WRN_TCF:
                    sWrnStr = " Attenzione !\r\n\r\nTest di apertura DataBase SQLite fallito !";
                    break;

                case WRN_RNF:
                    sWrnStr = " Attenzione !\r\n\r\nRecord " + WrnMsg.sMsg + " non trovato nel database !";
                    break;

                case WRN_RPS:
                    sWrnStr = " Attenzione !\r\n\r\nOrdine " + WrnMsg.sMsg + " già scaricato !";
                    break;

                case WRN_RAN:
                    sWrnStr = " Attenzione !\r\n\r\nl\'Ordine " + WrnMsg.sMsg + " è stato annullato !";
                    break;

                case WRN_GNA:
                    sWrnStr = " Attenzione !\r\n\r\n il Gruppo di Articoli contenuto nel barcode: " + WrnMsg.sMsg + "\r\n\r\n non è tra quelli accettati !";
                    break;

                case WRN_DBE:
                    sWrnStr = " Attenzione !\r\n\r\nErrore Database " + WrnMsg.sMsg + " !\n";
                    break;

                case WRN_SNE:
                    sWrnStr = " Attenzione !\r\n\r\nScontrino " + WrnMsg.sMsg + " già annullato o emesso da altra cassa!";
                    break;

                case WRN_SEX:
                    sWrnStr = " Attenzione !\r\n\r\nScontrino " + WrnMsg.sMsg + " annullato correttamente!";
                    break;

                case WRN_EXN:
                    sWrnStr = " Attenzione !\r\n\r\n file in uso o Excel potrebbe non essere installato su questo PC !";
                    break;

                case WRN_DNP:
                    sWrnStr = " Attenzione !\r\n\r\n Nessun ordine emesso,\r\n quindi la funzione non è disponibile !";
                    break;

                case WRN_DBR:
                    sWrnStr = String.Format("{0} : {1} !\n", "Attenzione!\n\nErrore Database remoto", WrnMsg.sMsg);
                    break;

                case WRN_WSCS:
                    bModal = true;
                    sWrnStr = String.Format("{0}\n\n{1}", "Stabilita connessione con il Web Server:", WrnMsg.sMsg);
                    break;

                case WRN_WPNF:
                    sWrnStr = String.Format("Listino prezzi non trovato su web server:\n\n {0} !", WrnMsg.sMsg);
                    break;

                case WRN_WLNF:
                    sWrnStr = String.Format("Log table non trovata su web server:\n\n {0} !", WrnMsg.sMsg);
                    break;

                case WRN_WSTS:
                    sWrnStr = String.Format("{0}\n\n  {1}\n\n{2}", "Test di comunicazione con il DataBase remoto", WrnMsg.sMsg, "eseguito con successo !");
                    break;

                case WRN_WSTF:
                    sWrnStr = String.Format("{0}\n\n  {1}\n\n{2}", "Test di comunicazione con il DataBase remoto", WrnMsg.sMsg, "fallito !");
                    break;

                case WRN_CKWO:
                    sWrnStr = " Attenzione !\r\n\r\nChecksum non corrispondente al listino nell'ordine: " + WrnMsg.sMsg + "\r\n\r\n" +
                              "L'ordine non viene accettato!";
                    break;

                case WRN_FOC:
                    sWrnStr = " Attenzione !\r\n\r\nChiudere il file " + WrnMsg.sMsg + " e riprovare !";
                    break;

                case WRN_DBL:
                    sWrnStr = " Attenzione !\r\n\r\nErrore caricamento Listino dal Database !\n";
                    break;

                case WRN_DBT:
                    sWrnStr = " Attenzione !\r\n\r\nErrore caricamento Test dal Database !\n";
                    break;

                case WRN_FPI:
                    sWrnStr = " Attenzione !\r\n\r\nQuesta è una Cassa Secondaria che usa il DataBase: il file Prezzi locale verrà ignorato." +
                            "\r\n\r\nListino, Disponibilità, impostazioni Griglia, Gruppi di stampa, verranno caricati dalla Cassa Principale !" +
                            "\r\n\r\nAvviare per prima la Cassa Principale e successivamente le Secondarie !";
                    break;

                case WRN_TKP: // Token presenti
                    sWrnStr = "Sono presenti caratteri  \'# ;\'  non ammessi, correggere !";
                    break;

                case WRN_NVD: // Numero Voci Eccessivo
                    sWrnStr = "Numero eccessivo di voci nel DB : " + WrnMsg.sMsg + " !";
                    break;

                case WRN_DNA:
                    sWrnStr = " Attenzione !\r\n\r\nDate tra le casse non allineate :\r\n\r\n" + WrnMsg.sMsg;
                    break;

                case WRN_TNFF:
                    sWrnStr = " Attenzione !\r\n\r\nNumero scontrino non trovato in: " + WrnMsg.sNomeFile + " !";
                    break;

                case WRN_TNNF:
                    sWrnStr = " Attenzione !\r\n\r\nNumero scontrino: " + WrnMsg.sMsg + " non trovato!";
                    break;

                case WRN_DLE:
                    sWrnStr = " Attenzione !\r\n\r\nDimensioni del Logo errate !";
                    break;

                case WRN_PDE:
                    sWrnStr = " Attenzione !\r\n\r\n Printer Driver error: " + WrnMsg.sMsg + " !";
                    break;

                case WRN_QRE:
                    sWrnStr = String.Format("{0} : {1} !\n", "Attenzione!\n\nErrore lettura QRcode", WrnMsg.sMsg);
                    break;

                case WRN_TQR:
                    sWrnStr = String.Format("{0} {1} !\n", "Complimenti!\n\nIl QRcode di prova è corretto:\n", WrnMsg.sMsg);
                    break;

#if STANDFACILE
                case WRN_LTE:
                    sWrnStr = " Attenzione !\r\n\r\n Errore lunghezza del testo errore: " + WrnMsg.sMsg + "\r\n\r\ndeve essere almeno" + MIN_COPIES_CHARS + " !";
                    break;
#endif
                case WRN_TIANF: // Test Item Match Not Found
                    sWrnStr = String.Format("Test sequence parsing:\n\n {0} \'{1}\'", "Test Item Match Not Found", WrnMsg.sMsg);
                    break;

                case WRN_PUPS:
                    bModal = true;
                    sWrnStr = String.Format("Upload del Listino prezzi su web server:\r\n\r\n {0} eseguito con successo !", WrnMsg.sMsg);
                    break;

                case WRN_STL: // Stringa Troppo Lunga
                    if (WrnMsg.iRiga >= 0)
                        sWrnStr = "Stringa troppo lunga nel File:   " + WrnMsg.sNomeFile + "\r\n\r\n alla riga: " + WrnMsg.iRiga + ",   Articolo: " + WrnMsg.sMsg +
                                "\r\n\r\ncorreggere per visualizzazione e stampa corretta degli Articoli,\r\npoi riavviare !";
                    else
                        sWrnStr = "Stringa troppo lunga per l'Articolo:\r\n\r\n   " + WrnMsg.sMsg + "\r\n\r\ncorreggere per visualizzazione e stampa corretta !";
                    break;

                case WRN_NQZ:
                    bModal = true;
                    sWrnStr = " Attenzione !\r\n\r\n per inserire una nota la quantità Articolo deve essere maggiore di zero !";
                    break;

                default:
                    sWrnStr = "Warning generico";
                    break;
            } // end switch

            sLogStr = String.Format("§W{0} {1}", WrnMsg.iErrID, sWrnStr);
            LogToFile(sLogStr);

            sCaptionBuf = String.Format("Avviso {0} : {1}", WrnMsg.iErrID, TITLE);

#if STAND_CUCINA || STAND_ORDINI || STAND_MONITOR

            if (rMessageDlg != null)
                rMessageDlg.MessageBox(Environment.NewLine + sWrnStr, sCaptionBuf, 10);
#else
            if (bModal)
                MessageBox.Show(sWrnStr, sCaptionBuf, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            else
                MessageBox.Show(sWrnStr, sCaptionBuf, MessageBoxButtons.OK);

#endif

        } // end WarningManager()

        /// <summary>
        /// funzione di generazione avviso all'utente a causa di un errore
        /// ogni errore ha un suo codice univoco : iErrID
        /// </summary>
        public static void ErrorManager(int iErrID)
        {
            TErrMsg ErrMsg = new TErrMsg();

            ErrMsg.sMsg = "";
            ErrMsg.iErrID = iErrID;

            ErrorManager(ErrMsg);
        }

        /// <summary>
        /// funzione di generazione avviso all'utente a causa di un errore
        /// </summary>
        public static void ErrorManager(TErrMsg ErrMsg)
        {
            bool bControlledExit = false;
            String sCaptionBuf, sErrStr, sLogStr;

            if (ErrMsg.iErrID == 0)
                return;

            switch (ErrMsg.iErrID)
            {
                case ERR_CNV: // Errore di conversione
                    sErrStr = "Errore di conversione nel File :\n\n " + ErrMsg.sNomeFile + "\n\n alla riga: " + ErrMsg.iRiga + " !";
                    break;
                case ERR_FNF: // File Not Found
                    sErrStr = "File : " + ErrMsg.sNomeFile + " \nnon trovato !";
                    break;

                case ERR_NVE: // Numero Voci Eccessivo
                    sErrStr = "Numero eccessivo di voci nel File :\n" + ErrMsg.sNomeFile + " !";
                    break;
                case ERR_SNF: // Semicolon ; Not Found
                    sErrStr = "Errore di formato nel File :\n\n " + ErrMsg.sNomeFile + "\n\n alla riga: " + ErrMsg.iRiga + " manca ';' !";
                    break;
                case ERR_STL: // Stringa Troppo Lunga
                    sErrStr = "Stringa troppo lunga nel File :\n\n " + ErrMsg.sNomeFile + "\n\n alla riga: " + ErrMsg.iRiga + " !";
                    break;
                case ERR_ECE: // Errore nella cifra in Euro
                    sErrStr = "Formato prezzo errato \n\nalla riga: " + ErrMsg.iRiga + " !";
                    break;
                case ERR_PVP: // Tipo Articolo vuoto con prezzo non nullo
                    sErrStr = "Manca il Tipo di Articolo nel File :\n\n " + ErrMsg.sNomeFile + "\n\n alla riga: " + ErrMsg.iRiga + " !";
                    break;
                case ERR_EDS: // Errore nella destinazione della stampa
                    sErrStr = "Destinazione errata della stampa nel File :\n\n " + ErrMsg.sNomeFile + "\n\n alla riga: " + ErrMsg.iRiga + " !";
                    break;
                case ERR_VRP: // ripetizione voci
                    sErrStr = "Voce " + ErrMsg.sMsg + " ripetuta nel file :\n\n " + ErrMsg.sNomeFile + " !";
                    break;
                case ERR_DNA: // Directory non aperta
                    sErrStr = "Directory " + ErrMsg.sMsg + " non apribile !";
                    break;
                case ERR_FNO: // File Not Opened
                    sErrStr = "File : " + ErrMsg.sNomeFile + "  non apribile !";
                    break;

                case ERR_ETH: // Ethernet Communication Error
                    sErrStr = "Ethernet communication error !";
                    break;

                case ERR_FNR: // File non Rinominabile
                    sErrStr = "Fallito Backup del file : " + ErrMsg.sNomeFile;
                    break;

                case ERR_RGT: // Problemi di scrittura nel Registry
                    sErrStr = "Impossibile salvare le impostazioni nel Registry !\n";
                    break;

                case ERR_NSN: // Problemi di scrittura nel Registry
                    sErrStr = "Numero scontrini non trovato !\n";
                    break;

                case ERR_NMN: // Problemi di scrittura nel Registry
                    sErrStr = "Numero messaggi non trovato !\n";
                    break;

#if STANDFACILE
                // ***** Avvisi e non errori *****
                case ERR_CDB:
                    sErrStr = "Cambio DataBase e/o Tipo di cassa!";
                    sLogStr = String.Format("#E{0} {1}", ErrMsg.iErrID, sErrStr);
                    LogToFile(sLogStr);
                    Directory.SetCurrentDirectory(DataManager.sGetExeDir());
                    System.Diagnostics.Process.Start(THE_APP);
                    bControlledExit = true;
                    break;

                case ERR_AZD:
                    sErrStr = "Dati di vendita azzerati !\n";
                    sLogStr = String.Format("#E{0} {1}", ErrMsg.iErrID, sErrStr);
                    LogToFile(sLogStr);

                    sCaptionBuf = String.Format("Avviso {0} : {1}", ErrMsg.iErrID, TITLE);
                    sErrStr += "\r\n\r\nIl programma viene terminato.";
                    MessageBox.Show(sErrStr, sCaptionBuf, MessageBoxButtons.OK);
                    bControlledExit = true;
                    break;

                case ERR_CHC:
                    sCaptionBuf = String.Format("Avviso {0} : {1}", ErrMsg.iErrID, TITLE);
                    sErrStr = "Chiusura cassa riuscita !\r\n\r\nIl programma viene terminato.";
                    MessageBox.Show(sErrStr, sCaptionBuf, MessageBoxButtons.OK);
                    bControlledExit = true;
                    break;

                case ERR_AZP:
                    sErrStr = "Importazione riuscita !\r\n\r\nIl programma viene riavviato.";
                    sLogStr = String.Format("#E{0} {1}", ErrMsg.iErrID, sErrStr);
                    LogToFile(sLogStr);

                    sCaptionBuf = String.Format("Avviso {0} : {1}", ErrMsg.iErrID, TITLE);
                    MessageBox.Show(sErrStr, sCaptionBuf, MessageBoxButtons.OK);
                    Directory.SetCurrentDirectory(DataManager.sGetExeDir());
                    System.Diagnostics.Process.Start(THE_APP);
                    bControlledExit = true;
                    break;
#endif

                case ERR_DLL: // Problemi di scrittura nel Registry
                    switch (iReadRegistry(DB_MODE_KEY, (int)DB_MODE.SQLITE))
                    {
                        case (int)DB_MODE.SQLITE:
                            if (Environment.Is64BitOperatingSystem)
                                sErrStr = String.Format("Manca il file {0} o {1} o {2}", DB_CONNECTOR_DLL_DEV, DB_CONNECTOR_DLL_QL, DB_CONNECTOR_DLL_QL64);
                            else
                                sErrStr = String.Format("Manca il file {0} o {1} o {2}", DB_CONNECTOR_DLL_DEV, DB_CONNECTOR_DLL_QL, DB_CONNECTOR_DLL_QL32);
                            break;
                        case (int)DB_MODE.MYSQL:
                            sErrStr = String.Format("Manca il file {0} o {1}", DB_CONNECTOR_DLL_DEV, DB_CONNECTOR_DLL_MY);
                            break;
                        case (int)DB_MODE.POSTGRES:
                            sErrStr = String.Format("Manca il file {0} o {1}", DB_CONNECTOR_DLL_DEV, DB_CONNECTOR_DLL_PG);
                            break;
                        default:
                            sErrStr = "nessun db in uso";
                            break;
                    }

                    break;

                default:
                    sErrStr = "Errore generico";
                    break;
            } // end switch

            sLogStr = String.Format("#E{0} {1}", ErrMsg.iErrID, sErrStr);
            LogToFile(sLogStr);

            if (!bControlledExit)
            {
                sCaptionBuf = String.Format("Errore {0} : {1}", ErrMsg.iErrID, TITLE);
                sErrStr += "\r\n\r\nIl programma viene terminato.";
                MessageBox.Show(sErrStr, sCaptionBuf, MessageBoxButtons.OK);

                if (bApplicationRuns)
                    Application.Exit();
                else
                    Environment.Exit(0);
            }
#if STANDFACILE
            else
            {
                rFrmMain.Close();
                Application.Exit();
            }
#endif

        } // end ErrorManager()

        /// <summary>
        /// Funzione per centrare le stringhe sulla larghezza della carta termica
        /// </summary>
        public static String sCenterJustify(String sText, int iWidth)
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
        public static String sCenterJustifyStars(String sText, int iWidth, char ch)
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
        public static uint uHash(String sData, int iParam = 0)
        {
            uint uCkecksum;

            MD5 md5 = MD5.Create();
            sData = sData.Trim();

            if (iParam > 0)
                sData = sData + iParam.ToString();

            byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(sData);
            byte[] hash = md5.ComputeHash(inputBytes);

            uCkecksum = BitConverter.ToUInt32(hash, 0);

            return uCkecksum;
        }

        /// <summary>overload funzione di calcolo checksum</summary>
        public static uint uHash(int iData)
        {
            String sData = iData.ToString();
            return uHash(sData);
        }

        /// <summary>costruzione file di test stampa</summary>
        public static String buildSampleText()
        {
            int i, j, ich;
            char[] cBuffer = new char[32];
            int iLMargin = 0;
            String sTmp;
            StreamWriter fData;
            String sDir = "";
            TErrMsg ErrMsg = new TErrMsg();

#if STANDFACILE
            sDir = DataManager.sGetExeDir() + "\\";
#endif

#if STAND_MONITOR
            sDir = sRootDir + "\\";
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
                sTmp = sCenterJustify(SF_Data.sHeaders[0], iMAX_RECEIPT_CHARS);

                for (j = 0; j < iLMargin; j++)
                    sTmp = sTmp.Insert(0, " ");

                if (!String.IsNullOrEmpty(SF_Data.sHeaders[0]))
                    fData.WriteLine(sTmp + "\n");

                sTmp = sCenterJustify(SF_Data.sHeaders[1], iMAX_RECEIPT_CHARS);

                for (j = 0; j < iLMargin; j++)
                    sTmp = sTmp.Insert(0, " ");

                if (!String.IsNullOrEmpty(SF_Data.sHeaders[1]))
                    fData.WriteLine(sTmp + "\n");
#elif STAND_CUCINA
                sTmp = sCenterJustify(dBaseIntf.DB_Data.sHeaders[0], iMAX_RECEIPT_CHARS);

                for (j = 0; j < iLMargin; j++)
                    sTmp = sTmp.Insert(0, " ");

                if (!String.IsNullOrEmpty(dBaseIntf.DB_Data.sHeaders[0]))
                    fData.WriteLine(sTmp + "\n");

                sTmp = sCenterJustify(dBaseIntf.DB_Data.sHeaders[1], iMAX_RECEIPT_CHARS);

                for (j = 0; j < iLMargin; j++)
                    sTmp = sTmp.Insert(0, " ");

                if (!String.IsNullOrEmpty(dBaseIntf.DB_Data.sHeaders[1]))
                    fData.WriteLine(sTmp + "\n");
#endif

                sTmp = sCenterJustify(GetDateTimeString(), iMAX_RECEIPT_CHARS);
                for (j = 0; j < iLMargin; j++)
                    sTmp = sTmp.Insert(0, " ");
                fData.WriteLine(sTmp + "\n");

                for (i = 0; i < iMAX_RECEIPT_CHARS / 2; i++)       // avanzamanto riga
                {
                    sTmp = "";
                    for (j = 0; j < iMAX_RECEIPT_CHARS; j++) // ripetizione riga
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

                sTmp = sCenterJustify("########################", iMAX_RECEIPT_CHARS);
                fData.WriteLine("{0}", sTmp);
                fData.WriteLine("{0}", sTmp);

                fData.WriteLine("");

#if STANDFACILE
                sTmp = sCenterJustify(SF_Data.sHeaders[2], iMAX_RECEIPT_CHARS);
                for (j = 0; j < iLMargin; j++)
                    sTmp = sTmp.Insert(0, " ");

                if (!String.IsNullOrEmpty(SF_Data.sHeaders[2]))
                    fData.WriteLine(sTmp + "\n");

                sTmp = sCenterJustify(SF_Data.sHeaders[MAX_NUM_HEADERS - 1], iMAX_RECEIPT_CHARS);
                for (j = 0; j < iLMargin; j++)
                    sTmp = sTmp.Insert(0, " ");

                if (!String.IsNullOrEmpty(SF_Data.sHeaders[MAX_NUM_HEADERS - 1]))
                    fData.WriteLine(sTmp + "\n");
#elif STAND_CUCINA
                sTmp = sCenterJustify(dBaseIntf.DB_Data.sHeaders[2], iMAX_RECEIPT_CHARS);
                for (j = 0; j < iLMargin; j++)
                    sTmp = sTmp.Insert(0, " ");

                if (!String.IsNullOrEmpty(dBaseIntf.DB_Data.sHeaders[2]))
                    fData.WriteLine(sTmp + "\n");

                sTmp = sCenterJustify(dBaseIntf.DB_Data.sHeaders[MAX_NUM_HEADERS - 1], iMAX_RECEIPT_CHARS);
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
            if (PrintReceiptConfigDlg.bGetPrinterTypeIsWinwows())
#else
            if (PrintConfigLightDlg.bGetPrinterTypeIsWinwows())
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
        public static String getNomeDatiDBTable(int iNumCassaParam, DateTime dateTimeParam)
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
        public static String getNomeOrdiniDBTable(DateTime dateTimeParam)
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
        public static String getNomeFileDati(int iNumCassaParam, DateTime dateTimeParam)
        {
            String sTmp, sDati;

            sTmp = dateTimeParam.ToString("'Dati_'MMdd'.txt'");
            sDati = String.Format("C{0}_{1}", iNumCassaParam, sTmp);

            return sDati;
        }

        /// <summary>
        /// ottiene il nome del file backup dati
        /// </summary>
        public static String getNomeFileDatiBak(int iNumCassaParam)
        {
            String sTmpBak, sDatiBak;

            sTmpBak = getActualDate().ToString("'Dati_'MMdd'.bak'");
            sDatiBak = String.Format("C{0}_{1}", iNumCassaParam, sTmpBak);

            return sDatiBak;
        }

#if STANDFACILE
        /// <summary>
        /// ottiene il nome del file DB SQLite, ricostruisce tutto il Path
        /// </summary>
        public static String getNomeFileDatiDB_SQLite(DateTime dateParam)
        {
            String sTmp, sAnno;

            sAnno = ANNO_DIR + dateParam.ToString("yyyy");

            sTmp = DataManager.sGetRootDir() + "\\" + sAnno + "\\" + "Dati_Standfacile.db";
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
                    sEuroDec = sEuroDec + "0";
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

        /// <summary>funzione per l'arrotondamento ai 10c,<br/>
        /// agisce su valori * 100, sempio 10.08 è rappresentato come 1008
        /// </summary>
        public static int arrotonda(double fParam)
        {
            int iDebug;

            iDebug = (int)Math.Round(fParam / 10.0) * 10;
            return iDebug;
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

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;

            //padding mode(if any extra byte added)
            tdes.Padding = PaddingMode.PKCS7;

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

            AppSettingsReader settingsReader = new AppSettingsReader();

            //if hashing was used get the hash code with regards to your key
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(CIPHER_KEY));

            hashmd5.Clear();

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;

            //mode of operation. there are other 4 modes. 
            //We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;

            //padding mode(if any extra byte added)
            tdes.Padding = PaddingMode.PKCS7;

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
        static public T deepCopy<T>(T obj)
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

        /// <summary>funzione per ricerca di una Stringa dentro alla Struct _ORDER_CONST<br/>
        /// il primo parametro è per la stringa da cercare, il secondo per eventuali esclusioni</summary>
        static public bool bStringBelongsTo_ORDER_CONST(String sStrParam, String sEsclusioneParam = SHMAGIC)
        {
            foreach (String sItem in _ORDER_CONST.sArray)
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
        static public Color[] getColor(int iParam)
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
            return (intParam & (1 << bitPosParam)) != 0;
        }

        // da evitare verifiche di 2 tipti: positive e negative
        /// <summary>verifica se il bit in posizione bitPosParam vale 0</summary>
        //public static bool IsBitClear(int intParam, int bitPosParam)
        //{
        //    return !IsBitSet(intParam, bitPosParam);
        //}

    } // end class
} // end namespace

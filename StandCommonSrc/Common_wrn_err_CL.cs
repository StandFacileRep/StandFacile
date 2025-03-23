/*****************************************************
 	NomeFile : StandCommonSrc/CommonFunc.cs
    Data	 : 06.12.2024
 	Autore	 : Mauro Artuso

	Classi statiche di uso comune
 *****************************************************/

using System;
using System.IO;
using System.Windows.Forms;

using static StandCommonFiles.ComDef;
using static StandCommonFiles.LogServer;

using StandFacile;
using static StandFacile.Define;

#if STAND_CUCINA || STAND_ORDINI || STAND_MONITOR
using static StandFacile.MessageDlg;
#endif

namespace StandCommonFiles
{
#pragma warning disable IDE0079

    /// <summary>
    /// definizione dei codici di errore e di warning
    /// </summary>
    public static partial class CommonCl
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
        /// <summary>Command Not Found</summary>
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
        public const int WRN_QRE = 830;
        /// <summary>test QR code !</summary>
        public const int WRN_TQR = 835;
        /// <summary>REMOTE Database error !</summary>
        public const int WRN_DBR = 840;
        /// <summary>Stabilita connessione con il WEB server !</summary>
        public const int WRN_WSCS = 850;
        /// <summary>Tabella Listino non trovata!</summary>
        public const int WRN_WPNF = 860;
        /// <summary>Tabella Log non trovata!</summary>
        public const int WRN_WLNF = 865;
        /// <summary>Test di comunicazione con il WEB server eseguito con successo !</summary>
        public const int WRN_WSTS = 870;
        /// <summary>Test di comunicazione con il WEB server fallito !</summary>
        public const int WRN_WSTF = 880;
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
        public const int WRN_TNNF = 980;
        /// <summary>Dimensioni del Logo errate</summary>
        public const int WRN_DLE = 990;
        /// <summary>Errore del Driver di stampa</summary>
        public const int WRN_PDE = 1000;

        /// <summary>Errore lunghezza del testo gruppi</summary>
        public const int WRN_LTE = 1010;

        /// <summary>Test Item Match Not Found</summary>
        public const int WRN_TIANF = 1020;

        /// <summary>Pricelist Upload success</summary>
        public const int WRN_PUPS = 1030;

        /// <summary>Stringa Troppo Lunga</summary>
        public const int WRN_STL = 1040;

        /// <summary>Richiesta aggiunta commento con quantità Zero</summary>
        public const int WRN_NQZ = 1050;

        /// <summary>
        /// Funzione di gestione dei warning,
        /// ogni warning ha un suo codice univoco iWrnID
        /// </summary>
        public static void WarningManager(int iWrnID)
        {
            TErrMsg WrnMsg = new TErrMsg()
            {
                sMsg = "",
                iErrID = iWrnID
            };

            WarningManager(WrnMsg);
        }

        /// <summary>
        /// funzione di generazione avviso all'utente
        /// </summary>
        public static void WarningManager(TErrMsg WrnMsg)
        {
#pragma warning disable CS0219

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
                    sWrnStr = String.Format("{0} {1}", "Stabilita connessione con il Web Server:", WrnMsg.sMsg);
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
                    {
                        if (String.IsNullOrEmpty(WrnMsg.sMsg))
                            sWrnStr = "Stringa troppo lunga nel File:   " + WrnMsg.sNomeFile + "\r\n\r\n alla riga: " + WrnMsg.iRiga +
                                    "\r\n\r\ncorreggere per visualizzazione e stampa corretta degli Articoli,\r\npoi riavviare !";
                        else
                            sWrnStr = "Stringa troppo lunga nel File:   " + WrnMsg.sNomeFile + "\r\n\r\n alla riga: " + WrnMsg.iRiga + ",   Articolo: " + WrnMsg.sMsg +
                                    "\r\n\r\ncorreggere per visualizzazione e stampa corretta degli Articoli,\r\npoi riavviare !";
                    }
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

            rMessageDlg?.MessageBox(Environment.NewLine + sWrnStr, sCaptionBuf, 10);
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
            TErrMsg ErrMsg = new TErrMsg()
            {
                sMsg = "",
                iErrID = iErrID
            };

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
                    Directory.SetCurrentDirectory(DataManager.GetExeDir());
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
                    Directory.SetCurrentDirectory(DataManager.GetExeDir());
                    System.Diagnostics.Process.Start(THE_APP);
                    bControlledExit = true;
                    break;
#endif

                case ERR_DLL: // Problemi di scrittura nel Registry
                    switch (ReadRegistry(DB_MODE_KEY, (int)DB_MODE.SQLITE))
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
                FrmMain.rFrmMain.Close();
                Application.Exit();
            }
#endif

        } // end ErrorManager()

    } // end class
} // end namespace

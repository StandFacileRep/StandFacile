/********************************************************************
	NomeFile : StandFacile/Define.cs
	Data	 : 01.11.2025
	Autore	 : Mauro Artuso

  Descrizione : definizioni specifiche per StandFacile
 ********************************************************************/

using System;
using static StandCommonFiles.ComDef;

namespace StandFacile
{
    /// <summary>classe di define specifici di StandFacile</summary>
    public static class Define
    {
#pragma warning disable IDE0060

        /// <summary>titolo</summary>
        public const string TITLE = "Stand Facile 2025";

        /// <summary>nome dell'eseguibile</summary>
        public const string THE_APP = "StandFacile.exe";

        /// <summary>nome del pdf del manuale</summary>
        public const string _NOME_MANUALE = "Manuale_StandFacile.pdf";

        // mnemonici per percorso chiave di registro
        /// <summary>chiave di registro</summary>
        public const string KEY_STAND_FACILE = "HKEY_CURRENT_USER\\Software\\StandFacile";

        /// <summary>registro: stringa del modello della stampante per i messaggi</summary>
        public const String WIN_MSG_PRINTER_MODEL_KEY = "sWinPrinterModel_msg";

        /// <summary>registro: stringa del modello della stampante per le copie</summary>
        public const string WIN_CPY_PRINTER_MODEL_MKEY = "sWinPrinterModel_{0}";

        /// <summary>registro: chiave per gestione disponibilità iniziale dei gruppi di Articoli</summary>
        public const string DISP_GROUP_KEY = "iDispGroups";

        /// <summary>registro: numero della cassa</summary>
        public const string NUM_CASSA_KEY = "iNumCassa";

        /// <summary>chiave che descrive il tema dei colori</summary>
        public const String COLOR_THEME_KEY = "iColorTheme";

        /// <summary>chiave che attiva la barra dei Pulsanti + - X</summary>
        public const String VBUTTONS_KEY = "iVButtons";

        /// <summary>posizione x della finestra principale</summary>
        public const String MAIN_WIN_POS_X = "iMainFrmPos_x";

        /// <summary>posizione y della finestra principale</summary>
        public const String MAIN_WIN_POS_Y = "iMainFrmPos_y";

        /// <summary>dimensione x della finestra principale</summary>
        public const String MAIN_WIN_SIZE_X = "iMainFrmSize_x";

        /// <summary>dimensione y della finestra principale</summary>
        public const String MAIN_WIN_SIZE_Y = "iMainFrmSize_y";

        /// <summary>posizione x della finestra  di anteprima</summary>
        public const String PREVIEW_WIN_POS_X = "iPreviewFrmPos_x";

        /// <summary>posizione y della finestra  di anteprima</summary>
        public const String PREVIEW_WIN_POS_Y = "iPreviewFrmPos_y";

        /// <summary>dimensione x della finestra  di anteprima</summary>
        public const String PREVIEW_WIN_SIZE_X = "iPreviewFrmSize_x";

        /// <summary>dimensione y della finestra  di anteprima</summary>
        public const String PREVIEW_WIN_SIZE_Y = "iPreviewFrmSize_y";

        /// <summary>dimensione y della finestra  di anteprima</summary>
        public const String PREVIEW_WIN_STATUS = "iPreviewFrmStatus";

        /// <summary>registro: numero dei temi colore</summary>
        public const int NUM_COLOR_THEMES = 3;

        /*****************************************************
         *                      eventi
         *****************************************************/
        /// <summary>evento reset del bottone di emissione scontrino</summary>
        public const string RESET_RECEIPT_BTN_EVENT = "resetReceiptBtnEvent";

        /// <summary>evento di aggiornamento stato COM</summary>
        public const string UPDATE_COM_STATUS_EVENT = "updateComStatusEvent";

        /// <summary>evento fine caricamento ordine di prevendita</summary>
        public const string PREV_ORDER_LOAD_DONE = "fattoCaricamento_PrevOrdineDB_Dlg_Event";

        /// <summary>evento inizio caricamento sintesi di tutti gli ordini</summary>
        public const string WEB_ALL_ORDERS_LOAD_START = "avviaCaricamento_EsploraWebOrdiniDB_Dlg_Event";

        /// <summary>evento fine caricamento sintesi di tutti gli ordini</summary>
        public const string WEB_ALL_ORDERS_LOAD_DONE = "fattoCaricamento_EsploraWebOrdiniDB_Dlg_Event";

        /// <summary>evento fine caricamento singolo ordine web</summary>
        public const string WEB_ORDER_LOAD_DONE = "fattoCaricamento_WebOrdineDB_Dlg_Event";

        /// <summary>evento inizio stampa ordine web</summary>
        public const string WEB_ORDER_PRINT_START = "avviaStampa_EsploraWebOrdiniDB_Dlg_Event";

        /// <summary>evento inizio stampa ordine dal dialogo DataCheckDlg</summary>
        public const string ORDER_PRINT_START = "avviaStampa_DataCheckDlg_Event";

        /// <summary>evento fine stampa ordine web, usato in 2 code eventi diverse: dBaseTunnel_my, EsploraRemOrdiniDB_Dlg</summary>
        public const string WEB_ORDER_PRINT_DONE = "fattaStampa_EsploraWebOrdiniDB_Dlg_Event";

        /// <summary>evento avvio caricamento del Listino</summary>
        public const string WEB_PRICELIST_LOAD_START = "avviaCaricamentoListino_dBaseTunnel_my_Event";

        /// <summary>evento avvio caricamento forzato del Listino</summary>
        public const string WEB_PRICELIST_FORCE_LOAD_START = "avviaCaricamentoForzatoListino_dBaseTunnel_my_Event";

        /// <summary>evento fine caricamento Listino</summary>
        public const string WEB_PRICELIST_LOAD_DONE = "fattoCaricamentoListino_dBaseTunnel_my_Event";

        /// <summary>evento reset del bottone di emissione scontrino</summary>
        public const string MAIN_GRID_UPDATE_EVENT = "mainGridUpdateEvent";

        /**********************************************************************************
               mnemonici chiavi del file config.ini:
               queste stringhe consentono l'attivazione di alcune funzioni di debug
         **********************************************************************************/

        /// <summary>
        ///  mnemonici chiavi del file config.ini:<br/>
        /// queste stringhe consentono l'attivazione di alcune funzioni di debug
        /// </summary>
        public struct CFG_SERVICE_STRINGS
        {
            /// <summary>serviceStrings: generazione random di scontrini per test</summary>
            public const string _AUTO_RECEIPT_GEN = "randTest";

            /// <summary>
            /// serviceStrings: generazione automatica di scontrini da file di sequenza per test <br/>
            /// aggiungendo "_C1" esegue test con sola cassa 1
            /// </summary>
            public const string _AUTO_SEQ_TEST = "seqTest"; // _C1

            /// <summary>serviceStrings: generazione automatica scontrini mediante qrcode per test</summary>
            public const string _AUTO_QRCODE_TEST = "qrcodeTest";

            /// <summary>serviceStrings: acquisizione di scontrini per test</summary>
            public const string _REC_TEST = "recTest";

            /// <summary>serviceStrings: evita le presentazione del dialogo relativo alla Data</summary>
            public const string _SKIP_DATA = "noData";

            /// <summary>serviceStrings: attivazione stampa su carta A4</summary>
            public const string PRINT_ON_A4_PAPER = "printOnA4Paper";

            /// <summary>serviceStrings: descrive se si stampano 4 righe prima del taglio carta, utile per carta A5</summary>
            public const string PRINT_ON_A5_PAPER = "printOnA5Paper";
        }

        /// <summary>refresh di sService * 250ms circa in StandFacile, 200 in StandClient</summary>
        public const int REFRESH_SSERVICE = 4 * 60;

        /// <summary>timer di refresh della disponibilità</summary>
        public const int _REFRESH_DISP = 4 * 40;
        /// <summary>timer breve di refresh della disponibilità</summary>
        public const int _REFRESH_DISP_SHORT = 8;

        // nomi delle directories in uso
        /// <summary>path della directory root</summary>
        public const string ROOT_DIR = "..\\StandDati";
        /// <summary>path dell'anno</summary>
        public const string ANNO_DIR = "Anno_";
        /// <summary>path dei dati</summary>
        public const string DATA_DIR = ".\\";
        /// <summary>path del Log</summary>
        public const string LOG_DIR = ".\\";

        // mnemonici per i nomi dei files, attenzione che la stampa è ricucita su questa lunghezza
        // se si cambia rivedere Printer_Epson_POS.cs, Printer_LP2844.cs

        /// <summary>formato nome dello scontrino</summary>
        public const string NOME_FILE_RECEIPT = "C{0}_TT{1:d4}.txt";
        /// <summary>formato nome della copia locale scontrino</summary>
        public const string NOME_FILE_RECEIPT_NP = "C{0}_TN{1:d4}.txt";

        /// <summary>formato nome copie</summary>
        public const string NOME_FILE_COPIE_NET = "C{0}_CT{1:d4}_G{2}.txt";
        /// <summary>formato nome copie singole</summary>
        public const string NOME_FILE_COPIE_SINGOLE = "C{0}_CT{1:d4}_G{2}_S{3}.txt";

        /// <summary>formato nome del messaggio</summary>
        public const string NOME_FILE_MESSAGGIO = "C{0}_Msg{1:d4}.txt";

        /// <summary>avviso file non esiste</summary>
        public const string FILE_NON_ESISTE = "FILE_NON_ESISTE!";

        /// <summary>QRCode di prova, solo ckecksum</summary>
        public const string QRC_TEST_CKS = "2C_#@$_EF2E";

        /// <summary>QRCode di prova presente nel manuale, solo ckecksum</summary>
        public const string QRC_TEST = "{\"-10\":\"js_order_v5c\",\"-9\":\"1042\",\"-6\":\"11\",\"-5\":0,\"-4\":\"mauro\",\"-3\":\"123\",\"-2\":\"prova\",\"-1\":\"2C_#@$_EF2E\",\"400\":4,\"0\":1,\"1\":2,\"2\":3}";

        /// <summary>valori massimi numero di messaggi</summary>
        public const int MAX_NUM_MSG = 999; // massimo numero di messaggi

        // 250ms
        /// <summary>timer visualizzazione dei Prezzi</summary>
        public const int TIMEOUT_VIS_PREZZI = 4 * 10;
        /// <summary>timer per la stampa web automatica</summary>
        public const int TIMEOUT_WEB_PRINT = 4 * 2;
        /// <summary>timer di modifica disponibilità</summary>
        public const int TIMEOUT_MOD_QUANTITA = 4 * 10;
        /// <summary>timeout per il reset del bottone di stampa scontrino</summary>
        public const int TIMEOUT_PRINT_RESET = 4 * 5;

        /// <summary>massima dimensione delle stringhe come 'COPIA GRUPPO1' ...</summary>
        public const int MIN_COPIES_CHARS = 5;

        /// <summary>max caratteri delle intestazioni TABs</summary>
        public const int MAX_PAGES_CHAR = 16;

        /// <summary>dimensioni larghezza della Main Window</summary>
        public const int MAINWD_WIDTH = 1024;
        /// <summary>dimensioni altezza della Main Window</summary>
        public const int MAINWD_HEIGHT = 660;

        /// <summary>struct per le gestione dello sconto</summary>
        [Serializable()]
        public struct TSconto
        {
            /// <summary>
            /// stato dello sconto 0x00zzyyyx :<br/>
            /// x rappresenta il tipo di sconto da applicare
            /// yyy rappresenta i flag bScontoGruppo[]<br/>
            /// zz rappresenta la percentuale dello sconto standard
            /// </summary>
            public int iStatusSconto;
            /// <summary>
            /// valore dello sconto percentuale applicato<br/>
            /// viene ricalcolato in base a iStatusReceipt
            /// </summary>
            public int iScontoValPerc;
            /// <summary>valore dello sconto fisso</summary>
            public int iScontoValFisso;
            /// <summary>causale dello sconto</summary>
            public string[] sScontoText;

            /// <summary>flag sconto per gruppi</summary>
            public bool[] bScontoGruppo;

            /// <summary>costruttore</summary>
            public TSconto(int iParam)
            {
                iStatusSconto = 0;
                iScontoValPerc = 0;
                iScontoValFisso = 0;

                sScontoText = new string[NUM_DISC_TYPE];

                sScontoText[(int)DISC_TYPE.DISC_NONE] = "";
                sScontoText[(int)DISC_TYPE.DISC_STD] = "";
                sScontoText[(int)DISC_TYPE.DISC_FIXED] = "";
                sScontoText[(int)DISC_TYPE.DISC_GRATIS] = "";

                bScontoGruppo = new bool[NUM_SEP_PRINT_GROUPS];
            }
        }

        /// <summary>definizione della struttura per sintesi dell'ordine remoto</summary>
        public struct TWebOrder
        {
            /// <summary>numero ordine remoto</summary>
            public int iNumOrdine;
            /// <summary>stato ordine remoto</summary>
            public int iStatus;
            /// <summary>coperti Scontrino remoto</summary>
            public int iNumCoperti;
            /// <summary>totale Scontrino remoto</summary>
            public int iTotaleReceipt;
            /// <summary>nome Cliente</summary>
            public String sCliente;
            /// <summary>data ed ora ordine web</summary>
            public String sDateTime;
            /// <summary>checksum</summary>
            public String sChecksum;

            /// <summary>costruttore</summary>
            public TWebOrder(int iParam)
            {
                iNumOrdine = 0;
                iStatus = 0;
                iNumCoperti = 0;
                iTotaleReceipt = 0;
                sCliente = "";
                sDateTime = "";
                sChecksum = "";
            }
        };

    } // end class
}

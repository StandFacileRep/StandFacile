/************************************************************
    NomeFile : StandCommonSrc/CommonDefine.cs
    Data	 : 30.11.2025
    Autore	 : Mauro Artuso
 ************************************************************/

using System;
using System.Drawing;

namespace StandCommonFiles
{
    /// <summary>
    /// classe di mnemonici comuni a StandFacile
    /// </summary>
    public static class ComDef
    {
#pragma warning disable IDE0060

        /// <summary>versione del Programma</summary>
        public const String RELEASE_SW = "v5.16.1";

        /// <summary>prefisso versione delle tabelle DB</summary>
        public const String RELEASE_DB_TBLS = "v5d";

        /// <summary>mail per informazioni</summary>
        public const String MAIL = "info@standfacile.org";
        /// <summary>autore del Programma</summary>
        public const String AUTHOR = "Mauro Artuso";
        /// <summary>sito internet del Programma</summary>
        public const String URL_SITO = "www.standfacile.org";

        /// <summary>dll Devart comune</summary>
        public const String DB_CONNECTOR_DLL_DEV = "Devart.Data.dll";
        /// <summary>dll Devart per MySQL</summary>
        public const String DB_CONNECTOR_DLL_MY = "Devart.Data.MySql.dll";
        /// <summary>dll Devart per PostgreSQL</summary>
        public const String DB_CONNECTOR_DLL_PG = "Devart.Data.PostgreSql.dll";
        /// <summary>dll Devart per SQLite</summary>
        public const String DB_CONNECTOR_DLL_QL = "Devart.Data.SQLite.dll";
        /// <summary>dll libreria SQLite</summary>
        public const String DB_CONNECTOR_DLL_QL32 = "x86\\sqlite3.dll";
        /// <summary>dll libreria SQLite</summary>
        public const String DB_CONNECTOR_DLL_QL64 = "x64\\sqlite3.dll";

        // *** evitare maiuscole ***
        /// <summary>database: nome tabella di stato</summary>
        public const string NOME_STATO_DBTBL = RELEASE_DB_TBLS + "_stato";

        /// <summary>prefisso tabella degli ordini</summary>
        public const string _dbOrdersTablePrefix = RELEASE_DB_TBLS + "_ordini";

        /// <summary>prefisso tabella degli ordini in prevendita</summary>
        public const string _dbPreOrdersTablePrefix = RELEASE_DB_TBLS + "_ordini_prev";

        /// <summary>prefisso tabella dei dati</summary>
        public const string _dbDataTablePrefix = RELEASE_DB_TBLS + "_dati";

        /// <summary>prefisso tabella dei dati di prevendita</summary>
        public const string _dbPreDataTablePrefix = RELEASE_DB_TBLS + "_dati_prev";

        /// <summary>
        ///  mnemonici chiavi del file config.ini:<br/>
        /// queste stringhe consentono l'attivazione di alcune funzioni di debug
        /// </summary>
        public struct CFG_COMMON_STRINGS
        {
            /// <summary>evita di stampare lo scontrino</summary>
            public const String _SKIP_STAMPA_RCP = "noStampaRcp";

            /// <summary>abilita la modalità Esperto</summary>
            public const String _ESPERTO = "Esperto";

            /// <summary>abilita le stampanti legacy</summary>
            public const String _HIDE_LEGACY_PRINTER = "noLegacyPrinters";
        }

        /// <summary>indicatore di ordine formattato JSON</summary>
        public const String _JS_ORDER_V5 = "js_order_v5c";

        /// <summary>radice segnaposto per indicare i tipi vuoti</summary>
        public const String SHMAGIC = "_#@$_";
        /// <summary>valore segnaposto per indicare i tipi vuoti</summary>
        public const String MAGIC = SHMAGIC + "{0}";

        /// <summary> stringhe costanti dentro alla db table ordini</summary>
        public struct ORDER_CONST
        {
            /// <summary>sTipo_Articolo per memorizzazione del tavolo nella tabella ordini_YYmmdd</summary>
            public const String _START_OF_ORDER = "_StartOfOrder" + SHMAGIC;

            /// <summary>sTipo_Articolo per memorizzazione del tavolo nella tabella ordini_YYmmdd</summary>
            public const String _TAVOLO = "_Table" + SHMAGIC;

            /// <summary>sTipo_Articolo per memorizzazione del nome utente nella tabella ordini_YYmmdd</summary>
            public const String _NOME = "_Name" + SHMAGIC;

            /// <summary>sTipo_Articolo per memorizzazione della nota nella tabella ordini_YYmmdd</summary>
            public const String _NOTA = "_Note" + SHMAGIC;

            /// <summary>sTipo_Articolo per memorizzazione dello sconto Fisso e Gratis nella tabella ordini_YYmmdd</summary>
            public const String _SCONTO = "_Sconto" + SHMAGIC;

            /// <summary>sTipo_Articolo per memorizzazione del valore Buoni Applicati nella tabella ordini_YYmmdd</summary>
            public const String _BUONI = "_Buoni" + SHMAGIC;

            /// <summary>menuItem_ID per memorizzazione del numero dell'ordine da web nella tabella ordini_YYmmdd</summary>
            public const String _NUM_ORD_WEB = "_NumWebOrd" + SHMAGIC;

            /// <summary>menuItem_ID per memorizzazione del numero dell'ordine in prevendita nella tabella ordini_YYmmdd</summary>
            public const String _NUM_ORD_PREV = "_NumOrdPrevendita" + SHMAGIC;

            /// <summary>menuItem_ID per memorizzazione checksum nella tabella ordini web</summary>
            public const String _PRICE_LIST_CHECKSUM = "_PriceListChecksum" + SHMAGIC;

            /// <summary>array per la ricerca di una stringa</summary>
            public static readonly String[] sArray = new String[] { _START_OF_ORDER, _TAVOLO, _NOME, _NOTA, _SCONTO,
                                                                        _BUONI, _NUM_ORD_WEB, _NUM_ORD_PREV };
        }

        /// <summary>mnemonico per il numero della cassa nelle stampe con Zoom numero scontrino</summary>
        public const String _TICK_CASSA = "Cassa =";
        
        /// <summary>mnemonico per il numero dello scontrino nelle stampe con Zoom numero scontrino</summary>
        public const String _TICK_NUM = "Num. =";

        /// <summary>mnemonico per il numero dello scontrino nelle stampe senza Zoom numero scontrino</summary>
        public const String _TICK_NUM_NZ = "Scontrino num.";

        /// <summary>mnemonico per il numero dello scontrino di prevendita nelle stampe</summary>
        public const String _PREV_NUM = "prevNum =";

        /// <summary>mnemonico per il numero dello scontrino di pre-ordine web nelle stampe</summary>
        public const String _WEB_NUM = "webNum =";


        /// <summary>numero massimo in assoluto di caratteri per Receipt</summary>
        public const int MAX_ABS_RECEIPT_CHARS = 33;

        /// <summary>numero massimo storico di caratteri per Receipt</summary>
        public const int MAX_LEG_RECEIPT_CHARS = 28;

        /// <summary>numero massimo di caratteri per riga per le copie (soggette a zoom)</summary>
        public const int MAX_RECEIPT_CHARS_CPY = 28;

        /// <summary>numero massimo in assoluto di caratteri per articolo</summary>
        public const int MAX_ABS_ART_CHAR = 23;

        /// <summary>numero massimo storico di caratteri per articolo</summary>
        public const int MAX_LEG_ART_CHAR = 18;

        /// <summary>max caratteri del testo decrittivo delle copie</summary>
        public const int MAX_COPIES_TEXT_CHARS = 25;

        /// <summary>Intero per centraggio del Numero dell'ordine</summary>
        public static int iCenterOrderNum;


        /// <summary>Stringhe di formattazione stampe<br/>
        /// larghezza 28 "{0,2} {1,-18}{2,7}" :89 123456789012345678 9876.00<br/>
        /// larghezza 33 "{0,2} {1,-23}{2,7}" :89 12345678901234567809123 9876.00
        /// </summary>
        public static String sRCP_FMT_RCPT, sRCP_FMT_CPY, sRCP_FMT_DSC, sRCP_FMT_DIF, sRCP_FMT_TOT, sRCP_FMT_DSH, sRCP_FMT_NOTE;

#if STANDFACILE
        /// <summary>Stringhe di formattazione griglia, listino, etc.</summary>
        public static String sGRD_FMT_STD, sGRD_FMT_TCH, sGRDW_FMT_STD, sGRDZ_FMT_TCH, sGRDW_FMT_TCH;
#endif

        /// <summary>Stringhe di formattazione dati</summary>
        public static String sDAT_FMT_PRL, sDAT_FMT_DAT, sDAT_FMT_TOT, sDAT_FMT_DSH;

        /// <summary>Stringhe di formattazione dati</summary>
        public static String sDAT_FMT_HED, sDAT_FMT_REP_RED, sDAT_FMT_DSH_RED, sDAT_FMT_TOT_RED;


        /// <summary>stringa per formattazione receipt base 28 char</summary>
        public const String _RCP_FMT_28_RCPT = "{0,2} {1,-18}{2,7}";

        /// <summary>stringa per formattazione receipt base 33 char</summary>
        public const String _RCP_FMT_33_RCPT = "{0,2} {1,-23}{2,7}";

        /// <summary>stringa per formattazione copie base 28 char</summary>
        public const String _RCP_FMT_28_CPY = "{0,3} {1,-18}";

        /// <summary>stringa per formattazione copie base 33 char</summary>
        public const String _RCP_FMT_33_CPY = "{0,3} {1,-23}";

        /// <summary>stringa per formattazione nota base 28 char</summary>
        public const String _RCP_FMT_28_NOTE = "   {0,-25}";

        /// <summary>stringa per formattazione nota base 33 char</summary>
        public const String _RCP_FMT_33_NOTE = "   {0,-30}";

        /// <summary>stringa per formattazione sconti base 28 char</summary>
        public const String _RCP_FMT_28_DSC = "{0,7}{1,6} {2,6}{3,8}";

        /// <summary>stringa per formattazione sconti base 33 char</summary>
        public const String _RCP_FMT_33_DSC = "{0,7}{1,6} {2,11}{3,8}";

        /// <summary>stringa per formattazione receipt base 28 char</summary>
        public const String _RCP_FMT_28_DIF = "{0,20}{1,8}";

        /// <summary>stringa per formattazione receipt base 33 char</summary>
        public const String _RCP_FMT_33_DIF = "{0,25}{1,8}";

        /// <summary>stringa per formattazione receipt base 28 char</summary>
        public const String _RCP_FMT_28_TOT = "{0,18}{1,10}";

        /// <summary>stringa per formattazione sconti base 33 char</summary>
        public const String _RCP_FMT_33_TOT = "{0,23}{1,10}";

        /// <summary>stringa per formattazione trattini di separazione base 28 char</summary>
        public const String _RCP_FMT_28_DSH = "{0,28}";

        /// <summary>stringa per formattazione trattini di separazione base 33 char</summary>
        public const String _RCP_FMT_33_DSH = "{0,33}";


#if STANDFACILE
        /// <summary>stringa per formattazione griglia base 28 char</summary>
        public const String _GRD_FMT_28_STD = " {0,2} {1,-18} {2,5:0.00}";

        /// <summary>stringa per formattazione griglia base 33 char</summary>
        public const String _GRD_FMT_33_STD = " {0,2} {1,-23} {2,5:0.00}";

        /// <summary>stringa per formattazione visListino griglia base 28 char</summary>
        public const String _GRD_FMT_28_TCH = " {0,2} {1,14:0.00}{2}{2}{3,-18}";

        /// <summary>stringa per formattazione visListino griglia base 33 char</summary>
        public const String _GRD_FMT_33_TCH = " {0,2} {1,14:0.00}{2}{2}{3,-23}";

        /// <summary>stringa per formattazione work griglia base 28 char</summary>
        public const String _GRDW_FMT_28_STD = "{0,3} {1,-18} {2,2}";

        /// <summary>stringa per formattazione work griglia base 33 char</summary>
        public const String _GRDW_FMT_33_STD = "{0,3} {1,-23} {2,2}";

        /// <summary>stringa per formattazione work griglia base 28 char</summary>
        public const String _GRDZ_FMT_28_TCH = "{0,18}{1}{1}{2,-18}";

        /// <summary>stringa per formattazione work griglia base 33 char</summary>
        public const String _GRDZ_FMT_33_TCH = "{0,23}{1}{1}{2,-23}";

        /// <summary>stringa per formattazione work griglia base 28 char</summary>
        public const String _GRDW_FMT_28_TCH = " ds:{0,-3}{1,11}{2}{2}{3,-18}";

        /// <summary>stringa per formattazione work griglia base 33 char</summary>
        public const String _GRDW_FMT_33_TCH = " ds:{0,-3}{1,11}{2}{2}{3,-23}";

#endif
        /// <summary>stringa per formattazione listino base 28 char</summary>
        public const String _DAT_FMT_28_PRL = "{0,-18} ; {1,6} ; {2,2}";

        /// <summary>stringa per formattazione listino base 33 char</summary>
        public const String _DAT_FMT_33_PRL = "{0,-23} ; {1,6} ; {2,2}";

        /// <summary>stringa per formattazione dati base 28 char</summary>
        public const String _DAT_FMT_28_DAT = "{0,-18}{1,8}{2,6}{3,9}{4,5}";

        /// <summary>stringa per formattazione dati base 33 char</summary>
        public const String _DAT_FMT_33_DAT = "{0,-23}{1,8}{2,6}{3,9}{4,5}";

        /// <summary>stringa per formattazione dati base 28 char</summary>
        public const String _DAT_FMT_28_TOT = " {0,30}{1,10}";

        /// <summary>stringa per formattazione dati base 33 char</summary>
        public const String _DAT_FMT_33_TOT = " {0,35}{1,10}";

        /// <summary>stringa per formattazione dati base 28 char</summary>
        public const String _DAT_FMT_28_DSH = " {0,40}";

        /// <summary>stringa per formattazione dati base 33 char</summary>
        public const String _DAT_FMT_33_DSH = " {0,45}";

        /// <summary>stringa per formattazione dati base 28 char</summary>
        public const String _DAT_FMT_28_HED = "{0,28}{1,4}";

        /// <summary>stringa per formattazione dati base 33 char</summary>
        public const String _DAT_FMT_33_HED = "{0,33}{1,4}";


        /// <summary>stringa per formattazione dati vis. ridotta base 28 char</summary>
        public const String _DAT_FMT_28_REP_RED = "{0,-18}{1,6}{2,9}";

        /// <summary>stringa per formattazione listino base 33 char</summary>
        public const String _DAT_FMT_33_REP_RED = "{0,-23}{1,6}{2,9}";

        /// <summary>stringa per formattazione dati vis. ridotta base 28 char</summary>
        public const String _DAT_FMT_28_DSH_RED = " {0,32}";

        /// <summary>stringa per formattazione listino base 33 char</summary>
        public const String _DAT_FMT_33_DSH_RED = " {0,37}";

        /// <summary>stringa per formattazione dati vis. ridotta base 28 char</summary>
        public const String _DAT_FMT_28_TOT_RED = " {0,22}{1,10}";

        /// <summary>stringa per formattazione listino base 33 char</summary>
        public const String _DAT_FMT_33_TOT_RED = " {0,27}{1,10}";

        // altre voci di registro

        /// <summary>chiave che descrive se la stampante è Windows o Legacy</summary>
        public const String SYS_PRINTER_TYPE_KEY = "iSysPrinter";

        /// <summary>chiave che descrive il tipo di stampante Legacy</summary>
        public const String LEGACY_PRINTER_MODEL_KEY = "iLegPrinterModel";

        /// <summary>chiave che descrive il tipo di stampante Windows (di Sistema con Driver)</summary>
        public const String WIN_PRINTER_MODEL_KEY = "sWinPrinterModel";

        /// <summary>chiave che descrive se è richiesta la stampa del barcode</summary>
        public const String STAMPA_BARCODE_KEY = "iStampaBarcode";

        /// <summary>chiave che descrive se si utilizzano 33 caratteri per riga, oppure 28</summary>
        public const String PRINT_ON_33CHARS_RECEIPT_KEY = "iPrint33CharsReceipt";

        /// <summary>chiave per la gestione del dialogo disponibilità</summary>
        public const string DISP_DLG_MNG_KEY = "bDispMngDlg";

        /// <summary>chiave per la gestione del caricamento della Prevendita</summary>
        public const string PRESALE_LOAD_MODE_KEY = "bPreSaleLoadMode";

        /// <summary>chiave per la visualizzazione dello scontrino precedente</summary>
        public const string VIEW_PREV_RECEIPT_KEY = "bShowPrevReceipt";

        /// <summary>chiave che descrive se è richiesto l'uso del DB</summary>
        public const String DB_MODE_KEY = "iDB_Mode";

        /// <summary>chiave che descrive se è richiesta la stampa della copia per il gruppo, ed il colore</summary>
        public const String SEL_DB_COPY_KEY = "iDB_RequestCopy{0}";

        /// <summary>chiave per memorizzare il DB server corrente</summary>
        public const String DBASE_SERVER_NAME_KEY = "sDB_ServerName";

        /// <summary>chiave che memorizza la password</summary>
        public const String DBASE_PASSWORD_KEY = "sDB_password";

        /// <summary>chiave per memorizzare i DB server per i quali la comunicazione ha avuto successo</summary>
        public const String SEL_DB_SERVER_KEY = "sComboServerDB{0}";

        /// <summary>chiave per memorizzare il nome della pagina web che fornisce il servizio</summary>
        public const String WEB_SERVER_NAME_KEY = "sWeb_Server";

        /// <summary>chiave per memorizzare il nome della pagina web che fornisce il servizio per i quali la comunicazione ha avuto successo</summary>
        public const String SEL_WEB_SERVER_KEY = "sComboServerWeb{0}";

        /// <summary>chiave per memorizzare il nome del DBase remoto</summary>
        public const String WEB_DBASE_NAME_KEY = "sWeb_DBase";

        /// <summary>chiave per memorizzare la password del DBase remoto</summary>
        public const String WEB_DBASE_PWD_KEY = "sWeb_DBasePwd";

        /// <summary>chiave che memorizza la password di protezione accesso</summary>
        public const String SET_ACCESS_KEY = "sAccess_Pwd";

        /// <summary>chiave che memorizza la WebService attivo o meno</summary>
        public const String WEB_SERVICE_MODE_KEY = "iWebService";

        /**************************************************************** 
         * attenzione: 
         * le funzioni di crittografia App e WebService sono diverse 
         * quindi la stessa stringa viene crittografata in modo diverso
         ****************************************************************/

        /// <summary>massimo numero di voci dei combo</summary>
        public const int MAX_COMBO_ITEMS = 4;

        // mnemonici per i nomi dei files        
        /// <summary>nome del file di stampa di prova</summary>
        public const String NOME_FILE_SAMPLE_TEXT = "TestoDiProva.txt";
        /// <summary>nome del file di stampa dati</summary>
        public const String NOME_FILE_STAMPA_LOC_TMP = "StampaTmp.txt";
        /// <summary>nome del file di stampa dati ridotta</summary>
        public const String NOME_FILE_STAMPA_LOC_RID_TMP = "StampaRidTmp.txt"; // numero di colonne ridotto -> font più grande

        /// <summary>nome del file di Listino</summary>
        public const String NOME_FILE_LISTINO = "Listino.txt";
        /// <summary>nome del file di backup Listino</summary>
        public const String NOME_FILE_LISTINO_BK = "Listino.bak";

        /// <summary>nome del file di configurazione</summary>
        public const String CONFIG_FILE = "config.ini";

        /// <summary>nome del file che contiene la sequenza di test</summary>
        public const string NOME_FILE_TEST = "sequenzaTest.txt";

        /// <summary>nome del file che contiene la sequenza di test in recording</summary>
        public const string NOME_FILE_REC = "testRecord.txt";

        /// <summary>nome della directory temporanea per visualizzare gli Scontrini emessi in altra data</summary>
        public const String NOME_DIR_RECEIPTS_VO = "ReceiptsTmp";

        /// <summary>nome della directory per gli Scontrini</summary>
        public const String NOME_DIR_RECEIPTS = "Receipts_";

        /// <summary>nome della directory per i Messages</summary>
        public const String NOME_DIR_MSGS = "Msgs_";

        /// <summary>nome della directory per le copie</summary>
        public const String NOME_DIR_COPIES = "Copies_";

        /// <summary>nome della directory temporanea per visualizzare le copie emesse in altra data</summary>
        public const String NOME_DIR_COPIES_VO = "CopiesTmp";


        /// <summary>Dimensioni di larghezza del Logo</summary>
        public const int LOGO_WIDTH = 600;
        /// <summary>Dimensioni di altezza del Logo</summary>
        public const int LOGO_HEIGHT = 500;

        /// <summary>timeout di accesso al DB con open()</summary>
        public const int TIMEOUT_DB_OPEN = 4;

        // mnemonici per alcuni tasti
        /// <summary>mnemonico tasto</summary>
        public const int KEY_TAB = 9;
        /// <summary>mnemonico tasto</summary>
        public const int KEY_FF = 12;
        /// <summary>mnemonico tasto</summary>
        public const int KEY_ENTER = 13;
        /// <summary>mnemonico tasto</summary>
        public const int KEY_ESC = 27;
        /// <summary>mnemonico tasto</summary>
        public const int KEY_NONE = 32;
        /// <summary>mnemonico tasto</summary>
        public const int KEY_PAGEUP = 33;
        /// <summary>mnemonico tasto</summary>
        public const int KEY_PAGEDOWN = 34;
        /// <summary>mnemonico tasto</summary>
        public const int KEY_END = 35;
        /// <summary>mnemonico tasto</summary>
        public const int KEY_HOME = 36;
        /// <summary>mnemonico tasto</summary>
        public const int KEY_LEFT = 37;
        /// <summary>mnemonico tasto</summary>
        public const int KEY_UP = 38;
        /// <summary>mnemonico tasto</summary>
        public const int KEY_RIGHT = 39;
        /// <summary>mnemonico tasto</summary>
        public const int KEY_DOWN = 40;
        /// <summary>mnemonico tasto</summary>
        public const int KEY_INS = 45;
        /// <summary>mnemonico tasto</summary>
        public const int KEY_DEL = 46;
        /// <summary>mnemonico tasto</summary>
        public const int KEYNUMPAD0 = 96;
        /// <summary>mnemonico tasto</summary>
        public const int KEYNUMPAD9 = 105;
        /// <summary>mnemonico tasto</summary>
        public const int KEY_PLUS_NUM = 107;
        /// <summary>mnemonico tasto</summary>
        public const int KEY_MIN_NUM = 109;
        /// <summary>mnemonico tasto</summary>
        public const int KEY_F1 = 112;
        /// <summary>mnemonico tasto</summary>
        public const int KEY_F2 = 113;
        /// <summary>mnemonico tasto</summary>
        public const int KEY_F3 = 114;
        /// <summary>mnemonico tasto</summary>
        public const int KEY_F4 = 115;
        /// <summary>mnemonico tasto</summary>
        public const int KEY_F5 = 116;
        /// <summary>mnemonico tasto</summary>
        public const int KEY_F6 = 117;
        /// <summary>mnemonico tasto</summary>
        public const int KEY_F7 = 118;
        /// <summary>mnemonico tasto</summary>
        public const int KEY_F8 = 119;
        /// <summary>mnemonico tasto</summary>
        public const int KEY_F9 = 120;
        /// <summary>mnemonico tasto</summary>
        public const int KEY_F10 = 121;
        /// <summary>mnemonico tasto</summary>
        public const int KEY_F11 = 122;
        /// <summary>mnemonico tasto</summary>
        public const int KEY_F12 = 123;

        /// <summary>mnemonico tasto</summary>
        public const int KEY_PLUS = 187;
        /// <summary>mnemonico tasto</summary>
        public const int KEY_MINUS = 189;

        /// <summary>mnemonico tasto</summary>
        public const int KEY_E = 69;

        /// <summary>mnemonico tasto</summary>
        public const int ASCII_XON = 0x11;
        /// <summary>mnemonico tasto</summary>
        public const int ASCII_XOFF = 0x13;

        /// <summary>struct per la gestione degli errori e warnings</summary>
        public struct TErrMsg
        {
            /// <summary>TErrMsg: ID eccezione</summary>
            public int iErrID;
            /// <summary>TErrMsg: eventuale riga del file in cui si è verificato l'errore</summary>
            public int iRiga;
            /// <summary>TErrMsg: stringa per il messaggio associato</summary>
            public String sMsg;
            /// <summary>TErrMsg: nome del file</summary>
            public String sNomeFile;

            /// <summary>costruttore struct per la gestione degli errori</summary>
            public TErrMsg(int i)
            {
                iErrID = i;
                iRiga = 0;
                sMsg = "";
                sNomeFile = "";
            }
        };

        /// <summary>
        /// enum per distinguere il tipo di database
        /// </summary>
        public enum DB_MODE
        {
            /// <summary>DB_MODE: SQLite</summary>
            SQLITE = 0,
            /// <summary>DB_MODE: MySQL</summary>
            MYSQL,
            /// <summary>DB_MODE: PostgreSQL</summary>
            POSTGRES
        };

        /// <summary>enum per le opzioni di conversione EuroToInt</summary>
        public enum EURO_CONVERSION
        {
            /// <summary>don't care returns 0 on 0 conversion</summary>
            EUROCONV_DONT_CARE = -1,
            /// <summary>returns -1 on 0 conversion</summary>
            EUROCONV_NZ,
            /// <summary>lancia warning</summary>
            EUROCONV_WARN,
            /// <summary>lancia errore</summary>
            EUROCONV_ERROR,
            /// <summary>accetta zero ma lancia warning</summary>
            EUROCONV_Z_WARN,
            /// <summary>accetta zero ma lancia errore</summary>
            EUROCONV_Z_ERROR
        };

        /// <summary>
        /// enum per la selezione della stampante
        /// </summary>
        public enum PRINTER_SEL
        {
            /// <summary>stampante windows</summary>
            STAMPANTE_WINDOWS = 0,
            /// <summary>stampante legacy</summary>
            STAMPANTE_LEGACY
        };

        /// <summary>
        /// struct per la gestione dei parametri della stampante seriale
        /// </summary>
        public struct TLegacyPrinterParams
        {
            /// <summary>modello di stampante</summary>
            public int iPrinterModel;
            /// <summary>1 se è richiesto il logo</summary>
            public int iLogoBmp;
            /// <summary>tipo di font</summary>
            public int iFontType;
            /// <summary>larghezza carta</summary>
            public int iPaperSize;
            /// <summary>velocità avanzamento carta</summary>
            public int iPaperSpeed;
            /// <summary>densità della stampa chiaro/scuro</summary>
            public int iDensity;
            /// <summary>tipo di controllo di flusso</summary>
            public int iFlowCtrl;
            /// <summary>porta della stampante</summary>
            public String sPort;
        };

        /// <summary>
        /// indica il gruppo di stampa
        /// </summary>
        public enum DEST_TYPE
        {
            /// <summary>gruppo generico es:primi</summary>
            DEST_TIPO1 = 0,
            /// <summary>gruppo generico es:secondi</summary>
            DEST_TIPO2,
            /// <summary>gruppo generico es:contorni</summary>
            DEST_TIPO3,
            /// <summary>gruppo generico es:analcolici</summary>
            DEST_TIPO4,
            /// <summary>gruppo generico es:alcolici</summary>
            DEST_TIPO5,
            /// <summary>gruppo generico es:dessert</summary>
            DEST_TIPO6,
            /// <summary>gruppo generico es:panini</summary>
            DEST_TIPO7,
            /// <summary>gruppo generico es:caffetteria</summary>
            DEST_TIPO8,
            /// <summary>gruppo generico es: GRUPPO 9 (no-web)</summary>
            DEST_TIPO9_NOWEB,
            /// <summary>gruppo destinazioni singole</summary>
            DEST_SINGLE,
            /// <summary>gruppo dei contatori: non ha un prezzo significativo</summary>
            DEST_COUNTER,
            /// <summary>gruppo dei buoni sconto: sono articoli con prezzo negativo</summary>
            DEST_BUONI
        };

        /// <summary>numero totale =12 dei diversi gruppi di Articoli compresi i contatori</summary>
        public static readonly int NUM_COPIES_GRPS = Enum.GetNames(typeof(DEST_TYPE)).Length;

        /// <summary>
        /// numero=10 dei gruppi di stampa separata, comprese copie sigole ma esclusi i contatori<br/>
        /// corrisponde anche all'indice della stampante Locale
        /// </summary>
        public static readonly int NUM_SEP_PRINT_GROUPS = NUM_COPIES_GRPS - 2;

        /// <summary>numero=14 del "gruppo di stampa virtuale" che identifica barcode di prevendita</summary>
        public static readonly int NUM_PRE_SALE_GRP = 14;

        /// <summary>numero=15 del "gruppo di stampa virtuale" che identifica barcode di vendita WEB</summary>
        public static readonly int NUM_WEB_SALE_GRP = 15;

        /// <summary>numero=9 dei gruppi di stampa con label editabile, sono i primi 9</summary>
        public static readonly int NUM_EDIT_GROUPS = (int)DEST_TYPE.DEST_TIPO9_NOWEB + 1;

        /// <summary>numero=5 dei colori dei gruppi di stampa: 0 corrisponde al trasparente</summary>
        public static readonly int NUM_GROUPS_COLORS = 5;

        /****************************************************************
         *              Flags di di stato gestione Ordine               *
         ****************************************************************/

        /// <summary>Flags  di Stato</summary>
        public enum STATUS_FLAGS
        {
            /// <summary>bit di iStatus che indica l'asporto</summary>
            BIT_ASPORTO = 0,

            /// <summary>bit di iStatus che indica Scontrino emesso durante la prevendita</summary>
            BIT_EMESSO_IN_PREVENDITA,

            /// <summary>bit di iStatus che indica Scontrino caricato da una prevendita</summary>
            BIT_CARICATO_DA_PREVENDITA,

            /// <summary>bit di iStatus che indica Scontrino caricato da web,<br/>
            /// settato da CaricaOrdineWeb, CaricaOrdine_QR_code
            /// </summary>
            BIT_CARICATO_DA_WEB,

            /// <summary>bit di iStatus che indica Scontrino generato direttamente da web</summary>
            BIT_ORDINE_DIRETTO_DA_WEB,

            /// <summary>bit di iStatus che indica che lo Scontrino è stato stampato da Stand Cucina</summary>
            BIT_RECEIPT_STAMPATO_DA_STANDCUCINA,

            /// <summary>bit di iStatus che indica che il messaggio è stato stampato da Stand Cucina</summary>
            BIT_MSG_STAMPATO_DA_STANDCUCINA,


            /// <summary>
            /// bit di iStatus che indica il pagamento di default mediante contanti<br/>
            /// serve utilizzarlo come LSB dei tipi di pagamento
            /// </summary>
            BIT_PAGAM_CASH = 10,

            /// <summary>
            /// bit di iStatus che indica il pagamento mediante CARD:<br/>
            /// bancomat, carta di credito
            /// </summary>
            BIT_PAGAM_CARD,

            /// <summary>bit di iStatus che indica il pagamento mediante Satispay</summary>
            BIT_PAGAM_SATISPAY
        }

        /****************************************************************
         *                Flags generali di StandFacile                 *
         ****************************************************************/

        /// <summary>Flags generali sul comportamento di StandFacile</summary>
        public enum GEN_PROGRAM_OPTIONS
        {
            /// <summary>bit di iGeneralProgOptions per gestione Tablet Mode</summary>
            BIT_TOUCH_MODE_REQUIRED = 0,

            /// <summary>bit di iGeneralProgOptions per obbligare ad indicazione del Tavolo ante emissione Receipt</summary>
            BIT_TABLE_REQUIRED,

            /// <summary>bit di iGeneralProgOptions per obbligare ad indicazione dei Coperti ante emissione Receipt</summary>
            BIT_PLACE_SETTINGS_REQUIRED,

            /// <summary>bit di iGeneralProgOptions per obbligare ad indicazione del Pagamento ante emissione Receipt</summary>
            BIT_PAYMENT_REQUIRED,

            /// <summary>bit di iGeneralProgOptions per consentire Articoli con Prezzo = zero</summary>
            BIT_ZERO_PRICE_ITEMS_ALLOWED,

            /// <summary>bit di iGeneralProgOptions per consentire la riservatezza</summary>
            BIT_PRIVACY,

            /// <summary>bit di iGeneralProgOptions per consentire la stampa con ENTER</summary>
            BIT_ENTER_PRINT_RECEIPT_ENABLED
        }

        /****************************************************************
         *     Flags generali di gestione della Stampante Generica      *
         ****************************************************************/
        /// <summary>Flags generali di gestione della Stampante Generica</summary>
        public enum GEN_PRINTER_OPTS
        {
            /// <summary>bit di iGeneralPrinterOptions per gestione righe vuote iniziali</summary>
            BIT_EMPTY_ROWS_INITIAL = 0, // 0-3 sono occupati da _iInitialRowsToAdd
            /// <summary>bit di iGeneralPrinterOptions per gestione righe vuote finali</summary>
            BIT_EMPTY_ROWS_FINAL = 4, // 4-7 sono occupati da _finalRowsToAdd

            /// <summary>bit di iGeneralPrinterOptions per gestione stampa numero cassa inline con il numero della Receipt</summary>
            BIT_CASSA_INLINE = 8,
            /// <summary>bit di iGeneralPrinterOptions per gestione stampa con asterischi sopra e sotto il gruppo</summary>
            BIT_STARS_ON_UNDER_GROUP,
            /// <summary>bit di iGeneralPrinterOptions per gestione stampa con tavolo e nome centrati</summary>
            BIT_CENTER_TABLE_AND_NAME,

            /// <summary>bit di iGeneralPrinterOptions per gestione stampa del Logo anche nelle copie</summary>
            BIT_LOGO_PRINT_ON_COPIES_REQUIRED,

            /// <summary>bit di iGeneralPrinterOptions per gestione stampa dei coperti anche nelle copie</summary>
            BIT_PLACESETTS_PRINT_ON_COPIES_REQUIRED,

            /// <summary>bit di iGeneralPrinterOptions per gestione stampa a 33 caratteri deli Articoli</summary>
            BIT_CHARS33_PRINT_REQUIRED = 16 

        }

        /****************************************************************
         *                Flags gestione stampa Copie Locali            *
         ****************************************************************/

        /// <summary>Flags per gestione stampa Copie Locali</summary>
        public enum LOCAL_COPIES_OPTS
        {
            /// <summary>bit di iLocalCopyOptions per gestione stampa con taglio nella copia locale Receipt</summary>
            BIT_PRINT_GROUPS_CUT_REQUIRED = 10, // 0-9 è occupato da _pCheckBoxCopia[i]

            /// <summary>bit di iLocalCopyOptions per gestione stampa Articoli con quantità uno nella copia locale Receipt</summary>
            BIT_QUANTITYONE_PRINT_REQUIRED,

            /// <summary>bit di iLocalCopyOptions per gestione stampa di un solo Articolo nella copia locale Receipt</summary>
            BIT_SINGLEROWITEMS_PRINT_REQUIRED,

            /// <summary>bit di iLocalCopyOptions per gestione stampa solo dei gruppi selezionati</summary>
            BIT_SELECTEDONLY_PRINT_REQUIRED,

            /// <summary>bit di iLocalCopyOptions per gestione stampa copia locale dello scontrino</summary>
            BIT_RECEIPT_LOCAL_COPY_REQUIRED,

            /// <summary>bit di iLocalCopyOptions per stampa gruppi contemporanea</summary>
            BIT_AVOIDPRINTGROUPS_PRINT_REQUIRED,

            /// <summary>bit di iLocalCopyOptions per chiedere la stampa dei prezzi anche nelle copie locali</summary>
            BIT_PRICE_PRINT_ON_COPIES_REQUIRED

            /*******************************************************************************************
	            i primi 10 bit di iLocalCopyOptions sono riservati per gestione _bSelectedGroups[i]
	         *******************************************************************************************/
        }

        /// <summary>indica il tipo di sconto</summary>
        public enum DISC_TYPE
        {
            /// <summary>nessuno</summary>
            DISC_NONE = 0,
            /// <summary>percentuale</summary>
            DISC_STD,
            /// <summary>fisso</summary>
            DISC_FIXED,
            /// <summary>gratis</summary>
            DISC_GRATIS
        }

        /// <summary>numero=3 dei diversi tipi di sconto</summary>
        public static readonly int NUM_DISC_TYPE = Enum.GetNames(typeof(DISC_TYPE)).Length;

        /// <summary>
        /// struct per la gestione dei parametri della
        /// stampante windows (USB o RETE) dotata di driver,
        /// </summary>
        [Serializable()]
        public struct TWinPrinterParams
        {
            /// <summary>flag per stampa su carta A4</summary>
            public bool bA4Paper;
            /// <summary>flag per stampa su carta A5</summary>
            public bool bA5Paper;

            /// <summary>modello di stampante windows per lo scontrino</summary>
            public String sTckPrinterModel;
            /// <summary>modello associato alla stampante windows per lo scontrino</summary>
            public int iTckPrinterModel;

            /// <summary>modello di stampante windows per il messaggio</summary>
            public String sMsgPrinterModel;
            /// <summary>modello associato alla stampante windows per il messaggio</summary>
            public int iMsgPrinterModel;

            /// <summary>modello di stampante windows</summary>
            public String[] sPrinterModel;
            /// <summary>numero associato alla stampante windows</summary>
            public int[] iPrinterModel;

            /// <summary>nome del file del logo Top</summary>
            public String sLogoName_T;
            /// <summary>larghezza logo Top</summary>
            public int iLogoWidth_T;
            /// <summary>altezza logo Top</summary>
            public int iLogoHeight_T;

            /// <summary>nome del file del logo Bottom</summary>
            public String sLogoName_B;
            /// <summary>larghezza logo Bottom</summary>
            public int iLogoWidth_B;
            /// <summary>altezza logo Bottom</summary>
            public int iLogoHeight_B;

            /// <summary>tipo di font per lo scontrino</summary>
            public String sTckFontType;
            /// <summary>margine sinistro per lo scontrino</summary>
            public int iTckLeftMargin;
            /// <summary>dimensione font per lo scontrino</summary>
            public float fTckFontSize;
            /// <summary>stile font per lo scontrino</summary>
            public FontStyle sTckFontStyle;

            /// <summary>tipo di font per i reports</summary>
            public String sRepFontType;
            /// <summary>margine sinistro per i reports</summary>
            public int iRepLeftMargin;
            /// <summary>dimensione font per i reports</summary>
            public float fRepFontSize;
            /// <summary>stile font per i reports</summary>
            public FontStyle sRepFontStyle;

            /// <summary>valore di Zoom per stampa Receipt in %</summary>
            public int iTckZoomValue;
            /// <summary>valore di Zoom per stampa Reports in %</summary>
            public int iRepZoomValue;
            /// <summary>valore di Zoom per stampa Logo in %</summary>
            public int iLogoZoomValue;
            /// <summary>valore per centraggio Logo</summary>
            public int iLogoCenter;

            /// <summary>costruttore</summary>
            public TWinPrinterParams(int i)
            {
                bA4Paper = false;
                bA5Paper = false;

                sTckPrinterModel = "";
                iTckPrinterModel = 0;
                sMsgPrinterModel = "";
                iMsgPrinterModel = 0;

                sPrinterModel = new String[NUM_SEP_PRINT_GROUPS];
                iPrinterModel = new int[NUM_SEP_PRINT_GROUPS];

                sLogoName_T = "";
                iLogoWidth_T = 0;
                iLogoHeight_T = 0;

                sLogoName_B = "";
                iLogoWidth_B = 0;
                iLogoHeight_B = 0;

                sTckFontType = "";
                iTckLeftMargin = 0;
                fTckFontSize = 11;
                sTckFontStyle = FontStyle.Regular;

                sRepFontType = "";
                iRepLeftMargin = 0;
                fRepFontSize = 9;
                sRepFontStyle = FontStyle.Regular;

                iTckZoomValue = 100;
                iRepZoomValue = 100;
                iLogoZoomValue = 100;

                iLogoCenter = 0;

                Console.WriteLine("TWinPrinterParams : costruttore {0}", i);
            }
        };

        /// <summary> valori massimi</summary>
        public const int DISP_OK = 99999;


        /****************************************************************
         *                Flags gestione stampa Articolo                *
         ****************************************************************/

        /// <summary>
        /// bit di iOptionsFlags che indica la richiesta di stampa singola nella<br/>
        /// copia locale dello scontrino
        /// </summary>
        public const int BIT_STAMPA_SINGOLA_NELLA_COPIA_RECEIPT = 0;

        /****************************************************************
         *                   Flags gestione sconti                      *
         ****************************************************************/

        /// <summary>bit di iStatusSconto che indica l'applicazione dello sconto per gli articoli</summary>
        public const int BIT_SCONTO_STD = 0;

        /// <summary>bit di iStatusSconto che indica l'applicazione dello sconto parziale</summary>
        public const int BIT_SCONTO_FISSO = 1;

        /// <summary>bit di iStatusSconto che indica l'applicazione dello sconto totale</summary>
        public const int BIT_SCONTO_GRATIS = 2;

        /****************************************************************
         *               Flags gestione flag di avvio                   *
         ****************************************************************/

        /// <summary>bit che indica la visualizzazione del Dialogo di gestione disponibilità</summary>
        public const int BIT_SHOW_DISP_DLG = 0;

        /// <summary>bit che indica il caricamento della disponibilità precedente</summary>
        public const int BIT_PREV_DISP_LOAD = 4;


        /// <summary>numero max delle righe di Header-Footer</summary>
        public const int MAX_NUM_HEADERS = 4;

        /// <summary>numero massimo di pagine della griglia in modo testo</summary>
        public const int PAGES_NUM_TXTM = 4;
        /// <summary>numero massimo di pagine della griglia in modo Touch</summary>
        public const int PAGES_NUM_TABM = 5;

        /// <summary>numero massimo di righe della griglia in modo testo</summary>
        public const int MAX_GRID_NROWS_TXTM = 25;
        /// <summary>numero massimo di colonne della griglia in modo testo</summary>
        public const int MAX_GRID_NCOLS_TXTM = 4;

        /// <summary>numero massimo di righe della griglia in modo Touch</summary>
        public const int MAX_GRID_NROWS_TABM = 4;
        /// <summary>numero massimo di colonne della griglia in modo Touch</summary>
        public const int MAX_GRID_NCOLS_TABM = 5;

        /// <summary>
        /// numero di default di righe della griglia<br/>
        /// possibili valori 16,20,25
        /// </summary>
        public const int DEF_GRID_NROWS = 20;
        /// <summary>
        /// numero di default di colonne della griglia<br/>
        /// possibili valori 3,4
        /// </summary>
        public const int DEF_GRID_NCOLS = 3;

        /// <summary>
        /// massimo numero di struct TArticolo allocate, corrisponde al numero di celle della MainGrid <br/>
        /// x il numero delle pagine, + 1 per COPERTI
        /// </summary>
        public const int MAX_NUM_ARTICOLI = MAX_GRID_NROWS_TXTM * MAX_GRID_NCOLS_TXTM * PAGES_NUM_TXTM + 1; //

        /// <summary>
        /// margine per non sforare in dbCaricaDatidaOrdini() con modifiche al Listino nella stessa sessione
        /// </summary>
        public const int EXTRA_NUM_ARTICOLI = 50;

        /// <summary>
        /// margine per non sforare l'array nel caricamento di Listino a causa dell'header
        /// </summary>
        public const int EXTRA_NUM_LISTINO_HEAD = 50;

        /// <summary>
        /// definizione della struttura fondamentale dell'Articolo
        /// </summary>
        [Serializable()]
        public struct TArticolo
        {
            /// <summary>flag per gestione stampa locale</summary>
            public bool bLocalPrinted;
            /// <summary>prezzo di una vaschetta di Patatine</summary>
            public int iPrezzoUnitario;
            /// <summary> usato solo dal DB per indicara la Q.tà consegnata</summary>
            public int iQuantita_Scaricata;
            /// <summary>Q.tà di vaschette di Patatine dello scontrino</summary>
            public int iQuantitaOrdine;
            /// <summary>indice del Listino, solo per gruppo copie singole, serve a costruire i files necessari</summary>
            public int iIndexListino;
            /// <summary>Q.tà vaschette di Patatine ancora disponibili in cucina</summary>
            public int iDisponibilita;
            /// <summary>intero che descrive il gruppo di stampa</summary>
            public int iGruppoStampa;
            /// <summary>intero che descrive alcuni Flag per la stampa a livello di Articolo</summary>
            public int iOptionsFlags;
            /// <summary>Q.tà totale di vaschette di Patatine vendute</summary>
            public int iQuantitaVenduta;
            /// <summary>Q.tà totale esportata</summary>
            public int iQtaEsportata;
            /// <summary>Es: Patatine</summary>
            public String sTipo;
            /// <summary>Es: Nota</summary>
            public String sNotaArt;

            /// <summary>costruttore</summary>
            public TArticolo(int iParam)
            {
                bLocalPrinted = false;
                iPrezzoUnitario = 0;
                iQuantita_Scaricata = 0;
                iQuantitaOrdine = 0;
                iIndexListino = 0;
                iGruppoStampa = 0;
                iOptionsFlags = 0;
                iQuantitaVenduta = 0;
                iQtaEsportata = 0;
                iDisponibilita = DISP_OK;
                sTipo = "";
                sNotaArt = "";
            }

            /// <summary>costruttore</summary>
            public TArticolo(TArticolo TArticoloParam)
            {
                bLocalPrinted = TArticoloParam.bLocalPrinted;
                iPrezzoUnitario = TArticoloParam.iPrezzoUnitario;
                iQuantita_Scaricata = TArticoloParam.iQuantita_Scaricata;
                iQuantitaOrdine = TArticoloParam.iQuantitaOrdine;
                iIndexListino = TArticoloParam.iIndexListino;
                iGruppoStampa = TArticoloParam.iGruppoStampa;
                iOptionsFlags = TArticoloParam.iOptionsFlags;
                iQuantitaVenduta = TArticoloParam.iQuantitaVenduta;
                iQtaEsportata = TArticoloParam.iQtaEsportata;
                iDisponibilita = TArticoloParam.iDisponibilita;
                sTipo = TArticoloParam.sTipo;
                sNotaArt = TArticoloParam.sNotaArt;
            }
        };

        /// <summary>
        /// definizione della struttura fondamentale
        /// c'è tutto l'occorrente per gestire la produzione di uno scontrino
        /// </summary>
        [Serializable()]
        public struct TData
        {
            /// <summary>flag per gestione del modo Prevendita attiva, salvato nel Listino</summary>
            public bool bPrevendita;
            /// <summary>flag di stato = scaricato usato con MySQL, PostGreSQL</summary>
            public bool bScaricato;
            /// <summary>flag di stato = stampato usato per ordini web</summary>
            public bool bStampato;
            /// <summary>flag di stato = annullato</summary>
            public bool bAnnullato;

            /// <summary>numero della cassa, parte da 1</summary>
            public int iNumCassa;
            /// <summary>intero i cui bit rappresentano lo stato</summary>
            public int iStatusReceipt;
            /// <summary>valore dello sconto percentuale applicato</summary>
            public int iScontoStdReceipt;
            /// <summary>valore dello sconto fisso applicato</summary>
            public int iScontoFissoReceipt;
            /// <summary>valore dello sconto gratis applicato</summary>
            public int iScontoGratisReceipt;
            /// <summary>valore dei buoni applicati</summary>
            public int iBuoniApplicatiReceipt;
            /// <summary>numero hex i cui bit rappresentano lo stato degli sconti</summary>
            public int iStatusSconto;
            /// <summary>testo dello sconto applicato</summary>
            public String sScontoText;
            /// <summary>numero hex per gestione barcode nelle copie</summary>
            public int iBarcodeRichiesto;
            /// <summary>numero hex per gestione delle opzioni generali di StandFacile</summary>
            public int iGeneralProgOptions;
            /// <summary>numero hex per gestione delle opzioni generiche della stampa</summary>
            public int iGenericPrintOptions;
            /// <summary>numero hex per gestione della stampa copie locali e copie con quantità Uno</summary>
            public int iLocalCopyOptions;
            /// <summary>numero di colonne della griglia</summary>
            public int iGridCols;
            /// <summary>numero di righe della griglia</summary>
            public int iGridRows;
            /// <summary>numero progressivo dell'ultimo scontrino emesso</summary>
            public int iNumOfLastReceipt;
            /// <summary>numero iniziale degli scontrini emessi</summary>
            public int iStartingNumOfReceipts;
            /// <summary>numero effettivo degli scontrini emessi</summary>
            public int iActualNumOfReceipts;
            /// <summary>numero progressivo degli scontrini emessi tramite QRcode</summary>
            public int iNumOfWebReceipts;
            /// <summary>numero di scontrini annullati</summary>
            public int iNumAnnullati;
            /// <summary>numero progressivo dei messaggi emessi</summary>
            public int iNumOfMessages;
            /// <summary>importo dello scontrino corrente</summary>
            public int iTotaleReceipt;
            /// <summary>importo dovuto dello scontrino corrente</summary>
            public int iTotaleReceiptDovuto;
            /// <summary>incasso totale</summary>
            public int iTotaleIncasso;
            /// <summary>incasso totale CARD</summary>
            public int iTotaleIncassoCard;
            /// <summary>incasso totale Satispay</summary>
            public int iTotaleIncassoSatispay;
            /// <summary>somma importo degli scontrini scontati in percentuale</summary>
            public int iTotaleScontatoStd;
            /// <summary>somma importo degli scontrini con sconto fisso</summary>
            public int iTotaleScontatoFisso;
            /// <summary>somma importo degli scontrini gratuiti</summary>
            public int iTotaleScontatoGratis;
            /// <summary>somma importo dei buoni applicati</summary>
            public int iTotaleBuoniApplicati;
            /// <summary>somma importo degli scontrini annullati </summary>
            public int iTotaleAnnullato;
            /// <summary>stringa del tavolo</summary>
            public String sTavolo;
            /// <summary>stringa del nome utente</summary>
            public String sNome;
            /// <summary>stringa della nota</summary>
            public String sNota;
            /// <summary>testo del messaggio</summary>
            public String sMessaggio;
            /// <summary>stringa di versione</summary>
            public String sVersione;
            /// <summary>stringa di data ed ora</summary>
            public String sDateTime;
            /// <summary>stringa di data ed ora per la Prevendita</summary>
            public String sPrevDateTime;
            /// <summary>stringa di data ed ora per gli ordini web</summary>
            public String sWebDateTime;
            /// <summary>stringa di data ed ora letta dal Listino</summary>
            public String sListinoDateTime;
            /// <summary>testo per il checksum listino prezzi negli ordini web</summary>
            public String sPL_Checksum;

            /// <summary>testo array degli headers</summary>
            public String[] sHeaders;
            /// <summary>testo array della TAbs della griglia</summary>
            public String[] sPageTabs;

            /// <summary>flag per la stampa delle copie</summary>
            public bool[] bCopiesGroupsFlag;
            /// <summary>testo per la stampa delle copie</summary>
            public String[] sCopiesGroupsText;

            /// <summary>colore per il raggruppamento della stampa delle copie</summary>
            public int[] iGroupsColor;
            /// <summary>testo per la stampa delle copie in base ai colori</summary>
            public String[] sColorGroupsText;

            /// <summary>numero dell'ordine in prevendita</summary>
            public int iNumOrdinePrev;

            /// <summary>numero dell'ordine remoto</summary>
            public int iNumOrdineWeb;

            /// <summary>array degli articoli</summary>
            public TArticolo[] Articolo;

            /// <summary>costruttore</summary>
            public TData(int iParam)
            {
                bPrevendita = false;
                bScaricato = false;
                bStampato = false;
                bAnnullato = false;

                iNumCassa = 0;
                iStatusReceipt = 0;
                iScontoStdReceipt = 0;
                iScontoFissoReceipt = 0;
                iScontoGratisReceipt = 0;
                iBuoniApplicatiReceipt = 0;
                iStatusSconto = 0;
                sScontoText = "";
                iBarcodeRichiesto = 0;
                iGeneralProgOptions = 0;
                iGenericPrintOptions = 0;
                iLocalCopyOptions = 0;
                iGridCols = 0;
                iGridRows = 0;
                iNumOfLastReceipt = 0;
                iStartingNumOfReceipts = 0;
                iActualNumOfReceipts = 0;
                iNumOfMessages = 0;
                iNumOfWebReceipts = 0;
                iTotaleReceipt = 0;
                iTotaleReceiptDovuto = 0;
                iTotaleIncasso = 0;
                iTotaleIncassoCard = 0;
                iTotaleIncassoSatispay = 0;
                iTotaleScontatoStd = 0;
                iTotaleScontatoFisso = 0;
                iTotaleScontatoGratis = 0;
                iTotaleBuoniApplicati = 0;
                iTotaleAnnullato = 0;
                iNumAnnullati = 0;

                sTavolo = "";
                sNome = "";
                sNota = "";
                sMessaggio = "";
                sVersione = "";
                sDateTime = "";
                sPrevDateTime = "";
                sWebDateTime = "";
                sListinoDateTime = "";
                sPL_Checksum = "";

                sHeaders = new String[MAX_NUM_HEADERS];
                sPageTabs = new String[PAGES_NUM_TABM];

                sCopiesGroupsText = new String[NUM_COPIES_GRPS]; // deve contenere anche stringhe per i contatori
                bCopiesGroupsFlag = new bool[NUM_SEP_PRINT_GROUPS];

                iGroupsColor = new int[NUM_EDIT_GROUPS];
                sColorGroupsText = new String[NUM_GROUPS_COLORS - 1];
                iNumOrdinePrev = 0;
                iNumOrdineWeb = 0;

                Articolo = new TArticolo[MAX_NUM_ARTICOLI + EXTRA_NUM_ARTICOLI];

                Console.WriteLine("TData : costruttore {0}", iParam);
            }
        };

        /// <summary>
        /// enum per lo stato delle COM di stampa
        /// </summary>
        public enum COM_STATUS
        {
            /// <summary>COM libera</summary>
            COM_FREE,
            /// <summary>COM in uso da questo processo</summary>
            COM_BUSY,
            /// <summary>COM in uso da altro processo</summary>
            COM_NOT_FREE
        };

        /// <summary>
        /// enum per gestione della coda di stampa
        /// </summary>
        public enum PRINT_QUEUE_ACTION
        {
            /// <summary>stampa immediata</summary>
            PRINT_NOW = '0',
            /// <summary>inserimento in coda</summary>
            PRINT_ENQUEUE,
            /// <summary>avvio stampa della coda</summary>
            PRINT_START
        };

        /// <summary>
        /// enum per descrivere il file da visualizzare
        /// </summary>
        public enum FILE_TO_SHOW
        {
            /// <summary>obbliga file dati</summary>
            FILE_DATI = '0',
            /// <summary>obbliga file Listino</summary>
            FILE_PREZZI,
            /// <summary>scelta automatica</summary>
            FILE_AUTO
        };

        /// <summary>numero minimo di righe per scontrino e copie</summary>
        public const int MIN_RECEIPT_ROWS_NUMBER = 15; // con 1,2,3 Articoli la stampa è lunga uguale

        /// <summary>enum per ricerca Scontrino e messaggi</summary>
        public enum SEARCH_TYPE
        {
            /// <summary>nessuna ricerca</summary>
            NO_SEARCH,
            /// <summary>ricerca in basso nel DB</summary>
            SEARCH_DOWN,
            /// <summary>ricerca in alto nel DB</summary>
            SEARCH_UP
        };

        /// <summary>enum per visualizzazione Scontrino e messaggi</summary>
        public enum VIEW_TYPE
        {
            /// <summary>normale</summary>
            NORMAL,
            /// <summary>cambia il tipo di pagamento</summary>
            CHANGE_PAYMENT,
            /// <summary>cancella l'ordine</summary>
            CANCEL_ORDER,
            /// <summary>nessuna visualizzazione</summary>
            NO_VIEW
        };

        /// <summary>const di cassa principale, necessario partire da 1</summary>
        public const int CASSA_PRINCIPALE = 1;

        /// <summary>max numero di casse secondarie</summary>
        public const int MAX_CASSE_SECONDARIE = 5;

        /// <summary>testo descrittivo del punto di taglio</summary>
        public static readonly String _CUT = "#####   CUT   #####";

        /// <summary>testo descrittivo del punto di inserimento Logo per compatibilità</summary>
        public static readonly String _LOGO = "#####   LOGO   #####";

        /// <summary>testo descrittivo del punto di inserimento Logo Top</summary>
        public static readonly String _LOGO_T = "#####  LOGO_T  #####";

        /// <summary>testo descrittivo del punto di inserimento Logo Bottom</summary>
        public static readonly String _LOGO_B = "#####  LOGO_B  #####";

        /// <summary>testo descrittivo del punto di inserimento Barcode</summary>
        public static readonly String _BARCODE = "#####  BARCODE  #####";

        /// <summary>testo descrittivo del formato per punto di taglio</summary>
        public static readonly String _CUT_FMT = "{0}\r\n\r\n";

        /// <summary>testo descrittivo dei coperti</summary>
        public static readonly String _COPERTO = "COPERTI";

        /// <summary>testo descrittivo delle 6 casse</summary>
        public static readonly String[] sConstCassaType = { "Cassa n.1 Principale", "Cassa n.2 Secondaria", "Cassa n.3 Secondaria",
                                                            "Cassa n.4 Secondaria", "Cassa n.5 Secondaria", "Cassa n.6 Secondaria" };

        /// <summary>testo descrittivo dei gruppi di stampa</summary>
        public static readonly String[] sConstGruppi = { "gruppo 1", "gruppo 2", "gruppo 3", "gruppo 4", "gruppo 5", "gruppo 6",
                                                          "gruppo 7", "gruppo 8", "gruppo 9 (no-web)", "Copie singole", "Contatori", "Buoni sconto" };

        /// <summary>testo short descrittivo dei gruppi di stampa</summary>
        public static readonly String[] sConstGruppiShort = { "G1", "G2", "G3", "G4", "G5", "G6", "G7", "G8", "G9", "CS", "CN", "BS" };

        /// <summary>testo esteso descrittivo dei gruppi di stampa</summary>
        public static readonly String[] sConstCopiesGroupsText =
        {
            "##### COPIA GRUPPO1 #####",
            "##### COPIA GRUPPO2 #####",
            "##### COPIA GRUPPO3 #####",
            "##### COPIA GRUPPO4 #####",
            "##### COPIA GRUPPO5 #####",
            "##### COPIA GRUPPO6 #####",
            "##### COPIA GRUPPO7 #####",
            "##### COPIA GRUPPO8 #####",
            "### COPIA GRP NO WEB  ###",
            "###   COPIE SINGOLE   ###",
            "###     CONTATORI     ###",
            "###    BUONI SCONTO   ###"
        };

        /// <summary>testo esteso descrittivo dei gruppi di stampa raggruppati per colore</summary>
        public static readonly String[] sConstColorsGroupsText =
        {
            "##### COPIA CUCINA  #####",
            "##### COPIA BIBITE  #####",
            "#####  COPIA GEN 3  #####",
            "#####  COPIA GEN 4  #####"
        };

        /// <summary>testo descrittivo della Esportazione</summary>
        public static readonly String[] sConst_Asporto =
        {
            "########################",
            "####   DA ASPORTO   ####",
            "########################"
        };

        /// <summary>testo descrittivo della Nota</summary>
        public static readonly String[] sConst_Nota =
        {
            "======= annotazione =======",
            "==========================="
        };

        /// <summary>testo descrittivo degli Sconti</summary>
        public static readonly String[] sConst_Sconti =
        {
            "== SCONTO ART. APPLICATO ==",
            "== SCONTO FISSO APPLICATO =",
            "=== SCONTRINO  GRATUITO ===",
            "==========================="
        };

        /// <summary>testo descrittivo dell'annullo</summary>
        public static readonly String[] sConst_Annullo =
        {
            "###########################",
            "### SCONTRINO ANNULLATO ###",
            "###########################"
        };

        /// <summary>testo descrittivo della Prevendita</summary>
        public static readonly String[] sConst_Prevendita =
        {
            "########################",
            "####  PRE-VENDITA   ####",
            "########################"
        };

        /// <summary>testo descrittivo del tipo di pagamento CARD</summary>
        public static readonly String sConst_Pagamento_da_EFFETTUARE = "@@@ PAGAM. DA EFFETTUARE @@@";

        /// <summary>testo descrittivo del tipo di pagamento CARD</summary>
        public static readonly String sConst_Pagamento_CARD = "#### PAGAMENTO-CARD ####";

        /// <summary>testo descrittivo del tipo di pagamento SATISPAY</summary>
        public static readonly String sConst_Pagamento_Satispay = "## PAGAMENTO-SATISPAY ##";

        /// <summary>testo descrittivo dei tipi di pagamento</summary>
        public static readonly String[] sConst_PaymentType =
        {
            "da effettuare",
            "Contanti",
            "Card",
            "Satispay",
            "   "
        };

        /// <summary>definizione della struttura per l'accesso al database remoto</summary>
        public struct TWebServerParams
        {
            /// <summary>prefisso tabella remota</summary>
            public String sWebTablePrefix;
            /// <summary>nome database remoto</summary>
            public String sWeb_DBase;
            /// <summary>password criptata database remoto</summary>
            public String sWebEncryptedPwd;

            /// <summary>costruttore indispensabile per inizializzare le stringhe</summary>
            public TWebServerParams(int iParam)
            {
                sWebTablePrefix = "";
                sWeb_DBase = "";
                sWebEncryptedPwd = "";
            }
        };

        /// <summary>definizione della struttura per i test di accesso al tipo di database selezionato in NetConfigDlg</summary>
        public struct TWebServerCheckParams
        {
            /// <summary>tipo di database di test</summary>
            public int iDB_mode;
            /// <summary>nome database di test</summary>
            public String sDB_ServerName;
            /// <summary>password database di test</summary>
            public String sDB_pwd;


            /// <summary>costruttore per inizializzazione</summary>
            public TWebServerCheckParams(int iParam)
            {
                iDB_mode = 0;
                sDB_ServerName = "";
                sDB_pwd = "";
            }
        };

        /// <summary>struct della stringhe per gestione ordine</summary>
        public struct TOrdineStrings
        {
            /// <summary>numero dell'ordine</summary>
            public String sOrdineNum;
            /// <summary>numero del tavolo</summary>
            public String sTavolo;
            /// <summary>nome del cliente</summary>
            public String sNome;
            /// <summary>numero dell'ordine Web</summary>
            public String sOrdNumWeb;
            /// <summary>numero dell'ordine ni Prevendita</summary>
            public String sOrdNumPrev;
        }

        /// <summary>struct per gestione configurazione da File</summary>
        public struct TConfig
        {
            /// <summary>flag per comunicare l'avvenuto caricamento del file config.ini</summary>
            public bool bFileLoaded;
            /// <summary>numero di avvio conteggio scontrini, normalmente = 1</summary>
            public int iReceiptStartNumber;
            /// <summary>numero di secondi del Timer di refresh, normalmente = 30s per StandMonitor</summary>
            public int iRefreshTimer;
            /// <summary>stringa per gestione Service Mode</summary>
            public String sService;

            /// <summary>flag per richiesta stampa copia Receipt</summary>
            public bool bRcpCopyRequired;
            /// <summary>stringa per testo descrittivo della copia Receipt</summary>
            public String sRcpCopyHeader;

            /// <summary>stringa per eventuale nuovo nome del database</summary>
            public String sDatabaseName;

            /// <summary>stringa per eventuale nuovo nome dell'utente del database</summary>
            public String sDatabaseUser;

            /// <summary>
            /// stringa di versione della pagina web per StandOrdiniWeb,
            /// non è detto che sia la stessa delle RELEASE_DB_TBLS, può essere sovrascritta
            /// </summary>
            public String sWebUrlVersion;

            /// <summary>stringa per testo intestazione/piè di pagina alternativo per la CASSA_SECONDARIA</summary>
            public String[] sRcp_CS_Header;

            /// <summary>costruttore per inizializzazione</summary>
            public TConfig(int iParam)
            {
                bFileLoaded = false;
                iReceiptStartNumber = 0;
                iRefreshTimer = 0;
                sService = "";
                bRcpCopyRequired = false;
                sRcpCopyHeader = "";
                sDatabaseName = "";
                sDatabaseUser = "";
                sWebUrlVersion = "v5c";
                sRcp_CS_Header = new String[4];
            }
        }

    } // end struct
} // end namespace


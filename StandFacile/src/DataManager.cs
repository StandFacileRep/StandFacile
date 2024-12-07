/**********************************************************************
     NomeFile : StandFacile/DataManager.cs
     Data	  : 19.09.2024
     Autore   : Mauro Artuso

     nb: DB_Data compare sempre a destra nelle assegnazioni
 **********************************************************************/

using System;
using System.IO;
using static System.Convert;
using System.Collections.Generic;

using static StandFacile.glb;
using static StandFacile.Define;
using static StandFacile.dBaseIntf;
using static StandFacile.ScontoDlg;
using static StandFacile.FrmMain;

using StandCommonFiles;
using static StandCommonFiles.ComDef;
using static StandCommonFiles.commonCl;
using static StandCommonFiles.ReceiptAndCopies;
using static StandCommonFiles.LogServer;

namespace StandFacile
{
    /// <summary>
    ///  Classe da modificare con accuratezza per assicurare la correttezza dei conti economici,<br/>
    ///  contiene i metodi per la gestione dei seguenti files :<br/><br/>
    ///  
    ///  Listino.txt File prezzi<br/>
    ///  Cx_Dati'mmdd'.txt File dati di riepilogo giornaliero<br/>
    ///  Cx_TTyyyy.txt scontrino completo<br/>
    ///  Cx_CTyyyy_Gz.txt copia di riepilogo per gruppi: pietanze, bibite, etc<br/>
    ///  legenda: x è il numero della cassa, mm il mese, dd il giorno,<br/>
    ///          yyyy è un numero progressivo, z il gruppo di articoli<br/>
    /// </summary>
    public partial class DataManager
    {
        static bool _bListinoModificato;
        static bool _bChecksumListinoCoerente;

        static bool[] _bSomethingInto_GrpToPrint = new bool[NUM_COPIES_GRPS];
        static bool[] _bSomethingInto_ClrToPrint = new bool[NUM_COPIES_GRPS];

        static bool _bListinoCaricatoConSuccesso;  // evita di salvare files incompleti
        static bool _bDB_InitialConnectionOk;      // usata da caricaDati

        static bool[] _bScontoGruppo = new bool[NUM_EDIT_GROUPS];

        /// <summary>
        /// ultima Articolo utile dell'array SF_Data.Articolo[], inizializzata in avvio <br/>
        /// da bSalvaListino e modificata da FrmMain.MainGridDragDrop
        /// </summary>
        static int _iLastArticoloIndexP1;

        /// <summary>
        /// numero di possibili Articoli nelle pagine, inizializzato in avvio
        /// da CaricaListino() e modificato da FrmSetGrid.OKBtnClick
        /// </summary>
        static int _iLastArticoloIndex;

        static ulong ulStart, ulStop, ulPingTime;

        /// <summary>variabile calcolata su tutti i dati del Listino</summary>
        static String _sLocListinoChecksum;

        /// <summary>variabile calcolata sui dati essenziali del Listino</summary>
        static String _sWebListinoChecksum;

        /// <summary>variabile letta dal file remoto</summary>
        static String _sRemDBChecksum;

        static String _sRootDir;    // "..\\StandDati"
        static String _sExeDir;   	// directory per il file Prezzi
        static String _sAnnoDir;    // una directory per ciascun anno
        static String _sDataDir;	// directory per i dati di riepilogo
        static String _sLogDir;	    // directory per i dati di log
        static String _sTicketsDir; // una directory per ciascun giorno
        static String _sMessagesDir;// una directory per ciascun giorno
        static String _sCopiesDir;  // una directory per ciascun giorno

        static TErrMsg _ErrMsg;

        static TArticolo[] _dispArticoli = new TArticolo[MAX_NUM_ARTICOLI];

        /// <summary>
        /// ottiene stringa path della root Directory
        /// </summary>
        public static String sGetRootDir() { return _sRootDir; }

        /// <summary>
        /// ottiene stringa path della Directory Anno
        /// </summary>
        public static String sGetAnnoDir() { return _sAnnoDir; }

        /// <summary>
        /// ottiene stringa path della Directory che contiene i Dati
        /// </summary>
        public static String sGetDataDir() { return _sDataDir; }

        /// <summary>
        /// ottiene stringa path della home Directory
        /// </summary>
        public static String sGetExeDir() { return _sExeDir; }

        /// <summary>
        /// ottiene stringa path della Log Directory
        /// </summary>
        public static String sGetLogDir() { return _sLogDir; }

        /// <summary>
        /// ottiene stringa path della Directory dei Tickets
        /// </summary>
        public static String sGetTicketsDir() { return _sTicketsDir; }

        /// <summary>
        /// ottiene stringa path della Directory dei Messaggi
        /// </summary>
        public static String sGetMessagesDir() { return _sMessagesDir; }

        /// <summary>
        /// ottiene stringa path della Directory delle Copie
        /// </summary>
        public static String sGetCopiesDir() { return _sCopiesDir; }

        /// <summary>verifica se il Listino è stato modificato</summary>
        public static bool bGetListinoModificato() { return _bListinoModificato; }

        /// <summary>aggiorna _sRemDBChecksum dopo che è stato aggiornato il listino nel webserver</summary>
        public static void SetRemDBChecksum() { _sRemDBChecksum = _sWebListinoChecksum; }

        /// <summary>ottiene il checksum relativo al listino web </summary>
        public static String sGetWebListinoChecksum() { return _sWebListinoChecksum; }

        /// <summary>
        /// imposta _iLastArticoloIndex, chiamata da ImpostaGriglia
        /// </summary>
        public static void SetLastArticoloIndex(int iParam) { _iLastArticoloIndex = iParam; }

        /// <summary>
        /// Costruttore
        /// </summary>
        public DataManager()
        {
            // impostazione della data, è necessario effettuarla qui prima di lanciare
            // la form di avvio, actualDateTime è inoltre usata dal processo LogServer
            initActualDate();

            InitDirectories(); // punto unico

            azzeraDati();


            // recupera solo la data
            bCaricaListino(true);

            _iLastArticoloIndex = MAX_NUM_ARTICOLI;

            _bListinoModificato = false;
            _bListinoCaricatoConSuccesso = false;
        }

        /// <summary>
        /// inizializza SF_Data.iNumCassa, azzera l'array SF_Data[]
        /// </summary>
        private static void azzeraDati()
        {
            int i;

            SF_Data.bTavoloRichiesto = false;
            SF_Data.bCopertoRichiesto = false;
            SF_Data.bRiservatezzaRichiesta = false;
            SF_Data.bPrevendita = false;
            SF_Data.bTouchMode = false;
            SF_Data.bModoPagamRichiesto = false;
            SF_Data.bAnnullato = false;
            SF_Data.bScaricato = false;
            SF_Data.bStampato = false;

            if (iReadRegistry(DB_MODE_KEY, (int)DB_MODE.SQLITE) > 0)
                SF_Data.iNumCassa = iReadRegistry(NUM_CASSA_KEY, 1);
            else
                SF_Data.iNumCassa = CASSA_PRINCIPALE;   // SQLite

            SF_Data.iStatusReceipt = 0;
            SF_Data.iScontoStdReceipt = 0;
            SF_Data.iScontoFissoReceipt = 0;
            SF_Data.iScontoGratisReceipt = 0;
            SF_Data.iStatusSconto = 0;
            SF_Data.sScontoReceipt = "";
            SF_Data.iBarcodeRichiesto = 0;
            SF_Data.iReceiptCopyOptions = 0;
            SF_Data.iGridCols = DEF_GRID_NCOLS;
            SF_Data.iGridRows = DEF_GRID_NROWS;
            SF_Data.iNumOfLastReceipt = 0;
            SF_Data.iStartingNumOfReceipts = 0;
            SF_Data.iActualNumOfReceipts = 0;
            SF_Data.iNumOfWebReceipts = 0;
            SF_Data.iNumAnnullati = 0;
            SF_Data.iNumOfMessages = 0;
            SF_Data.iTotaleReceipt = 0;
            SF_Data.iTotaleReceiptDovuto = 0;
            SF_Data.iTotaleIncasso = 0;
            SF_Data.iTotaleIncassoCard = 0;
            SF_Data.iTotaleIncassoSatispay = 0;
            SF_Data.iTotaleScontatoStd = 0;
            SF_Data.iTotaleScontatoFisso = 0;
            SF_Data.iTotaleScontatoGratis = 0;
            SF_Data.iTotaleAnnullato = 0;
            SF_Data.sTavolo = "";
            SF_Data.sNome = "";
            SF_Data.sNota = "";
            SF_Data.sMessaggio = "";

            SF_Data.sVersione = String.Format("StandFacile {0}", RELEASE_SW);
            SF_Data.sDateTime = GetDateTimeString();
            SF_Data.sWebDateTime = "";
            SF_Data.sPrevDateTime = "";
            SF_Data.sListinoDateTime = GetDateTimeString();

            SF_Data.iNumOrdinePrev = 0;
            SF_Data.iNumOrdineWeb = 0;

            for (i = 0; i < MAX_NUM_HEADERS; i++) // azzeramento
                SF_Data.sHeaders[i] = "";

            for (i = 0; i < PAGES_NUM_TABM; i++)
                SF_Data.sPageTabs[i] = String.Format("Pagina {0}", i + 1);

            for (i = 0; i < NUM_COPIES_GRPS; i++)
                SF_Data.sCopiesGroupsText[i] = sConstCopiesGroupsText[i];

            for (i = 0; i < NUM_EDIT_GROUPS; i++)
            {
                SF_Data.iGroupsColor[i] = 0;
                SF_Data.bCopiesGroupsFlag[i] = false;
            }

            for (i = 0; i < NUM_GROUPS_COLORS - 1; i++)
                SF_Data.sColorGroupsText[i] = "";

            for (i = 0; i < MAX_NUM_ARTICOLI + EXTRA_NUM_ARTICOLI; i++) // azzeramento
            {
                SF_Data.Articolo[i].bLocalPrinted = false;
                SF_Data.Articolo[i].sTipo = "";
                SF_Data.Articolo[i].sNotaArt = "";
                SF_Data.Articolo[i].iPrezzoUnitario = 0;
                SF_Data.Articolo[i].iQuantita_Scaricata = 0;
                SF_Data.Articolo[i].iQuantitaOrdine = 0;
                SF_Data.Articolo[i].iIndexListino = i;  // unico diverso da zero
                SF_Data.Articolo[i].iGruppoStampa = 0;
                SF_Data.Articolo[i].iOptionsFlags = 0;
                SF_Data.Articolo[i].iQuantitaVenduta = 0;
                SF_Data.Articolo[i].iQtaEsportata = 0;
                SF_Data.Articolo[i].iDisponibilita = DISP_OK;
            }

            // record di gestione dei coperti posto nell'ultimo articolo
            SF_Data.Articolo[MAX_NUM_ARTICOLI - 1].iGruppoStampa = (int)DEST_TYPE.DEST_COUNTER;
            SF_Data.Articolo[MAX_NUM_ARTICOLI - 1].iPrezzoUnitario = 0;
            SF_Data.Articolo[MAX_NUM_ARTICOLI - 1].sTipo = _COPERTO;
        }

        /// <summary>azzera i dati relativi all'immissione dello scontrino corrente</summary>
        public static void clearGrid()
        {
            // per pulire Stato, Sconti, Anteprima
            SF_Data.iStatusReceipt = 0;
            SF_Data.iStatusSconto = 0;
            SF_Data.iScontoStdReceipt = 0;
            SF_Data.iScontoFissoReceipt = 0;
            SF_Data.iScontoGratisReceipt = 0;
            SF_Data.sScontoReceipt = "";

            // si riparte con ordini locali
            SF_Data.iNumOrdinePrev = 0;
            SF_Data.iNumOrdineWeb = 0;

            SF_Data.sTavolo = "";
            SF_Data.sNome = "";
            SF_Data.sNota = "";

            for (int i = 0; i < MAX_NUM_ARTICOLI + EXTRA_NUM_ARTICOLI; i++)
            {
                SF_Data.Articolo[i].iQuantitaOrdine = 0;
                SF_Data.Articolo[i].iOptionsFlags = 0;
            }

            rFrmMain.setEditTavolo("");
            rFrmMain.setEditCoperto("");
            rFrmMain.setEditNota("");

            rFrmMain.resetPayment();

            rFrmMain.clearAnteprima();

            LogToFile("DataManager : resetGrid");
        }

        /// <summary>inizializzazione delle stringhe di formattazione</summary>
        public static void InitFormatStrings(bool bSetChars33_true = false)
        {

            if (bSetChars33_true)
                sGlbWinPrinterParams.bChars33 = true;

            iMAX_RECEIPT_CHARS = sGlbWinPrinterParams.bChars33 ? MAX_ABS_RECEIPT_CHARS : MAX_LEG_RECEIPT_CHARS;
            iMAX_ART_CHAR = sGlbWinPrinterParams.bChars33 ? MAX_ABS_ART_CHAR : MAX_LEG_ART_CHAR;

            iCenterOrderNum = sGlbWinPrinterParams.bChars33 ? MAX_ABS_RECEIPT_CHARS - 8 : MAX_LEG_RECEIPT_CHARS - 2;

            sRCP_FMT_RCPT = sGlbWinPrinterParams.bChars33 ? _RCP_FMT_33_RCPT : _RCP_FMT_28_RCPT;
            sRCP_FMT_CPY = sGlbWinPrinterParams.bChars33 ? _RCP_FMT_33_CPY : _RCP_FMT_28_CPY;
            sRCP_FMT_NOTE = sGlbWinPrinterParams.bChars33 ? _RCP_FMT_33_NOTE : _RCP_FMT_28_NOTE;
            sRCP_FMT_DSC = sGlbWinPrinterParams.bChars33 ? _RCP_FMT_33_DSC : _RCP_FMT_28_DSC;
            sRCP_FMT_DIF = sGlbWinPrinterParams.bChars33 ? _RCP_FMT_33_DIF : _RCP_FMT_28_DIF;
            sRCP_FMT_TOT = sGlbWinPrinterParams.bChars33 ? _RCP_FMT_33_TOT : _RCP_FMT_28_TOT;
            sRCP_FMT_DSH = sGlbWinPrinterParams.bChars33 ? _RCP_FMT_33_DSH : _RCP_FMT_28_DSH;

            sGRD_FMT_STD = sGlbWinPrinterParams.bChars33 ? _GRD_FMT_33_STD : _GRD_FMT_28_STD;
            sGRD_FMT_TCH = sGlbWinPrinterParams.bChars33 ? _GRD_FMT_33_TCH : _GRD_FMT_28_TCH;

            sGRDW_FMT_STD = sGlbWinPrinterParams.bChars33 ? _GRDW_FMT_33_STD : _GRDW_FMT_28_STD;
            sGRDZ_FMT_TCH = sGlbWinPrinterParams.bChars33 ? _GRDZ_FMT_33_TCH : _GRDZ_FMT_28_TCH;
            sGRDW_FMT_TCH = sGlbWinPrinterParams.bChars33 ? _GRDW_FMT_33_TCH : _GRDW_FMT_28_TCH;

            sDAT_FMT_PRL = sGlbWinPrinterParams.bChars33 ? _DAT_FMT_33_PRL : _DAT_FMT_28_PRL;
            sDAT_FMT_DAT = sGlbWinPrinterParams.bChars33 ? _DAT_FMT_33_DAT : _DAT_FMT_28_DAT;
            sDAT_FMT_TOT = sGlbWinPrinterParams.bChars33 ? _DAT_FMT_33_TOT : _DAT_FMT_28_TOT;
            sDAT_FMT_DSH = sGlbWinPrinterParams.bChars33 ? _DAT_FMT_33_DSH : _DAT_FMT_28_DSH;
            sDAT_FMT_HED = sGlbWinPrinterParams.bChars33 ? _DAT_FMT_33_HED : _DAT_FMT_28_HED;

            sDAT_FMT_REP_RED = sGlbWinPrinterParams.bChars33 ? _DAT_FMT_33_REP_RED : _DAT_FMT_28_REP_RED;
            sDAT_FMT_DSH_RED = sGlbWinPrinterParams.bChars33 ? _DAT_FMT_33_DSH_RED : _DAT_FMT_28_DSH_RED;
            sDAT_FMT_TOT_RED = sGlbWinPrinterParams.bChars33 ? _DAT_FMT_33_TOT_RED : _DAT_FMT_28_TOT_RED;
        }

        /// <summary>Crea le varie directories di lavoro</summary>
        public static void InitDirectories()
        {
            // impostazione della directory per il file Prezzi (la stessa dell'eseguibile)
            _sExeDir = Directory.GetCurrentDirectory();

            Directory.SetCurrentDirectory(_sExeDir);

            // impostazione della directory di default per operazioni sui dati
            _sRootDir = Define.ROOT_DIR;
            Directory.CreateDirectory(_sRootDir);	// si può creare solo una Dir per volta!
            Directory.SetCurrentDirectory(_sRootDir);
            _sRootDir = Directory.GetCurrentDirectory();

            // impostazione della directory per ciascun anno
            _sAnnoDir = ANNO_DIR + getActualDate().ToString("yyyy");
            Directory.CreateDirectory(_sAnnoDir);
            Directory.SetCurrentDirectory(_sAnnoDir);
            _sAnnoDir = Directory.GetCurrentDirectory();

            // impostazione della directory per i dati di riepilogo
            Directory.SetCurrentDirectory(_sAnnoDir);
            _sDataDir = Define.DATA_DIR;
            Directory.CreateDirectory(_sDataDir);
            Directory.SetCurrentDirectory(_sDataDir);
            _sDataDir = Directory.GetCurrentDirectory();

            // impostazione della directory per i dati di log
            Directory.SetCurrentDirectory(_sAnnoDir);
            _sLogDir = Define.LOG_DIR;
            Directory.CreateDirectory(_sLogDir);
            Directory.SetCurrentDirectory(_sLogDir);
            _sLogDir = Directory.GetCurrentDirectory();

            // impostazione della directory per gli scontrini
            Directory.SetCurrentDirectory(_sAnnoDir);
            _sTicketsDir = NOME_DIR_RECEIPTS + getActualDate().ToString("MMdd");
            Directory.CreateDirectory(_sTicketsDir);
            Directory.SetCurrentDirectory(_sTicketsDir);
            _sTicketsDir = Directory.GetCurrentDirectory();

            // impostazione della directory per i messaggi alla cucina
            Directory.SetCurrentDirectory(_sAnnoDir);
            _sMessagesDir = NOME_DIR_MSGS + getActualDate().ToString("MMdd");
            Directory.CreateDirectory(_sMessagesDir);
            Directory.SetCurrentDirectory(_sMessagesDir);
            _sMessagesDir = Directory.GetCurrentDirectory();

            // impostazione della directory per le stampe in cucina
            Directory.SetCurrentDirectory(_sAnnoDir);
            _sCopiesDir = NOME_DIR_COPIES + getActualDate().ToString("MMdd");
            Directory.CreateDirectory(_sCopiesDir);
            Directory.SetCurrentDirectory(_sCopiesDir);
            _sCopiesDir = Directory.GetCurrentDirectory();

            // ultima impostazione
            Directory.SetCurrentDirectory(_sDataDir);
        }

        /// <summary>
        /// esegue l'inizializzazione dei dati chiamando CaricaListino(), CaricaDatidaOrdini()
        /// </summary>
        public void Init()
        {
            bool bListinoOk;
            String sTmp;

            iMAX_RECEIPT_CHARS = MAX_LEG_RECEIPT_CHARS;
            iMAX_ART_CHAR = MAX_LEG_ART_CHAR;

            // eseguito una prima volta per inizializzare le stringhe di formattazione con
            // sGlbWinPrinterParams.bChars33 = true;
            InitFormatStrings(true);

            // test di connessione, se fallisce si evita di pedere tempo qui di seguito
            _bDB_InitialConnectionOk = _rdBaseIntf.dbInit(getActualDate(), CASSA_PRINCIPALE, true);

            // verifica se lavorare con la data del DB o quella del PC
            if (_bDB_InitialConnectionOk)                   // per non perdere tempo
                _bCheckStatus = _rdBaseIntf.dbCheckStatus(true);

            // salva una copia della disponibilità prima di chiamare CaricaDatidaOrdini()
            if (InitialDispDlg._getApplicaDisponibilita())
                _dispArticoli = deepCopy(DB_Data.Articolo);

            //più leggibile così che dentro al loop successivo
            // inizializza Articolo[] con i dati del file Prezzi, punto unico
            bListinoOk = bCaricaListino();

            // bCaricaListino potrebbe aver impostato uno sconto
            ScontoDlg._resetSconto();

            // inizializza SF_Data con i dati del database, punto unico
            if (bListinoOk & _bDB_InitialConnectionOk)
                CaricaDatidaOrdini();   // inizializza Articolo[] con i dati del database, punto unico

            if (InitialDispDlg._getApplicaDisponibilita())
                CaricaDisponibilita();

            _rdBaseIntf.dbSalvaOrdine(true);

            if (NetConfigDlg.rNetConfigDlg.bGetWebOrderEnabled())
            {
                sTmp = String.Format("Avvio StandFacile: {0} {1}, C={2}", RELEASE_SW, RELEASE_TBL, SF_Data.iNumCassa);
                dBaseTunnel_my.rdbLogWriteVersion(sTmp);
                LogToFile("Datamanager Init WebOrderEnabled");
            }
            else
                LogToFile("Datamanager Init !WebOrderEnabled");

            sTmp = String.Format("Datamanager Init: iMAX_RECEIPT_CHARS = {0}", iMAX_RECEIPT_CHARS);
            LogToFile(sTmp);

            Console.WriteLine(sTmp);

            InitFormatStrings();

        }

        /// <summary>
        /// caricamento da dB dei dati del venduto e disponibilità Articoli coerenti con il Listino<br/>
        /// funzione invasiva se bLigthModeParam == true: dato che resetta dbResetNumOfOrders, dbResetNumOfClients, dbResetNumOfMessages
        /// <summary/>
        private static void CaricaDatidaOrdini(bool bLigthModeParam = false, bool bSilentParam = false)
        {
            bool bMatch, bSingleWarn, bDbRead_Ok;
            int i, j, iLastArticoloDBIndexP1;
            String sDebug;

            try // eventuali errori bloccano l'esecuzione del programma
            {
                LogToFile("DataManager : I CaricaDati");

                SF_Data.iNumOfLastReceipt = _rdBaseIntf.dbCaricaDatidaOrdini(getActualDate(), SF_Data.iNumCassa, true);

                SF_Data.iActualNumOfReceipts = DB_Data.iActualNumOfReceipts;
                SF_Data.iStartingNumOfReceipts = sConfig.iReceiptStartNumber - 1;

                // nel caso in cui la tabella _sDBTNameOrdini non esiste (SF_Data.iNumOfLastReceipt == -1)
                if ((SF_Data.iNumOfLastReceipt < 0) || (SF_Data.iNumOfLastReceipt < (sConfig.iReceiptStartNumber - 1)))
                    SF_Data.iNumOfLastReceipt = sConfig.iReceiptStartNumber - 1;

                SF_Data.iNumOfMessages = DB_Data.iNumOfMessages;

                sDebug = DB_Data.Articolo[0].sTipo;
                iLastArticoloDBIndexP1 = MAX_NUM_ARTICOLI; // successivamente potrebbe incrementare

                bDbRead_Ok = (SF_Data.iNumOfLastReceipt > 0);

                if (_bUSA_NDB() && !bLigthModeParam)
                {
                    dbSetNumOfLastOrderFromDB(SF_Data.iNumOfLastReceipt);

                    dbSetNumOfLastMessageFromDB(SF_Data.iNumOfMessages);

                    /*****************************************************************
                     * solo la CASSA_PRINCIPALE può riscrivere la tabella num_ordini
                     * indispensabile creare la tabella se non esiste
                     * va messa alla fine del caricamento dati
                     *****************************************************************/
                    if (SF_Data.iNumCassa == CASSA_PRINCIPALE)
                    {
                        _rdBaseIntf.dbResetNumOfOrders(SF_Data.iNumOfLastReceipt);
                        _rdBaseIntf.dbResetNumOfMessages(SF_Data.iNumOfMessages);
                    }
                }
                //else

                /************************************
                 *		controllo di sicurezza
                 ************************************/
                bSingleWarn = false;

                for (j = 0; j < MAX_NUM_ARTICOLI + EXTRA_NUM_ARTICOLI; j++)
                {
                    if (String.IsNullOrEmpty(DB_Data.Articolo[j].sTipo))
                        continue;
                    else
                    {
                        bMatch = false;
                        sDebug = DB_Data.Articolo[j].sTipo;

                        for (i = 0; i < MAX_NUM_ARTICOLI; i++)
                        {
                            // caricamento in Articolo[], esclusi Prezzi Unitario, iGruppo_Stampa
                            if (SF_Data.Articolo[i].sTipo == DB_Data.Articolo[j].sTipo)
                            {
                                SF_Data.Articolo[i].iQuantitaVenduta = DB_Data.Articolo[j].iQuantitaVenduta;
                                SF_Data.Articolo[i].iDisponibilita = DB_Data.Articolo[j].iDisponibilita;
                                SF_Data.Articolo[i].iQuantita_Scaricata = DB_Data.Articolo[j].iQuantita_Scaricata;
                                bMatch = true;
                                break;
                            }
                        }

                        // aggiunge alla fine l'Articolo altrimenti non trovato, non serve aggiornare:
                        // DB_Data.iTotaleScontatoStd, DB_Data.iTotaleScontatoFisso, DB_Data.iTotaleScontatoGratis

                        // si evitano warnings sulle quantità nulle
                        if (!bMatch && (DB_Data.Articolo[j].iQuantitaVenduta > 0))
                        {
                            SF_Data.Articolo[iLastArticoloDBIndexP1].iQuantitaVenduta = DB_Data.Articolo[j].iQuantitaVenduta;
                            SF_Data.Articolo[iLastArticoloDBIndexP1].iDisponibilita = DB_Data.Articolo[j].iDisponibilita;
                            SF_Data.Articolo[iLastArticoloDBIndexP1].iQuantita_Scaricata = DB_Data.Articolo[j].iQuantita_Scaricata;

                            SF_Data.Articolo[iLastArticoloDBIndexP1].sTipo = DB_Data.Articolo[j].sTipo;
                            SF_Data.Articolo[iLastArticoloDBIndexP1].iPrezzoUnitario = DB_Data.Articolo[j].iPrezzoUnitario;
                            SF_Data.Articolo[iLastArticoloDBIndexP1].iGruppoStampa = DB_Data.Articolo[j].iGruppoStampa;

                            if (iLastArticoloDBIndexP1 < (MAX_NUM_ARTICOLI + EXTRA_NUM_ARTICOLI - 1))
                            {
                                iLastArticoloDBIndexP1++; // può arrivare a (MAX_NUM_ARTICOLI + EXTRA_NUM_ARTICOLI - 1) per non sforare
                                                          // bMatch = true;
                            }
                            else
                            {
                                _WrnMsg.sMsg = String.Format("{0}", DB_Data.Articolo[j].sTipo);
                                _WrnMsg.iErrID = WRN_NVD;
                                WarningManager(_WrnMsg);
                            }
                        }

                        // dà un solo avviso alla prima discordanza
                        if (bDbRead_Ok && !bMatch && !bSingleWarn && !bSilentParam)
                        {
                            _ErrMsg.sMsg = DB_Data.Articolo[j].sTipo;
                            _ErrMsg.iErrID = WRN_MNF;
                            WarningManager(_ErrMsg);
                            bSingleWarn = true;
                        }
                    }
                }

                SF_Data.iNumAnnullati = DB_Data.iNumAnnullati;

                SF_Data.iTotaleAnnullato = DB_Data.iTotaleAnnullato;

                SF_Data.iTotaleScontatoStd = DB_Data.iTotaleScontatoStd;
                SF_Data.iTotaleScontatoFisso = DB_Data.iTotaleScontatoFisso;
                SF_Data.iTotaleScontatoGratis = DB_Data.iTotaleScontatoGratis;

                SF_Data.iTotaleIncasso = DB_Data.iTotaleIncasso;

                SF_Data.iTotaleIncassoCard = DB_Data.iTotaleIncassoCard;
                SF_Data.iTotaleIncassoSatispay = DB_Data.iTotaleIncassoSatispay;
            }

            catch (Exception)
            {
                ErrorManager(_ErrMsg);
            }
        } // end CaricaDati

        /// <summary>
        /// Funzione di emissione dello scontrino
        /// produce tutte le copie ma stampa solo quelle richieste
        /// </summary>
        public static void Receipt()
        {
            bool[] bSelectedGroups = new bool[NUM_EDIT_GROUPS];

            int i;

            String sNomeFileTicketPrt;
            String sTmp, sDir;

            StreamWriter fPrint = null;

            SF_Data.iNumOfLastReceipt = iGetNumOfLocalOrders(); // punto unico

            SF_Data.sDateTime = sGetTicketTime();

            TOrdineStrings sOrdineStrings = new TOrdineStrings();

            sOrdineStrings = SetupHeaderStrings(SF_Data, 0);

            /*******************************
             ***     STAMPA SCONTRINO    ***
             *******************************/

            sNomeFileTicketPrt = String.Format(NOME_FILE_RECEIPT, SF_Data.iNumCassa, SF_Data.iNumOfLastReceipt);
            sDir = _sTicketsDir + "\\";

            WriteReceipt(ref SF_Data, 0, fPrint, sDir, sOrdineStrings);

            /*******************************************
             ***  STAMPA COPIE LOCALI SENZA PREZZI	 ***
             *******************************************/

            sDir = _sTicketsDir + "\\";

            WriteLocalCopy(SF_Data, 0, fPrint, sDir, sOrdineStrings);

            /*******************************************************
             *               STAMPA COPIE DI RETE
             * i contatori sono inclusi nelle PIETANZE es: COPERTI
             *******************************************************/

            sDir = _sCopiesDir + "\\";

            WriteNetworkCopy(SF_Data, 0, fPrint, sDir, sOrdineStrings, false);

            // AVVIO STAMPE LEGACY DOPO IL SALVATAGGIO SU DATABASE PER EVITARE PROBLEMI EVENTUALI
            if (!PrintReceiptConfigDlg.bGetPrinterTypeIsWinwows())
            {
                // Avvia eventuali code delle copie Legacy
                Printer_Legacy.PrintFile("", sGlbLegacyPrinterParams, (int)PRINT_QUEUE_ACTION.PRINT_START);
            }

            /**********************************************************************
             *  aggiornamento dati vendite in Articolo[i]
             *  deve essere posizionato in coda per via di .iQuantitaTicket = 0
             **********************************************************************/
            for (i = 0; i < MAX_NUM_ARTICOLI; i++)
            {
                if (SF_Data.Articolo[i].iQuantitaOrdine > 0)
                {
                    // si è già verificato che il numero sottostante non è negativo !
                    if (SF_Data.Articolo[i].iDisponibilita != DISP_OK)
                        SF_Data.Articolo[i].iDisponibilita -= SF_Data.Articolo[i].iQuantitaOrdine;

                    SF_Data.Articolo[i].iQuantitaVenduta += SF_Data.Articolo[i].iQuantitaOrdine;

                    SF_Data.Articolo[i].iQuantitaOrdine = 0;
                    SF_Data.Articolo[i].iOptionsFlags = 0;
                    SF_Data.Articolo[i].bLocalPrinted = false;
                    SF_Data.Articolo[i].sNotaArt = "";
                }
            }

            // per pulire Stato, Sconti, Anteprima
            SF_Data.iStatusReceipt = 0;
            SF_Data.iStatusSconto = 0;
            SF_Data.iScontoStdReceipt = 0;
            SF_Data.iScontoFissoReceipt = 0;
            SF_Data.iScontoGratisReceipt = 0;
            SF_Data.sScontoReceipt = "";

            SF_Data.sWebDateTime = "";
            SF_Data.sPrevDateTime = "";
            SF_Data.iNumOrdinePrev = 0;
            SF_Data.iNumOrdineWeb = 0;

            // per pulire l'Anteprima
            SF_Data.sTavolo = "";
            SF_Data.sNome = "";
            SF_Data.sNota = "";

            sTmp = String.Format("DataManager : Emesso Scontrino Num. {0}", SF_Data.iNumOfLastReceipt);
            LogToFile(sTmp);

            /*******************************************************************
             *  se cassa secondaria
             *  mette in tabella (coda) per la gestione della disponibilità
             *******************************************************************/
            if (bCheckIf_CassaSec_and_NDB())
            {
                // per aggiornare la Disponibilità
                _rdBaseIntf.dbCSecOrderEnqueue(SF_Data.iNumOfLastReceipt);
                LogToFile("DataManager : updateDispRequest");
            }

            // aggiornamento dati
            SalvaDati();
        }

        /// <summary>
        /// aggiorna la disponibilità:
        /// CASSA_PRINCIPALE aggiorna SF_Data analizzando la coda degli ultimi gli ordini emessi dalla cassa secondaria <br/>
        /// CASSE_SECONDARIE aggiornano SF_Data da data_c1_yymmdd
        /// </summary>
        public static void aggiornaDisponibilità()
        {
            // serve per gestire 1 ordine emesso e subito cancellato per cui risulta 2 volte nella lista,
            // per cui risulta DB_Data.bAnnullato == true sia con iNumScontrinoSec < 0 che con iNumScontrinoSec > 0
            bool bOrdineAnnullato;

            bool bMatch, bSingleWarn, bDbRead_Ok, bServeSalvare = false;
            int i, j, iNumScontrinoSec; // numero scontrino dalla cassa Secondaria
            String sTmp, sTipo, sDebug;

            // *** sicurezza ***
            if (!_bUSA_NDB()) return;

            if (SF_Data.iNumCassa == CASSA_PRINCIPALE)
            {
                do
                {
                    bOrdineAnnullato = false;

                    iNumScontrinoSec = _rdBaseIntf.dbClearOrdiniCSec(true);

                    // è stato annullato
                    if (iNumScontrinoSec < 0)
                    {
                        bOrdineAnnullato = true;
                        iNumScontrinoSec = -iNumScontrinoSec;
                        sTmp = String.Format("aggiornaDisponibilità : annullato {0}", iNumScontrinoSec);
                        LogToFile(sTmp);
                    }

                    if (iNumScontrinoSec > 0)
                    {
                        bServeSalvare = true;

                        // dbCaricaOrdine va qui per elaborare solo iNumScontrinoSec > 0
                        bDbRead_Ok = _rdBaseIntf.dbCaricaOrdine(getActualDate(), iNumScontrinoSec, false);

                        if (bDbRead_Ok) // l'ultimo DB letto esiste
                        {
                            /************************************
                             *		controllo di sicurezza
                             ************************************/
                            bSingleWarn = false;

                            for (j = 0; j < MAX_NUM_ARTICOLI; j++) // COPERTI inclusi
                            {
                                sTipo = DB_Data.Articolo[j].sTipo;

                                if (bStringBelongsTo_ORDER_CONST(sTipo) || String.IsNullOrEmpty(sTipo))
                                    continue;

                                bMatch = false;
                                for (i = 0; i < MAX_NUM_ARTICOLI; i++) // COPERTI inclusi date che deve essere gestito !
                                {
                                    // aggiornamento dell'Articolo[] presente nell'ordine
                                    if ((SF_Data.Articolo[i].sTipo == sTipo))
                                    {
                                        if (SF_Data.Articolo[i].iDisponibilita != DISP_OK)
                                        {
                                            if (bOrdineAnnullato)
                                                SF_Data.Articolo[i].iDisponibilita += DB_Data.Articolo[j].iQuantitaOrdine;
                                            else
                                                SF_Data.Articolo[i].iDisponibilita -= DB_Data.Articolo[j].iQuantitaOrdine;

                                            if (SF_Data.Articolo[i].iDisponibilita < 0)
                                            {
                                                WarningManager(WRN_QMD);
                                                SF_Data.Articolo[i].iDisponibilita = 0;
                                            }
                                        }

                                        bMatch = true;
                                        break;
                                    }
                                }

                                // dà un solo avviso alla prima discordanza
                                if (!bMatch && !bSingleWarn)
                                {
                                    _ErrMsg.sMsg = DB_Data.Articolo[j].sTipo;
                                    _ErrMsg.iErrID = WRN_MNF;
                                    WarningManager(_ErrMsg);
                                    bSingleWarn = true;
                                    LogToFile("aggiornaDisponibilità : record1 " + _ErrMsg.sMsg + "non trovato I !");
                                }
                            }

                            sTmp = String.Format("aggiornaDisponibilità : a seguito ordine {0}", iNumScontrinoSec);
                            LogToFile(sTmp);
                        }
                        else
                        {
                            sTmp = String.Format("aggiornaDisponibilità : impossibile caricare l'ordine {0}", iNumScontrinoSec);
                            LogToFile(sTmp);
                        }
                    }
                }
                while (iNumScontrinoSec > 0);

                if (bServeSalvare)
                    SalvaDati();
            }
            else
            {

                iNumScontrinoSec = _rdBaseIntf.dbClearOrdiniCSec(false);

                // controllo altrimenti se ci sono disp non elaborate dalla cassa Principale si crea confusione !
                if (iNumScontrinoSec == 0)
                {
                    // cassa secondaria
                    bDbRead_Ok = _rdBaseIntf.dbCaricaDisponibilità(getActualDate());

                    /************************************
                     *  controllo di sicurezza articoli
                     ************************************/
                    bSingleWarn = false;

                    sDebug = DB_Data.Articolo[0].sTipo;

                    for (i = 0; i < MAX_NUM_ARTICOLI; i++)
                    {
                        if (String.IsNullOrEmpty(SF_Data.Articolo[i].sTipo))
                            continue;
                        else
                        {
                            bMatch = false;

                            for (j = 0; j < MAX_NUM_ARTICOLI; j++)
                            {
                                // caricamento in Articolo[]
                                if (SF_Data.Articolo[i].sTipo == DB_Data.Articolo[j].sTipo)
                                {
                                    SF_Data.Articolo[i].iDisponibilita = DB_Data.Articolo[j].iDisponibilita;
                                    bMatch = true;
                                    break;
                                }
                            }

                            // dà un solo avviso alla prima discordanza
                            if (bDbRead_Ok && !bMatch && !bSingleWarn)
                            {
                                _ErrMsg.sMsg = SF_Data.Articolo[i].sTipo;
                                _ErrMsg.iErrID = WRN_MNF;
                                WarningManager(_ErrMsg);
                                bSingleWarn = true;
                                _ErrMsg.sMsg = "";
                                LogToFile("aggiornaDisponibilità : record2 " + _ErrMsg.sMsg + " non trovato I !");
                            }
                        }
                    }
                }
            }

            // aggiornamento griglia
            rFrmMain.FormResize(null, null);
            rFrmMain.MainGrid_Redraw(null, null);

        } // fine aggiornaDisponibilità()

        /// <summary>funzione che carica la disponibilità della sessione precedente, usata da Init()</summary>
        static void CaricaDisponibilita()
        {
            bool bSingleWarn, bMatch;
            int i, j;

            /********************************************************
             *	controllo di sicurezza per recupero disponibilità
             ********************************************************/
            bSingleWarn = false;

            for (i = 0; i < MAX_NUM_ARTICOLI; i++)
            {
                if (String.IsNullOrEmpty(SF_Data.Articolo[i].sTipo))
                    continue;
                else
                {
                    bMatch = false;

                    for (j = 0; j < MAX_NUM_ARTICOLI; j++)
                    {
                        if (SF_Data.Articolo[i].sTipo == _dispArticoli[j].sTipo)
                        {
                            //if (dispArticoli[j].iDisponibilita != DISP_OK)
                            SF_Data.Articolo[i].iDisponibilita = _dispArticoli[j].iDisponibilita;

                            bMatch = true;
                            break;
                        }
                    }

                    // dà un solo avviso alla prima discordanza
                    if (!bMatch && !bSingleWarn)
                    {
                        _ErrMsg.sMsg = SF_Data.Articolo[i].sTipo;
                        _ErrMsg.iErrID = WRN_MNF;
                        WarningManager(_ErrMsg);
                        bSingleWarn = true;
                    }
                }
            } // end for

            SalvaDati();
        }

        /// <summary>
        /// annulla uno scontrino e poi salva i dati, iNumAnnulloParam è il numero dell'ordine, <br/>
        /// bEseguiParam == false se si vuole solo controllare se è annullato <br/>
        /// bEseguiParam == true se si vuole eseguire l'annullo
        /// </summary>
        public static bool bAnnulloOrdine(int iNumAnnulloParam)
        {
            bool bResult, bMessaggioCaricato, bMatch, bSingleWarn;

            int i, j, iStatusTicket, iQuantita_Ordine;
            String sTmp, sTipo, sNomeFileMsg;
            StreamWriter fTxtFile;

            bool[] bGroupsColorPrinted = new bool[NUM_EDIT_GROUPS];

            bResult = _rdBaseIntf.dbAnnulloOrdine(getActualDate(), iNumAnnulloParam, "");

            /************************************
                    controllo di sicurezza
             ************************************/
            bSingleWarn = false;

            iStatusTicket = DB_Data.iStatusReceipt;

            for (j = 0; j < MAX_NUM_ARTICOLI; j++) // COPERTI inclusi
            {
                sTipo = DB_Data.Articolo[j].sTipo;

                iQuantita_Ordine = DB_Data.Articolo[j].iQuantitaOrdine;

                if (bStringBelongsTo_ORDER_CONST(sTipo) || String.IsNullOrEmpty(sTipo))
                    continue;

                bMatch = false;

                for (i = 0; i < MAX_NUM_ARTICOLI; i++)
                {
                    if (SF_Data.Articolo[i].sTipo == sTipo)
                    {
                        /****************************************************************
                         *   aggiornamento SF_Data[]
                         *   per i totali dbCaricaDatidaOrdini() mette a posto tutto
                         ****************************************************************/

                        SF_Data.Articolo[i].iQuantitaVenduta -= iQuantita_Ordine;

                        // questo aggiornamento è importante
                        if (SF_Data.Articolo[i].iDisponibilita != DISP_OK)
                            SF_Data.Articolo[i].iDisponibilita += iQuantita_Ordine;

                        bMatch = true;
                        break;
                    }
                }

                // dà un solo avviso alla prima discordanza
                if ((!bMatch) && (!bSingleWarn))
                {
                    _WrnMsg.sMsg = iNumAnnulloParam.ToString();
                    _WrnMsg.iErrID = WRN_RNF;
                    WarningManager(_WrnMsg);

                    sTmp = String.Format("dbAnnulloOrdine : iOrdine_ID = {0}, {1} non esiste!", iNumAnnulloParam, sTipo);
                    LogToFile(sTmp);
                }
            }

            if (bCheckIf_CassaSec_and_NDB())
            {
                // per aggiornare la Disponibilità, attenzione al - 
                // per non collidere in caso di Cassa Principale con StandFacile non in esecuzione
                _rdBaseIntf.dbCSecOrderEnqueue(-iNumAnnulloParam);
                LogToFile("DataManager : updateDispRequest");
            }

            if (bResult)
            {
                // in caso di annullo DB_Data è coerente con SF_Data
                CheckSomethingToPrint(_bSomethingInto_GrpToPrint, _bSomethingInto_ClrToPrint, DB_Data);

                ResetCopies_ToBePrintedOnce(bGroupsColorPrinted);

                bMessaggioCaricato = _rdBaseIntf.dbCaricaMessaggio(SF_Data.iNumOfMessages, false);

                if (bMessaggioCaricato)
                {
                    sNomeFileMsg = String.Format(NOME_FILE_MESSAGGIO, SF_Data.iNumCassa, SF_Data.iNumOfMessages);
                    fTxtFile = File.CreateText(_sMessagesDir + "\\" + sNomeFileMsg);
                    fTxtFile.WriteLine(DB_Data.sMessaggio);
                    fTxtFile.Close();

                    for (i = 0; (i < NUM_COPIES_GRPS) && !SF_Data.bPrevendita; i++)
                    {
                        // evita di stampare più di una volta gruppi dello stesso colore
                        if (!CheckCopy_ToBePrintedOnce(i, bGroupsColorPrinted, SF_Data))
                            continue;

                        if (_bSomethingInto_ClrToPrint[i])
                        {
                            if (PrintNetCopiesConfigDlg.bGetPrinterTypeIsWinwows(i))
                                Printer_Windows.PrintFile(_sMessagesDir + "\\" + sNomeFileMsg, sGlbWinPrinterParams, i);
                            else
                                Printer_Legacy.PrintFile(_sMessagesDir + "\\" + sNomeFileMsg, sGlbLegacyPrinterParams, (int)PRINT_QUEUE_ACTION.PRINT_NOW);
                        }
                    }
                }
            }

            SalvaDati();    // salva disponibilità

            if (bCheckService(Define._AUTO_SEQ_TEST))
            {
                CaricaDatidaOrdini(true, true); // attenzione che sovrascrive SF_Data.Articolo[i].iDisponibilita !

                SalvaDati();    // salva aggiornamenti annullati
            }

            return bResult;
        }

        /// <summary>da utilizzare solo per le verifiche di scontrino significativo</summary>
        public static bool bTicketIsGood()
        {
            bool bCounterPresente = false;
            int i, iTotaleCurrTicket = 0;

            // totale scontrino corrente
            for (i = 0; i < MAX_NUM_ARTICOLI; i++)
            {
                iTotaleCurrTicket += SF_Data.Articolo[i].iQuantitaOrdine * SF_Data.Articolo[i].iPrezzoUnitario;

                if ((SF_Data.Articolo[i].iQuantitaOrdine > 0) && (SF_Data.Articolo[i].iGruppoStampa == (int)DEST_TYPE.DEST_COUNTER))
                    bCounterPresente = true;
            }

            if ((iTotaleCurrTicket > 0) || bCounterPresente)
                return true;
            else
                return false;
        }

        /// <summary>
        /// verifica e ricalcola l'indice dell'ultimo Articolo presente nella griglia +1 <br/>
        /// serve per agevolare i controlli sui range, <br/> <br/>
        /// attenzione che il limite _iLastArticoloIndexP1 vale solo per SF_Data.Articolo[] e non per DB_Data.Articolo[]
        /// </summary>
        public static int iCheckLastArticoloIndexP1()
        {
            int i, iUltimoArticolo_NE;

            iUltimoArticolo_NE = 0;

            for (i = 0; (i < MAX_NUM_ARTICOLI - 1); i++) // COPERTI esclusi
                if (!String.IsNullOrEmpty(SF_Data.Articolo[i].sTipo))
                    iUltimoArticolo_NE = i;

            // iUltimoArticolo_NE è un indice di vettore e quindi parte da 0
            _iLastArticoloIndexP1 = iUltimoArticolo_NE + 1;

            return _iLastArticoloIndexP1;
        }

        /// <summary>
        /// verifica se è stata caricata una disponibilità significativa<br/>
        /// usato da MainForm durante la chiusura
        /// </summary>
        public static bool bCheckDispLoaded()
        {
            for (int i = 0; (i < MAX_NUM_ARTICOLI - 1); i++) // COPERTI esclusi
                if (SF_Data.Articolo[i].iDisponibilita != DISP_OK)
                    return true;

            return false;
        }

        /// <summary>da utilizzare per il controllo dei privilegi</summary>
        public static bool bCheckIf_CassaPri_and_NDB()
        {
            return ((SF_Data.iNumCassa == CASSA_PRINCIPALE) && _bUSA_NDB());
        }

        /// <summary>
        /// da utilizzare sopratutto per i caricamenti di:<br/>
        /// CaricaListino, CaricaDisponibilità
        /// </summary>
        public static bool bCheckIf_CassaSec_and_NDB()
        {
            return ((SF_Data.iNumCassa > CASSA_PRINCIPALE) && _bUSA_NDB());
        }

        /// <summary>ottiene numero complessivo Ordini emessi</summary>
        public static int iGetNumOfOrders()
        {
            int iNum;

            if (_bUSA_NDB() && _bCheckStatus)
            {
                // da eseguire solo se _bCheckStatus = true
                iNum = _rdBaseIntf.dbGetNumOfOrdersFromDB();
            }
            else
                // fornito da CaricaDatidaOrdini()
                iNum = SF_Data.iNumOfLastReceipt;

            return iNum;
        }

        /// <summary>ottiene numero dell'ultimo ordine emesso da questa cassa</summary>
        public static int iGetNumOfLocalOrders()
        {
            int iNum;

            if (!_bCheckStatus)
                return 0;

            if (_bUSA_NDB())
                iNum = dBaseIntf.dbGetNumOfLocalOrdersFromDB();
            else
                iNum = SF_Data.iNumOfLastReceipt;

            return iNum;
        }

        /// <summary>ottiene data ed ora per l'emissione dello scontrino</summary>
        static String sGetTicketTime()
        {
            String sDateTime;

            if (_bUSA_NDB())
                sDateTime = dBaseIntf.dbGetDateTimeFromDB();
            else
                sDateTime = GetDateTimeString();

            return sDateTime;
        }

        /// <summary>funzione per il caricamento dell'ordine in prevendita/// </summary>
        /// <param name="sOrdiniPrevTableParam"></param>
        /// <param name="iNumParam"></param>
        /// <returns></returns>
        public static bool caricaOrdinePrev(int iNumParam, String sOrdiniPrevTableParam)
        {
            bool bDbRead_Ok, bMatch, bSingleWarn;
            int i, j, iDebug;
            String sDebug;
            String[] sQueue_Object = new String[2];

            DataManager.clearGrid();

            bDbRead_Ok = false;
            SF_Data.iNumOrdinePrev = iNumParam;

            bDbRead_Ok = _rdBaseIntf.dbCaricaOrdine(getActualDate(), iNumParam, false, sOrdiniPrevTableParam);

            if (!bDbRead_Ok)
            {
                // caricamento ordine fallito
                _WrnMsg.iErrID = WRN_DBE;
                _WrnMsg.sMsg = String.Format("dbCaricaOrdine fallito:\n\ntabella {0} record n. {1}", sOrdiniPrevTableParam, iNumParam);
                WarningManager(_WrnMsg);

                LogToFile("caricaOrdinePrev : rdbCaricaOrdine");
                return false;
            }

            // se è gia annullato
            if (DB_Data.bAnnullato)
            {
                _WrnMsg.sMsg = "di prevendita n." + iNumParam.ToString();

                _WrnMsg.iErrID = WRN_RAN;
                WarningManager(_WrnMsg);
                LogToFile("caricaOrdinePrev : record di Prevendita " + _WrnMsg.sMsg + " annullato !");

                return false;
            }
            else if (DB_Data.bScaricato) // se è gia scaricato
            {
                _WrnMsg.sMsg = "di prevendita n." + iNumParam.ToString();

                _WrnMsg.iErrID = WRN_RPS;
                WarningManager(_WrnMsg);
                LogToFile("caricaOrdinePrev : record di Prevendita " + _WrnMsg.sMsg + " già scaricato !");

                return false;
            }

            /************************************
             *		controllo di sicurezza
             ************************************/
            bSingleWarn = false;

            for (j = 0; j < MAX_NUM_ARTICOLI; j++)
            {
                if (String.IsNullOrEmpty(DB_Data.Articolo[j].sTipo))
                    continue;
                else
                {
                    bMatch = false;
                    sDebug = DB_Data.Articolo[j].sTipo;
                    iDebug = DB_Data.Articolo[j].iQuantitaOrdine;

                    for (i = 0; i < MAX_NUM_ARTICOLI; i++)
                    {
                        // caricamento in Articolo[], esclusi Prezzi Unitario e Scontato, iGruppo_Stampa
                        if (SF_Data.Articolo[i].sTipo == DB_Data.Articolo[j].sTipo)
                        {
                            SF_Data.Articolo[i].iQuantitaOrdine = DB_Data.Articolo[j].iQuantitaOrdine;

                            bMatch = true;
                            break;
                        }
                    }

                    // dà un solo avviso alla prima discordanza
                    if (bDbRead_Ok && !bMatch && !bSingleWarn)
                    {
                        _ErrMsg.sMsg = DB_Data.Articolo[j].sTipo;
                        _ErrMsg.iErrID = WRN_MNF;
                        WarningManager(_ErrMsg);
                        bSingleWarn = true;
                    }
                }
            }

            SF_Data.iStatusReceipt = SetBit(DB_Data.iStatusReceipt, BIT_CARICATO_DA_PREVENDITA);
            SF_Data.sPrevDateTime = DB_Data.sDateTime;

            if (IsBitSet(DB_Data.iStatusReceipt, BIT_ESPORTAZIONE))
                rFrmMain.BtnEsportazione_Click(null, null);

            rFrmMain.setEditCoperto(DB_Data.Articolo[MAX_NUM_ARTICOLI - 1].iQuantitaOrdine.ToString());
            rFrmMain.setEditNota(DB_Data.sNota);

            AnteprimaDlg.rAnteprimaDlg.Show();
            AnteprimaDlg.rAnteprimaDlg.redrawReceipt();

            // avvia la visualizzazione della tabella
            sQueue_Object[0] = PREV_ORDER_LOAD_DONE;
            sQueue_Object[1] = "";

            FrmMain.eventEnqueue(sQueue_Object);

            return true;
        }

        static int _iPrevOrder = 0;

        /// <summary>funzione per il caricamento dell'ordine web/// </summary>
        public static bool caricaOrdineWeb(int iNumParam)
        {
            bool bDbRead_Ok, bMatch, bSingleWarn;
            int i, j, iDebug;
            String sDebug, sTmp;
            String[] sQueue_Object = new String[2];

            DataManager.clearGrid();

            ulStart = (ulong)Environment.TickCount;

            SF_Data.iNumOrdineWeb = iNumParam;

            bDbRead_Ok = dBaseTunnel_my.rdbCaricaOrdine(iNumParam);

            ulStop = (ulong)Environment.TickCount;
            ulPingTime = ulStop - ulStart;
            sTmp = String.Format("caricaOrdineWeb : {0} ms", ulPingTime);
            LogToFile(sTmp);
            Console.WriteLine(sTmp);

            if (!bDbRead_Ok && (_iPrevOrder != DB_Data.iNumOrdineWeb))
            {
                // caricamento ordine fallito
                _iPrevOrder = DB_Data.iNumOrdineWeb;
                _WrnMsg.iErrID = WRN_DBE;
                _WrnMsg.sMsg = String.Format("caricaOrdineWeb fallito:\n\nrecord n. {0}", iNumParam);
                WarningManager(_WrnMsg);

                LogToFile("caricaOrdineWeb : rdbCaricaOrdine");
                return false;
            }

            // se è gia annullato
            if (DB_Data.bAnnullato && (_iPrevOrder != DB_Data.iNumOrdineWeb))
            {
                _iPrevOrder = DB_Data.iNumOrdineWeb;

                _WrnMsg.sMsg = "di ordine web n." + iNumParam.ToString();

                _WrnMsg.iErrID = WRN_RAN;
                WarningManager(_WrnMsg);
                LogToFile("caricaOrdineWeb : record di ordine web " + _WrnMsg.sMsg + " annullato !");

                return false;
            }
            else if (DB_Data.bStampato && (_iPrevOrder != DB_Data.iNumOrdineWeb)) // se è gia stampato
            {
                _iPrevOrder = DB_Data.iNumOrdineWeb;

                _WrnMsg.sMsg = "di ordine web n." + iNumParam.ToString();
                _WrnMsg.iErrID = WRN_RPS;
                WarningManager(_WrnMsg);
                LogToFile("caricaOrdineWeb : record di ordine web " + _WrnMsg.sMsg + " già stampato !");

                return false;
            }

            /************************************
             *		controllo di sicurezza
             ************************************/
            if (_sWebListinoChecksum == DB_Data.sPL_Checksum)
            {
                bSingleWarn = false;

                for (j = 0; j < MAX_NUM_ARTICOLI; j++)
                {
                    if (String.IsNullOrEmpty(DB_Data.Articolo[j].sTipo))
                        continue;
                    else
                    {
                        bMatch = false;
                        sDebug = DB_Data.Articolo[j].sTipo;
                        iDebug = DB_Data.Articolo[j].iQuantitaOrdine;

                        for (i = 0; i < MAX_NUM_ARTICOLI; i++)
                        {
                            // caricamento in Articolo[], esclusi Prezzi Unitario e Scontato, iGruppo_Stampa
                            if (SF_Data.Articolo[i].sTipo == DB_Data.Articolo[j].sTipo)
                            {
                                SF_Data.Articolo[i].iQuantitaOrdine = DB_Data.Articolo[j].iQuantitaOrdine;

                                bMatch = true;
                                break;
                            }
                        }

                        // dà un solo avviso alla prima discordanza
                        if (bDbRead_Ok && !bMatch && !bSingleWarn)
                        {
                            _ErrMsg.sMsg = DB_Data.Articolo[j].sTipo;
                            _ErrMsg.iErrID = WRN_MNF;
                            WarningManager(_ErrMsg);
                            bSingleWarn = true;
                        }
                    }
                }

                SF_Data.iStatusReceipt = SetBit(DB_Data.iStatusReceipt, BIT_CARICATO_DA_WEB);
                SF_Data.sWebDateTime = DB_Data.sDateTime;

                if (IsBitSet(SF_Data.iStatusReceipt, BIT_ESPORTAZIONE))
                    rFrmMain.BtnEsportazione_Click(null, null);

                rFrmMain.setEditCoperto(SF_Data.Articolo[MAX_NUM_ARTICOLI - 1].iQuantitaOrdine.ToString());

                rFrmMain.setEditTavolo(DB_Data.sTavolo);
                rFrmMain.setEditNome(DB_Data.sNome);
                rFrmMain.setEditNota(DB_Data.sNota);

                bool bEsploraAuto = false;

                if ((EsploraRemOrdiniDB_Dlg.rEsploraRemOrdiniDB_Dlg != null) && EsploraRemOrdiniDB_Dlg.rEsploraRemOrdiniDB_Dlg.bGetAutoCheckbox())
                    bEsploraAuto = true;

                // anteprima solo se non è ordine Automatico
                if (!IsBitSet(SF_Data.iStatusReceipt, BIT_ORDINE_DIRETTO_DA_WEB) || !bEsploraAuto)
                    AnteprimaDlg.rAnteprimaDlg.Show();

                AnteprimaDlg.rAnteprimaDlg.redrawReceipt();

                // avvia la visualizzazione della tabella
                sQueue_Object[0] = WEB_ORDER_LOAD_DONE;
                sQueue_Object[1] = "";

                FrmMain.eventEnqueue(sQueue_Object);

                return true;
            }
            else
            {
                if (_iPrevOrder != DB_Data.iNumOrdineWeb)
                {
                    _iPrevOrder = DB_Data.iNumOrdineWeb;

                    _WrnMsg.sMsg = DB_Data.iNumOrdineWeb.ToString();
                    _WrnMsg.iErrID = WRN_CKWO;
                    WarningManager(_WrnMsg);
                }

                return false;
            }
        }

        /// <summary>funzione per il caricamento dell'ordine mediante QRCode,<br/>
        /// non c'è il match ARTICOLI dato che proprio non sono presenti nel json
        /// </summary>
        public static bool caricaOrdine_QR_code(int iNumParam)
        {
            int i;
            String[] sQueue_Object = new String[2];

            DataManager.clearGrid();

            SF_Data.iNumOrdineWeb = iNumParam;

            // se è gia annullato
            if (DB_Data.bAnnullato)
            {
                _WrnMsg.sMsg = "di ordine web n." + iNumParam.ToString();

                _WrnMsg.iErrID = WRN_RAN;
                WarningManager(_WrnMsg);
                LogToFile("caricaOrdineWeb : record di ordine web " + _WrnMsg.sMsg + " annullato !");

                return false;
            }
            else if (DB_Data.bStampato) // se è gia bStampato
            {
                _WrnMsg.sMsg = "di ordine web n." + iNumParam.ToString();

                _WrnMsg.iErrID = WRN_RPS;
                WarningManager(_WrnMsg);
                LogToFile("caricaOrdineWeb : record di ordine web " + _WrnMsg.sMsg + " già elaborato !");

                return false;
            }

            /************************************
             *		controllo di sicurezza
             ************************************/

            if (_sWebListinoChecksum == DB_Data.sPL_Checksum)
            {
                for (i = 0; i < MAX_NUM_ARTICOLI; i++)
                {
                    if (DB_Data.Articolo[i].iQuantitaOrdine > 0) // debug
                        SF_Data.Articolo[i].iQuantitaOrdine = DB_Data.Articolo[i].iQuantitaOrdine;
                }
            }
            else
            {
                _ErrMsg.sMsg = DB_Data.iNumOrdineWeb.ToString();
                _ErrMsg.iErrID = WRN_CKWO;
                WarningManager(_ErrMsg);
                return false;
            }

            SF_Data.iStatusReceipt = SetBit(DB_Data.iStatusReceipt, BIT_CARICATO_DA_WEB);
            SF_Data.sWebDateTime = DB_Data.sDateTime;

            if (IsBitSet(DB_Data.iStatusReceipt, BIT_ESPORTAZIONE))
                rFrmMain.BtnEsportazione_Click(null, null);

            rFrmMain.setEditCoperto(DB_Data.Articolo[MAX_NUM_ARTICOLI - 1].iQuantitaOrdine.ToString());

            rFrmMain.setEditNome(DB_Data.sNome);
            rFrmMain.setEditTavolo(DB_Data.sTavolo);
            rFrmMain.setEditNota(DB_Data.sNota);

            AnteprimaDlg.rAnteprimaDlg.Show();
            AnteprimaDlg.rAnteprimaDlg.redrawReceipt();

            // avvia la visualizzazione della tabella
            sQueue_Object[0] = WEB_ORDER_LOAD_DONE;
            sQueue_Object[1] = "";

            FrmMain.eventEnqueue(sQueue_Object);

            return true;
        }

    } // end class
} // end namespace

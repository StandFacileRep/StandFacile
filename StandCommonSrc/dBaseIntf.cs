/*****************************************************************************************
	NomeFile : StandCommonSrc/dBaseIntf.cs
	Data	 : 06.12.2024
	Autore   : Mauro Artuso

 *****************************************************************************************/

using System;
using System.Net;
using System.Collections.Generic;

using StandFacile_DB;
using static StandFacile.glb;

#if STANDFACILE
using static StandFacile_DB.dBaseIntf_ql;
#endif

using static StandFacile_DB.dBaseIntf_my;
using static StandFacile_DB.dBaseIntf_pg;

using static StandCommonFiles.ComDef;
using static StandCommonFiles.ComSafe;
using static StandCommonFiles.CommonCl;
using static StandCommonFiles.LogServer;

namespace StandFacile
{
#pragma warning disable IDE1006

    /// <summary>
    /// classe che contiene le variabili comuni ai 3 DB: SQLite, MySQL, PostGreSQL <br/>
    ///  <br/>
    /// invece le funzioni vengono tutte reindirizzate alla classe del DB specifico <br/>
    /// </summary>
    public class dBaseIntf
    {
        // *** evitare maiuscole ***

        /// <summary>nome della tabella del Listino</summary>
        public const String NOME_LISTINO_DBTBL = RELEASE_TBL + "_listino";
        /// <summary>nome della tabella del test</summary>
        public const String NOME_TEST_DBTBL = RELEASE_TBL + "_test_sequence";

        /// <summary>nome tabella per la gestione del numero progressivo degli ordini</summary>
        public const string NOME_NSC_DBTBL = RELEASE_TBL + "_num_ordini";
        /// <summary>nome tabella per la gestione del numero progressivo dei messaggi</summary>
        public const string NOME_NMSG_DBTBL = RELEASE_TBL + "_num_messaggi";
        /// <summary>nome tabella per la gestione disponibilità degli ordini emessi <br/>
        /// dalle casse secondarie</summary>
        public const string NOME_DISP_DBTBL = RELEASE_TBL + "_ordini_csec";
        /// <summary>nome tabella per lo scarico degli ordini web emessi </summary>
        public const string NOME_WEBORD_DBTBL = RELEASE_TBL + "_ordini_web";

        // variabili membro DB

        /// <summary>accesso al database: nome del server</summary>
        public static String _sDB_ServerName;
        /// <summary>accesso al database: password</summary>
        public static String _password;

        /// <summary>variabile che indica se è usato il network DB (MySQL, PostGreSQL) o no (SQLite)</summary>
        private static bool _bUsa_NDB;
        /// <summary>variabile che descrive il tipo di database (SQLite, MySQL, PostGreSQL)</summary>
        public static int _iNDbMode;

        /// <summary>variabile che descrive la lunghezza della stringa _sDBTNameOrdini sensa estensione da chiusura</summary>
        public static int _iDBTNameOrdiniLength;

        /// <summary>variabile che descrive la lunghezza degli Articoli letti dal DB</summary>
        public static bool _iDBArticoliLength_Is33;

        /// <summary>numero degli ordini emessi da questa cassa</summary>
        public static int _iNumOfLocalOrdersFromDB;
        /// <summary>numero degli messaggi emessi da questa cassa</summary>
        public static int _iNumOfLastMessageFromDB;

        /// <summary>data ed ora proveniente dalle tabelle del DB</summary>
        public static String _sDateTimeFromDB;

        /// <summary>data proveniente dalla tabella di stato del DB</summary>
        public static String _sDateFromDB;

        /// <summary>nome della tabella dati</summary>
        public static String _sDBTNameDati;
        /// <summary>nome della tabella ordini</summary>
        public static String _sDBTNameOrdini;

        /// <summary>struct per la gestione degli avvisi e/o errori</summary>
        public static TErrMsg _ErrMsg, _WrnMsg;

        /// <summary>copia del risultato di dbCheckStatus()/// </summary>
        public static bool _bCheckStatus;

        /// <summary>riferimento a dBaseIntf</summary>
        public static dBaseIntf _rdBaseIntf;

        /// <summary>Struct fondamentale per i dati del DB</summary>
        public static TData DB_Data = new TData(0);

#if STAND_CUCINA

        /// <summary> flag di cambio data utilizzato da STAND_CUCINA </summary>
        public static bool _bStatusDate_IsChanged = false;

        /// <summary> data precedente utilizzata da STAND_CUCINA </summary>
        public static String _sPrevStatus_Date;

        /// <summary>ottiene flag di cambio data DB</summary>
        public static bool Get_StatusDate_IsChanged() { return _bStatusDate_IsChanged; }

        /// <summary>imposta flag di cambio data DB</summary>
        public static void Reset_StatusDate_Changed() { _bStatusDate_IsChanged = false; }

        /// <summary>esegue check della connessione al DB</summary>
        public bool dbFeedbackCheck() { return dbCheck(_sDB_ServerName, _password, _iNDbMode, false); }

        /// <summary>esegue check silente della connessione al DB</summary>
        public void dbSilentCheck() { dbCheck(_sDB_ServerName, _password, _iNDbMode, true); }

#endif

#if STAND_ORDINI
        /// <summary>elenco degli ultimi scontrini emessi</summary>
        public static readonly String[] _sNumScontrino = new String[Define.MAX_RIGHE * 2];
#endif

#if STAND_MONITOR

        /// <summary>numero degli ultimi scontrini emessi</summary>
        public const int NUM_ULTIMI_SCONTRINI = 5;
        /// <summary>stringa per facilitare visualizzazione tabella ordini</summary>
        public const string ORDER_START_STR = "   *** order_start ***";

        /// <summary>array degli ultimi scontrini emessi</summary>
        public static readonly String[] _sNumScontrino = new String[NUM_ULTIMI_SCONTRINI];
        /// <summary>stringa del tempo di attesa </summary>
        public static String _sAttesaMedia = "xx";

        /// <summary>ottiene lo scontrino specifico</summary>
        public static String GetNumScontrino(int i) { return _sNumScontrino[i]; }

        /// <summary>ottiene l'attesa media</summary>
        public static String GetAttesaMedia() { return _sAttesaMedia; }

        /// <summary> flag per gestione errori </summary>
        public static bool _bErrorePrimaVolta = true;

        /// <summary> numero di scontrino per STAND_MONITOR </summary>
        public static int _iNumScontrini;

        /// <summary>ottiene il numero di scontrini emessi</summary>
        public static int GetNumScontrini() { return _iNumScontrini; } //
        /// <summary>ottiene l'indicazione di primo errore</summary>
        public static bool GetErrorePrimaVolta() { return _bErrorePrimaVolta; } //
        /// <summary>reset indicazione di primo errore</summary>
        public static void ClearErrorePrimaVolta() { _bErrorePrimaVolta = false; } //

#endif
        /// <summary>funzione che indica se è usato un DB di rete: MySQL, PostGreSQL</summary>
        public static bool bUSA_NDB() { return _bUsa_NDB; } //

        /// <summary>funzione che indica quale DB è usato 0:SQLite, 1:MySQL, 2:PostGreSQL</summary>
        public static int iUSA_NDB() { return _iNDbMode; } //

        /// <summary>funzione di verifica della connessione al DB di rete</summary>
        public bool dbCheck() { return dbCheck(_sDB_ServerName, _password, _iNDbMode); }

        /// <summary>controllo stato della connessione</summary>
        public bool GetDB_Connected()
        {
            if (_iNDbMode == (int)DB_MODE.MYSQL)
                return _rdBaseIntf_my.GetDB_Connected();
            else
            if (_iNDbMode == (int)DB_MODE.POSTGRES)
                return _rdBaseIntf_pg.GetDB_Connected();
            else
                return false;
        }

        /// <summary>ottiene ora e data dal DB</summary>
        public static string dbGetDateTimeFromDB() { return _sDateTimeFromDB; }

        /// <summary>ottiene la data dal DB</summary>
        public static string dbGetDateFromDB() { return _sDateFromDB; }

        /// <summary>ottiene il nome del DB server</summary>
        public static string GetDB_ServerName() { return _sDB_ServerName; }

        /// <summary>ottiene dal DB il numero degli ordini emessi da questa cassa</summary>
        public static int dbGetNumOfLocalOrdersFromDB() { return _iNumOfLocalOrdersFromDB; }

        // <summary>ottiene dal DB il numero dei messaggi emessi da questa cassa</summary>
        // public static int dbGetNumOfLastMessageFromDB() { return _iNumOfLastMessageFromDB; }

        /// <summary>imposta il numero degli ordini complessivi emessi</summary>
        public static void dbSetNumOfLastOrderFromDB(int iParam) { _iNumOfLocalOrdersFromDB = iParam; }
        /// <summary>imposta il numero dei messaggi complessivi emessi</summary>
        public static void dbSetNumOfLastMessageFromDB(int iParam) { _iNumOfLastMessageFromDB = iParam; }

        /// <summary>
        /// ottiene dal DB la lunghezza degli Articoli<br/>
        /// usata da VisOrdini(), VisDati()
        /// </summary>
        public static bool dbGetLengthArticoli() { return _iDBArticoliLength_Is33; }

        /// <summary>costruttore</summary>
        public dBaseIntf()
        {
            DateTime dateParam = GetActualDate();

            _rdBaseIntf = this;

            dbAzzeraDatiGen();
            dbAzzeraDatiOrdine();

            _sDB_ServerName = ReadRegistry(DBASE_SERVER_NAME_KEY, Dns.GetHostName());
            _password = Decrypt(ReadRegistry(DBASE_PASSWORD_KEY, DBASE_LAN_PASSWORD));
#if STANDFACILE
            _iNDbMode = ReadRegistry(DB_MODE_KEY, (int)DB_MODE.SQLITE); // punto unico
#else
            _iNDbMode = ReadRegistry(DB_MODE_KEY, (int)DB_MODE.MYSQL); // punto unico
#endif

#if STAND_CUCINA || STAND_ORDINI || STAND_MONITOR

            _bUsa_NDB = true;

            LogToFile(String.Format("dBaseForm : tipo db = {0}", _iNDbMode));
            return;

#elif STANDFACILE

            bool bDBConnection_Ok = false;

            _bUsa_NDB = (ReadRegistry(DB_MODE_KEY, 0) > 0); // punto doppio con DataManager

            LogToFile(String.Format("dBaseForm : tipo db = {0}", _iNDbMode));

            bDBConnection_Ok = dbInit(dateParam, SF_Data.iNumCassa, true); // silent

            if (!bDBConnection_Ok)
            {
                // fallisce il test della connessione al database
                if (_bUsa_NDB)
                {
                    _WrnMsg.sMsg = _sDB_ServerName; // inizializzato in dbInit()
                    _WrnMsg.iErrID = WRN_TDF;
                }
                else
                {
                    _WrnMsg.sMsg = "SQLite";
                    _WrnMsg.iErrID = WRN_TCF;
                }

                WarningManager(_WrnMsg);

                // connessione non possibile al database
                LogToFile("dBaseForm : costruttore dbException");
                return;
            }
#else
            _bUsa_NDB = false;
#endif
        }

        /// <summary> imposta i parametri della connessione letta dallo Stato NOME_STATO_DBTBL </summary>
        public static void SetDbMode(String sServerParam, String sPwdParam, int iDbModePrm)
        {
            _sDB_ServerName = sServerParam;
            _iNDbMode = iDbModePrm;
            _password = sPwdParam;

#if STAND_CUCINA
            if (_iNDbMode != iDbModePrm)
            {
                // altrimenti stampa scontrini a raffica
                _bStatusDate_IsChanged = true;
            }
#endif
        }

        /// <summary>
        /// Inizializzazione connessione in base a dateParam, iNumCassaParam <br/>
        /// imposta _sDBTNameDati, _sDBTNameOrdini <br/>
        /// se sNomeTabellaParam è vuota <br/>
        /// _sDBTNameDati viene impostato in base a dateParam, iNumCassaParam <br/>
        ///      altrimenti in base a sNomeTabellaParam <br/>
        /// ritorna false = errore, true = connessione effettuata <br/>
        /// </summary>
        public bool dbInit(DateTime dateParam, int iNumCassaParam, bool bSilentParam = false, String sNomeTabellaParam = "")
        {
            bool bResult;

#if STANDFACILE
            if (_iNDbMode == (int)DB_MODE.SQLITE)
                bResult = _rdBaseIntf_ql.dbInit(dateParam, bSilentParam, sNomeTabellaParam);
            else
#endif
            if (_iNDbMode == (int)DB_MODE.MYSQL)
                bResult = _rdBaseIntf_my.dbInit(dateParam, iNumCassaParam, bSilentParam, sNomeTabellaParam);
            else
                bResult = _rdBaseIntf_pg.dbInit(dateParam, iNumCassaParam, bSilentParam, sNomeTabellaParam);

            return bResult;
        }

        /// <summary>
        /// azzera DB_Data[] per la parte dello stato
        /// </summary>
        public static void dbAzzeraDatiGen()
        {
            int i;

            DB_Data.iReceiptCopyOptions = 0;
            DB_Data.iGridCols = DEF_GRID_NCOLS;
            DB_Data.iGridRows = DEF_GRID_NROWS;
            DB_Data.iNumOfLastReceipt = 0;
            //DB_Data.iStartingNumOfReceipts = 0; // non varia
            DB_Data.iActualNumOfReceipts = 0;
            DB_Data.iNumOfMessages = 0;
            DB_Data.iNumOfWebReceipts = 0;
            DB_Data.iNumAnnullati = 0;
            DB_Data.iTotaleIncasso = 0;
            DB_Data.iTotaleIncassoCard = 0;
            DB_Data.iTotaleIncassoSatispay = 0;
            DB_Data.iTotaleScontatoStd = 0;
            DB_Data.iTotaleScontatoFisso = 0;
            DB_Data.iTotaleScontatoGratis = 0;
            DB_Data.iTotaleAnnullato = 0;
            DB_Data.sVersione = SF_Data.sVersione;

            for (i = 0; i < PAGES_NUM_TABM; i++)
                DB_Data.sPageTabs[i] = ""; // non usati dal DB

            for (i = 0; i < MAX_NUM_HEADERS; i++)
                DB_Data.sHeaders[i] = "";

            for (i = 0; i < NUM_COPIES_GRPS; i++)
                DB_Data.sCopiesGroupsText[i] = sConstCopiesGroupsText[i];

            for (i = 0; i < NUM_EDIT_GROUPS; i++)
            {
                DB_Data.iGroupsColor[i] = 0;
                DB_Data.bCopiesGroupsFlag[i] = false;
            }

            for (i = 0; i < NUM_GROUPS_COLORS - 1; i++)
                DB_Data.sColorGroupsText[i] = sConstColorsGroupsText[i];

            // record di gestione dei coperti posto nell'ultimo articolo
            DB_Data.Articolo[MAX_NUM_ARTICOLI - 1].iGruppoStampa = (int)DEST_TYPE.DEST_COUNTER;
            DB_Data.Articolo[MAX_NUM_ARTICOLI - 1].iPrezzoUnitario = 0;
            DB_Data.Articolo[MAX_NUM_ARTICOLI - 1].sTipo = _COPERTO;
        }

        /// <summary>
        /// azzera DB_Data[] per la parte per la parte articolo
        /// e dati legati al singolo ordine
        /// </summary>
        public static void dbAzzeraDatiOrdine()
        {
            int i;

            DB_Data.iNumCassa = 1;

            DB_Data.iTotaleReceipt = 0;
            DB_Data.iTotaleReceiptDovuto = 0;
            DB_Data.iStatusReceipt = 0;
            DB_Data.iScontoStdReceipt = 0;
            DB_Data.iScontoFissoReceipt = 0;
            DB_Data.iScontoGratisReceipt = 0;
            DB_Data.iStatusSconto = 0;
            DB_Data.sScontoReceipt = "";
            DB_Data.bAnnullato = false;
            DB_Data.bScaricato = false;
            DB_Data.bStampato = false;

            DB_Data.sTavolo = "";
            DB_Data.sNome = "";
            DB_Data.sNota = "";
            DB_Data.sDateTime = GetDateTimeString();
            DB_Data.sWebDateTime = "";
            DB_Data.sPrevDateTime = "";
            DB_Data.sMessaggio = "";

            DB_Data.iNumOrdinePrev = 0;
            DB_Data.iNumOrdineWeb = 0;

            for (i = 0; i < MAX_NUM_ARTICOLI + EXTRA_NUM_ARTICOLI; i++) // azzeramento
            {
                DB_Data.Articolo[i].bLocalPrinted = false;
                DB_Data.Articolo[i].iPrezzoUnitario = 0;
                DB_Data.Articolo[i].iQuantita_Scaricata = 0;
                DB_Data.Articolo[i].iGruppoStampa = 0;
                DB_Data.Articolo[i].iOptionsFlags = 0;
                DB_Data.Articolo[i].iQuantitaOrdine = 0;
                DB_Data.Articolo[i].iIndexListino = 0;
                DB_Data.Articolo[i].iQuantitaVenduta = 0;
                DB_Data.Articolo[i].iQtaEsportata = 0;
                DB_Data.Articolo[i].iDisponibilita = DISP_OK;
                DB_Data.Articolo[i].sTipo = "";
                DB_Data.Articolo[i].sNotaArt = "";
            }
        }

        /// <summary>
        /// funzione di controllo e reset del record di stato contenente la data <br/>
        /// (solo da CASSA_PRINCIPALE e con bResetParam == true) <br/> <br/>
        /// 
        /// ritorna true se la verifica della data è corretta, false altrimenti <br/>
        /// utilizzata da : dbNewOrdineNumRequest, DataManager.Init <br/>
        /// </summary>
        public bool dbCheckStatus(bool bResetParam = false)
        {
#if STANDFACILE
            if (_iNDbMode == (int)DB_MODE.SQLITE)
                return _rdBaseIntf_ql.dbCheckStatus(bResetParam);
            else
#endif
            if (_iNDbMode == (int)DB_MODE.MYSQL)
                return _rdBaseIntf_my.dbCheckStatus(bResetParam);
            else
                return _rdBaseIntf_pg.dbCheckStatus(bResetParam);
        }


        /// <summary>
        ///   Utilizzata per resettare la tabella num_ordini con il numero <br/>
        ///   progressivo degli ordini, utilizza AUTO_INCREMENT <br/> <br/>
        /// 
        ///  utilizzata da dbCheckStatus, TDataManager::CaricaDati <br/>
        /// </summary>
        public bool dbResetNumOfOrders(int iInitialNumOfReceiptsParam)
        {
            if (_iNDbMode == (int)DB_MODE.MYSQL)
                return _rdBaseIntf_my.dbResetNumOfOrders(iInitialNumOfReceiptsParam);
            else
            if (_iNDbMode == (int)DB_MODE.POSTGRES)
                return _rdBaseIntf_pg.dbResetNumOfOrders(iInitialNumOfReceiptsParam);
            else
                return false;
        }

        /// <summary>
        ///   Utilizzata per resettare la tabella num_messaggi con il numero <br/>
        ///   progressivo degli ordini, utilizza AUTO_INCREMENT <br/> <br/>
        /// 
        ///  utilizzata da dbCheckStatus, TDataManager::CaricaDati <br/>
        /// </summary>
        public bool dbResetNumOfMessages(int iInitialNumOfMessagesParam)
        {
            if (_iNDbMode == (int)DB_MODE.MYSQL)
                return _rdBaseIntf_my.dbResetNumOfMessages(iInitialNumOfMessagesParam);
            else
            if (_iNDbMode == (int)DB_MODE.POSTGRES)
                return _rdBaseIntf_pg.dbResetNumOfMessages(iInitialNumOfMessagesParam);
            else
                return false;
        }

        /// <summary>
        /// Funzione di lettura del numero di Ordini ancora da stampare da parte di STAND_CUCINA<br/>
        /// bModeNextParam sceglie se ottenere i successivi o i precedenti
        /// </summary>
        public int dbGetNumOfPrintedOrders(int iNumOfReceiptParam, bool bModeNextParam = true)
        {
            if (_iNDbMode == (int)DB_MODE.MYSQL)
                return _rdBaseIntf_my.dbGetNumOfPrintedOrders(iNumOfReceiptParam, bModeNextParam);
            else
            if (_iNDbMode == (int)DB_MODE.POSTGRES)
                return _rdBaseIntf_pg.dbGetNumOfPrintedOrders(iNumOfReceiptParam, bModeNextParam);
            else
                return 0;
        }

        /// <summary>
        /// Funzione di lettura del numero di Ordini emessi dalle varie casse<br/>
        /// con 2 diverse modalità
        /// </summary>
        public int dbGetNumOfOrdersFromDB(bool bModeParamStandard = true)
        {
            if (_iNDbMode == (int)DB_MODE.MYSQL)
                return _rdBaseIntf_my.dbGetNumOfOrdersFromDB(bModeParamStandard);
            else
            if (_iNDbMode == (int)DB_MODE.POSTGRES)
                return _rdBaseIntf_pg.dbGetNumOfOrdersFromDB(bModeParamStandard);
            else
                return 0;
        }

        /// <summary>
        /// Funzione di lettura del numero di Messaggi emessi
        /// con 2 diverse modalità
        /// </summary>
        public int dbGetNumOfMessagesFromDB(bool bModeParamStandard = true)
        {
            if (_iNDbMode == (int)DB_MODE.MYSQL)
                return _rdBaseIntf_my.dbGetNumOfMessagesFromDB(bModeParamStandard);
            else
            if (_iNDbMode == (int)DB_MODE.POSTGRES)
                return _rdBaseIntf_pg.dbGetNumOfMessagesFromDB(bModeParamStandard);
            else
                return 0;
        }

        /// <summary>funzione di lettura parametri per accesso database remoto</summary>
        public TWebServerParams dbGetWebServerParams()
        {
            if (_iNDbMode == (int)DB_MODE.MYSQL)
                return _rdBaseIntf_my.dbGetWebServerParams();
            else
            if (_iNDbMode == (int)DB_MODE.POSTGRES)
                return _rdBaseIntf_pg.dbGetWebServerParams();
            else
#if STANDFACILE
                return _rdBaseIntf_ql.dbGetWebServerParams();
#else
                return new TWebServerParams(0);
#endif
        }

        /// <summary>funzione di scrittura parametri per accesso database remoto</summary>
        public bool dbSetWebServerParams(TWebServerParams sWebServerParams)
        {
            if (_iNDbMode == (int)DB_MODE.MYSQL)
                return _rdBaseIntf_my.dbSetWebServerParams(sWebServerParams);
            else
            if (_iNDbMode == (int)DB_MODE.POSTGRES)
                return _rdBaseIntf_pg.dbSetWebServerParams(sWebServerParams);
            else
#if STANDFACILE
                return _rdBaseIntf_ql.dbSetWebServerParams(sWebServerParams);
#else
                return false;
#endif
        }

        /// <summary>
        ///  Funzione di caricamento del listino dal database da parte della sola cassa secondaria <br/>
        ///  ritorna il numero di stringhe caricate
        /// </summary>
        /// <returns>il numero di elementi della Lista sStringsParam</returns>
        public int dbCaricaListino(List<string> sStringsParam)
        {
            if (_iNDbMode == (int)DB_MODE.MYSQL)
                return _rdBaseIntf_my.dbCaricaListino(sStringsParam);
            else
            if (_iNDbMode == (int)DB_MODE.POSTGRES)
                return _rdBaseIntf_pg.dbCaricaListino(sStringsParam);
            else
                return 0;
        }

        /// <summary>
        ///  Funzione di caricamento del Test dal database da parte della sola cassa secondaria <br/>
        ///  ritorna il numero di stringhe caricate
        /// </summary>
        /// <returns>il numero di elementi della Lista sStringsParam</returns>
        public int dbCaricaTest(List<string> sStringsParam)
        {
            if (_iNDbMode == (int)DB_MODE.MYSQL)
                return _rdBaseIntf_my.dbCaricaTest(sStringsParam);
            else
            if (_iNDbMode == (int)DB_MODE.POSTGRES)
                return _rdBaseIntf_pg.dbCaricaTest(sStringsParam);
            else
                return 0;
        }

        /// <summary>
        /// carica i Dati nella struct DB_Articolo[] prendendoli per maggiore sicurezza dalla tabella degli ordini <br/>
        /// se iNumCassaParam == 0 considera tutte le casse <br/> <br/>
        /// iReportParam > 0 considera il tipo di sconto applicato
        /// usata da DataManager, VisDatiDlg()<br/> <br/>
        /// 
        /// ritorna DB_Data.iNumOfReceipts se ha successo, -1 altrimenti
        /// </summary>
        public int dbCaricaDatidaOrdini(DateTime dateParam, int iNumCassaParam, bool bSilentParam = false, String sNomeTabellaParam = "", int iReportParam = 0)
        {
#if STANDFACILE
            if (_iNDbMode == (int)DB_MODE.SQLITE)
                return _rdBaseIntf_ql.dbCaricaDatidaOrdini(dateParam, bSilentParam, sNomeTabellaParam, iReportParam);
            else
#endif
            if (_iNDbMode == (int)DB_MODE.MYSQL)
                return _rdBaseIntf_my.dbCaricaDatidaOrdini(dateParam, iNumCassaParam, bSilentParam, sNomeTabellaParam, iReportParam);
            else
                return _rdBaseIntf_pg.dbCaricaDatidaOrdini(dateParam, iNumCassaParam, bSilentParam, sNomeTabellaParam, iReportParam);
        }

        /// <summary>
        /// la CASSA_PRINCIPALE può caricare la disponibilità della sessione precedente, <br/>
        /// la CASSA_SECONDARIA carica i Dati di Disponibilità in DB_Data, <br/>
        /// ritorna true se ha successo <br/>
        /// </summary>
        public bool dbCaricaDisponibilità(DateTime dateParam)
        {
#if STANDFACILE
            if (_iNDbMode == (int)DB_MODE.SQLITE)
                return _rdBaseIntf_ql.dbCaricaDisponibilità(dateParam);
            else
#endif
            if (_iNDbMode == (int)DB_MODE.MYSQL)
                return _rdBaseIntf_my.dbCaricaDisponibilità(dateParam);
            else
                return _rdBaseIntf_pg.dbCaricaDisponibilità(dateParam);
        }

        /// <summary>
        /// Funzione di lettura degli Ordini emessi dalle casse secondarie, <br/>
        /// ritorna 0 se non ce ne sono, negativo per gli ordini annullati<br/><br/>
        /// 
        /// chiamata da  AggiornaDisponibilità per aggiornare la disponibilità della CASSA_PRINCIPALE<br/>
        /// bClearOrdiniParam = true consente di eliminare un ordine dalla tabella ma solo se è CASSA_PRINCIPALE
        /// </summary>
        public int dbClearOrdiniCSec(bool bClearOrdiniParam)
        {
            if (_iNDbMode == (int)DB_MODE.MYSQL)
                return _rdBaseIntf_my.dbClearOrdiniCSec(bClearOrdiniParam);
            else
            if (_iNDbMode == (int)DB_MODE.POSTGRES)
                return _rdBaseIntf_pg.dbClearOrdiniCSec(bClearOrdiniParam);
            else
                return 0;
        }

        /// <summary>
        /// funziona che ottiene gli ordini web serviti
        /// ritorna il primo ordine della lista se ha successo <br/>
        /// 0 altrimenti
        /// </summary>
        public int dbGetOrdiniWebServiti()
        {
#if STANDFACILE
            if (_iNDbMode == (int)DB_MODE.SQLITE)
                return _rdBaseIntf_ql.dbGetOrdiniWebServiti();
            else
#endif
            if (_iNDbMode == (int)DB_MODE.MYSQL)
                return _rdBaseIntf_my.dbGetOrdiniWebServiti();
            else
                return _rdBaseIntf_pg.dbGetOrdiniWebServiti();
        }

        /// <summary>
        /// funziona che contrassegna gli ordini web serviti
        /// ritorna true se ha successo <br/>
        /// </summary>
        public bool dbClearOrdineWebServito(int iOrderParam)
        {
#if STANDFACILE
            if (_iNDbMode == (int)DB_MODE.SQLITE)
                return _rdBaseIntf_ql.dbClearOrdineWebServito(iOrderParam);
            else
#endif
            if (_iNDbMode == (int)DB_MODE.MYSQL)
                return _rdBaseIntf_my.dbClearOrdineWebServito(iOrderParam);
            else
                return _rdBaseIntf_pg.dbClearOrdineWebServito(iOrderParam);
        }

        /// <summary>
        /// Funzione di salvataggio nel database dei dati di riepilogo <br/>
        /// giornaliero, non deve fare conti !!!!!!!! <br/>
        /// ma solamente salvare SF_Data... <br/> <br/>
        /// 
        /// la Disponibilità è aggiornata solo nella CASSA_PRINCIPALE
        /// </summary>
        public void dbSalvaDati()
        {
#if STANDFACILE
            if (_iNDbMode == (int)DB_MODE.SQLITE)
                _rdBaseIntf_ql.dbSalvaDati();
            else
#endif
            if (_iNDbMode == (int)DB_MODE.MYSQL)
                _rdBaseIntf_my.dbSalvaDati();
            else
                _rdBaseIntf_pg.dbSalvaDati();
        }

        /// <summary>
        /// carica l'ordine iParam nella variabile DB_Articolo[], <br/>
        /// se però iParam == 0 carica _Versione, _Header, _HeaderText <br/> <br/>
        /// 
        /// ritorna true se ha successo, <br/>
        /// utilizzata da FrmVisOrdiniDlg, DataManager.AggiornaDisponibilità
        /// </summary>
        public bool dbCaricaOrdine(DateTime dateParam, int iParam, bool bFiltraPerCassa, String sNomeTabellaParam = "")
        {
#if STANDFACILE
            if (_iNDbMode == (int)DB_MODE.SQLITE)
                return _rdBaseIntf_ql.dbCaricaOrdine(dateParam, iParam, bFiltraPerCassa, sNomeTabellaParam);
            else
#endif
            if (_iNDbMode == (int)DB_MODE.MYSQL)
                return _rdBaseIntf_my.dbCaricaOrdine(dateParam, iParam, bFiltraPerCassa, sNomeTabellaParam);
            else
                return _rdBaseIntf_pg.dbCaricaOrdine(dateParam, iParam, bFiltraPerCassa, sNomeTabellaParam);
        }

        /// <summary>
        /// Aggiorna le intestazioni nel database ordini<br/>
        /// chiamata da SalvaListino()
        /// </summary>
        public void dbUpdateHeadOrdine()
        {
#if STANDFACILE
            if (_iNDbMode == (int)DB_MODE.SQLITE)
                _rdBaseIntf_ql.dbUpdateHeadOrdine();
            else
#endif
            if (_iNDbMode == (int)DB_MODE.MYSQL)
                _rdBaseIntf_my.dbUpdateHeadOrdine();
            else
                _rdBaseIntf_pg.dbUpdateHeadOrdine();
        }

        /// <summary>
        /// Salva l'ordine corrente nel database<br/>
        /// se bCreateHead = true salva solo l'intestazione
        /// </summary>
        public void dbSalvaOrdine(bool bCreateHead = false)
        {
#if STANDFACILE
            if (_iNDbMode == (int)DB_MODE.SQLITE)
                _rdBaseIntf_ql.dbSalvaOrdine(bCreateHead);
            else
#endif
            if (_iNDbMode == (int)DB_MODE.MYSQL)
                _rdBaseIntf_my.dbSalvaOrdine(bCreateHead);
            else
                _rdBaseIntf_pg.dbSalvaOrdine(bCreateHead);
        }

        /// <summary>
        /// annulla uno scontrino contrassegnando cancellation, cancellationTime e poi carica i dati, <br/>
        /// iNumAnnulloParam è il numero dell'ordine,  <br/>
        /// </summary>
        public bool dbAnnulloOrdine(DateTime dateParam, int iNumAnnulloParam, String sNomeTabellaParam = "")
        {
#if STANDFACILE
            if (_iNDbMode == (int)DB_MODE.SQLITE)
                return _rdBaseIntf_ql.dbAnnulloOrdine(dateParam, iNumAnnulloParam, sNomeTabellaParam);
            else
#endif
            if (_iNDbMode == (int)DB_MODE.MYSQL)
                return _rdBaseIntf_my.dbAnnulloOrdine(dateParam, iNumAnnulloParam, sNomeTabellaParam);
            else
                return _rdBaseIntf_pg.dbAnnulloOrdine(dateParam, iNumAnnulloParam, sNomeTabellaParam);
        }

        /// <summary>
        /// restituisce il messaggio nella variabile DB_Data.sMessaggio, <br/>
        /// ritorna true se ha successo, utilizzata da VisMessaggi
        /// </summary>
        public bool dbCaricaMessaggio(int iParam, bool bTutteCassaParam)
        {
#if STANDFACILE
            if (_iNDbMode == (int)DB_MODE.SQLITE)
                return _rdBaseIntf_ql.dbCaricaMessaggio(iParam, bTutteCassaParam);
            else
#endif
            if (_iNDbMode == (int)DB_MODE.MYSQL)
                return _rdBaseIntf_my.dbCaricaMessaggio(iParam, bTutteCassaParam);
            else
                return _rdBaseIntf_pg.dbCaricaMessaggio(iParam, bTutteCassaParam);
        }

        /// <summary>Salva il messaggio corrente nel database</summary>
        public void dbSalvaMessaggio(String[] rVisMessaggiLines, String sNomeFileMsg)
        {
#if STANDFACILE
            if (_iNDbMode == (int)DB_MODE.SQLITE)
                _rdBaseIntf_ql.dbSalvaMessaggio(rVisMessaggiLines, sNomeFileMsg);
            else
#endif
            if (_iNDbMode == (int)DB_MODE.MYSQL)
                _rdBaseIntf_my.dbSalvaMessaggio(rVisMessaggiLines, sNomeFileMsg);
            else
                _rdBaseIntf_pg.dbSalvaMessaggio(rVisMessaggiLines, sNomeFileMsg);
        }

        /// <summary>
        /// cerca il checksum nella tabella "Listino" ne ritorna la stringa se ha successo,  <br/>
        /// altrimenti ritorna "", solo con NDB <br/>
        /// </summary>
        public String dbCheckListino()
        {
            if (_iNDbMode == (int)DB_MODE.MYSQL)
                return _rdBaseIntf_my.dbCheckListino();
            else
            if (_iNDbMode == (int)DB_MODE.POSTGRES)
                return _rdBaseIntf_pg.dbCheckListino();
            else
                return "";
        }

        /// <summary>
        /// Funzione di salvataggio nel database MySQL del listino <br/>
        /// da parte della cassa primaria, a disposizione poi della cassa secondaria<br/>
        /// chiamata dal DataManager in CaricaListino, SalvaListino
        /// </summary>
        public void dbSalvaListino()
        {
            if (_iNDbMode == (int)DB_MODE.MYSQL)
                _rdBaseIntf_my.dbSalvaListino();
            else
            if (_iNDbMode == (int)DB_MODE.POSTGRES)
                _rdBaseIntf_pg.dbSalvaListino();
            else
                return;
        }

        /// <summary>
        /// Funzione di salvataggio nel database MySQL del Test <br/>
        /// da parte della cassa primaria, a disposizione poi <br/>
        /// della cassa secondaria, solo con MySQL
        /// </summary>
        public void dbSalvaTest()
        {
            if (_iNDbMode == (int)DB_MODE.MYSQL)
                _rdBaseIntf_my.dbSalvaTest();
            else
            if (_iNDbMode == (int)DB_MODE.POSTGRES)
                _rdBaseIntf_pg.dbSalvaTest();
            else
                return;
        }

        /// <summary>
        /// aggiunge il suffisso alle tabelle
        /// usato da chiudiIncasso()
        /// </summary>
        public bool dbRenameTables(String sNewPostFix)
        {
#if STANDFACILE
            if (_iNDbMode == (int)DB_MODE.SQLITE)
                return _rdBaseIntf_ql.dbRenameTables(sNewPostFix);
            else
#endif
            if (_iNDbMode == (int)DB_MODE.MYSQL)
                return _rdBaseIntf_my.dbRenameTables(sNewPostFix);
            else
                return _rdBaseIntf_pg.dbRenameTables(sNewPostFix);
        }

        /// <summary>
        /// aggiunge il suffisso alla tabella selezionata <br/>
        /// usato da EsploraDB_Dlg()
        /// </summary>
        /// <returns></returns>
        public bool dbRenameTable(String sOldTabellaParam, String sNewTabellaParam)
        {
#if STANDFACILE
            if (_iNDbMode == (int)DB_MODE.SQLITE)
                return _rdBaseIntf_ql.dbRenameTable(sOldTabellaParam, sNewTabellaParam);
            else
#endif
            if (_iNDbMode == (int)DB_MODE.MYSQL)
                return _rdBaseIntf_my.dbRenameTable(sOldTabellaParam, sNewTabellaParam);
            else
                return _rdBaseIntf_pg.dbRenameTable(sOldTabellaParam, sNewTabellaParam);
        }

        /// <summary>
        /// usato da EraseAllaData()
        /// </summary>
        /// <returns></returns>
        public bool dbDropTables()
        {
#if STANDFACILE
            if (_iNDbMode == (int)DB_MODE.SQLITE)
                return _rdBaseIntf_ql.dbDropTables();
            else
#endif
            if (_iNDbMode == (int)DB_MODE.MYSQL)
                return _rdBaseIntf_my.dbDropTables();
            else
                return _rdBaseIntf_pg.dbDropTables();
        }

        /// <summary>
        /// usato da EsploraDB_Dlg
        /// </summary>
        public bool dbDropTable(String sNomeTabellaParam)
        {
#if STANDFACILE
            if (_iNDbMode == (int)DB_MODE.SQLITE)
                return _rdBaseIntf_ql.dbDropTable(sNomeTabellaParam);
            else
#endif
            if (_iNDbMode == (int)DB_MODE.MYSQL)
                return _rdBaseIntf_my.dbDropTable(sNomeTabellaParam);
            else
                return _rdBaseIntf_pg.dbDropTable(sNomeTabellaParam);
        }

        /// <summary>
        /// Ottiene elenco delle tabelle del Database,<br/><br/>
        /// ritorna il numero di tabelle (ed elenco delle tabelle) se ha successo,<br/>
        /// 0 altrimenti, usato da EsploraDB_Dlg <br/>
        /// </summary>
        public int dbElencoTabelle(List<String> sStringsParam)
        {
#if STANDFACILE
            if (_iNDbMode == (int)DB_MODE.SQLITE)
                return _rdBaseIntf_ql.dbElencoTabelle(sStringsParam);
            else
#endif
            if (_iNDbMode == (int)DB_MODE.MYSQL)
                return _rdBaseIntf_my.dbElencoTabelle(sStringsParam);
            else
                return _rdBaseIntf_pg.dbElencoTabelle(sStringsParam);
        }

        /// <summary>
        /// Funzione di richiesta nuovo numero di scontrino <br/>
        /// solo con NDB, chiamata da BtnScontrinoClick
        /// </summary>
        public int dbNewOrdineNumRequest()
        {
            if (_iNDbMode == (int)DB_MODE.MYSQL)
                return _rdBaseIntf_my.dbNewOrdineNumRequest();
            else
            if (_iNDbMode == (int)DB_MODE.POSTGRES)
                return _rdBaseIntf_pg.dbNewOrdineNumRequest();
            else
                return 0;
        }

        /// <summary>
        /// Funzione di richiesta nuovo numero di messaggio <br/>
        /// solo con NDB, chiamata da VisMessaggiDlg
        /// </summary>
        public int dbNewMessageNumRequest()
        {
            if (_iNDbMode == (int)DB_MODE.MYSQL)
                return _rdBaseIntf_my.dbNewMessageNumRequest();
            else
            if (_iNDbMode == (int)DB_MODE.POSTGRES)
                return _rdBaseIntf_pg.dbNewMessageNumRequest();
            else
                return 0;
        }

        /// <summary>
        /// Funzione di inserimento record con numero Ordine dalla cassa Sec <br/>
        /// per successivo scarico della Disponibilità di Magazzino <br/>
        /// solo con MySQL, usata da Receipt() ed AnnulloOrdine()<br/>
        /// </summary>
        public void dbCSecOrderEnqueue(int iEnqueueParam)
        {
            if (_iNDbMode == (int)DB_MODE.MYSQL)
                _rdBaseIntf_my.dbCSecOrderEnqueue(iEnqueueParam);
            else
            if (_iNDbMode == (int)DB_MODE.POSTGRES)
                _rdBaseIntf_pg.dbCSecOrderEnqueue(iEnqueueParam);
            else
                return;
        }

#if STANDFACILE
        /// <summary>
        /// Funzione di inserimento record del numero Ordine web<br/>
        /// usata da MainForm nel caso fallisca il contrassegno diretto
        /// </summary>
        public void dbWebOrderEnqueue(int iEnqueueParam)
        {
            if (_iNDbMode == (int)DB_MODE.SQLITE)
                _rdBaseIntf_ql.dbWebOrderEnqueue(iEnqueueParam);
            else
            if (_iNDbMode == (int)DB_MODE.MYSQL)
                _rdBaseIntf_my.dbWebOrderEnqueue(iEnqueueParam);
            else
                _rdBaseIntf_pg.dbWebOrderEnqueue(iEnqueueParam);
            return;
        }

        /// <summary>contrassegna come scaricato uno scontrino dalla tabella delle Prevendite/// </summary>
        public bool dbScaricaOrdinePrevendita(int iOrderID, String sPrevTableParam)
        {
            if (_iNDbMode == (int)DB_MODE.SQLITE)
                return _rdBaseIntf_ql.dbScaricaOrdinePrevendita(iOrderID, sPrevTableParam);
            else
            if (_iNDbMode == (int)DB_MODE.MYSQL)
                return _rdBaseIntf_my.dbScaricaOrdinePrevendita(iOrderID, sPrevTableParam);
            else
                return _rdBaseIntf_pg.dbScaricaOrdinePrevendita(iOrderID, sPrevTableParam);
        }
#endif

        /// <summary>funzione di modifica dello stato con i vari flag, iOrderIDParam >= 0 per i Tickets, &lt; 0 per i messaggi</summary>
        public bool dbEditStatus(int iOrderID, int iStatus)
        {
#if STANDFACILE
            if (_iNDbMode == (int)DB_MODE.SQLITE)
                return _rdBaseIntf_ql.dbEditStatus(iOrderID, iStatus);
            else
#endif
            if (_iNDbMode == (int)DB_MODE.MYSQL)
                return _rdBaseIntf_my.dbEditStatus(iOrderID, iStatus);
            else
                return _rdBaseIntf_pg.dbEditStatus(iOrderID, iStatus);
        }


#if STAND_MONITOR
        /// <summary> costruisce la tabella del venduto </summary>
        public void dbBuildMonitorTable()
        {
            if (_iNDbMode == (int)DB_MODE.MYSQL)
                _rdBaseIntf_my.dbBuildMonitorTable();
            else
                _rdBaseIntf_pg.dbBuildMonitorTable();
        }

        /// <summary> 
        /// costruisce la tabella ordini evasi: <br/>
        /// se bFormIsVisibleParam == true ha senso aggiornare il form,<br/> 
        /// altrimenti si limita a ricavare gli ultimi ordini evasi
        /// </summary>
        public System.Data.DataSet dbOrdiniMonitorList(bool checkBoxParam, bool bFormIsVisibleParam)
        {
            if (_iNDbMode == (int)DB_MODE.MYSQL)
                return _rdBaseIntf_my.dbOrdiniMonitorList(checkBoxParam, bFormIsVisibleParam);
            else
                return _rdBaseIntf_pg.dbOrdiniMonitorList(checkBoxParam, bFormIsVisibleParam);
        }
#endif

#if STAND_ORDINI
        /// <summary>
        /// scarica uno scontrino dal database,<br/>
        /// se iGruppo è negativo allora si ignora il gruppo di stampa
        /// </summary>
        public bool dbScaricaOrdine(int iOrderID, int iGruppo)
        {
            if (_iNDbMode == (int)DB_MODE.MYSQL)
                return _rdBaseIntf_my.dbScaricaOrdine(iOrderID, iGruppo);
            else
                return _rdBaseIntf_pg.dbScaricaOrdine(iOrderID, iGruppo);
        }

        /// <summary> costruisce la tabella del venduto </summary>
        public bool dbCaricaUltimiOrdini()
        {
            if (_iNDbMode == (int)DB_MODE.MYSQL)
                return _rdBaseIntf_my.dbCaricaUltimiOrdini();
            else
                return _rdBaseIntf_pg.dbCaricaUltimiOrdini();
        }
#endif

        /// <summary>
        /// Test di connessione al db server MySQL
        /// se serve istanzia il gestore DB corretto
        /// </summary>
        public bool dbCheck(String sDB_ServerNamePrm, String sDB_pwdPrm, int iDbModePrm, bool bSilentParam = false)
        {
            if (iDbModePrm == (int)DB_MODE.MYSQL)
            {
                if (Program._rBdBaseIntf_my == null)
                    Program._rBdBaseIntf_my = new dBaseIntf_my();

                return _rdBaseIntf_my.dbCheck(sDB_ServerNamePrm, sDB_pwdPrm, bSilentParam);
            }
            else
            if (iDbModePrm == (int)DB_MODE.POSTGRES)
            {
                if (Program._rBdBaseIntf_pg == null)
                    Program._rBdBaseIntf_pg = new dBaseIntf_pg();

                return _rdBaseIntf_pg.dbCheck(sDB_ServerNamePrm, sDB_pwdPrm, bSilentParam);
            }
            else
                return false;
        }

        /// <summary>
        /// Returns a string with backslashes before characters that need to be quoted <br/>
        /// https://www.digitalcoding.com/Code-Snippets/C-Sharp/C-Code-Snippet-AddSlashes-StripSlashes-Escape-String.html
        /// </summary>
        /// <param name="InputTxt">Text string need to be escape with slashes</param>
        public static string AddSlashes(string InputTxt)
        {
            // List of characters handled:
            // \000 null
            // \010 backspace
            // \011 horizontal tab
            // \012 new line
            // \015 carriage return
            // \032 substitute
            // \042 double quote
            // \047 single quote
            // \134 backslash
            // \140 grave accent

            string Result = InputTxt;

            try
            {
                Result = System.Text.RegularExpressions.Regex.Replace(InputTxt, @"[\000\010\011\012\015\032\042\047\134\140]", "\\$0");
            }
            catch (Exception Ex)
            {
                // handle any exception here
                Console.WriteLine(Ex.Message);
            }

            return Result;
        }
        /// <summary>
        /// Un-quotes a quoted string<br/>
        /// https://www.digitalcoding.com/Code-Snippets/C-Sharp/C-Code-Snippet-AddSlashes-StripSlashes-Escape-String.html
        /// </summary>
        /// <param name="InputTxt">Text string need to be escape with slashes</param>
        public static string StripSlashes(string InputTxt)
        {
            // List of characters handled:
            // \000 null
            // \010 backspace
            // \011 horizontal tab
            // \012 new line
            // \015 carriage return
            // \032 substitute
            // \042 double quote
            // \047 single quote
            // \134 backslash
            // \140 grave accent

            string Result = InputTxt;

            try
            {
                Result = System.Text.RegularExpressions.Regex.Replace(InputTxt, @"(\\)([\000\010\011\012\015\032\042\047\134\140])", "$2");
            }
            catch (Exception Ex)
            {
                // handle any exception here
                Console.WriteLine(Ex.Message);
            }

            return Result;
        }

    }
}

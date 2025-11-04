/*****************************************************************************************
	 NomeFile : StandCommonSrc/dBaseIntf_my.cs
    Data	 : 06.12.2024
	 Autore   : Mauro Artuso

    nelle assegnazioni :
    DB_Data compare sempre a sx,
    SF_Data compare sempre a dx

    Backticks ``` are used in MySQL to select columns and tables from your MySQL source,
    single quotes are used for literals (stringhe).

    Attenzione : dbInit(dateParam) deve essere invocata all'inizio di ogni funzione
 *****************************************************************************************/

using System;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Data;

using Devart.Data.MySql;

using static StandCommonFiles.ComDef;
using static StandCommonFiles.CommonCl;
using static StandCommonFiles.LogServer;

using StandFacile;
using static StandFacile.glb;
using static StandFacile.dBaseIntf;

namespace StandFacile_DB
{
#pragma warning disable IDE0079
#pragma warning disable IDE0044
#pragma warning disable IDE1006

    /// <summary>classe per la gestione di MySQL</summary>
    public partial class dBaseIntf_my
    {
        /// <summary>riferimento a dBaseIntf</summary>
        public static dBaseIntf_my _rdBaseIntf_my;

        /// <summary>connessione per dBaseIntf</summary>
        static MySqlConnection _Connection;

        MySqlConnectionStringBuilder _dbCSB = new MySqlConnectionStringBuilder();

        MySqlConnection _ConnectionWeb;
        MySqlConnectionStringBuilder _dbCSB_Web = new MySqlConnectionStringBuilder();

        /// <summary>ottiene la connessione MySql</summary>
        public static MySqlConnection GetDB_Connection() { return _Connection; }

        /// <summary>controllo stato della connessione</summary>
        public bool GetDB_Connected()
        {
            if (_Connection != null)
                return (_Connection.State == ConnectionState.Open);
            else
                return false;
        }

        struct TReceiptListItem
        {
#pragma warning disable CS0649
            public string sArticoloRiga;
            public int iReceiptNum;
        }

        /// <summary>costruttore</summary>
        public dBaseIntf_my()
        {
            String sDir;

            _rdBaseIntf_my = this;

#if STANDFACILE
            sDir = DataManager.GetExeDir() + "\\";
#else
            sDir = sRootDir + "\\";
#endif

            // deve essere presente anche se non si usa il db
            if (!(File.Exists(sDir + DB_CONNECTOR_DLL_DEV) && File.Exists(sDir + DB_CONNECTOR_DLL_MY)))
                ErrorManager(ERR_DLL);
        }

        /// <summary>distruttore DB, libera la connessione</summary>
        ~dBaseIntf_my()
        {
            _Connection?.Close();
        }

        /// <summary>
        /// se sNomeTabellaParam è vuota imposta _sDBTNameDati, _sDBTNameOrdini <br/>
        /// in base a dateParam, iNumCassaParam <br/>
        /// altrimenti in base a sNomeTabellaParam <br/> <br/>
        /// 
        /// ritorna false = errore, true = connessione effettuata <br/>
        /// </summary>
        public bool dbInit(DateTime dateParam, int iNumCassaParam, bool bSilentParam = false, String sNomeTabellaParam = "")
        {
            String sTmp, sData, sPostFix, sDebugDati, sDebugOrdini;

            _Connection?.Close();

            // prepara connessione al DB
            if (bUSA_NDB())
            {
                try
                {
                    _sDBTNameDati = GetNomeDatiDBTable(iNumCassaParam, dateParam);
                    _sDBTNameOrdini = GetNomeOrdiniDBTable(dateParam);

                    // serve per visualizzazione ordini
                    _iDBTNameOrdiniLength = _sDBTNameOrdini.Length;

                    sDebugDati = _sDBTNameDati;
                    sDebugOrdini = _sDBTNameOrdini;

                    if (!String.IsNullOrEmpty(sNomeTabellaParam))
                    {
                        // gestione estensioni da chiusura cassa ricavate dalla tabella dati e/o ordini in prevendita
                        // prefisso
                        if (sNomeTabellaParam.Contains(_dbPreDataTablePrefix))
                        {
                            sData = sNomeTabellaParam.Remove(0, _dbPreDataTablePrefix.Length + 3); // OK
                            sTmp = sData.Insert(0, _dbPreOrdersTablePrefix);
                            _sDBTNameOrdini = sTmp;
                        }
                        else if (sNomeTabellaParam.Contains(_dbPreOrdersTablePrefix))
                        {
                            sData = sNomeTabellaParam.Remove(0, _dbPreOrdersTablePrefix.Length); // OK
                            sTmp = sData.Insert(0, String.Format("_c{0}", iNumCassaParam));
                            sTmp = sTmp.Insert(0, _dbPreDataTablePrefix);
                            _sDBTNameDati = sTmp;
                        }
                        // gestione estensioni da chiusura cassa ricavate dalla tabella dati e/o ordini
                        else if ((sNomeTabellaParam.Length >= (_sDBTNameOrdini.Length)) && sNomeTabellaParam.Contains(_dbOrdersTablePrefix))
                        {
                            if (sNomeTabellaParam.Length > (_sDBTNameOrdini.Length))
                            {
                                // postfisso OK
                                sTmp = sNomeTabellaParam.Remove(0, _dbOrdersTablePrefix.Length);
                                sData = sTmp.Substring(0, 7);
                                sTmp = sData.Insert(0, String.Format("_c{0}", iNumCassaParam));
                                sPostFix = sNomeTabellaParam.Substring(_sDBTNameOrdini.Length); // postfisso
                                _sDBTNameDati = _dbDataTablePrefix + sTmp + sPostFix;
                            }
                            else
                            {
                                // da EsploraDB_Dlg tabelle in altra data OK
                                sData = sNomeTabellaParam.Remove(0, _dbOrdersTablePrefix.Length);
                                sTmp = sData.Insert(0, String.Format("_c{0}", iNumCassaParam));
                                _sDBTNameDati = _dbDataTablePrefix + sTmp;
                            }
                        }
                        else if ((sNomeTabellaParam.Length >= (_sDBTNameDati.Length)) && sNomeTabellaParam.Contains(_dbDataTablePrefix))
                        {
                            if (sNomeTabellaParam.Length > (_sDBTNameDati.Length))
                            {
                                // postfisso OK
                                sTmp = sNomeTabellaParam.Remove(0, _dbDataTablePrefix.Length + 3);
                                sData = sTmp.Substring(0, 7);
                                sPostFix = sNomeTabellaParam.Substring(_sDBTNameDati.Length); // postfisso
                                _sDBTNameOrdini = _dbOrdersTablePrefix + sData + sPostFix;
                            }
                            else
                            {
                                // da EsploraDB_Dlg tabelle in altra data OK
                                sData = sNomeTabellaParam.Remove(0, _dbDataTablePrefix.Length + 3);
                                _sDBTNameOrdini = _dbOrdersTablePrefix + sData;
                            }
                        }
                    }

                    sDebugDati = _sDBTNameDati;
                    sDebugOrdini = _sDBTNameOrdini;

                    if (sNomeTabellaParam.Contains(_dbOrdersTablePrefix) || sNomeTabellaParam.Contains(_dbPreOrdersTablePrefix))
                        _sDBTNameOrdini = sNomeTabellaParam;

                    if (sNomeTabellaParam.Contains(_dbDataTablePrefix) || sNomeTabellaParam.Contains(_dbPreDataTablePrefix))
                        _sDBTNameDati = sNomeTabellaParam;

                    _dbCSB.Host = _sDB_ServerName;
                    _dbCSB.Database = _database;
                    _dbCSB.UserId = _uid;
                    _dbCSB.Password = _sDB_Password;
                    _dbCSB.FoundRows = true;
                    _dbCSB.Pooling = false;
                    _dbCSB.Unicode = true;
                    _dbCSB.ConnectionTimeout = TIMEOUT_DB_OPEN;

                    _Connection = new MySqlConnection(_dbCSB.ConnectionString);
                    _Connection.Open();

                    return true;
                }

                catch (Exception)
                {
#if STAND_CUCINA
                    String[] sOrdiniQueueObj = new String[2];

                    sOrdiniQueueObj[0] = Define.UPDATE_DB_LABEL_EVENT;
                    sOrdiniQueueObj[1] = "No connessione al DB !";
                    FrmMain.QueueUpdate(sOrdiniQueueObj);
#endif

                    // connessione non possibile al database
                    _WrnMsg.iErrID = WRN_DBE;
                    _WrnMsg.sMsg = "dbInit MySQL";

                    if (!bSilentParam) // per non accumulare troppe indicazioni
                        WarningManager(_WrnMsg);

                    LogToFile("dbInit : Exception MySQL");
                }
            }

            return false; // non dovrebbe mai passare di qui !
        } // end dbInit

        /// <summary>
        /// seconda dbInit: instanzia ulteriore connessione per il processo dBaseTunnel<br/>
        /// ritorna false = errore, true = connessione effettuata <br/>
        /// </summary>
        public bool dbInitWeb()
        {
            _ConnectionWeb?.Close();

            // prepara connessione al DB
            if (bUSA_NDB())
            {
                try
                {
                    _dbCSB_Web.Host = _sDB_ServerName;
                    _dbCSB_Web.Database = _database;
                    _dbCSB_Web.UserId = _uid;
                    _dbCSB_Web.Password = _sDB_Password;
                    _dbCSB_Web.Pooling = false;
                    _dbCSB_Web.Unicode = true;
                    _dbCSB_Web.ConnectionTimeout = TIMEOUT_DB_OPEN;

                    _ConnectionWeb = new MySqlConnection(_dbCSB_Web.ConnectionString);
                    _ConnectionWeb.Open();

                    return true;
                }

                catch (Exception)
                {
                    // connessione non possibile al database
                    _WrnMsg.iErrID = WRN_DBE;
                    _WrnMsg.sMsg = "dbInit MySql Web";

                    WarningManager(_WrnMsg);

                    LogToFile("dbInit : Exception MySql Web");
                }
            }

            return false; // non dovrebbe mai passare di qui !
        } // end dbInit

        /// <summary>
        /// funzione di controllo e reset del record di stato contenente la data <br/>
        /// (solo da CASSA_PRINCIPALE e con bResetParam == true) <br/> <br/>
        /// 
        /// ritorna true se la verifica/creazione della data è corretta, false altrimenti <br/>
        /// utilizzata da : dbNewOrdineNumRequest, dbNewMessageNumRequest, DataManager.Init <br/>
        /// </summary>
        public bool dbCheckStatus(bool bResetParam = false)
        {
            bool bDBConnection_Ok;
            int iGiorno, iMese, iAnno;
            String sQueryTxt, sTmp, sActualDateStr;
            String sReadVersion = RELEASE_SW;
            DateTime statusDate = GetActualDate();

            MySqlCommand cmd = new MySqlCommand();
            MySqlDataReader readerStato = null;
            TWebServerParams sWebServerParams = dbGetWebServerParams();

            bDBConnection_Ok = dbInit(GetActualDate(), SF_Data.iNumCassa, true);

            // sicurezza : si prosegue solo se c'è la connessione al DB
            if (!bDBConnection_Ok)
                return false;

            try
            {
                cmd.Connection = _Connection;
                cmd.CommandText = "SELECT * FROM " + NOME_STATO_DBTBL + " WHERE (key_ID = 'Data')";

                readerStato = cmd.ExecuteReader();

                if (readerStato != null)
                {
                    readerStato.Read();

                    iAnno = readerStato.GetInt32("iYear");
                    iMese = readerStato.GetInt32("iMonth");
                    iGiorno = readerStato.GetInt32("iDay");

#if STAND_CUCINA || STAND_ORDINI || STAND_MONITOR
                    // impostazione importante 
                    SetActualDate(new DateTime(iAnno, iMese, iGiorno));
#endif

                    DateTime tmpDate = new DateTime(iAnno, iMese, iGiorno);
                    statusDate = tmpDate;

                    _sDateFromDB = statusDate.ToString("dd/MM/yy");

#if STAND_CUCINA
                    if (_sPrevStatus_Date != _sDateFromDB)
                    {
                        //dbAzzeraDatiGen();

                        _bStatusDate_IsChanged = true;
                        _sPrevStatus_Date = _sDateFromDB;
                    }
#endif

                    LogToFile("dbCheckStatus : stato letto");

                    readerStato.Close();
                }
                else
                    LogToFile("dbCheckStatus : stato non letto");

                if ((SF_Data.iNumCassa == CASSA_PRINCIPALE) && bResetParam)
                {
                    // lettura Versione
                    cmd.CommandText = "SELECT * FROM " + NOME_STATO_DBTBL + " WHERE (key_ID = 'Versione')";
                    readerStato = cmd.ExecuteReader();

                    if (readerStato != null)
                    {
                        readerStato.Read();

                        sReadVersion = readerStato.GetString("sText");

                        readerStato.Close();
                        LogToFile("dbCheckStatus : versione letta");
                    }
                    else
                        LogToFile("dbCheckStatus : versione non letta");
                }
            }

            catch (Exception)
            {
                LogToFile("dbCheckStatus : stato dbException");
                _sDateFromDB = "Stato non presente!";
            }

            sActualDateStr = GetActualDate().ToString("dd/MM/yy");

            // *** confronto data ***
            if (_sDateFromDB != sActualDateStr)
            {

                // sicurezza : si prosegue solo se è CASSA_PRINCIPALE
                if ((SF_Data.iNumCassa == CASSA_PRINCIPALE) && bResetParam)
                {
#if STANDFACILE
                    // valore non plausibile se la chiave non esiste
                    int iKeyGood = ReadRegistry(DISP_DLG_MNG_KEY, -1);

                    bool bDispDlgShow = IsBitSet(ReadRegistry(DISP_DLG_MNG_KEY, SetBit(0, BIT_SHOW_DISP_DLG)), BIT_SHOW_DISP_DLG);
                    bool bPrevDispLoad = IsBitSet(ReadRegistry(DISP_DLG_MNG_KEY, SetBit(0, BIT_PREV_DISP_LOAD)), BIT_PREV_DISP_LOAD);

                    // la prima esecuzione non apre il dialogo
                    if (iKeyGood == -1)
                        WriteRegistry(DISP_DLG_MNG_KEY, 0);
                    else if (bDispDlgShow)
                    {
                        StartDispDlg rChooseDispDlg = new StartDispDlg(GetActualDate(), statusDate);
                    }
                    else if (bPrevDispLoad)
                    {
                        InitialDispDlg rInitDispDlg = new InitialDispDlg(statusDate, false);
                    }
#endif

                    try
                    {
                        // stato
                        sQueryTxt = "DROP TABLE IF EXISTS " + NOME_STATO_DBTBL + ";";

                        cmd.Connection = _Connection;
                        cmd.CommandText = sQueryTxt;
                        var qResult = cmd.ExecuteScalar();

                        LogToFile("dbCheckStatus : prima CREATE TABLE stato");

                        sQueryTxt = String.Format(@"CREATE TABLE IF NOT EXISTS {0} (key_ID VARCHAR(50) NOT NULL, sText VARCHAR(50),
                                iYear INT UNSIGNED NOT NULL, iMonth INT UNSIGNED NOT NULL, iDay INT UNSIGNED NOT NULL, PRIMARY KEY(key_ID));",
                            NOME_STATO_DBTBL);

                        cmd.CommandText = sQueryTxt;
                        qResult = cmd.ExecuteScalar();

                        sTmp = GetActualDate().ToString("yyyy");
                        iAnno = Convert.ToInt32(sTmp);

                        sTmp = GetActualDate().ToString("MM");
                        iMese = Convert.ToInt32(sTmp);

                        sTmp = GetActualDate().ToString("dd");
                        iGiorno = Convert.ToInt32(sTmp);

                        sQueryTxt = String.Format("INSERT INTO {0} (key_ID, sText, iYear, iMonth, iDay) VALUES (\'{1}\', \'{2}\', {3}, {4}, {5});",
                            NOME_STATO_DBTBL, "Data", "-", iAnno, iMese, iGiorno);
                        cmd.CommandText = sQueryTxt;
                        qResult = cmd.ExecuteScalar();

                        sQueryTxt = String.Format("INSERT INTO {0} (key_ID, sText, iYear, iMonth, iDay) VALUES (\'{1}\', \'{2}\', {3}, {4}, {5});",
                            NOME_STATO_DBTBL, "Versione", RELEASE_SW, 0, 0, 0);
                        cmd.CommandText = sQueryTxt;
                        qResult = cmd.ExecuteScalar();

                        sQueryTxt = String.Format("INSERT INTO {0} (key_ID, sText, iYear, iMonth, iDay) VALUES (\'{1}\', \'{2}\', {3}, {4}, {5});",
                            NOME_STATO_DBTBL, WEB_SERVER_NAME_KEY, sWebServerParams.sWebTablePrefix, 0, 0, 0);
                        cmd.CommandText = sQueryTxt;
                        qResult = cmd.ExecuteScalar();

                        sQueryTxt = String.Format("INSERT INTO {0} (key_ID, sText, iYear, iMonth, iDay) VALUES (\'{1}\', \'{2}\', {3}, {4}, {5});",
                            NOME_STATO_DBTBL, WEB_DBASE_NAME_KEY, sWebServerParams.sWeb_DBase, 0, 0, 0);
                        cmd.CommandText = sQueryTxt;
                        qResult = cmd.ExecuteScalar();

                        sQueryTxt = String.Format("INSERT INTO {0} (key_ID, sText, iYear, iMonth, iDay) VALUES (\'{1}\', \'{2}\', {3}, {4}, {5});",
                            NOME_STATO_DBTBL, WEB_DBASE_PWD_KEY, sWebServerParams.sWebEncryptedPwd, 0, 0, 0);
                        cmd.CommandText = sQueryTxt;
                        qResult = cmd.ExecuteScalar();

                        LogToFile("dbCheckStatus : dopo CREATE TABLE stato");

                        // ordini_csec
                        sQueryTxt = String.Format("DROP TABLE IF EXISTS {0};", NOME_DISP_DBTBL);
                        cmd.CommandText = sQueryTxt;
                        qResult = cmd.ExecuteScalar();

                        sQueryTxt = String.Format(@"CREATE TABLE IF NOT EXISTS {0} (iOrdine_ID INT NOT NULL, iNumCassa INT UNSIGNED NOT NULL, 
                                            sDataOra VARCHAR(50));", NOME_DISP_DBTBL);
                        cmd.CommandText = sQueryTxt;
                        qResult = cmd.ExecuteScalar();

                        LogToFile("dbCheckStatus : dopo CREATE TABLE ordini_csec");

                        // ci pensa il processo rdb_aggiornaOrdiniWebServiti() a svuotare la tabella ordini_web
                        //sQueryTxt = String.Format("DROP TABLE IF EXISTS {0};", NOME_WEBORD_DBTBL);
                        //cmd.CommandText = sQueryTxt;
                        //qResult = cmd.ExecuteScalar();

                        sQueryTxt = String.Format(@"CREATE TABLE IF NOT EXISTS {0} (iOrdine_ID INT NOT NULL, iNumCassa INT UNSIGNED NOT NULL, 
                                            sDataOra VARCHAR(50));", NOME_WEBORD_DBTBL);
                        cmd.CommandText = sQueryTxt;
                        qResult = cmd.ExecuteScalar();

                        LogToFile("dbCheckStatus : dopo CREATE TABLE ordini_web");
                    }

                    catch (Exception)
                    {
                        _WrnMsg.iErrID = WRN_DBE;
                        _WrnMsg.sMsg = String.Format("dbCheckStatus : creazione tabella {0}", bUSA_NDB());
                        WarningManager(_WrnMsg);
                        LogToFile("dbCheckStatus : dbException creazione tabelle");

                        return false;
                    }
                }
                else if ((SF_Data.iNumCassa != CASSA_PRINCIPALE) && bResetParam)
                {
                    _WrnMsg.iErrID = WRN_DNA;
                    _WrnMsg.sMsg = String.Format("- PC Locale :  {0}\n\n- Database : {1}\n", sActualDateStr, _sDateFromDB);
                    WarningManager(_WrnMsg);

                    LogToFile("dbCheckStatus : CASSA_SECONDARIA cambio data");
                    return false;
                }
                else
                    return false;
            }
            else if ((SF_Data.iNumCassa == CASSA_PRINCIPALE) && (sReadVersion != RELEASE_SW) && bResetParam)
            {
                // CASSA_PRINCIPALE, versioni differenti, stessa data

                // aggiorna versione
                sReadVersion = "";
                cmd.CommandText = "UPDATE " + NOME_STATO_DBTBL + " SET sText = '" + RELEASE_SW + "' WHERE (key_ID = 'Versione')";
                readerStato = cmd.ExecuteReader();
            }

            readerStato?.Close();

            cmd.Dispose();
            return true; // data DB corretta
        }

        /// <summary>
        ///   Utilizzata per resettare la tabella num_ordini con il numero <br/>
        ///   progressivo degli ordini, utilizza AUTO_INCREMENT <br/> <br/>
        /// 
        ///  utilizzata da dbCheckStatus, TDataManager::CaricaDati, solo con NDB <br/>
        /// </summary>
        public bool dbResetNumOfOrders(int iInitialNumOfReceiptsParam)
        {
            bool bDBConnection_Ok;
            String sQueryTxt, sTmp;
            MySqlCommand cmd = new MySqlCommand();

            bDBConnection_Ok = dbInit(GetActualDate(), SF_Data.iNumCassa, true);

            // sicurezza : si prosegue solo se c'è la connessione al DB
            if (!bDBConnection_Ok)
                return false;

            // si prosegue solo se è CASSA_PRINCIPALE e c'è la connessione al DB
            if ((SF_Data.iNumCassa == CASSA_PRINCIPALE) && bUSA_NDB())
            {
                try
                {
                    cmd.Connection = _Connection;

                    // tabella numerazione ordini
                    // svuotamento eventuale TRUNCATE non c'è in SQLite -> DROP & CREATE !
                    sQueryTxt = String.Format("DROP TABLE IF EXISTS {0};", NOME_NSC_DBTBL);
                    cmd.CommandText = sQueryTxt;
                    var qResult = cmd.ExecuteScalar();
                    sTmp = String.Format("dbResetNumOfOrders dopo DROP TABLE IF EXISTS {0};", NOME_NSC_DBTBL);
                    LogToFile(sTmp);

                    // creazione tabella num_Ordini
                    sQueryTxt = String.Format(@"CREATE TABLE IF NOT EXISTS {0} (iOrdine_ID INT NOT NULL AUTO_INCREMENT,
                                iNumCassa INT UNSIGNED NOT NULL, sDataOra VARCHAR(50), PRIMARY KEY(iOrdine_ID)); ", NOME_NSC_DBTBL);

                    cmd.CommandText = sQueryTxt;
                    qResult = cmd.ExecuteScalar();
                    sTmp = String.Format("dbResetNumOfOrders dopo CREATE TABLE IF NOT EXISTS {0}", NOME_NSC_DBTBL);
                    LogToFile(sTmp);

                    sQueryTxt = String.Format("ALTER TABLE {0} AUTO_INCREMENT={1};", NOME_NSC_DBTBL, iInitialNumOfReceiptsParam);
                    cmd.CommandText = sQueryTxt;
                    qResult = cmd.ExecuteScalar();
                    sTmp = String.Format("dbResetNumOfOrders dopo ALTER TABLE {0} AUTO_INCREMENT={1};",
                                            NOME_NSC_DBTBL, iInitialNumOfReceiptsParam);
                    LogToFile(sTmp);

                    if (iInitialNumOfReceiptsParam > 0) // corregge bug di MySQL su AUTO_INCREMENT = 0
                    {
                        // qui sfrutta l' AUTO_INCREMENT
                        sTmp = GetActualDate().ToString("yyMMdd");

                        sQueryTxt = String.Format("INSERT INTO {0} (iOrdine_ID, iNumCassa, sDataOra) VALUES (NULL, {1}, \'{2}\');",
                                    NOME_NSC_DBTBL, SF_Data.iNumCassa, GetDateTimeString());
                        cmd.CommandText = sQueryTxt;
                        qResult = cmd.ExecuteScalar();

                        sTmp = String.Format("dbResetNumOfOrders dopo INSERT {0} , {1}, \'{2}\');", NOME_NSC_DBTBL, SF_Data.iNumCassa, GetDateTimeString());
                        LogToFile(sTmp);
                    }
                }

                catch (Exception)
                {
                    _WrnMsg.iErrID = WRN_DBE;
                    _WrnMsg.sMsg = String.Format("dbResetNumOfOrders : creazione tabella: {0}", bUSA_NDB());
                    WarningManager(_WrnMsg);
                    LogToFile("dbResetNumOfOrders : dbException creazione tabella");

                    return false;
                }
            }

            cmd.Dispose();
            return true;
        }

        /// <summary>
        ///   Utilizzata per resettare la tabella num_messaggi con il numero <br/>
        ///   progressivo degli ordini, utilizza AUTO_INCREMENT <br/> <br/>
        /// 
        ///  utilizzata da dbCheckStatus, TDataManager::CaricaDati, solo con NDB <br/>
        /// </summary>
        public bool dbResetNumOfMessages(int iInitialNumOfMessagesParam)
        {
            bool bDBConnection_Ok;
            String sQueryTxt, sTmp;

            MySqlCommand cmd = new MySqlCommand();

            bDBConnection_Ok = dbInit(GetActualDate(), SF_Data.iNumCassa, true);

            // sicurezza : si prosegue solo se c'è la connessione a DB
            if (!bDBConnection_Ok)
                return false;

            // si prosegue solo se è CASSA_PRINCIPALE e c'è la connessione a DB
            if ((SF_Data.iNumCassa == CASSA_PRINCIPALE) && bUSA_NDB())
            {
                try
                {
                    cmd.Connection = _Connection;

                    // tabella numerazione messaggi
                    // svuotamento eventuale TRUNCATE non c'è in SQLite -> DROP & CREATE !
                    sQueryTxt = String.Format("DROP TABLE IF EXISTS {0};", NOME_NMSG_DBTBL);
                    cmd.CommandText = sQueryTxt;
                    var qResult = cmd.ExecuteScalar();

                    // creazione tabella num_messaggi
                    sQueryTxt = String.Format(@"CREATE TABLE IF NOT EXISTS {0} (iMsg_ID INT NOT NULL AUTO_INCREMENT,
                                iNumCassa INT UNSIGNED NOT NULL, sDataOra VARCHAR(50), PRIMARY KEY(iMsg_ID)); ", NOME_NMSG_DBTBL);

                    cmd.CommandText = sQueryTxt;
                    qResult = cmd.ExecuteScalar();

                    sQueryTxt = String.Format("ALTER TABLE {0} AUTO_INCREMENT={1};", NOME_NMSG_DBTBL, iInitialNumOfMessagesParam);
                    cmd.CommandText = sQueryTxt;
                    qResult = cmd.ExecuteScalar();

                    if (iInitialNumOfMessagesParam > 0) // corregge bug di MySQL su AUTO_INCREMENT = 0
                    {
                        // qui sfrutta l' AUTO_INCREMENT
                        sTmp = GetActualDate().ToString("yyMMdd");

                        sQueryTxt = String.Format("INSERT INTO {0} (iMsg_ID, iNumCassa, sDataOra) VALUES (NULL, {1}, \'{2}\');",
                                    NOME_NMSG_DBTBL, SF_Data.iNumCassa, GetDateTimeString());
                        cmd.CommandText = sQueryTxt;
                        qResult = cmd.ExecuteScalar();

                        LogToFile("dbResetNumOfMessages : dopo CREATE TABLE num_messaggi");
                    }
                }

                catch (Exception)
                {
                    _WrnMsg.iErrID = WRN_DBE;
                    _WrnMsg.sMsg = String.Format("dbResetNumOfMessages : creazione tabella: {0}", bUSA_NDB());
                    WarningManager(_WrnMsg);
                    LogToFile("dbResetNumOfMessages : dbException creazione tabella");

                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Funzione di lettura del numero di Ordini ancora da stampare da parte di STAND_CUCINA<br/>
        /// bModeNextParam sceglie se ottenere i successivi o i precedenti
        /// </summary>
        public int dbGetNumOfPrintedOrders(int iNumOfReceiptParam, bool bModeNextParam)
        {
            int iNum;
            bool bDBConnection_Ok;
            String sTmp;
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataReader readerOrdine = null;

            iNum = 0;

            bDBConnection_Ok = dbInit(GetActualDate(), SF_Data.iNumCassa, true);

            // sicurezza : si prosegue solo se c'è la connessione a DB
            if (!bDBConnection_Ok)
                return 0;

            try
            {
                cmd.Connection = _Connection;

                if (bModeNextParam)
                {
                    cmd.CommandText = String.Format("SELECT iOrdine_ID FROM " + _sDBTNameOrdini + " WHERE (iOrdine_ID >= {0}) AND (sTipo_Articolo = \'{1}\') AND (iAnnullato = 0) AND (iStatus & 0x{2:X8} = 0)",
                        iNumOfReceiptParam, ORDER_CONST._START_OF_ORDER, SetBit(0, (int)STATUS_FLAGS.BIT_RECEIPT_STAMPATO_DA_STANDCUCINA));
                }
                else
                {
                    cmd.CommandText = String.Format("SELECT iOrdine_ID FROM " + _sDBTNameOrdini + " WHERE (iOrdine_ID < {0}) AND (sTipo_Articolo = \'{1}\') AND (iAnnullato = 0) AND (iStatus & 0x{2:X8} = 0)",
                        iNumOfReceiptParam, ORDER_CONST._START_OF_ORDER, SetBit(0, (int)STATUS_FLAGS.BIT_RECEIPT_STAMPATO_DA_STANDCUCINA));
                }

                readerOrdine = cmd.ExecuteReader();

                while ((readerOrdine != null) && readerOrdine.Read())
                {
                    iNum++;
                }

                sTmp = String.Format("iGetNumOfOrdersFromDB : iNum = {0}", iNum);
                LogToFile(sTmp);
            }

            catch (Exception)
            {
                readerOrdine?.Close();

                iNum = 0;

                _WrnMsg.iErrID = WRN_DBE;
                _WrnMsg.sMsg = "Connessione al Server DB_NSC non possibile";
                WarningManager(_WrnMsg);
            }

            return iNum; // tutto OK
        }

        /// <summary>Funzione di lettura del numero di Ordini emessi<br/>
        /// con 2 diverse modalità
        /// </summary>
        public int dbGetNumOfOrdersFromDB(bool bModeParamStandard)
        {
            int iNum;
            bool bDBConnection_Ok;
            String sTmp;
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataReader readerOrdine = null;

            iNum = 0;

            bDBConnection_Ok = dbInit(GetActualDate(), SF_Data.iNumCassa, true);

            // sicurezza : si prosegue solo se c'è la connessione a DB
            if (!bDBConnection_Ok)
                return 0;

            try
            {
                cmd.Connection = _Connection;

                if (bModeParamStandard)
                    cmd.CommandText = "SELECT iOrdine_ID FROM " + NOME_NSC_DBTBL + " ORDER BY iOrdine_ID DESC LIMIT 1";
                else
                    cmd.CommandText = "select iOrdine_ID FROM " + _sDBTNameOrdini + " WHERE iOrdine_ID > 0 ORDER BY iOrdine_ID DESC LIMIT 1";

                readerOrdine = cmd.ExecuteReader();

                if ((readerOrdine != null) && readerOrdine.HasRows)
                {
                    readerOrdine.Read();

                    iNum = readerOrdine.GetInt32("iOrdine_ID");
                    readerOrdine.Close();
                }

                sTmp = String.Format("iGetNumOfOrdersFromDB : iNum = {0}", iNum);
                LogToFile(sTmp);
            }

            catch (Exception)
            {
                readerOrdine?.Close();

                iNum = 0;

#if !STAND_CUCINA
                _WrnMsg.iErrID = WRN_DBE;
                _WrnMsg.sMsg = "Connessione al Server DB_NSC non possibile";
                WarningManager(_WrnMsg);
#endif
            }

            return iNum; // tutto OK
        }

        /// <summary>
        /// Funzione di lettura del numero di Messaggi emessi
        /// con 2 diverse modalità
        /// </summary>
        public int dbGetNumOfMessagesFromDB(bool bModeParamStandard)
        {
            int iNum;
            bool bDBConnection_Ok;
            String sTmp;
            MySqlCommand cmd = new MySqlCommand();

            bDBConnection_Ok = dbInit(GetActualDate(), SF_Data.iNumCassa, true);

            // sicurezza : si prosegue solo se c'è la connessione a DB
            if (!bDBConnection_Ok)
                return 0;

            try
            {
                cmd.Connection = _Connection;

                if (bModeParamStandard)
                {
                    cmd.CommandText = "SELECT iMsg_ID FROM " + NOME_NMSG_DBTBL + " ORDER BY iMsg_ID DESC LIMIT 1";
                    iNum = int.Parse(cmd.ExecuteScalar() + "");
                }
                else
                {
                    cmd.CommandText = "select iOrdine_ID FROM " + _sDBTNameOrdini + " WHERE iOrdine_ID < 0 ORDER BY iOrdine_ID ASC LIMIT 1";
                    iNum = -int.Parse(cmd.ExecuteScalar() + "");
                }

                sTmp = String.Format("iGetNumOfMessagesFromDB : iNum = {0}", iNum);
                LogToFile(sTmp);
            }

            catch (Exception)
            {
                iNum = 0;

#if !STAND_CUCINA
                _WrnMsg.iErrID = WRN_DBE;
                _WrnMsg.sMsg = "Connessione al Server DB_NMSG non possibile";
                WarningManager(_WrnMsg);
#endif
            }

            return iNum; // tutto OK
        }

        /// <summary>funzione di lettura parametri per accesso database remoto</summary>
        public TWebServerParams dbGetWebServerParams()
        {
            bool bDBConnection_Ok;
            String sKey, sText;
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataReader readerStatus = null;
            TWebServerParams sWebServerParams = new TWebServerParams(0);


            bDBConnection_Ok = dbInit(GetActualDate(), SF_Data.iNumCassa, true);

            // sicurezza : si prosegue solo se c'è la connessione a DB
            if (!bDBConnection_Ok)
                return new TWebServerParams(0);

            try
            {
                cmd.Connection = _Connection;
                cmd.CommandText = "SELECT * FROM " + NOME_STATO_DBTBL + ";";
                readerStatus = cmd.ExecuteReader();

                while (readerStatus.Read())
                {
                    sKey = readerStatus.GetString("key_ID");
                    sText = readerStatus.GetString("sText");

                    switch (sKey)
                    {
                        case WEB_SERVER_NAME_KEY:
                            sWebServerParams.sWebTablePrefix = sText;
                            break;
                        case WEB_DBASE_NAME_KEY:
                            sWebServerParams.sWeb_DBase = sText;
                            break;
                        case WEB_DBASE_PWD_KEY:
                            sWebServerParams.sWebEncryptedPwd = sText;
                            break;
                        default:
                            break;
                    }
                }

                readerStatus?.Close();

                LogToFile("dbGetWebServerParams: parametri letti");
            }

            catch (Exception)
            {
                if (readerStatus != null)
                {
                    readerStatus.Close();

                    _WrnMsg.iErrID = WRN_DBE;
                    _WrnMsg.sMsg = "Lettura Stato non possibile";
                    WarningManager(_WrnMsg);
                }
            }

            return sWebServerParams; // tutto OK
        }

        /// <summary>funzione di scrittura parametri per accesso database remoto</summary>
        public bool dbSetWebServerParams(TWebServerParams sWebServerParams, TWebServerCheckParams sWebServerCheckParams)
        {
            bool bDBConnection_Ok;
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataReader readerStatus = null;

            if (String.IsNullOrEmpty(sWebServerCheckParams.sDB_ServerName))
            {
                bDBConnection_Ok = dbInit(GetActualDate(), SF_Data.iNumCassa, true);

                // sicurezza : si prosegue solo se c'è la connessione a DB
                if (!bDBConnection_Ok)
                    return false;
            }
            else
            {
                _dbCSB.Host = sWebServerCheckParams.sDB_ServerName;
                _dbCSB.Database = _database;
                _dbCSB.UserId = _uid;
                _dbCSB.Password = sWebServerCheckParams.sDB_pwd;
                _dbCSB.FoundRows = true;
                _dbCSB.Pooling = false;
                _dbCSB.Unicode = true;
                _dbCSB.ConnectionTimeout = TIMEOUT_DB_OPEN;
            }

            if (SF_Data.iNumCassa == CASSA_PRINCIPALE)
            {
                try
                {
                    if (!String.IsNullOrEmpty(sWebServerCheckParams.sDB_ServerName))
                    {
                        _Connection = new MySqlConnection(_dbCSB.ConnectionString);
                        _Connection.Open();
                    }

                    cmd.Connection = _Connection;

                    cmd.CommandText = "UPDATE " + NOME_STATO_DBTBL + " SET sText = '" + sWebServerParams.sWebTablePrefix + "' WHERE (key_ID = '" + WEB_SERVER_NAME_KEY + "')";
                    readerStatus = cmd.ExecuteReader();

                    cmd.CommandText = "UPDATE " + NOME_STATO_DBTBL + " SET sText = '" + sWebServerParams.sWeb_DBase + "' WHERE (key_ID = '" + WEB_DBASE_NAME_KEY + "')";
                    readerStatus = cmd.ExecuteReader();

                    cmd.CommandText = "UPDATE " + NOME_STATO_DBTBL + " SET sText = '" + sWebServerParams.sWebEncryptedPwd + "' WHERE (key_ID = '" + WEB_DBASE_PWD_KEY + "')";
                    readerStatus = cmd.ExecuteReader();

                    readerStatus?.Close();

                    LogToFile("dbSetWebServerParams: parametri scritti");
                }

                catch (Exception)
                {
                    readerStatus.Close();

                    _WrnMsg.iErrID = WRN_DBE;
                    _WrnMsg.sMsg = "Scrittura Stato non possibile";
                    WarningManager(_WrnMsg);

                    return false;
                }
            }
            return true; // tutto OK
        }

        /// <summary>
        /// Funzione di lettura degli Ordini emessi dalle casse secondarie, <br/>
        /// ritorna 0 se non ce ne sono, negativo per gli ordini annullati<br/><br/>
        /// 
        /// chiamata da  AggiornaDisponibilità per aggiornare la disponibilità della CASSA_PRINCIPALE<br/>
        /// e dalla CASSA_SECONDARIA per attendere lo svuotamento prima di aggiornare la sua disponibilità
        /// bClearOrdiniParam = true consente di eliminare un ordine dalla tabella ma solo se è CASSA_PRINCIPALE
        /// </summary>
        public int dbClearOrdiniCSec(bool bClearOrdiniParam)
        {
            bool bDBConnection_Ok;
            int iOrderNum = 0;
            String sTmp;

            MySqlCommand cmd = new MySqlCommand();
            MySqlDataReader readerCSec = null;

            bDBConnection_Ok = dbInit(GetActualDate(), SF_Data.iNumCassa, true);

            // sicurezza : si prosegue solo se c'è la connessione a MySQL
            // o se la Cassa secondaria non richiede il prelievo di un ordine dalla lista
            if (!bDBConnection_Ok && ((SF_Data.iNumCassa != CASSA_PRINCIPALE) && bClearOrdiniParam))
                return 0;

            try
            {
                cmd.CommandText = "SELECT * FROM " + NOME_DISP_DBTBL + " ORDER BY iOrdine_ID ASC LIMIT 1;";
                cmd.Connection = _Connection;

                readerCSec = cmd.ExecuteReader();

                if ((readerCSec != null) && readerCSec.HasRows)
                {
                    readerCSec.Read();

                    iOrderNum = readerCSec.GetInt32("iOrdine_ID");

                    readerCSec.Close();

                    if (bClearOrdiniParam)
                    {
                        cmd.CommandText = String.Format("DELETE FROM " + NOME_DISP_DBTBL + " WHERE iOrdine_ID = {0}", iOrderNum);

                        cmd.ExecuteNonQuery();

                        sTmp = String.Format("iGetOrdiniInCoda : cancellazione disp {0}", iOrderNum);
                        LogToFile(sTmp);
                    }
                    else
                        iOrderNum = 1; // ci sono ordini in attesa
                }
                else
                    iOrderNum = 0;
            }

            catch (Exception)
            {
                _WrnMsg.iErrID = WRN_DBE;
                _WrnMsg.sMsg = String.Format("accesso alla tabella DISP_DBTBL non possibile");
                WarningManager(_WrnMsg);
            }

            readerCSec?.Close();

            return iOrderNum; // tutto OK
        }

        /// <summary>
        /// funziona che ottiene gli ordini web serviti
        /// </summary>
        public int dbGetOrdiniWebServiti()
        {
            bool bDBConnection_Ok;
            int iOrderNum = 0;

            MySqlCommand cmd = new MySqlCommand();
            MySqlDataReader readerCSec = null;

#pragma warning disable IDE0059
            bDBConnection_Ok = dbInitWeb();

            try
            {
                cmd.CommandText = "SELECT * FROM " + NOME_WEBORD_DBTBL + " ORDER BY iOrdine_ID ASC LIMIT 1;";
                cmd.Connection = _ConnectionWeb;

                readerCSec = cmd.ExecuteReader();

                if ((readerCSec != null) && readerCSec.HasRows)
                {
                    readerCSec.Read();

                    iOrderNum = readerCSec.GetInt32("iOrdine_ID");
                }
                else
                    iOrderNum = 0;
            }

            catch (Exception)
            {
                _WrnMsg.iErrID = WRN_DBE;
                _WrnMsg.sMsg = String.Format("accesso alla tabella WEBORD_DBTBL non possibile");
                WarningManager(_WrnMsg);
            }

            readerCSec?.Close();

            return iOrderNum; // tutto OK
        }

        /// <summary>
        /// funziona che contrassegna gli ordini web serviti
        /// </summary>
        public bool dbClearOrdineWebServito(int iOrderParam)
        {
            bool bDBConnection_Ok;
            int iOrderNum = 0;
            String sTmp;

            MySqlCommand cmd = new MySqlCommand();

            bDBConnection_Ok = dbInitWeb();

            // sicurezza : si prosegue solo se c'è la connessione a MySQL
            // o se la Cassa secondaria non richiede il prelievo di un ordine dalla lista
            if (!bDBConnection_Ok && (SF_Data.iNumCassa != CASSA_PRINCIPALE))
                return false;

            try
            {
                cmd.CommandText = String.Format("DELETE FROM " + NOME_WEBORD_DBTBL + " WHERE iOrdine_ID = {0}", iOrderParam);
                cmd.Connection = _ConnectionWeb;

                cmd.ExecuteNonQuery();

                sTmp = String.Format("dbClearOrdineWebServito : cancellazione da coda ordine web {0}", iOrderNum);
                LogToFile(sTmp, true);
            }

            catch (Exception)
            {
                _WrnMsg.iErrID = WRN_DBE;
                _WrnMsg.sMsg = String.Format("dbClearOrdineWebServito :accesso alla tabella WEBORD_DBTBL non possibile");
                WarningManager(_WrnMsg);
                return false;
            }

            return true; // tutto OK
        }

        /// <summary>
        /// annulla uno scontrino contrassegnando cancellation, cancellationTime e poi carica i dati, <br/>
        /// iNumAnnulloParam è il numero dell'ordine
        /// </summary>
        public bool dbAnnulloOrdine(DateTime dateParam, int iNumAnnulloParam, String sNomeTabellaParam = "")
        {
            bool bAnnullato, bDBConnection_Ok;

            int iCount = 0, iUpdatedRows;
            int i, iNumCassa;

            String sTmp, sTipo, sQueryTxt;

            MySqlCommand cmd = new MySqlCommand();
            MySqlDataReader readerOrdini = null;
            MySqlDataAdapter dbDataAdapterSelect = null;

            DataTable dataTable = new DataTable();

            // dbAzzeraDatiGen(); non serve e poi cancella gli Headers
            dbAzzeraDatiOrdine(ref DB_Data);

            bAnnullato = false;
            iNumCassa = SF_Data.iNumCassa;
            bDBConnection_Ok = dbInit(dateParam, SF_Data.iNumCassa, false, sNomeTabellaParam);

            // sicurezza : si prosegue solo se c'è la connessione a DB
            if (!bDBConnection_Ok)
                return false;

            try
            {
                cmd.CommandText = "SELECT * FROM " + _sDBTNameOrdini + " LIMIT 1;";
                cmd.Connection = _Connection;

                // prima prova di connessione al DB per impostare readerDati != null
                readerOrdini = cmd.ExecuteReader();
            }

            catch (Exception)
            {
            }

            if ((readerOrdini != null) && bDBConnection_Ok)
            {
                readerOrdini.Close();

                LogToFile("dbAnnulloOrdine : bEseguiParam = true");

                try // *********** 1 *********
                {

                    // riempie con tutte le colonne
                    sQueryTxt = String.Format("SELECT * FROM {0} WHERE iOrdine_ID = {1};", _sDBTNameOrdini, iNumAnnulloParam);
                    dbDataAdapterSelect = new MySqlDataAdapter(sQueryTxt, _Connection);
                    iCount = dbDataAdapterSelect.Fill(dataTable);

                    // dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["sTipo_Articolo"] };

                    if (iCount == 0) // record non presente ma connessione OK
                    {
                        bAnnullato = false;

                        _WrnMsg.sMsg = iNumAnnulloParam.ToString();
                        _WrnMsg.iErrID = WRN_RNF;
                        WarningManager(_WrnMsg);

                        sTmp = String.Format("dbAnnulloOrdine : record {0} non esiste!", iNumAnnulloParam);
                        LogToFile(sTmp);
                    }
                    else // record presente
                    {
                        // controlla se l' Articolo è già stato annullato
                        bAnnullato = Convert.ToBoolean(dataTable.Rows[0]["iAnnullato"]);

                        /****************************************************************
                         *   controlla da che cassa è stato emesso lo scontrino         *
                         *   attenzione che iNumCassa parte da 1 "Cassa n.1 Principale, *
                         *   2 = Cassa n.2 Secondaria, etc                              *
                         ****************************************************************/
                        iNumCassa = Convert.ToInt32(dataTable.Rows[0]["iNumCassa"]);

                        sTmp = String.Format("dbAnnulloOrdine : pos 1, cassa = {0}", iNumCassa);
                        LogToFile(sTmp);
                    }
                }

                catch (Exception)
                {
                    _WrnMsg.iErrID = WRN_DBE;
                    _WrnMsg.sMsg = "dbAnnulloOrdine 1";
                    WarningManager(_WrnMsg);

                    LogToFile("dbAnnulloOrdine : dbException 1");
                }
            }
            else
                LogToFile("dbAnnulloOrdine : bEseguiParam = false");

            if ((iCount > 0) && (iNumCassa == SF_Data.iNumCassa))
            {

                try // *********** 2 **********
                {
                    MySqlTransaction transaction = _Connection.BeginTransaction();

                    MySqlDataAdapter dbDataAdapter = new MySqlDataAdapter();

                    sQueryTxt = String.Format(@"UPDATE {0} SET iAnnullato = @iAnnullato, sAnnullato = @sAnnullato WHERE
                         sTipo_Articolo = @oldId AND iOrdine_ID = {1};", _sDBTNameOrdini, iNumAnnulloParam);

                    dbDataAdapter.UpdateCommand = new MySqlCommand(sQueryTxt, _Connection);

                    dbDataAdapter.UpdateCommand.Parameters.Add("@iAnnullato", MySqlType.Int, 11, "iAnnullato");
                    dbDataAdapter.UpdateCommand.Parameters.Add("@sAnnullato", MySqlType.VarChar, 50, "sAnnullato");

                    dbDataAdapter.UpdateCommand.Parameters.Add("@oldId", MySqlType.VarChar, 50, "sTipo_Articolo").SourceVersion = DataRowVersion.Original;
                    dbDataAdapter.UpdateCommand.UpdatedRowSource = UpdateRowSource.None;

                    for (i = 0; i < iCount; i++)
                    {
                        sTipo = Convert.ToString(dataTable.Rows[i]["sTipo_Articolo"]);

                        if (sTipo == ORDER_CONST._START_OF_ORDER)
                            DB_Data.iStatusReceipt = Convert.ToInt32(dataTable.Rows[i]["iStatus"]);

                        DB_Data.Articolo[i].sTipo = sTipo;

                        if (!StringBelongsTo_ORDER_CONST(sTipo))
                        {
                            DB_Data.Articolo[i].iQuantitaOrdine = Convert.ToInt32(dataTable.Rows[i]["iQuantita_Ordine"]);
                            DB_Data.Articolo[i].iPrezzoUnitario = Convert.ToInt32(dataTable.Rows[i]["iPrezzo_Unitario"]);
                            DB_Data.Articolo[i].iGruppoStampa = Convert.ToInt32(dataTable.Rows[i]["iGruppo_Stampa"]);
                        }

                        // aggiorna solo se non è già stato annullato !!!
                        if (!bAnnullato)
                        {
                            /******************************
                             *		aggiornamento DB
                             ******************************/

                            dataTable.Rows[i]["iAnnullato"] = 1;

                            if (sTipo == ORDER_CONST._START_OF_ORDER)
                            {
                                sTmp = DateTime.Now.ToString("HH.mm.ss");

                                dataTable.Rows[i]["sAnnullato"] = sTmp;

                                sTmp = String.Format("dbAnnulloOrdine : trovato iOrdine_ID = {0}", iNumAnnulloParam);
                                LogToFile(sTmp);
                            }
                        }
                        else
                        {
                            // già annullato !!!
                            _WrnMsg.sMsg = iNumAnnulloParam.ToString();
                            _WrnMsg.iErrID = WRN_SNE;
                            WarningManager(_WrnMsg);

                            sTmp = String.Format("dbAnnulloOrdine : iOrdine_ID = {0} già annullato!", iNumAnnulloParam);
                            LogToFile(sTmp);

                            return false;
                        }

                    } // end while

                    LogToFile("dbAnnulloOrdine : pos 2");

                    // aggiorna il database su disco
                    LogToFile("dbAnnulloOrdine : ApplyUpdates");

                    /*** aggiorna il database su disco ***/
                    dbDataAdapter.UpdateBatchSize = 100;
                    iUpdatedRows = dbDataAdapter.Update(dataTable);

                    Console.WriteLine("dbAnnulloOrdine : iUpdatedRows = {0}", iUpdatedRows);

                    transaction.Commit();

                    dbDataAdapter.Dispose();
                    dbDataAdapterSelect.Dispose();

                    bAnnullato = true;

#if STANDFACILE
                    VisMessaggiDlg rVisMessaggiDlg = new VisMessaggiDlg();

                    rVisMessaggiDlg.ScriviMessaggioAnnullo(iNumAnnulloParam);
#endif

                    // Annullo corretto
                    _WrnMsg.sMsg = iNumAnnulloParam.ToString();
                    _WrnMsg.iErrID = WRN_SEX;

#if STANDFACILE
                    if (!CheckService(Define.CFG_SERVICE_STRINGS._AUTO_SEQ_TEST))
#endif
                    WarningManager(_WrnMsg);

                    sTmp = String.Format("dbAnnulloOrdine : annullo eseguito iOrdine_ID = {0} !", iNumAnnulloParam);
                    LogToFile(sTmp);
                }

                catch (Exception)
                {
                    _WrnMsg.iErrID = WRN_DBE;
                    _WrnMsg.sMsg = "dbAnnulloOrdine 2";
                    WarningManager(_WrnMsg);
                    LogToFile("dbAnnulloOrdine : dbException 2");
                }
            }
            else // cassa non coerente
            {
                if (iNumCassa != SF_Data.iNumCassa)
                {
                    _WrnMsg.sMsg = iNumAnnulloParam.ToString();
                    _WrnMsg.iErrID = WRN_SNE;
                    WarningManager(_WrnMsg);

                    sTmp = String.Format("dbAnnulloOrdine : iOrdine_ID = {0} emesso da altra cassa {1} questa = {2}!", iNumAnnulloParam, iNumCassa,
                            SF_Data.iNumCassa);
                    LogToFile(sTmp);
                }
                else if (readerOrdini == null)
                    LogToFile("dbAnnulloOrdine : readerOrdini == null");
            }

            cmd.Dispose();

            return bAnnullato;
        }

        /// <summary>
        /// cerca il checksum nella tabella "Listino" ne ritorna la stringa se ha successo,  <br/>
        /// altrimenti ritorna "", solo con NDB <br/>
        /// </summary>
        public String dbCheckListino()
        {
            bool bDBConnection_Ok;
            int i;
            String sRecord = "";
            MySqlCommand cmd = new MySqlCommand();

            bDBConnection_Ok = dbInit(GetActualDate(), SF_Data.iNumCassa);

            // sicurezza : si prosegue solo se c'è la connessione a DB
            if (!bDBConnection_Ok)
                return "";

            using (MySqlConnection dbConnection = new MySqlConnection(_dbCSB.ConnectionString))
            {
                try
                {
                    i = 0;

                    dbConnection.Open();
                    cmd.Connection = dbConnection;

                    cmd.CommandText = "SELECT sText FROM " + NOME_LISTINO_DBTBL + " ORDER BY iRiga_ID DESC LIMIT 2";

                    MySqlDataReader readerListino = cmd.ExecuteReader();

                    while (readerListino.Read() && (i < MAX_NUM_ARTICOLI + EXTRA_NUM_LISTINO_HEAD))
                    {
                        sRecord = readerListino.GetString("sText");

                        if (sRecord.StartsWith("#CKL"))
                        {
                            sRecord = sRecord.Substring(5);
                            break;
                        }

                        i++;
                    }

                    readerListino?.Close();

                    LogToFile("dbCheckListino : ricerca checksum eseguita");
                }

                catch (Exception)
                {
                    LogToFile("dbCheckListino : dbException");
                    return "";
                }
                return sRecord; // tutto OK
            }
        }

        /// <summary>
        /// aggiunge il suffisso alle tabelle <br/>
        /// usato da chiudiIncasso()
        /// </summary>
        public bool dbRenameTables(String sNewPostFix)
        {
            bool bDBConnection_Ok, bSuccess = true;
            int iNumCassa;
            String sQueryTxt, sNewTName;
            MySqlCommand cmd = new MySqlCommand();

            DateTime dateParam = GetActualDate();

            for (iNumCassa = 1; iNumCassa <= (MAX_CASSE_SECONDARIE + 1); iNumCassa++)
            {
                // imposta anche la directory con il full path
                bDBConnection_Ok = dbInit(dateParam, iNumCassa);

                // sicurezza : si prosegue solo se c'è la connessione a DB
                if (!bDBConnection_Ok)
                    return false;

                /***********************************
                 *		RENAME ClientDS_Dati
                 ***********************************/

                sNewTName = _sDBTNameDati + sNewPostFix;

                try
                {
                    cmd.Connection = _Connection;

                    // Query di rename tabella
                    sQueryTxt = String.Format("ALTER TABLE {0} RENAME TO {1};", _sDBTNameDati, sNewTName);
                    cmd.CommandText = sQueryTxt;
                    var qResult = cmd.ExecuteScalar();

                    LogToFile("dbRenameTables : RENAME TABLE DATI");
                }

                catch (Exception)
                {
                    // SF_Data.iNumCassa per SQLite, CASSA_PRINCIPALE per NDB
                    if (iNumCassa == CASSA_PRINCIPALE) // la secondaria è facoltativa
                    {
                        bSuccess = false;

                        _WrnMsg.iErrID = WRN_DBE;
                        _WrnMsg.sMsg = String.Format("RENAME DATI {0}", bUSA_NDB());
                        WarningManager(_WrnMsg);
                        LogToFile("dbRenameTables : dbException RENAME DATI");
                    }
                }
            }

            /**************************************
             *		RENAME ClientDS_Ordini
             **************************************/

            sNewTName = _sDBTNameOrdini + sNewPostFix;

            try
            {
                // Query di rename tabella
                sQueryTxt = String.Format("ALTER TABLE {0} RENAME TO {1};", _sDBTNameOrdini, sNewTName);
                cmd.CommandText = sQueryTxt;
                var qResult = cmd.ExecuteScalar();

                LogToFile("dbRenameTables : RENAME TABLE ORDINI");
            }

            catch (Exception)
            {

                bSuccess = false;
                _WrnMsg.iErrID = WRN_DBE;
                _WrnMsg.sMsg = String.Format("RENAME DATI {0}", bUSA_NDB());
                WarningManager(_WrnMsg);
                LogToFile("dbRenameTables : dbException RENAME ORDINI");
            }

            return bSuccess;
        }

        /// <summary>
        /// aggiunge il suffisso alla tabella selezionata <br/>
        /// usato da EsploraDB_Dlg()
        /// </summary>
        public bool dbRenameTable(String sOldTabellaParam, String sNewTabellaParam)
        {
            bool bDBConnection_Ok, bSuccess = true;
            String sTmp, sQueryTxt;
            MySqlCommand cmd = new MySqlCommand();

            bDBConnection_Ok = dbInit(GetActualDate(), SF_Data.iNumCassa, false, sOldTabellaParam);

            // sicurezza : si prosegue solo se c'è la connessione a DB
            if (!bDBConnection_Ok)
                return false;

            try
            {
                cmd.Connection = _Connection;

                // Query di rename tabella
                sQueryTxt = String.Format("ALTER TABLE {0} RENAME TO {1};", sOldTabellaParam, sNewTabellaParam);
                cmd.CommandText = sQueryTxt;
                var qResult = cmd.ExecuteScalar();

                sTmp = String.Format("dbRenameTable : RENAME TABLE {0} in {1}", sOldTabellaParam, sNewTabellaParam);
                LogToFile(sTmp);
            }

            catch (Exception)
            {
                if (bUSA_NDB())
                {
                    bSuccess = false;
                    _WrnMsg.iErrID = WRN_DBE;
                    _WrnMsg.sMsg = String.Format("RENAME TABLE {0}", bUSA_NDB());
                    WarningManager(_WrnMsg);
                }
            }

            return bSuccess;
        }

        /// <summary>usato da EraseAllaData()</summary>
        public bool dbDropTables()
        {
            bool bDBConnection_Ok, bSuccess = true;
            String sQueryTxt, sDBTLocNameDati;
            int iNumCassa;

            DateTime dateParam = GetActualDate();
            MySqlCommand cmd = new MySqlCommand();

            bDBConnection_Ok = dbInit(dateParam, SF_Data.iNumCassa, true);

            // sicurezza : si prosegue solo se c'è la connessione a DB
            if (!bDBConnection_Ok)
                return false;

            try // dati casse
            {
                cmd.Connection = _Connection;

                /*************************************
                 *		DROP ClientDS_Dati
                 *************************************/
                for (iNumCassa = 1; iNumCassa <= (MAX_CASSE_SECONDARIE + 1); iNumCassa++)
                {

                    sDBTLocNameDati = GetNomeDatiDBTable(iNumCassa, dateParam);

                    // Query di drop tabella
                    sQueryTxt = String.Format("DROP TABLE IF EXISTS {0};", sDBTLocNameDati);
                    cmd.CommandText = sQueryTxt;
                    cmd.ExecuteScalar();

                    LogToFile("dbDropTables : DROP TABLE DATI 0");
                }

                /**************************************************
                 *			DROP TABLE LISTINO
                 *	non è necessario eliminare questa tabella,
                 *	tanto poi viene ricostruita
                 **************************************************/

                /*************************************
                 *		DROP ClientDS_Ordini
                 *************************************/

                // Query di drop tabella
                sQueryTxt = String.Format("DROP TABLE IF EXISTS {0};", _sDBTNameOrdini);
                cmd.CommandText = sQueryTxt;
                cmd.ExecuteScalar();

                sQueryTxt = String.Format("TRUNCATE TABLE {0};", NOME_DISP_DBTBL);
                cmd.CommandText = sQueryTxt;
                cmd.ExecuteScalar();

                dbResetNumOfOrders(0);

                dbResetNumOfMessages(0);

                LogToFile("dbDropTables : DROP TABLE ORDINI");

            }
            catch (Exception)
            {
                if (bUSA_NDB())
                {
                    bSuccess = false;
                    _WrnMsg.iErrID = WRN_DBE;
                    _WrnMsg.sMsg = String.Format("DROP TABLE ORDINI {0}", bUSA_NDB());
                    WarningManager(_WrnMsg);
                }
            }

            return bSuccess;
        }

        /// <summary>usato da EsploraDB_Dlg</summary>
        public bool dbDropTable(String sNomeTabellaParam)
        {
            bool bDBConnection_Ok, bSuccess = true;
            String sQueryTxt, sTmp;
            MySqlCommand cmd = new MySqlCommand();

            bDBConnection_Ok = dbInit(GetActualDate(), SF_Data.iNumCassa, false, sNomeTabellaParam);

            // sicurezza : si prosegue solo se c'è la connessione a DB
            if (!bDBConnection_Ok)
                return false;

            try
            {
                cmd.Connection = _Connection;

                // Query di drop tabella
                sQueryTxt = String.Format("DROP TABLE IF EXISTS {0};", sNomeTabellaParam);
                cmd.CommandText = sQueryTxt;
                cmd.ExecuteScalar();


                sTmp = String.Format("dbDropTable : DROP TABLE {0}", sNomeTabellaParam);
                LogToFile(sTmp);
            }
            catch (Exception)
            {
                if (bUSA_NDB())
                {
                    bSuccess = false;
                    _WrnMsg.iErrID = WRN_DBE;
                    _WrnMsg.sMsg = String.Format("DROP TABLE {0}", sNomeTabellaParam);
                    WarningManager(_WrnMsg);
                }
            }

            return bSuccess;
        }

        /// <summary>
        /// Ottiene elenco delle tabelle del Database,<br/><br/>
        /// ritorna il numero di tabelle (ed elenco delle tabelle) se ha successo,<br/>
        /// 0 altrimenti, usato da EsploraDB_Dlg <br/>
        /// </summary>
        public int dbElencoTabelle(List<String> sStringsParam)
        {
            bool bDBConnection_Ok;
            MySqlDataReader readerTables = null;
            MySqlCommand cmd = new MySqlCommand();

            bDBConnection_Ok = dbInit(GetActualDate(), SF_Data.iNumCassa);

            // sicurezza : si prosegue solo se c'è la connessione a DB
            if (!bDBConnection_Ok)
                return 0;

            cmd.Connection = _Connection;

            cmd.CommandText = "SELECT TABLE_NAME FROM information_schema.TABLES WHERE TABLE_SCHEMA = \'" +
                                _database + "\' ORDER BY TABLE_NAME ASC;";

            try
            {
                readerTables = cmd.ExecuteReader();

                while (readerTables.Read())
                {
                    sStringsParam.Add(readerTables.GetString(0));
                }
            }

            catch (Exception)
            {
                _WrnMsg.iErrID = WRN_DBE;
                _WrnMsg.sMsg = "Elenca Tabelle";
                WarningManager(_WrnMsg);
                LogToFile("dbElencoTabelle : dbException RENAME DATI");
            }

            readerTables?.Close();

            return sStringsParam.Count;
        }

        /// <summary>
        /// Funzione di richiesta nuovo numero di scontrino <br/>
        /// solo con NDB, chiamata da BtnScontrinoClick
        /// </summary>
        public int dbNewOrdineNumRequest()
        {
            bool bDBConnection_Ok;
            String sQueryTxt;
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataReader readerOrdineNum = null;

            bDBConnection_Ok = dbInit(GetActualDate(), SF_Data.iNumCassa, true);

            // sicurezza : si prosegue solo se c'è la connessione a DB
            if (!bDBConnection_Ok)
                return 0;

            try
            {
                _bCheckStatus = dbCheckStatus();

                // controllo di coerenza della data
                if (_bCheckStatus)
                {
                    cmd.Connection = _Connection;

                    // qui sfrutta l' AUTO_INCREMENT
                    sQueryTxt = String.Format("INSERT INTO {0} (iOrdine_ID, iNumCassa, sDataOra) VALUES (NULL, {1}, \'{2}\');",
                        NOME_NSC_DBTBL, SF_Data.iNumCassa, GetDateTimeString());
                    cmd.CommandText = sQueryTxt;
                    var qResult = cmd.ExecuteScalar();

                    LogToFile("dbNewOrdineNumRequest : dopo INSERT INTO");

                    // legge il numero di scontrino dal dbServer filtrando per cassa
                    cmd.CommandText = String.Format(@"SELECT * FROM " + NOME_NSC_DBTBL +
                        " WHERE iNumCassa = {0} ORDER BY iOrdine_ID DESC LIMIT 1", SF_Data.iNumCassa);

                    readerOrdineNum = cmd.ExecuteReader();
                    readerOrdineNum.Read();

                    _iNumOfLocalOrdersFromDB = readerOrdineNum.GetInt32("iOrdine_ID");
                    _sDateTimeFromDB = readerOrdineNum.GetString("sDataOra");

                    LogToFile("dbNewOrdineNumRequest : _iNumOfLocalOrdersFromDB letto");

                    readerOrdineNum?.Close();

                    return _iNumOfLocalOrdersFromDB; // tutto OK
                }
                else
                {
                    readerOrdineNum?.Close();

                    return 0;
                }
            }

            catch (Exception)
            {
                readerOrdineNum?.Close();

                _WrnMsg.iErrID = WRN_DBE;
                _WrnMsg.sMsg = "Connessione al Server DB_NSC non possibile";
                WarningManager(_WrnMsg);

#if STANDFACILE
                // bypass del Server NSC solo se CASSA_PRINCIPALE
                if (!DataManager.CheckIf_CassaSec_and_NDB())
                {
                    _iNumOfLocalOrdersFromDB++;
                    _sDateTimeFromDB = GetDateTimeString();

                    LogToFile("#dbNewOrdineNumRequest : bypass del Server DB_NSC");
                    return _iNumOfLocalOrdersFromDB; // tutto OK
                }
                else
#endif
                    return 0;
            }
        }

        /// <summary>
        /// Funzione di richiesta nuovo numero di messaggio <br/>
        /// solo con NDB, chiamata da VisMessaggiDlg
        /// </summary>
        public int dbNewMessageNumRequest()
        {
            bool bDBConnection_Ok;
            String sQueryTxt;
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataReader readerMessaggioNum = null;

            bDBConnection_Ok = dbInit(GetActualDate(), SF_Data.iNumCassa, true);

            // sicurezza : si prosegue solo se c'è la connessione a DB
            if (!bDBConnection_Ok)
                return 0;

            try
            {
                _bCheckStatus = dbCheckStatus();

                // controllo di coerenza della data
                if (_bCheckStatus)
                {
                    cmd.Connection = _Connection;

                    // qui sfrutta l' AUTO_INCREMENT
                    sQueryTxt = String.Format("INSERT INTO {0} (iMsg_ID, iNumCassa, sDataOra) VALUES (NULL, {1}, \'{2}\');",
                        NOME_NMSG_DBTBL, SF_Data.iNumCassa, GetDateTimeString());
                    cmd.CommandText = sQueryTxt;
                    var qResult = cmd.ExecuteScalar();

                    LogToFile("dbNewMessageNumRequest : dopo INSERT INTO");

                    // legge il numero di scontrino dal dbServer filtrando per cassa
                    cmd.CommandText = String.Format(@"SELECT * FROM " + NOME_NMSG_DBTBL +
                        " WHERE iNumCassa = {0} ORDER BY iMsg_ID DESC LIMIT 1", SF_Data.iNumCassa);

                    readerMessaggioNum = cmd.ExecuteReader();
                    readerMessaggioNum.Read();

                    _iNumOfLastMessageFromDB = readerMessaggioNum.GetInt32("iMsg_ID");
                    _sDateTimeFromDB = readerMessaggioNum.GetString("sDataOra");

                    LogToFile("dbNewMessageNumRequest : _iNumOfLastMessageFromDB letto");

                    readerMessaggioNum?.Close();

                    return _iNumOfLastMessageFromDB; // tutto OK
                }
                else
                {
                    readerMessaggioNum?.Close();
                    return 0;
                }
            }

            catch (Exception)
            {
                readerMessaggioNum?.Close();

                _WrnMsg.iErrID = WRN_DBE;
                _WrnMsg.sMsg = "Connessione al Server DB_NMSG non possibile";
                WarningManager(_WrnMsg);

#if STANDFACILE
                // bypass del Server NSC solo se CASSA_PRINCIPALE
                if (!DataManager.CheckIf_CassaSec_and_NDB())
                {
                    _iNumOfLastMessageFromDB++;
                    _sDateTimeFromDB = GetDateTimeString();

                    LogToFile("#dbNewMessageNumRequest : bypass del Server DB_NMSG");
                    return _iNumOfLastMessageFromDB; // tutto OK
                }
                else
#endif
                    return 0;
            }
        }

        /// <summary>
        /// Funzione di inserimento record con numero Ordine dalla cassa Sec <br/>
        /// per successivo scarico della Disponibilità di Magazzino <br/>
        /// solo con NDB, usata da Receipt <br/>
        /// </summary>
        public void dbCSecOrderEnqueue(int iEnqueueParam)
        {
            bool bDBConnection_Ok;
            String sQueryTxt;
            MySqlCommand cmd = new MySqlCommand();

            bDBConnection_Ok = dbInit(GetActualDate(), SF_Data.iNumCassa, true);

#if STANDFACILE
            // sicurezza : si prosegue solo se è CASSA_SECONDARIA && bUsa_DB
            if (!DataManager.CheckIf_CassaSec_and_NDB()) // cassa secondaria e DB
                return;
#endif

            try
            {
                if (bDBConnection_Ok)
                {
                    sQueryTxt = String.Format("INSERT INTO {0} (iOrdine_ID, iNumCassa, sDataOra) VALUES ({1}, {2}, \'{3}\');",
                        NOME_DISP_DBTBL, iEnqueueParam, SF_Data.iNumCassa, GetDateTimeString());

                    cmd.CommandText = sQueryTxt;
                    cmd.Connection = _Connection;
                    var qResult = cmd.ExecuteScalar();
                }
            }

            catch (Exception)
            {
                _WrnMsg.iErrID = WRN_DBE;
                _WrnMsg.sMsg = "accesso alla tabella DISP_DBTBL non possibile";
                WarningManager(_WrnMsg);
            }
        }

        /// <summary>
        /// Funzione di inserimento record del numero Ordine web<br/>
        /// usata da MainForm nel caso fallisca il contrassegno diretto
        /// </summary>
        public void dbWebOrderEnqueue(int iEnqueueParam)
        {
            bool bDBConnection_Ok;
            String sTmp, sQueryTxt;
            MySqlCommand cmd = new MySqlCommand();

            bDBConnection_Ok = dbInit(GetActualDate(), SF_Data.iNumCassa, true);

            // sicurezza
            if (iEnqueueParam < 1001)
                return;

            try
            {
                if (bDBConnection_Ok)
                {
                    sQueryTxt = String.Format("INSERT INTO {0} (iOrdine_ID, iNumCassa, sDataOra) VALUES ({1}, {2}, \'{3}\');",
                        NOME_WEBORD_DBTBL, iEnqueueParam, SF_Data.iNumCassa, GetDateTimeString());

                    cmd.CommandText = sQueryTxt;
                    cmd.Connection = _Connection;
                    var qResult = cmd.ExecuteScalar();

                    sTmp = String.Format("dbWebOrderEnqueue : inserimento record Ordine web = {0}", iEnqueueParam);
                    LogToFile(sTmp, true);
                }
            }

            catch (Exception)
            {
                _WrnMsg.iErrID = WRN_DBE;
                _WrnMsg.sMsg = "accesso alla tabella WEBORD_DBTBL non possibile";
                WarningManager(_WrnMsg);
            }
        }

        /// <summary>
        /// scarica uno scontrino dalla tabella delle Prevendite
        /// </summary>
        public bool dbScaricaOrdinePrevendita(int iOrderID, String sPrevOrderTableParam)
        {
            bool bResult, bDBConnection_Ok;
            bool bScaricato, bAnnullato;

            int iCountOrdini;
            int i, iNumCassa;
            int iScaricato, iUpdatedRowsOrdini;
            String sFilter, sTipo, sTmp, sQuery;

            MySqlDataAdapter dbOrdiniAdapter = null;
            MySqlDataAdapter dbOrdiniAdapterSelect = null;

            DataTable ordiniTable = new DataTable();

            MySqlTransaction transaction = null;

            iNumCassa = 0; // partono da 1 !!!

            bAnnullato = true;
            bResult = true;

            iCountOrdini = -1;

            bDBConnection_Ok = dbInit(GetActualDate(), CASSA_PRINCIPALE, false, sPrevOrderTableParam);

            try
            {
                sFilter = String.Format("iOrdine_ID = {0}", iOrderID);

                // riempie con tutti i record di ordini da scaricare
                sQuery = "select * FROM " + _sDBTNameOrdini + " WHERE " + String.Format("iOrdine_ID = {0};", iOrderID);

                dbOrdiniAdapterSelect = new MySqlDataAdapter(sQuery, _Connection);
                iCountOrdini = dbOrdiniAdapterSelect.Fill(ordiniTable);
            }

            catch (Exception)
            {
                _WrnMsg.iErrID = WRN_DBE;
                _WrnMsg.sMsg = "dbScaricaOrdine 1";
                WarningManager(_WrnMsg);

                LogToFile("dbScaricaOrdinePrev : dbException 1");
            }

            if (iCountOrdini >= 0)
            {
                LogToFile("dbScaricaOrdinePrev : iCountOrdini > 0");

                try // *********** 1 *********
                {
                    if (iCountOrdini == 0) // record non presente ma connessione OK
                    {
                        _WrnMsg.sMsg = iOrderID.ToString();
                        _WrnMsg.iErrID = WRN_RNF;
                        WarningManager(_WrnMsg);
                        sTmp = String.Format("dbScaricaOrdine : record {0} non esiste!", iOrderID);
                        LogToFile(sTmp);
                    }
                    else
                    {
                        /*********************************************************************
                         *   controlla se il primo record dell'ordine è stato annullato      *
                         *********************************************************************/
                        bAnnullato = Convert.ToBoolean(ordiniTable.Rows[0]["iAnnullato"]);

                        if (bAnnullato) // record  presente ma annullato
                        {
                            bResult = false;

                            _WrnMsg.sMsg = iOrderID.ToString();
                            _WrnMsg.iErrID = WRN_RAN;
                            WarningManager(_WrnMsg);
                            sTmp = String.Format("dbScaricaOrdine : record {0} esiste ma è stato annullato!", iOrderID);
                            LogToFile(sTmp);
                        }

                        /****************************************************************
                         *   controlla da che cassa è stato emesso lo scontrino         *
                         *   attenzione che iNumCassa parte da 1 "Cassa n.1 Principale, *
                         *   2 = Cassa n.2 Secondaria, etc                              *
                         ****************************************************************/
                        iNumCassa = Convert.ToInt32(ordiniTable.Rows[0]["iNumCassa"]);

                        sTmp = String.Format("dbScaricaOrdine : pos 2");
                        LogToFile(sTmp);
                    }
                }

                catch (Exception)
                {
                    bResult = false;

                    _WrnMsg.iErrID = WRN_DBE;
                    _WrnMsg.sMsg = "dbScaricaOrdine 2";
                    WarningManager(_WrnMsg);

                    LogToFile("dbScaricaOrdinePrev : dbException 2");
                }
            }
            else
                LogToFile("dbScaricaOrdinePrev : NOT Active");

            if ((iCountOrdini > 0) && !bAnnullato)
            {

                try // *********** 2 **********
                {
                    transaction = _Connection.BeginTransaction();

                    // *********** predispone per aggiornamento colonna scarico ordini ************
                    dbOrdiniAdapter = new MySqlDataAdapter();

                    sQuery = String.Format(@"UPDATE {0} SET iScaricato = @iScaricato, sScaricato = @sScaricato WHERE
                         sTipo_Articolo = @oldId AND iOrdine_ID = {1};", _sDBTNameOrdini, iOrderID);

                    dbOrdiniAdapter.UpdateCommand = new MySqlCommand(sQuery, _Connection);

                    dbOrdiniAdapter.UpdateCommand.Parameters.Add("@iScaricato", MySqlType.Int, 11, "iScaricato");
                    dbOrdiniAdapter.UpdateCommand.Parameters.Add("@sScaricato", MySqlType.VarChar, 50, "sScaricato");

                    dbOrdiniAdapter.UpdateCommand.Parameters.Add("@oldId", MySqlType.VarChar, 50, "sTipo_Articolo").SourceVersion = DataRowVersion.Original;
                    dbOrdiniAdapter.UpdateCommand.UpdatedRowSource = UpdateRowSource.None;

                    for (i = 0; i < iCountOrdini; i++)
                    {
                        // init 
                        iScaricato = 0;

                        sTipo = Convert.ToString(ordiniTable.Rows[i]["sTipo_Articolo"]);

                        if (ordiniTable.Rows[i]["iScaricato"] != DBNull.Value)
                            iScaricato = Convert.ToInt32(ordiniTable.Rows[i]["iScaricato"]);

                        bScaricato = (iScaricato == 1);

                        // aggiorna solo se non è già stato scaricato !!!
                        if (bScaricato)
                        {
                            bResult = false;

                            // gia scaricato !!!
                            if (sTipo == ORDER_CONST._START_OF_ORDER)
                            {
                                Console.Beep();

                                sTmp = String.Format("dbScaricaOrdinePrev : bRecord = {0} già scaricato!", iOrderID);
                                LogToFile(sTmp);
                            }
                        }
                        else
                        {
                            /******************************
                             *		aggiornamento DB
                             ******************************/

                            ordiniTable.Rows[i]["iScaricato"] = 1;

                            if (sTipo == ORDER_CONST._START_OF_ORDER)
                            {
                                sTmp = DateTime.Now.ToString("HH.mm.ss");

                                ordiniTable.Rows[i]["sScaricato"] = sTmp;

                                sTmp = String.Format("dbScaricaOrdine : trovato iOrderID = {0}", iOrderID);
                                LogToFile(sTmp);
                            }
                            else
                                ordiniTable.Rows[i]["sScaricato"] = "-";
                        }

                    } // end for

                    LogToFile("dbScaricaOrdinePrev : pos 3");

                    // aggiorna il database su disco
                    LogToFile("dbScaricaOrdinePrev : ApplyUpdates");

                    //dbOrdiniAdapter.UpdateBatchSize = 100;
                    iUpdatedRowsOrdini = dbOrdiniAdapter.Update(ordiniTable);

                    Console.WriteLine("dbScaricaOrdinePrev : iUpdatedRows ordini = {0}", iUpdatedRowsOrdini);

                    /*** aggiorna il database su disco ***/
                    transaction.Commit();

                    dbOrdiniAdapter.Dispose();
                    dbOrdiniAdapterSelect.Dispose();

                    Console.Beep();
                }

                catch (Exception)
                {
                    bResult = false;

                    _WrnMsg.iErrID = WRN_DBE;
                    _WrnMsg.sMsg = "dbScaricaOrdine 3";
                    WarningManager(_WrnMsg);
                    LogToFile("dbScaricaOrdinePrev : dbException 2");
                }
            }
            else
            {
                bResult = false;
                LogToFile("dbScaricaOrdinePrev : readerOrdini == null");
            }

            return bResult;
        }

        /// <summary>funzione di modifica dello stato con i vari flag, iOrderIDParam >= 0 per i Tickets, &lt; 0 per i messaggi</summary>
        public bool dbEditStatus(int iOrderIDParam, int iStatusParam)
        {
            bool bDBConnection_Ok;
            String sQueryTxt;
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataReader readerStatus = null;

            bDBConnection_Ok = dbInit(GetActualDate(), SF_Data.iNumCassa, true);

            // sicurezza : si prosegue solo se c'è la connessione a DB
            if (!bDBConnection_Ok)
                return false;

            try
            {
                if (iOrderIDParam >= 0)
                {
                    sQueryTxt = String.Format("UPDATE {0} SET iStatus = {1} WHERE (iOrdine_ID = {2} AND sTipo_Articolo = '{3}');",
                                _sDBTNameOrdini, iStatusParam, iOrderIDParam, ORDER_CONST._START_OF_ORDER);
                }
                else
                    sQueryTxt = String.Format("UPDATE {0} SET iStatus = {1} WHERE iOrdine_ID = {2};", _sDBTNameOrdini, iStatusParam, iOrderIDParam);

                cmd.Connection = _Connection;
                cmd.CommandText = sQueryTxt;

                readerStatus = cmd.ExecuteReader();

                readerStatus?.Close();

                LogToFile("dbEditStatus: stato aggiornato");
            }

            catch (Exception)
            {
                readerStatus.Close();

                _WrnMsg.iErrID = WRN_DBE;
                _WrnMsg.sMsg = "aggiornamento Stato non possibile";
                WarningManager(_WrnMsg);

                return false;
            }

            return true; // tutto OK
        }

        /// <summary>Test di connessione al db server</summary>
        public bool dbCheck(String sDB_ServerNamePrm, String sDB_pwdPrm, bool bSilentParam)
        {

            if (String.IsNullOrEmpty(sDB_ServerNamePrm))
                sDB_ServerNamePrm = Dns.GetHostName();

            _Connection?.Close();

            _dbCSB.Host = sDB_ServerNamePrm;
            _dbCSB.Password = sDB_pwdPrm;
            _dbCSB.Database = _database;
            _dbCSB.UserId = _uid;
            _dbCSB.FoundRows = true;
            _dbCSB.Pooling = false;
            _dbCSB.Unicode = true;
            _dbCSB.ConnectionTimeout = TIMEOUT_DB_OPEN;

            try
            {
                _WrnMsg.sMsg = "MySql :\r\n\r\n'" + sDB_ServerNamePrm + " '";
                LogToFile("dbCheck : dbCheck Open()");

                // prova di connessione al DB
                _Connection = new MySqlConnection(_dbCSB.ConnectionString);
                _Connection.Open();

                // imposta la variabile membro solo se si connette
                if (_Connection.State == ConnectionState.Open)
                {
#if STAND_MONITOR
                    if (_iAvvisoDbCheckCounter < 0)
                        bSilentParam = true;
#endif
                    if (!bSilentParam) // Messaggio di connessione
                    {
                        _WrnMsg.iErrID = WRN_TDS;
                        WarningManager(_WrnMsg);
                    }
                }
            }
            catch (Exception)
            {
#if STAND_CUCINA
                String[] sOrdiniQueueObj = new String[2];

                sOrdiniQueueObj[0] = Define.UPDATE_DB_LABEL_EVENT;
                sOrdiniQueueObj[1] = "No connessione al DB !";
                FrmMain.QueueUpdate(sOrdiniQueueObj);
#endif

                if (!bSilentParam) // Messaggio di connessione
                {
                    _WrnMsg.iErrID = WRN_TDF;
                    WarningManager(_WrnMsg);
                }
                // connessione non possibile al database
                LogToFile("dbMySQL_Check : dbException dbCheck");
            }

            return (_Connection.State == ConnectionState.Open);
        }

    }
}

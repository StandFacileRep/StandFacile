/*************************************************************************************************
	NomeFile : StandCommonSrc/dBaseIntf_ql.cs
    Data	 : 06.12.2025
	Autore   : Mauro Artuso

    nelle assegnazioni :
    DB_Data compare sempre a sx,
    SF_Data compare sempre a dx

    ANSI SQL uses double quotes only for SQL identifiers, 
    single quotes are used for literals (stringhe).
    SQLite non utilizza UNSIGNED 

    Attenzione : dbInit(dateParam) deve essere invocata all'inizio di ogni funzione
 *************************************************************************************************/

using System;
using System.IO;
using System.Collections.Generic;
using System.Data;

using Devart.Data.SQLite;

using static StandCommonFiles.ComDef;
using static StandCommonFiles.CommonCl;
using static StandCommonFiles.LogServer;

using StandFacile;
using static StandFacile.glb;
using static StandFacile.dBaseIntf;

namespace StandFacile_DB
{
#pragma warning disable IDE0079
#pragma warning disable IDE1006

    /// <summary>classe per la gestione di SQLite</summary>
    public partial class dBaseIntf_ql
    {

        /// <summary>riferimento a dBaseIntf</summary>
        public static dBaseIntf_ql _rdBaseIntf_ql;

        SQLiteConnection _Connection, _ConnectionWeb;

        /// <summary>costruttore</summary>
        public dBaseIntf_ql()
        {
            String sDir;

            _rdBaseIntf_ql = this;

            sDir = DataManager.GetExeDir() + "\\";

            // devono essere presente anche se non si usa il db
            if (!(File.Exists(sDir + DB_CONNECTOR_DLL_DEV) && File.Exists(sDir + DB_CONNECTOR_DLL_QL)))
                ErrorManager(ERR_DLL);

            if (Environment.Is64BitOperatingSystem)
            {
                LogToFile("dBaseIntf_ql : 64bit OS");

                if (!File.Exists(sDir + DB_CONNECTOR_DLL_QL64))
                    ErrorManager(ERR_DLL);
            }
            else
            {
                LogToFile("dBaseIntf_ql : not 64 bit OS");

                if (!File.Exists(sDir + DB_CONNECTOR_DLL_QL32))
                    ErrorManager(ERR_DLL);
            }
        }

        /// <summary>distruttore DB, libera la connessione</summary>
        ~dBaseIntf_ql()
        {
            //            if (_Connection != null)
            //                _Connection.Close();
        }

        /// <summary>
        /// se sNomeTabellaParam è vuota imposta _sDBTNameDati, _sDBTNameOrdini <br/>
        /// in base a dateParam, altrimenti in base a sNomeTabellaParam <br/> <br/>
        /// 
        /// ritorna false = errore, true = connessione effettuata <br/>
        /// </summary>
        public bool dbInit(DateTime dateParam, bool bSilentParam = false, String sNomeTabellaParam = "")
        {
            String sTmp, sData, sPostFix, sDebugDati, sDebugOrdini;

            _Connection?.Close();

            // prepara connessione al DB
            if (!bUSA_NDB())
            {
                try
                {
                    _sDBTNameDati = GetNomeDatiDBTable(CASSA_PRINCIPALE, dateParam);
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
                            sData = sNomeTabellaParam.Remove(0, _dbPreDataTablePrefix.Length + 3);
                            sTmp = sData.Insert(0, _dbPreOrdersTablePrefix);
                            _sDBTNameOrdini = sTmp;
                        }
                        else if (sNomeTabellaParam.Contains(_dbPreOrdersTablePrefix))
                        {
                            sData = sNomeTabellaParam.Remove(0, _dbPreOrdersTablePrefix.Length);
                            sTmp = sData.Insert(0, String.Format("_c{0}", CASSA_PRINCIPALE));
                            sTmp = sTmp.Insert(0, _dbPreDataTablePrefix);
                            _sDBTNameDati = sTmp;
                        }
                        // gestione estensioni da chiusura cassa ricavate dalla tabella dati e/o ordini
                        else if ((sNomeTabellaParam.Length >= (_sDBTNameOrdini.Length)) && sNomeTabellaParam.Contains(_dbOrdersTablePrefix))
                        {
                            if (sNomeTabellaParam.Length > (_sDBTNameOrdini.Length))
                            {
                                // postfisso
                                sTmp = sNomeTabellaParam.Remove(0, _dbOrdersTablePrefix.Length);
                                sData = sTmp.Substring(0, 7);
                                sTmp = sData.Insert(0, String.Format("_c{0}", CASSA_PRINCIPALE));
                                sPostFix = sNomeTabellaParam.Substring(_sDBTNameOrdini.Length); // postfisso
                                _sDBTNameDati = _dbDataTablePrefix + sTmp + sPostFix;
                            }
                            else
                            {
                                // da EsploraDB_Dlg tabelle in altra data
                                sData = sNomeTabellaParam.Remove(0, _dbOrdersTablePrefix.Length);
                                sTmp = sData.Insert(0, String.Format("_c{0}", CASSA_PRINCIPALE));
                                _sDBTNameDati = _dbDataTablePrefix + sTmp;
                            }
                        }
                        else if ((sNomeTabellaParam.Length >= (_sDBTNameDati.Length)) && sNomeTabellaParam.Contains(_dbDataTablePrefix))
                        {
                            if (sNomeTabellaParam.Length > (_sDBTNameDati.Length))
                            {
                                // postfisso
                                sTmp = sNomeTabellaParam.Remove(0, _dbDataTablePrefix.Length + 3);
                                sData = sTmp.Substring(0, 7);
                                sPostFix = sNomeTabellaParam.Substring(_sDBTNameDati.Length); // postfisso
                                _sDBTNameOrdini = _dbOrdersTablePrefix + sData + sPostFix;
                            }
                            else
                            {
                                // da EsploraDB_Dlg tabelle in altra data
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

                    SQLiteConnectionStringBuilder dbCSB = new SQLiteConnectionStringBuilder()
                    {
                        DataSource = GetNomeFileDatiDB_SQLite(GetActualDate()),
                        FailIfMissing = false,
                        Pooling = false,
                        ConnectionTimeout = TIMEOUT_DB_OPEN
                    };

                    _Connection = new SQLiteConnection(dbCSB.ConnectionString);
                    _Connection.Open();

                    return true;
                }

                catch (Exception)
                {
                    // connessione non possibile al database
                    _WrnMsg.iErrID = WRN_DBE;
                    _WrnMsg.sMsg = "dbInit SQLite";

                    if (!bSilentParam) // per non accumulare troppe indicazioni
                        WarningManager(_WrnMsg);

                    LogToFile("dbInit : Exception SQLite");
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
            if (!bUSA_NDB())
            {
                try
                {
                    SQLiteConnectionStringBuilder _dbCSB_Web = new SQLiteConnectionStringBuilder()
                    {
                        DataSource = GetNomeFileDatiDB_SQLite(GetActualDate()),
                        FailIfMissing = false,
                        Pooling = false,
                        ConnectionTimeout = TIMEOUT_DB_OPEN
                    };

                    _ConnectionWeb = new SQLiteConnection(_dbCSB_Web.ConnectionString);
                    _ConnectionWeb.Open();

                    return true;
                }

                catch (Exception)
                {
                    // connessione non possibile al database
                    _WrnMsg.iErrID = WRN_DBE;
                    _WrnMsg.sMsg = "dbInit SQLite Web";

                    WarningManager(_WrnMsg);

                    LogToFile("dbInit : Exception SQLite Web");
                }
            }

            return false; // non dovrebbe mai passare di qui !
        } // end dbInit

        /// <summary>
        /// funzione di controllo e reset del record di stato contenente la data <br/>
        /// (solo con bResetParam == true) <br/> <br/>
        /// 
        /// ritorna true se la verifica/creazione della data è corretta, false altrimenti <br/>
        /// utilizzata da : DataManager.Init <br/>
        /// </summary>
        public bool dbCheckStatus(bool bResetParam = false)
        {
            bool bDBConnection_Ok;
            int iGiorno, iMese, iAnno;
            String sQueryTxt, sTmp, sActualDateStr;
            String sReadVersion = RELEASE_SW;
            DateTime statusDate = GetActualDate();

            SQLiteCommand cmd = new SQLiteCommand();
            SQLiteDataReader readerStato = null;
            TWebServerParams sWebServerParams = dbGetWebServerParams();

            bDBConnection_Ok = dbInit(GetActualDate(), true);

            // sicurezza : si prosegue solo se c'è la connessione al DB
            if (!bDBConnection_Ok)
                return false;

            try
            {
                cmd.Connection = _Connection;
                cmd.CommandText = "SELECT * FROM " + NOME_STATUS_DBTBL + " WHERE (key_ID = 'Data')";

                readerStato = cmd.ExecuteReader();

                if (readerStato != null)
                {
                    readerStato.Read();

                    iAnno = readerStato.GetInt32("iYear");
                    iMese = readerStato.GetInt32("iMonth");
                    iGiorno = readerStato.GetInt32("iDay");

                    DateTime tmpDate = new DateTime(iAnno, iMese, iGiorno);
                    statusDate = tmpDate;

                    _sDateFromDB = statusDate.ToString("dd/MM/yy");

                    LogToFile("dbCheckStatus : stato letto");

                    readerStato.Close();
                }
                else
                    LogToFile("dbCheckStatus : stato non letto");

                // lettura Versione
                cmd.CommandText = "SELECT * FROM " + NOME_STATUS_DBTBL + " WHERE (key_ID = 'Versione')";
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

            catch (Exception)
            {
                LogToFile("dbCheckStatus : stato dbException");
                _sDateFromDB = "Stato non presente!";
            }

            sActualDateStr = GetActualDate().ToString("dd/MM/yy");

            // *** confronto data ***
            if (_sDateFromDB != sActualDateStr)
            {
                if (bResetParam)
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
                        sQueryTxt = "DROP TABLE IF EXISTS " + NOME_STATUS_DBTBL + ";";

                        cmd.Connection = _Connection;
                        cmd.CommandText = sQueryTxt;
                        var qResult = cmd.ExecuteScalar();

                        LogToFile("dbCheckStatus : prima CREATE TABLE stato");

                        sQueryTxt = String.Format(@"CREATE TABLE IF NOT EXISTS {0} (key_ID VARCHAR(50) NOT NULL, sText VARCHAR(50),
                                iYear INT NOT NULL, iMonth INT NOT NULL, iDay INT NOT NULL, PRIMARY KEY(key_ID));",
                            NOME_STATUS_DBTBL);

                        cmd.CommandText = sQueryTxt;
                        qResult = cmd.ExecuteScalar();

                        sTmp = GetActualDate().ToString("yyyy");
                        iAnno = Convert.ToInt32(sTmp);

                        sTmp = GetActualDate().ToString("MM");
                        iMese = Convert.ToInt32(sTmp);

                        sTmp = GetActualDate().ToString("dd");
                        iGiorno = Convert.ToInt32(sTmp);

                        sQueryTxt = String.Format("INSERT INTO {0} (key_ID, sText, iYear, iMonth, iDay) VALUES (\'{1}\', \'{2}\', {3}, {4}, {5});",
                            NOME_STATUS_DBTBL, "Data", "-", iAnno, iMese, iGiorno);
                        cmd.CommandText = sQueryTxt;
                        qResult = cmd.ExecuteScalar();

                        sQueryTxt = String.Format("INSERT INTO {0} (key_ID, sText, iYear, iMonth, iDay) VALUES (\'{1}\', \'{2}\', {3}, {4}, {5});",
                            NOME_STATUS_DBTBL, "Versione", RELEASE_SW, 0, 0, 0);
                        cmd.CommandText = sQueryTxt;
                        qResult = cmd.ExecuteScalar();

                        sQueryTxt = String.Format("INSERT INTO {0} (key_ID, sText, iYear, iMonth, iDay) VALUES (\'{1}\', \'{2}\', {3}, {4}, {5});",
                            NOME_STATUS_DBTBL, WEB_SERVER_NAME_KEY, sWebServerParams.sWebTablePrefix, 0, 0, 0);
                        cmd.CommandText = sQueryTxt;
                        qResult = cmd.ExecuteScalar();

                        sQueryTxt = String.Format("INSERT INTO {0} (key_ID, sText, iYear, iMonth, iDay) VALUES (\'{1}\', \'{2}\', {3}, {4}, {5});",
                            NOME_STATUS_DBTBL, WEB_DBASE_NAME_KEY, sWebServerParams.sWeb_DBase, 0, 0, 0);
                        cmd.CommandText = sQueryTxt;
                        qResult = cmd.ExecuteScalar();

                        sQueryTxt = String.Format("INSERT INTO {0} (key_ID, sText, iYear, iMonth, iDay) VALUES (\'{1}\', \'{2}\', {3}, {4}, {5});",
                            NOME_STATUS_DBTBL, WEB_DBASE_PWD_KEY, sWebServerParams.sWebEncryptedPwd, 0, 0, 0);
                        cmd.CommandText = sQueryTxt;
                        qResult = cmd.ExecuteScalar();

                        LogToFile("dbCheckStatus : dopo CREATE TABLE stato");

                        // ci pensa il processo rdb_aggiornaOrdiniWebServiti() a svuotare la tabella ordini_web
                        //sQueryTxt = String.Format("DROP TABLE IF EXISTS {0};", NOME_WEBORD_DBTBL);
                        //cmd.CommandText = sQueryTxt;
                        //qResult = cmd.ExecuteScalar();

                        sQueryTxt = String.Format(@"CREATE TABLE IF NOT EXISTS {0} (iOrdine_ID INT NOT NULL, iNumCassa INT UNSIGNED NOT NULL, 
                                            sDataOra VARCHAR(50));", NOME_WEBORD_DBTBL);
                        cmd.CommandText = sQueryTxt;
                        qResult = cmd.ExecuteScalar();

                        LogToFile("dbCheckStatus : dopo CREATE TABLE _ordini_web");
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
                else
                    return false;
            }
            else if ((sReadVersion != RELEASE_SW) && bResetParam)
            {
                // CASSA_PRINCIPALE, versioni differenti, stessa data

                // aggiorna versione
                sReadVersion = "";
                cmd.CommandText = "UPDATE " + NOME_STATUS_DBTBL + " SET sText = '" + RELEASE_SW + "' WHERE (key_ID = 'Versione')";
                readerStato = cmd.ExecuteReader();
            }

            readerStato?.Close();

            cmd.Dispose();
            return true; // data DB corretta
        }

        /// <summary>funzione di lettura parametri per accesso database remoto</summary>
        public TWebServerParams dbGetWebServerParams()
        {
            bool bDBConnection_Ok;
            String sKey, sText;
            SQLiteCommand cmd = new SQLiteCommand();
            SQLiteDataReader readerStatus = null;
            TWebServerParams sWebServerParams = new TWebServerParams(0);


            bDBConnection_Ok = dbInit(GetActualDate(), true);

            // sicurezza : si prosegue solo se c'è la connessione a DB
            if (!bDBConnection_Ok)
                return new TWebServerParams(0);

            try
            {
                cmd.Connection = _Connection;
                cmd.CommandText = "SELECT * FROM " + NOME_STATUS_DBTBL + ";";
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
                if (readerStatus == null) // il dbase SQlite non esiste
                {
                    // solo per SQlite che viene cancellato più spesso nei miei test
                    if (String.IsNullOrEmpty(sWebServerParams.sWebTablePrefix))
                        sWebServerParams.sWebTablePrefix = ReadRegistry(WEB_SERVER_NAME_KEY, "");

                    if (String.IsNullOrEmpty(sWebServerParams.sWeb_DBase))
                        sWebServerParams.sWeb_DBase = ReadRegistry(WEB_DBASE_NAME_KEY, "");

                    if (String.IsNullOrEmpty(sWebServerParams.sWebEncryptedPwd))
                        sWebServerParams.sWebEncryptedPwd = ReadRegistry(WEB_DBASE_PWD_KEY, "");
                }
                else
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
        public bool dbSetWebServerParams(TWebServerParams sWebServerParams)
        {
            bool bDBConnection_Ok;
            SQLiteCommand cmd = new SQLiteCommand();
            SQLiteDataReader readerStatus = null;

            bDBConnection_Ok = dbInit(GetActualDate(), true);

            // sicurezza : si prosegue solo se c'è la connessione a DB
            if (!bDBConnection_Ok)
                return false;

            try
            {
                cmd.Connection = _Connection;

                cmd.CommandText = "UPDATE " + NOME_STATUS_DBTBL + " SET sText = '" + sWebServerParams.sWebTablePrefix + "' WHERE (key_ID = '" + WEB_SERVER_NAME_KEY + "')";
                readerStatus = cmd.ExecuteReader();

                cmd.CommandText = "UPDATE " + NOME_STATUS_DBTBL + " SET sText = '" + sWebServerParams.sWeb_DBase + "' WHERE (key_ID = '" + WEB_DBASE_NAME_KEY + "')";
                readerStatus = cmd.ExecuteReader();

                cmd.CommandText = "UPDATE " + NOME_STATUS_DBTBL + " SET sText = '" + sWebServerParams.sWebEncryptedPwd + "' WHERE (key_ID = '" + WEB_DBASE_PWD_KEY + "')";
                readerStatus = cmd.ExecuteReader();

                readerStatus?.Close();

                LogToFile("dbSetWebServerParams: parametri scritti");
            }

            catch (Exception)
            {
                readerStatus?.Close();

                _WrnMsg.iErrID = WRN_DBE;
                _WrnMsg.sMsg = "Scrittura Stato non possibile";
                WarningManager(_WrnMsg);

                //return false;
            }

            return true; // tutto OK
        }

        /// <summary>
        /// funziona che ottiene gli ordini web serviti
        /// </summary>
        public int dbGetOrdiniWebServiti()
        {
            bool bDBConnection_Ok;
            int iOrderNum = 0;

            SQLiteCommand cmd = new SQLiteCommand();
            SQLiteDataReader readerCSec = null;

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

            SQLiteCommand cmd = new SQLiteCommand();

            bDBConnection_Ok = dbInitWeb();

            // sicurezza : si prosegue solo se c'è la connessione a MySQL
            // o se la Cassa secondaria non richiede il prelievo di un ordine dalla lista
            if (!bDBConnection_Ok)
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
        /// iNumAnnulloParam è il numero dell'ordine,  <br/>
        /// </summary>
        public bool dbAnnulloOrdine(DateTime dateParam, int iNumAnnulloParam, String sNomeTabellaParam = "")
        {
            bool bAnnullato, bDBConnection_Ok;

            int iCount = 0, iUpdatedRows;
            int i, iNumCassa;
            String sTmp, sTipo, sQueryTxt;

            SQLiteCommand cmd = new SQLiteCommand();
            SQLiteDataReader readerOrdini = null;
            SQLiteDataAdapter dbDataAdapterSelect = null;

            DataTable dataTable = new DataTable();

            // dbAzzeraDatiGen(); non serve e poi cancella gli Headers
            dbAzzeraDatiOrdine(ref DB_Data);

            bAnnullato = false;
            iNumCassa = SF_Data.iNumCassa;
            bDBConnection_Ok = dbInit(dateParam, false, sNomeTabellaParam);

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
                    dbDataAdapterSelect = new SQLiteDataAdapter(sQueryTxt, _Connection);
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
                    SQLiteTransaction transaction = _Connection.BeginTransaction();

                    SQLiteDataAdapter dbDataAdapter = new SQLiteDataAdapter();

                    sQueryTxt = String.Format(@"UPDATE {0} SET ""iAnnullato"" = @iAnnullato, ""sAnnullato"" = @sAnnullato WHERE
                         ""sTipo_Articolo"" = @oldId AND ""iOrdine_ID"" = {1};", _sDBTNameOrdini, iNumAnnulloParam);

                    dbDataAdapter.UpdateCommand = new SQLiteCommand(sQueryTxt, _Connection);

                    dbDataAdapter.UpdateCommand.Parameters.Add("@iAnnullato", SQLiteType.Int32, 11, "iAnnullato");
                    dbDataAdapter.UpdateCommand.Parameters.Add("@sAnnullato", SQLiteType.Text, 50, "sAnnullato");

                    dbDataAdapter.UpdateCommand.Parameters.Add("@oldId", SQLiteType.Text, 50, "sTipo_Articolo").SourceVersion = DataRowVersion.Original;
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
                    //dbDataAdapter.UpdateBatchSize = 100;
                    iUpdatedRows = dbDataAdapter.Update(dataTable);

                    Console.WriteLine("dbAnnulloOrdine : iUpdatedRows = {0}", iUpdatedRows);

                    transaction.Commit();

                    dbDataAdapter.Dispose();
                    dbDataAdapterSelect.Dispose();

                    bAnnullato = true;

#if STANDFACILE
                    //VisMessaggiDlg rVisMessaggiDlg = new VisMessaggiDlg();

                    //rVisMessaggiDlg.ScriviMessaggioAnnullo(iNumAnnulloParam);
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
        /// aggiunge il suffisso alle tabelle
        /// usato da chiudiIncasso()
        /// </summary>
        public bool dbRenameTables(String sNewPostFix)
        {
            bool bDBConnection_Ok, bSuccess = true;
            String sQueryTxt, sNewTName;
            SQLiteCommand cmd = new SQLiteCommand();

            DateTime dateParam = GetActualDate();

            // imposta anche la directory con il full path
            bDBConnection_Ok = dbInit(dateParam);

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
                bSuccess = false;

                _WrnMsg.iErrID = WRN_DBE;
                _WrnMsg.sMsg = String.Format("RENAME DATI {0}", bUSA_NDB());
                WarningManager(_WrnMsg);
                LogToFile("dbRenameTables : dbException RENAME DATI");
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
            SQLiteCommand cmd = new SQLiteCommand();

            bDBConnection_Ok = dbInit(GetActualDate(), false, sOldTabellaParam);

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
                if (!bUSA_NDB())
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

            DateTime dateParam = GetActualDate();
            SQLiteCommand cmd = new SQLiteCommand();

            bDBConnection_Ok = dbInit(dateParam, true);

            // sicurezza : si prosegue solo se c'è la connessione a DB
            if (!bDBConnection_Ok)
                return false;

            try // dati casse
            {
                cmd.Connection = _Connection;

                /*************************************
                 *		DROP ClientDS_Dati
                 *************************************/

                sDBTLocNameDati = GetNomeDatiDBTable(CASSA_PRINCIPALE, dateParam);

                // Query di drop tabella
                sQueryTxt = String.Format("DROP TABLE IF EXISTS {0};", sDBTLocNameDati);
                cmd.CommandText = sQueryTxt;
                cmd.ExecuteScalar();

                LogToFile("dbDropTables : DROP TABLE DATI 0");

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

                LogToFile("dbDropTables : DROP TABLE ORDINI");

            }
            catch (Exception)
            {
                if (!bUSA_NDB())
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
            SQLiteCommand cmd = new SQLiteCommand();

            bDBConnection_Ok = dbInit(GetActualDate(), false, sNomeTabellaParam);

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
                if (!bUSA_NDB())
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
            SQLiteDataReader readerTables = null;
            SQLiteCommand cmd = new SQLiteCommand();
            String sTmp;

            bDBConnection_Ok = dbInit(GetActualDate());

            // sicurezza : si prosegue solo se c'è la connessione a DB
            if (!bDBConnection_Ok)
                return 0;

            cmd.Connection = _Connection;
            // cmd.CommandText = "show tables from " + _sDB_DatabaseName; //non è ordinato
            cmd.CommandText = "SELECT name FROM sqlite_master ORDER BY name ASC;";

            try
            {
                readerTables = cmd.ExecuteReader();

                while (readerTables.Read())
                {
                    sTmp = readerTables.GetString(0);

                    if (!readerTables.GetString(0).Contains("sqlite"))
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
        /// Funzione di inserimento record del numero Ordine web<br/>
        /// usata da MainForm nel caso fallisca il contrassegno diretto
        /// </summary>
        public void dbWebOrderEnqueue(int iEnqueueParam)
        {
            bool bDBConnection_Ok;
            String sTmp, sQueryTxt;
            SQLiteCommand cmd = new SQLiteCommand();

            bDBConnection_Ok = dbInit(GetActualDate(), true);

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

            SQLiteDataAdapter dbOrdiniAdapter = null;
            SQLiteDataAdapter dbOrdiniAdapterSelect = null;

            DataTable ordiniTable = new DataTable();

            SQLiteTransaction transaction = null;

            iNumCassa = 0; // partono da 1 !!!

            bAnnullato = true;
            bResult = true;

            iCountOrdini = -1;

            bDBConnection_Ok = dbInit(GetActualDate(), false, sPrevOrderTableParam);

            try
            {
                sFilter = String.Format("iOrdine_ID = {0}", iOrderID);

                // riempie con tutti i record di ordini da scaricare
                sQuery = "select * FROM " + _sDBTNameOrdini + " WHERE " + String.Format("iOrdine_ID = {0};", iOrderID);

                dbOrdiniAdapterSelect = new SQLiteDataAdapter(sQuery, _Connection);
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
                    dbOrdiniAdapter = new SQLiteDataAdapter();

                    sQuery = String.Format(@"UPDATE {0} SET ""iScaricato"" = @iScaricato, ""sScaricato"" = @sScaricato WHERE
                         ""sTipo_Articolo"" = @oldId AND ""iOrdine_ID"" = {1};", _sDBTNameOrdini, iOrderID);

                    dbOrdiniAdapter.UpdateCommand = new SQLiteCommand(sQuery, _Connection);

                    dbOrdiniAdapter.UpdateCommand.Parameters.Add("@iScaricato", SQLiteType.Int32, 11, "iScaricato");
                    dbOrdiniAdapter.UpdateCommand.Parameters.Add("@sScaricato", SQLiteType.Text, 50, "sScaricato");

                    dbOrdiniAdapter.UpdateCommand.Parameters.Add("@oldId", SQLiteType.Text, 50, "sTipo_Articolo").SourceVersion = DataRowVersion.Original;
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

                                sTmp = String.Format("dbScaricaOrdinePrev : trovato iOrderID = {0}", iOrderID);
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

        /// <summary>funzione di modifica del tipo di pagamento</summary>
        public bool dbEditStatus(int iOrderIDParam, int iStatusParam)
        {
            bool bDBConnection_Ok;
            String sQueryTxt;
            SQLiteCommand cmd = new SQLiteCommand();
            SQLiteDataReader readerStatus = null;

            bDBConnection_Ok = dbInit(GetActualDate(), true);

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
                    sQueryTxt = String.Format("UPDATE {0} SET iStatus = {1} WHERE (iOrdine_ID = {2});", _sDBTNameOrdini, iStatusParam, iOrderIDParam);

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

    }
}

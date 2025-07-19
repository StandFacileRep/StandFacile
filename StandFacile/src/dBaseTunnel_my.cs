/*****************************************************************************************
	NomeFile : StandFacile/dBaseTunnel_my.cs
    Data	 : 23.05.2025
	Autore   : Mauro Artuso

    Classe per la lettura degli ordini in remoto, utilizza HTTP tunneling
    la eventuale tabella viene restituita dal DB server in formato JSON
 *****************************************************************************************/

// se è commentato accede al database remoto
// se non è commentato accede a localhost

using System;
using System.IO;
using System.Net;
using System.Text;
using System.Timers;
using System.Collections;
using System.Windows.Forms;
using System.Web.Script.Serialization;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Net.NetworkInformation;

using static StandCommonFiles.ComDef;
using static StandCommonFiles.ComSafe;
using static StandCommonFiles.CommonCl;
using static StandCommonFiles.LogServer;

using static StandFacile.Define;
using static StandFacile.glb;
using static StandFacile.dBaseIntf;

namespace StandFacile
{
#pragma warning disable IDE0044
#pragma warning disable IDE1006

    /// <summary>
    /// classe per la gestione del database mysql remoto mediante tunnel HTTP
    /// </summary>
    public class dBaseTunnel_my
    {

        private static readonly string _NO_DB_ERRORS = "\"errornumber\":\"0\",\"errordescr\":\"";

        private static readonly string _MYSQL_TUNNEL = "mysqlTunnel_v5c.php";

        static bool _bStartReadRemTable, _bWebServiceRequested, _bPrimaVolta_o_ForzaCaricamentoListino, _bPrimaVoltaLog;

        /// <summary>prefisso per la gestione delle tabelle remote</summary>
        static String _sRemoteTablePrefix;

        /// <summary>Struct fondamentale per i dati dell' RDB</summary>
        public static TData RDB_Data = new TData(0);

        /// <summary>nome tabella per la gestione del numero progressivo degli ordini</summary>
        static string NOME_NSC_RDBTBL;

        /// <summary>nome della tabella del Listino</summary>
        static String NOME_PREZZI_RDBTBL;

        /// <summary>nome della tabella di Stato</summary>
        static String NOME_STATUS_RDBTBL;

        /// <summary> tabella degli ordini remota</summary>
        static string NOME_ORDERS_RDBTBL;

        /// <summary> tabella Log remota</summary>
        static string NOME_LOG_RDBTBL;


        /// <summary>riferimento a dBaseTunnel_my</summary>
        public static dBaseTunnel_my _rdBaseTunnel_my;

        static System.Timers.Timer _timer;

        static int _iTimerCounter = 5 * 30;

        // Attenzione l'utente del database è uguale al nome del DataBase stesso VIP !!!
        static String _sEncryptedHost, _sEncryptedDatabase;
        static String _sHost, _sTunnel_URL;

        static String[] _sQueue_Object = new String[2];

        static byte[] _key, _iv;

        static TErrMsg _ErrMsg, _WrnMsg;

        // gestione cross thread
        static readonly Queue eventQueue = new Queue();

        /// <summary>Lista per gestione ordini remoti</summary>
        public static List<TWebOrder> _sWebOrdersList = new List<TWebOrder>();

        /// <summary>struct per accesso database remoto</summary>
        public static TWebServerParams _sWebServerParams = new TWebServerParams(0);

        /// <summary>ottiene la richiesta di webService</summary>
        public static bool GetWebServiceReq() { return _bWebServiceRequested; }

        /// <summary>imposta la richiesta di caricamento listino Web</summary>
        public static void SetWebPriceListLoadRequest() { _bPrimaVolta_o_ForzaCaricamentoListino = true; }

        /// <summary>mette evento in coda cross thread</summary>
        public static void EventEnqueue(String[] sEvQueueObj) { eventQueue.Enqueue(sEvQueueObj); }

        /// <summary>costruttore che predispone per la crittografia</summary>
        public dBaseTunnel_my()
        {
            _rdBaseTunnel_my = this;

            // prepara le costanti per la crittografia:
            // Create sha256 hash
            SHA256 mySHA256 = SHA256Managed.Create();
            _key = mySHA256.ComputeHash(Encoding.UTF8.GetBytes(CIPHER_KEY));

            // Create secret IV
            _iv = new byte[16] { 0x03, 0x01, 0x04, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };

            _bStartReadRemTable = false;
            _bWebServiceRequested = (ReadRegistry(WEB_SERVICE_MODE_KEY, 0) == 1);
            _bPrimaVolta_o_ForzaCaricamentoListino = true;

            _sWebServerParams = _rdBaseIntf.dbGetWebServerParams(); // si prendono dal DB

            if (_sWebServerParams.sWebTablePrefix == null)
                _sRemoteTablePrefix = "";
            else
                _sRemoteTablePrefix = Path.GetFileName(_sWebServerParams.sWebTablePrefix).ToLower();

            NOME_NSC_RDBTBL = RELEASE_TBL + "_" + _sRemoteTablePrefix + "_num_of_orders";
            NOME_PREZZI_RDBTBL = RELEASE_TBL + "_" + _sRemoteTablePrefix + "_price_list";
            NOME_STATUS_RDBTBL = RELEASE_TBL + "_" + _sRemoteTablePrefix + "_status";
            NOME_ORDERS_RDBTBL = RELEASE_TBL + "_" + _sRemoteTablePrefix + "_orders";
            NOME_LOG_RDBTBL = RELEASE_TBL + "_" + _sRemoteTablePrefix + "_log";

            if (_sWebServerParams.sWeb_DBase == "standfacile_rdb")
            {
                _sTunnel_URL = String.Format("http://localhost/standfacile_{0}_php/{1}?", RELEASE_TBL, _MYSQL_TUNNEL);
                _sHost = "localhost";
            }
            else
            {
                _sTunnel_URL = String.Format("https://www.standfacile.org/standfacile_{0}_php/{1}?", RELEASE_TBL, _MYSQL_TUNNEL);
                _sHost = DB_WEB_SERVER;
            }

            _sEncryptedHost = Encrypt_WS(_sHost);
            _sEncryptedDatabase = Encrypt_WS(_sWebServerParams.sWeb_DBase);

            eventQueue.Clear();

            // istanziazione Timer
            _timer = new System.Timers.Timer(200);

            // Hook up the Elapsed event for the timer. 
            _timer.Elapsed += TimerElapsed;
            _timer.AutoReset = true;
            _timer.Enabled = true;

            if (_bWebServiceRequested)
                LogToFile("dBaseTunnel_my: WebOrderEnabled");
            else
                LogToFile("dBaseTunnel_my: !WebOrderEnabled");

            //tunnelListinoTest();
        }

        /// <summary>
        /// contatore per la riduzione degli avvisi WRN_DBR di mancata connessione al database remoto
        /// </summary>
        static int iAvvisiCounter = 0;

        /// <summary> effettua un ping per verificare la connessione http,<br/>
        /// con localhost funziona sempre
        /// </summary>
        public static bool rdbPing()
        {
            Ping myPing = new Ping();
            PingOptions options = new PingOptions()
            {
                DontFragment = true
            };

            string sTmp, data = "abcdefghijknopgrstuxyvwz12345678";
            byte[] buffer = Encoding.UTF8.GetBytes(data);
            int timeout = 4000;

            try
            {
                PingReply reply = myPing.Send(_sHost, timeout, buffer, options);

                if (reply.Status == IPStatus.Success)
                {
                    sTmp = String.Format("dBaseTunnel : rdbPing {0} {1} ms", reply.Address.ToString(), reply.RoundtripTime);

                    Console.WriteLine("dBaseTunnel: rdbPing Address: {0}", reply.Address.ToString());
                    Console.WriteLine("dBaseTunnel: rdbPing RoundTrip time: {0} ms", reply.RoundtripTime);
                    return true;
                }
            }
            catch (Exception)
            {
                // connessione non possibile al database remoto
                _WrnMsg.iErrID = WRN_DBR;
                _WrnMsg.sMsg = "dBaseTunnel server non raggiungibile";
                if (iAvvisiCounter == 0)
                {
                    iAvvisiCounter = 6;
                    WarningManager(_WrnMsg);
                }
                else
                    iAvvisiCounter--;

                LogToFile("dBaseTunnel : rdbPing");
            }

            return false; // non dovrebbe mai passare di qui !
        }

        /// <summary>
        /// funzione per l'invio di una richiesta al Tunnel Http<br/>
        /// se fallisce sResponseFromServer = ""<br/>
        /// altrimenti ritorn la risposta del Tunnel decriptata
        /// </summary>
        private static String SendWebRequest(String sSQL_QueryPrm, int iTimeoutParam = 4000)
        {
            String sSQL_Query, sGQuery, sResponseFromServer = "";
            StreamReader reader;
            Stream dataStream;

            // LogToFile(String.Format("dBaseTunnel : sSQL_Query {0}", sSQL_QueryPrm));
            sSQL_Query = Encrypt_WS(sSQL_QueryPrm);

            /*****************************************************************************
             * VIP: l'utente ha lo stesso nome del database, quindi non serve inviarlo !
             *****************************************************************************/

            sGQuery = String.Format(@"{0}host={1}&dbname={2}&password={3}&query={4}&encrypted=1",
                        _sTunnel_URL, Base64Encode(_sEncryptedHost), Base64Encode(_sEncryptedDatabase),
                        Base64Encode(_sWebServerParams.sWebEncryptedPwd), Base64Encode(sSQL_Query));

            LogToFile(String.Format("SendWebRequest : sGQuery lenght={0}", sGQuery.Length));

            WebResponse response;
            WebRequest request = WebRequest.Create(sGQuery);
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Timeout = iTimeoutParam;

            try
            {
                response = request.GetResponse();

                if (((HttpWebResponse)response).StatusCode == HttpStatusCode.OK)
                {
                    dataStream = response.GetResponseStream();
                    reader = new StreamReader(dataStream);
                    // necessario invocare questa funzione, altrimenti ci sono errori timeout
                    sResponseFromServer = reader.ReadToEnd();

                    sResponseFromServer = Decrypt_WS(sResponseFromServer);

                    response.Close();
                    dataStream.Close();
                    reader.Close();
                }
                else
                {
                    LogToFile(String.Format("dBaseTunnel : HttpStatusCode KO"));
                }
            }
            catch (Exception e)
            {
                LogToFile(String.Format("dBaseTunnel : HttpWebResponse Exception {0}", e.ToString()));
            }

            return sResponseFromServer;
        }


        /// <summary>
        /// cerca di leggere la tabella "v5 ... _status"
        /// ritorna true se la connessione al webserver ha successo, false altrimenti<br/>
        /// viene controllata la lettura della tabella di Stato
        /// </summary>
        public static bool rdbCheckConnection(String sWebPageParam, String sWeb_DBaseParam, String sWeb_DBasePwdParam, bool bSilentParam)
        {
            bool bHostConnection_Ok;
            String sSQL_Query, sGQuery;
            String sResponseFromServer = "";

            String sEncryptedDatabase = Encrypt_WS(sWeb_DBaseParam);
            String sEncryptedPwd = Encrypt_WS(sWeb_DBasePwdParam);

            String sTunnel_URL, sEncryptedHost;

            if (sWeb_DBaseParam == "standfacile_rdb")
            {
                sTunnel_URL = String.Format("http://localhost/standfacile_{0}_php/{1}?", RELEASE_TBL, _MYSQL_TUNNEL);
                sEncryptedHost = Encrypt_WS("localhost");
            }
            else
            {
                sTunnel_URL = String.Format("https://www.standfacile.org/standfacile_{0}_php/{1}?", RELEASE_TBL, _MYSQL_TUNNEL);
                sEncryptedHost = Encrypt_WS(DB_WEB_SERVER);
            }

            bHostConnection_Ok = rdbPing();

            // sicurezza : si prosegue solo se c'è la connessione al DB
            if (!bHostConnection_Ok)
                return false;

            try
            {
                sSQL_Query = String.Format("SELECT * FROM {0};", RELEASE_TBL + "_" + Path.GetFileName(sWebPageParam) + "_status");
                sSQL_Query = Encrypt_WS(sSQL_Query);

                sGQuery = String.Format(@"{0}host={1}&dbname={2}&password={3}&query={4}&encrypted=1",
                            sTunnel_URL, Base64Encode(sEncryptedHost), Base64Encode(sEncryptedDatabase),
                            Base64Encode(sEncryptedPwd), Base64Encode(sSQL_Query));

                WebResponse response = null;
                WebRequest request = WebRequest.Create(sGQuery);
                request.Credentials = CredentialCache.DefaultCredentials;
                request.Timeout = 4000;

                try
                {
                    response = request.GetResponse();
                }
                catch (Exception e)
                {
                    LogToFile(String.Format("rdbCheck : HttpWebResponse Exception {0}", e.ToString()));
                }

                if ((response != null) && ((HttpWebResponse)response).StatusCode == HttpStatusCode.OK)
                {
                    Stream dataStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);
                    sResponseFromServer = reader.ReadToEnd();

                    sResponseFromServer = Decrypt_WS(sResponseFromServer);
                }

                _WrnMsg.sMsg = "'" + sWeb_DBaseParam + "'";

                if (sResponseFromServer.Contains(_NO_DB_ERRORS))
                {
                    if (!bSilentParam) // Messaggio di connessione
                    {
                        _WrnMsg.iErrID = WRN_WSTS;
                        WarningManager(_WrnMsg);
                    }

                    LogToFile("rdbCheckConnection : connessione Ok");
                    return true;
                }
                else
                {
                    if (!bSilentParam) // Messaggio di connessione
                    {
                        _WrnMsg.iErrID = WRN_WSTF;
                        WarningManager(_WrnMsg);
                    }

                    LogToFile("rdbCheckConnection : versione non Ok");
                    return false;
                }
            }

            catch (Exception)
            {
                LogToFile("rdbCheckConnection : stato dbException");
                return false;
            }
        }

        /// <summary>funzione di inizializzazione caricamento per visione tabella ordini remota</summary>
        public static void rdbCaricaTabella()
        {
            _sWebOrdersList.Clear();
            EsploraRemOrdiniDB_Dlg.Init();

            rdbCaricaTabellaStart();
        }

        /// <summary>funzione di avvio caricamento per visione tabella ordini remota, ritorna immediatamente</summary>
        public static void rdbCaricaTabellaStart()
        {
            _bStartReadRemTable = true;
        }

        ///<summary> 
        ///thread per eseguire rdbCaricaTabellaOrdini() <br/>
        ///eseguito ogni 200ms
        /// </summary>

        static void TimerElapsed(Object source, ElapsedEventArgs e)
        {
            String[] sEvQueueObj;

            if (_bStartReadRemTable)
            {
                _bStartReadRemTable = false;
                rdbCaricaTabellaOrdini();
            }

            while (eventQueue.Count > 0)
            {
                sEvQueueObj = (String[])eventQueue.Dequeue();

                // avvia Upload del Listino mediante altro Thread
                if (sEvQueueObj[0] == WEB_PRICELIST_LOAD_START)
                {
                    if (rdbUploadListino(false))
                    {
                        DataManager.SetRemDBChecksum();

                        _WrnMsg.sMsg = _sHost;
                        _WrnMsg.iErrID = WRN_PUPS;
                        WarningManager(_WrnMsg);
                    }
                }
                // avvia Upload del Listino mediante altro Thread
                if (sEvQueueObj[0] == WEB_PRICELIST_FORCE_LOAD_START)
                {
                    if (rdbUploadListino(true))
                    {
                        DataManager.SetRemDBChecksum();

                        _WrnMsg.sMsg = _sHost;
                        _WrnMsg.iErrID = WRN_PUPS;
                        WarningManager(_WrnMsg);
                    }
                }
                else if (sEvQueueObj[0] == WEB_ORDER_PRINT_DONE)
                {
                    // invoca rdb_aggiornaOrdiniWebServiti()
                    _iTimerCounter = 1;
                }
            }

            if (_iTimerCounter == 0)
            {
                _iTimerCounter = 5 * 60;

                if (_bWebServiceRequested)
                {
                    // rdb_aggiornaOrdiniWebServiti() deve essere chiamato solo se _bWebServiceRequested == true
                    // altrimenti il ping() genera eccezioni

                    if (rdb_aggiornaOrdiniWebServiti())
                        _iTimerCounter = 5 * 5; // accelera con solo 5s perchè ce ne sono altri
                }
            }
            else
            {
                if (_iTimerCounter > 0)
                    _iTimerCounter--;
            }
        }

        /// <summary>funzione di caricamento per visione tabella ordini remota</summary>
        static void rdbCaricaTabellaOrdini()
        {
#pragma warning disable IDE0018

            int iTableRow, iIndex, iNumCoperti, iNumOrdine, iPrevOrder = 0;
            String sInStr, sSQL_Query, sResponseFromServer;

            // avvia la visualizzazione della tabella
            _sQueue_Object[0] = WEB_ALL_ORDERS_LOAD_START;
            _sQueue_Object[1] = "";

            EsploraRemOrdiniDB_Dlg.EventEnqueue(_sQueue_Object);

            TWebOrder sOrdineTmp = new TWebOrder(0);

            try
            {
                // ORDER BY order_ID ASC
                sSQL_Query = String.Format("SELECT * from {0} WHERE ((menuItem_ID = '{1}' OR menuItem_ID = '{2}') AND (cancellation = 0) AND (print = 0)) ORDER BY order_ID ASC LIMIT 200",
                    NOME_ORDERS_RDBTBL, ORDER_CONST._START_OF_ORDER, ORDER_CONST._PRICE_LIST_CHECKSUM);

                sResponseFromServer = SendWebRequest(sSQL_Query);

                LogToFile(String.Format("rdbCaricaTabellaOrdini : sResponse lenght={0}", sResponseFromServer.Length));

                // ottiene la tabella dell'output JSON
                var jss = new JavaScriptSerializer();
                var dict = jss.Deserialize<dynamic>(sResponseFromServer);
                var sTable = dict["rows"];

                iTableRow = 0;
                iIndex = 0;
                _sWebOrdersList.Clear();

                LogToFile(String.Format("rdbCaricaTabellaOrdini : sTable lenght={0}", sTable.Length));

                if (sTable.Length > 0)
                    iPrevOrder = Convert.ToInt32(sTable[iTableRow][0]["1"]);

                while (iTableRow < sTable.Length)
                {
                    sInStr = sTable[iTableRow][2]["3"];
                    iNumOrdine = Convert.ToInt32(sTable[iTableRow][0]["1"]);

                    if (iPrevOrder != iNumOrdine)
                    {
                        iPrevOrder = iNumOrdine;
                        sOrdineTmp = new TWebOrder(0);
                        iIndex = 0;
                    }

                    if (sInStr == ORDER_CONST._START_OF_ORDER)
                    {

                        sOrdineTmp.iNumOrdine = Convert.ToInt32(sTable[iTableRow][0]["1"]);
                        sOrdineTmp.iStatus = Convert.ToInt32(sTable[iTableRow][7]["8"]);
                        sOrdineTmp.sCliente = sTable[iTableRow][1]["2"];
                        sOrdineTmp.sDateTime = sTable[iTableRow][5]["6"];
                        sOrdineTmp.iTotaleReceipt = Convert.ToInt32(sTable[iTableRow][3]["4"]);

                        if (int.TryParse(sTable[iTableRow][4]["5"], out iNumCoperti) == true)
                            sOrdineTmp.iNumCoperti = iNumCoperti;

                        iIndex++;
                    }
                    else if (sInStr == ORDER_CONST._PRICE_LIST_CHECKSUM)
                    {
                        sOrdineTmp.sChecksum = sTable[iTableRow][5]["6"];

                        iIndex++;
                    }

                    // fine analisi intestazioni ordine
                    if (iIndex == 2)
                        _sWebOrdersList.Add(sOrdineTmp);

                    iTableRow++;
                }
            }
            catch (Exception)
            {
                // connessione non possibile al database remoto
                _WrnMsg.iErrID = WRN_DBR;
                _WrnMsg.sMsg = "dBaseTunnel load Table";
                WarningManager(_WrnMsg);

                LogToFile("dBaseTunnel : rdbCaricaTabella");
            }

            // completa la visualizzazione della tabella
            _sQueue_Object[0] = WEB_ALL_ORDERS_LOAD_DONE;
            EsploraRemOrdiniDB_Dlg.EventEnqueue(_sQueue_Object);
        }

        ///<summary>funzione di caricamento ordine remoto mediante tunnel HTTP</summary>
        public static bool rdbCaricaOrdine(int iOrdineParam)    // verificare il caricamento di tutte le righe !!!
        {
            bool bNoProblem = true;
            int i, iIndex;
            String sTipo, sSQL_Query, sResponseFromServer;

            dbAzzeraDatiOrdine(ref RDB_Data);

            try
            {
                // ORDER BY order_ID ASC
                sSQL_Query = String.Format("SELECT * from {0} WHERE order_ID = {1} ORDER BY order_ID ASC LIMIT 100",
                            NOME_ORDERS_RDBTBL, iOrdineParam);

                sResponseFromServer = SendWebRequest(sSQL_Query);

                LogToFile(String.Format("rdbCaricaOrdine : sResponse lenght={0}", sResponseFromServer.Length));

                // ottiene la tabella dell'output JSON
                var jss = new JavaScriptSerializer();
                var dict = jss.Deserialize<dynamic>(sResponseFromServer);
                var sTable = dict["rows"];

                iIndex = 0;

                i = 0;
                while (iIndex < sTable.Length)
                {
                    sTipo = sTable[iIndex][2]["3"];

                    if (sTipo == ORDER_CONST._START_OF_ORDER)
                    {
                        //String sDebug = sTable[iIndex][8]["9"];

                        int iRDB_StatusWeb = Convert.ToInt32(sTable[iIndex][7]["8"]); //readerOrdine.GetInt32("status");

                        RDB_Data.iStatusReceipt = iRDB_StatusWeb & 0xFFF1;
                        // | BIT_CARICATO_DA_WEB doppione utile per la comprensione
                        RDB_Data.iStatusReceipt = SetBit(RDB_Data.iStatusReceipt, (int)STATUS_FLAGS.BIT_CARICATO_DA_WEB);

                        // shift per rialineare i bit da web e maschera bit sconti
                        RDB_Data.iStatusSconto = (iRDB_StatusWeb >> 1) & 0x0007;

                        RDB_Data.iNumOrdineWeb = Convert.ToInt32(sTable[iIndex][0]["1"]);    //readerOrdine.GetInt32("order_ID");
                        RDB_Data.sWebDateTime = Convert.ToString(sTable[iIndex][5]["6"]);    //readerOrdine.GetString("sText");
                        RDB_Data.bAnnullato = (Convert.ToInt32(sTable[iIndex][8]["9"]) != 0);    //readerOrdine.GetBoolean("iAnnullato");
                        RDB_Data.bStampato = (Convert.ToInt32(sTable[iIndex][10]["11"]) != 0);  //readerOrdine.GetBoolean("bStampato");
                    }

                    // Tavolo
                    else if (sTipo == ORDER_CONST._TAVOLO)
                        RDB_Data.sTavolo = sTable[iIndex][5]["6"];

                    // Name
                    else if (sTipo == ORDER_CONST._NOME)
                        RDB_Data.sNome = sTable[iIndex][5]["6"];

                    // Nota
                    else if (sTipo == ORDER_CONST._NOTA)
                        RDB_Data.sNota = sTable[iIndex][5]["6"];

                    // Checksum
                    else if (sTipo == ORDER_CONST._PRICE_LIST_CHECKSUM)
                    {
                        RDB_Data.sPL_Checksum = sTable[iIndex][5]["6"];
                    }

                    // Sconto alla cassa

                    else
                    {
                        RDB_Data.Articolo[i].sTipo = sTipo;
                        RDB_Data.Articolo[i].iQuantitaOrdine = Convert.ToInt32(sTable[iIndex][4]["5"]);   //readerOrdine.GetInt32("quantity");
                        i++;
                    }

                    iIndex++;
                } // end while

            }

            catch (Exception)
            {
                bNoProblem = false;
                // connessione non possibile al database remoto
                _WrnMsg.iErrID = WRN_DBR;
                _WrnMsg.sMsg = "dBaseTunnel load Order";
                WarningManager(_WrnMsg);

                LogToFile("dBaseTunnel : rdbCaricaOrdine");
            }

            return bNoProblem;
        }

        /// <summary>funzione di annullo ordine remoto</summary>
        public static bool rdbAnnullaOrdine(int iOrdineParam)
        {
            String sSQL_Query, sResponseFromServer;

            LogToFile(String.Format("dBaseTunnel : rdbAnnullaOrdine {0}", iOrdineParam));

            try
            {
                // ORDER BY order_ID ASC
                sSQL_Query = String.Format("UPDATE {0} SET cancellation = 1 WHERE order_ID = {1} LIMIT 200",
                            NOME_ORDERS_RDBTBL, iOrdineParam);

                sResponseFromServer = SendWebRequest(sSQL_Query);

                if (!sResponseFromServer.Contains(_NO_DB_ERRORS))
                    return false;


                // ORDER BY order_ID ASC
                sSQL_Query = String.Format("UPDATE {0} SET cancellationTime = \'{1}\' WHERE order_ID = {2} ORDER BY order_ID ASC LIMIT 1",
                            NOME_ORDERS_RDBTBL, GetDateTimeString(), iOrdineParam);

                sResponseFromServer = SendWebRequest(sSQL_Query);

                if (sResponseFromServer.Contains(_NO_DB_ERRORS))
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                // connessione non possibile al database remoto
                _WrnMsg.iErrID = WRN_DBR;
                _WrnMsg.sMsg = "dBaseTunnel rdbAnnullaOrdine";
                WarningManager(_WrnMsg);

                LogToFile("dBaseTunnel : rdbAnnullaOrdine");
                return false;
            }
        }

        /// <summary>funzione che flagga l'ordine remoto come stampato</summary>
        public static bool rdbSegnaOrdineStampato(int iOrdineParam)
        {
            String sSQL_Query, sResponseFromServer;

            try
            {
                // sicurezza
                if (!_bWebServiceRequested)
                    return false;

                LogToFile(String.Format("dBaseTunnel : rdbSegnaOrdineStampato {0}", iOrdineParam));

                // ORDER BY order_ID ASC
                sSQL_Query = String.Format("UPDATE {0} SET print = 1 WHERE order_ID = {1} LIMIT 200",
                            NOME_ORDERS_RDBTBL, iOrdineParam);

                sResponseFromServer = SendWebRequest(sSQL_Query);

                if (!sResponseFromServer.Contains(_NO_DB_ERRORS))
                    return false;

                // ORDER BY order_ID ASC
                sSQL_Query = String.Format("UPDATE {0} SET printTime = \'{1}\' WHERE order_ID = {2} ORDER BY order_ID ASC LIMIT 1",
                            NOME_ORDERS_RDBTBL, GetDateTimeString(), iOrdineParam);

                sResponseFromServer = SendWebRequest(sSQL_Query);

                if (sResponseFromServer.Contains(_NO_DB_ERRORS))
                {
                    String[] sQueue_Object = new String[2] { WEB_ORDER_PRINT_DONE, "" };
                    EsploraRemOrdiniDB_Dlg.EventEnqueue(sQueue_Object);

                    return true;
                }
                else
                    return false;
            }
            catch (Exception)
            {
                // connessione non possibile al database remoto
                _WrnMsg.iErrID = WRN_DBR;
                _WrnMsg.sMsg = "dBaseTunnel flag Order as Printed";
                WarningManager(_WrnMsg);

                LogToFile("dBaseTunnel : rdbSegnaOrdineStampato");
                return false;
            }
        }


        /// <summary> scrive una stringa nel Logfile remoto</summary>
        public static bool rdbLogWriteVersion(String sTxtParam)
        {
            bool bHostConnection_Ok;
            String sSQL_Query, sResponseFromServer;

            try
            {
                bHostConnection_Ok = rdbPing();

                // sicurezza : si prosegue solo se c'è la connessione all' rDB
                if (!bHostConnection_Ok)
                    return false;

                if (string.IsNullOrEmpty(_sWebServerParams.sWeb_DBase))
                    return false;

                sSQL_Query = String.Format("INSERT INTO {0} (user_ID, text, date) VALUES ('{1}', '{2}', '{3}')", NOME_LOG_RDBTBL, -2, sTxtParam, GetDateTimeString());

                sResponseFromServer = SendWebRequest(sSQL_Query, 2000);

                // connessione Host ma la tabella Listino non esiste
                if (sResponseFromServer.Contains(NOME_LOG_RDBTBL) &&
                    sResponseFromServer.Contains("doesn") && sResponseFromServer.Contains("exist"))
                {
                    LogToFile("rdbLogWriteVersion : tabella Log non esiste");

                    if (_bPrimaVoltaLog)
                    {
                        _bPrimaVoltaLog = false;

                        // connessione non possibile al database remoto
                        _WrnMsg.sMsg = _sHost;
                        _WrnMsg.iErrID = WRN_WLNF;
                        WarningManager(_WrnMsg);

                        return false;
                    }
                }
                // connessione Host ma mancata connessione al database
                else if (!sResponseFromServer.Contains(_NO_DB_ERRORS))
                {
                    LogToFile("rdbLogWriteVersion : no Response From DB Server");

                    if (_bPrimaVoltaLog)
                    {
                        _bPrimaVoltaLog = false;

                        // connessione non possibile al database remoto
                        _WrnMsg.sMsg = _sHost;
                        _WrnMsg.iErrID = WRN_DBR;
                        WarningManager(_WrnMsg);

                        return false;
                    }
                }

                LogToFile("rdbLogWriteVersion : scrittuta Log eseguita");
            }

            catch (Exception)
            {
                LogToFile("rdbLogWriteVersion : dbException");

                if (_bPrimaVoltaLog)
                {
                    _bPrimaVoltaLog = false;

                    // connessione non possibile al database remoto
                    _WrnMsg.iErrID = WRN_DBR;
                    _WrnMsg.sMsg = "rdb CheckListino priceList";
                    WarningManager(_WrnMsg);

                    return false;
                }
            }

            return true; // tutto OK
        }

        /// <summary>
        /// cerca il checksum nella tabella "Listino" lo ritorna come stringa se ha successo,<br/>
        /// se la tabella non esiste ritorna "---" per caricamento successivo,<br/>
        ///  altrimenti se non c'è connessione al DB server ritorna "",<br/>
        ///  solo con MySQL
        /// </summary>
        public static String rdbCheckListino(int iTimeoutParam)
        {
            String sSQL_Query, sResponseFromServer, sRecord = "";

            try
            {
                sSQL_Query = "SELECT text FROM " + NOME_PREZZI_RDBTBL + " ORDER BY row_ID DESC LIMIT 1";

                sResponseFromServer = SendWebRequest(sSQL_Query, iTimeoutParam);

                // connessione Host ma la tabella Listino non esiste
                if (sResponseFromServer.Contains(NOME_PREZZI_RDBTBL) &&
                    sResponseFromServer.Contains("doesn") && sResponseFromServer.Contains("exist"))
                {
                    LogToFile("rdbCheckListino : tabella Listino non esiste");

                    if (_bPrimaVolta_o_ForzaCaricamentoListino)
                    {
                        _bPrimaVolta_o_ForzaCaricamentoListino = false;

                        // connessione non possibile al database remoto
                        _WrnMsg.sMsg = _sHost;
                        _WrnMsg.iErrID = WRN_WPNF;
                        WarningManager(_WrnMsg);

                        return "---";
                    }
                }
                // connessione Host ma mancata connessione al database
                else if (!sResponseFromServer.Contains(_NO_DB_ERRORS))
                {
                    LogToFile("rdbCheckListino : no Response From DB Server");

                    if (_bPrimaVolta_o_ForzaCaricamentoListino)
                    {
                        _bPrimaVolta_o_ForzaCaricamentoListino = false;

                        // connessione non possibile al database remoto
                        _WrnMsg.sMsg = _sHost;
                        _WrnMsg.iErrID = WRN_DBR;
                        WarningManager(_WrnMsg);

                        return "";
                    }
                }

                // ottiene la tabella dell'output JSON
                var jss = new JavaScriptSerializer();
                var dict = jss.Deserialize<dynamic>(sResponseFromServer);
                var sTable = dict["rows"];

                sRecord = sTable[0][0]["1"];

                if (sRecord.Substring(0, 5) == "#CKW ")
                {
                    sRecord = sRecord.Substring(5, 8);

                    if (_bPrimaVolta_o_ForzaCaricamentoListino)
                    {
                        _bPrimaVolta_o_ForzaCaricamentoListino = false;

                        _WrnMsg.sMsg = _sHost;
                        _WrnMsg.iErrID = WRN_WSCS;
                        WarningManager(_WrnMsg);
                    }
                }

                LogToFile("rdbCheckListino : ricerca checksum eseguita");
            }

            catch (Exception)
            {
                LogToFile("rdbCheckListino : dbException");

                if (_bPrimaVolta_o_ForzaCaricamentoListino)
                {
                    _bPrimaVolta_o_ForzaCaricamentoListino = false;

                    // connessione non possibile al database remoto
                    _WrnMsg.iErrID = WRN_DBR;
                    _WrnMsg.sMsg = "rdb CheckListino priceList";
                    WarningManager(_WrnMsg);

                    return "---";
                }
            }

            return sRecord; // tutto OK
        }

        /// <summary>
        /// Funzione di upload nel database MySQL remoto del listino <br/>
        /// da parte della cassa primaria, a disposizione poi degli ordini web
        /// </summary>
        static bool rdbUploadListino(bool bForceUpload)
        {
            bool bHostConnection_Ok;

            int i, j;
            // obbiettivo url lenght < 2083  bytes
            // 60 max, 85 ko altrimenti url è troppo lungo
            const int MAX_JOINED_ROWS = 40;

            String sInStr, sSQL_Query, sResponseFromServer;
            String sNomeFilePrezzi, sDir, sTmp;

            DialogResult dResult;
            StreamReader fprz = null;

            try
            {
                bHostConnection_Ok = rdbPing();

                // sicurezza : si prosegue solo se c'è la connessione all' rDB
                if (!bHostConnection_Ok || !(_bWebServiceRequested || bForceUpload) || (SF_Data.iNumCassa != CASSA_PRINCIPALE))
                    return false;

                if (String.IsNullOrEmpty(_sWebServerParams.sWeb_DBase))
                    return false;

                // ulteriore sicurezza MessageBox modale 
                sTmp = String.Format("Sei sicuro di procedere con upload Listino in\n\n   {0}?", _sRemoteTablePrefix);
                dResult = MessageBox.Show(sTmp, "Attenzione !", MessageBoxButtons.YesNo,
                                            MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, MessageBoxOptions.DefaultDesktopOnly);

                if (dResult == DialogResult.No)
                    return false;

                sSQL_Query = String.Format("DROP TABLE IF EXISTS {0}", NOME_PREZZI_RDBTBL);

                sResponseFromServer = SendWebRequest(sSQL_Query);

                // table create se non esiste
                sSQL_Query = String.Format("CREATE TABLE IF NOT EXISTS {0} (row_ID INT NOT NULL, text VARCHAR(100), PRIMARY KEY(row_ID));", NOME_PREZZI_RDBTBL);

                sResponseFromServer = SendWebRequest(sSQL_Query);

                if (sResponseFromServer.Contains(_NO_DB_ERRORS))
                {
                    sDir = DataManager.GetExeDir() + "\\";

                    sNomeFilePrezzi = NOME_FILE_LISTINO;
                    fprz = File.OpenText(sDir + NOME_FILE_LISTINO);

                    _ErrMsg.sNomeFile = sNomeFilePrezzi;

                    if (fprz == null)
                    {
                        _ErrMsg.iErrID = WRN_FNO;
                        WarningManager(_ErrMsg);
                    }

                    i = 0;
                    j = 0;
                    sInStr = "";

                    while ((sInStr != null) && (i < 1000))
                    {
                        sSQL_Query = String.Format("INSERT INTO {0} (row_ID, text) VALUES ", NOME_PREZZI_RDBTBL);

                        // meglio usare una lista da leggere preventivamente ???

                        while ((sInStr = fprz.ReadLine()) != null)
                        {
                            if (j > 0)
                            {
                                sSQL_Query += ",";
                            }

                            sInStr = AddSlashes(sInStr);

                            sSQL_Query += String.Format("({0}, \'{1}\')", i, sInStr);

                            i++;
                            j++;

                            if (j >= MAX_JOINED_ROWS)
                                break;
                        }

                        if ((sInStr == null) && (j == 0))
                            break;

                        sSQL_Query += ";";

                        sResponseFromServer = SendWebRequest(sSQL_Query);

                        if (!sResponseFromServer.Contains(_NO_DB_ERRORS))
                        {
                            throw new WebException("dBaseTunnel: errore rdbSalvaListino");
                        }

                        j = 0;
                    }
                }

                fprz?.Close();
                return true;
            }
            catch (Exception)
            {
                // connessione non possibile al database remoto
                _WrnMsg.iErrID = WRN_DBR;
                _WrnMsg.sMsg = "dBaseTunnel rdbSalvaListino";
                WarningManager(_WrnMsg);
            }

            LogToFile("dBaseTunnel: rdbSalvaListino");

            fprz?.Close();
            return false;
        }

        /// <summary>
        /// funzione che contrassegna in background gli ordini web serviti leggendo dalla tabella NOME_WEBORD_DBTBL
        /// </summary>
        public static bool rdb_aggiornaOrdiniWebServiti()
        {
            int iNumOrdineWeb;
            bool bHostConnection_Ok;
            String sTmp;

            if (SF_Data.iNumCassa == CASSA_PRINCIPALE)
            {
                bHostConnection_Ok = rdbPing();

                // sicurezza : si prosegue solo se c'è la connessione all' rDB
                if (!bHostConnection_Ok || !_bWebServiceRequested)
                    return false;

                try
                {
                    iNumOrdineWeb = _rdBaseIntf.dbGetOrdiniWebServiti();

                    if (iNumOrdineWeb > 0)
                    {
                        if (rdbSegnaOrdineStampato(iNumOrdineWeb))
                        {
                            _rdBaseIntf.dbClearOrdineWebServito(iNumOrdineWeb);

                            if ((EsploraRemOrdiniDB_Dlg.rEsploraRemOrdiniDB_Dlg != null) && EsploraRemOrdiniDB_Dlg.rEsploraRemOrdiniDB_Dlg.Visible)
                                EsploraRemOrdiniDB_Dlg.RefreshTableRequest();

                            sTmp = String.Format("rdb_aggiornaOrdiniWebServiti : ordine {0} aggiornato", iNumOrdineWeb);
                            LogToFile(sTmp, true);

                            //sTmp = String.Format("rdb_aggiornaOrdiniWebServiti : RCPs={0}", SF_Data.iNumOfLastReceipt);
                            //rdbLogWriteVersion(sTmp);

                            return true;
                        }
                        else
                        {
                            sTmp = String.Format("rdb_aggiornaOrdiniWebServiti : ordine {0} non aggiornato", iNumOrdineWeb);

                            LogToFile(sTmp, true);
                            return false;
                        }
                    }
                    else
                        return false;
                }

                catch (Exception)
                {
                    _WrnMsg.iErrID = WRN_DBE;
                    _WrnMsg.sMsg = String.Format("accesso alla tabella WEBORD_DBTBL non possibile");
                    WarningManager(_WrnMsg);
                    return false;
                }
            }
            else
                return false;
        }


        /// <summary>overload Funzione di codifica AES-256</summary>
        public static string Encrypt_WS(string plainTextParam)
        {
            return Encrypt_WS(plainTextParam, _key, _iv);
        }

        /// <summary>
        /// Funzione di codifica AES-256 <br/>
        /// Return the encrypted data as a string <br/>
        /// https://odan.github.io/2017/08/10/aes-256-encryption-and-decryption-in-php-and-csharp.html
        /// </summary>
        public static string Encrypt_WS(string plainText, byte[] key, byte[] iv)
        {
            // Instantiate a new Aes object to perform string symmetric encryption
            Aes encryptor = Aes.Create();

            encryptor.Mode = CipherMode.CBC;

            // Set key and IV
            byte[] aesKey = new byte[32];
            Array.Copy(key, 0, aesKey, 0, 32);
            encryptor.Key = aesKey;
            encryptor.IV = iv;

            // Instantiate a new MemoryStream object to contain the encrypted bytes
            MemoryStream memoryStream = new MemoryStream();

            // Instantiate a new encryptor from our Aes object
            ICryptoTransform aesEncryptor = encryptor.CreateEncryptor();

            // Instantiate a new CryptoStream object to process the data and write it to the 
            // memory stream
            CryptoStream cryptoStream = new CryptoStream(memoryStream, aesEncryptor, CryptoStreamMode.Write);

            // Convert the plainText string into a byte array
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);

            // Encrypt the input plaintext string
            cryptoStream.Write(plainBytes, 0, plainBytes.Length);

            // Complete the encryption process
            cryptoStream.FlushFinalBlock();

            // Convert the encrypted data from a MemoryStream to a byte array
            byte[] cipherBytes = memoryStream.ToArray();

            // Close both the MemoryStream and the CryptoStream
            memoryStream.Close();
            cryptoStream.Close();

            // Convert the encrypted byte array to a base64 encoded string
            string cipherText = Convert.ToBase64String(cipherBytes, 0, cipherBytes.Length);

            return cipherText;
        }

        /// <summary>overload Funzione di decodifica AES-256 <br/>
        /// return the decrypted data as a string <br/>
        /// </summary>
        public static string Decrypt_WS(string cipherTextParam)
        {
            try
            {
                return Decrypt_WS(cipherTextParam, _key, _iv);
            }
            catch (Exception e)
            {
                LogToFile(String.Format("dBaseTunnel : Decrypt_WS Exception {0} {1}", cipherTextParam, e.Message));
                return "";
            }
        }

        /// <summary>Funzione di decodifica AES-256 <br/>
        /// return the decrypted data as a string <br/>
        /// </summary>
        public static string Decrypt_WS(string cipherText, byte[] key, byte[] iv)
        {

            // Instantiate a new Aes object to perform string symmetric encryption
            Aes encryptor = Aes.Create();

            encryptor.Mode = CipherMode.CBC;

            // Set key and IV
            byte[] aesKey = new byte[32];
            Array.Copy(key, 0, aesKey, 0, 32);
            encryptor.Key = aesKey;
            encryptor.IV = iv;

            // Instantiate a new MemoryStream object to contain the encrypted bytes
            MemoryStream memoryStream = new MemoryStream();

            // Instantiate a new encryptor from our Aes object
            ICryptoTransform aesDecryptor = encryptor.CreateDecryptor();

            // Instantiate a new CryptoStream object to process the data and write it to the 
            // memory stream
            CryptoStream cryptoStream = new CryptoStream(memoryStream, aesDecryptor, CryptoStreamMode.Write);

            // Will contain decrypted plaintext
            string plainText = String.Empty;

            try
            {
                // Convert the ciphertext string into a byte array
                byte[] cipherBytes = Convert.FromBase64String(cipherText);

                // Decrypt the input ciphertext string
                cryptoStream.Write(cipherBytes, 0, cipherBytes.Length);

                // Complete the decryption process
                cryptoStream.FlushFinalBlock();

                // Convert the decrypted data from a MemoryStream to a byte array
                byte[] plainBytes = memoryStream.ToArray();

                // Convert the decrypted byte array to string
                plainText = Encoding.UTF8.GetString(plainBytes, 0, plainBytes.Length);
            }
            finally
            {
                // Close both the MemoryStream and the CryptoStream
                memoryStream.Close();
                cryptoStream.Close();
            }

            return plainText;
        }

        /// <summary>codifica Base64Encode per invio stringhe via HTTP <br/>
        /// return plainText;
        /// </summary>
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

    }
}


/*****************************************************************************************
	 NomeFile : StandCommonSrc/dBaseIntf_my.cs
	 Data	  : 02.11.2025
	 Autore   : Mauro Artuso

    nelle assegnazioni :
    DB_Data compare sempre a sx,
    SF_Data compare sempre a dx

    Backticks ``` are used in MySQL to select columns and tables from your MySQL source,
    single quotes are used for literals (stringhe).

    Attenzione : dbInit(dateParam) deve essere invocata all'inizio di ogni funzione
 *****************************************************************************************/

using System;
using System.Collections.Generic;
using static System.Convert;

using Devart.Data.MySql;

using static StandCommonFiles.ComDef;
using static StandCommonFiles.CommonCl;
using static StandCommonFiles.LogServer;

using StandFacile;
using static StandFacile.glb;
using static StandFacile.dBaseIntf;

namespace StandFacile_DB
{
#pragma warning disable IDE1006

    /// <summary>classe per la gestione di MySQL</summary>
    public partial class dBaseIntf_my
    {
        /// <summary>
        ///  Funzione di caricamento del listino dal database da parte della sola cassa secondaria <br/>
        ///  ritorna il numero di stringhe caricate
        /// </summary>
        /// <returns>il numero di elementi della Lista sStringsParam</returns>
        public int dbCaricaListino(List<string> sStringsParam)
        {
            bool bDBConnection_Ok;
            int i;
            String sRecord;
            MySqlDataReader readerListino;

#if STANDFACILE
            // sicurezza : si prosegue solo se è CASSA_SECONDARIA && bUSA_NDB
            if (!DataManager.CheckIf_CassaSec_and_NDB())
                return 0;
#endif

            bDBConnection_Ok = dbInit(GetActualDate(), SF_Data.iNumCassa);

            // sicurezza : si prosegue solo se c'è la connessione a DB
            if (!bDBConnection_Ok)
                return 0;

            try
            {
                i = 0;
                sStringsParam.Clear();

                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _Connection,
                    CommandText = "SELECT sText FROM " + NOME_LISTINO_DBTBL + " ORDER BY iRiga_ID ASC"
                };

                readerListino = cmd.ExecuteReader();

                while (readerListino.Read() && (i < MAX_NUM_ARTICOLI + EXTRA_NUM_LISTINO_HEAD))
                {
                    sRecord = readerListino.GetString("sText");
                    sStringsParam.Add(sRecord);

                    i++;
                }
            }

            catch (Exception)
            {
                _WrnMsg.iErrID = WRN_DBL;
                WarningManager(_WrnMsg);
                LogToFile("dbCaricaListino : dbException dbCaricaListino");
                return 0;
            }

            readerListino?.Close();

            return sStringsParam.Count;
        }

        /// <summary>
        ///  Funzione di caricamento della sequenza di test dal database da parte della sola cassa secondaria <br/>
        ///  ritorna il numero di stringhe caricate
        /// </summary>
        /// <returns>il numero di elementi della Lista sStringsParam</returns>
        public int dbCaricaTest(List<string> sStringsParam)
        {
            bool bDBConnection_Ok;
            int i;
            String sRecord;
            MySqlDataReader readerTest;

#if STANDFACILE
            // sicurezza : si prosegue solo se è CASSA_SECONDARIA && bUsaMySQL_db
            if (!DataManager.CheckIf_CassaSec_and_NDB()) // cassa secondaria e MySQL
                return 0;
#endif

            bDBConnection_Ok = dbInit(GetActualDate(), SF_Data.iNumCassa);

            // sicurezza : si prosegue solo se c'è la connessione a MySQL
            if (!bDBConnection_Ok)
                return 0;

            try
            {
                i = 0;
                sStringsParam.Clear();

                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _Connection,
                    CommandText = "SELECT sText FROM " + NOME_TEST_DBTBL + " ORDER BY iRiga_ID ASC"
                };

                readerTest = cmd.ExecuteReader();

                while (readerTest.Read() && (i < 10000))
                {
                    sRecord = readerTest.GetString("sText");
                    sStringsParam.Add(sRecord);

                    i++;
                }
            }

            catch (Exception)
            {
                _WrnMsg.iErrID = WRN_DBT;
                WarningManager(_WrnMsg);
                LogToFile("dbCaricaListino : dbException dbCaricaTest");
                return 0;
            }

            readerTest?.Close();

            return sStringsParam.Count;
        }

        /// <summary>
        /// carica i Dati nella struct DB_Articolo[], <br/>
        /// prendendoli per maggiore sicurezza dalla tabella degli ordini <br/>
        /// se iNumCassaParam == 0 considera tutte le casse <br/> <br/>
        /// iReportParam > 0 considera il tipo di sconto applicato
        /// usata da VisDatiDlg() ma solo in modo esperto <br/> <br/>
        /// 
        /// ritorna DB_Data.iNumOfLastReceipt se ha successo, -1 altrimenti
        /// </summary>
        public int dbCaricaDatidaOrdini(DateTime dateParam, int iNumCassaParam, bool bSilentParam = false, String sNomeTabellaParam = "", int iReportParam = 0)
        {
            bool bDBConnection_Ok, bDB_Read_CP_Ok;
            bool bNoProblem, bRigaAnnullata;
            bool bMatch, bSingleWarn, bScaricato;

            int i, j, iStatus, iDebug;

            // non si riferiscono a nessun ordine in particolare
            int iPrezzoUnitario, iQuantitaOrdine, iGruppoStampa;
            int iTotaleReceipt, iBuoniApplicatiReceipt;
            int iStatusScontoReceipt, iScontoStdReceipt, iScontoFissoReceipt, iScontoGratisReceipt;

            int iLastArticoloDBIndexP1;

            String sTmp, sTipo, sDebug;

            _WrnMsg.iErrID = 0; // resetta errori in altra data

#if !STAND_ORDINI
            _iDBArticoliLength_Is33 = IsBitSet(DB_Data.iGenericPrintOptions, (int)GEN_PRINTER_OPTS.BIT_CHARS33_PRINT_REQUIRED);
#endif
            // *** sicurezza ***
            if (!bUSA_NDB()) return -1;

            iLastArticoloDBIndexP1 = MAX_NUM_ARTICOLI; // successivamente potrebbe incrementare

            MySqlCommand cmd_Dati = new MySqlCommand();
            MySqlDataReader readerDati = null;

            MySqlCommand cmd_Ordini = new MySqlCommand();
            MySqlDataReader readerOrdine = null;

            // qualche variabile azzerata provoca elaborazioni aggiuntive
            // ma maggiore sicurezza, tanto si invoca poche volte
            dbAzzeraDatiGen();
            dbAzzeraDatiOrdine(ref DB_Data);

            /*********************************************************************
             *      prima parte Dati 1/2 caricamento da CASSA_PRINCIPALE
             *********************************************************************/
            try
            {
                // iNumCassaParam per SQLite, si prova con CASSA_PRINCIPALE per NDB
                bDBConnection_Ok = dbInit(dateParam, CASSA_PRINCIPALE, bSilentParam, sNomeTabellaParam);

                cmd_Dati.Connection = _Connection;

                cmd_Dati.CommandText = "SELECT * FROM " + _sDBTNameDati + " ORDER BY iRiga_ID ASC";

                if (bDBConnection_Ok)
                {
                    readerDati = cmd_Dati.ExecuteReader();
                    bDB_Read_CP_Ok = true;
                }
                else
                {
                    bDB_Read_CP_Ok = false;
                    return -1;
                }
            }
            catch (Exception)
            {
                bDB_Read_CP_Ok = false;

                LogToFile("dbCaricaDatidaOrdini : dbException 1/3");
            }

            /*********************************************************************
             *        prima parte Dati 2/2 caricamento da iNumCassaParam
             *********************************************************************/
            if (!bDB_Read_CP_Ok)
            {
                try
                {
                    bDB_Read_CP_Ok = dbCreaTableDati();

                    // iNumCassaParam per SQLite, si prova con CASSA_PRINCIPALE per NDB
                    bDBConnection_Ok = dbInit(dateParam, iNumCassaParam, bSilentParam, sNomeTabellaParam);

                    cmd_Dati.Connection = _Connection;

                    cmd_Dati.CommandText = "SELECT * FROM " + _sDBTNameDati + " ORDER BY iRiga_ID ASC";

                    if (bDBConnection_Ok && bDB_Read_CP_Ok)
                    {
                        readerDati = cmd_Dati.ExecuteReader();
                    }
                    else
                    {
                        return -1;
                    }
                }
                catch (Exception)
                {
                    LogToFile("dbCaricaDatidaOrdini : dbException 2/2");
                    return -1;
                }
            }

            try
            {
                if (readerDati != null)
                {
                    bNoProblem = true;

                    // serve leggere questi dati solo per le stampe in altra data
                    sTmp = "-13";
                    if (readerDati.Read())
                        DB_Data.sVersione = readerDati.GetString("sText");
                    else
                        bNoProblem = false;

                    sTmp = "-12";
                    if (readerDati.Read())
                        DB_Data.iNumCassa = readerDati.GetInt32("iQuantita_Venduta");
                    else
                        bNoProblem = false;

                    sTmp = "-11";
                    if (readerDati.Read())
                        DB_Data.sDateTime = readerDati.GetString("sText");
                    else
                        bNoProblem = false;

                    sTmp = "-10";
                    if (readerDati.Read())
                        DB_Data.sHeaders[0] = readerDati.GetString("sText");
                    else
                        bNoProblem = false;

                    sTmp = "-9";
                    if (readerDati.Read())
                        DB_Data.sHeaders[1] = readerDati.GetString("sText");
                    else
                        bNoProblem = false;

                    // sTmp = "-8"; *** non serve viene ricalcolato dopo ***
                    readerDati.Read(); // 8
                    readerDati.Read(); // 7
                    readerDati.Read(); // 6
                    readerDati.Read(); // 5
                    readerDati.Read(); // 4
                    readerDati.Read(); // 3
                    readerDati.Read(); // 2
                    readerDati.Read(); // 1

                    for (i = 0; (i < MAX_NUM_ARTICOLI) && bNoProblem; i++)
                    {
                        if (readerDati.Read())
                        {
                            sTmp = readerDati.GetString("sTipo_Articolo");

                            if (sTmp.Contains(SHMAGIC)) // MAGIC
                                DB_Data.Articolo[i].sTipo = "";
                            else
                                DB_Data.Articolo[i].sTipo = sTmp;

                            if (!string.IsNullOrEmpty(sTmp) && (sTmp.Length > MAX_LEG_ART_CHAR))
                                _iDBArticoliLength_Is33 = true;

                            DB_Data.Articolo[i].iGruppoStampa = readerDati.GetInt32("iGruppo_Stampa");
                            DB_Data.Articolo[i].iPrezzoUnitario = readerDati.GetInt32("iPrezzo_Unitario");

                            sTmp = readerDati.GetString("sDisponibilita");
                            if (sTmp == "OK")
                                DB_Data.Articolo[i].iDisponibilita = DISP_OK;
                            else
                                DB_Data.Articolo[i].iDisponibilita = Convert.ToInt32(sTmp);
                        }
                        else
                        {
                            bNoProblem = false;
                            break;
                        }
                    } // end for

                    if (!bNoProblem) // c'è qualche problema
                    {
                        _WrnMsg.iErrID = WRN_RNF;
                        _WrnMsg.sMsg = sTmp;
                        WarningManager(_WrnMsg);
                        LogToFile("dbCaricaDatidaOrdini : record" + _WrnMsg.sMsg + "non trovato !");
                    }

                    sDebug = DB_Data.Articolo[0].sTipo;

                    LogToFile("dbCaricaDatidaOrdini : carico eseguito D2");
                }
            }

            catch (Exception)
            {
                _WrnMsg.iErrID = WRN_DBE;
                _WrnMsg.sMsg = String.Format("dbCaricaDatidaOrdini : {0}", bUSA_NDB());
                WarningManager(_WrnMsg);
                LogToFile("dbCaricaDatidaOrdini : dbException");

                readerDati?.Close();
                return -1;
            }

            readerDati?.Close();

            /*********************************************************************
             *  seconda parte: iNumOfLastReceipt, iNumOfMessages
             *                 iStartingNumOfReceipts, iActualNumOfReceipts
             *********************************************************************/

            try
            {
                // se prima c'erano i dati ci devono essere anche gli ordini
                // funziona perchè c'è sempre iOrdine_ID == 0

                cmd_Ordini.Connection = _Connection;

                // DB_Data.iNumOfMessages
                cmd_Ordini.CommandText = "SELECT iOrdine_ID FROM " + _sDBTNameOrdini + " ORDER BY iOrdine_ID ASC LIMIT 1";
                try
                {
                    readerOrdine = cmd_Ordini.ExecuteReader();

                    if ((readerOrdine != null) && readerOrdine.HasRows)
                    {
                        readerOrdine.Read();

                        DB_Data.iNumOfMessages = -readerOrdine.GetInt32("iOrdine_ID");
                        iDebug = DB_Data.iNumOfMessages;

                        readerOrdine.Close();
                    }
                    else
                        DB_Data.iNumOfMessages = 0; // così non si blocca la lettura di iNum
                }
                catch (Exception)
                {
                    DB_Data.iNumOfMessages = 0;
                }

                // DB_Data.iNumOfLastReceipt
                cmd_Ordini.CommandText = "SELECT iOrdine_ID FROM " + _sDBTNameOrdini + " ORDER BY iOrdine_ID DESC LIMIT 1";
                try
                {
                    readerOrdine = cmd_Ordini.ExecuteReader();

                    if ((readerOrdine != null) && readerOrdine.HasRows)
                    {
                        readerOrdine.Read();

                        DB_Data.iNumOfLastReceipt = readerOrdine.GetInt32("iOrdine_ID");
                        iDebug = DB_Data.iNumOfLastReceipt;

                        readerOrdine.Close();
                    }
                    else
                        DB_Data.iNumOfLastReceipt = 0; // così non si blocca la lettura di iNum
                }
                catch (Exception)
                {
                    DB_Data.iNumOfLastReceipt = -1;
                }

                // DB_Data.iStartNumOfReceipts
                cmd_Ordini.CommandText = "SELECT iOrdine_ID FROM " + _sDBTNameOrdini + " WHERE (iOrdine_ID > 0) ORDER BY iOrdine_ID ASC LIMIT 1";
                try
                {
                    readerOrdine = cmd_Ordini.ExecuteReader();

                    if ((readerOrdine != null) && readerOrdine.HasRows)
                    {
                        readerOrdine.Read();

                        DB_Data.iStartingNumOfReceipts = readerOrdine.GetInt32("iOrdine_ID");
                        iDebug = DB_Data.iStartingNumOfReceipts;

                        readerOrdine.Close();
                    }
                    else
                        DB_Data.iStartingNumOfReceipts = 0; // così non si blocca la lettura di iNum
                }
                catch (Exception)
                {
                    DB_Data.iStartingNumOfReceipts = 0;
                }

                // DB_Data.iActualNumOfReceipts
                cmd_Ordini.CommandText = "SELECT iOrdine_ID FROM " + _sDBTNameOrdini + " WHERE (sTipo_Articolo = '" + ORDER_CONST._START_OF_ORDER + "')";
                try
                {
                    readerOrdine = cmd_Ordini.ExecuteReader();

                    if ((readerOrdine != null) && readerOrdine.HasRows)
                    {
                        int iCount = 0;
                        while (readerOrdine.Read())
                        {
                            iCount++;
                        }

                        DB_Data.iActualNumOfReceipts = iCount;

                        readerOrdine.Close();
                    }
                    else
                        DB_Data.iActualNumOfReceipts = 0; // così non si blocca la lettura di iNum
                }
                catch (Exception)
                {
                    DB_Data.iActualNumOfReceipts = 0;
                }

                /*********************************************************************
                 *                       terza parte Ordini
                 *********************************************************************/

                _WrnMsg.sMsg = "";
                bSingleWarn = false;

                for (j = 1; j <= DB_Data.iNumOfLastReceipt; j++)
                {
                    readerOrdine?.Close();

                    // filtraggio più o meno specifico
                    if (iNumCassaParam == 0)
                        sTmp = String.Format(" WHERE (iOrdine_ID = {0})", j);
                    else
                        sTmp = String.Format(" WHERE (iOrdine_ID = {0}) AND (iNumCassa = {1})", j, iNumCassaParam);

                    cmd_Ordini.CommandText = "SELECT * FROM " + _sDBTNameOrdini + sTmp;

                    readerOrdine = cmd_Ordini.ExecuteReader();

                    // se il record non c'è ritorna, emissione da altra cassa
                    if (!readerOrdine.HasRows)
                        continue;

                    iStatus = 0;
                    bScaricato = false;

                    // non si riferiscono a nessun ordine in particolare
                    iPrezzoUnitario = 0;
                    iQuantitaOrdine = 0;
                    iGruppoStampa = 0;
                    iTotaleReceipt = 0;
                    iStatusScontoReceipt = 0;
                    iScontoStdReceipt = 0;
                    iScontoFissoReceipt = 0;
                    iScontoGratisReceipt = 0;
                    iBuoniApplicatiReceipt = 0;

                    try
                    {
                        while (readerOrdine.Read())
                        {
                            sTipo = readerOrdine.GetString("sTipo_Articolo");
                            bRigaAnnullata = readerOrdine.GetBoolean("iAnnullato");

                            if (sTipo == ORDER_CONST._START_OF_ORDER)
                            {
                                iStatus = readerOrdine.GetInt32("iStatus");
                                bScaricato = readerOrdine.GetBoolean("iScaricato");

#if STANDFACILE || STAND_MONITOR
                                // prosegue solo se è stato effettuato un certo tipo di pagamento
                                // deve stare prima dei vari DB_Data.iTotaleBuoniApplicati +=
                                if ((VisDatiDlg.PaymentReportIsRequested()) && !IsBitSet(iStatus, VisDatiDlg.GetPaymentReportBit()))
                                {
                                    break;
                                }
#endif
                                if (IsBitSet(iStatus, (int)STATUS_FLAGS.BIT_CARICATO_DA_WEB))
                                    DB_Data.iNumOfWebReceipts++;

                                DB_Data.bAnnullato = readerOrdine.GetBoolean("iAnnullato");

                                if (DB_Data.bAnnullato)
                                    DB_Data.iNumAnnullati++;
                            }
                            else if (sTipo == ORDER_CONST._SCONTO)
                            {
                                iStatusScontoReceipt = readerOrdine.GetInt32("iStatus");
                                iPrezzoUnitario = readerOrdine.GetInt32("iPrezzo_Unitario");
                            }
                            else if (sTipo == ORDER_CONST._BUONI)
                            {
                                iBuoniApplicatiReceipt = readerOrdine.GetInt32("iPrezzo_Unitario");

                                if (bRigaAnnullata)
                                    // come per gli sconti i Buoni Applicati potrebbero essere parziali
                                    DB_Data.iTotaleAnnullato -= iBuoniApplicatiReceipt;
                                else
                                    DB_Data.iTotaleBuoniApplicati += iBuoniApplicatiReceipt;
                            }
                            else
                            {
                                iPrezzoUnitario = readerOrdine.GetInt32("iPrezzo_Unitario");
                                iQuantitaOrdine = readerOrdine.GetInt32("iQuantita_Ordine");
                                iGruppoStampa = readerOrdine.GetInt32("iGruppo_Stampa");
                            }

                            if (StringBelongsTo_ORDER_CONST(sTipo, ORDER_CONST._SCONTO))
                                continue;

                            /************************************
                             *     controllo di sicurezza
                             ************************************/
                            bMatch = false;

                            for (i = 0; i < iLastArticoloDBIndexP1; i++)
                            {
                                if (String.IsNullOrEmpty(DB_Data.Articolo[i].sTipo))
                                    continue;
                                else
                                {
                                    bMatch = false;

                                    sDebug = DB_Data.Articolo[0].sTipo;

                                    if ((DB_Data.Articolo[i].sTipo == sTipo) || (sTipo == ORDER_CONST._SCONTO))
                                    {
                                        if (bRigaAnnullata)
                                        {
                                            if (DB_Data.Articolo[i].iGruppoStampa != (int)DEST_TYPE.DEST_BUONI)
                                                DB_Data.iTotaleAnnullato += iPrezzoUnitario * iQuantitaOrdine;
                                        }
                                        else
                                        {
                                            if (IsBitSet(iStatusScontoReceipt, BIT_SCONTO_STD) && (sTipo == ORDER_CONST._SCONTO))
                                            {
                                                DB_Data.iTotaleScontatoStd += iPrezzoUnitario;
                                                iScontoStdReceipt = iPrezzoUnitario;
                                            }
                                            else if (IsBitSet(iStatusScontoReceipt, BIT_SCONTO_FISSO) && (sTipo == ORDER_CONST._SCONTO))
                                            {
                                                DB_Data.iTotaleScontatoFisso += iPrezzoUnitario;
                                                iScontoFissoReceipt = iPrezzoUnitario;
                                            }
                                            else if (IsBitSet(iStatusScontoReceipt, BIT_SCONTO_GRATIS) && (sTipo == ORDER_CONST._SCONTO))
                                            {
                                                DB_Data.iTotaleScontatoGratis += iPrezzoUnitario;
                                                iScontoGratisReceipt = iPrezzoUnitario;
                                            }
                                            else
                                            {
#if STANDFACILE || STAND_MONITOR
                                                // considera solo gli sconti
                                                if (VisDatiDlg.DiscountReportIsRequested() && !IsBitSet(iStatusScontoReceipt, VisDatiDlg.GetDiscountReportBit()))
                                                {
                                                    bMatch = true;
                                                    break;
                                                }

                                                // considera solo i gruppi cui lo sconto è applicato
                                                if (VisDatiDlg.DiscountReportIsRequested() && !IsBitSet(iStatusScontoReceipt, DB_Data.Articolo[i].iGruppoStampa + 4) &&
                                                    (VisDatiDlg.GetDiscountReportBit() == BIT_SCONTO_STD))
                                                {
                                                    bMatch = true;
                                                    break;
                                                }
#endif
                                                DB_Data.Articolo[i].iQuantitaVenduta += iQuantitaOrdine;

                                                if (DB_Data.Articolo[i].iGruppoStampa != (int)DEST_TYPE.DEST_BUONI)
                                                {
                                                    iTotaleReceipt += iPrezzoUnitario * iQuantitaOrdine;
                                                    DB_Data.iTotaleIncasso += iPrezzoUnitario * iQuantitaOrdine;
                                                }
                                            }

                                            if (IsBitSet(iStatus, (int)STATUS_FLAGS.BIT_ASPORTO))
                                                DB_Data.Articolo[i].iQtaEsportata += iQuantitaOrdine;

                                            if (bScaricato)
                                                DB_Data.Articolo[i].iQuantita_Scaricata += iQuantitaOrdine;
                                        }

                                        bMatch = true;
                                        break;
                                    } //
                                } // end if
                            } // end for i

                            // aggiunge alla fine l'Articolo altrimenti non trovato
                            // sTipo == _SCONTO non passa mai di qua, quindi non serve aggiornare 
                            // DB_Data.iTotaleScontatoStd, DB_Data.iTotaleScontatoFisso, DB_Data.iTotaleScontatoGratis
                            if (!bMatch)
                            {
                                DB_Data.Articolo[iLastArticoloDBIndexP1].sTipo = sTipo;
                                DB_Data.Articolo[iLastArticoloDBIndexP1].iPrezzoUnitario = iPrezzoUnitario;
                                DB_Data.Articolo[iLastArticoloDBIndexP1].iGruppoStampa = iGruppoStampa;

                                if (DB_Data.bAnnullato)
                                {
                                    if (DB_Data.Articolo[i].iGruppoStampa != (int)DEST_TYPE.DEST_BUONI)
                                        DB_Data.iTotaleAnnullato += iPrezzoUnitario * iQuantitaOrdine;
                                }
#if STANDFACILE || STAND_MONITOR
                                else if (VisDatiDlg.DiscountReportIsRequested() && !IsBitSet(iStatusScontoReceipt, VisDatiDlg.GetDiscountReportBit()))
                                {
                                    bMatch = true;
                                }
#endif
                                else
                                {
                                    DB_Data.Articolo[iLastArticoloDBIndexP1].iQuantitaVenduta += iQuantitaOrdine;
                                    DB_Data.iTotaleIncasso += iPrezzoUnitario * iQuantitaOrdine;
                                    iTotaleReceipt += iPrezzoUnitario * iQuantitaOrdine;

                                    if (IsBitSet(iStatus, (int)STATUS_FLAGS.BIT_ASPORTO))
                                        DB_Data.Articolo[iLastArticoloDBIndexP1].iQtaEsportata += iQuantitaOrdine;

                                    if (bScaricato)
                                        DB_Data.Articolo[iLastArticoloDBIndexP1].iQuantita_Scaricata += iQuantitaOrdine;
                                }

                                if (iLastArticoloDBIndexP1 < (MAX_NUM_ARTICOLI + EXTRA_NUM_ARTICOLI - 1))
                                {
                                    iLastArticoloDBIndexP1++; // può arrivare a (MAX_NUM_ARTICOLI + EXTRA_NUM_ARTICOLI - 1) per non sforare
                                                              // bMatch = true;
                                }
                                else
                                {
                                    _WrnMsg.sMsg = String.Format("{0}", sTipo);
                                    _WrnMsg.iErrID = WRN_NVD;
                                    WarningManager(_WrnMsg);
                                }
                            }

                            // dà un solo avviso alla prima discordanza
                            if (!bMatch && !bSingleWarn)
                            {
                                bSingleWarn = true;

                                if ((dateParam.ToString("dd/MM/yy") == GetActualDate().ToString("dd/MM/yy")))
                                {
                                    _WrnMsg.sMsg = sTipo;
                                    _WrnMsg.iErrID = WRN_RNF;
                                    //WarningManager(_WrnMsg); se si abilita si ha eccezione !
                                }

                                sTmp = String.Format("dbCaricaDatidaOrdini : order_ID = {0} non esiste!", sTipo);
                                LogToFile(sTmp);
                            }

                        } // end while (readerOrdine.Read())

                        if (IsBitSet(iStatus, (int)STATUS_FLAGS.BIT_PAGAM_CARD))
                        {
                            DB_Data.iTotaleIncassoCard += iTotaleReceipt - iScontoStdReceipt - iScontoFissoReceipt - iScontoGratisReceipt;
                            iDebug = DB_Data.iTotaleIncassoCard;
                        }

                        if (IsBitSet(iStatus, (int)STATUS_FLAGS.BIT_PAGAM_SATISPAY))
                        {
                            DB_Data.iTotaleIncassoSatispay += iTotaleReceipt - iScontoStdReceipt - iScontoFissoReceipt - iScontoGratisReceipt;
                            iDebug = DB_Data.iTotaleIncassoSatispay;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("dbCaricaDatidaOrdini Eccezione : {0}", e.Message);
                    }
                    finally
                    {
                        // always call Close when done reading.
                        readerOrdine.Close();
                    }
                } // end for J

                if ((_WrnMsg.iErrID == WRN_RNF) && !bSilentParam)
                    WarningManager(_WrnMsg);

                LogToFile("dbCaricaDatidaOrdini : carico eseguito 3");
            }

            catch (Exception)
            {
                _WrnMsg.iErrID = WRN_DBE;
                _WrnMsg.sMsg = String.Format("dbCaricaDatidaOrdini : {0}", bUSA_NDB());
                WarningManager(_WrnMsg);

                LogToFile("dbCaricaDatidaOrdini : dbException");

                readerOrdine?.Close();

                return -1;
            }

            //for (i = 0; i < MAX_NUM_ARTICOLI; i++)
            //    if (DB_Data.Articolo[i].iQuantita_Scaricata > 0)
            //        Console.WriteLine("dbCaricaDatidaOrdini : Tipo = {0}, Qta Scar.{1}", DB_Data.Articolo[i].sTipo, DB_Data.Articolo[i].iQuantita_Scaricata);

            return DB_Data.iNumOfLastReceipt; // tutto OK
        } // end dbCaricaDatidaOrdini()

        /// <summary>
        /// la CASSA_PRINCIPALE può caricare la disponibilità della sessione precedente, <br/>
        /// la CASSA_SECONDARIA carica i Dati di Disponibilità in DB_Data, <br/>
        /// ritorna true se ha successo <br/>
        /// </summary>
        public bool dbCaricaDisponibilità(DateTime dateParam)
        {
            bool bDBConnection_Ok;
            int i;
            String sTmp;

            MySqlCommand cmd = new MySqlCommand();
            MySqlDataReader readerDisp = null;

            bDBConnection_Ok = dbInit(dateParam, CASSA_PRINCIPALE);

            // sicurezza : si prosegue solo se c'è la connessione a DB
            if (!bDBConnection_Ok)
                return false;

            cmd.Connection = _Connection;
            cmd.CommandText = "SELECT * FROM " + _sDBTNameDati + " WHERE (iRiga_ID >= 0) ORDER BY iRiga_ID ASC";

            dbAzzeraDatiGen();
            dbAzzeraDatiOrdine(ref DB_Data);

            try
            {
                readerDisp = cmd.ExecuteReader();
            }
            catch (Exception)
            {
                LogToFile("dbCaricaDisponibilità : dbException Open()");

                readerDisp?.Close();

                return false;
            }

            try
            {
                if (readerDisp != null)
                {
                    while (readerDisp.Read())
                    {
                        i = readerDisp.GetInt32("iRiga_ID");

                        if ((i < 0) || (i >= MAX_NUM_ARTICOLI))
                            continue;

                        sTmp = readerDisp.GetString("sTipo_Articolo");

                        if (sTmp.Contains(SHMAGIC)) // MAGIC
                            DB_Data.Articolo[i].sTipo = "";
                        else
                            DB_Data.Articolo[i].sTipo = sTmp;

                        // serve a InitDispDlg che ripristina in base al Gruppo_Stampa
                        DB_Data.Articolo[i].iGruppoStampa = readerDisp.GetInt32("iGruppo_Stampa");

                        sTmp = readerDisp.GetString("sDisponibilita");
                        if (sTmp == "OK")
                            DB_Data.Articolo[i].iDisponibilita = DISP_OK;
                        else
                            DB_Data.Articolo[i].iDisponibilita = Convert.ToInt32(sTmp);
                    } // end while

                    LogToFile("dbCaricaDisponibilità : carico eseguito");
                }
            }

            catch (Exception)
            {
                _WrnMsg.iErrID = WRN_DBE;
                _WrnMsg.sMsg = String.Format("dbCaricaDisponibilità : {0}", bUSA_NDB());
                WarningManager(_WrnMsg);
                LogToFile("dbCaricaDisponibilità : dbException");
            }

            readerDisp?.Close();

            return true; // tutto OK
        } // end dbCaricaDisponibilità

        /// <summary>
        /// carica l'ordine iParam nella variabile DB_Articolo[], <br/>
        /// se però iParam == 0 carica _Versione, _Header, _HeaderText <br/> <br/>
        /// 
        /// ritorna true se ha successo, <br/>
        /// utilizzata da FrmVisOrdiniDlg, DataManager.AggiornaDisponibilità
        /// </summary>
        public bool dbCaricaOrdine(DateTime dateParam, int iParam, bool bFiltraPerCassa, String sNomeTabellaParam = "")
        {
            bool bTrovato, bNoProblem = true;
            int i, iStatus, iCount, iDebug;

            String sInStr, sFilterText, sTmp;

            MySqlCommand cmd = new MySqlCommand();
            MySqlDataReader readerOrdine = null;

            dbInit(dateParam, SF_Data.iNumCassa, false, sNomeTabellaParam);

            // dbAzzeraDatiGen() va più sotto
            dbAzzeraDatiOrdine(ref DB_Data);

#if !STAND_ORDINI
            _iDBArticoliLength_Is33 = IsBitSet(DB_Data.iGenericPrintOptions, (int)GEN_PRINTER_OPTS.BIT_CHARS33_PRINT_REQUIRED);
#endif
            try
            {
                cmd.Connection = _Connection;

                _WrnMsg.sMsg = iParam.ToString();

                /******************************************************************
                 * 	carica DB_Data.sVersione, DB_Data.sHeaders[i], 
                 * 	DB_Data.sCopiesGroupsText[i], DB_Data.bCopiesGroupsFlag[i]
                 ******************************************************************/
                if (iParam == 0)
                {
                    dbAzzeraDatiGen();

                    cmd.CommandText = "SELECT * FROM " + _sDBTNameOrdini + " WHERE iOrdine_ID = 0";

                    readerOrdine = cmd.ExecuteReader();

                    while (readerOrdine.Read())
                    {
                        bTrovato = false;

                        // serve leggere questi dati solo per le stampe in altra data
                        sInStr = readerOrdine.GetString("sTipo_Articolo");

                        if (sInStr == "_Versione")
                        {
                            DB_Data.sVersione = readerOrdine.GetString("sText");
                            continue;
                        }

                        else if (sInStr == "_BCD_Settings")
                        {
                            sTmp = readerOrdine.GetString("sText");
                            sTmp = sTmp.Substring(3);
                            i = ToInt32(sTmp, 16);

                            DB_Data.iBarcodeRichiesto = i;
                            continue;
                        }

                        else if (sInStr == "_GenProgOptions")
                        {
                            sTmp = readerOrdine.GetString("sText");
                            sTmp = sTmp.Substring(3);
                            i = ToInt32(sTmp, 16);

                            DB_Data.iGeneralProgOptions = i;
                            continue;
                        }

                        else if (sInStr == "_GenPrintSettings")
                        {
                            sTmp = readerOrdine.GetString("sText");
                            sTmp = sTmp.Substring(3);
                            i = ToInt32(sTmp, 16);

                            DB_Data.iGenericPrintOptions = i;
                            continue;
                        }

                        else if (sInStr == "_LocCopySettings")
                        {
                            sTmp = readerOrdine.GetString("sText");
                            sTmp = sTmp.Substring(3);
                            i = ToInt32(sTmp, 16);

                            DB_Data.iLocalCopyOptions = i;
                            continue;
                        }

                        for (i = 0; i < MAX_NUM_HEADERS; i++)
                        {
                            sTmp = String.Format("_Header_{0}", i);
                            if (sInStr == sTmp)
                            {
                                DB_Data.sHeaders[i] = readerOrdine.GetString("sText");
                                bTrovato = true;
                                break;
                            }
                        }

                        if (bTrovato)
                            continue;

                        // ****	Copie/Text and Flag ****
                        for (i = 0; i < NUM_EDIT_GROUPS; i++)
                        {
                            sTmp = String.Format("_GroupText_{0}", i);
                            if (sInStr == sTmp)
                            {
                                DB_Data.sCopiesGroupsText[i] = readerOrdine.GetString("sText");

                                iStatus = readerOrdine.GetInt32("iStatus");

                                if (iStatus >= 10)
                                    DB_Data.bCopiesGroupsFlag[i] = true;
                                else
                                    DB_Data.bCopiesGroupsFlag[i] = false;

                                DB_Data.iGroupsColor[i] = iStatus % 10;

                                bTrovato = true;
                                break;
                            }
                        }

                        if (bTrovato)
                            continue;

                        // ****	Colors/Text ****
                        for (i = 0; i < NUM_GROUPS_COLORS - 1; i++)
                        {
                            sTmp = String.Format("_ColorText_{0}", i);
                            if (sInStr == sTmp)
                            {
                                DB_Data.sColorGroupsText[i] = readerOrdine.GetString("sText");

                                bTrovato = true;
                                break;
                            }
                        }

                        if (bTrovato)
                            continue;

                        // se il codice arriva qui vuole dire che i loop precedenti non hanno trovato un item
                        bNoProblem = false;
                    }
                }
                else // carica l'ordine vero e proprio
                {
                    // filtraggio più o meno specifico
                    if (bFiltraPerCassa)
                        sFilterText = String.Format(" WHERE (iOrdine_ID = {0}) AND (iNumCassa = {1})", iParam, SF_Data.iNumCassa);
                    else
                        sFilterText = String.Format(" WHERE (iOrdine_ID = {0})", iParam);

                    cmd.CommandText = "SELECT * FROM " + _sDBTNameOrdini + sFilterText;

                    readerOrdine = cmd.ExecuteReader();

                    // se il record non c'è ritorna
                    if (!readerOrdine.HasRows)
                    {
                        _WrnMsg.sMsg = iParam.ToString();
                        return false;
                    }

                    iCount = 0;

                    while (readerOrdine.Read() && bNoProblem)
                    {
                        sInStr = readerOrdine.GetString("sTipo_Articolo");

                        // Start Of Order
                        if (sInStr == ORDER_CONST._START_OF_ORDER)
                        {
                            DB_Data.iStatusReceipt = readerOrdine.GetInt32("iStatus");
                            DB_Data.sDateTime = readerOrdine.GetString("sText");
                            DB_Data.iNumCassa = readerOrdine.GetInt32("iNumCassa");
                            DB_Data.bAnnullato = readerOrdine.GetBoolean("iAnnullato");
                            DB_Data.bScaricato = readerOrdine.GetBoolean("iScaricato");
                            continue;
                        }

                        // Tavolo
                        else if (sInStr == ORDER_CONST._TAVOLO)
                        {
                            DB_Data.sTavolo = readerOrdine.GetString("sText");
                            continue;
                        }

                        // Nome
                        else if (sInStr == ORDER_CONST._NOME)
                        {
                            DB_Data.sNome = readerOrdine.GetString("sText");
                            continue;
                        }

                        // Nota
                        else if (sInStr == ORDER_CONST._NOTA)
                        {
                            DB_Data.sNota = readerOrdine.GetString("sText");
                            continue;
                        }

                        // Sconto
                        else if (sInStr == ORDER_CONST._SCONTO)
                        {
                            DB_Data.iStatusSconto = readerOrdine.GetInt32("iStatus");
                            DB_Data.sScontoText = readerOrdine.GetString("sText");

                            if (IsBitSet(DB_Data.iStatusSconto, BIT_SCONTO_STD))
                                DB_Data.iScontoStdReceipt = readerOrdine.GetInt32("iPrezzo_Unitario");
                            else if (IsBitSet(DB_Data.iStatusSconto, BIT_SCONTO_FISSO))
                                DB_Data.iScontoFissoReceipt = readerOrdine.GetInt32("iPrezzo_Unitario");
                            else if (IsBitSet(DB_Data.iStatusSconto, BIT_SCONTO_GRATIS))
                                DB_Data.iScontoGratisReceipt = readerOrdine.GetInt32("iPrezzo_Unitario");

                            continue;
                        }

                        // numero Ordine Web
                        else if (sInStr == ORDER_CONST._NUM_ORD_WEB)
                        {
                            DB_Data.iNumOrdineWeb = readerOrdine.GetInt32("iPrezzo_Unitario");
                            DB_Data.sWebDateTime = readerOrdine.GetString("sText");
                            String sDebug = DB_Data.sWebDateTime;

                            continue;
                        }

                        // numero Ordine in Prevendita
                        else if (sInStr == ORDER_CONST._NUM_ORD_PREV)
                        {
                            DB_Data.iNumOrdinePrev = readerOrdine.GetInt32("iPrezzo_Unitario");
                            DB_Data.sPrevDateTime = readerOrdine.GetString("sText");

                            continue;
                        }
                        else
                        {
                            DB_Data.Articolo[iCount].sTipo = readerOrdine.GetString("sTipo_Articolo");
                            DB_Data.Articolo[iCount].sNotaArt = readerOrdine.GetString("sText");
                            DB_Data.Articolo[iCount].iPrezzoUnitario = readerOrdine.GetInt32("iPrezzo_Unitario");
                            DB_Data.Articolo[iCount].iQuantitaOrdine = readerOrdine.GetInt32("iQuantita_Ordine");
                            DB_Data.Articolo[iCount].iIndexListino = readerOrdine.GetInt32("iIndex_Listino");
                            DB_Data.Articolo[iCount].iGruppoStampa = readerOrdine.GetInt32("iGruppo_Stampa");
                            DB_Data.Articolo[iCount].iOptionsFlags = readerOrdine.GetInt32("iStatus");

                            sTmp = readerOrdine.GetString("sTipo_Articolo");
                            if (!string.IsNullOrEmpty(sTmp) && (sTmp.Length > MAX_LEG_ART_CHAR))
                                _iDBArticoliLength_Is33 = true;

                            iCount++;
                        }
                    }
                }

                iDebug = DB_Data.iStatusReceipt;

                if (!bNoProblem)
                {
                    _WrnMsg.iErrID = WRN_RNF;
                    WarningManager(_WrnMsg);
                    LogToFile("dbCaricaOrdine : record " + _WrnMsg.sMsg + " non trovato !");
                }

                LogToFile("dbCaricaOrdine : carico eseguito");
            }

            catch (Exception)
            {
#if !STAND_CUCINA
                _WrnMsg.iErrID = WRN_DBE;
                _WrnMsg.sMsg = String.Format("dbCaricaOrdine : {0} {1}", bUSA_NDB(), iParam);
                WarningManager(_WrnMsg);
#endif
                LogToFile("dbCaricaOrdine : dbException");

                bNoProblem = false;
            }

            readerOrdine?.Close();

            return bNoProblem; // tutto OK
        } // end dbCaricaOrdine

        /// <summary>
        /// restituisce il messaggio nella variabile DB_Data.sMessaggio, <br/>
        /// ritorna true se ha successo, utilizzata da VisMessaggi
        /// </summary>
        public bool dbCaricaMessaggio(int iParam, bool bTutteCassaParam)
        {
            bool bDBConnection_Ok;
            String sFilterText;

            MySqlCommand cmd = new MySqlCommand();
            MySqlDataReader readerMessaggio;

            DateTime dateParam = GetActualDate();

            bDBConnection_Ok = dbInit(dateParam, SF_Data.iNumCassa);

            // sicurezza : si prosegue solo se c'è la connessione a DB
            if (!bDBConnection_Ok)
                return false;

            dbAzzeraDatiOrdine(ref DB_Data);

            try
            {
                // filtraggio più o meno specifico
                if (bTutteCassaParam)
                    sFilterText = String.Format(" WHERE (iOrdine_ID = {0})", -iParam); // Attenzione al "-" ***************
                else
                    sFilterText = String.Format(" WHERE (iOrdine_ID = {0}) AND (iNumCassa = {1})", -iParam, SF_Data.iNumCassa); // Attenzione al "-"

                cmd.Connection = _Connection;
                cmd.CommandText = "SELECT * FROM " + _sDBTNameOrdini + sFilterText;

                if (bUSA_NDB())
                    readerMessaggio = cmd.ExecuteReader();
                else
                    return false;

                if (readerMessaggio.Read())
                {
                    DB_Data.sMessaggio = readerMessaggio.GetString("sText");
                    DB_Data.iNumCassa = readerMessaggio.GetInt32("iNumCassa");
                    DB_Data.iStatusReceipt = readerMessaggio.GetInt32("iStatus");

                    return true;
                }
                else
                {
                    DB_Data.sMessaggio = String.Format("Messaggio Num. : {0} non esiste !", iParam);

                    return false;
                }
            }

            catch (Exception)
            {
#if !STAND_CUCINA
                _WrnMsg.iErrID = WRN_DBE;
                _WrnMsg.sMsg = String.Format("dbCaricaMessaggio : {0}", bUSA_NDB());
                WarningManager(_WrnMsg);
#endif
                LogToFile("dbCaricaMessaggio : dbException");

                return false;
            }
        }

    }
}

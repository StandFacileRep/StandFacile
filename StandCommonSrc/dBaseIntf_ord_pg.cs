/*************************************************************************************************
	NomeFile : StandCommonSrc/dBaseIntf_ord_pg.cs
    Data	 : 06.12.2024
	Autore   : Mauro Artuso

    nelle assegnazioni :
    DB_Data compare sempre a sx,
    SF_Data compare sempre a dx

    ANSI SQL uses double quotes only for SQL identifiers, single quotes are used for literals (stringhe).
    PostGreSQL non utilizza UNSIGNED 

    PGSQL è case sensitive quindi per gestire le maiuscole bisogna includere le stringhe tra ""
    

    Attenzione : dbInit(dateParam) deve essere invocata all'inizio di ogni funzione
 *************************************************************************************************/

using System;
using System.Data;

using Devart.Data.PostgreSql;

using static StandCommonFiles.ComDef;
using static StandCommonFiles.CommonCl;
using static StandCommonFiles.LogServer;

using StandFacile;
using static StandFacile.glb;
using static StandFacile.dBaseIntf;

namespace StandFacile_DB
{
#pragma warning disable IDE0059
#pragma warning disable IDE1006

    /// <summary>classe per la gestione di PostGreSQL</summary>
    public partial class dBaseIntf_pg
    {

        /// <summary>
        /// scarica uno scontrino dal database,<br/>
        /// se iGruppo è negativo allora si ignora il gruppo di stampa
        /// </summary>
        public bool dbScaricaOrdine(int iOrderID, int iGruppo)
        {
            bool bResult;
            bool bScaricato, bAnnullato;

            int iCountOrdini, iContDati;
            int i, iQuantitaReceipt, iQuantitaScaricata, iNumCassa;
            int iScaricato, iUpdatedRowsOrdini, iUpdatedRowsDati;
            String sFilter, sTmpFilter, sTipo, sTmp, sQuery;

            PgSqlDataAdapter dbOrdiniAdapter = null;
            PgSqlDataAdapter dbOrdiniAdapterSelect = null;

            PgSqlDataAdapter dbDataAdapter = null;
            PgSqlDataAdapter dbDataAdapterSelect = null;

            DataTable ordiniTable = new DataTable();
            DataTable dataTable = new DataTable();
            DataRow dataRow;

            PgSqlTransaction transaction = null;

            iNumCassa = 0; // partono da 1 !!!

            bAnnullato = true;
            bResult = false;
            iCountOrdini = -1;

            dbInit(GetActualDate(), CASSA_PRINCIPALE);

            /****************************************************
                    costruzione filtro per scarico ordini
                    esteso ai gruppi dello stesso colore
             ****************************************************/

            try
            {
                if (iGruppo >= 0) // sparato barcode
                {
                    sTmpFilter = String.Format(@"(""iGruppo_Stampa"" = {0}) OR ", iGruppo);

                    for (i = 0; i < NUM_EDIT_GROUPS; i++)
                    {
                        if ((DB_Data.iGroupsColor[i] == DB_Data.iGroupsColor[iGruppo]) && (DB_Data.iGroupsColor[i] > 0) && (i != iGruppo))
                            sTmpFilter += String.Format(@"(""iGruppo_Stampa"" = {0}) OR ", i);
                    }

                    if (iGruppo == (int)DEST_TYPE.DEST_TIPO1)
                    {
                        // scarica iGruppo, _START_OF_ORDER e COPERTI
                        sFilter = String.Format(@"((""iOrdine_ID"" = {0}) AND ({1} (""sTipo_Articolo"" = '{2}') OR
                            ((""iGruppo_Stampa"" = {3}) AND (""sTipo_Articolo"" = 'COPERTI')) ))",
                            iOrderID, sTmpFilter, ORDER_CONST._START_OF_ORDER, (int)DEST_TYPE.DEST_COUNTER);
                    }
                    else
                    {
                        // scarica solo iGruppo e _START_OF_ORDER
                        sFilter = String.Format(@"((""iOrdine_ID"" = {0}) AND ({1} (""sTipo_Articolo"" = '{2}')))",
                            iOrderID, sTmpFilter, ORDER_CONST._START_OF_ORDER);
                    }
                }
                else
                    sFilter = String.Format("\"iOrdine_ID\" = {0}", iOrderID);

                // debug
                //sFilter = String.Format(@"((""iOrdine_ID"" = {0}) AND ((""iGruppo_Stampa"" = {1}) OR (""sTipo_Articolo"" = '{2}') OR
                //            ((""iGruppo_Stampa"" = {3}) AND (""sTipo_Articolo"" = 'COPERTI')) ))",
                //    iOrderID, 1, ORDER_CONST._START_OF_ORDER, (int) DEST_TYPE.DEST_COUNTER);

                // riempie con tutti i record di ordini da scaricare
                sQuery = "select * FROM " + _sDBTNameOrdini + " WHERE " + sFilter + " ;";

                dbOrdiniAdapterSelect = new PgSqlDataAdapter(sQuery, _Connection);
                iCountOrdini = dbOrdiniAdapterSelect.Fill(ordiniTable);
            }

            catch (Exception)
            {
                _WrnMsg.iErrID = WRN_DBE;
                _WrnMsg.sMsg = "dbScaricaOrdine 1";
                WarningManager(_WrnMsg);

                LogToFile("dbScaricaOrdine : dbException 1");
            }

            if (iCountOrdini >= 0)
            {
                LogToFile("dbScaricaOrdine : iCountOrdini");

                try // *********** 1 *********
                {
                    //ordiniTable.PrimaryKey = new DataColumn[] { ordiniTable.Columns["sTipo_Articolo"] };

                    if (iCountOrdini == 0) // record non presente ma connessione OK
                    {
                        bResult = true;

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
                            bResult = true;

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
                    _WrnMsg.iErrID = WRN_DBE;
                    _WrnMsg.sMsg = "dbScaricaOrdine 2";
                    WarningManager(_WrnMsg);

                    LogToFile("dbScaricaOrdine : dbException 2");
                }
            }
            else
                LogToFile("dbScaricaOrdine : NOT Active");

            // non occorre disconnettere ClientDS_Ordini
            if (GetDB_Connected())
                dbInit(GetActualDate(), iNumCassa);

            if ((iCountOrdini > 0) && !bAnnullato)
            {

                try // *********** 2 **********
                {
                    transaction = _Connection.BeginTransaction();

                    // *********** predispone per aggiornamento colonna scarico ordini ************
                    dbOrdiniAdapter = new PgSqlDataAdapter();

                    sQuery = String.Format(@"UPDATE {0} SET ""iScaricato"" = @iScaricato, ""sScaricato"" = @sScaricato WHERE
                         ""sTipo_Articolo"" = @oldId AND ""iOrdine_ID"" = {1};", _sDBTNameOrdini, iOrderID);

                    dbOrdiniAdapter.UpdateCommand = new PgSqlCommand(sQuery, _Connection);

                    dbOrdiniAdapter.UpdateCommand.Parameters.Add("@iScaricato", PgSqlType.Int, 11, "iScaricato");
                    dbOrdiniAdapter.UpdateCommand.Parameters.Add("@sScaricato", PgSqlType.VarChar, 50, "sScaricato");

                    dbOrdiniAdapter.UpdateCommand.Parameters.Add("@oldId", PgSqlType.VarChar, 50, "sTipo_Articolo").SourceVersion = DataRowVersion.Original;
                    dbOrdiniAdapter.UpdateCommand.UpdatedRowSource = UpdateRowSource.None;

                    // *********** predispone per aggiornamento quantità scaricata dati ************

                    // riempie dataTable con tutte le colonne
                    sQuery = "select * FROM " + _sDBTNameDati + ";";
                    dbDataAdapterSelect = new PgSqlDataAdapter(sQuery, _Connection);
                    iContDati = dbDataAdapterSelect.Fill(dataTable);
                    dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["sTipo_Articolo"] };

                    LogToFile("dbScaricaOrdine : dataTable Fill");

                    dbDataAdapter = new PgSqlDataAdapter();

                    sQuery = "UPDATE " + _sDBTNameDati + @" SET ""iRiga_ID"" = @iRiga_ID, ""sTipo_Articolo"" = @sTipo_Articolo, 
                                ""iQuantita_Scaricata"" = @iQuantita_Scaricata WHERE ""iRiga_ID"" = @oldId;";

                    dbDataAdapter.UpdateCommand = new PgSqlCommand(sQuery, _Connection);

                    dbDataAdapter.UpdateCommand.Parameters.Add("@iRiga_ID", PgSqlType.Int, 11, "iRiga_ID");
                    dbDataAdapter.UpdateCommand.Parameters.Add("@sTipo_Articolo", PgSqlType.VarChar, 50, "sTipo_Articolo");
                    dbDataAdapter.UpdateCommand.Parameters.Add("@iQuantita_Scaricata", PgSqlType.Int, 10, "iQuantita_Scaricata");

                    dbDataAdapter.UpdateCommand.Parameters.Add("@oldId", PgSqlType.Int, 11, "iRiga_ID").SourceVersion = DataRowVersion.Original;
                    dbDataAdapter.UpdateCommand.UpdatedRowSource = UpdateRowSource.None;

                    for (i = 0; i < iCountOrdini; i++)
                    {
                        // init 
                        dataRow = null;
                        iScaricato = 0;
                        iQuantitaReceipt = 0;
                        iQuantitaScaricata = 0;

                        sTipo = Convert.ToString(ordiniTable.Rows[i]["sTipo_Articolo"]);

                        if (!StringBelongsTo_ORDER_CONST(sTipo))
                        {
                            dataRow = dataTable.Rows.Find(sTipo);

                            if (dataRow == null)
                            {
                                sTmp = String.Format("dbScaricaOrdine : {0} non trovato in {1} !", sTipo, _sDBTNameDati);
                                LogToFile(sTmp);
                                // continue scarica lo stesso il record
                            }
                            else
                            {
                                iQuantitaReceipt = Convert.ToInt32(ordiniTable.Rows[i]["iQuantita_Ordine"]);
                                iQuantitaScaricata = Convert.ToInt32(dataRow["iQuantita_Scaricata"]);
                            }
                        }

                        if (ordiniTable.Rows[i]["iScaricato"] != DBNull.Value)
                            iScaricato = Convert.ToInt32(ordiniTable.Rows[i]["iScaricato"]);

                        bScaricato = (iScaricato == 1);

                        // aggiorna solo se non è già stato scaricato !!!
                        if (bScaricato)
                        {
                            // gia scaricato !!!
                            if (sTipo == ORDER_CONST._START_OF_ORDER)
                            {
                                Console.Beep();

                                sTmp = String.Format("dbScaricaOrdine : bRecord = {0} già scaricato!", iOrderID);
                                LogToFile(sTmp);
                            }
                        }
                        else
                        {
                            /******************************
                             *		aggiornamento DB
                             ******************************/
                            if (!StringBelongsTo_ORDER_CONST(sTipo) && (dataRow != null)) // sennò eccezione con PARMESAN
                                dataRow["iQuantita_Scaricata"] = iQuantitaScaricata + iQuantitaReceipt;

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

                    LogToFile("dbScaricaOrdine : pos 3");

                    // aggiorna il database su disco
                    LogToFile("dbScaricaOrdine : ApplyUpdates");

                    //dbOrdiniAdapter.UpdateBatchSize = 100;
                    iUpdatedRowsOrdini = dbOrdiniAdapter.Update(ordiniTable);

                    if (iUpdatedRowsOrdini > 0)
                        FrmMain.StartAntBmpTimer();

                    Console.WriteLine("dbScaricaOrdine : iUpdatedRows ordini = {0}", iUpdatedRowsOrdini);

                    //dbDataAdapter.UpdateBatchSize = 100;
                    iUpdatedRowsDati = dbDataAdapter.Update(dataTable);
                    Console.WriteLine("dbScaricaOrdine : iUpdatedRows dati = {0}", iUpdatedRowsDati);

                    /*** aggiorna il database su disco ***/
                    transaction.Commit();

                    dbOrdiniAdapter.Dispose();
                    dbOrdiniAdapterSelect.Dispose();

                    dbDataAdapterSelect.Dispose();
                    dbDataAdapter.Dispose();

                    bResult = true;
                    Console.Beep();
                }

                catch (Exception)
                {
                    _WrnMsg.iErrID = WRN_DBE;
                    _WrnMsg.sMsg = "dbScaricaOrdine 3";
                    WarningManager(_WrnMsg);
                    LogToFile("dbScaricaOrdine : dbException 2");
                }
            }
            else
            {
                LogToFile("dbScaricaOrdine : readerOrdini == null");
            }

            return bResult;
        }

        /// <summary> funzione di caricamento ultimi ordini serviti </summary>
        public bool dbCaricaUltimiOrdini()
        {
            int i, j, k, iAttesa, iNumDivCount;
            int iMinText, iMinScaricato, iHtext, iHScaricato;
            bool bDBConnected, bNumDiverso;
            String sTmpStr, sTmpStr2;
            String sQuery;

            try
            {
                bDBConnected = dbInit(GetActualDate(), CASSA_PRINCIPALE);
                /******************************
                 *    costruzione filtro
                 ******************************/

                if (bDBConnected)
                {
                    sQuery = "SELECT * FROM " + _sDBTNameOrdini + " WHERE (\"sTipo_Articolo\" = '" + ORDER_CONST._START_OF_ORDER + "') AND (\"iScaricato\" = 1) ";
                    sQuery += " ORDER BY \"sScaricato\" DESC LIMIT " + (Define.MAX_RIGHE * 2).ToString();

                    PgSqlCommand cmd = new PgSqlCommand()
                    {
                        Connection = _Connection,
                        CommandText = sQuery
                    };

                    PgSqlDataReader readerScaricati = cmd.ExecuteReader();

                    for (k = Define.MAX_RIGHE * 2 - 1; k >= 0; k--)
                        _sNumScontrino[k] = " ";

                    iMinText = 0;
                    iMinScaricato = 0;
                    iNumDivCount = 0;

                    for (k = Define.MAX_RIGHE * 2 - 1; k >= 0; k--)
                    {
                        while (readerScaricati.Read())
                        {
                            i = readerScaricati.GetInt32("iOrdine_ID");
                            if (!readerScaricati.IsDBNull("sText") && !readerScaricati.IsDBNull("sScaricato"))
                            {
                                sTmpStr2 = readerScaricati.GetString("sText");
                                sTmpStr = sTmpStr2.Substring(13, 2);
                                iHtext = Convert.ToInt32(sTmpStr);
                                sTmpStr = sTmpStr2.Substring(16, 2);
                                iMinText = Convert.ToInt32(sTmpStr);

                                iMinText += 60 * iHtext;

                                sTmpStr2 = readerScaricati.GetString("sScaricato");
                                sTmpStr = sTmpStr2.Substring(0, 2);
                                iHScaricato = Convert.ToInt32(sTmpStr);
                                sTmpStr = sTmpStr2.Substring(3, 2);
                                iMinScaricato = Convert.ToInt32(sTmpStr);

                                iMinScaricato += 60 * iHScaricato;

                                iAttesa = iMinScaricato - iMinText;
                            }

                            bNumDiverso = true;
                            for (j = Define.MAX_RIGHE * 2 - 1; j >= 0; j--)
                                if (_sNumScontrino[j] == i.ToString())
                                    bNumDiverso = false;

                            if (bNumDiverso)
                            {
                                _sNumScontrino[k] = i.ToString();
                                iNumDivCount++;
                                break;
                            }
                        }
                    }

                    readerScaricati.Close();
                }
                else
                {
                    LogToFile("FrmVisOrdini : !bDBConnected");
                }
            }

            catch (Exception)
            {
                LogToFile("FrmVisOrdini dbException: Aggiorna2");
            }

            return true;
        }

    }
}

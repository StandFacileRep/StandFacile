/*****************************************************************************************
	 NomeFile : StandCommonSrc/dBaseIntf_my.cs
	 Data	  : 25.09.2024
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
using static StandCommonFiles.commonCl;
using static StandCommonFiles.LogServer;

using StandFacile;
using static StandFacile.glb;
using static StandFacile.dBaseIntf;

namespace StandFacile_DB
{
    /// <summary>classe per la gestione di MySQL</summary>
    public partial class dBaseIntf_my
    {

        /// <summary>
        /// Funzione di salvataggio nel database dei dati di riepilogo <br/>
        /// giornaliero, non deve fare conti !!!!!!!! <br/>
        /// ma solamente salvare SF_Data... <br/> <br/>
        /// 
        /// la Disponibilità è aggiornata solo nella CASSA_PRINCIPALE
        /// </summary>
        public void dbSalvaDati()
        {
            bool bNoProblem, bDBConnection_Ok;
            bool bRecordEquals;
            int i, iUpdatedRows = 0;
            String sTmp, sDisp, sQueryTxt;

            MySqlCommand cmd = new MySqlCommand();
            MySqlDataReader readerDati = null;
            DataTable dataTable = new DataTable();
            DataRow dataRow;

            bDBConnection_Ok = dbInit(getActualDate(), SF_Data.iNumCassa);

            // sicurezza : si prosegue solo se c'è la connessione a DB
            if (!bDBConnection_Ok || !_bCheckStatus)
                return;

            try
            {
                cmd.CommandText = "SELECT * FROM " + _sDBTNameDati + " LIMIT 1;";
                cmd.Connection = _Connection;

                // prima prova di connessione al DB per impostare readerDati != null
                readerDati = cmd.ExecuteReader();
            }

            catch (Exception)
            {
                if (readerDati != null)
                    readerDati.Close();
            }

            try
            {
                // se il database non esiste ... come in dbSalvaOrdine ma con il MySqlDataReader
                if ((readerDati == null) && bDBConnection_Ok)
                {
                    // creazione Tabella Dati
                    dbCreaTableDati();

                    // seconda prova di connessione al DB
                    cmd.Connection = _Connection;
                    readerDati = cmd.ExecuteReader();
                }

                if (bDBConnection_Ok) // se la connessione non è OK evita solo messagggi di errore
                {
                    if (readerDati != null)
                        readerDati.Close(); // non serve più

                    bNoProblem = true;

                    MySqlTransaction transaction = _Connection.BeginTransaction();

                    // riempie dataTable con tutte le colonne
                    sQueryTxt = "SELECT * FROM " + _sDBTNameDati + ";";
                    MySqlDataAdapter dbDataAdapterSelect = new MySqlDataAdapter(sQueryTxt, _Connection);
                    dbDataAdapterSelect.Fill(dataTable);
                    dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["iRiga_ID"] };

                    LogToFile("dbSalvaDati : dataTable Fill");

                    MySqlDataAdapter dbDataAdapter = new MySqlDataAdapter();
                    MySqlParameter parm;

                    sQueryTxt = "UPDATE " + _sDBTNameDati +
                        @" SET iRiga_ID = @iRiga_ID, sTipo_Articolo = @sTipo_Articolo, iPrezzo_Unitario = @iPrezzo_Unitario, 
                        iQuantita_Venduta = @iQuantita_Venduta, iQuantita_Scaricata = @iQuantita_Scaricata, sDisponibilita = @sDisponibilita, 
                        iGruppo_Stampa = @iGruppo_Stampa, sText = @sText WHERE iRiga_ID = @oldId;";

                    dbDataAdapter.UpdateCommand = new MySqlCommand(sQueryTxt, _Connection);

                    dbDataAdapter.UpdateCommand.Parameters.Add("@iRiga_ID", MySqlType.Int, 11, "iRiga_ID");
                    dbDataAdapter.UpdateCommand.Parameters.Add("@sTipo_Articolo", MySqlType.VarChar, 50, "sTipo_Articolo");
                    dbDataAdapter.UpdateCommand.Parameters.Add("@iPrezzo_Unitario", MySqlType.Int, 10, "iPrezzo_Unitario");
                    dbDataAdapter.UpdateCommand.Parameters.Add("@iQuantita_Venduta", MySqlType.Int, 10, "iQuantita_Venduta");
                    dbDataAdapter.UpdateCommand.Parameters.Add("@iQuantita_Scaricata", MySqlType.Int, 10, "iQuantita_Scaricata");
                    dbDataAdapter.UpdateCommand.Parameters.Add("@sDisponibilita", MySqlType.VarChar, 10, "sDisponibilita");
                    dbDataAdapter.UpdateCommand.Parameters.Add("@iGruppo_Stampa", MySqlType.Int, 10, "iGruppo_Stampa");
                    dbDataAdapter.UpdateCommand.Parameters.Add("@sText", MySqlType.VarChar, 50, "sText");

                    parm = dbDataAdapter.UpdateCommand.Parameters.Add("@oldId", MySqlType.Int, 11, "iRiga_ID");
                    parm.SourceVersion = DataRowVersion.Original;

                    dbDataAdapter.UpdateCommand.UpdatedRowSource = UpdateRowSource.None;


                    // ******** DOCUMENTAZIONE ********
                    // https://docs.microsoft.com/it-it/dotnet/framework/data/adonet/dataadapter-parameters
                    // https://dev.mysql.com/doc/dev/connector-net/8.0/html/P_MySql_Data_MySqlClient_MySqlDataAdapter_UpdateCommand.htm

                    /************************************
                     *	inizio salvataggio dati header
                     ************************************/

                    sTmp = "-13";
                    dataRow = dataTable.Rows.Find(sTmp);
                    if ((dataRow != null))
                    {
                        if (dataRow["sText"].ToString() != SF_Data.sVersione)
                            dataRow["sText"] = SF_Data.sVersione;
                    }
                    else
                        bNoProblem = false;

                    sTmp = "-12";
                    dataRow = dataTable.Rows.Find(sTmp);
                    if ((dataRow != null))
                    {
                        if (Convert.ToInt32(dataRow["iQuantita_Venduta"]) != SF_Data.iNumCassa)
                            dataRow["iQuantita_Venduta"] = SF_Data.iNumCassa;
                    }
                    else
                        bNoProblem = false;

                    sTmp = "-11"; // salvare sempre
                    dataRow = dataTable.Rows.Find(sTmp);
                    if (dataRow != null)
                        dataRow["sText"] = SF_Data.sDateTime;
                    else
                        bNoProblem = false;

                    sTmp = "-10";
                    dataRow = dataTable.Rows.Find(sTmp);
                    if ((dataRow != null))
                    {
                        if (dataRow["sText"].ToString() != SF_Data.sHeaders[0])
                            dataRow["sText"] = SF_Data.sHeaders[0];
                    }
                    else
                        bNoProblem = false;

                    sTmp = "-9";
                    dataRow = dataTable.Rows.Find(sTmp);
                    if ((dataRow != null))
                    {
                        if (dataRow["sText"].ToString() != SF_Data.sHeaders[1])
                            dataRow["sText"] = SF_Data.sHeaders[1];
                    }
                    else
                        bNoProblem = false;

                    /***************************************************************
                     *	Salvataggio del Numero di Scontrini e di Messaggi emessi
                     ***************************************************************/
                    sTmp = "-8";
                    dataRow = dataTable.Rows.Find(sTmp);
                    if ((dataRow != null))
                    {
                        if (Convert.ToInt32(dataRow["iQuantita_Venduta"]) != (SF_Data.iNumOfLastReceipt - SF_Data.iStartingNumOfReceipts))
                            dataRow["iQuantita_Venduta"] = SF_Data.iNumOfLastReceipt - SF_Data.iStartingNumOfReceipts;
                    }
                    else
                        bNoProblem = false;

                    sTmp = "-7";
                    dataRow = dataTable.Rows.Find(sTmp);
                    if ((dataRow != null))
                    {
                        if (Convert.ToInt32(dataRow["iQuantita_Venduta"]) != SF_Data.iNumAnnullati)
                            dataRow["iQuantita_Venduta"] = SF_Data.iNumAnnullati;
                    }
                    else
                        bNoProblem = false;

                    sTmp = "-6";
                    dataRow = dataTable.Rows.Find(sTmp);
                    if ((dataRow != null))
                    {
                        if (Convert.ToInt32(dataRow["iQuantita_Venduta"]) != SF_Data.iNumOfMessages)
                            dataRow["iQuantita_Venduta"] = SF_Data.iNumOfMessages;
                    }
                    else
                        bNoProblem = false;

                    sTmp = "-5";
                    dataRow = dataTable.Rows.Find(sTmp);
                    if ((dataRow != null))
                    {
                        if (Convert.ToInt32(dataRow["iQuantita_Venduta"]) != SF_Data.iTotaleAnnullato)
                            dataRow["iQuantita_Venduta"] = SF_Data.iTotaleAnnullato;
                    }
                    else
                        bNoProblem = false;

                    sTmp = "-4";
                    dataRow = dataTable.Rows.Find(sTmp);
                    if ((dataRow != null))
                    {
                        if (Convert.ToInt32(dataRow["iQuantita_Venduta"]) != SF_Data.iTotaleScontatoStd)
                            dataRow["iQuantita_Venduta"] = SF_Data.iTotaleScontatoStd;
                    }
                    else
                        bNoProblem = false;

                    sTmp = "-3";
                    dataRow = dataTable.Rows.Find(sTmp);
                    if ((dataRow != null))
                    {
                        if (Convert.ToInt32(dataRow["iQuantita_Venduta"]) != SF_Data.iTotaleScontatoFisso)
                            dataRow["iQuantita_Venduta"] = SF_Data.iTotaleScontatoFisso;
                    }
                    else
                        bNoProblem = false;

                    sTmp = "-2";
                    dataRow = dataTable.Rows.Find(sTmp);
                    if ((dataRow != null))
                    {
                        if (Convert.ToInt32(dataRow["iQuantita_Venduta"]) != SF_Data.iTotaleScontatoGratis)
                            dataRow["iQuantita_Venduta"] = SF_Data.iTotaleScontatoGratis;
                    }
                    else
                        bNoProblem = false;

                    // iIncassoTotale è disponibile per ultimo
                    sTmp = "-1";
                    dataRow = dataTable.Rows.Find(sTmp);
                    if ((dataRow != null))
                    {
                        if (Convert.ToInt32(dataRow["iQuantita_Venduta"]) != SF_Data.iTotaleIncasso)
                            dataRow["iQuantita_Venduta"] = SF_Data.iTotaleIncasso;
                    }
                    else
                        bNoProblem = false;

                    for (i = 0; (i < MAX_NUM_ARTICOLI) && bNoProblem; i++)
                    {
                        if (DISP_OK == SF_Data.Articolo[i].iDisponibilita)
                            sDisp = "OK";
                        else
                            sDisp = SF_Data.Articolo[i].iDisponibilita.ToString();

                        // ricerca chiave
                        sTmp = i.ToString();
                        dataRow = dataTable.Rows.Find(sTmp);

                        if (dataRow != null)
                        {
                            bRecordEquals =
                                (
                                    // manca volutamente iQuantita_Scaricata
                                    (SF_Data.Articolo[i].sTipo == dataRow["sTipo_Articolo"].ToString()) &&
                                    (SF_Data.Articolo[i].iPrezzoUnitario == Convert.ToInt32(dataRow["iPrezzo_Unitario"])) &&
                                    (SF_Data.Articolo[i].iQuantitaVenduta == Convert.ToInt32(dataRow["iQuantita_Venduta"])) &&
                                    //(SF_Data.Articolo[i].iQuantita_Scaricata == Convert.ToInt32(dataRow["iQuantita_Scaricata"])) &&
                                    (sDisp == dataRow["sDisponibilita"].ToString()) &&
                                    (SF_Data.Articolo[i].iGruppoStampa == Convert.ToInt32(dataRow["iGruppo_Stampa"]))
                                );

                            // ottimizzazione scritture db
                            if (bRecordEquals)
                                continue;

                            if (dataRow["sTipo_Articolo"].ToString().Contains(SHMAGIC) && String.IsNullOrEmpty(SF_Data.Articolo[i].sTipo))
                                continue;

                            if (!String.IsNullOrEmpty(SF_Data.Articolo[i].sTipo))
                                dataRow["sTipo_Articolo"] = SF_Data.Articolo[i].sTipo;
                            else
                            {
                                sTmp = String.Format(MAGIC, i); // Magic
                                dataRow["sTipo_Articolo"] = sTmp;
                            }

                            dataRow["iPrezzo_Unitario"] = SF_Data.Articolo[i].iPrezzoUnitario;
                            dataRow["iQuantita_Venduta"] = SF_Data.Articolo[i].iQuantitaVenduta;

                            dataRow["iGruppo_Stampa"] = SF_Data.Articolo[i].iGruppoStampa;

                            dataRow["sDisponibilita"] = sDisp;
                        }
                        else
                        {
                            bNoProblem = false;
                            _WrnMsg.sMsg = String.Format("dbSalvaDati problema indice {0}", i);
                            break;
                        }
                    }

                    /*** aggiorna il database su disco ***/
                    dbDataAdapter.UpdateBatchSize = 100;
                    iUpdatedRows = dbDataAdapter.Update(dataTable);

                    Console.WriteLine("dbSalvaDati : iUpdatedRows = {0}", iUpdatedRows);

                    transaction.Commit();

                    dbDataAdapterSelect.Dispose();
                    dbDataAdapter.Dispose();

                    /********************
                     *		Warning
                     ********************/
                    if (!bNoProblem) // c'è qualche problema
                    {
                        _WrnMsg.iErrID = WRN_RNF;
                        _WrnMsg.sMsg = sTmp;
                        WarningManager(_WrnMsg);
                        LogToFile("dbSalvaDati : record" + _WrnMsg.sMsg + "non trovato !");
                    }
                } // if (bDBConnection_Ok)
            }

            catch (Exception)
            {
                _WrnMsg.iErrID = WRN_DBE;
                _WrnMsg.sMsg = String.Format("dbSalvaDati : {0}", _bUSA_NDB());
                WarningManager(_WrnMsg);

                LogToFile("dbSalvaDati : dbException");
            }

            sTmp = String.Format("dbSalvaDati: records = {0}", iUpdatedRows);
            LogToFile(sTmp);
        } // end dbSalvaDati


        /// <summary>crea la tabella dei dati del giorno</summary>
        public bool dbCreaTableDati()
        {
            int i;
            String sTmp, sQueryTxt;

            MySqlCommand cmd = new MySqlCommand();
            MySqlDataReader readerDati = null;
            DataTable dataTable = new DataTable();

            // *** sicurezza ***
            if (!_bUSA_NDB()) return false;

            try
            {
                dbInit(getActualDate(), SF_Data.iNumCassa);

                cmd.CommandText = "SELECT * FROM " + _sDBTNameDati + " LIMIT 1;";
                cmd.Connection = _Connection;

                // prima prova di connessione al DB per impostare readerDati != null
                readerDati = cmd.ExecuteReader();

                // la tabella esiste e non serve proseguire
                return true;
            }

            catch (Exception)
            {
                if (readerDati != null)
                    readerDati.Close();
            }

            try
            {

                /**************************************************
                 * Query di creazione tabella Dati : attenzione
                 * non possono esserci ripetizioni delle chiavi !
                 * KEY sTipo_Articolo è richiesta da StandOrdini
                 **************************************************/
                sQueryTxt = String.Format(@"CREATE TABLE IF NOT EXISTS {0} (iRiga_ID INT NOT NULL, sTipo_Articolo VARCHAR(50),      
                            iPrezzo_Unitario INT UNSIGNED NOT NULL, iQuantita_Venduta INT UNSIGNED NOT NULL,
                            iQuantita_Scaricata INT UNSIGNED NOT NULL, sDisponibilita VARCHAR(10),
                            iGruppo_Stampa INT UNSIGNED NOT NULL, sText VARCHAR(50),
                            PRIMARY KEY(iRiga_ID, sTipo_Articolo)); ", _sDBTNameDati);


                cmd.Connection = _Connection;
                cmd.CommandText = sQueryTxt;
                var qResult = cmd.ExecuteScalar();

                LogToFile("dbCreaTableDati : CREATE TABLE Dati");

                MySqlTransaction transaction = _Connection.BeginTransaction();

                MySqlDataAdapter dbDataAdapter = new MySqlDataAdapter();

                sQueryTxt = "INSERT INTO " + _sDBTNameDati +
                                @" (iRiga_ID, sTipo_Articolo, iPrezzo_Unitario, iQuantita_Venduta,
                                iQuantita_Scaricata, sDisponibilita, iGruppo_Stampa, sText) VALUES 
                                (@iRiga_ID, @sTipo_Articolo, @iPrezzo_Unitario, @iQuantita_Venduta, 
                                @iQuantita_Scaricata, @sDisponibilita, @iGruppo_Stampa, @sText);";

                dbDataAdapter.InsertCommand = new MySqlCommand(sQueryTxt, _Connection);

                dbDataAdapter.InsertCommand.Parameters.Add("@iRiga_ID", MySqlType.Int, 11, "iRiga_ID");
                dbDataAdapter.InsertCommand.Parameters.Add("@sTipo_Articolo", MySqlType.VarChar, 50, "sTipo_Articolo");
                dbDataAdapter.InsertCommand.Parameters.Add("@iPrezzo_Unitario", MySqlType.Int, 10, "iPrezzo_Unitario");
                dbDataAdapter.InsertCommand.Parameters.Add("@iQuantita_Venduta", MySqlType.Int, 10, "iQuantita_Venduta");
                dbDataAdapter.InsertCommand.Parameters.Add("@iQuantita_Scaricata", MySqlType.Int, 10, "iQuantita_Scaricata");
                dbDataAdapter.InsertCommand.Parameters.Add("@sDisponibilita", MySqlType.VarChar, 10, "sDisponibilita");
                dbDataAdapter.InsertCommand.Parameters.Add("@iGruppo_Stampa", MySqlType.Int, 10, "iGruppo_Stampa");
                dbDataAdapter.InsertCommand.Parameters.Add("@sText", MySqlType.VarChar, 50, "sText");
                dbDataAdapter.InsertCommand.UpdatedRowSource = UpdateRowSource.None;

                // riempie con tutte le colonne
                sQueryTxt = "SELECT * FROM " + _sDBTNameDati + ";";
                MySqlDataAdapter dbDataAdapterSelect = new MySqlDataAdapter(sQueryTxt, _Connection);
                dbDataAdapterSelect.Fill(dataTable);

                DataRow row = dataTable.NewRow();

                row["iRiga_ID"] = -13;
                row["sTipo_Articolo"] = "_Versione";
                row["iPrezzo_Unitario"] = 0;
                row["iQuantita_Venduta"] = 0;
                row["sDisponibilita"] = "0";
                row["iGruppo_Stampa"] = 0;
                row["iQuantita_Scaricata"] = 0;

                sTmp = String.Format("StandFacile {0}", RELEASE_SW);
                row["sText"] = sTmp;

                dataTable.Rows.Add(row);
                row = dataTable.NewRow();

                row["iRiga_ID"] = -12;
                row["sTipo_Articolo"] = "_Cassa";
                row["iPrezzo_Unitario"] = 0;
                row["iQuantita_Venduta"] = SF_Data.iNumCassa;
                row["sDisponibilita"] = "0";
                row["iGruppo_Stampa"] = 0;
                row["iQuantita_Scaricata"] = 0;
                row["sText"] = "";

                dataTable.Rows.Add(row);
                row = dataTable.NewRow();

                row["iRiga_ID"] = -11;
                row["sTipo_Articolo"] = "_DateTime";
                row["iPrezzo_Unitario"] = 0;
                row["iQuantita_Venduta"] = 0;
                row["sDisponibilita"] = "0";
                row["iGruppo_Stampa"] = 0;
                row["iQuantita_Scaricata"] = 0;
                row["sText"] = SF_Data.sDateTime;

                dataTable.Rows.Add(row);
                row = dataTable.NewRow();

                row["iRiga_ID"] = -10;
                row["sTipo_Articolo"] = "_Header_0";
                row["iPrezzo_Unitario"] = 0;
                row["iQuantita_Venduta"] = 0;
                row["sDisponibilita"] = "0";
                row["iGruppo_Stampa"] = 0;
                row["iQuantita_Scaricata"] = 0;
                row["sText"] = SF_Data.sHeaders[0];

                dataTable.Rows.Add(row);
                row = dataTable.NewRow();

                row["iRiga_ID"] = -9;
                row["sTipo_Articolo"] = "_Header_1";
                row["iPrezzo_Unitario"] = 0;
                row["iQuantita_Venduta"] = 0;
                row["sDisponibilita"] = "0";
                row["iGruppo_Stampa"] = 0;
                row["iQuantita_Scaricata"] = 0;
                row["sText"] = SF_Data.sHeaders[1];

                dataTable.Rows.Add(row);
                row = dataTable.NewRow();

                row["iRiga_ID"] = -8;
                row["sTipo_Articolo"] = "_NumScontrini";
                row["iPrezzo_Unitario"] = 0;
                row["iQuantita_Venduta"] = SF_Data.iNumOfLastReceipt;
                row["sDisponibilita"] = "0";
                row["iGruppo_Stampa"] = 0;
                row["iQuantita_Scaricata"] = 0;
                row["sText"] = "";

                dataTable.Rows.Add(row);
                row = dataTable.NewRow();

                row["iRiga_ID"] = -7;
                row["sTipo_Articolo"] = "_NumAnnulli";
                row["iPrezzo_Unitario"] = 0;
                row["iQuantita_Venduta"] = SF_Data.iNumAnnullati;
                row["sDisponibilita"] = "0";
                row["iGruppo_Stampa"] = 0;
                row["iQuantita_Scaricata"] = 0;
                row["sText"] = "";

                dataTable.Rows.Add(row);
                row = dataTable.NewRow();

                row["iRiga_ID"] = -6;
                row["sTipo_Articolo"] = "_NumMessaggi";
                row["iPrezzo_Unitario"] = 0;
                row["iQuantita_Venduta"] = SF_Data.iNumOfMessages;
                row["sDisponibilita"] = "0";
                row["iGruppo_Stampa"] = 0;
                row["iQuantita_Scaricata"] = 0;
                row["sText"] = "";

                dataTable.Rows.Add(row);
                row = dataTable.NewRow();

                row["iRiga_ID"] = -5;
                row["sTipo_Articolo"] = "_TotaleAnnulli";
                row["iPrezzo_Unitario"] = 0;
                row["iQuantita_Venduta"] = SF_Data.iTotaleAnnullato;
                row["sDisponibilita"] = "0";
                row["iGruppo_Stampa"] = 0;
                row["iQuantita_Scaricata"] = 0;
                row["sText"] = "";

                dataTable.Rows.Add(row);
                row = dataTable.NewRow();

                row["iRiga_ID"] = -4;
                row["sTipo_Articolo"] = "_TotaleScontiArt";
                row["iPrezzo_Unitario"] = 0;
                row["iQuantita_Venduta"] = SF_Data.iTotaleScontatoStd;
                row["sDisponibilita"] = "0";
                row["iGruppo_Stampa"] = 0;
                row["iQuantita_Scaricata"] = 0;
                row["sText"] = "";

                dataTable.Rows.Add(row);
                row = dataTable.NewRow();

                row["iRiga_ID"] = -3;
                row["sTipo_Articolo"] = "_TotaleScontiFissi";
                row["iPrezzo_Unitario"] = 0;
                row["iQuantita_Venduta"] = SF_Data.iTotaleScontatoFisso;
                row["sDisponibilita"] = "0";
                row["iGruppo_Stampa"] = 0;
                row["iQuantita_Scaricata"] = 0;
                row["sText"] = "";

                dataTable.Rows.Add(row);
                row = dataTable.NewRow();

                row["iRiga_ID"] = -2;
                row["sTipo_Articolo"] = "_TotaleGratis";
                row["iPrezzo_Unitario"] = 0;
                row["iQuantita_Venduta"] = SF_Data.iTotaleScontatoGratis;
                row["sDisponibilita"] = "0";
                row["iGruppo_Stampa"] = 0;
                row["iQuantita_Scaricata"] = 0;
                row["sText"] = "";

                dataTable.Rows.Add(row);
                row = dataTable.NewRow();

                row["iRiga_ID"] = -1;
                row["sTipo_Articolo"] = "_TotaleIncasso";
                row["iPrezzo_Unitario"] = 0;
                row["iQuantita_Venduta"] = SF_Data.iTotaleIncasso;
                row["sDisponibilita"] = "0";
                row["iGruppo_Stampa"] = 0;
                row["iQuantita_Scaricata"] = 0;
                row["sText"] = "";

                dataTable.Rows.Add(row);

                // popolazione della tabella dati con DB_Articolo[i]
                for (i = 0; i < MAX_NUM_ARTICOLI; i++)
                {
                    row = dataTable.NewRow(); // crea riga in ds

                    row["iRiga_ID"] = i;

                    if (!String.IsNullOrEmpty(SF_Data.Articolo[i].sTipo))
                        row["sTipo_Articolo"] = SF_Data.Articolo[i].sTipo;
                    else
                    {
                        sTmp = String.Format(MAGIC, i); // Magic per PRIMARY KEY
                        row["sTipo_Articolo"] = sTmp;
                    }

                    row["iPrezzo_Unitario"] = SF_Data.Articolo[i].iPrezzoUnitario;
                    row["iQuantita_Venduta"] = SF_Data.Articolo[i].iQuantitaVenduta;
                    row["iGruppo_Stampa"] = SF_Data.Articolo[i].iGruppoStampa;
                    row["iQuantita_Scaricata"] = 0;
                    row["sText"] = "";
                    row["sDisponibilita"] = "OK";

                    dataTable.Rows.Add(row);// aggiungi riga in ds
                }

                /*** aggiorna il database su disco ***/
                dbDataAdapter.UpdateBatchSize = 100;
                dbDataAdapter.Update(dataTable);

                transaction.Commit();

                dbDataAdapter.Dispose();
                dbDataAdapterSelect.Dispose();
                return true;
            }

            catch (Exception)
            {
                //ClientDS_Dati->Close();

                _WrnMsg.iErrID = WRN_DBE;
                _WrnMsg.sMsg = "dbCreaTableDati";
                WarningManager(_WrnMsg);

                LogToFile("dbCreaTableDati : dbException dbCreaTableDati");
                return false;
            }
        }

        /// <summary>
        /// aggiorna le intestazioni nel database ordini, ma solo se è la CASSA_PRINCIPALE <br/>
        /// chiamata da SalvaListino()
        /// </summary>
        public void dbUpdateHeadOrdine()
        {
            bool bDBConnection_Ok, bNoProblem;
            int i, iNumRowsOrdini = 0, iUpdatedRows = 0;
            String sTmp, sTmpSearch, sQueryTxt;

            MySqlDataAdapter dbDataAdapterSelect = null;
            MySqlDataAdapter dbDataAdapter;

            DataTable ordiniTable = new DataTable();
            DataRow[] ordiniRows;

            // prosegue solo se la cassa è principale
            if (SF_Data.iNumCassa != CASSA_PRINCIPALE)
                return;

            bDBConnection_Ok = dbInit(getActualDate(), SF_Data.iNumCassa);

            // sicurezza : si prosegue solo se c'è la connessione a DB
            if (!bDBConnection_Ok)
                return;

            try
            {
                // verifica se esiste la tabella
                sQueryTxt = "SELECT * FROM " + _sDBTNameOrdini + " WHERE iOrdine_ID = 0 ;";
                dbDataAdapterSelect = new MySqlDataAdapter(sQueryTxt, _Connection);

                iNumRowsOrdini = dbDataAdapterSelect.Fill(ordiniTable);
            }

            catch (Exception)
            {
                if (dbDataAdapterSelect != null)
                    dbDataAdapterSelect.Dispose();
            }

            try
            {
                if ((iNumRowsOrdini > 0) && bDBConnection_Ok)
                {

                    bNoProblem = true;

                    MySqlTransaction transaction = _Connection.BeginTransaction();

                    sQueryTxt = "UPDATE " + _sDBTNameOrdini + " SET sText = @sText, iStatus = @iStatus WHERE sTipo_Articolo=@oldId;";

                    dbDataAdapter = new MySqlDataAdapter
                    {
                        UpdateCommand = new MySqlCommand(sQueryTxt, _Connection)
                    };

                    dbDataAdapter.UpdateCommand.Parameters.Add("@sText", MySqlType.VarChar, 50, "sText");
                    dbDataAdapter.UpdateCommand.Parameters.Add("@iStatus", MySqlType.Int, 10, "iStatus");

                    dbDataAdapter.UpdateCommand.Parameters.Add("@oldId", MySqlType.VarChar, 50, "sTipo_Articolo").SourceVersion = DataRowVersion.Original;
                    dbDataAdapter.UpdateCommand.UpdatedRowSource = UpdateRowSource.None;

                    sTmpSearch = String.Format("sTipo_Articolo = \'_Versione\'");
                    ordiniRows = ordiniTable.Select(sTmpSearch);
                    if (ordiniRows != null)
                    {
                        sTmp = String.Format("StandFacile {0}", RELEASE_SW);
                        ordiniRows[0]["sText"] = sTmp;
                    }
                    else
                        bNoProblem = false;

                    for (i = 0; (i < MAX_NUM_HEADERS) && bNoProblem; i++)
                    {
                        sTmpSearch = String.Format("sTipo_Articolo = \'_Header_{0}\'", i);
                        ordiniRows = ordiniTable.Select(sTmpSearch);

                        if (ordiniRows != null)
                        {
                            sTmp = ordiniRows[0]["sText"].ToString();
                            ordiniRows[0]["sText"] = SF_Data.sHeaders[i];
                        }
                        else
                            bNoProblem = false;
                    }

                    for (i = 0; (i < NUM_EDIT_GROUPS) && bNoProblem; i++)
                    {
                        sTmpSearch = String.Format("sTipo_Articolo = \'_GroupText_{0}\'", i);
                        ordiniRows = ordiniTable.Select(sTmpSearch);

                        if (ordiniRows != null)
                        {
                            ordiniRows[0]["sText"] = SF_Data.sCopiesGroupsText[i];

                            if (SF_Data.bCopiesGroupsFlag[i])
                                ordiniRows[0]["iStatus"] = 10 + SF_Data.iGroupsColor[i];
                            else
                                ordiniRows[0]["iStatus"] = SF_Data.iGroupsColor[i];
                        }
                        else
                            bNoProblem = false;
                    }

                    for (i = 0; (i < NUM_GROUPS_COLORS - 1) && bNoProblem; i++)
                    {
                        sTmpSearch = String.Format("sTipo_Articolo = \'_ColorText_{0}\'", i);
                        ordiniRows = ordiniTable.Select(sTmpSearch);

                        if (ordiniRows != null)
                            ordiniRows[0]["sText"] = SF_Data.sColorGroupsText[i];
                        else
                            bNoProblem = false;
                    }

                    // aggiorna il database su disco
                    dbDataAdapter.UpdateBatchSize = 100;
                    iUpdatedRows = dbDataAdapter.Update(ordiniTable);

                    Console.WriteLine("dbUpdateHeadOrdine : iUpdatedRows = {0}", iUpdatedRows);

                    transaction.Commit();

                    dbDataAdapter.Dispose();

                    if (!bNoProblem)
                    {
                        _WrnMsg.iErrID = WRN_RNF;
                        _WrnMsg.sMsg = sTmpSearch;
                        WarningManager(_WrnMsg);
                        LogToFile("dbUpdateHeadOrdine : record " + _WrnMsg.sMsg + " non trovato !");
                    }
                }
            }

            catch (Exception)
            {
                _WrnMsg.iErrID = WRN_DBE;
                _WrnMsg.sMsg = String.Format("dbUpdateHeadOrdine : {0}", _bUSA_NDB());
                WarningManager(_WrnMsg);

                LogToFile("dbUpdateHeadOrdine : dbSalvaHeadOrdini Ordini");
            }

            sTmp = String.Format("dbUpdateHeadOrdine: record {0}", iUpdatedRows);
            LogToFile(sTmp);
        }

        /// <summary>
        /// Salva l'ordine corrente nel database<br/>
        /// se bCreateHead = true salva solo l'intestazione
        /// </summary>
        public void dbSalvaOrdine(bool bCreateHead = false)
        {
            bool bDBConnection_Ok;
            int i, iNumRowsOrdini = 0, iUpdatedRows = 0;
            String sTmp, sQueryTxt;

            MySqlCommand cmd = new MySqlCommand();

            MySqlDataAdapter dbDataAdapterSelect = null;
            MySqlDataAdapter dbDataAdapterInsert = null;

            DataTable ordiniTable = new DataTable();
            DataRow row;

            // dbSalvaOrdine è silente mentre dbSalvaDati non lo è
            bDBConnection_Ok = dbInit(getActualDate(), SF_Data.iNumCassa, true);

            // sicurezza : si prosegue solo se c'è la connessione al DB
            if (!bDBConnection_Ok)
                return;

            try
            {
                // preparazione inserimento
                dbDataAdapterInsert = new MySqlDataAdapter();

                sQueryTxt = "INSERT INTO " + _sDBTNameOrdini +
                                @" (iOrdine_ID, sTipo_Articolo, iPrezzo_Unitario, iQuantita_Ordine, iIndex_Listino, sText, 
                                    iGruppo_Stampa, iStatus, iNumCassa, iScaricato, sScaricato, iAnnullato, sAnnullato) VALUES 
                                   (@iOrdine_ID, @sTipo_Articolo, @iPrezzo_Unitario, @iQuantita_Ordine, @iIndex_Listino, @sText, 
                                    @iGruppo_Stampa, @iStatus, @iNumCassa, @iScaricato, @sScaricato, @iAnnullato, @sAnnullato);";

                dbDataAdapterInsert.InsertCommand = new MySqlCommand(sQueryTxt, _Connection);

                dbDataAdapterInsert.InsertCommand.Parameters.Add("@iOrdine_ID", MySqlType.Int, 11, "iOrdine_ID");
                dbDataAdapterInsert.InsertCommand.Parameters.Add("@sTipo_Articolo", MySqlType.VarChar, 50, "sTipo_Articolo");
                dbDataAdapterInsert.InsertCommand.Parameters.Add("@iPrezzo_Unitario", MySqlType.Int, 10, "iPrezzo_Unitario");
                dbDataAdapterInsert.InsertCommand.Parameters.Add("@iQuantita_Ordine", MySqlType.Int, 10, "iQuantita_Ordine");
                dbDataAdapterInsert.InsertCommand.Parameters.Add("@iIndex_Listino", MySqlType.Int, 10, "iIndex_Listino");
                dbDataAdapterInsert.InsertCommand.Parameters.Add("@sText", MySqlType.VarChar, 50, "sText");
                dbDataAdapterInsert.InsertCommand.Parameters.Add("@iGruppo_Stampa", MySqlType.Int, 10, "iGruppo_Stampa");
                dbDataAdapterInsert.InsertCommand.Parameters.Add("@iStatus", MySqlType.Int, 10, "iStatus");
                dbDataAdapterInsert.InsertCommand.Parameters.Add("@iNumCassa", MySqlType.Int, 10, "iNumCassa");
                dbDataAdapterInsert.InsertCommand.Parameters.Add("@iScaricato", MySqlType.Int, 10, "iScaricato");
                dbDataAdapterInsert.InsertCommand.Parameters.Add("@sScaricato", MySqlType.VarChar, 10, "sScaricato");
                dbDataAdapterInsert.InsertCommand.Parameters.Add("@iAnnullato", MySqlType.Int, 10, "iAnnullato");
                dbDataAdapterInsert.InsertCommand.Parameters.Add("@sAnnullato", MySqlType.VarChar, 10, "sAnnullato");
                dbDataAdapterInsert.InsertCommand.UpdatedRowSource = UpdateRowSource.None;

                // verifica se esiste la tabella
                sQueryTxt = "SELECT * FROM " + _sDBTNameOrdini + " LIMIT 1;";
                dbDataAdapterSelect = new MySqlDataAdapter(sQueryTxt, _Connection);

                iNumRowsOrdini = dbDataAdapterSelect.Fill(ordiniTable);
            }

            catch (Exception)
            {
                if (dbDataAdapterSelect != null)
                    dbDataAdapterSelect.Dispose();
            }

            try
            {
                MySqlTransaction transaction = _Connection.BeginTransaction();

                // se il database non esiste ... come in dbSalvaDati ma con il MySqlDataAdapter
                // dbSalvaDati Crea e poi Aggiorna, dbSalvaOrdine accoda sempre eventuale Head e poi l'Ordine

                if ((iNumRowsOrdini == 0) && bDBConnection_Ok)
                {
                    /***************************************************************************************
                     *                              *** SALVA HEAD ORDINE ***
                     * 
                     *  Query di creazione tabella : non ci sono KEY
                     *  in quanto tutte le voci hanni ripetizioni
                     *
                     *  iNumCassa INT UNSIGNED deve essere NOT NULL
                     *  altrimenti con SQLite il filtro fallisce per type mismatch
                     *
                     *  iScaricato è un flag, sScaricato conterrà la stringa dell'ora dello scarico
                     *  sText deve avere una dimensione di almeno :
                     *  iMAX_RECEIPT_CHARS*(6+3) (intestazioni + contenuto) per memorizzare i Messaggi = 300
                     *
                     *	append continuo ordini, non serve KEY
                     ***************************************************************************************/

                    sQueryTxt = String.Format(@"CREATE TABLE IF NOT EXISTS {0} (iOrdine_ID INT NOT NULL, sTipo_Articolo VARCHAR(50) NOT NULL,        
                                        iPrezzo_Unitario INT UNSIGNED, iQuantita_Ordine INT UNSIGNED, iIndex_Listino INT UNSIGNED, sText VARCHAR(400), 
                                        iGruppo_Stampa INT UNSIGNED, iStatus INT UNSIGNED, iNumCassa INT UNSIGNED NOT NULL, iScaricato INT UNSIGNED, 
                                        sScaricato VARCHAR(10), iAnnullato INT UNSIGNED, sAnnullato VARCHAR(10)); ", _sDBTNameOrdini);

                    cmd.CommandText = sQueryTxt;
                    cmd.Connection = _Connection;

                    var qResult = cmd.ExecuteScalar();

                    LogToFile("dbSalvaOrdine : CREATE TABLE Ordini");

                    // legge di nuovo la tabella Ordini
                    sQueryTxt = "SELECT * FROM " + _sDBTNameOrdini + " LIMIT 1;";
                    dbDataAdapterSelect = new MySqlDataAdapter(sQueryTxt, _Connection);

                    iNumRowsOrdini = dbDataAdapterSelect.Fill(ordiniTable);

                    row = ordiniTable.NewRow();

                    row["iOrdine_ID"] = 0;
                    row["sTipo_Articolo"] = "_Versione";
                    sTmp = String.Format("StandFacile {0}", RELEASE_SW);
                    row["sText"] = sTmp;
                    row["iNumCassa"] = 0;

                    ordiniTable.Rows.Add(row);

                    for (i = 0; i < MAX_NUM_HEADERS; i++)
                    {
                        row = ordiniTable.NewRow();

                        row["iOrdine_ID"] = 0;
                        sTmp = String.Format("_Header_{0}", i);
                        row["sTipo_Articolo"] = sTmp;
                        row["sText"] = SF_Data.sHeaders[i];
                        row["iNumCassa"] = 0;

                        ordiniTable.Rows.Add(row);
                    }

                    // meglio scriverli tutti eventualmente con elementi vuoti
                    for (i = 0; i < NUM_EDIT_GROUPS; i++)
                    {
                        row = ordiniTable.NewRow();

                        row["iOrdine_ID"] = 0;
                        sTmp = String.Format("_GroupText_{0}", i);
                        row["sTipo_Articolo"] = sTmp;
                        row["sText"] = SF_Data.sCopiesGroupsText[i];
                        row["iNumCassa"] = 0;

                        if (SF_Data.bCopiesGroupsFlag[i])
                            row["iStatus"] = 10 + SF_Data.iGroupsColor[i];
                        else
                            row["iStatus"] = SF_Data.iGroupsColor[i];

                        ordiniTable.Rows.Add(row);
                    }

                    for (i = 0; (i < NUM_GROUPS_COLORS - 1); i++)
                    {
                        row = ordiniTable.NewRow();

                        row["iOrdine_ID"] = 0;
                        sTmp = String.Format("_ColorText_{0}", i);
                        row["sTipo_Articolo"] = sTmp;
                        row["sText"] = SF_Data.sColorGroupsText[i];
                        row["iNumCassa"] = 0;

                        ordiniTable.Rows.Add(row);
                    }
                }

                if (bDBConnection_Ok && !bCreateHead) // se la connessione non è OK evita solo messagggi di errore
                {
                    row = ordiniTable.NewRow();

                    row["iOrdine_ID"] = SF_Data.iNumOfLastReceipt;
                    row["sTipo_Articolo"] = _ORDER_CONST._START_OF_ORDER;
                    row["sText"] = SF_Data.sDateTime;
                    row["iStatus"] = SF_Data.iStatusReceipt;
                    row["iNumCassa"] = SF_Data.iNumCassa;
                    row["iScaricato"] = 0;
                    row["iAnnullato"] = 0;

                    ordiniTable.Rows.Add(row);

                    if (!String.IsNullOrEmpty(SF_Data.sTavolo)) // Tavolo
                    {
                        row = ordiniTable.NewRow();

                        row["iOrdine_ID"] = SF_Data.iNumOfLastReceipt;
                        row["sTipo_Articolo"] = _ORDER_CONST._TAVOLO;
                        //row["iPrezzo_Unitario"] = 0;
                        //row["iQuantita_Ordine"] = 0;
                        //row["iIndex_Listino"] = 0;
                        //row["iGruppo_Stampa"] = 0;
                        row["sText"] = SF_Data.sTavolo;
                        row["iNumCassa"] = SF_Data.iNumCassa;
                        row["iScaricato"] = 0;
                        row["iAnnullato"] = 0;

                        ordiniTable.Rows.Add(row);
                    }

                    if (!String.IsNullOrEmpty(SF_Data.sNome)) // Nome
                    {
                        row = ordiniTable.NewRow();

                        row["iOrdine_ID"] = SF_Data.iNumOfLastReceipt;
                        row["sTipo_Articolo"] = _ORDER_CONST._NOME;
                        //row["iPrezzo_Unitario"] = 0;
                        //row["iQuantita_Ordine"] = 0;
                        //row["iIndex_Listino"] = 0;
                        //row["iGruppo_Stampa"] = 0;
                        row["sText"] = SF_Data.sNome;
                        row["iNumCassa"] = SF_Data.iNumCassa;
                        row["iScaricato"] = 0;
                        row["iAnnullato"] = 0;

                        ordiniTable.Rows.Add(row);
                    }

                    if (!String.IsNullOrEmpty(SF_Data.sNota)) // Nota
                    {
                        row = ordiniTable.NewRow();

                        row["iOrdine_ID"] = SF_Data.iNumOfLastReceipt;
                        row["sTipo_Articolo"] = _ORDER_CONST._NOTA;
                        //row["iPrezzo_Unitario"] = 0;
                        //row["iQuantita_Ordine"] = 0;
                        //row["iIndex_Listino"] = 0;
                        //row["iGruppo_Stampa"] = 0;
                        row["sText"] = SF_Data.sNota;
                        row["iNumCassa"] = SF_Data.iNumCassa;
                        row["iScaricato"] = 0;
                        row["iAnnullato"] = 0;

                        ordiniTable.Rows.Add(row);
                    }

                    if (IsBitSet(SF_Data.iStatusSconto, BIT_SCONTO_STD))
                    {
                        row = ordiniTable.NewRow();

                        row["iOrdine_ID"] = SF_Data.iNumOfLastReceipt;
                        row["sTipo_Articolo"] = _ORDER_CONST._SCONTO;
                        row["iPrezzo_Unitario"] = SF_Data.iScontoStdReceipt;
                        //row["iQuantita_Ordine"] = 0;
                        //row["iIndex_Listino"] = 0;
                        //row["iGruppo_Stampa"] = 0;
                        row["sText"] = SF_Data.sScontoReceipt;
                        row["iStatus"] = SF_Data.iStatusSconto;
                        row["iNumCassa"] = SF_Data.iNumCassa;
                        row["iScaricato"] = 0;
                        row["iAnnullato"] = 0;

                        ordiniTable.Rows.Add(row);
                    }

                    if (IsBitSet(SF_Data.iStatusSconto, BIT_SCONTO_FISSO))
                    {
                        row = ordiniTable.NewRow();

                        row["iOrdine_ID"] = SF_Data.iNumOfLastReceipt;
                        row["sTipo_Articolo"] = _ORDER_CONST._SCONTO;
                        row["iPrezzo_Unitario"] = SF_Data.iScontoFissoReceipt;
                        //row["iQuantita_Ordine"] = 0;
                        //row["iIndex_Listino"] = 0;
                        //row["iGruppo_Stampa"] = 0;
                        row["sText"] = SF_Data.sScontoReceipt;
                        row["iStatus"] = SF_Data.iStatusSconto;
                        row["iNumCassa"] = SF_Data.iNumCassa;
                        row["iScaricato"] = 0;
                        row["iAnnullato"] = 0;

                        ordiniTable.Rows.Add(row);
                    }

                    if (IsBitSet(SF_Data.iStatusSconto, BIT_SCONTO_GRATIS))
                    {
                        row = ordiniTable.NewRow();

                        row["iOrdine_ID"] = SF_Data.iNumOfLastReceipt;
                        row["sTipo_Articolo"] = _ORDER_CONST._SCONTO;
                        row["iPrezzo_Unitario"] = SF_Data.iScontoGratisReceipt;
                        //row["iQuantita_Ordine"] = 0;
                        //row["iIndex_Listino"] = 0;
                        //row["iGruppo_Stampa"] = 0;
                        row["sText"] = SF_Data.sScontoReceipt;
                        row["iStatus"] = SF_Data.iStatusSconto;
                        row["iNumCassa"] = SF_Data.iNumCassa;
                        row["iScaricato"] = 0;
                        row["iAnnullato"] = 0;

                        ordiniTable.Rows.Add(row);
                    }

                    if (IsBitSet(SF_Data.iStatusReceipt, BIT_CARICATO_DA_WEB))
                    {
                        row = ordiniTable.NewRow();

                        row["iOrdine_ID"] = SF_Data.iNumOfLastReceipt;
                        row["sTipo_Articolo"] = _ORDER_CONST._NUM_ORD_WEB;
                        row["iPrezzo_Unitario"] = SF_Data.iNumOrdineWeb;
                        //row["iQuantita_Ordine"] = 0;
                        //row["iIndex_Listino"] = 0;
                        //row["iGruppo_Stampa"] = 0;
                        row["sText"] = SF_Data.sWebDateTime;
                        row["iNumCassa"] = SF_Data.iNumCassa;
                        row["iScaricato"] = 0;
                        row["iAnnullato"] = 0;

                        ordiniTable.Rows.Add(row);
                    }

                    if (IsBitSet(SF_Data.iStatusReceipt, BIT_CARICATO_DA_PREVENDITA))
                    {
                        row = ordiniTable.NewRow();

                        row["iOrdine_ID"] = SF_Data.iNumOfLastReceipt;
                        row["sTipo_Articolo"] = _ORDER_CONST._NUM_ORD_PREV;
                        row["iPrezzo_Unitario"] = SF_Data.iNumOrdinePrev;
                        //row["iQuantita_Ordine"] = 0;
                        //row["iIndex_Listino"] = 0;
                        //row["iGruppo_Stampa"] = 0;
                        row["sText"] = SF_Data.sPrevDateTime;
                        row["iNumCassa"] = SF_Data.iNumCassa;
                        row["iScaricato"] = 0;
                        row["iAnnullato"] = 0;

                        ordiniTable.Rows.Add(row);
                    }

                    for (i = 0; i < MAX_NUM_ARTICOLI; i++)
                    {
                        if (SF_Data.Articolo[i].iQuantitaOrdine > 0)
                        {
                            row = ordiniTable.NewRow();

                            row["iOrdine_ID"] = SF_Data.iNumOfLastReceipt;
                            row["sTipo_Articolo"] = SF_Data.Articolo[i].sTipo;
                            row["iNumCassa"] = SF_Data.iNumCassa;
                            row["iStatus"] = SF_Data.Articolo[i].iOptionsFlags;
                            row["iGruppo_Stampa"] = SF_Data.Articolo[i].iGruppoStampa;
                            row["sText"] = SF_Data.Articolo[i].sNotaArt;
                            row["iQuantita_Ordine"] = SF_Data.Articolo[i].iQuantitaOrdine;
                            row["iIndex_Listino"] = SF_Data.Articolo[i].iIndexListino;
                            row["iPrezzo_Unitario"] = SF_Data.Articolo[i].iPrezzoUnitario;
                            row["iScaricato"] = 0;
                            row["iAnnullato"] = 0;

                            ordiniTable.Rows.Add(row);
                        }
                    }

#if STANDFACILE
                    if (bCheckService(Define._AUTO_SEQ_TEST))
                    {
                        while (TestManager.sFakeItem.Count > 0)
                        {
                            row = ordiniTable.NewRow();

                            row["iOrdine_ID"] = SF_Data.iNumOfLastReceipt;
                            row["sTipo_Articolo"] = TestManager.sFakeItem[0].sItem;
                            row["iNumCassa"] = SF_Data.iNumCassa;
                            row["iGruppo_Stampa"] = DEST_TYPE.DEST_TIPO1;
                            row["iQuantita_Ordine"] = TestManager.sFakeItem[0].iQuantity;
                            row["iIndex_Listino"] = MAX_NUM_ARTICOLI - 1;
                            row["iPrezzo_Unitario"] = TestManager.sFakeItem[0].iUnitPrice;

                            row["iScaricato"] = 0;
                            row["iAnnullato"] = 0;

                            ordiniTable.Rows.Add(row);

                            TestManager.sFakeItem.RemoveAt(0);
                        }
                    }
#endif
                } // if (bDBConnection_Ok)

                if (bDBConnection_Ok)
                {
                    /*** aggiorna il database su disco ***/
                    dbDataAdapterInsert.UpdateBatchSize = 100;
                    iUpdatedRows = dbDataAdapterInsert.Update(ordiniTable);

                    sTmp = String.Format("dbSalvaOrdine : iUpdatedRows = {0}", iUpdatedRows);

                    Console.WriteLine(sTmp);

                    transaction.Commit();

                    dbDataAdapterInsert.Dispose();
                    dbDataAdapterSelect.Dispose();

                    LogToFile(sTmp);
                }
            }

            catch (Exception)
            {
                _WrnMsg.iErrID = WRN_DBE;
                _WrnMsg.sMsg = String.Format("dbSalvaOrdine : {0}", _bUSA_NDB());
                WarningManager(_WrnMsg);

                LogToFile("dbSalvaOrdine : dbException Ordini");
            }

            sTmp = String.Format("dbSalvaOrdine: record {0}", iUpdatedRows);
            LogToFile(sTmp);
        }

        /// <summary>Salva il messaggio corrente nel database</summary>
        public void dbSalvaMessaggio(String[] rVisMessaggiLines, String sNomeFileMsg)
        {

            bool bDBConnection_Ok;
            int i;
            String sTmp, sMessage, sQueryTxt;

            MySqlCommand cmd = new MySqlCommand();
            MySqlDataReader readerOrdine = null;

            bDBConnection_Ok = dbInit(getActualDate(), SF_Data.iNumCassa);

            // sicurezza : si prosegue solo se c'è la connessione a DB
            if (!bDBConnection_Ok)
                return;

            try
            {
                // prima prova di connessione al DB
                cmd.CommandText = "SELECT * FROM " + _sDBTNameOrdini + " WHERE iOrdine_ID = 0";
                cmd.Connection = _Connection;

                readerOrdine = cmd.ExecuteReader();
            }
            catch (Exception)
            {
            }

            try
            {
                // se il database non esiste ...
                if (readerOrdine == null)
                {
                    dbSalvaOrdine();
                }
                else
                    readerOrdine.Close(); // connessione rimane

                sMessage = ""; // nel DB si salvano 255 char max

                sTmp = sCenterJustify(SF_Data.sHeaders[0], iMAX_RECEIPT_CHARS);
                if (!String.IsNullOrEmpty(SF_Data.sHeaders[0]))
                    sMessage += String.Format("{0}\n\n", sTmp);

                sTmp = String.Format("{0,-22}C.{1}", GetDateTimeString(), SF_Data.iNumCassa);
                sTmp = sCenterJustify(sTmp, iMAX_RECEIPT_CHARS);
                sMessage += String.Format("{0}\n\n", sTmp);

                sTmp = String.Format("{0}{1,4}", "Messaggio Numero =", SF_Data.iNumOfMessages);
                sTmp = sCenterJustify(sTmp, iMAX_RECEIPT_CHARS);
                sMessage += String.Format("{0}\n\n", sTmp);

                for (i = 0; i < rVisMessaggiLines.Length; i++)
                    sMessage += String.Format(" {0}\n", rVisMessaggiLines[i]);

                sMessage = sMessage.Replace("'", "''"); // prepara la query

                // append continuo ordini, non serve KEY
                sQueryTxt = String.Format("INSERT INTO {0} (iOrdine_ID, sTipo_Articolo, sText, iNumCassa) VALUES " +
                    "({1}, \'{2}\', \'{3}\', {4});", _sDBTNameOrdini, -SF_Data.iNumOfMessages, sNomeFileMsg, sMessage, SF_Data.iNumCassa);

                cmd.Connection = _Connection;
                cmd.CommandText = sQueryTxt;
                var qResult = cmd.ExecuteScalar();

                LogToFile("dbSalvaMessaggio : aggiornamento record");
            }

            catch (Exception)
            {

                _WrnMsg.iErrID = WRN_DBE;
                _WrnMsg.sMsg = String.Format("dbSalvaMessaggio : {0}", _bUSA_NDB());
                WarningManager(_WrnMsg);

                LogToFile("dbSalvaMessaggio : dbException dbSalvaMessaggio");
            }

        }

        /// <summary>
        /// Funzione di salvataggio nel database del listino da parte della cassa primaria, <br/>
        /// a disposizione poi della cassa secondaria, solo con NDB
        /// </summary>
        public void dbSalvaListino()
        {
            int i, iNumRowsOrdini, iUpdatedRows = 0;

            bool bDBConnection_Ok;
            String sQueryTxt;
            String sInStr;

            StreamReader fprz = null;

            MySqlCommand cmd = new MySqlCommand();
            DataTable listinoTable = new DataTable();
            MySqlDataAdapter dbDataAdapterSelect = null;
            MySqlDataAdapter dbDataAdapterIns = null;

            DataRow row;

            bDBConnection_Ok = dbInit(getActualDate(), SF_Data.iNumCassa, true);

            // sicurezza : si prosegue solo se è CASSA_PRINCIPALE e c'è la connessione a DB
            if (!((SF_Data.iNumCassa == CASSA_PRINCIPALE) && bDBConnection_Ok))
                return;

            try
            {
                cmd.Connection = _Connection;

                // svuotamento tabella per riscrittura
                cmd.CommandText = "DROP TABLE IF EXISTS " + NOME_LISTINO_DBTBL;

                LogToFile("dbSalvaListino : prima di DROP TABLE");

                // svuotamento eventuale TRUNCATE non c'è in SQLite -> DROP & CREATE !

                var qResult = cmd.ExecuteScalar();
                LogToFile("dbSalvaListino : dopo DROP TABLE");

                // creazione tabella se non esiste
                sQueryTxt = String.Format(@"CREATE TABLE IF NOT EXISTS {0} (iRiga_ID INT NOT NULL, sText VARCHAR(100),
                                                PRIMARY KEY(iRiga_ID)); ", NOME_LISTINO_DBTBL);

                cmd.CommandText = sQueryTxt;
                qResult = cmd.ExecuteScalar();
                LogToFile("dbSalvaListino : dopo CREATE TABLE");

                // verifica se esiste la tabella
                sQueryTxt = "SELECT * FROM " + NOME_LISTINO_DBTBL;
                dbDataAdapterSelect = new MySqlDataAdapter(sQueryTxt, _Connection);

                iNumRowsOrdini = dbDataAdapterSelect.Fill(listinoTable);

            }
            catch (Exception)
            {
                _WrnMsg.iErrID = WRN_DBE;
                _WrnMsg.sMsg = String.Format("dbSalvaListino creazione tabella : {0}", NOME_LISTINO_DBTBL);
                WarningManager(_WrnMsg);
                LogToFile("dbSalvaListino : dbException creazione tabella");
            }

            try
            {
                MySqlTransaction transaction = _Connection.BeginTransaction();

                dbDataAdapterIns = new MySqlDataAdapter();

                sQueryTxt = "INSERT INTO " + NOME_LISTINO_DBTBL + " (iRiga_ID, sText) VALUES (@iRiga_ID, @sText);";

                dbDataAdapterIns.InsertCommand = new MySqlCommand(sQueryTxt, _Connection);

                dbDataAdapterIns.InsertCommand.Parameters.Add("@iRiga_ID", MySqlType.Int, 10, "iRiga_ID");
                dbDataAdapterIns.InsertCommand.Parameters.Add("@sText", MySqlType.VarChar, 50, "sText");
                dbDataAdapterIns.InsertCommand.UpdatedRowSource = UpdateRowSource.None;

#if STANDFACILE
                String sNomeFilePrezzi, sDir = "";

                sDir = DataManager.sGetExeDir() + "\\";

                sNomeFilePrezzi = NOME_FILE_LISTINO;
                fprz = File.OpenText(sDir + NOME_FILE_LISTINO);

                _ErrMsg.sNomeFile = sNomeFilePrezzi;
#endif

                if (fprz == null)
                {
                    _ErrMsg.iErrID = WRN_FNO;
                    WarningManager(_ErrMsg);
                }

                i = 0;

                while (((sInStr = fprz.ReadLine()) != null) && (i < 1000))
                {
                    i++;

                    row = listinoTable.NewRow();

                    row["iRiga_ID"] = i;
                    row["sText"] = sInStr;

                    listinoTable.Rows.Add(row);
                }

                /*** aggiorna il database su disco ***/
                dbDataAdapterIns.UpdateBatchSize = 100;
                iUpdatedRows = dbDataAdapterIns.Update(listinoTable);

                Console.WriteLine("dbSalvaListino : iUpdatedRows = {0}", iUpdatedRows);

                transaction.Commit();
            }

            catch (Exception)
            {
                _WrnMsg.iErrID = WRN_DBE;
                _WrnMsg.sMsg = String.Format("dbSalvaListino popolazione tabella : {0}", _bUSA_NDB());
                WarningManager(_WrnMsg);
                LogToFile("dbSalvaListino : dbException popolazione tabella");
            }

            dbDataAdapterIns.Dispose();
            dbDataAdapterSelect.Dispose();

            fprz.Close();
        }

        /// <summary>
        /// Funzione di salvataggio nel database del test <br/>
        /// da parte della cassa primaria, a disposizione poi <br/>
        /// della cassa secondaria, solo con NDB
        /// </summary>
        public void dbSalvaTest()
        {
            int i, iNumRowsOrdini, iUpdatedRows = 0;

            bool bDBConnection_Ok;
            String sQueryTxt;
            String sInStr, sDir = "";

            StreamReader fTest = null;

            MySqlCommand cmd = new MySqlCommand();
            DataTable testTable = new DataTable();
            MySqlDataAdapter dbDataAdapterSelect = null;
            MySqlDataAdapter dbDataAdapterIns = null;

            DataRow row;

            bDBConnection_Ok = dbInit(getActualDate(), SF_Data.iNumCassa, true);

            // sicurezza : si prosegue solo se è CASSA_PRINCIPALE e c'è la connessione a MySQL
            if (!((SF_Data.iNumCassa == CASSA_PRINCIPALE) && bDBConnection_Ok))
                return;

            try
            {
                cmd.Connection = _Connection;

                // svuotamento tabella per riscrittura
                cmd.CommandText = "DROP TABLE IF EXISTS " + NOME_TEST_DBTBL;

                LogToFile("dbSalvaTest : prima di DROP TABLE");

                // svuotamento eventuale TRUNCATE non c'è in SQLite -> DROP & CREATE !

                var qResult = cmd.ExecuteScalar();
                LogToFile("dbSalvaTest : dopo DROP TABLE");

                // creazione tabella se non esiste
                sQueryTxt = String.Format(@"CREATE TABLE IF NOT EXISTS {0} (iRiga_ID INT NOT NULL, sText VARCHAR(100),
                                                PRIMARY KEY(iRiga_ID)); ", NOME_TEST_DBTBL);


                cmd.CommandText = sQueryTxt;
                qResult = cmd.ExecuteScalar();
                LogToFile("dbSalvaTest : dopo CREATE TABLE");

                // verifica se esiste la tabella
                sQueryTxt = "SELECT * FROM " + NOME_TEST_DBTBL;
                dbDataAdapterSelect = new MySqlDataAdapter(sQueryTxt, _Connection);

                iNumRowsOrdini = dbDataAdapterSelect.Fill(testTable);

            }
            catch (Exception)
            {
                _WrnMsg.iErrID = WRN_DBE;
                _WrnMsg.sMsg = String.Format("dbSalvaTest creazione tabella : {0}", NOME_TEST_DBTBL);
                WarningManager(_WrnMsg);
                LogToFile("dbSalvaTest : dbException creazione tabella");
            }

            try
            {
                MySqlTransaction transaction = _Connection.BeginTransaction();

                dbDataAdapterIns = new MySqlDataAdapter();

                sQueryTxt = "INSERT INTO " + NOME_TEST_DBTBL + " (iRiga_ID, sText) VALUES (@iRiga_ID, @sText);";

                dbDataAdapterIns.InsertCommand = new MySqlCommand(sQueryTxt, _Connection);

                dbDataAdapterIns.InsertCommand.Parameters.Add("@iRiga_ID", MySqlType.Int, 10, "iRiga_ID");
                dbDataAdapterIns.InsertCommand.Parameters.Add("@sText", MySqlType.VarChar, 50, "sText");
                dbDataAdapterIns.InsertCommand.UpdatedRowSource = UpdateRowSource.None;

#if STANDFACILE
                sDir = DataManager.sGetExeDir() + "\\";

                _ErrMsg.sNomeFile = NOME_FILE_TEST;
#endif

                fTest = File.OpenText(sDir + NOME_FILE_TEST);
                if (fTest == null)
                {
                    _ErrMsg.iErrID = WRN_FNO;
                    WarningManager(_ErrMsg);
                }

                i = 0;

                while (((sInStr = fTest.ReadLine()) != null) && (i < 1000))
                {
                    i++;

                    // omette le righe vuote e di commento
                    if (string.IsNullOrEmpty(sInStr) || sInStr.StartsWith(";"))
                        continue;

                    row = testTable.NewRow();

                    row["iRiga_ID"] = i;
                    row["sText"] = sInStr;

                    testTable.Rows.Add(row);
                }

                /*** aggiorna il database su disco ***/
                dbDataAdapterIns.UpdateBatchSize = 100;
                iUpdatedRows = dbDataAdapterIns.Update(testTable);

                Console.WriteLine("dbSalvaTest : iUpdatedRows = {0}", iUpdatedRows);

                transaction.Commit();

                dbDataAdapterIns.Dispose();
                dbDataAdapterSelect.Dispose();
            }

            catch (Exception)
            {
                _WrnMsg.iErrID = WRN_DBE;
                _WrnMsg.sMsg = String.Format("dbSalvaTest popolazione tabella : {0}", _bUSA_NDB());
                WarningManager(_WrnMsg);
                LogToFile("dbSalvaTest : dbException popolazione tabella");
            }

            dbDataAdapterIns.Dispose();
            dbDataAdapterSelect.Dispose();

            fTest.Close();
        }

        /// <summary>
        /// aggiunge il suffisso alle tabelle <br/>
        /// usato da chiudiIncasso()
        /// </summary>

    }
}

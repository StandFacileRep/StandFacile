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
#pragma warning disable IDE0059
#pragma warning disable IDE1006

    /// <summary>classe per la gestione di MySQL</summary>
    public partial class dBaseIntf_my
    {
        /// <summary> costruisce la tabella del venduto </summary>
        public void dbBuildMonitorTable()
        {
            bool bRecordExists;
            bool bDBConnected = false;

            int iArrayIndex, i, j, iNumCassa;
            int iNumScontriniTmp;

            String sTmpStr;
            String sQuery, sTmpFilter;
            String sFilter = "";

            _iNumScontrini = 0;

            /*************************************************
                lettura prima tabella in DB_Data.Articolo[]
             *************************************************/
            try
            {
                bDBConnected = _rdBaseIntf.dbInit(GetActualDate(), CASSA_PRINCIPALE);

                if (bDBConnected)
                {
                    // inizializzazioni
                    iArrayIndex = 0;
                    dbAzzeraDatiOrdine(ref DB_Data);

                    /****************************************************
                         vista tabella ordinata e costruzione filtro
                     ****************************************************/

                    j = 0;
                    sTmpFilter = "";
                    for (i = 0; i < NUM_EDIT_GROUPS; i++)
                    {
                        if (NetConfigLightDlg.GetCopiaGroup(i))
                        {
                            sTmpFilter = String.Format("iGruppo_Stampa = {0}", i);
                            j = i;
                            break;
                        }
                    }

                    sFilter = sTmpFilter;
                    for (i = j + 1; i < NUM_EDIT_GROUPS; i++)
                    {
                        if (NetConfigLightDlg.GetCopiaGroup(i))
                        {
                            sTmpFilter = String.Format(" OR iGruppo_Stampa ={0}", i);
                            sFilter += sTmpFilter;
                        }
                    }

                    sQuery = "SELECT * FROM " + _sDBTNameDati + " WHERE (iRiga_ID >= 0) AND (iPrezzo_Unitario > 0) ";

                    if (!String.IsNullOrEmpty(sFilter))
                        sQuery += "AND (" + sFilter + ")";

                    MySqlCommand cmd = new MySqlCommand
                    {
                        Connection = _Connection,
                        CommandText = sQuery
                    };

                    MySqlDataReader readerDatiCassa;

                    readerDatiCassa = cmd.ExecuteReader();

                    if (readerDatiCassa.HasRows)
                    {
                        while (readerDatiCassa.Read() && (iArrayIndex < MAX_NUM_ARTICOLI))
                        {
                            sTmpStr = readerDatiCassa.GetString("sTipo_Articolo");

                            if (sTmpStr.Contains(SHMAGIC))
                                continue;

                            // else
                            DB_Data.Articolo[iArrayIndex].sTipo = sTmpStr;

                            // somma iQuantitaVenduta
                            DB_Data.Articolo[iArrayIndex].iQuantitaVenduta = readerDatiCassa.GetInt32("iQuantita_Venduta");

                            DB_Data.Articolo[iArrayIndex].iQuantita_Scaricata = readerDatiCassa.GetInt32("iQuantita_Scaricata");
                            DB_Data.Articolo[iArrayIndex].iGruppoStampa = readerDatiCassa.GetInt32("iGruppo_Stampa");

                            sTmpStr = readerDatiCassa.GetString("sDisponibilita");

                            if (sTmpStr == "OK")
                                DB_Data.Articolo[iArrayIndex].iDisponibilita = DISP_OK;
                            else
                                DB_Data.Articolo[iArrayIndex].iDisponibilita = Convert.ToInt32(sTmpStr);

                            iArrayIndex++;
                        }
                    }

                    readerDatiCassa.Close();

                    cmd.Dispose();

                    sQuery = "SELECT iQuantita_Venduta FROM " + _sDBTNameDati + " WHERE (iRiga_ID = -8)";

                    // recupera il numero dello scontrino
                    MySqlCommand cmdNSc = new MySqlCommand
                    {
                        Connection = _Connection,
                        CommandText = sQuery
                    };

                    //ExecuteScalar will return one value
                    _iNumScontrini = int.Parse(cmdNSc.ExecuteScalar() + "");

                    cmdNSc.Dispose();
                }
            }
            catch (Exception)
            {
                bDBConnected = false;

                if (_bErrorePrimaVolta)
                {
                    _WrnMsg.sMsg = _sDBTNameDati;
                    _WrnMsg.iErrID = WRN_TDQ;
                    WarningManager(_WrnMsg);

                    _bErrorePrimaVolta = false;
                }

                LogToFile("Timer_MainLoop : dbException prima tabella");
            }

            /*********************************************************************
             *   lettura delle tabelle secondarie (non è detto che esistano)
             *   per caricamento in DB_Data.Articolo[]
             *********************************************************************/

            if (bDBConnected)
            {
                List<String> Tablenames = new List<String>();

                sQuery = "SHOW TABLES";

                MySqlCommand cmd = new MySqlCommand()
                {
                    Connection = _Connection,
                    CommandText = sQuery,
                };

                cmd.ExecuteScalar();

                MySqlDataReader readerTable = cmd.ExecuteReader();

                while (readerTable.Read())
                    Tablenames.Add(readerTable.GetString(0));

                readerTable.Close();

                for (iNumCassa = CASSA_PRINCIPALE + 1; iNumCassa <= (MAX_CASSE_SECONDARIE + 1); iNumCassa++)
                {
                    try
                    {
                        _sDBTNameDati = GetNomeDatiDBTable(iNumCassa, GetActualDate());

                        if (Tablenames.Contains(_sDBTNameDati))// la tabella esiste
                        {
                            sQuery = "SELECT * FROM " + _sDBTNameDati + " WHERE (iRiga_ID >= 0) AND (iPrezzo_Unitario > 0) ";

                            if (!String.IsNullOrEmpty(sFilter))
                                sQuery += "AND (" + sFilter + ")";

                            cmd.CommandText = sQuery;
                            MySqlDataReader readerDatiCassaSec = cmd.ExecuteReader();

                            iArrayIndex = 0;

                            if (readerDatiCassaSec.HasRows)
                            {
                                while (readerDatiCassaSec.Read() && (iArrayIndex < MAX_NUM_ARTICOLI))
                                {
                                    sTmpStr = readerDatiCassaSec.GetString("sTipo_Articolo");

                                    if (sTmpStr.Contains(SHMAGIC) || (readerDatiCassaSec.GetInt32("iQuantita_Venduta") == 0))
                                        continue;

                                    // else
                                    bRecordExists = false;

                                    // ricerca corrispondenza tipo
                                    for (j = 0; j < MAX_NUM_ARTICOLI; j++)
                                        if ((sTmpStr == DB_Data.Articolo[j].sTipo) && !String.IsNullOrEmpty(sTmpStr))
                                        {
                                            // somma i dati delle tabelle
                                            DB_Data.Articolo[j].iQuantitaVenduta += readerDatiCassaSec.GetInt32("iQuantita_Venduta");
                                            DB_Data.Articolo[j].iQuantita_Scaricata += readerDatiCassaSec.GetInt32("iQuantita_Scaricata");

                                            // DB_Data.Articolo[j].iDisponibilita teniamo quella della cassa principale
                                            // DB_Data.Articolo[j].iGruppoStampa  teniamo quella della cassa principale

                                            bRecordExists = true;
                                            break;
                                        }

                                    // Append nell' Array, iArrayIndex non va inizializzato
                                    if (!bRecordExists)
                                    {
                                        DB_Data.Articolo[iArrayIndex].sTipo = sTmpStr;

                                        // somma iQuantitaVenduta
                                        DB_Data.Articolo[iArrayIndex].iQuantitaVenduta = readerDatiCassaSec.GetInt32("iQuantita_Venduta");

                                        DB_Data.Articolo[iArrayIndex].iQuantita_Scaricata = readerDatiCassaSec.GetInt32("iQuantita_Scaricata");
                                        DB_Data.Articolo[iArrayIndex].iGruppoStampa = readerDatiCassaSec.GetInt32("iGruppo_Stampa");

                                        sTmpStr = readerDatiCassaSec.GetString("sDisponibilita");

                                        if (sTmpStr == "OK")
                                            DB_Data.Articolo[iArrayIndex].iDisponibilita = DISP_OK;
                                        else
                                            DB_Data.Articolo[iArrayIndex].iDisponibilita = Convert.ToInt32(sTmpStr);

                                        iArrayIndex++;
                                    }
                                }
                            }

                            readerDatiCassaSec.Close();

                            // recupera il numero dello scontrino
                            MySqlCommand cmdNSc = new MySqlCommand
                            {
                                Connection = _Connection,
                                CommandText = "SELECT iQuantita_Venduta FROM " + _sDBTNameDati + " WHERE (iRiga_ID = -8)"
                            };

                            iNumScontriniTmp = int.Parse(cmdNSc.ExecuteScalar() + "");

                            if (_iNumScontrini < iNumScontriniTmp)
                                _iNumScontrini = iNumScontriniTmp;

                            cmdNSc.Dispose();

                        } // if la tabella _sDBTNameDati esiste

                    }
                    catch (Exception)
                    {
                        // bDatabaseConnected = false;
                        // LogServer->LogToFile("Timer_MainLoop : dbException seconda tabella");
                    }

                } // ciclo for casse secondarie

                cmd.Dispose();

            } // end if (bDatabaseConnected)
            else
            {
                // try recovery
                if (!NetConfigLightDlg.rNetConfigLightDlg.Visible)
                    _rdBaseIntf.dbCheck(_sDB_ServerName, _password, _iNDbMode, true);
            }

            _Connection?.Close();
        }

        /// <summary> 
        /// costruisce la tabella ordini evasi: <br/>
        /// se bFormIsVisibleParam == true ha senso aggiornare il form,<br/> 
        /// altrimenti si limita a ricavare gli ultimi ordini evasi
        /// </summary>
        public DataSet dbOrdiniMonitorList(bool checkBoxParam, bool bFormIsVisibleParam)
        {
            int i, j, k, iAttesa, iAttesaMedia, iNumDivCount;
            int iMinText, iMinScaricato, iHtext, iHScaricato;
            int[] _iAttesa = new int[NUM_ULTIMI_SCONTRINI];
            bool bNumDiverso;
            String sTmpStr, sTmpStr2;
            String sQuery;
            MySqlDataAdapter adapter;

            DataSet DS = new DataSet();

            if (bFormIsVisibleParam)
            {
                j = 0;
                sTmpStr2 = "(";
                for (i = 0; i < NUM_EDIT_GROUPS; i++)
                {
                    if (NetConfigLightDlg.GetCopiaGroup(i))
                    {
                        sTmpStr2 = String.Format("(iGruppo_Stampa = {0})", i);
                        j = i;
                        break;
                    }
                }

                sTmpStr = sTmpStr2;
                for (i = j + 1; i < NUM_EDIT_GROUPS; i++)
                {
                    if (NetConfigLightDlg.GetCopiaGroup(i))
                    {
                        sTmpStr2 = String.Format(" OR (iGruppo_Stampa = {0})", i);
                        sTmpStr += sTmpStr2;
                    }
                }

                sQuery = "SELECT iOrdine_ID, sTipo_Articolo, iQuantita_Ordine, iGruppo_Stampa, sScaricato, iNumCassa, iStatus, iScaricato";
                sQuery += (" FROM " + _sDBTNameOrdini);

                if (checkBoxParam)
                {
                    sQuery += " WHERE ((iQuantita_Ordine > 0) OR (sTipo_Articolo = \'_StartOfOrder_#@$_\')) AND (iOrdine_ID > 0) AND (iAnnullato = 0) AND (iScaricato = 0) AND (" + sTmpStr + " OR (sTipo_Articolo = '_StartOfOrder_#@$_'))";
                    sQuery += " ORDER BY iOrdine_ID ASC, iGruppo_Stampa ASC";
                }
                else
                {
                    sQuery += " WHERE ((iQuantita_Ordine > 0) OR (sTipo_Articolo = \'_StartOfOrder_#@$_\')) AND (iOrdine_ID > 0) AND (iAnnullato = 0) AND (" + sTmpStr + " OR (sTipo_Articolo = '_StartOfOrder_#@$_'))";
                    sQuery += " ORDER BY iOrdine_ID DESC, iGruppo_Stampa ASC";
                }

                adapter = new MySqlDataAdapter(sQuery, _Connection);
                adapter.Fill(DS);

                // bool per cancellazione ordini "vuoti" che non interessano
                List<TReceiptListItem> sTableItems = new List<TReceiptListItem>();

                String sTmp;
                i = 0;
                foreach (DataRow dtr in DS.Tables[0].Rows)
                {
                    sTmp = (String)dtr[1];

                    if (sTmp == ORDER_CONST._START_OF_ORDER)
                    {
                        // sostituzione stringa
                        sTmp = sTmp.Replace(ORDER_CONST._START_OF_ORDER, ORDER_START_STR);
                        dtr[1] = sTmp;
                    }

                    TReceiptListItem sTmptableItem = new TReceiptListItem
                    {
                        iReceiptNum = (int)dtr[0],
                        sArticoloRiga = (String)dtr[1]
                    };

                    sTableItems.Add(sTmptableItem);
                    i++;
                }

                // cancellazione "ordini vuoti"
                i = 0;
                foreach (DataRow dtr in DS.Tables[0].Rows)
                {
                    if ((i < (DS.Tables[0].Rows.Count - 1)) && (sTableItems[i].sArticoloRiga == ORDER_START_STR) &&
                       (sTableItems[i].sArticoloRiga == sTableItems[i + 1].sArticoloRiga) && (sTableItems[i].iReceiptNum != sTableItems[i + 1].iReceiptNum))
                    {
                        sTmp = String.Format("{0} {1}", (int)dtr[0], (String)dtr[1]);
                        dtr.Delete();
                    }

                    i++;
                }

                DS.AcceptChanges();
            }

            sQuery = "SELECT * FROM " + _sDBTNameOrdini + " WHERE (sTipo_Articolo = '" + ORDER_CONST._START_OF_ORDER + "') AND (iScaricato = 1) ";
            sQuery += " ORDER BY sScaricato DESC LIMIT " + NUM_ULTIMI_SCONTRINI.ToString();

            MySqlCommand cmd = new MySqlCommand()
            {
                Connection = _Connection,
                CommandText = sQuery
            };

            MySqlDataReader readerScaricati = cmd.ExecuteReader();

            for (k = NUM_ULTIMI_SCONTRINI - 1; k >= 0; k--)
                _sNumScontrino[k] = " ";

            if (readerScaricati.HasRows)
            {

                iAttesaMedia = 0;
                iMinText = 0;
                iMinScaricato = 0;
                iNumDivCount = 0;

                for (k = NUM_ULTIMI_SCONTRINI - 1; k >= 0; k--)
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
                        for (j = NUM_ULTIMI_SCONTRINI - 1; j >= 0; j--)
                            if (_sNumScontrino[j] == i.ToString())
                                bNumDiverso = false;

                        if (bNumDiverso)
                        {
                            _sNumScontrino[k] = i.ToString();
                            _iAttesa[k] = iMinScaricato - iMinText;
                            iAttesaMedia += _iAttesa[k];
                            iNumDivCount++;
                            break;
                        }
                    }
                }

                if (iNumDivCount > 0)
                    _sAttesaMedia = Convert.ToString(Math.Round((float)(iAttesaMedia / iNumDivCount)));
                else
                    _sAttesaMedia = "xx";
            }

            readerScaricati.Close();

            return DS;
        }

    }
}

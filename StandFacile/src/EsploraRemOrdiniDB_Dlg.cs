/********************************************************************
  	NomeFile : StandFacile/EsploraRemOrdiniDB_Dlg.cs
	Data	 : 20.03.2025
  	Autore   : Mauro Artuso

  Classe di esplorazione del database remoto, 
  ha senso chiamarla solo se il database remoto è selezionato
 ********************************************************************/

using System;
using System.Threading;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;

using static StandCommonFiles.CommonCl;
using static StandCommonFiles.LogServer;

using static StandFacile.Define;
using static StandCommonFiles.ComDef;
using static StandFacile.dBaseTunnel_my;

namespace StandFacile
{
    /// <summary>classe per l'esplorazione del database ordini remoti</summary>
    public partial class EsploraRemOrdiniDB_Dlg : Form
    {
#pragma warning disable IDE0059
#pragma warning disable IDE1006

        const int REFRESH_PERIOD = 4 * 60; // 60s
        const int REFRESH_PERIOD_QUICK = 4 * 2; // 2s

        bool bPrimaVolta;

        int iGridStringsCount, _iDBGridRowIndex;
        static int iTableAutoLoadPeriod = REFRESH_PERIOD;

        ulong ulStart, ulStop, ulPingTime;

        static bool _bProcessingOrder = false;

        /// <summary>struct per la gestione degli avvisi e/o errori</summary>
        public static TErrMsg _ErrMsg, _WrnMsg;

        /// <summary>riferimento</summary>
        public static EsploraRemOrdiniDB_Dlg rEsploraRemOrdiniDB_Dlg;

        // gestione cross thread
        static readonly Queue eventQueue = new Queue();

        /// <summary>mette eventi da dBaseTunnel_my in coda cross thread</summary>
        public static void EventEnqueue(String[] sEvQueueObj) { eventQueue.Enqueue(sEvQueueObj); }

        /// <summary>ottiene lo stato di ckBoxAuto</summary>
        public bool GetAutoCheckbox() { return ckBoxAutoLoad.Checked; }

        /// <summary>imposta _bProcessingOrder = false per proseguire con la stampa automatica</summary>
        public static void KeepPrintWebOrders() { _bProcessingOrder = false; }

        readonly ToolTip _tt = new ToolTip
        {
            InitialDelay = 50,
            ShowAlways = true
        };

        /// <summary>costruttore</summary>
        public EsploraRemOrdiniDB_Dlg()
        {
            InitializeComponent();

            rEsploraRemOrdiniDB_Dlg = this;

            // pulisce in caso siano stati inseriti ordini mediante barcode
            eventQueue.Clear();

            dbGrid.Focus();
            _iDBGridRowIndex = 0;

            bPrimaVolta = true;

            _tt.SetToolTip(BtnRem_Canc, "contrassegna come cancellato l'ordine web selezionato");
            _tt.SetToolTip(BtnRem_Load, "Carica nella griglia e visualizza l'ordine web selezionato");
            _tt.SetToolTip(dbConnStatusBox, "Stato della connessione al database remoto e/o attività in corso");

            //if (SF_Data.iNumCassa == CASSA_PRINCIPALE)
            //{
            BtnRem_Canc.Enabled = true;
            BtnRem_Load.Enabled = true;
            dbGrid.Enabled = true;
            //}
            //else
            //{
            //    // solo visualizzazione, escluso caricamento ordine remoto
            //    BtnRem_Canc.Enabled = false;
            //    BtnRem_Load.Enabled = false;
            //    dbGrid.Enabled = false;
            //}

            if (glb.SF_Data.iNumCassa == CASSA_PRINCIPALE)
                ckBoxAutoLoad.Enabled = true;
            else
                ckBoxAutoLoad.Enabled = false;

            RefreshTable();
        }

        /// <summary>inizializzazione della form per l'esplorazione del database remoto</summary>
        public static void Init()
        {
            rEsploraRemOrdiniDB_Dlg.dbConnStatusBox.Image = Properties.Resources.circleRed;

            rEsploraRemOrdiniDB_Dlg.RefreshTable();
            rEsploraRemOrdiniDB_Dlg._iDBGridRowIndex = 0;

            Thread.Sleep(200);
        }

        /// <summary>
        /// funzione che imposta la variabile per il refresh veloce della tabella per l'esplorazione del database remoto
        /// </summary>
        public static void RefreshTableRequest()
        {
            iTableAutoLoadPeriod = REFRESH_PERIOD_QUICK;
        }

        /// <summary>ricarica il contenuto della tabella per l'esplorazione del database remoto</summary>
        void RefreshTable()
        {
            bool bDbRead_Ok;
            int i, iDebug, iMainFormEventQueueCount;
            string sTmp;

            if (!Visible)
                return;

            dbGrid.RowCount = 1;
            dbGrid.ColumnCount = 6; // numero delle colonne della griglia

            dbGrid.Rows[0].Cells[0].Value = "";
            dbGrid.Rows[0].Cells[1].Value = "";
            dbGrid.Rows[0].Cells[2].Value = "";
            dbGrid.Rows[0].Cells[3].Value = "";
            dbGrid.Rows[0].Cells[4].Value = "";
            dbGrid.Rows[0].Cells[5].Value = "";

            iGridStringsCount = 0;

            iDebug = _sWebOrdersList.Count; // debug

            iMainFormEventQueueCount = FrmMain.GetEventQueueCount();

            ulStart = (ulong)Environment.TickCount;

            rdbPing();

            dbConnStatusBox.Image = Properties.Resources.circleGreen;
            ulStop = (ulong)Environment.TickCount;
            ulPingTime = ulStop - ulStart;
            labelQueryTime.Text = String.Format("tempo risposta server: {0} ms", ulPingTime);

            for (i = 0; i < _sWebOrdersList.Count; i++)
            {
                if (radioBtn0.Checked ||                                                                    //  tutti
                    radioBtn1.Checked && !IsBitSet(_sWebOrdersList[i].iStatus, (int)STATUS_FLAGS.BIT_ORDINE_DIRETTO_DA_WEB) || // solo pre-ordini
                    radioBtn2.Checked && IsBitSet(_sWebOrdersList[i].iStatus, (int)STATUS_FLAGS.BIT_ORDINE_DIRETTO_DA_WEB))   // solo ordini diretti autorizzati
                {
                    dbGrid.RowCount = iGridStringsCount + 1;

                    dbGrid.Rows[iGridStringsCount].Cells[0].Value = _sWebOrdersList[i].iNumOrdine;
                    dbGrid.Rows[iGridStringsCount].Cells[1].Value = _sWebOrdersList[i].sDateTime;
                    dbGrid.Rows[iGridStringsCount].Cells[2].Value = _sWebOrdersList[i].iNumCoperti;
                    dbGrid.Rows[iGridStringsCount].Cells[3].Value = IntToEuro(_sWebOrdersList[i].iTotaleReceipt);
                    dbGrid.Rows[iGridStringsCount].Cells[4].Value = _sWebOrdersList[i].sCliente;
                    dbGrid.Rows[iGridStringsCount].Cells[5].Value = _sWebOrdersList[i].sChecksum;
                    dbGrid.Rows[iGridStringsCount].Height = 26;

                    if (IsBitSet(_sWebOrdersList[i].iStatus, (int)STATUS_FLAGS.BIT_ORDINE_DIRETTO_DA_WEB))
                    {
                        dbGrid.Rows[iGridStringsCount].DefaultCellStyle.ForeColor = Color.Black;
                        dbGrid.Rows[iGridStringsCount].DefaultCellStyle.BackColor = Color.LightYellow;
                    }
                    else
                    {
                        dbGrid.Rows[iGridStringsCount].DefaultCellStyle.ForeColor = Color.White;
                        dbGrid.Rows[iGridStringsCount].DefaultCellStyle.BackColor = Color.Teal;
                    }

                    // evita di caricare la coda di eventi quando è ancora in elaborazione
                    if (IsBitSet(_sWebOrdersList[i].iStatus, (int)STATUS_FLAGS.BIT_ORDINE_DIRETTO_DA_WEB) && (iMainFormEventQueueCount == 0) &&
                        ckBoxAutoLoad.Checked && (_bProcessingOrder == false))
                    {
                        ulStart = (ulong)Environment.TickCount;

                        bDbRead_Ok = rdbCaricaOrdine(_sWebOrdersList[i].iNumOrdine);

                        dbConnStatusBox.Image = Properties.Resources.circleGreen;
                        ulStop = (ulong)Environment.TickCount;
                        ulPingTime = ulStop - ulStart;
                        labelQueryTime.Text = String.Format("tempo risposta server: {0} ms", ulPingTime);

                        sTmp = String.Format("rdbCaricaOrdine : {0} ms", ulPingTime);
                        LogToFile(sTmp);

                        // così ci passa una volta sola
                        _bProcessingOrder = true;

                        // se non si può caricare ad esempio per checksum errato lo annulla
                        //if (!bDbRead_Ok && !RDB_Data.bAnnullato)
                        //    bResult = rdbAnnullaOrdine(_sWebOrdersList[i].iNumOrdine);

                        if (bDbRead_Ok)
                        {
                            if (FrmMain.rFrmMain.GetAnteprima_TP_IsZero())
                            {
                                DataManager.CaricaOrdineWeb();

                                // avvia la stampa dell'ordine
                                String[] sQueue_Object = new String[2] { WEB_ORDER_PRINT_START, _sWebOrdersList[i].iNumOrdine.ToString() };

                                FrmMain.EventEnqueue(sQueue_Object);
                                iTableAutoLoadPeriod = REFRESH_PERIOD_QUICK;

                                LogToFile("EsploraRemOrdiniDB_Dlg : stampa automatica dell'ordine web", true);
                            }
                            else
                            {
                                // non è possibile continuare
                                _bProcessingOrder = false;

                                iTableAutoLoadPeriod = REFRESH_PERIOD;
                            }
                        }
                        else
                        {
                            // caricamento ordine fallito
                            _bProcessingOrder = false;
                            _WrnMsg.iErrID = WRN_DBE;
                            _WrnMsg.sMsg = String.Format("rdbCaricaOrdine fallito:\n\nrecord n. {0}", _sWebOrdersList[i].iNumOrdine);
                            WarningManager(_WrnMsg);

                            LogToFile("EsploraRemOrdiniDB_Dlg : rdbCaricaOrdine");
                        }
                    }

                    iGridStringsCount++;
                }
            }

            if ((_iDBGridRowIndex > 0) && (_iDBGridRowIndex < dbGrid.Rows.Count))
                dbGrid.CurrentCell = dbGrid.Rows[_iDBGridRowIndex].Cells[0];
            else if ((_iDBGridRowIndex > 0) && (_iDBGridRowIndex == dbGrid.Rows.Count))
                dbGrid.CurrentCell = dbGrid.Rows[_iDBGridRowIndex - 1].Cells[0]; // cancellata l'ultima riga

            dbGrid.Refresh();
            dbConnStatusBox.Refresh();

            FormEsplora_Resize(this, null);
        }

        /// <summary>ridisegna l'aspetto della tabella per l'esplorazione del database remoto</summary>
        private void FormEsplora_Resize(object sender, EventArgs e)
        {
            int iRowsHeight;

            float fWidth;
            float fFontHeaderHeight, fFontHeight;

            // Posizione griglia
            dbGrid.Height = this.Height - 250;
            //OrdiniGrid.Width = this.Width - 50;

            if ((dbGrid.ColumnCount > 0) && bPrimaVolta)// altrimenti genera eccezione
            {
                bPrimaVolta = false;

                fWidth = dbGrid.Width;
                dbGrid.Columns[0].Width = (int)(fWidth * 0.12f); // num
                dbGrid.Columns[1].Width = (int)(fWidth * 0.32f); // data e ora
                dbGrid.Columns[2].Width = (int)(fWidth * 0.14f); // coperti
                dbGrid.Columns[3].Width = (int)(fWidth * 0.14f); // importo
                dbGrid.Columns[4].Width = (int)(fWidth * 0.14f); // ID_utente
                dbGrid.Columns[5].Width = (int)(fWidth * 0.14f); // checksum

                // imposta Font sulla base della larghezza della finestra
                fFontHeight = ((float)dbGrid.Width) / 60;
                dbGrid.Font = new System.Drawing.Font(dbGrid.DefaultCellStyle.Font.Name, fFontHeight);

                // fFontHeaderHeight = ((float)dbGrid.Width) / 50;
                fFontHeaderHeight = fFontHeight;

                dbGrid.Columns[0].HeaderCell.Style.Font = new System.Drawing.Font(dbGrid.DefaultCellStyle.Font.Name, fFontHeaderHeight);
                dbGrid.Columns[1].HeaderCell.Style.Font = dbGrid.Columns[0].HeaderCell.Style.Font;
                dbGrid.Columns[2].HeaderCell.Style.Font = dbGrid.Columns[0].HeaderCell.Style.Font;
                dbGrid.Columns[3].HeaderCell.Style.Font = dbGrid.Columns[0].HeaderCell.Style.Font;
                dbGrid.Columns[4].HeaderCell.Style.Font = dbGrid.Columns[0].HeaderCell.Style.Font;
                dbGrid.Columns[5].HeaderCell.Style.Font = dbGrid.Columns[0].HeaderCell.Style.Font;

                dbGrid.Columns[0].HeaderText = "ord num";
                dbGrid.Columns[1].HeaderText = "data ed ora";
                dbGrid.Columns[2].HeaderText = "coperti";
                dbGrid.Columns[3].HeaderText = "Totale";
                dbGrid.Columns[4].HeaderText = "ID_utente";
                dbGrid.Columns[5].HeaderText = "checksum";

                dbGrid.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

                iRowsHeight = dbGrid.Height / 12;
                dbGrid.RowTemplate.Height = iRowsHeight;
                dbGrid.ColumnHeadersHeight = (int)(iRowsHeight * 0.8f);
            }

            //dbGrid.AutoResizeRows();
        }

        /// <summary>carica l'ordina remoto selezionato in MainForm senza stamparlo</summary>
        private void dbGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            bool bDbRead_Ok;
            int iSelOrder, iRow;
            string sTmp;
            String[] sQueue_Object = new String[2];

            Cursor = Cursors.AppStarting;

            if (iGridStringsCount > 0)
            {
                dbConnStatusBox.Image = Properties.Resources.circleRed;
                dbConnStatusBox.Refresh();
                Thread.Sleep(200);

                iSelOrder = Convert.ToInt32(dbGrid.CurrentRow.Cells[0].Value);
                iRow = Convert.ToInt32(dbGrid.CurrentRow.Index);

                if (!ckBoxAutoLoad.Checked)
                {
                    ulStart = (ulong)Environment.TickCount;

                    bDbRead_Ok = rdbCaricaOrdine(iSelOrder);

                    dbConnStatusBox.Image = Properties.Resources.circleGreen;
                    ulStop = (ulong)Environment.TickCount;
                    ulPingTime = ulStop - ulStart;
                    labelQueryTime.Text = String.Format("tempo risposta server: {0} ms", ulPingTime);

                    sTmp = String.Format("rdbCaricaOrdine : {0} ms", ulPingTime);
                    LogToFile(sTmp, true);

                    if (!bDbRead_Ok)
                    {
                        // caricamento ordine fallito
                        _WrnMsg.iErrID = WRN_DBE;
                        _WrnMsg.sMsg = String.Format("rdbCaricaOrdine fallito:\n\nrecord n. {0}", iSelOrder);
                        WarningManager(_WrnMsg);

                        LogToFile("EsploraRemOrdiniDB_Dlg : rdbCaricaOrdine");
                    }
                    else
                    {
                        DataManager.CaricaOrdineWeb();

                        // avvia la stampa dell'ordine
                        sQueue_Object[0] = WEB_ORDER_LOAD_DONE;
                        sQueue_Object[1] = "";
                        FrmMain.EventEnqueue(sQueue_Object);

                        LogToFile("EsploraRemOrdiniDB_Dlg : ordine web caricato", true);
                    }
                }
            }

            Cursor = Cursors.Default;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            int iSelOrder;
            String[] sEvQueueObj;

            /***********************************
             *      gestione coda eventi
             ***********************************/

            if (!Visible)
                return;

            while (eventQueue.Count > 0)
            {
                sEvQueueObj = (String[])eventQueue.Dequeue();

                if (sEvQueueObj[0] == WEB_ALL_ORDERS_LOAD_START)
                {
                    Cursor = Cursors.AppStarting;
                }
                else if (sEvQueueObj[0] == WEB_ALL_ORDERS_LOAD_DONE)
                {
                    iTableAutoLoadPeriod = REFRESH_PERIOD;
                    RefreshTable();

                    Cursor = Cursors.Default;
                    dbConnStatusBox.Image = Properties.Resources.circleGreen;
                }
                else if (sEvQueueObj[0] == WEB_ORDER_PRINT_DONE)
                {
                    Cursor = Cursors.AppStarting;

                    if (iGridStringsCount > 0)
                        iSelOrder = Convert.ToInt32(dbGrid.CurrentRow.Cells[0].Value);

                    iTableAutoLoadPeriod = REFRESH_PERIOD_QUICK;
                }

            }

            if (iTableAutoLoadPeriod > 0)
            {
                iTableAutoLoadPeriod--;
            }
            else
            {
                iTableAutoLoadPeriod = REFRESH_PERIOD;
                rdbCaricaTabellaStart();
            }
        }

        /// <summary>
        /// funziona di cancellazione di un ordine remoto
        /// </summary>
        private void BtnRem_Canc_Click(object sender, EventArgs e)
        {
            bool bResult;
            int iSelOrder;
            string sTmp;
            DialogResult dResult;

            Cursor = Cursors.AppStarting;

            if (iGridStringsCount > 0)
            {
                iSelOrder = Convert.ToInt32(dbGrid.CurrentRow.Cells[0].Value);

                sTmp = String.Format("Sei sicuro di annullare l'ordine n. {0} ?", iSelOrder);
                dResult = MessageBox.Show(sTmp, "Attenzione !", MessageBoxButtons.YesNo);

                if (dResult == DialogResult.Yes)
                {
                    dbConnStatusBox.Image = Properties.Resources.circleRed;
                    dbConnStatusBox.Refresh();
                    Thread.Sleep(200);

                    bResult = dBaseTunnel_my.rdbAnnullaOrdine(iSelOrder);

                    if (bResult)
                        dbConnStatusBox.Image = Properties.Resources.circleGreen;

                    dbConnStatusBox.Refresh();
                }
            }

            iTableAutoLoadPeriod = REFRESH_PERIOD_QUICK;
            Cursor = Cursors.Default;
        }

        /// <summary>
        /// cambia il comportamento del bottone di Close altrimenti distrugge l'oggetto <br/>
        /// invece di nasconderlo soltanto
        /// </summary>
        private void EsploraRemOrdiniDB_Dlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void CheckBoxAuto_CheckedChanged(object sender, EventArgs e)
        {
            iTableAutoLoadPeriod = REFRESH_PERIOD_QUICK;
        }

        private void RadioBtn_CheckedChanged(object sender, EventArgs e)
        {
            iTableAutoLoadPeriod = REFRESH_PERIOD_QUICK;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            ckBoxAutoLoad.Checked = false;
            Close();
        }

        private void BTR_Load_Click(object sender, EventArgs e)
        {
            dbGrid_CellDoubleClick(sender, null);
            LogToFile("EsploraRemOrdiniDB : load Btn");
        }

    }
}

/************************************************************
  NomeFile : StandMonitor/MainForm.cs
  Data	   : 25.09.2024
  Autore   : Mauro Artuso

  Programma per monitorare la statistica degli ordini
 ************************************************************/

using System;
using System.Data;
using System.IO;
using System.Threading;
using System.Windows.Forms;

using static StandCommonFiles.ComDef;
using static StandCommonFiles.commonCl;
using static StandCommonFiles.LogServer;
using static StandCommonFiles.Printer_Legacy;

using static StandFacile.glb;
using static StandFacile.Define;

using static StandFacile.NetConfigLightDlg;
using static StandFacile.VisOrdiniTableFrm;
using static StandFacile.dBaseIntf;

// altrimenti c'è confusione tra le variabili
// using static StandFacile.dBaseIntf; 

namespace StandFacile
{
    /// <summary>
    /// form principale
    /// </summary>
    public partial class FrmMain : Form
    {
        /// <summary>riferimento a FrmMain</summary>
        public static FrmMain rFrmMain;

        // variabili membro
        bool _bDatabaseConnected;

        static bool _bInitNetReadParams = true;

        int iBlink;
        int iRefresh;

        /// <summary>
        /// dataset per costruzione tabella
        /// </summary>
        public DataSet DS = new DataSet();

        DataRow row;
        readonly DataColumn column;

        readonly DataTable table_0 = new DataTable();
        readonly DataTable table_1 = new DataTable();

        /// <summary>griglia per ordini</summary>
        public DataGridView pDBGrid;

        /// <summary>ottiene flag modo esperto</summary>
        /// <returns></returns>
        public bool bGetEsperto() { return MnuEsperto.Checked; }

        /// <summary>ottiene stringa dell'orologio</summary>
        /// <returns></returns>
        public String get_sClockLabel() { return LabelClock.Text; }

        /// <summary>ottiene stringa ultimi scontrini serviti</summary>
        public String get_sNumScontrinoLabel() { return LabelNumScontrino.Text; }

        /// <summary>costruttore</summary>
        public FrmMain()
        {
            String sTmpStr;

            InitializeComponent();

            // TextBox ToolTip
            ToolTip tt = new ToolTip();
            tt.InitialDelay = 0;
            tt.ShowAlways = true;
            tt.SetToolTip(btnAnt, "test di connessione al DataBase");
            tt.Dispose();

            LabelClock.Text = "";
            initActualDate();

            LogToFile("Mainform : Avvio StandMonitor");

            Text = Define.TITLE;
            this.MinimumSize = new System.Drawing.Size(Define.MAINWD_WIDTH, Define.MAINWD_HEIGHT);

            this.Size = new System.Drawing.Size(1000, 680);

            DBGrid.ReadOnly = true; // sicurezza
            pDBGrid = DBGrid;

            SF_Data.iNumCassa = CASSA_PRINCIPALE;

            // impostazione della directory di default per operazioni sui dati
            sRootDir = Directory.GetCurrentDirectory();

            sTmpStr = String.Format("FrmMain : iRefreshTimer = {0}", Define.REFRESH_TIMER);
            LogToFile(sTmpStr);

            rFrmMain = this;

            for (int iWindow = 0; iWindow < 2; iWindow++)
            {
                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "sTipo_Articolo";
                column.AutoIncrement = false;
                column.ReadOnly = true;
                column.Unique = false;

                if (iWindow == 0)
                    table_0.Columns.Add(column);
                else
                    table_1.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.Int32");
                column.ColumnName = "iQuantita_Venduta";
                column.AutoIncrement = false;
                column.ReadOnly = true;
                column.Unique = false;

                if (iWindow == 0)
                    table_0.Columns.Add(column);
                else
                    table_1.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.Int32");
                column.ColumnName = "iQuantita_Residua";
                column.AutoIncrement = false;
                column.ReadOnly = true;
                column.Unique = false;

                if (iWindow == 0)
                    table_0.Columns.Add(column);
                else
                    table_1.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "sDisponibilita";
                column.AutoIncrement = false;
                column.ReadOnly = true;
                column.Unique = false;

                if (iWindow == 0)
                    table_0.Columns.Add(column);
                else
                    table_1.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "sGruppo_Stampa";
                column.AutoIncrement = false;
                column.ReadOnly = true;
                column.Unique = false;

                if (iWindow == 0)
                    table_0.Columns.Add(column);
                else
                    table_1.Columns.Add(column);

                if (iWindow == 0)
                    DS.Tables.Add(table_0);
                else
                    DS.Tables.Add(table_1);
            }

            DBGrid.DataSource = DS.Tables[0];
            DBGrid.MultiSelect = false;

            DBGrid.Columns[0].HeaderText = "Articolo";
            DBGrid.Columns[1].HeaderText = "Quantità Venduta";
            DBGrid.Columns[2].HeaderText = "da consegnare";
            DBGrid.Columns[3].HeaderText = "Disponibilità";
            DBGrid.Columns[4].HeaderText = "Gruppo";

            DBGrid.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            DBGrid.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            DBGrid.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            DBGrid.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DBGrid.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // gestione visibilità del Super User passato su linea di comando
            // per accedere alla visibilità e stampa degli incassi
            if (!bSuperUser)
            {
                MnuStampaDiProva.Enabled = false;
                MnuStampaDiProva.Visible = false;

                MnuConfigurazioneStampe.Enabled = false;
                MnuConfigurazioneStampe.Visible = false;

                MnuVisIncassoOggi.Enabled = false;
                MnuVisIncassoOggi.Visible = false;

                MnuVisIncassoAltraData.Enabled = false;
                MnuVisIncassoAltraData.Visible = false;

                MnuVisOrdiniOggi.Enabled = false;
                MnuVisOrdiniOggi.Visible = false;

                MnuVisOrdiniAltraData.Enabled = false;
                MnuVisOrdiniAltraData.Visible = false;

                MnuEsploraDB.Enabled = false;
                MnuEsploraDB.Visible = false;

                N2.Enabled = false;
                N2.Visible = false;

                N2.Enabled = false;
                N2.Visible = false;

                N3.Enabled = false;
                N3.Visible = false;
            }

            if (bCheckService(_ESPERTO))
                MnuEspertoClick(this, null);

        }

        /// <summary>Init()</summary>
        public void Init()
        {
            String sKeyGood;

            sKeyGood = sReadRegistry(DBASE_SERVER_NAME_KEY, "");

            iMAX_RECEIPT_CHARS = sGlbWinPrinterParams.bChars33 ? MAX_ABS_RECEIPT_CHARS : MAX_LEG_RECEIPT_CHARS;

            // inizializzazione delle stringhe di formattazione
            sRCP_FMT_RCPT = sGlbWinPrinterParams.bChars33 ? _RCP_FMT_33_RCPT : _RCP_FMT_28_RCPT;
            sRCP_FMT_CPY = sGlbWinPrinterParams.bChars33 ? _RCP_FMT_33_CPY : _RCP_FMT_28_CPY;
            sRCP_FMT_NOTE = sGlbWinPrinterParams.bChars33 ? _RCP_FMT_33_NOTE : _RCP_FMT_28_NOTE;
            sRCP_FMT_DSC = sGlbWinPrinterParams.bChars33 ? _RCP_FMT_33_DSC : _RCP_FMT_28_DSC;
            sRCP_FMT_DIF = sGlbWinPrinterParams.bChars33 ? _RCP_FMT_33_DIF : _RCP_FMT_28_DIF;
            sRCP_FMT_TOT = sGlbWinPrinterParams.bChars33 ? _RCP_FMT_33_TOT : _RCP_FMT_28_TOT;
            sRCP_FMT_DSH = sGlbWinPrinterParams.bChars33 ? _RCP_FMT_33_DSH : _RCP_FMT_28_DSH;

            if (String.IsNullOrEmpty(sKeyGood))
            {
                MessageBox.Show("E' la prima esecuzione, imposta la connessione al database !", "Attenzione !", MessageBoxButtons.OK);

                // Imposta il nome del server
                NetConfigLightDlg.rNetConfigLightDlg.ShowDialog();
            }

            _rdBaseIntf.dbInit(getActualDate(), CASSA_PRINCIPALE);

            /**********************************************************
             *   ci deve essere stata almeno la prima connessione !!!
             **********************************************************/

            iRefresh = 0;

            FiltroDlg.rFiltroDlg.caricaFiltro();

            checkMenuItems();
            FormResize(this, null);

            Timer.Enabled = true;

            LogToFile("FrmMain : Init");

            // VisOrdiniFrm.rVisOrdiniFrm.ShowDialog(); // debug
        }

        /**************************************
         *             main loop 
         **************************************/
        private void timer_MainLoop(object sender, EventArgs e)
        {

            bool bFilterFound, bExcludedItem;

            int iArrayIndex, iGlbNumOfTickets, i, j;
            ulong ulStart, ulStop, ulPingTime;

            String sTime, sData, sExcludedItem;
            String sTmpStr, sDisp;

            sData = getActualDate().ToString("ddd dd/MM/yy");
            sTmpStr = sData.ToUpper();
            sTmpStr = sTmpStr.Substring(0, 1);

            sData = sData.Substring(1);
            sData = sTmpStr + sData;

            sTime = DateTime.Now.ToString("HH:mm:ss");

            // altrimenti gli errori si accavallano
            if (MessageDlg.rMessageDlg.Visible || NetConfigLightDlg.rNetConfigLightDlg.Visible)
                return;

            if (iRefresh == 0)
            {
                iRefresh = Define.REFRESH_TIMER;

                try
                {
                    ulStart = (ulong)Environment.TickCount;

                    _rdBaseIntf.dbCheckStatus();

                    iGlbNumOfTickets = _rdBaseIntf.dbGetNumOfOrdersFromDB(false);

                    // c'è la connessione ma non i dati = StandFacile da avviare
                    // StandMonitor non usa DB_Data.iGroupsColor[i] ma è meglio uniformare
                    if (iGlbNumOfTickets == 0)
                        _bInitNetReadParams = true;

                    // _bInitNetReadParams è true anche in fase di avvio
                    if (_bInitNetReadParams)
                    {
                        rNetConfigLightDlg.NetConfig_ReadParams();
                        _bInitNetReadParams = false;
                    }

                    /*********************************
                            aggiorna gli ordini
                     *********************************/
                    rVisOrdiniFrm.Aggiorna();

                    _rdBaseIntf.dbBuildMonitorTable();

                    // misura del tempo in ms per leggere la tabella ClientDS_DatiCassa
                    ulStop = (ulong)Environment.TickCount;
                    ulPingTime = (ulong)(ulStop - ulStart);
                    sTmpStr = String.Format("{0} ms", ulPingTime);

                    lblElapsedTime.Text = sTmpStr;

                    LogToFile("timer_MainLoop : dbFuncTime = " + sTmpStr);
                }
                catch (Exception)
                {
                    _bDatabaseConnected = false;

                    if (get_bErrorePrimaVolta())
                    {
                        //WrnMsg.sMsg = _sDBTNameDati;
                        //WrnMsg.iErrID = WRN_TDQ;
                        //WarningManager(WrnMsg);

                        clear_bErrorePrimaVolta();
                    }

                    LogToFile("timer_MainLoop : dbException prima tabella");
                }

                LabelNumScontrino.Text = String.Format("scontrini: {0},  ultimi serviti: {1}, {2}, {3}, {4}, {5}", get_iNumScontrini(),
                    get_sNumScontrino(NUM_ULTIMI_SCONTRINI - 1),
                    get_sNumScontrino(NUM_ULTIMI_SCONTRINI - 2),
                    get_sNumScontrino(NUM_ULTIMI_SCONTRINI - 3),
                    get_sNumScontrino(NUM_ULTIMI_SCONTRINI - 4),
                    get_sNumScontrino(NUM_ULTIMI_SCONTRINI - 5));

                LabelClock.Text = String.Format("{0} {1},  attesa: {2} m", sData, sTime, get_sAttesaMedia());

                /***********************************************
                       scrittura tabella DBGrid
                 ***********************************************/

                try
                {

                    /************************************************
                        popolazione tabella_0 DB_Data.Articolo[]
                     ************************************************/

                    for (j = 0; j < 2; j++)
                    {
                        if (j == 0)
                            table_0.Clear();
                        else
                            table_1.Clear();

                        for (iArrayIndex = 0; iArrayIndex < MAX_NUM_ARTICOLI; iArrayIndex++)
                        {
                            bFilterFound = false;

                            if (j == 0)
                            {
                                if (sFiltroMon_0.Count > 0)
                                {
                                    for (i = 0; i < sFiltroMon_0.Count; i++)
                                    {
                                        bExcludedItem = sFiltroMon_0[i].StartsWith("!");

                                        if (bExcludedItem && (sFiltroMon_0[i].Length > 1))
                                            sExcludedItem = sFiltroMon_0[i].Substring(1);
                                        else
                                            sExcludedItem = "";

                                        if (bExcludedItem && DB_Data.Articolo[iArrayIndex].sTipo.ToUpper().Contains(sExcludedItem.ToUpper()))
                                        {
                                            bFilterFound = false;
                                            break;
                                        }
                                        else if (DB_Data.Articolo[iArrayIndex].sTipo.ToUpper().Contains(sFiltroMon_0[i].ToUpper()))
                                            bFilterFound = true;
                                    }

                                    // *** se l'Articolo non è compreso nel filtro non viene visualizzato ***
                                    if (!bFilterFound)
                                        continue;
                                }
                            }
                            else
                            {
                                if (sFiltroMon_1.Count > 0)
                                {
                                    for (i = 0; i < sFiltroMon_1.Count; i++)
                                    {
                                        bExcludedItem = sFiltroMon_1[i].StartsWith("!");

                                        if (bExcludedItem && (sFiltroMon_1[i].Length > 1))
                                            sExcludedItem = sFiltroMon_1[i].Substring(1);
                                        else
                                            sExcludedItem = "";

                                        if (bExcludedItem && DB_Data.Articolo[iArrayIndex].sTipo.ToUpper().Contains(sExcludedItem.ToUpper()))
                                        {
                                            bFilterFound = false;
                                            break;
                                        }
                                        else if (DB_Data.Articolo[iArrayIndex].sTipo.ToUpper().Contains(sFiltroMon_1[i].ToUpper()))
                                            bFilterFound = true;
                                    }

                                    // *** se l'Articolo non è compreso nel filtro non viene visualizzato ***
                                    if (!bFilterFound)
                                        continue;
                                }
                            }

                            if (!String.IsNullOrEmpty(DB_Data.Articolo[iArrayIndex].sTipo))
                            {
                                if (j == 0)
                                    row = table_0.NewRow();
                                else
                                    row = table_1.NewRow();

                                if (DISP_OK == DB_Data.Articolo[iArrayIndex].iDisponibilita)
                                    sDisp = "OK";
                                else
                                    sDisp = DB_Data.Articolo[iArrayIndex].iDisponibilita.ToString();

                                row["sTipo_Articolo"] = DB_Data.Articolo[iArrayIndex].sTipo;
                                row["iQuantita_Venduta"] = DB_Data.Articolo[iArrayIndex].iQuantitaVenduta;
                                row["iQuantita_Residua"] = DB_Data.Articolo[iArrayIndex].iQuantitaVenduta - DB_Data.Articolo[iArrayIndex].iQuantita_Scaricata;
                                row["sGruppo_Stampa"] = sConstGruppiShort[DB_Data.Articolo[iArrayIndex].iGruppoStampa];
                                row["sDisponibilita"] = sDisp;

                                if (j == 0)
                                    table_0.Rows.Add(row);
                                else
                                    table_1.Rows.Add(row);
                            }
                        }
                    }

                    // il sort deve avvenire solo alla fine della fusione delle tabelle
                    DBGrid.Sort(DBGrid.Columns[1], System.ComponentModel.ListSortDirection.Descending);

                    _bDatabaseConnected = true;

                    StartAntBmpTimer();

                    FormResize(this, null);

                }
                catch (Exception)
                {
                    _bDatabaseConnected = false;
                    LogToFile("timer_MainLoop : dbException DBGrid");
                }

                FormResize(this, null);

                /***************************************
                    visualizza nella seconda finestra
                 ***************************************/
                if ((AuxForm.rAuxForm != null) && AuxForm.rAuxForm.Visible)
                    AuxForm.rAuxForm.refresh();
            }
            else
                iRefresh--;


            /**************************************
                Blink Antenna di connessione DB
             **************************************/
            if (_bDatabaseConnected)
            {

                switch (iBlink)
                {
                    case 0:
                        btnAnt.Image = BtnImgList.Images[2]; // celeste piccola
                        iBlink++;
                        break;
                    case 3:
                        btnAnt.Image = BtnImgList.Images[3]; // celeste media
                        iBlink++;
                        break;
                    case 6:
                        btnAnt.Image = BtnImgList.Images[4]; // celeste grande
                        iBlink++;
                        break;

                    default:
                        iBlink++;
                        if (iBlink > 9)
                        {
                            btnAnt.Image = BtnImgList.Images[1];  // verde piccola
                            iBlink = 10;
                        }
                        break;
                }
            }
            else
            {
                btnAnt.Image = BtnImgList.Images[0]; // rossa piccola
                iBlink = 10;
            }
        }

        /*****************************************************
	     *   Protezione contro le modifiche non volute
         *****************************************************/
        private void MnuEspertoClick(object sender, EventArgs e)
        {
            String sTmp;
            DialogResult dResult;

            if (!bCheckService(_ESPERTO) && (!MnuEsperto.Checked))
                dResult = MessageBox.Show("E' importante aver letto e compreso il manuale prima di proseguire !\r\n\r\n" +
                       "Il manuale pdf è presente nella cartella di installazione e si può aprire anche dal pulsante presente nel menù di Aiuto->Aiuto Rapido.",
                       "Attenzione !", MessageBoxButtons.OKCancel);
            else
                dResult = DialogResult.OK;

            if (dResult == DialogResult.OK)
            {
                MnuEsperto.Checked = !MnuEsperto.Checked;

                checkMenuItems();

                sTmp = String.Format("FrmMain: Modo Esperto {0}", MnuEsperto.Checked);
                LogToFile(sTmp);
            }
        }

        /***************************************************
         *       Abilita/disabilita le varie voci
         *       del Menù Principale
         ***************************************************/
        private void checkMenuItems()
        {
            if (MnuEsperto.Checked)
            {
                MnuDBServer.Enabled = true;
                MnuFiltro.Enabled = true;
                MnuConfigurazioneStampe.Enabled = true;
            }
            else
            {
                MnuDBServer.Enabled = false;
                MnuFiltro.Enabled = false;
                MnuConfigurazioneStampe.Enabled = false;
            }

            FormResize(this, null);
        }

        private void MnuDBServer_Click(object sender, EventArgs e)
        {
            NetConfigLightDlg.rNetConfigLightDlg.ShowDialog();
            iRefresh = 0; // refresh immediato
        }

        private void MnuAbout_Click(object sender, EventArgs e)
        {
            InfoDlg rInfoDlg = new InfoDlg();
            rInfoDlg.ShowDialog();
            rInfoDlg.Dispose();
        }

        void StartAntBmpTimer()
        {
            iBlink = 0;
        }

        private void FormResize(object sender, EventArgs e)
        {
            int iRowsHeight;
            float fWidth;
            float fFontHeaderHeight, fFontHeight;

            // altrimenti da errore
            if (DBGrid.ColumnCount == 0)
                return;

            DBGrid.Width = this.Width - 40;

            // imposta altezza sulla base della altezza della finestra
            if (WindowState != FormWindowState.Minimized)
            {
                DBGrid.Height = this.Height - LabelClock.Height - LabelNumScontrino.Height - 120;

                iRowsHeight = DBGrid.Height / 12;
                DBGrid.RowTemplate.Height = iRowsHeight;
                DBGrid.ColumnHeadersHeight = (int)(iRowsHeight * 0.8f);
            }

            fWidth = DBGrid.Width;

            if (MnuVisGruppi.Checked)
            {
                DBGrid.Columns[0].Width = (int)(fWidth * 0.40f);
                DBGrid.Columns[1].Width = (int)(fWidth * 0.18f);
                DBGrid.Columns[2].Width = (int)(fWidth * 0.18f);
                DBGrid.Columns[3].Width = (int)(fWidth * 0.12f);
                DBGrid.Columns[4].Width = (int)(fWidth * 0.12f);
            }
            else
            {
                DBGrid.Columns[0].Width = (int)(fWidth * 0.48f);
                DBGrid.Columns[1].Width = (int)(fWidth * 0.18f);
                DBGrid.Columns[2].Width = (int)(fWidth * 0.18f);
                DBGrid.Columns[3].Width = (int)(fWidth * 0.14f);
                DBGrid.Columns[4].Width = (int)(fWidth * 0.12f);
            }

            // imposta Font sulla base della larghezza della finestra
            fFontHeight = ((float)DBGrid.Width) / 36;
            DBGrid.Font = new System.Drawing.Font(DBGrid.DefaultCellStyle.Font.Name, fFontHeight);

            fFontHeaderHeight = ((float)DBGrid.Width) / 80;
            DBGrid.Columns[0].HeaderCell.Style.Font = new System.Drawing.Font(DBGrid.DefaultCellStyle.Font.Name, fFontHeaderHeight);
            DBGrid.Columns[1].HeaderCell.Style.Font = DBGrid.Columns[0].HeaderCell.Style.Font;
            DBGrid.Columns[2].HeaderCell.Style.Font = DBGrid.Columns[0].HeaderCell.Style.Font;
            DBGrid.Columns[3].HeaderCell.Style.Font = DBGrid.Columns[0].HeaderCell.Style.Font;
            DBGrid.Columns[4].HeaderCell.Style.Font = DBGrid.Columns[0].HeaderCell.Style.Font;

            if (DBGrid.Rows.Count > 0)
            {
                DBGrid.Rows[0].Selected = true;
                DBGrid.FirstDisplayedScrollingRowIndex = 0;
            }

            DBGrid.AutoResizeRows();
        }

        private void DBGrid_Scroll(object sender, ScrollEventArgs e)
        {
            iRefresh = REFRESH_TIMER;
        }

        private void DBGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            iRefresh = REFRESH_TIMER;
        }

        private void DBGrid_KeyDown(object sender, KeyEventArgs e)
        {
            iRefresh = REFRESH_TIMER;
        }

        private void BtnAnt_Click(object sender, EventArgs e)
        {
            ulong ulStart, ulStop, ulPingTime;
            String sTmp;

            ulStart = (ulong)Environment.TickCount;

            _rdBaseIntf.dbCheck();
            iRefresh = 0; // refresh immediato

            // misura del tempo in ms per eseguire dbScaricaOrdine
            ulStop = (ulong)Environment.TickCount;
            ulPingTime = ulStop - ulStart;
            sTmp = String.Format("{0} ms", ulPingTime);
            lblElapsedTime.Text = sTmp;
        }

        private void FormClose_Click(object sender, EventArgs e)
        {
            LogToFile("Mainform : uscita");
            Close();
        }

        private void MnuVisGruppi_Click(object sender, EventArgs e)
        {
            MnuVisGruppi.Checked = !MnuVisGruppi.Checked;
            iRefresh = 0;
        }

        private void MnuVisOrdini_Click(object sender, EventArgs e)
        {
            VisOrdiniTableFrm.rVisOrdiniFrm.Show();
            VisOrdiniTableFrm.rVisOrdiniFrm.Aggiorna();
        }

        private void MnuVisIncassoOggi_Click(object sender, EventArgs e)
        {
            VisDatiDlg rVisDatiDlg = new VisDatiDlg();

            _rdBaseIntf.dbCheckStatus();

            // riporta DatePicker alla data di oggi
            SelDataDlg.rSelDataDlg.setTodayDate();

            // usa la data corrente
            rVisDatiDlg.VisualizzaDati((int)FILE_TO_SHOW.FILE_DATI, getActualDate(), CASSA_PRINCIPALE, false);

            rVisDatiDlg.Dispose();
        }

        private void MnuVisIncassoAltraData_Click(object sender, EventArgs e)
        {
            SelectionRange selDates;
            VisDatiDlg rVisDatiDlg = new VisDatiDlg();

            SelDataDlg.rSelDataDlg.ShowDialog();
            selDates = SelDataDlg.rSelDataDlg.getDateFromPicker();

            if (selDates != null) // non sono uscito con Cancel ...
                rVisDatiDlg.VisualizzaDati((int)FILE_TO_SHOW.FILE_DATI, selDates, SF_Data.iNumCassa, true);

            rVisDatiDlg.Dispose();

        }

        private void MnuEsploraDB_Click(object sender, EventArgs e)
        {
            EsploraDB_Dlg rEsploraDB_Dlg = new EsploraDB_Dlg();
            rEsploraDB_Dlg.ShowDialog();

            rEsploraDB_Dlg.Dispose();
        }

        private void MnuAuxWindow_Click(object sender, EventArgs e)
        {
            new AuxForm(2);

            AuxForm.rAuxForm.refresh();
            AuxForm.rAuxForm.Show();
        }
        private void MnuAuxWindow3_Click(object sender, EventArgs e)
        {
            new AuxForm(3);

            AuxForm.rAuxForm.refresh();
            AuxForm.rAuxForm.Show();
        }

        private void MnuFiltro_Click(object sender, EventArgs e)
        {
            FiltroDlg.rFiltroDlg.ShowDialog();
        }

        private void MnuStampaDiProva_Click(object sender, EventArgs e)
        {
            String sTmp, sFileToPrint;

            sFileToPrint = buildSampleText();

            sTmp = String.Format("Mainform : printSampleText() {0}", sFileToPrint);
            LogToFile(sTmp);

            GenPrintFile(sFileToPrint);
        }

        private void MnuConfigurazioneStampe_Click(object sender, EventArgs e)
        {
            PrintConfigLightDlg.rPrintConfigLightDlg.Init(true);
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            LogToFile("Mainform : uscita");

            StopPrintServer();
            StopLogServer(); // deve stare per ultimo
        }

        private void FrmMain_SizeChanged(object sender, EventArgs e)
        {
            //iRefresh = 0;
        }

        private void MnuVisOrdiniOggi_Click(object sender, EventArgs e)
        {
            VisOrdiniDlg rVisOrdiniDlg = new VisOrdiniDlg(getActualDate(), VisOrdiniDlg.MAX_NUM_TICKET);
            rVisOrdiniDlg.Dispose();
        }

        private void MnuVisOrdiniAltraData_Click(object sender, EventArgs e)
        {
            SelectionRange selDates;

            SelDataDlg.rSelDataDlg.ShowDialog();
            selDates = SelDataDlg.rSelDataDlg.getDateFromPicker();

            if (selDates != null) // non sono uscito con Cancel ...
            {
                VisOrdiniDlg rVisOrdiniDlg = new VisOrdiniDlg(selDates.Start, VisOrdiniDlg.MAX_NUM_TICKET);
                rVisOrdiniDlg.Dispose();
            }
        }

        private void MnuManuale_Click(object sender, EventArgs e)
        {
            QuickHelpDlg rQuickHelpDlg = new QuickHelpDlg();

            rQuickHelpDlg.Dispose();
        }

    }
}


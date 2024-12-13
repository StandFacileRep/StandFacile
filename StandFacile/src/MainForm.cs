/***********************************************
  NomeFile  : StandFacile/MainForm.cs
  Data	    : 01.09.2024
  Autore    : Mauro Artuso
 ***********************************************/

// #define FONT_CHECK

using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.Threading;
using System.Windows.Forms;

using static StandFacile.glb;
using static StandFacile.Define;
using static StandFacile.dBaseIntf;
using static StandFacile.dBaseTunnel_my;
using System.Collections.Generic;

using static StandCommonFiles.ComDef;
using static StandCommonFiles.CommonCl;
using static StandCommonFiles.LogServer;
using static StandCommonFiles.Printer_Legacy;
using System.Web.Script.Serialization;

namespace StandFacile
{

    /// <summary>
    /// form principale
    /// </summary>
    public partial class FrmMain : Form
    {
#pragma warning disable IDE0044
#pragma warning disable IDE0059

        const int CHANGE_PAGE_TIMEOUT = 5;      // *250ms
        const int BC_FOCUS_TIMEOUT = 8 * 4;     // *250ms

        static bool _bListinoModificato;  // true se ci son state modifiche al Listino
        static bool _bPasswordIsGood;
        static bool _bShowTotaleScontrinoPrec;

        /// <summary>variabile per tracking tasto Crtl pressed per gestione Note</summary>
        static bool _bCrtlIsPressed;

        bool _bPrimaEsecuzione;
        bool _bPrintTimeoutEnabled;

        bool bMouseWrongPos;        //true se il mouse è fuori la MainGrid
        bool bSkipDrag; 	        //serve per evitare un BeginDrag sul doppio click
        bool bStartDrag;
        bool bPageDxProximity;
        bool bPageSxProximity;

        static int _iDBDispTimeout;

        int _iCellPt;      // indice lineare di selezione Articolo

        int _iVisPrzTimeout;
        int _webPrintTimeout;
        int _iPrintTimeout;
        int _iNumOfOrders;
        int _iModDisponibilitaTimeout;
        int _iContante;
        int _iAnteprimaTotParziale;
        int iLastGridIndex; //indice dell'ultimo Articolo della griglia corrente  = iGridRows * iGridCols
        int iArrayOffset;   //indice della primo Articolo della griglia corrente  nel vettore Articolo[],
                            // = TabSet->SelectedIndex * iLastGridIndex;/
        int iSwapStartIndex;
        int iChangePageDxTimeout;
        int iChangePageSxTimeout;
        int iFocus_BC_Timeout;
        int _iColorTheme;

        float _fFontWidth = 9;

        String sStatusText;
        static String _sEditTavolo, _sEditNome, _sEditCoperti, _sEditContante, _sEditNota;
        String _sCopertiPrev;

        String _sOrdiniPrevDBTable;
        String _sEditNotaCopy;

        DataGridViewCellStyle _prevStyle = new DataGridViewCellStyle();
        DataGridViewCellStyle[,] _gridStyle = new DataGridViewCellStyle[NUM_COLOR_THEMES, NUM_COPIES_GRPS];
        DataGridViewCellStyle[] _gridCrossStyle = new DataGridViewCellStyle[NUM_COLOR_THEMES];
        DataGridViewCellStyle[] _selectedCellStyle = new DataGridViewCellStyle[NUM_COLOR_THEMES];
        DataGridViewCellStyle[] _zeroAvailabilityStyle = new DataGridViewCellStyle[NUM_COLOR_THEMES];

        TErrMsg WrnMsg;

        static EsploraRemOrdiniDB_Dlg rEsploraRemOrdiniDB_Dlg;

        // TextBox ToolTip
        readonly ToolTip _tt = new ToolTip
        {
            InitialDelay = 50,
            ShowAlways = true
        };

        /// <summary>ottiene il flag di modo esperto</summary>
        public bool GetEsperto() { return MnuEsperto.Checked; }

        /// <summary>imposta il flag di password corretta</summary>
        public static void SetPasswordIsGood(bool passwordIsGoodPrm) { _bPasswordIsGood = passwordIsGoodPrm; }

        /// <summary>
        /// resetta il flag di modifica listino necessaria<br/>
        /// chiamata da DataManager.SalvaListino();
        /// </summary>
        public static void ClearListinoModificato() { _bListinoModificato = false; }

        /// <summary>mette evento in coda cross thread</summary>
        public static void EventEnqueue(String[] sEvQueueObjParam)
        {
            bool bContains = false; // non si può usare eventQueue.Contains()

            foreach (String[] s in eventQueue)
            {
                if ((s[0] == sEvQueueObjParam[0]) && (s[1] == sEvQueueObjParam[1]))
                {
                    bContains = true;
                    break;
                }
            }

            if (bContains)
            {
                LogToFile(String.Format("FrmMain : not eventQueue.Enqueue {0}", sEvQueueObjParam[1]));
                Console.WriteLine(String.Format("FrmMain : not eventQueue.Enqueue {0}", sEvQueueObjParam[1]));
            }
            else
            {
                // crea una copia dell'oggetto passato per riferimento
                String[] sQueue_Object = new String[2] { sEvQueueObjParam[0], sEvQueueObjParam[1] };

                eventQueue.Enqueue(sQueue_Object);

                LogToFile(String.Format("FrmMain : eventQueue.Enqueue {0}", sEvQueueObjParam[1]));
                Console.WriteLine(String.Format("FrmMain : eventQueue.Enqueue {0}", sEvQueueObjParam[1]));
            }
        }

        /// <summary>ottiene numero di eventi in coda</summary>
        public static int GetEventQueueCount() { return eventQueue.Count; }

        /// <summary>riferimento a FrmMain</summary>
        public static FrmMain rFrmMain;

        // gestione cross thread
        static readonly Queue eventQueue = new Queue();

        // gestione cross thread
        static readonly Queue scannerInputQueue = new Queue();

        /// <summary>imposta il testo dei coperti</summary>
        public void SetEditCoperto(String sCopertoParam) { EditCoperti.Text = sCopertoParam; TextBox_KeyUp(null, null); }

        /// <summary>ottiene il testo dei coperti</summary>
        public String GetEditCoperto() { return EditCoperti.Text; }

        /// <summary>imposta il testo del tavolo</summary>
        public void SetEditTavolo(String sTavoloParam) { EditTavolo.Text = sTavoloParam; TextBox_KeyUp(null, null); }

        /// <summary>imposta il testo del nome</summary>
        public void SetEditNome(String sNomeParam) { EditNome.Text = sNomeParam; TextBox_KeyUp(null, null); }

        /// <summary>ottiene il testo del tavolo</summary>
        public String GetEditTavolo() { return EditTavolo.Text; }

        /// <summary>resetta il tipo di pagamento CONT./CARD/SATISPAY</summary>
        public void ResetPayment() { comboCashPos.SelectedIndex = sConst_PaymentType.Length - 1; }

        /// <summary>imposta _iAnteprimaTotParziale</summary>
        public void SetAnteprima() { _iAnteprimaTotParziale = AnteprimaDlg.GetTotaleReceipt(); }

        /// <summary>azzera _iAnteprimaTotParziale</summary>
        public void ClearAnteprima() { _iAnteprimaTotParziale = 0; }

        /// <summary>ottiene il testo della nota</summary>
        public String GetEditNota() { return EditNota.Text; }

        /// <summary>ottiene lo stato della nota: true => nota in calce</summary>
        public bool GetStatusNota() { return EditNota.BackColor == Color.Gainsboro; }

        /// <summary>imposta il testo della nota</summary>
        public void SetEditNota(String sNotaParam) { EditNota.Text = sNotaParam; TextBox_KeyUp(null, null); }

        /// <summary>imposta il testo della nota Articolo, usato da TestManager</summary>
        public void SetEditNotaArticolo(int iCellParam, String sNotaParam)
        {
            int i, j;
            Message msg = new Message();

            i = iCellParam % MainGrid.RowCount;
            j = iCellParam / MainGrid.RowCount;

            _bCrtlIsPressed = true;
            MainGrid_CellClick(this, new DataGridViewCellEventArgs(j, i));

            EditNota.Text = sNotaParam; // simulta input testo
            ProcessCmdKey(ref msg, Keys.Enter);

            EditNota.Refresh();
        }

        /// <summary>imposta il testo della nota</summary>
        public void SetEditStatus_BC(String sBCParam) { EditStatus_BC.Text = sBCParam; TextBox_KeyUp(null, null); }

        /// <summary>imposta il testo dello stato</summary>
        public void SetStatus(String sNotaParam) { sStatusText = sNotaParam; }

        /// <summary>resetta il timer del Focus al BC_Edit</summary>
        void ClearBC_FocusTimer() { iFocus_BC_Timeout = BC_FOCUS_TIMEOUT; }

        /// <summary>imposta il flag per nascondere lblStatusTotalePrec</summary>
        public static void SetShowTotaleScontrinoPrec(bool bShowParam) { _bShowTotaleScontrinoPrec = bShowParam; }

        /// <summary>imposta la Tab</summary>
        public void SelectTab(int iParam)
        {
            int iPageIndex = iParam / (MainGrid.RowCount * MainGrid.ColumnCount);

            // sicurezza
            if (CheckService(Define._AUTO_SEQ_TEST) || (MnuImpListino.Checked == true))
            {
                if (iPageIndex == 1)
                    TabSet.SelectedTab = tabPage1;
                else if (iPageIndex == 2)
                    TabSet.SelectedTab = tabPage2;
                else if (iPageIndex == 3)
                    TabSet.SelectedTab = tabPage3;
                else if (iPageIndex == 4)
                    TabSet.SelectedTab = tabPage4;
                else
                    TabSet.SelectedTab = tabPage0;
            }
            else
                return;

        }

        /// <summary>costruttore</summary>
        public FrmMain()
        {
            InitializeComponent();

            //bool bTmp = StringBelongsTo_ORDER_CONST(ORDER_CONST._NOTA, ORDER_CONST._NOTA);

            _tt.SetToolTip(EditTavolo, "click o (F1) dalla griglia per inserire il Tavolo");
            _tt.SetToolTip(EditCoperti, "click o (F2) dalla griglia per inserire i coperti");
            _tt.SetToolTip(EditNota, "click o (F3) dalla griglia per inserire una Nota nello scontrino\ncon Crtl+click su una cella per inserire una nota relativa all'Articolo");
            _tt.SetToolTip(EditResto, "resto calcolato");
            _tt.SetToolTip(EditContante, "click o (F4) dalla griglia per inserire il resto");
            _tt.SetToolTip(EditStatus_BC, "area per lettura barcode prevendite e pre-ordini web");
            _tt.SetToolTip(comboCashPos, "tipo di pagamento: Contanti/Card/Satispay");

            // layout Toolbar e Menu
            toolStrip.Left = 60;

            comboCashPos.Items.Clear();

            for (int i = sConst_PaymentType.Length - 1; i >= 0; i--) // OK
                comboCashPos.Items.Insert(0, sConst_PaymentType[i]);

            ResetPayment();

            // VisMessaggi può inviare una stampa a qualsiasi stampante

            //if (!bUSA_NDB())
            //{
            //    BtnSendMsg.Visible = false;
            //    BtnSep2.Visible = false;

            //    MnuVisMessaggiInviati.Visible = false;
            //}

            _bPrintTimeoutEnabled = false;
            _bPrimaEsecuzione = true;
            _bCrtlIsPressed = false;

            bSkipDrag = false;
            bMouseWrongPos = false;
            bPageDxProximity = false;
            bPageSxProximity = false;

            rFrmMain = this;

            Text = String.Format("{0} {1} {2} {1} webcks Listino = {3}", TITLE, "    ", RELEASE_SW, DataManager.GetWebListinoChecksum());
            MinimumSize = new Size(MAINWD_WIDTH, MAINWD_HEIGHT);

            // Larghezza barra di stato
            //LblStatus_Status.Width = 320;

            EditNota.MaxLength = iMAX_RECEIPT_CHARS;
            EditNota.Width = 240;

            EditTavolo.MaxLength = 18;
            EditTavolo.Width = 100;

            EditNome.MaxLength = 20;
            EditNome.Width = 140;

            EditStatus_BC.Text = "";
            EditStatus_BC.MaxLength = 0; // nessun limite


            SetColorsTheme();

            LogToFile("Mainform : Avvio()");

            _iAnteprimaTotParziale = 0;
            _bPasswordIsGood = false;
        }

        /// <summary>Init</summary>
        public void Init()
        {
            int iKeyGood;
            String sDataStr, sTmpStr;

            // se serve (aumento dimensioni griglia) forza il salvataggio del file Prezzi
            // deve prima essere eseguito DataManager.Init()
            _bListinoModificato |= DataManager.GetListinoModificato();

            iKeyGood = ReadRegistry(SYS_PRINTER_TYPE_KEY, -1);

            if (iKeyGood == -1)
            {
                MnuEsperto.Checked = true;
                MessageBox.Show("E' la prima esecuzione, imposta la stampante ed il listino Prezzi !", "Attenzione !", MessageBoxButtons.OK);

                // default
                WriteRegistry(SYS_PRINTER_TYPE_KEY, (int)PRINTER_SEL.STAMPANTE_WINDOWS);
            }
            else if (CheckService(_ESPERTO))
            {
                _bPasswordIsGood = true;
                MnuEsperto_Click(this, null);
            }

            sDataStr = GetActualDate().ToString("dddd  dd/MM/yy");
            sTmpStr = sDataStr.ToUpper();
            sTmpStr = sTmpStr.Substring(0, 1);

            sDataStr = sDataStr.Substring(1);
            sDataStr = sTmpStr + sDataStr;

            sStatusText = "Pronto";

            lblStatusTickNum.Text = "Num. Ordini : " + DataManager.GetNumOfOrders().ToString();

            if (OptionsDlg._rOptionsDlg.GetShowPrevReceipt())
            {
                _bShowTotaleScontrinoPrec = true;
                lblStatusTotalePrec.Text = "Totale Pr. = 0";
            }
            else
            {
                _bShowTotaleScontrinoPrec = false;
                lblStatusTotalePrec.Text = "";
            }

            lblStatus_Date.Text = sDataStr;

            SetTabsAppearance();

            // altrimenti la disponibilità cambia ad ogni avvio
            _iNumOfOrders = DataManager.GetNumOfOrders();

            _iDBDispTimeout = _REFRESH_DISP_SHORT;

            MainGrid.CurrentCell = MainGrid.Rows[0].Cells[0];

            MainGrid.Focus();

            iFocus_BC_Timeout = BC_FOCUS_TIMEOUT;
            Timer.Enabled = true;
            CheckMenuItems();

            LogToFile("Mainform : Init()");

            // altrimenti con F3 si riordinano le colonne !!!
            foreach (DataGridViewColumn column in MainGrid.Columns)
                column.SortMode = DataGridViewColumnSortMode.NotSortable;

        }

        /// <summary>imposta i Temi colore</summary>
        public void SetColorsTheme()
        {
            // ************** gestione temi colore *****************

            _iColorTheme = OptionsDlg.GGetColorThemeIndex();

            for (int i = 0; i < NUM_COLOR_THEMES; i++)
            {
                for (int j = 0; j < NUM_COPIES_GRPS; j++)
                    _gridStyle[i, j] = new DataGridViewCellStyle();

                _gridCrossStyle[i] = new DataGridViewCellStyle();
                _selectedCellStyle[i] = new DataGridViewCellStyle();
                _zeroAvailabilityStyle[i] = new DataGridViewCellStyle();
            }

            // disponibilità zero
            _zeroAvailabilityStyle[0].ForeColor = Color.Plum;
            _zeroAvailabilityStyle[1].ForeColor = Color.MediumVioletRed;
            _zeroAvailabilityStyle[2].ForeColor = Color.MediumVioletRed;

            _gridCrossStyle[0].ForeColor = Color.Gainsboro;
            _gridCrossStyle[1].ForeColor = Color.LightSteelBlue;
            _gridCrossStyle[2].ForeColor = Color.Gainsboro;

            _selectedCellStyle[0].ForeColor = Color.White; _selectedCellStyle[0].BackColor = Color.Navy;
            _selectedCellStyle[1].ForeColor = Color.White; _selectedCellStyle[1].BackColor = Color.RoyalBlue;
            _selectedCellStyle[2].ForeColor = Color.MidnightBlue; _selectedCellStyle[2].BackColor = Color.LightSkyBlue;

            // classico
            _gridStyle[0, 0].ForeColor = Color.White; _gridStyle[0, 0].BackColor = Color.Teal;      // PRIMI
            _gridStyle[0, 1].ForeColor = Color.Cornsilk; _gridStyle[0, 1].BackColor = Color.Teal;   // SECONDI
            _gridStyle[0, 2].ForeColor = Color.Yellow; _gridStyle[0, 2].BackColor = Color.Teal;     // CONTORNI
            _gridStyle[0, 3].ForeColor = Color.Aqua; _gridStyle[0, 3].BackColor = Color.Teal;       // ANALCOLICI
            _gridStyle[0, 4].ForeColor = Color.Bisque; _gridStyle[0, 4].BackColor = Color.Teal;     // ALCOLICI
            _gridStyle[0, 5].ForeColor = Color.Aquamarine; _gridStyle[0, 5].BackColor = Color.Teal; // VARIE
            _gridStyle[0, 6].ForeColor = Color.LemonChiffon; _gridStyle[0, 6].BackColor = Color.Teal; // PANINI
            _gridStyle[0, 7].ForeColor = Color.MistyRose; _gridStyle[0, 7].BackColor = Color.Teal;  // GRUPPO 8 NOWEB
            _gridStyle[0, 8].ForeColor = Color.White; _gridStyle[0, 8].BackColor = Color.Teal;      // CONTATORI

            // chiaro
            _gridStyle[1, 0].ForeColor = Color.Black; _gridStyle[1, 0].BackColor = Color.LightYellow; // Cornsilk
            _gridStyle[1, 1].ForeColor = Color.Black; _gridStyle[1, 1].BackColor = Color.Wheat;
            _gridStyle[1, 2].ForeColor = Color.Black; _gridStyle[1, 2].BackColor = Color.LightGreen;
            _gridStyle[1, 3].ForeColor = Color.Black; _gridStyle[1, 3].BackColor = Color.SkyBlue;
            _gridStyle[1, 4].ForeColor = Color.Black; _gridStyle[1, 4].BackColor = Color.MistyRose;
            _gridStyle[1, 5].ForeColor = Color.Black; _gridStyle[1, 5].BackColor = Color.PaleGoldenrod; // RosyBrown
            _gridStyle[1, 6].ForeColor = Color.Black; _gridStyle[1, 6].BackColor = Color.PowderBlue; // Ivory
            _gridStyle[1, 7].ForeColor = Color.Black; _gridStyle[1, 7].BackColor = Color.WhiteSmoke;
            _gridStyle[1, 8].ForeColor = Color.Black; _gridStyle[1, 8].BackColor = Color.WhiteSmoke; // SeaShell

            // scuro
            _gridStyle[2, 0].ForeColor = Color.White; _gridStyle[2, 0].BackColor = Color.Goldenrod;
            _gridStyle[2, 1].ForeColor = Color.White; _gridStyle[2, 1].BackColor = Color.IndianRed;
            _gridStyle[2, 2].ForeColor = Color.White; _gridStyle[2, 2].BackColor = Color.MediumSeaGreen;
            _gridStyle[2, 3].ForeColor = Color.White; _gridStyle[2, 3].BackColor = Color.MediumBlue;
            _gridStyle[2, 4].ForeColor = Color.White; _gridStyle[2, 4].BackColor = Color.MediumOrchid;
            _gridStyle[2, 5].ForeColor = Color.White; _gridStyle[2, 5].BackColor = Color.SaddleBrown;
            _gridStyle[2, 6].ForeColor = Color.White; _gridStyle[2, 6].BackColor = Color.SlateGray;  // DarkGray
            _gridStyle[2, 7].ForeColor = Color.White; _gridStyle[2, 7].BackColor = Color.SeaGreen;
            _gridStyle[2, 8].ForeColor = Color.White; _gridStyle[2, 8].BackColor = Color.SeaGreen;
        }

        /// <summary>imposta grafica TABS</summary>
        public void SetTabsAppearance()
        {
            bool bPaginaVuota;
            int i, j;
            String sTmp;

            MainGrid.RowCount = SF_Data.iGridRows;
            MainGrid.ColumnCount = SF_Data.iGridCols;

            iLastGridIndex = SF_Data.iGridRows * SF_Data.iGridCols;

            for (i = 0; i < PAGES_NUM_TABM; i++)
            {
                // verifica pagina vuota
                bPaginaVuota = true;

                for (j = i * iLastGridIndex; j < (i + 1) * iLastGridIndex; j++)
                {
                    if (!String.IsNullOrEmpty(SF_Data.Articolo[j].sTipo))
                    {
                        bPaginaVuota = false;
                        break;
                    }
                }

                if (bPaginaVuota)
                    sTmp = String.Format("  {0}  ( )", SF_Data.sPageTabs[i]);
                else
                    sTmp = String.Format("  {0}  ", SF_Data.sPageTabs[i]);

                switch (i)
                {
                    case 0: tabPage0.Text = sTmp; break;
                    case 1: tabPage1.Text = sTmp; break;
                    case 2: tabPage2.Text = sTmp; break;
                    case 3: tabPage3.Text = sTmp; break;
                    case 4:
                        if (SF_Data.bTouchMode)
                            tabPage4.Text = sTmp;
                        else
                            tabPage4.Text = "";
                        break;
                }
            }
        }

        /*****************************************************************
         *  gestisce varie segnalazioni fra cui l'antenna che segnala
         *  la risosta dal server NSC (Rosso)
         *****************************************************************/

        static bool bFlash = false, bPrevFlash = false, bFocusFlash = false;
        static bool bOrderProcessingRunning = false;

        static int iPrevFlash = 0, iFocusFlash = 0;

        private void MainFormTimer_Tick(object sender, EventArgs e)
        {
            bool bResult;
            int iPageNumTmp;
            String[] sEvQueueObj;

            // controlli vari dopo l'init della grafica e della coda eventi
            if (_bPrimaEsecuzione)
            {
                _bPrimaEsecuzione = false;

                // mette a posto la MainGrid
                FormResize(this, null);
                MainGrid_Redraw(this, null);
            }

            lblStatus_Time.Text = DateTime.Now.ToString("HH.mm.ss"); // STATUSBAR_TIME

            // lampeggio
            bFlash = !bFlash;

            if (CheckService(Define._AUTO_SEQ_TEST))
                lblStatus_Status.Text = sStatusText;
            // lampeggia quando uno dei menù è attivo
            else if (bFlash && !String.IsNullOrEmpty(lblStatus_Status.Text) && !lblStatus_Status.Text.Equals("Pronto"))
                lblStatus_Status.Text = "";
            else
                lblStatus_Status.Text = sStatusText;

            if ((iFocusFlash == 0) && EditStatus_BC.Focused)
            {
                iFocusFlash = 4;
                bFocusFlash = !bFocusFlash;

                if (bFocusFlash && EditStatus_BC.Focused)
                    EditStatus_BC.BackColor = Color.Gainsboro;
                else
                    EditStatus_BC.BackColor = Color.Honeydew;
            }
            else
            {
                if (iFocusFlash > 0)
                    iFocusFlash--;
            }

            if ((iPrevFlash == 0) && SF_Data.bPrevendita)
            {
                iPrevFlash = 5;
                bPrevFlash = !bPrevFlash;

                if (bPrevFlash && SF_Data.bPrevendita)
                    Text = TITLE + String.Format("{0,42}", sConst_Prevendita[1]);
                else
                    Text = String.Format("{0} {1} {2} {1} webcks Listino = {3}", TITLE, "    ", RELEASE_SW, DataManager.GetWebListinoChecksum());
            }
            else
            {
                if (iPrevFlash > 0)
                    iPrevFlash--;
            }

            // mette a posto il bottone di stampa
            if (_bPrintTimeoutEnabled)
            {
                if (_iPrintTimeout > 0)
                    _iPrintTimeout--;
                else
                {
                    _bPrintTimeoutEnabled = false;
                    ResetBtnScontrino();
                    LogToFile("Mainform : #W ResetBtnScontrino() per Timeout");
                }
            }

            // generazione automatica di scontrini debug se c'è la
            // giusta stringa nel Registro
            if (CheckService(Define._AUTO_RECEIPT_GEN))
                AutoTicketGenerator();  // ogni 250ms

            if (CheckService(Define._AUTO_SEQ_TEST))
                TestManager.CaricaTest();  // ogni 250ms

            // visualizzazione del Listino
            if ((_iVisPrzTimeout != 0) && BtnVisListino.Checked && !MnuImpListino.Checked)
            {
                _iVisPrzTimeout--;

                if (_iVisPrzTimeout == 0)
                {
                    EditCoperti.Text = _sCopertiPrev;

                    BtnVisListino.Checked = false;
                    CheckMenuItems();
                }
                else
                {
                    sStatusText = String.Format("Visual. Listino: Esc per uscire {0}s", _iVisPrzTimeout / 4);
                }
            }

            // gestione modo disponibilità
            if ((_iModDisponibilitaTimeout > 0) && MnuModDispArticoli.Checked)
            {
                if ((EditDispArticoliDlg.rDispDlg != null) && EditDispArticoliDlg.rDispDlg.Visible)
                {
                    _iModDisponibilitaTimeout = TIMEOUT_MOD_QUANTITA; // prolunga
                    sStatusText = String.Format("Modif. Disponibilità : Esc per uscire {0}s", _iModDisponibilitaTimeout / 4);
                }
                else
                {
                    _iModDisponibilitaTimeout--;
                    sStatusText = String.Format("Modif. Disponibilità : Esc per uscire {0}s", _iModDisponibilitaTimeout / 4);

                    if (_iModDisponibilitaTimeout == 0)
                    {
                        MnuModDispArticoli.Checked = false;
                        CheckMenuItems();
                    }
                }
            }

            if ((iFocus_BC_Timeout > 0) && (OptionsDlg._rOptionsDlg.GetPresales_LoadMode() || NetConfigDlg.rNetConfigDlg.GetWebOrderEnabled()))
            {
                iFocus_BC_Timeout--;

                if (iFocus_BC_Timeout == 0)
                {
                    iFocus_BC_Timeout = BC_FOCUS_TIMEOUT;
                    EditStatus_BC.Focus();
                    // EditStatus_BC.UseSystemPasswordChar = false;
                    EditStatus_BC.Text = ""; // pulizia
                }
            }

            // cambio TAB verso dx
            if ((MnuImpListino.Checked == true) && bPageDxProximity)
                iChangePageDxTimeout--;
            else
                iChangePageDxTimeout = CHANGE_PAGE_TIMEOUT;

            if (SF_Data.bTouchMode)
                iPageNumTmp = PAGES_NUM_TABM;
            else
                iPageNumTmp = PAGES_NUM_TXTM;

            if (iChangePageDxTimeout == 0)
                if (TabSet.SelectedIndex < (iPageNumTmp - 1))
                {
                    TabSet.SelectedIndex++;
                    iChangePageDxTimeout = 2 * CHANGE_PAGE_TIMEOUT;
                }

            // cambio TAB verso sx
            if ((MnuImpListino.Checked == true) && bPageSxProximity)
                iChangePageSxTimeout--;
            else
                iChangePageSxTimeout = CHANGE_PAGE_TIMEOUT;

            if (iChangePageSxTimeout == 0)
                if (TabSet.SelectedIndex > 0)
                {
                    TabSet.SelectedIndex--;
                    iChangePageSxTimeout = 2 * CHANGE_PAGE_TIMEOUT;
                }

            // indicazione Totale provvisorio
            if ((_iAnteprimaTotParziale > 0) || IsBitSet(SF_Data.iStatusSconto, BIT_SCONTO_GRATIS))
            {
                lblStatus_TC.Text = String.Format("TC = {0}", IntToEuro(_iAnteprimaTotParziale));
                lblStatus_TC.BackColor = Color.LightBlue;
            }
            else
            {
                lblStatus_TC.Text = "";
                lblStatus_TC.BackColor = SystemColors.Control;
            }

            /*************************************
             *       calcolo del resto
             *************************************/

            _sEditContante = EditContante.Text.Trim();

            if (!String.IsNullOrEmpty(_sEditContante))
            {
                _iContante = EuroToInt(_sEditContante, EURO_CONVERSION.EUROCONV_DONT_CARE, WrnMsg);

                if (_iContante != -1)
                {
                    EditResto.Text = IntToEuro(_iContante - _iAnteprimaTotParziale);
                    EditContante.BackColor = Color.Aquamarine;
                }
                else
                {
                    EditResto.Text = "";
                    EditContante.BackColor = Color.Red;
                }
            }
            else
                EditResto.Text = "";

            // aggiornamento disponibilità
            if (bUSA_NDB())
            {
                if (_iDBDispTimeout > 0) // 1min
                    _iDBDispTimeout--;
                else
                {
                    _iDBDispTimeout = _REFRESH_DISP;

                    if (!NetConfigDlg.rNetConfigDlg.Visible)
                        DataManager.AggiornaDisponibilità();

                    FormResize(null, null);
                    MainGrid_Redraw(this, null);
                }
            }

            // aggiorna icona
            if (_rdBaseIntf.GetDB_Connected())
                BtnDB.Image = BtnImgList.Images[1];
            else
                BtnDB.Image = BtnImgList.Images[0];

            // rimette a posto
            if (AnteprimaDlg.rAnteprimaDlg.Visible)
                MnuReceiptPreview.Checked = true;
            else
                MnuReceiptPreview.Checked = false;

            if ((EsploraRemOrdiniDB_Dlg.rEsploraRemOrdiniDB_Dlg != null) && EsploraRemOrdiniDB_Dlg.rEsploraRemOrdiniDB_Dlg.Visible)
                MnuEsploraOrdiniWeb.Checked = true;
            else
                MnuEsploraOrdiniWeb.Checked = false;

            /***********************************
             *      gestione coda eventi
             ***********************************/
            if (eventQueue.Count > 0)
            {
                sEvQueueObj = (String[])eventQueue.Peek();

                // nel caso di questo evento non è detto sia possibile stampare subito
                if (sEvQueueObj[0] != WEB_ORDER_PRINT_START)
                {
                    sEvQueueObj = (String[])eventQueue.Dequeue();
                }

                if (sEvQueueObj[0] == RESET_RECEIPT_BTN_EVENT)
                {
                    ResetBtnScontrino();
                }
                else if (sEvQueueObj[0] == PREV_ORDER_LOAD_DONE)
                {
                    _iAnteprimaTotParziale = AnteprimaDlg.GetTotaleReceipt();
                    EditStatus_BC.Focus();
                    MainGrid_Redraw(this, null);
                }
                else if (sEvQueueObj[0] == WEB_ORDER_LOAD_DONE)
                {
                    bool bEsploraAuto = false;

                    if ((EsploraRemOrdiniDB_Dlg.rEsploraRemOrdiniDB_Dlg != null) && !EsploraRemOrdiniDB_Dlg.rEsploraRemOrdiniDB_Dlg.GetAutoCheckbox())
                        bEsploraAuto = true;

                    /********************************************************************************************
                        importante perchè gli ordini con QR_Code si possono editare
                        quelli automatici da servlet invece no perchè mantengono _iAnteprimaTotParziale == 0
                    *********************************************************************************************/
                    if (!IsBitSet(DB_Data.iStatusReceipt, BIT_ORDINE_DIRETTO_DA_WEB) || bEsploraAuto)
                        _iAnteprimaTotParziale = AnteprimaDlg.GetTotaleReceipt();

                    // EditStatus_BC.UseSystemPasswordChar = false;
                    EditStatus_BC.Text = ""; // pulizia
                    MainGrid_Redraw(this, null);
                }
                else if (sEvQueueObj[0] == MAIN_GRID_UPDATE_EVENT)
                {
                    MainGrid_Redraw(this, null);
                }

                // solo se non c'è emissione di scontrini in corso _iAnteprimaTotParziale == 0
                else if ((sEvQueueObj[0] == WEB_ORDER_PRINT_START) && (_iAnteprimaTotParziale == 0))
                {
                    if ((_webPrintTimeout <= 0) && !bOrderProcessingRunning)
                    {
                        bOrderProcessingRunning = true;

                        // timeout lungo per eseguire operazioni
                        _webPrintTimeout = TIMEOUT_WEB_PRINT * 100;

                        MainGrid.Enabled = false;
                        EditNome.Enabled = false;
                        EditTavolo.Enabled = false;
                        EditCoperti.Enabled = false;
                        EditNota.Enabled = false;

                        // bOrderProcessingRunning evita che l'attesa di premere Ok provoca altri eventi del timer
                        bResult = DataManager.CaricaOrdineWeb(Convert.ToInt32(sEvQueueObj[1]));

                        // se non si può caricare ad esempio per checksum errato lo annulla
                        if (!bResult && !DB_Data.bAnnullato)
                            bResult = rdbAnnullaOrdine(SF_Data.iNumOrdineWeb);

                        sEvQueueObj = (String[])eventQueue.Dequeue();

                        MainGrid.Enabled = true;
                        EditNome.Enabled = true;
                        EditTavolo.Enabled = true;
                        EditCoperti.Enabled = true;
                        EditNota.Enabled = true;

                        MainGrid_Redraw(this, null);

                        // emissione scontrino
                        BtnScontrino_Click(sender, null);

                        // timeout corto
                        _webPrintTimeout = TIMEOUT_WEB_PRINT;

                        bOrderProcessingRunning = false;
                    }
                    else
                        _webPrintTimeout--;
                }

            }
        }

        /****************************************************
         *    genera delle ordinazioni in modo random per
         *    testare la comunicazione client-server
         ****************************************************/
        // il 4* per ottenere un conto in secondi
        static int iTicketGenCnt = 4 * 30; // 10s in avvio
        static int iTicketBtnPressedDelay;
        static bool bPressTicketBtn = false;
        static bool bInitDisp = false;

        void AutoTicketGenerator()
        {
            int i, iArticolo, iNumArticoli;

            Random iRandNum = new Random();

            iTicketGenCnt--;

            // generazione ordinazioni
            iNumArticoli = 6 + iRandNum.Next(12); // media 12 voci

            // inizializza le disponibilità, solo se è _USA_DB limita alla CASSA_PRINCIPALE
            if (!bInitDisp && (_iNumOfOrders == 0) && ((SF_Data.iNumCassa == CASSA_PRINCIPALE) || !bUSA_NDB()))
            {
                bInitDisp = true;

                for (i = 0; i < 2 * iNumArticoli; i++)
                {
                    iArticolo = iRandNum.Next(DataManager.CheckLastArticoloIndexP1() - 1);

                    if (!String.IsNullOrEmpty(SF_Data.Articolo[iArticolo].sTipo))
                        SF_Data.Articolo[iArticolo].iDisponibilita = 20 + iRandNum.Next(200);
                }
            }

            if (iTicketGenCnt == 0)
            {
                iTicketGenCnt = 4 * (15 + iRandNum.Next(10)); // media 20s;

                for (i = 0; i < iNumArticoli; i++)
                {
                    iArticolo = iRandNum.Next(DataManager.CheckLastArticoloIndexP1() - 1);

                    if (!String.IsNullOrEmpty(SF_Data.Articolo[iArticolo].sTipo) && (SF_Data.Articolo[iArticolo].iDisponibilita > 0))
                        SF_Data.Articolo[iArticolo].iQuantitaOrdine = 1 + iRandNum.Next(16);
                }

                // altrimenti se le 2 casse collidono continua la generazione di errori
                for (i = 0; i < MAX_NUM_ARTICOLI - 1; i++)
                {
                    if (SF_Data.Articolo[i].iQuantitaOrdine > SF_Data.Articolo[i].iDisponibilita)
                        SF_Data.Articolo[i].iQuantitaOrdine = SF_Data.Articolo[i].iDisponibilita;
                }

                bPressTicketBtn = true;
                iTicketBtnPressedDelay = 4 * 5; // 5s

                // *** ESPORTAZIONE ***
                BtnEsportazione.Checked = (iRandNum.Next(4) == 0);

                // *** NOTE ***
                if (iRandNum.Next(5) == 0) // 1 su 5
                    EditNota.Text = String.Format("CIAO {0}", iRandNum.Next(100));

                // *** TAVOLO ***
                if (iRandNum.Next(4) == 0) // 1 su 4
                    EditTavolo.Text = iRandNum.Next(100).ToString();

                // *** COPERTI ***
                if (iRandNum.Next(4) == 0) // 1 su 4
                    EditCoperti.Text = iRandNum.Next(10).ToString();

                // *** SCONTO ***
                if (iRandNum.Next(10) == 0) // 1 su 10
                    BtnSconto.Checked = true;

                _sEditTavolo = EditTavolo.Text.Trim();
                _sEditNota = EditNota.Text.Trim();
                _sEditCoperti = EditCoperti.Text.Trim();

                BtnScontrino.Checked = true;
                AnteprimaDlg.rAnteprimaDlg.RedrawReceipt();
                _iAnteprimaTotParziale = AnteprimaDlg.GetTotaleReceipt();
                MainGrid_Redraw(this, null);
            }

            if (bPressTicketBtn)
            {
                iTicketBtnPressedDelay--;

                if (iTicketBtnPressedDelay == 0)
                {
                    // emissione scontrino
                    BtnScontrino_Click(this, null);
                    bPressTicketBtn = false;
                    MainGrid_Redraw(this, null);
                }
            }
        }

        private void MainGrid_Resize(object sender, EventArgs e)
        {
            Console.WriteLine("MainGrid_Resize");
            FormResize(sender, null);
        }

        /// <summary>
        /// calcola la dimensione delle celle della griglia
        /// e ridisegna il contenuto della griglia
        /// </summary>
        public void FormResize(object sender, EventArgs e)
        {
            int i, j, k, h;
            int iRowsHeight, iPrimoGruppoStampa, iGruppoStampa;
            String sText;

            float fColumnsWidth, fTextSize;

            // Toolbar
            TabSet.Width = lblNome.Location.X - 10;

            // tutto dipende dal topPanel
            topPanel.Width = this.Size.Width - 22;

            if (SF_Data.bTouchMode) // priorità
            {
                TabSet.Height = 34;
                TabSet.ItemSize = new Size(67, TabSet.Height - 3);

                EditCoperti.Font = new Font("Microsoft Sans Serif", 15);

                BtnScontrino.Size = new Size(80, 60);

                topPanel.Height = 60;

                toolStripR.Visible = true;
                toolStripR.Enabled = true;
                toolStripR.Width = 82;

                EditCoperti.Left = topPanel.Width - toolStripR.Width / 2 - EditCoperti.Width / 2 - 2;

                btnSep_R1.Height = toolStripR.Height / 10;
                btnSep_R2.Height = toolStripR.Height / 12;
                btnSep_R3.Height = btnSep_R2.Height;
            }
            else if (OptionsDlg._rOptionsDlg.Get_VButtons())
            {
                TabSet.Height = 26;
                TabSet.ItemSize = new Size(67, TabSet.Height - 3);

                EditCoperti.Font = new Font("Microsoft Sans Serif", 12);

                BtnScontrino.Size = new Size(45, 38);

                topPanel.Height = 42;

                toolStripR.Visible = true;
                toolStripR.Enabled = true;
                toolStripR.Width = 82;

                EditCoperti.Left = topPanel.Width - toolStripR.Width / 2 - EditCoperti.Width / 2 - 2;

                btnSep_R1.Height = toolStripR.Height / 10;
                btnSep_R2.Height = toolStripR.Height / 12;
                btnSep_R3.Height = btnSep_R2.Height;
            }
            else
            {
                TabSet.Height = 26;
                TabSet.ItemSize = new Size(67, TabSet.Height - 3);

                EditCoperti.Font = new Font("Microsoft Sans Serif", 12);

                BtnScontrino.Size = new Size(45, 38);

                topPanel.Height = 42;

                toolStripR.Visible = false;
                toolStripR.Enabled = false;
                toolStripR.Width = 2;

                EditCoperti.Left = topPanel.Width - 75;
            }

            // tutto dipende dal topPanel
            toolStrip.Width = topPanel.Width - toolStrip.Left - 80; // era 150
            toolStrip.Height = topPanel.Height - 2;

            // posizionamento verticale elementi
            TabSet.Top = MainMenu.Height + topPanel.Height + topPanel.Margin.Top + topPanel.Margin.Bottom;

            EditTavolo.Top = TabSet.Top;
            EditCoperti.Top = TabSet.Top;
            EditNome.Top = TabSet.Top;

            EditTavolo.Font = EditCoperti.Font;
            EditNome.Font = EditCoperti.Font;

            lblCoperti.Top = TabSet.Top + 2;
            lblTavolo.Top = lblCoperti.Top;
            lblNome.Top = lblCoperti.Top;

            MainGrid.Top = TabSet.Top + TabSet.Height;
            toolStripR.Top = MainGrid.Top;
            //lblPagato.Top = lblResto.Top; provoca modifica Anchor !

            toolStripR.Left = topPanel.Width - toolStripR.Width - MainGrid.Location.X;

            lblCoperti.Left = EditCoperti.Left - lblCoperti.Width - 2;

            EditTavolo.Left = EditCoperti.Left - EditTavolo.Width - 80;
            lblTavolo.Left = EditTavolo.Left - lblTavolo.Width - 2;

            EditNome.Left = EditTavolo.Left - EditNome.Width - 80;
            lblNome.Left = EditNome.Left - lblNome.Width - 2;

            //lblStatusResto.Left = EditStatusResto.Left - lblStatusResto.Width-2;
            //lblStatusPagato.Left = EditStatusContante.Left - lblStatusPagato.Width-2;

            BtnVisListino.Size = BtnScontrino.Size;
            BtnSendMsg.Size = BtnScontrino.Size;
            BtnX10.Size = BtnScontrino.Size;
            BtnEsportazione.Size = BtnScontrino.Size;
            BtnSconto.Size = BtnScontrino.Size;
            BtnDB.Size = BtnScontrino.Size;

            // imposta la larghezza della griglia in base alla larghezza della form principale
            MainGrid.Width = topPanel.Width - toolStripR.Width - MainGrid.Location.X * 2;

            // imposta l'altezza della griglia in base all'altezza della form principale
            MainGrid.Height = Height - MainMenu.Size.Height - toolStrip.Size.Height - TabSet.Height -
                                StatusBar_Upper.Size.Height - StatusBar.Size.Height - 52;

            // imposta altezza delle righe sulla base della altezza della finestra
            if ((WindowState != FormWindowState.Minimized) && (MainGrid.RowCount > 0))
            {
                iPrimoGruppoStampa = CheckFirstGroupIndex();
                _prevStyle.ForeColor = _gridStyle[_iColorTheme, iPrimoGruppoStampa].ForeColor;
                _prevStyle.BackColor = _gridStyle[_iColorTheme, iPrimoGruppoStampa].BackColor;

                if (MnuImpListino.Checked || BtnVisListino.Checked)
                {
                    if (SF_Data.bTouchMode)
                        sText = sGlbWinPrinterParams.bChars33 ? "12345678901234567890123" : "123456789012345678";
                    else
                        // String.Format(" {0,2} {1,-18} {2,5:0.00}", // width=28
                        sText = sGlbWinPrinterParams.bChars33 ? "123456789012345678901234567809123" : "1234567890123456789012345678";
                }
                else
                {
                    if (SF_Data.bTouchMode)
                        sText = sGlbWinPrinterParams.bChars33 ? "12345678901234567809123" : "123456789012345678";
                    else
                        // String.Format("{0,3} {1,-18} {2,2}" : // width=25
                        sText = sGlbWinPrinterParams.bChars33 ? "123456789012345678901234567890" : "1234567890123456789012345";
                }

                if ((MainGrid.Height % MainGrid.RowCount) < 3)
                    iRowsHeight = (MainGrid.Height / MainGrid.RowCount) - 1; // evita scroll verticale griglia
                else
                    iRowsHeight = MainGrid.Height / MainGrid.RowCount;


                for (i = 0; i < MainGrid.RowCount; i++)
                    MainGrid.Rows[i].Height = iRowsHeight;

                fColumnsWidth = MainGrid.Columns[0].Width;

                fTextSize = TextRenderer.MeasureText(sText, MainGrid.DefaultCellStyle.Font).Width;
                _fFontWidth = fColumnsWidth / sText.Length + 0.5f;

                // avvio form
                if (_fFontWidth < 5.0)
                    _fFontWidth = ((float)iRowsHeight * 0.45f);

                //Console.WriteLine("_fFontWidth = {0}, {1}, {2}, {3}", _fFontWidth, fColumnsWidth, fTextSize, sText.Length);

                MainGrid.Font = new Font(MainGrid.DefaultCellStyle.Font.Name, _fFontWidth);

                // Grid appearance
                MainGrid.GridColor = _gridCrossStyle[_iColorTheme].ForeColor;
                MainGrid.DefaultCellStyle.SelectionForeColor = _selectedCellStyle[_iColorTheme].ForeColor;
                MainGrid.DefaultCellStyle.SelectionBackColor = _selectedCellStyle[_iColorTheme].BackColor;
                MainGrid.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                //sText += (Environment.NewLine + "\r\n --------------");
            }

            // prepara l'aspetto della stringa visualizzata nelle celle
            for (k = 0; ((k < iLastGridIndex) && (MainGrid.RowCount > 0)); k++)
            {
                i = k % MainGrid.RowCount;
                j = k / MainGrid.RowCount;

                h = k + iArrayOffset;

                iGruppoStampa = SF_Data.Articolo[h].iGruppoStampa;

                if (MnuImpListino.Checked || BtnVisListino.Checked)
                {
                    if (String.IsNullOrEmpty(SF_Data.Articolo[h].sTipo))
                    {
                        MainGrid.Rows[i].Cells[j].Style = new DataGridViewCellStyle(_prevStyle);
                    }
                    else
                    {
                        MainGrid.Rows[i].Cells[j].Style.ForeColor = _gridStyle[_iColorTheme, iGruppoStampa].ForeColor;
                        MainGrid.Rows[i].Cells[j].Style.BackColor = _gridStyle[_iColorTheme, iGruppoStampa].BackColor;

                        _prevStyle = new DataGridViewCellStyle(MainGrid.Rows[i].Cells[j].Style);
                        MainGrid.Rows[i].Cells[j].Style.Font = new Font(MainGrid.DefaultCellStyle.Font.Name, _fFontWidth, FontStyle.Regular);
                    }
                }
                else
                {
                    // Disponibilità zero colore rosso
                    if (SF_Data.Articolo[h].iDisponibilita == 0)
                    {
                        MainGrid.Rows[i].Cells[j].Style.ForeColor = _zeroAvailabilityStyle[_iColorTheme].ForeColor;
                        MainGrid.Rows[i].Cells[j].Style.BackColor = _gridStyle[_iColorTheme, iGruppoStampa].BackColor;

                        MainGrid.Rows[i].Cells[j].Style.Font = new Font(MainGrid.DefaultCellStyle.Font.Name, _fFontWidth, FontStyle.Bold);
                    }
                    else if (String.IsNullOrEmpty(SF_Data.Articolo[h].sTipo))
                    {
                        MainGrid.Rows[i].Cells[j].Style = new DataGridViewCellStyle(_prevStyle);
                    }
                    else
                    {
                        MainGrid.Rows[i].Cells[j].Style.ForeColor = _gridStyle[_iColorTheme, iGruppoStampa].ForeColor;
                        MainGrid.Rows[i].Cells[j].Style.BackColor = _gridStyle[_iColorTheme, iGruppoStampa].BackColor;

                        _prevStyle = new DataGridViewCellStyle(MainGrid.Rows[i].Cells[j].Style);

                        MainGrid.Rows[i].Cells[j].Style.Font = new Font(MainGrid.DefaultCellStyle.Font.Name, _fFontWidth, FontStyle.Regular);
                    }
                }

                // Grid appearance: estende il colore dell'angolo in basso a destra a tutta la griglia (in particolare al bordo inferiore)
                if (h == MainGrid.RowCount * MainGrid.ColumnCount + iArrayOffset - 1)
                    MainGrid.BackgroundColor = MainGrid.Rows[i].Cells[j].Style.BackColor;
            }

            //topPanel.Refresh();
            //TabSet.Refresh();
            toolStripR.Refresh();

            MainGrid.Refresh();
        } // end FormResize


        /// <summary>
        /// ridisegna il contenuto della griglia principale
        /// </summary>
        public void MainGrid_Redraw(object sender, EventArgs e)
        {
            int i, j, k, h;
            String sTipoTmp, sTickQty, sDispQty, sPrzTmp, sCellText;
            String sToolTip;

            // prepara la stringa visualizzata nelle celle
            for (k = 0; ((k < iLastGridIndex) && (MainGrid.RowCount > 0)); k++)
            {
                i = k % MainGrid.RowCount;
                j = k / MainGrid.RowCount;

                h = k + iArrayOffset;

                if (!String.IsNullOrEmpty(SF_Data.Articolo[h].sTipo) && IsBitSet(SF_Data.Articolo[h].iOptionsFlags, BIT_STAMPA_SINGOLA_NELLA_COPIA_RECEIPT))
                    sTipoTmp = SF_Data.Articolo[h].sTipo.Insert(0, "$ ");
                else
                    sTipoTmp = SF_Data.Articolo[h].sTipo;

                if (SF_Data.Articolo[h].iQuantitaOrdine != 0)
                    sTickQty = SF_Data.Articolo[h].iQuantitaOrdine.ToString();
                else
                    sTickQty = "";

                if (SF_Data.Articolo[h].iDisponibilita != DISP_OK)
                    sDispQty = SF_Data.Articolo[h].iDisponibilita.ToString();
                else
                    sDispQty = "";

                sCellText = "";

                if ((MnuImpListino.Checked || BtnVisListino.Checked) && !String.IsNullOrEmpty(SF_Data.Articolo[h].sTipo))
                {

                    sPrzTmp = IntToEuro(SF_Data.Articolo[h].iPrezzoUnitario);

                    switch (SF_Data.Articolo[h].iGruppoStampa)
                    {
                        // ******** allineamento *******
                        // 89 123456789012345678 9876.00
                        // fPrint.WriteLine("{0,2} {1,-18} {2,5}", // width=28
                        case (int)DEST_TYPE.DEST_TIPO1:
                            if (SF_Data.bTouchMode)
                                sCellText = String.Format(sGRD_FMT_TCH, "G1", sPrzTmp, Environment.NewLine, CenterJustify(SF_Data.Articolo[h].sTipo, 18));
                            else
                                sCellText = String.Format(sGRD_FMT_STD, "G1", SF_Data.Articolo[h].sTipo, sPrzTmp);
                            break;
                        case (int)DEST_TYPE.DEST_TIPO2:
                            if (SF_Data.bTouchMode)
                                sCellText = String.Format(sGRD_FMT_TCH, "G2", sPrzTmp, Environment.NewLine, CenterJustify(SF_Data.Articolo[h].sTipo, 18));
                            else
                                sCellText = String.Format(sGRD_FMT_STD, "G2", SF_Data.Articolo[h].sTipo, sPrzTmp);
                            break;
                        case (int)DEST_TYPE.DEST_TIPO3:
                            if (SF_Data.bTouchMode)
                                sCellText = String.Format(sGRD_FMT_TCH, "G3", sPrzTmp, Environment.NewLine, CenterJustify(SF_Data.Articolo[h].sTipo, 18));
                            else
                                sCellText = String.Format(sGRD_FMT_STD, "G3", SF_Data.Articolo[h].sTipo, sPrzTmp);
                            break;
                        case (int)DEST_TYPE.DEST_TIPO4:
                            if (SF_Data.bTouchMode)
                                sCellText = String.Format(sGRD_FMT_TCH, "G4", sPrzTmp, Environment.NewLine, CenterJustify(SF_Data.Articolo[h].sTipo, 18));
                            else
                                sCellText = String.Format(sGRD_FMT_STD, "G4", SF_Data.Articolo[h].sTipo, sPrzTmp);
                            break;
                        case (int)DEST_TYPE.DEST_TIPO5:
                            if (SF_Data.bTouchMode)
                                sCellText = String.Format(sGRD_FMT_TCH, "G5", sPrzTmp, Environment.NewLine, CenterJustify(SF_Data.Articolo[h].sTipo, 18));
                            else
                                sCellText = String.Format(sGRD_FMT_STD, "G5", SF_Data.Articolo[h].sTipo, sPrzTmp);
                            break;
                        case (int)DEST_TYPE.DEST_TIPO6:
                            if (SF_Data.bTouchMode)
                                sCellText = String.Format(sGRD_FMT_TCH, "G6", sPrzTmp, Environment.NewLine, CenterJustify(SF_Data.Articolo[h].sTipo, 18));
                            else
                                sCellText = String.Format(sGRD_FMT_STD, "G6", SF_Data.Articolo[h].sTipo, sPrzTmp);
                            break;
                        case (int)DEST_TYPE.DEST_TIPO7:
                            if (SF_Data.bTouchMode)
                                sCellText = String.Format(sGRD_FMT_TCH, "G7", sPrzTmp, Environment.NewLine, CenterJustify(SF_Data.Articolo[h].sTipo, 18));
                            else
                                sCellText = String.Format(sGRD_FMT_STD, "G7", SF_Data.Articolo[h].sTipo, sPrzTmp);
                            break;
                        case (int)DEST_TYPE.DEST_TIPO8:
                            if (SF_Data.bTouchMode)
                                sCellText = String.Format(sGRD_FMT_TCH, "G8", sPrzTmp, Environment.NewLine, CenterJustify(SF_Data.Articolo[h].sTipo, 18));
                            else
                                sCellText = String.Format(sGRD_FMT_STD, "G8", SF_Data.Articolo[h].sTipo, sPrzTmp);
                            break;
                        case (int)DEST_TYPE.DEST_COUNTER:
                            if (SF_Data.bTouchMode)
                                sCellText = String.Format(sGRD_FMT_TCH, "CN", sPrzTmp, Environment.NewLine, CenterJustify(SF_Data.Articolo[h].sTipo, 18));
                            else
                                sCellText = String.Format(sGRD_FMT_STD, "CN", SF_Data.Articolo[h].sTipo, sPrzTmp);
                            break;
                    }

                    // impostazione dei ToolTip di cella
                    if (String.IsNullOrEmpty(SF_Data.Articolo[h].sTipo))
                        sToolTip = "";
                    else
                    {
                        sToolTip = string.Format("Articolo: {0}\n", SF_Data.Articolo[h].sTipo);

                        sToolTip += string.Format("   prezzo: {0}\n", IntToEuro(SF_Data.Articolo[h].iPrezzoUnitario));

                        sToolTip += string.Format("   gruppo stampa: {0}\n", SF_Data.sCopiesGroupsText[SF_Data.Articolo[h].iGruppoStampa]);

                        sToolTip += string.Format("   {0}: {1}\n", "gruppo breve", sConstGruppiShort[SF_Data.Articolo[h].iGruppoStampa]);

                    }

                    MainGrid.Rows[i].Cells[j].ToolTipText = sToolTip;
                }
                else
                {
                    // solo se disponibilità zero colore rosso
                    if (SF_Data.Articolo[h].iDisponibilita == 0)
                    {
                        MainGrid.Rows[i].Cells[j].Style.ForeColor = _zeroAvailabilityStyle[_iColorTheme].ForeColor;
                        MainGrid.Rows[i].Cells[j].Style.BackColor = _gridStyle[_iColorTheme, SF_Data.Articolo[h].iGruppoStampa].BackColor;

                        MainGrid.Rows[i].Cells[j].Style.Font = new Font(MainGrid.DefaultCellStyle.Font.Name, _fFontWidth, FontStyle.Bold);
                    }

                    if (SF_Data.bTouchMode)
                    {
                        if (string.IsNullOrEmpty(sDispQty))
                            sCellText = String.Format(sGRDZ_FMT_TCH, sTickQty, Environment.NewLine, CenterJustify(sTipoTmp, 18));
                        else
                            sCellText = String.Format(sGRDW_FMT_TCH, sDispQty, sTickQty, Environment.NewLine, CenterJustify(sTipoTmp, 18));
                    }
                    else
                        sCellText = String.Format(sGRDW_FMT_STD, sDispQty, sTipoTmp, sTickQty);

                    // impostazione dei ToolTip di cella
                    if (String.IsNullOrEmpty(SF_Data.Articolo[h].sTipo) || String.IsNullOrEmpty(SF_Data.Articolo[h].sNotaArt))
                    {
                        MainGrid.Rows[i].Cells[j].ToolTipText = "";
                    }
                    else
                    {
                        sToolTip = string.Format("Nota: {0}", SF_Data.Articolo[h].sNotaArt);
                        MainGrid.Rows[i].Cells[j].ToolTipText = sToolTip;
                    }
                }

#if FONT_CHECK
                if (!_bPrimaEsecuzione && (((i + j + iArrayOffset) % 6) == 0))
                    MainGrid.Rows[i].Cells[j].Value = sText;
                else
                    MainGrid.Rows[i].Cells[j].Value = sCellText;
#else
                if (!_bPrimaEsecuzione)
                    MainGrid.Rows[i].Cells[j].Value = sCellText;
#endif
            }

            // aggiorna il web checksum in caso di modifica Listino
            Text = String.Format("{0} {1} {2} {1} webcks Listino = {3}", TITLE, "    ", RELEASE_SW, DataManager.GetWebListinoChecksum());

            MainGrid.Refresh();
        } // end MainGrid_Redraw


        /********************************************************
         *	chiama il dialogo di modifica del record TArticolo
         ********************************************************/
        private void MainGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // punto unico _iPt_DoubleClick =
            _iCellPt = e.ColumnIndex * MainGrid.RowCount + e.RowIndex + iArrayOffset;

            if (MnuModDispArticoli.Checked)
            {
                EditDispArticoliDlg rDispDlg = new EditDispArticoliDlg();
                rDispDlg.Init(_iCellPt);

                FormResize(null, null);
                MainGrid_Redraw(this, null);
                rDispDlg.Dispose();
            }
            else if (MnuImpListino.Checked)
            {
                EditArticoloDlg rModificaArticoloDlg = new EditArticoloDlg();
                rModificaArticoloDlg.Init(_iCellPt, true);

                _bListinoModificato |= EditArticoloDlg.GetListinoModificato();

                EditCoperti.Text = String.Format("{0,4:0.00}", SF_Data.Articolo[MAX_NUM_ARTICOLI - 1].iPrezzoUnitario / 100.0f);

                CheckMenuItems();
                rModificaArticoloDlg.Dispose();
            }

            bSkipDrag = true; // evitare un BeginDrag sul doppio click
        }

        /*****************************
         *     Inizio di un Drag
         *****************************/
        private void MainGrid_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            // il click del mouse annulla il conteggio per barcode scanner detect
            scannerInputQueue.Clear();

            iSwapStartIndex = e.ColumnIndex * MainGrid.RowCount + e.RowIndex + iArrayOffset;

            if (MnuImpListino.Checked && (e.Button == MouseButtons.Left) && !bSkipDrag && !bMouseWrongPos)
            {
                bStartDrag = true;
                Cursor = Cursors.Hand;
            }

            bSkipDrag = false;
        }

        /*******************************************************
         *	Fine di un Drag, si molla il tasto Left del mouse
         *******************************************************/
        private void MainGrid_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            int m, n, iLastArticoloPageIndex;
            int ToCellX, ToCellY;
            TArticolo tmpArticolo;

            // altrimenti può rimanere true
            bPageDxProximity = false;
            bPageSxProximity = false;

            if (MnuImpListino.Checked && (e.Button == MouseButtons.Left) && bStartDrag)
            {
                ToCellX = e.ColumnIndex;
                ToCellY = e.RowIndex;

                m = ToCellX * MainGrid.RowCount + ToCellY + iArrayOffset;
                n = iSwapStartIndex;

                if (SF_Data.bTouchMode)
                    iLastArticoloPageIndex = SF_Data.iGridRows * SF_Data.iGridCols * PAGES_NUM_TABM;
                else
                    iLastArticoloPageIndex = SF_Data.iGridRows * SF_Data.iGridCols * PAGES_NUM_TXTM;

                // controllo per evitare sforamenti del vettore
                if ((m < iLastArticoloPageIndex) && (n < iLastArticoloPageIndex) && (n != m))
                {
                    tmpArticolo = SF_Data.Articolo[m];

                    SF_Data.Articolo[m] = SF_Data.Articolo[n];
                    SF_Data.Articolo[n] = tmpArticolo;
                    _bListinoModificato = true;
                }

                bStartDrag = false;
                Cursor = Cursors.Default;

                DataManager.CheckLastArticoloIndexP1();

                FormResize(null, null);
                MainGrid_Redraw(this, null);
            }
        }

        // il mouse pointer è dentro MainGrid
        private void MainGrid_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ((e.ColumnIndex >= 0) && (e.ColumnIndex < MainGrid.ColumnCount) && (e.RowIndex >= 0) && (e.RowIndex < MainGrid.RowCount) &&
                (e.Button != MouseButtons.Left))
            {
                Cursor = Cursors.Default;
            }

            bMouseWrongPos = false; // il mouse pointer è dentro MainGrid
        }

        // il mouse pointer è fuori MainGrid, non funziona
        private void MainGrid_DragLeave(object sender, EventArgs e)
        {
            if (MnuImpListino.Checked)
            {
                Cursor = Cursors.No;
                bMouseWrongPos = true; // il mouse pointer non è in MainGrid
            }
        }

        // simula MainGridDragOver verifica la posizione rispetto ai bordi
        private void MainGrid_MouseMove(object sender, MouseEventArgs e)
        {
            bool bModArtVisibile;

            if (EditArticoloDlg.rModificaArticoloDlg != null)
                bModArtVisibile = EditArticoloDlg.rModificaArticoloDlg.Visible;
            else
                bModArtVisibile = false;

            if (((e.Y < 5) || ((MainGrid.Height - e.Y) < 5)) && MnuImpListino.Checked && !bModArtVisibile && bStartDrag)
            {
                // esce da MainGrid
                Cursor = Cursors.No;
                bPageDxProximity = false;
                bPageSxProximity = false;
            }
            else if (((MainGrid.Width - e.X) < 50) && MnuImpListino.Checked && !bModArtVisibile && bStartDrag)
            {
                // si sposta a destra
                MainGrid.Cursor = Cursors.AppStarting;
                bPageDxProximity = true;
            }
            else if ((e.X < 50) && MnuImpListino.Checked && !bModArtVisibile && bStartDrag)
            {
                // si sposta a sinistra
                MainGrid.Cursor = Cursors.AppStarting;
                bPageSxProximity = true;
            }
            else if (bStartDrag && !bMouseWrongPos)
            {
                Cursor = Cursors.Hand;
                bPageDxProximity = false;
                bPageSxProximity = false;
            }
            else if (bMouseWrongPos)
            {
                Cursor = Cursors.No;
                bPageDxProximity = false;
                bPageSxProximity = false;
            }
            else
            {
                Cursor = Cursors.Default;
                bPageDxProximity = false;
                bPageSxProximity = false;
            }

            if (BtnVisListino.Checked && BtnVisListino.Enabled)
                _iVisPrzTimeout = TIMEOUT_VIS_PREZZI;
        }

        /// <summary>
        /// special processing for KEY_TAB, KEY_UP, KEY_DOWN, KEY_LEFT, KEY_RIGHT
        /// </summary>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            int iPageNumTmp, iRow, iCol;

            if (SF_Data.bTouchMode)
                iPageNumTmp = PAGES_NUM_TABM;
            else
                iPageNumTmp = PAGES_NUM_TXTM;

            // gestione TABS
            if ((MainGrid.Focused || TabSet.Focused) && (keyData == (Keys.Tab | Keys.Control))) // indietro
            {
                TabSet.SelectedIndex = (TabSet.SelectedIndex + iPageNumTmp - 1) % iPageNumTmp;
                return true;
            }
            else if ((MainGrid.Focused || TabSet.Focused) && (keyData == Keys.Tab)) // avanti
            {
                TabSet.SelectedIndex = (TabSet.SelectedIndex + 1) % iPageNumTmp;
                return true;
            }
            else if ((MainGrid.Focused || TabSet.Focused) && (
                (keyData == Keys.Up) || (keyData == Keys.Down) || (keyData == Keys.Left) || (keyData == Keys.Right) ||
                (keyData == Keys.Home) || (keyData == Keys.End) || (keyData == Keys.Escape)))
            {
                // il movimento con frecce annulla il conteggio per barcode scanner detect
                scannerInputQueue.Clear();

                MainGrid.Focus();

                if (VerificaQuantita())
                    return base.ProcessCmdKey(ref msg, keyData);
                else
                    return true; // cursore rimane sul posto
            }
            else if ((MainGrid.Focused || TabSet.Focused) && (keyData == Keys.Enter))
            {
                if (OptionsDlg._rOptionsDlg.GetEnterPrintReceipt())
                    BtnScontrino_Click(this, null);

                return true; // cursore rimane sul posto
            }
            else if (EditNota.Focused && (keyData == Keys.Enter))
            {
                if (EditNota.BackColor == Color.LightBlue)
                {
                    // acquisisce Nota Articolo
                    SF_Data.Articolo[_iCellPt].sNotaArt = EditNota.Text;

                    EditNota.BackColor = Color.Gainsboro;
                    EditNota.Text = _sEditNotaCopy;
                    EditNota.MaxLength = 28;
                    lblNota.Text = "Nota:";

                    MainGrid_Redraw(this, null);
                }

                return true; // cursore rimane sul posto
            }
            // gestione scorrimento UP nella matrice
            else if (MainGrid.Focused && MnuImpListino.Checked && (keyData == (Keys.Up | Keys.Control)))
            {
                _iCellPt = MainGrid.CurrentCell.ColumnIndex * MainGrid.RowCount + MainGrid.CurrentCell.RowIndex + iArrayOffset;
                String sTime = DateTime.Now.ToString("HH.mm.ss");

                iRow = _iCellPt % MainGrid.RowCount;
                iCol = (_iCellPt % (MainGrid.RowCount * MainGrid.ColumnCount)) / MainGrid.RowCount;

                Console.WriteLine("ProcessCmdKey1: KEY_UP _iCellPt = {0}, {1}, {2}, {3}", _iCellPt, iRow, iCol, sTime);

                if (_iCellPt > 0)
                {
                    Console.WriteLine("ProcessCmdKey2: KEY_UP");

                    TArticolo tmpArticolo = new TArticolo(SF_Data.Articolo[_iCellPt - 1]);

                    SF_Data.Articolo[_iCellPt - 1] = SF_Data.Articolo[_iCellPt];
                    SF_Data.Articolo[_iCellPt] = tmpArticolo;

                    // siamo lungo il bordo superiore, cambio colonna
                    if (iRow == 0)
                    {
                        Console.WriteLine("ProcessCmdKey3: KEY_UP cambio colonna");

                        if (iCol > 0)
                            MainGrid.CurrentCell = MainGrid.Rows[MainGrid.RowCount - 1].Cells[iCol - 1];
                        else
                            MainGrid.CurrentCell = MainGrid.Rows[MainGrid.RowCount - 1].Cells[MainGrid.ColumnCount - 1];

                        SelectTab(_iCellPt - 1);
                        MainGrid.Focus();
                    }
                    else
                        MainGrid.CurrentCell = MainGrid.Rows[iRow - 1].Cells[iCol];

                    _bListinoModificato = true;

                    FormResize(null, null);
                    MainGrid_Redraw(this, null);

                    return true;
                }
            }
            // gestione scorrimento DOWN nella matrice
            else if (MainGrid.Focused && MnuImpListino.Checked && (keyData == (Keys.Down | Keys.Control)))
            {
                _iCellPt = MainGrid.CurrentCell.ColumnIndex * MainGrid.RowCount + MainGrid.CurrentCell.RowIndex + iArrayOffset;
                String sTime = DateTime.Now.ToString("HH.mm.ss");

                iRow = _iCellPt % MainGrid.RowCount;
                iCol = (_iCellPt % (MainGrid.RowCount * MainGrid.ColumnCount)) / MainGrid.RowCount;

                Console.WriteLine("ProcessCmdKey1: KEY_DOWN _iCellPt = {0}, {1}, {2}, {3}", _iCellPt, iRow, iCol, sTime);

                if (_iCellPt < (iLastGridIndex * iPageNumTmp - 1))
                {
                    Console.WriteLine("ProcessCmdKey2: KEY_DOWN");

                    TArticolo tmpArticolo = new TArticolo(SF_Data.Articolo[_iCellPt + 1]);

                    SF_Data.Articolo[_iCellPt + 1] = SF_Data.Articolo[_iCellPt];
                    SF_Data.Articolo[_iCellPt] = tmpArticolo;

                    // siamo lungo il bordo superiore, cambio colonna
                    if (iRow == MainGrid.RowCount - 1)
                    {
                        Console.WriteLine("ProcessCmdKey3: KEY_DOWN cambio colonna");

                        if (iCol < (MainGrid.ColumnCount - 1))
                            MainGrid.CurrentCell = MainGrid.Rows[0].Cells[iCol + 1];
                        else
                            MainGrid.CurrentCell = MainGrid.Rows[0].Cells[0];

                        SelectTab(_iCellPt + 1);
                        MainGrid.Focus();
                    }
                    else
                        MainGrid.CurrentCell = MainGrid.Rows[iRow + 1].Cells[iCol];

                    _bListinoModificato = true;

                    FormResize(null, null);
                    MainGrid_Redraw(this, null);

                    return true;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        /*****************************************
         *       gestione pressione tasti
         *****************************************/
        private void MainGrid_KeyDown(object sender, KeyEventArgs e)
        {
            int i, iD, iU;
            int iKey = (int)e.KeyValue;
            int iPageNumTmp;

            String sTmp;

            if (SF_Data.bTouchMode)
                iPageNumTmp = PAGES_NUM_TABM;
            else
                iPageNumTmp = PAGES_NUM_TXTM;

            if (!MnuImpListino.Checked && !BtnVisListino.Checked)
            {
                // tentativo di gestione qrcode sparato sulla griglia
                scannerInputQueue.Enqueue(iKey);

                // EAN13 contains 13 characters
                if (scannerInputQueue.Count >= 14)
                {
                    EditStatus_BC.Text = "";
                    DataManager.ClearGrid();

                    while (scannerInputQueue.Count > 0)
                    {
                        i = (int)scannerInputQueue.Dequeue();
                        EditStatus_BC.Text += Convert.ToChar(i);
                    }

                    // elimina quantità spurie
                    DataManager.ClearGrid();

                    KeyPressEventArgs ek = new KeyPressEventArgs('\r');
                    EditStatus_BC_KeyPress(EditStatus_BC, ek);
                }
            }

            ClearBC_FocusTimer();

            // punto unico _iCellPt =
            _iCellPt = MainGrid.CurrentCell.ColumnIndex * MainGrid.RowCount + MainGrid.CurrentCell.RowIndex + iArrayOffset;

            //Console.WriteLine("iKey = {0}, e.Modifiers = {1}, Ctrl = {2}", e.KeyValue, e.Modifiers, e.Control);

            if (e.Modifiers == Keys.Control)
                _bCrtlIsPressed = true;
            else
                _bCrtlIsPressed = false;

            switch (iKey)
            {
                case KEY_HOME:
                    MainGrid.CurrentCell = MainGrid.Rows[0].Cells[0];

                    if (e.Modifiers == Keys.Control)
                        TabSet.SelectedIndex = 0;

                    iKey = KEY_NONE;
                    MainGrid.Focus();
                    break;

                case KEY_END:
                    MainGrid.CurrentCell = MainGrid.Rows[MainGrid.RowCount - 1].Cells[MainGrid.ColumnCount - 1];

                    if (e.Modifiers == Keys.Control)
                        TabSet.SelectedIndex = iPageNumTmp - 1;

                    iKey = KEY_NONE;
                    MainGrid.Focus();
                    break;

                case KEY_ESC:

                    // reset selezioni Menù
                    if (MnuModDispArticoli.Checked)
                    {
                        MnuModDispArticoli.Checked = false;
                        MnuModDispArticoli_Click(this, null);
                    }

                    if (MnuImpListino.Checked)
                    {
                        EditCoperti.Text = _sCopertiPrev;
                        MnuImpListino.Checked = false;
                        MnuImpListino_Click(this, null);
                    }

                    if (BtnVisListino.Checked && BtnVisListino.Enabled)
                    {
                        EditCoperti.Text = _sCopertiPrev;
                        BtnVisListino.Checked = false;
                    }

                    // ci passa se si modifica l'header altrimenti
                    // ci pensa CheckMenuItems();
                    if (_bListinoModificato)
                        DataManager.SalvaListino();

                    // default TAB
                    TabSet.SelectedIndex = 0;
                    MainGrid.Focus();
                    CheckMenuItems();

                    break;

                case KEY_INS:
                case KEY_PLUS:
                case KEY_PLUS_NUM:
                    if (e.Modifiers == Keys.Control)
                    {
                        // inserisce oltre la Pagina corrente, entro l'ultima pagina
                        if (String.IsNullOrEmpty(SF_Data.Articolo[iLastGridIndex * iPageNumTmp - 1].sTipo) && (MnuImpListino.Checked == true))
                        {
                            for (i = iLastGridIndex * iPageNumTmp - 1; i > _iCellPt; i--)
                                SF_Data.Articolo[i] = SF_Data.Articolo[i - 1];

                            SF_Data.Articolo[_iCellPt] = new TArticolo(0);

                            _bListinoModificato = true;
                        }
                    }
                    else
                    {
                        // inserisce entro la Pagina corrente
                        if (String.IsNullOrEmpty(SF_Data.Articolo[iArrayOffset + iLastGridIndex - 1].sTipo) && (MnuImpListino.Checked == true))
                        {
                            for (i = iArrayOffset + iLastGridIndex - 1; i > _iCellPt; i--)
                                SF_Data.Articolo[i] = SF_Data.Articolo[i - 1];

                            SF_Data.Articolo[_iCellPt] = new TArticolo(0);

                            _bListinoModificato = true;
                        }
                    }

                    iKey = KEY_NONE;
                    break;

                case KEY_DEL:
                case KEY_MINUS:
                case KEY_MIN_NUM:

                    if (String.IsNullOrEmpty(SF_Data.Articolo[_iCellPt].sTipo) && (MnuImpListino.Checked == true))
                    {
                        if (e.Modifiers == Keys.Control)
                        {
                            // cancella spostando da oltre la Pagina corrente
                            for (i = _iCellPt; i < iLastGridIndex * iPageNumTmp - 1; i++)
                                SF_Data.Articolo[i] = SF_Data.Articolo[i + 1];

                            SF_Data.Articolo[iLastGridIndex * iPageNumTmp - 1] = new TArticolo(0);
                        }
                        else
                        {
                            // cancella entro la Pagina corrente
                            for (i = _iCellPt; i < iArrayOffset + iLastGridIndex - 1; i++)
                                SF_Data.Articolo[i] = SF_Data.Articolo[i + 1];

                            SF_Data.Articolo[iArrayOffset + iLastGridIndex - 1] = new TArticolo(0);
                        }

                        _bListinoModificato = true;
                        iKey = KEY_NONE;
                    }
                    else if ((!MnuImpListino.Checked) && (!MnuModDispArticoli.Checked) && (iKey != KEY_MIN_NUM))
                    {
                        // così poi aggiorna il TP Totale Parziale
                        iKey = '0';
                    }

                    break;

                case KEY_F1:
                    EditTavolo.Focus();
                    break;
                case KEY_F2:
                    EditCoperti.Focus();
                    break;
                case KEY_F3:
                    EditNota.Focus();
                    break;
                case KEY_F4:
                    EditContante.Focus();
                    break;

                case KEY_F5:
                    BtnVisListino_Click(this, null);
                    break;
                case KEY_F6:
                    BtnSendMsg_Click(this, null);
                    break;
                case KEY_F7:
                    BtnX10.Checked = !BtnX10.Checked;
                    break;
                case KEY_F8:
                    BtnEsportazione.Checked = !BtnEsportazione.Checked;
                    BtnEsportazione_Click(this, null);
                    break;
                case KEY_F9:
                    BtnSconto_Click(this, null);
                    break;
                case KEY_F10:
                    BtnScontrino_Click(this, null);
                    break;
                case KEY_ENTER:
                    // viene processato come Keys.Enter per evitare Key_Down
                    //if (OptionsDlg._rOptionsDlg.GetEnterPrintReceipt())
                    //{
                    //    BtnScontrino_Click(this, null);
                    //}
                    break;

                case '0':
                case KEYNUMPAD0:
                    if (e.Modifiers == Keys.Control)
                    {
                        sTmp = String.Format("FrmMain : premuto 0 + = {0}", e.Modifiers);
                        LogToFile(sTmp);

                        // Ctrl + 0
                        SetEditStatus_BC("");

                        DataManager.ClearGrid();
                        AnteprimaDlg.rAnteprimaDlg.RedrawReceipt();
                    }
                    break;

                // stampa copia locale scontrino normale
                case 'A':
                    if (e.Modifiers == Keys.Control)
                        SF_Data.Articolo[_iCellPt].iOptionsFlags = ClearBit(SF_Data.Articolo[_iCellPt].iOptionsFlags, BIT_STAMPA_SINGOLA_NELLA_COPIA_RECEIPT);
                    break;

                case 'E':
                    if (MnuEsperto.Checked && (e.Modifiers == (Keys.Alt | Keys.Control)))
                    {
                        sTmp = String.Format("FrmMain : premuto E + {0}", e.Modifiers);
                        LogToFile(sTmp);

                        // Ctrl + Alt + E
                        EraseAllaData();
                        return; //  altrimenti il successivo FormResize() genera eccezioni
                    }
                    break;

                case 'C':
                    if (e.Modifiers == Keys.Control)
                    {
                        comboCashPos.SelectedIndex = (comboCashPos.SelectedIndex + 1) % (sConst_PaymentType.Length - 1);
                        scannerInputQueue.Clear();
                    }
                    AnteprimaDlg.rAnteprimaDlg.RedrawReceipt();
                    break;

                case 'P':
                    if (e.Modifiers == Keys.Control)
                    {
                        MnuAnteprimaScontrino_Click(null, null);
                        scannerInputQueue.Clear();
                    }
                    break;

                // stampa copia locale scontrino singola
                case 'S':
                    if ((e.Modifiers == Keys.Control) && PrintReceiptConfigDlg.GetTicketNoPriceCopy())
                        SF_Data.Articolo[_iCellPt].iOptionsFlags = SetBit(SF_Data.Articolo[_iCellPt].iOptionsFlags, BIT_STAMPA_SINGOLA_NELLA_COPIA_RECEIPT);
                    break;

                default:
                    break;
            } // end switch

            // gestione Keypad numerico
            if ((iKey >= KEYNUMPAD0) && (iKey <= KEYNUMPAD9))
            {
                iKey -= 48;
            }

            /************************************************************************************
             *  non si è in modo modifica disponibilità, non si è in modo modifica del Listino,
             *  non si stanno consultando i prezzi
             ************************************************************************************/
            if ((iKey >= '0') && (iKey <= '9') && !String.IsNullOrEmpty(SF_Data.Articolo[_iCellPt].sTipo) &&
                !MnuImpListino.Checked &&       // non si è in modo modifica del Listino
                !BtnVisListino.Checked &&       // non si stanno consultando i prezzi
                !MnuModDispArticoli.Checked)    // non si sta modificando la disponibilità
            {
                if (SF_Data.Articolo[_iCellPt].iDisponibilita != 0) //
                {
                    if (BtnX10.Checked) // Q.tà ordinazioni a 2 cifre
                    {
                        iD = (SF_Data.Articolo[_iCellPt].iQuantitaOrdine) % 10;
                        iU = (iKey - '0');
                        SF_Data.Articolo[_iCellPt].iQuantitaOrdine = iD * 10 + iU;

                        BtnX10.Checked = false;
                        sStatusText = "Pronto";
                    }
                    else
                    {
                        iU = (iKey - '0');
                        SF_Data.Articolo[_iCellPt].iQuantitaOrdine = iU;
                    }
                }
                else // azzeramento consentito per ordini web non possibili
                {
                    iU = (iKey - '0');

                    if (iU == 0)
                        SF_Data.Articolo[_iCellPt].iQuantitaOrdine = 0;
                }

                AnteprimaDlg.rAnteprimaDlg.RedrawReceipt();
                _iAnteprimaTotParziale = AnteprimaDlg.GetTotaleReceipt();
            }
            // per sicurezza si azzerano in caso numeri di fuori griglia
            else if ((iKey == '0') && String.IsNullOrEmpty(SF_Data.Articolo[_iCellPt].sTipo) &&
                !MnuImpListino.Checked &&      // non si è in modo modifica del Listino
                !BtnVisListino.Checked &&      // non si stanno consultando i prezzi
                !MnuModDispArticoli.Checked)   // non si sta modificando la disponibilità
            {
                SF_Data.Articolo[_iCellPt].iQuantitaOrdine = 0;

                AnteprimaDlg.rAnteprimaDlg.RedrawReceipt();
                _iAnteprimaTotParziale = AnteprimaDlg.GetTotaleReceipt();
            }

            if (MnuImpListino.Checked)
                FormResize(this, null);

            if (SF_Data.Articolo[_iCellPt].iQuantitaOrdine == 0)
            {
                // reset EditNota
                EditNota.BackColor = Color.Gainsboro;
                SF_Data.Articolo[_iCellPt].sNotaArt = "";

                MainGrid_Redraw(this, null);
            }

            MainGrid_Redraw(this, null);
        }

        private void MainGrid_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control)
                _bCrtlIsPressed = true;
            else
                _bCrtlIsPressed = false;

            if ((EditNota.BackColor == Color.LightBlue) && !_bCrtlIsPressed)
            {
                // reset EditNota
                EditNota.BackColor = Color.Gainsboro;
                EditNota.Text = _sEditNotaCopy;
                EditNota.MaxLength = 28;
                lblNota.Text = "Nota:";

                MainGrid_Redraw(this, null);
            }

            Console.WriteLine("MainGrid_KeyUp: {0}", _bCrtlIsPressed);
        }

        void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            int iKey = (int)e.KeyValue;

            iFocus_BC_Timeout = BC_FOCUS_TIMEOUT;

            // ridà il controllo alla griglia
            if ((sender == EditStatus_BC && String.IsNullOrEmpty(EditStatus_BC.Text)) &&
                ((e.KeyValue == KEY_DOWN) || (e.KeyValue == KEY_UP) ||
                (e.KeyValue == KEY_LEFT) || (e.KeyValue == KEY_RIGHT)))
            {
                MainGrid.Focus();
            }

            switch (iKey)
            {
                case KEY_F1:
                    if (EditTavolo.Focused)
                        MainGrid.Focus();
                    else if (EditCoperti.Focused)
                        EditTavolo.Focus();
                    else if (EditNota.Focused)
                        EditTavolo.Focus();
                    else if (EditContante.Focused)
                        EditTavolo.Focus();
                    break;
                case KEY_F2:
                    if (EditCoperti.Focused)
                        MainGrid.Focus();
                    else if (EditTavolo.Focused)
                        EditCoperti.Focus();
                    else if (EditNota.Focused)
                        EditCoperti.Focus();
                    else if (EditContante.Focused)
                        EditCoperti.Focus();
                    break;
                case KEY_F3:
                    if (EditNota.Focused)
                    {
                        MainGrid.Focus();

                        if ((EditNota.BackColor == Color.LightBlue) && !_bCrtlIsPressed)
                        {
                            // reset EditNota
                            EditNota.BackColor = Color.Gainsboro;
                            EditNota.Text = _sEditNotaCopy;
                            EditNota.MaxLength = 28;
                            lblNota.Text = "Nota:";

                            MainGrid_Redraw(this, null);
                        }
                    }
                    else if (EditTavolo.Focused)
                        EditNota.Focus();
                    else if (EditCoperti.Focused)
                        EditNota.Focus();
                    else if (EditContante.Focused)
                        EditNota.Focus();
                    break;
                case KEY_F4:
                    if (EditContante.Focused)
                        MainGrid.Focus();
                    else if (EditCoperti.Focused)
                        EditContante.Focus();
                    else if (EditNota.Focused)
                        EditContante.Focus();
                    else if (EditTavolo.Focused)
                        EditContante.Focus();
                    break;
                case KEY_F5:
                case KEY_F6:
                case KEY_F7:
                case KEY_F8:
                case KEY_F9:
                case KEY_F10:
                    MainGrid_KeyDown(sender, e);
                    break;
            }
        }

        // accetta solo numeri o backspace
        private void EditFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || (e.KeyChar == '\b')))
                e.Handled = true;
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            // sicurezza per scannerInputQueue.Enqueue(iKey)
            // altrimenti il prezzo dei coperti genera eccezioni
            if (!MnuImpListino.Checked && !BtnVisListino.Checked)
            {
                _sEditTavolo = EditTavolo.Text.Trim();
                SF_Data.sTavolo = _sEditTavolo;

                _sEditNome = EditNome.Text.Trim();
                SF_Data.sNome = _sEditNome;

                if (GetStatusNota())
                {
                    _sEditNota = EditNota.Text.Trim();
                    SF_Data.sNota = _sEditNota;
                }

                _sEditCoperti = EditCoperti.Text.Trim();

                // editabili solo caratteri numerici
                if (String.IsNullOrEmpty(_sEditCoperti))
                    SF_Data.Articolo[MAX_NUM_ARTICOLI - 1].iQuantitaOrdine = 0;
                else
                {
                    if (!_sEditCoperti.Contains(","))
                        SF_Data.Articolo[MAX_NUM_ARTICOLI - 1].iQuantitaOrdine = Convert.ToInt32(_sEditCoperti);
                    else
                        Console.WriteLine(_sEditCoperti); // debug
                }

                AnteprimaDlg.rAnteprimaDlg.RedrawReceipt();
            }
        }

        private void EditContante_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || (e.KeyChar == '.') || (e.KeyChar == ',') || (e.KeyChar == '\b')))
                e.Handled = true;
        }

        /************************************************
         * cancella dati con : Shift + Alt + Ctrl + E
         ************************************************/
        private void EraseAllaData()
        {
            int i;
            String sNomeFileTmp, sNomeDirTmp;
            DialogResult dResult;

            if (bUSA_NDB() && (SF_Data.iNumCassa != CASSA_PRINCIPALE)) // sicurezza
                return;

            dResult = MessageBox.Show("Sei sicuro di voler azzerare i dati di oggi ?", "Attenzione !", MessageBoxButtons.YesNo);

            if (dResult == DialogResult.Yes)
            {
                dResult = MessageBox.Show("Ne sei proprio sicuro ?\n\nTutti i dati di oggi verranno azzerati ed il programma verrà terminato !",
                                                                  "Attenzione !", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dResult == DialogResult.Yes)
                {
                    // cancellazione dati riepilogo
                    Directory.SetCurrentDirectory(DataManager.GetAnnoDir());

                    try
                    {
                        for (i = CASSA_PRINCIPALE; i <= (MAX_CASSE_SECONDARIE + 1); i++)
                        {
                            // cancellazione dati riepilogo
                            sNomeFileTmp = DATA_DIR + "\\" + GetNomeFileDati(i, GetActualDate());
                            File.Delete(sNomeFileTmp);

                            // cancellazione backup dati riepilogo
                            sNomeFileTmp = DATA_DIR + "\\" + GetNomeFileDatiBak(i);
                            File.Delete(sNomeFileTmp);
                        }

                        // cancellazione tickets
                        sNomeDirTmp = DataManager.GetTicketsDir();
                        if (Directory.Exists(sNomeDirTmp))
                            Directory.Delete(sNomeDirTmp, true);

                        // cancellazione messaggi
                        sNomeDirTmp = DataManager.GetMessagesDir();
                        if (Directory.Exists(sNomeDirTmp))
                            Directory.Delete(sNomeDirTmp, true);

                        // cancellazione copie
                        sNomeDirTmp = DataManager.GetCopiesDir();
                        if (Directory.Exists(sNomeDirTmp))
                            Directory.Delete(sNomeDirTmp, true);

                    }
                    catch (Exception)
                    {
                        LogToFile("EraseAllaData: problemi di cancellazione Dir");
                    }

                    _rdBaseIntf.dbDropTables();

                    // altrimenti in uscita viene chiamata SalvaDati() a causa della disponibilità != DISP_OK
                    for (i = 0; i < MAX_NUM_ARTICOLI + EXTRA_NUM_ARTICOLI; i++) // azzeramento
                        SF_Data.Articolo[i].iDisponibilita = DISP_OK;

                    // altrimenti in uscita viene chiamata SalvaDati()
                    SF_Data.iNumOfLastReceipt = 0;

                    ErrorManager(ERR_AZD);
                }
            }
        }

        /*********************************************
            ritorna true se non ci sono errori
         *********************************************/
        bool VerificaQuantita()
        {
            if (SF_Data.Articolo[_iCellPt].iQuantitaOrdine > SF_Data.Articolo[_iCellPt].iDisponibilita)
            {
                WrnMsg.sMsg = SF_Data.Articolo[_iCellPt].sTipo;
                WrnMsg.iErrID = WRN_QMD;

                WarningManager(WrnMsg);
                return false;
            }
            else
                return true;
        }

        /// <summary>
        /// ritorna true se c'è disponibilità sufficente di Articoli e Componenti
        /// </summary>
        bool VerificaTutteQuantita()
        {
            int i;

            // totale scontrino corrente
            for (i = 0; i < MAX_NUM_ARTICOLI - 1; i++)
                if (SF_Data.Articolo[i].iQuantitaOrdine > SF_Data.Articolo[i].iDisponibilita)
                {
                    WrnMsg.sMsg = SF_Data.Articolo[i].sTipo;
                    WrnMsg.iErrID = WRN_QMD;

                    WarningManager(WrnMsg);
                    return false;
                }

            return true;
        }

        /*********************************************************************************
         * metodo chiamato alla risposta del Server NSC con il nuovo numero di scontrino
         * le verifiche con : DataManager.getTotaleCurrReceipt() && VerificaQuantita()
         * vanno fatte ancora prima di interrogare il server NSC
         *********************************************************************************/
        private void EmissioneScontrino()
        {
            LogToFile("Mainform : EmissioneScontrino 1");

            // ripetuto per sicurezza
            if (BtnEsportazione.Checked)
                SF_Data.iStatusReceipt = SetBit(SF_Data.iStatusReceipt, BIT_ESPORTAZIONE);
            else
                SF_Data.iStatusReceipt = ClearBit(SF_Data.iStatusReceipt, BIT_ESPORTAZIONE);

            if (SF_Data.bPrevendita)
                SF_Data.iStatusReceipt = SetBit(SF_Data.iStatusReceipt, BIT_EMESSO_IN_PREVENDITA);
            else
                SF_Data.iStatusReceipt = ClearBit(SF_Data.iStatusReceipt, BIT_EMESSO_IN_PREVENDITA);

            if (CheckService(Define._REC_TEST))
                TestManager.TestRecord_order();  // ogni 250ms

            DataManager.Receipt();  // salva anche i Dati

            // non può essere dentro allo Scontrino altrimenti blocca scrittura files, etc.
            if (SF_Data.iTotaleReceipt == 0)
                WarningManager(WRN_TZT);

            if (_bListinoModificato)
                DataManager.SalvaListino();

            _iAnteprimaTotParziale = 0;

            if (iSysPrinterType == (int)PRINTER_SEL.STAMPANTE_WINDOWS)
                ResetBtnScontrino();
            // else
            //  viene eseguito dopo la stampa Legacy con l'evento RESET_RECEIPT_BTN_EVENT
        }

        /// <summary>aggiorna la barra di stato</summary>
        public void UpdateStatusBar(int iNumOfTicketsParm, int iTotaleTicketParm)
        {
            if (_bShowTotaleScontrinoPrec)
                lblStatusTotalePrec.Text = String.Format("Totale Pr. = {0}", IntToEuro(iTotaleTicketParm));
            else
                lblStatusTotalePrec.Text = "";

            lblStatusTickNum.Text = String.Format("Num. Ordini: {0}", iNumOfTicketsParm);

            // aggiornamento del resto
            EditResto.Text = "";
            EditContante.Text = "";
        }

        /// <summary>overload: aggiorna la barra di stato</summary>
        public void UpdateStatusBar(String sStatusParam)
        {
            sStatusText = sStatusParam;
        }

        /// <summary>
        /// rimette a posto l'astetto dei bottoni dopo
        /// l'emissione di uno scontrino con Timer o forzato
        /// </summary>
        private void ResetBtnScontrino()
        {
            _bPrintTimeoutEnabled = false;
            // stop forzato

            BtnEsportazione.Checked = false;

            BtnSconto.Checked = false;
            ScontoDlg.ResetSconto();
            BtnSconto.Image = Properties.Resources.sconto_no;

            BtnScontrino.Enabled = true;
            BtnScontrino.Checked = false;

            EditTavolo.Text = "";
            _sEditTavolo = "";

            EditNome.Text = "";
            _sEditNome = "";

            EditCoperti.Text = "";
            _sEditCoperti = "";

            EditNota.Text = "";
            _sEditNota = "";

            lblStatus_TC.Text = "";

            EditStatus_BC.Text = "";

            UpdateStatusBar(DataManager.GetNumOfLocalOrders(), SF_Data.iTotaleReceiptDovuto);

            // comboCashPos.Text = ""; // inutile cambio SelectedIndex lo reimposta
            comboCashPos.SelectedIndex = sConst_PaymentType.Length - 1;

            if (OptionsDlg._rOptionsDlg.GetPresales_LoadMode())
                EditStatus_BC.Focus();
            else
                MainGrid.Focus(); // evita inserimenti indesiderati del tavolo

            LogToFile("ResetBtnScontrino");
        }

        /// <summary>
        /// abilita/disabilita le varie voci del Menù Principale
        /// </summary>
        void CheckMenuItems()
        {
            // bottoni e/o menù rilevanti
            if (MnuModDispArticoli.Checked)
            {
                // Menù File e Stampa
                MnuStampaDiProva.Enabled = false;
                MnuStampaFile.Enabled = false;
                MnuEsportaListino.Enabled = false;
                MnuImportaListino.Enabled = false;
                MnuChiudiIncasso.Enabled = false;

                // Menù Modifica
                MnuModifica.Enabled = true;
                MnuModDispArticoli.Enabled = true;
                MnuAnnulloOrdine.Enabled = false;
                MnuChangePayment.Enabled = false;

                // Menù Visualizza
                MnuVisualizza.Enabled = false;

                // Menù Impostazioni
                MnuImpostazioni.Enabled = false;

                // Bottoni
                BtnVisListino.Enabled = false;
                BtnSendMsg.Enabled = false;
                BtnX10.Enabled = false;
                BtnEsportazione.Enabled = false;
                BtnSconto.Enabled = false;
                BtnScontrino.Enabled = false;
                BtnDB.Enabled = false;

                // Bottoni Touch
                btnPlus.Enabled = false;
                btnMinus.Enabled = false;
                btnCanc.Enabled = false;

                lblNome.Enabled = false;
                EditNome.Enabled = false;
                lblTavolo.Enabled = false;
                EditTavolo.Enabled = false;
                lblCoperti.Enabled = false;
                EditCoperti.Enabled = false;
                lblNota.Enabled = false;
                EditNota.Enabled = false;
                lblResto.Enabled = false;
                EditResto.Enabled = false;
                lblPagato.Enabled = false;
                EditContante.Enabled = false;

                comboCashPos.Enabled = false;
                EditStatus_BC.Enabled = false;
                lblRead_bcd.Enabled = false;
            }
            else if (MnuImpListino.Checked)
            {
                // Menù File e Stampa
                MnuStampaDiProva.Enabled = false;
                MnuStampaFile.Enabled = false;
                MnuEsportaListino.Enabled = false;
                MnuImportaListino.Enabled = false;
                MnuChiudiIncasso.Enabled = false;

                // Menù Modifica
                MnuModifica.Enabled = false;
                MnuModDispArticoli.Enabled = false;

                // Menù Visualizza
                MnuVisualizza.Enabled = false;

                // Menù Impostazioni
                MnuImpostazioni.Enabled = true;
                MnuEsperto.Enabled = false;
                MnuImpostaRete.Enabled = false;
                MnuImpostaStampanteWin.Enabled = false;
                MnuImpostaStampanteLegacy.Enabled = false;
                MnuImpostaCopieLocali.Enabled = false;
                MnuImpostaCopieInRete.Enabled = false;
                MnuImpOpzioni.Enabled = false;
                MnuCambiaPassword.Enabled = false;
                MnuImpHeader.Enabled = false;
                MnuImpTabsGrid.Enabled = false;
                MnuImpListino.Enabled = true;

                // Bottoni
                BtnVisListino.Enabled = false;
                //VisListino.Checked = false;

                BtnSendMsg.Enabled = false;
                BtnX10.Enabled = false;
                BtnEsportazione.Enabled = false;
                BtnSconto.Enabled = false;
                BtnScontrino.Enabled = false;
                BtnDB.Enabled = false;

                sStatusText = "Modifica Listino : Esc per uscire";

                // Bottoni Touch
                btnPlus.Enabled = false;
                btnMinus.Enabled = false;
                btnCanc.Enabled = false;

                lblNome.Enabled = false;
                EditNome.Enabled = false;
                lblTavolo.Enabled = false;
                EditTavolo.Enabled = false;
                lblCoperti.Enabled = false;
                EditCoperti.Enabled = false;
                lblNota.Enabled = false;
                EditNota.Enabled = false;
                lblResto.Enabled = false;
                EditResto.Enabled = false;
                lblPagato.Enabled = false;
                EditContante.Enabled = false;

                comboCashPos.Enabled = false;
                EditStatus_BC.Enabled = false;
                lblRead_bcd.Enabled = false;
            }
            // con BtnVisListino->Enabled = false -->  BtnVisListino->Down == true sempre
            else if (BtnVisListino.Checked && BtnVisListino.Enabled)
            {
                // Menù File e Stampa
                MnuStampaDiProva.Enabled = false;
                MnuStampaFile.Enabled = false;
                MnuEsportaListino.Enabled = false;
                MnuImportaListino.Enabled = false;
                MnuChiudiIncasso.Enabled = false;

                // Menù Modifica
                MnuModifica.Enabled = false;

                // Menù Visualizza
                MnuVisualizza.Enabled = false;

                // Menù Impostazioni
                MnuImpostazioni.Enabled = false;

                // Bottoni
                // BtnVisListino.Enabled = false;
                BtnSendMsg.Enabled = false;
                BtnX10.Enabled = false;
                BtnEsportazione.Enabled = false;
                BtnSconto.Enabled = false;
                BtnScontrino.Enabled = false;
                BtnDB.Enabled = false;

                // Bottoni Touch
                btnPlus.Enabled = false;
                btnMinus.Enabled = false;
                btnCanc.Enabled = false;

                lblNome.Enabled = false;
                EditNome.Enabled = false;
                lblTavolo.Enabled = false;
                EditTavolo.Enabled = false;
                lblCoperti.Enabled = false;
                EditCoperti.Enabled = false;
                lblNota.Enabled = false;
                EditNota.Enabled = false;
                lblResto.Enabled = false;
                EditResto.Enabled = false;
                lblPagato.Enabled = false;
                EditContante.Enabled = false;

                comboCashPos.Enabled = false;
                EditStatus_BC.Enabled = false;
                lblRead_bcd.Enabled = false;
            }
            else
            {
                /*******************************************
                        nessun bottone o menù rilevante            
                 *******************************************/

                // più sopra poteva essere disabilitato
                MnuEsperto.Enabled = true;

                // Bottoni Touch
                btnPlus.Enabled = true;
                btnMinus.Enabled = true;
                btnCanc.Enabled = true;

                MainGrid.Enabled = true; // sicurezza

                lblNome.Enabled = true;
                EditNome.Enabled = true;
                lblTavolo.Enabled = true;
                EditTavolo.Enabled = true;
                lblCoperti.Enabled = true;
                EditCoperti.Enabled = true;
                lblNota.Enabled = true;
                EditNota.Enabled = true;
                lblResto.Enabled = true;
                EditResto.Enabled = true;
                lblPagato.Enabled = true;
                EditContante.Enabled = true;

                comboCashPos.Enabled = true;

                // per sicurezza accettazione BarCode non con la prevendita in corso
                if (SF_Data.bPrevendita)
                {
                    EditStatus_BC.Enabled = false;
                    lblRead_bcd.Enabled = false;
                }
                else
                {
                    EditStatus_BC.Enabled = true;
                    lblRead_bcd.Enabled = true;
                }

                if (MnuEsperto.Checked)
                {
                    // Menù File e Stampa

                    if (DataManager.CheckIf_CassaSec_and_NDB()) // cassa secondaria e DB
                    {
                        MnuEsportaListino.Enabled = false;
                        MnuImportaListino.Enabled = false;
                        MnuChiudiIncasso.Enabled = false;
                        MnuImpHeader.Enabled = false;
                        MnuImpTabsGrid.Enabled = false;

                        if (SF_Data.bPrevendita)
                            MnuImpOpzioni.Enabled = false;
                        else
                            // per poter eliminare il Flag
                            MnuImpOpzioni.Enabled = true;

                        MnuImpListino.Enabled = false;
                    }
                    else
                    {
                        MnuEsportaListino.Enabled = true;
                        MnuImportaListino.Enabled = true;
                        // altrimenti con la Prevendita è troppo complessa la gestione dei nomi delle tabelle
                        MnuChiudiIncasso.Enabled = !SF_Data.bPrevendita;
                        MnuImpHeader.Enabled = true;
                        MnuImpTabsGrid.Enabled = true;
                        MnuImpOpzioni.Enabled = true;
                        MnuImpListino.Enabled = true;
                    }

                    // Menù Modifica
                    // Menù Visualizza
                    MnuVisIncassoOggi.Enabled = true;
                    MnuVisIncassoAltraData.Enabled = true;
                    MnuEsploraDB.Enabled = true;

                    // Menù Impostazioni
                    MnuCambiaPassword.Enabled = true;
                    MnuImpostaRete.Enabled = true;
                    MnuImpostaStampanteWin.Enabled = true;
                    MnuImpostaStampanteLegacy.Enabled = true;
                    MnuImpostaCopieLocali.Enabled = true;
                    MnuImpostaCopieInRete.Enabled = true;

                }
                else // non Esperto
                {
                    // Menù File e Stampa
                    MnuEsportaListino.Enabled = false;
                    MnuImportaListino.Enabled = false;
                    MnuChiudiIncasso.Enabled = false;

                    // Menù Modifica
                    // Menù Visualizza
                    if (SF_Data.bRiservatezzaRichiesta)
                    {
                        MnuVisIncassoOggi.Enabled = false;
                        MnuVisIncassoAltraData.Enabled = false;
                        MnuEsploraDB.Enabled = false;
                        MnuVisOrdiniAltraData.Enabled = false;
                    }
                    else
                    {
                        MnuVisIncassoOggi.Enabled = true;
                        MnuVisIncassoAltraData.Enabled = true;
                        MnuEsploraDB.Enabled = true;
                        MnuVisOrdiniAltraData.Enabled = true;
                    }

                    // Menù Impostazioni
                    MnuImpostaRete.Enabled = false;
                    MnuImpostaStampanteWin.Enabled = false;
                    MnuImpostaStampanteLegacy.Enabled = false;
                    MnuImpostaCopieLocali.Enabled = false;
                    MnuImpostaCopieInRete.Enabled = false;
                    MnuCambiaPassword.Enabled = false;
                    MnuImpOpzioni.Enabled = false;
                    MnuImpHeader.Enabled = false;
                    MnuImpTabsGrid.Enabled = false;
                    MnuImpListino.Enabled = false;
                }

                // cassa secondaria e DB
                if (DataManager.CheckIf_CassaSec_and_NDB())
                    MnuModDispArticoli.Enabled = false;
                else
                    MnuModDispArticoli.Enabled = true;


                // tutti i Menù abilitati
                MnuModifica.Enabled = true;
                MnuVisualizza.Enabled = true;
                MnuImpostazioni.Enabled = true;
                MnuQuickHelp.Enabled = true;

                // Menù File e Stampa
                MnuStampaDiProva.Enabled = true;
                MnuStampaFile.Enabled = true;

                // Menù Modifica
                MnuAnnulloOrdine.Enabled = true;
                MnuChangePayment.Enabled = true;

                // Menù Visualizza
                MnuVisOrdiniOggi.Enabled = true;

                MnuEsploraOrdiniWeb.Enabled = NetConfigDlg.rNetConfigDlg.GetWebOrderEnabled();
                MnuVisListino.Enabled = true;
                MnuReceiptPreview.Enabled = true;

                // più sopra poteva essere disabilitato
                MnuEsperto.Enabled = true;

                // Bottoni
                BtnVisListino.Enabled = true;
                //    altrimenti non si visualizza il listino
                //    BtnVisListino.Checked = false;

                BtnSendMsg.Enabled = true;
                BtnSendMsg.Checked = false;

                BtnX10.Enabled = true;
                BtnX10.Checked = false;

                BtnEsportazione.Enabled = true;

                //BtnEsportazione.Checked = false;
                if (SF_Data.bPrevendita)
                    BtnSconto.Enabled = false;
                else
                    BtnSconto.Enabled = true;

                // ripristina lo stato dopo che si sono consultati i prezzi
                if ((SF_Data.iStatusSconto & 0x000000FF) != 0)
                {
                    BtnSconto.Checked = true;
                    BtnSconto.Image = Properties.Resources.sconto_si;
                }
                else
                {
                    BtnSconto.Checked = false;
                    BtnSconto.Image = Properties.Resources.sconto_no;
                }

                BtnScontrino.Enabled = true;
                BtnScontrino.Checked = false;

                BtnDB.Enabled = true;

                if (_bListinoModificato)
                {
                    DataManager.SalvaListino();

                    DataManager.SalvaDati(); // così si visualizzano prezzi e dati aggiornati
                    SetTabsAppearance();
                }

                sStatusText = "Pronto";
            }

            FormResize(null, null);
            MainGrid_Redraw(this, null);
        }

        // toggle del Focus tra griglia ed EditStatusTavolo
        // verifica inserimento del Tavolo se non è esportazione e c'è almeno una Pietanza
        bool VerificaTavoloRichiesto()
        {
            if (SF_Data.bTavoloRichiesto && !BtnEsportazione.Checked && !CheckService(Define._AUTO_SEQ_TEST))
                if (String.IsNullOrEmpty(_sEditTavolo))
                {
                    MessageBox.Show("Inserisci il numero del Tavolo,\n\ncon (F1) passi dalla griglia alla casella del Tavolo.",
                        "Attenzione !", MessageBoxButtons.OK);

                    EditTavolo.Focus();
                    return false;
                }
                else
                    return true;
            else
                return true; // si omette la verifica
        }

        // verifica inserimento dei coperti se non è esportazione e c'è almeno una Pietanza
        bool VerificaCopertoRichiesto()
        {
            if (SF_Data.bCopertoRichiesto && !BtnEsportazione.Checked && !CheckService(Define._AUTO_SEQ_TEST))
                if (String.IsNullOrEmpty(_sEditCoperti) || (Convert.ToInt32(_sEditCoperti) <= 0))
                {
                    MessageBox.Show("Inserisci il numero dei Coperti,\n\ncon (F2) passi dalla griglia alla casella dei Coperti.",
                        "Attenzione !", MessageBoxButtons.OK);

                    EditCoperti.Focus();
                    return false;
                }
                else
                    return true;
            else
                return true; // si omette la verifica
        }

        // verifica inserimento del pagamento in Contanti/Card/Satispay
        bool VerificaPOS_Richiesto()
        {
            if (SF_Data.bModoPagamRichiesto && !CheckService(Define._AUTO_SEQ_TEST))
                if (String.IsNullOrEmpty(comboCashPos.Text.Trim()))
                {
                    MessageBox.Show("Inserisci il tipo di pagamento Contanti/Card/Satispay",
                        "Attenzione !", MessageBoxButtons.OK);

                    return false;
                }
                else
                    return true;
            else
                return true; // si omette la verifica
        }

        /**********************************************
         ***	Gestione del menù: File e stampa 	***
         **********************************************/
        private void MnuPrintTest_Click(object sender, EventArgs e)
        {
            String sTmp, sFileToPrint;

            sFileToPrint = BuildSampleText();

            sTmp = String.Format("Mainform : PrintSampleText() {0}", sFileToPrint);
            LogToFile(sTmp);

            GenPrintFile(sFileToPrint);
        }

        private void MnuStampaFile_Click(object sender, EventArgs e)
        {
            string sFileToPrint;

            openFileDialog.Filter = "Files di testo (*.txt)|*.TXT";
            openFileDialog.FileName = "";
            openFileDialog.InitialDirectory = DataManager.GetDataDir();

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                sFileToPrint = openFileDialog.FileName;

                GenPrintFile(sFileToPrint);
            }
        }

        private void MnuEsportaListino_Click(object sender, EventArgs e)
        {
            bool bResult = true;
            String sSourceFile, sDestinationFile;
            String sTmp;
            DialogResult dResult;

            sSourceFile = DataManager.GetExeDir() + "\\" + NOME_FILE_LISTINO;

            DlgSaveFile.Filter = "Files di testo (*.txt)|*.TXT";
            DlgSaveFile.InitialDirectory = DataManager.GetRootDir();
            DlgSaveFile.DefaultExt = "txt";
            DlgSaveFile.CheckFileExists = false;
            DlgSaveFile.FileName = NOME_FILE_LISTINO;

            if (DlgSaveFile.ShowDialog() == DialogResult.OK)
            {
                sDestinationFile = DlgSaveFile.FileName;

                if (File.Exists(sDestinationFile))
                {
                    sTmp = "Sei sicuro di voler sovrascrivere il file : \n\n" + Path.GetFileName(sDestinationFile) + " ?";
                    dResult = MessageBox.Show(sTmp, "Attenzione !", MessageBoxButtons.YesNo);

                    if (dResult == DialogResult.Yes)
                        try
                        {
                            File.Copy(sSourceFile, sDestinationFile, true);
                        }
                        catch (IOException)
                        {
                            MessageBox.Show("Esportazione non riuscita,\n\nscegliere un nome del file diverso !", "Attenzione !", MessageBoxButtons.OK);
                            bResult = false;
                        }
                    else
                        bResult = false;
                }
                else
                {
                    try
                    {
                        File.Copy(sSourceFile, sDestinationFile, false);
                    }
                    catch (IOException)
                    {
                        MessageBox.Show("Esportazione non riuscita !", "Attenzione !", MessageBoxButtons.OK);
                        bResult = false;
                    }
                }

                if (bResult)
                    MessageBox.Show("Esportazione riuscita !", "Attenzione !", MessageBoxButtons.OK);
                else
                    MessageBox.Show("Esportazione non riuscita !", "Attenzione !", MessageBoxButtons.OK);
            }
        }

        private void MnuImportaListino_Click(object sender, EventArgs e)
        {
            String sSourceFile, sDestinationFile, sBacukpFile;
            DialogResult dResult;

            if (MnuEsperto.Checked)
            {
                dResult = MessageBox.Show("Sei sicuro di voler cambiare i prezzi attuali ?", "Attenzione !", MessageBoxButtons.YesNo);

                if (dResult == DialogResult.Yes)
                {
                    dResult = MessageBox.Show("Ne sei proprio sicuro ?\n\nIl file Prezzi verrà sovrascritto ed il Programma verrà riavviato !",
                                                      "Attenzione !", MessageBoxButtons.YesNo);
                    if (dResult == DialogResult.Yes)
                    {
                        sDestinationFile = DataManager.GetExeDir() + "\\" + NOME_FILE_LISTINO;
                        sBacukpFile = DataManager.GetExeDir() + "\\" + NOME_FILE_LISTINO_BK;

                        openFileDialog.Filter = "Files di testo (*.txt)|*.TXT";
                        openFileDialog.InitialDirectory = DataManager.GetRootDir();
                        openFileDialog.DefaultExt = "txt";

                        if (openFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            sSourceFile = openFileDialog.FileName;

                            Directory.SetCurrentDirectory(DataManager.GetExeDir());

                            if (sSourceFile == sDestinationFile)
                                MessageBox.Show("Scegliere un file diverso !", "Attenzione !", MessageBoxButtons.OK);
                            else
                            {
                                try
                                {
                                    // crea copia di backup
                                    File.Delete(sBacukpFile);
                                    File.Move(sDestinationFile, sBacukpFile);
                                    File.Copy(sSourceFile, sDestinationFile, false);

                                    ErrorManager(ERR_AZP);
                                }
                                catch (IOException)
                                {
                                    MessageBox.Show("Importazione non riuscita !", "Attenzione !", MessageBoxButtons.OK);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// forza chiusura incasso
        /// </summary>
        private void MnuChiudiIncasso_Click(object sender, EventArgs e)
        {
            bool bResult;
            int iNumCassa;
            String sNomeDirTmp;
            String sPrefix, sSourceFile, sDestinationFile;
            String sDestinationPostFix, sTmp;
            DialogResult dResult;

            bResult = true;

            dResult = MessageBox.Show("Sei sicuro di voler chiudere l'attuale incasso per proseguire con un nuovo conteggio ?" +
                "\n\nVerrà chiesto un suffisso per l'archiviazione poi il programma verrà terminato !", "Attenzione !", MessageBoxButtons.YesNo);

            if (dResult == DialogResult.Yes)
            {
                // avvia la form principale
                RenameDlg rRenameDlg = new RenameDlg();

                rRenameDlg.Init("Inserisci un suffisso per tabella e file dati :", "");
                dResult = rRenameDlg.ShowDialog();

                // apre form per postfisso
                if (dResult == DialogResult.OK)
                {
                    sPrefix = GetNomeFileDati(CASSA_PRINCIPALE, GetActualDate()).Remove(12); // elimina il .txt

                    sDestinationPostFix = "_" + rRenameDlg.GetEdit();
                    sDestinationFile = sPrefix + sDestinationPostFix + ".txt";

                    sTmp = String.Format("FrmMain chiudiIncasso: sDestinationFile = {0}", sDestinationFile);
                    LogToFile(sTmp);

                    try
                    {
                        if (File.Exists(sDestinationFile))
                        {
                            sTmp = String.Format("File esistente o nome non corretto, specificare un altro suffisso !\n");

                            dResult = MessageBox.Show(sTmp, "Attenzione !", MessageBoxButtons.OK);
                            bResult = false;
                        }
                        else
                        {
                            // rinomina il file dati
                            Directory.SetCurrentDirectory(DataManager.GetDataDir());

                            for (iNumCassa = CASSA_PRINCIPALE; iNumCassa <= (MAX_CASSE_SECONDARIE + 1); iNumCassa++)
                            {
                                sSourceFile = GetNomeFileDati(iNumCassa, GetActualDate());

                                sPrefix = GetNomeFileDati(iNumCassa, GetActualDate()).Remove(12); // elimina il .txt
                                sDestinationPostFix = "_" + rRenameDlg.GetEdit();
                                sDestinationFile = sPrefix + sDestinationPostFix + ".txt";

                                if (File.Exists(sSourceFile))
                                    File.Move(sSourceFile, sDestinationFile);

                                sSourceFile = GetNomeFileDatiBak(iNumCassa);

                                sPrefix = GetNomeFileDati(iNumCassa, GetActualDate()).Remove(12); // elimina il .txt
                                sDestinationPostFix = "_" + rRenameDlg.GetEdit();
                                sDestinationFile = sPrefix + sDestinationPostFix + ".bak";

                                if (File.Exists(sSourceFile))
                                    File.Move(sSourceFile, sDestinationFile);
                            }

                            // elimina le Directories
                            Directory.SetCurrentDirectory(DataManager.GetAnnoDir());

                            // cancellazione tickets per evitare ambiguità
                            sNomeDirTmp = DataManager.GetTicketsDir();
                            if (Directory.Exists(sNomeDirTmp))
                                Directory.Delete(sNomeDirTmp, true);

                            // cancellazione messaggi
                            sNomeDirTmp = DataManager.GetMessagesDir();
                            if (Directory.Exists(sNomeDirTmp))
                                Directory.Delete(sNomeDirTmp, true);

                            // cancellazione copie
                            sNomeDirTmp = DataManager.GetCopiesDir();
                            if (Directory.Exists(sNomeDirTmp))
                                Directory.Delete(sNomeDirTmp, true);

                            // *** azione sul DB ***
                            bResult &= _rdBaseIntf.dbRenameTables(sDestinationPostFix);
                        }
                    }
                    catch (IOException)
                    {
                        bResult = false;
                    }

                    if (bResult == true)
                    {
                        sTmp = String.Format("FrmMain chiudiIncasso : {0} Chiusura cassa riuscita !", sDestinationPostFix);
                        LogToFile(sTmp);
                    }
                    else
                    {
                        // errore in caso di file esistente
                        sTmp = String.Format("FrmMain chiudiIncasso : Chiusura cassa non riuscita !");
                        LogToFile(sTmp);

                        MessageBox.Show("Chiusura cassa non riuscita !", "Attenzione !", MessageBoxButtons.OK);
                    }

                    // altrimenti chiama SalvaDati
                    SF_Data.iNumOfLastReceipt = 0;

                    ErrorManager(ERR_CHC);
                }

                rRenameDlg.Dispose();
            }
        }

        private void MnuExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        /******************************************
         ***	Gestione del menù: Modifica 	***
         ******************************************/
        private void MnuModDispArticoli_Click(object sender, EventArgs e)
        {
            if (!MnuEsperto.Checked && MnuModDispArticoli.Checked)
                MessageBox.Show("Per modificare la disponibilità fare\ndoppio click sulla voce che interessa.", "Attenzione", MessageBoxButtons.OK);

            CheckMenuItems();
            _iModDisponibilitaTimeout = Define.TIMEOUT_MOD_QUANTITA;
        }

        private void MnuAnnulloOrdine_Click(object sender, EventArgs e)
        {
            if (DataManager.GetNumOfLocalOrders() == 0)
            {
                WrnMsg.iErrID = WRN_DNP;
                WarningManager(WRN_DNP);
                return;
            }

            VisOrdiniDlg rVisOrdiniDlg = new VisOrdiniDlg(GetActualDate(), DataManager.GetNumOfLocalOrders(), "", VIEW_TYPE.CANCEL_ORDER);

            FormResize(null, null);
            MainGrid_Redraw(this, null);

            rVisOrdiniDlg.Dispose();
        }

        private void MnuChangePayment_Click(object sender, EventArgs e)
        {
            if (DataManager.GetNumOfLocalOrders() == 0)
            {
                WrnMsg.iErrID = WRN_DNP;
                WarningManager(WRN_DNP);
                return;
            }

            VisOrdiniDlg rVisOrdiniDlg = new VisOrdiniDlg(GetActualDate(), DataManager.GetNumOfLocalOrders(), "", VIEW_TYPE.CHANGE_PAYMENT);

            rVisOrdiniDlg.Dispose();
        }

        /******************************************
         ***    Gestione del menù: Visualizza 	***
         ******************************************/
        private void MnuVisIncassoOggi_Click(object sender, EventArgs e)
        {
            String sTmp;

            if (DataManager.GetNumOfLocalOrders() == 0)
            {
                WrnMsg.iErrID = WRN_DNP;
                WarningManager(WRN_DNP);
                return;
            }

            VisDatiDlg rVisDatiDlg = new VisDatiDlg();

            sTmp = DataManager.GetDataDir() + "\\" + GetNomeFileDati(SF_Data.iNumCassa, GetActualDate());

            if (!File.Exists(sTmp))
                DataManager.SalvaDati();

            // usa la data corrente
            rVisDatiDlg.VisualizzaDati((int)FILE_TO_SHOW.FILE_DATI, GetActualDate(), SF_Data.iNumCassa, false);
            rVisDatiDlg.Dispose();
        }

        private void MnuVisIncassoAltraData_Click(object sender, EventArgs e)
        {
            SelectionRange selDates;
            VisDatiDlg rVisDatiDlg = new VisDatiDlg();

            SelDataDlg.rSelDataDlg.ShowDialog();
            selDates = SelDataDlg.rSelDataDlg.GetDateFromPicker();

            if (selDates != null) // non sono uscito con Cancel ...
                rVisDatiDlg.VisualizzaDati((int)FILE_TO_SHOW.FILE_DATI, selDates, SF_Data.iNumCassa, true);

            rVisDatiDlg.Dispose();
        }

        private void MnuVisListino_Click(object sender, EventArgs e)
        {
            VisDatiDlg rVisDatiDlg = new VisDatiDlg();

            if (_bListinoModificato)
                DataManager.SalvaListino(); // aggiorna il file prima di visualizzarlo

            rVisDatiDlg.VisualizzaDati((int)FILE_TO_SHOW.FILE_PREZZI, GetActualDate(), SF_Data.iNumCassa, false);

            rVisDatiDlg.Dispose();
        }

        private void MnuEsploraDB_Click(object sender, EventArgs e)
        {
            EsploraDB_Dlg rEsploraDB_Dlg = new EsploraDB_Dlg();
            rEsploraDB_Dlg.ShowDialog();

            rEsploraDB_Dlg.Dispose();
        }

        private void MnuEsploraRemDB_Click(object sender, EventArgs e)
        {
            if (rEsploraRemOrdiniDB_Dlg == null)
            {
                rEsploraRemOrdiniDB_Dlg = new EsploraRemOrdiniDB_Dlg();

                _webPrintTimeout = 0;
                rdbCaricaTabella();
                rEsploraRemOrdiniDB_Dlg.Show();
            }
            else if (rEsploraRemOrdiniDB_Dlg.Visible)
            {
                rEsploraRemOrdiniDB_Dlg.Hide();
                MnuEsploraOrdiniWeb.Checked = false;
            }
            else
            {
                rEsploraRemOrdiniDB_Dlg.Show();
                MnuEsploraOrdiniWeb.Checked = true;

                _webPrintTimeout = 0;
                rdbCaricaTabella();
            }
        }

        private void MnuAnteprimaScontrino_Click(object sender, EventArgs e)
        {
            if (AnteprimaDlg.rAnteprimaDlg.Visible)
            {
                AnteprimaDlg.rAnteprimaDlg.Hide();
                MnuReceiptPreview.Checked = false;
            }
            else
            {
                AnteprimaDlg.rAnteprimaDlg.Show();
                MnuReceiptPreview.Checked = true;

                AnteprimaDlg.rAnteprimaDlg.RedrawReceipt();
                Focus();
            }
        }

        private void MnuVisMessaggiInviati_Click(object sender, EventArgs e)
        {
            VisMessaggiDlg rVisMessaggiDlg = new VisMessaggiDlg();

            rVisMessaggiDlg.VisualizzaMessaggio(MAX_NUM_MSG);

            rVisMessaggiDlg.Dispose();
        }

        private void MnuVisOrdiniAltraData_Click(object sender, EventArgs e)
        {
            SelectionRange selDates;

            SelDataDlg.rSelDataDlg.ShowDialog();
            selDates = SelDataDlg.rSelDataDlg.GetDateFromPicker();

            if (selDates != null) // non sono uscito con Cancel ...
            {
                VisOrdiniDlg rVisOrdiniDlg = new VisOrdiniDlg(selDates.Start, VisOrdiniDlg.MAX_NUM_TICKET);
                rVisOrdiniDlg.Dispose();
            }
        }

        private void MnuVisOrdiniEmessi_Click(object sender, EventArgs e)
        {
            if (DataManager.GetNumOfLocalOrders() == 0)
            {
                WrnMsg.iErrID = WRN_DNP;
                WarningManager(WRN_DNP);
                return;
            }

            SF_Data.iNumOfLastReceipt = DataManager.GetNumOfOrders();
            UpdateStatusBar(SF_Data.iNumOfLastReceipt, 0);

            VisOrdiniDlg rVisOrdiniDlg = new VisOrdiniDlg(GetActualDate(), DataManager.GetNumOfLocalOrders());

            rVisOrdiniDlg.Dispose();
        }

        /**************************************
         ***    Gestione del menù: Imposta 	***
         **************************************/

        /// <summary>Protezione contro le modifiche non volute</summary>
        private void MnuEsperto_Click(object sender, EventArgs e)
        {
            String sTmp;
            DialogResult dResult;

            if (!_bPasswordIsGood)
            {
                if (!CheckService(_ESPERTO) && !MnuEsperto.Checked)
                {
                    MessageBox.Show("E' importante aver letto e compreso il manuale prima di proseguire !\r\n\r\n" +
                            "Il manuale pdf è presente nella cartella di installazione e si può aprire anche dal pulsante presente nel menù di Aiuto->Aiuto Rapido.",
                            "Attenzione !", MessageBoxButtons.OK);
                }

                // avvia dialogo di verifica password
                PasswordDlg _rPasswordDlg = new PasswordDlg(true);
                dResult = _rPasswordDlg.ShowDialog();

                if (dResult == DialogResult.OK)
                {
                    MnuEsperto.Checked = !MnuEsperto.Checked;
                    MessageBox.Show("Password verificata con successo !", "Attenzione !", MessageBoxButtons.OK);
                }
                else
                    return;
            }
            else
            {
                MnuEsperto.Checked = !MnuEsperto.Checked;
            }

            CheckMenuItems();

            sTmp = String.Format("Modo Esperto {0}", MnuEsperto.Checked);
            LogToFile(sTmp);
        }

        private void MnuCambiaPassword_Click(object sender, EventArgs e)
        {
            DialogResult dResult;

            PasswordDlg _rPasswordDlg = new PasswordDlg(false);
            dResult = _rPasswordDlg.ShowDialog();

            if (dResult == DialogResult.OK)
                MessageBox.Show("Password modificata con successo !", "Attenzione !", MessageBoxButtons.OK);
        }

        private void MnuConfigurazioneRete_Click(object sender, EventArgs e)
        {
            NetConfigDlg.rNetConfigDlg.Init(true);
        }

        private void MnuImpostaStampanteWin_Click(object sender, EventArgs e)
        {
            WinPrinterDlg._rWinPrinterDlg.Init(true);

            if (WinPrinterDlg.GetListinoModificato())
                DataManager.SalvaListino();
        }

        private void MnuImpostaStampanteLegacy_Click(object sender, EventArgs e)
        {
            LegacyPrinterDlg.rThermPrinterDlg.Init(true);
        }

        private void MnuImpostaCopieLocali_Click(object sender, EventArgs e)
        {
            PrintReceiptConfigDlg._rPrintTckConfigDlg.Init(true);

            if (PrintReceiptConfigDlg.GetListinoModificato())
                DataManager.SalvaListino();
        }

        private void MnuImpostaCopieInRete_Click(object sender, EventArgs e)
        {
            PrintNetCopiesConfigDlg._rPrintConfigDlg.Init(true);

            if (PrintNetCopiesConfigDlg.GetListinoModificato())
                DataManager.SalvaListino();
        }

        private void MnuImpOpzioni_Click(object sender, EventArgs e)
        {
            OptionsDlg._rOptionsDlg.Init(true);

            if (OptionsDlg.GetListinoModificato())
                DataManager.SalvaListino();

            if (OptionsDlg._rOptionsDlg.GetShowPrevReceipt())
                SetShowTotaleScontrinoPrec(true);
            else
            {
                SetShowTotaleScontrinoPrec(false);
                lblStatusTotalePrec.Text = "";
            }

            SetColorsTheme();

            CheckMenuItems();
        }

        private void MnuImpHeader_Click(object sender, EventArgs e)
        {
            EditHeaderFooterDlg rHeaderFooterDlg = new EditHeaderFooterDlg();

            if (EditHeaderFooterDlg.bGetListinoModificato())
                DataManager.SalvaListino();

            rHeaderFooterDlg.Dispose();
        }

        private void MnuImpDimGrid_Click(object sender, EventArgs e)
        {
            EditGrigliaDlg rSetGridDlg = new EditGrigliaDlg();

            if (EditGrigliaDlg.GetListinoModificato())
            {
                DataManager.SalvaListino();

                SetTabsAppearance();
                UpdateStatusBar("Pronto");
                FormResize(this, null);
            }

            rSetGridDlg.Dispose();
        }

        private void MnuImpListino_Click(object sender, EventArgs e)
        {
            if (MnuImpListino.Checked)
            {
                BtnVisListino.Checked = true;

                String sTmp = " Per modificare il Listino prezzi fare doppio click sulla voce che interessa.\n\n\n" +
                    " E'possibile riorganizzare la disposizione degli Articoli con i seguenti tasti:\n\n " +
                    "  Ins, Del: inserisce/elimina spazi vuoti all'interno della griglia visualizzata,\n\n" +
                    "  Ctrl+Ins, Ctrl+Del: inserisce/elimina spazi vuoti oltre la griglia visualizzata,\n\n" +
                    "  Ctrl+Up, Ctrl+Down: fa scorrere la cella corrente verso l'alto/basso.\n\n\n" +
                    "  Si possono anche trascinare le celle della griglia con il mouse: si veda il manuale";

                QuickHelpDlg rQuickHelpDlg = new QuickHelpDlg(sTmp.Replace("\n", "\r\n"), 500, 340);
                rQuickHelpDlg.Dispose();

                _sCopertiPrev = EditCoperti.Text;
                EditCoperti.Text = String.Format("{0,4:0.00}", SF_Data.Articolo[MAX_NUM_ARTICOLI - 1].iPrezzoUnitario / 100.0f);
            }
            else
            {
                BtnVisListino.Checked = false; // importante
                EditCoperti.Text = _sCopertiPrev;
            }

            CheckMenuItems();
        }

        private void MnuQHelp_Click(object sender, EventArgs e)
        {
            QuickHelpDlg rQuickHelpDlg = new QuickHelpDlg();

            rQuickHelpDlg.Dispose();
        }

        private void MnuInfo_Click(object sender, EventArgs e)
        {
            InfoDlg rInfoDlg = new InfoDlg();
            rInfoDlg.ShowDialog();
            rInfoDlg.Dispose();
        }

        /******************************************
         ***		Gestione dei buttons  		***
         ******************************************/
        private void BtnVisListino_Click(object sender, EventArgs e)
        {

            if (BtnVisListino.Checked)
            {
                BtnVisListino.Checked = false;

                EditCoperti.Text = _sCopertiPrev;
            }
            else
            {
                BtnVisListino.Checked = true;
                _iVisPrzTimeout = TIMEOUT_VIS_PREZZI;

                _sCopertiPrev = EditCoperti.Text;
                EditCoperti.Text = String.Format("{0,4:0.00}", SF_Data.Articolo[MAX_NUM_ARTICOLI - 1].iPrezzoUnitario / 100.0f);
            }

            CheckMenuItems();
        }

        private void BtnX10_Click(object sender, EventArgs e)
        {
            if (BtnX10.Checked)
                sStatusText = "Premuto x10, inserire le unità";
            else
                sStatusText = "Pronto";
        }

        /// <summary>Verifica che tutto sia pronto per l'emissione dello scontrino</summary>
        public void BtnScontrino_Click(object sender, EventArgs e)
        {
            bool bResult;
            String sActualDateStr;
            String[] sQueue_Object = new String[2] { WEB_ORDER_PRINT_DONE, "" };

            // se si arriva qui dal test
            if (CheckService(Define._AUTO_SEQ_TEST))
            {
                BtnScontrino.Checked = true;
                Thread.Sleep(1000); // per rendere il push visibile
            }

            MainGrid.Focus(); // evita inserimenti indesiderati del tavolo

            if (bUSA_NDB())
            {
                // carica la disponibilità dal DB
                _iDBDispTimeout = _REFRESH_DISP;
                DataManager.AggiornaDisponibilità(); // carica i dati aggiornati di Disponibilità

            }

            // verifiche : scontrino non nulle, quantità < disponibilità, dimenticanza tavolo
            if (DataManager.TicketIsGood() && VerificaTutteQuantita() && VerificaTavoloRichiesto() && VerificaCopertoRichiesto() && VerificaPOS_Richiesto())
            {
                //
                if (IsBitSet(SF_Data.iStatusReceipt, BIT_CARICATO_DA_WEB))
                {
                    // scarico immediato solo per la CASSA_PRINCIPALE, per le CASSE_SECONDARIE
                    // si mette in coda l'ordine
                    //if (SF_Data.iNumCassa == CASSA_PRINCIPALE)
                    bResult = rdbSegnaOrdineStampato(SF_Data.iNumOrdineWeb);
                    //else
                    //    bResult = false;

                    if (bResult)
                        EsploraRemOrdiniDB_Dlg.EventEnqueue(sQueue_Object);
                    else
                        _rdBaseIntf.dbWebOrderEnqueue(SF_Data.iNumOrdineWeb);
                }

                if (IsBitSet(SF_Data.iStatusReceipt, BIT_CARICATO_DA_PREVENDITA))
                    _rdBaseIntf.dbScaricaOrdinePrevendita(SF_Data.iNumOrdinePrev, _sOrdiniPrevDBTable);

                _bPrintTimeoutEnabled = true;
                _iPrintTimeout = TIMEOUT_PRINT_RESET; // 5s

                BtnScontrino.Enabled = true;
                EditContante.Enabled = true;
                comboCashPos.Enabled = true;
                EditResto.Enabled = true;

                if (bUSA_NDB())
                {
                    /***********************************************************************************
                     * controllo coerenza : se le date sono diverse il DB ritorna 0 e solleva l'errore
                     * quindi emissione Scontrino o messaggio di errore
                     ***********************************************************************************/
                    if (_rdBaseIntf.dbNewOrdineNumRequest() > 0)
                    {
                        EmissioneScontrino();
                        LogToFile("Mainform : EmissioneScontrino");
                    }
                    else
                    {
                        sActualDateStr = GetActualDate().ToString("dd/MM/yy");

                        WrnMsg.iErrID = WRN_DNA;
                        WrnMsg.sMsg = String.Format("- PC Locale : {0}\n\n- Database : {1}\n", sActualDateStr, dbGetDateFromDB());
                        WarningManager(WrnMsg);
                    }
                }
                else
                {
                    // *** punto unico ***
                    SF_Data.iNumOfLastReceipt++;
                    SF_Data.iActualNumOfReceipts++;

                    EmissioneScontrino();
                    LogToFile("Mainform : EmissioneScontrino _ql");
                }

                AnteprimaDlg.rAnteprimaDlg.RedrawTicketNum();
            }
            else
            {
                BtnScontrino.Checked = false;
                BtnScontrino.Enabled = true;

                EditStatus_BC.Text = "";

                if (OptionsDlg._rOptionsDlg.GetPresales_LoadMode())
                    EditStatus_BC.Focus();
            }

            CheckMenuItems();

            MainGrid_Redraw(this, null); // aggiorna visivamente
        }

        // percorsa al cambiare del Tab
        private void TabSet_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TabSet.SelectedTab == tabPage0)
            {
                iArrayOffset = 0;
            }
            else if (TabSet.SelectedTab == tabPage1)
            {
                iArrayOffset = 1 * iLastGridIndex;
            }
            else if (TabSet.SelectedTab == tabPage2)
            {
                iArrayOffset = 2 * iLastGridIndex;
            }
            else if (TabSet.SelectedTab == tabPage3)
            {
                iArrayOffset = 3 * iLastGridIndex;
            }
            else if ((TabSet.SelectedTab == tabPage4) && SF_Data.bTouchMode)
            {
                iArrayOffset = 4 * iLastGridIndex;
            }

            FormResize(this, null);
            MainGrid_Redraw(this, null);
        }

        private void TabSet_MouseMove(object sender, MouseEventArgs e)
        {
            if (MnuImpListino.Checked)
            {
                Cursor = Cursors.Default;
                bMouseWrongPos = true; // il mouse pointer non è in MainGrid
            }
        }

        private void BtnDB_Click(object sender, EventArgs e)
        {
            String sRemDBChecksum;
            String[] sQueue_Object = new String[2];

            if (bUSA_NDB())
            {
                SF_Data.iNumOfLastReceipt = DataManager.GetNumOfOrders();
                UpdateStatusBar(SF_Data.iNumOfLastReceipt, 0);

                _rdBaseIntf.dbCheck();

                DataManager.AggiornaDisponibilità();

                BtnDB.Image = BtnImgList.Images[0];
                _iDBDispTimeout = _REFRESH_DISP_SHORT; // dopo 2s dbCaricaDisponibilità()
            }

            if (dBaseTunnel_my.GetWebServiceReq())
            {
                if ((SF_Data.iNumCassa == CASSA_PRINCIPALE) && dBaseTunnel_my.rdbPing())
                {
                    SetWebPriceListLoadRequest();

                    sRemDBChecksum = dBaseTunnel_my.rdbCheckListino(2000); // se il listino non esiste ritorna una stringa vuota

                    if (!String.IsNullOrEmpty(sRemDBChecksum) && (sRemDBChecksum != DataManager.GetWebListinoChecksum()) && (SF_Data.iNumCassa == CASSA_PRINCIPALE))
                    {
                        LogToFile("Mainform : rdbSalvaListino() per checksum non corrispondente");

                        // avvia rdbSalvaListino()
                        sQueue_Object[0] = WEB_PRICELIST_LOAD_START;

                        dBaseTunnel_my.EventEnqueue(sQueue_Object);
                    }
                }
            }
        }

        private void BtnSendMsg_Click(object sender, EventArgs e)
        {
            VisMessaggiDlg rVisMessaggiDlg = new VisMessaggiDlg();

            rVisMessaggiDlg.NuovoMessaggioCucina();

            rVisMessaggiDlg.Dispose();
        }


        private void BtnSconto_Click(object sender, EventArgs e)
        {
            // sicurezza
            if (!(MnuImpListino.Checked || MnuModDispArticoli.Checked))
            {
                ScontoDlg.rScontoDlg.Init();

                if ((SF_Data.iStatusSconto & 0x000000FF) != 0)
                {
                    BtnSconto.Checked = true;
                    BtnSconto.Image = Properties.Resources.sconto_si;
                }
                else
                {
                    BtnSconto.Checked = false;
                    BtnSconto.Image = Properties.Resources.sconto_no;
                }
            }
            else
                BtnSconto.Checked = false;

            AnteprimaDlg.rAnteprimaDlg.RedrawReceipt();
            _iAnteprimaTotParziale = AnteprimaDlg.GetTotaleReceipt();
        }

        private void BtnPlus_Click(object sender, EventArgs e)
        {
            if (EditCoperti.Focused)
            {
                int iNum = 0;

                if (!String.IsNullOrEmpty(EditCoperti.Text))
                    iNum = Convert.ToInt32(EditCoperti.Text);

                iNum++;

                EditCoperti.Text = String.Format("{0,3}", iNum);

                TextBox_KeyUp(EditCoperti, null);
            }
            else if (!String.IsNullOrEmpty(SF_Data.Articolo[_iCellPt].sTipo))
            {
                iFocus_BC_Timeout = BC_FOCUS_TIMEOUT;

                SF_Data.Articolo[_iCellPt].iQuantitaOrdine++;

                AnteprimaDlg.rAnteprimaDlg.RedrawReceipt();
                _iAnteprimaTotParziale = AnteprimaDlg.GetTotaleReceipt();

                MainGrid_Redraw(this, null);
            }

            scannerInputQueue.Clear();
        }

        private void BtnMinus_Click(object sender, EventArgs e)
        {
            if (EditCoperti.Focused)
            {
                int iNum = 0;

                if (!String.IsNullOrEmpty(EditCoperti.Text))
                    iNum = Convert.ToInt32(EditCoperti.Text);

                if (iNum > 0)
                    iNum--;

                EditCoperti.Text = String.Format("{0,3}", iNum);

                TextBox_KeyUp(EditCoperti, null);
            }
            else if (!String.IsNullOrEmpty(SF_Data.Articolo[_iCellPt].sTipo))
            {
                iFocus_BC_Timeout = BC_FOCUS_TIMEOUT;

                if (SF_Data.Articolo[_iCellPt].iQuantitaOrdine > 0)
                    SF_Data.Articolo[_iCellPt].iQuantitaOrdine--;

                AnteprimaDlg.rAnteprimaDlg.RedrawReceipt();

                _iAnteprimaTotParziale = AnteprimaDlg.GetTotaleReceipt();
                MainGrid_Redraw(this, null);
            }

            scannerInputQueue.Clear();
        }

        private void BtnCanc_Click(object sender, EventArgs e)
        {
            if (EditCoperti.Focused)
            {
                EditCoperti.Text = String.Format("{0,3}", 0);

                TextBox_KeyUp(EditCoperti, null);
            }
            else
            {
                SF_Data.Articolo[_iCellPt].iQuantitaOrdine = 0;
                iFocus_BC_Timeout = BC_FOCUS_TIMEOUT;
            }

            AnteprimaDlg.rAnteprimaDlg.RedrawReceipt();
            _iAnteprimaTotParziale = AnteprimaDlg.GetTotaleReceipt();

            scannerInputQueue.Clear();
            MainGrid_Redraw(this, null);
        }

        private void ComboCashPos_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboCashPos.SelectedIndex)
            {
                case 0:
                    SF_Data.iStatusReceipt = ClearBit(SF_Data.iStatusReceipt, BIT_PAGAM_CARD);
                    SF_Data.iStatusReceipt = ClearBit(SF_Data.iStatusReceipt, BIT_PAGAM_SATISPAY);
                    AnteprimaDlg.rAnteprimaDlg.RedrawReceipt();
                    break;
                case 1:
                    SF_Data.iStatusReceipt = SetBit(SF_Data.iStatusReceipt, BIT_PAGAM_CARD);
                    SF_Data.iStatusReceipt = ClearBit(SF_Data.iStatusReceipt, BIT_PAGAM_SATISPAY);
                    AnteprimaDlg.rAnteprimaDlg.RedrawReceipt();
                    break;
                case 2:
                    SF_Data.iStatusReceipt = ClearBit(SF_Data.iStatusReceipt, BIT_PAGAM_CARD);
                    SF_Data.iStatusReceipt = SetBit(SF_Data.iStatusReceipt, BIT_PAGAM_SATISPAY);
                    AnteprimaDlg.rAnteprimaDlg.RedrawReceipt();
                    break;
                default:
                    // se si toglie il commento si pulisce l'Anteprima dopo la stampa dello scontrino
                    // perchè tutte le quantità sono a 0
                    // AnteprimaDlg.rAnteprimaDlg.RedrawReceipt(); 
                    break;
            }
        }

        /// <summary>/// imposta CARD come metodo di pagamento/// </summary>
        public void SetPagamento_CARD()
        {
            comboCashPos.SelectedIndex = 0;
            comboCashPos.SelectedIndex = 1; // forza la chiamata di ComboCashPos_SelectedIndexChanged
        }

        /// <summary>/// imposta SATISPAY come metodo di pagamento/// </summary>
        public void SetPagamento_SATISPAY()
        {
            comboCashPos.SelectedIndex = 0;
            comboCashPos.SelectedIndex = 2; // forza la chiamata di ComboCashPos_SelectedIndexChanged
        }

        private void MainGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //MainGrid.Focus(); // non serve cliccandoci ho il focus
            iFocus_BC_Timeout = BC_FOCUS_TIMEOUT;

            // _iNewCellPt serve a non incrementare quando ci si sposta sulla griglia
            int _iNewCellPt = e.ColumnIndex * MainGrid.RowCount + e.RowIndex + iArrayOffset;

            if ((EditNota.BackColor == Color.LightBlue) && !_bCrtlIsPressed)
            {
                // reset EditNota
                EditNota.BackColor = Color.Gainsboro;
                EditNota.Text = _sEditNotaCopy;
                EditNota.MaxLength = 28;
                lblNota.Text = "Nota:";

                MainGrid_Redraw(this, null);
            }
            // si impone che deve essere iQuantitaOrdine > 0 per imporstare la nota Articolo
            else if ((!MnuImpListino.Checked) && (!MnuModDispArticoli.Checked) && _bCrtlIsPressed &&
                    (SF_Data.Articolo[_iNewCellPt].iQuantitaOrdine > 0))
            {
                _sEditNotaCopy = EditNota.Text;

                // si predispone per acquisire la Nota Articolo
                EditNota.Text = SF_Data.Articolo[_iNewCellPt].sNotaArt;
                EditNota.BackColor = Color.LightBlue;
                EditNota.MaxLength = 25;
                lblNota.Text = "Nota Art:";

                _bCrtlIsPressed = false; // altrimenti rimane troppo a lungo il Focus()
                EditNota.Focus();
            }
            else if ((!MnuImpListino.Checked) && (!MnuModDispArticoli.Checked) && _bCrtlIsPressed &&
                    (SF_Data.Articolo[_iNewCellPt].iQuantitaOrdine == 0))
            {
                _bCrtlIsPressed = false; // altrimenti rimane troppo a lungo il Focus()

                _WrnMsg.iErrID = WRN_NQZ;
                WarningManager(_WrnMsg);
            }

            else if (SF_Data.bTouchMode && (_iCellPt == _iNewCellPt) && !MnuModDispArticoli.Checked && !MnuImpListino.Checked)
                BtnPlus_Click(sender, e);

            _iCellPt = _iNewCellPt;
        }

        /// <summary></summary>
        public void BtnEsportazione_Click(object sender, EventArgs e)
        {
            // se si arriva solo dal test con sender == null
            if (!BtnEsportazione.Checked && (sender == null))
                BtnEsportazione.Checked = true;

            // ripetuto per sicurezza
            if (BtnEsportazione.Checked)
                SF_Data.iStatusReceipt = SetBit(SF_Data.iStatusReceipt, BIT_ESPORTAZIONE);
            else
                SF_Data.iStatusReceipt = ClearBit(SF_Data.iStatusReceipt, BIT_ESPORTAZIONE);

            AnteprimaDlg.rAnteprimaDlg.RedrawReceipt();
        }

        private void EditStatusResto_MouseClick(object sender, MouseEventArgs e)
        {
            EditContante.Focus();
        }

        private void EditStatus_BC_KeyPress(object sender, KeyPressEventArgs e)
        {
            bool bResult;
            int iNumScontrino, iGruppo, iLength;
            String sLog, sStrBarcode, sStrNum, sStrDay, sStrGruppo;
            String JSON_Type = "";

            //Dictionary<int, object> dict = null;

            iFocus_BC_Timeout = BC_FOCUS_TIMEOUT;

            // EditStatus_BC.UseSystemPasswordChar = true; si perde il carattere successivo

            /*************************
                      ENTER
             *************************/
            if (e.KeyChar == '\r')
            {
                // scarta edit vuoti
                if (String.IsNullOrEmpty(EditStatus_BC.Text))
                    return;
                else
                {
                    sStrBarcode = EditStatus_BC.Text;
                    sStrBarcode = sStrBarcode.Trim();

                    iLength = sStrBarcode.Length;

                    try
                    {
                        Dictionary<String, object> dict = null;

                        try
                        {
                            // ottiene la tabella dell'output JSON
                            var jss = new JavaScriptSerializer();
                            string sTmpJson = "";

                            // correzione tastiera Italiana 142, QRcode non produce le parentesi !
                            if ((sStrBarcode[0] != '{') && sStrBarcode.Contains(_JS_ORDER_V5))
                                sTmpJson = '{' + sStrBarcode + '}';
                            else
                                sTmpJson = sStrBarcode;

                            dict = jss.Deserialize<dynamic>(sTmpJson);

                            JSON_Type = dict["-10"].ToString();
                        }
                        catch (Exception)
                        {
                        }

                        if (JSON_Type == _JS_ORDER_V5)
                        {
                            dbAzzeraDatiOrdine();

                            DB_Data.iNumOrdineWeb = Convert.ToInt32(dict["-9"]);

                            if (dict.ContainsKey("-8"))
                                DB_Data.bAnnullato = (Convert.ToInt32(dict["-8"]) > 0);

                            if (dict.ContainsKey("-7"))
                                DB_Data.bStampato = (Convert.ToInt32(dict["-7"]) > 0);

                            DB_Data.iNumCassa = Convert.ToInt32(dict["-6"]);
                            DB_Data.iStatusReceipt = Convert.ToInt32(dict["-5"]);

                            if (dict.ContainsKey("-4"))
                                DB_Data.sNome = dict["-4"].ToString();

                            if (dict.ContainsKey("-3"))
                                DB_Data.sTavolo = dict["-3"].ToString();

                            if (dict.ContainsKey("-2"))
                                DB_Data.sNota = dict["-2"].ToString();

                            DB_Data.sPL_Checksum = dict["-1"].ToString();

                            // sparato il QRCode di test
                            if (DB_Data.sPL_Checksum == QRC_TEST_CKS)
                            {
                                if (sStrBarcode == QRC_TEST)
                                {
                                    WrnMsg.sMsg = QRC_TEST;
                                    WrnMsg.iErrID = WRN_TQR;

                                    WarningManager(WrnMsg);

                                    bResult = false;
                                    EditStatus_BC.Text = ""; // pulizia
                                    return;
                                }
                            }

                            int iIndex, iQuantitaOrdine;

                            foreach (var item in dict)
                            {
                                if (Convert.ToInt32(item.Key) >= 0)
                                {
                                    iIndex = Convert.ToInt32(item.Key);
                                    iQuantitaOrdine = Convert.ToInt32(item.Value);

                                    DB_Data.Articolo[iIndex].iQuantitaOrdine = iQuantitaOrdine;
                                }
                            }

                            if (DB_Data.bAnnullato || DB_Data.bStampato)
                                bResult = false;
                            else
                            {
                                sLog = String.Format("Mainform : CKW = {0}, {1}", DataManager.GetWebListinoChecksum(), sStrBarcode);
                                LogToFile(sLog);

                                // ************* caricamento in SF_Data e MainForm *************
                                bResult = DataManager.CaricaOrdine_QR_code(DB_Data.iNumOrdineWeb);
                            }

                            if (!bResult)
                                EditStatus_BC.Text = ""; // pulizia

                            sLog = String.Format("Mainform : barcode web = {0}, {1}", DB_Data.iNumOrdineWeb, bResult);
                            LogToFile(sLog);

                            if (CheckService(Define._AUTO_QRCODE_TEST))
                            {
                                // emissione scontrino
                                BtnScontrino_Click(this, null);
                                MainGrid_Redraw(this, null);
                            }
                        }

                        // sparato barcode con checksum corretto
                        else if (StandCommonFiles.Barcode_EAN13.VerifyChecksum(sStrBarcode))
                        {
                            // lo spazio è solo per allineamento
                            sStrNum = sStrBarcode.Substring(8, 4); // numero Scontrino

                            // ribaltamento formato data
                            sStrDay = sStrBarcode.Substring(6, 2) + sStrBarcode.Substring(4, 2) + sStrBarcode.Substring(2, 2);

                            sStrGruppo = sStrBarcode.Substring(0, 2);       // Gruppo

                            iNumScontrino = Convert.ToInt32(sStrNum);
                            iGruppo = Convert.ToInt32(sStrGruppo);

                            if (iGruppo == NUM_PRE_SALE_GRP) // sicurezza
                            {
                                _sOrdiniPrevDBTable = String.Format("{0}_{1}", _dbPreOrdersTablePrefix, sStrDay);
                                bResult = DataManager.CaricaOrdinePrev(iNumScontrino, _sOrdiniPrevDBTable);

                                if (!bResult)
                                    EditStatus_BC.Text = ""; // pulizia

                                sLog = String.Format("Mainform : barcode prevendita = {0}, {1}, {2}", _sOrdiniPrevDBTable, sStrNum, bResult);
                                LogToFile(sLog);
                            }
                            else if (iGruppo == NUM_WEB_SALE_GRP) // sicurezza
                            {
                                bResult = DataManager.CaricaOrdineWeb(iNumScontrino);

                                sLog = String.Format("Mainform : barcode web = {0}", iNumScontrino);
                                LogToFile(sLog);
                            }

                            if (CheckService(Define._AUTO_QRCODE_TEST))
                            {
                                // emissione scontrino
                                BtnScontrino_Click(this, null);
                                MainGrid_Redraw(this, null);
                            }
                        }
                        else
                        {
                            EditStatus_BC.Text = "";
                            return;
                        }

                    }
                    catch (Exception)
                    {
                        EditStatus_BC.Text = "";
                        WarningManager(WRN_QRE);
                    }

                    // EditStatus_BC.Text = ""; // è la stampa scontrino che farà pulizia !
                    return;
                }
            }
        }

        /// <summary>
        /// ricalcola il gruppo di stampa del primo articolo presente nalla pagina corrente della griglia<br/>
        /// attenzione che il limite _iLastArticoloIndexP1 vale solo per SF_Data.Articolo[] e non per DB_Data.Articolo[]
        /// </summary>
        public int CheckFirstGroupIndex()
        {
            int iPrimaGroup = 0;

            for (int i = iArrayOffset; (i < MAX_NUM_ARTICOLI - 1); i++) // COPERTI esclusi
                if (!String.IsNullOrEmpty(SF_Data.Articolo[i].sTipo))
                {
                    iPrimaGroup = SF_Data.Articolo[i].iGruppoStampa;
                    break;
                }

            return iPrimaGroup;
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            int i, j, iNumTicket, iDB_StringsCount, iDB_dataTablesCount;
            string sNomeTableTmp, sCell, sNomeDirTmp, sTmp;
            List<string> sElencoStrings = new List<string>();

            // Stop del timer
            Timer.Enabled = false;

            if ((SF_Data.iNumOfLastReceipt > 0) || (SF_Data.iNumOfMessages > 0) || DataManager.CheckDispLoaded())
                DataManager.SalvaDati();

            if (_bListinoModificato)
                DataManager.SalvaListino();

            if (!CheckService(Define._AUTO_RECEIPT_GEN) && !CheckService(Define._AUTO_SEQ_TEST))
            {
                // ***** puliza finale *****
                try
                {
                    // cancellazione tickets
                    sNomeDirTmp = DataManager.GetTicketsDir();
                    if (Directory.Exists(sNomeDirTmp))
                        Directory.Delete(sNomeDirTmp, true);

                    // cancellazione messaggi
                    sNomeDirTmp = DataManager.GetMessagesDir();
                    if (Directory.Exists(sNomeDirTmp))
                        Directory.Delete(sNomeDirTmp, true);

                    // cancellazione copie
                    sNomeDirTmp = DataManager.GetCopiesDir();
                    if (Directory.Exists(sNomeDirTmp))
                        Directory.Delete(sNomeDirTmp, true);

                    sElencoStrings.Clear();

                    iDB_StringsCount = _rdBaseIntf.dbElencoTabelle(sElencoStrings);
                    iDB_dataTablesCount = 0;

                    for (i = CASSA_PRINCIPALE; i < (MAX_CASSE_SECONDARIE + 1); i++)
                    {
                        sNomeTableTmp = GetNomeDatiDBTable(i, GetActualDate());

                        for (j = 0; j < iDB_StringsCount; j++)
                        {
                            sCell = sElencoStrings[j];

                            // esclude tabelle di chiusura
                            if (sCell.Contains(sNomeTableTmp) && !sCell.Contains(sNomeTableTmp + '_'))
                            {
                                // ci sono altre tabelle dati oltre alla CASSA_PRINCIPALE
                                iDB_dataTablesCount++;
                            }
                        }

                    }

                    // cancellazione tabella dati vuota e priva di disponibilità caricata in data odierna,
                    // e non ci sono altre tabelle dati
                    //  
                    // SICUREZZA:
                    // si chiama _rdBaseIntf.dbCaricaDatidaOrdini(GetActualDate(), SF_Data.iNumCassa, true) invece di GetNumOfOrders()
                    // perchè restituisce -1 in caso di errore, inoltre è comune a SQLite, MySql, PostgreSql
                    iNumTicket = _rdBaseIntf.dbCaricaDatidaOrdini(GetActualDate(), SF_Data.iNumCassa, true);

                    if ((iNumTicket <= 0) && (DB_Data.iNumOfMessages <= 0) && !DataManager.CheckDispLoaded() && (iDB_dataTablesCount == 1))
                    {
                        _rdBaseIntf.dbDropTable(GetNomeDatiDBTable(SF_Data.iNumCassa, GetActualDate()));
                        _rdBaseIntf.dbDropTable(GetNomeOrdiniDBTable(GetActualDate()));
                    }

                    if (NetConfigDlg.rNetConfigDlg.GetWebOrderEnabled())
                    {
                        sTmp = String.Format("Chiusura StandFacile: TK={0}, C={1}", iNumTicket, SF_Data.iNumCassa);
                        rdbLogWriteVersion(sTmp);
                    }
                }
                catch (Exception)
                {
                    LogToFile("FormClosing: problemi di cancellazione Dir");
                }
            }

            LogToFile("End Program");
            Thread.Sleep(500); // per dare il tempo al LogServer

            _tt.Dispose();

            // arresto dei server
            StopPrintServer();
            StopLogServer(); // deve stare per ultimo
        }

    } // end class
} // end namespace



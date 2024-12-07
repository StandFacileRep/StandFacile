/************************************************************
  NomeFile : StandCucina/MainForm.cs
  Data	   : 25.09.2024
  Autore   : Mauro Artuso
 ************************************************************/

using System;
using System.IO;
using System.Windows.Forms;

using static StandCommonFiles.ComDef;
using static StandCommonFiles.commonCl;
using static StandCommonFiles.ReceiptAndCopies;
using static StandCommonFiles.LogServer;
using static StandCommonFiles.Printer_Legacy;

using static StandFacile.glb;
using static StandFacile.Define;
using static StandFacile.NetConfigLightDlg;
using static StandFacile.dBaseIntf;
using static StandFacile.LogForm;
using System.Collections;
using System.Collections.Generic;

namespace StandFacile
{
    /// <summary>main form</summary>
    public partial class FrmMain : Form
    {
        /// <summary>riferimento a FrmMain</summary>
        public static FrmMain rFrmMain;

        // variabili membro
        static bool _bOnLine;

        static bool _bInitNetReadParams = true;

        static readonly bool[] _bCopyToPrint = new bool[NUM_EDIT_GROUPS];

        /// <summary>numro di scontrini da stampare che precedono iGlbCurrentOffline_TicketNum</summary>
        static int _iPrevTicketNum;
        /// <summary>numro di scontrini da stampare che comprendono e seguono iGlbCurrentOffline_TicketNum</summary>
        static int _iNextTicketNum;

        /// <summary>numero dello scontrino corrente in modo offline</summary>
        public static int iGlbCurrentOffline_TicketNum;

        /// <summary>numero del messaggio corrente in modo offline</summary>
        public static int iGlbCurrentOffline_MessageNum;

        /// <summary>numero dello scontrino corrente in modo online</summary>
        static int _iPrevShownOnline_TicketNum;

        /// <summary>numero del messaggio corrente in modo online</summary>
        static int _iPrevShownOnline_MessageNum;

        static ulong ulStart, ulStop, ulPingTime;

        static String _sNomeFileTicket;

        static int iTimerBmpTag;
        static int _iLEDstatus;
        static int _iWP_Delay = 0;

        bool[] _bSomethingInto_GrpToPrint = new bool[NUM_COPIES_GRPS]; // OK
        bool[] _bSomethingInto_ClrToPrint = new bool[NUM_COPIES_GRPS]; // OK

        // gestione cross thread
        static readonly Queue eventQueue = new Queue();

        List<int> _iElencoOrdiniNoPrint = new List<int>();

        /// <summary>evento cross Thread per completare le grafiche</summary>
        public static void evUpdateCOMLed(String[] sEvQueueObj) { eventQueue.Enqueue(sEvQueueObj); }

        /// <summary>evento cross Thread che attiva l'agiornamento della label DB</summary>
        public static void evQueueUpdate(String[] sEvQueueObj) { eventQueue.Enqueue(sEvQueueObj); }

        VisOrdiniDlg _rVisOrdiniDlg;

        /// <summary>costruttore</summary>
        public FrmMain()
        {
            InitializeComponent();

            TB_Tickets.ScrollBars = ScrollBars.Vertical;
            TB_Messaggi.ScrollBars = ScrollBars.Vertical;

            // TextBox ToolTip
            ToolTip tt = new ToolTip();
            tt.InitialDelay = 0;
            tt.ShowAlways = true;
            tt.SetToolTip(BtnOnline, "passa dal modo MANUALE ad AUTO e viceversa");
            tt.SetToolTip(btnAnt, "test connessione al DataBase");
            tt.SetToolTip(ME_TickNum, "Inserire Num Scontrino");
            tt.SetToolTip(BtnPrintTicket, "stampa scontrino");
            tt.SetToolTip(BtnPrintMsg, "stampa messaggio");
            tt.SetToolTip(BtnPrevTicket, "Visualizza Precedente");
            tt.SetToolTip(BtnNextTicket, "Visualizza Successivo");
            tt.SetToolTip(BtnPrevMsg, "Visualizza Precedente");
            tt.SetToolTip(BtnNextMsg, "Visualizza Successivo");

            tt.SetToolTip(TB_Tickets, "lo sfondo è Giallo per gli scontrini già stampati da StandCucina");
            tt.SetToolTip(TB_Messaggi, "lo sfondo è Giallo per i messaggi già stampati da StandCucina");

            initActualDate();

            String[] sQueue_Object = new String[2];

            rFrmMain = this;

            Text = Define.TITLE;

            _bOnLine = true;
            _iPrevShownOnline_TicketNum = -100;
            _iPrevShownOnline_MessageNum = -100;

            SF_Data.iNumCassa = CASSA_PRINCIPALE;

            // impostazione della directory di default per operazioni sui dati
            sRootDir = Directory.GetCurrentDirectory();

            sQueue_Object[0] = UPDATE_COM_LED_EVENT;
            sQueue_Object[1] = String.Format("{0:d1}", (int)COM_STATUS.COM_FREE);
            evUpdateCOMLed(sQueue_Object);

            if (bCheckService(_ESPERTO))
                MnuEspertoClick(this, null);
        }

        /// <summary>Init</summary>
        public void Init()
        {
            String sKeyGood;

            sKeyGood = sReadRegistry(DBASE_SERVER_NAME_KEY, "");

            if (String.IsNullOrEmpty(sKeyGood))
            {
                MnuEsperto.Checked = true;
                MessageBox.Show("E' la prima esecuzione, imposta la connessione al database e la stampante !", "Attenzione !", MessageBoxButtons.OK);

                // Imposta il nome del server
                NetConfigLightDlg.rNetConfigLightDlg.ShowDialog();
            }

            ME_TickNum.Text = "0";

            ClientTimer.Interval = DB_CLIENT_TIMER;

            // Il timer avvia tute le richieste al DB server
            ClientTimer.Enabled = true;

            iMAX_RECEIPT_CHARS = sGlbWinPrinterParams.bChars33 ? MAX_ABS_RECEIPT_CHARS : MAX_LEG_RECEIPT_CHARS;

            // inizializzazione delle stringhe di formattazione
            sRCP_FMT_RCPT = sGlbWinPrinterParams.bChars33 ? _RCP_FMT_33_RCPT : _RCP_FMT_28_RCPT;
            sRCP_FMT_CPY = sGlbWinPrinterParams.bChars33 ? _RCP_FMT_33_CPY : _RCP_FMT_28_CPY;
            sRCP_FMT_NOTE = sGlbWinPrinterParams.bChars33 ? _RCP_FMT_33_NOTE : _RCP_FMT_28_NOTE;
            sRCP_FMT_DSC = sGlbWinPrinterParams.bChars33 ? _RCP_FMT_33_DSC : _RCP_FMT_28_DSC;
            sRCP_FMT_DIF = sGlbWinPrinterParams.bChars33 ? _RCP_FMT_33_DIF : _RCP_FMT_28_DIF;
            sRCP_FMT_TOT = sGlbWinPrinterParams.bChars33 ? _RCP_FMT_33_TOT : _RCP_FMT_28_TOT;
            sRCP_FMT_DSH = sGlbWinPrinterParams.bChars33 ? _RCP_FMT_33_DSH : _RCP_FMT_28_DSH;

            _rVisOrdiniDlg = new VisOrdiniDlg(getActualDate(), VisOrdiniDlg.MAX_NUM_TICKET, "", VIEW_TYPE.NO_VIEW);

            // prima lettura DB
            mainTimerLoop_Tick(this, null);

            ME_TickNum.Text = iGlbNumOfTickets.ToString();

            aggiornaAspettoControlli();

            _iElencoOrdiniNoPrint.Clear();

            TB_Tickets.ReadOnly = true;
            TB_Messaggi.ReadOnly = true;

            LogToFile("FrmMain : Init");
        }

        private void BtnOnline_Click(object sender, EventArgs e)
        {
            String sTmp;
            DialogResult dResult = DialogResult.No;

            // in modo esperto non c'è il MessageBox
            if (_bOnLine && !MnuEsperto.Checked)
            {
                sTmp = "Sicuro di voler andare OFFLINE ?\n\nPotresti dover ristampare manualmente alcuni scontrini!";
                dResult = MessageBox.Show(sTmp, "Attenzione !", MessageBoxButtons.YesNo);
            }

            if ((dResult == DialogResult.Yes) || !_bOnLine || MnuEsperto.Checked)
            {
                _bOnLine = !_bOnLine;

                // allineamento
                iGlbCurrentOffline_TicketNum = iGlbNumOfTickets;
                _iPrevShownOnline_TicketNum = iGlbNumOfTickets;

                VisualizzaTicket(iGlbNumOfTickets);
            }

            if (!_bOnLine)
                ME_TickNum.Focus();

            aggiornaAspettoControlli();
        }

        void aggiornaAspettoControlli()
        {
            if (_bOnLine)
            {
                BtnPrevTicket.Enabled = false;
                BtnNextTicket.Enabled = false;

                BtnPrevMsg.Enabled = false;
                BtnNextMsg.Enabled = false;

                BtnPrintTicket.Enabled = false;
                BtnPrintMsg.Enabled = false;

                ME_TickNum.Enabled = false;

                BtnOnline.Text = "modo MANUALE";
                LblStatus.Text = "Stato : AUTO";

                checkBoxSkipPrinted.Enabled = false;
                label2.Enabled = false;
                label3.Enabled = false;
            }
            else
            {
                BtnPrevTicket.Enabled = true;
                BtnNextTicket.Enabled = true;

                BtnPrevMsg.Enabled = true;
                BtnNextMsg.Enabled = true;

                BtnPrintTicket.Enabled = true;
                BtnPrintMsg.Enabled = true;

                ME_TickNum.Enabled = true;

                BtnOnline.Text = "modo AUTO";
                LblStatus.Text = "Stato : MANUALE";

                checkBoxSkipPrinted.Enabled = true;
                label2.Enabled = true;
                label3.Enabled = true;
            }

            ME_TickNum.Text = iGlbCurrentOffline_TicketNum.ToString();
        }

        private void BtnPrevTicket_Click(object sender, EventArgs e)
        {
            bool bThereIsSomethingToPrint = false;
            int iDebug1, iDebug2; ;

            if ((iGlbCurrentOffline_TicketNum - 1) < DB_Data.iStartingNumOfReceipts)
            {
                if (iGlbNumOfTickets > 0)
                    iGlbCurrentOffline_TicketNum = DB_Data.iStartingNumOfReceipts;
                else
                    iGlbCurrentOffline_TicketNum = DB_Data.iStartingNumOfReceipts - 1;
            }
            else
            {
                do
                {
                    iGlbCurrentOffline_TicketNum--;
                    iDebug1 = iGlbCurrentOffline_TicketNum;

                    bThereIsSomethingToPrint = false;
                    _rdBaseIntf.dbCaricaOrdine(getActualDate(), iGlbCurrentOffline_TicketNum, false);
                    iDebug2 = DB_Data.iStatusReceipt;

                    CheckSomethingToPrint(_bSomethingInto_GrpToPrint, _bSomethingInto_ClrToPrint, DB_Data);

                    for (int i = 0; i < NUM_EDIT_GROUPS; i++)
                    {
                        if (_bSomethingInto_GrpToPrint[i] && bGetCopiaGroup(i))
                        {
                            bThereIsSomethingToPrint = true;
                            break;
                        }
                    }

                    if (_bSomethingInto_GrpToPrint[(int)DEST_TYPE.DEST_COUNTER] && bGetCopiaGroup((int)DEST_TYPE.DEST_TIPO1))
                        bThereIsSomethingToPrint = true;

                    // aggiorna l'elenco di ordini per i quali non è richiesta la stampa
                    if (!bThereIsSomethingToPrint && !_iElencoOrdiniNoPrint.Contains(iGlbCurrentOffline_TicketNum))
                        _iElencoOrdiniNoPrint.Add(iGlbCurrentOffline_TicketNum);

                    iDebug2 = DB_Data.iStatusReceipt;
                }
                while (checkBoxSkipPrinted.Checked && (iGlbCurrentOffline_TicketNum > 1) &&
                    (IsBitSet(DB_Data.iStatusReceipt, BIT_RECEIPT_STAMPATO_DA_STANDCUCINA)) || DB_Data.bAnnullato || !bThereIsSomethingToPrint);
            }

            ME_TickNum.Text = iGlbCurrentOffline_TicketNum.ToString();
            ME_TickNum.Focus();

            _iNextTicketNum = _rdBaseIntf.dbGetNumOfPrintedOrders(iGlbCurrentOffline_TicketNum, true);
            _iPrevTicketNum = _rdBaseIntf.dbGetNumOfPrintedOrders(iGlbCurrentOffline_TicketNum, false);

            correggiNumeroOrdiniDaStampare();

            iGlbNumOfTickets = _rdBaseIntf.dbGetNumOfOrdersFromDB(false);
            VisualizzaTicket(iGlbCurrentOffline_TicketNum, _iPrevTicketNum, _iNextTicketNum);
        }

        private void BtnNextTicket_Click(object sender, EventArgs e)
        {
            bool bThereIsSomethingToPrint = false;
            int iDebug;

            if ((iGlbCurrentOffline_TicketNum + 1) > iGlbNumOfTickets)
                iGlbCurrentOffline_TicketNum = iGlbNumOfTickets;
            else
            {
                do
                {
                    iGlbCurrentOffline_TicketNum++;
                    iDebug = iGlbCurrentOffline_TicketNum;

                    bThereIsSomethingToPrint = false;
                    _rdBaseIntf.dbCaricaOrdine(getActualDate(), iGlbCurrentOffline_TicketNum, false);
                    CheckSomethingToPrint(_bSomethingInto_GrpToPrint, _bSomethingInto_ClrToPrint, DB_Data);

                    for (int i = 0; i < NUM_EDIT_GROUPS; i++)
                    {
                        if (_bSomethingInto_GrpToPrint[i] && bGetCopiaGroup(i))
                        {
                            bThereIsSomethingToPrint = true;
                            break;
                        }
                    }

                    if (_bSomethingInto_GrpToPrint[(int)DEST_TYPE.DEST_COUNTER] && bGetCopiaGroup((int)DEST_TYPE.DEST_TIPO1))
                        bThereIsSomethingToPrint = true;

                    // aggiorna l'elenco di ordini per i quali non è richiesta la stampa
                    if (!bThereIsSomethingToPrint && !_iElencoOrdiniNoPrint.Contains(iGlbCurrentOffline_TicketNum))
                        _iElencoOrdiniNoPrint.Add(iGlbCurrentOffline_TicketNum);

                    iDebug = DB_Data.iStatusReceipt;
                }
                while (checkBoxSkipPrinted.Checked && (iGlbCurrentOffline_TicketNum < iGlbNumOfTickets) &&
                    (IsBitSet(DB_Data.iStatusReceipt, BIT_RECEIPT_STAMPATO_DA_STANDCUCINA)) || DB_Data.bAnnullato || !bThereIsSomethingToPrint);
            }

            ME_TickNum.Text = iGlbCurrentOffline_TicketNum.ToString();
            ME_TickNum.Focus();

            _iNextTicketNum = _rdBaseIntf.dbGetNumOfPrintedOrders(iGlbCurrentOffline_TicketNum, true);
            _iPrevTicketNum = _rdBaseIntf.dbGetNumOfPrintedOrders(iGlbCurrentOffline_TicketNum, false);

            correggiNumeroOrdiniDaStampare();

            iGlbNumOfTickets = _rdBaseIntf.dbGetNumOfOrdersFromDB(false);
            VisualizzaTicket(iGlbCurrentOffline_TicketNum, _iPrevTicketNum, _iNextTicketNum);
        }

        private void BtnPrevMsg_Click(object sender, EventArgs e)
        {
            if ((iGlbCurrentOffline_MessageNum - 1) < 1)
            {
                if (iGlbNumOfMessages > 0)
                    iGlbCurrentOffline_MessageNum = 1;
                else
                    iGlbCurrentOffline_MessageNum = 0;
            }
            else
                iGlbCurrentOffline_MessageNum--;

            iGlbNumOfMessages = _rdBaseIntf.dbGetNumOfMessagesFromDB(false);
            VisualizzaMsg(iGlbCurrentOffline_MessageNum);
        }

        private void BtnNextMsg_Click(object sender, EventArgs e)
        {
            if ((iGlbCurrentOffline_MessageNum + 1) > iGlbNumOfMessages) // Nuovo Messaggio
                iGlbCurrentOffline_MessageNum = iGlbNumOfMessages;
            else
                iGlbCurrentOffline_MessageNum++;

            iGlbNumOfMessages = _rdBaseIntf.dbGetNumOfMessagesFromDB(false);
            VisualizzaMsg(iGlbCurrentOffline_MessageNum);
        }

        private void BtnPrintTicket_Click(object sender, EventArgs e)
        {
            int iDebug;
            String sTmp;
            DialogResult dResult = DialogResult.No;

            iDebug = DB_Data.iStatusReceipt;

            if (IsBitSet(DB_Data.iStatusReceipt, BIT_RECEIPT_STAMPATO_DA_STANDCUCINA))
            {
                sTmp = "Copia cucina già stampata !\n\nSei sicuro di voler ripetere la stampa?";
                dResult = MessageBox.Show(sTmp, "Attenzione !", MessageBoxButtons.YesNo);

                if (dResult == DialogResult.Yes)
                {
                    stampaCopie();
                }
            }
            else
            {
                stampaCopie();

                // aggiorna il flag BIT_TICKET_STAMPATO_DA_STANDCUCINA per contrassegnare la stampa avvenuta
                _rdBaseIntf.dbEditStatus(iGlbCurrentOffline_TicketNum, SetBit(DB_Data.iStatusReceipt, BIT_RECEIPT_STAMPATO_DA_STANDCUCINA));

                TB_Tickets.BackColor = System.Drawing.Color.Gold;
                TB_Tickets.ForeColor = System.Drawing.Color.Black;

                _iNextTicketNum--; // sottrae 1 a _iNextTicketNum
                // VisualizzaTicket(iGlbCurrentOffline_TicketNum, _iPrevTicketNum, _iNextTicketNum);

                toolStripCurrTicketNum.Text = String.Format("Scontrino num : {0}, Prec.={1}, Succ.={2}", iGlbCurrentOffline_TicketNum, _iPrevTicketNum, _iNextTicketNum);
            }
        }

        private void BtnPrintMsg_Click(object sender, EventArgs e)
        {
            VisualizzaMsg(iGlbCurrentOffline_MessageNum);

            stampaMessaggio();

            // aggiorna il flag BIT_MSG_STAMPATO_DA_STANDCUCINA per contrassegnare la stampa avvenuta
            _rdBaseIntf.dbEditStatus(-iGlbCurrentOffline_MessageNum, SetBit(DB_Data.iStatusReceipt, BIT_MSG_STAMPATO_DA_STANDCUCINA));

            TB_Messaggi.BackColor = System.Drawing.Color.Gold;
            TB_Messaggi.ForeColor = System.Drawing.Color.Black;
        }

        private void btnAnt_Click(object sender, EventArgs e)
        {
            String sTmp;

            ulStart = (ulong)Environment.TickCount;

            _rdBaseIntf.db_FeedbackCheck();

            ulStop = (ulong)Environment.TickCount;
            ulPingTime = ulStop - ulStart;
            sTmp = String.Format(" {0} ms", ulPingTime);

            Label_ServerName.Text = sGetDB_ServerName() + sTmp;
        }

        private void timer_ImgLoop(object sender, EventArgs e)
        {
            String[] sEvQueueObj;

            /**************************************
             *         indicazione Led COM
             **************************************/
            if (_iWP_Delay > 0)
            {
                _iWP_Delay--;

                if (_iWP_Delay == 0)
                    _iLEDstatus = (int)COM_STATUS.COM_FREE;
            }

            if (_iLEDstatus == (int)COM_STATUS.COM_BUSY)
                printerPicBox.Image = Properties.Resources.circleRed; // Red
            else if (_iLEDstatus == (int)COM_STATUS.COM_FREE)
                printerPicBox.Image = Properties.Resources.circleGreen; // Green
            else
                printerPicBox.Image = Properties.Resources.circlePink; // Grey

            /**************************************
             *  Blink Antenna di connessione DB
             **************************************/
            if (_rdBaseIntf.bGetDB_Connected())
            {

                switch (iTimerBmpTag)
                {
                    case 0:
                        btnAnt.Image = Properties.Resources.ant_b1; // celeste piccola
                        iTimerBmpTag++;
                        break;
                    case 3:
                        btnAnt.Image = Properties.Resources.ant_b2; // celeste media
                        iTimerBmpTag++;
                        break;
                    case 6:
                        btnAnt.Image = Properties.Resources.ant_b3; // celeste grande
                        iTimerBmpTag++;
                        break;
                    default:
                        iTimerBmpTag++;
                        if (iTimerBmpTag > 9)
                        {
                            btnAnt.Image = Properties.Resources.ant_g2; // celeste piccola
                            iTimerBmpTag = 10;
                        }
                        break;
                }
            }
            else
            {
                btnAnt.Image = Properties.Resources.ant_r2; // rossa piccola
                iTimerBmpTag = 10;
            }

            /***********************************
             *      gestione coda eventi
             ***********************************/
            while (eventQueue.Count > 0)
            {
                sEvQueueObj = (String[])eventQueue.Dequeue();

                if (sEvQueueObj[0] == UPDATE_DB_LABEL_EVENT)
                {
                    Label_ServerName.Text = sEvQueueObj[1];
                }
                else if (sEvQueueObj[0] == UPDATE_COM_LED_EVENT)
                {
                    _iLEDstatus = Convert.ToInt32(sEvQueueObj[1]);
                }
            }

            // rimette a posto
            if (rLogForm.Visible)
                MnuVisLog.Checked = true;
            else
                MnuVisLog.Checked = false;
        }

        /// <summary>avvio timer</summary>
        public void StartBmpTimer()
        {
            iTimerBmpTag = 0;
        }

        /// <summary>
        /// Protezione contro le modifiche non volute
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

                if (MnuEsperto.Checked)
                {
                    MnuNetConfig.Enabled = true;
                    MnuConfigurazioneStampe.Enabled = true;
                }
                else
                {
                    MnuNetConfig.Enabled = false;
                    MnuConfigurazioneStampe.Enabled = false;
                }

                sTmp = String.Format("FrmMain: Modo Esperto {0}", MnuEsperto.Checked);
                LogToFile(sTmp);
            }
        }

        private void FormClose_Click(object sender, EventArgs e)
        {
            LogToFile("Mainform : uscita");
            Close();
        }

        private void MnuStampaFile_Click(object sender, EventArgs e)
        {
            string sFileToPrint;

            openFileDialog.Filter = "Files di testo (*.txt)|*.TXT";
            openFileDialog.FileName = "";
            openFileDialog.InitialDirectory = ".\\";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                sFileToPrint = openFileDialog.FileName;

                GenPrintFile(sFileToPrint);
            }
        }

        private void MnuPrintTest_Click(object sender, EventArgs e)
        {
            String sFileToPrint;

            sFileToPrint = buildSampleText();

            LogToFile(String.Format("Mainform : printSampleText() {0}", sFileToPrint));

            GenPrintFile(sFileToPrint);
        }

        private void MnuVisLog_Click(object sender, EventArgs e)
        {
            if (rLogForm.Visible)
            {
                rLogForm.Hide();
                MnuVisLog.Checked = false;
            }
            else
            {
                rLogForm.Show();
                MnuVisLog.Checked = true;
            }
        }

        private void mnuConfigurazioneStampeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintConfigLightDlg.rPrintConfigLightDlg.Init(true);
        }

        private void MnuNetConfig_Click(object sender, EventArgs e)
        {
            _rdBaseIntf.dbCaricaOrdine(getActualDate(), 0, false);

            rNetConfigLightDlg.ShowDialog();
        }

        private void MnuAbout_Click(object sender, EventArgs e)
        {
            InfoDlg rInfoDlg = new InfoDlg();
            rInfoDlg.ShowDialog();
        }

        private void mainTimerLoop_Tick(object sender, EventArgs e)
        {
            int iDebug;
            String sTmp, sTime;

            // if (!rNetConfigDlg.Visible && _bOnLine && _rdBaseIntf.bGetDB_Connected())
            if (!rNetConfigLightDlg.Visible && _rdBaseIntf.bGetDB_Connected())
            {
                StartBmpTimer(); // aggiorna bmp trasmissione

                // carica iGlbNumOfTickets, iGlbNumOfMessages, _Versione, _Header, _HeaderText
                if (!rNetConfigLightDlg.Visible)
                {
                    _rdBaseIntf.dbCheckStatus();

                    iGlbNumOfTickets = _rdBaseIntf.dbGetNumOfOrdersFromDB(false);
                    iGlbNumOfMessages = _rdBaseIntf.dbGetNumOfMessagesFromDB(false);

                    _iNextTicketNum = _rdBaseIntf.dbGetNumOfPrintedOrders(iGlbCurrentOffline_TicketNum, true);
                    _iPrevTicketNum = _rdBaseIntf.dbGetNumOfPrintedOrders(iGlbCurrentOffline_TicketNum, false);

                    correggiNumeroOrdiniDaStampare();

                    //if (_bOnLine)
                    //    toolStripCurrTicketNum.Text = String.Format("Ticket num : {0}", iGlbCurrentOffline_TicketNum);
                    //else
                    //    toolStripCurrTicketNum.Text = String.Format("Ticket num : {0}, Prec.={1}, Succ.={2}", iGlbNumOfTickets, _iPrevTicketNum, _iNextTicketNum);

                    toolStripTotTicketNum.Text = String.Format("Presenti : {0}", iGlbNumOfTickets);

                    iDebug = ClientTimer.Interval;
                }

                iDebug = iGlbNumOfTickets;

                // c'è la connessione ma non i dati = StandFacile da avviare
                if (iGlbNumOfTickets == 0)
                    _bInitNetReadParams = true;

                // _bInitNetReadParams è true anche in fase di avvio
                if (_bInitNetReadParams)
                {
                    rNetConfigLightDlg.NetConfig_ReadParams();
                    _bInitNetReadParams = false;
                }

                /****************************************
                 *      controllo emissione Ticket
                 ****************************************/
                if ((_iPrevShownOnline_TicketNum != iGlbNumOfTickets) && (iGlbNumOfTickets > 0))
                {
                    if (_iPrevShownOnline_TicketNum == -100) // skip la prima volta
                    {
                        // Init
                        bReset_StatusDate_Changed();

                        iGlbCurrentOffline_TicketNum = iGlbNumOfTickets;
                        _iPrevShownOnline_TicketNum = iGlbNumOfTickets;

                        VisualizzaTicket(iGlbNumOfTickets);
                    }
                    else if (_bOnLine || (_iPrevShownOnline_TicketNum == 0)) // Visualizza almeno _iPrevShownOnline_TicketNum = 1
                    {
                        if (!bGet_StatusDate_IsChanged())
                            _iPrevShownOnline_TicketNum++;                  // così li stampa tutti
                        else
                            _iPrevShownOnline_TicketNum = iGlbNumOfTickets; // evita raffica di scontrini

                        VisualizzaTicket(_iPrevShownOnline_TicketNum);

                        ClientTimer.Interval = DB_CLIENT_TIMER_SHORT; // accelera

                        if (!bGet_StatusDate_IsChanged() && _bOnLine)
                        {
                            // *** stampa solo le copie e non lo scontrino ***
                            if ((_sNomeFileTicket != NOME_FILE_RECEIPT) && !bGetStampaSoloManuale() && !DB_Data.bAnnullato && !IsBitSet(DB_Data.iStatusReceipt, BIT_RECEIPT_STAMPATO_DA_STANDCUCINA))
                            {
                                stampaCopie();

                                // aggiorna il flag BIT_RECEIPT_STAMPATO_DA_STANDCUCINA per contrassegnare la stampa avvenuta
                                _rdBaseIntf.dbEditStatus(_iPrevShownOnline_TicketNum, SetBit(DB_Data.iStatusReceipt, BIT_RECEIPT_STAMPATO_DA_STANDCUCINA));

                                TB_Tickets.BackColor = System.Drawing.Color.Gold;
                                TB_Tickets.ForeColor = System.Drawing.Color.Black;
                            }
                            else // visualizza soltanto
                            {
                                // aggiorna l'elenco di ordini per i quali non è richiesta la stampa
                                if (!IsBitSet(DB_Data.iStatusReceipt, BIT_RECEIPT_STAMPATO_DA_STANDCUCINA))
                                    if (!_iElencoOrdiniNoPrint.Contains(_iPrevShownOnline_TicketNum))
                                        _iElencoOrdiniNoPrint.Add(_iPrevShownOnline_TicketNum);

                                sTime = DateTime.Now.ToString("HH.mm.ss");
                                sTmp = String.Format("{0} DB_Client : {1}, C = {2}, N. = {3}", sTime, " - - - ", DB_Data.iNumCassa, _iPrevShownOnline_TicketNum);
                                rLogForm.LogAddLine(sTmp);

                                sTmp = String.Format("DB_Client : {0}, C = {1}, N. = {2}, ", " - - - ", DB_Data.iNumCassa, _iPrevShownOnline_TicketNum);
                                LogToFile(sTmp);

                                ClientTimer.Interval = DB_CLIENT_TIMER_SHORT / 2; // accelera ancora di più se non stampa
                            }
                        }
                    }
                }
                else
                {
                    ClientTimer.Interval = DB_CLIENT_TIMER; // siamo in passo: intervallo standard 16s
                }

                if (_bOnLine)
                    ME_TickNum.Text = _iPrevShownOnline_TicketNum.ToString();

                /****************************************
                 *    controllo emissione messaggi
                 ****************************************/
                if (_iPrevShownOnline_MessageNum != iGlbNumOfMessages)
                {
                    iDebug = iGlbNumOfMessages;

                    if (_iPrevShownOnline_MessageNum == -100) // skip la prima volta
                    {
                        iGlbCurrentOffline_MessageNum = iGlbNumOfMessages;
                        _iPrevShownOnline_MessageNum = iGlbNumOfMessages;

                        VisualizzaMsg(iGlbNumOfMessages);
                    }
                    else if (_bOnLine)
                    {
                        if (!bGet_StatusDate_IsChanged())
                            _iPrevShownOnline_MessageNum++; // così li stampa tutti
                        else
                            _iPrevShownOnline_MessageNum = iGlbNumOfMessages;

                        VisualizzaMsg(_iPrevShownOnline_MessageNum);

                        if (!bGet_StatusDate_IsChanged() && !bGetStampaSoloManuale() && !IsBitSet(DB_Data.iStatusReceipt, BIT_MSG_STAMPATO_DA_STANDCUCINA))
                        {
                            stampaMessaggio();

                            // aggiorna il flag BIT_MSG_STAMPATO_DA_STANDCUCINA per contrassegnare la stampa avvenuta
                            _rdBaseIntf.dbEditStatus(-_iPrevShownOnline_MessageNum, SetBit(DB_Data.iStatusReceipt, BIT_MSG_STAMPATO_DA_STANDCUCINA));

                            TB_Messaggi.BackColor = System.Drawing.Color.Gold;
                            TB_Messaggi.ForeColor = System.Drawing.Color.Black;
                        }

                        // *** alla fine c'è il reset cambio data ***
                        bReset_StatusDate_Changed();

                        sTime = DateTime.Now.ToString("HH.mm.ss");
                        sTmp = String.Format("{0} DB_Client : C = {1}, Msg N. = {2}", sTime, DB_Data.iNumCassa, _iPrevShownOnline_MessageNum);
                        rLogForm.LogAddLine(sTmp);

                        sTmp = String.Format("DB_Client : C = {0}, Msg N. = {1}", DB_Data.iNumCassa, _iPrevShownOnline_MessageNum);
                        LogToFile(sTmp);
                    }
                }
            }
            else
            {
                // al tick successivo troverà la connessione attiva!
                if (!rNetConfigLightDlg.Visible)
                    _rdBaseIntf.db_SilentCheck();

                ClientTimer.Interval = DB_CLIENT_TIMER_SHORT; // accelera
            }
        }

        /// <summary>Invio copie alla Stampante</summary>
        void stampaCopie()
        {
            int i;
            String sTmp, sTime, sFileToPrint;

            for (i = 0; i < NUM_EDIT_GROUPS; i++)
                if (_bCopyToPrint[i])
                {
                    sFileToPrint = String.Format(NOME_FILE_COPIE, i);
                    if (File.Exists(sFileToPrint) && !bCheckService(_SKIP_STAMPA))
                    {
                        Console.Beep();

                        // Invio alla Stampante
                        LogToFile("ClientTimer : stampa di " + sFileToPrint);

                        if (iSysPrinterType == (int)PRINTER_SEL.STAMPANTE_WINDOWS)
                        {
                            _iWP_Delay = LED_TIMER;
                            printerPicBox.Image = Properties.Resources.circleRed; // Red
                            _iLEDstatus = (int)COM_STATUS.COM_BUSY;

                            printerPicBox.Refresh();
                        }

                        GenPrintFile(sFileToPrint);

                        sTime = DateTime.Now.ToString("HH.mm.ss");
                        sTmp = String.Format("{0} DB_Client : {1}, C = {2}, N. = {3}", sTime, sConstGruppi[i], DB_Data.iNumCassa, _iPrevShownOnline_TicketNum);
                        rLogForm.LogAddLine(sTmp);

                        sTmp = String.Format("DB_Client : {0}, C = {1}, N. = {2}", sConstGruppi[i], DB_Data.iNumCassa, _iPrevShownOnline_TicketNum);
                        LogToFile(sTmp);
                    }
                }
        }

        /// <summary>Invio messaggio alla Stampante</summary>
        void stampaMessaggio()
        {
            String sFileToPrint;

            sFileToPrint = String.Format(NOME_FILE_MESSAGGIO);
            if (File.Exists(sFileToPrint) && !bCheckService(_SKIP_STAMPA) && !bGet_StatusDate_IsChanged())
            {
                Console.Beep();

                // Invio alla Stampante
                LogToFile("ClientTimer : stampa di " + sFileToPrint);

                GenPrintFile(sFileToPrint);
            }
        }

        /// <summary>Visualizza lo Scontrino appoggiandosi a VisOrdiniDlg.ReceiptRebuild()</summary>
        public void VisualizzaTicket(int iTicketNumParam, int iPrevParam = -1, int iNextParam = -1)
        {
            int i;
            StreamReader fTxtFile;
            String sInStr, sTmp, sDir;

            FrmMain_FileCleaning();

            // mantiene il filtraggio entro i limiti
            if (iTicketNumParam < 1)
                iTicketNumParam = 1;
            else
            {
                if (iTicketNumParam > iGlbNumOfTickets)
                    iTicketNumParam = iGlbNumOfTickets;
            }

            ulStart = (ulong)Environment.TickCount;

            if (_rdBaseIntf.dbCaricaOrdine(getActualDate(), iTicketNumParam, false))
                _rVisOrdiniDlg.ReceiptRebuild(getActualDate(), iTicketNumParam); // sola chiamata

            CheckSomethingToPrint(_bSomethingInto_GrpToPrint, _bSomethingInto_ClrToPrint, DB_Data);

            // ci devono essere sia i checkBox selezionati che qualcosa da stampare
            // dei gruppi selezionati, contatori qui esclusi
            for (i = 0; i < NUM_EDIT_GROUPS; i++) // OK
                _bCopyToPrint[i] = _bSomethingInto_ClrToPrint[i] && bGetCopiaGroup(i);

            // consente stampa contatori assieme alle pietanze
            if (_bSomethingInto_GrpToPrint[(int)DEST_TYPE.DEST_COUNTER] && bGetCopiaGroup((int)DEST_TYPE.DEST_TIPO1))
                _bCopyToPrint[(int)DEST_TYPE.DEST_TIPO1] = true;

            _sNomeFileTicket = NOME_FILE_RECEIPT;

            /***************************************************************************
            * esce alla prima occorrenza della copia, quindi verrà visualizzato 
            * solo questo file ma stampati tutti
            ***************************************************************************/
            for (i = 0; i < NUM_EDIT_GROUPS; i++)
            {
                if (_bCopyToPrint[i])
                {
                    _sNomeFileTicket = String.Format(NOME_FILE_COPIE, i);
                    break;
                }
            }

            // misura del tempo in ms per leggere la tabella ClientDS_Ordini
            ulStop = (ulong)Environment.TickCount;
            ulPingTime = ulStop - ulStart;
            sTmp = String.Format(" {0} ms", ulPingTime);

            Label_ServerName.Text = sGetDB_ServerName() + sTmp;

            /********************************
             *	visualizzazione del file
             ********************************/

            TB_Tickets.Clear();
            TB_Tickets.BackColor = System.Drawing.Color.Teal;
            TB_Tickets.Refresh();

            // gestione colori
            if (DB_Data.bAnnullato)
            {
                TB_Tickets.BackColor = System.Drawing.Color.Red;
                TB_Tickets.ForeColor = System.Drawing.SystemColors.Window;
            }
            else if (IsBitSet(DB_Data.iStatusReceipt, BIT_RECEIPT_STAMPATO_DA_STANDCUCINA))
            {
                TB_Tickets.BackColor = System.Drawing.Color.Gold;
                TB_Tickets.ForeColor = System.Drawing.Color.Black;
            }
            else if (_sNomeFileTicket == NOME_FILE_RECEIPT) // non c'è contenuto significativo
            {
                TB_Tickets.BackColor = System.Drawing.Color.Gray;
                TB_Tickets.ForeColor = System.Drawing.SystemColors.Window;
            }
            else
            {
                TB_Tickets.BackColor = System.Drawing.Color.Teal;
                TB_Tickets.ForeColor = System.Drawing.SystemColors.Window;
            }

            sDir = sRootDir + "\\";

            if (File.Exists(sDir + _sNomeFileTicket))
            {
                fTxtFile = File.OpenText(sDir + _sNomeFileTicket);

                while ((sInStr = fTxtFile.ReadLine()) != null)
                {
                    TB_Tickets.AppendText(sInStr + "\r\n");
                }

                fTxtFile.Close();

            }
            else
            {
                TB_Tickets.BackColor = System.Drawing.Color.Black;
                TB_Tickets.AppendText("\r\nScontrino non trovato !");
            }

            if (iPrevParam != -1)
                toolStripCurrTicketNum.Text = String.Format("Scontrino num : {0}, Prec.={1}, Succ.={2}", iTicketNumParam, iPrevParam, iNextParam);
            else
                toolStripCurrTicketNum.Text = String.Format("Scontrino num : {0}", iTicketNumParam);

            toolStripTotTicketNum.Text = String.Format("Presenti : {0}", iGlbNumOfTickets);

            //TB_Tickets.ScrollBars = ScrollBars.Vertical;
            TB_Tickets.SelectionStart = 0;
            TB_Tickets.ScrollToCaret();
        }

        /// <summary>Visualizza messaggio</summary>
        public void VisualizzaMsg(int iNum)
        {
            int i, iEqRowsNumber;
            String sLineOfText;
            StreamWriter fTxtFile;

            if (iNum < 1)
                iNum = 1;
            else
            {
                if (iNum > iGlbNumOfMessages)
                    iNum = iGlbNumOfMessages;
            }

            _rdBaseIntf.dbCaricaMessaggio(iNum, false);

            /***********************************
             *	visualizzazione del Messaggio
             ***********************************/

            TB_Messaggi.Clear();
            TB_Messaggi.BackColor = System.Drawing.Color.Teal;

            if (IsBitSet(DB_Data.iStatusReceipt, BIT_MSG_STAMPATO_DA_STANDCUCINA))
            {
                TB_Messaggi.BackColor = System.Drawing.Color.Gold;
                TB_Messaggi.ForeColor = System.Drawing.Color.Black;
            }
            else
            {
                TB_Messaggi.BackColor = System.Drawing.Color.Teal;
                TB_Messaggi.ForeColor = System.Drawing.SystemColors.Window;
            }

            TB_Messaggi.Text = DB_Data.sMessaggio.Replace("\n", "\r\n");

            /***********************************
             *	salva il Messaggio su file
             ***********************************/
            iEqRowsNumber = 1; // riga di partenza

            fTxtFile = File.CreateText(NOME_FILE_MESSAGGIO);

            if (fTxtFile != null)
            {
                for (i = 0; i < TB_Messaggi.Lines.Length; i++)
                {
                    sLineOfText = TB_Messaggi.Lines[i];
                    fTxtFile.WriteLine("{0}", sLineOfText);

                    iEqRowsNumber++;
                }

                do
                {
                    fTxtFile.WriteLine();
                    iEqRowsNumber++;
                }
                while (iEqRowsNumber < MIN_ROWS_NUMBER);

                fTxtFile.Close();
            }
            else
            {
                TB_Messaggi.BackColor = System.Drawing.Color.Black;
                TB_Messaggi.AppendText("\r\nMessaggio non trovato !");
            }

            toolStripCurrMsgNum.Text = String.Format("Messaggio num : {0}", iNum);
            toolStripTotMsgNum.Text = String.Format("Presenti : {0}", iGlbNumOfMessages);

            // TB_Messaggi.ScrollBars = ScrollBars.Vertical;
            TB_Messaggi.SelectionStart = 0;
            TB_Messaggi.ScrollToCaret();
        }

        // accetta solo numeri, backspace
        private void ME_TickNum_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (!(char.IsDigit(e.KeyChar) || (e.KeyChar == '\b')))
            {
                e.Handled = true;
                return;
            }
        }

        private void MnuManuale_Click(object sender, EventArgs e)
        {
            QuickHelpDlg rQuickHelpDlg = new QuickHelpDlg();

            rQuickHelpDlg.Dispose();
        }

        // accetta i testi funzione etc...
        private void ME_TickNum_KeyDown(object sender, KeyEventArgs e)
        {
            int iKey;
            String sTmpString;

            iKey = (int)e.KeyValue;

            switch (iKey)
            {
                case KEY_ENTER:
                    sTmpString = ME_TickNum.Text.Trim();
                    sTmpString = sTmpString.Trim();

                    if (Convert.ToInt32(sTmpString) <= iGlbNumOfTickets)
                        iGlbCurrentOffline_TicketNum = Convert.ToInt32(sTmpString);
                    break;

                case KEY_F2:
                    sTmpString = ME_TickNum.Text;
                    sTmpString = sTmpString.Trim();

                    if (Convert.ToInt32(sTmpString) <= iGlbNumOfTickets)
                    {
                        Console.Beep();
                        iGlbCurrentOffline_TicketNum = Convert.ToInt32(sTmpString);

                        // restartPrint così stampa l'attuale
                        _iPrevShownOnline_TicketNum = Convert.ToInt32(sTmpString) - 1;
                        ClientTimer.Interval = 1000; // 1s

                        _bOnLine = !_bOnLine;
                        aggiornaAspettoControlli();
                    }
                    break;
                case KEY_DOWN:
                case KEY_RIGHT:
                    BtnNextTicket_Click(this, null);
                    ME_TickNum.Text = iGlbCurrentOffline_TicketNum.ToString();
                    break;

                case KEY_UP:
                case KEY_LEFT:
                    BtnPrevTicket_Click(this, null);
                    ME_TickNum.Text = iGlbCurrentOffline_TicketNum.ToString();
                    break;
                case KEY_PAGEUP:
                    if ((iGlbCurrentOffline_TicketNum - 10) < DB_Data.iStartingNumOfReceipts)
                    {
                        if (iGlbNumOfTickets > 0)
                            iGlbCurrentOffline_TicketNum = DB_Data.iStartingNumOfReceipts;
                        else
                            iGlbCurrentOffline_TicketNum = DB_Data.iStartingNumOfReceipts - 1;
                    }
                    else
                        iGlbCurrentOffline_TicketNum -= 10;

                    ME_TickNum.Text = iGlbCurrentOffline_TicketNum.ToString();
                    break;

                case KEY_PAGEDOWN:
                    if ((iGlbCurrentOffline_TicketNum + 10) > iGlbNumOfTickets)
                        iGlbCurrentOffline_TicketNum = iGlbNumOfTickets;
                    else
                        iGlbCurrentOffline_TicketNum += 10;

                    ME_TickNum.Text = iGlbCurrentOffline_TicketNum.ToString();
                    break;

                case KEY_HOME:
                    if (iGlbNumOfTickets > 0)
                        iGlbCurrentOffline_TicketNum = DB_Data.iStartingNumOfReceipts;
                    else
                        iGlbCurrentOffline_TicketNum = DB_Data.iStartingNumOfReceipts - 1;

                    ME_TickNum.Text = iGlbCurrentOffline_TicketNum.ToString();
                    break;

                case KEY_END:
                    iGlbCurrentOffline_TicketNum = iGlbNumOfTickets;
                    ME_TickNum.Text = iGlbCurrentOffline_TicketNum.ToString();
                    break;
                default:
                    break;

            }

            _iNextTicketNum = _rdBaseIntf.dbGetNumOfPrintedOrders(iGlbCurrentOffline_TicketNum, true);
            _iPrevTicketNum = _rdBaseIntf.dbGetNumOfPrintedOrders(iGlbCurrentOffline_TicketNum, false);

            correggiNumeroOrdiniDaStampare();

            VisualizzaTicket(iGlbCurrentOffline_TicketNum, _iPrevTicketNum, _iNextTicketNum);
        }

        /// <summary> Pulizia files </summary>
        private void FrmMain_FileCleaning()
        {
            int i;
            String sNomeFileCopiePrt;

            if (File.Exists(NOME_FILE_RECEIPT))
                File.Delete(NOME_FILE_RECEIPT);

            for (i = 0; i < MAX_NUM_ARTICOLI - 1; i++)
            {
                sNomeFileCopiePrt = String.Format(NOME_FILE_COPIE, i);
                if (File.Exists(sNomeFileCopiePrt))
                    File.Delete(sNomeFileCopiePrt);
            }
        }

        /// <summary>
        /// analizza la lista degli ordini che non devono essere stampati,<br/>
        /// la aggiorna e corregge _iPrevTicketNum, _iNextTicketNum che altrimenti <br/>
        /// sono basati solo su BIT_RECEIPT_STAMPATO_DA_STANDCUCINA e non sul contenuto significativo
        /// </summary>
        void correggiNumeroOrdiniDaStampare()
        {
            // si tiene conto degli ordini senza stampe richieste
            foreach (int iOrdine in _iElencoOrdiniNoPrint)
            {
                if (iOrdine >= iGlbCurrentOffline_TicketNum)
                    _iNextTicketNum--;
                else if (iOrdine < iGlbCurrentOffline_TicketNum)
                    _iPrevTicketNum--;
            }
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            FrmMain_FileCleaning();

            LogToFile("Mainform : uscita");

            StopPrintServer();
            StopLogServer(); // deve stare per ultimo
        }

    }
}


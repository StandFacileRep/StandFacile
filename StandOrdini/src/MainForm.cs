/*******************************************************************
  	NomeFile : StandOrdini/MainForm.cs
	Data	 : 06.12.2024
  	Autore   : Mauro Artuso

  Programma per visualizzare in un monitor gli scontrini serviti
  ed eseguire lo scarico degli ordini consegnati dal database
  contrassegnando il campo "iScaricato"
 *******************************************************************/

using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Windows.Forms;
using static StandCommonFiles.ComDef;
using static StandCommonFiles.CommonCl;
using static StandCommonFiles.LogServer;
using static StandFacile.dBaseIntf;
using static StandFacile.Define;
using static StandFacile.glb;
using static StandFacile.NetConfigLightDlg;

namespace StandFacile
{
    /// <summary>
    /// main form
    /// </summary>
    public partial class FrmMain : Form
    {
#pragma warning disable IDE0059

        bool _bCrtlKeyPressed = false;
        bool _bShiftKeyPressed = false;
        static bool _bInitNetReadParams = true;

        /// <summary>riferimento a FrmMain</summary>
        public static FrmMain rFrmMain;

        // variabili membro
        static int iBlink, iScaricoDelayTimer, iTimerTag;
        static int _iFuncMode;

        readonly static int[] iTimeBlinkCounter = new int[4 * MAX_RIGHE]; // contatore per il termine dei lampeggi
        static TQueue_Obj sOrdiniQueueObj;

        ulong ulStart, ulStop, ulPingTime;

        readonly static String[] sTextToBlink = new String[4 * MAX_RIGHE]; // copia Stringhe originali per consentire il blink
        static String _sDataBarcode;
        static String _sOldString;

        // stringhe ausiliarie per gestione righe TextBox che altrimenti è read-only
        String[] textLine_L;
        String[] textLine_R;

        static TErrMsg _WrnMsg;

        /// <summary>costruttore</summary>
        public FrmMain()
        {
            InitializeComponent();

            // TextBox ToolTip
            ToolTip tt = new ToolTip
            {
                InitialDelay = 0,
                ShowAlways = true
            };
            tt.SetToolTip(btnAnt, "test connessione al DataBase");
            tt.SetToolTip(EditInput, "inserimento manuale del numero dello scontrino");
            tt.SetToolTip(CancelBtn, "pulisce le 2 finestre di segnalazione");
            tt.Dispose();

            rFrmMain = this;

            InitActualDate(); // deve precedere GetActualDate()

            iTimerTag = 0;
            _sDataBarcode = GetActualDate().ToString("ddMMyy");

            EditInput.Text = "";
            EditInput.MaxLength = 13; // EAN 13 (12 + checksum)

            // clear iniziale
            CancelBtn_Click(this, null);

            LabelClock.Text = "";

            Text = Define.TITLE;
            this.MinimumSize = new System.Drawing.Size(Define.MAINWD_WIDTH, Define.MAINWD_HEIGHT);

            this.Size = new System.Drawing.Size(640, 480);

            // impostazione della directory di default per operazioni sui dati
            sRootDir = Directory.GetCurrentDirectory();

            CheckMenuItems();
            FormResize(this, null);

            bWarnOnce = true;
            LogToFile("Mainform : Avvio StandOrdini");
        }

        /// <summary>Init()</summary>
        public void Init()
        {
            string sShortDBType;
            String sKeyGood;

            sKeyGood = ReadRegistry(DBASE_SERVER_NAME_KEY, "");
            _iFuncMode = ReadRegistry(KEY_FUNC_MODE, 0);

            switch (_iFuncMode)
            {
                case (int)FUNC_MODE.SCARICO_DB:
                    MnuScarico_DB_Click(this, null);
                    break;
                case (int)FUNC_MODE.SOLO_LETTURA_BC:
                    MnuSoloLettura_BC_Click(this, null);
                    break;
                case (int)FUNC_MODE.DUPLICAZIONE_MONITOR:
                    MnuDuplicazioneMonitor_Click(this, null);
                    break;
                default:
                    MnuScarico_DB_Click(this, null);
                    break;
            }

            if (String.IsNullOrEmpty(sKeyGood))
            {
                MessageBox.Show("E' la prima esecuzione, imposta la connessione al database !", "Attenzione !", MessageBoxButtons.OK);

                // Imposta il nome del server
                NetConfigLightDlg.rNetConfigLightDlg.Init(true);
            }
            else if (CheckService(CFG_COMMON_STRINGS._ESPERTO))
                MnuEspertoClick(this, null);

            iScaricoDelayTimer = 2;
            Timer.Enabled = true;

            switch (dBaseIntf.iUSA_NDB())
            {
                case (int)DB_MODE.SQLITE:
                    sShortDBType = "ql";
                    break;
                case (int)DB_MODE.MYSQL:
                    sShortDBType = "my";
                    break;
                case (int)DB_MODE.POSTGRES:
                    sShortDBType = "pg";
                    break;
                default:
                    sShortDBType = "";
                    break;
            }

            Text = String.Format("{0}   {1}", Define.TITLE, sShortDBType);

            LogToFile("FrmMain : Init");

            _rdBaseIntf.dbCaricaOrdine(GetActualDate(), 0, false);
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            int i;

            EditInput.Text = "";

            TicketsList_L.Clear();
            TicketsList_R.Clear();

            for (i = 0; i < MAX_RIGHE; i++)
            {
                TicketsList_L.AppendText(" \r\n");
                TicketsList_R.AppendText(" \r\n");
            }

            for (i = 0; i < 4 * MAX_RIGHE; i++)
            {
                sTextToBlink[i] = " ";
                iTimeBlinkCounter[i] = -1;
            }

            textLine_L = TicketsList_L.Lines;
            textLine_R = TicketsList_R.Lines;

            LogToFile("Mainform : BtnCancelClick");
        }

        private void EditInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            bool bFound = false;

            String sLog, sStrTmp, sStrBarcode, sStrNum, sStrDay, sStrGruppo;
            int i, j, iNumScontrino, iGruppo, iLength;

            /********************************************
                        ENTER senza e con Ctrl
            ********************************************/
            
            if ((e.KeyChar == '\r') || (e.KeyChar == '\n'))
            {
                // scarta edit vuoti
                if (String.IsNullOrEmpty(EditInput.Text))
                    return;
                else
                {
                    sStrBarcode = EditInput.Text;
                    sStrBarcode = sStrBarcode.Trim();

                    /* brutto ma efficace dato che la stringa EditInput.Text :
                     * se è lunga 13 contiene leading zeroes ed il checksum
                     * se è lunga 12 o meno il lettore non legge i "leading zeroes"
                     * quindi può essere 0001xxxxxxxxx ed al massimo vanno aggiunti 3 zeri !
                    */

                    // adegua leading zeroes
                    iLength = sStrBarcode.Length;

                    if (iLength == 12)
                        sStrBarcode = "0" + sStrBarcode;
                    else if (iLength == 11)
                        sStrBarcode = "00" + sStrBarcode;
                    else if (iLength == 10)
                        sStrBarcode = "000" + sStrBarcode;

                    // sparato barcode con checksum corretto
                    if (StandCommonFiles.Barcode_EAN13.VerifyChecksum(sStrBarcode))
                    {
                        // lo spazio è solo per allineamento
                        sStrNum = sStrBarcode.Substring(sStrBarcode.Length - 5, 4); // numero
                        sStrDay = sStrBarcode.Substring(2, 6);      // data
                        sStrGruppo = sStrBarcode.Substring(0, 2);   // Gruppo

                        sLog = String.Format("Mainform : barcode = {0}", sStrBarcode);
                        LogToFile(sLog);
                    }
                    else
                    {
                        sStrNum = sStrBarcode;     // numero
                        sStrDay = _sDataBarcode;   // data
                        sStrGruppo = "-1";         // Gruppo

                        sLog = String.Format("Mainform : manuale = {0}", sStrBarcode);
                        LogToFile(sLog);
                    }

                    try
                    {
                        iNumScontrino = Convert.ToInt32(sStrNum);
                        sStrNum = String.Format(" {0:d4}", iNumScontrino); // aggiunge zeri
                        iGruppo = Convert.ToInt32(sStrGruppo);
                    }

                    catch (Exception)
                    {
                        // Errore di conversione !
                        EditInput.Text = "";
                        return;
                    }

                    // scarta se la data è diversa
                    if (sStrDay != _sDataBarcode)
                    {
                        EditInput.Text = "";
                        Console.Beep();
                        return;
                    }
                    else
                        EditInput.Text = "";

                    if (_bCrtlKeyPressed)
                    {
                        bFound = false;

                        // cancellazione ordine dal monitor
                        for (j = 0; j < 4 * MAX_RIGHE; j++)
                        {
                            if (sTextToBlink[j] == sStrNum)
                            {
                                for (i = j; i < 4 * MAX_RIGHE - 1; i++)
                                {
                                    sTextToBlink[i] = sTextToBlink[i + 1];
                                    iTimeBlinkCounter[i] = iTimeBlinkCounter[i + 1];
                                }

                                sTextToBlink[4 * MAX_RIGHE - 1] = "";
                                iTimeBlinkCounter[4 * MAX_RIGHE - 1] = -1;

                                bFound = true;
                                break;
                            }
                        }

                        if (!bFound)
                        {
                            _WrnMsg.sMsg = sStrNum;
                            _WrnMsg.iErrID = WRN_TNNF;
                            WarningManager(_WrnMsg);

                            String sTmp = String.Format("dbScaricaOrdine Crtl: numero {0} non trovato!", sStrNum);
                            LogToFile(sTmp);
                        }

                        sLog = String.Format("Mainform : Shift = {0}", sStrNum);
                        LogToFile(sLog);

                        // aggiornamento stringhe textBox L & R
                        for (i = 0; i < MAX_RIGHE; i++)
                        {
                            textLine_R[i] = sTextToBlink[i + MAX_RIGHE];
                            textLine_L[i] = sTextToBlink[i];
                        }

                        // può ritornare
                        return;
                    }
                    // non visualizza numeri ripetuti
                    else if (sStrNum != sTextToBlink[0])
                    {

                        /************************************************
                         * Shif evita visualizzazione a Monitor ma 
                         * esegue le operazioni successive
                         ************************************************/
                        if (!_bShiftKeyPressed)
                        {
                            // scorrimento stringhe sTextToBlink e contatori
                            for (i = 4 * MAX_RIGHE - 1; i > 0; i--)
                            {
                                sTextToBlink[i] = sTextToBlink[i - 1];
                                iTimeBlinkCounter[i] = iTimeBlinkCounter[i - 1];
                            }

                            iTimeBlinkCounter[0] = MAX_BLINK;
                            sTextToBlink[0] = sStrNum;

                            sLog = String.Format("Mainform : sBlink = {0}", sStrNum);
                            LogToFile(sLog);
                        }
                    }

                    // aggiornamento stringhe textBox L & R
                    for (i = 0; i < MAX_RIGHE; i++)
                    {
                        textLine_R[i] = sTextToBlink[i + MAX_RIGHE];
                        textLine_L[i] = sTextToBlink[i];
                    }

                    /*******************************************
                         chiama la funzione di scarico ordine
                     *******************************************/
                    sStrTmp = String.Format("Mainform : dbScaricaOrdine = {0}", iNumScontrino);
                    LogToFile(sStrTmp);

                    if (_iFuncMode == (int)FUNC_MODE.SCARICO_DB)
                    {
                        // fondamentale instanziare un nuovo oggetto, 
                        // altrimenti l'inserimento in coda non funziona
                        sOrdiniQueueObj = new TQueue_Obj(SCARICO_DB_EVENT, iNumScontrino, iGruppo);

                        ordiniQueue.Enqueue(sOrdiniQueueObj);

                        sStrTmp = String.Format("Mainform : Enqueue = {0}", iNumScontrino);
                        Console.WriteLine(sStrTmp);

                        iScaricoDelayTimer = 2; // 0,5s
                    }

                    /*******************************************
                        chiama la funzione di sintesi vocale
                     *******************************************/
                    SpeechSynth.rSpeechSynth.TextSpeak(iNumScontrino.ToString());

                }
            }
            else
            {
                if (!(char.IsDigit(e.KeyChar) || (e.KeyChar == '\b')))
                    e.Handled = true;
            }
        }

        private void EditInput_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Modifiers == Keys.Control)
            {
                _bCrtlKeyPressed = true;
                _bShiftKeyPressed = false;

                if ((char.IsDigit((char)e.KeyValue)))
                {
                    EditInput.Text += (char)e.KeyValue;
                }

                EditInput.BackColor = System.Drawing.Color.Red;
                EditInput.ForeColor = System.Drawing.SystemColors.HighlightText;
            }
            else if (e.Modifiers == Keys.Shift)
            {
                _bCrtlKeyPressed = false;
                _bShiftKeyPressed = true;

                if ((char.IsDigit((char)e.KeyValue)))
                {
                    EditInput.Text += (char)e.KeyValue;
                }

                EditInput.BackColor = System.Drawing.SystemColors.Highlight;
                EditInput.ForeColor = System.Drawing.SystemColors.HighlightText;
            }
            else
            {
                _bCrtlKeyPressed = false;
                _bShiftKeyPressed = false;

                EditInput.BackColor = System.Drawing.SystemColors.Window;
                EditInput.ForeColor = System.Drawing.SystemColors.WindowText;
            }
        }


        /**************************************
         *             main loop 
         **************************************/
        private void Timer_MainLoop(object sender, EventArgs e)
        {
            bool bScaricoEseguito;

            int i, iNumScontrino, iGruppo, iGlbNumOfTickets;
            int iDebug;

            String sTime, sTmp, sDebug;

            sTime = DateTime.Now.ToString("HH:mm:ss");
            LabelClock.Text = sTime;

            // altrimenti è difficoltoso chiudere MsgForm
            if (!(MessageDlg.rMessageDlg.Visible || SpeechSynth.rSpeechSynth.Visible))
                EditInput.Focus();

            // Lampeggio L
            for (i = 0; i < MAX_RIGHE; i++)
            {
                if (iTimeBlinkCounter[i] > 0) // lampeggio
                {
                    if (iTimerTag > 2)
                    {
                        if (textLine_L[i] != sTextToBlink[i])
                        {
                            textLine_L[i] = sTextToBlink[i];
                        }
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(textLine_L[i]))
                        {
                            textLine_L[i] = "";
                        }
                    }

                    iTimeBlinkCounter[i]--;
                }
                else if (iTimeBlinkCounter[i] == 0)
                {
                    // stabilizza l'acceso mettendo il contatore a -1
                    if (textLine_L[i] != sTextToBlink[i])
                    {
                        textLine_L[i] = sTextToBlink[i];
                    }

                    iTimeBlinkCounter[i]--;
                }
            }

            // Lampeggio R
            for (i = MAX_RIGHE; i < 2 * MAX_RIGHE; i++)
            {
                if (iTimeBlinkCounter[i] > 0) // lampeggio
                {
                    if (iTimerTag > 2)
                    {
                        if (textLine_R[i - MAX_RIGHE] != sTextToBlink[i])
                        {
                            textLine_R[i - MAX_RIGHE] = sTextToBlink[i];
                        }
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(textLine_R[i - MAX_RIGHE]))
                        {
                            textLine_R[i - MAX_RIGHE] = "";
                        }
                    }

                    iTimeBlinkCounter[i]--;
                }
                else if (iTimeBlinkCounter[i] == 0)
                {
                    if (textLine_R[i - MAX_RIGHE] != sTextToBlink[i])
                    {
                        textLine_R[i - MAX_RIGHE] = sTextToBlink[i];
                    }

                    iTimeBlinkCounter[i]--;
                }
            }

            TicketsList_L.Lines = textLine_L;
            TicketsList_R.Lines = textLine_R;

            iTimerTag++;
            iTimerTag %= 6;

            /**************************************
                Blink Antenna di connessione DB
             **************************************/
            if (_rdBaseIntf.GetDB_Connected())
            {

                switch (iBlink)
                {
                    case 0:
                        btnAnt.Image = BtnImgList.Images[1]; // verde piccola
                        iBlink++;
                        break;
                    case 3:
                        btnAnt.Image = BtnImgList.Images[2]; // celeste piccola
                        iBlink++;
                        break;
                    case 6:
                        btnAnt.Image = BtnImgList.Images[3]; // celeste media
                        iBlink++;
                        break;
                    case 9:
                        btnAnt.Image = BtnImgList.Images[4]; // celeste grande
                        iBlink++;
                        break;

                    default:
                        iBlink++;
                        if (iBlink > 12)
                        {
                            btnAnt.Image = BtnImgList.Images[1];  // verde piccola
                            iBlink = 20;
                        }
                        break;
                }
            }
            else
            {
                btnAnt.Image = BtnImgList.Images[0]; // rossa piccola
                iBlink = 10;
            }

            /*******************************************
                    invocazione dbScaricaOrdine
             *******************************************/

            iScaricoDelayTimer--;

            if (iScaricoDelayTimer == 0)
            {
                iScaricoDelayTimer = MAX_RETRY;

                iDebug = ordiniQueue.Count;

                if ((ordiniQueue.Count > 0) && MnuScarico_DB.Checked)
                {
                    // legge ma non elimina dalla coda
                    sOrdiniQueueObj = (TQueue_Obj)ordiniQueue.Peek();

                    iNumScontrino = sOrdiniQueueObj.iNumTicket;
                    iGruppo = sOrdiniQueueObj.iGruppo;

                    ulStart = (ulong)Environment.TickCount;

                    iGlbNumOfTickets = _rdBaseIntf.dbGetNumOfOrdersFromDB(false);

                    // c'è la connessione ma non i dati = StandFacile da avviare
                    if (iGlbNumOfTickets == 0)
                        _bInitNetReadParams = true;

                    // _bInitNetReadParams è true anche in fase di avvio
                    if (_bInitNetReadParams)
                    {
                        rNetConfigLightDlg.NetConfig_ReadParams();
                        _bInitNetReadParams = false;
                    }

                    if (sOrdiniQueueObj.sEvent == SCARICO_DB_EVENT)
                    {
                        _rdBaseIntf.dbCheckStatus();
                        bScaricoEseguito = _rdBaseIntf.dbScaricaOrdine(iNumScontrino, iGruppo);

                        if (bScaricoEseguito)
                        {
                            // misura del tempo in ms per eseguire dbScaricaOrdine
                            ulStop = (ulong)Environment.TickCount;
                            ulPingTime = ulStop - ulStart;
                            sTmp = String.Format("{0} ms", ulPingTime);
                            lblElapsedTime.Text = sTmp;

                            LogToFile("timer scarica Ordine : dbFuncTime = " + sTmp);

                            // elimina dalla coda
                            sOrdiniQueueObj = (TQueue_Obj)ordiniQueue.Dequeue();

                            sTmp = String.Format("timer scarica Ordine : eseguito iNumScontrino={0}, iGruppo={1} ", iNumScontrino, iGruppo);
                            LogToFile(sTmp);
                            bWarnOnce = true;

                            if (ordiniQueue.Count > 0)
                                iScaricoDelayTimer = 2; // 0,5s
                        }
                        else
                        {
                            sTmp = String.Format("§ timer scarica Ordine : *** NON ESEGUITO *** iNumScontrino={0}, iGruppo={1} ", iNumScontrino, iGruppo);
                            LogToFile(sTmp);
                        }
                    }
                }

                if (_iFuncMode == (int)FUNC_MODE.DUPLICAZIONE_MONITOR)
                {
                    _rdBaseIntf.dbCheckStatus();
                    _rdBaseIntf.dbCaricaUltimiOrdini();

                    // scorrimento stringhe sTextToBlink e contatori
                    for (i = 0; i < 2 * MAX_RIGHE; i++)
                    {
                        // stop blink
                        if (i > 0)
                            iTimeBlinkCounter[i] = 0;

                        sTmp = _sNumScontrino[2 * MAX_RIGHE - i - 1];

                        if (String.IsNullOrEmpty(sTmp.Trim()))
                            sTextToBlink[i] = sTmp;
                        else
                            sTextToBlink[i] = String.Format(" {0:d4}", Convert.ToInt32(sTmp));
                    }

                    sDebug = _sNumScontrino[0];

                    if (_sOldString != sTextToBlink[0])
                    {
                        _sOldString = sTextToBlink[0];
                        iTimeBlinkCounter[0] = MAX_BLINK;
                    }
                }
            }
        }

        private void FormResize(object sender, EventArgs e)
        {
            float fFontHeight_V, fFontHeight_H;

            TicketsList_L.Top = 90;
            TicketsList_R.Top = TicketsList_L.Top;

            TicketsList_L.Height = rFrmMain.Height - 200;
            TicketsList_L.Width = rFrmMain.Width / 2 - 40;

            TicketsList_R.Height = TicketsList_L.Height;
            TicketsList_R.Width = TicketsList_L.Width;

            TicketsList_R.Left = 2 * TicketsList_L.Left + TicketsList_L.Width;

            if (WindowState != FormWindowState.Minimized)
            {
                fFontHeight_V = (float)Math.Round((TicketsList_L.Height - 24) / (1.66 * MAX_RIGHE));
                fFontHeight_H = (float)Math.Round((TicketsList_L.Width - 24) / (1.2 * 4));

                if (fFontHeight_H > fFontHeight_V)
                    TicketsList_L.Font = new System.Drawing.Font(TicketsList_L.Font.Name, fFontHeight_V);
                else
                    TicketsList_L.Font = new System.Drawing.Font(TicketsList_L.Font.Name, fFontHeight_H);

                //if (fFontHeight_H <= 52)
                //{
                //    LabelTitolo.Font = new System.Drawing.Font(LabelTitolo.Font.Name, fFontHeight_H / 1.8f);
                //    LabelClock.Font = new System.Drawing.Font(LabelClock.Font.Name, fFontHeight_H / 2.4f);
                //}
            }

            TicketsList_R.Font = TicketsList_L.Font;
        }

        /*****************************************************
         *     Protezione contro le modifiche non volute
         *****************************************************/
        private void MnuEspertoClick(object sender, EventArgs e)
        {
            DialogResult dResult;
            String sTmp;

            if (!CheckService(CFG_COMMON_STRINGS._ESPERTO) && (!MnuEsperto.Checked))
                dResult = MessageBox.Show("E' importante aver letto e compreso il manuale prima di proseguire !\r\n\r\n" +
                        "Il manuale pdf è presente nella cartella di installazione e si può aprire anche dal pulsante presente nel menù di Aiuto->Aiuto Rapido.",
                        "Attenzione !", MessageBoxButtons.OKCancel);

            else
                dResult = DialogResult.OK;

            if (dResult == DialogResult.OK)
                MnuEsperto.Checked = !MnuEsperto.Checked;

            CheckMenuItems();

            sTmp = String.Format("Mainform : Modo Esperto {0}", MnuEsperto.Checked);
            LogToFile(sTmp);
        }

        /// <summary>
        ///  Abilita/disabilita le varie voci del Menù Principale
        /// </summary>
        private void CheckMenuItems()
        {
            if (MnuEsperto.Checked)
            {
                MnuDBServer.Enabled = true;

                MnuSintesiVocale.Enabled = true;
                MnuScarico_DB.Enabled = true;
                MnuDuplicazioneMonitor.Enabled = true;
                MnuSoloLettura_BC.Enabled = true;
            }
            else
            {
                MnuDBServer.Enabled = false;

                MnuSintesiVocale.Enabled = false;
                MnuScarico_DB.Enabled = false;
                MnuDuplicazioneMonitor.Enabled = false;
                MnuSoloLettura_BC.Enabled = false;
            }

            FormResize(this, null);
        }

        private void MnuDBServer_Click(object sender, EventArgs e)
        {
            NetConfigLightDlg.rNetConfigLightDlg.Init(true);
        }

        private void MnuAbout_Click(object sender, EventArgs e)
        {
            InfoDlg rInfoDlg = new InfoDlg();
            rInfoDlg.ShowDialog();
            rInfoDlg.Dispose();
        }

        private void MnuExit_Click(object sender, EventArgs e)
        {
            LogToFile("FrmMain : uscita");
            Close();
        }


        /// <summary>blink antenna</summary>
        public static void StartAntBmpTimer()
        {
            iBlink = 0;
        }

        private void BtnAnt_Click(object sender, EventArgs e)
        {
            String sTmp;

            ulStart = (ulong)Environment.TickCount;

            _rdBaseIntf.dbCheck();

            // misura del tempo in ms per eseguire dbCheck
            ulStop = (ulong)Environment.TickCount;
            ulPingTime = ulStop - ulStart;
            sTmp = String.Format("{0} ms", ulPingTime);
            lblElapsedTime.Text = sTmp;
        }

        private void MnuManuale_Click(object sender, EventArgs e)
        {
            String sDir, sNomeDoc;
            TErrMsg WrnMsg = new TErrMsg();

            // prende il manuale dalla Directory della documentazione
            sNomeDoc = "..\\..\\doc\\" + _NOME_MANUALE;

            sDir = Directory.GetCurrentDirectory() + "\\";

            if (File.Exists(sDir + _NOME_MANUALE))
                System.Diagnostics.Process.Start(sDir + _NOME_MANUALE);
            else if (File.Exists(sDir + sNomeDoc))
                System.Diagnostics.Process.Start(sDir + sNomeDoc);
            else
            {
                WrnMsg.sNomeFile = _NOME_MANUALE;
                WrnMsg.iErrID = WRN_FNF;
                WarningManager(WrnMsg);
            }
        }

        private void MnuScarico_DB_Click(object sender, EventArgs e)
        {
            _iFuncMode = 0;
            MnuScarico_DB.Checked = true;
            MnuDuplicazioneMonitor.Checked = false;
            MnuSoloLettura_BC.Checked = false;
            EditInput.Enabled = true;
        }

        private void MnuSintesiVocale_Click(object sender, EventArgs e)
        {
            SpeechSynth.rSpeechSynth.Init(true);
        }

        private void MnuSoloLettura_BC_Click(object sender, EventArgs e)
        {
            _iFuncMode = 1;
            MnuScarico_DB.Checked = false;
            MnuDuplicazioneMonitor.Checked = false;
            MnuSoloLettura_BC.Checked = true;
            EditInput.Enabled = true;
        }

        private void MnuDuplicazioneMonitor_Click(object sender, EventArgs e)
        {
            _iFuncMode = 2;
            iScaricoDelayTimer = 2;

            MnuScarico_DB.Checked = false;
            MnuDuplicazioneMonitor.Checked = true;
            MnuSoloLettura_BC.Checked = false;
            EditInput.Enabled = false;
        }

        private void MnuAiutoRapido_Click(object sender, EventArgs e)
        {
            QuickHelpDlg rQuickHelpDlg = new QuickHelpDlg();

            rQuickHelpDlg.Dispose();
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MnuEsperto.Checked)
                WriteRegistry(KEY_FUNC_MODE, _iFuncMode);

            LogToFile("Mainform : uscita");

            StopLogServer(); // deve stare per ultimo
        }

    }
}


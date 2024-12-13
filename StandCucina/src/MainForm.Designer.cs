namespace StandFacile
{
    partial class FrmMain
    {
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Pulire le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.Timer = new System.Windows.Forms.Timer();
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.MnuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.MnuPrintTest = new System.Windows.Forms.ToolStripMenuItem();
            this.MnuStampaFile = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.MnuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.MnuVisualizza = new System.Windows.Forms.ToolStripMenuItem();
            this.MnuVisLog = new System.Windows.Forms.ToolStripMenuItem();
            this.MnuImpostazioni = new System.Windows.Forms.ToolStripMenuItem();
            this.MnuEsperto = new System.Windows.Forms.ToolStripMenuItem();
            this.N1 = new System.Windows.Forms.ToolStripSeparator();
            this.MnuNetConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.MnuConfigurazioneStampe = new System.Windows.Forms.ToolStripMenuItem();
            this.MnuAiuto = new System.Windows.Forms.ToolStripMenuItem();
            this.MnuAiutoRapido = new System.Windows.Forms.ToolStripMenuItem();
            this.MnuInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.TB_Tickets = new System.Windows.Forms.TextBox();
            this.TB_Messaggi = new System.Windows.Forms.TextBox();
            this.SBar_Tickets = new System.Windows.Forms.StatusStrip();
            this.toolStripCurrTicketNum = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripTotTicketNum = new System.Windows.Forms.ToolStripStatusLabel();
            this.SBar_Messages = new System.Windows.Forms.StatusStrip();
            this.toolStripCurrMsgNum = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripTotMsgNum = new System.Windows.Forms.ToolStripStatusLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.LblStatus = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.BtnOnline = new System.Windows.Forms.Button();
            this.ME_TickNum = new System.Windows.Forms.TextBox();
            this.Label_ServerName = new System.Windows.Forms.Label();
            this.ClientTimer = new System.Windows.Forms.Timer();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.btnAnt = new System.Windows.Forms.Button();
            this.printerPicBox = new System.Windows.Forms.PictureBox();
            this.BtnPrintTicket = new System.Windows.Forms.Button();
            this.BtnPrintMsg = new System.Windows.Forms.Button();
            this.BtnPrevMsg = new System.Windows.Forms.Button();
            this.BtnNextMsg = new System.Windows.Forms.Button();
            this.BtnPrevTicket = new System.Windows.Forms.Button();
            this.BtnNextTicket = new System.Windows.Forms.Button();
            this.checkBoxSkipPrinted = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.MainMenu.SuspendLayout();
            this.SBar_Tickets.SuspendLayout();
            this.SBar_Messages.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.printerPicBox)).BeginInit();
            this.SuspendLayout();
            // 
            // Timer
            // 
            this.Timer.Enabled = true;
            this.Timer.Tick += new System.EventHandler(this.Timer_ImgLoop);
            // 
            // MainMenu
            // 
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MnuFile,
            this.MnuVisualizza,
            this.MnuImpostazioni,
            this.MnuAiuto});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Size = new System.Drawing.Size(535, 24);
            this.MainMenu.TabIndex = 3;
            this.MainMenu.Text = "menuStrip";
            // 
            // MnuFile
            // 
            this.MnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MnuPrintTest,
            this.MnuStampaFile,
            this.toolStripSeparator1,
            this.MnuExit});
            this.MnuFile.Name = "MnuFile";
            this.MnuFile.Size = new System.Drawing.Size(37, 20);
            this.MnuFile.Text = "File";
            // 
            // MnuPrintTest
            // 
            this.MnuPrintTest.Name = "MnuPrintTest";
            this.MnuPrintTest.Size = new System.Drawing.Size(160, 22);
            this.MnuPrintTest.Text = "Stampa di &prova";
            this.MnuPrintTest.Click += new System.EventHandler(this.MnuPrintTest_Click);
            // 
            // MnuStampaFile
            // 
            this.MnuStampaFile.Name = "MnuStampaFile";
            this.MnuStampaFile.Size = new System.Drawing.Size(160, 22);
            this.MnuStampaFile.Text = "Stampa &File ...";
            this.MnuStampaFile.Click += new System.EventHandler(this.MnuStampaFile_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(157, 6);
            // 
            // MnuExit
            // 
            this.MnuExit.Name = "MnuExit";
            this.MnuExit.Size = new System.Drawing.Size(160, 22);
            this.MnuExit.Text = "E&sci";
            this.MnuExit.Click += new System.EventHandler(this.FormClose_Click);
            // 
            // MnuVisualizza
            // 
            this.MnuVisualizza.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MnuVisLog});
            this.MnuVisualizza.Name = "MnuVisualizza";
            this.MnuVisualizza.Size = new System.Drawing.Size(69, 20);
            this.MnuVisualizza.Text = "Visualizza";
            // 
            // MnuVisLog
            // 
            this.MnuVisLog.Name = "MnuVisLog";
            this.MnuVisLog.Size = new System.Drawing.Size(151, 22);
            this.MnuVisLog.Text = "Finestra di &Log";
            this.MnuVisLog.Click += new System.EventHandler(this.MnuVisLog_Click);
            // 
            // MnuImpostazioni
            // 
            this.MnuImpostazioni.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MnuEsperto,
            this.N1,
            this.MnuNetConfig,
            this.toolStripSeparator2,
            this.MnuConfigurazioneStampe});
            this.MnuImpostazioni.Name = "MnuImpostazioni";
            this.MnuImpostazioni.Size = new System.Drawing.Size(62, 20);
            this.MnuImpostazioni.Text = "Imposta";
            // 
            // MnuEsperto
            // 
            this.MnuEsperto.Name = "MnuEsperto";
            this.MnuEsperto.Size = new System.Drawing.Size(239, 22);
            this.MnuEsperto.Text = "&Modo Esperto";
            this.MnuEsperto.Click += new System.EventHandler(this.MnuEspertoClick);
            // 
            // N1
            // 
            this.N1.Name = "N1";
            this.N1.Size = new System.Drawing.Size(236, 6);
            // 
            // MnuNetConfig
            // 
            this.MnuNetConfig.Enabled = false;
            this.MnuNetConfig.Name = "MnuNetConfig";
            this.MnuNetConfig.Size = new System.Drawing.Size(239, 22);
            this.MnuNetConfig.Text = "Configurazione &Rete e Copie  ...";
            this.MnuNetConfig.Click += new System.EventHandler(this.MnuNetConfig_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(236, 6);
            // 
            // MnuConfigurazioneStampe
            // 
            this.MnuConfigurazioneStampe.Enabled = false;
            this.MnuConfigurazioneStampe.Name = "MnuConfigurazioneStampe";
            this.MnuConfigurazioneStampe.Size = new System.Drawing.Size(239, 22);
            this.MnuConfigurazioneStampe.Text = "Configurazione &Stampe ...";
            this.MnuConfigurazioneStampe.Click += new System.EventHandler(this.MnuConfigurazioneStampeToolStripMenuItem_Click);
            // 
            // MnuAiuto
            // 
            this.MnuAiuto.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MnuAiutoRapido,
            this.MnuInfo});
            this.MnuAiuto.Name = "MnuAiuto";
            this.MnuAiuto.Size = new System.Drawing.Size(48, 20);
            this.MnuAiuto.Text = "Aiuto";
            // 
            // MnuAiutoRapido
            // 
            this.MnuAiutoRapido.Name = "MnuAiutoRapido";
            this.MnuAiutoRapido.Size = new System.Drawing.Size(180, 22);
            this.MnuAiutoRapido.Text = "&Aiuto rapido";
            this.MnuAiutoRapido.Click += new System.EventHandler(this.MnuManuale_Click);
            // 
            // MnuInfo
            // 
            this.MnuInfo.Name = "MnuInfo";
            this.MnuInfo.Size = new System.Drawing.Size(180, 22);
            this.MnuInfo.Text = "&Informazioni su ...";
            this.MnuInfo.Click += new System.EventHandler(this.MnuAbout_Click);
            // 
            // TB_Tickets
            // 
            this.TB_Tickets.AcceptsReturn = true;
            this.TB_Tickets.AcceptsTab = true;
            this.TB_Tickets.BackColor = System.Drawing.Color.Teal;
            this.TB_Tickets.Font = new System.Drawing.Font("Courier New", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TB_Tickets.ForeColor = System.Drawing.SystemColors.Window;
            this.TB_Tickets.Location = new System.Drawing.Point(12, 36);
            this.TB_Tickets.Multiline = true;
            this.TB_Tickets.Name = "TB_Tickets";
            this.TB_Tickets.ReadOnly = true;
            this.TB_Tickets.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TB_Tickets.Size = new System.Drawing.Size(336, 303);
            this.TB_Tickets.TabIndex = 5;
            // 
            // TB_Messaggi
            // 
            this.TB_Messaggi.AcceptsReturn = true;
            this.TB_Messaggi.AcceptsTab = true;
            this.TB_Messaggi.BackColor = System.Drawing.Color.Teal;
            this.TB_Messaggi.Font = new System.Drawing.Font("Courier New", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TB_Messaggi.ForeColor = System.Drawing.SystemColors.Window;
            this.TB_Messaggi.Location = new System.Drawing.Point(12, 381);
            this.TB_Messaggi.Multiline = true;
            this.TB_Messaggi.Name = "TB_Messaggi";
            this.TB_Messaggi.ReadOnly = true;
            this.TB_Messaggi.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TB_Messaggi.Size = new System.Drawing.Size(336, 154);
            this.TB_Messaggi.TabIndex = 6;
            // 
            // SBar_Tickets
            // 
            this.SBar_Tickets.AutoSize = false;
            this.SBar_Tickets.Dock = System.Windows.Forms.DockStyle.None;
            this.SBar_Tickets.Font = new System.Drawing.Font("Segoe UI Symbol", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SBar_Tickets.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.SBar_Tickets.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripCurrTicketNum,
            this.toolStripTotTicketNum});
            this.SBar_Tickets.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.SBar_Tickets.Location = new System.Drawing.Point(12, 342);
            this.SBar_Tickets.Name = "SBar_Tickets";
            this.SBar_Tickets.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.SBar_Tickets.Size = new System.Drawing.Size(336, 22);
            this.SBar_Tickets.SizingGrip = false;
            this.SBar_Tickets.Stretch = false;
            this.SBar_Tickets.TabIndex = 7;
            // 
            // toolStripCurrTicketNum
            // 
            this.toolStripCurrTicketNum.AutoSize = false;
            this.toolStripCurrTicketNum.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripCurrTicketNum.Name = "toolStripCurrTicketNum";
            this.toolStripCurrTicketNum.Size = new System.Drawing.Size(210, 17);
            this.toolStripCurrTicketNum.Text = "Scontrino Num. : ";
            this.toolStripCurrTicketNum.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripTotTicketNum
            // 
            this.toolStripTotTicketNum.AutoSize = false;
            this.toolStripTotTicketNum.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripTotTicketNum.Name = "toolStripTotTicketNum";
            this.toolStripTotTicketNum.Size = new System.Drawing.Size(100, 17);
            this.toolStripTotTicketNum.Text = "Emessi : ";
            this.toolStripTotTicketNum.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SBar_Messages
            // 
            this.SBar_Messages.AutoSize = false;
            this.SBar_Messages.Dock = System.Windows.Forms.DockStyle.None;
            this.SBar_Messages.Font = new System.Drawing.Font("Segoe UI Symbol", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SBar_Messages.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.SBar_Messages.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripCurrMsgNum,
            this.toolStripTotMsgNum});
            this.SBar_Messages.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.SBar_Messages.Location = new System.Drawing.Point(12, 538);
            this.SBar_Messages.Name = "SBar_Messages";
            this.SBar_Messages.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.SBar_Messages.Size = new System.Drawing.Size(336, 22);
            this.SBar_Messages.SizingGrip = false;
            this.SBar_Messages.Stretch = false;
            this.SBar_Messages.TabIndex = 8;
            // 
            // toolStripCurrMsgNum
            // 
            this.toolStripCurrMsgNum.AutoSize = false;
            this.toolStripCurrMsgNum.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripCurrMsgNum.Name = "toolStripCurrMsgNum";
            this.toolStripCurrMsgNum.Size = new System.Drawing.Size(210, 17);
            this.toolStripCurrMsgNum.Text = "Messaggio Num. : ";
            this.toolStripCurrMsgNum.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripTotMsgNum
            // 
            this.toolStripTotMsgNum.AutoSize = false;
            this.toolStripTotMsgNum.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripTotMsgNum.Name = "toolStripTotMsgNum";
            this.toolStripTotMsgNum.Size = new System.Drawing.Size(100, 17);
            this.toolStripTotMsgNum.Text = "Emessi : ";
            this.toolStripTotMsgNum.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(370, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(142, 16);
            this.label1.TabIndex = 9;
            this.label1.Text = "Connessione al server :";
            // 
            // LblStatus
            // 
            this.LblStatus.AutoSize = true;
            this.LblStatus.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblStatus.Location = new System.Drawing.Point(366, 90);
            this.LblStatus.Name = "LblStatus";
            this.LblStatus.Size = new System.Drawing.Size(83, 16);
            this.LblStatus.TabIndex = 10;
            this.LblStatus.Text = "Stato : AUTO";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(366, 284);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 16);
            this.label3.TabIndex = 11;
            this.label3.Text = "Vai al numero :";
            // 
            // BtnOnline
            // 
            this.BtnOnline.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnOnline.Location = new System.Drawing.Point(356, 125);
            this.BtnOnline.Name = "BtnOnline";
            this.BtnOnline.Size = new System.Drawing.Size(107, 30);
            this.BtnOnline.TabIndex = 13;
            this.BtnOnline.Text = "modo MANUALE";
            this.BtnOnline.UseVisualStyleBackColor = true;
            this.BtnOnline.Click += new System.EventHandler(this.BtnOnline_Click);
            // 
            // ME_TickNum
            // 
            this.ME_TickNum.Enabled = false;
            this.ME_TickNum.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ME_TickNum.Location = new System.Drawing.Point(473, 278);
            this.ME_TickNum.Name = "ME_TickNum";
            this.ME_TickNum.Size = new System.Drawing.Size(40, 27);
            this.ME_TickNum.TabIndex = 21;
            this.ME_TickNum.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ME_TickNum_KeyDown);
            this.ME_TickNum.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ME_TickNum_KeyPress);
            // 
            // Label_ServerName
            // 
            this.Label_ServerName.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label_ServerName.Location = new System.Drawing.Point(370, 45);
            this.Label_ServerName.Name = "Label_ServerName";
            this.Label_ServerName.Size = new System.Drawing.Size(143, 22);
            this.Label_ServerName.TabIndex = 23;
            this.Label_ServerName.Text = ".........................";
            this.Label_ServerName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ClientTimer
            // 
            this.ClientTimer.Interval = 1000;
            this.ClientTimer.Tick += new System.EventHandler(this.MainTimerLoop_Tick);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            // 
            // btnAnt
            // 
            this.btnAnt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnAnt.Image = global::StandFacile.Properties.Resources.ant_g2;
            this.btnAnt.Location = new System.Drawing.Point(469, 113);
            this.btnAnt.Name = "btnAnt";
            this.btnAnt.Size = new System.Drawing.Size(44, 42);
            this.btnAnt.TabIndex = 26;
            this.btnAnt.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnAnt.UseVisualStyleBackColor = true;
            this.btnAnt.Click += new System.EventHandler(this.BtnAnt_Click);
            // 
            // printerPicBox
            // 
            this.printerPicBox.Image = global::StandFacile.Properties.Resources.circleGreen;
            this.printerPicBox.Location = new System.Drawing.Point(383, 319);
            this.printerPicBox.Name = "printerPicBox";
            this.printerPicBox.Size = new System.Drawing.Size(47, 44);
            this.printerPicBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.printerPicBox.TabIndex = 24;
            this.printerPicBox.TabStop = false;
            // 
            // BtnPrintTicket
            // 
            this.BtnPrintTicket.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.BtnPrintTicket.Enabled = false;
            this.BtnPrintTicket.Image = global::StandFacile.Properties.Resources.printer_s;
            this.BtnPrintTicket.Location = new System.Drawing.Point(459, 319);
            this.BtnPrintTicket.Name = "BtnPrintTicket";
            this.BtnPrintTicket.Size = new System.Drawing.Size(54, 44);
            this.BtnPrintTicket.TabIndex = 20;
            this.BtnPrintTicket.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.BtnPrintTicket.UseVisualStyleBackColor = true;
            this.BtnPrintTicket.Click += new System.EventHandler(this.BtnPrintTicket_Click);
            // 
            // BtnPrintMsg
            // 
            this.BtnPrintMsg.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.BtnPrintMsg.Enabled = false;
            this.BtnPrintMsg.Image = global::StandFacile.Properties.Resources.printer_s;
            this.BtnPrintMsg.Location = new System.Drawing.Point(459, 481);
            this.BtnPrintMsg.Name = "BtnPrintMsg";
            this.BtnPrintMsg.Size = new System.Drawing.Size(54, 44);
            this.BtnPrintMsg.TabIndex = 19;
            this.BtnPrintMsg.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.BtnPrintMsg.UseVisualStyleBackColor = true;
            this.BtnPrintMsg.Click += new System.EventHandler(this.BtnPrintMsg_Click);
            // 
            // BtnPrevMsg
            // 
            this.BtnPrevMsg.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.BtnPrevMsg.Enabled = false;
            this.BtnPrevMsg.Image = global::StandFacile.Properties.Resources.ArrowSLeft;
            this.BtnPrevMsg.Location = new System.Drawing.Point(376, 418);
            this.BtnPrevMsg.Name = "BtnPrevMsg";
            this.BtnPrevMsg.Size = new System.Drawing.Size(54, 33);
            this.BtnPrevMsg.TabIndex = 16;
            this.BtnPrevMsg.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.BtnPrevMsg.UseVisualStyleBackColor = true;
            this.BtnPrevMsg.Click += new System.EventHandler(this.BtnPrevMsg_Click);
            // 
            // BtnNextMsg
            // 
            this.BtnNextMsg.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.BtnNextMsg.Enabled = false;
            this.BtnNextMsg.Image = global::StandFacile.Properties.Resources.ArrowSRight;
            this.BtnNextMsg.Location = new System.Drawing.Point(459, 418);
            this.BtnNextMsg.Name = "BtnNextMsg";
            this.BtnNextMsg.Size = new System.Drawing.Size(54, 33);
            this.BtnNextMsg.TabIndex = 17;
            this.BtnNextMsg.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.BtnNextMsg.UseVisualStyleBackColor = true;
            this.BtnNextMsg.Click += new System.EventHandler(this.BtnNextMsg_Click);
            // 
            // BtnPrevTicket
            // 
            this.BtnPrevTicket.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.BtnPrevTicket.Enabled = false;
            this.BtnPrevTicket.Image = global::StandFacile.Properties.Resources.ArrowSLeft;
            this.BtnPrevTicket.Location = new System.Drawing.Point(376, 174);
            this.BtnPrevTicket.Name = "BtnPrevTicket";
            this.BtnPrevTicket.Size = new System.Drawing.Size(54, 33);
            this.BtnPrevTicket.TabIndex = 14;
            this.BtnPrevTicket.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.BtnPrevTicket.UseVisualStyleBackColor = true;
            this.BtnPrevTicket.Click += new System.EventHandler(this.BtnPrevTicket_Click);
            // 
            // BtnNextTicket
            // 
            this.BtnNextTicket.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.BtnNextTicket.Enabled = false;
            this.BtnNextTicket.Image = global::StandFacile.Properties.Resources.ArrowSRight;
            this.BtnNextTicket.Location = new System.Drawing.Point(459, 174);
            this.BtnNextTicket.Name = "BtnNextTicket";
            this.BtnNextTicket.Size = new System.Drawing.Size(54, 33);
            this.BtnNextTicket.TabIndex = 15;
            this.BtnNextTicket.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.BtnNextTicket.UseVisualStyleBackColor = true;
            this.BtnNextTicket.Click += new System.EventHandler(this.BtnNextTicket_Click);
            // 
            // checkBoxSkipPrinted
            // 
            this.checkBoxSkipPrinted.AutoSize = true;
            this.checkBoxSkipPrinted.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxSkipPrinted.Location = new System.Drawing.Point(360, 230);
            this.checkBoxSkipPrinted.Name = "checkBoxSkipPrinted";
            this.checkBoxSkipPrinted.Size = new System.Drawing.Size(168, 18);
            this.checkBoxSkipPrinted.TabIndex = 27;
            this.checkBoxSkipPrinted.Text = "evita Tickets già stampati,";
            this.checkBoxSkipPrinted.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(377, 247);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 16);
            this.label2.TabIndex = 28;
            this.label2.Text = "o annullati";
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(535, 573);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.checkBoxSkipPrinted);
            this.Controls.Add(this.btnAnt);
            this.Controls.Add(this.printerPicBox);
            this.Controls.Add(this.Label_ServerName);
            this.Controls.Add(this.ME_TickNum);
            this.Controls.Add(this.BtnPrintTicket);
            this.Controls.Add(this.BtnPrintMsg);
            this.Controls.Add(this.BtnPrevMsg);
            this.Controls.Add(this.BtnNextMsg);
            this.Controls.Add(this.BtnPrevTicket);
            this.Controls.Add(this.BtnNextTicket);
            this.Controls.Add(this.BtnOnline);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.LblStatus);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SBar_Messages);
            this.Controls.Add(this.SBar_Tickets);
            this.Controls.Add(this.TB_Messaggi);
            this.Controls.Add(this.TB_Tickets);
            this.Controls.Add(this.MainMenu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.MainMenu;
            this.MaximizeBox = false;
            this.Name = "FrmMain";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Stand Cucina";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ME_TickNum_KeyPress);
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.SBar_Tickets.ResumeLayout(false);
            this.SBar_Tickets.PerformLayout();
            this.SBar_Messages.ResumeLayout(false);
            this.SBar_Messages.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.printerPicBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer Timer;
        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.ToolStripMenuItem MnuFile;
        private System.Windows.Forms.ToolStripMenuItem MnuImpostazioni;
        private System.Windows.Forms.ToolStripMenuItem MnuVisualizza;
        private System.Windows.Forms.ToolStripMenuItem MnuAiuto;
        private System.Windows.Forms.ToolStripMenuItem MnuEsperto;
        private System.Windows.Forms.ToolStripMenuItem MnuInfo;
        private System.Windows.Forms.ToolStripMenuItem MnuExit;
        private System.Windows.Forms.ToolStripMenuItem MnuConfigurazioneStampe;
        private System.Windows.Forms.ToolStripSeparator N1;
        private System.Windows.Forms.ToolStripMenuItem MnuVisLog;
        private System.Windows.Forms.TextBox TB_Tickets;
        private System.Windows.Forms.TextBox TB_Messaggi;
        private System.Windows.Forms.StatusStrip SBar_Tickets;
        private System.Windows.Forms.ToolStripStatusLabel toolStripCurrTicketNum;
        private System.Windows.Forms.ToolStripStatusLabel toolStripTotTicketNum;
        private System.Windows.Forms.StatusStrip SBar_Messages;
        private System.Windows.Forms.ToolStripStatusLabel toolStripCurrMsgNum;
        private System.Windows.Forms.ToolStripStatusLabel toolStripTotMsgNum;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label LblStatus;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button BtnOnline;
        private System.Windows.Forms.Button BtnPrevTicket;
        private System.Windows.Forms.Button BtnNextTicket;
        private System.Windows.Forms.Button BtnPrevMsg;
        private System.Windows.Forms.Button BtnNextMsg;
        private System.Windows.Forms.Button BtnPrintMsg;
        private System.Windows.Forms.Button BtnPrintTicket;
        private System.Windows.Forms.TextBox ME_TickNum;
        private System.Windows.Forms.ToolStripMenuItem MnuPrintTest;
        private System.Windows.Forms.ToolStripMenuItem MnuStampaFile;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem MnuNetConfig;
        private System.Windows.Forms.Label Label_ServerName;
        private System.Windows.Forms.Timer ClientTimer;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.PictureBox printerPicBox;
        private System.Windows.Forms.Button btnAnt;
        private System.Windows.Forms.ToolStripMenuItem MnuAiutoRapido;
        private System.Windows.Forms.CheckBox checkBoxSkipPrinted;
        private System.Windows.Forms.Label label2;
    }
}


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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.Timer = new System.Windows.Forms.Timer(this.components);
            this.LabelTitolo = new System.Windows.Forms.Label();
            this.LabelClock = new System.Windows.Forms.Label();
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.MnuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.MnuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.MnuImpostazioni = new System.Windows.Forms.ToolStripMenuItem();
            this.MnuEsperto = new System.Windows.Forms.ToolStripMenuItem();
            this.N1 = new System.Windows.Forms.ToolStripSeparator();
            this.MnuDBServer = new System.Windows.Forms.ToolStripMenuItem();
            this.MnuScarico_DB = new System.Windows.Forms.ToolStripMenuItem();
            this.MnuSoloLettura_BC = new System.Windows.Forms.ToolStripMenuItem();
            this.MnuDuplicazioneMonitor = new System.Windows.Forms.ToolStripMenuItem();
            this.MnuAiuto = new System.Windows.Forms.ToolStripMenuItem();
            this.Mnu_AiutoRapido = new System.Windows.Forms.ToolStripMenuItem();
            this.MnuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.BtnImgList = new System.Windows.Forms.ImageList(this.components);
            this.lblElapsedTime = new System.Windows.Forms.Label();
            this.EditInput = new System.Windows.Forms.TextBox();
            this.TicketsList_L = new System.Windows.Forms.TextBox();
            this.TicketsList_R = new System.Windows.Forms.TextBox();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.btnAnt = new System.Windows.Forms.Button();
            this.MainMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // Timer
            // 
            this.Timer.Interval = 250;
            this.Timer.Tick += new System.EventHandler(this.Timer_MainLoop);
            // 
            // LabelTitolo
            // 
            this.LabelTitolo.Font = new System.Drawing.Font("Microsoft Sans Serif", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelTitolo.ForeColor = System.Drawing.Color.Yellow;
            this.LabelTitolo.Location = new System.Drawing.Point(12, 33);
            this.LabelTitolo.Name = "LabelTitolo";
            this.LabelTitolo.Size = new System.Drawing.Size(587, 44);
            this.LabelTitolo.TabIndex = 1;
            this.LabelTitolo.Text = "Serviamo lo scontrino numero :";
            // 
            // LabelClock
            // 
            this.LabelClock.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LabelClock.Font = new System.Drawing.Font("Verdana", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelClock.ForeColor = System.Drawing.Color.Yellow;
            this.LabelClock.Location = new System.Drawing.Point(12, 382);
            this.LabelClock.Name = "LabelClock";
            this.LabelClock.Size = new System.Drawing.Size(198, 45);
            this.LabelClock.TabIndex = 2;
            this.LabelClock.Text = "18:30:00";
            this.LabelClock.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // MainMenu
            // 
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MnuFile,
            this.MnuImpostazioni,
            this.MnuAiuto});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Size = new System.Drawing.Size(624, 24);
            this.MainMenu.TabIndex = 3;
            this.MainMenu.Text = "menuStrip";
            // 
            // MnuFile
            // 
            this.MnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MnuExit});
            this.MnuFile.Name = "MnuFile";
            this.MnuFile.Size = new System.Drawing.Size(37, 20);
            this.MnuFile.Text = "File";
            // 
            // MnuExit
            // 
            this.MnuExit.Name = "MnuExit";
            this.MnuExit.Size = new System.Drawing.Size(94, 22);
            this.MnuExit.Text = "E&sci";
            this.MnuExit.Click += new System.EventHandler(this.MnuExit_Click);
            // 
            // MnuImpostazioni
            // 
            this.MnuImpostazioni.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MnuEsperto,
            this.N1,
            this.MnuDBServer,
            this.MnuScarico_DB,
            this.MnuSoloLettura_BC,
            this.MnuDuplicazioneMonitor});
            this.MnuImpostazioni.Name = "MnuImpostazioni";
            this.MnuImpostazioni.Size = new System.Drawing.Size(62, 20);
            this.MnuImpostazioni.Text = "Imposta";
            // 
            // MnuEsperto
            // 
            this.MnuEsperto.Name = "MnuEsperto";
            this.MnuEsperto.Size = new System.Drawing.Size(320, 22);
            this.MnuEsperto.Text = "&Modo Esperto";
            this.MnuEsperto.Click += new System.EventHandler(this.MnuEspertoClick);
            // 
            // N1
            // 
            this.N1.Name = "N1";
            this.N1.Size = new System.Drawing.Size(317, 6);
            // 
            // MnuDBServer
            // 
            this.MnuDBServer.Enabled = false;
            this.MnuDBServer.Name = "MnuDBServer";
            this.MnuDBServer.Size = new System.Drawing.Size(320, 22);
            this.MnuDBServer.Text = "Imposta &Rete e scarico ordini ...";
            this.MnuDBServer.Click += new System.EventHandler(this.MnuDBServer_Click);
            // 
            // MnuScarico_DB
            // 
            this.MnuScarico_DB.Name = "MnuScarico_DB";
            this.MnuScarico_DB.Size = new System.Drawing.Size(320, 22);
            this.MnuScarico_DB.Text = "- Scarico &barcode degli ordini serviti (standard)";
            this.MnuScarico_DB.Click += new System.EventHandler(this.MnuScarico_DB_Click);
            // 
            // MnuSoloLettura_BC
            // 
            this.MnuSoloLettura_BC.Name = "MnuSoloLettura_BC";
            this.MnuSoloLettura_BC.Size = new System.Drawing.Size(320, 22);
            this.MnuSoloLettura_BC.Text = "- Sola lettura barcode (&no scarico DB)";
            this.MnuSoloLettura_BC.Click += new System.EventHandler(this.MnuSoloLettura_BC_Click);
            // 
            // MnuDuplicazioneMonitor
            // 
            this.MnuDuplicazioneMonitor.Name = "MnuDuplicazioneMonitor";
            this.MnuDuplicazioneMonitor.Size = new System.Drawing.Size(320, 22);
            this.MnuDuplicazioneMonitor.Text = "- &Duplicazione monitor ordini serviti";
            this.MnuDuplicazioneMonitor.Click += new System.EventHandler(this.MnuDuplicazioneMonitor_Click);
            // 
            // MnuAiuto
            // 
            this.MnuAiuto.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Mnu_AiutoRapido,
            this.MnuAbout});
            this.MnuAiuto.Name = "MnuAiuto";
            this.MnuAiuto.Size = new System.Drawing.Size(48, 20);
            this.MnuAiuto.Text = "Aiuto";
            // 
            // Mnu_AiutoRapido
            // 
            this.Mnu_AiutoRapido.Name = "Mnu_AiutoRapido";
            this.Mnu_AiutoRapido.Size = new System.Drawing.Size(168, 22);
            this.Mnu_AiutoRapido.Text = "&Aiuto rapido";
            this.Mnu_AiutoRapido.Click += new System.EventHandler(this.MnuAiutoRapido_Click);
            // 
            // MnuAbout
            // 
            this.MnuAbout.Name = "MnuAbout";
            this.MnuAbout.Size = new System.Drawing.Size(168, 22);
            this.MnuAbout.Text = "&Informazioni su ...";
            this.MnuAbout.Click += new System.EventHandler(this.MnuAbout_Click);
            // 
            // BtnImgList
            // 
            this.BtnImgList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("BtnImgList.ImageStream")));
            this.BtnImgList.TransparentColor = System.Drawing.Color.Transparent;
            this.BtnImgList.Images.SetKeyName(0, "ant_r2.png");
            this.BtnImgList.Images.SetKeyName(1, "ant_g2.png");
            this.BtnImgList.Images.SetKeyName(2, "ImageAnt1");
            this.BtnImgList.Images.SetKeyName(3, "ImageAnt2");
            this.BtnImgList.Images.SetKeyName(4, "ImageAnt2");
            // 
            // lblElapsedTime
            // 
            this.lblElapsedTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblElapsedTime.AutoSize = true;
            this.lblElapsedTime.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblElapsedTime.ForeColor = System.Drawing.Color.Yellow;
            this.lblElapsedTime.Location = new System.Drawing.Point(306, 413);
            this.lblElapsedTime.Name = "lblElapsedTime";
            this.lblElapsedTime.Size = new System.Drawing.Size(38, 16);
            this.lblElapsedTime.TabIndex = 5;
            this.lblElapsedTime.Text = "50ms";
            // 
            // EditInput
            // 
            this.EditInput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.EditInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EditInput.Location = new System.Drawing.Point(504, 400);
            this.EditInput.MaxLength = 13;
            this.EditInput.Name = "EditInput";
            this.EditInput.Size = new System.Drawing.Size(86, 29);
            this.EditInput.TabIndex = 9;
            this.EditInput.Text = "1234567";
            this.EditInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EditInput_KeyDown);
            this.EditInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.EditInput_KeyPress);
            // 
            // TicketsList_L
            // 
            this.TicketsList_L.AcceptsReturn = true;
            this.TicketsList_L.BackColor = System.Drawing.Color.Teal;
            this.TicketsList_L.Cursor = System.Windows.Forms.Cursors.Default;
            this.TicketsList_L.Font = new System.Drawing.Font("Verdana", 54F);
            this.TicketsList_L.ForeColor = System.Drawing.SystemColors.Window;
            this.TicketsList_L.Location = new System.Drawing.Point(20, 91);
            this.TicketsList_L.Multiline = true;
            this.TicketsList_L.Name = "TicketsList_L";
            this.TicketsList_L.Size = new System.Drawing.Size(276, 273);
            this.TicketsList_L.TabIndex = 14;
            this.TicketsList_L.Text = "0001\r\n0002\r\n0003";
            this.TicketsList_L.WordWrap = false;
            // 
            // TicketsList_R
            // 
            this.TicketsList_R.AcceptsReturn = true;
            this.TicketsList_R.BackColor = System.Drawing.Color.Teal;
            this.TicketsList_R.Cursor = System.Windows.Forms.Cursors.Default;
            this.TicketsList_R.Font = new System.Drawing.Font("Verdana", 54F);
            this.TicketsList_R.ForeColor = System.Drawing.SystemColors.Window;
            this.TicketsList_R.Location = new System.Drawing.Point(323, 91);
            this.TicketsList_R.Multiline = true;
            this.TicketsList_R.Name = "TicketsList_R";
            this.TicketsList_R.Size = new System.Drawing.Size(276, 273);
            this.TicketsList_R.TabIndex = 15;
            this.TicketsList_R.Text = "0004\r\n0005\r\n0006";
            this.TicketsList_R.WordWrap = false;
            // 
            // CancelBtn
            // 
            this.CancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBtn.Image = ((System.Drawing.Image)(resources.GetObject("CancelBtn.Image")));
            this.CancelBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.CancelBtn.Location = new System.Drawing.Point(388, 399);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(73, 32);
            this.CancelBtn.TabIndex = 8;
            this.CancelBtn.Text = "Clear";
            this.CancelBtn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // btnAnt
            // 
            this.btnAnt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAnt.BackColor = System.Drawing.Color.White;
            this.btnAnt.ImageIndex = 1;
            this.btnAnt.ImageList = this.BtnImgList;
            this.btnAnt.Location = new System.Drawing.Point(252, 385);
            this.btnAnt.Margin = new System.Windows.Forms.Padding(0);
            this.btnAnt.Name = "btnAnt";
            this.btnAnt.Size = new System.Drawing.Size(44, 44);
            this.btnAnt.TabIndex = 4;
            this.btnAnt.UseVisualStyleBackColor = false;
            this.btnAnt.Click += new System.EventHandler(this.BtnAnt_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.HotTrack;
            this.ClientSize = new System.Drawing.Size(624, 441);
            this.Controls.Add(this.TicketsList_R);
            this.Controls.Add(this.TicketsList_L);
            this.Controls.Add(this.EditInput);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.lblElapsedTime);
            this.Controls.Add(this.btnAnt);
            this.Controls.Add(this.LabelClock);
            this.Controls.Add(this.LabelTitolo);
            this.Controls.Add(this.MainMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.MainMenu;
            this.MinimumSize = new System.Drawing.Size(640, 480);
            this.Name = "FrmMain";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "StandOrdini";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.Resize += new System.EventHandler(this.FormResize);
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer Timer;
        private System.Windows.Forms.Label LabelTitolo;
        private System.Windows.Forms.Label LabelClock;
        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.ToolStripMenuItem MnuFile;
        private System.Windows.Forms.ToolStripMenuItem MnuImpostazioni;
        private System.Windows.Forms.ToolStripMenuItem MnuAiuto;
        private System.Windows.Forms.ToolStripMenuItem MnuEsperto;
        private System.Windows.Forms.ToolStripMenuItem MnuDBServer;
        private System.Windows.Forms.ToolStripMenuItem MnuAbout;
        private System.Windows.Forms.ToolStripMenuItem MnuExit;
        private System.Windows.Forms.ImageList BtnImgList;
        private System.Windows.Forms.Button btnAnt;
        private System.Windows.Forms.Label lblElapsedTime;
        private System.Windows.Forms.ToolStripSeparator N1;
        private System.Windows.Forms.ToolStripMenuItem MnuSoloLettura_BC;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.TextBox EditInput;
        private System.Windows.Forms.TextBox TicketsList_L;
        private System.Windows.Forms.TextBox TicketsList_R;
        private System.Windows.Forms.ToolStripMenuItem MnuScarico_DB;
        private System.Windows.Forms.ToolStripMenuItem MnuDuplicazioneMonitor;
        private System.Windows.Forms.ToolStripMenuItem Mnu_AiutoRapido;
    }
}


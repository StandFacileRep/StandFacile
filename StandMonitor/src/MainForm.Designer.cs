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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.DBGrid = new System.Windows.Forms.DataGridView();
            this.Timer = new System.Windows.Forms.Timer(this.components);
            this.LabelNumScontrino = new System.Windows.Forms.Label();
            this.LabelClock = new System.Windows.Forms.Label();
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.MnuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.MnuStampaDiProva = new System.Windows.Forms.ToolStripMenuItem();
            this.MnuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.MnuVisualizza = new System.Windows.Forms.ToolStripMenuItem();
            this.MnuVisIncassoOggi = new System.Windows.Forms.ToolStripMenuItem();
            this.MnuVisIncassoAltraData = new System.Windows.Forms.ToolStripMenuItem();
            this.N1 = new System.Windows.Forms.ToolStripSeparator();
            this.MnuVisOrdiniOggi = new System.Windows.Forms.ToolStripMenuItem();
            this.MnuVisOrdiniAltraData = new System.Windows.Forms.ToolStripMenuItem();
            this.N2 = new System.Windows.Forms.ToolStripSeparator();
            this.MnuAuxWindow2 = new System.Windows.Forms.ToolStripMenuItem();
            this.MnuAuxWindow3 = new System.Windows.Forms.ToolStripMenuItem();
            this.N3 = new System.Windows.Forms.ToolStripSeparator();
            this.MnuEsploraDB = new System.Windows.Forms.ToolStripMenuItem();
            this.N4 = new System.Windows.Forms.ToolStripSeparator();
            this.MnuVisGruppi = new System.Windows.Forms.ToolStripMenuItem();
            this.MnuVisOrdini = new System.Windows.Forms.ToolStripMenuItem();
            this.MnuImpostazioni = new System.Windows.Forms.ToolStripMenuItem();
            this.MnuEsperto = new System.Windows.Forms.ToolStripMenuItem();
            this.MnuConfigurazioneStampe = new System.Windows.Forms.ToolStripMenuItem();
            this.N5 = new System.Windows.Forms.ToolStripSeparator();
            this.MnuDBServer = new System.Windows.Forms.ToolStripMenuItem();
            this.MnuFiltro = new System.Windows.Forms.ToolStripMenuItem();
            this.MnuAiuto = new System.Windows.Forms.ToolStripMenuItem();
            this.MnuAiutoRapido = new System.Windows.Forms.ToolStripMenuItem();
            this.MnuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.BtnImgList = new System.Windows.Forms.ImageList(this.components);
            this.lblElapsedTime = new System.Windows.Forms.Label();
            this.btnAnt = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.DBGrid)).BeginInit();
            this.MainMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // DBGrid
            // 
            this.DBGrid.AllowUserToAddRows = false;
            this.DBGrid.AllowUserToDeleteRows = false;
            this.DBGrid.AllowUserToResizeRows = false;
            this.DBGrid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.DBGrid.BackgroundColor = System.Drawing.Color.Navy;
            this.DBGrid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Navy;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Yellow;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DBGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.DBGrid.ColumnHeadersHeight = 30;
            this.DBGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Navy;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DBGrid.DefaultCellStyle = dataGridViewCellStyle2;
            this.DBGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.DBGrid.EnableHeadersVisualStyles = false;
            this.DBGrid.GridColor = System.Drawing.SystemColors.ControlLight;
            this.DBGrid.Location = new System.Drawing.Point(10, 85);
            this.DBGrid.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.DBGrid.MultiSelect = false;
            this.DBGrid.Name = "DBGrid";
            this.DBGrid.ReadOnly = true;
            this.DBGrid.RowHeadersVisible = false;
            this.DBGrid.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.DBGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DBGrid.Size = new System.Drawing.Size(756, 381);
            this.DBGrid.TabIndex = 0;
            this.DBGrid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DBGrid_CellClick);
            this.DBGrid.Scroll += new System.Windows.Forms.ScrollEventHandler(this.DBGrid_Scroll);
            this.DBGrid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DBGrid_KeyDown);
            // 
            // Timer
            // 
            this.Timer.Interval = 250;
            this.Timer.Tick += new System.EventHandler(this.timer_MainLoop);
            // 
            // LabelNumScontrino
            // 
            this.LabelNumScontrino.AutoSize = true;
            this.LabelNumScontrino.Font = new System.Drawing.Font("Microsoft Sans Serif", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelNumScontrino.ForeColor = System.Drawing.Color.Yellow;
            this.LabelNumScontrino.Location = new System.Drawing.Point(2, 33);
            this.LabelNumScontrino.Name = "LabelNumScontrino";
            this.LabelNumScontrino.Size = new System.Drawing.Size(360, 44);
            this.LabelNumScontrino.TabIndex = 1;
            this.LabelNumScontrino.Text = "Numero scontrini : 0";
            // 
            // LabelClock
            // 
            this.LabelClock.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LabelClock.AutoSize = true;
            this.LabelClock.Font = new System.Drawing.Font("Microsoft Sans Serif", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelClock.ForeColor = System.Drawing.Color.Yellow;
            this.LabelClock.Location = new System.Drawing.Point(23, 486);
            this.LabelClock.Name = "LabelClock";
            this.LabelClock.Size = new System.Drawing.Size(412, 44);
            this.LabelClock.TabIndex = 2;
            this.LabelClock.Text = "Sab 25.07.15  18:30:00";
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
            this.MainMenu.Size = new System.Drawing.Size(785, 24);
            this.MainMenu.TabIndex = 3;
            this.MainMenu.Text = "menuStrip";
            // 
            // MnuFile
            // 
            this.MnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MnuStampaDiProva,
            this.MnuExit});
            this.MnuFile.Font = new System.Drawing.Font("Tahoma", 9F);
            this.MnuFile.Name = "MnuFile";
            this.MnuFile.Size = new System.Drawing.Size(36, 20);
            this.MnuFile.Text = "File";
            // 
            // MnuStampaDiProva
            // 
            this.MnuStampaDiProva.Name = "MnuStampaDiProva";
            this.MnuStampaDiProva.Size = new System.Drawing.Size(162, 22);
            this.MnuStampaDiProva.Text = "Stampa di &prova";
            this.MnuStampaDiProva.Click += new System.EventHandler(this.MnuStampaDiProva_Click);
            // 
            // MnuExit
            // 
            this.MnuExit.Name = "MnuExit";
            this.MnuExit.Size = new System.Drawing.Size(162, 22);
            this.MnuExit.Text = "E&sci";
            this.MnuExit.Click += new System.EventHandler(this.FormClose_Click);
            // 
            // MnuVisualizza
            // 
            this.MnuVisualizza.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MnuVisIncassoOggi,
            this.MnuVisIncassoAltraData,
            this.N1,
            this.MnuVisOrdiniOggi,
            this.MnuVisOrdiniAltraData,
            this.N2,
            this.MnuAuxWindow2,
            this.MnuAuxWindow3,
            this.N3,
            this.MnuEsploraDB,
            this.N4,
            this.MnuVisGruppi,
            this.MnuVisOrdini});
            this.MnuVisualizza.Font = new System.Drawing.Font("Tahoma", 9F);
            this.MnuVisualizza.Name = "MnuVisualizza";
            this.MnuVisualizza.Size = new System.Drawing.Size(67, 20);
            this.MnuVisualizza.Text = "Visualizza";
            // 
            // MnuVisIncassoOggi
            // 
            this.MnuVisIncassoOggi.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MnuVisIncassoOggi.Name = "MnuVisIncassoOggi";
            this.MnuVisIncassoOggi.Size = new System.Drawing.Size(237, 22);
            this.MnuVisIncassoOggi.Text = "&Incasso in data corrente";
            this.MnuVisIncassoOggi.Click += new System.EventHandler(this.MnuVisIncassoOggi_Click);
            // 
            // MnuVisIncassoAltraData
            // 
            this.MnuVisIncassoAltraData.Font = new System.Drawing.Font("Tahoma", 9F);
            this.MnuVisIncassoAltraData.Name = "MnuVisIncassoAltraData";
            this.MnuVisIncassoAltraData.Size = new System.Drawing.Size(237, 22);
            this.MnuVisIncassoAltraData.Text = "Incasso in altra &data";
            this.MnuVisIncassoAltraData.Click += new System.EventHandler(this.MnuVisIncassoAltraData_Click);
            // 
            // N1
            // 
            this.N1.Name = "N1";
            this.N1.Size = new System.Drawing.Size(234, 6);
            // 
            // MnuVisOrdiniOggi
            // 
            this.MnuVisOrdiniOggi.Name = "MnuVisOrdiniOggi";
            this.MnuVisOrdiniOggi.Size = new System.Drawing.Size(237, 22);
            this.MnuVisOrdiniOggi.Text = "&Ordini emessi in data corrente";
            this.MnuVisOrdiniOggi.Click += new System.EventHandler(this.MnuVisOrdiniOggi_Click);
            // 
            // MnuVisOrdiniAltraData
            // 
            this.MnuVisOrdiniAltraData.Name = "MnuVisOrdiniAltraData";
            this.MnuVisOrdiniAltraData.Size = new System.Drawing.Size(237, 22);
            this.MnuVisOrdiniAltraData.Text = "O&rdini emessi in altra data";
            this.MnuVisOrdiniAltraData.Click += new System.EventHandler(this.MnuVisOrdiniAltraData_Click);
            // 
            // N2
            // 
            this.N2.Name = "N2";
            this.N2.Size = new System.Drawing.Size(234, 6);
            // 
            // MnuAuxWindow2
            // 
            this.MnuAuxWindow2.Font = new System.Drawing.Font("Tahoma", 9F);
            this.MnuAuxWindow2.Name = "MnuAuxWindow2";
            this.MnuAuxWindow2.Size = new System.Drawing.Size(237, 22);
            this.MnuAuxWindow2.Text = "Finestra Aux su &2° monitor";
            this.MnuAuxWindow2.Click += new System.EventHandler(this.MnuAuxWindow_Click);
            // 
            // MnuAuxWindow3
            // 
            this.MnuAuxWindow3.Font = new System.Drawing.Font("Tahoma", 9F);
            this.MnuAuxWindow3.Name = "MnuAuxWindow3";
            this.MnuAuxWindow3.Size = new System.Drawing.Size(237, 22);
            this.MnuAuxWindow3.Text = "Finestra Aux su &3° monitor";
            this.MnuAuxWindow3.Click += new System.EventHandler(this.MnuAuxWindow3_Click);
            // 
            // N3
            // 
            this.N3.Name = "N3";
            this.N3.Size = new System.Drawing.Size(234, 6);
            // 
            // MnuEsploraDB
            // 
            this.MnuEsploraDB.Font = new System.Drawing.Font("Tahoma", 9F);
            this.MnuEsploraDB.Name = "MnuEsploraDB";
            this.MnuEsploraDB.Size = new System.Drawing.Size(237, 22);
            this.MnuEsploraDB.Text = "&Esplora DB";
            this.MnuEsploraDB.Click += new System.EventHandler(this.MnuEsploraDB_Click);
            // 
            // N4
            // 
            this.N4.Name = "N4";
            this.N4.Size = new System.Drawing.Size(234, 6);
            // 
            // MnuVisGruppi
            // 
            this.MnuVisGruppi.Font = new System.Drawing.Font("Tahoma", 9F);
            this.MnuVisGruppi.Name = "MnuVisGruppi";
            this.MnuVisGruppi.Size = new System.Drawing.Size(237, 22);
            this.MnuVisGruppi.Text = "&Colonna ID Gruppi";
            this.MnuVisGruppi.Click += new System.EventHandler(this.MnuVisGruppi_Click);
            // 
            // MnuVisOrdini
            // 
            this.MnuVisOrdini.Font = new System.Drawing.Font("Tahoma", 9F);
            this.MnuVisOrdini.Name = "MnuVisOrdini";
            this.MnuVisOrdini.Size = new System.Drawing.Size(237, 22);
            this.MnuVisOrdini.Text = "Visualizza tabella scarico &Ordini";
            this.MnuVisOrdini.Click += new System.EventHandler(this.MnuVisOrdini_Click);
            // 
            // MnuImpostazioni
            // 
            this.MnuImpostazioni.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MnuEsperto,
            this.MnuConfigurazioneStampe,
            this.N5,
            this.MnuDBServer,
            this.MnuFiltro});
            this.MnuImpostazioni.Font = new System.Drawing.Font("Tahoma", 9F);
            this.MnuImpostazioni.Name = "MnuImpostazioni";
            this.MnuImpostazioni.Size = new System.Drawing.Size(63, 20);
            this.MnuImpostazioni.Text = "Imposta";
            // 
            // MnuEsperto
            // 
            this.MnuEsperto.Name = "MnuEsperto";
            this.MnuEsperto.Size = new System.Drawing.Size(244, 22);
            this.MnuEsperto.Text = "&Modo Esperto";
            this.MnuEsperto.Click += new System.EventHandler(this.MnuEspertoClick);
            // 
            // MnuConfigurazioneStampe
            // 
            this.MnuConfigurazioneStampe.Name = "MnuConfigurazioneStampe";
            this.MnuConfigurazioneStampe.Size = new System.Drawing.Size(244, 22);
            this.MnuConfigurazioneStampe.Text = "Configurazione &Stampe ...";
            this.MnuConfigurazioneStampe.Click += new System.EventHandler(this.MnuConfigurazioneStampe_Click);
            // 
            // N5
            // 
            this.N5.Name = "N5";
            this.N5.Size = new System.Drawing.Size(241, 6);
            // 
            // MnuDBServer
            // 
            this.MnuDBServer.Name = "MnuDBServer";
            this.MnuDBServer.Size = new System.Drawing.Size(244, 22);
            this.MnuDBServer.Text = "Configurazione &Rete e Copie ...";
            this.MnuDBServer.Click += new System.EventHandler(this.MnuDBServer_Click);
            // 
            // MnuFiltro
            // 
            this.MnuFiltro.Name = "MnuFiltro";
            this.MnuFiltro.Size = new System.Drawing.Size(244, 22);
            this.MnuFiltro.Text = "Fil&tro ...";
            this.MnuFiltro.Click += new System.EventHandler(this.MnuFiltro_Click);
            // 
            // MnuAiuto
            // 
            this.MnuAiuto.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MnuAiutoRapido,
            this.MnuAbout});
            this.MnuAiuto.Font = new System.Drawing.Font("Tahoma", 9F);
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
            // MnuAbout
            // 
            this.MnuAbout.Name = "MnuAbout";
            this.MnuAbout.Size = new System.Drawing.Size(180, 22);
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
            this.lblElapsedTime.Location = new System.Drawing.Point(727, 517);
            this.lblElapsedTime.Name = "lblElapsedTime";
            this.lblElapsedTime.Size = new System.Drawing.Size(38, 16);
            this.lblElapsedTime.TabIndex = 5;
            this.lblElapsedTime.Text = "50ms";
            // 
            // btnAnt
            // 
            this.btnAnt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAnt.BackColor = System.Drawing.Color.White;
            this.btnAnt.ImageIndex = 1;
            this.btnAnt.ImageList = this.BtnImgList;
            this.btnAnt.Location = new System.Drawing.Point(668, 486);
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
            this.ClientSize = new System.Drawing.Size(785, 550);
            this.Controls.Add(this.lblElapsedTime);
            this.Controls.Add(this.btnAnt);
            this.Controls.Add(this.LabelClock);
            this.Controls.Add(this.LabelNumScontrino);
            this.Controls.Add(this.DBGrid);
            this.Controls.Add(this.MainMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.MainMenu;
            this.MinimumSize = new System.Drawing.Size(640, 480);
            this.Name = "FrmMain";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "StandMonitor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.SizeChanged += new System.EventHandler(this.FrmMain_SizeChanged);
            this.Resize += new System.EventHandler(this.FormResize);
            ((System.ComponentModel.ISupportInitialize)(this.DBGrid)).EndInit();
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView DBGrid;
        private System.Windows.Forms.Timer Timer;
        private System.Windows.Forms.Label LabelNumScontrino;
        private System.Windows.Forms.Label LabelClock;
        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.ToolStripMenuItem MnuFile;
        private System.Windows.Forms.ToolStripMenuItem MnuImpostazioni;
        private System.Windows.Forms.ToolStripMenuItem MnuVisualizza;
        private System.Windows.Forms.ToolStripMenuItem MnuAiuto;
        private System.Windows.Forms.ToolStripMenuItem MnuEsperto;
        private System.Windows.Forms.ToolStripMenuItem MnuDBServer;
        private System.Windows.Forms.ToolStripMenuItem MnuVisGruppi;
        private System.Windows.Forms.ToolStripMenuItem MnuAbout;
        private System.Windows.Forms.ToolStripMenuItem MnuExit;
        private System.Windows.Forms.ImageList BtnImgList;
        private System.Windows.Forms.Button btnAnt;
        private System.Windows.Forms.ToolStripMenuItem MnuVisOrdini;
        private System.Windows.Forms.Label lblElapsedTime;
        private System.Windows.Forms.ToolStripSeparator N2;
        private System.Windows.Forms.ToolStripMenuItem MnuAuxWindow2;
        private System.Windows.Forms.ToolStripSeparator N3;
        private System.Windows.Forms.ToolStripMenuItem MnuConfigurazioneStampe;
        private System.Windows.Forms.ToolStripSeparator N5;
        private System.Windows.Forms.ToolStripMenuItem MnuVisIncassoOggi;
        private System.Windows.Forms.ToolStripMenuItem MnuVisIncassoAltraData;
        private System.Windows.Forms.ToolStripMenuItem MnuEsploraDB;
        private System.Windows.Forms.ToolStripSeparator N4;
        private System.Windows.Forms.ToolStripMenuItem MnuStampaDiProva;
        private System.Windows.Forms.ToolStripMenuItem MnuFiltro;
        private System.Windows.Forms.ToolStripSeparator N1;
        private System.Windows.Forms.ToolStripMenuItem MnuVisOrdiniAltraData;
        private System.Windows.Forms.ToolStripMenuItem MnuVisOrdiniOggi;
        private System.Windows.Forms.ToolStripMenuItem MnuAiutoRapido;
        private System.Windows.Forms.ToolStripMenuItem MnuAuxWindow3;
    }
}


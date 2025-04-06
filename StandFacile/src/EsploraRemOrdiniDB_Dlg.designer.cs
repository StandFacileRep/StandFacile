namespace StandFacile
{
    partial class EsploraRemOrdiniDB_Dlg
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EsploraRemOrdiniDB_Dlg));
            this.dbGrid = new System.Windows.Forms.DataGridView();
            this.BtnRem_Load = new System.Windows.Forms.Button();
            this.topPanel = new System.Windows.Forms.Panel();
            this.warn_lbl = new System.Windows.Forms.Label();
            this.DBR_lbl = new System.Windows.Forms.Label();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.dbConnStatusBox = new System.Windows.Forms.PictureBox();
            this.ckBoxAutoLoad = new System.Windows.Forms.CheckBox();
            this.labelQueryTime = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioBtn2 = new System.Windows.Forms.RadioButton();
            this.radioBtn1 = new System.Windows.Forms.RadioButton();
            this.radioBtn0 = new System.Windows.Forms.RadioButton();
            this.BtnRem_Canc = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dbGrid)).BeginInit();
            this.topPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dbConnStatusBox)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dbGrid
            // 
            this.dbGrid.AllowUserToAddRows = false;
            this.dbGrid.AllowUserToDeleteRows = false;
            this.dbGrid.AllowUserToResizeRows = false;
            this.dbGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dbGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dbGrid.BackgroundColor = System.Drawing.Color.Teal;
            this.dbGrid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dbGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 9.75F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlDarkDark;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dbGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dbGrid.ColumnHeadersHeight = 26;
            this.dbGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Teal;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 12F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dbGrid.DefaultCellStyle = dataGridViewCellStyle2;
            this.dbGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dbGrid.EnableHeadersVisualStyles = false;
            this.dbGrid.GridColor = System.Drawing.Color.Gainsboro;
            this.dbGrid.Location = new System.Drawing.Point(12, 94);
            this.dbGrid.Name = "dbGrid";
            this.dbGrid.RowHeadersVisible = false;
            this.dbGrid.RowHeadersWidth = 10;
            this.dbGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dbGrid.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dbGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dbGrid.ShowCellToolTips = false;
            this.dbGrid.Size = new System.Drawing.Size(600, 348);
            this.dbGrid.TabIndex = 13;
            this.dbGrid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dbGrid_CellDoubleClick);
            // 
            // BtnRem_Load
            // 
            this.BtnRem_Load.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnRem_Load.Image = global::StandFacile.Properties.Resources.ArrowR;
            this.BtnRem_Load.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnRem_Load.Location = new System.Drawing.Point(497, 461);
            this.BtnRem_Load.Name = "BtnRem_Load";
            this.BtnRem_Load.Size = new System.Drawing.Size(104, 33);
            this.BtnRem_Load.TabIndex = 0;
            this.BtnRem_Load.Text = "carica Ordine";
            this.BtnRem_Load.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnRem_Load.UseVisualStyleBackColor = true;
            this.BtnRem_Load.Click += new System.EventHandler(this.BTR_Load_Click);
            // 
            // topPanel
            // 
            this.topPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.topPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.topPanel.Controls.Add(this.warn_lbl);
            this.topPanel.Controls.Add(this.DBR_lbl);
            this.topPanel.Location = new System.Drawing.Point(18, 17);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(594, 66);
            this.topPanel.TabIndex = 9;
            // 
            // warn_lbl
            // 
            this.warn_lbl.AutoSize = true;
            this.warn_lbl.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.warn_lbl.Location = new System.Drawing.Point(16, 9);
            this.warn_lbl.Name = "warn_lbl";
            this.warn_lbl.Size = new System.Drawing.Size(433, 14);
            this.warn_lbl.TabIndex = 1;
            this.warn_lbl.Text = "Attenzione: questa form accede al web-server e può provocare rallentamenti.";
            // 
            // DBR_lbl
            // 
            this.DBR_lbl.AutoSize = true;
            this.DBR_lbl.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DBR_lbl.Location = new System.Drawing.Point(16, 33);
            this.DBR_lbl.Name = "DBR_lbl";
            this.DBR_lbl.Size = new System.Drawing.Size(337, 14);
            this.DBR_lbl.TabIndex = 0;
            this.DBR_lbl.Text = "Con doppio click si carica nella griglia l\'ordine web selezionato";
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Interval = 250;
            this.timer.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // dbConnStatusBox
            // 
            this.dbConnStatusBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.dbConnStatusBox.ErrorImage = global::StandFacile.Properties.Resources.Cancel;
            this.dbConnStatusBox.Image = global::StandFacile.Properties.Resources.circleRed;
            this.dbConnStatusBox.Location = new System.Drawing.Point(283, 459);
            this.dbConnStatusBox.Name = "dbConnStatusBox";
            this.dbConnStatusBox.Size = new System.Drawing.Size(47, 44);
            this.dbConnStatusBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.dbConnStatusBox.TabIndex = 25;
            this.dbConnStatusBox.TabStop = false;
            // 
            // ckBoxAutoLoad
            // 
            this.ckBoxAutoLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ckBoxAutoLoad.AutoSize = true;
            this.ckBoxAutoLoad.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ckBoxAutoLoad.Location = new System.Drawing.Point(18, 469);
            this.ckBoxAutoLoad.Name = "ckBoxAutoLoad";
            this.ckBoxAutoLoad.Size = new System.Drawing.Size(248, 18);
            this.ckBoxAutoLoad.TabIndex = 27;
            this.ckBoxAutoLoad.Text = "caricamento automatico ordini autorizzati";
            this.ckBoxAutoLoad.UseVisualStyleBackColor = true;
            this.ckBoxAutoLoad.CheckedChanged += new System.EventHandler(this.CheckBoxAuto_CheckedChanged);
            // 
            // labelQueryTime
            // 
            this.labelQueryTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelQueryTime.AutoSize = true;
            this.labelQueryTime.Font = new System.Drawing.Font("Tahoma", 9F);
            this.labelQueryTime.Location = new System.Drawing.Point(284, 525);
            this.labelQueryTime.Name = "labelQueryTime";
            this.labelQueryTime.Size = new System.Drawing.Size(130, 14);
            this.labelQueryTime.TabIndex = 28;
            this.labelQueryTime.Text = "risposta server: xxx ms";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.radioBtn2);
            this.groupBox1.Controls.Add(this.radioBtn1);
            this.groupBox1.Controls.Add(this.radioBtn0);
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9F);
            this.groupBox1.Location = new System.Drawing.Point(18, 505);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(248, 44);
            this.groupBox1.TabIndex = 29;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "filtro";
            // 
            // radioBtn2
            // 
            this.radioBtn2.AutoSize = true;
            this.radioBtn2.Location = new System.Drawing.Point(162, 16);
            this.radioBtn2.Name = "radioBtn2";
            this.radioBtn2.Size = new System.Drawing.Size(79, 18);
            this.radioBtn2.TabIndex = 2;
            this.radioBtn2.TabStop = true;
            this.radioBtn2.Text = "autorizzati";
            this.radioBtn2.UseVisualStyleBackColor = true;
            this.radioBtn2.CheckedChanged += new System.EventHandler(this.RadioBtn_CheckedChanged);
            // 
            // radioBtn1
            // 
            this.radioBtn1.AutoSize = true;
            this.radioBtn1.Location = new System.Drawing.Point(74, 16);
            this.radioBtn1.Name = "radioBtn1";
            this.radioBtn1.Size = new System.Drawing.Size(76, 18);
            this.radioBtn1.TabIndex = 1;
            this.radioBtn1.TabStop = true;
            this.radioBtn1.Text = "pre-ordini";
            this.radioBtn1.UseVisualStyleBackColor = true;
            this.radioBtn1.CheckedChanged += new System.EventHandler(this.RadioBtn_CheckedChanged);
            // 
            // radioBtn0
            // 
            this.radioBtn0.AutoSize = true;
            this.radioBtn0.Checked = true;
            this.radioBtn0.Location = new System.Drawing.Point(10, 16);
            this.radioBtn0.Name = "radioBtn0";
            this.radioBtn0.Size = new System.Drawing.Size(49, 18);
            this.radioBtn0.TabIndex = 0;
            this.radioBtn0.TabStop = true;
            this.radioBtn0.Text = "tutti";
            this.radioBtn0.UseVisualStyleBackColor = true;
            this.radioBtn0.CheckedChanged += new System.EventHandler(this.RadioBtn_CheckedChanged);
            // 
            // BtnRem_Canc
            // 
            this.BtnRem_Canc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnRem_Canc.Image = ((System.Drawing.Image)(resources.GetObject("BtnRem_Canc.Image")));
            this.BtnRem_Canc.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnRem_Canc.Location = new System.Drawing.Point(349, 461);
            this.BtnRem_Canc.Name = "BtnRem_Canc";
            this.BtnRem_Canc.Size = new System.Drawing.Size(112, 33);
            this.BtnRem_Canc.TabIndex = 30;
            this.BtnRem_Canc.Text = " annulla Ordine";
            this.BtnRem_Canc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnRem_Canc.UseVisualStyleBackColor = true;
            this.BtnRem_Canc.Click += new System.EventHandler(this.BtnRem_Canc_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Image = global::StandFacile.Properties.Resources.Cancel;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(521, 516);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 33);
            this.btnCancel.TabIndex = 31;
            this.btnCancel.Text = "Esci";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // EsploraRemOrdiniDB_Dlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 562);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.BtnRem_Canc);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.labelQueryTime);
            this.Controls.Add(this.ckBoxAutoLoad);
            this.Controls.Add(this.dbConnStatusBox);
            this.Controls.Add(this.dbGrid);
            this.Controls.Add(this.BtnRem_Load);
            this.Controls.Add(this.topPanel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(640, 600);
            this.Name = "EsploraRemOrdiniDB_Dlg";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "Esplora Ordini web";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EsploraRemOrdiniDB_Dlg_FormClosing);
            this.Resize += new System.EventHandler(this.FormEsplora_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.dbGrid)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dbConnStatusBox)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dbGrid;
        private System.Windows.Forms.Button BtnRem_Load;
        private System.Windows.Forms.Panel topPanel;
        private System.Windows.Forms.Label DBR_lbl;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.PictureBox dbConnStatusBox;
        private System.Windows.Forms.CheckBox ckBoxAutoLoad;
        private System.Windows.Forms.Label labelQueryTime;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioBtn0;
        private System.Windows.Forms.RadioButton radioBtn2;
        private System.Windows.Forms.RadioButton radioBtn1;
        private System.Windows.Forms.Button BtnRem_Canc;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label warn_lbl;
    }
}
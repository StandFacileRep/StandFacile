namespace StandFacile
{
    partial class WinPrinterDlg
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
            this.PrintersListCombo = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.BtnTicketsFontSelect = new System.Windows.Forms.Button();
            this.BtnLogoFileSelect = new System.Windows.Forms.Button();
            this.BtnReportsFontSelect = new System.Windows.Forms.Button();
            this.BtnDeleteLogo = new System.Windows.Forms.Button();
            this.SampleTextBtn = new System.Windows.Forms.Button();
            this.Memo = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lblLogo = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.T_Edit = new System.Windows.Forms.TextBox();
            this.R_Edit = new System.Windows.Forms.TextBox();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.logoImage = new System.Windows.Forms.PictureBox();
            this.checkBox_A5_paper = new System.Windows.Forms.CheckBox();
            this.numUpDownTicket = new System.Windows.Forms.NumericUpDown();
            this.numUpDownReports = new System.Windows.Forms.NumericUpDown();
            this.numUpDownLogo = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.labelZoomLogo = new System.Windows.Forms.Label();
            this.checkBox_Chars33 = new System.Windows.Forms.CheckBox();
            this.checkBox_LogoNelleCopie = new System.Windows.Forms.CheckBox();
            this.checkBox_CopertiNelleCopie = new System.Windows.Forms.CheckBox();
            this.labelA5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.logoImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownTicket)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownReports)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // PrintersListCombo
            // 
            this.PrintersListCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PrintersListCombo.FormattingEnabled = true;
            this.PrintersListCombo.Location = new System.Drawing.Point(26, 45);
            this.PrintersListCombo.Name = "PrintersListCombo";
            this.PrintersListCombo.Size = new System.Drawing.Size(200, 21);
            this.PrintersListCombo.TabIndex = 1;
            this.PrintersListCombo.SelectedIndexChanged += new System.EventHandler(this.PrintersListCombo_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(23, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "Elenco Stampanti :";
            // 
            // BtnTicketsFontSelect
            // 
            this.BtnTicketsFontSelect.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnTicketsFontSelect.Location = new System.Drawing.Point(26, 102);
            this.BtnTicketsFontSelect.Name = "BtnTicketsFontSelect";
            this.BtnTicketsFontSelect.Size = new System.Drawing.Size(141, 23);
            this.BtnTicketsFontSelect.TabIndex = 4;
            this.BtnTicketsFontSelect.Text = "Scegli &Font scontrini ...";
            this.BtnTicketsFontSelect.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnTicketsFontSelect.UseVisualStyleBackColor = true;
            this.BtnTicketsFontSelect.Click += new System.EventHandler(this.BtnTicketsFontSelect_Click);
            // 
            // BtnLogoFileSelect
            // 
            this.BtnLogoFileSelect.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnLogoFileSelect.Location = new System.Drawing.Point(26, 349);
            this.BtnLogoFileSelect.Name = "BtnLogoFileSelect";
            this.BtnLogoFileSelect.Size = new System.Drawing.Size(141, 23);
            this.BtnLogoFileSelect.TabIndex = 10;
            this.BtnLogoFileSelect.Text = "Scegli il File del &Logo";
            this.BtnLogoFileSelect.UseVisualStyleBackColor = true;
            this.BtnLogoFileSelect.Click += new System.EventHandler(this.BtnLogoFileSelect_Click);
            // 
            // BtnReportsFontSelect
            // 
            this.BtnReportsFontSelect.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnReportsFontSelect.Location = new System.Drawing.Point(26, 205);
            this.BtnReportsFontSelect.Name = "BtnReportsFontSelect";
            this.BtnReportsFontSelect.Size = new System.Drawing.Size(141, 23);
            this.BtnReportsFontSelect.TabIndex = 7;
            this.BtnReportsFontSelect.Text = "Scegli F&ont riepiloghi ...";
            this.BtnReportsFontSelect.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnReportsFontSelect.UseVisualStyleBackColor = true;
            this.BtnReportsFontSelect.Click += new System.EventHandler(this.BtnReportsFontSelect_Click);
            // 
            // BtnDeleteLogo
            // 
            this.BtnDeleteLogo.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnDeleteLogo.Image = global::StandFacile.Properties.Resources.Cancel;
            this.BtnDeleteLogo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnDeleteLogo.Location = new System.Drawing.Point(26, 392);
            this.BtnDeleteLogo.Name = "BtnDeleteLogo";
            this.BtnDeleteLogo.Size = new System.Drawing.Size(141, 23);
            this.BtnDeleteLogo.TabIndex = 11;
            this.BtnDeleteLogo.Text = "    Elimina Logo";
            this.BtnDeleteLogo.UseVisualStyleBackColor = true;
            this.BtnDeleteLogo.Click += new System.EventHandler(this.BtnDeleteLogo_Click);
            // 
            // SampleTextBtn
            // 
            this.SampleTextBtn.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SampleTextBtn.Location = new System.Drawing.Point(26, 435);
            this.SampleTextBtn.Name = "SampleTextBtn";
            this.SampleTextBtn.Size = new System.Drawing.Size(141, 23);
            this.SampleTextBtn.TabIndex = 13;
            this.SampleTextBtn.Text = "stampa testo di prova";
            this.SampleTextBtn.UseVisualStyleBackColor = true;
            this.SampleTextBtn.Click += new System.EventHandler(this.SampleTextBtn_Click);
            // 
            // Memo
            // 
            this.Memo.AcceptsReturn = true;
            this.Memo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Memo.Location = new System.Drawing.Point(276, 45);
            this.Memo.Multiline = true;
            this.Memo.Name = "Memo";
            this.Memo.ReadOnly = true;
            this.Memo.Size = new System.Drawing.Size(250, 219);
            this.Memo.TabIndex = 3;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Image = global::StandFacile.Properties.Resources.Cancel;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(159, 576);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(81, 28);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "Annulla";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.Image = global::StandFacile.Properties.Resources.OK;
            this.btnOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOK.Location = new System.Drawing.Point(302, 576);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(72, 28);
            this.btnOK.TabIndex = 15;
            this.btnOK.Text = "OK";
            this.btnOK.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(273, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(144, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Riepilogo Impostazioni :";
            // 
            // lblLogo
            // 
            this.lblLogo.AutoSize = true;
            this.lblLogo.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLogo.Location = new System.Drawing.Point(273, 284);
            this.lblLogo.Name = "lblLogo";
            this.lblLogo.Size = new System.Drawing.Size(106, 16);
            this.lblLogo.TabIndex = 12;
            this.lblLogo.Text = "Anteprima Logo :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 9F);
            this.label4.Location = new System.Drawing.Point(34, 150);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(123, 14);
            this.label4.TabIndex = 5;
            this.label4.Text = "Margine SX (0,1mm):";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(34, 259);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(123, 14);
            this.label5.TabIndex = 8;
            this.label5.Text = "Margine SX (0,1mm):";
            // 
            // T_Edit
            // 
            this.T_Edit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.T_Edit.Location = new System.Drawing.Point(181, 146);
            this.T_Edit.MaxLength = 3;
            this.T_Edit.Name = "T_Edit";
            this.T_Edit.Size = new System.Drawing.Size(45, 22);
            this.T_Edit.TabIndex = 6;
            // 
            // R_Edit
            // 
            this.R_Edit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.R_Edit.Location = new System.Drawing.Point(181, 255);
            this.R_Edit.MaxLength = 3;
            this.R_Edit.Name = "R_Edit";
            this.R_Edit.Size = new System.Drawing.Size(45, 22);
            this.R_Edit.TabIndex = 9;
            // 
            // timer
            // 
            this.timer.Interval = 2000;
            this.timer.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // logoImage
            // 
            this.logoImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.logoImage.Location = new System.Drawing.Point(276, 309);
            this.logoImage.Name = "logoImage";
            this.logoImage.Size = new System.Drawing.Size(250, 150);
            this.logoImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.logoImage.TabIndex = 17;
            this.logoImage.TabStop = false;
            // 
            // checkBox_A5_paper
            // 
            this.checkBox_A5_paper.AutoSize = true;
            this.checkBox_A5_paper.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox_A5_paper.Location = new System.Drawing.Point(26, 523);
            this.checkBox_A5_paper.Name = "checkBox_A5_paper";
            this.checkBox_A5_paper.Size = new System.Drawing.Size(108, 18);
            this.checkBox_A5_paper.TabIndex = 24;
            this.checkBox_A5_paper.Text = "uso di carta A5";
            this.checkBox_A5_paper.UseVisualStyleBackColor = true;
            // 
            // numUpDownTicket
            // 
            this.numUpDownTicket.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numUpDownTicket.Location = new System.Drawing.Point(181, 105);
            this.numUpDownTicket.Maximum = new decimal(new int[] {
            140,
            0,
            0,
            0});
            this.numUpDownTicket.Minimum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numUpDownTicket.Name = "numUpDownTicket";
            this.numUpDownTicket.Size = new System.Drawing.Size(45, 20);
            this.numUpDownTicket.TabIndex = 25;
            this.numUpDownTicket.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // numUpDownReports
            // 
            this.numUpDownReports.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numUpDownReports.Location = new System.Drawing.Point(181, 207);
            this.numUpDownReports.Maximum = new decimal(new int[] {
            140,
            0,
            0,
            0});
            this.numUpDownReports.Minimum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numUpDownReports.Name = "numUpDownReports";
            this.numUpDownReports.Size = new System.Drawing.Size(45, 20);
            this.numUpDownReports.TabIndex = 26;
            this.numUpDownReports.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // numUpDownLogo
            // 
            this.numUpDownLogo.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numUpDownLogo.Location = new System.Drawing.Point(181, 316);
            this.numUpDownLogo.Maximum = new decimal(new int[] {
            140,
            0,
            0,
            0});
            this.numUpDownLogo.Minimum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numUpDownLogo.Name = "numUpDownLogo";
            this.numUpDownLogo.Size = new System.Drawing.Size(45, 20);
            this.numUpDownLogo.TabIndex = 27;
            this.numUpDownLogo.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(178, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 16);
            this.label3.TabIndex = 28;
            this.label3.Text = "Zoom %";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(178, 188);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(55, 16);
            this.label6.TabIndex = 29;
            this.label6.Text = "Zoom %";
            // 
            // labelZoomLogo
            // 
            this.labelZoomLogo.AutoSize = true;
            this.labelZoomLogo.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelZoomLogo.Location = new System.Drawing.Point(90, 297);
            this.labelZoomLogo.Name = "labelZoomLogo";
            this.labelZoomLogo.Size = new System.Drawing.Size(140, 16);
            this.labelZoomLogo.TabIndex = 30;
            this.labelZoomLogo.Text = "Logo, Barcode Zoom %";
            // 
            // checkBox_Chars33
            // 
            this.checkBox_Chars33.AutoSize = true;
            this.checkBox_Chars33.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox_Chars33.Location = new System.Drawing.Point(26, 486);
            this.checkBox_Chars33.Name = "checkBox_Chars33";
            this.checkBox_Chars33.Size = new System.Drawing.Size(223, 18);
            this.checkBox_Chars33.TabIndex = 31;
            this.checkBox_Chars33.Text = "articoli su 23 caratteri (invece di 18)";
            this.checkBox_Chars33.UseVisualStyleBackColor = true;
            // 
            // checkBox_LogoNelleCopie
            // 
            this.checkBox_LogoNelleCopie.AutoSize = true;
            this.checkBox_LogoNelleCopie.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox_LogoNelleCopie.Location = new System.Drawing.Point(276, 523);
            this.checkBox_LogoNelleCopie.Name = "checkBox_LogoNelleCopie";
            this.checkBox_LogoNelleCopie.Size = new System.Drawing.Size(196, 18);
            this.checkBox_LogoNelleCopie.TabIndex = 72;
            this.checkBox_LogoNelleCopie.Text = "stampa il Logo in tutte le copie";
            this.checkBox_LogoNelleCopie.UseVisualStyleBackColor = true;
            // 
            // checkBox_CopertiNelleCopie
            // 
            this.checkBox_CopertiNelleCopie.AutoSize = true;
            this.checkBox_CopertiNelleCopie.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox_CopertiNelleCopie.Location = new System.Drawing.Point(276, 486);
            this.checkBox_CopertiNelleCopie.Name = "checkBox_CopertiNelleCopie";
            this.checkBox_CopertiNelleCopie.Size = new System.Drawing.Size(205, 18);
            this.checkBox_CopertiNelleCopie.TabIndex = 73;
            this.checkBox_CopertiNelleCopie.Text = "stampa i coperti in tutte le copie";
            this.checkBox_CopertiNelleCopie.UseVisualStyleBackColor = true;
            // 
            // labelA5
            // 
            this.labelA5.AutoSize = true;
            this.labelA5.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelA5.ForeColor = System.Drawing.SystemColors.Highlight;
            this.labelA5.Location = new System.Drawing.Point(43, 544);
            this.labelA5.Name = "labelA5";
            this.labelA5.Size = new System.Drawing.Size(187, 13);
            this.labelA5.TabIndex = 74;
            this.labelA5.Text = "evita righe aggiuntive per taglio carta";
            // 
            // WinPrinterDlg
            // 
            this.AcceptButton = this.btnCancel;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(543, 620);
            this.Controls.Add(this.labelA5);
            this.Controls.Add(this.checkBox_CopertiNelleCopie);
            this.Controls.Add(this.checkBox_LogoNelleCopie);
            this.Controls.Add(this.checkBox_Chars33);
            this.Controls.Add(this.labelZoomLogo);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.numUpDownLogo);
            this.Controls.Add(this.numUpDownReports);
            this.Controls.Add(this.numUpDownTicket);
            this.Controls.Add(this.checkBox_A5_paper);
            this.Controls.Add(this.logoImage);
            this.Controls.Add(this.R_Edit);
            this.Controls.Add(this.T_Edit);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblLogo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.Memo);
            this.Controls.Add(this.SampleTextBtn);
            this.Controls.Add(this.BtnDeleteLogo);
            this.Controls.Add(this.BtnReportsFontSelect);
            this.Controls.Add(this.BtnLogoFileSelect);
            this.Controls.Add(this.BtnTicketsFontSelect);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.PrintersListCombo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WinPrinterDlg";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Impostazioni stampante Scontrino Windows";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WinPrinterDlg_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.logoImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownTicket)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownReports)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox PrintersListCombo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BtnTicketsFontSelect;
        private System.Windows.Forms.Button BtnLogoFileSelect;
        private System.Windows.Forms.Button BtnReportsFontSelect;
        private System.Windows.Forms.Button BtnDeleteLogo;
        private System.Windows.Forms.Button SampleTextBtn;
        private System.Windows.Forms.TextBox Memo;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblLogo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox T_Edit;
        private System.Windows.Forms.TextBox R_Edit;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.PictureBox logoImage;
        private System.Windows.Forms.CheckBox checkBox_A5_paper;
        private System.Windows.Forms.NumericUpDown numUpDownTicket;
        private System.Windows.Forms.NumericUpDown numUpDownReports;
        private System.Windows.Forms.NumericUpDown numUpDownLogo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label labelZoomLogo;
        private System.Windows.Forms.CheckBox checkBox_Chars33;
        private System.Windows.Forms.CheckBox checkBox_LogoNelleCopie;
        private System.Windows.Forms.CheckBox checkBox_CopertiNelleCopie;
        private System.Windows.Forms.Label labelA5;
    }
}
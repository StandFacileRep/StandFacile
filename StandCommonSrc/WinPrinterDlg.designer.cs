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
            this.PrintersListCombo = new System.Windows.Forms.ComboBox();
            this.lblPrintersList = new System.Windows.Forms.Label();
            this.BtnRcpFontSelect = new System.Windows.Forms.Button();
            this.BtnLogoFileSelect = new System.Windows.Forms.Button();
            this.BtnRepFontSelect = new System.Windows.Forms.Button();
            this.BtnDeleteLogo = new System.Windows.Forms.Button();
            this.SampleTextBtn = new System.Windows.Forms.Button();
            this.Memo = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblRecap = new System.Windows.Forms.Label();
            this.labelMarginRcp = new System.Windows.Forms.Label();
            this.labelMarginRep = new System.Windows.Forms.Label();
            this.logoImage = new System.Windows.Forms.PictureBox();
            this.numUpDownRcpZoom = new System.Windows.Forms.NumericUpDown();
            this.numUpDownRepZoom = new System.Windows.Forms.NumericUpDown();
            this.numUpDownLogoZoom = new System.Windows.Forms.NumericUpDown();
            this.labelZoomRcp = new System.Windows.Forms.Label();
            this.labelZoomRep = new System.Windows.Forms.Label();
            this.labelZoomLogo = new System.Windows.Forms.Label();
            this.checkBox_Chars33 = new System.Windows.Forms.CheckBox();
            this.checkBox_LogoNelleCopie = new System.Windows.Forms.CheckBox();
            this.checkBox_CopertiNelleCopie = new System.Windows.Forms.CheckBox();
            this.labelCenterLogo = new System.Windows.Forms.Label();
            this.numUpDown_RcpMargin = new System.Windows.Forms.NumericUpDown();
            this.numUpDown_RepMargin = new System.Windows.Forms.NumericUpDown();
            this.numUpDown_LogoCenter = new System.Windows.Forms.NumericUpDown();
            this.BtnDefaults = new System.Windows.Forms.Button();
            this.labelDefault = new System.Windows.Forms.Label();
            this.groupBoxLogoChoice = new System.Windows.Forms.GroupBox();
            this.lblLogoPreview = new System.Windows.Forms.Label();
            this.RadioBtnLogo_B = new System.Windows.Forms.RadioButton();
            this.RadioBtnLogo_T = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.logoImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownRcpZoom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownRepZoom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownLogoZoom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown_RcpMargin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown_RepMargin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown_LogoCenter)).BeginInit();
            this.groupBoxLogoChoice.SuspendLayout();
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
            // lblPrintersList
            // 
            this.lblPrintersList.AutoSize = true;
            this.lblPrintersList.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPrintersList.Location = new System.Drawing.Point(23, 19);
            this.lblPrintersList.Name = "lblPrintersList";
            this.lblPrintersList.Size = new System.Drawing.Size(110, 14);
            this.lblPrintersList.TabIndex = 0;
            this.lblPrintersList.Text = "Elenco Stampanti :";
            // 
            // BtnRcpFontSelect
            // 
            this.BtnRcpFontSelect.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnRcpFontSelect.Location = new System.Drawing.Point(26, 85);
            this.BtnRcpFontSelect.Name = "BtnRcpFontSelect";
            this.BtnRcpFontSelect.Size = new System.Drawing.Size(141, 23);
            this.BtnRcpFontSelect.TabIndex = 4;
            this.BtnRcpFontSelect.Text = "Scegli &Font scontrini ...";
            this.BtnRcpFontSelect.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnRcpFontSelect.UseVisualStyleBackColor = true;
            this.BtnRcpFontSelect.Click += new System.EventHandler(this.BtnTicketsFontSelect_Click);
            // 
            // BtnLogoFileSelect
            // 
            this.BtnLogoFileSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BtnLogoFileSelect.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnLogoFileSelect.Location = new System.Drawing.Point(26, 421);
            this.BtnLogoFileSelect.Name = "BtnLogoFileSelect";
            this.BtnLogoFileSelect.Size = new System.Drawing.Size(141, 23);
            this.BtnLogoFileSelect.TabIndex = 10;
            this.BtnLogoFileSelect.Text = "Scegli il File del &Logo";
            this.BtnLogoFileSelect.UseVisualStyleBackColor = true;
            this.BtnLogoFileSelect.Click += new System.EventHandler(this.BtnLogoFileSelect_Click);
            // 
            // BtnRepFontSelect
            // 
            this.BtnRepFontSelect.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnRepFontSelect.Location = new System.Drawing.Point(26, 188);
            this.BtnRepFontSelect.Name = "BtnRepFontSelect";
            this.BtnRepFontSelect.Size = new System.Drawing.Size(141, 23);
            this.BtnRepFontSelect.TabIndex = 7;
            this.BtnRepFontSelect.Text = "Scegli F&ont riepiloghi ...";
            this.BtnRepFontSelect.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnRepFontSelect.UseVisualStyleBackColor = true;
            this.BtnRepFontSelect.Click += new System.EventHandler(this.BtnReportsFontSelect_Click);
            // 
            // BtnDeleteLogo
            // 
            this.BtnDeleteLogo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BtnDeleteLogo.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnDeleteLogo.Image = global::StandFacile.Properties.Resources.Cancel;
            this.BtnDeleteLogo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnDeleteLogo.Location = new System.Drawing.Point(26, 464);
            this.BtnDeleteLogo.Name = "BtnDeleteLogo";
            this.BtnDeleteLogo.Size = new System.Drawing.Size(141, 23);
            this.BtnDeleteLogo.TabIndex = 11;
            this.BtnDeleteLogo.Text = "    Elimina Logo";
            this.BtnDeleteLogo.UseVisualStyleBackColor = true;
            this.BtnDeleteLogo.Click += new System.EventHandler(this.BtnDeleteLogo_Click);
            // 
            // SampleTextBtn
            // 
            this.SampleTextBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SampleTextBtn.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SampleTextBtn.Location = new System.Drawing.Point(26, 506);
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
            this.Memo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
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
            this.btnCancel.Location = new System.Drawing.Point(152, 650);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(81, 28);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "Annulla";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.Image = global::StandFacile.Properties.Resources.OK;
            this.btnOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOK.Location = new System.Drawing.Point(302, 652);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(72, 28);
            this.btnOK.TabIndex = 15;
            this.btnOK.Text = "OK";
            this.btnOK.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // lblRecap
            // 
            this.lblRecap.AutoSize = true;
            this.lblRecap.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRecap.Location = new System.Drawing.Point(273, 19);
            this.lblRecap.Name = "lblRecap";
            this.lblRecap.Size = new System.Drawing.Size(144, 16);
            this.lblRecap.TabIndex = 2;
            this.lblRecap.Text = "Riepilogo Impostazioni :";
            // 
            // labelMarginRcp
            // 
            this.labelMarginRcp.AutoSize = true;
            this.labelMarginRcp.Font = new System.Drawing.Font("Tahoma", 9F);
            this.labelMarginRcp.Location = new System.Drawing.Point(34, 130);
            this.labelMarginRcp.Name = "labelMarginRcp";
            this.labelMarginRcp.Size = new System.Drawing.Size(123, 14);
            this.labelMarginRcp.TabIndex = 5;
            this.labelMarginRcp.Text = "Margine SX (0,1mm):";
            // 
            // labelMarginRep
            // 
            this.labelMarginRep.AutoSize = true;
            this.labelMarginRep.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMarginRep.Location = new System.Drawing.Point(34, 229);
            this.labelMarginRep.Name = "labelMarginRep";
            this.labelMarginRep.Size = new System.Drawing.Size(123, 14);
            this.labelMarginRep.TabIndex = 8;
            this.labelMarginRep.Text = "Margine SX (0,1mm):";
            // 
            // logoImage
            // 
            this.logoImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.logoImage.Location = new System.Drawing.Point(276, 346);
            this.logoImage.Name = "logoImage";
            this.logoImage.Size = new System.Drawing.Size(250, 150);
            this.logoImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.logoImage.TabIndex = 17;
            this.logoImage.TabStop = false;
            // 
            // numUpDownRcpZoom
            // 
            this.numUpDownRcpZoom.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numUpDownRcpZoom.Location = new System.Drawing.Point(181, 88);
            this.numUpDownRcpZoom.Maximum = new decimal(new int[] {
            140,
            0,
            0,
            0});
            this.numUpDownRcpZoom.Minimum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numUpDownRcpZoom.Name = "numUpDownRcpZoom";
            this.numUpDownRcpZoom.Size = new System.Drawing.Size(45, 20);
            this.numUpDownRcpZoom.TabIndex = 25;
            this.numUpDownRcpZoom.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numUpDownRcpZoom.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numUpDownRcpZoom.Click += new System.EventHandler(this.NumUpDown_Click);
            // 
            // numUpDownRepZoom
            // 
            this.numUpDownRepZoom.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numUpDownRepZoom.Location = new System.Drawing.Point(181, 190);
            this.numUpDownRepZoom.Maximum = new decimal(new int[] {
            140,
            0,
            0,
            0});
            this.numUpDownRepZoom.Minimum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numUpDownRepZoom.Name = "numUpDownRepZoom";
            this.numUpDownRepZoom.Size = new System.Drawing.Size(45, 20);
            this.numUpDownRepZoom.TabIndex = 26;
            this.numUpDownRepZoom.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numUpDownRepZoom.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numUpDownRepZoom.Click += new System.EventHandler(this.NumUpDown_Click);
            // 
            // numUpDownLogoZoom
            // 
            this.numUpDownLogoZoom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.numUpDownLogoZoom.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numUpDownLogoZoom.Location = new System.Drawing.Point(181, 337);
            this.numUpDownLogoZoom.Maximum = new decimal(new int[] {
            140,
            0,
            0,
            0});
            this.numUpDownLogoZoom.Minimum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numUpDownLogoZoom.Name = "numUpDownLogoZoom";
            this.numUpDownLogoZoom.Size = new System.Drawing.Size(45, 20);
            this.numUpDownLogoZoom.TabIndex = 27;
            this.numUpDownLogoZoom.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numUpDownLogoZoom.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numUpDownLogoZoom.Click += new System.EventHandler(this.NumUpDown_Click);
            // 
            // labelZoomRcp
            // 
            this.labelZoomRcp.AutoSize = true;
            this.labelZoomRcp.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelZoomRcp.Location = new System.Drawing.Point(178, 69);
            this.labelZoomRcp.Name = "labelZoomRcp";
            this.labelZoomRcp.Size = new System.Drawing.Size(55, 16);
            this.labelZoomRcp.TabIndex = 28;
            this.labelZoomRcp.Text = "Zoom %";
            // 
            // labelZoomRep
            // 
            this.labelZoomRep.AutoSize = true;
            this.labelZoomRep.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelZoomRep.Location = new System.Drawing.Point(178, 171);
            this.labelZoomRep.Name = "labelZoomRep";
            this.labelZoomRep.Size = new System.Drawing.Size(55, 16);
            this.labelZoomRep.TabIndex = 29;
            this.labelZoomRep.Text = "Zoom %";
            // 
            // labelZoomLogo
            // 
            this.labelZoomLogo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelZoomLogo.AutoSize = true;
            this.labelZoomLogo.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelZoomLogo.Location = new System.Drawing.Point(23, 337);
            this.labelZoomLogo.Name = "labelZoomLogo";
            this.labelZoomLogo.Size = new System.Drawing.Size(140, 16);
            this.labelZoomLogo.TabIndex = 30;
            this.labelZoomLogo.Text = "Logo, Barcode Zoom %";
            // 
            // checkBox_Chars33
            // 
            this.checkBox_Chars33.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBox_Chars33.AutoSize = true;
            this.checkBox_Chars33.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox_Chars33.Location = new System.Drawing.Point(26, 572);
            this.checkBox_Chars33.Name = "checkBox_Chars33";
            this.checkBox_Chars33.Size = new System.Drawing.Size(223, 18);
            this.checkBox_Chars33.TabIndex = 31;
            this.checkBox_Chars33.Text = "articoli su 23 caratteri (invece di 18)";
            this.checkBox_Chars33.UseVisualStyleBackColor = true;
            // 
            // checkBox_LogoNelleCopie
            // 
            this.checkBox_LogoNelleCopie.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBox_LogoNelleCopie.AutoSize = true;
            this.checkBox_LogoNelleCopie.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox_LogoNelleCopie.Location = new System.Drawing.Point(276, 609);
            this.checkBox_LogoNelleCopie.Name = "checkBox_LogoNelleCopie";
            this.checkBox_LogoNelleCopie.Size = new System.Drawing.Size(203, 18);
            this.checkBox_LogoNelleCopie.TabIndex = 72;
            this.checkBox_LogoNelleCopie.Text = "stampa il Logo anche nelle copie";
            this.checkBox_LogoNelleCopie.UseVisualStyleBackColor = true;
            // 
            // checkBox_CopertiNelleCopie
            // 
            this.checkBox_CopertiNelleCopie.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBox_CopertiNelleCopie.AutoSize = true;
            this.checkBox_CopertiNelleCopie.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox_CopertiNelleCopie.Location = new System.Drawing.Point(276, 572);
            this.checkBox_CopertiNelleCopie.Name = "checkBox_CopertiNelleCopie";
            this.checkBox_CopertiNelleCopie.Size = new System.Drawing.Size(205, 18);
            this.checkBox_CopertiNelleCopie.TabIndex = 73;
            this.checkBox_CopertiNelleCopie.Text = "stampa i coperti in tutte le copie";
            this.checkBox_CopertiNelleCopie.UseVisualStyleBackColor = true;
            // 
            // labelCenterLogo
            // 
            this.labelCenterLogo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelCenterLogo.AutoSize = true;
            this.labelCenterLogo.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCenterLogo.Location = new System.Drawing.Point(19, 379);
            this.labelCenterLogo.Name = "labelCenterLogo";
            this.labelCenterLogo.Size = new System.Drawing.Size(152, 14);
            this.labelCenterLogo.TabIndex = 8;
            this.labelCenterLogo.Text = "Centro Logo (+/- 0,1mm):";
            // 
            // numUpDown_RcpMargin
            // 
            this.numUpDown_RcpMargin.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numUpDown_RcpMargin.Location = new System.Drawing.Point(181, 129);
            this.numUpDown_RcpMargin.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.numUpDown_RcpMargin.Name = "numUpDown_RcpMargin";
            this.numUpDown_RcpMargin.Size = new System.Drawing.Size(45, 20);
            this.numUpDown_RcpMargin.TabIndex = 75;
            this.numUpDown_RcpMargin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numUpDown_RcpMargin.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numUpDown_RcpMargin.Click += new System.EventHandler(this.NumUpDown_Click);
            // 
            // numUpDown_RepMargin
            // 
            this.numUpDown_RepMargin.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numUpDown_RepMargin.Location = new System.Drawing.Point(181, 228);
            this.numUpDown_RepMargin.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.numUpDown_RepMargin.Name = "numUpDown_RepMargin";
            this.numUpDown_RepMargin.Size = new System.Drawing.Size(45, 20);
            this.numUpDown_RepMargin.TabIndex = 76;
            this.numUpDown_RepMargin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numUpDown_RepMargin.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numUpDown_RepMargin.Click += new System.EventHandler(this.NumUpDown_Click);
            // 
            // numUpDown_LogoCenter
            // 
            this.numUpDown_LogoCenter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.numUpDown_LogoCenter.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numUpDown_LogoCenter.Location = new System.Drawing.Point(181, 377);
            this.numUpDown_LogoCenter.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.numUpDown_LogoCenter.Minimum = new decimal(new int[] {
            200,
            0,
            0,
            -2147483648});
            this.numUpDown_LogoCenter.Name = "numUpDown_LogoCenter";
            this.numUpDown_LogoCenter.Size = new System.Drawing.Size(45, 20);
            this.numUpDown_LogoCenter.TabIndex = 77;
            this.numUpDown_LogoCenter.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numUpDown_LogoCenter.Click += new System.EventHandler(this.NumUpDown_Click);
            // 
            // BtnDefaults
            // 
            this.BtnDefaults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BtnDefaults.Image = global::StandFacile.Properties.Resources.Cancel1;
            this.BtnDefaults.Location = new System.Drawing.Point(470, 519);
            this.BtnDefaults.Name = "BtnDefaults";
            this.BtnDefaults.Size = new System.Drawing.Size(35, 26);
            this.BtnDefaults.TabIndex = 78;
            this.BtnDefaults.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnDefaults.UseVisualStyleBackColor = true;
            this.BtnDefaults.Click += new System.EventHandler(this.BtnDefaults_Click);
            // 
            // labelDefault
            // 
            this.labelDefault.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelDefault.AutoSize = true;
            this.labelDefault.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDefault.Location = new System.Drawing.Point(273, 525);
            this.labelDefault.Name = "labelDefault";
            this.labelDefault.Size = new System.Drawing.Size(181, 14);
            this.labelDefault.TabIndex = 79;
            this.labelDefault.Text = "ripristina default Zoom e margini";
            // 
            // groupBoxLogoChoice
            // 
            this.groupBoxLogoChoice.Controls.Add(this.lblLogoPreview);
            this.groupBoxLogoChoice.Controls.Add(this.RadioBtnLogo_B);
            this.groupBoxLogoChoice.Controls.Add(this.RadioBtnLogo_T);
            this.groupBoxLogoChoice.Location = new System.Drawing.Point(276, 270);
            this.groupBoxLogoChoice.Name = "groupBoxLogoChoice";
            this.groupBoxLogoChoice.Size = new System.Drawing.Size(250, 71);
            this.groupBoxLogoChoice.TabIndex = 81;
            this.groupBoxLogoChoice.TabStop = false;
            // 
            // lblLogoPreview
            // 
            this.lblLogoPreview.AutoSize = true;
            this.lblLogoPreview.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLogoPreview.Location = new System.Drawing.Point(13, 47);
            this.lblLogoPreview.Name = "lblLogoPreview";
            this.lblLogoPreview.Size = new System.Drawing.Size(106, 16);
            this.lblLogoPreview.TabIndex = 14;
            this.lblLogoPreview.Text = "Anteprima Logo :";
            // 
            // RadioBtnLogo_B
            // 
            this.RadioBtnLogo_B.AutoSize = true;
            this.RadioBtnLogo_B.Location = new System.Drawing.Point(142, 16);
            this.RadioBtnLogo_B.Name = "RadioBtnLogo_B";
            this.RadioBtnLogo_B.Size = new System.Drawing.Size(89, 17);
            this.RadioBtnLogo_B.TabIndex = 2;
            this.RadioBtnLogo_B.TabStop = true;
            this.RadioBtnLogo_B.Text = "Logo inferiore";
            this.RadioBtnLogo_B.UseVisualStyleBackColor = true;
            this.RadioBtnLogo_B.Click += new System.EventHandler(this.RadioBtnLogo_Click);
            // 
            // RadioBtnLogo_T
            // 
            this.RadioBtnLogo_T.AutoSize = true;
            this.RadioBtnLogo_T.Checked = true;
            this.RadioBtnLogo_T.Location = new System.Drawing.Point(16, 16);
            this.RadioBtnLogo_T.Name = "RadioBtnLogo_T";
            this.RadioBtnLogo_T.Size = new System.Drawing.Size(95, 17);
            this.RadioBtnLogo_T.TabIndex = 1;
            this.RadioBtnLogo_T.TabStop = true;
            this.RadioBtnLogo_T.Text = "Logo superiore";
            this.RadioBtnLogo_T.UseVisualStyleBackColor = true;
            this.RadioBtnLogo_T.Click += new System.EventHandler(this.RadioBtnLogo_Click);
            // 
            // WinPrinterDlg
            // 
            this.AcceptButton = this.btnCancel;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(543, 691);
            this.Controls.Add(this.groupBoxLogoChoice);
            this.Controls.Add(this.labelDefault);
            this.Controls.Add(this.BtnDefaults);
            this.Controls.Add(this.numUpDown_LogoCenter);
            this.Controls.Add(this.numUpDown_RepMargin);
            this.Controls.Add(this.numUpDown_RcpMargin);
            this.Controls.Add(this.checkBox_CopertiNelleCopie);
            this.Controls.Add(this.checkBox_LogoNelleCopie);
            this.Controls.Add(this.checkBox_Chars33);
            this.Controls.Add(this.labelZoomLogo);
            this.Controls.Add(this.labelZoomRep);
            this.Controls.Add(this.labelZoomRcp);
            this.Controls.Add(this.numUpDownLogoZoom);
            this.Controls.Add(this.numUpDownRepZoom);
            this.Controls.Add(this.numUpDownRcpZoom);
            this.Controls.Add(this.logoImage);
            this.Controls.Add(this.labelCenterLogo);
            this.Controls.Add(this.labelMarginRep);
            this.Controls.Add(this.labelMarginRcp);
            this.Controls.Add(this.lblRecap);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.Memo);
            this.Controls.Add(this.SampleTextBtn);
            this.Controls.Add(this.BtnDeleteLogo);
            this.Controls.Add(this.BtnRepFontSelect);
            this.Controls.Add(this.BtnLogoFileSelect);
            this.Controls.Add(this.BtnRcpFontSelect);
            this.Controls.Add(this.lblPrintersList);
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
            this.Click += new System.EventHandler(this.NumUpDown_Click);
            ((System.ComponentModel.ISupportInitialize)(this.logoImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownRcpZoom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownRepZoom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownLogoZoom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown_RcpMargin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown_RepMargin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown_LogoCenter)).EndInit();
            this.groupBoxLogoChoice.ResumeLayout(false);
            this.groupBoxLogoChoice.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox PrintersListCombo;
        private System.Windows.Forms.Label lblPrintersList;
        private System.Windows.Forms.Button BtnRcpFontSelect;
        private System.Windows.Forms.Button BtnLogoFileSelect;
        private System.Windows.Forms.Button BtnRepFontSelect;
        private System.Windows.Forms.Button BtnDeleteLogo;
        private System.Windows.Forms.Button SampleTextBtn;
        private System.Windows.Forms.TextBox Memo;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblRecap;
        private System.Windows.Forms.Label labelMarginRcp;
        private System.Windows.Forms.Label labelMarginRep;
        private System.Windows.Forms.PictureBox logoImage;
        private System.Windows.Forms.NumericUpDown numUpDownRcpZoom;
        private System.Windows.Forms.NumericUpDown numUpDownRepZoom;
        private System.Windows.Forms.NumericUpDown numUpDownLogoZoom;
        private System.Windows.Forms.Label labelZoomRcp;
        private System.Windows.Forms.Label labelZoomRep;
        private System.Windows.Forms.Label labelZoomLogo;
        private System.Windows.Forms.CheckBox checkBox_Chars33;
        private System.Windows.Forms.CheckBox checkBox_LogoNelleCopie;
        private System.Windows.Forms.CheckBox checkBox_CopertiNelleCopie;
        private System.Windows.Forms.Label labelCenterLogo;
        private System.Windows.Forms.NumericUpDown numUpDown_RcpMargin;
        private System.Windows.Forms.NumericUpDown numUpDown_RepMargin;
        private System.Windows.Forms.NumericUpDown numUpDown_LogoCenter;
        private System.Windows.Forms.Button BtnDefaults;
        private System.Windows.Forms.Label labelDefault;
        private System.Windows.Forms.GroupBox groupBoxLogoChoice;
        private System.Windows.Forms.RadioButton RadioBtnLogo_T;
        private System.Windows.Forms.Label lblLogoPreview;
        private System.Windows.Forms.RadioButton RadioBtnLogo_B;
    }
}
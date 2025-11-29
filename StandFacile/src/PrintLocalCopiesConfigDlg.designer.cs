namespace StandFacile
{
    partial class PrintLocalCopiesConfigDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrintLocalCopiesConfigDlg));
            this.RadioGroup_PrinterType = new System.Windows.Forms.GroupBox();
            this.prt_Legacy = new System.Windows.Forms.RadioButton();
            this.prt_Windows = new System.Windows.Forms.RadioButton();
            this.printersGroupBox = new System.Windows.Forms.GroupBox();
            this.BtnGenPrinterOptions = new System.Windows.Forms.Button();
            this.BtnLegacy = new System.Windows.Forms.Button();
            this.BtnWin = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.panelLocalCopies = new System.Windows.Forms.Panel();
            this.checkBox_LocPrices = new System.Windows.Forms.CheckBox();
            this.labelWarn1 = new System.Windows.Forms.Label();
            this.checkBox_AvoidPrintGroups = new System.Windows.Forms.CheckBox();
            this.labelWarn2 = new System.Windows.Forms.Label();
            this.checkBox_CUT = new System.Windows.Forms.CheckBox();
            this.checkBoxUnitItems = new System.Windows.Forms.CheckBox();
            this.panelCopies = new System.Windows.Forms.Panel();
            this.checkBoxCopia_9 = new System.Windows.Forms.CheckBox();
            this.checkBoxCopia_8 = new System.Windows.Forms.CheckBox();
            this.labelNoWebLoad = new System.Windows.Forms.Label();
            this.checkBoxCopia_7 = new System.Windows.Forms.CheckBox();
            this.checkBoxCopia_6 = new System.Windows.Forms.CheckBox();
            this.checkBoxCopia_5 = new System.Windows.Forms.CheckBox();
            this.checkBoxCopia_4 = new System.Windows.Forms.CheckBox();
            this.checkBoxCopia_3 = new System.Windows.Forms.CheckBox();
            this.checkBoxCopia_2 = new System.Windows.Forms.CheckBox();
            this.checkBoxCopia_1 = new System.Windows.Forms.CheckBox();
            this.checkBoxCopia_0 = new System.Windows.Forms.CheckBox();
            this.checkBoxSingleRowItems = new System.Windows.Forms.CheckBox();
            this.checkBoxSelectedOnly = new System.Windows.Forms.CheckBox();
            this.checkBox_LocalCopy = new System.Windows.Forms.CheckBox();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.LinkLbl_Mnu_CCR = new System.Windows.Forms.LinkLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.RadioGroup_PrinterType.SuspendLayout();
            this.printersGroupBox.SuspendLayout();
            this.panelLocalCopies.SuspendLayout();
            this.panelCopies.SuspendLayout();
            this.SuspendLayout();
            // 
            // RadioGroup_PrinterType
            // 
            this.RadioGroup_PrinterType.Controls.Add(this.prt_Legacy);
            this.RadioGroup_PrinterType.Controls.Add(this.prt_Windows);
            this.RadioGroup_PrinterType.Font = new System.Drawing.Font("Tahoma", 9F);
            this.RadioGroup_PrinterType.Location = new System.Drawing.Point(312, 20);
            this.RadioGroup_PrinterType.Name = "RadioGroup_PrinterType";
            this.RadioGroup_PrinterType.Size = new System.Drawing.Size(211, 94);
            this.RadioGroup_PrinterType.TabIndex = 0;
            this.RadioGroup_PrinterType.TabStop = false;
            this.RadioGroup_PrinterType.Text = "tipo di Stampante dello Scontrino";
            // 
            // prt_Legacy
            // 
            this.prt_Legacy.AutoSize = true;
            this.prt_Legacy.Location = new System.Drawing.Point(128, 50);
            this.prt_Legacy.Name = "prt_Legacy";
            this.prt_Legacy.Size = new System.Drawing.Size(69, 18);
            this.prt_Legacy.TabIndex = 1;
            this.prt_Legacy.TabStop = true;
            this.prt_Legacy.Text = "LEGACY";
            this.prt_Legacy.UseVisualStyleBackColor = true;
            // 
            // prt_Windows
            // 
            this.prt_Windows.AutoSize = true;
            this.prt_Windows.Location = new System.Drawing.Point(19, 50);
            this.prt_Windows.Name = "prt_Windows";
            this.prt_Windows.Size = new System.Drawing.Size(85, 18);
            this.prt_Windows.TabIndex = 0;
            this.prt_Windows.TabStop = true;
            this.prt_Windows.Text = "WINDOWS";
            this.prt_Windows.UseVisualStyleBackColor = true;
            // 
            // printersGroupBox
            // 
            this.printersGroupBox.Controls.Add(this.BtnGenPrinterOptions);
            this.printersGroupBox.Controls.Add(this.BtnLegacy);
            this.printersGroupBox.Controls.Add(this.BtnWin);
            this.printersGroupBox.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.printersGroupBox.Location = new System.Drawing.Point(30, 20);
            this.printersGroupBox.Name = "printersGroupBox";
            this.printersGroupBox.Size = new System.Drawing.Size(260, 94);
            this.printersGroupBox.TabIndex = 1;
            this.printersGroupBox.TabStop = false;
            this.printersGroupBox.Text = "Impostazione stampanti Windows e Legacy";
            // 
            // BtnGenPrinterOptions
            // 
            this.BtnGenPrinterOptions.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.BtnGenPrinterOptions.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.BtnGenPrinterOptions.Image = global::StandFacile.Properties.Resources.globe;
            this.BtnGenPrinterOptions.Location = new System.Drawing.Point(106, 29);
            this.BtnGenPrinterOptions.Name = "BtnGenPrinterOptions";
            this.BtnGenPrinterOptions.Size = new System.Drawing.Size(50, 50);
            this.BtnGenPrinterOptions.TabIndex = 4;
            this.BtnGenPrinterOptions.UseVisualStyleBackColor = false;
            this.BtnGenPrinterOptions.Click += new System.EventHandler(this.BtnGeneric_Click);
            // 
            // BtnLegacy
            // 
            this.BtnLegacy.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.BtnLegacy.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.BtnLegacy.Image = ((System.Drawing.Image)(resources.GetObject("BtnLegacy.Image")));
            this.BtnLegacy.Location = new System.Drawing.Point(192, 29);
            this.BtnLegacy.Name = "BtnLegacy";
            this.BtnLegacy.Size = new System.Drawing.Size(50, 50);
            this.BtnLegacy.TabIndex = 1;
            this.BtnLegacy.UseVisualStyleBackColor = false;
            this.BtnLegacy.Click += new System.EventHandler(this.BtnLegacy_Click);
            // 
            // BtnWin
            // 
            this.BtnWin.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.BtnWin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.BtnWin.Image = ((System.Drawing.Image)(resources.GetObject("BtnWin.Image")));
            this.BtnWin.Location = new System.Drawing.Point(19, 29);
            this.BtnWin.Name = "BtnWin";
            this.BtnWin.Size = new System.Drawing.Size(50, 50);
            this.BtnWin.TabIndex = 0;
            this.BtnWin.UseVisualStyleBackColor = false;
            this.BtnWin.Click += new System.EventHandler(this.BtnWin_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(163, 610);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 28);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOK.Image = global::StandFacile.Properties.Resources.OK;
            this.btnOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOK.Location = new System.Drawing.Point(299, 610);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 28);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK  ";
            this.btnOK.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // panelLocalCopies
            // 
            this.panelLocalCopies.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.panelLocalCopies.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelLocalCopies.Controls.Add(this.checkBox_LocPrices);
            this.panelLocalCopies.Controls.Add(this.labelWarn1);
            this.panelLocalCopies.Controls.Add(this.checkBox_AvoidPrintGroups);
            this.panelLocalCopies.Controls.Add(this.labelWarn2);
            this.panelLocalCopies.Controls.Add(this.checkBox_CUT);
            this.panelLocalCopies.Controls.Add(this.checkBoxUnitItems);
            this.panelLocalCopies.Controls.Add(this.panelCopies);
            this.panelLocalCopies.Controls.Add(this.checkBoxSingleRowItems);
            this.panelLocalCopies.Controls.Add(this.checkBoxSelectedOnly);
            this.panelLocalCopies.Controls.Add(this.checkBox_LocalCopy);
            this.panelLocalCopies.Location = new System.Drawing.Point(16, 200);
            this.panelLocalCopies.Name = "panelLocalCopies";
            this.panelLocalCopies.Size = new System.Drawing.Size(517, 390);
            this.panelLocalCopies.TabIndex = 30;
            // 
            // checkBox_LocPrices
            // 
            this.checkBox_LocPrices.AutoSize = true;
            this.checkBox_LocPrices.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox_LocPrices.Location = new System.Drawing.Point(17, 52);
            this.checkBox_LocPrices.Name = "checkBox_LocPrices";
            this.checkBox_LocPrices.Size = new System.Drawing.Size(215, 18);
            this.checkBox_LocPrices.TabIndex = 72;
            this.checkBox_LocPrices.Text = "abilita stampa dei prezzi nelle copie";
            this.checkBox_LocPrices.UseVisualStyleBackColor = true;
            // 
            // labelWarn1
            // 
            this.labelWarn1.AutoSize = true;
            this.labelWarn1.BackColor = System.Drawing.SystemColors.Control;
            this.labelWarn1.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.labelWarn1.ForeColor = System.Drawing.SystemColors.Highlight;
            this.labelWarn1.Location = new System.Drawing.Point(264, 41);
            this.labelWarn1.Name = "labelWarn1";
            this.labelWarn1.Size = new System.Drawing.Size(240, 13);
            this.labelWarn1.TabIndex = 71;
            this.labelWarn1.Text = "attivabile se sono selezionate le opzioni qui sotto";
            // 
            // checkBox_AvoidPrintGroups
            // 
            this.checkBox_AvoidPrintGroups.AutoSize = true;
            this.checkBox_AvoidPrintGroups.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox_AvoidPrintGroups.Location = new System.Drawing.Point(265, 24);
            this.checkBox_AvoidPrintGroups.Name = "checkBox_AvoidPrintGroups";
            this.checkBox_AvoidPrintGroups.Size = new System.Drawing.Size(239, 18);
            this.checkBox_AvoidPrintGroups.TabIndex = 70;
            this.checkBox_AvoidPrintGroups.Text = "evita stampa per gruppi non selezionati";
            this.checkBox_AvoidPrintGroups.UseVisualStyleBackColor = true;
            // 
            // labelWarn2
            // 
            this.labelWarn2.AutoSize = true;
            this.labelWarn2.BackColor = System.Drawing.SystemColors.Control;
            this.labelWarn2.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.labelWarn2.ForeColor = System.Drawing.SystemColors.Highlight;
            this.labelWarn2.Location = new System.Drawing.Point(283, 100);
            this.labelWarn2.Name = "labelWarn2";
            this.labelWarn2.Size = new System.Drawing.Size(185, 13);
            this.labelWarn2.TabIndex = 69;
            this.labelWarn2.Text = "opzioni ad elevato consumo di carta !";
            // 
            // checkBox_CUT
            // 
            this.checkBox_CUT.AutoSize = true;
            this.checkBox_CUT.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox_CUT.Location = new System.Drawing.Point(17, 89);
            this.checkBox_CUT.Name = "checkBox_CUT";
            this.checkBox_CUT.Size = new System.Drawing.Size(221, 18);
            this.checkBox_CUT.TabIndex = 38;
            this.checkBox_CUT.Text = "stampa con taglio di separaz. gruppi";
            this.checkBox_CUT.UseVisualStyleBackColor = true;
            // 
            // checkBoxUnitItems
            // 
            this.checkBoxUnitItems.AutoSize = true;
            this.checkBoxUnitItems.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxUnitItems.Location = new System.Drawing.Point(265, 122);
            this.checkBoxUnitItems.Name = "checkBoxUnitItems";
            this.checkBoxUnitItems.Size = new System.Drawing.Size(204, 18);
            this.checkBoxUnitItems.TabIndex = 36;
            this.checkBoxUnitItems.Text = "singolo articolo con quantità uno";
            this.checkBoxUnitItems.UseVisualStyleBackColor = true;
            this.checkBoxUnitItems.CheckedChanged += new System.EventHandler(this.CheckBoxUnitItems_CheckedChanged);
            // 
            // panelCopies
            // 
            this.panelCopies.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panelCopies.Controls.Add(this.checkBoxCopia_9);
            this.panelCopies.Controls.Add(this.checkBoxCopia_8);
            this.panelCopies.Controls.Add(this.labelNoWebLoad);
            this.panelCopies.Controls.Add(this.checkBoxCopia_7);
            this.panelCopies.Controls.Add(this.checkBoxCopia_6);
            this.panelCopies.Controls.Add(this.checkBoxCopia_5);
            this.panelCopies.Controls.Add(this.checkBoxCopia_4);
            this.panelCopies.Controls.Add(this.checkBoxCopia_3);
            this.panelCopies.Controls.Add(this.checkBoxCopia_2);
            this.panelCopies.Controls.Add(this.checkBoxCopia_1);
            this.panelCopies.Controls.Add(this.checkBoxCopia_0);
            this.panelCopies.Location = new System.Drawing.Point(10, 164);
            this.panelCopies.Name = "panelCopies";
            this.panelCopies.Size = new System.Drawing.Size(496, 209);
            this.panelCopies.TabIndex = 35;
            // 
            // checkBoxCopia_9
            // 
            this.checkBoxCopia_9.AutoSize = true;
            this.checkBoxCopia_9.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxCopia_9.Location = new System.Drawing.Point(265, 173);
            this.checkBoxCopia_9.Name = "checkBoxCopia_9";
            this.checkBoxCopia_9.Size = new System.Drawing.Size(189, 17);
            this.checkBoxCopia_9.TabIndex = 67;
            this.checkBoxCopia_9.Text = "##### COPIE SINGOLE #####";
            this.checkBoxCopia_9.UseVisualStyleBackColor = true;
            // 
            // checkBoxCopia_8
            // 
            this.checkBoxCopia_8.AutoSize = true;
            this.checkBoxCopia_8.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxCopia_8.Location = new System.Drawing.Point(265, 130);
            this.checkBoxCopia_8.Name = "checkBoxCopia_8";
            this.checkBoxCopia_8.Size = new System.Drawing.Size(178, 17);
            this.checkBoxCopia_8.TabIndex = 66;
            this.checkBoxCopia_8.Text = "### COPIA GRP NO WEB ###";
            this.checkBoxCopia_8.UseVisualStyleBackColor = true;
            // 
            // labelNoWebLoad
            // 
            this.labelNoWebLoad.AutoSize = true;
            this.labelNoWebLoad.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.labelNoWebLoad.ForeColor = System.Drawing.SystemColors.Highlight;
            this.labelNoWebLoad.Location = new System.Drawing.Point(282, 147);
            this.labelNoWebLoad.Name = "labelNoWebLoad";
            this.labelNoWebLoad.Size = new System.Drawing.Size(163, 13);
            this.labelNoWebLoad.TabIndex = 65;
            this.labelNoWebLoad.Text = "no ordini web per questo gruppo";
            // 
            // checkBoxCopia_7
            // 
            this.checkBoxCopia_7.AutoSize = true;
            this.checkBoxCopia_7.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxCopia_7.Location = new System.Drawing.Point(265, 92);
            this.checkBoxCopia_7.Name = "checkBoxCopia_7";
            this.checkBoxCopia_7.Size = new System.Drawing.Size(194, 17);
            this.checkBoxCopia_7.TabIndex = 11;
            this.checkBoxCopia_7.Text = "##### COPIA GRUPPO8 #####";
            this.checkBoxCopia_7.UseVisualStyleBackColor = true;
            // 
            // checkBoxCopia_6
            // 
            this.checkBoxCopia_6.AutoSize = true;
            this.checkBoxCopia_6.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxCopia_6.Location = new System.Drawing.Point(265, 54);
            this.checkBoxCopia_6.Name = "checkBoxCopia_6";
            this.checkBoxCopia_6.Size = new System.Drawing.Size(194, 17);
            this.checkBoxCopia_6.TabIndex = 11;
            this.checkBoxCopia_6.Text = "##### COPIA GRUPPO7 #####";
            this.checkBoxCopia_6.UseVisualStyleBackColor = true;
            // 
            // checkBoxCopia_5
            // 
            this.checkBoxCopia_5.AutoSize = true;
            this.checkBoxCopia_5.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxCopia_5.Location = new System.Drawing.Point(265, 16);
            this.checkBoxCopia_5.Name = "checkBoxCopia_5";
            this.checkBoxCopia_5.Size = new System.Drawing.Size(194, 17);
            this.checkBoxCopia_5.TabIndex = 11;
            this.checkBoxCopia_5.Text = "##### COPIA GRUPPO6 #####";
            this.checkBoxCopia_5.UseVisualStyleBackColor = true;
            // 
            // checkBoxCopia_4
            // 
            this.checkBoxCopia_4.AutoSize = true;
            this.checkBoxCopia_4.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxCopia_4.Location = new System.Drawing.Point(16, 168);
            this.checkBoxCopia_4.Name = "checkBoxCopia_4";
            this.checkBoxCopia_4.Size = new System.Drawing.Size(194, 17);
            this.checkBoxCopia_4.TabIndex = 10;
            this.checkBoxCopia_4.Text = "##### COPIA GRUPPO5 #####";
            this.checkBoxCopia_4.UseVisualStyleBackColor = true;
            // 
            // checkBoxCopia_3
            // 
            this.checkBoxCopia_3.AutoSize = true;
            this.checkBoxCopia_3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxCopia_3.Location = new System.Drawing.Point(16, 130);
            this.checkBoxCopia_3.Name = "checkBoxCopia_3";
            this.checkBoxCopia_3.Size = new System.Drawing.Size(194, 17);
            this.checkBoxCopia_3.TabIndex = 9;
            this.checkBoxCopia_3.Text = "##### COPIA GRUPPO4 #####";
            this.checkBoxCopia_3.UseVisualStyleBackColor = true;
            // 
            // checkBoxCopia_2
            // 
            this.checkBoxCopia_2.AutoSize = true;
            this.checkBoxCopia_2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxCopia_2.Location = new System.Drawing.Point(16, 92);
            this.checkBoxCopia_2.Name = "checkBoxCopia_2";
            this.checkBoxCopia_2.Size = new System.Drawing.Size(194, 17);
            this.checkBoxCopia_2.TabIndex = 8;
            this.checkBoxCopia_2.Text = "##### COPIA GRUPPO3 #####";
            this.checkBoxCopia_2.UseVisualStyleBackColor = true;
            // 
            // checkBoxCopia_1
            // 
            this.checkBoxCopia_1.AutoSize = true;
            this.checkBoxCopia_1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxCopia_1.Location = new System.Drawing.Point(16, 54);
            this.checkBoxCopia_1.Name = "checkBoxCopia_1";
            this.checkBoxCopia_1.Size = new System.Drawing.Size(194, 17);
            this.checkBoxCopia_1.TabIndex = 7;
            this.checkBoxCopia_1.Text = "##### COPIA GRUPPO2 #####";
            this.checkBoxCopia_1.UseVisualStyleBackColor = true;
            // 
            // checkBoxCopia_0
            // 
            this.checkBoxCopia_0.AutoSize = true;
            this.checkBoxCopia_0.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxCopia_0.Location = new System.Drawing.Point(16, 16);
            this.checkBoxCopia_0.Name = "checkBoxCopia_0";
            this.checkBoxCopia_0.Size = new System.Drawing.Size(194, 17);
            this.checkBoxCopia_0.TabIndex = 6;
            this.checkBoxCopia_0.Text = "##### COPIA GRUPPO1 #####";
            this.checkBoxCopia_0.UseVisualStyleBackColor = true;
            // 
            // checkBoxSingleRowItems
            // 
            this.checkBoxSingleRowItems.AutoSize = true;
            this.checkBoxSingleRowItems.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxSingleRowItems.Location = new System.Drawing.Point(265, 74);
            this.checkBoxSingleRowItems.Name = "checkBoxSingleRowItems";
            this.checkBoxSingleRowItems.Size = new System.Drawing.Size(200, 18);
            this.checkBoxSingleRowItems.TabIndex = 31;
            this.checkBoxSingleRowItems.Text = "singolo articolo nella copia locale";
            this.checkBoxSingleRowItems.UseVisualStyleBackColor = true;
            this.checkBoxSingleRowItems.CheckedChanged += new System.EventHandler(this.CheckBoxSingleRowItems_CheckedChanged);
            // 
            // checkBoxSelectedOnly
            // 
            this.checkBoxSelectedOnly.AutoSize = true;
            this.checkBoxSelectedOnly.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxSelectedOnly.Location = new System.Drawing.Point(17, 126);
            this.checkBoxSelectedOnly.Name = "checkBoxSelectedOnly";
            this.checkBoxSelectedOnly.Size = new System.Drawing.Size(149, 18);
            this.checkBoxSelectedOnly.TabIndex = 30;
            this.checkBoxSelectedOnly.Text = "stampa solo selezionati";
            this.checkBoxSelectedOnly.UseVisualStyleBackColor = true;
            this.checkBoxSelectedOnly.CheckedChanged += new System.EventHandler(this.CheckBoxNoPrice_CheckedChanged);
            // 
            // checkBox_LocalCopy
            // 
            this.checkBox_LocalCopy.AutoSize = true;
            this.checkBox_LocalCopy.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox_LocalCopy.Location = new System.Drawing.Point(17, 15);
            this.checkBox_LocalCopy.Name = "checkBox_LocalCopy";
            this.checkBox_LocalCopy.Size = new System.Drawing.Size(165, 18);
            this.checkBox_LocalCopy.TabIndex = 29;
            this.checkBox_LocalCopy.Text = "abilita stampa copie Locali";
            this.checkBox_LocalCopy.UseVisualStyleBackColor = true;
            this.checkBox_LocalCopy.CheckedChanged += new System.EventHandler(this.CheckBoxNoPrice_CheckedChanged);
            // 
            // timer
            // 
            this.timer.Interval = 250;
            this.timer.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // LinkLbl_Mnu_CCR
            // 
            this.LinkLbl_Mnu_CCR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LinkLbl_Mnu_CCR.AutoSize = true;
            this.LinkLbl_Mnu_CCR.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LinkLbl_Mnu_CCR.Location = new System.Drawing.Point(339, 169);
            this.LinkLbl_Mnu_CCR.Name = "LinkLbl_Mnu_CCR";
            this.LinkLbl_Mnu_CCR.Size = new System.Drawing.Size(188, 14);
            this.LinkLbl_Mnu_CCR.TabIndex = 34;
            this.LinkLbl_Mnu_CCR.TabStop = true;
            this.LinkLbl_Mnu_CCR.Text = "\"Configurazione Gruppi di Articoli\"";
            this.LinkLbl_Mnu_CCR.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLbl_Mnu_CCR_LinkClicked);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(21, 148);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(489, 16);
            this.label2.TabIndex = 37;
            this.label2.Text = "per modifica testo descrittivo dei gruppi, raggruppamento per colore, stampa in r" +
    "ete";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(21, 168);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(317, 16);
            this.label3.TabIndex = 36;
            this.label3.Text = "e/o ordini da smartphone usare in alternativa il menu:";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(21, 128);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(450, 16);
            this.label1.TabIndex = 35;
            this.label1.Text = "da qui si effettuano le impostazioni di stampa delle sole copie in cassa locale:";
            // 
            // PrintLocalCopiesConfigDlg
            // 
            this.AcceptButton = this.btnCancel;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(539, 654);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LinkLbl_Mnu_CCR);
            this.Controls.Add(this.panelLocalCopies);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.printersGroupBox);
            this.Controls.Add(this.RadioGroup_PrinterType);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PrintLocalCopiesConfigDlg";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Configurazione Gruppi e Stampante per le copie Locali";
            this.RadioGroup_PrinterType.ResumeLayout(false);
            this.RadioGroup_PrinterType.PerformLayout();
            this.printersGroupBox.ResumeLayout(false);
            this.panelLocalCopies.ResumeLayout(false);
            this.panelLocalCopies.PerformLayout();
            this.panelCopies.ResumeLayout(false);
            this.panelCopies.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox RadioGroup_PrinterType;
        private System.Windows.Forms.RadioButton prt_Legacy;
        private System.Windows.Forms.RadioButton prt_Windows;
        private System.Windows.Forms.GroupBox printersGroupBox;
        private System.Windows.Forms.Button BtnWin;
        private System.Windows.Forms.Button BtnLegacy;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Panel panelLocalCopies;
        private System.Windows.Forms.CheckBox checkBoxSelectedOnly;
        private System.Windows.Forms.CheckBox checkBox_LocalCopy;
        private System.Windows.Forms.CheckBox checkBoxSingleRowItems;
        private System.Windows.Forms.Panel panelCopies;
        private System.Windows.Forms.CheckBox checkBoxCopia_7;
        private System.Windows.Forms.CheckBox checkBoxCopia_6;
        private System.Windows.Forms.CheckBox checkBoxCopia_5;
        private System.Windows.Forms.CheckBox checkBoxCopia_4;
        private System.Windows.Forms.CheckBox checkBoxCopia_3;
        private System.Windows.Forms.CheckBox checkBoxCopia_2;
        private System.Windows.Forms.CheckBox checkBoxCopia_1;
        private System.Windows.Forms.CheckBox checkBoxCopia_0;
        private System.Windows.Forms.CheckBox checkBoxUnitItems;
        private System.Windows.Forms.Label labelNoWebLoad;
        private System.Windows.Forms.CheckBox checkBox_CUT;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.LinkLabel LinkLbl_Mnu_CCR;
        private System.Windows.Forms.Label labelWarn2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBox_AvoidPrintGroups;
        private System.Windows.Forms.CheckBox checkBoxCopia_8;
        private System.Windows.Forms.CheckBox checkBoxCopia_9;
        private System.Windows.Forms.Label labelWarn1;
        private System.Windows.Forms.CheckBox checkBox_LocPrices;
        private System.Windows.Forms.Button BtnGenPrinterOptions;
    }
}
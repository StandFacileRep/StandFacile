namespace StandFacile
{
    partial class PrintReceiptConfigDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrintReceiptConfigDlg));
            this.RadioGroup_PrinterType = new System.Windows.Forms.GroupBox();
            this.prt_Legacy = new System.Windows.Forms.RadioButton();
            this.prt_Windows = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.BtnLegacy = new System.Windows.Forms.Button();
            this.BtnWin = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.panelLocalCopies = new System.Windows.Forms.Panel();
            this.checkBox_AvoidPrintGroups = new System.Windows.Forms.CheckBox();
            this.labelWarn = new System.Windows.Forms.Label();
            this.checkBox_CUT = new System.Windows.Forms.CheckBox();
            this.checkBoxUnitItems = new System.Windows.Forms.CheckBox();
            this.panelCopies = new System.Windows.Forms.Panel();
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
            this.checkBoxLocalCopy = new System.Windows.Forms.CheckBox();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.LinkLbl_Mnu_CCR = new System.Windows.Forms.LinkLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.RadioGroup_PrinterType.SuspendLayout();
            this.groupBox1.SuspendLayout();
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
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.BtnLegacy);
            this.groupBox1.Controls.Add(this.BtnWin);
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(30, 20);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(260, 94);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Impostazione stampanti Windows e Legacy";
            // 
            // BtnLegacy
            // 
            this.BtnLegacy.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.BtnLegacy.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.BtnLegacy.Image = ((System.Drawing.Image)(resources.GetObject("BtnLegacy.Image")));
            this.BtnLegacy.Location = new System.Drawing.Point(172, 29);
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
            this.BtnWin.Location = new System.Drawing.Point(43, 29);
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
            this.btnCancel.Location = new System.Drawing.Point(163, 551);
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
            this.btnOK.Location = new System.Drawing.Point(299, 551);
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
            this.panelLocalCopies.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelLocalCopies.Controls.Add(this.checkBox_AvoidPrintGroups);
            this.panelLocalCopies.Controls.Add(this.labelWarn);
            this.panelLocalCopies.Controls.Add(this.checkBox_CUT);
            this.panelLocalCopies.Controls.Add(this.checkBoxUnitItems);
            this.panelLocalCopies.Controls.Add(this.panelCopies);
            this.panelLocalCopies.Controls.Add(this.checkBoxSingleRowItems);
            this.panelLocalCopies.Controls.Add(this.checkBoxSelectedOnly);
            this.panelLocalCopies.Controls.Add(this.checkBoxLocalCopy);
            this.panelLocalCopies.Location = new System.Drawing.Point(16, 211);
            this.panelLocalCopies.Name = "panelLocalCopies";
            this.panelLocalCopies.Size = new System.Drawing.Size(517, 325);
            this.panelLocalCopies.TabIndex = 30;
            // 
            // checkBox_AvoidPrintGroups
            // 
            this.checkBox_AvoidPrintGroups.AutoSize = true;
            this.checkBox_AvoidPrintGroups.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox_AvoidPrintGroups.Location = new System.Drawing.Point(265, 15);
            this.checkBox_AvoidPrintGroups.Name = "checkBox_AvoidPrintGroups";
            this.checkBox_AvoidPrintGroups.Size = new System.Drawing.Size(239, 18);
            this.checkBox_AvoidPrintGroups.TabIndex = 70;
            this.checkBox_AvoidPrintGroups.Text = "evita stampa per gruppi non selezionati";
            this.checkBox_AvoidPrintGroups.UseVisualStyleBackColor = true;
            this.checkBox_AvoidPrintGroups.Click += new System.EventHandler(this.CheckBox_AvoidPrintGroups_Click);
            // 
            // labelWarn
            // 
            this.labelWarn.AutoSize = true;
            this.labelWarn.BackColor = System.Drawing.SystemColors.Control;
            this.labelWarn.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.labelWarn.ForeColor = System.Drawing.SystemColors.Highlight;
            this.labelWarn.Location = new System.Drawing.Point(291, 73);
            this.labelWarn.Name = "labelWarn";
            this.labelWarn.Size = new System.Drawing.Size(185, 13);
            this.labelWarn.TabIndex = 69;
            this.labelWarn.Text = "opzioni ad elevato consumo di carta !";
            // 
            // checkBox_CUT
            // 
            this.checkBox_CUT.AutoSize = true;
            this.checkBox_CUT.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox_CUT.Location = new System.Drawing.Point(17, 54);
            this.checkBox_CUT.Name = "checkBox_CUT";
            this.checkBox_CUT.Size = new System.Drawing.Size(221, 18);
            this.checkBox_CUT.TabIndex = 38;
            this.checkBox_CUT.Text = "stampa con taglio di separaz. gruppi";
            this.checkBox_CUT.UseVisualStyleBackColor = true;
            this.checkBox_CUT.Click += new System.EventHandler(this.CheckBox_CUT_Click);
            // 
            // checkBoxUnitItems
            // 
            this.checkBoxUnitItems.AutoSize = true;
            this.checkBoxUnitItems.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxUnitItems.Location = new System.Drawing.Point(265, 90);
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
            this.panelCopies.Controls.Add(this.labelNoWebLoad);
            this.panelCopies.Controls.Add(this.checkBoxCopia_7);
            this.panelCopies.Controls.Add(this.checkBoxCopia_6);
            this.panelCopies.Controls.Add(this.checkBoxCopia_5);
            this.panelCopies.Controls.Add(this.checkBoxCopia_4);
            this.panelCopies.Controls.Add(this.checkBoxCopia_3);
            this.panelCopies.Controls.Add(this.checkBoxCopia_2);
            this.panelCopies.Controls.Add(this.checkBoxCopia_1);
            this.panelCopies.Controls.Add(this.checkBoxCopia_0);
            this.panelCopies.Location = new System.Drawing.Point(10, 127);
            this.panelCopies.Name = "panelCopies";
            this.panelCopies.Size = new System.Drawing.Size(496, 178);
            this.panelCopies.TabIndex = 35;
            // 
            // labelNoWebLoad
            // 
            this.labelNoWebLoad.AutoSize = true;
            this.labelNoWebLoad.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.labelNoWebLoad.ForeColor = System.Drawing.SystemColors.Highlight;
            this.labelNoWebLoad.Location = new System.Drawing.Point(289, 153);
            this.labelNoWebLoad.Name = "labelNoWebLoad";
            this.labelNoWebLoad.Size = new System.Drawing.Size(163, 13);
            this.labelNoWebLoad.TabIndex = 65;
            this.labelNoWebLoad.Text = "no ordini web per questo gruppo";
            // 
            // checkBoxCopia_7
            // 
            this.checkBoxCopia_7.AutoSize = true;
            this.checkBoxCopia_7.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxCopia_7.Location = new System.Drawing.Point(268, 133);
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
            this.checkBoxCopia_6.Location = new System.Drawing.Point(268, 94);
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
            this.checkBoxCopia_5.Location = new System.Drawing.Point(268, 55);
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
            this.checkBoxCopia_4.Location = new System.Drawing.Point(268, 16);
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
            this.checkBoxCopia_3.Location = new System.Drawing.Point(16, 133);
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
            this.checkBoxCopia_2.Location = new System.Drawing.Point(16, 94);
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
            this.checkBoxCopia_1.Location = new System.Drawing.Point(16, 55);
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
            this.checkBoxSingleRowItems.Location = new System.Drawing.Point(265, 52);
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
            this.checkBoxSelectedOnly.Location = new System.Drawing.Point(17, 91);
            this.checkBoxSelectedOnly.Name = "checkBoxSelectedOnly";
            this.checkBoxSelectedOnly.Size = new System.Drawing.Size(149, 18);
            this.checkBoxSelectedOnly.TabIndex = 30;
            this.checkBoxSelectedOnly.Text = "stampa solo selezionati";
            this.checkBoxSelectedOnly.UseVisualStyleBackColor = true;
            this.checkBoxSelectedOnly.CheckedChanged += new System.EventHandler(this.CheckBoxNoPrice_CheckedChanged);
            // 
            // checkBoxLocalCopy
            // 
            this.checkBoxLocalCopy.AutoSize = true;
            this.checkBoxLocalCopy.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxLocalCopy.Location = new System.Drawing.Point(17, 15);
            this.checkBoxLocalCopy.Name = "checkBoxLocalCopy";
            this.checkBoxLocalCopy.Size = new System.Drawing.Size(206, 18);
            this.checkBoxLocalCopy.TabIndex = 29;
            this.checkBoxLocalCopy.Text = "abilita stampa copie locali in cassa";
            this.checkBoxLocalCopy.UseVisualStyleBackColor = true;
            this.checkBoxLocalCopy.CheckedChanged += new System.EventHandler(this.CheckBoxNoPrice_CheckedChanged);
            // 
            // timer
            // 
            this.timer.Interval = 250;
            this.timer.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // LinkLbl_Mnu_CCR
            // 
            this.LinkLbl_Mnu_CCR.AutoSize = true;
            this.LinkLbl_Mnu_CCR.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LinkLbl_Mnu_CCR.Location = new System.Drawing.Point(351, 186);
            this.LinkLbl_Mnu_CCR.Name = "LinkLbl_Mnu_CCR";
            this.LinkLbl_Mnu_CCR.Size = new System.Drawing.Size(170, 14);
            this.LinkLbl_Mnu_CCR.TabIndex = 34;
            this.LinkLbl_Mnu_CCR.TabStop = true;
            this.LinkLbl_Mnu_CCR.Text = "\"Configurazione Copie in rete\"";
            this.LinkLbl_Mnu_CCR.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLbl_Mnu_CCR_LinkClicked);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(21, 165);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(489, 16);
            this.label2.TabIndex = 37;
            this.label2.Text = "per modifica testo descrittivo dei gruppi, raggruppamento per colore, stampa in r" +
    "ete";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(21, 185);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(317, 16);
            this.label3.TabIndex = 36;
            this.label3.Text = "e/o ordini da smartphone usare in alternativa il menu:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(21, 145);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(450, 16);
            this.label1.TabIndex = 35;
            this.label1.Text = "da qui si effettuano le impostazioni di stampa delle sole copie in cassa locale:";
            // 
            // PrintReceiptConfigDlg
            // 
            this.AcceptButton = this.btnCancel;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(539, 592);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LinkLbl_Mnu_CCR);
            this.Controls.Add(this.panelLocalCopies);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.RadioGroup_PrinterType);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PrintReceiptConfigDlg";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Configurazione Stampa Scontrino e copie locali";
            this.RadioGroup_PrinterType.ResumeLayout(false);
            this.RadioGroup_PrinterType.PerformLayout();
            this.groupBox1.ResumeLayout(false);
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
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button BtnWin;
        private System.Windows.Forms.Button BtnLegacy;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Panel panelLocalCopies;
        private System.Windows.Forms.CheckBox checkBoxSelectedOnly;
        private System.Windows.Forms.CheckBox checkBoxLocalCopy;
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
        private System.Windows.Forms.Label labelWarn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBox_AvoidPrintGroups;
    }
}
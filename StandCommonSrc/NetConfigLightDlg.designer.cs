namespace StandFacile
{
    partial class NetConfigLightDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NetConfigLightDlg));
            this.Combo_DBServerName = new System.Windows.Forms.ComboBox();
            this.BtnDB_ServerTest = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.labelGroups = new System.Windows.Forms.Label();
            this.checkBoxCopia_0 = new System.Windows.Forms.CheckBox();
            this.checkBoxCopia_1 = new System.Windows.Forms.CheckBox();
            this.checkBoxCopia_2 = new System.Windows.Forms.CheckBox();
            this.checkBoxCopia_3 = new System.Windows.Forms.CheckBox();
            this.checkBoxCopia_4 = new System.Windows.Forms.CheckBox();
            this.checkBoxCopia_5 = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dbPasswordEdit = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.combo_TipoDBase = new System.Windows.Forms.ComboBox();
            this.checkBoxBarcode = new System.Windows.Forms.CheckBox();
            this.checkBox_StampaManuale = new System.Windows.Forms.CheckBox();
            this.panelCopies = new System.Windows.Forms.Panel();
            this.checkBoxCopia_8 = new System.Windows.Forms.CheckBox();
            this.checkBoxCopia_7 = new System.Windows.Forms.CheckBox();
            this.checkBoxCopia_6 = new System.Windows.Forms.CheckBox();
            this.textBoxColor_3 = new System.Windows.Forms.TextBox();
            this.labelColors = new System.Windows.Forms.Label();
            this.textBoxColor_2 = new System.Windows.Forms.TextBox();
            this.textBoxColor_1 = new System.Windows.Forms.TextBox();
            this.textBoxColor_0 = new System.Windows.Forms.TextBox();
            this.textBoxWarn = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.dbUsernameEdit = new System.Windows.Forms.TextBox();
            this.dbDatabaseEdit = new System.Windows.Forms.TextBox();
            this.panelCopies.SuspendLayout();
            this.SuspendLayout();
            // 
            // Combo_DBServerName
            // 
            this.Combo_DBServerName.FormattingEnabled = true;
            this.Combo_DBServerName.Location = new System.Drawing.Point(19, 49);
            this.Combo_DBServerName.Name = "Combo_DBServerName";
            this.Combo_DBServerName.Size = new System.Drawing.Size(231, 21);
            this.Combo_DBServerName.TabIndex = 0;
            // 
            // BtnDB_ServerTest
            // 
            this.BtnDB_ServerTest.Location = new System.Drawing.Point(288, 162);
            this.BtnDB_ServerTest.Name = "BtnDB_ServerTest";
            this.BtnDB_ServerTest.Size = new System.Drawing.Size(86, 30);
            this.BtnDB_ServerTest.TabIndex = 1;
            this.BtnDB_ServerTest.Text = "Test DBase";
            this.BtnDB_ServerTest.UseVisualStyleBackColor = true;
            this.BtnDB_ServerTest.Click += new System.EventHandler(this.BtnDB_ServerTest_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(146, 610);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 32);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Annulla";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Image = ((System.Drawing.Image)(resources.GetObject("btnOK.Image")));
            this.btnOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOK.Location = new System.Drawing.Point(287, 610);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 32);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK   ";
            this.btnOK.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(16, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(217, 16);
            this.label1.TabIndex = 4;
            this.label1.Text = "Inserire il nome del Database Server";
            // 
            // labelGroups
            // 
            this.labelGroups.AutoSize = true;
            this.labelGroups.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelGroups.Location = new System.Drawing.Point(22, 248);
            this.labelGroups.Name = "labelGroups";
            this.labelGroups.Size = new System.Drawing.Size(464, 16);
            this.labelGroups.TabIndex = 5;
            this.labelGroups.Text = "Spunta i gruppi da stampare, il colore è determinato esclusivamente dal Listino:";
            // 
            // checkBoxCopia_0
            // 
            this.checkBoxCopia_0.AutoCheck = false;
            this.checkBoxCopia_0.AutoSize = true;
            this.checkBoxCopia_0.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxCopia_0.Location = new System.Drawing.Point(21, 10);
            this.checkBoxCopia_0.Name = "checkBoxCopia_0";
            this.checkBoxCopia_0.Size = new System.Drawing.Size(218, 20);
            this.checkBoxCopia_0.TabIndex = 6;
            this.checkBoxCopia_0.Text = "##### COPIA GRUPPO1 #####";
            this.checkBoxCopia_0.UseVisualStyleBackColor = true;
            this.checkBoxCopia_0.MouseClick += new System.Windows.Forms.MouseEventHandler(this.CheckBoxCopia_MouseClick);
            // 
            // checkBoxCopia_1
            // 
            this.checkBoxCopia_1.AutoCheck = false;
            this.checkBoxCopia_1.AutoSize = true;
            this.checkBoxCopia_1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxCopia_1.Location = new System.Drawing.Point(21, 38);
            this.checkBoxCopia_1.Name = "checkBoxCopia_1";
            this.checkBoxCopia_1.Size = new System.Drawing.Size(218, 20);
            this.checkBoxCopia_1.TabIndex = 7;
            this.checkBoxCopia_1.Text = "##### COPIA GRUPPO2 #####";
            this.checkBoxCopia_1.UseVisualStyleBackColor = true;
            this.checkBoxCopia_1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.CheckBoxCopia_MouseClick);
            // 
            // checkBoxCopia_2
            // 
            this.checkBoxCopia_2.AutoCheck = false;
            this.checkBoxCopia_2.AutoSize = true;
            this.checkBoxCopia_2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxCopia_2.Location = new System.Drawing.Point(21, 66);
            this.checkBoxCopia_2.Name = "checkBoxCopia_2";
            this.checkBoxCopia_2.Size = new System.Drawing.Size(218, 20);
            this.checkBoxCopia_2.TabIndex = 8;
            this.checkBoxCopia_2.Text = "##### COPIA GRUPPO3 #####";
            this.checkBoxCopia_2.UseVisualStyleBackColor = true;
            this.checkBoxCopia_2.MouseClick += new System.Windows.Forms.MouseEventHandler(this.CheckBoxCopia_MouseClick);
            // 
            // checkBoxCopia_3
            // 
            this.checkBoxCopia_3.AutoCheck = false;
            this.checkBoxCopia_3.AutoSize = true;
            this.checkBoxCopia_3.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxCopia_3.Location = new System.Drawing.Point(21, 94);
            this.checkBoxCopia_3.Name = "checkBoxCopia_3";
            this.checkBoxCopia_3.Size = new System.Drawing.Size(218, 20);
            this.checkBoxCopia_3.TabIndex = 9;
            this.checkBoxCopia_3.Text = "##### COPIA GRUPPO4 #####";
            this.checkBoxCopia_3.UseVisualStyleBackColor = true;
            this.checkBoxCopia_3.MouseClick += new System.Windows.Forms.MouseEventHandler(this.CheckBoxCopia_MouseClick);
            // 
            // checkBoxCopia_4
            // 
            this.checkBoxCopia_4.AutoCheck = false;
            this.checkBoxCopia_4.AutoSize = true;
            this.checkBoxCopia_4.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxCopia_4.Location = new System.Drawing.Point(21, 124);
            this.checkBoxCopia_4.Name = "checkBoxCopia_4";
            this.checkBoxCopia_4.Size = new System.Drawing.Size(218, 20);
            this.checkBoxCopia_4.TabIndex = 10;
            this.checkBoxCopia_4.Text = "##### COPIA GRUPPO5 #####";
            this.checkBoxCopia_4.UseVisualStyleBackColor = true;
            this.checkBoxCopia_4.MouseClick += new System.Windows.Forms.MouseEventHandler(this.CheckBoxCopia_MouseClick);
            // 
            // checkBoxCopia_5
            // 
            this.checkBoxCopia_5.AutoCheck = false;
            this.checkBoxCopia_5.AutoSize = true;
            this.checkBoxCopia_5.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxCopia_5.Location = new System.Drawing.Point(268, 10);
            this.checkBoxCopia_5.Name = "checkBoxCopia_5";
            this.checkBoxCopia_5.Size = new System.Drawing.Size(218, 20);
            this.checkBoxCopia_5.TabIndex = 11;
            this.checkBoxCopia_5.Text = "##### COPIA GRUPPO6 #####";
            this.checkBoxCopia_5.UseVisualStyleBackColor = true;
            this.checkBoxCopia_5.MouseClick += new System.Windows.Forms.MouseEventHandler(this.CheckBoxCopia_MouseClick);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(16, 148);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(171, 14);
            this.label3.TabIndex = 13;
            this.label3.Text = "Password utente \"standfacile\"";
            // 
            // dbPasswordEdit
            // 
            this.dbPasswordEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dbPasswordEdit.Location = new System.Drawing.Point(19, 170);
            this.dbPasswordEdit.MaxLength = 24;
            this.dbPasswordEdit.Name = "dbPasswordEdit";
            this.dbPasswordEdit.PasswordChar = '*';
            this.dbPasswordEdit.Size = new System.Drawing.Size(170, 22);
            this.dbPasswordEdit.TabIndex = 14;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(285, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 14);
            this.label2.TabIndex = 16;
            this.label2.Text = "Tipo di database";
            // 
            // combo_TipoDBase
            // 
            this.combo_TipoDBase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_TipoDBase.Font = new System.Drawing.Font("Tahoma", 9F);
            this.combo_TipoDBase.FormattingEnabled = true;
            this.combo_TipoDBase.Location = new System.Drawing.Point(288, 48);
            this.combo_TipoDBase.Name = "combo_TipoDBase";
            this.combo_TipoDBase.Size = new System.Drawing.Size(170, 22);
            this.combo_TipoDBase.TabIndex = 15;
            this.combo_TipoDBase.SelectedIndexChanged += new System.EventHandler(this.Combo_TipoDBase_SelectedIndexChanged);
            // 
            // checkBoxBarcode
            // 
            this.checkBoxBarcode.AutoSize = true;
            this.checkBoxBarcode.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.checkBoxBarcode.Location = new System.Drawing.Point(41, 211);
            this.checkBoxBarcode.Name = "checkBoxBarcode";
            this.checkBoxBarcode.Size = new System.Drawing.Size(183, 20);
            this.checkBoxBarcode.TabIndex = 17;
            this.checkBoxBarcode.Text = "stampa barcode nelle copie";
            this.checkBoxBarcode.UseVisualStyleBackColor = true;
            // 
            // checkBox_StampaManuale
            // 
            this.checkBox_StampaManuale.AutoSize = true;
            this.checkBox_StampaManuale.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.checkBox_StampaManuale.Location = new System.Drawing.Point(288, 211);
            this.checkBox_StampaManuale.Name = "checkBox_StampaManuale";
            this.checkBox_StampaManuale.Size = new System.Drawing.Size(148, 20);
            this.checkBox_StampaManuale.TabIndex = 18;
            this.checkBox_StampaManuale.Text = "solo stampa manuale";
            this.checkBox_StampaManuale.UseVisualStyleBackColor = true;
            // 
            // panelCopies
            // 
            this.panelCopies.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelCopies.Controls.Add(this.checkBoxCopia_8);
            this.panelCopies.Controls.Add(this.checkBoxCopia_7);
            this.panelCopies.Controls.Add(this.checkBoxCopia_6);
            this.panelCopies.Controls.Add(this.checkBoxCopia_5);
            this.panelCopies.Controls.Add(this.checkBoxCopia_4);
            this.panelCopies.Controls.Add(this.checkBoxCopia_3);
            this.panelCopies.Controls.Add(this.checkBoxCopia_2);
            this.panelCopies.Controls.Add(this.checkBoxCopia_1);
            this.panelCopies.Controls.Add(this.checkBoxCopia_0);
            this.panelCopies.Location = new System.Drawing.Point(19, 267);
            this.panelCopies.Name = "panelCopies";
            this.panelCopies.Size = new System.Drawing.Size(496, 154);
            this.panelCopies.TabIndex = 19;
            // 
            // checkBoxCopia_8
            // 
            this.checkBoxCopia_8.AutoCheck = false;
            this.checkBoxCopia_8.AutoSize = true;
            this.checkBoxCopia_8.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxCopia_8.Location = new System.Drawing.Point(268, 94);
            this.checkBoxCopia_8.Name = "checkBoxCopia_8";
            this.checkBoxCopia_8.Size = new System.Drawing.Size(218, 20);
            this.checkBoxCopia_8.TabIndex = 12;
            this.checkBoxCopia_8.Text = "##### COPIA GRUPPO9 #####";
            this.checkBoxCopia_8.UseVisualStyleBackColor = true;
            this.checkBoxCopia_8.MouseClick += new System.Windows.Forms.MouseEventHandler(this.CheckBoxCopia_MouseClick);
            // 
            // checkBoxCopia_7
            // 
            this.checkBoxCopia_7.AutoCheck = false;
            this.checkBoxCopia_7.AutoSize = true;
            this.checkBoxCopia_7.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxCopia_7.Location = new System.Drawing.Point(268, 66);
            this.checkBoxCopia_7.Name = "checkBoxCopia_7";
            this.checkBoxCopia_7.Size = new System.Drawing.Size(218, 20);
            this.checkBoxCopia_7.TabIndex = 11;
            this.checkBoxCopia_7.Text = "##### COPIA GRUPPO8 #####";
            this.checkBoxCopia_7.UseVisualStyleBackColor = true;
            this.checkBoxCopia_7.MouseClick += new System.Windows.Forms.MouseEventHandler(this.CheckBoxCopia_MouseClick);
            // 
            // checkBoxCopia_6
            // 
            this.checkBoxCopia_6.AutoCheck = false;
            this.checkBoxCopia_6.AutoSize = true;
            this.checkBoxCopia_6.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxCopia_6.Location = new System.Drawing.Point(268, 38);
            this.checkBoxCopia_6.Name = "checkBoxCopia_6";
            this.checkBoxCopia_6.Size = new System.Drawing.Size(218, 20);
            this.checkBoxCopia_6.TabIndex = 11;
            this.checkBoxCopia_6.Text = "##### COPIA GRUPPO7 #####";
            this.checkBoxCopia_6.UseVisualStyleBackColor = true;
            this.checkBoxCopia_6.MouseClick += new System.Windows.Forms.MouseEventHandler(this.CheckBoxCopia_MouseClick);
            // 
            // textBoxColor_3
            // 
            this.textBoxColor_3.BackColor = System.Drawing.Color.Red;
            this.textBoxColor_3.ForeColor = System.Drawing.Color.White;
            this.textBoxColor_3.Location = new System.Drawing.Point(287, 568);
            this.textBoxColor_3.Name = "textBoxColor_3";
            this.textBoxColor_3.ReadOnly = true;
            this.textBoxColor_3.Size = new System.Drawing.Size(194, 20);
            this.textBoxColor_3.TabIndex = 68;
            this.textBoxColor_3.Text = "##### COPIA GEN 4 #####";
            this.textBoxColor_3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelColors
            // 
            this.labelColors.AutoSize = true;
            this.labelColors.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelColors.Location = new System.Drawing.Point(32, 508);
            this.labelColors.Name = "labelColors";
            this.labelColors.Size = new System.Drawing.Size(268, 14);
            this.labelColors.TabIndex = 67;
            this.labelColors.Text = "testo accorpamenti per colore diverso da grigio:";
            // 
            // textBoxColor_2
            // 
            this.textBoxColor_2.BackColor = System.Drawing.Color.Yellow;
            this.textBoxColor_2.ForeColor = System.Drawing.Color.Black;
            this.textBoxColor_2.Location = new System.Drawing.Point(34, 568);
            this.textBoxColor_2.Name = "textBoxColor_2";
            this.textBoxColor_2.ReadOnly = true;
            this.textBoxColor_2.Size = new System.Drawing.Size(194, 20);
            this.textBoxColor_2.TabIndex = 66;
            this.textBoxColor_2.Text = "##### COPIA GEN 3 #####";
            this.textBoxColor_2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxColor_1
            // 
            this.textBoxColor_1.BackColor = System.Drawing.Color.Blue;
            this.textBoxColor_1.ForeColor = System.Drawing.Color.White;
            this.textBoxColor_1.Location = new System.Drawing.Point(288, 532);
            this.textBoxColor_1.Name = "textBoxColor_1";
            this.textBoxColor_1.ReadOnly = true;
            this.textBoxColor_1.Size = new System.Drawing.Size(194, 20);
            this.textBoxColor_1.TabIndex = 65;
            this.textBoxColor_1.Text = "##### COPIA GEN 2 #####";
            this.textBoxColor_1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxColor_0
            // 
            this.textBoxColor_0.BackColor = System.Drawing.Color.LimeGreen;
            this.textBoxColor_0.ForeColor = System.Drawing.Color.Black;
            this.textBoxColor_0.Location = new System.Drawing.Point(34, 532);
            this.textBoxColor_0.Name = "textBoxColor_0";
            this.textBoxColor_0.ReadOnly = true;
            this.textBoxColor_0.Size = new System.Drawing.Size(194, 20);
            this.textBoxColor_0.TabIndex = 64;
            this.textBoxColor_0.Text = "##### COPIA GEN 1 #####";
            this.textBoxColor_0.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxWarn
            // 
            this.textBoxWarn.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.textBoxWarn.Location = new System.Drawing.Point(19, 434);
            this.textBoxWarn.Multiline = true;
            this.textBoxWarn.Name = "textBoxWarn";
            this.textBoxWarn.Size = new System.Drawing.Size(496, 58);
            this.textBoxWarn.TabIndex = 69;
            this.textBoxWarn.Text = "il grigio mantiene gli accorpamenti per gruppo di articoli, gli altri colori inve" +
    "ce producono accorpamenti che hanno priorità rispetti ai gruppi.";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(16, 90);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(157, 14);
            this.label4.TabIndex = 70;
            this.label4.Text = "Username utente Database";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(285, 90);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(92, 14);
            this.label5.TabIndex = 71;
            this.label5.Text = "Nome database";
            // 
            // dbUsernameEdit
            // 
            this.dbUsernameEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dbUsernameEdit.Location = new System.Drawing.Point(19, 107);
            this.dbUsernameEdit.MaxLength = 24;
            this.dbUsernameEdit.Name = "dbUsernameEdit";
            this.dbUsernameEdit.Size = new System.Drawing.Size(170, 22);
            this.dbUsernameEdit.TabIndex = 72;
            // 
            // dbDatabaseEdit
            // 
            this.dbDatabaseEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dbDatabaseEdit.Location = new System.Drawing.Point(288, 107);
            this.dbDatabaseEdit.MaxLength = 24;
            this.dbDatabaseEdit.Name = "dbDatabaseEdit";
            this.dbDatabaseEdit.Size = new System.Drawing.Size(170, 22);
            this.dbDatabaseEdit.TabIndex = 73;
            // 
            // NetConfigLightDlg
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(532, 663);
            this.Controls.Add(this.dbDatabaseEdit);
            this.Controls.Add(this.dbUsernameEdit);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxWarn);
            this.Controls.Add(this.textBoxColor_3);
            this.Controls.Add(this.labelColors);
            this.Controls.Add(this.textBoxColor_2);
            this.Controls.Add(this.textBoxColor_1);
            this.Controls.Add(this.textBoxColor_0);
            this.Controls.Add(this.panelCopies);
            this.Controls.Add(this.checkBox_StampaManuale);
            this.Controls.Add(this.checkBoxBarcode);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.combo_TipoDBase);
            this.Controls.Add(this.dbPasswordEdit);
            this.Controls.Add(this.labelGroups);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.BtnDB_ServerTest);
            this.Controls.Add(this.Combo_DBServerName);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NetConfigLightDlg";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Imposta rete e Gruppi";
            this.TopMost = true;
            this.Shown += new System.EventHandler(this.NetConfigDlg_Shown);
            this.panelCopies.ResumeLayout(false);
            this.panelCopies.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox Combo_DBServerName;
        private System.Windows.Forms.Button BtnDB_ServerTest;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelGroups;
        private System.Windows.Forms.CheckBox checkBoxCopia_0;
        private System.Windows.Forms.CheckBox checkBoxCopia_1;
        private System.Windows.Forms.CheckBox checkBoxCopia_2;
        private System.Windows.Forms.CheckBox checkBoxCopia_3;
        private System.Windows.Forms.CheckBox checkBoxCopia_4;
        private System.Windows.Forms.CheckBox checkBoxCopia_5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox dbPasswordEdit;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox combo_TipoDBase;
        private System.Windows.Forms.CheckBox checkBoxBarcode;
        private System.Windows.Forms.CheckBox checkBox_StampaManuale;
        private System.Windows.Forms.Panel panelCopies;
        private System.Windows.Forms.CheckBox checkBoxCopia_7;
        private System.Windows.Forms.CheckBox checkBoxCopia_6;
        private System.Windows.Forms.TextBox textBoxColor_3;
        private System.Windows.Forms.Label labelColors;
        private System.Windows.Forms.TextBox textBoxColor_2;
        private System.Windows.Forms.TextBox textBoxColor_1;
        private System.Windows.Forms.TextBox textBoxColor_0;
        private System.Windows.Forms.TextBox textBoxWarn;
        private System.Windows.Forms.CheckBox checkBoxCopia_8;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox dbUsernameEdit;
        private System.Windows.Forms.TextBox dbDatabaseEdit;
    }
}
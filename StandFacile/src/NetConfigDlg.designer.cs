namespace StandFacile
{
    partial class NetConfigDlg
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.Combo_NumCassa = new System.Windows.Forms.ComboBox();
            this.NC_lbl3 = new System.Windows.Forms.Label();
            this.Panel_Tipo_Cassa = new System.Windows.Forms.Panel();
            this.textBoxCassa = new System.Windows.Forms.TextBox();
            this.Panel_DB = new System.Windows.Forms.Panel();
            this.NC_lbl4 = new System.Windows.Forms.Label();
            this.combo_TipoDBase = new System.Windows.Forms.ComboBox();
            this.NC_lbl2 = new System.Windows.Forms.Label();
            this.dbPasswordEdit = new System.Windows.Forms.TextBox();
            this.Btn_DBServerTest = new System.Windows.Forms.Button();
            this.Combo_DBServerName = new System.Windows.Forms.ComboBox();
            this.NC_lbl1 = new System.Windows.Forms.Label();
            this.panelWeb = new System.Windows.Forms.Panel();
            this.Edit_WebService_Name = new System.Windows.Forms.TextBox();
            this.link_QRcode = new System.Windows.Forms.LinkLabel();
            this.pictureBox_QRCode = new System.Windows.Forms.PictureBox();
            this.NC_lbl8 = new System.Windows.Forms.Label();
            this.NC_lbl5 = new System.Windows.Forms.Label();
            this.Edit_WebServiceDBaseName = new System.Windows.Forms.TextBox();
            this.WO_ckBox = new System.Windows.Forms.CheckBox();
            this.NC_lbl6 = new System.Windows.Forms.Label();
            this.Edit_WebServiceDBasePwd = new System.Windows.Forms.TextBox();
            this.NC_btn_webSiteTest = new System.Windows.Forms.Button();
            this.NC_lbl7 = new System.Windows.Forms.Label();
            this.Panel_Tipo_Cassa.SuspendLayout();
            this.Panel_DB.SuspendLayout();
            this.panelWeb.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_QRCode)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Image = global::StandFacile.Properties.Resources.Cancel;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(132, 584);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 28);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Annulla";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Image = global::StandFacile.Properties.Resources.OK;
            this.btnOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOK.Location = new System.Drawing.Point(292, 584);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 28);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK  ";
            this.btnOK.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // Combo_NumCassa
            // 
            this.Combo_NumCassa.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Combo_NumCassa.Font = new System.Drawing.Font("Tahoma", 9F);
            this.Combo_NumCassa.FormattingEnabled = true;
            this.Combo_NumCassa.Location = new System.Drawing.Point(15, 44);
            this.Combo_NumCassa.Name = "Combo_NumCassa";
            this.Combo_NumCassa.Size = new System.Drawing.Size(173, 22);
            this.Combo_NumCassa.TabIndex = 1;
            this.Combo_NumCassa.SelectedIndexChanged += new System.EventHandler(this.Combo_NumCassa_SelectedIndexChanged);
            // 
            // NC_lbl3
            // 
            this.NC_lbl3.AutoSize = true;
            this.NC_lbl3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NC_lbl3.Location = new System.Drawing.Point(12, 16);
            this.NC_lbl3.Name = "NC_lbl3";
            this.NC_lbl3.Size = new System.Drawing.Size(77, 14);
            this.NC_lbl3.TabIndex = 0;
            this.NC_lbl3.Text = "Tipo di Cassa";
            // 
            // Panel_Tipo_Cassa
            // 
            this.Panel_Tipo_Cassa.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Panel_Tipo_Cassa.Controls.Add(this.Combo_NumCassa);
            this.Panel_Tipo_Cassa.Controls.Add(this.NC_lbl3);
            this.Panel_Tipo_Cassa.Controls.Add(this.textBoxCassa);
            this.Panel_Tipo_Cassa.Location = new System.Drawing.Point(12, 178);
            this.Panel_Tipo_Cassa.Name = "Panel_Tipo_Cassa";
            this.Panel_Tipo_Cassa.Size = new System.Drawing.Size(499, 124);
            this.Panel_Tipo_Cassa.TabIndex = 1;
            // 
            // textBoxCassa
            // 
            this.textBoxCassa.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxCassa.Location = new System.Drawing.Point(210, 16);
            this.textBoxCassa.Multiline = true;
            this.textBoxCassa.Name = "textBoxCassa";
            this.textBoxCassa.ReadOnly = true;
            this.textBoxCassa.Size = new System.Drawing.Size(270, 91);
            this.textBoxCassa.TabIndex = 2;
            this.textBoxCassa.TabStop = false;
            this.textBoxCassa.Text = "La cassa principale ...";
            // 
            // Panel_DB
            // 
            this.Panel_DB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Panel_DB.Controls.Add(this.NC_lbl4);
            this.Panel_DB.Controls.Add(this.combo_TipoDBase);
            this.Panel_DB.Controls.Add(this.NC_lbl2);
            this.Panel_DB.Controls.Add(this.dbPasswordEdit);
            this.Panel_DB.Controls.Add(this.Btn_DBServerTest);
            this.Panel_DB.Controls.Add(this.Combo_DBServerName);
            this.Panel_DB.Controls.Add(this.NC_lbl1);
            this.Panel_DB.Location = new System.Drawing.Point(12, 9);
            this.Panel_DB.Name = "Panel_DB";
            this.Panel_DB.Size = new System.Drawing.Size(499, 154);
            this.Panel_DB.TabIndex = 0;
            // 
            // NC_lbl4
            // 
            this.NC_lbl4.AutoSize = true;
            this.NC_lbl4.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NC_lbl4.Location = new System.Drawing.Point(314, 18);
            this.NC_lbl4.Name = "NC_lbl4";
            this.NC_lbl4.Size = new System.Drawing.Size(97, 14);
            this.NC_lbl4.TabIndex = 7;
            this.NC_lbl4.Text = "Tipo di database";
            // 
            // combo_TipoDBase
            // 
            this.combo_TipoDBase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_TipoDBase.Font = new System.Drawing.Font("Tahoma", 9F);
            this.combo_TipoDBase.FormattingEnabled = true;
            this.combo_TipoDBase.Location = new System.Drawing.Point(293, 44);
            this.combo_TipoDBase.Name = "combo_TipoDBase";
            this.combo_TipoDBase.Size = new System.Drawing.Size(186, 22);
            this.combo_TipoDBase.TabIndex = 6;
            this.combo_TipoDBase.SelectedIndexChanged += new System.EventHandler(this.Combo_NumCassa_SelectedIndexChanged);
            // 
            // NC_lbl2
            // 
            this.NC_lbl2.AutoSize = true;
            this.NC_lbl2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NC_lbl2.Location = new System.Drawing.Point(14, 92);
            this.NC_lbl2.Name = "NC_lbl2";
            this.NC_lbl2.Size = new System.Drawing.Size(171, 14);
            this.NC_lbl2.TabIndex = 4;
            this.NC_lbl2.Text = "Password utente \"standfacile\"";
            // 
            // dbPasswordEdit
            // 
            this.dbPasswordEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dbPasswordEdit.Location = new System.Drawing.Point(15, 112);
            this.dbPasswordEdit.MaxLength = 24;
            this.dbPasswordEdit.Name = "dbPasswordEdit";
            this.dbPasswordEdit.PasswordChar = '*';
            this.dbPasswordEdit.Size = new System.Drawing.Size(170, 22);
            this.dbPasswordEdit.TabIndex = 5;
            // 
            // Btn_DBServerTest
            // 
            this.Btn_DBServerTest.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_DBServerTest.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Btn_DBServerTest.Location = new System.Drawing.Point(293, 111);
            this.Btn_DBServerTest.Name = "Btn_DBServerTest";
            this.Btn_DBServerTest.Size = new System.Drawing.Size(171, 23);
            this.Btn_DBServerTest.TabIndex = 3;
            this.Btn_DBServerTest.Text = "Test Connessione DBase";
            this.Btn_DBServerTest.UseVisualStyleBackColor = true;
            this.Btn_DBServerTest.Click += new System.EventHandler(this.Btn_DBServerTest_Click);
            // 
            // Combo_DBServerName
            // 
            this.Combo_DBServerName.Font = new System.Drawing.Font("Tahoma", 9F);
            this.Combo_DBServerName.FormattingEnabled = true;
            this.Combo_DBServerName.Location = new System.Drawing.Point(15, 44);
            this.Combo_DBServerName.Name = "Combo_DBServerName";
            this.Combo_DBServerName.Size = new System.Drawing.Size(204, 22);
            this.Combo_DBServerName.TabIndex = 2;
            // 
            // NC_lbl1
            // 
            this.NC_lbl1.AutoSize = true;
            this.NC_lbl1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NC_lbl1.Location = new System.Drawing.Point(12, 18);
            this.NC_lbl1.Name = "NC_lbl1";
            this.NC_lbl1.Size = new System.Drawing.Size(232, 14);
            this.NC_lbl1.TabIndex = 1;
            this.NC_lbl1.Text = "Nome di rete o indirizzo IP del  DB Server";
            // 
            // panelWeb
            // 
            this.panelWeb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelWeb.Controls.Add(this.Edit_WebService_Name);
            this.panelWeb.Controls.Add(this.link_QRcode);
            this.panelWeb.Controls.Add(this.pictureBox_QRCode);
            this.panelWeb.Controls.Add(this.NC_lbl8);
            this.panelWeb.Controls.Add(this.NC_lbl5);
            this.panelWeb.Controls.Add(this.Edit_WebServiceDBaseName);
            this.panelWeb.Controls.Add(this.WO_ckBox);
            this.panelWeb.Controls.Add(this.NC_lbl6);
            this.panelWeb.Controls.Add(this.Edit_WebServiceDBasePwd);
            this.panelWeb.Controls.Add(this.NC_btn_webSiteTest);
            this.panelWeb.Controls.Add(this.NC_lbl7);
            this.panelWeb.Location = new System.Drawing.Point(12, 317);
            this.panelWeb.Name = "panelWeb";
            this.panelWeb.Size = new System.Drawing.Size(499, 250);
            this.panelWeb.TabIndex = 5;
            // 
            // Edit_WebService_Name
            // 
            this.Edit_WebService_Name.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Edit_WebService_Name.Location = new System.Drawing.Point(15, 161);
            this.Edit_WebService_Name.MaxLength = 24;
            this.Edit_WebService_Name.Name = "Edit_WebService_Name";
            this.Edit_WebService_Name.Size = new System.Drawing.Size(170, 22);
            this.Edit_WebService_Name.TabIndex = 16;
            // 
            // link_QRcode
            // 
            this.link_QRcode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.link_QRcode.Font = new System.Drawing.Font("Tahoma", 9F);
            this.link_QRcode.Location = new System.Drawing.Point(19, 226);
            this.link_QRcode.Name = "link_QRcode";
            this.link_QRcode.Size = new System.Drawing.Size(466, 16);
            this.link_QRcode.TabIndex = 15;
            this.link_QRcode.TabStop = true;
            this.link_QRcode.Text = "https://www.standfacile.org";
            this.link_QRcode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.link_QRcode.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.link_QRcode_LinkClicked);
            // 
            // pictureBox_QRCode
            // 
            this.pictureBox_QRCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox_QRCode.Location = new System.Drawing.Point(293, 72);
            this.pictureBox_QRCode.Name = "pictureBox_QRCode";
            this.pictureBox_QRCode.Size = new System.Drawing.Size(153, 142);
            this.pictureBox_QRCode.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_QRCode.TabIndex = 12;
            this.pictureBox_QRCode.TabStop = false;
            // 
            // NC_lbl8
            // 
            this.NC_lbl8.AutoSize = true;
            this.NC_lbl8.Enabled = false;
            this.NC_lbl8.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NC_lbl8.Location = new System.Drawing.Point(18, 141);
            this.NC_lbl8.Name = "NC_lbl8";
            this.NC_lbl8.Size = new System.Drawing.Size(163, 14);
            this.NC_lbl8.TabIndex = 10;
            this.NC_lbl8.Text = "https://www.standfacile.org";
            // 
            // NC_lbl5
            // 
            this.NC_lbl5.AutoSize = true;
            this.NC_lbl5.Enabled = false;
            this.NC_lbl5.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NC_lbl5.Location = new System.Drawing.Point(12, 14);
            this.NC_lbl5.Name = "NC_lbl5";
            this.NC_lbl5.Size = new System.Drawing.Size(92, 14);
            this.NC_lbl5.TabIndex = 8;
            this.NC_lbl5.Text = "Database Name";
            // 
            // Edit_WebServiceDBaseName
            // 
            this.Edit_WebServiceDBaseName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Edit_WebServiceDBaseName.Location = new System.Drawing.Point(15, 32);
            this.Edit_WebServiceDBaseName.MaxLength = 24;
            this.Edit_WebServiceDBaseName.Name = "Edit_WebServiceDBaseName";
            this.Edit_WebServiceDBaseName.Size = new System.Drawing.Size(170, 22);
            this.Edit_WebServiceDBaseName.TabIndex = 9;
            // 
            // WO_ckBox
            // 
            this.WO_ckBox.AutoSize = true;
            this.WO_ckBox.Font = new System.Drawing.Font("Tahoma", 9F);
            this.WO_ckBox.Location = new System.Drawing.Point(293, 13);
            this.WO_ckBox.Name = "WO_ckBox";
            this.WO_ckBox.Size = new System.Drawing.Size(192, 18);
            this.WO_ckBox.TabIndex = 6;
            this.WO_ckBox.Text = "attiva upload listino/ordini web";
            this.WO_ckBox.UseVisualStyleBackColor = true;
            this.WO_ckBox.CheckedChanged += new System.EventHandler(this.WO_ckBox_CheckedChanged);
            // 
            // NC_lbl6
            // 
            this.NC_lbl6.AutoSize = true;
            this.NC_lbl6.Enabled = false;
            this.NC_lbl6.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NC_lbl6.Location = new System.Drawing.Point(12, 68);
            this.NC_lbl6.Name = "NC_lbl6";
            this.NC_lbl6.Size = new System.Drawing.Size(112, 14);
            this.NC_lbl6.TabIndex = 4;
            this.NC_lbl6.Text = "Database Password";
            // 
            // Edit_WebServiceDBasePwd
            // 
            this.Edit_WebServiceDBasePwd.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Edit_WebServiceDBasePwd.Location = new System.Drawing.Point(15, 87);
            this.Edit_WebServiceDBasePwd.MaxLength = 24;
            this.Edit_WebServiceDBasePwd.Name = "Edit_WebServiceDBasePwd";
            this.Edit_WebServiceDBasePwd.PasswordChar = '*';
            this.Edit_WebServiceDBasePwd.Size = new System.Drawing.Size(170, 22);
            this.Edit_WebServiceDBasePwd.TabIndex = 5;
            // 
            // NC_btn_webSiteTest
            // 
            this.NC_btn_webSiteTest.Font = new System.Drawing.Font("Tahoma", 9F);
            this.NC_btn_webSiteTest.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.NC_btn_webSiteTest.Location = new System.Drawing.Point(293, 38);
            this.NC_btn_webSiteTest.Name = "NC_btn_webSiteTest";
            this.NC_btn_webSiteTest.Size = new System.Drawing.Size(153, 23);
            this.NC_btn_webSiteTest.TabIndex = 3;
            this.NC_btn_webSiteTest.Text = "Test Connessione web";
            this.NC_btn_webSiteTest.UseVisualStyleBackColor = true;
            this.NC_btn_webSiteTest.Click += new System.EventHandler(this.NC_btn_webSiteTest_Click);
            // 
            // NC_lbl7
            // 
            this.NC_lbl7.AutoSize = true;
            this.NC_lbl7.Enabled = false;
            this.NC_lbl7.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NC_lbl7.Location = new System.Drawing.Point(12, 123);
            this.NC_lbl7.Name = "NC_lbl7";
            this.NC_lbl7.Size = new System.Drawing.Size(185, 14);
            this.NC_lbl7.TabIndex = 1;
            this.NC_lbl7.Text = "Pagina webservice assegnata su:";
            // 
            // NetConfigDlg
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(526, 627);
            this.Controls.Add(this.panelWeb);
            this.Controls.Add(this.Panel_DB);
            this.Controls.Add(this.Panel_Tipo_Cassa);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NetConfigDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Configurazione di rete";
            this.Panel_Tipo_Cassa.ResumeLayout(false);
            this.Panel_Tipo_Cassa.PerformLayout();
            this.Panel_DB.ResumeLayout(false);
            this.Panel_DB.PerformLayout();
            this.panelWeb.ResumeLayout(false);
            this.panelWeb.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_QRCode)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ComboBox Combo_NumCassa;
        private System.Windows.Forms.Label NC_lbl3;
        private System.Windows.Forms.Panel Panel_Tipo_Cassa;
        private System.Windows.Forms.Button Btn_DBServerTest;
        private System.Windows.Forms.Label NC_lbl1;
        private System.Windows.Forms.TextBox textBoxCassa;
        private System.Windows.Forms.Label NC_lbl2;
        private System.Windows.Forms.TextBox dbPasswordEdit;
        private System.Windows.Forms.Label NC_lbl4;
        private System.Windows.Forms.ComboBox combo_TipoDBase;
        private System.Windows.Forms.Panel panelWeb;
        private System.Windows.Forms.Label NC_lbl5;
        private System.Windows.Forms.TextBox Edit_WebServiceDBaseName;
        private System.Windows.Forms.CheckBox WO_ckBox;
        private System.Windows.Forms.Label NC_lbl6;
        private System.Windows.Forms.TextBox Edit_WebServiceDBasePwd;
        private System.Windows.Forms.Button NC_btn_webSiteTest;
        private System.Windows.Forms.Label NC_lbl7;
        private System.Windows.Forms.Label NC_lbl8;
        private System.Windows.Forms.PictureBox pictureBox_QRCode;
        private System.Windows.Forms.LinkLabel link_QRcode;
        private System.Windows.Forms.TextBox Edit_WebService_Name;
        private System.Windows.Forms.ComboBox Combo_DBServerName;
        public System.Windows.Forms.Panel Panel_DB;
    }
}
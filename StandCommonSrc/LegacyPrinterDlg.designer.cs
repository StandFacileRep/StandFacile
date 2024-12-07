namespace StandFacile
{
    partial class LegacyPrinterDlg
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
            this.PORT_Combo = new System.Windows.Forms.ComboBox();
            this.PrinterTypeCombo = new System.Windows.Forms.ComboBox();
            this.LogoBmpCombo = new System.Windows.Forms.ComboBox();
            this.FontTypeCombo = new System.Windows.Forms.ComboBox();
            this.lblPrt = new System.Windows.Forms.Label();
            this.lblCom = new System.Windows.Forms.Label();
            this.lblLogo = new System.Windows.Forms.Label();
            this.lblFont = new System.Windows.Forms.Label();
            this.PaperSizeGroupBox = new System.Windows.Forms.GroupBox();
            this.width80 = new System.Windows.Forms.RadioButton();
            this.width57 = new System.Windows.Forms.RadioButton();
            this.speedGroupBox = new System.Windows.Forms.GroupBox();
            this.speedAlta = new System.Windows.Forms.RadioButton();
            this.speedBassa = new System.Windows.Forms.RadioButton();
            this.speedMedia = new System.Windows.Forms.RadioButton();
            this.densityGroupBox = new System.Windows.Forms.GroupBox();
            this.densAlta = new System.Windows.Forms.RadioButton();
            this.densMedia = new System.Windows.Forms.RadioButton();
            this.densBassa = new System.Windows.Forms.RadioButton();
            this.btnAutotest = new System.Windows.Forms.Button();
            this.btnTestoProva = new System.Windows.Forms.Button();
            this.btnInfo = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.Lbl_ImpSer = new System.Windows.Forms.Label();
            this.SettingsGroupBox = new System.Windows.Forms.GroupBox();
            this.flow_RTS_CTS = new System.Windows.Forms.RadioButton();
            this.flow_XON_XOFF = new System.Windows.Forms.RadioButton();
            this.flow_NONE = new System.Windows.Forms.RadioButton();
            this.FlowRadio = new System.Windows.Forms.GroupBox();
            this.PaperSizeGroupBox.SuspendLayout();
            this.speedGroupBox.SuspendLayout();
            this.densityGroupBox.SuspendLayout();
            this.SettingsGroupBox.SuspendLayout();
            this.FlowRadio.SuspendLayout();
            this.SuspendLayout();
            // 
            // PORT_Combo
            // 
            this.PORT_Combo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PORT_Combo.FormattingEnabled = true;
            this.PORT_Combo.Location = new System.Drawing.Point(229, 53);
            this.PORT_Combo.Margin = new System.Windows.Forms.Padding(4);
            this.PORT_Combo.Name = "PORT_Combo";
            this.PORT_Combo.Size = new System.Drawing.Size(138, 24);
            this.PORT_Combo.TabIndex = 5;
            // 
            // PrinterTypeCombo
            // 
            this.PrinterTypeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PrinterTypeCombo.FormattingEnabled = true;
            this.PrinterTypeCombo.Location = new System.Drawing.Point(16, 53);
            this.PrinterTypeCombo.Margin = new System.Windows.Forms.Padding(4);
            this.PrinterTypeCombo.Name = "PrinterTypeCombo";
            this.PrinterTypeCombo.Size = new System.Drawing.Size(165, 24);
            this.PrinterTypeCombo.TabIndex = 3;
            this.PrinterTypeCombo.SelectedIndexChanged += new System.EventHandler(this.PrinterTypeCombo_SelectedIndexChanged);
            // 
            // LogoBmpCombo
            // 
            this.LogoBmpCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.LogoBmpCombo.FormattingEnabled = true;
            this.LogoBmpCombo.Location = new System.Drawing.Point(17, 164);
            this.LogoBmpCombo.Margin = new System.Windows.Forms.Padding(4);
            this.LogoBmpCombo.Name = "LogoBmpCombo";
            this.LogoBmpCombo.Size = new System.Drawing.Size(164, 24);
            this.LogoBmpCombo.TabIndex = 8;
            // 
            // FontTypeCombo
            // 
            this.FontTypeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FontTypeCombo.Enabled = false;
            this.FontTypeCombo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FontTypeCombo.FormattingEnabled = true;
            this.FontTypeCombo.Location = new System.Drawing.Point(16, 285);
            this.FontTypeCombo.Margin = new System.Windows.Forms.Padding(4);
            this.FontTypeCombo.Name = "FontTypeCombo";
            this.FontTypeCombo.Size = new System.Drawing.Size(192, 23);
            this.FontTypeCombo.TabIndex = 12;
            // 
            // lblPrt
            // 
            this.lblPrt.AutoSize = true;
            this.lblPrt.Location = new System.Drawing.Point(12, 33);
            this.lblPrt.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPrt.Name = "lblPrt";
            this.lblPrt.Size = new System.Drawing.Size(144, 16);
            this.lblPrt.TabIndex = 2;
            this.lblPrt.Text = "Scelta della stampante";
            // 
            // lblCom
            // 
            this.lblCom.AutoSize = true;
            this.lblCom.Location = new System.Drawing.Point(226, 33);
            this.lblCom.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCom.Name = "lblCom";
            this.lblCom.Size = new System.Drawing.Size(141, 16);
            this.lblCom.TabIndex = 4;
            this.lblCom.Text = "Scelta porta COM/LPT";
            // 
            // lblLogo
            // 
            this.lblLogo.AutoSize = true;
            this.lblLogo.Location = new System.Drawing.Point(13, 145);
            this.lblLogo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLogo.Name = "lblLogo";
            this.lblLogo.Size = new System.Drawing.Size(101, 16);
            this.lblLogo.TabIndex = 7;
            this.lblLogo.Text = "Scelta del Logo";
            // 
            // lblFont
            // 
            this.lblFont.AutoSize = true;
            this.lblFont.Location = new System.Drawing.Point(12, 265);
            this.lblFont.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFont.Name = "lblFont";
            this.lblFont.Size = new System.Drawing.Size(210, 16);
            this.lblFont.TabIndex = 11;
            this.lblFont.Text = "Scelta del Font della Copia cliente";
            // 
            // PaperSizeGroupBox
            // 
            this.PaperSizeGroupBox.Controls.Add(this.width80);
            this.PaperSizeGroupBox.Controls.Add(this.width57);
            this.PaperSizeGroupBox.Location = new System.Drawing.Point(229, 114);
            this.PaperSizeGroupBox.Margin = new System.Windows.Forms.Padding(4);
            this.PaperSizeGroupBox.Name = "PaperSizeGroupBox";
            this.PaperSizeGroupBox.Padding = new System.Windows.Forms.Padding(4);
            this.PaperSizeGroupBox.Size = new System.Drawing.Size(138, 138);
            this.PaperSizeGroupBox.TabIndex = 9;
            this.PaperSizeGroupBox.TabStop = false;
            this.PaperSizeGroupBox.Text = "larghezza carta";
            // 
            // width80
            // 
            this.width80.AutoSize = true;
            this.width80.Location = new System.Drawing.Point(31, 85);
            this.width80.Margin = new System.Windows.Forms.Padding(4);
            this.width80.Name = "width80";
            this.width80.Size = new System.Drawing.Size(64, 20);
            this.width80.TabIndex = 1;
            this.width80.TabStop = true;
            this.width80.Text = "80 mm";
            this.width80.UseVisualStyleBackColor = true;
            this.width80.CheckedChanged += new System.EventHandler(this.PrinterTypeCombo_SelectedIndexChanged);
            // 
            // width57
            // 
            this.width57.AutoSize = true;
            this.width57.Location = new System.Drawing.Point(31, 28);
            this.width57.Margin = new System.Windows.Forms.Padding(4);
            this.width57.Name = "width57";
            this.width57.Size = new System.Drawing.Size(64, 20);
            this.width57.TabIndex = 0;
            this.width57.TabStop = true;
            this.width57.Text = "57 mm";
            this.width57.UseVisualStyleBackColor = true;
            this.width57.CheckedChanged += new System.EventHandler(this.PrinterTypeCombo_SelectedIndexChanged);
            // 
            // speedGroupBox
            // 
            this.speedGroupBox.Controls.Add(this.speedAlta);
            this.speedGroupBox.Controls.Add(this.speedBassa);
            this.speedGroupBox.Controls.Add(this.speedMedia);
            this.speedGroupBox.Location = new System.Drawing.Point(229, 285);
            this.speedGroupBox.Margin = new System.Windows.Forms.Padding(4);
            this.speedGroupBox.Name = "speedGroupBox";
            this.speedGroupBox.Padding = new System.Windows.Forms.Padding(4);
            this.speedGroupBox.Size = new System.Drawing.Size(138, 117);
            this.speedGroupBox.TabIndex = 13;
            this.speedGroupBox.TabStop = false;
            this.speedGroupBox.Text = "velocità di stampa";
            // 
            // speedAlta
            // 
            this.speedAlta.AutoSize = true;
            this.speedAlta.Location = new System.Drawing.Point(23, 84);
            this.speedAlta.Margin = new System.Windows.Forms.Padding(4);
            this.speedAlta.Name = "speedAlta";
            this.speedAlta.Size = new System.Drawing.Size(47, 20);
            this.speedAlta.TabIndex = 2;
            this.speedAlta.TabStop = true;
            this.speedAlta.Text = "alta";
            this.speedAlta.UseVisualStyleBackColor = true;
            this.speedAlta.CheckedChanged += new System.EventHandler(this.PrinterTypeCombo_SelectedIndexChanged);
            this.speedAlta.RightToLeftChanged += new System.EventHandler(this.PrinterTypeCombo_SelectedIndexChanged);
            // 
            // speedBassa
            // 
            this.speedBassa.AutoSize = true;
            this.speedBassa.Location = new System.Drawing.Point(23, 28);
            this.speedBassa.Margin = new System.Windows.Forms.Padding(4);
            this.speedBassa.Name = "speedBassa";
            this.speedBassa.Size = new System.Drawing.Size(63, 20);
            this.speedBassa.TabIndex = 0;
            this.speedBassa.TabStop = true;
            this.speedBassa.Text = "bassa";
            this.speedBassa.UseVisualStyleBackColor = true;
            this.speedBassa.CheckedChanged += new System.EventHandler(this.PrinterTypeCombo_SelectedIndexChanged);
            // 
            // speedMedia
            // 
            this.speedMedia.AutoSize = true;
            this.speedMedia.Location = new System.Drawing.Point(23, 56);
            this.speedMedia.Margin = new System.Windows.Forms.Padding(4);
            this.speedMedia.Name = "speedMedia";
            this.speedMedia.Size = new System.Drawing.Size(63, 20);
            this.speedMedia.TabIndex = 1;
            this.speedMedia.TabStop = true;
            this.speedMedia.Text = "media";
            this.speedMedia.UseVisualStyleBackColor = true;
            this.speedMedia.CheckedChanged += new System.EventHandler(this.PrinterTypeCombo_SelectedIndexChanged);
            // 
            // densityGroupBox
            // 
            this.densityGroupBox.Controls.Add(this.densAlta);
            this.densityGroupBox.Controls.Add(this.densMedia);
            this.densityGroupBox.Controls.Add(this.densBassa);
            this.densityGroupBox.Location = new System.Drawing.Point(390, 285);
            this.densityGroupBox.Margin = new System.Windows.Forms.Padding(4);
            this.densityGroupBox.Name = "densityGroupBox";
            this.densityGroupBox.Padding = new System.Windows.Forms.Padding(4);
            this.densityGroupBox.Size = new System.Drawing.Size(146, 117);
            this.densityGroupBox.TabIndex = 14;
            this.densityGroupBox.TabStop = false;
            this.densityGroupBox.Text = "densità di stampa";
            // 
            // densAlta
            // 
            this.densAlta.AutoSize = true;
            this.densAlta.Location = new System.Drawing.Point(29, 83);
            this.densAlta.Margin = new System.Windows.Forms.Padding(4);
            this.densAlta.Name = "densAlta";
            this.densAlta.Size = new System.Drawing.Size(47, 20);
            this.densAlta.TabIndex = 2;
            this.densAlta.TabStop = true;
            this.densAlta.Text = "alta";
            this.densAlta.UseVisualStyleBackColor = true;
            this.densAlta.CheckedChanged += new System.EventHandler(this.PrinterTypeCombo_SelectedIndexChanged);
            // 
            // densMedia
            // 
            this.densMedia.AutoSize = true;
            this.densMedia.Location = new System.Drawing.Point(29, 53);
            this.densMedia.Margin = new System.Windows.Forms.Padding(4);
            this.densMedia.Name = "densMedia";
            this.densMedia.Size = new System.Drawing.Size(63, 20);
            this.densMedia.TabIndex = 1;
            this.densMedia.TabStop = true;
            this.densMedia.Text = "media";
            this.densMedia.UseVisualStyleBackColor = true;
            this.densMedia.CheckedChanged += new System.EventHandler(this.PrinterTypeCombo_SelectedIndexChanged);
            // 
            // densBassa
            // 
            this.densBassa.AutoSize = true;
            this.densBassa.Location = new System.Drawing.Point(29, 23);
            this.densBassa.Margin = new System.Windows.Forms.Padding(4);
            this.densBassa.Name = "densBassa";
            this.densBassa.Size = new System.Drawing.Size(63, 20);
            this.densBassa.TabIndex = 0;
            this.densBassa.TabStop = true;
            this.densBassa.Text = "bassa";
            this.densBassa.UseVisualStyleBackColor = true;
            this.densBassa.CheckedChanged += new System.EventHandler(this.PrinterTypeCombo_SelectedIndexChanged);
            // 
            // btnAutotest
            // 
            this.btnAutotest.Location = new System.Drawing.Point(19, 427);
            this.btnAutotest.Margin = new System.Windows.Forms.Padding(4);
            this.btnAutotest.Name = "btnAutotest";
            this.btnAutotest.Size = new System.Drawing.Size(119, 28);
            this.btnAutotest.TabIndex = 15;
            this.btnAutotest.Text = "stampa Autotest";
            this.btnAutotest.UseVisualStyleBackColor = true;
            this.btnAutotest.Click += new System.EventHandler(this.btnAutotest_Click);
            // 
            // btnTestoProva
            // 
            this.btnTestoProva.Location = new System.Drawing.Point(367, 427);
            this.btnTestoProva.Margin = new System.Windows.Forms.Padding(4);
            this.btnTestoProva.Name = "btnTestoProva";
            this.btnTestoProva.Size = new System.Drawing.Size(160, 28);
            this.btnTestoProva.TabIndex = 17;
            this.btnTestoProva.Text = "stampa testo di prova";
            this.btnTestoProva.UseVisualStyleBackColor = true;
            this.btnTestoProva.Click += new System.EventHandler(this.btnTestoProva_Click);
            // 
            // btnInfo
            // 
            this.btnInfo.Location = new System.Drawing.Point(199, 427);
            this.btnInfo.Margin = new System.Windows.Forms.Padding(4);
            this.btnInfo.Name = "btnInfo";
            this.btnInfo.Size = new System.Drawing.Size(107, 28);
            this.btnInfo.TabIndex = 16;
            this.btnInfo.Text = "stampa Info";
            this.btnInfo.UseVisualStyleBackColor = true;
            this.btnInfo.Click += new System.EventHandler(this.btnInfo_Click);
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.SystemColors.Control;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Image = global::StandFacile.Properties.Resources.OK;
            this.btnOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOK.Location = new System.Drawing.Point(327, 485);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 28);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK  ";
            this.btnOK.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnCancel.Image = global::StandFacile.Properties.Resources.Cancel;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(153, 485);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 28);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Annulla";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // Lbl_ImpSer
            // 
            this.Lbl_ImpSer.AutoSize = true;
            this.Lbl_ImpSer.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Lbl_ImpSer.Location = new System.Drawing.Point(10, 28);
            this.Lbl_ImpSer.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Lbl_ImpSer.Name = "Lbl_ImpSer";
            this.Lbl_ImpSer.Size = new System.Drawing.Size(75, 16);
            this.Lbl_ImpSer.TabIndex = 0;
            this.Lbl_ImpSer.Text = "19200,N,8,1";
            // 
            // SettingsGroupBox
            // 
            this.SettingsGroupBox.Controls.Add(this.Lbl_ImpSer);
            this.SettingsGroupBox.Location = new System.Drawing.Point(390, 33);
            this.SettingsGroupBox.Name = "SettingsGroupBox";
            this.SettingsGroupBox.Size = new System.Drawing.Size(146, 58);
            this.SettingsGroupBox.TabIndex = 6;
            this.SettingsGroupBox.TabStop = false;
            this.SettingsGroupBox.Text = "impostazioni seriale";
            // 
            // flow_RTS_CTS
            // 
            this.flow_RTS_CTS.AutoSize = true;
            this.flow_RTS_CTS.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.flow_RTS_CTS.Location = new System.Drawing.Point(13, 64);
            this.flow_RTS_CTS.Name = "flow_RTS_CTS";
            this.flow_RTS_CTS.Size = new System.Drawing.Size(123, 17);
            this.flow_RTS_CTS.TabIndex = 1;
            this.flow_RTS_CTS.TabStop = true;
            this.flow_RTS_CTS.Text = "RTS/CTS (standard)";
            this.flow_RTS_CTS.UseVisualStyleBackColor = true;
            // 
            // flow_XON_XOFF
            // 
            this.flow_XON_XOFF.AutoSize = true;
            this.flow_XON_XOFF.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.flow_XON_XOFF.Location = new System.Drawing.Point(13, 101);
            this.flow_XON_XOFF.Name = "flow_XON_XOFF";
            this.flow_XON_XOFF.Size = new System.Drawing.Size(80, 17);
            this.flow_XON_XOFF.TabIndex = 2;
            this.flow_XON_XOFF.TabStop = true;
            this.flow_XON_XOFF.Text = "XON/XOFF";
            this.flow_XON_XOFF.UseVisualStyleBackColor = true;
            // 
            // flow_NONE
            // 
            this.flow_NONE.AutoSize = true;
            this.flow_NONE.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.flow_NONE.Location = new System.Drawing.Point(13, 27);
            this.flow_NONE.Name = "flow_NONE";
            this.flow_NONE.Size = new System.Drawing.Size(112, 17);
            this.flow_NONE.TabIndex = 0;
            this.flow_NONE.TabStop = true;
            this.flow_NONE.Text = "NESSUNO (delay)";
            this.flow_NONE.UseVisualStyleBackColor = true;
            // 
            // FlowRadio
            // 
            this.FlowRadio.Controls.Add(this.flow_NONE);
            this.FlowRadio.Controls.Add(this.flow_XON_XOFF);
            this.FlowRadio.Controls.Add(this.flow_RTS_CTS);
            this.FlowRadio.Location = new System.Drawing.Point(390, 114);
            this.FlowRadio.Name = "FlowRadio";
            this.FlowRadio.Size = new System.Drawing.Size(146, 138);
            this.FlowRadio.TabIndex = 10;
            this.FlowRadio.TabStop = false;
            this.FlowRadio.Text = "controllo di flusso";
            // 
            // ThermPrinterDlg
            // 
            this.AcceptButton = this.btnCancel;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.ClientSize = new System.Drawing.Size(558, 531);
            this.Controls.Add(this.FlowRadio);
            this.Controls.Add(this.SettingsGroupBox);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnInfo);
            this.Controls.Add(this.btnTestoProva);
            this.Controls.Add(this.btnAutotest);
            this.Controls.Add(this.speedGroupBox);
            this.Controls.Add(this.densityGroupBox);
            this.Controls.Add(this.PaperSizeGroupBox);
            this.Controls.Add(this.lblFont);
            this.Controls.Add(this.lblLogo);
            this.Controls.Add(this.lblCom);
            this.Controls.Add(this.lblPrt);
            this.Controls.Add(this.FontTypeCombo);
            this.Controls.Add(this.LogoBmpCombo);
            this.Controls.Add(this.PrinterTypeCombo);
            this.Controls.Add(this.PORT_Combo);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ThermPrinterDlg";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Impostazioni stampante Legacy";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ThermPrinterDlg_FormClosing);
            this.PaperSizeGroupBox.ResumeLayout(false);
            this.PaperSizeGroupBox.PerformLayout();
            this.speedGroupBox.ResumeLayout(false);
            this.speedGroupBox.PerformLayout();
            this.densityGroupBox.ResumeLayout(false);
            this.densityGroupBox.PerformLayout();
            this.SettingsGroupBox.ResumeLayout(false);
            this.SettingsGroupBox.PerformLayout();
            this.FlowRadio.ResumeLayout(false);
            this.FlowRadio.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox PORT_Combo;
        private System.Windows.Forms.ComboBox PrinterTypeCombo;
        private System.Windows.Forms.ComboBox LogoBmpCombo;
        private System.Windows.Forms.ComboBox FontTypeCombo;
        private System.Windows.Forms.Label lblPrt;
        private System.Windows.Forms.Label lblCom;
        private System.Windows.Forms.Label lblLogo;
        private System.Windows.Forms.Label lblFont;
        private System.Windows.Forms.GroupBox PaperSizeGroupBox;
        private System.Windows.Forms.RadioButton width80;
        private System.Windows.Forms.RadioButton width57;
        private System.Windows.Forms.GroupBox speedGroupBox;
        private System.Windows.Forms.GroupBox densityGroupBox;
        private System.Windows.Forms.RadioButton densMedia;
        private System.Windows.Forms.RadioButton densBassa;
        private System.Windows.Forms.RadioButton densAlta;
        private System.Windows.Forms.RadioButton speedAlta;
        private System.Windows.Forms.RadioButton speedBassa;
        private System.Windows.Forms.RadioButton speedMedia;
        private System.Windows.Forms.Button btnAutotest;
        private System.Windows.Forms.Button btnTestoProva;
        private System.Windows.Forms.Button btnInfo;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label Lbl_ImpSer;
        private System.Windows.Forms.GroupBox SettingsGroupBox;
        private System.Windows.Forms.RadioButton flow_RTS_CTS;
        private System.Windows.Forms.RadioButton flow_XON_XOFF;
        private System.Windows.Forms.RadioButton flow_NONE;
        private System.Windows.Forms.GroupBox FlowRadio;
    }
}
namespace StandFacile
{
    partial class VisDatiDlg
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
            this.textEditDati = new System.Windows.Forms.TextBox();
            this.CheckBoxRidColonne = new System.Windows.Forms.CheckBox();
            this.CheckBoxXls = new System.Windows.Forms.CheckBox();
            this.LblCassa = new System.Windows.Forms.Label();
            this.Combo_NumCassa = new System.Windows.Forms.ComboBox();
            this.CkBoxUnioneCasse = new System.Windows.Forms.CheckBox();
            this.OKBtn = new System.Windows.Forms.Button();
            this.BtnXls = new System.Windows.Forms.Button();
            this.BtnDate = new System.Windows.Forms.Button();
            this.BtnPrt = new System.Windows.Forms.Button();
            this.comboReport = new System.Windows.Forms.ComboBox();
            this.LblReport = new System.Windows.Forms.Label();
            this.CkBoxSkipZero = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // textEditDati
            // 
            this.textEditDati.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textEditDati.BackColor = System.Drawing.Color.Teal;
            this.textEditDati.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textEditDati.ForeColor = System.Drawing.SystemColors.Window;
            this.textEditDati.Location = new System.Drawing.Point(12, 12);
            this.textEditDati.Multiline = true;
            this.textEditDati.Name = "textEditDati";
            this.textEditDati.ReadOnly = true;
            this.textEditDati.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textEditDati.Size = new System.Drawing.Size(568, 333);
            this.textEditDati.TabIndex = 1;
            // 
            // CheckBoxRidColonne
            // 
            this.CheckBoxRidColonne.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CheckBoxRidColonne.AutoSize = true;
            this.CheckBoxRidColonne.Checked = true;
            this.CheckBoxRidColonne.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckBoxRidColonne.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CheckBoxRidColonne.Location = new System.Drawing.Point(208, 366);
            this.CheckBoxRidColonne.Name = "CheckBoxRidColonne";
            this.CheckBoxRidColonne.Size = new System.Drawing.Size(156, 18);
            this.CheckBoxRidColonne.TabIndex = 3;
            this.CheckBoxRidColonne.Text = "Riduzione colonne/righe";
            this.CheckBoxRidColonne.UseVisualStyleBackColor = true;
            this.CheckBoxRidColonne.Click += new System.EventHandler(this.CheckBoxRidColonne_Click);
            // 
            // CheckBoxXls
            // 
            this.CheckBoxXls.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CheckBoxXls.AutoSize = true;
            this.CheckBoxXls.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CheckBoxXls.Location = new System.Drawing.Point(12, 366);
            this.CheckBoxXls.Name = "CheckBoxXls";
            this.CheckBoxXls.Size = new System.Drawing.Size(179, 18);
            this.CheckBoxXls.TabIndex = 2;
            this.CheckBoxXls.Text = "Visualizza esportazione Excel";
            this.CheckBoxXls.UseVisualStyleBackColor = true;
            // 
            // LblCassa
            // 
            this.LblCassa.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LblCassa.AutoSize = true;
            this.LblCassa.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblCassa.Location = new System.Drawing.Point(229, 437);
            this.LblCassa.Name = "LblCassa";
            this.LblCassa.Size = new System.Drawing.Size(110, 14);
            this.LblCassa.TabIndex = 4;
            this.LblCassa.Text = "numero della Cassa";
            // 
            // Combo_NumCassa
            // 
            this.Combo_NumCassa.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Combo_NumCassa.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Combo_NumCassa.Enabled = false;
            this.Combo_NumCassa.FormattingEnabled = true;
            this.Combo_NumCassa.Location = new System.Drawing.Point(200, 454);
            this.Combo_NumCassa.Name = "Combo_NumCassa";
            this.Combo_NumCassa.Size = new System.Drawing.Size(164, 21);
            this.Combo_NumCassa.TabIndex = 5;
            this.Combo_NumCassa.SelectedIndexChanged += new System.EventHandler(this.Combo_NumCassa_SelectedIndexChanged);
            // 
            // CkBoxUnioneCasse
            // 
            this.CkBoxUnioneCasse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CkBoxUnioneCasse.AutoSize = true;
            this.CkBoxUnioneCasse.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CkBoxUnioneCasse.Location = new System.Drawing.Point(12, 400);
            this.CkBoxUnioneCasse.Name = "CkBoxUnioneCasse";
            this.CkBoxUnioneCasse.Size = new System.Drawing.Size(121, 18);
            this.CkBoxUnioneCasse.TabIndex = 6;
            this.CkBoxUnioneCasse.Text = "Unione dati casse";
            this.CkBoxUnioneCasse.UseVisualStyleBackColor = true;
            this.CkBoxUnioneCasse.Click += new System.EventHandler(this.CheckBoxRidColonne_Click);
            // 
            // OKBtn
            // 
            this.OKBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.OKBtn.BackColor = System.Drawing.SystemColors.Control;
            this.OKBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OKBtn.Image = global::StandFacile.Properties.Resources.OK;
            this.OKBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.OKBtn.Location = new System.Drawing.Point(489, 446);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(70, 35);
            this.OKBtn.TabIndex = 0;
            this.OKBtn.Text = "OK";
            this.OKBtn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            this.OKBtn.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OKBtn_KeyPress);
            // 
            // BtnXls
            // 
            this.BtnXls.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BtnXls.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.BtnXls.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BtnXls.Image = global::StandFacile.Properties.Resources.Xls;
            this.BtnXls.Location = new System.Drawing.Point(393, 366);
            this.BtnXls.Name = "BtnXls";
            this.BtnXls.Size = new System.Drawing.Size(70, 35);
            this.BtnXls.TabIndex = 7;
            this.BtnXls.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.BtnXls.UseVisualStyleBackColor = true;
            this.BtnXls.Click += new System.EventHandler(this.BtnXls_Click);
            // 
            // BtnDate
            // 
            this.BtnDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BtnDate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.BtnDate.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BtnDate.Image = global::StandFacile.Properties.Resources.Calendar;
            this.BtnDate.Location = new System.Drawing.Point(489, 366);
            this.BtnDate.Name = "BtnDate";
            this.BtnDate.Size = new System.Drawing.Size(70, 35);
            this.BtnDate.TabIndex = 8;
            this.BtnDate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.BtnDate.UseVisualStyleBackColor = true;
            this.BtnDate.Click += new System.EventHandler(this.BtnDate_Click);
            // 
            // BtnPrt
            // 
            this.BtnPrt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BtnPrt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.BtnPrt.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BtnPrt.Image = global::StandFacile.Properties.Resources.printer_s;
            this.BtnPrt.Location = new System.Drawing.Point(393, 446);
            this.BtnPrt.Name = "BtnPrt";
            this.BtnPrt.Size = new System.Drawing.Size(70, 35);
            this.BtnPrt.TabIndex = 9;
            this.BtnPrt.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.BtnPrt.UseVisualStyleBackColor = true;
            this.BtnPrt.Click += new System.EventHandler(this.BtnPrt_Click);
            // 
            // comboReport
            // 
            this.comboReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboReport.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboReport.FormattingEnabled = true;
            this.comboReport.Location = new System.Drawing.Point(21, 454);
            this.comboReport.Name = "comboReport";
            this.comboReport.Size = new System.Drawing.Size(164, 21);
            this.comboReport.TabIndex = 10;
            this.comboReport.SelectedIndexChanged += new System.EventHandler(this.ComboReport_SelectedIndexChanged);
            // 
            // LblReport
            // 
            this.LblReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LblReport.AutoSize = true;
            this.LblReport.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblReport.Location = new System.Drawing.Point(52, 437);
            this.LblReport.Name = "LblReport";
            this.LblReport.Size = new System.Drawing.Size(90, 14);
            this.LblReport.TabIndex = 11;
            this.LblReport.Text = "modalità report";
            // 
            // CkBoxSkipZero
            // 
            this.CkBoxSkipZero.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CkBoxSkipZero.AutoSize = true;
            this.CkBoxSkipZero.Checked = true;
            this.CkBoxSkipZero.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CkBoxSkipZero.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CkBoxSkipZero.Location = new System.Drawing.Point(208, 400);
            this.CkBoxSkipZero.Name = "CkBoxSkipZero";
            this.CkBoxSkipZero.Size = new System.Drawing.Size(144, 18);
            this.CkBoxSkipZero.TabIndex = 12;
            this.CkBoxSkipZero.Text = "tralascia quantità zero";
            this.CkBoxSkipZero.UseVisualStyleBackColor = true;
            this.CkBoxSkipZero.Click += new System.EventHandler(this.CheckBoxRidColonne_Click);
            // 
            // VisDatiDlg
            // 
            this.AcceptButton = this.OKBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.OKBtn;
            this.ClientSize = new System.Drawing.Size(592, 494);
            this.Controls.Add(this.CkBoxSkipZero);
            this.Controls.Add(this.LblReport);
            this.Controls.Add(this.comboReport);
            this.Controls.Add(this.BtnXls);
            this.Controls.Add(this.BtnDate);
            this.Controls.Add(this.CkBoxUnioneCasse);
            this.Controls.Add(this.Combo_NumCassa);
            this.Controls.Add(this.LblCassa);
            this.Controls.Add(this.CheckBoxXls);
            this.Controls.Add(this.CheckBoxRidColonne);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.BtnPrt);
            this.Controls.Add(this.textEditDati);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VisDatiDlg";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Incasso";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

#endregion

        private System.Windows.Forms.TextBox textEditDati;
        private System.Windows.Forms.Button BtnPrt;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.CheckBox CheckBoxRidColonne;
        private System.Windows.Forms.CheckBox CheckBoxXls;
        private System.Windows.Forms.Label LblCassa;
        private System.Windows.Forms.ComboBox Combo_NumCassa;
        private System.Windows.Forms.CheckBox CkBoxUnioneCasse;
        private System.Windows.Forms.Button BtnDate;
        private System.Windows.Forms.Button BtnXls;
        private System.Windows.Forms.ComboBox comboReport;
        private System.Windows.Forms.Label LblReport;
        private System.Windows.Forms.CheckBox CkBoxSkipZero;
    }
}
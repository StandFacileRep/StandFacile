namespace StandFacile
{
    partial class OptionsDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsDlg));
            this.checkBoxCoperti = new System.Windows.Forms.CheckBox();
            this.checkBoxTavolo = new System.Windows.Forms.CheckBox();
            this.checkBoxDisp = new System.Windows.Forms.CheckBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panelPresale = new System.Windows.Forms.Panel();
            this.checkBoxPresales_loadMode = new System.Windows.Forms.CheckBox();
            this.checkBoxPresale_workMode = new System.Windows.Forms.CheckBox();
            this.checkBoxPayment = new System.Windows.Forms.CheckBox();
            this.checkBoxPrivacy = new System.Windows.Forms.CheckBox();
            this.comboColorTheme = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBox_Enter = new System.Windows.Forms.CheckBox();
            this.checkBox_VButtons = new System.Windows.Forms.CheckBox();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.checkBoxShowPrevReceipt = new System.Windows.Forms.CheckBox();
            this.checkBox_ZeroPriceItems = new System.Windows.Forms.CheckBox();
            this.panelPresale.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // checkBoxCoperti
            // 
            this.checkBoxCoperti.AutoSize = true;
            this.checkBoxCoperti.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxCoperti.Location = new System.Drawing.Point(15, 107);
            this.checkBoxCoperti.Name = "checkBoxCoperti";
            this.checkBoxCoperti.Size = new System.Drawing.Size(296, 18);
            this.checkBoxCoperti.TabIndex = 3;
            this.checkBoxCoperti.Text = "rendi obbligatorio l\'inserimento del num di Coperti";
            this.checkBoxCoperti.UseVisualStyleBackColor = true;
            // 
            // checkBoxTavolo
            // 
            this.checkBoxTavolo.AutoSize = true;
            this.checkBoxTavolo.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxTavolo.Location = new System.Drawing.Point(15, 69);
            this.checkBoxTavolo.Name = "checkBoxTavolo";
            this.checkBoxTavolo.Size = new System.Drawing.Size(252, 18);
            this.checkBoxTavolo.TabIndex = 2;
            this.checkBoxTavolo.Text = "rendi obbligatorio l\'inserimento del Tavolo";
            this.checkBoxTavolo.UseVisualStyleBackColor = true;
            // 
            // checkBoxDisp
            // 
            this.checkBoxDisp.AutoSize = true;
            this.checkBoxDisp.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxDisp.Location = new System.Drawing.Point(360, 107);
            this.checkBoxDisp.Name = "checkBoxDisp";
            this.checkBoxDisp.Size = new System.Drawing.Size(328, 18);
            this.checkBoxDisp.TabIndex = 7;
            this.checkBoxDisp.Text = "mostra all\'avvio il dialogo di selezione disponibilità Articoli";
            this.checkBoxDisp.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOK.Image = ((System.Drawing.Image)(resources.GetObject("btnOK.Image")));
            this.btnOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOK.Location = new System.Drawing.Point(390, 299);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 32);
            this.btnOK.TabIndex = 11;
            this.btnOK.Text = "OK   ";
            this.btnOK.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(223, 299);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 32);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Annulla";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // panelPresale
            // 
            this.panelPresale.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelPresale.Controls.Add(this.checkBoxPresales_loadMode);
            this.panelPresale.Controls.Add(this.checkBoxPresale_workMode);
            this.panelPresale.Location = new System.Drawing.Point(349, 186);
            this.panelPresale.Name = "panelPresale";
            this.panelPresale.Size = new System.Drawing.Size(312, 90);
            this.panelPresale.TabIndex = 8;
            // 
            // checkBoxPresales_loadMode
            // 
            this.checkBoxPresales_loadMode.AutoSize = true;
            this.checkBoxPresales_loadMode.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxPresales_loadMode.Location = new System.Drawing.Point(11, 53);
            this.checkBoxPresales_loadMode.Name = "checkBoxPresales_loadMode";
            this.checkBoxPresales_loadMode.Size = new System.Drawing.Size(249, 18);
            this.checkBoxPresales_loadMode.TabIndex = 10;
            this.checkBoxPresales_loadMode.Text = "modalità caricamento ordini in prevendita";
            this.checkBoxPresales_loadMode.UseVisualStyleBackColor = true;
            this.checkBoxPresales_loadMode.CheckedChanged += new System.EventHandler(this.CheckBoxLoadPresale_CheckedChanged);
            // 
            // checkBoxPresale_workMode
            // 
            this.checkBoxPresale_workMode.AutoSize = true;
            this.checkBoxPresale_workMode.Enabled = false;
            this.checkBoxPresale_workMode.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxPresale_workMode.Location = new System.Drawing.Point(11, 19);
            this.checkBoxPresale_workMode.Name = "checkBoxPresale_workMode";
            this.checkBoxPresale_workMode.Size = new System.Drawing.Size(291, 18);
            this.checkBoxPresale_workMode.TabIndex = 9;
            this.checkBoxPresale_workMode.Text = "modalità giorno di prevendita (riavvio necessario)";
            this.checkBoxPresale_workMode.UseVisualStyleBackColor = true;
            this.checkBoxPresale_workMode.CheckedChanged += new System.EventHandler(this.CheckBoxPresaleMode_CheckedChanged);
            // 
            // checkBoxPayment
            // 
            this.checkBoxPayment.AutoSize = true;
            this.checkBoxPayment.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxPayment.Location = new System.Drawing.Point(15, 145);
            this.checkBoxPayment.Name = "checkBoxPayment";
            this.checkBoxPayment.Size = new System.Drawing.Size(326, 18);
            this.checkBoxPayment.TabIndex = 4;
            this.checkBoxPayment.Text = "rendi obbligatorio l\'inserimento del modo di pagamento";
            this.checkBoxPayment.UseVisualStyleBackColor = true;
            // 
            // checkBoxPrivacy
            // 
            this.checkBoxPrivacy.AutoSize = true;
            this.checkBoxPrivacy.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxPrivacy.Location = new System.Drawing.Point(15, 259);
            this.checkBoxPrivacy.Name = "checkBoxPrivacy";
            this.checkBoxPrivacy.Size = new System.Drawing.Size(205, 18);
            this.checkBoxPrivacy.TabIndex = 5;
            this.checkBoxPrivacy.Text = "modo riservato (nascondi incassi)";
            this.checkBoxPrivacy.UseVisualStyleBackColor = true;
            // 
            // comboColorTheme
            // 
            this.comboColorTheme.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboColorTheme.FormattingEnabled = true;
            this.comboColorTheme.Items.AddRange(new object[] {
            "Classico con diversi colori carattere",
            "Chiaro con diversi colori sfondo",
            "Vivace con diversi colori sfondo"});
            this.comboColorTheme.Location = new System.Drawing.Point(15, 20);
            this.comboColorTheme.Name = "comboColorTheme";
            this.comboColorTheme.Size = new System.Drawing.Size(205, 21);
            this.comboColorTheme.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F);
            this.label1.Location = new System.Drawing.Point(231, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 14);
            this.label1.TabIndex = 22;
            this.label1.Text = "selezione tema colori";
            // 
            // checkBox_Enter
            // 
            this.checkBox_Enter.AutoSize = true;
            this.checkBox_Enter.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox_Enter.Location = new System.Drawing.Point(15, 221);
            this.checkBox_Enter.Name = "checkBox_Enter";
            this.checkBox_Enter.Size = new System.Drawing.Size(234, 18);
            this.checkBox_Enter.TabIndex = 6;
            this.checkBox_Enter.Text = "consenti stampa scontrino con ENTER";
            this.checkBox_Enter.UseVisualStyleBackColor = true;
            // 
            // checkBox_VButtons
            // 
            this.checkBox_VButtons.AutoSize = true;
            this.checkBox_VButtons.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox_VButtons.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBox_VButtons.Location = new System.Drawing.Point(360, 69);
            this.checkBox_VButtons.Name = "checkBox_VButtons";
            this.checkBox_VButtons.Size = new System.Drawing.Size(156, 18);
            this.checkBox_VButtons.TabIndex = 23;
            this.checkBox_VButtons.Text = "attiva barra pulsanti      ";
            this.checkBox_VButtons.UseVisualStyleBackColor = true;
            // 
            // pictureBox
            // 
            this.pictureBox.ErrorImage = null;
            this.pictureBox.Image = global::StandFacile.Properties.Resources.plusMinCan;
            this.pictureBox.InitialImage = null;
            this.pictureBox.Location = new System.Drawing.Point(495, 65);
            this.pictureBox.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(70, 26);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox.TabIndex = 24;
            this.pictureBox.TabStop = false;
            // 
            // checkBoxShowPrevReceipt
            // 
            this.checkBoxShowPrevReceipt.AutoSize = true;
            this.checkBoxShowPrevReceipt.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxShowPrevReceipt.Location = new System.Drawing.Point(360, 145);
            this.checkBoxShowPrevReceipt.Name = "checkBoxShowPrevReceipt";
            this.checkBoxShowPrevReceipt.Size = new System.Drawing.Size(319, 18);
            this.checkBoxShowPrevReceipt.TabIndex = 25;
            this.checkBoxShowPrevReceipt.Text = "mostra nello stato il totale dello scontrino precedente";
            this.checkBoxShowPrevReceipt.UseVisualStyleBackColor = true;
            // 
            // checkBox_ZeroPriceItems
            // 
            this.checkBox_ZeroPriceItems.AutoSize = true;
            this.checkBox_ZeroPriceItems.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox_ZeroPriceItems.Location = new System.Drawing.Point(15, 183);
            this.checkBox_ZeroPriceItems.Name = "checkBox_ZeroPriceItems";
            this.checkBox_ZeroPriceItems.Size = new System.Drawing.Size(202, 18);
            this.checkBox_ZeroPriceItems.TabIndex = 26;
            this.checkBox_ZeroPriceItems.Text = "consenti Articoli con prezzo zero";
            this.checkBox_ZeroPriceItems.UseVisualStyleBackColor = true;
            // 
            // OptionsDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(696, 347);
            this.Controls.Add(this.checkBox_ZeroPriceItems);
            this.Controls.Add(this.checkBoxShowPrevReceipt);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.checkBox_VButtons);
            this.Controls.Add(this.checkBox_Enter);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboColorTheme);
            this.Controls.Add(this.checkBoxPrivacy);
            this.Controls.Add(this.checkBoxPayment);
            this.Controls.Add(this.panelPresale);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.checkBoxDisp);
            this.Controls.Add(this.checkBoxCoperti);
            this.Controls.Add(this.checkBoxTavolo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsDlg";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Opzioni varie";
            this.panelPresale.ResumeLayout(false);
            this.panelPresale.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBoxCoperti;
        private System.Windows.Forms.CheckBox checkBoxTavolo;
        private System.Windows.Forms.CheckBox checkBoxDisp;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel panelPresale;
        private System.Windows.Forms.CheckBox checkBoxPresales_loadMode;
        private System.Windows.Forms.CheckBox checkBoxPresale_workMode;
        private System.Windows.Forms.CheckBox checkBoxPayment;
        private System.Windows.Forms.CheckBox checkBoxPrivacy;
        private System.Windows.Forms.ComboBox comboColorTheme;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBox_Enter;
        private System.Windows.Forms.CheckBox checkBox_VButtons;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.CheckBox checkBoxShowPrevReceipt;
        private System.Windows.Forms.CheckBox checkBox_ZeroPriceItems;
    }
}
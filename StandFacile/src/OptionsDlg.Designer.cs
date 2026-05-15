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
            this.checkBox_Coperti = new System.Windows.Forms.CheckBox();
            this.checkBox_Tavolo = new System.Windows.Forms.CheckBox();
            this.checkBox_InitialAvailabilityDlg = new System.Windows.Forms.CheckBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panelPresale = new System.Windows.Forms.Panel();
            this.checkBox_Presales_loadMode = new System.Windows.Forms.CheckBox();
            this.checkBox_Presale_workMode = new System.Windows.Forms.CheckBox();
            this.checkBox_Payment = new System.Windows.Forms.CheckBox();
            this.checkBox_Privacy = new System.Windows.Forms.CheckBox();
            this.comboColorTheme = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBox_Enter = new System.Windows.Forms.CheckBox();
            this.checkBox_ShowPrevReceipt = new System.Windows.Forms.CheckBox();
            this.checkBox_ZeroPriceItems = new System.Windows.Forms.CheckBox();
            this.btnReset = new System.Windows.Forms.Button();
            this.panelShowHideButtons = new System.Windows.Forms.Panel();
            this.checkBox_ShowDiscount = new System.Windows.Forms.CheckBox();
            this.checkBox_ShowSendMsg = new System.Windows.Forms.CheckBox();
            this.checkBox_ShowTakeAway = new System.Windows.Forms.CheckBox();
            this.checkBox_Show_DB_Check = new System.Windows.Forms.CheckBox();
            this.checkBox_Show_OSKeyb = new System.Windows.Forms.CheckBox();
            this.checkBox_Name = new System.Windows.Forms.CheckBox();
            this.panelPresale.SuspendLayout();
            this.panelShowHideButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkBox_Coperti
            // 
            this.checkBox_Coperti.AutoSize = true;
            this.checkBox_Coperti.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox_Coperti.Location = new System.Drawing.Point(15, 69);
            this.checkBox_Coperti.Name = "checkBox_Coperti";
            this.checkBox_Coperti.Size = new System.Drawing.Size(296, 18);
            this.checkBox_Coperti.TabIndex = 3;
            this.checkBox_Coperti.Text = "rendi obbligatorio l\'inserimento del num di Coperti";
            this.checkBox_Coperti.UseVisualStyleBackColor = true;
            // 
            // checkBox_Tavolo
            // 
            this.checkBox_Tavolo.AutoSize = true;
            this.checkBox_Tavolo.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox_Tavolo.Location = new System.Drawing.Point(15, 105);
            this.checkBox_Tavolo.Name = "checkBox_Tavolo";
            this.checkBox_Tavolo.Size = new System.Drawing.Size(252, 18);
            this.checkBox_Tavolo.TabIndex = 2;
            this.checkBox_Tavolo.Text = "rendi obbligatorio l\'inserimento del Tavolo";
            this.checkBox_Tavolo.UseVisualStyleBackColor = true;
            // 
            // checkBox_InitialAvailabilityDlg
            // 
            this.checkBox_InitialAvailabilityDlg.AutoSize = true;
            this.checkBox_InitialAvailabilityDlg.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox_InitialAvailabilityDlg.Location = new System.Drawing.Point(15, 333);
            this.checkBox_InitialAvailabilityDlg.Name = "checkBox_InitialAvailabilityDlg";
            this.checkBox_InitialAvailabilityDlg.Size = new System.Drawing.Size(328, 18);
            this.checkBox_InitialAvailabilityDlg.TabIndex = 7;
            this.checkBox_InitialAvailabilityDlg.Text = "mostra all\'avvio il dialogo di selezione disponibilità Articoli";
            this.checkBox_InitialAvailabilityDlg.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOK.Image = ((System.Drawing.Image)(resources.GetObject("btnOK.Image")));
            this.btnOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOK.Location = new System.Drawing.Point(440, 423);
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
            this.btnCancel.Location = new System.Drawing.Point(311, 423);
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
            this.panelPresale.Controls.Add(this.checkBox_Presales_loadMode);
            this.panelPresale.Controls.Add(this.checkBox_Presale_workMode);
            this.panelPresale.Location = new System.Drawing.Point(354, 285);
            this.panelPresale.Name = "panelPresale";
            this.panelPresale.Size = new System.Drawing.Size(312, 102);
            this.panelPresale.TabIndex = 8;
            // 
            // checkBox_Presales_loadMode
            // 
            this.checkBox_Presales_loadMode.AutoSize = true;
            this.checkBox_Presales_loadMode.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox_Presales_loadMode.Location = new System.Drawing.Point(11, 58);
            this.checkBox_Presales_loadMode.Name = "checkBox_Presales_loadMode";
            this.checkBox_Presales_loadMode.Size = new System.Drawing.Size(249, 18);
            this.checkBox_Presales_loadMode.TabIndex = 10;
            this.checkBox_Presales_loadMode.Text = "modalità caricamento ordini in prevendita";
            this.checkBox_Presales_loadMode.UseVisualStyleBackColor = true;
            this.checkBox_Presales_loadMode.CheckedChanged += new System.EventHandler(this.CheckBoxLoadPresale_CheckedChanged);
            // 
            // checkBox_Presale_workMode
            // 
            this.checkBox_Presale_workMode.AutoSize = true;
            this.checkBox_Presale_workMode.Enabled = false;
            this.checkBox_Presale_workMode.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox_Presale_workMode.Location = new System.Drawing.Point(11, 19);
            this.checkBox_Presale_workMode.Name = "checkBox_Presale_workMode";
            this.checkBox_Presale_workMode.Size = new System.Drawing.Size(291, 18);
            this.checkBox_Presale_workMode.TabIndex = 9;
            this.checkBox_Presale_workMode.Text = "modalità giorno di prevendita (riavvio necessario)";
            this.checkBox_Presale_workMode.UseVisualStyleBackColor = true;
            this.checkBox_Presale_workMode.CheckedChanged += new System.EventHandler(this.CheckBoxPresaleMode_CheckedChanged);
            // 
            // checkBox_Payment
            // 
            this.checkBox_Payment.AutoSize = true;
            this.checkBox_Payment.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox_Payment.Location = new System.Drawing.Point(15, 177);
            this.checkBox_Payment.Name = "checkBox_Payment";
            this.checkBox_Payment.Size = new System.Drawing.Size(326, 18);
            this.checkBox_Payment.TabIndex = 4;
            this.checkBox_Payment.Text = "rendi obbligatorio l\'inserimento del modo di pagamento";
            this.checkBox_Payment.UseVisualStyleBackColor = true;
            // 
            // checkBox_Privacy
            // 
            this.checkBox_Privacy.AutoSize = true;
            this.checkBox_Privacy.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox_Privacy.Location = new System.Drawing.Point(15, 297);
            this.checkBox_Privacy.Name = "checkBox_Privacy";
            this.checkBox_Privacy.Size = new System.Drawing.Size(205, 18);
            this.checkBox_Privacy.TabIndex = 5;
            this.checkBox_Privacy.Text = "modo riservato (nascondi incassi)";
            this.checkBox_Privacy.UseVisualStyleBackColor = true;
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
            this.checkBox_Enter.Location = new System.Drawing.Point(15, 261);
            this.checkBox_Enter.Name = "checkBox_Enter";
            this.checkBox_Enter.Size = new System.Drawing.Size(234, 18);
            this.checkBox_Enter.TabIndex = 6;
            this.checkBox_Enter.Text = "consenti stampa scontrino con ENTER";
            this.checkBox_Enter.UseVisualStyleBackColor = true;
            // 
            // checkBox_ShowPrevReceipt
            // 
            this.checkBox_ShowPrevReceipt.AutoSize = true;
            this.checkBox_ShowPrevReceipt.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox_ShowPrevReceipt.Location = new System.Drawing.Point(15, 369);
            this.checkBox_ShowPrevReceipt.Name = "checkBox_ShowPrevReceipt";
            this.checkBox_ShowPrevReceipt.Size = new System.Drawing.Size(319, 18);
            this.checkBox_ShowPrevReceipt.TabIndex = 25;
            this.checkBox_ShowPrevReceipt.Text = "mostra nello stato il totale dello scontrino precedente";
            this.checkBox_ShowPrevReceipt.UseVisualStyleBackColor = true;
            // 
            // checkBox_ZeroPriceItems
            // 
            this.checkBox_ZeroPriceItems.AutoSize = true;
            this.checkBox_ZeroPriceItems.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox_ZeroPriceItems.Location = new System.Drawing.Point(15, 225);
            this.checkBox_ZeroPriceItems.Name = "checkBox_ZeroPriceItems";
            this.checkBox_ZeroPriceItems.Size = new System.Drawing.Size(202, 18);
            this.checkBox_ZeroPriceItems.TabIndex = 26;
            this.checkBox_ZeroPriceItems.Text = "consenti Articoli con prezzo zero";
            this.checkBox_ZeroPriceItems.UseVisualStyleBackColor = true;
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnReset.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReset.Image = ((System.Drawing.Image)(resources.GetObject("btnReset.Image")));
            this.btnReset.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReset.Location = new System.Drawing.Point(90, 423);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(115, 28);
            this.btnReset.TabIndex = 29;
            this.btnReset.Text = "ripristina default";
            this.btnReset.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnDefault_Click);
            // 
            // panelShowHideButtons
            // 
            this.panelShowHideButtons.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelShowHideButtons.Controls.Add(this.checkBox_ShowDiscount);
            this.panelShowHideButtons.Controls.Add(this.checkBox_ShowSendMsg);
            this.panelShowHideButtons.Controls.Add(this.checkBox_ShowTakeAway);
            this.panelShowHideButtons.Controls.Add(this.checkBox_Show_DB_Check);
            this.panelShowHideButtons.Controls.Add(this.checkBox_Show_OSKeyb);
            this.panelShowHideButtons.Location = new System.Drawing.Point(354, 52);
            this.panelShowHideButtons.Name = "panelShowHideButtons";
            this.panelShowHideButtons.Size = new System.Drawing.Size(312, 212);
            this.panelShowHideButtons.TabIndex = 30;
            // 
            // checkBox_ShowDiscount
            // 
            this.checkBox_ShowDiscount.AutoSize = true;
            this.checkBox_ShowDiscount.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox_ShowDiscount.Location = new System.Drawing.Point(11, 92);
            this.checkBox_ShowDiscount.Name = "checkBox_ShowDiscount";
            this.checkBox_ShowDiscount.Size = new System.Drawing.Size(154, 18);
            this.checkBox_ShowDiscount.TabIndex = 33;
            this.checkBox_ShowDiscount.Text = "mostra pulsante sconto";
            this.checkBox_ShowDiscount.UseVisualStyleBackColor = true;
            // 
            // checkBox_ShowSendMsg
            // 
            this.checkBox_ShowSendMsg.AutoSize = true;
            this.checkBox_ShowSendMsg.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox_ShowSendMsg.Location = new System.Drawing.Point(11, 16);
            this.checkBox_ShowSendMsg.Name = "checkBox_ShowSendMsg";
            this.checkBox_ShowSendMsg.Size = new System.Drawing.Size(249, 18);
            this.checkBox_ShowSendMsg.TabIndex = 32;
            this.checkBox_ShowSendMsg.Text = "mostra pulsante invio messaggio a cucina";
            this.checkBox_ShowSendMsg.UseVisualStyleBackColor = true;
            // 
            // checkBox_ShowTakeAway
            // 
            this.checkBox_ShowTakeAway.AutoSize = true;
            this.checkBox_ShowTakeAway.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox_ShowTakeAway.Location = new System.Drawing.Point(11, 54);
            this.checkBox_ShowTakeAway.Name = "checkBox_ShowTakeAway";
            this.checkBox_ShowTakeAway.Size = new System.Drawing.Size(160, 18);
            this.checkBox_ShowTakeAway.TabIndex = 31;
            this.checkBox_ShowTakeAway.Text = "mostra pulsante Asporto";
            this.checkBox_ShowTakeAway.UseVisualStyleBackColor = true;
            // 
            // checkBox_Show_DB_Check
            // 
            this.checkBox_Show_DB_Check.AutoSize = true;
            this.checkBox_Show_DB_Check.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox_Show_DB_Check.Location = new System.Drawing.Point(11, 130);
            this.checkBox_Show_DB_Check.Name = "checkBox_Show_DB_Check";
            this.checkBox_Show_DB_Check.Size = new System.Drawing.Size(168, 18);
            this.checkBox_Show_DB_Check.TabIndex = 30;
            this.checkBox_Show_DB_Check.Text = "mostra pulsante DB check";
            this.checkBox_Show_DB_Check.UseVisualStyleBackColor = true;
            // 
            // checkBox_Show_OSKeyb
            // 
            this.checkBox_Show_OSKeyb.AutoSize = true;
            this.checkBox_Show_OSKeyb.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox_Show_OSKeyb.Location = new System.Drawing.Point(11, 168);
            this.checkBox_Show_OSKeyb.Name = "checkBox_Show_OSKeyb";
            this.checkBox_Show_OSKeyb.Size = new System.Drawing.Size(218, 18);
            this.checkBox_Show_OSKeyb.TabIndex = 29;
            this.checkBox_Show_OSKeyb.Text = "mostra pulsante Tastiera OnScreen";
            this.checkBox_Show_OSKeyb.UseVisualStyleBackColor = true;
            // 
            // checkBox_Name
            // 
            this.checkBox_Name.AutoSize = true;
            this.checkBox_Name.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox_Name.Location = new System.Drawing.Point(15, 141);
            this.checkBox_Name.Name = "checkBox_Name";
            this.checkBox_Name.Size = new System.Drawing.Size(289, 18);
            this.checkBox_Name.TabIndex = 31;
            this.checkBox_Name.Text = "rendi obbligatorio l\'inserimento del nome utente";
            this.checkBox_Name.UseVisualStyleBackColor = true;
            // 
            // OptionsDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(681, 471);
            this.Controls.Add(this.checkBox_Name);
            this.Controls.Add(this.panelShowHideButtons);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.checkBox_ZeroPriceItems);
            this.Controls.Add(this.checkBox_ShowPrevReceipt);
            this.Controls.Add(this.checkBox_Enter);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboColorTheme);
            this.Controls.Add(this.checkBox_Privacy);
            this.Controls.Add(this.checkBox_Payment);
            this.Controls.Add(this.panelPresale);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.checkBox_InitialAvailabilityDlg);
            this.Controls.Add(this.checkBox_Coperti);
            this.Controls.Add(this.checkBox_Tavolo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsDlg";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Opzioni varie";
            this.panelPresale.ResumeLayout(false);
            this.panelPresale.PerformLayout();
            this.panelShowHideButtons.ResumeLayout(false);
            this.panelShowHideButtons.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBox_Coperti;
        private System.Windows.Forms.CheckBox checkBox_Tavolo;
        private System.Windows.Forms.CheckBox checkBox_InitialAvailabilityDlg;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel panelPresale;
        private System.Windows.Forms.CheckBox checkBox_Presales_loadMode;
        private System.Windows.Forms.CheckBox checkBox_Presale_workMode;
        private System.Windows.Forms.CheckBox checkBox_Payment;
        private System.Windows.Forms.CheckBox checkBox_Privacy;
        private System.Windows.Forms.ComboBox comboColorTheme;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBox_Enter;
        private System.Windows.Forms.CheckBox checkBox_ShowPrevReceipt;
        private System.Windows.Forms.CheckBox checkBox_ZeroPriceItems;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Panel panelShowHideButtons;
        private System.Windows.Forms.CheckBox checkBox_ShowSendMsg;
        private System.Windows.Forms.CheckBox checkBox_ShowTakeAway;
        private System.Windows.Forms.CheckBox checkBox_Show_DB_Check;
        private System.Windows.Forms.CheckBox checkBox_Show_OSKeyb;
        private System.Windows.Forms.CheckBox checkBox_ShowDiscount;
        private System.Windows.Forms.CheckBox checkBox_Name;
    }
}
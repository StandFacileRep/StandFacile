namespace StandFacile
{
    partial class GenPrinterDlg
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
            this.labelEmptyInitial = new System.Windows.Forms.Label();
            this.labelEmptyFinal = new System.Windows.Forms.Label();
            this.numUpDown_RigheIniziali = new System.Windows.Forms.NumericUpDown();
            this.numUpDown_RigheFinali = new System.Windows.Forms.NumericUpDown();
            this.checkBox_CassaInlineNumero = new System.Windows.Forms.CheckBox();
            this.checkBox_CenterTableAndName = new System.Windows.Forms.CheckBox();
            this.checkBox_CopertiNelleCopie = new System.Windows.Forms.CheckBox();
            this.checkBox_LogoNelleCopie = new System.Windows.Forms.CheckBox();
            this.checkBox_Chars33 = new System.Windows.Forms.CheckBox();
            this.checkBox_StarsOnUnderGroup = new System.Windows.Forms.CheckBox();
            this.ckBoxLocalSettings = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown_RigheIniziali)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown_RigheFinali)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Image = global::StandFacile.Properties.Resources.Cancel;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(126, 268);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(81, 28);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "Annulla";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.Image = global::StandFacile.Properties.Resources.OK;
            this.btnOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOK.Location = new System.Drawing.Point(272, 268);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(72, 28);
            this.btnOK.TabIndex = 15;
            this.btnOK.Text = "OK";
            this.btnOK.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // labelEmptyInitial
            // 
            this.labelEmptyInitial.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelEmptyInitial.AutoSize = true;
            this.labelEmptyInitial.Font = new System.Drawing.Font("Tahoma", 9F);
            this.labelEmptyInitial.Location = new System.Drawing.Point(278, 184);
            this.labelEmptyInitial.Name = "labelEmptyInitial";
            this.labelEmptyInitial.Size = new System.Drawing.Size(109, 14);
            this.labelEmptyInitial.TabIndex = 82;
            this.labelEmptyInitial.Text = "Righe vuote iniziali:";
            // 
            // labelEmptyFinal
            // 
            this.labelEmptyFinal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelEmptyFinal.AutoSize = true;
            this.labelEmptyFinal.Font = new System.Drawing.Font("Tahoma", 9F);
            this.labelEmptyFinal.Location = new System.Drawing.Point(278, 230);
            this.labelEmptyFinal.Name = "labelEmptyFinal";
            this.labelEmptyFinal.Size = new System.Drawing.Size(104, 14);
            this.labelEmptyFinal.TabIndex = 83;
            this.labelEmptyFinal.Text = "Righe vuote finali:";
            // 
            // numUpDown_RigheIniziali
            // 
            this.numUpDown_RigheIniziali.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.numUpDown_RigheIniziali.Location = new System.Drawing.Point(401, 182);
            this.numUpDown_RigheIniziali.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numUpDown_RigheIniziali.Name = "numUpDown_RigheIniziali";
            this.numUpDown_RigheIniziali.Size = new System.Drawing.Size(45, 20);
            this.numUpDown_RigheIniziali.TabIndex = 84;
            this.numUpDown_RigheIniziali.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numUpDown_RigheIniziali.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numUpDown_RigheFinali
            // 
            this.numUpDown_RigheFinali.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.numUpDown_RigheFinali.Location = new System.Drawing.Point(401, 228);
            this.numUpDown_RigheFinali.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numUpDown_RigheFinali.Name = "numUpDown_RigheFinali";
            this.numUpDown_RigheFinali.Size = new System.Drawing.Size(45, 20);
            this.numUpDown_RigheFinali.TabIndex = 85;
            this.numUpDown_RigheFinali.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numUpDown_RigheFinali.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // checkBox_CassaInlineNumero
            // 
            this.checkBox_CassaInlineNumero.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBox_CassaInlineNumero.AutoSize = true;
            this.checkBox_CassaInlineNumero.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox_CassaInlineNumero.Location = new System.Drawing.Point(23, 228);
            this.checkBox_CassaInlineNumero.Name = "checkBox_CassaInlineNumero";
            this.checkBox_CassaInlineNumero.Size = new System.Drawing.Size(194, 18);
            this.checkBox_CassaInlineNumero.TabIndex = 87;
            this.checkBox_CassaInlineNumero.Text = "Cassa inline con numero ordine";
            // 
            // checkBox_CenterTableAndName
            // 
            this.checkBox_CenterTableAndName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBox_CenterTableAndName.AutoSize = true;
            this.checkBox_CenterTableAndName.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox_CenterTableAndName.Location = new System.Drawing.Point(264, 84);
            this.checkBox_CenterTableAndName.Name = "checkBox_CenterTableAndName";
            this.checkBox_CenterTableAndName.Size = new System.Drawing.Size(149, 18);
            this.checkBox_CenterTableAndName.TabIndex = 89;
            this.checkBox_CenterTableAndName.Text = "Centra Tavolo e Nome";
            // 
            // checkBox_CopertiNelleCopie
            // 
            this.checkBox_CopertiNelleCopie.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBox_CopertiNelleCopie.AutoSize = true;
            this.checkBox_CopertiNelleCopie.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox_CopertiNelleCopie.Location = new System.Drawing.Point(23, 132);
            this.checkBox_CopertiNelleCopie.Name = "checkBox_CopertiNelleCopie";
            this.checkBox_CopertiNelleCopie.Size = new System.Drawing.Size(205, 18);
            this.checkBox_CopertiNelleCopie.TabIndex = 92;
            this.checkBox_CopertiNelleCopie.Text = "stampa i coperti in tutte le copie";
            this.checkBox_CopertiNelleCopie.UseVisualStyleBackColor = true;
            // 
            // checkBox_LogoNelleCopie
            // 
            this.checkBox_LogoNelleCopie.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBox_LogoNelleCopie.AutoSize = true;
            this.checkBox_LogoNelleCopie.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox_LogoNelleCopie.Location = new System.Drawing.Point(23, 180);
            this.checkBox_LogoNelleCopie.Name = "checkBox_LogoNelleCopie";
            this.checkBox_LogoNelleCopie.Size = new System.Drawing.Size(203, 18);
            this.checkBox_LogoNelleCopie.TabIndex = 91;
            this.checkBox_LogoNelleCopie.Text = "stampa il Logo anche nelle copie";
            this.checkBox_LogoNelleCopie.UseVisualStyleBackColor = true;
            // 
            // checkBox_Chars33
            // 
            this.checkBox_Chars33.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBox_Chars33.AutoSize = true;
            this.checkBox_Chars33.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox_Chars33.Location = new System.Drawing.Point(23, 84);
            this.checkBox_Chars33.Name = "checkBox_Chars33";
            this.checkBox_Chars33.Size = new System.Drawing.Size(223, 18);
            this.checkBox_Chars33.TabIndex = 90;
            this.checkBox_Chars33.Text = "articoli su 23 caratteri (invece di 18)";
            this.checkBox_Chars33.UseVisualStyleBackColor = true;
            // 
            // checkBox_StarsOnUnderGroup
            // 
            this.checkBox_StarsOnUnderGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBox_StarsOnUnderGroup.AutoSize = true;
            this.checkBox_StarsOnUnderGroup.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox_StarsOnUnderGroup.Location = new System.Drawing.Point(264, 134);
            this.checkBox_StarsOnUnderGroup.Name = "checkBox_StarsOnUnderGroup";
            this.checkBox_StarsOnUnderGroup.Size = new System.Drawing.Size(202, 18);
            this.checkBox_StarsOnUnderGroup.TabIndex = 93;
            this.checkBox_StarsOnUnderGroup.Text = "evidenzia con \'#\' il gruppo copie";
            // 
            // ckBoxLocalSettings
            // 
            this.ckBoxLocalSettings.AutoSize = true;
            this.ckBoxLocalSettings.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ckBoxLocalSettings.Location = new System.Drawing.Point(23, 32);
            this.ckBoxLocalSettings.Name = "ckBoxLocalSettings";
            this.ckBoxLocalSettings.Size = new System.Drawing.Size(357, 18);
            this.ckBoxLocalSettings.TabIndex = 94;
            this.ckBoxLocalSettings.Text = "memorizza localmente (o vengono prese tramite il DataBase)";
            this.ckBoxLocalSettings.UseVisualStyleBackColor = true;
            this.ckBoxLocalSettings.CheckedChanged += new System.EventHandler(this.ckBoxLocalSettings_CheckedChanged);
            // 
            // GenPrinterDlg
            // 
            this.AcceptButton = this.btnCancel;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(473, 313);
            this.Controls.Add(this.ckBoxLocalSettings);
            this.Controls.Add(this.checkBox_StarsOnUnderGroup);
            this.Controls.Add(this.checkBox_CopertiNelleCopie);
            this.Controls.Add(this.checkBox_LogoNelleCopie);
            this.Controls.Add(this.checkBox_Chars33);
            this.Controls.Add(this.checkBox_CenterTableAndName);
            this.Controls.Add(this.numUpDown_RigheFinali);
            this.Controls.Add(this.numUpDown_RigheIniziali);
            this.Controls.Add(this.labelEmptyFinal);
            this.Controls.Add(this.labelEmptyInitial);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.checkBox_CassaInlineNumero);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GenPrinterDlg";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Impostazioni Generiche di stampa";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GenPrinterDlg_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown_RigheIniziali)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown_RigheFinali)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label labelEmptyInitial;
        private System.Windows.Forms.Label labelEmptyFinal;
        private System.Windows.Forms.NumericUpDown numUpDown_RigheIniziali;
        private System.Windows.Forms.NumericUpDown numUpDown_RigheFinali;
        private System.Windows.Forms.CheckBox checkBox_CassaInlineNumero;
        private System.Windows.Forms.CheckBox checkBox_CenterTableAndName;
        private System.Windows.Forms.CheckBox checkBox_CopertiNelleCopie;
        private System.Windows.Forms.CheckBox checkBox_LogoNelleCopie;
        private System.Windows.Forms.CheckBox checkBox_Chars33;
        private System.Windows.Forms.CheckBox checkBox_StarsOnUnderGroup;
        private System.Windows.Forms.CheckBox ckBoxLocalSettings;
    }
}
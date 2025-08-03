namespace StandFacile
{
    partial class ConfigIniDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigIniDlg));
            this.textBox_FileConfig = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.BtnCanc = new System.Windows.Forms.Button();
            this.btnAnnulla = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox_FileConfig
            // 
            this.textBox_FileConfig.AcceptsReturn = true;
            this.textBox_FileConfig.AcceptsTab = true;
            this.textBox_FileConfig.BackColor = System.Drawing.Color.Navy;
            this.textBox_FileConfig.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_FileConfig.ForeColor = System.Drawing.SystemColors.Window;
            this.textBox_FileConfig.Location = new System.Drawing.Point(12, 12);
            this.textBox_FileConfig.Multiline = true;
            this.textBox_FileConfig.Name = "textBox_FileConfig";
            this.textBox_FileConfig.ReadOnly = true;
            this.textBox_FileConfig.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_FileConfig.Size = new System.Drawing.Size(520, 325);
            this.textBox_FileConfig.TabIndex = 0;
            this.textBox_FileConfig.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBox_FileConfig_KeyUp);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Image = global::StandFacile.Properties.Resources.OK;
            this.btnOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOK.Location = new System.Drawing.Point(411, 356);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 28);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "Salva";
            this.btnOK.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // BtnCanc
            // 
            this.BtnCanc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BtnCanc.Image = ((System.Drawing.Image)(resources.GetObject("BtnCanc.Image")));
            this.BtnCanc.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnCanc.Location = new System.Drawing.Point(32, 356);
            this.BtnCanc.Name = "BtnCanc";
            this.BtnCanc.Size = new System.Drawing.Size(80, 28);
            this.BtnCanc.TabIndex = 3;
            this.BtnCanc.Text = "Cancella";
            this.BtnCanc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnCanc.UseVisualStyleBackColor = true;
            this.BtnCanc.Click += new System.EventHandler(this.BtnCanc_Click);
            // 
            // btnAnnulla
            // 
            this.btnAnnulla.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAnnulla.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnAnnulla.Image = global::StandFacile.Properties.Resources.Cancel;
            this.btnAnnulla.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAnnulla.Location = new System.Drawing.Point(297, 356);
            this.btnAnnulla.Name = "btnAnnulla";
            this.btnAnnulla.Size = new System.Drawing.Size(80, 28);
            this.btnAnnulla.TabIndex = 2;
            this.btnAnnulla.Text = "Annulla";
            this.btnAnnulla.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAnnulla.UseVisualStyleBackColor = true;
            this.btnAnnulla.Click += new System.EventHandler(this.BtnAnnulla_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEdit.Image = global::StandFacile.Properties.Resources.edit;
            this.btnEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEdit.Location = new System.Drawing.Point(143, 356);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(80, 28);
            this.btnEdit.TabIndex = 4;
            this.btnEdit.Text = "Modifica";
            this.btnEdit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // ConfigIniDlg
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(544, 398);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.BtnCanc);
            this.Controls.Add(this.textBox_FileConfig);
            this.Controls.Add(this.btnAnnulla);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigIniDlg";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "File di configurazione";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAnnulla;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox textBox_FileConfig;
        private System.Windows.Forms.Button BtnCanc;
        private System.Windows.Forms.Button btnEdit;
    }
}
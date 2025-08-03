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
            this.textBox = new System.Windows.Forms.TextBox();
            this.BtnSalva = new System.Windows.Forms.Button();
            this.BtnCanc = new System.Windows.Forms.Button();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.BtnEdit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox
            // 
            this.textBox.AcceptsReturn = true;
            this.textBox.AcceptsTab = true;
            this.textBox.BackColor = System.Drawing.Color.Navy;
            this.textBox.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox.ForeColor = System.Drawing.Color.LightGray;
            this.textBox.Location = new System.Drawing.Point(12, 12);
            this.textBox.Multiline = true;
            this.textBox.Name = "textBox";
            this.textBox.ReadOnly = true;
            this.textBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox.Size = new System.Drawing.Size(520, 325);
            this.textBox.TabIndex = 0;
            this.textBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBox_FileConfig_KeyUp);
            // 
            // BtnSalva
            // 
            this.BtnSalva.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BtnSalva.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.BtnSalva.Image = global::StandFacile.Properties.Resources.OK;
            this.BtnSalva.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnSalva.Location = new System.Drawing.Point(327, 356);
            this.BtnSalva.Name = "BtnSalva";
            this.BtnSalva.Size = new System.Drawing.Size(80, 28);
            this.BtnSalva.TabIndex = 1;
            this.BtnSalva.Text = "Salva";
            this.BtnSalva.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnSalva.UseVisualStyleBackColor = true;
            this.BtnSalva.Click += new System.EventHandler(this.BtnOK_Click);
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
            // BtnCancel
            // 
            this.BtnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnCancel.Image = global::StandFacile.Properties.Resources.Cancel;
            this.BtnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnCancel.Location = new System.Drawing.Point(434, 356);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(80, 28);
            this.BtnCancel.TabIndex = 2;
            this.BtnCancel.Text = "Esci";
            this.BtnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.BtnAnnulla_Click);
            // 
            // BtnEdit
            // 
            this.BtnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BtnEdit.Image = global::StandFacile.Properties.Resources.edit;
            this.BtnEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnEdit.Location = new System.Drawing.Point(143, 356);
            this.BtnEdit.Name = "BtnEdit";
            this.BtnEdit.Size = new System.Drawing.Size(80, 28);
            this.BtnEdit.TabIndex = 4;
            this.BtnEdit.Text = "Modifica";
            this.BtnEdit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnEdit.UseVisualStyleBackColor = true;
            this.BtnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // ConfigIniDlg
            // 
            this.AcceptButton = this.BtnSalva;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(544, 398);
            this.Controls.Add(this.BtnEdit);
            this.Controls.Add(this.BtnCanc);
            this.Controls.Add(this.textBox);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.BtnSalva);
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

        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Button BtnSalva;
        private System.Windows.Forms.TextBox textBox;
        private System.Windows.Forms.Button BtnCanc;
        private System.Windows.Forms.Button BtnEdit;
    }
}
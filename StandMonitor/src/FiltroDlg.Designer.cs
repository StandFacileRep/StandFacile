namespace StandFacile
{
    partial class FiltroDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FiltroDlg));
            this.textBox_Filtro = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.BtnCanc = new System.Windows.Forms.Button();
            this.btnAnnulla = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox_Filtro
            // 
            this.textBox_Filtro.AcceptsReturn = true;
            this.textBox_Filtro.AcceptsTab = true;
            this.textBox_Filtro.BackColor = System.Drawing.Color.Navy;
            this.textBox_Filtro.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_Filtro.ForeColor = System.Drawing.SystemColors.Window;
            this.textBox_Filtro.Location = new System.Drawing.Point(12, 12);
            this.textBox_Filtro.Multiline = true;
            this.textBox_Filtro.Name = "textBox_Filtro";
            this.textBox_Filtro.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_Filtro.Size = new System.Drawing.Size(343, 355);
            this.textBox_Filtro.TabIndex = 0;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Image = global::StandFacile.Properties.Resources.OK;
            this.btnOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOK.Location = new System.Drawing.Point(254, 390);
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
            this.BtnCanc.Image = ((System.Drawing.Image)(resources.GetObject("BtnCanc.Image")));
            this.BtnCanc.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnCanc.Location = new System.Drawing.Point(32, 390);
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
            this.btnAnnulla.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnAnnulla.Image = global::StandFacile.Properties.Resources.Cancel;
            this.btnAnnulla.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAnnulla.Location = new System.Drawing.Point(143, 390);
            this.btnAnnulla.Name = "btnAnnulla";
            this.btnAnnulla.Size = new System.Drawing.Size(80, 28);
            this.btnAnnulla.TabIndex = 2;
            this.btnAnnulla.Text = "Annulla";
            this.btnAnnulla.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAnnulla.UseVisualStyleBackColor = true;
            this.btnAnnulla.Click += new System.EventHandler(this.BtnAnnulla_Click);
            // 
            // FiltroDlg
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(367, 430);
            this.Controls.Add(this.BtnCanc);
            this.Controls.Add(this.textBox_Filtro);
            this.Controls.Add(this.btnAnnulla);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FiltroDlg";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Filtro";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAnnulla;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox textBox_Filtro;
        private System.Windows.Forms.Button BtnCanc;
    }
}
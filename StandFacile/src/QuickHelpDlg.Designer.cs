namespace StandFacile
{
    partial class QuickHelpDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QuickHelpDlg));
            this.textBox = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.ManBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox
            // 
            this.textBox.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox.Location = new System.Drawing.Point(12, 12);
            this.textBox.Multiline = true;
            this.textBox.Name = "textBox";
            this.textBox.ReadOnly = true;
            this.textBox.Size = new System.Drawing.Size(370, 325);
            this.textBox.TabIndex = 0;
            this.textBox.TabStop = false;
            this.textBox.Text = resources.GetString("textBox.Text");
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Image = global::StandFacile.Properties.Resources.OK;
            this.btnOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOK.Location = new System.Drawing.Point(249, 361);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 28);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK  ";
            this.btnOK.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // ManBtn
            // 
            this.ManBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ManBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ManBtn.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ManBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ManBtn.Location = new System.Drawing.Point(58, 359);
            this.ManBtn.Name = "ManBtn";
            this.ManBtn.Size = new System.Drawing.Size(98, 30);
            this.ManBtn.TabIndex = 1;
            this.ManBtn.Text = "manuale (pdf)";
            this.ManBtn.UseVisualStyleBackColor = true;
            this.ManBtn.Click += new System.EventHandler(this.ManBtn_Click);
            // 
            // QuickHelpDlg
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnOK;
            this.ClientSize = new System.Drawing.Size(396, 401);
            this.Controls.Add(this.ManBtn);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.textBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "QuickHelpDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Aiuto Rapido";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.QuickHelpDlg_KeyPress);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button ManBtn;
    }
}
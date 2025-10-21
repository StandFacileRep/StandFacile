namespace StandFacile
{
    partial class PrintConfigLightDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrintConfigLightDlg));
            this.RadioGroup_PrinterType = new System.Windows.Forms.GroupBox();
            this.prt_Legacy = new System.Windows.Forms.RadioButton();
            this.prt_Windows = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.BtnGeneric = new System.Windows.Forms.Button();
            this.BtnLegacy = new System.Windows.Forms.Button();
            this.BtnWin = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.RadioGroup_PrinterType.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // RadioGroup_PrinterType
            // 
            this.RadioGroup_PrinterType.Controls.Add(this.prt_Legacy);
            this.RadioGroup_PrinterType.Controls.Add(this.prt_Windows);
            this.RadioGroup_PrinterType.Font = new System.Drawing.Font("Tahoma", 9F);
            this.RadioGroup_PrinterType.Location = new System.Drawing.Point(23, 144);
            this.RadioGroup_PrinterType.Name = "RadioGroup_PrinterType";
            this.RadioGroup_PrinterType.Size = new System.Drawing.Size(280, 99);
            this.RadioGroup_PrinterType.TabIndex = 0;
            this.RadioGroup_PrinterType.TabStop = false;
            this.RadioGroup_PrinterType.Text = "tipo di Stampante dello Scontrino";
            // 
            // prt_Legacy
            // 
            this.prt_Legacy.AutoSize = true;
            this.prt_Legacy.Location = new System.Drawing.Point(173, 50);
            this.prt_Legacy.Name = "prt_Legacy";
            this.prt_Legacy.Size = new System.Drawing.Size(69, 18);
            this.prt_Legacy.TabIndex = 1;
            this.prt_Legacy.TabStop = true;
            this.prt_Legacy.Text = "LEGACY";
            this.prt_Legacy.UseVisualStyleBackColor = true;
            // 
            // prt_Windows
            // 
            this.prt_Windows.AutoSize = true;
            this.prt_Windows.Location = new System.Drawing.Point(23, 50);
            this.prt_Windows.Name = "prt_Windows";
            this.prt_Windows.Size = new System.Drawing.Size(85, 18);
            this.prt_Windows.TabIndex = 0;
            this.prt_Windows.TabStop = true;
            this.prt_Windows.Text = "WINDOWS";
            this.prt_Windows.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.BtnGeneric);
            this.groupBox1.Controls.Add(this.BtnLegacy);
            this.groupBox1.Controls.Add(this.BtnWin);
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(23, 22);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(280, 103);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Impostazione stampanti Windows e Legacy";
            // 
            // BtnGeneric
            // 
            this.BtnGeneric.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.BtnGeneric.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.BtnGeneric.Image = global::StandFacile.Properties.Resources.globe;
            this.BtnGeneric.Location = new System.Drawing.Point(116, 34);
            this.BtnGeneric.Name = "BtnGeneric";
            this.BtnGeneric.Size = new System.Drawing.Size(42, 40);
            this.BtnGeneric.TabIndex = 2;
            this.BtnGeneric.UseVisualStyleBackColor = false;
            this.BtnGeneric.Click += new System.EventHandler(this.BtnGeneric_Click);
            // 
            // BtnLegacy
            // 
            this.BtnLegacy.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.BtnLegacy.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.BtnLegacy.Image = ((System.Drawing.Image)(resources.GetObject("BtnLegacy.Image")));
            this.BtnLegacy.Location = new System.Drawing.Point(194, 29);
            this.BtnLegacy.Name = "BtnLegacy";
            this.BtnLegacy.Size = new System.Drawing.Size(50, 50);
            this.BtnLegacy.TabIndex = 1;
            this.BtnLegacy.UseVisualStyleBackColor = false;
            this.BtnLegacy.Click += new System.EventHandler(this.BtnLegacy_Click);
            // 
            // BtnWin
            // 
            this.BtnWin.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.BtnWin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.BtnWin.Image = ((System.Drawing.Image)(resources.GetObject("BtnWin.Image")));
            this.BtnWin.Location = new System.Drawing.Point(32, 29);
            this.BtnWin.Name = "BtnWin";
            this.BtnWin.Size = new System.Drawing.Size(50, 50);
            this.BtnWin.TabIndex = 0;
            this.BtnWin.UseVisualStyleBackColor = false;
            this.BtnWin.Click += new System.EventHandler(this.BtnWin_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(51, 278);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 28);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Image = global::StandFacile.Properties.Resources.OK;
            this.btnOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOK.Location = new System.Drawing.Point(187, 278);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 28);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK  ";
            this.btnOK.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // PrintConfigLightDlg
            // 
            this.AcceptButton = this.btnCancel;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(327, 329);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.RadioGroup_PrinterType);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PrintConfigLightDlg";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Configurazione Stampanti";
            this.RadioGroup_PrinterType.ResumeLayout(false);
            this.RadioGroup_PrinterType.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox RadioGroup_PrinterType;
        private System.Windows.Forms.RadioButton prt_Legacy;
        private System.Windows.Forms.RadioButton prt_Windows;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button BtnWin;
        private System.Windows.Forms.Button BtnLegacy;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button BtnGeneric;
    }
}
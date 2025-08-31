namespace StandFacile
{
    partial class SelDataDlg
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
            this.OKBtn = new System.Windows.Forms.Button();
            this.textBox = new System.Windows.Forms.TextBox();
            this.mCalendar = new System.Windows.Forms.MonthCalendar();
            this.SuspendLayout();
            // 
            // OKBtn
            // 
            this.OKBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.OKBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OKBtn.Image = global::StandFacile.Properties.Resources.OK;
            this.OKBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.OKBtn.Location = new System.Drawing.Point(173, 286);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(78, 28);
            this.OKBtn.TabIndex = 0;
            this.OKBtn.Text = "OK ";
            this.OKBtn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            this.OKBtn.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SelDataDlg_KeyPress);
            // 
            // textBox
            // 
            this.textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox.Location = new System.Drawing.Point(15, 12);
            this.textBox.Multiline = true;
            this.textBox.Name = "textBox";
            this.textBox.ReadOnly = true;
            this.textBox.Size = new System.Drawing.Size(385, 65);
            this.textBox.TabIndex = 1;
            this.textBox.Text = "Selezionare la data di interesse: è anche possibile selezionare un intervallo di " +
    "giorni cliccando sull\'inizio desiderato, e muovendosi con le frecce tenendo prem" +
    "uto il tasto Shift";
            // 
            // mCalendar
            // 
            this.mCalendar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.mCalendar.Location = new System.Drawing.Point(98, 103);
            this.mCalendar.MaxSelectionCount = 60;
            this.mCalendar.MinDate = new System.DateTime(2025, 1, 1, 0, 0, 0, 0);
            this.mCalendar.Name = "mCalendar";
            this.mCalendar.TabIndex = 3;
            this.mCalendar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MCalendar_MouseDown);
            // 
            // SelDataDlg
            // 
            this.AcceptButton = this.OKBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(423, 333);
            this.Controls.Add(this.mCalendar);
            this.Controls.Add(this.textBox);
            this.Controls.Add(this.OKBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelDataDlg";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Selezione Data";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SelDataDlg_KeyPress);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.TextBox textBox;
        private System.Windows.Forms.MonthCalendar mCalendar;
    }
}
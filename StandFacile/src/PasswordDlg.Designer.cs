namespace StandFacile
{
    partial class PasswordDlg
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PasswordDlg));
            this.OKBtn = new System.Windows.Forms.Button();
            this.textBox_PWD = new System.Windows.Forms.TextBox();
            this.textBox_VER = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.labelPWD = new System.Windows.Forms.Label();
            this.labelRIP = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // OKBtn
            // 
            this.OKBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OKBtn.Image = ((System.Drawing.Image)(resources.GetObject("OKBtn.Image")));
            this.OKBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.OKBtn.Location = new System.Drawing.Point(184, 143);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(80, 28);
            this.OKBtn.TabIndex = 5;
            this.OKBtn.Text = "OK  ";
            this.OKBtn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // textBox_PWD
            // 
            this.textBox_PWD.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_PWD.Location = new System.Drawing.Point(44, 31);
            this.textBox_PWD.MaxLength = 20;
            this.textBox_PWD.Name = "textBox_PWD";
            this.textBox_PWD.PasswordChar = '*';
            this.textBox_PWD.Size = new System.Drawing.Size(220, 24);
            this.textBox_PWD.TabIndex = 6;
            // 
            // textBox_VER
            // 
            this.textBox_VER.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_VER.Location = new System.Drawing.Point(44, 91);
            this.textBox_VER.MaxLength = 20;
            this.textBox_VER.Name = "textBox_VER";
            this.textBox_VER.PasswordChar = '*';
            this.textBox_VER.Size = new System.Drawing.Size(220, 24);
            this.textBox_VER.TabIndex = 7;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Image = global::StandFacile.Properties.Resources.Cancel;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(44, 143);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 28);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Annulla";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Interval = 200;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // labelPWD
            // 
            this.labelPWD.AutoSize = true;
            this.labelPWD.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPWD.Location = new System.Drawing.Point(44, 13);
            this.labelPWD.Name = "labelPWD";
            this.labelPWD.Size = new System.Drawing.Size(205, 14);
            this.labelPWD.TabIndex = 9;
            this.labelPWD.Text = "inserire la password (min 6 caratteri)";
            // 
            // labelRIP
            // 
            this.labelRIP.AutoSize = true;
            this.labelRIP.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRIP.Location = new System.Drawing.Point(44, 74);
            this.labelRIP.Name = "labelRIP";
            this.labelRIP.Size = new System.Drawing.Size(54, 14);
            this.labelRIP.TabIndex = 10;
            this.labelRIP.Text = "ripetere:";
            // 
            // PasswordDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(303, 189);
            this.Controls.Add(this.labelRIP);
            this.Controls.Add(this.labelPWD);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.textBox_VER);
            this.Controls.Add(this.textBox_PWD);
            this.Controls.Add(this.OKBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PasswordDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Imposta password";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.TextBox textBox_PWD;
        private System.Windows.Forms.TextBox textBox_VER;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Label labelPWD;
        private System.Windows.Forms.Label labelRIP;
    }
}
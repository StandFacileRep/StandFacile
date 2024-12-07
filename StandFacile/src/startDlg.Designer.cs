namespace StandFacile
{
    partial class startDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(startDlg));
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.OraLbl = new System.Windows.Forms.Label();
            this.DateLbl = new System.Windows.Forms.Label();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.AuthorLbl = new System.Windows.Forms.Label();
            this.LblTitle = new System.Windows.Forms.Label();
            this.LinkLbl_Web = new System.Windows.Forms.LinkLabel();
            this.OkBtn = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Lbl_Listino = new System.Windows.Forms.Label();
            this.LblPrinter = new System.Windows.Forms.Label();
            this.Lbl_DB = new System.Windows.Forms.Label();
            this.textBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer
            // 
            this.timer.Interval = 1000;
            this.timer.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // OraLbl
            // 
            this.OraLbl.AutoSize = true;
            this.OraLbl.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.OraLbl.Location = new System.Drawing.Point(261, 328);
            this.OraLbl.Name = "OraLbl";
            this.OraLbl.Size = new System.Drawing.Size(45, 16);
            this.OraLbl.TabIndex = 7;
            this.OraLbl.Text = "Ora   :";
            // 
            // DateLbl
            // 
            this.DateLbl.AutoSize = true;
            this.DateLbl.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.DateLbl.Location = new System.Drawing.Point(55, 328);
            this.DateLbl.Name = "DateLbl";
            this.DateLbl.Size = new System.Drawing.Size(42, 16);
            this.DateLbl.TabIndex = 6;
            this.DateLbl.Text = "Data :";
            // 
            // pictureBox
            // 
            this.pictureBox.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox.Image")));
            this.pictureBox.Location = new System.Drawing.Point(98, 49);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(44, 40);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox.TabIndex = 14;
            this.pictureBox.TabStop = false;
            // 
            // AuthorLbl
            // 
            this.AuthorLbl.AutoSize = true;
            this.AuthorLbl.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AuthorLbl.Location = new System.Drawing.Point(170, 49);
            this.AuthorLbl.Name = "AuthorLbl";
            this.AuthorLbl.Size = new System.Drawing.Size(135, 16);
            this.AuthorLbl.TabIndex = 2;
            this.AuthorLbl.Text = "Autore : Mauro Artuso";
            // 
            // LblTitle
            // 
            this.LblTitle.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblTitle.Location = new System.Drawing.Point(13, 9);
            this.LblTitle.Name = "LblTitle";
            this.LblTitle.Size = new System.Drawing.Size(390, 23);
            this.LblTitle.TabIndex = 1;
            this.LblTitle.Text = "Stand Facile  ...";
            this.LblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LinkLbl_Web
            // 
            this.LinkLbl_Web.AutoSize = true;
            this.LinkLbl_Web.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LinkLbl_Web.Location = new System.Drawing.Point(170, 73);
            this.LinkLbl_Web.Name = "LinkLbl_Web";
            this.LinkLbl_Web.Size = new System.Drawing.Size(72, 16);
            this.LinkLbl_Web.TabIndex = 3;
            this.LinkLbl_Web.TabStop = true;
            this.LinkLbl_Web.Text = "www....org";
            this.LinkLbl_Web.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLbl_Web_LinkClicked);
            // 
            // OkBtn
            // 
            this.OkBtn.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.OkBtn.Image = global::StandFacile.Properties.Resources.OK;
            this.OkBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.OkBtn.Location = new System.Drawing.Point(173, 366);
            this.OkBtn.Name = "OkBtn";
            this.OkBtn.Size = new System.Drawing.Size(80, 28);
            this.OkBtn.TabIndex = 0;
            this.OkBtn.Text = "OK  ";
            this.OkBtn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.OkBtn.UseVisualStyleBackColor = true;
            this.OkBtn.Click += new System.EventHandler(this.OkBtn_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.Lbl_Listino);
            this.panel1.Controls.Add(this.LblPrinter);
            this.panel1.Controls.Add(this.Lbl_DB);
            this.panel1.Location = new System.Drawing.Point(13, 106);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(390, 119);
            this.panel1.TabIndex = 4;
            // 
            // Lbl_Listino
            // 
            this.Lbl_Listino.AutoSize = true;
            this.Lbl_Listino.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Lbl_Listino.Location = new System.Drawing.Point(3, 12);
            this.Lbl_Listino.Name = "Lbl_Listino";
            this.Lbl_Listino.Size = new System.Drawing.Size(81, 16);
            this.Lbl_Listino.TabIndex = 2;
            this.Lbl_Listino.Text = "data Listino :";
            // 
            // LblPrinter
            // 
            this.LblPrinter.AutoSize = true;
            this.LblPrinter.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblPrinter.Location = new System.Drawing.Point(3, 80);
            this.LblPrinter.Name = "LblPrinter";
            this.LblPrinter.Size = new System.Drawing.Size(76, 16);
            this.LblPrinter.TabIndex = 1;
            this.LblPrinter.Text = "stampante :";
            // 
            // Lbl_DB
            // 
            this.Lbl_DB.AutoSize = true;
            this.Lbl_DB.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Lbl_DB.Location = new System.Drawing.Point(3, 45);
            this.Lbl_DB.Name = "Lbl_DB";
            this.Lbl_DB.Size = new System.Drawing.Size(68, 16);
            this.Lbl_DB.TabIndex = 0;
            this.Lbl_DB.Text = "database :";
            // 
            // textBox
            // 
            this.textBox.BackColor = System.Drawing.SystemColors.Control;
            this.textBox.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox.Location = new System.Drawing.Point(13, 239);
            this.textBox.Margin = new System.Windows.Forms.Padding(8);
            this.textBox.Multiline = true;
            this.textBox.Name = "textBox";
            this.textBox.ReadOnly = true;
            this.textBox.Size = new System.Drawing.Size(390, 70);
            this.textBox.TabIndex = 15;
            this.textBox.Text = " Avviare per prima la Cassa Principale e poi le Secondarie,  \r\n \r\n verificare che" +
    " la data e l\'ora del PC siano corrette !";
            // 
            // startDlg
            // 
            this.AcceptButton = this.OkBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(417, 408);
            this.Controls.Add(this.textBox);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.OkBtn);
            this.Controls.Add(this.LinkLbl_Web);
            this.Controls.Add(this.OraLbl);
            this.Controls.Add(this.DateLbl);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.AuthorLbl);
            this.Controls.Add(this.LblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "startDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Avvio ...";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label OraLbl;
        private System.Windows.Forms.Label DateLbl;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Label AuthorLbl;
        private System.Windows.Forms.Label LblTitle;
        private System.Windows.Forms.LinkLabel LinkLbl_Web;
        private System.Windows.Forms.Button OkBtn;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label LblPrinter;
        private System.Windows.Forms.Label Lbl_DB;
        private System.Windows.Forms.Label Lbl_Listino;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.TextBox textBox;
    }
}
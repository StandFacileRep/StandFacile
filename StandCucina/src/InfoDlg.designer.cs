namespace StandFacile
{
    partial class InfoDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InfoDlg));
            this.textBox = new System.Windows.Forms.TextBox();
            this.lblTitolo = new System.Windows.Forms.Label();
            this.lblAuthor = new System.Windows.Forms.Label();
            this.lblRel = new System.Windows.Forms.Label();
            this.LinkLbl_Web = new System.Windows.Forms.LinkLabel();
            this.LinkLbl_mail = new System.Windows.Forms.LinkLabel();
            this.lblDB = new System.Windows.Forms.Label();
            this.LblPrinter = new System.Windows.Forms.Label();
            this.Lbl_DB = new System.Windows.Forms.Label();
            this.ImageLic = new System.Windows.Forms.PictureBox();
            this.ProgramIcon = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.ImageLic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProgramIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox
            // 
            this.textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox.Location = new System.Drawing.Point(14, 146);
            this.textBox.Multiline = true;
            this.textBox.Name = "textBox";
            this.textBox.Size = new System.Drawing.Size(574, 261);
            this.textBox.TabIndex = 0;
            this.textBox.TabStop = false;
            this.textBox.Text = resources.GetString("textBox.Text");
            // 
            // lblTitolo
            // 
            this.lblTitolo.AutoSize = true;
            this.lblTitolo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitolo.Location = new System.Drawing.Point(94, 12);
            this.lblTitolo.Name = "lblTitolo";
            this.lblTitolo.Size = new System.Drawing.Size(88, 16);
            this.lblTitolo.TabIndex = 1;
            this.lblTitolo.Text = "StandFacile...";
            // 
            // lblAuthor
            // 
            this.lblAuthor.AutoSize = true;
            this.lblAuthor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAuthor.Location = new System.Drawing.Point(94, 35);
            this.lblAuthor.Name = "lblAuthor";
            this.lblAuthor.Size = new System.Drawing.Size(131, 16);
            this.lblAuthor.TabIndex = 2;
            this.lblAuthor.Text = "Autore: Mauro Artuso";
            // 
            // lblRel
            // 
            this.lblRel.AutoSize = true;
            this.lblRel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRel.Location = new System.Drawing.Point(226, 12);
            this.lblRel.Name = "lblRel";
            this.lblRel.Size = new System.Drawing.Size(68, 16);
            this.lblRel.TabIndex = 3;
            this.lblRel.Text = "Release...";
            // 
            // LinkLbl_Web
            // 
            this.LinkLbl_Web.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LinkLbl_Web.AutoSize = true;
            this.LinkLbl_Web.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LinkLbl_Web.Location = new System.Drawing.Point(298, 451);
            this.LinkLbl_Web.Name = "LinkLbl_Web";
            this.LinkLbl_Web.Size = new System.Drawing.Size(130, 17);
            this.LinkLbl_Web.TabIndex = 7;
            this.LinkLbl_Web.TabStop = true;
            this.LinkLbl_Web.Text = "www.standfacile.org";
            this.LinkLbl_Web.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLbl_Web_LinkClicked);
            // 
            // LinkLbl_mail
            // 
            this.LinkLbl_mail.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LinkLbl_mail.AutoSize = true;
            this.LinkLbl_mail.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LinkLbl_mail.Location = new System.Drawing.Point(298, 425);
            this.LinkLbl_mail.Name = "LinkLbl_mail";
            this.LinkLbl_mail.Size = new System.Drawing.Size(118, 17);
            this.LinkLbl_mail.TabIndex = 10;
            this.LinkLbl_mail.TabStop = true;
            this.LinkLbl_mail.Text = "info@standfacile...";
            this.LinkLbl_mail.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLbl_mail_LinkClicked);
            // 
            // lblDB
            // 
            this.lblDB.AutoSize = true;
            this.lblDB.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDB.Location = new System.Drawing.Point(94, 93);
            this.lblDB.Name = "lblDB";
            this.lblDB.Size = new System.Drawing.Size(89, 16);
            this.lblDB.TabIndex = 13;
            this.lblDB.Text = "DB dll version:";
            // 
            // LblPrinter
            // 
            this.LblPrinter.AutoSize = true;
            this.LblPrinter.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.LblPrinter.Location = new System.Drawing.Point(94, 117);
            this.LblPrinter.Name = "LblPrinter";
            this.LblPrinter.Size = new System.Drawing.Size(78, 16);
            this.LblPrinter.TabIndex = 12;
            this.LblPrinter.Text = "Stampante :";
            // 
            // Lbl_DB
            // 
            this.Lbl_DB.AutoSize = true;
            this.Lbl_DB.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Lbl_DB.Location = new System.Drawing.Point(94, 69);
            this.Lbl_DB.Name = "Lbl_DB";
            this.Lbl_DB.Size = new System.Drawing.Size(67, 16);
            this.Lbl_DB.TabIndex = 11;
            this.Lbl_DB.Text = "DB server:";
            // 
            // ImageLic
            // 
            this.ImageLic.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ImageLic.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ImageLic.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ImageLic.Image = global::StandFacile.Properties.Resources.gplv3_84x42;
            this.ImageLic.Location = new System.Drawing.Point(141, 422);
            this.ImageLic.Margin = new System.Windows.Forms.Padding(0);
            this.ImageLic.Name = "ImageLic";
            this.ImageLic.Size = new System.Drawing.Size(86, 44);
            this.ImageLic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.ImageLic.TabIndex = 6;
            this.ImageLic.TabStop = false;
            this.ImageLic.Click += new System.EventHandler(this.ImageLic_Click);
            // 
            // ProgramIcon
            // 
            this.ProgramIcon.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ProgramIcon.Image = global::StandFacile.Properties.Resources.Stand_C;
            this.ProgramIcon.Location = new System.Drawing.Point(24, 12);
            this.ProgramIcon.Margin = new System.Windows.Forms.Padding(0);
            this.ProgramIcon.Name = "ProgramIcon";
            this.ProgramIcon.Size = new System.Drawing.Size(32, 32);
            this.ProgramIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.ProgramIcon.TabIndex = 4;
            this.ProgramIcon.TabStop = false;
            // 
            // InfoDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 474);
            this.Controls.Add(this.lblDB);
            this.Controls.Add(this.LblPrinter);
            this.Controls.Add(this.Lbl_DB);
            this.Controls.Add(this.LinkLbl_mail);
            this.Controls.Add(this.LinkLbl_Web);
            this.Controls.Add(this.ImageLic);
            this.Controls.Add(this.ProgramIcon);
            this.Controls.Add(this.lblRel);
            this.Controls.Add(this.lblAuthor);
            this.Controls.Add(this.lblTitolo);
            this.Controls.Add(this.textBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InfoDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Informazioni";
            ((System.ComponentModel.ISupportInitialize)(this.ImageLic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProgramIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox;
        private System.Windows.Forms.Label lblTitolo;
        private System.Windows.Forms.Label lblAuthor;
        private System.Windows.Forms.Label lblRel;
        private System.Windows.Forms.PictureBox ProgramIcon;
        private System.Windows.Forms.PictureBox ImageLic;
        private System.Windows.Forms.LinkLabel LinkLbl_Web;
        private System.Windows.Forms.LinkLabel LinkLbl_mail;
        private System.Windows.Forms.Label lblDB;
        private System.Windows.Forms.Label LblPrinter;
        private System.Windows.Forms.Label Lbl_DB;
    }
}
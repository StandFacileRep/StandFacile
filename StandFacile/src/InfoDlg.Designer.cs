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
            this.lblSW_Rel = new System.Windows.Forms.Label();
            this.ImageLic = new System.Windows.Forms.PictureBox();
            this.ProgramIcon = new System.Windows.Forms.PictureBox();
            this.LinkLbl_Web = new System.Windows.Forms.LinkLabel();
            this.Lbl_DB = new System.Windows.Forms.Label();
            this.lbl_Printer = new System.Windows.Forms.Label();
            this.lbl_Ver = new System.Windows.Forms.Label();
            this.LinkLbl_mail = new System.Windows.Forms.LinkLabel();
            this.ImageDona = new System.Windows.Forms.PictureBox();
            this.lblDB_Rel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ImageLic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProgramIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImageDona)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox
            // 
            this.textBox.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox.Location = new System.Drawing.Point(14, 153);
            this.textBox.Multiline = true;
            this.textBox.Name = "textBox";
            this.textBox.Size = new System.Drawing.Size(543, 264);
            this.textBox.TabIndex = 7;
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
            this.lblTitolo.TabIndex = 2;
            this.lblTitolo.Text = "StandFacile...";
            // 
            // lblAuthor
            // 
            this.lblAuthor.AutoSize = true;
            this.lblAuthor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAuthor.Location = new System.Drawing.Point(94, 35);
            this.lblAuthor.Name = "lblAuthor";
            this.lblAuthor.Size = new System.Drawing.Size(131, 16);
            this.lblAuthor.TabIndex = 4;
            this.lblAuthor.Text = "Autore: Mauro Artuso";
            // 
            // lblSW_Rel
            // 
            this.lblSW_Rel.AutoSize = true;
            this.lblSW_Rel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSW_Rel.Location = new System.Drawing.Point(226, 12);
            this.lblSW_Rel.Name = "lblSW_Rel";
            this.lblSW_Rel.Size = new System.Drawing.Size(68, 16);
            this.lblSW_Rel.TabIndex = 3;
            this.lblSW_Rel.Text = "Release...";
            // 
            // ImageLic
            // 
            this.ImageLic.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ImageLic.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ImageLic.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ImageLic.ErrorImage = global::StandFacile.Properties.Resources.paypal_dona;
            this.ImageLic.Image = global::StandFacile.Properties.Resources.gplv3_84x42;
            this.ImageLic.Location = new System.Drawing.Point(52, 433);
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
            this.ProgramIcon.Image = global::StandFacile.Properties.Resources.Spaghetti;
            this.ProgramIcon.Location = new System.Drawing.Point(24, 12);
            this.ProgramIcon.Margin = new System.Windows.Forms.Padding(0);
            this.ProgramIcon.Name = "ProgramIcon";
            this.ProgramIcon.Size = new System.Drawing.Size(32, 29);
            this.ProgramIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.ProgramIcon.TabIndex = 4;
            this.ProgramIcon.TabStop = false;
            // 
            // LinkLbl_Web
            // 
            this.LinkLbl_Web.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LinkLbl_Web.AutoSize = true;
            this.LinkLbl_Web.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LinkLbl_Web.Location = new System.Drawing.Point(319, 464);
            this.LinkLbl_Web.Name = "LinkLbl_Web";
            this.LinkLbl_Web.Size = new System.Drawing.Size(117, 17);
            this.LinkLbl_Web.TabIndex = 1;
            this.LinkLbl_Web.TabStop = true;
            this.LinkLbl_Web.Text = "www.standfacile...";
            this.LinkLbl_Web.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLbl_Web_LinkClicked);
            // 
            // Lbl_DB
            // 
            this.Lbl_DB.AutoSize = true;
            this.Lbl_DB.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Lbl_DB.Location = new System.Drawing.Point(94, 71);
            this.Lbl_DB.Name = "Lbl_DB";
            this.Lbl_DB.Size = new System.Drawing.Size(67, 16);
            this.Lbl_DB.TabIndex = 5;
            this.Lbl_DB.Text = "DB server:";
            // 
            // lbl_Printer
            // 
            this.lbl_Printer.AutoSize = true;
            this.lbl_Printer.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.lbl_Printer.Location = new System.Drawing.Point(94, 123);
            this.lbl_Printer.Name = "lbl_Printer";
            this.lbl_Printer.Size = new System.Drawing.Size(78, 16);
            this.lbl_Printer.TabIndex = 6;
            this.lbl_Printer.Text = "Stampante :";
            // 
            // lbl_Ver
            // 
            this.lbl_Ver.AutoSize = true;
            this.lbl_Ver.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Ver.Location = new System.Drawing.Point(94, 98);
            this.lbl_Ver.Name = "lbl_Ver";
            this.lbl_Ver.Size = new System.Drawing.Size(89, 16);
            this.lbl_Ver.TabIndex = 8;
            this.lbl_Ver.Text = "DB dll version:";
            // 
            // LinkLbl_mail
            // 
            this.LinkLbl_mail.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LinkLbl_mail.AutoSize = true;
            this.LinkLbl_mail.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LinkLbl_mail.Location = new System.Drawing.Point(319, 439);
            this.LinkLbl_mail.Name = "LinkLbl_mail";
            this.LinkLbl_mail.Size = new System.Drawing.Size(118, 17);
            this.LinkLbl_mail.TabIndex = 9;
            this.LinkLbl_mail.TabStop = true;
            this.LinkLbl_mail.Text = "info@standfacile...";
            this.LinkLbl_mail.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLbl_mail_LinkClicked);
            // 
            // ImageDona
            // 
            this.ImageDona.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ImageDona.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ImageDona.Image = global::StandFacile.Properties.Resources.paypal_dona;
            this.ImageDona.Location = new System.Drawing.Point(171, 433);
            this.ImageDona.Margin = new System.Windows.Forms.Padding(0);
            this.ImageDona.Name = "ImageDona";
            this.ImageDona.Size = new System.Drawing.Size(136, 44);
            this.ImageDona.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.ImageDona.TabIndex = 10;
            this.ImageDona.TabStop = false;
            this.ImageDona.Click += new System.EventHandler(this.ImageDona_Click);
            // 
            // lblDB_Rel
            // 
            this.lblDB_Rel.AutoSize = true;
            this.lblDB_Rel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDB_Rel.Location = new System.Drawing.Point(343, 12);
            this.lblDB_Rel.Name = "lblDB_Rel";
            this.lblDB_Rel.Size = new System.Drawing.Size(72, 16);
            this.lblDB_Rel.TabIndex = 12;
            this.lblDB_Rel.Text = "db tables...";
            // 
            // InfoDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(569, 491);
            this.Controls.Add(this.lblDB_Rel);
            this.Controls.Add(this.ImageDona);
            this.Controls.Add(this.LinkLbl_mail);
            this.Controls.Add(this.lbl_Ver);
            this.Controls.Add(this.lbl_Printer);
            this.Controls.Add(this.Lbl_DB);
            this.Controls.Add(this.LinkLbl_Web);
            this.Controls.Add(this.ImageLic);
            this.Controls.Add(this.ProgramIcon);
            this.Controls.Add(this.lblSW_Rel);
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
            ((System.ComponentModel.ISupportInitialize)(this.ImageDona)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox;
        private System.Windows.Forms.Label lblTitolo;
        private System.Windows.Forms.Label lblAuthor;
        private System.Windows.Forms.Label lblSW_Rel;
        private System.Windows.Forms.PictureBox ProgramIcon;
        private System.Windows.Forms.PictureBox ImageLic;
        private System.Windows.Forms.LinkLabel LinkLbl_Web;
        private System.Windows.Forms.Label Lbl_DB;
        private System.Windows.Forms.Label lbl_Printer;
        private System.Windows.Forms.Label lbl_Ver;
        private System.Windows.Forms.LinkLabel LinkLbl_mail;
        private System.Windows.Forms.PictureBox ImageDona;
        private System.Windows.Forms.Label lblDB_Rel;
    }
}
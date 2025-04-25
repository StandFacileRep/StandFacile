namespace StandFacile
{
    partial class VisOrdiniDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VisOrdiniDlg));
            this.textEdit_Ticket = new System.Windows.Forms.TextBox();
            this.VisTicketStatusBar = new System.Windows.Forms.StatusStrip();
            this.toolStripNumTicket = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripTotaleOrdine = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripCassa = new System.Windows.Forms.ToolStripStatusLabel();
            this.CkBoxTutteCasse = new System.Windows.Forms.CheckBox();
            this.OKBtn = new System.Windows.Forms.Button();
            this.AnnulloBtn = new System.Windows.Forms.Button();
            this.BtnPrev = new System.Windows.Forms.Button();
            this.BtnNext = new System.Windows.Forms.Button();
            this.BtnPrt = new System.Windows.Forms.Button();
            this.labelPrint = new System.Windows.Forms.Label();
            this.comboPaymentType = new System.Windows.Forms.ComboBox();
            this.labelPayMethod = new System.Windows.Forms.Label();
            this.checkBoxNotPaid = new System.Windows.Forms.CheckBox();
            this.lbl_Info = new System.Windows.Forms.Label();
            this.VisTicketStatusBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // textEdit_Ticket
            // 
            this.textEdit_Ticket.BackColor = System.Drawing.Color.Teal;
            this.textEdit_Ticket.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textEdit_Ticket.ForeColor = System.Drawing.SystemColors.Window;
            this.textEdit_Ticket.Location = new System.Drawing.Point(12, 12);
            this.textEdit_Ticket.Multiline = true;
            this.textEdit_Ticket.Name = "textEdit_Ticket";
            this.textEdit_Ticket.ReadOnly = true;
            this.textEdit_Ticket.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textEdit_Ticket.Size = new System.Drawing.Size(376, 409);
            this.textEdit_Ticket.TabIndex = 1;
            // 
            // VisTicketStatusBar
            // 
            this.VisTicketStatusBar.AutoSize = false;
            this.VisTicketStatusBar.Dock = System.Windows.Forms.DockStyle.None;
            this.VisTicketStatusBar.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.VisTicketStatusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripNumTicket,
            this.toolStripTotaleOrdine,
            this.toolStripCassa});
            this.VisTicketStatusBar.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.VisTicketStatusBar.Location = new System.Drawing.Point(13, 424);
            this.VisTicketStatusBar.Name = "VisTicketStatusBar";
            this.VisTicketStatusBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.VisTicketStatusBar.Size = new System.Drawing.Size(375, 22);
            this.VisTicketStatusBar.SizingGrip = false;
            this.VisTicketStatusBar.Stretch = false;
            this.VisTicketStatusBar.TabIndex = 8;
            // 
            // toolStripNumTicket
            // 
            this.toolStripNumTicket.AutoSize = false;
            this.toolStripNumTicket.Name = "toolStripNumTicket";
            this.toolStripNumTicket.Size = new System.Drawing.Size(120, 17);
            this.toolStripNumTicket.Text = "Scontrino : 100";
            this.toolStripNumTicket.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripTotaleOrdine
            // 
            this.toolStripTotaleOrdine.AutoSize = false;
            this.toolStripTotaleOrdine.Name = "toolStripTotaleOrdine";
            this.toolStripTotaleOrdine.Size = new System.Drawing.Size(80, 17);
            this.toolStripTotaleOrdine.Text = "Totali : 100";
            this.toolStripTotaleOrdine.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripCassa
            // 
            this.toolStripCassa.AutoSize = false;
            this.toolStripCassa.Name = "toolStripCassa";
            this.toolStripCassa.Size = new System.Drawing.Size(140, 17);
            this.toolStripCassa.Text = "Cassa n.1  Principale";
            this.toolStripCassa.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CkBoxTutteCasse
            // 
            this.CkBoxTutteCasse.AutoSize = true;
            this.CkBoxTutteCasse.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CkBoxTutteCasse.Location = new System.Drawing.Point(29, 486);
            this.CkBoxTutteCasse.Name = "CkBoxTutteCasse";
            this.CkBoxTutteCasse.Size = new System.Drawing.Size(129, 18);
            this.CkBoxTutteCasse.TabIndex = 2;
            this.CkBoxTutteCasse.Text = "Vedi tutte le casse";
            this.CkBoxTutteCasse.UseVisualStyleBackColor = true;
            this.CkBoxTutteCasse.Click += new System.EventHandler(this.CkBoxTutteCasse_Click);
            // 
            // OKBtn
            // 
            this.OKBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OKBtn.Image = global::StandFacile.Properties.Resources.OK;
            this.OKBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.OKBtn.Location = new System.Drawing.Point(308, 546);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(70, 35);
            this.OKBtn.TabIndex = 3;
            this.OKBtn.Text = "OK  ";
            this.OKBtn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.OKBtn.UseVisualStyleBackColor = true;
            // 
            // AnnulloBtn
            // 
            this.AnnulloBtn.Image = ((System.Drawing.Image)(resources.GetObject("AnnulloBtn.Image")));
            this.AnnulloBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.AnnulloBtn.Location = new System.Drawing.Point(29, 546);
            this.AnnulloBtn.Name = "AnnulloBtn";
            this.AnnulloBtn.Size = new System.Drawing.Size(113, 35);
            this.AnnulloBtn.TabIndex = 4;
            this.AnnulloBtn.Text = " annullo Ordine";
            this.AnnulloBtn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.AnnulloBtn.UseVisualStyleBackColor = true;
            this.AnnulloBtn.Click += new System.EventHandler(this.AnnulloBtn_Click);
            // 
            // BtnPrev
            // 
            this.BtnPrev.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.BtnPrev.Image = global::StandFacile.Properties.Resources.ArrowSLeft;
            this.BtnPrev.Location = new System.Drawing.Point(211, 477);
            this.BtnPrev.Name = "BtnPrev";
            this.BtnPrev.Size = new System.Drawing.Size(70, 35);
            this.BtnPrev.TabIndex = 5;
            this.BtnPrev.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.BtnPrev.UseVisualStyleBackColor = true;
            this.BtnPrev.Click += new System.EventHandler(this.PrevBtn_Click);
            // 
            // BtnNext
            // 
            this.BtnNext.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.BtnNext.Image = global::StandFacile.Properties.Resources.ArrowSRight;
            this.BtnNext.Location = new System.Drawing.Point(308, 477);
            this.BtnNext.Name = "BtnNext";
            this.BtnNext.Size = new System.Drawing.Size(70, 35);
            this.BtnNext.TabIndex = 6;
            this.BtnNext.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.BtnNext.UseVisualStyleBackColor = true;
            this.BtnNext.Click += new System.EventHandler(this.NextBtn_Click);
            // 
            // BtnPrt
            // 
            this.BtnPrt.Image = global::StandFacile.Properties.Resources.printer_s;
            this.BtnPrt.Location = new System.Drawing.Point(211, 546);
            this.BtnPrt.Name = "BtnPrt";
            this.BtnPrt.Size = new System.Drawing.Size(70, 35);
            this.BtnPrt.TabIndex = 7;
            this.BtnPrt.UseVisualStyleBackColor = true;
            this.BtnPrt.Click += new System.EventHandler(this.BtnPrt_Click);
            // 
            // labelPrint
            // 
            this.labelPrint.AutoSize = true;
            this.labelPrint.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPrint.Location = new System.Drawing.Point(209, 526);
            this.labelPrint.Name = "labelPrint";
            this.labelPrint.Size = new System.Drawing.Size(110, 13);
            this.labelPrint.TabIndex = 9;
            this.labelPrint.Text = "stampa tutto in locale";
            // 
            // comboPaymentType
            // 
            this.comboPaymentType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboPaymentType.FormattingEnabled = true;
            this.comboPaymentType.ItemHeight = 13;
            this.comboPaymentType.Items.AddRange(new object[] {
            "CONTANTI",
            "CARD",
            "SATYSPAY",
            "   "});
            this.comboPaymentType.Location = new System.Drawing.Point(29, 518);
            this.comboPaymentType.Name = "comboPaymentType";
            this.comboPaymentType.Size = new System.Drawing.Size(113, 21);
            this.comboPaymentType.TabIndex = 25;
            this.comboPaymentType.SelectedIndexChanged += new System.EventHandler(this.ComboCashPos_SelectedIndexChanged);
            // 
            // labelPayMethod
            // 
            this.labelPayMethod.AutoSize = true;
            this.labelPayMethod.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPayMethod.Location = new System.Drawing.Point(28, 500);
            this.labelPayMethod.Name = "labelPayMethod";
            this.labelPayMethod.Size = new System.Drawing.Size(147, 13);
            this.labelPayMethod.TabIndex = 26;
            this.labelPayMethod.Text = "cambia metodo di pagamento";
            // 
            // checkBoxNotPaid
            // 
            this.checkBoxNotPaid.AutoSize = true;
            this.checkBoxNotPaid.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxNotPaid.Location = new System.Drawing.Point(29, 458);
            this.checkBoxNotPaid.Name = "checkBoxNotPaid";
            this.checkBoxNotPaid.Size = new System.Drawing.Size(154, 18);
            this.checkBoxNotPaid.TabIndex = 27;
            this.checkBoxNotPaid.Text = "filtra pag. da effettuare";
            this.checkBoxNotPaid.UseVisualStyleBackColor = true;
            // 
            // lbl_Info
            // 
            this.lbl_Info.AutoSize = true;
            this.lbl_Info.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_Info.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Info.Location = new System.Drawing.Point(50, 575);
            this.lbl_Info.Name = "lbl_Info";
            this.lbl_Info.Padding = new System.Windows.Forms.Padding(2);
            this.lbl_Info.Size = new System.Drawing.Size(79, 19);
            this.lbl_Info.TabIndex = 28;
            this.lbl_Info.Text = "  info colori ?  ";
            this.lbl_Info.Click += new System.EventHandler(this.Lbl_Info_Click);
            // 
            // VisOrdiniDlg
            // 
            this.AcceptButton = this.OKBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 594);
            this.Controls.Add(this.lbl_Info);
            this.Controls.Add(this.checkBoxNotPaid);
            this.Controls.Add(this.labelPayMethod);
            this.Controls.Add(this.comboPaymentType);
            this.Controls.Add(this.labelPrint);
            this.Controls.Add(this.AnnulloBtn);
            this.Controls.Add(this.CkBoxTutteCasse);
            this.Controls.Add(this.VisTicketStatusBar);
            this.Controls.Add(this.BtnPrev);
            this.Controls.Add(this.BtnNext);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.BtnPrt);
            this.Controls.Add(this.textEdit_Ticket);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VisOrdiniDlg";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Visualizza Ordini";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VisOrdiniDlg_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.VisTicketsDlg_KeyDown);
            this.VisTicketStatusBar.ResumeLayout(false);
            this.VisTicketStatusBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textEdit_Ticket;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button BtnPrt;
        private System.Windows.Forms.Button BtnNext;
        private System.Windows.Forms.Button BtnPrev;
        private System.Windows.Forms.StatusStrip VisTicketStatusBar;
        private System.Windows.Forms.ToolStripStatusLabel toolStripNumTicket;
        private System.Windows.Forms.ToolStripStatusLabel toolStripTotaleOrdine;
        private System.Windows.Forms.ToolStripStatusLabel toolStripCassa;
        private System.Windows.Forms.CheckBox CkBoxTutteCasse;
        private System.Windows.Forms.Button AnnulloBtn;
        private System.Windows.Forms.Label labelPrint;
        private System.Windows.Forms.ComboBox comboPaymentType;
        private System.Windows.Forms.Label labelPayMethod;
        private System.Windows.Forms.CheckBox checkBoxNotPaid;
        private System.Windows.Forms.Label lbl_Info;
    }
}
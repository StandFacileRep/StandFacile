namespace StandFacile
{
    partial class VisMessaggiDlg
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
            this.textEdit_Messaggi = new System.Windows.Forms.TextBox();
            this.BtnPrev = new System.Windows.Forms.Button();
            this.BtnNext = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.VisTicketStatusBar = new System.Windows.Forms.StatusStrip();
            this.toolStripNumMessaggio = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripTotaleMessaggi = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripCassa = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblRemChar = new System.Windows.Forms.Label();
            this.ckBoxTutteCasse = new System.Windows.Forms.CheckBox();
            this.BtnPrintMsg = new System.Windows.Forms.Button();
            this.PrintersListCombo = new System.Windows.Forms.ComboBox();
            this.VisTicketStatusBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // textEdit_Messaggi
            // 
            this.textEdit_Messaggi.AcceptsReturn = true;
            this.textEdit_Messaggi.AcceptsTab = true;
            this.textEdit_Messaggi.BackColor = System.Drawing.Color.Teal;
            this.textEdit_Messaggi.Font = new System.Drawing.Font("Courier New", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textEdit_Messaggi.ForeColor = System.Drawing.SystemColors.Window;
            this.textEdit_Messaggi.Location = new System.Drawing.Point(12, 47);
            this.textEdit_Messaggi.Multiline = true;
            this.textEdit_Messaggi.Name = "textEdit_Messaggi";
            this.textEdit_Messaggi.ReadOnly = true;
            this.textEdit_Messaggi.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textEdit_Messaggi.Size = new System.Drawing.Size(368, 269);
            this.textEdit_Messaggi.TabIndex = 3;
            this.textEdit_Messaggi.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textEdit_Messaggi_KeyUp);
            // 
            // BtnPrev
            // 
            this.BtnPrev.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.BtnPrev.Image = global::StandFacile.Properties.Resources.ArrowSLeft;
            this.BtnPrev.Location = new System.Drawing.Point(187, 364);
            this.BtnPrev.Name = "BtnPrev";
            this.BtnPrev.Size = new System.Drawing.Size(70, 30);
            this.BtnPrev.TabIndex = 4;
            this.BtnPrev.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.BtnPrev.UseVisualStyleBackColor = true;
            this.BtnPrev.Click += new System.EventHandler(this.prevBtn_Click);
            // 
            // BtnNext
            // 
            this.BtnNext.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.BtnNext.Image = global::StandFacile.Properties.Resources.ArrowSRight;
            this.BtnNext.Location = new System.Drawing.Point(295, 364);
            this.BtnNext.Name = "BtnNext";
            this.BtnNext.Size = new System.Drawing.Size(70, 30);
            this.BtnNext.TabIndex = 5;
            this.BtnNext.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.BtnNext.UseVisualStyleBackColor = true;
            this.BtnNext.Click += new System.EventHandler(this.nextBtn_Click);
            // 
            // btnSend
            // 
            this.btnSend.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSend.Image = global::StandFacile.Properties.Resources.OK;
            this.btnSend.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSend.Location = new System.Drawing.Point(295, 475);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(70, 30);
            this.btnSend.TabIndex = 0;
            this.btnSend.Text = "Invia";
            this.btnSend.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // VisTicketStatusBar
            // 
            this.VisTicketStatusBar.AutoSize = false;
            this.VisTicketStatusBar.Dock = System.Windows.Forms.DockStyle.None;
            this.VisTicketStatusBar.Font = new System.Drawing.Font("Segoe UI Symbol", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VisTicketStatusBar.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.VisTicketStatusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripNumMessaggio,
            this.toolStripTotaleMessaggi,
            this.toolStripCassa});
            this.VisTicketStatusBar.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.VisTicketStatusBar.Location = new System.Drawing.Point(12, 329);
            this.VisTicketStatusBar.Name = "VisTicketStatusBar";
            this.VisTicketStatusBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.VisTicketStatusBar.Size = new System.Drawing.Size(368, 22);
            this.VisTicketStatusBar.SizingGrip = false;
            this.VisTicketStatusBar.Stretch = false;
            this.VisTicketStatusBar.TabIndex = 6;
            // 
            // toolStripNumMessaggio
            // 
            this.toolStripNumMessaggio.AutoSize = false;
            this.toolStripNumMessaggio.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripNumMessaggio.Name = "toolStripNumMessaggio";
            this.toolStripNumMessaggio.Size = new System.Drawing.Size(120, 17);
            this.toolStripNumMessaggio.Text = "Scontrino : 100";
            this.toolStripNumMessaggio.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripTotaleMessaggi
            // 
            this.toolStripTotaleMessaggi.AutoSize = false;
            this.toolStripTotaleMessaggi.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripTotaleMessaggi.Name = "toolStripTotaleMessaggi";
            this.toolStripTotaleMessaggi.Size = new System.Drawing.Size(80, 17);
            this.toolStripTotaleMessaggi.Text = "Totali : 100";
            this.toolStripTotaleMessaggi.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripCassa
            // 
            this.toolStripCassa.AutoSize = false;
            this.toolStripCassa.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripCassa.Name = "toolStripCassa";
            this.toolStripCassa.Size = new System.Drawing.Size(140, 17);
            this.toolStripCassa.Text = "Cassa n.1  Principale";
            this.toolStripCassa.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnExit
            // 
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.Image = global::StandFacile.Properties.Resources.Cancel;
            this.btnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExit.Location = new System.Drawing.Point(30, 475);
            this.btnExit.Margin = new System.Windows.Forms.Padding(4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(70, 30);
            this.btnExit.TabIndex = 1;
            this.btnExit.Text = "Esci";
            this.btnExit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExit.UseVisualStyleBackColor = true;
            // 
            // lblRemChar
            // 
            this.lblRemChar.AutoSize = true;
            this.lblRemChar.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRemChar.Location = new System.Drawing.Point(17, 21);
            this.lblRemChar.Name = "lblRemChar";
            this.lblRemChar.Size = new System.Drawing.Size(115, 14);
            this.lblRemChar.TabIndex = 2;
            this.lblRemChar.Text = "Caratteri rimanenti :";
            // 
            // ckBoxTutteCasse
            // 
            this.ckBoxTutteCasse.AutoSize = true;
            this.ckBoxTutteCasse.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ckBoxTutteCasse.Location = new System.Drawing.Point(30, 371);
            this.ckBoxTutteCasse.Name = "ckBoxTutteCasse";
            this.ckBoxTutteCasse.Size = new System.Drawing.Size(129, 18);
            this.ckBoxTutteCasse.TabIndex = 7;
            this.ckBoxTutteCasse.Text = "Vedi tutte le casse";
            this.ckBoxTutteCasse.UseVisualStyleBackColor = true;
            // 
            // BtnPrintMsg
            // 
            this.BtnPrintMsg.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.BtnPrintMsg.Image = global::StandFacile.Properties.Resources.printer_s;
            this.BtnPrintMsg.Location = new System.Drawing.Point(187, 461);
            this.BtnPrintMsg.Name = "BtnPrintMsg";
            this.BtnPrintMsg.Size = new System.Drawing.Size(54, 44);
            this.BtnPrintMsg.TabIndex = 20;
            this.BtnPrintMsg.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.BtnPrintMsg.UseVisualStyleBackColor = true;
            this.BtnPrintMsg.Click += new System.EventHandler(this.BtnPrintMsg_Click);
            // 
            // PrintersListCombo
            // 
            this.PrintersListCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PrintersListCombo.FormattingEnabled = true;
            this.PrintersListCombo.Location = new System.Drawing.Point(30, 422);
            this.PrintersListCombo.Name = "PrintersListCombo";
            this.PrintersListCombo.Size = new System.Drawing.Size(211, 21);
            this.PrintersListCombo.TabIndex = 21;
            // 
            // VisMessaggiDlg
            // 
            this.AcceptButton = this.btnExit;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(395, 517);
            this.Controls.Add(this.PrintersListCombo);
            this.Controls.Add(this.BtnPrintMsg);
            this.Controls.Add(this.ckBoxTutteCasse);
            this.Controls.Add(this.lblRemChar);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.VisTicketStatusBar);
            this.Controls.Add(this.BtnPrev);
            this.Controls.Add(this.BtnNext);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.textEdit_Messaggi);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VisMessaggiDlg";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Messaggio inviato mediante stampa o StandCucina";
            this.Shown += new System.EventHandler(this.VisMessaggiDlg_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.VisTicketsDlg_KeyDown);
            this.VisTicketStatusBar.ResumeLayout(false);
            this.VisTicketStatusBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textEdit_Messaggi;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button BtnNext;
        private System.Windows.Forms.Button BtnPrev;
        private System.Windows.Forms.StatusStrip VisTicketStatusBar;
        private System.Windows.Forms.ToolStripStatusLabel toolStripNumMessaggio;
        private System.Windows.Forms.ToolStripStatusLabel toolStripTotaleMessaggi;
        private System.Windows.Forms.ToolStripStatusLabel toolStripCassa;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblRemChar;
        private System.Windows.Forms.CheckBox ckBoxTutteCasse;
        private System.Windows.Forms.Button BtnPrintMsg;
        private System.Windows.Forms.ComboBox PrintersListCombo;
    }
}
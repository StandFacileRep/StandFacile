namespace StandFacile
{
    partial class EditDispArticoliDlg
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
            this.lb1 = new System.Windows.Forms.Label();
            this.TipoEdit = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.AzzeraBtn = new System.Windows.Forms.Button();
            this.RipristinaBtn = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.DispEdit = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.lbl2 = new System.Windows.Forms.Label();
            this.lblDescGruppo = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lb1
            // 
            this.lb1.AutoSize = true;
            this.lb1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb1.Location = new System.Drawing.Point(16, 19);
            this.lb1.Name = "lb1";
            this.lb1.Size = new System.Drawing.Size(105, 14);
            this.lb1.TabIndex = 4;
            this.lb1.Text = "Nome dell\'Articolo";
            // 
            // TipoEdit
            // 
            this.TipoEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TipoEdit.Location = new System.Drawing.Point(135, 15);
            this.TipoEdit.MaxLength = 18;
            this.TipoEdit.Name = "TipoEdit";
            this.TipoEdit.ReadOnly = true;
            this.TipoEdit.Size = new System.Drawing.Size(200, 22);
            this.TipoEdit.TabIndex = 0;
            this.TipoEdit.TabStop = false;
            this.TipoEdit.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EditDispArticoliDlg_KeyDown);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.AzzeraBtn);
            this.groupBox1.Controls.Add(this.RipristinaBtn);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.DispEdit);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.groupBox1.Location = new System.Drawing.Point(19, 95);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(316, 123);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            // 
            // AzzeraBtn
            // 
            this.AzzeraBtn.Image = global::StandFacile.Properties.Resources.Cancel;
            this.AzzeraBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.AzzeraBtn.Location = new System.Drawing.Point(18, 78);
            this.AzzeraBtn.Name = "AzzeraBtn";
            this.AzzeraBtn.Size = new System.Drawing.Size(110, 32);
            this.AzzeraBtn.TabIndex = 1;
            this.AzzeraBtn.TabStop = false;
            this.AzzeraBtn.Text = "Azzera Disp.";
            this.AzzeraBtn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.AzzeraBtn.UseVisualStyleBackColor = true;
            this.AzzeraBtn.Click += new System.EventHandler(this.BtnAzzera_Click);
            this.AzzeraBtn.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EditDispArticoliDlg_KeyDown);
            // 
            // RipristinaBtn
            // 
            this.RipristinaBtn.Image = global::StandFacile.Properties.Resources.edit;
            this.RipristinaBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.RipristinaBtn.Location = new System.Drawing.Point(154, 78);
            this.RipristinaBtn.Name = "RipristinaBtn";
            this.RipristinaBtn.Size = new System.Drawing.Size(143, 32);
            this.RipristinaBtn.TabIndex = 2;
            this.RipristinaBtn.TabStop = false;
            this.RipristinaBtn.Text = "  Ripristina max Disp.";
            this.RipristinaBtn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.RipristinaBtn.UseVisualStyleBackColor = true;
            this.RipristinaBtn.Click += new System.EventHandler(this.BtnRipristina_Click);
            this.RipristinaBtn.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EditDispArticoliDlg_KeyDown);
            // 
            // labelMarginRep
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(161, 26);
            this.label5.Name = "labelMarginRep";
            this.label5.Size = new System.Drawing.Size(110, 14);
            this.label5.TabIndex = 4;
            this.label5.Text = "OK = nessun limite";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(15, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 14);
            this.label4.TabIndex = 3;
            this.label4.Text = "Disponibilità";
            // 
            // DispEdit
            // 
            this.DispEdit.Location = new System.Drawing.Point(95, 24);
            this.DispEdit.MaxLength = 6;
            this.DispEdit.Name = "DispEdit";
            this.DispEdit.Size = new System.Drawing.Size(47, 20);
            this.DispEdit.TabIndex = 0;
            this.DispEdit.Text = "OK";
            this.DispEdit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.DispEdit.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EditDispArticoliDlg_KeyDown);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Image = global::StandFacile.Properties.Resources.Cancel;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(81, 239);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 28);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EditDispArticoliDlg_KeyDown);
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Image = global::StandFacile.Properties.Resources.OK;
            this.btnOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOK.Location = new System.Drawing.Point(206, 239);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 28);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK  ";
            this.btnOK.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
            this.btnOK.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EditDispArticoliDlg_KeyDown);
            // 
            // lbl2
            // 
            this.lbl2.AutoSize = true;
            this.lbl2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl2.Location = new System.Drawing.Point(16, 59);
            this.lbl2.Name = "lbl2";
            this.lbl2.Size = new System.Drawing.Size(91, 14);
            this.lbl2.TabIndex = 5;
            this.lbl2.Text = "Gruppo Articoli:";
            // 
            // lblDescGruppo
            // 
            this.lblDescGruppo.AutoSize = true;
            this.lblDescGruppo.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescGruppo.Location = new System.Drawing.Point(109, 59);
            this.lblDescGruppo.Name = "lblDescGruppo";
            this.lblDescGruppo.Size = new System.Drawing.Size(63, 14);
            this.lblDescGruppo.TabIndex = 6;
            this.lblDescGruppo.Text = "PIETANZE";
            // 
            // EditDispArticoliDlg
            // 
            this.AcceptButton = this.btnCancel;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(354, 285);
            this.ControlBox = false;
            this.Controls.Add(this.lblDescGruppo);
            this.Controls.Add(this.lbl2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.TipoEdit);
            this.Controls.Add(this.lb1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "EditDispArticoliDlg";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Modifica Disponibilità";
            this.Shown += new System.EventHandler(this.DispDlg_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EditDispArticoliDlg_KeyDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lb1;
        private System.Windows.Forms.TextBox TipoEdit;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox DispEdit;
        private System.Windows.Forms.Button AzzeraBtn;
        private System.Windows.Forms.Button RipristinaBtn;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lbl2;
        private System.Windows.Forms.Label lblDescGruppo;
    }
}
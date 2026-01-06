namespace StandFacile
{
    partial class DataCheckDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataCheckDlg));
            this.lblTavolo = new System.Windows.Forms.Label();
            this.EditTavolo = new System.Windows.Forms.TextBox();
            this.lblCoperti = new System.Windows.Forms.Label();
            this.EditCoperti = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.radioSatispayBtn = new System.Windows.Forms.RadioButton();
            this.radioCardBtn = new System.Windows.Forms.RadioButton();
            this.radioContantiBtn = new System.Windows.Forms.RadioButton();
            this.BtnPrt = new System.Windows.Forms.Button();
            this.groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTavolo
            // 
            this.lblTavolo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTavolo.AutoSize = true;
            this.lblTavolo.Font = new System.Drawing.Font("Tahoma", 11.25F);
            this.lblTavolo.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.lblTavolo.Location = new System.Drawing.Point(55, 42);
            this.lblTavolo.Margin = new System.Windows.Forms.Padding(3, 0, 3, 4);
            this.lblTavolo.Name = "lblTavolo";
            this.lblTavolo.Size = new System.Drawing.Size(57, 18);
            this.lblTavolo.TabIndex = 20;
            this.lblTavolo.Text = "Tavolo:";
            // 
            // EditTable
            // 
            this.EditTavolo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.EditTavolo.BackColor = System.Drawing.Color.LightBlue;
            this.EditTavolo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EditTavolo.Location = new System.Drawing.Point(151, 35);
            this.EditTavolo.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.EditTavolo.MaxLength = 12;
            this.EditTavolo.Name = "EditTable";
            this.EditTavolo.Size = new System.Drawing.Size(100, 26);
            this.EditTavolo.TabIndex = 0;
            this.EditTavolo.WordWrap = false;
            this.EditTavolo.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TextBox_KeyUp);
            // 
            // lblCoperti
            // 
            this.lblCoperti.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCoperti.AutoSize = true;
            this.lblCoperti.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCoperti.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.lblCoperti.Location = new System.Drawing.Point(52, 91);
            this.lblCoperti.Margin = new System.Windows.Forms.Padding(3, 0, 3, 4);
            this.lblCoperti.Name = "lblCoperti";
            this.lblCoperti.Size = new System.Drawing.Size(58, 18);
            this.lblCoperti.TabIndex = 22;
            this.lblCoperti.Text = "Coperti:";
            // 
            // EditCoperti
            // 
            this.EditCoperti.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.EditCoperti.BackColor = System.Drawing.Color.LightBlue;
            this.EditCoperti.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EditCoperti.Location = new System.Drawing.Point(151, 83);
            this.EditCoperti.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.EditCoperti.MaxLength = 3;
            this.EditCoperti.Name = "EditCoperti";
            this.EditCoperti.Size = new System.Drawing.Size(48, 26);
            this.EditCoperti.TabIndex = 1;
            this.EditCoperti.WordWrap = false;
            this.EditCoperti.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.EditFilter_KeyPress);
            this.EditCoperti.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TextBox_KeyUp);
            // 
            // btnOK
            // 
            this.btnOK.Image = ((System.Drawing.Image)(resources.GetObject("btnOK.Image")));
            this.btnOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOK.Location = new System.Drawing.Point(249, 258);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 35);
            this.btnOK.TabIndex = 9;
            this.btnOK.Text = "OK   ";
            this.btnOK.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(30, 258);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 35);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Annulla";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.radioSatispayBtn);
            this.groupBox.Controls.Add(this.radioCardBtn);
            this.groupBox.Controls.Add(this.radioContantiBtn);
            this.groupBox.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.groupBox.Location = new System.Drawing.Point(30, 143);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(299, 70);
            this.groupBox.TabIndex = 3;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "modo di pagamento";
            // 
            // radioSatispayBtn
            // 
            this.radioSatispayBtn.AutoSize = true;
            this.radioSatispayBtn.Location = new System.Drawing.Point(210, 30);
            this.radioSatispayBtn.Name = "radioSatispayBtn";
            this.radioSatispayBtn.Size = new System.Drawing.Size(73, 20);
            this.radioSatispayBtn.TabIndex = 6;
            this.radioSatispayBtn.TabStop = true;
            this.radioSatispayBtn.Text = "Satispay";
            this.radioSatispayBtn.UseVisualStyleBackColor = true;
            this.radioSatispayBtn.Click += new System.EventHandler(this.RadioBtn_Click);
            // 
            // radioCardBtn
            // 
            this.radioCardBtn.AutoSize = true;
            this.radioCardBtn.Location = new System.Drawing.Point(126, 30);
            this.radioCardBtn.Name = "radioCardBtn";
            this.radioCardBtn.Size = new System.Drawing.Size(52, 20);
            this.radioCardBtn.TabIndex = 5;
            this.radioCardBtn.TabStop = true;
            this.radioCardBtn.Text = "Card";
            this.radioCardBtn.UseVisualStyleBackColor = true;
            this.radioCardBtn.Click += new System.EventHandler(this.RadioBtn_Click);
            // 
            // radioContantiBtn
            // 
            this.radioContantiBtn.AutoSize = true;
            this.radioContantiBtn.Location = new System.Drawing.Point(25, 30);
            this.radioContantiBtn.Name = "radioContantiBtn";
            this.radioContantiBtn.Size = new System.Drawing.Size(72, 20);
            this.radioContantiBtn.TabIndex = 4;
            this.radioContantiBtn.TabStop = true;
            this.radioContantiBtn.Text = "Contanti";
            this.radioContantiBtn.UseVisualStyleBackColor = true;
            this.radioContantiBtn.Click += new System.EventHandler(this.RadioBtn_Click);
            // 
            // BtnPrt
            // 
            this.BtnPrt.Image = global::StandFacile.Properties.Resources.printer_s;
            this.BtnPrt.Location = new System.Drawing.Point(151, 257);
            this.BtnPrt.Name = "BtnPrt";
            this.BtnPrt.Size = new System.Drawing.Size(70, 35);
            this.BtnPrt.TabIndex = 8;
            this.BtnPrt.UseVisualStyleBackColor = true;
            this.BtnPrt.Click += new System.EventHandler(this.BtnPrt_Click);
            // 
            // DataCheckDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(363, 309);
            this.Controls.Add(this.BtnPrt);
            this.Controls.Add(this.groupBox);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.EditCoperti);
            this.Controls.Add(this.lblCoperti);
            this.Controls.Add(this.EditTavolo);
            this.Controls.Add(this.lblTavolo);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DataCheckDlg";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "modulo di verifica dei campi obbligatori";
            this.groupBox.ResumeLayout(false);
            this.groupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTavolo;
        private System.Windows.Forms.TextBox EditTavolo;
        private System.Windows.Forms.Label lblCoperti;
        private System.Windows.Forms.TextBox EditCoperti;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox;
        private System.Windows.Forms.RadioButton radioSatispayBtn;
        private System.Windows.Forms.RadioButton radioCardBtn;
        private System.Windows.Forms.RadioButton radioContantiBtn;
        private System.Windows.Forms.Button BtnPrt;
    }
}
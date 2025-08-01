namespace StandFacile
{
    partial class ScontoDlg
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
            this.BT_Cancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.DS_grpBxDiscount = new System.Windows.Forms.GroupBox();
            this.DS_rdBtnDiscountNone = new System.Windows.Forms.RadioButton();
            this.DS_rdBtnDiscountGratis = new System.Windows.Forms.RadioButton();
            this.DS_rdBtnDiscountFixed = new System.Windows.Forms.RadioButton();
            this.DS_rdBtnDiscountStd = new System.Windows.Forms.RadioButton();
            this.textBoxMotivazione = new System.Windows.Forms.TextBox();
            this.DS_lblDiscountTxt = new System.Windows.Forms.Label();
            this.textBoxFixVal = new System.Windows.Forms.TextBox();
            this.DS_lblDiscountVal = new System.Windows.Forms.Label();
            this.ckBox_0 = new System.Windows.Forms.CheckBox();
            this.ckBox_1 = new System.Windows.Forms.CheckBox();
            this.ckBox_2 = new System.Windows.Forms.CheckBox();
            this.ckBox_3 = new System.Windows.Forms.CheckBox();
            this.ckBox_4 = new System.Windows.Forms.CheckBox();
            this.ckBox_5 = new System.Windows.Forms.CheckBox();
            this.textBoxPercVal = new System.Windows.Forms.TextBox();
            this.DS_lblDiscountPerc = new System.Windows.Forms.Label();
            this.DS_btnSave = new System.Windows.Forms.Button();
            this.ckBox_6 = new System.Windows.Forms.CheckBox();
            this.ckBox_7 = new System.Windows.Forms.CheckBox();
            this.DS_lblDiscountWarn = new System.Windows.Forms.Label();
            this.radioBtn100 = new System.Windows.Forms.RadioButton();
            this.ckBox_9 = new System.Windows.Forms.CheckBox();
            this.ckBox_8 = new System.Windows.Forms.CheckBox();
            this.DS_grpBxDiscount.SuspendLayout();
            this.SuspendLayout();
            // 
            // BT_Cancel
            // 
            this.BT_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BT_Cancel.Image = global::StandFacile.Properties.Resources.Cancel;
            this.BT_Cancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BT_Cancel.Location = new System.Drawing.Point(296, 269);
            this.BT_Cancel.Name = "BT_Cancel";
            this.BT_Cancel.Size = new System.Drawing.Size(80, 33);
            this.BT_Cancel.TabIndex = 10;
            this.BT_Cancel.Text = "Cancel";
            this.BT_Cancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BT_Cancel.UseVisualStyleBackColor = true;
            this.BT_Cancel.Click += new System.EventHandler(this.BT_Cancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Image = global::StandFacile.Properties.Resources.OK;
            this.btnOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOK.Location = new System.Drawing.Point(420, 269);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 33);
            this.btnOK.TabIndex = 11;
            this.btnOK.Text = "OK  ";
            this.btnOK.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // DS_grpBxDiscount
            // 
            this.DS_grpBxDiscount.Controls.Add(this.DS_rdBtnDiscountNone);
            this.DS_grpBxDiscount.Controls.Add(this.DS_rdBtnDiscountGratis);
            this.DS_grpBxDiscount.Controls.Add(this.DS_rdBtnDiscountFixed);
            this.DS_grpBxDiscount.Controls.Add(this.DS_rdBtnDiscountStd);
            this.DS_grpBxDiscount.Font = new System.Drawing.Font("Tahoma", 9F);
            this.DS_grpBxDiscount.Location = new System.Drawing.Point(12, 12);
            this.DS_grpBxDiscount.Name = "DS_grpBxDiscount";
            this.DS_grpBxDiscount.Size = new System.Drawing.Size(228, 220);
            this.DS_grpBxDiscount.TabIndex = 8;
            this.DS_grpBxDiscount.TabStop = false;
            this.DS_grpBxDiscount.Text = "tipo di sconto da applicare";
            // 
            // DS_rdBtnDiscountNone
            // 
            this.DS_rdBtnDiscountNone.AutoSize = true;
            this.DS_rdBtnDiscountNone.Checked = true;
            this.DS_rdBtnDiscountNone.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DS_rdBtnDiscountNone.Location = new System.Drawing.Point(26, 32);
            this.DS_rdBtnDiscountNone.Name = "DS_rdBtnDiscountNone";
            this.DS_rdBtnDiscountNone.Size = new System.Drawing.Size(70, 18);
            this.DS_rdBtnDiscountNone.TabIndex = 0;
            this.DS_rdBtnDiscountNone.TabStop = true;
            this.DS_rdBtnDiscountNone.Text = "nessuno";
            this.DS_rdBtnDiscountNone.UseVisualStyleBackColor = true;
            this.DS_rdBtnDiscountNone.Click += new System.EventHandler(this.DS_rdBtnDiscountNone_Click);
            // 
            // DS_rdBtnDiscountGratis
            // 
            this.DS_rdBtnDiscountGratis.AutoSize = true;
            this.DS_rdBtnDiscountGratis.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DS_rdBtnDiscountGratis.Location = new System.Drawing.Point(26, 182);
            this.DS_rdBtnDiscountGratis.Name = "DS_rdBtnDiscountGratis";
            this.DS_rdBtnDiscountGratis.Size = new System.Drawing.Size(141, 18);
            this.DS_rdBtnDiscountGratis.TabIndex = 3;
            this.DS_rdBtnDiscountGratis.TabStop = true;
            this.DS_rdBtnDiscountGratis.Text = "sconto totale (gratis)";
            this.DS_rdBtnDiscountGratis.UseVisualStyleBackColor = true;
            this.DS_rdBtnDiscountGratis.Click += new System.EventHandler(this.DS_rdBtnDiscountGratis_Click);
            // 
            // DS_rdBtnDiscountFixed
            // 
            this.DS_rdBtnDiscountFixed.AutoSize = true;
            this.DS_rdBtnDiscountFixed.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DS_rdBtnDiscountFixed.Location = new System.Drawing.Point(26, 132);
            this.DS_rdBtnDiscountFixed.Name = "DS_rdBtnDiscountFixed";
            this.DS_rdBtnDiscountFixed.Size = new System.Drawing.Size(89, 18);
            this.DS_rdBtnDiscountFixed.TabIndex = 2;
            this.DS_rdBtnDiscountFixed.TabStop = true;
            this.DS_rdBtnDiscountFixed.Text = "sconto fisso";
            this.DS_rdBtnDiscountFixed.UseVisualStyleBackColor = true;
            this.DS_rdBtnDiscountFixed.Click += new System.EventHandler(this.DS_rdBtnDiscountFixed_Click);
            // 
            // DS_rdBtnDiscountStd
            // 
            this.DS_rdBtnDiscountStd.AutoSize = true;
            this.DS_rdBtnDiscountStd.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DS_rdBtnDiscountStd.Location = new System.Drawing.Point(26, 82);
            this.DS_rdBtnDiscountStd.Name = "DS_rdBtnDiscountStd";
            this.DS_rdBtnDiscountStd.Size = new System.Drawing.Size(91, 18);
            this.DS_rdBtnDiscountStd.TabIndex = 1;
            this.DS_rdBtnDiscountStd.TabStop = true;
            this.DS_rdBtnDiscountStd.Text = "sconto in %";
            this.DS_rdBtnDiscountStd.UseVisualStyleBackColor = true;
            this.DS_rdBtnDiscountStd.Click += new System.EventHandler(this.DS_rdBtnDiscountStd_Click);
            // 
            // textBoxMotivazione
            // 
            this.textBoxMotivazione.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxMotivazione.Location = new System.Drawing.Point(260, 46);
            this.textBoxMotivazione.MaxLength = 25;
            this.textBoxMotivazione.Name = "textBoxMotivazione";
            this.textBoxMotivazione.Size = new System.Drawing.Size(251, 23);
            this.textBoxMotivazione.TabIndex = 0;
            this.textBoxMotivazione.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TextBoxMotivazione_KeyUp);
            // 
            // DS_lblDiscountTxt
            // 
            this.DS_lblDiscountTxt.AutoSize = true;
            this.DS_lblDiscountTxt.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DS_lblDiscountTxt.Location = new System.Drawing.Point(257, 20);
            this.DS_lblDiscountTxt.Name = "DS_lblDiscountTxt";
            this.DS_lblDiscountTxt.Size = new System.Drawing.Size(257, 14);
            this.DS_lblDiscountTxt.TabIndex = 10;
            this.DS_lblDiscountTxt.Text = "causale obbligatoria (min. 8 caratteri, max 25)";
            // 
            // textBoxFixVal
            // 
            this.textBoxFixVal.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxFixVal.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.textBoxFixVal.Location = new System.Drawing.Point(260, 209);
            this.textBoxFixVal.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxFixVal.MaxLength = 6;
            this.textBoxFixVal.Name = "textBoxFixVal";
            this.textBoxFixVal.Size = new System.Drawing.Size(65, 23);
            this.textBoxFixVal.TabIndex = 8;
            this.textBoxFixVal.WordWrap = false;
            this.textBoxFixVal.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBoxFixVal_KeyPress);
            this.textBoxFixVal.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TextBoxFixVal_KeyUp);
            // 
            // DS_lblDiscountVal
            // 
            this.DS_lblDiscountVal.AutoSize = true;
            this.DS_lblDiscountVal.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DS_lblDiscountVal.Location = new System.Drawing.Point(257, 188);
            this.DS_lblDiscountVal.Name = "DS_lblDiscountVal";
            this.DS_lblDiscountVal.Size = new System.Drawing.Size(121, 14);
            this.DS_lblDiscountVal.TabIndex = 12;
            this.DS_lblDiscountVal.Text = "importo sconto fisso:";
            // 
            // ckBox_0
            // 
            this.ckBox_0.AutoSize = true;
            this.ckBox_0.Location = new System.Drawing.Point(369, 127);
            this.ckBox_0.Name = "ckBox_0";
            this.ckBox_0.Size = new System.Drawing.Size(40, 17);
            this.ckBox_0.TabIndex = 2;
            this.ckBox_0.Text = "G1";
            this.ckBox_0.UseVisualStyleBackColor = true;
            this.ckBox_0.Click += new System.EventHandler(this.CheckBox_Click);
            // 
            // ckBox_1
            // 
            this.ckBox_1.AutoSize = true;
            this.ckBox_1.Location = new System.Drawing.Point(432, 127);
            this.ckBox_1.Name = "ckBox_1";
            this.ckBox_1.Size = new System.Drawing.Size(40, 17);
            this.ckBox_1.TabIndex = 3;
            this.ckBox_1.Text = "G2";
            this.ckBox_1.UseVisualStyleBackColor = true;
            this.ckBox_1.Click += new System.EventHandler(this.CheckBox_Click);
            // 
            // ckBox_2
            // 
            this.ckBox_2.AutoSize = true;
            this.ckBox_2.Location = new System.Drawing.Point(495, 127);
            this.ckBox_2.Name = "ckBox_2";
            this.ckBox_2.Size = new System.Drawing.Size(40, 17);
            this.ckBox_2.TabIndex = 4;
            this.ckBox_2.Text = "G3";
            this.ckBox_2.UseVisualStyleBackColor = true;
            this.ckBox_2.Click += new System.EventHandler(this.CheckBox_Click);
            // 
            // ckBox_3
            // 
            this.ckBox_3.AutoSize = true;
            this.ckBox_3.Location = new System.Drawing.Point(558, 128);
            this.ckBox_3.Name = "ckBox_3";
            this.ckBox_3.Size = new System.Drawing.Size(40, 17);
            this.ckBox_3.TabIndex = 5;
            this.ckBox_3.Text = "G4";
            this.ckBox_3.UseVisualStyleBackColor = true;
            this.ckBox_3.Click += new System.EventHandler(this.CheckBox_Click);
            // 
            // ckBox_4
            // 
            this.ckBox_4.AutoSize = true;
            this.ckBox_4.Location = new System.Drawing.Point(369, 156);
            this.ckBox_4.Name = "ckBox_4";
            this.ckBox_4.Size = new System.Drawing.Size(40, 17);
            this.ckBox_4.TabIndex = 6;
            this.ckBox_4.Text = "G5";
            this.ckBox_4.UseVisualStyleBackColor = true;
            this.ckBox_4.Click += new System.EventHandler(this.CheckBox_Click);
            // 
            // ckBox_5
            // 
            this.ckBox_5.AutoSize = true;
            this.ckBox_5.Location = new System.Drawing.Point(432, 156);
            this.ckBox_5.Name = "ckBox_5";
            this.ckBox_5.Size = new System.Drawing.Size(40, 17);
            this.ckBox_5.TabIndex = 7;
            this.ckBox_5.Text = "G6";
            this.ckBox_5.UseVisualStyleBackColor = true;
            this.ckBox_5.Click += new System.EventHandler(this.CheckBox_Click);
            // 
            // textBoxPercVal
            // 
            this.textBoxPercVal.Font = new System.Drawing.Font("Tahoma", 9F);
            this.textBoxPercVal.Location = new System.Drawing.Point(259, 125);
            this.textBoxPercVal.MaxLength = 2;
            this.textBoxPercVal.Name = "textBoxPercVal";
            this.textBoxPercVal.Size = new System.Drawing.Size(41, 22);
            this.textBoxPercVal.TabIndex = 1;
            this.textBoxPercVal.Text = " %";
            this.textBoxPercVal.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBoxPerc_KeyPress);
            this.textBoxPercVal.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TextBoxPerc_KeyUp);
            // 
            // DS_lblDiscountPerc
            // 
            this.DS_lblDiscountPerc.AutoSize = true;
            this.DS_lblDiscountPerc.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DS_lblDiscountPerc.Location = new System.Drawing.Point(257, 99);
            this.DS_lblDiscountPerc.Name = "DS_lblDiscountPerc";
            this.DS_lblDiscountPerc.Size = new System.Drawing.Size(341, 14);
            this.DS_lblDiscountPerc.TabIndex = 27;
            this.DS_lblDiscountPerc.Text = "perc. sconto e spunta dei gruppi sui quali applicare lo sconto:";
            // 
            // DS_btnSave
            // 
            this.DS_btnSave.Image = global::StandFacile.Properties.Resources.Save;
            this.DS_btnSave.Location = new System.Drawing.Point(102, 269);
            this.DS_btnSave.Name = "DS_btnSave";
            this.DS_btnSave.Size = new System.Drawing.Size(55, 33);
            this.DS_btnSave.TabIndex = 9;
            this.DS_btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.DS_btnSave.UseVisualStyleBackColor = true;
            this.DS_btnSave.Click += new System.EventHandler(this.DS_btnSalva_Click);
            // 
            // ckBox_6
            // 
            this.ckBox_6.AutoSize = true;
            this.ckBox_6.Location = new System.Drawing.Point(495, 156);
            this.ckBox_6.Name = "ckBox_6";
            this.ckBox_6.Size = new System.Drawing.Size(40, 17);
            this.ckBox_6.TabIndex = 28;
            this.ckBox_6.Text = "G7";
            this.ckBox_6.UseVisualStyleBackColor = true;
            this.ckBox_6.Click += new System.EventHandler(this.CheckBox_Click);
            // 
            // ckBox_7
            // 
            this.ckBox_7.AutoSize = true;
            this.ckBox_7.Location = new System.Drawing.Point(558, 156);
            this.ckBox_7.Name = "ckBox_7";
            this.ckBox_7.Size = new System.Drawing.Size(40, 17);
            this.ckBox_7.TabIndex = 29;
            this.ckBox_7.Text = "G8";
            this.ckBox_7.UseVisualStyleBackColor = true;
            this.ckBox_7.Click += new System.EventHandler(this.CheckBox_Click);
            // 
            // DS_lblDiscountWarn
            // 
            this.DS_lblDiscountWarn.AutoSize = true;
            this.DS_lblDiscountWarn.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DS_lblDiscountWarn.Location = new System.Drawing.Point(14, 244);
            this.DS_lblDiscountWarn.Name = "DS_lblDiscountWarn";
            this.DS_lblDiscountWarn.Size = new System.Drawing.Size(220, 14);
            this.DS_lblDiscountWarn.TabIndex = 30;
            this.DS_lblDiscountWarn.Text = "il pulsante è attivo se i 3 modi sono OK";
            // 
            // radioBtn100
            // 
            this.radioBtn100.AutoCheck = false;
            this.radioBtn100.AutoSize = true;
            this.radioBtn100.Location = new System.Drawing.Point(306, 128);
            this.radioBtn100.Name = "radioBtn100";
            this.radioBtn100.Size = new System.Drawing.Size(51, 17);
            this.radioBtn100.TabIndex = 31;
            this.radioBtn100.TabStop = true;
            this.radioBtn100.Text = "100%";
            this.radioBtn100.UseVisualStyleBackColor = true;
            this.radioBtn100.Click += new System.EventHandler(this.RadioBtn100_Click);
            // 
            // ckBox_9
            // 
            this.ckBox_9.AutoSize = true;
            this.ckBox_9.Location = new System.Drawing.Point(558, 184);
            this.ckBox_9.Name = "ckBox_9";
            this.ckBox_9.Size = new System.Drawing.Size(40, 17);
            this.ckBox_9.TabIndex = 32;
            this.ckBox_9.Text = "CS";
            this.ckBox_9.UseVisualStyleBackColor = true;
            this.ckBox_9.Click += new System.EventHandler(this.CheckBox_Click);
            // 
            // ckBox_8
            // 
            this.ckBox_8.AutoSize = true;
            this.ckBox_8.Location = new System.Drawing.Point(495, 185);
            this.ckBox_8.Name = "ckBox_8";
            this.ckBox_8.Size = new System.Drawing.Size(40, 17);
            this.ckBox_8.TabIndex = 33;
            this.ckBox_8.Text = "G9";
            this.ckBox_8.UseVisualStyleBackColor = true;
            this.ckBox_8.Click += new System.EventHandler(this.CheckBox_Click);
            // 
            // ScontoDlg
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(614, 326);
            this.Controls.Add(this.ckBox_8);
            this.Controls.Add(this.ckBox_9);
            this.Controls.Add(this.radioBtn100);
            this.Controls.Add(this.DS_lblDiscountWarn);
            this.Controls.Add(this.ckBox_7);
            this.Controls.Add(this.ckBox_6);
            this.Controls.Add(this.DS_btnSave);
            this.Controls.Add(this.DS_lblDiscountPerc);
            this.Controls.Add(this.textBoxPercVal);
            this.Controls.Add(this.ckBox_5);
            this.Controls.Add(this.ckBox_4);
            this.Controls.Add(this.ckBox_3);
            this.Controls.Add(this.ckBox_2);
            this.Controls.Add(this.ckBox_1);
            this.Controls.Add(this.ckBox_0);
            this.Controls.Add(this.DS_lblDiscountVal);
            this.Controls.Add(this.textBoxFixVal);
            this.Controls.Add(this.DS_lblDiscountTxt);
            this.Controls.Add(this.textBoxMotivazione);
            this.Controls.Add(this.DS_grpBxDiscount);
            this.Controls.Add(this.BT_Cancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ScontoDlg";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sconto";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ScontoDlg_FormClosing);
            this.DS_grpBxDiscount.ResumeLayout(false);
            this.DS_grpBxDiscount.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BT_Cancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.GroupBox DS_grpBxDiscount;
        private System.Windows.Forms.RadioButton DS_rdBtnDiscountGratis;
        private System.Windows.Forms.RadioButton DS_rdBtnDiscountFixed;
        private System.Windows.Forms.RadioButton DS_rdBtnDiscountStd;
        private System.Windows.Forms.TextBox textBoxMotivazione;
        private System.Windows.Forms.Label DS_lblDiscountTxt;
        private System.Windows.Forms.RadioButton DS_rdBtnDiscountNone;
        private System.Windows.Forms.TextBox textBoxFixVal;
        private System.Windows.Forms.Label DS_lblDiscountVal;
        private System.Windows.Forms.CheckBox ckBox_0;
        private System.Windows.Forms.CheckBox ckBox_1;
        private System.Windows.Forms.CheckBox ckBox_2;
        private System.Windows.Forms.CheckBox ckBox_3;
        private System.Windows.Forms.CheckBox ckBox_4;
        private System.Windows.Forms.CheckBox ckBox_5;
        private System.Windows.Forms.TextBox textBoxPercVal;
        private System.Windows.Forms.Label DS_lblDiscountPerc;
        private System.Windows.Forms.Button DS_btnSave;
        private System.Windows.Forms.CheckBox ckBox_6;
        private System.Windows.Forms.CheckBox ckBox_7;
        private System.Windows.Forms.Label DS_lblDiscountWarn;
        private System.Windows.Forms.RadioButton radioBtn100;
        private System.Windows.Forms.CheckBox ckBox_9;
        private System.Windows.Forms.CheckBox ckBox_8;
    }
}
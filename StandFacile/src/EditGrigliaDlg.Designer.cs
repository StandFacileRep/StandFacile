namespace StandFacile
{
    partial class EditGrigliaDlg
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
            this.radioRows1 = new System.Windows.Forms.RadioButton();
            this.radioCols1 = new System.Windows.Forms.RadioButton();
            this.radioRows2 = new System.Windows.Forms.RadioButton();
            this.radioRows3 = new System.Windows.Forms.RadioButton();
            this.radioCols2 = new System.Windows.Forms.RadioButton();
            this.RadioRows = new System.Windows.Forms.GroupBox();
            this.RadioCols = new System.Windows.Forms.GroupBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.Edit_0 = new System.Windows.Forms.TextBox();
            this.Edit_1 = new System.Windows.Forms.TextBox();
            this.Edit_2 = new System.Windows.Forms.TextBox();
            this.Edit_3 = new System.Windows.Forms.TextBox();
            this.BtnCanc_0 = new System.Windows.Forms.Button();
            this.BtnCanc_1 = new System.Windows.Forms.Button();
            this.BtnCanc_2 = new System.Windows.Forms.Button();
            this.BtnCanc_3 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBoxTouchMode = new System.Windows.Forms.CheckBox();
            this.Edit_4 = new System.Windows.Forms.TextBox();
            this.radioCols3 = new System.Windows.Forms.RadioButton();
            this.BtnCanc_4 = new System.Windows.Forms.Button();
            this.RadioRows.SuspendLayout();
            this.RadioCols.SuspendLayout();
            this.SuspendLayout();
            // 
            // radioRows1
            // 
            this.radioRows1.AutoSize = true;
            this.radioRows1.Location = new System.Drawing.Point(19, 31);
            this.radioRows1.Name = "radioRows1";
            this.radioRows1.Size = new System.Drawing.Size(37, 17);
            this.radioRows1.TabIndex = 0;
            this.radioRows1.TabStop = true;
            this.radioRows1.Text = "16";
            this.radioRows1.UseVisualStyleBackColor = true;
            // 
            // radioCols1
            // 
            this.radioCols1.AutoSize = true;
            this.radioCols1.Location = new System.Drawing.Point(21, 31);
            this.radioCols1.Name = "radioCols1";
            this.radioCols1.Size = new System.Drawing.Size(31, 17);
            this.radioCols1.TabIndex = 0;
            this.radioCols1.TabStop = true;
            this.radioCols1.Text = "3";
            this.radioCols1.UseVisualStyleBackColor = true;
            // 
            // radioRows2
            // 
            this.radioRows2.AutoSize = true;
            this.radioRows2.Location = new System.Drawing.Point(19, 85);
            this.radioRows2.Name = "radioRows2";
            this.radioRows2.Size = new System.Drawing.Size(37, 17);
            this.radioRows2.TabIndex = 1;
            this.radioRows2.TabStop = true;
            this.radioRows2.Text = "20";
            this.radioRows2.UseVisualStyleBackColor = true;
            // 
            // radioRows3
            // 
            this.radioRows3.AutoSize = true;
            this.radioRows3.Location = new System.Drawing.Point(19, 139);
            this.radioRows3.Name = "radioRows3";
            this.radioRows3.Size = new System.Drawing.Size(37, 17);
            this.radioRows3.TabIndex = 2;
            this.radioRows3.TabStop = true;
            this.radioRows3.Text = "25";
            this.radioRows3.UseVisualStyleBackColor = true;
            // 
            // radioCols2
            // 
            this.radioCols2.AutoSize = true;
            this.radioCols2.Location = new System.Drawing.Point(21, 85);
            this.radioCols2.Name = "radioCols2";
            this.radioCols2.Size = new System.Drawing.Size(31, 17);
            this.radioCols2.TabIndex = 1;
            this.radioCols2.TabStop = true;
            this.radioCols2.Text = "4";
            this.radioCols2.UseVisualStyleBackColor = true;
            // 
            // RadioRows
            // 
            this.RadioRows.Controls.Add(this.radioRows3);
            this.RadioRows.Controls.Add(this.radioRows2);
            this.RadioRows.Controls.Add(this.radioRows1);
            this.RadioRows.Location = new System.Drawing.Point(12, 71);
            this.RadioRows.Name = "RadioRows";
            this.RadioRows.Size = new System.Drawing.Size(90, 178);
            this.RadioRows.TabIndex = 0;
            this.RadioRows.TabStop = false;
            this.RadioRows.Text = "Righe";
            // 
            // RadioCols
            // 
            this.RadioCols.Controls.Add(this.radioCols3);
            this.RadioCols.Controls.Add(this.radioCols2);
            this.RadioCols.Controls.Add(this.radioCols1);
            this.RadioCols.Location = new System.Drawing.Point(121, 71);
            this.RadioCols.Name = "RadioCols";
            this.RadioCols.Size = new System.Drawing.Size(90, 178);
            this.RadioCols.TabIndex = 1;
            this.RadioCols.TabStop = false;
            this.RadioCols.Text = "Colonne";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Image = global::StandFacile.Properties.Resources.Cancel;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(124, 277);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 28);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Annulla";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Image = global::StandFacile.Properties.Resources.OK;
            this.btnOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOK.Location = new System.Drawing.Point(249, 277);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 28);
            this.btnOK.TabIndex = 12;
            this.btnOK.Text = "OK  ";
            this.btnOK.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // Edit_0
            // 
            this.Edit_0.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Edit_0.Location = new System.Drawing.Point(249, 50);
            this.Edit_0.Name = "Edit_0";
            this.Edit_0.Size = new System.Drawing.Size(112, 21);
            this.Edit_0.TabIndex = 3;
            // 
            // Edit_1
            // 
            this.Edit_1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Edit_1.Location = new System.Drawing.Point(249, 94);
            this.Edit_1.Name = "Edit_1";
            this.Edit_1.Size = new System.Drawing.Size(112, 21);
            this.Edit_1.TabIndex = 5;
            // 
            // Edit_2
            // 
            this.Edit_2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Edit_2.Location = new System.Drawing.Point(249, 138);
            this.Edit_2.Name = "Edit_2";
            this.Edit_2.Size = new System.Drawing.Size(112, 21);
            this.Edit_2.TabIndex = 7;
            // 
            // Edit_3
            // 
            this.Edit_3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Edit_3.Location = new System.Drawing.Point(249, 182);
            this.Edit_3.Name = "Edit_3";
            this.Edit_3.Size = new System.Drawing.Size(112, 21);
            this.Edit_3.TabIndex = 9;
            // 
            // BtnCanc_0
            // 
            this.BtnCanc_0.Image = global::StandFacile.Properties.Resources.Cancel1;
            this.BtnCanc_0.Location = new System.Drawing.Point(384, 50);
            this.BtnCanc_0.Name = "BtnCanc_0";
            this.BtnCanc_0.Size = new System.Drawing.Size(27, 23);
            this.BtnCanc_0.TabIndex = 4;
            this.BtnCanc_0.UseVisualStyleBackColor = true;
            this.BtnCanc_0.Click += new System.EventHandler(this.BtnCanc_0_Click);
            // 
            // BtnCanc_1
            // 
            this.BtnCanc_1.Image = global::StandFacile.Properties.Resources.Cancel1;
            this.BtnCanc_1.Location = new System.Drawing.Point(384, 94);
            this.BtnCanc_1.Name = "BtnCanc_1";
            this.BtnCanc_1.Size = new System.Drawing.Size(27, 23);
            this.BtnCanc_1.TabIndex = 6;
            this.BtnCanc_1.UseVisualStyleBackColor = true;
            this.BtnCanc_1.Click += new System.EventHandler(this.BtnCanc_1_Click);
            // 
            // BtnCanc_2
            // 
            this.BtnCanc_2.Image = global::StandFacile.Properties.Resources.Cancel1;
            this.BtnCanc_2.Location = new System.Drawing.Point(384, 138);
            this.BtnCanc_2.Name = "BtnCanc_2";
            this.BtnCanc_2.Size = new System.Drawing.Size(27, 23);
            this.BtnCanc_2.TabIndex = 8;
            this.BtnCanc_2.UseVisualStyleBackColor = true;
            this.BtnCanc_2.Click += new System.EventHandler(this.BtnCanc_2_Click);
            // 
            // BtnCanc_3
            // 
            this.BtnCanc_3.Image = global::StandFacile.Properties.Resources.Cancel1;
            this.BtnCanc_3.Location = new System.Drawing.Point(384, 182);
            this.BtnCanc_3.Name = "BtnCanc_3";
            this.BtnCanc_3.Size = new System.Drawing.Size(27, 23);
            this.BtnCanc_3.TabIndex = 10;
            this.BtnCanc_3.UseVisualStyleBackColor = true;
            this.BtnCanc_3.Click += new System.EventHandler(this.BtnCanc_3_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(253, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 14);
            this.label1.TabIndex = 2;
            this.label1.Text = "etichette TABs";
            // 
            // checkBoxTouchMode
            // 
            this.checkBoxTouchMode.AutoSize = true;
            this.checkBoxTouchMode.Checked = true;
            this.checkBoxTouchMode.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxTouchMode.Font = new System.Drawing.Font("Tahoma", 9F);
            this.checkBoxTouchMode.Location = new System.Drawing.Point(31, 18);
            this.checkBoxTouchMode.Name = "checkBoxTouchMode";
            this.checkBoxTouchMode.Size = new System.Drawing.Size(96, 18);
            this.checkBoxTouchMode.TabIndex = 14;
            this.checkBoxTouchMode.Text = "Touch mode";
            this.checkBoxTouchMode.UseVisualStyleBackColor = true;
            this.checkBoxTouchMode.CheckedChanged += new System.EventHandler(this.checkBoxTouchMode_CheckedChanged);
            // 
            // Edit_4
            // 
            this.Edit_4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Edit_4.Location = new System.Drawing.Point(249, 226);
            this.Edit_4.Name = "Edit_4";
            this.Edit_4.Size = new System.Drawing.Size(112, 21);
            this.Edit_4.TabIndex = 15;
            // 
            // radioCols3
            // 
            this.radioCols3.AutoSize = true;
            this.radioCols3.Location = new System.Drawing.Point(21, 139);
            this.radioCols3.Name = "radioCols3";
            this.radioCols3.Size = new System.Drawing.Size(31, 17);
            this.radioCols3.TabIndex = 2;
            this.radioCols3.TabStop = true;
            this.radioCols3.Text = "5";
            this.radioCols3.UseVisualStyleBackColor = true;
            // 
            // BtnCanc_4
            // 
            this.BtnCanc_4.Image = global::StandFacile.Properties.Resources.Cancel1;
            this.BtnCanc_4.Location = new System.Drawing.Point(384, 226);
            this.BtnCanc_4.Name = "BtnCanc_4";
            this.BtnCanc_4.Size = new System.Drawing.Size(27, 23);
            this.BtnCanc_4.TabIndex = 16;
            this.BtnCanc_4.UseVisualStyleBackColor = true;
            this.BtnCanc_4.Click += new System.EventHandler(this.BtnCanc_4_Click);
            // 
            // EditGrigliaDlg
            // 
            this.AcceptButton = this.btnCancel;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(452, 326);
            this.Controls.Add(this.BtnCanc_4);
            this.Controls.Add(this.Edit_4);
            this.Controls.Add(this.checkBoxTouchMode);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BtnCanc_3);
            this.Controls.Add(this.BtnCanc_2);
            this.Controls.Add(this.BtnCanc_1);
            this.Controls.Add(this.BtnCanc_0);
            this.Controls.Add(this.Edit_3);
            this.Controls.Add(this.Edit_2);
            this.Controls.Add(this.Edit_1);
            this.Controls.Add(this.Edit_0);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.RadioCols);
            this.Controls.Add(this.RadioRows);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditGrigliaDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Impostazioni  Griglia";
            this.RadioRows.ResumeLayout(false);
            this.RadioRows.PerformLayout();
            this.RadioCols.ResumeLayout(false);
            this.RadioCols.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton radioRows1;
        private System.Windows.Forms.RadioButton radioCols1;
        private System.Windows.Forms.RadioButton radioRows2;
        private System.Windows.Forms.RadioButton radioRows3;
        private System.Windows.Forms.RadioButton radioCols2;
        private System.Windows.Forms.GroupBox RadioRows;
        private System.Windows.Forms.GroupBox RadioCols;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox Edit_0;
        private System.Windows.Forms.TextBox Edit_1;
        private System.Windows.Forms.TextBox Edit_2;
        private System.Windows.Forms.TextBox Edit_3;
        private System.Windows.Forms.Button BtnCanc_0;
        private System.Windows.Forms.Button BtnCanc_1;
        private System.Windows.Forms.Button BtnCanc_2;
        private System.Windows.Forms.Button BtnCanc_3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBoxTouchMode;
        private System.Windows.Forms.TextBox Edit_4;
        private System.Windows.Forms.RadioButton radioCols3;
        private System.Windows.Forms.Button BtnCanc_4;
    }
}
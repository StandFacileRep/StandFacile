namespace StandFacile
{
    partial class startDispDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(startDispDlg));
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblMessage1 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblMessage2 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ckBoxNoMoreView = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.Image = ((System.Drawing.Image)(resources.GetObject("btnOK.Image")));
            this.btnOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOK.Location = new System.Drawing.Point(42, 238);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(358, 32);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "avvio con piena disponibilità degli articoli";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnFullDisp_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Image = global::StandFacile.Properties.Resources.edit;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(42, 300);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.btnCancel.Size = new System.Drawing.Size(358, 32);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "vai al dialogo di scelta disponibilità per gli articoli ";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.SD_btnEditDisp_Click);
            // 
            // lblMessage1
            // 
            this.lblMessage1.AutoSize = true;
            this.lblMessage1.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage1.Location = new System.Drawing.Point(134, 61);
            this.lblMessage1.Name = "lblMessage1";
            this.lblMessage1.Size = new System.Drawing.Size(166, 18);
            this.lblMessage1.TabIndex = 3;
            this.lblMessage1.Text = "PC Locale :  23/11/2023";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(53, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(148, 18);
            this.label1.TabIndex = 2;
            this.label1.Text = "Rilevato cambio data:";
            // 
            // lblMessage2
            // 
            this.lblMessage2.AutoSize = true;
            this.lblMessage2.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage2.Location = new System.Drawing.Point(134, 94);
            this.lblMessage2.Name = "lblMessage2";
            this.lblMessage2.Size = new System.Drawing.Size(166, 18);
            this.lblMessage2.TabIndex = 4;
            this.lblMessage2.Text = "Database :  22/11/2023";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(53, 141);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(303, 18);
            this.label2.TabIndex = 5;
            this.label2.Text = "la numerazione degli scontrini verrà riavviata !";
            // 
            // ckBoxNoMoreView
            // 
            this.ckBoxNoMoreView.AutoSize = true;
            this.ckBoxNoMoreView.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.ckBoxNoMoreView.Location = new System.Drawing.Point(42, 185);
            this.ckBoxNoMoreView.Name = "ckBoxNoMoreView";
            this.ckBoxNoMoreView.Size = new System.Drawing.Size(210, 17);
            this.ckBoxNoMoreView.TabIndex = 6;
            this.ckBoxNoMoreView.Text = "memorizza la scelta e non mostrare più,";
            this.ckBoxNoMoreView.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(57, 205);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(258, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "si può sempre modificare da: Imposta -> Opzioni Varie";
            // 
            // startDispDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 359);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ckBoxNoMoreView);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblMessage2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblMessage1);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "startDispDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Imposta avvio";
            this.Click += new System.EventHandler(this.BtnFullDisp_Click);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblMessage1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblMessage2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox ckBoxNoMoreView;
        private System.Windows.Forms.Label label3;
    }
}
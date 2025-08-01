namespace StandFacile
{
    partial class EditArticoloDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditArticoloDlg));
            this.btnElimina = new System.Windows.Forms.Button();
            this.AnnullaBtn = new System.Windows.Forms.Button();
            this.OkBtn = new System.Windows.Forms.Button();
            this.ckBoxSkipEmpty = new System.Windows.Forms.CheckBox();
            this.btnSalva = new System.Windows.Forms.Button();
            this.btnNavRight = new System.Windows.Forms.Button();
            this.btnNavLeft = new System.Windows.Forms.Button();
            this.tabEditArticolo = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupsText = new System.Windows.Forms.TextBox();
            this.labelNota = new System.Windows.Forms.Label();
            this.LblMaxChar = new System.Windows.Forms.Label();
            this.LbRimanenti = new System.Windows.Forms.Label();
            this.groupsCombo = new System.Windows.Forms.ComboBox();
            this.lbl3 = new System.Windows.Forms.Label();
            this.lbl2 = new System.Windows.Forms.Label();
            this.lb1 = new System.Windows.Forms.Label();
            this.PrzEdit = new System.Windows.Forms.TextBox();
            this.TipoEdit = new System.Windows.Forms.TextBox();
            this.tabEditArticolo.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnElimina
            // 
            this.btnElimina.Image = ((System.Drawing.Image)(resources.GetObject("btnElimina.Image")));
            this.btnElimina.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnElimina.Location = new System.Drawing.Point(59, 387);
            this.btnElimina.Name = "btnElimina";
            this.btnElimina.Size = new System.Drawing.Size(80, 28);
            this.btnElimina.TabIndex = 3;
            this.btnElimina.Text = "Elimina";
            this.btnElimina.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnElimina.UseVisualStyleBackColor = true;
            this.btnElimina.Click += new System.EventHandler(this.BtnElimina_Click);
            // 
            // AnnullaBtn
            // 
            this.AnnullaBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.AnnullaBtn.Image = global::StandFacile.Properties.Resources.Cancel;
            this.AnnullaBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.AnnullaBtn.Location = new System.Drawing.Point(178, 387);
            this.AnnullaBtn.Name = "AnnullaBtn";
            this.AnnullaBtn.Size = new System.Drawing.Size(80, 28);
            this.AnnullaBtn.TabIndex = 1;
            this.AnnullaBtn.Text = "Annulla";
            this.AnnullaBtn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.AnnullaBtn.UseVisualStyleBackColor = true;
            this.AnnullaBtn.Click += new System.EventHandler(this.AnnullaBtn_Click);
            // 
            // OkBtn
            // 
            this.OkBtn.Image = global::StandFacile.Properties.Resources.OK;
            this.OkBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.OkBtn.Location = new System.Drawing.Point(294, 387);
            this.OkBtn.Name = "OkBtn";
            this.OkBtn.Size = new System.Drawing.Size(80, 28);
            this.OkBtn.TabIndex = 2;
            this.OkBtn.Text = "OK";
            this.OkBtn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.OkBtn.UseVisualStyleBackColor = true;
            this.OkBtn.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // ckBoxSkipEmpty
            // 
            this.ckBoxSkipEmpty.AutoSize = true;
            this.ckBoxSkipEmpty.Checked = true;
            this.ckBoxSkipEmpty.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckBoxSkipEmpty.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.ckBoxSkipEmpty.Location = new System.Drawing.Point(264, 338);
            this.ckBoxSkipEmpty.Name = "ckBoxSkipEmpty";
            this.ckBoxSkipEmpty.Size = new System.Drawing.Size(127, 20);
            this.ckBoxSkipEmpty.TabIndex = 38;
            this.ckBoxSkipEmpty.Text = "salta Articoli vuoti";
            this.ckBoxSkipEmpty.UseVisualStyleBackColor = true;
            // 
            // btnSalva
            // 
            this.btnSalva.Image = global::StandFacile.Properties.Resources.Save;
            this.btnSalva.Location = new System.Drawing.Point(60, 333);
            this.btnSalva.Name = "btnSalva";
            this.btnSalva.Size = new System.Drawing.Size(43, 28);
            this.btnSalva.TabIndex = 37;
            this.btnSalva.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSalva.UseVisualStyleBackColor = true;
            this.btnSalva.Click += new System.EventHandler(this.BtnSalva_Click);
            // 
            // btnNavRight
            // 
            this.btnNavRight.Image = global::StandFacile.Properties.Resources.ArrowR;
            this.btnNavRight.Location = new System.Drawing.Point(204, 333);
            this.btnNavRight.Name = "btnNavRight";
            this.btnNavRight.Size = new System.Drawing.Size(43, 28);
            this.btnNavRight.TabIndex = 36;
            this.btnNavRight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnNavRight.UseVisualStyleBackColor = true;
            this.btnNavRight.Click += new System.EventHandler(this.BtnNavRight_Click);
            this.btnNavRight.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EditArticoloDlg_KeyDown);
            // 
            // btnNavLeft
            // 
            this.btnNavLeft.Image = global::StandFacile.Properties.Resources.ArrowL;
            this.btnNavLeft.Location = new System.Drawing.Point(135, 333);
            this.btnNavLeft.Name = "btnNavLeft";
            this.btnNavLeft.Size = new System.Drawing.Size(43, 28);
            this.btnNavLeft.TabIndex = 35;
            this.btnNavLeft.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnNavLeft.UseVisualStyleBackColor = true;
            this.btnNavLeft.Click += new System.EventHandler(this.BtnNavLeft_Click);
            this.btnNavLeft.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EditArticoloDlg_KeyDown);
            // 
            // tabEditArticolo
            // 
            this.tabEditArticolo.Controls.Add(this.tabPage1);
            this.tabEditArticolo.Controls.Add(this.tabPage2);
            this.tabEditArticolo.Controls.Add(this.tabPage3);
            this.tabEditArticolo.Font = new System.Drawing.Font("Tahoma", 11.25F);
            this.tabEditArticolo.Location = new System.Drawing.Point(23, 17);
            this.tabEditArticolo.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.tabEditArticolo.Name = "tabEditArticolo";
            this.tabEditArticolo.Padding = new System.Drawing.Point(12, 3);
            this.tabEditArticolo.SelectedIndex = 0;
            this.tabEditArticolo.Size = new System.Drawing.Size(248, 23);
            this.tabEditArticolo.TabIndex = 4;
            this.tabEditArticolo.SelectedIndexChanged += new System.EventHandler(this.TabEditArt_SelectedIndexChanged);
            this.tabEditArticolo.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabEditArticolo_Selecting);
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 27);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(240, 0);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Articoli";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 27);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(240, 0);
            this.tabPage2.TabIndex = 2;
            this.tabPage2.Text = "Buoni sconto";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 27);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(240, 0);
            this.tabPage3.TabIndex = 1;
            this.tabPage3.Text = "Coperti";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.groupsText);
            this.panel1.Controls.Add(this.labelNota);
            this.panel1.Controls.Add(this.LblMaxChar);
            this.panel1.Controls.Add(this.LbRimanenti);
            this.panel1.Controls.Add(this.groupsCombo);
            this.panel1.Controls.Add(this.lbl3);
            this.panel1.Controls.Add(this.lbl2);
            this.panel1.Controls.Add(this.lb1);
            this.panel1.Controls.Add(this.PrzEdit);
            this.panel1.Controls.Add(this.TipoEdit);
            this.panel1.Location = new System.Drawing.Point(23, 43);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(394, 269);
            this.panel1.TabIndex = 40;
            // 
            // groupsText
            // 
            this.groupsText.BackColor = System.Drawing.SystemColors.Window;
            this.groupsText.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupsText.Location = new System.Drawing.Point(150, 181);
            this.groupsText.MaxLength = 18;
            this.groupsText.Name = "groupsText";
            this.groupsText.ReadOnly = true;
            this.groupsText.Size = new System.Drawing.Size(226, 22);
            this.groupsText.TabIndex = 27;
            // 
            // labelNota
            // 
            this.labelNota.AutoSize = true;
            this.labelNota.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNota.Location = new System.Drawing.Point(17, 150);
            this.labelNota.Name = "labelNota";
            this.labelNota.Size = new System.Drawing.Size(306, 16);
            this.labelNota.TabIndex = 24;
            this.labelNota.Text = "nota : \"Contatori\" ha il Prezzo = 0 quindi è gratuito !";
            // 
            // LblMaxChar
            // 
            this.LblMaxChar.AutoSize = true;
            this.LblMaxChar.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblMaxChar.Location = new System.Drawing.Point(16, 32);
            this.LblMaxChar.Name = "LblMaxChar";
            this.LblMaxChar.Size = new System.Drawing.Size(109, 16);
            this.LblMaxChar.TabIndex = 22;
            this.LblMaxChar.Text = "(max xx caratteri)";
            // 
            // LbRimanenti
            // 
            this.LbRimanenti.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbRimanenti.Location = new System.Drawing.Point(180, 40);
            this.LbRimanenti.Name = "LbRimanenti";
            this.LbRimanenti.Size = new System.Drawing.Size(130, 25);
            this.LbRimanenti.TabIndex = 26;
            this.LbRimanenti.Text = "car. rimanenti x";
            // 
            // groupsCombo
            // 
            this.groupsCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.groupsCombo.DropDownWidth = 250;
            this.groupsCombo.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupsCombo.FormattingEnabled = true;
            this.groupsCombo.Location = new System.Drawing.Point(150, 209);
            this.groupsCombo.MaxDropDownItems = 20;
            this.groupsCombo.Name = "groupsCombo";
            this.groupsCombo.Size = new System.Drawing.Size(225, 22);
            this.groupsCombo.TabIndex = 2;
            this.groupsCombo.SelectedIndexChanged += new System.EventHandler(this.groupsCombo_SelectedIndexChanged);
            // 
            // lbl3
            // 
            this.lbl3.AutoSize = true;
            this.lbl3.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl3.Location = new System.Drawing.Point(19, 215);
            this.lbl3.Name = "lbl3";
            this.lbl3.Size = new System.Drawing.Size(110, 16);
            this.lbl3.TabIndex = 25;
            this.lbl3.Text = "Gruppo di Stampa";
            // 
            // lbl2
            // 
            this.lbl2.AutoSize = true;
            this.lbl2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl2.Location = new System.Drawing.Point(17, 93);
            this.lbl2.Name = "lbl2";
            this.lbl2.Size = new System.Drawing.Size(110, 16);
            this.lbl2.TabIndex = 23;
            this.lbl2.Text = "Prezzo Standard :";
            // 
            // lb1
            // 
            this.lb1.AutoSize = true;
            this.lb1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb1.Location = new System.Drawing.Point(16, 18);
            this.lb1.Name = "lb1";
            this.lb1.Size = new System.Drawing.Size(119, 16);
            this.lb1.TabIndex = 21;
            this.lb1.Text = "Nome dell\'Articolo :";
            // 
            // PrzEdit
            // 
            this.PrzEdit.Font = new System.Drawing.Font("Tahoma", 9F);
            this.PrzEdit.Location = new System.Drawing.Point(150, 90);
            this.PrzEdit.MaxLength = 6;
            this.PrzEdit.Name = "PrzEdit";
            this.PrzEdit.Size = new System.Drawing.Size(60, 22);
            this.PrzEdit.TabIndex = 1;
            this.PrzEdit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.PrzEdit_KeyPress);
            this.PrzEdit.KeyUp += new System.Windows.Forms.KeyEventHandler(this.PrzEdit_KeyUp);
            // 
            // TipoEdit
            // 
            this.TipoEdit.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TipoEdit.Location = new System.Drawing.Point(149, 15);
            this.TipoEdit.MaxLength = 18;
            this.TipoEdit.Name = "TipoEdit";
            this.TipoEdit.Size = new System.Drawing.Size(226, 22);
            this.TipoEdit.TabIndex = 0;
            this.TipoEdit.TextChanged += new System.EventHandler(this.TipoEdit_TextChanged);
            // 
            // EditArticoloDlg
            // 
            this.AcceptButton = this.OkBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(437, 431);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tabEditArticolo);
            this.Controls.Add(this.ckBoxSkipEmpty);
            this.Controls.Add(this.btnSalva);
            this.Controls.Add(this.btnNavRight);
            this.Controls.Add(this.btnNavLeft);
            this.Controls.Add(this.btnElimina);
            this.Controls.Add(this.AnnullaBtn);
            this.Controls.Add(this.OkBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditArticoloDlg";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Imposta Articolo";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EditArticoloDlg_KeyDown);
            this.tabEditArticolo.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button AnnullaBtn;
        private System.Windows.Forms.Button OkBtn;
        private System.Windows.Forms.Button btnElimina;
        private System.Windows.Forms.CheckBox ckBoxSkipEmpty;
        private System.Windows.Forms.Button btnSalva;
        private System.Windows.Forms.Button btnNavRight;
        private System.Windows.Forms.Button btnNavLeft;
        private System.Windows.Forms.TabControl tabEditArticolo;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelNota;
        private System.Windows.Forms.Label LblMaxChar;
        private System.Windows.Forms.Label LbRimanenti;
        private System.Windows.Forms.ComboBox groupsCombo;
        private System.Windows.Forms.Label lbl3;
        private System.Windows.Forms.Label lbl2;
        private System.Windows.Forms.Label lb1;
        private System.Windows.Forms.TextBox PrzEdit;
        private System.Windows.Forms.TextBox TipoEdit;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox groupsText;
    }
}
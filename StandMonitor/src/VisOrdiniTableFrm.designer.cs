namespace StandFacile
{
    partial class VisOrdiniTableFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VisOrdiniTableFrm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.OKBtn = new System.Windows.Forms.Button();
            this.OrdiniGrid = new System.Windows.Forms.DataGridView();
            this.checkBox_nonevasi = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.OrdiniGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // OKBtn
            // 
            this.OKBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OKBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OKBtn.Image = ((System.Drawing.Image)(resources.GetObject("OKBtn.Image")));
            this.OKBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.OKBtn.Location = new System.Drawing.Point(476, 550);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(68, 30);
            this.OKBtn.TabIndex = 4;
            this.OKBtn.Text = "OK ";
            this.OKBtn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // OrdiniGrid
            // 
            this.OrdiniGrid.AllowUserToAddRows = false;
            this.OrdiniGrid.AllowUserToDeleteRows = false;
            this.OrdiniGrid.AllowUserToResizeColumns = false;
            this.OrdiniGrid.AllowUserToResizeRows = false;
            this.OrdiniGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OrdiniGrid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.OrdiniGrid.BackgroundColor = System.Drawing.Color.Navy;
            this.OrdiniGrid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Navy;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Yellow;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.OrdiniGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.OrdiniGrid.ColumnHeadersHeight = 30;
            this.OrdiniGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Navy;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.OrdiniGrid.DefaultCellStyle = dataGridViewCellStyle2;
            this.OrdiniGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.OrdiniGrid.EnableHeadersVisualStyles = false;
            this.OrdiniGrid.GridColor = System.Drawing.SystemColors.ControlLight;
            this.OrdiniGrid.Location = new System.Drawing.Point(0, 0);
            this.OrdiniGrid.Margin = new System.Windows.Forms.Padding(20, 0, 20, 0);
            this.OrdiniGrid.MultiSelect = false;
            this.OrdiniGrid.Name = "OrdiniGrid";
            this.OrdiniGrid.ReadOnly = true;
            this.OrdiniGrid.RowHeadersVisible = false;
            this.OrdiniGrid.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.OrdiniGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.OrdiniGrid.Size = new System.Drawing.Size(594, 518);
            this.OrdiniGrid.TabIndex = 5;
            this.OrdiniGrid.Resize += new System.EventHandler(this.FormResize);
            // 
            // checkBox_nonevasi
            // 
            this.checkBox_nonevasi.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_nonevasi.AutoSize = true;
            this.checkBox_nonevasi.Checked = true;
            this.checkBox_nonevasi.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_nonevasi.Location = new System.Drawing.Point(278, 558);
            this.checkBox_nonevasi.Name = "checkBox_nonevasi";
            this.checkBox_nonevasi.Size = new System.Drawing.Size(156, 17);
            this.checkBox_nonevasi.TabIndex = 6;
            this.checkBox_nonevasi.Text = "mostra solo ordini non evasi";
            this.checkBox_nonevasi.UseVisualStyleBackColor = true;
            this.checkBox_nonevasi.Click += new System.EventHandler(this.checkBox_nonevasi_Click);
            // 
            // VisOrdiniFrm
            // 
            this.AcceptButton = this.OKBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(594, 596);
            this.Controls.Add(this.checkBox_nonevasi);
            this.Controls.Add(this.OrdiniGrid);
            this.Controls.Add(this.OKBtn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(600, 400);
            this.Name = "VisOrdiniFrm";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "Ordini emessi";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VisOrdiniFrm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.OrdiniGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.DataGridView OrdiniGrid;
        private System.Windows.Forms.CheckBox checkBox_nonevasi;
    }
}
namespace StandFacile
{
    partial class EsploraDB_Dlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EsploraDB_Dlg));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.topPanel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.EliminaBtn = new System.Windows.Forms.Button();
            this.OKBtn = new System.Windows.Forms.Button();
            this.RenameBtn = new System.Windows.Forms.Button();
            this.dbGrid = new System.Windows.Forms.DataGridView();
            this.topPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dbGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // topPanel
            // 
            this.topPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.topPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.topPanel.Controls.Add(this.label1);
            this.topPanel.Location = new System.Drawing.Point(11, 12);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(306, 38);
            this.topPanel.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(16, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(263, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "Con il doppio click si  visualizza il file selezionato ";
            // 
            // EliminaBtn
            // 
            this.EliminaBtn.Image = ((System.Drawing.Image)(resources.GetObject("EliminaBtn.Image")));
            this.EliminaBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.EliminaBtn.Location = new System.Drawing.Point(58, 463);
            this.EliminaBtn.Name = "EliminaBtn";
            this.EliminaBtn.Size = new System.Drawing.Size(80, 33);
            this.EliminaBtn.TabIndex = 6;
            this.EliminaBtn.Text = "Elimina";
            this.EliminaBtn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.EliminaBtn.UseVisualStyleBackColor = true;
            this.EliminaBtn.Click += new System.EventHandler(this.EliminaBtn_Click);
            // 
            // OKBtn
            // 
            this.OKBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OKBtn.Image = global::StandFacile.Properties.Resources.OK;
            this.OKBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.OKBtn.Location = new System.Drawing.Point(186, 463);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(80, 33);
            this.OKBtn.TabIndex = 5;
            this.OKBtn.Text = "OK  ";
            this.OKBtn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.OKBtn.UseVisualStyleBackColor = true;
            // 
            // RenameBtn
            // 
            this.RenameBtn.Enabled = false;
            this.RenameBtn.Image = global::StandFacile.Properties.Resources.Rename;
            this.RenameBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.RenameBtn.Location = new System.Drawing.Point(199, 485);
            this.RenameBtn.Name = "RenameBtn";
            this.RenameBtn.Size = new System.Drawing.Size(91, 33);
            this.RenameBtn.TabIndex = 7;
            this.RenameBtn.Text = "Rinomina";
            this.RenameBtn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.RenameBtn.UseVisualStyleBackColor = true;
            this.RenameBtn.Visible = false;
            this.RenameBtn.Click += new System.EventHandler(this.RenameBtn_Click);
            // 
            // dbGrid
            // 
            this.dbGrid.AllowUserToAddRows = false;
            this.dbGrid.AllowUserToDeleteRows = false;
            this.dbGrid.AllowUserToResizeColumns = false;
            this.dbGrid.AllowUserToResizeRows = false;
            this.dbGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dbGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dbGrid.BackgroundColor = System.Drawing.Color.Teal;
            this.dbGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Teal;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dbGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dbGrid.ColumnHeadersHeight = 26;
            this.dbGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dbGrid.ColumnHeadersVisible = false;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Teal;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Courier New", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dbGrid.DefaultCellStyle = dataGridViewCellStyle2;
            this.dbGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dbGrid.EnableHeadersVisualStyles = false;
            this.dbGrid.GridColor = System.Drawing.Color.Gainsboro;
            this.dbGrid.Location = new System.Drawing.Point(11, 67);
            this.dbGrid.Name = "dbGrid";
            this.dbGrid.RowHeadersVisible = false;
            this.dbGrid.RowHeadersWidth = 10;
            this.dbGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dbGrid.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dbGrid.ShowCellToolTips = false;
            this.dbGrid.Size = new System.Drawing.Size(306, 383);
            this.dbGrid.TabIndex = 8;
            this.dbGrid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dbGrid_CellEnter);
            this.dbGrid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dbGrid_CellDoubleClick);
            this.dbGrid.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dbGrid_CellEnter);
            this.dbGrid.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dbGrid_CellEnter);
            this.dbGrid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dbGrid_KeyDown);
            // 
            // EsploraDB_Dlg
            // 
            this.AcceptButton = this.OKBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.OKBtn;
            this.ClientSize = new System.Drawing.Size(332, 508);
            this.Controls.Add(this.dbGrid);
            this.Controls.Add(this.RenameBtn);
            this.Controls.Add(this.EliminaBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.topPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EsploraDB_Dlg";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Esplora DB";
            this.Shown += new System.EventHandler(this.EsploraDB_Dlg_Shown);
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dbGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel topPanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button EliminaBtn;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button RenameBtn;
        private System.Windows.Forms.DataGridView dbGrid;
    }
}
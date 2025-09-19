/************************************************************
    NomeFile : StandCommonSrc/FrmUpdateProgress.cs, versione utente
    Data	 : 03.04.2024
    Autore	 : Nicola Bizzotto
 ************************************************************/

using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StandFacile
{
    /// <summary>  
    /// Represents a form that displays a progress message during an update operation.  
    /// </summary>  
    public partial class FrmUpdateProgress : Form
    {
        private Action action;

        /// </summary>  
        public FrmUpdateProgress(string title, string message, Action action)
        {
            InitializeComponent();
            this.Text = title;
            this.labelMessage.Text = message;
            this.action = action;
        }

        /// <summary>  
        /// Configures the components of the form.  
        /// </summary>  
        private void InitializeComponent()
        {
            this.labelMessage = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // labelMessage
            // 
            this.labelMessage.AutoSize = true;
            this.labelMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMessage.Location = new System.Drawing.Point(12, 9);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(199, 18);
            this.labelMessage.TabIndex = 0;
            this.labelMessage.Text = "Messaggio di aggiornamento";
            this.labelMessage.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(16, 38);
            this.progressBar1.MarqueeAnimationSpeed = 30;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(346, 23);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar1.TabIndex = 1;
            // 
            // FrmUpdateProgress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 73);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.labelMessage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmUpdateProgress";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Aggiornamento";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        /// <summary>  
        /// Handles the OnShown event to start the action when the form is displayed.  
        /// </summary>  
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            Task.Run(() =>
            {
                this.action?.Invoke();
                this.Invoke((Action)(() => this.Close()));
            });
        }

        private ProgressBar progressBar1;
        private System.Windows.Forms.Label labelMessage;
    }
} 


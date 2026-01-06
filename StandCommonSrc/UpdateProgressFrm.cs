/************************************************************
    NomeFile : StandCommonSrc/UpdateProgressFrm.cs
    Data	 : 05.01.2026
    Autore	 : nicola02nb
 ************************************************************/

using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using static StandCommonFiles.LogServer;

namespace StandFacile
{
    /// <summary>Rappresenta una finestra che mostra un messaggio di avanzamento durante un'operazione di aggiornamento.</summary>  
    public partial class FrmUpdateProgress : Form
    {
        private readonly Action action;

        /// <summary>Inizializza una nuova istanza della classe con il titolo, il messaggio e l'azione da eseguire.</summary>
        public FrmUpdateProgress(string title, string message, Action action)
        {
            InitializeComponent();
            this.Text = title;
            this.labelMessage.Text = message;
            this.action = action;
        }

        /// <summary>Configura i componenti della finestra.</summary>  
        private void InitializeComponent()
        {
            this.labelMessage = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // labelMessage
            // 
            this.labelMessage.AutoSize = true;
            this.labelMessage.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMessage.Location = new System.Drawing.Point(12, 9);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(193, 18);
            this.labelMessage.TabIndex = 0;
            this.labelMessage.Text = "Messaggio di aggiornamento";
            this.labelMessage.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(16, 41);
            this.progressBar1.MarqueeAnimationSpeed = 30;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(452, 23);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar1.TabIndex = 1;
            // 
            // FrmUpdateProgress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(480, 85);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.labelMessage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmUpdateProgress";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Aggiornamento ...";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        /// <summary>Gestisce l'evento OnShown per avviare l'azione quando la finestra viene visualizzata.</summary>  
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            Task.Run(() =>
            {
                this.action?.Invoke();
                this.Invoke((Action)(() => this.Close()));
            });

            LogToFile("FrmUpdateProgress");
        }

        private ProgressBar progressBar1;
        private System.Windows.Forms.Label labelMessage;
    }
}

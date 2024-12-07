/***********************************************
  NomeFile : StandCucina/LogForm.cs
  Data	   : 08.11.2023
  Autore   : Mauro Artuso

  Visualizza una finestra di Log dell'attività
 ************************************************/

using System;
using System.Windows.Forms;

namespace StandFacile
{
    /// <summary>
    /// finestra di Log attività
    /// </summary>
    public partial class LogForm : Form
    {
        /// <summary>riferimento a LogForm</summary>
        public static LogForm rLogForm;

        /// <summary>costruttore</summary>
        public LogForm()
        {
            InitializeComponent();

            rLogForm = this;

            String sTmp;

            Width = 400;
            TB_Log.Clear();

            sTmp = "Start: " + DateTime.Now.ToString("HH.mm.ss");

            TB_Log.AppendText(sTmp + "\r\n");

            LogForm_Resize(this, null);
        }

        /// <summary>
        /// funzione che aggiunge una stringa al Log
        /// </summary>
        /// <param name="sLineOfText"></param>
        public void LogAddLine(String sLineOfText)
        {
            TB_Log.AppendText(sLineOfText + "\r\n");

            // scroll alla fine
            if (Visible)
            {
                TB_Log.Focus();

                TB_Log.ScrollBars = ScrollBars.Vertical;
                TB_Log.SelectionStart = TB_Log.Text.Length;
                TB_Log.ScrollToCaret();
            }
        }

        private void TB_Log_VisibleChanged(object sender, EventArgs e)
        {
            Height = FrmMain.rFrmMain.Height;
        }

        private void LogForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            // evita la distruzione della form creando una eccezione alla successiva apertura
            e.Cancel = true;
        }

        private void LogForm_Resize(object sender, EventArgs e)
        {
            TB_Log.Height = rLogForm.Height - 72;
        }
    }
}

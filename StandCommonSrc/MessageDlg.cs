/*****************************************************
	NomeFile : StandCommonSrc/MessageDlg.cs
    Data	 : 06.12.2024
	Autore	 : Mauro Artuso

 *****************************************************/

using System;
using System.Windows.Forms;

using static StandFacile.NetConfigLightDlg;

namespace StandFacile
{
    /// <summary>
    /// finestra di visualizzazione di un Messaggio
    /// con scadenza temporizzata
    /// </summary>
    public partial class MessageDlg : Form
    {
        int _iTimer;
        String sTmp;

        /// <summary>riferimento a MessageDlg</summary>
        public static MessageDlg rMessageDlg;

        /// <summary>costruttore</summary>
        public MessageDlg()
        {
            InitializeComponent();

            rMessageDlg = this;
        }

        /// <summary>
        /// visualizzazione del messaggio
        /// </summary>
        /// <param name="sMsg"></param>
        /// <param name="sTitle"></param>
        /// <param name="iTimerParam"></param>
        public void MessageBox(String sMsg, String sTitle, int iTimerParam)
        {
            Text = sTitle;

            // correzione ritorni di linea
            if (!sMsg.Contains("\r"))
                sMsg = sMsg.Replace("\n", "\r\n");

            MemoBox.Text = sMsg;

            _iTimer = iTimerParam;
            MsgTimer.Enabled = true;

            sTmp = String.Format("chiude in {0}s", _iTimer);
            timeLbl.Text = sTmp;

            Show(); // no ShowDialog !!!
        }

        private void MsgTimer_Tick(object sender, EventArgs e)
        {
            sTmp = String.Format("chiude in {0}s", _iTimer);
            timeLbl.Text = sTmp;
            Focus();

            if (_iTimer == 0)
            {
                Hide();
                MsgTimer.Enabled = false;
            }
            else
                _iTimer--;
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void MessageDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            MsgTimer.Enabled = false;

            // evita la distruzione della form creando una eccezione alla successiva apertura
            e.Cancel = true;
        }
    }
}

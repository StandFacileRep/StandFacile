/***************************************************
	 NomeFile : StandFacile/ChooseDispDlg.cs
	 Data	  : 23.11.2020
	 Autore   : Mauro Artuso
 ***************************************************/

using System;
using System.Windows.Forms;

using static StandCommonFiles.LogServer;

using static StandCommonFiles.ComDef;
using static StandCommonFiles.commonCl;

namespace StandFacile
{
    /// <summary> classe per la scelta di avvio </summary>
    public partial class startDispDlg : Form
    {
        int _iDispMngStatus;
        DateTime _dateFromDB;

        /// <summary>costruttore dialogo scelta disponibilità</summary>
        public startDispDlg(DateTime actualDateParam, DateTime dateFromDBParam)
        {
            InitializeComponent();

            _dateFromDB = dateFromDBParam;

            lblMessage1.Text = String.Format("PC Locale: {0}", actualDateParam.ToString("dd/MM/yy"));
            lblMessage2.Text = String.Format("Database:  {0}", dateFromDBParam.ToString("dd/MM/yy"));

            ShowDialog();
        }

        private void btnFullDisp_Click(object sender, EventArgs e)
        {
            String sTmp;

            if (ckBoxNoMoreView.Checked)
            {
                // non visualizza e non recupera disponibilità
                _iDispMngStatus = 0;
                LogToFile("startDispDlg: ckBoxNoMore");
            }
            else
            {
                // visualizza, non recupera disponibilità
                _iDispMngStatus = SetBit(0, BIT_SHOW_DISP_DLG);
            }

            WriteRegistry(DISP_DLG_MNG_KEY, _iDispMngStatus);

            sTmp = String.Format("startDispDlg : btnOK, DispMngStatus = {0}", _iDispMngStatus);
            LogToFile(sTmp);

            Close();
        }

        private void SD_btnEditDisp_Click(object sender, EventArgs e)
        {
            String sTmp;

            if (ckBoxNoMoreView.Checked)
            {
                // non visualizza, recupera disponibilità
                _iDispMngStatus = SetBit(0, BIT_PREV_DISP_LOAD);
                LogToFile("startDispDlg: ckBoxNoMore");
            }
            else
            {
                // visualizza, recuperare la disponibilità non ha senso poichè appare il Dialogo e l'utente sceglie
                _iDispMngStatus = SetBit(0, BIT_SHOW_DISP_DLG);
            }

            WriteRegistry(DISP_DLG_MNG_KEY, _iDispMngStatus);

            sTmp = String.Format("startDispDlg : btnCancel, DispMngStatus = {0}", _iDispMngStatus);
            LogToFile(sTmp);

            Close();

            InitialDispDlg rInitDispDlg = new InitialDispDlg(_dateFromDB, true);
        }

    }
}

/********************************************************************
  NomeFile : StandFacile/StartDlg.cs
  Data	   : 23.06.2023
  Autore   : Mauro Artuso

  *******************************************************************/

using System;
using System.Windows.Forms;

using static StandCommonFiles.ComDef;
using static StandCommonFiles.CommonCl;
using static StandCommonFiles.LogServer;
using static StandCommonFiles.Printer_Legacy;

using static StandFacile.glb;
using static StandFacile.Define;
using static StandFacile.dBaseIntf;

namespace StandFacile
{
    /// <summary>
    /// classe di avvio con informazioni
    /// </summary>
    public partial class startDlg : Form
    {
        /// <summary>costruttore</summary>
        public startDlg()
        {
            InitializeComponent();

            int iPrinterType;
            String sDataStr, sTmp, sCassa;

            sDataStr = GetActualDate().ToString("dddd  dd/MM/yy");
            sTmp = sDataStr.ToUpper();
            sTmp = sTmp.Substring(0, 1);

            sDataStr = sDataStr.Substring(1);
            sDataStr = sTmp + sDataStr;

            DateLbl.Text = "Data : " + sDataStr;
            OraLbl.Text = "Ora : " + DateTime.Now.ToString("HH.mm.ss");

            LblTitle.Text = String.Format("{0}, {1} {2}", TITLE, RELEASE_SW, RELEASE_TBL);

            LinkLbl_Web.Text = URL_SITO;
            sCassa = sConstCassaType[SF_Data.iNumCassa - 1];

            if (iUSA_NDB() == (int)DB_MODE.MYSQL)
                Lbl_DB.Text = String.Format("MySQL server : {0}, {1}", GetDB_ServerName(), sCassa);
            else if (iUSA_NDB() == (int)DB_MODE.POSTGRES)
                Lbl_DB.Text = String.Format("PostgreSQL server : {0}, {1}", GetDB_ServerName(), sCassa);
            else
                Lbl_DB.Text = String.Format("DB_SQLite, {0}", sCassa);

            //lettura stampante windows o Legacy
            iPrinterType = ReadRegistry(SYS_PRINTER_TYPE_KEY, (int)PRINTER_SEL.STAMPANTE_WINDOWS);

            if (iPrinterType == (int)PRINTER_SEL.STAMPANTE_WINDOWS)
                sTmp = String.Format("Stampante windows : {0}", sGlbWinPrinterParams.sTckPrinterModel);
            else
                sTmp = String.Format("Stampante legacy : {0}, {1}, {2}", sConstLegacyModels[sGlbLegacyPrinterParams.iPrinterModel],
                    sGlbLegacyPrinterParams.sPort, sConstFlow[sGlbLegacyPrinterParams.iFlowCtrl]);

            LblPrinter.Text = sTmp;

            Lbl_Listino.Text = "data Listino : " + SF_Data.sListinoDateTime;

            if (CheckService(_SKIP_DATA))
                timer.Enabled = false;
            else
            {
                timer.Enabled = true;
                if (!bUSA_NDB())
                {
                    textBox.ResetText();
                    textBox.AppendText("\r\n   Verificare che la data e l'ora del PC siano corrette !");
                }

                ShowDialog();
            }

            LogToFile("StartDlg : Start Program");
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            OraLbl.Text = "Ora : " + DateTime.Now.ToString("HH.mm.ss");
        }

        private void LinkLbl_Web_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.LinkLbl_Web.LinkVisited = true;

            System.Diagnostics.Process.Start(URL_SITO);
        }

        private void OkBtn_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;
        }
    }
}

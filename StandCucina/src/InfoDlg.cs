/***********************************************
  	NomeFile : StandCucina/InfoDlg.cs
	Data	 : 06.12.2024
  	Autore   : Mauro Artuso
 ************************************************/

using System;
using System.Reflection;
using System.Windows.Forms;

using static StandFacile.glb;
using static StandCommonFiles.ComDef;
using static StandCommonFiles.Printer_Legacy;

namespace StandFacile
{
    /// <summary>
    ///  Visualizza Info sul Programma ed autore
    /// </summary>
    public partial class InfoDlg : Form
    {
        static String sDllVersion;
        readonly Assembly dllAssembly;

        /// <summary>costruttore</summary>
        public InfoDlg()
        {
            InitializeComponent();
            
            lblTitolo.Text = Define.TITLE;
            lblRel.Text = RELEASE_SW;
            LinkLbl_Web.Text = URL_SITO;
            LinkLbl_mail.Text = MAIL;

            Lbl_DB.Text = String.Format("DB server : {0}", dBaseIntf.GetDB_ServerName());

            switch (dBaseIntf.iUSA_NDB())
            {
                case (int)DB_MODE.MYSQL:
                    dllAssembly = Assembly.LoadFrom(DB_CONNECTOR_DLL_MY);
                    sDllVersion = dllAssembly.GetName().Version.ToString();
                    lblDB.Text = String.Format("Database MySQL, versione dll : {0}", sDllVersion);
                    break;
                case (int)DB_MODE.POSTGRES:
                    dllAssembly = Assembly.LoadFrom(DB_CONNECTOR_DLL_PG);
                    sDllVersion = dllAssembly.GetName().Version.ToString();
                    lblDB.Text = String.Format("Database PostgreSQL, versione dll : {0}", sDllVersion);
                    break;
                default:
                    lblDB.Text = "dll: nessun db in uso";
                    break;
            }

            if (PrintConfigLightDlg.GetPrinterTypeIsWinwows())
                LblPrinter.Text = String.Format("Stampante windows : {0}", sGlbWinPrinterParams.sTckPrinterModel);
            else
                LblPrinter.Text = String.Format("Stampante legacy : {0}, {1}, {2}", sConstLegacyModels[sGlbLegacyPrinterParams.iPrinterModel],
                    sGlbLegacyPrinterParams.sPort, sConstFlow[sGlbLegacyPrinterParams.iFlowCtrl]);
        }

        private void LinkLbl_Web_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.LinkLbl_Web.LinkVisited = true;

            System.Diagnostics.Process.Start(URL_SITO);
        }

        private void ImageLic_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.gnu.org/licenses");
        }

        private void LinkLbl_mail_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("mailto:" + MAIL);
        }
    }
}

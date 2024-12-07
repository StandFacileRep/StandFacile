/***********************************************
  NomeFile : StandFacile/InfoDlg.cs
  Data	   : 23.06.2023
  Autore   : Mauro Artuso

  Visualizza Info sul Programma ed autore
 ************************************************/

using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

using static StandCommonFiles.ComDef;
using static StandCommonFiles.Printer_Legacy;

using static StandFacile.glb;

namespace StandFacile
{
    /// <summary>form per la visualizzazione di informazioni utili</summary>
    public partial class InfoDlg : Form
    {

        static String sDir, sCassa, sDllVersion;
        readonly Assembly dllAssembly;

        /// <summary>costruttore</summary>
        public InfoDlg()
        {
            InitializeComponent();


            lblTitolo.Text = Define.TITLE;
            lblSW_Rel.Text = String.Format("versione = {0}", RELEASE_SW);
            lblDB_Rel.Text = String.Format("db tables = {0}", RELEASE_TBL);
            LinkLbl_Web.Text = URL_SITO;
            LinkLbl_mail.Text = MAIL;

            sCassa = sConstCassaType[SF_Data.iNumCassa - 1];

            sDir = DataManager.sGetExeDir() + "\\";

            if (dBaseIntf._bUSA_NDB())
                Lbl_DB.Text = String.Format("DB server : {0}, tipo di cassa : {1}", dBaseIntf.sGetDB_ServerName(), sCassa);
            else
                Lbl_DB.Text = String.Format("Tipo di cassa : {0}", sCassa);

            switch (dBaseIntf._iUSA_NDB())
            {
                case (int)DB_MODE.SQLITE:
                    dllAssembly = Assembly.LoadFrom(sDir + DB_CONNECTOR_DLL_QL);
                    sDllVersion = dllAssembly.GetName().Version.ToString();
                    lbl_Ver.Text = String.Format("Database SQLite, versione dll : {0}", sDllVersion);
                    break;
                case (int)DB_MODE.MYSQL:
                    dllAssembly = Assembly.LoadFrom(sDir + DB_CONNECTOR_DLL_MY);
                    sDllVersion = dllAssembly.GetName().Version.ToString();
                    lbl_Ver.Text = String.Format("Database MySQL, versione dll : {0}", sDllVersion);
                    break;
                case (int)DB_MODE.POSTGRES:
                    dllAssembly = Assembly.LoadFrom(sDir + DB_CONNECTOR_DLL_PG);
                    sDllVersion = dllAssembly.GetName().Version.ToString();
                    lbl_Ver.Text = String.Format("Database PostgreSQL, versione dll : {0}", sDllVersion);
                    break;
                default:
                    lbl_Ver.Text = "dll: nessun db in uso";
                    break;
            }

            if (iSysPrinterType == (int)PRINTER_SEL.STAMPANTE_WINDOWS)
                lbl_Printer.Text = String.Format("Stampante windows : {0}", sGlbWinPrinterParams.sTckPrinterModel);
            else
                lbl_Printer.Text = String.Format("Stampante legacy : {0}, {1}, {2}", sConstLegacyModels[sGlbLegacyPrinterParams.iPrinterModel],
                    sGlbLegacyPrinterParams.sPort, sConstFlow[sGlbLegacyPrinterParams.iFlowCtrl]);
        }

        private void LinkLbl_Web_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.LinkLbl_Web.LinkVisited = true;

            System.Diagnostics.Process.Start(URL_SITO);
        }

        private void ImageDona_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.standfacile.org/");
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

/***********************************************
  NomeFile : StandOrdini/InfoDlg.cs
  Data	   : 23.11.2023
  Autore   : Mauro Artuso

  Visualizza Info sul Programma ed autore
 ************************************************/

using System;
using System.Reflection;
using System.Windows.Forms;

using static StandCommonFiles.ComDef;

namespace StandFacile
{
    /// <summary>
    /// form per la visualizzazione di informazioni utili
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

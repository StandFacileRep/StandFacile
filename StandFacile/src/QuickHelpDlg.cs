/*************************************************
	NomeFile : StandFacile/QuickHelpDlg.cs
	Data	 : 23.11.2023
	Autore	 : Mauro Artuso
 
 *************************************************/
using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

using static StandCommonFiles.ComDef;
using static StandCommonFiles.commonCl;

using static StandFacile.Define;


namespace StandFacile
{
    /// <summary>Piccolo Help</summary>
    public partial class QuickHelpDlg : Form
    {
        /// <summary>costruttore</summary>
        public QuickHelpDlg(String sStringsParam = "", int iHSizeParam = 0, int iVSizeParam = 0)
        {
            InitializeComponent();

            if (iHSizeParam > 0)
            {
                Size = new Size(iHSizeParam, iVSizeParam);
                textBox.Size = new Size(iHSizeParam - 2 * textBox.Location.X - 20, iVSizeParam - 140);
            }

            if (!String.IsNullOrEmpty(sStringsParam))
                textBox.Text = sStringsParam;

            ShowDialog();
        }

        private void ManBtn_Click(object sender, EventArgs e)
        {
            String sDir, sNomeDoc;
            TErrMsg WrnMsg = new TErrMsg();

            // prende il manuale dalla Directory della documentazione
            sNomeDoc = "..\\..\\doc\\" + _NOME_MANUALE;

            sDir = DataManager.sGetExeDir() + "\\";

            if (File.Exists(sDir + _NOME_MANUALE))
                System.Diagnostics.Process.Start(sDir + _NOME_MANUALE);
            else if (File.Exists(sDir + sNomeDoc))
                System.Diagnostics.Process.Start(sDir + sNomeDoc);
            else
            {
                WrnMsg.sNomeFile = _NOME_MANUALE;
                WrnMsg.iErrID = WRN_FNF;
                WarningManager(WrnMsg);
            }
        }

        private void QuickHelpDlg_KeyPress(object sender, KeyPressEventArgs e)
        {
            int iKey = (int)e.KeyChar;

            if (iKey == KEY_ESC)
                DialogResult = DialogResult.Cancel;
        }
    }
}

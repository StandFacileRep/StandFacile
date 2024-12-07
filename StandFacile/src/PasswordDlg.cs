/***************************************************************************
	 NomeFile : StandFacile/PasswordDlg.cs
	 Data	  : 03.08.2023
	 Autore	  : Mauro Artuso
	 
 ***************************************************************************/

using System;
using System.Drawing;
using System.Windows.Forms;

using static StandCommonFiles.ComDef;
using static StandCommonFiles.commonCl;
using static StandCommonFiles.LogServer;

namespace StandFacile
{
    /// <summary>
    /// classe per l'impostazione della password
    /// </summary>

    public partial class PasswordDlg : Form
    {
        /// <summary>riferimento a PasswordDlg</summary>
        public static PasswordDlg _rPasswordDlg;

        private static bool _bCheckOnly;
        private readonly string sDEFAULT_PWD = "61CqIdmESD2LzbYXvF/MfQ==";

        readonly ToolTip _tt = new ToolTip
        {
            InitialDelay = 50,
            ShowAlways = true
        };

        /// <summary>costruttore: se bCheckOnlyParam verifica solamente la password,<br/> altrimenti la imposta</summary>
        public PasswordDlg(bool bCheckOnlyParam)
        {
            int OFFSET = 60;

            InitializeComponent();

            _bCheckOnly = bCheckOnlyParam;

            if (_bCheckOnly)
            {
                _tt.SetToolTip(OKBtn, "si abilita se la password corrisponde");
                textBox_PWD.Text = dBaseTunnel_my.Decrypt_WS(sReadRegistry(SET_ACCESS_KEY, sDEFAULT_PWD));

                Text = "Inserimento password";
                textBox_PWD.Visible = false;
                labelRIP.Visible = false;

                textBox_VER.Top -= OFFSET;
                OKBtn.Top -= OFFSET;
                btnCancel.Top -= OFFSET;
                Height -= OFFSET;
            }
            else
            {
                _tt.SetToolTip(OKBtn, "si abilita se le password corrispondono");

                textBox_PWD.Visible = true;
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (textBox_PWD.Focused && (textBox_PWD.Text.Length > 0) && (textBox_PWD.Text.Length < 6))
            {
                textBox_PWD.BackColor = Color.Red;
                textBox_PWD.ForeColor = SystemColors.HighlightText;
                OKBtn.Enabled = false;
            }
            else
            {
                textBox_PWD.BackColor = SystemColors.Window;
                textBox_PWD.ForeColor = SystemColors.WindowText;
            }

            if (textBox_VER.Focused && (textBox_VER.Text.Length > 0) && (textBox_VER.Text.Length < 6))
            {
                textBox_VER.BackColor = Color.Red;
                textBox_VER.ForeColor = SystemColors.HighlightText;
                OKBtn.Enabled = false;
            }
            else
            {
                textBox_VER.BackColor = SystemColors.Window;
                textBox_VER.ForeColor = SystemColors.WindowText;
            }

            if (((textBox_PWD.Text == textBox_VER.Text) || (dBaseTunnel_my.Decrypt_WS(sDEFAULT_PWD) == textBox_VER.Text)) && (textBox_PWD.Text.Length >= 6) && (textBox_VER.Text.Length >= 6))
            {
                OKBtn.Enabled = true;
                FrmMain.bSetPasswordIsGood(true);
            }
            else
            {
                OKBtn.Enabled = false;
            }
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            WriteRegistry(SET_ACCESS_KEY, dBaseTunnel_my.Encrypt_WS(textBox_PWD.Text));
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

/*****************************************************
	NomeFile : StandFacile/RenameDlg.cs
	Data	 : 20.10.2024
	Autore	 : Mauro Artuso

 *****************************************************/
using System;
using System.Windows.Forms;

namespace StandFacile
{
    /// <summary>
    /// classe che rinomina i files dati per chiudere il conteggio
    /// </summary>
    public partial class RenameDlg : Form
    {
        /// <summary>ottiene il suffisso</summary>
        public String sGetEdit()
        {
            return textBox.Text;
        }

        /// <summary>costruttore</summary>
        public RenameDlg()
        {
            InitializeComponent();
        }

        /// <summary>Init</summary>
        public void Init(String sAvvisoParam, String sEditParam)
        {
            RD_textBox.Text = sAvvisoParam;
            textBox.Text = sEditParam;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            String sTmp;

            sTmp = textBox.Text.Trim();

            if (String.IsNullOrEmpty(sTmp))
                DialogResult = DialogResult.Cancel;
            else
                DialogResult = DialogResult.OK;
        }

        // non accetta caratteri diversi da lettere o numeri
        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsLetterOrDigit(e.KeyChar) || (e.KeyChar == '_')))
                e.Handled = true;
        }
    }
}

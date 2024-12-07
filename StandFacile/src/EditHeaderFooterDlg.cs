/***********************************************
  NomeFile : StandFacile/EditHeaderFooterDlg.cs
  Data	   : 23.06.2023
  Autore   : Mauro Artuso
 **********************************************/

using System;
using System.Windows.Forms;

using static StandFacile.glb;
using static StandFacile.Define;

using static StandCommonFiles.ComDef;
using static StandCommonFiles.commonCl;
using static StandCommonFiles.LogServer;

namespace StandFacile
{
    /// <summary>classe per l'impostazione di intestazione e piè di pagina</summary>
    public partial class EditHeaderFooterDlg : Form
    {
        static bool _bListinoModificato;

        /// <summary>ottiene flag di listino modificato</summary>
        public static bool bGetListinoModificato()
        {
            return _bListinoModificato;
        }

        /// <summary>costruttore</summary>
        public EditHeaderFooterDlg()
        {
            InitializeComponent();

            _bListinoModificato = false;

            IntestazioneEdit1.MaxLength = iMAX_RECEIPT_CHARS;
            IntestazioneEdit2.MaxLength = iMAX_RECEIPT_CHARS;
            PiePaginaEdit1.MaxLength = iMAX_RECEIPT_CHARS;
            PiePaginaEdit2.MaxLength = iMAX_RECEIPT_CHARS;

            IntestazioneEdit1.Text = sCenterJustify(SF_Data.sHeaders[0], iMAX_RECEIPT_CHARS);
            IntestazioneEdit2.Text = sCenterJustify(SF_Data.sHeaders[1], iMAX_RECEIPT_CHARS);
            PiePaginaEdit1.Text = sCenterJustify(SF_Data.sHeaders[2], iMAX_RECEIPT_CHARS);
            PiePaginaEdit2.Text = sCenterJustify(SF_Data.sHeaders[3], iMAX_RECEIPT_CHARS);

            Timer.Enabled = true;

            ShowDialog();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if ((SF_Data.sHeaders[0] != IntestazioneEdit1.Text) ||
                (SF_Data.sHeaders[1] != IntestazioneEdit2.Text) ||
                (SF_Data.sHeaders[2] != PiePaginaEdit1.Text)    ||
                (SF_Data.sHeaders[3] != PiePaginaEdit2.Text))
            {
                _bListinoModificato = true;

                SF_Data.sHeaders[0] = sCenterJustify(IntestazioneEdit1.Text, iMAX_RECEIPT_CHARS);
                SF_Data.sHeaders[1] = sCenterJustify(IntestazioneEdit2.Text, iMAX_RECEIPT_CHARS);
                SF_Data.sHeaders[2] = sCenterJustify(PiePaginaEdit1.Text, iMAX_RECEIPT_CHARS);
                SF_Data.sHeaders[3] = sCenterJustify(PiePaginaEdit2.Text, iMAX_RECEIPT_CHARS);

            }
            else
                _bListinoModificato = false;

            LogToFile("HeadFoot btnOK");
            Close();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(IntestazioneEdit1.Text))
                BtnCanc0.Enabled = false;
            else
                BtnCanc0.Enabled = true;

            if (String.IsNullOrEmpty(IntestazioneEdit2.Text))
                BtnCanc1.Enabled = false;
            else
                BtnCanc1.Enabled = true;

            if (String.IsNullOrEmpty(PiePaginaEdit1.Text))
                BtnCanc2.Enabled = false;
            else
                BtnCanc2.Enabled = true;

            if (String.IsNullOrEmpty(PiePaginaEdit2.Text))
                BtnCanc3.Enabled = false;
            else
                BtnCanc3.Enabled = true;
        }

        private void BtnCanc0_Click(object sender, EventArgs e)
        {
            IntestazioneEdit1.Text = "";
        }

        private void BtnCanc1_Click(object sender, EventArgs e)
        {
            IntestazioneEdit2.Text = "";
        }

        private void BtnCanc2_Click(object sender, EventArgs e)
        {
            PiePaginaEdit1.Text = "";
        }

        private void BtnCanc3_Click(object sender, EventArgs e)
        {
            PiePaginaEdit2.Text = URL_SITO;
        }
    }
}

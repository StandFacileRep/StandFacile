/***********************************************
  	NomeFile : StandFacile/EditHeaderFooterDlg.cs
	Data	 : 06.12.2024
  	Autore   : Mauro Artuso
 **********************************************/

using System;
using System.Drawing;
using System.Windows.Forms;

using static StandFacile.glb;
using static StandFacile.Define;

using static StandCommonFiles.ComDef;
using static StandCommonFiles.CommonCl;
using static StandCommonFiles.LogServer;

namespace StandFacile
{
    /// <summary>classe per l'impostazione di intestazione e piè di pagina</summary>
    public partial class EditHeaderFooterDlg : Form
    {
        static bool _bListinoModificato;

        /// <summary>ottiene flag di listino modificato</summary>
        public static bool GetListinoModificato()
        {
            return _bListinoModificato;
        }

        /// <summary>costruttore</summary>
        public EditHeaderFooterDlg()
        {
            float fFontSize;

            InitializeComponent();

            _bListinoModificato = false;

            IntestazioneEdit1.MaxLength = iMAX_RECEIPT_CHARS;
            IntestazioneEdit2.MaxLength = iMAX_RECEIPT_CHARS;
            PiePaginaEdit1.MaxLength = iMAX_RECEIPT_CHARS;
            PiePaginaEdit2.MaxLength = iMAX_RECEIPT_CHARS;

            bool bChars33 = IsBitSet(SF_Data.iGenericPrintOptions, (int)GEN_PRINTER_OPTS.BIT_CHARS33_PRINT_REQUIRED);

            fFontSize = bChars33 ? 11.0f : 12.0f;

            IntestazioneEdit1.Font = new Font("Tahoma", fFontSize);
            IntestazioneEdit2.Font = new Font("Tahoma", fFontSize);
            PiePaginaEdit1.Font = new Font("Tahoma", fFontSize);
            PiePaginaEdit2.Font = new Font("Tahoma", fFontSize);

            IntestazioneEdit1.Text = CenterJustify(SF_Data.sHeaders[0], iMAX_RECEIPT_CHARS);
            IntestazioneEdit2.Text = CenterJustify(SF_Data.sHeaders[1], iMAX_RECEIPT_CHARS);
            PiePaginaEdit1.Text = CenterJustify(SF_Data.sHeaders[2], iMAX_RECEIPT_CHARS);
            PiePaginaEdit2.Text = CenterJustify(SF_Data.sHeaders[3], iMAX_RECEIPT_CHARS);

            Timer.Enabled = true;

            ShowDialog();
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            if ((SF_Data.sHeaders[0] != IntestazioneEdit1.Text) ||
                (SF_Data.sHeaders[1] != IntestazioneEdit2.Text) ||
                (SF_Data.sHeaders[2] != PiePaginaEdit1.Text)    ||
                (SF_Data.sHeaders[3] != PiePaginaEdit2.Text))
            {
                _bListinoModificato = true;

                SF_Data.sHeaders[0] = CenterJustify(IntestazioneEdit1.Text, iMAX_RECEIPT_CHARS);
                SF_Data.sHeaders[1] = CenterJustify(IntestazioneEdit2.Text, iMAX_RECEIPT_CHARS);
                SF_Data.sHeaders[2] = CenterJustify(PiePaginaEdit1.Text, iMAX_RECEIPT_CHARS);
                SF_Data.sHeaders[3] = CenterJustify(PiePaginaEdit2.Text, iMAX_RECEIPT_CHARS);

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

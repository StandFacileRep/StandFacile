/***********************************************
  	NomeFile : StandFacile/MainForm.cs
    Data	 : 10.09.2025
  	Autore   : Mauro Artuso
 ***********************************************/

using System;
using System.Windows.Forms;
using System.Drawing;

using static StandFacile.glb;
using static StandFacile.Define;
using static StandFacile.FrmMain;

using static StandCommonFiles.ComDef;
using static StandCommonFiles.CommonCl;

namespace StandFacile
{
    public partial class DataCheckDlg : Form
    {
        readonly ToolTip _tt = new ToolTip
        {
            InitialDelay = 50,
            ShowAlways = true
        };

        /// <summary>
        /// costruttore con parametri che consente verifica rapida di ciò che manca
        /// </summary>
        public DataCheckDlg(String EditTavoloParam, String EditCopertiParam, int comboCashIndexParam)
        {
            InitializeComponent();

            _tt.SetToolTip(btnCancel, "chiude il dialogo senza salvare nulla");
            _tt.SetToolTip(BtnPrt, "solo se ci sono tutti i dati richiesti,\r\navvia la stampa e chiude il dialogo");
            _tt.SetToolTip(btnOK, "solo se ci sono tutti i dati richiesti chiude il dialogo");

            EditTavolo.Text = EditTavoloParam.Trim();
            EditCoperti.Text = EditCopertiParam.Trim();

            switch (comboCashIndexParam)
            {
                case 0:
                    radioContantiBtn.Checked = true;
                    break;
                case 1:
                    radioCardBtn.Checked = true;
                    break;
                case 2:
                    radioSatispayBtn.Checked = true;
                    break;
            }

            if (IsBitSet(SF_Data.iGeneralProgOptions, (int)GEN_PROGRAM_OPTIONS.BIT_TABLE_REQUIRED))
            {
                lblTavolo.ForeColor = Color.Blue;
                lblTavolo.Font = new Font("Tahoma", 12); ;
            }

            if (IsBitSet(SF_Data.iGeneralProgOptions, (int)GEN_PROGRAM_OPTIONS.BIT_PLACE_SETTINGS_REQUIRED))
            {
                lblCoperti.ForeColor = Color.Blue;
                lblCoperti.Font = new Font("Tahoma", 12); ;
            }

            if (IsBitSet(SF_Data.iGeneralProgOptions, (int)GEN_PROGRAM_OPTIONS.BIT_PAYMENT_REQUIRED))
            {
                groupBox.ForeColor = Color.Blue;
                groupBox.Font = new Font("Tahoma", 12); ;
            }

            ShowDialog();
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            UpdateDataCheckParams();
        }

        private void radioBtn_Click(object sender, EventArgs e)
        {
            UpdateDataCheckParams();
        }

        /// <summary>accetta solo numeri o backspace</summary>
        private void EditFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || (e.KeyChar == '\b')))
                e.Handled = true;
        }

        /// <summary>
        ///  funzione che imposta tutti i parametri necessari a DataCheckDlg<br/>
        ///  prelevandoli dai controlli e non dal Registro
        /// </summary>       
        void UpdateDataCheckParams()
        {
            rFrmMain.SetEditTavolo(EditTavolo.Text.Trim());
            rFrmMain.SetEditCoperto(EditCoperti.Text.Trim());

            if (radioContantiBtn.Checked)
                rFrmMain.SetPagamento_CASH();

            else if (radioCardBtn.Checked)
                rFrmMain.SetPagamento_CARD();

            else if (radioSatispayBtn.Checked)
                rFrmMain.SetPagamento_SATISPAY();

            else
                rFrmMain.ResetPayment();
        }

        bool VerificaPOS_Richiesto()
        {
            return (radioContantiBtn.Checked || radioCardBtn.Checked || radioSatispayBtn.Checked);
        }

        private void BtnPrt_Click(object sender, EventArgs e)
        {
            // avvia la stampa dell'ordine
            String[] sQueue_Object = new String[2] { ORDER_PRINT_START, "" };

            UpdateDataCheckParams();

            if ((!IsBitSet(SF_Data.iGeneralProgOptions, (int)GEN_PROGRAM_OPTIONS.BIT_TABLE_REQUIRED) || !String.IsNullOrEmpty(EditTavolo.Text.Trim())) &&
                (!IsBitSet(SF_Data.iGeneralProgOptions, (int)GEN_PROGRAM_OPTIONS.BIT_PLACE_SETTINGS_REQUIRED) || (!String.IsNullOrEmpty(EditCoperti.Text.Trim()) && Convert.ToInt32(EditCoperti.Text) >= 0)) &&
                (!IsBitSet(SF_Data.iGeneralProgOptions, (int)GEN_PROGRAM_OPTIONS.BIT_PAYMENT_REQUIRED) || VerificaPOS_Richiesto()))
            {
                FrmMain.EventEnqueue(sQueue_Object);
                Close();
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            UpdateDataCheckParams();

            if ((!IsBitSet(SF_Data.iGeneralProgOptions, (int)GEN_PROGRAM_OPTIONS.BIT_TABLE_REQUIRED) || !String.IsNullOrEmpty(EditTavolo.Text.Trim())) &&
                (!IsBitSet(SF_Data.iGeneralProgOptions, (int)GEN_PROGRAM_OPTIONS.BIT_PLACE_SETTINGS_REQUIRED) || (!String.IsNullOrEmpty(EditCoperti.Text.Trim()) && Convert.ToInt32(EditCoperti.Text) >= 0)) &&
                (!IsBitSet(SF_Data.iGeneralProgOptions, (int)GEN_PROGRAM_OPTIONS.BIT_PAYMENT_REQUIRED) || VerificaPOS_Richiesto()))
            {
                Close();
            }
        }

    }
}

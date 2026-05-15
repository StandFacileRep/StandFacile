/***********************************************
  	NomeFile : StandFacile/MainForm.cs
    Data	 : 05.05.2026
  	Autore   : Mauro Artuso
 ***********************************************/

using System;
using System.Windows.Forms;
using System.Drawing;

using static StandFacile.glb;
using static StandFacile.Define;
using static StandFacile.FrmMain;
using static StandFacile.OptionsDlg;

using static StandCommonFiles.ComDef;
using static StandCommonFiles.CommonCl;

namespace StandFacile
{
    /// <summary>
    /// dialogo per la verifica dei dati obbligatori
    /// </summary>
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
        public DataCheckDlg(String EditCopertiParam, String EditTavoloParam, String EditName_param, int comboCashIndexParam)
        {
            InitializeComponent();

            _tt.SetToolTip(btnCancel, "chiude il dialogo senza salvare nulla");
            _tt.SetToolTip(BtnPrt, "solo se ci sono tutti i dati richiesti,\r\navvia la stampa e chiude il dialogo");
            _tt.SetToolTip(btnOK, "solo se ci sono tutti i dati richiesti chiude il dialogo");

            EditCoperti.Text = EditCopertiParam.Trim();
            EditTavolo.Text = EditTavoloParam.Trim();
            EditName.Text = EditName_param.Trim();

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

            if (GetPlaceSettings_MandatoryFlag())
            {
                lblCoperti.ForeColor = Color.Blue;
                lblCoperti.Font = new Font("Tahoma", 12);
            }

            if (GetTable_MandatoryFlag())
            {
                lblTavolo.ForeColor = Color.Blue;
                lblTavolo.Font = new Font("Tahoma", 12);
            }

            if (GetName_MandatoryFlag())
            {
                lblName.ForeColor = Color.Blue;
                lblName.Font = new Font("Tahoma", 12);
            }

            if (GetPayment_MandatoryFlag())
            {
                groupBox.ForeColor = Color.Blue;
                groupBox.Font = new Font("Tahoma", 12);
            }

            ShowDialog();
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            UpdateDataCheckParams();
        }

        private void RadioBtn_Click(object sender, EventArgs e)
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
            rFrmMain.SetEditCoperto(EditCoperti.Text.Trim());
            rFrmMain.SetEditTavolo(EditTavolo.Text.Trim());
            rFrmMain.SetEditName(EditName.Text.Trim());

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

            if ((!GetTable_MandatoryFlag() || !String.IsNullOrEmpty(EditTavolo.Text.Trim())) &&
                (!GetName_MandatoryFlag() || (EditName.Text.Trim().Length > 2)) &&
                (!GetPlaceSettings_MandatoryFlag() || (!String.IsNullOrEmpty(EditCoperti.Text.Trim()) && Convert.ToInt32(EditCoperti.Text) >= 0)) &&
                (!GetPayment_MandatoryFlag() || VerificaPOS_Richiesto()))
            {
                FrmMain.EventEnqueue(sQueue_Object);
                Close();
            }
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            UpdateDataCheckParams();

            if ((!GetTable_MandatoryFlag() || !String.IsNullOrEmpty(EditTavolo.Text.Trim())) &&
                (!GetName_MandatoryFlag() || (EditName.Text.Trim().Length > 2)) &&
                (!GetPlaceSettings_MandatoryFlag() || (!String.IsNullOrEmpty(EditCoperti.Text.Trim()) && Convert.ToInt32(EditCoperti.Text) >= 0)) &&
                (!GetPayment_MandatoryFlag() || VerificaPOS_Richiesto()))
            {
                Close();
            }
        }

    }
}

/**************************************************
	NomeFile : StandFacile/EditDispArticoliDlg.cs
	Data	 : 23.11.2023
	Autore	 : Mauro Artuso
 **************************************************/
using System;
using System.Windows.Forms;

using static StandCommonFiles.ComDef;
using static StandCommonFiles.commonCl;
using static StandCommonFiles.LogServer;

using static StandFacile.glb;

namespace StandFacile
{
    /// <summary>classe per l'immissione della disponibilità degli Articoli</summary>
    public partial class EditDispArticoliDlg : Form
    {
        static int _iPt;
        /// <summary>riferimento DispDlg</summary>
        public static EditDispArticoliDlg rDispDlg;

        /// <summary>costruttore</summary>
        public EditDispArticoliDlg()
        {
            InitializeComponent();
            rDispDlg = this;

            TipoEdit.MaxLength = iMAX_ART_CHAR;
        }

        /// <summary>
        /// si passa un indice dell'array fondamentale
        /// </summary>
        public void Init(int iPt)
        {
            int i;
            String sTmp;

            _iPt = iPt;

            TipoEdit.Text = SF_Data.Articolo[_iPt].sTipo;
            TipoEdit.Text = sCenterJustify(TipoEdit.Text, iMAX_RECEIPT_CHARS);

            i = SF_Data.Articolo[_iPt].iGruppoStampa;
            lblDescGruppo.Text = sConstGruppi[i].ToUpper();

            if (SF_Data.Articolo[_iPt].iDisponibilita == DISP_OK)
                DispEdit.Text = "OK";
            else
                DispEdit.Text = SF_Data.Articolo[_iPt].iDisponibilita.ToString();

            sTmp = String.Format("FrmDispDlg Init, articolo {0}", _iPt);
            LogToFile(sTmp);

            if (!Visible && !String.IsNullOrEmpty(TipoEdit.Text))
                ShowDialog();
        }

        /************************************************
          funzione di verifica della correttezza dei dati
          immessi, modifica direttamente l'array globale
         ************************************************/
        private void btnOK_Click(object sender, EventArgs e)
        {
            int iDisp;

            DispEdit.Text = DispEdit.Text.Trim();

            try
            {
                if (DispEdit.Text.ToUpper() == "OK")
                    iDisp = DISP_OK;
                else if (!String.IsNullOrEmpty(DispEdit.Text))
                    iDisp = Convert.ToInt32(DispEdit.Text); // conversione
                else
                    iDisp = -1; // campo vuoto
            }

            catch (Exception)
            {
                // Errore di conversione !
                WarningManager(WRN_ECQ);
                return;
            }

            if (iDisp == -1)
                WarningManager(WRN_CQZ); // Quantità nulla
            else  // se non ci sono errori uscita con chiusura Form
            {
                SF_Data.Articolo[_iPt].iDisponibilita = iDisp;

                DataManager.SalvaDati();
                Close();
            }
        }

        private void btnAzzera_Click(object sender, EventArgs e)
        {
            DispEdit.Text = "0";
        }

        private void btnRipristina_Click(object sender, EventArgs e)
        {
            DispEdit.Text = "OK";
        }

        private void DispDlg_Shown(object sender, EventArgs e)
        {
            DispEdit.Focus();
        }

        private void EditDispArticoliDlg_KeyDown(object sender, KeyEventArgs e)
        {
            int iKey;

            if (e == null)
                iKey = (int)KEY_NONE;
            else
                iKey = (int)e.KeyValue;

            switch (iKey)
            {
                case KEY_ESC:
                    Close();
                    break;

                default:
                    break;
            } // END switch
        }
        
    }
}

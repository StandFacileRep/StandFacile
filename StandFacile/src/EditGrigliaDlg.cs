/****************************************************************************
    NomeFile : StandFacile/ImpostaGrigliaDlg.cs
	Data	 : 23.05.2025
    Autore   : Mauro Artuso

    modo Touch:     3 4 5 	colonne
                    3 4 -	righe

    modo solo testo: 3  4  - colonne
                    16 20 25 righe

  Classe di scelta delle dimensioni di Griglia nella modalità Touch e non,
  se necessario si procede a compattare il vettore del listino
 ****************************************************************************/
using System;
using System.Windows.Forms;

using static StandFacile.glb;
using static StandFacile.Define;

using static StandCommonFiles.ComDef;
using static StandCommonFiles.CommonCl;
using static StandCommonFiles.LogServer;

namespace StandFacile
{
    /// <summary>
    /// classe di impostazione griglia
    /// </summary>
    public partial class EditGrigliaDlg : Form
    {
        readonly TextBox[] _pEdit;

        static bool _bListinoModificato;

        /// <summary>ottiene flag di modifica listino necessaria</summary>
        public static bool GetListinoModificato() { return _bListinoModificato; }

        /// <summary>costruttore</summary>
        public EditGrigliaDlg()
        {
            InitializeComponent();

            _pEdit = new TextBox[PAGES_NUM_TABM];

            _pEdit[0] = Edit_0;
            _pEdit[1] = Edit_1;
            _pEdit[2] = Edit_2;
            _pEdit[3] = Edit_3;
            _pEdit[4] = Edit_4;

            Edit_0.MaxLength = MAX_PAGES_CHAR;
            Edit_1.MaxLength = MAX_PAGES_CHAR;
            Edit_2.MaxLength = MAX_PAGES_CHAR;
            Edit_3.MaxLength = MAX_PAGES_CHAR;
            Edit_4.MaxLength = MAX_PAGES_CHAR;

            checkBoxTouchMode.Checked = IsBitSet(SF_Data.iGeneralOptions, (int)GEN_OPTS.BIT_TOUCH_MODE_REQUIRED);

            Edit_0.Text = SF_Data.sPageTabs[0];
            Edit_1.Text = SF_Data.sPageTabs[1];
            Edit_2.Text = SF_Data.sPageTabs[2];
            Edit_3.Text = SF_Data.sPageTabs[3];
            Edit_4.Text = SF_Data.sPageTabs[4];

            _bListinoModificato = false;

            CheckBoxTouchMode_CheckedChanged(this, null);
            LogToFile("ImpostaGriglia Init()");

            ShowDialog();
        }

        private void BtnCanc_0_Click(object sender, EventArgs e)
        {
            Edit_0.Text = "Pagina 1";
        }

        private void BtnCanc_1_Click(object sender, EventArgs e)
        {
            Edit_1.Text = "Pagina 2";
        }

        private void BtnCanc_2_Click(object sender, EventArgs e)
        {
            Edit_2.Text = "Pagina 3";
        }

        private void BtnCanc_3_Click(object sender, EventArgs e)
        {
            Edit_3.Text = "Pagina 4";
        }

        private void BtnCanc_4_Click(object sender, EventArgs e)
        {
            Edit_4.Text = "Pagina 5";
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            bool bModificaOK = false;

            int i, j, iPageNumTmp;
            int iNumColonneTmp, iNumRigheTmp;
            int iNumArticoli_NE;

            DialogResult dResult;

            // Articolo per compattazione
            TArticolo[] tmpArticolo = new TArticolo[MAX_NUM_ARTICOLI];

            if (checkBoxTouchMode.Checked)
            {
                if (radioRows1.Checked)
                    iNumRigheTmp = 3;
                else if (radioRows2.Checked)
                    iNumRigheTmp = 4;
                else if (radioRows3.Checked)
                    iNumRigheTmp = 4;
                else if (radioRows4.Checked)
                    iNumRigheTmp = 4;
                else
                    iNumRigheTmp = 3;

                if (radioCols1.Checked)
                    iNumColonneTmp = 3;
                else if (radioCols2.Checked)
                    iNumColonneTmp = 4;
                else if (radioCols3.Checked)
                    iNumColonneTmp = 5;
                else
                    iNumColonneTmp = 3;
            }
            else
            {
                if (radioRows1.Checked)
                    iNumRigheTmp = 10;
                else if (radioRows2.Checked)
                    iNumRigheTmp = 15;
                else if (radioRows3.Checked)
                    iNumRigheTmp = 20;
                else if (radioRows4.Checked)
                    iNumRigheTmp = 25;
                else
                    iNumRigheTmp = 16;

                if (radioCols1.Checked)
                    iNumColonneTmp = 3;
                else if (radioCols2.Checked)
                    iNumColonneTmp = 4;
                else if (radioCols3.Checked)
                    iNumColonneTmp = 4;
                else
                    iNumColonneTmp = 4;
            }

            for (i = 0; i < MAX_NUM_ARTICOLI; i++)
            {
                tmpArticolo[i].sTipo = "";
                tmpArticolo[i].iPrezzoUnitario = 0;
                tmpArticolo[i].iQuantita_Scaricata = 0;
                tmpArticolo[i].iQuantitaOrdine = 0;
                tmpArticolo[i].iIndexListino = 0;
                tmpArticolo[i].iGruppoStampa = 0;
                tmpArticolo[i].iQuantitaVenduta = 0;
                tmpArticolo[i].iQtaEsportata = 0;
                tmpArticolo[i].iDisponibilita = DISP_OK;
            }

            /**************************************************
                verifica necessità di eventuali compattazioni
             **************************************************/

            // verifica numero delle voci non nulle esclusa l'ultima
            iNumArticoli_NE = 0;

            for (i = 0; i < MAX_NUM_ARTICOLI - 1; i++) // COPERTI esclusi
                if (!String.IsNullOrEmpty(SF_Data.Articolo[i].sTipo))
                    iNumArticoli_NE++;

            if (checkBoxTouchMode.Checked)
                iPageNumTmp = PAGES_NUM_TABM;
            else
                iPageNumTmp = PAGES_NUM_TXTM;

            if (DataManager.CheckLastArticoloIndexP1() > (iNumColonneTmp * iNumRigheTmp * iPageNumTmp))
            {
                // compattazione necessaria

                if (iNumArticoli_NE <= (iNumColonneTmp * iNumRigheTmp * iPageNumTmp)) // compattazione possibile
                {
                    Hide();

                    dResult = MessageBox.Show("E' necessario compattare il vettore Articolo[] !", "Attenzione !", MessageBoxButtons.YesNo);

                    if (dResult == DialogResult.Yes)
                    {
                        j = 0;

                        for (i = 0; (i < MAX_NUM_ARTICOLI - 1); i++) // COPERTI esclusi
                            if (!(String.IsNullOrEmpty(SF_Data.Articolo[i].sTipo)))
                            {
                                tmpArticolo[j] = SF_Data.Articolo[i];
                                j++;
                            }

                        // ricopia
                        for (i = 0; (i < MAX_NUM_ARTICOLI - 1); i++) // COPERTI esclusi
                            SF_Data.Articolo[i] = tmpArticolo[i];

                        bModificaOK = true;
                        LogToFile("ImpostaGrigliaDlg : compattazione");
                    }
                }
                else // compattazione non possibile
                {
                    WarningManager(WRN_MNP);
                    return;
                }
            }
            else
                bModificaOK = true;


            if (bModificaOK)
            {
                if (checkBoxTouchMode.Checked)
                    SF_Data.iGeneralOptions = SetBit(SF_Data.iGeneralOptions, (int)GEN_OPTS.BIT_TOUCH_MODE_REQUIRED);
                else
                    SF_Data.iGeneralOptions = ClearBit(SF_Data.iGeneralOptions, (int)GEN_OPTS.BIT_TOUCH_MODE_REQUIRED);

                SF_Data.iGridCols = iNumColonneTmp;
                SF_Data.iGridRows = iNumRigheTmp;

                /************************************************************
                 *   aggiorna numero di possibili Articoli nelle pagine
                 ************************************************************/
                DataManager.SetLastArticoloIndex(SF_Data.iGridRows * SF_Data.iGridCols * iPageNumTmp);

                for (i = 0; i < PAGES_NUM_TABM; i++)
                {
                    if (!String.IsNullOrEmpty(_pEdit[i].Text))
                        SF_Data.sPageTabs[i] = _pEdit[i].Text;
                    else
                        SF_Data.sPageTabs[i] = String.Format("Pagina {0}", i + 1);
                }

                _bListinoModificato = true;

                DataManager.CheckLastArticoloIndexP1();

                LogToFile("ImpostaGriglia end");
            }

            Close();
        }

        private void CheckBoxTouchMode_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxTouchMode.Checked)
            {
                // modo Touch: poca scelta di righe, più colonne e 5 Tabs
                radioRows3.Enabled = false;
                radioRows4.Enabled = false;
                radioCols3.Enabled = true;

                Edit_4.Enabled = true;
                BtnCanc_4.Enabled = true;

                radioRows1.Text = "3";
                radioRows2.Text = "4";
                radioRows3.Text = "---";
                radioRows4.Text = "---";

                radioCols1.Text = "3";
                radioCols2.Text = "4";
                radioCols3.Text = "5";

                switch (SF_Data.iGridRows)
                {
                    case 3:
                        radioRows1.Checked = true;
                        break;
                    case 4:
                        radioRows2.Checked = true;
                        break;
                    default:
                        radioRows1.Checked = true;
                        break;
                }

                switch (SF_Data.iGridCols)
                {
                    case 3:
                        radioCols1.Checked = true;
                        break;
                    case 4:
                        radioCols2.Checked = true;
                        break;
                    case 5:
                        radioCols3.Checked = true;
                        break;
                    default:
                        radioCols1.Checked = true;
                        break;
                }
            }
            else
            {
                // modo solo testo poca scelta di colonne e di Tabs, molte righe
                radioRows3.Enabled = true;
                radioRows4.Enabled = true;
                radioCols3.Enabled = false;

                Edit_4.Enabled = false;
                BtnCanc_4.Enabled = false;

                radioRows1.Text = "10";
                radioRows2.Text = "15";
                radioRows3.Text = "20";
                radioRows4.Text = "25";

                radioCols1.Text = "3";
                radioCols2.Text = "4";
                radioCols3.Text = "---";

                switch (SF_Data.iGridRows)
                {
                    case 10:
                        radioRows1.Checked = true;
                        break;
                    case 15:
                        radioRows2.Checked = true;
                        break;
                    case 20:
                        radioRows3.Checked = true;
                        break;
                    case 25:
                        radioRows4.Checked = true;
                        break;
                    default:
                        radioRows1.Checked = true;
                        break;
                }

                switch (SF_Data.iGridCols)
                {
                    case 3:
                        radioCols1.Checked = true;
                        break;
                    case 4:
                        radioCols2.Checked = true;
                        break;
                    default:
                        radioCols1.Checked = true;
                        break;
                }
            }
        }

    }
}
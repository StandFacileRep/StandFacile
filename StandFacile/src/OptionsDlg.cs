/***************************************************
	NomeFile : StandFacile\optionsDlg.cs
    Data	 : 18.04.2025
	Autore   : Mauro Artuso

     un solo flag viene salvato nel registro
 ***************************************************/

using System;
using System.Windows.Forms;

using static StandCommonFiles.ComDef;
using static StandCommonFiles.CommonCl;
using static StandCommonFiles.LogServer;

using static StandFacile.glb;
using static StandFacile.Define;

namespace StandFacile
{
    /// <summary></summary>
    public partial class OptionsDlg : Form
    {
        static int _iDispMngStatus, _iColorThemeIndex;

        static bool _bListinoModificato;

        /// <summary>riferimento a dBaseIntf</summary>
        public static OptionsDlg _rOptionsDlg;

        /// <summary>ottiene flag di modifica listino necessaria</summary>
        public static bool GetListinoModificato() { return _bListinoModificato; }

        /// <summary>ottiene indice del tema dei colori</summary>
        public static int GetColorThemeIndex() { return _iColorThemeIndex; }

        /// <summary>ottiene flag di visualizzazione totale scontrino precedente</summary>
        public bool GetShowPrevReceipt() { return checkBoxShowPrevReceipt.Checked; }

        /// <summary>ottiene flag di modo caricamento Prevendita per gestione Focus</summary>
        public bool GetPresales_LoadMode() { return checkBoxPresales_loadMode.Checked; }

        /// <summary>ottiene flag di avvio stampa Scontrino con il tasto Enter</summary>
        public bool GetEnterPrintReceipt() { return checkBox_Enter.Checked; }

        /// <summary>ottiene flag di consenso agli articoli con Prezzo zero Euro</summary>
        public bool GetZeroPriceEnabled() { return checkBox_ZeroPriceItems.Checked; }

        /// <summary>ottiene flag di attivazione Pulsanti + - X</summary>
        public bool Get_VButtons() { return checkBox_VButtons.Checked; }

        readonly ToolTip _tt = new ToolTip
        {
            InitialDelay = 50,
            ShowAlways = true
        };

        /// <summary>costruttore</summary>
        public OptionsDlg()
        {

            InitializeComponent();

            _rOptionsDlg = this;

            _tt.SetToolTip(checkBox_ZeroPriceItems, "consente o meno Articoli con Prezzo = zero,\nattenzione che la disattivazione modifica il Listino eliminando eventuali Articoli con prezzo nullo,\nin questo caso si modifica anche il checksum web!");

            _tt.SetToolTip(checkBoxPrivacy, "attiva il modo riservato: si richiede cioè l'inserimento della password per visualizzare gli incassi");

            Init(false);
        }

        /// <summary>Inizializzazione dei vari Flag caricati dal Listino e dal Registry</summary>
        public void Init(bool bShow)
        {
            checkBoxTavolo.Checked = IsBitSet(SF_Data.iGeneralOptions, (int)GEN_OPTS.BIT_TABLE_REQUIRED);
            checkBoxCoperti.Checked = IsBitSet(SF_Data.iGeneralOptions, (int)GEN_OPTS.BIT_PLACE_SETTINGS_REQUIRED);
            checkBoxPayment.Checked = IsBitSet(SF_Data.iGeneralOptions, (int)GEN_OPTS.BIT_PAYMENT_REQUIRED);
            checkBox_ZeroPriceItems.Checked = IsBitSet(SF_Data.iGeneralOptions, (int)GEN_OPTS.BIT_ZERO_PRICE_ITEMS_ALLOWED);
            checkBoxPrivacy.Checked = IsBitSet(SF_Data.iGeneralOptions, (int)GEN_OPTS.BIT_PRIVACY);
            checkBox_Enter.Checked = IsBitSet(SF_Data.iGeneralOptions, (int)GEN_OPTS.BIT_ENTER_PRINT_RECEIPT_ENABLED);

            checkBoxPresale_workMode.Checked = SF_Data.bPrevendita; //  passato dal Listino


            if (DataManager.CheckIf_CassaSec_and_NDB()) // cassa secondaria e DB
            {
                checkBoxDisp.Enabled = false;

                checkBoxTavolo.Enabled = false;
                checkBoxCoperti.Enabled = false;
                checkBoxPayment.Enabled = false;
                checkBoxPrivacy.Enabled = false;
                checkBox_Enter.Enabled = false;
                checkBox_ZeroPriceItems.Enabled = false;
                checkBoxPresale_workMode.Enabled = false;

                // il modo prevendita impedisce il caricamento ordini nelle casse secondarie
                if (SF_Data.bPrevendita)
                    checkBoxPresales_loadMode.Enabled = false;
            }
            else
            {
                _iDispMngStatus = ReadRegistry(DISP_DLG_MNG_KEY, SetBit(0, BIT_SHOW_DISP_DLG));
                checkBoxDisp.Enabled = true;
                checkBoxDisp.Checked = IsBitSet(_iDispMngStatus, BIT_SHOW_DISP_DLG);

                checkBoxTavolo.Enabled = true;
                checkBoxCoperti.Enabled = true;
                checkBoxPayment.Enabled = true;
                checkBoxPrivacy.Enabled = true;
                checkBox_Enter.Enabled = true;
                checkBox_ZeroPriceItems.Enabled = true;
                checkBoxPresale_workMode.Enabled = true;
            }

            // letto dal Registry 
            checkBoxShowPrevReceipt.Checked = (ReadRegistry(VIEW_PREV_RECEIPT_KEY, 0) == 1);

            checkBoxPresales_loadMode.Checked = (ReadRegistry(PRESALE_LOAD_MODE_KEY, 0) == 1);

            checkBox_VButtons.Checked = (ReadRegistry(VBUTTONS_KEY, 1) == 1);

            _iColorThemeIndex = ReadRegistry(COLOR_THEME_KEY, 1);

            if (_iColorThemeIndex < NUM_COLOR_THEMES)
                comboColorTheme.SelectedIndex = _iColorThemeIndex;
            else
                comboColorTheme.SelectedIndex = 0;

            _bListinoModificato = false;

            if (bShow)
                ShowDialog();
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            bool bRiavvio = false;
            int iGeneralOptionsCopy;
            String sTmp;

            iGeneralOptionsCopy = 0;

            if (checkBoxTavolo.Checked)
                iGeneralOptionsCopy = SetBit(iGeneralOptionsCopy, (int)GEN_OPTS.BIT_TABLE_REQUIRED);

            if (checkBoxCoperti.Checked)
                iGeneralOptionsCopy = SetBit(iGeneralOptionsCopy, (int)GEN_OPTS.BIT_PLACE_SETTINGS_REQUIRED);

            if (checkBoxPayment.Checked)
                iGeneralOptionsCopy = SetBit(iGeneralOptionsCopy, (int)GEN_OPTS.BIT_PAYMENT_REQUIRED);

            if (checkBox_ZeroPriceItems.Checked)
                iGeneralOptionsCopy = SetBit(iGeneralOptionsCopy, (int)GEN_OPTS.BIT_ZERO_PRICE_ITEMS_ALLOWED);

            if (checkBoxPrivacy.Checked)
                iGeneralOptionsCopy = SetBit(iGeneralOptionsCopy, (int)GEN_OPTS.BIT_PRIVACY);

            if (checkBox_Enter.Checked)
                iGeneralOptionsCopy = SetBit(iGeneralOptionsCopy, (int)GEN_OPTS.BIT_ENTER_PRINT_RECEIPT_ENABLED);

            // controllo _bListinoModificato a gruppi, salvataggio in : SF_Data[]
            if (SF_Data.iGeneralOptions != iGeneralOptionsCopy) 
            {
                _bListinoModificato = true;

                SF_Data.iGeneralOptions = iGeneralOptionsCopy;
            }

            if (SF_Data.bPrevendita != checkBoxPresale_workMode.Checked)
            {
                _bListinoModificato = true;

                SF_Data.bPrevendita = checkBoxPresale_workMode.Checked;
                bRiavvio = true;
            }

            if (checkBoxDisp.Checked)
                _iDispMngStatus = SetBit(0, BIT_SHOW_DISP_DLG);
            else
                _iDispMngStatus = ClearBit(_iDispMngStatus, BIT_SHOW_DISP_DLG);

            if (comboColorTheme.SelectedIndex != _iColorThemeIndex)
            {
                _iColorThemeIndex = comboColorTheme.SelectedIndex;
                WriteRegistry(COLOR_THEME_KEY, _iColorThemeIndex);
            }

            WriteRegistry(DISP_DLG_MNG_KEY, _iDispMngStatus);

            WriteRegistry(VIEW_PREV_RECEIPT_KEY, checkBoxShowPrevReceipt.Checked ? 1 : 0);

            WriteRegistry(PRESALE_LOAD_MODE_KEY, checkBoxPresales_loadMode.Checked ? 1 : 0);

            WriteRegistry(VBUTTONS_KEY, checkBox_VButtons.Checked ? 1 : 0);

            sTmp = String.Format("optionsDlg OK: {0}, {1}, {2}, {3}, {4}", checkBoxTavolo.Checked, checkBoxTavolo.Checked,
                checkBoxTavolo.Checked, checkBoxTavolo.Checked, checkBoxPresale_workMode.Checked);
            LogToFile(sTmp);

            if (bRiavvio)
            {
                MessageBox.Show("Il modo di vendita è cambiato,\nil programma verrà riavviato !", "Attenzione !", MessageBoxButtons.OK);

                sTmp = String.Format("OptionsDlg, Tipo di database = {0}, funzione di Cassa = {1}", (ReadRegistry(DB_MODE_KEY, 0) > 0), SF_Data.bPrevendita);
                LogToFile(sTmp);

                ErrorManager(ERR_CDB);
            }

            Close();
        }

        /// <summary>le 2 checkbox possono essere entrambe deselezionate</summary>
        private void CheckBoxPresaleMode_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxPresale_workMode.Checked)
                checkBoxPresales_loadMode.Checked = false;
        }

        /// <summary>le 2 checkbox possono essere entrambe deselezionate</summary>
        private void CheckBoxLoadPresale_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxPresales_loadMode.Checked)
                checkBoxPresale_workMode.Checked = false;
        }

    }
}

/***************************************************
	NomeFile : StandFacile\OptionsDlg.cs
    Data	 : 04.05.2026
	Autore   : Mauro Artuso
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
        public bool GetShowPrevReceipt() { return checkBox_ShowPrevReceipt.Checked; }

        /// <summary>ottiene flag di modo caricamento Prevendita per gestione Focus</summary>
        public bool GetPresales_LoadMode() { return checkBox_Presales_loadMode.Checked; }

        /// <summary>ottiene flag di avvio stampa Scontrino con il tasto Enter</summary>
        public bool GetEnterPrintReceipt() { return checkBox_Enter.Checked; }

        /// <summary>ottiene flag di consenso agli articoli con Prezzo zero Euro</summary>
        public bool GetZeroPriceEnabled() { return checkBox_ZeroPriceItems.Checked; }

        /// <summary>ottiene flag di inserimento coperti obbligatorio</summary>
        public static bool GetPlaceSettings_MandatoryFlag() { return IsBitSet(SF_Data.iGeneralProgOptions, (int)GEN_PROGRAM_OPTIONS.BIT_PLACE_SETTINGS_REQUIRED); }

        /// <summary>ottiene flag di inserimento tavolo obbligatorio</summary>
        public static bool GetTable_MandatoryFlag() { return IsBitSet(SF_Data.iGeneralProgOptions, (int)GEN_PROGRAM_OPTIONS.BIT_TABLE_REQUIRED); }

        /// <summary>ottiene flag di inserimento nome obbligatorio</summary>
        public static bool GetName_MandatoryFlag() { return IsBitSet(SF_Data.iGeneralProgOptions, (int)GEN_PROGRAM_OPTIONS.BIT_NAME_REQUIRED); }

        /// <summary>ottiene flag di inserimento pagamento obbligatorio</summary>
        public static bool GetPayment_MandatoryFlag() { return IsBitSet(SF_Data.iGeneralProgOptions, (int)GEN_PROGRAM_OPTIONS.BIT_PAYMENT_REQUIRED); }

        /// <summary>ottiene flag di modo Touch</summary>
        public static bool GetTouchModeEnabled() { return IsBitSet(SF_Data.iGeneralProgOptions, (int)GEN_PROGRAM_OPTIONS.BIT_TOUCH_MODE_REQUIRED); }

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

            _tt.SetToolTip(checkBox_Privacy, "attiva il modo riservato: si richiede cioè l'inserimento della password per visualizzare gli incassi");

            //_tt.SetToolTip(checkBox_Show_DB_Check, "visualizza o meno il pulsante DB check");
            //_tt.SetToolTip(checkBox_Show_OSKeyb, "visualizza o meno il pulsante tastiera on screen");

            Init(false);
        }

        /// <summary>Inizializzazione dei vari Flag caricati dal Listino e dal Registry</summary>
        public void Init(bool bShow)
        {
            checkBox_Coperti.Checked = GetPlaceSettings_MandatoryFlag();
            checkBox_Tavolo.Checked = GetTable_MandatoryFlag();
            checkBox_Name.Checked = GetName_MandatoryFlag();
            checkBox_Payment.Checked = GetPayment_MandatoryFlag();

            checkBox_ZeroPriceItems.Checked = IsBitSet(SF_Data.iGeneralProgOptions, (int)GEN_PROGRAM_OPTIONS.BIT_ZERO_PRICE_ITEMS_ALLOWED);
            checkBox_Enter.Checked = IsBitSet(SF_Data.iGeneralProgOptions, (int)GEN_PROGRAM_OPTIONS.BIT_ENTER_PRINT_RECEIPT_ENABLED);
            checkBox_Privacy.Checked = IsBitSet(SF_Data.iGeneralProgOptions, (int)GEN_PROGRAM_OPTIONS.BIT_PRIVACY);

            // 0 = visualizzo, 1 = nascondo, tranne checkBox_Show_OSKeyb
            checkBox_ShowSendMsg.Checked = !IsBitSet(SF_Data.iGeneralProgOptions, (int)GEN_PROGRAM_OPTIONS.BIT_SHOW_MSG_BUTTON);
            checkBox_ShowTakeAway.Checked = !IsBitSet(SF_Data.iGeneralProgOptions, (int)GEN_PROGRAM_OPTIONS.BIT_SHOW_TAKEAWAY_BUTTON);
            checkBox_ShowDiscount.Checked = !IsBitSet(SF_Data.iGeneralProgOptions, (int)GEN_PROGRAM_OPTIONS.BIT_SHOW_DISCOUNT_BUTTON);
            checkBox_Show_DB_Check.Checked = !IsBitSet(SF_Data.iGeneralProgOptions, (int)GEN_PROGRAM_OPTIONS.BIT_SHOW_DB_BUTTON);

            // altra regola per Btn_OSKeyb
            checkBox_Show_OSKeyb.Checked = IsBitSet(SF_Data.iGeneralProgOptions, (int)GEN_PROGRAM_OPTIONS.BIT_SHOW_TOUCH_KEYB_BUTTON);

            checkBox_Presale_workMode.Checked = SF_Data.bPrevendita; //  passato dal Listino


            if (DataManager.CheckIf_CassaSec_and_NDB()) // cassa secondaria e DB
            {
                checkBox_InitialAvailabilityDlg.Enabled = false;
                checkBox_InitialAvailabilityDlg.Checked = false;

                checkBox_Coperti.Enabled = false;
                checkBox_Tavolo.Enabled = false;
                checkBox_Name.Enabled = false;
                checkBox_Payment.Enabled = false;
                checkBox_Privacy.Enabled = false;
                checkBox_Enter.Enabled = false;
                checkBox_ZeroPriceItems.Enabled = false;
                checkBox_Presale_workMode.Enabled = false;

                panelShowHideButtons.Enabled = false; 

                // il modo prevendita impedisce il caricamento ordini nelle casse secondarie
                if (SF_Data.bPrevendita)
                    checkBox_Presales_loadMode.Enabled = false;
            }
            else
            {
                _iDispMngStatus = ReadRegistry(DISP_DLG_MNG_KEY, SetBit(0, BIT_SHOW_DISP_DLG));

                checkBox_InitialAvailabilityDlg.Enabled = true;
                checkBox_InitialAvailabilityDlg.Checked = IsBitSet(_iDispMngStatus, BIT_SHOW_DISP_DLG);

                checkBox_Coperti.Enabled = true;
                checkBox_Tavolo.Enabled = true;
                checkBox_Name.Enabled = true;
                checkBox_Payment.Enabled = true;
                checkBox_Privacy.Enabled = true;
                checkBox_Enter.Enabled = true;
                checkBox_ZeroPriceItems.Enabled = true;
                checkBox_Presale_workMode.Enabled = true;

                panelShowHideButtons.Enabled = true;
            }

            // letto dal Registry 
            checkBox_ShowPrevReceipt.Checked = (ReadRegistry(VIEW_PREV_RECEIPT_KEY, 0) == 1);

            checkBox_Presales_loadMode.Checked = (ReadRegistry(PRESALE_LOAD_MODE_KEY, 0) == 1);

            _iColorThemeIndex = ReadRegistry(COLOR_THEME_KEY, 1);

            if (_iColorThemeIndex < NUM_COLOR_THEMES)
                comboColorTheme.SelectedIndex = _iColorThemeIndex;
            else
                comboColorTheme.SelectedIndex = 0;

            _bListinoModificato = false;

            if (bShow)
                ShowDialog();
        }

        /// <summary>le 2 checkbox possono essere entrambe deselezionate</summary>
        private void CheckBoxPresaleMode_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Presale_workMode.Checked)
                checkBox_Presales_loadMode.Checked = false;
        }

        /// <summary>le 2 checkbox possono essere entrambe deselezionate</summary>
        private void CheckBoxLoadPresale_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Presales_loadMode.Checked)
                checkBox_Presale_workMode.Checked = false;
        }

        private void btnDefault_Click(object sender, EventArgs e)
        {
            checkBox_Coperti.Checked = false;
            checkBox_Tavolo.Checked = false;
            checkBox_Name.Enabled = false;
            checkBox_Payment.Checked = false;
            checkBox_ZeroPriceItems.Checked = false;
            checkBox_Enter.Checked = false;

            checkBox_ShowSendMsg.Checked = true;
            checkBox_ShowTakeAway.Checked = true;
            checkBox_ShowDiscount.Checked = true;
            checkBox_Show_DB_Check.Checked = true;
            checkBox_Show_OSKeyb.Checked = false;

            checkBox_Privacy.Checked = false;
            checkBox_InitialAvailabilityDlg.Checked = false;
            checkBox_ShowPrevReceipt.Checked = false;

            checkBox_Presale_workMode.Checked = false;
            checkBox_Presales_loadMode.Checked = false;
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            bool bRiavvio = false;
            int iGeneralProgOptionsCopy;
            String sTmp;

            iGeneralProgOptionsCopy = 0;

            // BIT_TOUCH_MODE_REQUIRED è l'unico bit definito in un altro dialogo
            // va impostato qui altrimenti viene azzerato

            if (GetTouchModeEnabled())
                iGeneralProgOptionsCopy = SetBit(iGeneralProgOptionsCopy, (int)GEN_PROGRAM_OPTIONS.BIT_TOUCH_MODE_REQUIRED);

            if (checkBox_Coperti.Checked)
                iGeneralProgOptionsCopy = SetBit(iGeneralProgOptionsCopy, (int)GEN_PROGRAM_OPTIONS.BIT_PLACE_SETTINGS_REQUIRED);

            if (checkBox_Tavolo.Checked)
                iGeneralProgOptionsCopy = SetBit(iGeneralProgOptionsCopy, (int)GEN_PROGRAM_OPTIONS.BIT_TABLE_REQUIRED);

            if (checkBox_Name.Checked)
                iGeneralProgOptionsCopy = SetBit(iGeneralProgOptionsCopy, (int)GEN_PROGRAM_OPTIONS.BIT_NAME_REQUIRED);

            if (checkBox_Payment.Checked)
                iGeneralProgOptionsCopy = SetBit(iGeneralProgOptionsCopy, (int)GEN_PROGRAM_OPTIONS.BIT_PAYMENT_REQUIRED);

            if (checkBox_ZeroPriceItems.Checked)
                iGeneralProgOptionsCopy = SetBit(iGeneralProgOptionsCopy, (int)GEN_PROGRAM_OPTIONS.BIT_ZERO_PRICE_ITEMS_ALLOWED);

            if (checkBox_Privacy.Checked)
                iGeneralProgOptionsCopy = SetBit(iGeneralProgOptionsCopy, (int)GEN_PROGRAM_OPTIONS.BIT_PRIVACY);

            if (checkBox_Enter.Checked)
                iGeneralProgOptionsCopy = SetBit(iGeneralProgOptionsCopy, (int)GEN_PROGRAM_OPTIONS.BIT_ENTER_PRINT_RECEIPT_ENABLED);

            // 0 = visualizzo, 1 = nascondo, tranne checkBox_Show_OSKeyb
            if (!checkBox_ShowSendMsg.Checked)
                iGeneralProgOptionsCopy = SetBit(iGeneralProgOptionsCopy, (int)GEN_PROGRAM_OPTIONS.BIT_SHOW_MSG_BUTTON);

            if (!checkBox_ShowTakeAway.Checked)
                iGeneralProgOptionsCopy = SetBit(iGeneralProgOptionsCopy, (int)GEN_PROGRAM_OPTIONS.BIT_SHOW_TAKEAWAY_BUTTON);

            if (!checkBox_ShowDiscount.Checked)
                iGeneralProgOptionsCopy = SetBit(iGeneralProgOptionsCopy, (int)GEN_PROGRAM_OPTIONS.BIT_SHOW_DISCOUNT_BUTTON);

            if (!checkBox_Show_DB_Check.Checked)
                iGeneralProgOptionsCopy = SetBit(iGeneralProgOptionsCopy, (int)GEN_PROGRAM_OPTIONS.BIT_SHOW_DB_BUTTON);

            // altra regola per Btn_OSKeyb
            if (checkBox_Show_OSKeyb.Checked)
                iGeneralProgOptionsCopy = SetBit(iGeneralProgOptionsCopy, (int)GEN_PROGRAM_OPTIONS.BIT_SHOW_TOUCH_KEYB_BUTTON);

            // controllo _bListinoModificato a gruppi, salvataggio in : SF_Data[]
            if (SF_Data.iGeneralProgOptions != iGeneralProgOptionsCopy)
            {
                _bListinoModificato = true;

                SF_Data.iGeneralProgOptions = iGeneralProgOptionsCopy;
            }

            if (SF_Data.bPrevendita != checkBox_Presale_workMode.Checked)
            {
                _bListinoModificato = true;

                SF_Data.bPrevendita = checkBox_Presale_workMode.Checked;
                bRiavvio = true;
            }

            if (checkBox_InitialAvailabilityDlg.Checked)
                _iDispMngStatus = SetBit(0, BIT_SHOW_DISP_DLG);
            else
                _iDispMngStatus = ClearBit(_iDispMngStatus, BIT_SHOW_DISP_DLG);

            if (comboColorTheme.SelectedIndex != _iColorThemeIndex)
            {
                _iColorThemeIndex = comboColorTheme.SelectedIndex;
                WriteRegistry(COLOR_THEME_KEY, _iColorThemeIndex);
            }

            WriteRegistry(DISP_DLG_MNG_KEY, _iDispMngStatus);

            WriteRegistry(VIEW_PREV_RECEIPT_KEY, checkBox_ShowPrevReceipt.Checked ? 1 : 0);

            WriteRegistry(PRESALE_LOAD_MODE_KEY, checkBox_Presales_loadMode.Checked ? 1 : 0);

            sTmp = String.Format("optionsDlg OK: {0}, {1}, {2}, {3:X5}", checkBox_Coperti.Checked, checkBox_Tavolo.Checked, checkBox_Name.Checked,
                                    SF_Data.iGeneralProgOptions);
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

    }
}

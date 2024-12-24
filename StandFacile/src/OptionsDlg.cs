/***************************************************
	NomeFile : StandFacile\optionsDlg.cs
    Data	 : 06.12.2024
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

        /// <summary>ottiene flag di attivazione Pulsanti + - X</summary>
        public bool Get_VButtons() { return checkBox_VButtons.Checked; }

        /// <summary>costruttore</summary>
        public OptionsDlg()
        {

            InitializeComponent();

            _rOptionsDlg = this;

            Init(false);
        }

        /// <summary>Inizializzazione dei vari Flag caricati dal Listino e dal Registry</summary>
        public void Init(bool bShow)
        {
            checkBoxTavolo.Checked = SF_Data.bTavoloRichiesto;
            checkBoxCoperti.Checked = SF_Data.bCopertoRichiesto;
            checkBoxPayment.Checked = SF_Data.bModoPagamRichiesto;
            checkBoxPrivacy.Checked = SF_Data.bRiservatezzaRichiesta;

            checkBoxPresale_workMode.Checked = SF_Data.bPrevendita; //  passato dal Listino

            if (DataManager.CheckIf_CassaSec_and_NDB()) // cassa secondaria e DB
            {
                checkBoxDisp.Enabled = false;

                checkBoxTavolo.Enabled = false;
                checkBoxCoperti.Enabled = false;
                checkBoxPayment.Enabled = false;
                checkBoxPrivacy.Enabled = false;
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
                checkBoxPresale_workMode.Enabled = true;
            }

            // letto dal Registry 
            checkBoxShowPrevReceipt.Checked = (ReadRegistry(VIEW_PREV_RECEIPT_KEY, 0) == 1);

            checkBoxPresales_loadMode.Checked = (ReadRegistry(PRESALE_LOAD_MODE_KEY, 0) == 1);

            checkBox_Enter.Checked = (ReadRegistry(ENTER_PRINT_RECEIPT_KEY, 0) == 1);

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
            String sTmp;

            if (comboColorTheme.SelectedIndex != _iColorThemeIndex)
            {
                _iColorThemeIndex = comboColorTheme.SelectedIndex;
                WriteRegistry(COLOR_THEME_KEY, _iColorThemeIndex);
            }

            // controllo _bListinoModificato a gruppi, salvataggio in : SF_Data[]
            if ((SF_Data.bTavoloRichiesto != checkBoxTavolo.Checked) || (SF_Data.bCopertoRichiesto != checkBoxCoperti.Checked) ||
                 (SF_Data.bPrevendita != checkBoxPresale_workMode.Checked) || (SF_Data.bModoPagamRichiesto != checkBoxPayment.Checked) ||
                 (SF_Data.bRiservatezzaRichiesta != checkBoxPrivacy.Checked))
            {
                _bListinoModificato = true;

                SF_Data.bTavoloRichiesto = checkBoxTavolo.Checked;
                SF_Data.bCopertoRichiesto = checkBoxCoperti.Checked;
                SF_Data.bModoPagamRichiesto = checkBoxPayment.Checked;
                SF_Data.bRiservatezzaRichiesta = checkBoxPrivacy.Checked;
            }

            if (SF_Data.bPrevendita != checkBoxPresale_workMode.Checked)
            {
                _bListinoModificato = true;

                SF_Data.bPrevendita = checkBoxPresale_workMode.Checked;
                bRiavvio = true;
            }

            if (checkBoxDisp.Checked)
                _iDispMngStatus = SetBit( 0 ,BIT_SHOW_DISP_DLG);
            else
                _iDispMngStatus = ClearBit(_iDispMngStatus, BIT_SHOW_DISP_DLG);

            WriteRegistry(DISP_DLG_MNG_KEY, _iDispMngStatus);

            WriteRegistry(VIEW_PREV_RECEIPT_KEY, checkBoxShowPrevReceipt.Checked ? 1 : 0);

            WriteRegistry(PRESALE_LOAD_MODE_KEY, checkBoxPresales_loadMode.Checked ? 1 : 0);

            WriteRegistry(ENTER_PRINT_RECEIPT_KEY, checkBox_Enter.Checked ? 1 : 0);

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

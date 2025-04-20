/*****************************************************
 	NomeFile : StandCommonSrc/PrintTicketConfigDlg.cs
    Data	 : 06.12.2024
 	Autore   : Mauro Artuso

 *****************************************************/

using System;
using System.Windows.Forms;

using static StandFacile.glb;

using static StandCommonFiles.CommonCl;
using static StandCommonFiles.ComDef;
using static StandCommonFiles.LogServer;

namespace StandFacile
{
    /// <summary>
    /// Classe per la configurazione delle copie di stampa
    /// </summary>
    public partial class PrintLocalCopiesConfigDlg : Form
    {
#pragma warning disable IDE0044

        /// <summary>riferimento a PrintTicketConfigDlg</summary>
        public static PrintLocalCopiesConfigDlg _rPrintTckConfigDlg;

        readonly ToolTip _tt = new ToolTip
        {
            InitialDelay = 50,
            ShowAlways = true
        };

        /*****************************************************
            vanno restituite solo variabili e non controlli,
            che potrebbero non essere confermati con OK
         *****************************************************/

        static bool _bListinoModificato;
        static bool _bCheckBox_AvoidPrintGroupsCheckedCopy, _bCheckBox_CUT_CheckedCopy;

        /// <summary>ottiene flag di modifica listino necessaria</summary>
        public static bool GetListinoModificato() { return _bListinoModificato; }

        /// <summary>ottiene true se la stampante è windows</summary>
        public static bool GetPrinterTypeIsWinwows() { return (iSysPrinterType == (int)PRINTER_SEL.STAMPANTE_WINDOWS); }

        /// <summary>ottiene flag di richiesta stampa copia scontrino NoPrice</summary>
        public static bool GetTicketNoPriceCopy() { return _rPrintTckConfigDlg.checkBoxLocalCopy.Checked; }

        /// <summary>imposta il puntatore ai checkBox delle copie</summary>
        CheckBox[] _pCheckBoxCopia = new CheckBox[NUM_SEP_PRINT_GROUPS];

        /// <summary>struct per la gestione degli avvisi e/o errori</summary>
        public static TErrMsg _WrnMsg;

        /// <summary>costruttore</summary>
        public PrintLocalCopiesConfigDlg()
        {
            InitializeComponent();

            _rPrintTckConfigDlg = this;

            _tt.SetToolTip(BtnWin, "imposta stampante Windows: USB, LAN, WiFi");
            _tt.SetToolTip(BtnLegacy, "imposta stampante Legacy: COM, LPT");

            _pCheckBoxCopia[0] = checkBoxCopia_0;
            _pCheckBoxCopia[1] = checkBoxCopia_1;
            _pCheckBoxCopia[2] = checkBoxCopia_2;
            _pCheckBoxCopia[3] = checkBoxCopia_3;
            _pCheckBoxCopia[4] = checkBoxCopia_4;
            _pCheckBoxCopia[5] = checkBoxCopia_5;
            _pCheckBoxCopia[6] = checkBoxCopia_6;
            _pCheckBoxCopia[7] = checkBoxCopia_7;
            _pCheckBoxCopia[8] = checkBoxCopia_8;
            _pCheckBoxCopia[9] = checkBoxCopia_9;

            if (CheckService(_HIDE_LEGACY_PRINTER))
            {
                printersGroupBox.Visible = false;
                RadioGroup_PrinterType.Visible = false;

                this.Height = 512;
            }

                Init(false);
        }

        /// <summary>
        /// Inizializzazione con lettura dal Registro e dal DataManager
        /// </summary>
        /// <param name="bShow"></param>
        public void Init(bool bShow)
        {
            int iPrinterTypeRadio;

            //inizializzazione stampante windows o Legacy
            iPrinterTypeRadio = ReadRegistry(SYS_PRINTER_TYPE_KEY, (int)PRINTER_SEL.STAMPANTE_WINDOWS);

            if ((iPrinterTypeRadio == (int)PRINTER_SEL.STAMPANTE_WINDOWS) || CheckService(_HIDE_LEGACY_PRINTER))

            {
                prt_Windows.Checked = true;
                iSysPrinterType = (int)PRINTER_SEL.STAMPANTE_WINDOWS;
            }
            else
            {
                prt_Legacy.Checked = true;
                iSysPrinterType = (int)PRINTER_SEL.STAMPANTE_LEGACY;
            }

            if (DataManager.CheckIf_CassaSec_and_NDB()) // cassa secondaria e DB
            {
                panelLocalCopies.Enabled = false;
                label1.Enabled = false;
                label2.Enabled = false;
                label3.Enabled = false;
            }
            else
            {
                panelLocalCopies.Enabled = true;
                label1.Enabled = true;
                label2.Enabled = true;
                label3.Enabled = true;
            }

            for (int i = 0; i < NUM_SEP_PRINT_GROUPS; i++)
            {
                if (!String.IsNullOrEmpty(SF_Data.sCopiesGroupsText[i]))
                    _pCheckBoxCopia[i].Text = SF_Data.sCopiesGroupsText[i];

                _tt.SetToolTip(_pCheckBoxCopia[i], "questa descrizione si imposta da:\r\n \"Configurazione Copie in rete\"");

                _pCheckBoxCopia[i].Checked = IsBitSet(SF_Data.iReceiptCopyOptions, i);
            }

            checkBox_AvoidPrintGroups.Checked = IsBitSet(SF_Data.iReceiptCopyOptions, (int)LOCAL_COPIES_OPTS.BIT_AVOIDPRINTGROUPS_PRINT_REQUIRED);
            _bCheckBox_AvoidPrintGroupsCheckedCopy = checkBox_AvoidPrintGroups.Checked;

            checkBoxLocalCopy.Checked = IsBitSet(SF_Data.iReceiptCopyOptions, (int)LOCAL_COPIES_OPTS.BIT_RECEIPT_LOCAL_COPY_REQUIRED);
            checkBoxSelectedOnly.Checked = IsBitSet(SF_Data.iReceiptCopyOptions, (int)LOCAL_COPIES_OPTS.BIT_SELECTEDONLY_PRINT_REQUIRED);
            checkBoxSingleRowItems.Checked = IsBitSet(SF_Data.iReceiptCopyOptions, (int)LOCAL_COPIES_OPTS.BIT_SINGLEROWITEMS_PRINT_REQUIRED);
            checkBoxUnitItems.Checked = IsBitSet(SF_Data.iReceiptCopyOptions, (int)LOCAL_COPIES_OPTS.BIT_QUANTITYONE_PRINT_REQUIRED);

            checkBox_CUT.Checked = IsBitSet(SF_Data.iReceiptCopyOptions, (int)LOCAL_COPIES_OPTS.BIT_PRINT_GROUPS_CUT_REQUIRED);
            _bCheckBox_CUT_CheckedCopy = checkBox_CUT.Checked;

            CheckBoxNoPrice_CheckedChanged(this, null);

            _bListinoModificato = false;

            if (bShow)
            {
                timer.Enabled = true;
                ShowDialog();
            }
            else
                timer.Enabled = false;
        }

        private void BtnLegacy_Click(object sender, EventArgs e)
        {
            LegacyPrinterDlg.rThermPrinterDlg.Init(true);
        }

        private void BtnWin_Click(object sender, EventArgs e)
        {
            WinPrinterDlg._rWinPrinterDlg.Init(true);

            if (WinPrinterDlg.GetListinoModificato())
                DataManager.SalvaListino();
        }

        private void CheckBoxNoPrice_CheckedChanged(object sender, EventArgs e)
        {
            checkBox_AvoidPrintGroups.Enabled = checkBoxLocalCopy.Checked;
            checkBoxSelectedOnly.Enabled = checkBoxLocalCopy.Checked;
            checkBoxSingleRowItems.Enabled = checkBoxLocalCopy.Checked;
            checkBoxUnitItems.Enabled = checkBoxLocalCopy.Checked;

            panelCopies.Enabled = checkBoxLocalCopy.Checked && checkBoxSelectedOnly.Checked;

            labelWarn.Enabled = checkBoxLocalCopy.Checked;
        }

        private void CheckBoxSingleRowItems_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxSingleRowItems.Checked)
                checkBoxUnitItems.Checked = false;
        }

        private void CheckBoxUnitItems_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxUnitItems.Checked)
                checkBoxSingleRowItems.Checked = false;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            checkBox_CUT.Enabled = checkBoxLocalCopy.Checked && (!checkBox_AvoidPrintGroups.Checked || !checkBoxSingleRowItems.Checked && !checkBoxUnitItems.Checked);

            checkBox_AvoidPrintGroups.Enabled = checkBoxLocalCopy.Checked && checkBoxSelectedOnly.Checked && (checkBoxSingleRowItems.Checked || checkBoxUnitItems.Checked);

            // fa vedere false quando è disabilitato
            if (checkBox_AvoidPrintGroups.Enabled)
                checkBox_AvoidPrintGroups.Checked = _bCheckBox_AvoidPrintGroupsCheckedCopy;
            else
                checkBox_AvoidPrintGroups.Checked = false;

            checkBox_CUT.Enabled = checkBoxLocalCopy.Checked && (!checkBox_AvoidPrintGroups.Checked || !checkBoxSingleRowItems.Checked && !checkBoxUnitItems.Checked);

            // fa vedere false quando è disabilitato
            if (checkBox_CUT.Enabled)
                checkBox_CUT.Checked = _bCheckBox_CUT_CheckedCopy;
            else
                checkBox_CUT.Checked = false;
        }

        private void LinkLbl_Mnu_CCR_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Close();

            PrintNetCopiesConfigDlg._rPrintConfigDlg.Init(true);

            if (PrintNetCopiesConfigDlg.GetListinoModificato())
                DataManager.SalvaListino();
        }

        private void CheckBox_AvoidPrintGroups_Click(object sender, EventArgs e)
        {
            _bCheckBox_AvoidPrintGroupsCheckedCopy = checkBox_AvoidPrintGroups.Checked;
        }

        private void CheckBox_CUT_Click(object sender, EventArgs e)
        {
            _bCheckBox_CUT_CheckedCopy = checkBox_CUT.Checked;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;
        }

        /**************************************************
	        letture dai controlli dalle variabili per la
	        scrittura nel registro
         **************************************************/
        private void BtnOK_Click(object sender, EventArgs e)
        {
            int i, iReceiptCopyOptions;

            iReceiptCopyOptions = 0;

            for (i = 0; i < NUM_SEP_PRINT_GROUPS; i++)
            {
                if (_pCheckBoxCopia[i].Checked)
                    iReceiptCopyOptions = SetBit(iReceiptCopyOptions, i);
            }

            if (checkBox_AvoidPrintGroups.Checked)
                iReceiptCopyOptions = SetBit(iReceiptCopyOptions, (int)LOCAL_COPIES_OPTS.BIT_AVOIDPRINTGROUPS_PRINT_REQUIRED);

            if (checkBoxLocalCopy.Checked)
                iReceiptCopyOptions = SetBit(iReceiptCopyOptions, (int)LOCAL_COPIES_OPTS.BIT_RECEIPT_LOCAL_COPY_REQUIRED);

            if (checkBoxSelectedOnly.Checked)
                iReceiptCopyOptions = SetBit(iReceiptCopyOptions, (int)LOCAL_COPIES_OPTS.BIT_SELECTEDONLY_PRINT_REQUIRED);

            if (checkBoxSingleRowItems.Checked)
                iReceiptCopyOptions = SetBit(iReceiptCopyOptions, (int)LOCAL_COPIES_OPTS.BIT_SINGLEROWITEMS_PRINT_REQUIRED);

            if (checkBoxUnitItems.Checked)
                iReceiptCopyOptions = SetBit(iReceiptCopyOptions, (int)LOCAL_COPIES_OPTS.BIT_QUANTITYONE_PRINT_REQUIRED);

            if (checkBox_CUT.Checked)
                iReceiptCopyOptions = SetBit(iReceiptCopyOptions, (int)LOCAL_COPIES_OPTS.BIT_PRINT_GROUPS_CUT_REQUIRED);

            if (WinPrinterDlg.GetCopies_PlaceSettingsToBePrinted())
                iReceiptCopyOptions = SetBit(iReceiptCopyOptions, (int)LOCAL_COPIES_OPTS.BIT_PLACESETTS_PRINT_ON_COPIES_REQUIRED);

            if (WinPrinterDlg.GetCopies_LogoToBePrinted())
                iReceiptCopyOptions = SetBit(iReceiptCopyOptions, (int)LOCAL_COPIES_OPTS.BIT_LOGO_PRINT_REQUIRED);

            if (sGlbWinPrinterParams.bChars33)
                iReceiptCopyOptions = SetBit(iReceiptCopyOptions, (int)LOCAL_COPIES_OPTS.BIT_CHARS33_PRINT_REQUIRED);

            // controllo _bListinoModificato per gestione della stampa QuantitàUno, salvataggio in : SF_Data[]
            if (SF_Data.iReceiptCopyOptions != iReceiptCopyOptions)
            {
                _bListinoModificato = true;

                SF_Data.iReceiptCopyOptions = iReceiptCopyOptions;
            }

            if (prt_Windows.Checked || CheckService(_HIDE_LEGACY_PRINTER))
            {
                WriteRegistry(SYS_PRINTER_TYPE_KEY, (int)PRINTER_SEL.STAMPANTE_WINDOWS);
                iSysPrinterType = (int)PRINTER_SEL.STAMPANTE_WINDOWS;
                prt_Windows.Checked = true;
            }
            else
            {
                WriteRegistry(SYS_PRINTER_TYPE_KEY, (int)PRINTER_SEL.STAMPANTE_LEGACY);
                iSysPrinterType = (int)PRINTER_SEL.STAMPANTE_LEGACY;
                prt_Legacy.Checked = true;
            }

            LogToFile("PrintTicketConfigDlg OK");

            timer.Enabled = false;
            Close();
        }
    }
}

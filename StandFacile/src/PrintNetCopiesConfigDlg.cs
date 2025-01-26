/*****************************************************
 	NomeFile : StandFacile/PrintConfigDlg.cs
    Data	 : 06.12.2024
 	Autore   : Mauro Artuso
 *****************************************************/

using System;
using System.Windows.Forms;
using System.Drawing.Printing;

using static StandFacile.glb;
using static StandFacile.Define;
using static StandFacile.dBaseIntf;

using static StandCommonFiles.CommonCl;
using static StandCommonFiles.ReceiptAndCopies;
using static StandCommonFiles.ComDef;
using static StandCommonFiles.LogServer;
using static StandCommonFiles.Printer_Legacy;
using static StandCommonFiles.Printer_Windows;


namespace StandFacile
{
    /// <summary>
    /// classe per la configurazione delle copie di stampa
    /// </summary>
    public partial class PrintNetCopiesConfigDlg : Form
    {
#pragma warning disable IDE0044

        const String _LEGACY_PRINTER = "Legacy Printer (COM, LPT)";

        /// <summary>riferimento a rPrintConfigDlg</summary>
        public static PrintNetCopiesConfigDlg _rPrintConfigDlg;

        static PrinterSettings _settings = new PrinterSettings();
        static String _sDefaultPrinter;

        static bool _bListinoModificato;

        /// <summary>struct per la gestione degli avvisi e/o errori</summary>
        public static TErrMsg _WrnMsg;

        /// <summary>ottiene flag di modifica listino necessaria</summary>
        public static bool GetListinoModificato() { return _bListinoModificato; }

        //TErrMsg _ErrMsg;

        Button[] _pBtnPrintCheck;

        ComboBox[] _pPrintersListCombo;
        CheckBox[] _pCheckBoxCopia;
        CheckBox[] _pCheckBox_BCD;
        TextBox[] _pPrintCopyText;
        TextBox[] _pTextBoxColor;

        int[] _iGroupsColor = new int[NUM_EDIT_GROUPS];

        /*****************************************************
            vanno restituite solo variabili e non controlli,
            che potrebbero non essere confermati con OK
         *****************************************************/
        /// <summary> funzione che ritorna true se la stampante in uso in CASSA è windows</summary>
        public static bool GetPrinterTypeIsWinwows() { return (iSysPrinterType == (int)PRINTER_SEL.STAMPANTE_WINDOWS); }

        /// <summary> overload funzione che ritorna true se la stampante copie in uso è windows</summary>
        public static bool GetPrinterTypeIsWinwows(int iPrinterIndex)
        {
            if (iPrinterIndex == NUM_EDIT_GROUPS + 1)  // stampa Messaggi
                return (sGlbWinPrinterParams.sMsgPrinterModel != _LEGACY_PRINTER);
            else if (iPrinterIndex == (NUM_EDIT_GROUPS)) // stampa Tickets
                return (sGlbWinPrinterParams.sTckPrinterModel != _LEGACY_PRINTER);
            else
                return (sGlbWinPrinterParams.sPrinterModel[iPrinterIndex] != _LEGACY_PRINTER);
        }

        readonly ToolTip _tt = new ToolTip
        {
            InitialDelay = 50,
            ShowAlways = true
        };

        /// <summary>costruttore</summary>
        public PrintNetCopiesConfigDlg()
        {
            int i;

            InitializeComponent();

            _rPrintConfigDlg = this;
            //Inizializzazione dei puntatori ai componenti per un codice più chiaro

            _pBtnPrintCheck = new Button[NUM_EDIT_GROUPS];
            _pPrintCopyText = new TextBox[NUM_EDIT_GROUPS];
            _pCheckBoxCopia = new CheckBox[NUM_EDIT_GROUPS];
            _pCheckBox_BCD = new CheckBox[NUM_EDIT_GROUPS];
            _pPrintersListCombo = new ComboBox[NUM_EDIT_GROUPS];

            _pTextBoxColor = new TextBox[NUM_GROUPS_COLORS];


            _pBtnPrintCheck[0] = BtnPrintCheck_0;
            _pBtnPrintCheck[1] = BtnPrintCheck_1;
            _pBtnPrintCheck[2] = BtnPrintCheck_2;
            _pBtnPrintCheck[3] = BtnPrintCheck_3;
            _pBtnPrintCheck[4] = BtnPrintCheck_4;
            _pBtnPrintCheck[5] = BtnPrintCheck_5;
            _pBtnPrintCheck[6] = BtnPrintCheck_6;
            _pBtnPrintCheck[7] = BtnPrintCheck_7;

            _pCheckBoxCopia[0] = checkBoxCopia_0;
            _pCheckBoxCopia[1] = checkBoxCopia_1;
            _pCheckBoxCopia[2] = checkBoxCopia_2;
            _pCheckBoxCopia[3] = checkBoxCopia_3;
            _pCheckBoxCopia[4] = checkBoxCopia_4;
            _pCheckBoxCopia[5] = checkBoxCopia_5;
            _pCheckBoxCopia[6] = checkBoxCopia_6;
            _pCheckBoxCopia[7] = checkBoxCopia_7;

            _pCheckBox_BCD[0] = checkBoxBCD_0;
            _pCheckBox_BCD[1] = checkBoxBCD_1;
            _pCheckBox_BCD[2] = checkBoxBCD_2;
            _pCheckBox_BCD[3] = checkBoxBCD_3;
            _pCheckBox_BCD[4] = checkBoxBCD_4;
            _pCheckBox_BCD[5] = checkBoxBCD_5;
            _pCheckBox_BCD[6] = checkBoxBCD_6;
            _pCheckBox_BCD[7] = checkBoxBCD_7;

            _pPrintCopyText[0] = CopiaText_0;
            _pPrintCopyText[1] = CopiaText_1;
            _pPrintCopyText[2] = CopiaText_2;
            _pPrintCopyText[3] = CopiaText_3;
            _pPrintCopyText[4] = CopiaText_4;
            _pPrintCopyText[5] = CopiaText_5;
            _pPrintCopyText[6] = CopiaText_6;
            _pPrintCopyText[7] = CopiaText_7;

            _pTextBoxColor[0] = textBoxColor_0;
            _pTextBoxColor[1] = textBoxColor_1;
            _pTextBoxColor[2] = textBoxColor_2;
            _pTextBoxColor[3] = textBoxColor_3;

            for (i = 0; i < NUM_EDIT_GROUPS; i++)
                _iGroupsColor[i] = 0;

            _pPrintersListCombo[0] = PrintersListCombo_0;
            _pPrintersListCombo[1] = PrintersListCombo_1;
            _pPrintersListCombo[2] = PrintersListCombo_2;
            _pPrintersListCombo[3] = PrintersListCombo_3;
            _pPrintersListCombo[4] = PrintersListCombo_4;
            _pPrintersListCombo[5] = PrintersListCombo_5;
            _pPrintersListCombo[6] = PrintersListCombo_6;
            _pPrintersListCombo[7] = PrintersListCombo_7;

            for (i = 0; i < NUM_EDIT_GROUPS; i++)
            {
                _tt.SetToolTip(_pCheckBoxCopia[i], "Ctrl + click per cambiare il colore");

                if (i < NUM_EDIT_GROUPS - 1)
                    _tt.SetToolTip(_pPrintCopyText[i], "etichetta modificabile che apparirà nelle stampe e negli ordini web");
                else
                    _tt.SetToolTip(_pPrintCopyText[i], "etichetta modificabile che apparirà nelle stampe ma non negli ordini web");

                _tt.SetToolTip(_pBtnPrintCheck[i], "effettua stampa di prova");
                _tt.SetToolTip(_pCheckBox_BCD[i], "stampa il barcode nella copia");
                _tt.SetToolTip(_pPrintersListCombo[i], "imposta la stampante per la copia");

                _pPrintCopyText[i].MaxLength = MAX_COPIES_TEXT_CHARS;
            }

            for (i = 0; i < NUM_GROUPS_COLORS - 1; i++)
                _pTextBoxColor[i].MaxLength = MAX_COPIES_TEXT_CHARS;

            _bListinoModificato = false;
            Init(false);
        }

        /// <summary>
        /// Inizializzazione con lettura dal Registro e dal DataManager
        /// </summary>
        public void Init(bool bShow)
        {
            int i, j;
            String sTmp;

            // stampante Windows di Default
            _sDefaultPrinter = _settings.PrinterName;

            for (i = 0; i < NUM_EDIT_GROUPS; i++)
            {
                sTmp = String.Format(WIN_CPY_PRINTER_MODEL_MKEY, i);
                sGlbWinPrinterParams.sPrinterModel[i] = ReadRegistry(sTmp, _sDefaultPrinter); //stampa copie

                _pPrintersListCombo[i].Items.Clear();

                j = 0;
                _pPrintersListCombo[i].Items.Add(_LEGACY_PRINTER);

                foreach (String printer in PrinterSettings.InstalledPrinters)
                {
                    _pPrintersListCombo[i].Items.Add(printer);

                    if (printer == sGlbWinPrinterParams.sPrinterModel[i])
                        sGlbWinPrinterParams.iPrinterModel[i] = j + 1; // tiene conto di _LEGACY_PRINTER

                    j++;
                }

                if ((sGlbWinPrinterParams.iPrinterModel[i] < PrinterSettings.InstalledPrinters.Count + 1) && (sGlbWinPrinterParams.iPrinterModel[i] >= 0))
                    _pPrintersListCombo[i].SelectedIndex = sGlbWinPrinterParams.iPrinterModel[i];
                else
                    _pPrintersListCombo[i].SelectedIndex = 0;
            }

            //letture dal DataManager
            for (i = 0; i < NUM_EDIT_GROUPS; i++)
            {
                _pCheckBoxCopia[i].Checked = SF_Data.bCopiesGroupsFlag[i];
                _pPrintCopyText[i].Text = SF_Data.sCopiesGroupsText[i]; //copia locale
            }

            for (i = 0; i < NUM_EDIT_GROUPS; i++)
            {
                _iGroupsColor[i] = SF_Data.iGroupsColor[i];

                _pCheckBoxCopia[i].BackColor = GetColor(_iGroupsColor[i])[0];
                _pCheckBoxCopia[i].ForeColor = GetColor(_iGroupsColor[i])[1];
            }

            for (i = 0; i < NUM_GROUPS_COLORS - 1; i++)
                _pTextBoxColor[i].Text = SF_Data.sColorGroupsText[i];

            if (bUSA_NDB())
            {
                for (i = 0; i < NUM_EDIT_GROUPS; i++)
                    _pCheckBox_BCD[i].Checked = IsBitSet(SF_Data.iBarcodeRichiesto, i);
            }

            if (DataManager.CheckIf_CassaSec_and_NDB()) // cassa secondaria e DB
            {
                for (i = 0; i < NUM_EDIT_GROUPS; i++)
                {
                    _pCheckBoxCopia[i].Enabled = false;
                    _pPrintCopyText[i].Enabled = false;
                    _pPrintCopyText[i].ReadOnly = true;
                    _pPrintCopyText[i].BackColor = System.Drawing.Color.AntiqueWhite;
                }

                for (i = 0; i < NUM_GROUPS_COLORS - 1; i++)
                    _pTextBoxColor[i].ReadOnly = true;

                btnReset.Enabled = false;
            }
            else
            {
                for (i = 0; i < NUM_EDIT_GROUPS; i++)
                {
                    _pCheckBoxCopia[i].Enabled = true;
                    _pPrintCopyText[i].Enabled = true;
                    _pPrintCopyText[i].ReadOnly = false;
                    _pPrintCopyText[i].BackColor = System.Drawing.SystemColors.Window;
                }

                for (i = 0; i < NUM_GROUPS_COLORS - 1; i++)
                    _pTextBoxColor[i].ReadOnly = false;

                btnReset.Enabled = true;
            }

            if (bShow)
            {
                timer.Enabled = true;
                ShowDialog();
            }
            else
                timer.Enabled = false;

        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            int i;

            for (i = 0; i < NUM_EDIT_GROUPS; i++)
            {
                _pPrintCopyText[i].Text = sConstCopiesGroupsText[i];

                _iGroupsColor[i] = 0;
                _pCheckBoxCopia[i].BackColor = GetColor(_iGroupsColor[i])[0];
                _pCheckBoxCopia[i].ForeColor = GetColor(_iGroupsColor[i])[1];
            }

            for (i = 0; i < NUM_GROUPS_COLORS - 1; i++)
            {
                _pTextBoxColor[i].Text = sConstColorsGroupsText[i];
            }
        }

        // mette a posto l'aspetto
        private void Timer_Tick(object sender, EventArgs e)
        {
            int i, j;

            bool[] bFirstColorPrinterFound = new bool[NUM_EDIT_GROUPS];

            for (j = 0; j < NUM_EDIT_GROUPS; j++)
                bFirstColorPrinterFound[j] = false;

            _sDefaultPrinter = _settings.PrinterName;

            for (i = 0; i < NUM_EDIT_GROUPS; i++)
            {
                if (!bFirstColorPrinterFound[i])
                {
                    if (_pCheckBoxCopia[i].Checked)
                    {
                        // abilitato solo se CASSA_PRINCIPALE e bUSA_NDB()
                        _pCheckBox_BCD[i].Enabled = DataManager.CheckIf_CassaPri_and_NDB();
                        _pPrintersListCombo[i].Enabled = true;

                        _pPrintCopyText[i].BackColor = System.Drawing.SystemColors.Window;
                        _pBtnPrintCheck[i].Enabled = true;
                    }
                    else
                    {
                        _pCheckBox_BCD[i].Enabled = false;
                        _pPrintersListCombo[i].Enabled = false;

                        _pPrintCopyText[i].BackColor = System.Drawing.Color.AntiqueWhite;
                        _pBtnPrintCheck[i].Enabled = false;
                    }

                    bFirstColorPrinterFound[i] = true;

                    // estende le selezioni a tutti i gruppi dello stesso colore
                    // solo per colori diversi dal grigio rende non modificabili
                    // le stampanti successiva alla prima, associate allo stesso colore e spuntate
                    if (_iGroupsColor[i] > 0)
                    {
                        for (j = i + 1; j < NUM_EDIT_GROUPS; j++)
                        {
                            if (_iGroupsColor[i] == _iGroupsColor[j])
                            {
                                bFirstColorPrinterFound[j] = true;

                                _pCheckBoxCopia[j].Checked = _pCheckBoxCopia[i].Checked;
                                _pPrintCopyText[j].BackColor = _pPrintCopyText[i].BackColor;
                                _pBtnPrintCheck[j].Enabled = false;

                                _pCheckBox_BCD[j].Enabled = false;
                                _pCheckBox_BCD[j].Checked = _pCheckBox_BCD[i].Checked;

                                _pPrintersListCombo[j].Enabled = false;
                                _pPrintersListCombo[j].SelectedIndex = _pPrintersListCombo[i].SelectedIndex;
                            }
                        }
                    }
                }
            }
        }

        private void BtnCheck_Click(object sender, EventArgs e)
        {
            int i;
            String sTmp, sFileToPrint;

            sFileToPrint = BuildSampleText();

            for (i = 0; i < NUM_EDIT_GROUPS; i++)
            {
                if (sender == _pBtnPrintCheck[i])
                {
                    if (PrintNetCopiesConfigDlg.GetPrinterTypeIsWinwows(i))
                        PrintFile(sFileToPrint, sGlbWinPrinterParams, 0, _pPrintersListCombo[i].Text);
                    else
                        PrintFile(sFileToPrint, sGlbLegacyPrinterParams, (int)PRINT_QUEUE_ACTION.PRINT_NOW);

                    sTmp = String.Format("Mainform : printSampleText_c({0}) {1}", i, sFileToPrint);
                    LogToFile(sTmp);
                }
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;
        }

        private void CheckBoxCopia_MouseClick(object sender, MouseEventArgs e)
        {
#pragma warning disable IDE0059

            int i, iActualIndex;

            for (i = 0; i < NUM_EDIT_GROUPS; i++)
            {
                if (sender == _pCheckBoxCopia[i])
                {
                    // premuto Ctrl
                    if ((Control.ModifierKeys & Keys.Control) != 0)
                    {
                        _iGroupsColor[i] = (_iGroupsColor[i] + 1) % NUM_GROUPS_COLORS;
                        _pCheckBoxCopia[i].BackColor = GetColor(_iGroupsColor[i])[0];
                        _pCheckBoxCopia[i].ForeColor = GetColor(_iGroupsColor[i])[1];
                    }
                    else
                    {
                        _pCheckBoxCopia[i].Checked = !_pCheckBoxCopia[i].Checked;
                    }

                    iActualIndex = i;
                    break;
                }
            }

            // estende selezione a tutti i gruppi dello stesso colore
            for (int j = 0; j < NUM_EDIT_GROUPS; j++)
                if ((_iGroupsColor[i] == _iGroupsColor[j]) && (_iGroupsColor[i] > 0) && (i != j))
                    _pCheckBoxCopia[j].Checked = _pCheckBoxCopia[i].Checked;
        }

        private void CheckBoxBCD_MouseClick(object sender, MouseEventArgs e)
        {
            int i, iActualIndex = 0;

            for (i = 0; i < NUM_EDIT_GROUPS; i++)
            {
                if (sender == _pCheckBox_BCD[i])
                {
                    _pCheckBox_BCD[i].Checked = !_pCheckBox_BCD[i].Checked;
                    iActualIndex = i;
                    break;
                }
            }
        }

        /**************************************************
            letture dai controlli dalle variabili per la
            scrittura nel registro
         **************************************************/
        private void BtnOK_Click(object sender, EventArgs e)
        {
            int i, iBarcodeRichiestoTmp;
            String sTmp;
            String[] sQueue_Object = new String[2];

            _bListinoModificato = false;

            // controllo lunghezza minima del testo di descrizione dei gruppi
            for (i = 0; i < NUM_EDIT_GROUPS; i++)
            {
                // trim() opportuno
                _pPrintCopyText[i].Text = _pPrintCopyText[i].Text.Trim();

                if (_pPrintCopyText[i].Text.Length < MIN_COPIES_CHARS)
                {
                    _WrnMsg.sMsg = String.Format("{0}", _pPrintCopyText[i].Text);
                    _WrnMsg.iErrID = WRN_LTE;
                    WarningManager(_WrnMsg);

                    return;
                }
            }

            // controllo lunghezza minima del testo di descrizione dei colori
            for (i = 0; i < NUM_GROUPS_COLORS - 1; i++)
            {
                _pTextBoxColor[i].Text = _pTextBoxColor[i].Text.Trim();

                if (_pTextBoxColor[i].Text.Length < MIN_COPIES_CHARS)
                {
                    _WrnMsg.sMsg = String.Format("{0}", _pTextBoxColor[i].Text);
                    _WrnMsg.iErrID = WRN_LTE;
                    WarningManager(_WrnMsg);

                    return;
                }
            }

            //scrittura nel registro del PC locale, ma non nel Listino condiviso
            for (i = 0; i < NUM_EDIT_GROUPS; i++)
            {
                sGlbWinPrinterParams.sPrinterModel[i] = _pPrintersListCombo[i].Text;
                sGlbWinPrinterParams.iPrinterModel[i] = _pPrintersListCombo[i].SelectedIndex;

                sTmp = String.Format(WIN_CPY_PRINTER_MODEL_MKEY, i);
                WriteRegistry(sTmp, sGlbWinPrinterParams.sPrinterModel[i]);
            }

            iBarcodeRichiestoTmp = 0;
            for (i = 0; i < NUM_EDIT_GROUPS; i++)
            {
                if (_pCheckBox_BCD[i].Checked)
                    iBarcodeRichiestoTmp += (int)Math.Pow(2, i); // 0x000000FF
            }

            // controllo _bListinoModificato per barcode, salvataggio in : SF_Data[]
            if (SF_Data.iBarcodeRichiesto != iBarcodeRichiestoTmp)
            {
                _bListinoModificato = true;

                SF_Data.iBarcodeRichiesto = iBarcodeRichiestoTmp;
            }

            // controllo _bListinoModificato a gruppi
            for (i = 0; i < NUM_EDIT_GROUPS; i++)
            {
                // controllo _bListinoModificato a gruppi, salvataggio in : SF_Data[]
                if ((SF_Data.bCopiesGroupsFlag[i] != _pCheckBoxCopia[i].Checked) || (SF_Data.sCopiesGroupsText[i] != _pPrintCopyText[i].Text))
                {
                    _bListinoModificato = true;

                    SF_Data.bCopiesGroupsFlag[i] = _pCheckBoxCopia[i].Checked;
                    SF_Data.sCopiesGroupsText[i] = _pPrintCopyText[i].Text;
                }
            }

            // controllo modifica colori
            for (i = 0; i < NUM_EDIT_GROUPS; i++)
            {
                if (SF_Data.iGroupsColor[i] != _iGroupsColor[i])
                {
                    _bListinoModificato = true;

                    SF_Data.iGroupsColor[i] = _iGroupsColor[i];
                }
            }

            // controllo modifica testi colori
            for (i = 0; i < NUM_GROUPS_COLORS - 1; i++)
            {
                if (SF_Data.sColorGroupsText[i] != _pTextBoxColor[i].Text)
                {
                    _bListinoModificato = true;

                    SF_Data.sColorGroupsText[i] = _pTextBoxColor[i].Text;
                }
            }

            timer.Enabled = false;
            LogToFile("PrintConfigDlg OK");

            Close();
        }

    }
}

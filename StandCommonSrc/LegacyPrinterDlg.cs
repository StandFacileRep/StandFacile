/*****************************************************************************
	NomeFile : StandCommonSrc/ThermPrinterDlg.cs
    Data	 : 06.12.2024
	Autore   : Mauro Artuso
 ****************************************************************************/

using System;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

using StandCommonFiles;
using static StandCommonFiles.ComDef;
using static StandCommonFiles.CommonCl;
using static StandCommonFiles.LogServer;
using static StandCommonFiles.Printer_Legacy;

using static StandFacile.glb;

namespace StandFacile
{
    /// <summary>
    /// classe per la gestione della Form per l'impostazione dei parametri <br/>
    ///  delle stampanti, tutti i dati vengono memorizzati nel registro <br/>
    /// su pressione del tasto OK, la struct sPrinterParam consente di passare <br/>
    /// tutti i parametri alla classe Print_Legacy
    /// </summary>
    public partial class LegacyPrinterDlg : Form
    {
        /// <summary>possibili dimensioni della carta</summary>
        public enum PAPER_SIZE
        {
            /// <summary>enum carta da 57mm</summary>
            S57MM = 0,
            /// <summary>enum carta da 80mm</summary>
            S80MM
        };

        /// <summary>possibi velocità di stampa</summary>
        public enum RANGE
        {
            /// <summary>enum velocità bassa</summary>
            LOW = 0,
            /// <summary>enum velocità media</summary>
            MEDIUM,
            /// <summary>enum velocità alta</summary>
            HIGH
        };

        /// <summary>controllo di flusso</summary>
        public enum FLOW_CONTROL
        {
            /// <summary>enum nessun controllo</summary>
            FLOW_NONE = 0,
            /// <summary>enum controllo RTS/CTS</summary>
            RTS_CTS,
            /// <summary>enum controllo XON/XOFF</summary>
            XON_XOFF
        };

        private bool _bInitComplete = false;

        static TLegacyPrinterParams _sLegacyPrinterParamsCopy = new TLegacyPrinterParams();

        /// <summary>riferimento a LegacyPrinterDlg</summary>
        public static LegacyPrinterDlg rThermPrinterDlg;

        TErrMsg _ErrMsg;

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern SafeFileHandle CreateFile(string lpFileName, FileAccess dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, FileMode dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

        /// <summary>costruttore</summary>
        public LegacyPrinterDlg()
        {
            InitializeComponent();

            rThermPrinterDlg = this;

            /************************************************************
             *  i Combo devono essere popolati preventivamente
             *  prima di impostare il SelectedIndex con il max di voci
             ************************************************************/

            PrinterTypeCombo.Items.Clear();

            for (int i = 0; i < MAX_LEGACY_MODELS; i++)
                PrinterTypeCombo.Items.Add(sConstLegacyModels[i]);

            LogoBmpCombo.Items.Clear();
            LogoBmpCombo.Items.Add("nessun Logo");
            LogoBmpCombo.Items.Add("Logo bmp 1");
            LogoBmpCombo.Items.Add("Logo bmp 2");
            LogoBmpCombo.Items.Add("Logo bmp 3");
            LogoBmpCombo.Items.Add("Logo bmp 4");

            FontTypeCombo.Items.Clear();
            FontTypeCombo.Items.Add("LP2844 : Font originali");
            FontTypeCombo.Items.Add("LP2844 : Soft Font \"a\"");
            FontTypeCombo.Items.Add("LP2844 : Soft Font \"b\"");
            FontTypeCombo.Items.Add("LP2844 : Soft Font \"c\"");
            FontTypeCombo.Items.Add("LP2844 : Soft Font \"d\"");

            Init(false);
        }

         /// <summary>
         /// Inizializzazione con lettura dal Registro
         /// </summary>
        public void Init(bool bShow)
        {
            // 8
            sGlbLegacyPrinterParams.iPrinterModel = ReadRegistry(LEGACY_PRINTER_MODEL_KEY, 0);
            sGlbLegacyPrinterParams.iLogoBmp = ReadRegistry("iLegacyLogoBmp", 0);
            sGlbLegacyPrinterParams.iFontType = ReadRegistry("iLegacyFontType", 0);
            sGlbLegacyPrinterParams.iFlowCtrl = ReadRegistry("iLegacyFlowCtrl", (int)FLOW_CONTROL.FLOW_NONE);
            sGlbLegacyPrinterParams.iPaperSize = ReadRegistry("iLegacyPaperSize", 1);
            sGlbLegacyPrinterParams.iPaperSpeed = ReadRegistry("iLegacyPaperSpeed", 0);
            sGlbLegacyPrinterParams.iDensity = ReadRegistry("iLegacyDensity", 0);
            sGlbLegacyPrinterParams.sPort = ReadRegistry("sLegacyPort", "COM1");

            // *** copia locale ***
            _sLegacyPrinterParamsCopy = sGlbLegacyPrinterParams;

            if (sGlbLegacyPrinterParams.iPrinterModel < PrinterTypeCombo.Items.Count) // controllo sul range 
                PrinterTypeCombo.SelectedIndex = sGlbLegacyPrinterParams.iPrinterModel;
            else
            {
                PrinterTypeCombo.SelectedIndex = 0;
                sGlbLegacyPrinterParams.iPrinterModel = 0;
            }

            if (sGlbLegacyPrinterParams.iLogoBmp < LogoBmpCombo.Items.Count) // controllo sul range
                LogoBmpCombo.SelectedIndex = sGlbLegacyPrinterParams.iLogoBmp;
            else
                LogoBmpCombo.SelectedIndex = 0;

            if (sGlbLegacyPrinterParams.iFontType < FontTypeCombo.Items.Count) // controllo sul range
                FontTypeCombo.SelectedIndex = sGlbLegacyPrinterParams.iFontType;
            else
                FontTypeCombo.SelectedIndex = 0;

            if (PrinterTypeCombo.SelectedIndex == (int)LEGACY_PRINTER_MODELS.STAMPANTE_TM_T88_SER)
                flow_XON_XOFF.Enabled = true;
            else
                flow_XON_XOFF.Enabled = false;

            switch (sGlbLegacyPrinterParams.iFlowCtrl)
            {
                case (int)FLOW_CONTROL.RTS_CTS:
                    flow_RTS_CTS.Checked = true;
                    break;
                case (int)FLOW_CONTROL.XON_XOFF:
                    flow_XON_XOFF.Checked = true;
                    break;
                default:
                    flow_NONE.Checked = true;
                    break;
            }

            switch (sGlbLegacyPrinterParams.iPaperSize)
            {
                case (int)PAPER_SIZE.S57MM:
                    width57.Checked = true;
                    break;
                case (int)PAPER_SIZE.S80MM:
                    width80.Checked = true;
                    break;
                default:
                    width80.Checked = true;
                    break;
            }

            switch (sGlbLegacyPrinterParams.iPaperSpeed)
            {
                case (int)RANGE.LOW:
                    speedBassa.Checked = true;
                    break;
                case (int)RANGE.HIGH:
                    speedAlta.Checked = true;
                    break;
                default:
                    speedMedia.Checked = true;
                    break;
            }

            switch (sGlbLegacyPrinterParams.iDensity)
            {
                case (int)RANGE.LOW:
                    densBassa.Checked = true;
                    break;
                case (int)RANGE.HIGH:
                    densAlta.Checked = true;
                    break;
                default:
                    densMedia.Checked = true;
                    break;
            }

            FindPorts();

            PORT_Combo.Text = sGlbLegacyPrinterParams.sPort;

            // si sono inizializzati i Combo
            _bInitComplete = true;

            AggiornaAspettoControlli();

            if ((iSysPrinterType == (int)PRINTER_SEL.STAMPANTE_LEGACY) || bShow)
            {
                if (!Printer_Legacy.PortVerify(sGlbLegacyPrinterParams))
                {
                    _ErrMsg.sMsg = ReadRegistry("sLegacyPort", "COMx");
                    _ErrMsg.iErrID = WRN_CNA;
                    WarningManager(_ErrMsg);
                }
            }

            if (bShow)
                // no ShowDialog altrimenti si perde il Focus su eventuali MessageDlg 
                // che quindi non si riesce più a chiudere
                rThermPrinterDlg.Show();

            return;
        }

         /// <summary>
         /// funzione che inposta tutti i parametri necessari
         /// alla classe prelevandoli dai controlli e non dal Registro
         /// </summary>
        public void UpdateLegacyPrinterParam()
        {
            _sLegacyPrinterParamsCopy.iPrinterModel = PrinterTypeCombo.SelectedIndex;
            _sLegacyPrinterParamsCopy.iLogoBmp = LogoBmpCombo.SelectedIndex;
            _sLegacyPrinterParamsCopy.iFontType = FontTypeCombo.SelectedIndex;

            _sLegacyPrinterParamsCopy.sPort = PORT_Combo.Text;

            if (flow_RTS_CTS.Checked)
                _sLegacyPrinterParamsCopy.iFlowCtrl = (int)FLOW_CONTROL.RTS_CTS;
            else if (flow_XON_XOFF.Checked)
                _sLegacyPrinterParamsCopy.iFlowCtrl = (int)FLOW_CONTROL.XON_XOFF;
            else
                _sLegacyPrinterParamsCopy.iFlowCtrl = (int)FLOW_CONTROL.FLOW_NONE;

            if (width57.Checked)
                _sLegacyPrinterParamsCopy.iPaperSize = (int)PAPER_SIZE.S57MM;
            else
                _sLegacyPrinterParamsCopy.iPaperSize = (int)PAPER_SIZE.S80MM;

            if (speedBassa.Checked)
                _sLegacyPrinterParamsCopy.iPaperSpeed = (int)RANGE.LOW;
            else if (speedAlta.Checked)
                _sLegacyPrinterParamsCopy.iPaperSpeed = (int)RANGE.HIGH;
            else
                _sLegacyPrinterParamsCopy.iPaperSpeed = (int)RANGE.MEDIUM;

            if (densBassa.Checked)
                _sLegacyPrinterParamsCopy.iDensity = (int)RANGE.LOW;
            else if (densAlta.Checked)
                _sLegacyPrinterParamsCopy.iDensity = (int)RANGE.HIGH;
            else
                _sLegacyPrinterParamsCopy.iDensity = (int)RANGE.MEDIUM;
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            // AuxRefresh lettura controlli
            UpdateLegacyPrinterParam();

            // acquisizione impostazioni
            sGlbLegacyPrinterParams = _sLegacyPrinterParamsCopy;

            // 8 scritture nel registro
            WriteRegistry(LEGACY_PRINTER_MODEL_KEY, sGlbLegacyPrinterParams.iPrinterModel);
            WriteRegistry("iLegacyLogoBmp", sGlbLegacyPrinterParams.iLogoBmp);
            WriteRegistry("iLegacyFontType", sGlbLegacyPrinterParams.iFontType);
            WriteRegistry("iLegacyFlowCtrl", sGlbLegacyPrinterParams.iFlowCtrl);
            WriteRegistry("iLegacyPaperSize", sGlbLegacyPrinterParams.iPaperSize);
            WriteRegistry("iLegacyPaperSpeed", sGlbLegacyPrinterParams.iPaperSpeed);
            WriteRegistry("iLegacyDensity", sGlbLegacyPrinterParams.iDensity);
            WriteRegistry("sLegacyPort", sGlbLegacyPrinterParams.sPort);

            if (!PortVerify(sGlbLegacyPrinterParams))
            {
                _ErrMsg.sMsg = ReadRegistry("sLegacyPort", "COMx");
                _ErrMsg.iErrID = WRN_CNA;
                WarningManager(_ErrMsg);
            }

            Hide();
            LogToFile("ThermPrinter : OKBtnClick");
        }

        /**************************************************************
         * funzione che aggiorna l'aspetto dei bottoni sulla base delle
         * caratteristiche possibili per ciascuna stampante,
         * come regola i controlli sono stabiliti a mano e non vengono
         * letti dal Registry
         **************************************************************/
        private void AggiornaAspettoControlli()
        {
            // altrimenti viene percorsa inizialmente con dati sbagliati
            if (!_bInitComplete)
                return;

            PORT_Combo.Text = _sLegacyPrinterParamsCopy.sPort;

            switch (PrinterTypeCombo.SelectedIndex)
            {
                case (int)LEGACY_PRINTER_MODELS.STAMPANTE_TM_T88_SER:
                case (int)LEGACY_PRINTER_MODELS.STAMPANTE_TM_L90_LPT:

                    width80.Checked = true;
                    PaperSizeGroupBox.Enabled = false;

                    LogoBmpCombo.Enabled = true;
                    FontTypeCombo.Enabled = true;

                    if (!FontTypeCombo.SelectedItem.ToString().Contains("TM-T88"))
                    {
                        FontTypeCombo.Items.Clear();
                        FontTypeCombo.Items.Add("TM-T88 : Font B  9x17 x2");
                        FontTypeCombo.Items.Add("TM-T88 : Font A 12x24 x1");
                        FontTypeCombo.SelectedIndex = 0;
                    }

                    if (PrinterTypeCombo.SelectedIndex == (int)LEGACY_PRINTER_MODELS.STAMPANTE_TM_T88_SER)
                    {
                        Lbl_ImpSer.Enabled = true;
                        SettingsGroupBox.Enabled = true;

                        FlowRadio.Enabled = true;
                        flow_NONE.Enabled = true;
                        flow_RTS_CTS.Enabled = true;
                        flow_XON_XOFF.Enabled = true;

                        if (flow_RTS_CTS.Checked)
                            _sLegacyPrinterParamsCopy.iFlowCtrl = (int)FLOW_CONTROL.RTS_CTS;
                        else if (flow_XON_XOFF.Checked)
                            _sLegacyPrinterParamsCopy.iFlowCtrl = (int)FLOW_CONTROL.XON_XOFF;
                        else
                            _sLegacyPrinterParamsCopy.iFlowCtrl = (int)FLOW_CONTROL.FLOW_NONE;

                    }
                    else
                    {
                        Lbl_ImpSer.Enabled = false;
                        SettingsGroupBox.Enabled = false;

                        FlowRadio.Enabled = false;
                        flow_NONE.Enabled = false;
                        flow_RTS_CTS.Enabled = false;
                        flow_XON_XOFF.Enabled = false;
                    }

                    speedGroupBox.Enabled = false;
                    speedMedia.Checked = true;

                    densityGroupBox.Enabled = false;
                    densMedia.Checked = true;

                    btnInfo.Enabled = false;
                    btnAutotest.Enabled = true;

                    break;

                case (int)LEGACY_PRINTER_MODELS.STAMPANTE_LP2844_PAGEMODE_SER:
                case (int)LEGACY_PRINTER_MODELS.STAMPANTE_LP2844_PAGEMODE_LPT:

                    PaperSizeGroupBox.Enabled = true;
                    LogoBmpCombo.Enabled = true;

                    if (!FontTypeCombo.SelectedItem.ToString().Contains("LP2844"))
                    {
                        FontTypeCombo.Items.Clear();

                        if (width80.Checked)
                        {
                            FontTypeCombo.Items.Add("LP2844 : Font originali");
                            FontTypeCombo.Items.Add("LP2844 : Soft Font \"a\"");
                            FontTypeCombo.Items.Add("LP2844 : Soft Font \"b\"");
                            FontTypeCombo.Items.Add("LP2844 : Soft Font \"c\"");
                            FontTypeCombo.Items.Add("LP2844 : Soft Font \"d\"");
                            FontTypeCombo.Enabled = true;
                        }
                        else
                        {
                            FontTypeCombo.Items.Add("LP2844 : Font standard");
                            FontTypeCombo.Enabled = false;
                        }

                        FontTypeCombo.SelectedIndex = 0;
                    }

                    if (PrinterTypeCombo.SelectedIndex == (int)LEGACY_PRINTER_MODELS.STAMPANTE_LP2844_PAGEMODE_SER)
                    {
                        Lbl_ImpSer.Enabled = true;
                        SettingsGroupBox.Enabled = true;

                        FlowRadio.Enabled = true;
                        flow_NONE.Enabled = true;
                        flow_RTS_CTS.Enabled = true;
                        flow_XON_XOFF.Enabled = false;

                        if (flow_RTS_CTS.Checked)
                            _sLegacyPrinterParamsCopy.iFlowCtrl = (int)FLOW_CONTROL.RTS_CTS;
                        else
                            _sLegacyPrinterParamsCopy.iFlowCtrl = (int)FLOW_CONTROL.FLOW_NONE;
                    }
                    else
                    {
                        Lbl_ImpSer.Enabled = false;
                        SettingsGroupBox.Enabled = false;

                        FlowRadio.Enabled = false;
                        flow_NONE.Enabled = false;
                        flow_RTS_CTS.Enabled = false;
                        flow_XON_XOFF.Enabled = false;

                        flow_NONE.Checked = true;
                    }

                    speedGroupBox.Enabled = true;
                    densityGroupBox.Enabled = true;

                    btnInfo.Enabled = true;
                    btnAutotest.Enabled = true;

                    break;
                default:
                    flow_NONE.Enabled = false;
                    flow_RTS_CTS.Enabled = false;
                    flow_XON_XOFF.Enabled = false;
                    PORT_Combo.Enabled = false;

                    PaperSizeGroupBox.Enabled = false;
                    LogoBmpCombo.Enabled = false;

                    speedGroupBox.Enabled = false;
                    densityGroupBox.Enabled = false;

                    btnInfo.Enabled = false;
                    btnAutotest.Enabled = false;
                    break;
            }
        }

        // funzione chiamata da variazioni di qualsiasi controllo
        private void PrinterTypeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_bInitComplete)
                return;

            FontTypeCombo.Items.Clear();
            FontTypeCombo.Items.Add("FONT RESET");
            FontTypeCombo.SelectedIndex = 0;

            UpdateLegacyPrinterParam();

            AggiornaAspettoControlli();

            FindPorts();

            // reset selezione
            if (PORT_Combo.Items.Count > 0)
                PORT_Combo.SelectedIndex = 0;
        }

        private void BtnAutotest_Click(object sender, EventArgs e)
        {
            UpdateLegacyPrinterParam();

            Printer_Legacy.PrintAutoTest();
        }

        private void BtnTestoProva_Click(object sender, EventArgs e)
        {
            UpdateLegacyPrinterParam();

            Printer_Legacy.PrintSampleText(_sLegacyPrinterParamsCopy);
        }

        private void BtnInfo_Click(object sender, EventArgs e)
        {
            UpdateLegacyPrinterParam();

            Printer_Legacy.PrintInfo();
        }

        private void FindPorts()
        {
            // ***** ricerca COMs *****

            string[] sCOM_PortName = SerialPort.GetPortNames();

            if (Lbl_ImpSer.Enabled)
            {
                PORT_Combo.Items.Clear();
                foreach (string port in sCOM_PortName)

                    PORT_Combo.Items.Add(port);
            }
            else
            {
                PORT_Combo.Items.Clear();

                // ***** ricerca LPTs (non c'è la classe ParallelPort) *****
                for (int i = 1; i < 10; i++)
                {
                    String sLPT_PortName = String.Format("LPT{0}", i);

                    bool bAvailable = false;

                    // Try to open the port
                    SafeFileHandle hLPT = CreateFile(sLPT_PortName, FileAccess.Write, 0, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);

                    if (!hLPT.IsInvalid)
                    {
                        //bInstalled = true;
                        bAvailable = true;
                        hLPT.Close();
                    }

                    if (bAvailable)
                    {
                        sLPT_PortName = String.Format("LPT{0}", i);
                        PORT_Combo.Items.Add(sLPT_PortName);
                    }
                }
            }
        }

        private void ThermPrinterDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            // evita la distruzione della form creando una eccezione alla successiva apertura
            e.Cancel = true;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Hide();
        }
    }
}



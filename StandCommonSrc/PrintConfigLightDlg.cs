/*****************************************************
 	NomeFile : StandCommonSrc/PrintConfigLightDlg.cs
    Data	 : 06.12.2024
 	Autore   : Mauro Artuso

 *****************************************************/

using System;
using System.Windows.Forms;

using static StandFacile.glb;

using static StandCommonFiles.CommonCl;
using static StandCommonFiles.ComDef;
using static StandCommonFiles.LogServer;
using static StandCommonFiles.Printer_Legacy;

namespace StandFacile
{
    /// <summary>
    /// Classe per la configurazione delle copie di stampa
    /// </summary>
    public partial class PrintConfigLightDlg : Form
    {
        /// <summary>riferimento a PrintConfigLightDlg</summary>
        public static PrintConfigLightDlg rPrintConfigLightDlg;

        TErrMsg _ErrMsg;

        readonly ToolTip _tt = new ToolTip
        {
            InitialDelay = 50,
            ShowAlways = true
        };

        /*****************************************************
            vanno restituite solo variabili e non controlli,
            che potrebbero non essere confermati con OK
         *****************************************************/

        /// <summary>ottiene true se la stampante è windows</summary>
        public static bool GetPrinterTypeIsWinwows() { return (iSysPrinterType == (int)PRINTER_SEL.STAMPANTE_WINDOWS); }

        /// <summary>costruttore</summary>
        public PrintConfigLightDlg()
        {
            InitializeComponent();

            rPrintConfigLightDlg = this;

            _tt.SetToolTip(BtnWin, "imposta stampante Windows: USB, LAN, WiFi");
            _tt.SetToolTip(BtnGeneric, "impostazioni Generiche stampa");
            _tt.SetToolTip(BtnLegacy, "imposta stampante Legacy: COM, LPT");

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

            if ((iPrinterTypeRadio == (int)PRINTER_SEL.STAMPANTE_WINDOWS) || CheckService(CFG_COMMON_STRINGS._HIDE_LEGACY_PRINTER))
            {
                prt_Windows.Checked = true;
                iSysPrinterType = (int)PRINTER_SEL.STAMPANTE_WINDOWS;
            }
            else
            {
                prt_Legacy.Checked = true;
                iSysPrinterType = (int)PRINTER_SEL.STAMPANTE_LEGACY;
            }

            if (bShow)
                ShowDialog();
        }

        /**************************************************
	        letture dai controlli dalle variabili per la
	        scrittura nel registro
         **************************************************/
        private void BtnOK_Click(object sender, EventArgs e)
        {

            if (prt_Windows.Checked || CheckService(CFG_COMMON_STRINGS._HIDE_LEGACY_PRINTER))
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

                // Verify
                switch (sGlbLegacyPrinterParams.iPrinterModel)
                {
                    case (int)LEGACY_PRINTER_MODELS.STAMPANTE_TM_T88_SER:

                        if (!PortVerify(sGlbLegacyPrinterParams))
                        {
                            _ErrMsg.sMsg = ReadRegistry("sPort", "COM1");
                            _ErrMsg.iErrID = WRN_CNA;
                            WarningManager(_ErrMsg);
                        }
                        break;
                }
            }

            LogToFile("PrintConfigLightDlg OK");
        }

        private void BtnGeneric_Click(object sender, EventArgs e)
        {
            GenPrinterDlg rGenericPrintDlg = new GenPrinterDlg();
        }

        private void BtnLegacy_Click(object sender, EventArgs e)
        {
            LegacyPrinterDlg.rThermPrinterDlg.Init(true);
        }

        private void BtnWin_Click(object sender, EventArgs e)
        {
            WinPrinterDlg._rWinPrinterDlg.Init(true);
        }

    }
}

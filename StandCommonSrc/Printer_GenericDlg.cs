/*******************************************************************************
	NomeFile : StandCommonSrc\Printer_GenericDlg.cs
    Data	 : 21.12.2025
	Autore   : Mauro Artuso/nicola02nb

	Descrizione : classe per la gestione della Form per l'impostazione dei
                  parametri generici delle stampanti
 *******************************************************************************/

using System;
using System.Windows.Forms;

using StandCommonFiles;
using static StandCommonFiles.ComDef;
using static StandCommonFiles.CommonCl;
using static StandCommonFiles.LogServer;

using static StandFacile.dBaseIntf;
using static StandFacile.Define;
using static StandFacile.glb;

namespace StandFacile
{
    /// <summary>
    /// classe per l'impostazione dei parametri generici di stampa
    /// </summary>
    public partial class GenPrinterDlg : Form
    {
#pragma warning disable IDE0044

        readonly ToolTip _tt = new ToolTip
        {
            InitialDelay = 50,
            ShowAlways = true
        };

        static bool _bListinoModificato;
        static int _iGenericPrintOptions_Copy;

        /// <summary>ottiene flag di modifica listino necessaria</summary>
        public static bool GetListinoModificato() { return _bListinoModificato; }

        /// <summary>riferimento a GenPrinterDlg</summary>
        public static GenPrinterDlg _rGenPrinterDlg;

        /// <summary>costruttore</summary>
        public GenPrinterDlg()
        {
            InitializeComponent();

            _rGenPrinterDlg = this;

            _tt.SetToolTip(checkBox_Chars33, "con 33 caratteri le descrizioni Articoli sono migliori ma le stampe più piccole\r\n" +
                                          "sconsigliato per formato carta da 57mm");

            LogToFile("GenPrinterDlg : Init in");

#if STANDFACILE

            // se cassa secondaria e DB si può scegliere
            if (DataManager.CheckIf_CassaSec_and_NDB())
            {
                Height = 352;
                ckBoxLocalSettings.Visible = true;
                ckBoxLocalSettings.Checked = (ReadRegistry(GEN_PRINT_LOC_STORE_KEY, 0) == 1);

                CheckBoxLocalSettings_CheckedChanged(this, null);
            }
            else
            {
                Height = 300;
                ckBoxLocalSettings.Visible = false;
                ckBoxLocalSettings.Checked = false;
            }

#else
            // se STAND_MONITOR || STAND_CUCINA si può scegliere
            Height = 352;
            ckBoxLocalSettings.Visible = true;
            ckBoxLocalSettings.Checked = (ReadRegistry(GEN_PRINT_LOC_STORE_KEY, 0) == 1);

            CheckBoxLocalSettings_CheckedChanged(this, null);
#endif

            Init(false);

            LogToFile("GenPrinterDlg : Init out");

            return;
        }

        /// <summary>
        /// inizializza i controlli dal SF_Data (HeadOrdini) o dal registro
        /// </summary>
        public void Init(bool bShow)
        {
            int iGenericPrintOptions;


            if (ckBoxLocalSettings.Checked)
            {
                iGenericPrintOptions = ReadRegistry(GEN_PRINT_OPT_KEY, 1);
            }
            else
            {
                // copia impostazioni di stampa
#if STANDFACILE
                iGenericPrintOptions = SF_Data.iGenericPrintOptions;
#else
                iGenericPrintOptions = DB_Data.iGenericPrintOptions;
#endif

            }

#if STANDFACILE
            _iGenericPrintOptions_Copy = SF_Data.iGenericPrintOptions;
#else
            _iGenericPrintOptions_Copy = DB_Data.iGenericPrintOptions;
#endif

            // caricato comunque dal DB
            checkBox_Chars33.Checked = IsBitSet(_iGenericPrintOptions_Copy, (int)GEN_PRINTER_OPTS.BIT_CHARS33_PRINT_REQUIRED);
            checkBox_CopertiNelleCopie.Checked = IsBitSet(_iGenericPrintOptions_Copy, (int)GEN_PRINTER_OPTS.BIT_PLACESETTS_PRINT_ON_COPIES_REQUIRED);
            checkBox_CassaInlineNumero.Checked = IsBitSet(_iGenericPrintOptions_Copy, (int)GEN_PRINTER_OPTS.BIT_CASSA_INLINE);
            checkBox_CenterTableAndName.Checked = IsBitSet(_iGenericPrintOptions_Copy, (int)GEN_PRINTER_OPTS.BIT_CENTER_TABLE_AND_NAME);
            checkBox_StarsOnUnderGroup.Checked = IsBitSet(_iGenericPrintOptions_Copy, (int)GEN_PRINTER_OPTS.BIT_STARS_ON_UNDER_GROUP);

            // caricato dal DB o da impostazioni locali
            checkBox_LogoNelleCopie.Checked = IsBitSet(iGenericPrintOptions, (int)GEN_PRINTER_OPTS.BIT_LOGO_PRINT_ON_COPIES_REQUIRED);

            numUpDown_RigheIniziali.Value = GetNumberOfSetBits(iGenericPrintOptions, (int)GEN_PRINTER_OPTS.BIT_EMPTY_ROWS_INITIAL, 4);
            numUpDown_RigheFinali.Value = GetNumberOfSetBits(iGenericPrintOptions, (int)GEN_PRINTER_OPTS.BIT_EMPTY_ROWS_FINAL, 4);

            if (bShow)
                ShowDialog();
        }

        /// <summary>
        /// funzione chiamata da STAND_CUCINA || STAND_MONITOR <br/>
        /// oppure da STAND_FACILE mo solo CASSA_SECONDARIA
        /// </summary>
        private void CheckBoxLocalSettings_CheckedChanged(object sender, EventArgs e)
        {
            if (ckBoxLocalSettings.Checked)
            {
#if STANDFACILE
                if (DataManager.CheckIf_CassaSec_and_NDB())
                {
                    checkBox_Chars33.Enabled = false;
                    checkBox_CopertiNelleCopie.Enabled = false;
                    checkBox_CassaInlineNumero.Enabled = false;
                    checkBox_CenterTableAndName.Enabled = false;
                    checkBox_StarsOnUnderGroup.Enabled = false;
                }
                else
#endif
                {
                    checkBox_Chars33.Enabled = true;
                    checkBox_CopertiNelleCopie.Enabled = true;
                    checkBox_CassaInlineNumero.Enabled = true;
                    checkBox_CenterTableAndName.Enabled = true;
                    checkBox_StarsOnUnderGroup.Enabled = true;
                }

                checkBox_LogoNelleCopie.Enabled = true;

                labelEmptyInitial.Enabled = true;
                labelEmptyFinal.Enabled = true;
                numUpDown_RigheIniziali.Enabled = true;
                numUpDown_RigheFinali.Enabled = true;
            }
            else
            {
                checkBox_Chars33.Enabled = false;
                checkBox_CopertiNelleCopie.Enabled = false;
                checkBox_LogoNelleCopie.Enabled = false;
                checkBox_CassaInlineNumero.Enabled = false;
                checkBox_CenterTableAndName.Enabled = false;
                checkBox_StarsOnUnderGroup.Enabled = false;

                labelEmptyInitial.Enabled = false;
                labelEmptyFinal.Enabled = false;
                numUpDown_RigheIniziali.Enabled = false;
                numUpDown_RigheFinali.Enabled = false;
            }

            Init(false);
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            bool bChars33;
            int iGenPrinterOptionsCopy;

            _bListinoModificato = false;

            iGenPrinterOptionsCopy = 0;

            iGenPrinterOptionsCopy = UpdateBit(iGenPrinterOptionsCopy, checkBox_Chars33.Checked, (int)GEN_PRINTER_OPTS.BIT_CHARS33_PRINT_REQUIRED);
            iGenPrinterOptionsCopy = UpdateBit(iGenPrinterOptionsCopy, checkBox_CopertiNelleCopie.Checked, (int)GEN_PRINTER_OPTS.BIT_PLACESETTS_PRINT_ON_COPIES_REQUIRED);
            iGenPrinterOptionsCopy = UpdateBit(iGenPrinterOptionsCopy, checkBox_LogoNelleCopie.Checked, (int)GEN_PRINTER_OPTS.BIT_LOGO_PRINT_ON_COPIES_REQUIRED);

            iGenPrinterOptionsCopy = UpdateBit(iGenPrinterOptionsCopy, checkBox_CassaInlineNumero.Checked, (int)GEN_PRINTER_OPTS.BIT_CASSA_INLINE);
            iGenPrinterOptionsCopy = UpdateBit(iGenPrinterOptionsCopy, checkBox_StarsOnUnderGroup.Checked, (int)GEN_PRINTER_OPTS.BIT_STARS_ON_UNDER_GROUP);
            iGenPrinterOptionsCopy = UpdateBit(iGenPrinterOptionsCopy, checkBox_CenterTableAndName.Checked, (int)GEN_PRINTER_OPTS.BIT_CENTER_TABLE_AND_NAME);

            iGenPrinterOptionsCopy = SetNumberOfSetBits(iGenPrinterOptionsCopy, (int)numUpDown_RigheIniziali.Value, (int)GEN_PRINTER_OPTS.BIT_EMPTY_ROWS_INITIAL, 4);
            iGenPrinterOptionsCopy = SetNumberOfSetBits(iGenPrinterOptionsCopy, (int)numUpDown_RigheFinali.Value, (int)GEN_PRINTER_OPTS.BIT_EMPTY_ROWS_FINAL, 4);

            if (SF_Data.iGenericPrintOptions != iGenPrinterOptionsCopy)
            {
                _bListinoModificato = true;

                SF_Data.iGenericPrintOptions = iGenPrinterOptionsCopy;
            }

            bChars33 = IsBitSet(SF_Data.iGenericPrintOptions, (int)GEN_PRINTER_OPTS.BIT_CHARS33_PRINT_REQUIRED);
            InitFormatStrings(bChars33);

#if STANDFACILE
            AnteprimaDlg.rAnteprimaDlg.RedrawReceipt();
#endif

            WriteRegistry(GEN_PRINT_LOC_STORE_KEY, ckBoxLocalSettings.Checked ? 1 : 0);

            if (ckBoxLocalSettings.Checked)
                WriteRegistry(GEN_PRINT_OPT_KEY, iGenPrinterOptionsCopy);

            Hide();
            LogToFile("GenPrinterDlg : OKBtnClick");
        }

        private void GenPrinterDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            // evita la distruzione della form creando una eccezione alla successiva apertura
            e.Cancel = true;
        }

    }
}

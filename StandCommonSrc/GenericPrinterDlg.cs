/*******************************************************************************
	NomeFile : StandCommonSrc/GenericPrinterDlg.cs
    Data	 : 22.09.2025
	Autore   : Nicola Bizzotto

	Descrizione : classe per la gestione della Form per l'impostazione dei
                  parametri generici delle stampanti
 *******************************************************************************/

using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Printing;

using StandCommonFiles;
using static StandCommonFiles.ComDef;
using static StandCommonFiles.CommonCl;
using static StandCommonFiles.LogServer;
using static StandFacile.Define;
using static StandFacile.glb;

namespace StandFacile
{
    /// <summary>
    /// classe per l'impostazione dei parametri generici di stampa
    /// </summary>
    public partial class GenericPrinterDlg : Form
    {
#pragma warning disable IDE0044

        static bool _bListinoModificato;

        /// <summary>riferimento a GenericPrinterDlg</summary>
        public static GenericPrinterDlg _rGenericPrinterDlg;

        /// <summary>ottiene flag di modifica listino necessaria</summary>
        public static bool GetListinoModificato() { return _bListinoModificato; }

        readonly ToolTip _tt = new ToolTip
        {
            InitialDelay = 50,
            ShowAlways = true
        };

        /// <summary>costruttore</summary>
        public GenericPrinterDlg()
        {
            InitializeComponent();

            _rGenericPrinterDlg = this;

            _tt.SetToolTip(numUpDown_RigheIniziali, "Numero di righe vuote iniziali");
            _tt.SetToolTip(numUpDown_RigheFinali, "Numero di righe vuote finali");
            _tt.SetToolTip(checkBox_CassaInlineNumero, "Visualizza il numero della cassa inline con numero scontrino");
            _tt.SetToolTip(checkBox_StarOnUnderGroup, "Visualizza l'asterisco sopra e sotto il gruppo");
            _tt.SetToolTip(checkBox_CenterTableAndName, "Centra il tavolo e il nome cliente");
            _tt.SetToolTip(checkBox_LogoNelleCopie, "Stampa il logo anche nelle copie");
            _tt.SetToolTip(checkBox_CopertiNelleCopie, "Stampa il numero dei coperti anche nelle copie");
            _tt.SetToolTip(checkBox_Chars33, "Abilita la stampa su 23 caratteri per linea (33 caratteri)");

            Init(false); // imposta sGlobGenericPrinterParams, VIP
        }

        /// <summary>
        /// Inizializzazione con lettura dal Registro
        /// </summary>
        public bool Init(bool bShow)
        {
            DialogResult result = DialogResult.None;

            LogToFile("GenericPrinterDlg : Init in");

            numUpDown_RigheIniziali.Value = GetNumberOfSetBits(SF_Data.iGenericPrinterOptions, (int)GEN_PRINTER_OPTS.BIT_EMPTY_ROWS_INITIAL, 4);
            numUpDown_RigheFinali.Value = GetNumberOfSetBits(SF_Data.iGenericPrinterOptions, (int)GEN_PRINTER_OPTS.BIT_EMPTY_ROWS_FINAL, 4);
            checkBox_CassaInlineNumero.Checked = IsBitSet(SF_Data.iGenericPrinterOptions, (int)GEN_PRINTER_OPTS.BIT_CASSA_INLINE);
            checkBox_StarOnUnderGroup.Checked = IsBitSet(SF_Data.iGenericPrinterOptions, (int)GEN_PRINTER_OPTS.BIT_STAR_ON_UNDER_GROUP);
            checkBox_CenterTableAndName.Checked = IsBitSet(SF_Data.iGenericPrinterOptions, (int)GEN_PRINTER_OPTS.BIT_CENTER_TABLE_AND_NAME);
            checkBox_LogoNelleCopie.Checked = IsBitSet(SF_Data.iGenericPrinterOptions, (int)GEN_PRINTER_OPTS.BIT_LOGO_PRINT_REQUIRED);
            checkBox_CopertiNelleCopie.Checked = IsBitSet(SF_Data.iGenericPrinterOptions, (int)GEN_PRINTER_OPTS.BIT_PLACESETTS_PRINT_ON_COPIES_REQUIRED);
            checkBox_Chars33.Checked = IsBitSet(SF_Data.iGenericPrinterOptions, (int)GEN_PRINTER_OPTS.BIT_CHARS33_PRINT_REQUIRED);

#if STANDFACILE
            if (DataManager.CheckIf_CassaSec_and_NDB())
#else
            if (true)
#endif
            {
                numUpDown_RigheIniziali.Enabled = false;
                numUpDown_RigheFinali.Enabled = false;
                checkBox_CassaInlineNumero.Enabled = false;
                checkBox_StarOnUnderGroup.Enabled = false;
                checkBox_CenterTableAndName.Enabled = false;
                checkBox_LogoNelleCopie.Enabled = false;
                checkBox_CopertiNelleCopie.Enabled = false;
                checkBox_Chars33.Enabled = false;
            }

            _bListinoModificato = false;

            if (bShow)
                result = ShowDialog();

            return (result == DialogResult.OK); // true se è cliccato OK
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            int iGenericPrinterOptionsCopy;
            String sTmp;

            iGenericPrinterOptionsCopy = 0;

            // acquisizione impostazioni

            iGenericPrinterOptionsCopy = SetNumberOfSetBits(iGenericPrinterOptionsCopy, (int)numUpDown_RigheIniziali.Value, (int)GEN_PRINTER_OPTS.BIT_EMPTY_ROWS_INITIAL, 4);
            iGenericPrinterOptionsCopy = SetNumberOfSetBits(iGenericPrinterOptionsCopy, (int)numUpDown_RigheFinali.Value, (int)GEN_PRINTER_OPTS.BIT_EMPTY_ROWS_FINAL, 4);
            iGenericPrinterOptionsCopy = UpdateBit(iGenericPrinterOptionsCopy, checkBox_CassaInlineNumero.Checked,(int)GEN_PRINTER_OPTS.BIT_CASSA_INLINE);
            iGenericPrinterOptionsCopy = UpdateBit(iGenericPrinterOptionsCopy, checkBox_StarOnUnderGroup.Checked, (int)GEN_PRINTER_OPTS.BIT_STAR_ON_UNDER_GROUP);
            iGenericPrinterOptionsCopy = UpdateBit(iGenericPrinterOptionsCopy, checkBox_CenterTableAndName.Checked, (int)GEN_PRINTER_OPTS.BIT_CENTER_TABLE_AND_NAME);
            iGenericPrinterOptionsCopy = UpdateBit(iGenericPrinterOptionsCopy, checkBox_LogoNelleCopie.Checked, (int)GEN_PRINTER_OPTS.BIT_LOGO_PRINT_REQUIRED);
            iGenericPrinterOptionsCopy = UpdateBit(iGenericPrinterOptionsCopy, checkBox_CopertiNelleCopie.Checked, (int)GEN_PRINTER_OPTS.BIT_PLACESETTS_PRINT_ON_COPIES_REQUIRED);
            iGenericPrinterOptionsCopy = UpdateBit(iGenericPrinterOptionsCopy, checkBox_Chars33.Checked, (int)GEN_PRINTER_OPTS.BIT_CHARS33_PRINT_REQUIRED);

            if (iGenericPrinterOptionsCopy != SF_Data.iGenericPrinterOptions)
            {
                SF_Data.iGenericPrinterOptions = iGenericPrinterOptionsCopy;
                _bListinoModificato = true;
            }
            else
                _bListinoModificato = false;

            if (_bListinoModificato)
            {
#if STANDFACILE
                AnteprimaDlg.rAnteprimaDlg.RedrawReceipt();
#elif STAND_CUCINA
                FrmMain.rFrmMain.VisualizzaTicket();
#endif
            }

            sTmp = String.Format("optionsDlg OK: {0}, {1}, {2}, {3}, {4}", numUpDown_RigheIniziali.Value, numUpDown_RigheFinali.Value,
                checkBox_CassaInlineNumero.Checked, checkBox_StarOnUnderGroup.Checked, checkBox_CenterTableAndName.Checked);
            LogToFile(sTmp);

            LogToFile("GenericPrinterDlg : OKBtnClick");

            Close();
        }

        private void GenericPrinterDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            // evita la distruzione della form creando una eccezione alla successiva apertura
            e.Cancel = true;
        }

    }
}

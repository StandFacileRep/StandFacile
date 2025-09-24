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

        /// <summary>stringa per il salvataggio nel registro del numero di righe iniziali vuote</summary>
        const String EMPTY_ROWS_INITIAL_KEY = "iGenericEmptyRowsInitial";
        /// <summary>stringa per il salvataggio nel registro del numero di righe finali vuote</summary>
        const String EMPTY_ROWS_FINAL_KEY = "iGenericEmptyRowsFinal";
        /// <summary>stringa per il salvataggio nel registro del flag cassa inline con numero ordine</summary>
        const String CASSA_INLINE_WITH_ORDER_NUMBER_KEY = "iGenericCassaInlineWithOrderNumber";
        /// <summary>stringa per il salvataggio nel registro del flag hash sotto e sopra il gruppo</summary>
        const String HASH_ON_UNDER_GROUP_KEY = "iGenericHashOnUnderGroup";

        static bool _bListinoModificato;

        bool _bInitComplete = false;
        TGenericPrinterParams _sGenericPrinterParamsCopy;

        /// <summary>riferimento a GenericPrinterDlg</summary>
        public static GenericPrinterDlg _rGenericPrinterDlg;

        /// <summary>ottiene flag di modifica listino necessaria</summary>
        public static bool GetListinoModificato() { return _bListinoModificato; }

        static TErrMsg _ErrMsg;

        /// <summary>costruttore</summary>
        public GenericPrinterDlg()
        {
            InitializeComponent();

            _rGenericPrinterDlg = this;

            _sGenericPrinterParamsCopy = new TGenericPrinterParams();

            Init(false); // imposta sGlobGenericPrinterParams, VIP
        }

        /// <summary>
        /// Inizializzazione con lettura dal Registro
        /// </summary>
        public bool Init(bool bShow)
        {
            DialogResult result = DialogResult.None;

            LogToFile("GenericPrinterDlg : Init in");


            sGlbGenericPrinterParams.iRowsInitial = ReadRegistry(EMPTY_ROWS_INITIAL_KEY, 1);
            sGlbGenericPrinterParams.iRowsFinal = ReadRegistry(EMPTY_ROWS_FINAL_KEY, 4);
            sGlbGenericPrinterParams.iCassaInline = ReadRegistry(CASSA_INLINE_WITH_ORDER_NUMBER_KEY, 0) != 0;
            sGlbGenericPrinterParams.iHashAroundGroup = ReadRegistry(HASH_ON_UNDER_GROUP_KEY, 0) != 0;

            numUpDown_RigheIniziali.Value = sGlbGenericPrinterParams.iRowsInitial;
            numUpDown_RigheFinali.Value = sGlbGenericPrinterParams.iRowsFinal;
            checkBox_CassaInlineNumero.Checked = sGlbGenericPrinterParams.iCassaInline;
            checkBox_HashAroundGroup.Checked = sGlbGenericPrinterParams.iHashAroundGroup;

            _sGenericPrinterParamsCopy = DeepCopy(sGlbGenericPrinterParams);

            _bInitComplete = true;

            // non chiamare qui UpdateGenericPrinterParam()
            AggiornaAspettoControlli();

            if (bShow)
                result = ShowDialog();

            LogToFile("GenericPrinterDlg : Init out");

            return (result == DialogResult.OK); // true se è cliccato OK
        }


        /// <summary>
        ///  funzione che imposta tutti i parametri necessari a GenericPrinterDlg<br/>
        ///  prelevandoli dai controlli e non dal Registro
        /// </summary>       
        void UpdateGenericPrinterParam()
        {
            _sGenericPrinterParamsCopy.iRowsInitial = (int)numUpDown_RigheIniziali.Value;
            _sGenericPrinterParamsCopy.iRowsFinal = (int)numUpDown_RigheFinali.Value;
            _sGenericPrinterParamsCopy.iCassaInline = checkBox_CassaInlineNumero.Checked;
            _sGenericPrinterParamsCopy.iHashAroundGroup = checkBox_HashAroundGroup.Checked;
        }

        void AggiornaAspettoControlli()
        {
            
        }

        private void SampleTextBtn_Click(object sender, EventArgs e)
        {
            UpdateGenericPrinterParam();
            AggiornaAspettoControlli();

            LogToFile("GenericPrinterDlg : SampleTextBtnClick");
        }

        /******************************************************************
            come regola i controlli non vengono	letti dal Registry
         ******************************************************************/
               

        private void NumUpDown_Click(object sender, EventArgs e)
        {
            UpdateGenericPrinterParam();
            AggiornaAspettoControlli();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            AggiornaAspettoControlli();
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            UpdateGenericPrinterParam();
            AggiornaAspettoControlli();

            // acquisizione impostazioni
            sGlbGenericPrinterParams = DeepCopy(_sGenericPrinterParamsCopy);

#if STANDFACILE
            AnteprimaDlg.rAnteprimaDlg.RedrawReceipt();
#endif

            WriteRegistry(EMPTY_ROWS_INITIAL_KEY, sGlbGenericPrinterParams.iRowsInitial);
            WriteRegistry(EMPTY_ROWS_FINAL_KEY, sGlbGenericPrinterParams.iRowsFinal);
            WriteRegistry(CASSA_INLINE_WITH_ORDER_NUMBER_KEY, sGlbGenericPrinterParams.iCassaInline ? 1 : 0);
            WriteRegistry(HASH_ON_UNDER_GROUP_KEY, sGlbGenericPrinterParams.iHashAroundGroup ? 1 : 0);

            _bListinoModificato = false;

            LogToFile("GenericPrinterDlg : OKBtnClick");
        }

        private void GenericPrinterDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            // evita la distruzione della form creando una eccezione alla successiva apertura
            e.Cancel = true;
        }

    }
}

/*******************************************************************************
	NomeFile : StandCommonSrc/WinPrinterDlg.cs
    Data	 : 06.12.2024
	Autore   : Mauro Artuso

	Descrizione : classe per la gestione della Form per l'impostazione dei
                  parametri delle stampanti mediante Driver Windows
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

using static StandFacile.glb;

namespace StandFacile
{
    /// <summary>
    /// classe per l'impostazione della stampante windows  <br/>
    /// cioè gestita mediante driver
    /// </summary>
    public partial class WinPrinterDlg : Form
    {
#pragma warning disable IDE0044

        readonly ToolTip _tt = new ToolTip
        {
            InitialDelay = 50,
            ShowAlways = true
        };

        /// <summary>stringa per il salvataggio nel registro del tipo di font Scontrino con stampa windows</summary>
        const String TCK_WIN_FONT_TYPE_KEY = "sWinTckFontType";
        /// <summary>stringa per il salvataggio nel registro del font report con stampa windows</summary>
        const String REP_WIN_FONT_TYPE_KEY = "sWinRepFontType";

        /// <summary>stringa per il salvataggio nel registro della dimensione font Scontrino con stampa windows</summary>
        const String TCK_WIN_FONT_SIZE_KEY = "fWinTckFontSize";
        /// <summary>stringa per il salvataggio nel registro della dimensione font report con stampa windows</summary>
        const String REP_WIN_FONT_SIZE_KEY = "fWinRepFontSize";

        /// <summary>stringa per il salvataggio nel registro dello style font Scontrino con stampa windows</summary>
        const String TCK_WIN_FONT_STYLE_KEY = "iWinTckFontStyle";
        /// <summary>stringa per il salvataggio nel registro dello style font report con stampa windows</summary>
        const String REP_WIN_FONT_STYLE_KEY = "iWinRepFontStyle";

        /// <summary>stringa per il salvataggio nel registro del margine sx Scontrino con stampa windows</summary>
        const String TCK_WIN_FONT_MARGIN_KEY = "iWinTckLeftMargin";
        /// <summary>stringa per il salvataggio nel registro del margine sx report con stampa windows</summary>
        const String REP_WIN_FONT_MARGIN_KEY = "iWinRepLeftMargin";

        /// <summary>stringa per il salvataggio nel registro del nome del Logo con stampa windows</summary>
        const String WIN_LOGO_NAME_KEY = "sWinLogoName";

        /// <summary>stringa per il salvataggio nel registro dello zoom  per il Scontrino con stampa windows</summary>
        const String TCK_WIN_ZOOM_KEY = "iWinTicketZoom";
        /// <summary>stringa per il salvataggio nel registro dello zoom  per i reports con stampa windows</summary>
        const String REP_WIN_ZOOM_KEY = "iWinReportsZoom";
        /// <summary>stringa per il salvataggio nel registro dello zoom  per il Logo con stampa windows</summary>
        const String LOGO_WIN_ZOOM_KEY = "iWinLogoZoom";

        static bool _bListinoModificato;

        bool _bInitComplete = false;
        int _iMaxMarginWidth, _iTckLeftMargin, _iRepLeftMargin;

#if !STANDFACILE
        int _yDisp = 0;
#endif
        static Font tckFont, repFont;
        FontDialog fontTckDlg, fontRepDlg;

        TWinPrinterParams _sWinPrinterParamsCopy;

        /// <summary>riferimento a WinPrinterDlg</summary>
        public static WinPrinterDlg _rWinPrinterDlg;

        // deve stare qui per non essere instanziato spesso
        static PrintDocument pd = new PrintDocument();

        static PrinterSettings settings = new PrinterSettings();
        static String sDefaultPrinter;

        /// <summary>ottiene flag di modifica listino necessaria</summary>
        public static bool GetListinoModificato() { return _bListinoModificato; }

        /// <summary>ottiene limmagine bitmap del Logo</summary>
        public Image GetWinPrinterLogo() { return logoImage.Image; }

        /// <summary>ottiene flag di richiesta stampa coperti nelle copie</summary>
        public static bool GetCopies_PlaceSettingsToBePrinted() { return _rWinPrinterDlg.checkBox_CopertiNelleCopie.Checked; }

        /// <summary>ottiene flag di richiesta stampa del Logo</summary>
        public static bool GetCopies_LogoToBePrinted() { return _rWinPrinterDlg.checkBox_LogoNelleCopie.Checked; }

        static TErrMsg _ErrMsg;

        /// <summary>costruttore</summary>
        public WinPrinterDlg()
        {
            InitializeComponent();

            _rWinPrinterDlg = this;

            fontTckDlg = new FontDialog()
            {
                FixedPitchOnly = true
            };

            fontRepDlg = new FontDialog()
            {
                FixedPitchOnly = true
            };

            _sWinPrinterParamsCopy = new TWinPrinterParams(0);

            sGlbWinPrinterParams.iLogoWidth = 500;
            sGlbWinPrinterParams.iLogoHeight = 500;

            _tt.SetToolTip(checkBox_Chars33, "con 33 caratteri le descrizioni Articoli sono migliori ma le stampe più piccole\r\n" +
                                             "sconsigliato per formato carta da 57mm");

            Init(false); // imposta sGlbWinPrinterParams, VIP
        }

        /// <summary>
        /// Inizializzazione con lettura dal Registro
        /// </summary>
        public bool Init(bool bShow)
        {
            int i = 0, j = 0;

            String sDir = "";
            DialogResult result = DialogResult.None;
            Bitmap tmpImage;

            LogToFile("WinPrinterDlg : Init in");

            // stampante Windows di Default
            sDefaultPrinter = settings.PrinterName;

            //	letto anche dalla classe TFrmPrintConfig() così non ci sono problemi di ordine della
            //	instanziazione delle classi

            //  l'ultimo elemento del vettore è la stampante Locale ( = non quella delle copie)
            sGlbWinPrinterParams.sTckPrinterModel = ReadRegistry(WIN_PRINTER_MODEL_KEY, sDefaultPrinter);

            sGlbWinPrinterParams.iTckPrinterModel = 0;

            PrintersListCombo.Items.Clear();

            foreach (string printer in PrinterSettings.InstalledPrinters)
            {
                PrintersListCombo.Items.Add(printer);

                if (printer == sGlbWinPrinterParams.sTckPrinterModel)
                {
                    sGlbWinPrinterParams.iTckPrinterModel = i;
                    j = i;
                }

                i++;
            }

            // imposta nel combo la stampante Locale
            if ((j >= 0) && (j < PrintersListCombo.Items.Count))
                PrintersListCombo.SelectedIndex = j;
            else
                PrintersListCombo.SelectedIndex = 0;

#if STANDFACILE
            // 7+1, sPrinterModel è sopra
            sGlbWinPrinterParams.sLogoName = ReadRegistry(WIN_LOGO_NAME_KEY, "");
#endif

            sGlbWinPrinterParams.sTckFontType = ReadRegistry(TCK_WIN_FONT_TYPE_KEY, "Lucida Console");
            sGlbWinPrinterParams.fTckFontSize = ReadRegistry(TCK_WIN_FONT_SIZE_KEY, 1100) / 100.0f;
            sGlbWinPrinterParams.sTckFontStyle = (FontStyle)ReadRegistry(TCK_WIN_FONT_STYLE_KEY, 0);
            sGlbWinPrinterParams.iTckLeftMargin = ReadRegistry(TCK_WIN_FONT_MARGIN_KEY, 4);

            //if (sGlbWinPrinterParams.fTckFontSize < 5)
            //    sGlbWinPrinterParams.fTckFontSize = 11;

            String stmp = sGlbWinPrinterParams.sTckFontType;

            tckFont = new Font(sGlbWinPrinterParams.sTckFontType, sGlbWinPrinterParams.fTckFontSize, sGlbWinPrinterParams.sTckFontStyle);

            sGlbWinPrinterParams.sRepFontType = ReadRegistry(REP_WIN_FONT_TYPE_KEY, "Lucida Console");
            sGlbWinPrinterParams.fRepFontSize = ReadRegistry(REP_WIN_FONT_SIZE_KEY, 900) / 100.0f;
            sGlbWinPrinterParams.sRepFontStyle = (FontStyle)ReadRegistry(REP_WIN_FONT_STYLE_KEY, 0);
            sGlbWinPrinterParams.iRepLeftMargin = ReadRegistry(REP_WIN_FONT_MARGIN_KEY, 10);

            //if (sGlbWinPrinterParams.fRepFontSize < 5)
            //    sGlbWinPrinterParams.fRepFontSize = 9;

            repFont = new Font(sGlbWinPrinterParams.sRepFontType, sGlbWinPrinterParams.fRepFontSize, sGlbWinPrinterParams.sRepFontStyle);

            _iTckLeftMargin = sGlbWinPrinterParams.iTckLeftMargin;
            _iRepLeftMargin = sGlbWinPrinterParams.iRepLeftMargin;

            checkBox_A5_paper.Checked = (ReadRegistry(PRINT_ON_A5_PAPER_KEY, 0) == 1);
            sGlbWinPrinterParams.bA5Paper = checkBox_A5_paper.Checked;

            sGlbWinPrinterParams.iTckZoomValue = ReadRegistry(TCK_WIN_ZOOM_KEY, 100);
            sGlbWinPrinterParams.iRepZoomValue = ReadRegistry(REP_WIN_ZOOM_KEY, 100);
            sGlbWinPrinterParams.iLogoZoomValue = ReadRegistry(LOGO_WIN_ZOOM_KEY, 100);

            if (sGlbWinPrinterParams.iTckZoomValue < numUpDownTicket.Minimum)
                sGlbWinPrinterParams.iTckZoomValue = 100;

            if (sGlbWinPrinterParams.iTckZoomValue > numUpDownTicket.Maximum)
                sGlbWinPrinterParams.iTckZoomValue = 100;

            if (sGlbWinPrinterParams.iRepZoomValue < numUpDownReports.Minimum)
                sGlbWinPrinterParams.iRepZoomValue = 100;

            if (sGlbWinPrinterParams.iRepZoomValue > numUpDownReports.Maximum)
                sGlbWinPrinterParams.iRepZoomValue = 100;

            if (sGlbWinPrinterParams.iLogoZoomValue < numUpDownLogo.Minimum)
                sGlbWinPrinterParams.iLogoZoomValue = 100;

            if (sGlbWinPrinterParams.iLogoZoomValue > numUpDownLogo.Maximum)
                sGlbWinPrinterParams.iLogoZoomValue = 100;

            numUpDownTicket.Value = sGlbWinPrinterParams.iTckZoomValue;
            numUpDownLogo.Value = sGlbWinPrinterParams.iLogoZoomValue;
            numUpDownReports.Value = sGlbWinPrinterParams.iRepZoomValue;

            //deve essere fdLimitSize per produrre effetto set

            //LogoImage.Width = LOGO_WIDTH / 2;
            //LogoImage.Height = LOGO_HEIGHT / 2;

#if STANDFACILE

            // caricato dal Listino
            checkBox_Chars33.Checked = sGlbWinPrinterParams.bChars33;

            checkBox_LogoNelleCopie.Checked = IsBitSet(SF_Data.iReceiptCopyOptions, BIT_LOGO_PRINT_REQUIRED);

            checkBox_CopertiNelleCopie.Checked = IsBitSet(SF_Data.iReceiptCopyOptions, BIT_EXTEND_PLACESETTINGS_PRINT_REQUIRED);

            sDir = DataManager.GetExeDir() + "\\";
            //FileOpenLogo.InitialDirectory = DataManager.GetExeDir();

            BtnLogoFileSelect.Enabled = true;
            BtnDeleteLogo.Enabled = true;
            logoImage.Enabled = true;
            lblLogo.Enabled = true;
#else

            checkBox_Chars33.Checked = (ReadRegistry(PRINT_ON_33CHARS_RECEIPT_KEY, 0) == 1);
            sGlbWinPrinterParams.bChars33 = checkBox_Chars33.Checked;

            checkBox_CopertiNelleCopie.Checked = (ReadRegistry(PRINT_PLACESETTINGS_ON_COPIES_KEY, 0) == 1);

            if (checkBox_CopertiNelleCopie.Checked)
                SF_Data.iReceiptCopyOptions = SetBit(SF_Data.iReceiptCopyOptions, BIT_EXTEND_PLACESETTINGS_PRINT_REQUIRED);
            else
                SF_Data.iReceiptCopyOptions = ClearBit(SF_Data.iReceiptCopyOptions, BIT_EXTEND_PLACESETTINGS_PRINT_REQUIRED);

            checkBox_LogoNelleCopie.Checked = false;
            checkBox_LogoNelleCopie.Visible = false;
            BtnLogoFileSelect.Enabled = false;
            BtnLogoFileSelect.Visible = false;
            BtnDeleteLogo.Enabled = false;
            BtnDeleteLogo.Visible = false;
            logoImage.Enabled = false;
            logoImage.Visible = false;
            lblLogo.Enabled = false;
            lblLogo.Visible = false;

            // esegue solo una volta
            if (_yDisp == 0)
            {
                _yDisp = SampleTextBtn.Top - BtnLogoFileSelect.Top;

                // dipende da proprieta Anchor
                //btnOK.Top -= _yDisp;
                //btnCancel.Top -= _yDisp;
                //SampleTextBtn.Top -= _yDisp - 8;

                //checkBox_Chars33.Top -= _yDisp;
                //checkBox_A5_paper.Top -= _yDisp;

                Height -= _yDisp;
            }
#endif

            if (!String.IsNullOrEmpty(sGlbWinPrinterParams.sLogoName)) //carica il file grafico
            {
                if (File.Exists(sDir + sGlbWinPrinterParams.sLogoName))
                {
                    tmpImage = new Bitmap(sDir + sGlbWinPrinterParams.sLogoName);
                    logoImage.Image = new Bitmap(tmpImage);

                    sGlbWinPrinterParams.iLogoWidth = tmpImage.Size.Width;
                    sGlbWinPrinterParams.iLogoHeight = tmpImage.Size.Height;

                    LogToFile("WinPrinterDlg : Load Logo" + sGlbWinPrinterParams.sLogoName);

                    // libera la risorsa file
                    tmpImage.Dispose();
                }
                else
                {
                    if (logoImage.Image != null)
                    {
                        logoImage.Image.Dispose();
                        logoImage.Image = null;
                    }

                    LogToFile("WinPrinterDlg : Init nessun Logo");
                }
            }
            else
            {
                if (logoImage.Image != null)
                {
                    logoImage.Image.Dispose();
                    logoImage.Image = null;
                }

                LogToFile("WinPrinterDlg : Init nessun Logo");
            }

            //copia locale
            _sWinPrinterParamsCopy = sGlbWinPrinterParams;

            _bInitComplete = true;

            // non chiamare qui UpdateWinPrinterParam()
            AggiornaAspettoControlli();

            if (bShow)
            {
                timer.Enabled = true;
                result = ShowDialog();
            }
            else
                timer.Enabled = false;

            LogToFile("WinPrinterDlg : Init out");

            // caricato da CaricaListino(...)
#if !STANDFACILE
            InitFormatStrings();
#endif
            return (result == DialogResult.OK); // true se è cliccato OK
        }

        /******************************************************************
           funzione che imposta tutti i parametri necessari alla classe
           FrmPrintServer prelevandoli dai controlli e non dal Registro
        ******************************************************************/
        void UpdateWinPrinterParam()
        {
            String sPrinterName;

            // 7, no Logo letto a parte
            sPrinterName = PrintersListCombo.SelectedItem.ToString();

            _sWinPrinterParamsCopy.sTckPrinterModel = sPrinterName;
            _sWinPrinterParamsCopy.iTckPrinterModel = PrintersListCombo.SelectedIndex;

            _sWinPrinterParamsCopy.iTckLeftMargin = _iTckLeftMargin;

            _sWinPrinterParamsCopy.iRepLeftMargin = _iRepLeftMargin;

            _sWinPrinterParamsCopy.iTckZoomValue = (int)numUpDownTicket.Value;
            _sWinPrinterParamsCopy.iRepZoomValue = (int)numUpDownReports.Value;
            _sWinPrinterParamsCopy.iLogoZoomValue = (int)numUpDownLogo.Value;

            _sWinPrinterParamsCopy.bChars33 = checkBox_Chars33.Checked;
            _sWinPrinterParamsCopy.bA5Paper = checkBox_A5_paper.Checked;
        }

        private void SampleTextBtn_Click(object sender, EventArgs e)
        {
            UpdateWinPrinterParam();
            AggiornaAspettoControlli();

            LogToFile("WinPrinterDlg : SampleTextBtnClick");
            Printer_Windows.PrintSampleText(_sWinPrinterParamsCopy);

        }

        /******************************************************************
            come regola i controlli non vengono	letti dal Registry
         ******************************************************************/
        void AggiornaAspettoControlli()
        {
            int iParam1 = 0, iParam2 = 0;
            float fParam1;
            String sText;

            pd.PrinterSettings.PrinterName = sGlbWinPrinterParams.sTckPrinterModel;

            if (!String.IsNullOrEmpty(pd.PrinterSettings.PrinterName))
            {
                try
                {
                    _iMaxMarginWidth = pd.PrinterSettings.DefaultPageSettings.PaperSize.Width / 3;

                    // istruzioni lente, quindi vanno entrambi prima del Clear()
                    iParam1 = pd.PrinterSettings.DefaultPageSettings.PaperSize.Width * pd.PrinterSettings.DefaultPageSettings.PrinterResolution.X / 100;
                    iParam2 = pd.PrinterSettings.DefaultPageSettings.PaperSize.Height * pd.PrinterSettings.DefaultPageSettings.PrinterResolution.Y / 100;
                }
                catch
                {
                    _ErrMsg.sMsg = pd.PrinterSettings.PrinterName;
                    _ErrMsg.iErrID = WRN_PDE;
                    WarningManager(_ErrMsg);
                }
            }


            if ((tckFont.Name != _sWinPrinterParamsCopy.sTckFontType) || (tckFont.Size != _sWinPrinterParamsCopy.fTckFontSize) || (tckFont.Style != _sWinPrinterParamsCopy.sTckFontStyle))
            {
                tckFont.Dispose();
                tckFont = new Font(_sWinPrinterParamsCopy.sTckFontType, _sWinPrinterParamsCopy.fTckFontSize, _sWinPrinterParamsCopy.sTckFontStyle);
            }

            if ((repFont.Name != _sWinPrinterParamsCopy.sRepFontType) || (repFont.Size != _sWinPrinterParamsCopy.fRepFontSize) || (repFont.Style != _sWinPrinterParamsCopy.sRepFontStyle))
            {
                repFont.Dispose();
                repFont = new Font(_sWinPrinterParamsCopy.sRepFontType, _sWinPrinterParamsCopy.fRepFontSize, _sWinPrinterParamsCopy.sRepFontStyle);
            }

            Memo.Clear();

            sText = "Stampante : " + _sWinPrinterParamsCopy.sTckPrinterModel;
            Memo.AppendText(sText); Memo.AppendText(Environment.NewLine);

            sText = String.Format("Larghezza pagina : {0} px\n", iParam1);
            Memo.AppendText(sText); Memo.AppendText(Environment.NewLine);

            sText = String.Format("Lunghezza pagina : {0} px\n", iParam2);
            Memo.AppendText(sText); Memo.AppendText(Environment.NewLine);

            Memo.AppendText(Environment.NewLine);
            Memo.AppendText("Scontrini :"); Memo.AppendText(Environment.NewLine);

            sText = _sWinPrinterParamsCopy.sTckFontType;
            sText = "Tipo di Font : " + sText;
            Memo.AppendText(sText); Memo.AppendText(Environment.NewLine);

            fParam1 = _sWinPrinterParamsCopy.fTckFontSize;
            sText = String.Format("Dimensione Font : {0:0.00}\n", fParam1);
            Memo.AppendText(sText); Memo.AppendText(Environment.NewLine);

            Memo.AppendText(Environment.NewLine);
            Memo.AppendText("Riepiloghi :"); Memo.AppendText(Environment.NewLine);

            sText = _sWinPrinterParamsCopy.sRepFontType;
            sText = "Tipo di Font : " + sText;
            Memo.AppendText(sText); Memo.AppendText(Environment.NewLine);

            fParam1 = _sWinPrinterParamsCopy.fRepFontSize;
            sText = String.Format("Dimensione Font : {0:0.00}\n", fParam1);
            Memo.AppendText(sText); Memo.AppendText(Environment.NewLine);

            //Margini
            T_Edit.Text = _sWinPrinterParamsCopy.iTckLeftMargin.ToString();
            R_Edit.Text = _sWinPrinterParamsCopy.iRepLeftMargin.ToString();

            if ((logoImage.Size.Height > 50) && !String.IsNullOrEmpty(_sWinPrinterParamsCopy.sLogoName))
            {
                String sTmp;
                sTmp = String.Format("Anteprima Logo {0} x {1} px :\n", _sWinPrinterParamsCopy.iLogoWidth, _sWinPrinterParamsCopy.iLogoHeight);
                lblLogo.Text = sTmp;
            }
            else
                lblLogo.Text = "Anteprima Logo :";
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            bool bNoErrors;
            String sTick_Text, sRep_Text;

            bNoErrors = true;
            sTick_Text = T_Edit.Text.Trim();
            sRep_Text = R_Edit.Text.Trim();

            if (!String.IsNullOrEmpty(sTick_Text) && this.Visible)
            {
                try
                {
                    _iTckLeftMargin = Convert.ToInt32(sTick_Text);
                }
                catch (Exception)
                {
                    bNoErrors = false;
                }

                if (bNoErrors && (_iTckLeftMargin < _iMaxMarginWidth))
                    T_Edit.BackColor = Color.LightGreen;
                else
                    T_Edit.BackColor = Color.Red;
            }

            if (!String.IsNullOrEmpty(sRep_Text) && this.Visible)
            {
                try
                {
                    _iRepLeftMargin = Convert.ToInt32(sRep_Text);
                }
                catch (Exception)
                {
                    bNoErrors = false;
                }

                if (bNoErrors && (_iRepLeftMargin < _iMaxMarginWidth))
                    R_Edit.BackColor = Color.LightGreen;
                else
                    R_Edit.BackColor = Color.Red;

            }

            if (R_Edit.Focused || T_Edit.Focused)
            {
                UpdateWinPrinterParam();
                AggiornaAspettoControlli();
            }

        }

        private void BtnTicketsFontSelect_Click(object sender, EventArgs e)
        {
            fontTckDlg.Font = tckFont;

            DialogResult result = fontTckDlg.ShowDialog();

            // See if OK was pressed.
            if (result == DialogResult.OK)
            {
                _sWinPrinterParamsCopy.sTckFontType = fontTckDlg.Font.Name;
                _sWinPrinterParamsCopy.fTckFontSize = fontTckDlg.Font.Size;
                _sWinPrinterParamsCopy.sTckFontStyle = fontTckDlg.Font.Style;

                UpdateWinPrinterParam();
                AggiornaAspettoControlli();
            }
        }

        private void BtnReportsFontSelect_Click(object sender, EventArgs e)
        {
            fontRepDlg.Font = repFont;

            DialogResult result = fontRepDlg.ShowDialog();

            // See if OK was pressed.
            if (result == DialogResult.OK)
            {
                _sWinPrinterParamsCopy.sRepFontType = fontRepDlg.Font.Name;
                _sWinPrinterParamsCopy.fRepFontSize = fontRepDlg.Font.Size;
                _sWinPrinterParamsCopy.sRepFontStyle = fontRepDlg.Font.Style;

                UpdateWinPrinterParam();
                AggiornaAspettoControlli();
            }
        }

        private void WinPrinterDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            // evita la distruzione della form creando una eccezione alla successiva apertura
            e.Cancel = true;
        }

        private void PrintersListCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            String sPrinterName;

            if (!_bInitComplete) return;

            //legge dal combo ed aggiorna
            UpdateWinPrinterParam();
            AggiornaAspettoControlli();

            sPrinterName = PrintersListCombo.Text;
            LogToFile("WinPrinterDlg : ComboChange" + sPrinterName);

        }

        private void BtnLogoFileSelect_Click(object sender, EventArgs e)
        {
            String sDir = "", sTmpDot;
            String sSourceFile, sDestinationFile;
            Bitmap tmpImage;

            OpenFileDialog FileOpenLogo = new OpenFileDialog()
            {
                Filter = "jpg files (*.jpg) |*.jpg|bmp files (*.bmp) |*.bmp|png files (*.png) |*.png| All files (*.*)|*.*"
            };

            DialogResult result = FileOpenLogo.ShowDialog();

            // See if OK was pressed.
            if (result == DialogResult.OK)
            {
                sSourceFile = FileOpenLogo.FileName;
                sTmpDot = Path.GetExtension(sSourceFile);

#if STANDFACILE
                sDir = DataManager.GetExeDir() + "\\";
#endif
                sDestinationFile = "Logo" + sTmpDot;

                // copia del file
                if (sDestinationFile != sSourceFile)
                {
                    try
                    {
                        if (logoImage.Image != null)
                        {
                            logoImage.Image.Dispose();
                            logoImage.Image = null;
                        }

                        GC.Collect();
                        GC.WaitForPendingFinalizers();

                        if (File.Exists(sDir + "Logo.jpg"))
                            File.Delete(sDir + "Logo.jpg");

                        if (File.Exists(sDir + "Logo.bmp"))
                            File.Delete(sDir + "Logo.bmp");

                        if (File.Exists(sDir + "Logo.png"))
                            File.Delete(sDir + "Logo.png");

                        File.Copy(sSourceFile, sDir + sDestinationFile, true);

                        tmpImage = new Bitmap(sDir + sDestinationFile);

                        // controlli sul Logo
                        if ((tmpImage.Width > 50) && (tmpImage.Width < (LOGO_WIDTH + 100)) &&
                            (tmpImage.Height > 50) && (tmpImage.Height < (LOGO_HEIGHT + 100)))
                        {
                            logoImage.Image = tmpImage;

                            _sWinPrinterParamsCopy.sLogoName = sDestinationFile;
                            _sWinPrinterParamsCopy.iLogoWidth = tmpImage.Width;
                            _sWinPrinterParamsCopy.iLogoHeight = tmpImage.Height;
                        }
                        else
                            WarningManager(WRN_DLE);

                        AggiornaAspettoControlli();

                        LogToFile("WinPrinterDlg : Load Logo" + sDestinationFile);
                    }
                    catch (Exception)
                    {
                        BtnDeleteLogo_Click(this, null); //elimina caricamenti successivi
                        LogToFile("WinPrinterDlg : Exception nessun Logo Open");
                    }
                }
                else
                    LogToFile("WinPrinterDlg : nessun Logo Open");
            }

        }

        private void BtnDeleteLogo_Click(object sender, EventArgs e)
        {
            _sWinPrinterParamsCopy.sLogoName = "";

            if (logoImage.Image != null)
            {
                logoImage.Image.Dispose();
                logoImage.Image = null;
            }

            AggiornaAspettoControlli();

            LogToFile("WinPrinterDlg : LogoDelete");
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;
            _sWinPrinterParamsCopy = sGlbWinPrinterParams;
            AggiornaAspettoControlli();
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            UpdateWinPrinterParam();
            AggiornaAspettoControlli();

            // acquisizione impostazioni
            sGlbWinPrinterParams = _sWinPrinterParamsCopy;

            // 8 scrittura nel registro
            WriteRegistry(WIN_PRINTER_MODEL_KEY, sGlbWinPrinterParams.sTckPrinterModel);

            WriteRegistry(WIN_LOGO_NAME_KEY, sGlbWinPrinterParams.sLogoName);

            WriteRegistry(TCK_WIN_FONT_TYPE_KEY, sGlbWinPrinterParams.sTckFontType);
            WriteRegistry(TCK_WIN_FONT_SIZE_KEY, sGlbWinPrinterParams.fTckFontSize * 100.0f);
            WriteRegistry(TCK_WIN_FONT_STYLE_KEY, (int)sGlbWinPrinterParams.sTckFontStyle);
            WriteRegistry(TCK_WIN_FONT_MARGIN_KEY, sGlbWinPrinterParams.iTckLeftMargin);

            WriteRegistry(REP_WIN_FONT_TYPE_KEY, sGlbWinPrinterParams.sRepFontType);
            WriteRegistry(REP_WIN_FONT_SIZE_KEY, sGlbWinPrinterParams.fRepFontSize * 100.0f);
            WriteRegistry(REP_WIN_FONT_STYLE_KEY, (int)sGlbWinPrinterParams.sRepFontStyle);
            WriteRegistry(REP_WIN_FONT_MARGIN_KEY, sGlbWinPrinterParams.iRepLeftMargin);

            WriteRegistry(TCK_WIN_ZOOM_KEY, sGlbWinPrinterParams.iTckZoomValue);
            WriteRegistry(REP_WIN_ZOOM_KEY, sGlbWinPrinterParams.iRepZoomValue);
            WriteRegistry(LOGO_WIN_ZOOM_KEY, sGlbWinPrinterParams.iLogoZoomValue);

            _bListinoModificato = false;

            if (_sWinPrinterParamsCopy.bChars33)
            {
                if (!IsBitSet(SF_Data.iReceiptCopyOptions, BIT_CHARS33_PRINT_REQUIRED))
                {
                    SF_Data.iReceiptCopyOptions = SetBit(SF_Data.iReceiptCopyOptions, BIT_CHARS33_PRINT_REQUIRED);
                    sGlbWinPrinterParams.bChars33 = true;

                    WriteRegistry(PRINT_ON_33CHARS_RECEIPT_KEY, 1);
                    _bListinoModificato = true;
                }
            }
            else
            {
                if (IsBitSet(SF_Data.iReceiptCopyOptions, BIT_CHARS33_PRINT_REQUIRED))
                {
                    SF_Data.iReceiptCopyOptions = ClearBit(SF_Data.iReceiptCopyOptions, BIT_CHARS33_PRINT_REQUIRED);
                    sGlbWinPrinterParams.bChars33 = false;

                    WriteRegistry(PRINT_ON_33CHARS_RECEIPT_KEY, 0);
                    _bListinoModificato = true;
                }
            }

            InitFormatStrings();

#if STANDFACILE

            if (checkBox_LogoNelleCopie.Checked)
            {
                if (!IsBitSet(SF_Data.iReceiptCopyOptions, BIT_LOGO_PRINT_REQUIRED))
                {
                    SF_Data.iReceiptCopyOptions = SetBit(SF_Data.iReceiptCopyOptions, BIT_LOGO_PRINT_REQUIRED);

                    _bListinoModificato = true;
                }
            }
            else
            {
                if (IsBitSet(SF_Data.iReceiptCopyOptions, BIT_LOGO_PRINT_REQUIRED))
                {
                    SF_Data.iReceiptCopyOptions = ClearBit(SF_Data.iReceiptCopyOptions, BIT_LOGO_PRINT_REQUIRED);

                    _bListinoModificato = true;
                }
            }
#endif


            if (checkBox_CopertiNelleCopie.Checked)
            {
                if (!IsBitSet(SF_Data.iReceiptCopyOptions, BIT_EXTEND_PLACESETTINGS_PRINT_REQUIRED))
                {
                    SF_Data.iReceiptCopyOptions = SetBit(SF_Data.iReceiptCopyOptions, BIT_EXTEND_PLACESETTINGS_PRINT_REQUIRED);

                    WriteRegistry(PRINT_PLACESETTINGS_ON_COPIES_KEY, 1);
                    _bListinoModificato = true;
                }
            }
            else
            {
                if (IsBitSet(SF_Data.iReceiptCopyOptions, BIT_EXTEND_PLACESETTINGS_PRINT_REQUIRED))
                {
                    SF_Data.iReceiptCopyOptions = ClearBit(SF_Data.iReceiptCopyOptions, BIT_EXTEND_PLACESETTINGS_PRINT_REQUIRED);

                    WriteRegistry(PRINT_PLACESETTINGS_ON_COPIES_KEY, 0);
                    _bListinoModificato = true;
                }
            }

            if (_sWinPrinterParamsCopy.bA5Paper)
                WriteRegistry(PRINT_ON_A5_PAPER_KEY, 1);
            else
                WriteRegistry(PRINT_ON_A5_PAPER_KEY, 0);

            timer.Enabled = false;
            LogToFile("WinPrinterDlg : OKBtnClick");
        }

    }
}

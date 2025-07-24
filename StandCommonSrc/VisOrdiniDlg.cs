/***************************************************************************
	NomeFile : StandCommonSrc/VisOrdiniDlg.cs
	Data	 : 24.07.2025
	Autore	 : Mauro Artuso
	 
 ***************************************************************************/

using System;
using System.IO;
using System.Windows.Forms;

using StandCommonFiles;
using static StandFacile.glb;
using static StandFacile.Define;
using static StandFacile.dBaseIntf;
using static StandCommonFiles.ComDef;
using static StandCommonFiles.CommonCl;
using static StandCommonFiles.ReceiptAndCopies;
using static StandCommonFiles.LogServer;

namespace StandFacile
{
    /// <summary>
    /// Classe di visualizzazione dello scontrino, in caso di buchi della <br/>
    /// numerazione prosegue oltre. <br/>  <br/>
    /// 
    /// utilizza sempre DB_Data, tranne dove è richiesto SF_Data.iNumCassa, ed i flag SF_Data.bCopiesGroupsFlag[i], SF_Data.iGroupsColor[i] <br/>  <br/>
    /// 
    /// In mancanza dei files scontrini e copie li ricostruisce a partire dal Database
    /// 
    /// </summary>
    public partial class VisOrdiniDlg : Form
    {
#pragma warning disable IDE0044
#pragma warning disable IDE0059

        const int SEARCH_LIMIT = 999;

        /// <summary>massimo numero di scontrini per ricerca</summary>
        public const int MAX_NUM_TICKET = 9999;

        bool _bOrdineCaricato, _bAnnulloOrdine, _bCambiaPagamento;
        int _iNum, _iUpperLimit;
        DateTime _dateOrdine;
        String _sNomeFileTicket, _sNomeFileTicketNpPrt, _sNomeTabella;

        bool[] _bScontoGruppo = new bool[NUM_SEP_PRINT_GROUPS];

        /// <summary>colore di sfondo dello scontrino Annullato</summary>
        System.Drawing.Color _clrAnnullatoBkgr;
        /// <summary>colore di sfondo della copia scontrino da pagare</summary>
        System.Drawing.Color _clrNonAncoraPagatoBkgr;
        /// <summary>colore di sfondo della copia scontrino scaricato da StandOrdini</summary>
        System.Drawing.Color _clrScaricatoBkgr;
        /// <summary>colore di sfondo della copia scontrino emesso da altra cassa e scaricato da StandOrdini</summary>
        System.Drawing.Color _clrEAC_ScaricatoBkgr;
        /// <summary>colore di sfondo della copia scontrino emessa non ancora scaricato da StandOrdini</summary>
        System.Drawing.Color _clrNonAncoraScaricatoBkgr;
        /// <summary>colore di sfondo della copia scontrino emesso da altra cassa e non ancora scaricato da StandOrdini</summary>
        System.Drawing.Color _clrEAC_NonAncoraScaricatoBkgr;

        // TextBox ToolTip
        ToolTip tt = new ToolTip()
        {
            InitialDelay = 0,
            ShowAlways = true
        };

        String _tt_InfoLabelText;

        TErrMsg _ErrMsg;

#if STANDFACILE
        TData _dataIdentifierParam;
#endif

        /// <summary>costruttore</summary>
        public VisOrdiniDlg(DateTime dateParam, int iNumParam, String sNomeTabellaParam = "", VIEW_TYPE eViewTypeParam = VIEW_TYPE.NORMAL)
        {
            bool bHide;

            InitializeComponent();

            // impostazione colori di sfondo
            _clrAnnullatoBkgr = System.Drawing.Color.Red;
            _clrNonAncoraPagatoBkgr = System.Drawing.Color.MistyRose;

            _clrScaricatoBkgr = System.Drawing.Color.PaleGreen;
            _clrEAC_ScaricatoBkgr = System.Drawing.Color.LightSkyBlue;

            _clrNonAncoraScaricatoBkgr = System.Drawing.Color.Teal;
            _clrEAC_NonAncoraScaricatoBkgr = System.Drawing.Color.MediumBlue;

            _iNum = 0;
            _dateOrdine = dateParam;
            _sNomeTabella = sNomeTabellaParam;

            _bAnnulloOrdine = (eViewTypeParam == VIEW_TYPE.CANCEL_ORDER);
            _bCambiaPagamento = (eViewTypeParam == VIEW_TYPE.CHANGE_PAYMENT);

            bHide = (eViewTypeParam == VIEW_TYPE.NO_VIEW);

            Height = 636; // MAINWD_HEIGHT = 660

            // SQLite, impostazione comune
            if (bUSA_NDB())
            {
                _tt_InfoLabelText = "Lo sfondo VERDE-CHIARO indica che lo scontrino è stato scaricato da StandOrdini,\r\n" +
                    "Lo sfondo BLU-CHIARO indica che lo scontrino emesso da altra cassa è stato scaricato da StandOrdini,,\r\n\r\n" +
                    "lo sfondo VERDE-BLU indica che lo scontrino non è stato scaricato da StandOrdini,\r\n" +
                    "lo sfondo BLU indica che lo scontrino emesso da altra cassa non è stato scaricato da StandOrdini,\r\n,\r\n" +
                    "lo sfondo ROSA indica che lo scontrino emesso online non è stato pagato,\r\n" +
                    "lo sfondo ROSSO indica che lo scontrino è stato Annullato.\r\n";
            }
            else
            {
                _tt_InfoLabelText =
                    "lo sfondo ROSA indica che lo scontrino emesso online non è stato pagato,\r\n" +
                    "lo sfondo ROSSO indica che lo scontrino è stato Annullato.\r\n";

                CkBoxTutteCasse.Visible = false; 
                CkBoxTutteCasse.Enabled = false;
            }

            tt.SetToolTip(lbl_Info, _tt_InfoLabelText);

#if STANDFACILE

            _dataIdentifierParam = SF_Data;

            if (GetActualDate().ToString("dd/MM/yy") != _dateOrdine.ToString("dd/MM/yy"))
            {
                // cancellazione tickets altrimenti si vedono tickets precedenti
                String sNomeDirTmp = GetVisTicketsDir();

                if (Directory.Exists(sNomeDirTmp))
                    Directory.Delete(sNomeDirTmp, true);
            }

            if (_dataIdentifierParam.bPrevendita || sNomeTabellaParam.Contains(_dbPreOrdersTablePrefix))
                CkBoxTutteCasse.Checked = true;

            // carica il numero di Tickets in caso di altra data
            if (GetActualDate().ToString("dd/MM/yy") == dateParam.ToString("dd/MM/yy") && !_sNomeTabella.Contains(_dbOrdersTablePrefix))
                _iUpperLimit = DataManager.GetNumOfOrders();
            else
#endif
                _iUpperLimit = _rdBaseIntf.dbCaricaDatidaOrdini(dateParam, 0, true, _sNomeTabella);

#if STAND_CUCINA || STAND_MONITOR

            CkBoxTutteCasse.Checked = true;
            CkBoxTutteCasse.Enabled = false;
#endif

            if (_bAnnulloOrdine) // Annullo, mantiene posizione by Design
            {
                this.Text = "Annulla Ordine";

                comboPaymentType.Enabled = false;
                comboPaymentType.Visible = false;

                checkBoxNotPaid.Enabled = false;
                checkBoxNotPaid.Visible = false;

                labelPayMethod.Visible = false;

                if (GetActualDate().ToString("dd/MM/yy") == dateParam.ToString("dd/MM/yy"))
                {
                    AnnulloBtn.Enabled = true;
                    AnnulloBtn.Visible = true;
                }

                BtnPrt.Enabled = false;
                lbl_Info.Visible = true;
                lbl_Info.Top = BtnPrev.Top + 32;

                CkBoxTutteCasse.Enabled = ((SF_Data.iNumCassa == CASSA_PRINCIPALE) && bUSA_NDB());
                OKBtn.Text = "Esci";
            }
            else if (_bCambiaPagamento)
            {
                this.Text = "Cambia pagamento Ordini";

                comboPaymentType.Items.Clear();

                for (int i = sConst_PaymentType.Length - 1; i >= 0; i--) // OK
                    comboPaymentType.Items.Insert(0, sConst_PaymentType[i]);

                AnnulloBtn.Enabled = false;
                AnnulloBtn.Visible = false;

                if (GetActualDate().ToString("dd/MM/yy") == dateParam.ToString("dd/MM/yy"))
                {
                    comboPaymentType.Enabled = true;
                }

                BtnPrt.Enabled = false;
                lbl_Info.Visible = false;

                CkBoxTutteCasse.Enabled = ((SF_Data.iNumCassa == CASSA_PRINCIPALE) && bUSA_NDB());
                comboPaymentType.Visible = true;
                labelPayMethod.Visible = true;

                OKBtn.Text = "Esci";

                labelPayMethod.Top = labelPrint.Top;
                comboPaymentType.Top = labelPayMethod.Top + 20;
            }
            else  // vis Ordini
            {
                this.Text = "Visualizza Ordini";

                AnnulloBtn.Enabled = false;
                AnnulloBtn.Visible = false;
                comboPaymentType.Enabled = false;
                comboPaymentType.Visible = false;
                labelPayMethod.Visible = false;

                BtnPrt.Enabled = true;

                lbl_Info.Visible = bUSA_NDB();
                lbl_Info.Top = BtnPrev.Top + 46;

                CkBoxTutteCasse.Enabled = true;
                OKBtn.Text = "OK";

                // compattazione
                if (!bUSA_NDB())
                {
                    BtnPrev.Left -= 200;
                    BtnNext.Left = BtnPrev.Left + 90;
                    //OKBtn.Left = BtnPrt.Left + 96;

                    BtnPrev.Top += 12;

                    BtnNext.Top = BtnPrev.Top;
                    BtnPrt.Top = BtnPrev.Top;
                    OKBtn.Top = BtnPrev.Top;

                    labelPrint.Top = BtnPrev.Top - 22;
                    checkBoxNotPaid.Top = labelPrint.Top - 2;

                    Height -= 50;
                    //textEdit_Ticket.Height = Height;   
                }
            }

            if (iNumParam == MAX_NUM_TICKET)
                _iNum = _iUpperLimit;
            else
                _iNum = iNumParam;

            // carica _Versione, _Header, _HeaderText
            _rdBaseIntf.dbCaricaOrdine(dateParam, 0, false, _sNomeTabella);

            LogToFile("VisOrdiniDlg : Init");

            if (!bHide)
            {
                // utilizza CkBoxTutteCasse
                VisualizzaTicket(_iNum, (int)SEARCH_TYPE.SEARCH_DOWN);

                AggiornaAspettoControlli();

                ShowDialog();
            }
        }

        /// <summary>
        /// Nel caso di più casse il numero dei scontrini succesivi su
        /// ogni Cassa può non essere contiguo, per cui ci si sposta
        /// in modo laborioso su numeri non contigui utilizzando
        /// SEARCH_DOWN, SEARCH_UP
        /// </summary>
        void VisualizzaTicket(int iNum, int iDir)
        {
            int i, iTmp;
            bool bFound = false;
            String sDir, sInStr, sTmp;
            StreamReader fTxtFile;

            LogToFile(String.Format("VisTicketsDlg : {0}, {1}", iNum, iDir));

            sDir = GetVisTicketsDir() + "\\";

            i = DB_Data.iStartingNumOfReceipts;

            // mantiene il filtraggio entro i limiti
            if (iNum < DB_Data.iStartingNumOfReceipts)
            {
                iNum = DB_Data.iStartingNumOfReceipts;
                _iNum = iNum; // esteso anche alla var membro
            }
            else
            {
                sTmp = _sDBTNameOrdini;
                iTmp = _iDBTNameOrdiniLength;

                if ((CkBoxTutteCasse.Checked) || (GetActualDate().ToString("dd/MM/yy") != _dateOrdine.ToString("dd/MM/yy")) ||
                    (_sDBTNameOrdini.Length > _iDBTNameOrdiniLength))
                {
                    if (iNum > _iUpperLimit)
                    {
                        iNum = _iUpperLimit;
                        _iNum = iNum; // esteso anche alla var membro
                    }
                }
#if STANDFACILE
                else
                {
                    if (iNum > DataManager.GetNumOfLocalOrders())
                    {
                        iNum = DataManager.GetNumOfLocalOrders();
                        _iNum = iNum; // esteso anche alla var membro
                    }
                }
#endif
            }

            if (iDir == (int)SEARCH_TYPE.SEARCH_UP) // ricerca a crescere
            {
                for (i = 0; ((i < SEARCH_LIMIT) && !bFound && ((iNum + i) <= _iUpperLimit)); i++)
                {
                    if (ReceiptRebuild(_dateOrdine, iNum + i))
                    {
                        bFound = true;
                        _iNum = iNum + i;
                        // aggiornamento
                    }
                }
            }
            else if (iDir == (int)SEARCH_TYPE.SEARCH_DOWN) // ricerca a crescere
            {
                for (i = 0; ((i < SEARCH_LIMIT) && !bFound && ((iNum - i) > 0)); i++)
                {
                    if (ReceiptRebuild(_dateOrdine, iNum - i))
                    {
                        bFound = true;
                        _iNum = iNum - i;
                        // aggiornamento
                    }
                }
            }
            else // NO_SEARCH
                bFound = ReceiptRebuild(_dateOrdine, iNum);

            // **** visualizzazione su textEdit ****
            textEdit_Ticket.Clear();
            textEdit_Ticket.ScrollBars = ScrollBars.None;

            if (File.Exists(sDir + _sNomeFileTicket) && bFound)
            {
                fTxtFile = File.OpenText(sDir + _sNomeFileTicket);

                while ((sInStr = fTxtFile.ReadLine()) != null)
                {
                    textEdit_Ticket.AppendText(sInStr + "\r\n");
                }

                fTxtFile.Close();

            }
            else
                textEdit_Ticket.AppendText("Il File non esiste !");

            toolStripNumTicket.Text = String.Format("Scontrino : {0}", _iNum);
            toolStripTotaleOrdine.Text = String.Format("Totali : {0}", _iUpperLimit);
            toolStripCassa.Text = String.Format("{0}", sConstCassaType[DB_Data.iNumCassa - 1]);

            textEdit_Ticket.ScrollBars = ScrollBars.Vertical;
            //textEdit_Ticket.SelectionStart = 0;
            //textEdit_Ticket.ScrollToCaret();
        }

        private void PrevBtn_Click(object sender, EventArgs e)
        {
            bool bSomePayment_IsPresent;
            int iDebug;

            do
            {
                _iNum -= 1;

                _rdBaseIntf.dbCaricaOrdine(GetActualDate(), _iNum, false);

                bSomePayment_IsPresent = IsBitSet(DB_Data.iStatusReceipt, (int)STATUS_FLAGS.BIT_PAGAM_CASH) ||
                    IsBitSet(DB_Data.iStatusReceipt, (int)STATUS_FLAGS.BIT_PAGAM_CARD) || IsBitSet(DB_Data.iStatusReceipt, (int)STATUS_FLAGS.BIT_PAGAM_SATISPAY);

                iDebug = DB_Data.iStatusReceipt;
            }
            while (checkBoxNotPaid.Checked && (_iNum > DB_Data.iStartingNumOfReceipts) && (bSomePayment_IsPresent || DB_Data.bAnnullato));

            if (!(checkBoxNotPaid.Checked && (bSomePayment_IsPresent || DB_Data.bAnnullato)))
                VisualizzaTicket(_iNum, (int)SEARCH_TYPE.SEARCH_DOWN);

            AggiornaAspettoControlli();
        }

        private void NextBtn_Click(object sender, EventArgs e)
        {
            bool bSomePayment_IsPresent;
            int iDebug;

            do
            {
                _iNum += 1;
                _rdBaseIntf.dbCaricaOrdine(GetActualDate(), _iNum, false);

                bSomePayment_IsPresent = IsBitSet(DB_Data.iStatusReceipt, (int)STATUS_FLAGS.BIT_PAGAM_CASH) ||
                    IsBitSet(DB_Data.iStatusReceipt, (int)STATUS_FLAGS.BIT_PAGAM_CARD) || IsBitSet(DB_Data.iStatusReceipt, (int)STATUS_FLAGS.BIT_PAGAM_SATISPAY);

                iDebug = DB_Data.iStatusReceipt;
            }
            while (checkBoxNotPaid.Checked && (_iNum < _iUpperLimit) && (bSomePayment_IsPresent || DB_Data.bAnnullato));

            if (!(checkBoxNotPaid.Checked && (bSomePayment_IsPresent || DB_Data.bAnnullato)))
                VisualizzaTicket(_iNum, (int)SEARCH_TYPE.SEARCH_UP);

            AggiornaAspettoControlli();
        }

        private void VisTicketsDlg_KeyDown(object sender, KeyEventArgs e)
        {
            int iKey = (int)e.KeyValue;

            switch (iKey)
            {
                case KEY_LEFT:
                case KEY_UP:
                    PrevBtn_Click(this, null);
                    break;

                case KEY_RIGHT:
                case KEY_DOWN:
                    NextBtn_Click(this, null);
                    break;

                case KEY_HOME:
                    VisualizzaTicket(1, (int)SEARCH_TYPE.SEARCH_UP);
                    break;
                case KEY_END:
                    VisualizzaTicket(MAX_NUM_TICKET, (int)SEARCH_TYPE.SEARCH_DOWN);
                    break;
                case KEY_ESC:
                    Close();
                    break;
                case KEY_PAGEUP:
                    _iNum -= 10;
                    VisualizzaTicket(_iNum, (int)SEARCH_TYPE.SEARCH_DOWN);
                    break;
                case KEY_PAGEDOWN:
                    _iNum += 10;
                    VisualizzaTicket(_iNum, (int)SEARCH_TYPE.SEARCH_UP);
                    break;

            }

            AggiornaAspettoControlli();
        }

        void AggiornaAspettoControlli()
        {
            if (_bCambiaPagamento)
            {
                if (IsBitSet(DB_Data.iStatusReceipt, (int)STATUS_FLAGS.BIT_PAGAM_CASH))
                    comboPaymentType.SelectedIndex = 1;

                else if (IsBitSet(DB_Data.iStatusReceipt, (int)STATUS_FLAGS.BIT_PAGAM_CARD))
                    comboPaymentType.SelectedIndex = 2;

                else if (IsBitSet(DB_Data.iStatusReceipt, (int)STATUS_FLAGS.BIT_PAGAM_SATISPAY))
                    comboPaymentType.SelectedIndex = 3;

                else
                    comboPaymentType.SelectedIndex = 0;
            }
        }

        /***************************************************************
         *   Invio alla Stampante, prima le copie e poi lo Scontrino
         ***************************************************************/
        private void BtnPrt_Click(object sender, EventArgs e)
        {
            bool bTicketCopy_NoPrice;

            _sNomeFileTicket = String.Format(NOME_FILE_RECEIPT, DB_Data.iNumCassa, _iNum);
            _sNomeFileTicketNpPrt = String.Format(NOME_FILE_RECEIPT_NP, DB_Data.iNumCassa, _iNum);

            bTicketCopy_NoPrice = IsBitSet(SF_Data.iReceiptCopyOptions, (int)LOCAL_COPIES_OPTS.BIT_RECEIPT_LOCAL_COPY_REQUIRED);

            // STAMPA SCONTRINO PRINCIPALE
#if STANDFACILE
            if (PrintLocalCopiesConfigDlg.GetPrinterTypeIsWinwows())
#else
            if (PrintConfigLightDlg.GetPrinterTypeIsWinwows())
#endif
            {
                Printer_Windows.PrintFile(GetVisTicketsDir() + "\\" + _sNomeFileTicket, sGlbWinPrinterParams, NUM_SEP_PRINT_GROUPS);

                if (bTicketCopy_NoPrice)
                    Printer_Windows.PrintFile(GetVisTicketsDir() + "\\" + _sNomeFileTicketNpPrt, sGlbWinPrinterParams, NUM_SEP_PRINT_GROUPS);
            }
            else
            {
                Printer_Legacy.PrintFile(GetVisTicketsDir() + "\\" + _sNomeFileTicket, sGlbLegacyPrinterParams,
                    (int)PRINT_QUEUE_ACTION.PRINT_ENQUEUE);

                if (bTicketCopy_NoPrice)
                    Printer_Legacy.PrintFile(GetVisTicketsDir() + "\\" + _sNomeFileTicketNpPrt, sGlbLegacyPrinterParams,
                    (int)PRINT_QUEUE_ACTION.PRINT_ENQUEUE);
            }

#if STANDFACILE
            int i;
            String sNomeFileCopiePrt;

            // controllo che non ci sia anche la copia scontrino no Price, sennò non si capisce più nulla !
            // *** MESSA IN CODA DI STAMPA COPIE ***
            for (i = 0; i < NUM_EDIT_GROUPS; i++)
            {
                sNomeFileCopiePrt = String.Format(NOME_FILE_COPIE, DB_Data.iNumCassa, _iNum, i);

                if (_dataIdentifierParam.bCopiesGroupsFlag[i])
                {
                    if (PrintLocalCopiesConfigDlg.GetPrinterTypeIsWinwows())
                        Printer_Windows.PrintFile(GetVisCopiesDir() + "\\" + sNomeFileCopiePrt, sGlbWinPrinterParams, NUM_SEP_PRINT_GROUPS);
                    else
                        Printer_Legacy.PrintFile(GetVisCopiesDir() + "\\" + sNomeFileCopiePrt, sGlbLegacyPrinterParams,
                        (int)PRINT_QUEUE_ACTION.PRINT_ENQUEUE);
                }
            }

            for (i = 0; i < MAX_NUM_ARTICOLI; i++)
            {
                if ((DB_Data.Articolo[i].iQuantitaOrdine > 0) && (DB_Data.Articolo[i].iGruppoStampa == (int)DEST_TYPE.DEST_SINGLE))
                {
                    sNomeFileCopiePrt = String.Format(NOME_FILE_COPIE_SINGOLE, DB_Data.iNumCassa, _iNum, (int)DEST_TYPE.DEST_SINGLE,
                                        DB_Data.Articolo[i].iIndexListino);

                    // *** MESSA IN CODA DI STAMPA COPIE SINGOLE ***
                    if (SF_Data.bCopiesGroupsFlag[(int)DEST_TYPE.DEST_SINGLE])
                    {
                        if (PrintLocalCopiesConfigDlg.GetPrinterTypeIsWinwows())
                            Printer_Windows.PrintFile(GetVisCopiesDir() + "\\" + sNomeFileCopiePrt, sGlbWinPrinterParams, NUM_SEP_PRINT_GROUPS);
                        else
                            Printer_Legacy.PrintFile(GetVisCopiesDir() + "\\" + sNomeFileCopiePrt, sGlbLegacyPrinterParams, (int)PRINT_QUEUE_ACTION.PRINT_ENQUEUE);
                    }
                }
            }

#endif

#if STANDFACILE
            if (!PrintLocalCopiesConfigDlg.GetPrinterTypeIsWinwows())
#else
            if (!PrintConfigLightDlg.GetPrinterTypeIsWinwows())
#endif
            {
                // Avvia eventuali code delle copie Legacy
                Printer_Legacy.PrintFile("", sGlbLegacyPrinterParams, (int)PRINT_QUEUE_ACTION.PRINT_START);
            }

            LogToFile(String.Format("VisOrdiniDlg : stampa {0}", _sNomeFileTicket));
        }

        /// <summary>se ricostruisce il file ritorna true/// </summary>
        public bool ReceiptRebuild(DateTime dateParam, int iParam)
        {
            bool bOrdineAnnullato, bSomePayment_IsPresent;

            bool[] bGroupsColorPrinted = new bool[NUM_EDIT_GROUPS];
            bool[] bSelectedGroups = new bool[NUM_EDIT_GROUPS];

            String sDir;

            StreamWriter fPrint = null;

            /**********************************************************
             *   carica l'ordine, serve anche per sapere iNumCassa
             **********************************************************/

            // va dopo dbAnnulloOrdine che entrambi azzerano DB_Data
            _bOrdineCaricato = _rdBaseIntf.dbCaricaOrdine(dateParam, iParam, !CkBoxTutteCasse.Checked, _sNomeTabella);

            bSomePayment_IsPresent = IsBitSet(DB_Data.iStatusReceipt, (int)STATUS_FLAGS.BIT_PAGAM_CASH) ||
                    IsBitSet(DB_Data.iStatusReceipt, (int)STATUS_FLAGS.BIT_PAGAM_CARD) || IsBitSet(DB_Data.iStatusReceipt, (int)STATUS_FLAGS.BIT_PAGAM_SATISPAY);

            InitFormatStrings(dbGetLengthArticoli());

            bOrdineAnnullato = DB_Data.bAnnullato;

            // importante la priorità, in ordine di gravità
            if (bOrdineAnnullato)
            {
                textEdit_Ticket.ForeColor = System.Drawing.SystemColors.Window;
                textEdit_Ticket.BackColor = _clrAnnullatoBkgr; // annullato
                AnnulloBtn.Enabled = false;
            }
            else if (!bSomePayment_IsPresent)
            {
                textEdit_Ticket.ForeColor = System.Drawing.Color.Black;
                textEdit_Ticket.BackColor = _clrNonAncoraPagatoBkgr; // da pagare

                if (DB_Data.iNumCassa == SF_Data.iNumCassa)
                    AnnulloBtn.Enabled = _bAnnulloOrdine;
            }
            else if (DB_Data.bScaricato && (DB_Data.iNumCassa == SF_Data.iNumCassa))
            {
                textEdit_Ticket.ForeColor = System.Drawing.Color.Black;
                textEdit_Ticket.BackColor = _clrScaricatoBkgr; // bScaricato

                AnnulloBtn.Enabled = true;
            }
            else if (DB_Data.bScaricato && (DB_Data.iNumCassa != SF_Data.iNumCassa))
            {
                textEdit_Ticket.ForeColor = System.Drawing.Color.Black;
                textEdit_Ticket.BackColor = _clrEAC_ScaricatoBkgr; // bScaricato

                if (DB_Data.iNumCassa == CASSA_PRINCIPALE)
                    AnnulloBtn.Enabled = true;
                else
                    AnnulloBtn.Enabled = false; // già consegnato da una CASSA_SECONDARIA diversa
            }
            // questi ultimi 2 casi sono esaustivi del 100% delle situazioni restanti
            else if (DB_Data.iNumCassa == SF_Data.iNumCassa)
            {
                textEdit_Ticket.ForeColor = System.Drawing.SystemColors.Window;
                textEdit_Ticket.BackColor = _clrNonAncoraScaricatoBkgr; // tutto OK
                AnnulloBtn.Enabled = _bAnnulloOrdine;
            }
            else if (DB_Data.iNumCassa != SF_Data.iNumCassa) // cassa diversa
            {
                textEdit_Ticket.ForeColor = System.Drawing.SystemColors.Window;
                textEdit_Ticket.BackColor = _clrEAC_NonAncoraScaricatoBkgr;
                AnnulloBtn.Enabled = false; // altra cassa
            }

            if (!_bOrdineCaricato) // solo caso di return false
                return false;
            else
            {
                /********************************************** 
                 *  dati preliminari alla scrittura del file 
                 **********************************************/

                TOrdineStrings sOrdineStrings = new TOrdineStrings();

                sOrdineStrings = SetupHeaderStrings(DB_Data, iParam);

                /**************************************
                 *      RICOSTRUZIONE SCONTRINO       *
                 **************************************/

                // costruzione del nome del file scontrino

                _sNomeFileTicket = String.Format(NOME_FILE_RECEIPT, DB_Data.iNumCassa, iParam);
                _ErrMsg.sNomeFile = _sNomeFileTicket;

                sDir = GetVisTicketsDir() + "\\";

                WriteReceipt(ref DB_Data, iParam, fPrint, sDir, sOrdineStrings);

#if STANDFACILE
                /************************************************
                 ***        RICOSTRUZIONE COPIE LOCALI        ***
                 ************************************************/

                sDir = GetVisTicketsDir() + "\\";

                WriteLocalCopy(DB_Data, iParam, sDir, sOrdineStrings);
#endif

#if STANDFACILE || STAND_CUCINA
                /***********************************************************
                 *                 RICOSTRUZIONE COPIE
                 * i contatori sono inclusi nelle PIETANZE es: COPERTI
                 ***********************************************************/

                sDir = GetVisCopiesDir() + "\\";

                WriteNetworkCopy(DB_Data, iParam, fPrint, sDir, sOrdineStrings, false);
#endif

                // terzo return
                return true;
            }
        }

        private void CkBoxTutteCasse_Click(object sender, EventArgs e)
        {
            VisualizzaTicket(_iNum, (int)SEARCH_TYPE.NO_SEARCH);
        }

        private void Lbl_Info_Click(object sender, EventArgs e)
        {
            int durationMilliseconds = 10000;
            tt.Show(_tt_InfoLabelText, lbl_Info, durationMilliseconds);
        }

        private void ComboCashPos_SelectedIndexChanged(object sender, EventArgs e)
        {
#if STANDFACILE
            int iNewStatus;
            DialogResult dResult;
            string sPagamento;

            iNewStatus = DB_Data.iStatusReceipt;

            switch (comboPaymentType.SelectedIndex)
            {
                case 0:
                    iNewStatus = ClearBit(iNewStatus, (int)STATUS_FLAGS.BIT_PAGAM_CASH);
                    iNewStatus = ClearBit(iNewStatus, (int)STATUS_FLAGS.BIT_PAGAM_CARD);
                    iNewStatus = ClearBit(iNewStatus, (int)STATUS_FLAGS.BIT_PAGAM_SATISPAY);
                    break;
                case 1:
                    iNewStatus = SetBit(iNewStatus, (int)STATUS_FLAGS.BIT_PAGAM_CASH);
                    iNewStatus = ClearBit(iNewStatus, (int)STATUS_FLAGS.BIT_PAGAM_CARD);
                    iNewStatus = ClearBit(iNewStatus, (int)STATUS_FLAGS.BIT_PAGAM_SATISPAY);
                    break;
                case 2:
                    iNewStatus = ClearBit(iNewStatus, (int)STATUS_FLAGS.BIT_PAGAM_CASH);
                    iNewStatus = SetBit(iNewStatus, (int)STATUS_FLAGS.BIT_PAGAM_CARD);
                    iNewStatus = ClearBit(iNewStatus, (int)STATUS_FLAGS.BIT_PAGAM_SATISPAY);
                    break;
                case 3:
                    iNewStatus = ClearBit(iNewStatus, (int)STATUS_FLAGS.BIT_PAGAM_CASH);
                    iNewStatus = ClearBit(iNewStatus, (int)STATUS_FLAGS.BIT_PAGAM_CARD);
                    iNewStatus = SetBit(iNewStatus, (int)STATUS_FLAGS.BIT_PAGAM_SATISPAY);
                    break;
                default:
                    break;
            }

            if (iNewStatus == DB_Data.iStatusReceipt)
                return;

            if (IsBitSet(DB_Data.iStatusReceipt, (int)STATUS_FLAGS.BIT_PAGAM_CASH))
                sPagamento = "Pagamento corrente in contanti\n\nSei sicuro di voler cambiare il tipo di pagamento ?";

            else if (IsBitSet(DB_Data.iStatusReceipt, (int)STATUS_FLAGS.BIT_PAGAM_CARD))
                sPagamento = "Pagamento corrente di tipo Card\n\nSei sicuro di voler cambiare il tipo di pagamento ?";

            else if (IsBitSet(DB_Data.iStatusReceipt, (int)STATUS_FLAGS.BIT_PAGAM_SATISPAY))
                sPagamento = "Pagamento corrente di tipo Satispay\n\nSei sicuro di voler cambiare il tipo di pagamento ?";

            else
                sPagamento = "Sei sicuro di voler applicare un tipo di pagamento ?";

            dResult = MessageBox.Show(sPagamento, "Attenzione !", MessageBoxButtons.YesNo);

            if (dResult == DialogResult.Yes)
            {
                if (_rdBaseIntf.dbEditStatus(_iNum, iNewStatus))
                {
                    DB_Data.iStatusReceipt = iNewStatus;
                    VisualizzaTicket(_iNum, (int)SEARCH_TYPE.NO_SEARCH);
                }
            }
#endif

        }

        /// <summary>
        /// ci passa solo se la data è quella attuale perché 
        /// altrimenti il bottone viene disabilitato
        /// </summary>
        private void AnnulloBtn_Click(object sender, EventArgs e)
        {
#if STANDFACILE
            String[] sQueue_Object = new String[2];
            DialogResult dResult;

            dResult = MessageBox.Show("Sei sicuro di voler annullare l'ordine selezionato ?", "Attenzione !", MessageBoxButtons.YesNo);

            if (dResult == DialogResult.Yes)
            {
                dResult = MessageBox.Show("Ne sei proprio sicuro ?\n\nGli articoli dell'ordine verranno contrassegnati " +
                        "come 'annullati' ed eliminati dal conteggio!", "Attenzione !", MessageBoxButtons.YesNo);

                if (dResult == DialogResult.Yes)
                {
                    DataManager.AnnulloOrdine(_iNum);
                    VisualizzaTicket(_iNum, (int)SEARCH_TYPE.NO_SEARCH);

                    // avvia il refresh della griglia principale
                    sQueue_Object[0] = MAIN_GRID_UPDATE_EVENT;
                    sQueue_Object[1] = "";

                    FrmMain.EventEnqueue(sQueue_Object);
                }
            }
#endif
        }

        private string GetVisTicketsDir()
        {
            string sDir;

#if STANDFACILE

            if (GetActualDate().ToString("dd/MM/yy") == _dateOrdine.ToString("dd/MM/yy"))
            {
                sDir = DataManager.GetRootDir() + "\\" + ANNO_DIR + _dateOrdine.ToString("yyyy") + "\\" + NOME_DIR_RECEIPTS + _dateOrdine.ToString("MMdd");
            }
            else
            {
                sDir = DataManager.GetRootDir() + "\\" + ANNO_DIR + _dateOrdine.ToString("yyyy") + "\\" + NOME_DIR_RECEIPTS_VO;
            }
#elif STAND_CUCINA
            sDir = sRootDir + "\\";
#else
            sDir = sRootDir + "\\" + NOME_DIR_RECEIPTS_VO + "\\";
#endif

            if (!Directory.Exists(sDir))
                Directory.CreateDirectory(sDir);

            return sDir;
        }
        private string GetVisCopiesDir()
        {
            string sDir;

#if STANDFACILE
            if (GetActualDate().ToString("dd/MM/yy") == _dateOrdine.ToString("dd/MM/yy"))
            {
                sDir = DataManager.GetRootDir() + "\\" + ANNO_DIR + _dateOrdine.ToString("yyyy") + "\\" + NOME_DIR_COPIES + _dateOrdine.ToString("MMdd");
            }
            else
            {
                sDir = DataManager.GetRootDir() + "\\" + ANNO_DIR + _dateOrdine.ToString("yyyy") + "\\" + NOME_DIR_COPIES_VO;
            }
#elif STAND_CUCINA
            sDir = sRootDir;
#else
            sDir = sRootDir + "\\" + NOME_DIR_COPIES_VO;
#endif

            if (!Directory.Exists(sDir))
                Directory.CreateDirectory(sDir);

            return sDir;
        }

        private void VisOrdiniDlg_FormClosing(object sender, FormClosingEventArgs e)
        {

#if STANDFACILE || STAND_MONITOR
            String sNomeDirTmp;
#endif

#if STANDFACILE
            // operazione sicura con Directory NOME_DIR_RECEIPTS_VO
            if (GetActualDate().ToString("dd/MM/yy") != _dateOrdine.ToString("dd/MM/yy"))
            {
                // cancellazione tickets VisOrdini
                sNomeDirTmp = GetVisTicketsDir();

                if (Directory.Exists(sNomeDirTmp))
                    Directory.Delete(sNomeDirTmp, true);

                // cancellazione copie VisOrdini
                sNomeDirTmp = GetVisCopiesDir();

                if (Directory.Exists(sNomeDirTmp))
                    Directory.Delete(sNomeDirTmp, true);
            }

#elif STAND_MONITOR

            /**************************************
             * cancellazione tickets VisOrdini
             * cartella locale a StandMonitor
             **************************************/
            sNomeDirTmp = GetVisTicketsDir();

            if (Directory.Exists(sNomeDirTmp))
                Directory.Delete(sNomeDirTmp, true);

            // cancellazione copie VisOrdini
            sNomeDirTmp = GetVisCopiesDir();

            if (Directory.Exists(sNomeDirTmp))
                Directory.Delete(sNomeDirTmp, true);
#endif
        }

    }
}

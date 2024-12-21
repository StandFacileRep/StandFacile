/***************************************************************************
	NomeFile : StandCommonSrc/VisOrdiniDlg.cs
    Data	 : 06.12.2024
	Autore	  : Mauro Artuso
	 
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

        const int SEARCH_LIMIT = 999;

        /// <summary>massimo numero di scontrini per ricerca</summary>
        public const int MAX_NUM_TICKET = 9999;

        bool _bOrdineCaricato, _bAnnulloOrdine;
        int _iNum, _iUpperLimit;
        DateTime _dateOrdine;
        String _sNomeFileTicket, _sNomeFileTicketNpPrt, _sNomeTabella;

        bool[] _bScontoGruppo = new bool[NUM_EDIT_GROUPS];

        TErrMsg _ErrMsg;

#if STANDFACILE
        TData _dataIdentifierParam;
#endif

        /// <summary>costruttore</summary>
        public VisOrdiniDlg(DateTime dateParam, int iNumParam, String sNomeTabellaParam = "", VIEW_TYPE eViewTypeParam = VIEW_TYPE.NORMAL)
        {
            bool bCambiaPagamento, bHide;

            InitializeComponent();

            _iNum = 0;
            _dateOrdine = dateParam;
            _sNomeTabella = sNomeTabellaParam;

            _bAnnulloOrdine = (eViewTypeParam == VIEW_TYPE.CANCEL_ORDER);
            bCambiaPagamento = (eViewTypeParam == VIEW_TYPE.CHANGE_PAYMENT);
            bHide = (eViewTypeParam == VIEW_TYPE.NO_VIEW);

            // SQLite
            if (!bUSA_NDB())
            {
                CkBoxTutteCasse.Visible = false;
                CkBoxTutteCasse.Enabled = false;

                AnnulloBtn.Top = BtnPrev.Top;
            }

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

            if (_bAnnulloOrdine) // Annullo
            {
                this.Text = "Annulla Ordine";

                comboCashPos.Enabled = false;
                comboCashPos.Visible = false;
                labelPayMethod.Visible = false;

                if (GetActualDate().ToString("dd/MM/yy") == dateParam.ToString("dd/MM/yy"))
                {
                    AnnulloBtn.Enabled = true;
                    AnnulloBtn.Visible = true;
                }

                BtnPrt.Enabled = false;

                CkBoxTutteCasse.Enabled = ((SF_Data.iNumCassa == CASSA_PRINCIPALE) && bUSA_NDB());
                OKBtn.Text = "Esci";
            }
            else if (bCambiaPagamento)
            {
                this.Text = "Cambia pagamento Ordini";

                comboCashPos.Items.Clear();

                for (int i = sConst_PaymentType.Length - 1; i >= 0; i--) // OK
                    comboCashPos.Items.Insert(0, sConst_PaymentType[i]);

                comboCashPos.Top = AnnulloBtn.Top;
                labelPayMethod.Top = comboCashPos.Top - 20;

                AnnulloBtn.Enabled = false;
                AnnulloBtn.Visible = false;

                if (GetActualDate().ToString("dd/MM/yy") == dateParam.ToString("dd/MM/yy"))
                {
                    comboCashPos.Enabled = true;
                }

                BtnPrt.Enabled = false;

                CkBoxTutteCasse.Enabled = ((SF_Data.iNumCassa == CASSA_PRINCIPALE) && bUSA_NDB());
                comboCashPos.Visible = true;
                labelPayMethod.Visible = true;

                OKBtn.Text = "Esci";
            }
            else  // vis Ordini
            {
                this.Text = "Visualizza Ordini";

                AnnulloBtn.Enabled = false;
                AnnulloBtn.Visible = false;
                comboCashPos.Enabled = false;
                comboCashPos.Visible = false;
                labelPayMethod.Visible = false;

                BtnPrt.Enabled = true;

                CkBoxTutteCasse.Enabled = true;
                OKBtn.Text = "OK";

                // compattazione
                if (!bUSA_NDB())
                {
                    BtnPrev.Left -= 200;
                    BtnNext.Left = BtnPrev.Left + 90;
                    OKBtn.Left = BtnPrt.Left + 96;

                    BtnPrt.Top = BtnPrev.Top;
                    OKBtn.Top = BtnPrev.Top;

                    Height -= 50;
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

                if (bCambiaPagamento)
                {
                    if (IsBitSet(DB_Data.iStatusReceipt, BIT_PAGAM_CARD))
                        comboCashPos.SelectedIndex = 1;

                    else if (IsBitSet(DB_Data.iStatusReceipt, BIT_PAGAM_SATISPAY))
                        comboCashPos.SelectedIndex = 2;

                    else
                        comboCashPos.SelectedIndex = 0;
                }

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
            _iNum -= 1;
            VisualizzaTicket(_iNum, (int)SEARCH_TYPE.SEARCH_DOWN);
        }

        private void NextBtn_Click(object sender, EventArgs e)
        {
            _iNum += 1;
            VisualizzaTicket(_iNum, (int)SEARCH_TYPE.SEARCH_UP);
        }

        private void VisTicketsDlg_KeyDown(object sender, KeyEventArgs e)
        {
            int iKey = (int)e.KeyValue;

            switch (iKey)
            {
                case KEY_LEFT:
                case KEY_UP:
                    _iNum -= 1;
                    VisualizzaTicket(_iNum, (int)SEARCH_TYPE.SEARCH_DOWN);
                    break;

                case KEY_RIGHT:
                case KEY_DOWN:
                    _iNum += 1;
                    VisualizzaTicket(_iNum, (int)SEARCH_TYPE.SEARCH_UP);
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
        }

        /***************************************************************
         *   Invio alla Stampante, prima le copie e poi lo Scontrino
         ***************************************************************/
        private void BtnPrt_Click(object sender, EventArgs e)
        {
            bool bTicketCopy_NoPrice;

            _sNomeFileTicket = String.Format(NOME_FILE_RECEIPT, DB_Data.iNumCassa, _iNum);
            _sNomeFileTicketNpPrt = String.Format(NOME_FILE_RECEIPT_NP, DB_Data.iNumCassa, _iNum);

            bTicketCopy_NoPrice = IsBitSet(SF_Data.iReceiptCopyOptions, BIT_RECEIPT_LOCAL_COPY_REQUIRED);

            // STAMPA SCONTRINO PRINCIPALE
#if STANDFACILE
            if (PrintReceiptConfigDlg.GetPrinterTypeIsWinwows())
#else
            if (PrintConfigLightDlg.GetPrinterTypeIsWinwows())
#endif
            {
                Printer_Windows.PrintFile(GetVisTicketsDir() + "\\" + _sNomeFileTicket, sGlbWinPrinterParams, NUM_EDIT_GROUPS);

                if (bTicketCopy_NoPrice)
                    Printer_Windows.PrintFile(GetVisTicketsDir() + "\\" + _sNomeFileTicketNpPrt, sGlbWinPrinterParams, NUM_EDIT_GROUPS);
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
                    if (PrintReceiptConfigDlg.GetPrinterTypeIsWinwows())
                        Printer_Windows.PrintFile(GetVisCopiesDir() + "\\" + sNomeFileCopiePrt, sGlbWinPrinterParams, NUM_EDIT_GROUPS);
                    else
                        Printer_Legacy.PrintFile(GetVisCopiesDir() + "\\" + sNomeFileCopiePrt, sGlbLegacyPrinterParams,
                        (int)PRINT_QUEUE_ACTION.PRINT_ENQUEUE);
                }
            }

#endif

#if STANDFACILE
            if (!PrintReceiptConfigDlg.GetPrinterTypeIsWinwows())
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
            bool bOrdineAnnullato;

            bool[] bGroupsColorPrinted = new bool[NUM_EDIT_GROUPS];
            bool[] bSelectedGroups = new bool[NUM_EDIT_GROUPS];

            String sDir;

            StreamWriter fPrint = null;

            /**********************************************************
             *   carica l'ordine, serve anche per sapere iNumCassa
             **********************************************************/

            // va dopo dbAnnulloOrdine che entrambi azzerano DB_Data
            _bOrdineCaricato = _rdBaseIntf.dbCaricaOrdine(dateParam, iParam, !CkBoxTutteCasse.Checked, _sNomeTabella);

            bOrdineAnnullato = DB_Data.bAnnullato;

            if (DB_Data.bScaricato)
            {
                textEdit_Ticket.ForeColor = System.Drawing.Color.Black;
                textEdit_Ticket.BackColor = System.Drawing.Color.Gold; // bScaricato
                AnnulloBtn.Enabled = false;
            }
            else if (!bOrdineAnnullato && (DB_Data.iNumCassa == SF_Data.iNumCassa))
            {
                textEdit_Ticket.ForeColor = System.Drawing.SystemColors.Window;
                textEdit_Ticket.BackColor = System.Drawing.Color.Teal; // tutto OK
                AnnulloBtn.Enabled = _bAnnulloOrdine;
            }
            else if (!bOrdineAnnullato && (DB_Data.iNumCassa != SF_Data.iNumCassa)) // cassa diversa
            {
                textEdit_Ticket.ForeColor = System.Drawing.SystemColors.Window;
                textEdit_Ticket.BackColor = System.Drawing.Color.RoyalBlue;
                AnnulloBtn.Enabled = false;
            }
            else
            {
                textEdit_Ticket.ForeColor = System.Drawing.SystemColors.Window;
                textEdit_Ticket.BackColor = System.Drawing.Color.Red; // annullato
                AnnulloBtn.Enabled = false;
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

        /// <summary>
        /// da utilizzare solo per le verifiche di scontrino scontato significativo, <br/>
        /// utilizzato da DataManager per inserire nota nello Scontrino
        /// </summary>
        public bool TicketScontatoIsGood()
        {
            int i;
            int iTotaleScontatoCurrTicket = 0;

            double fPerc = (double)((DB_Data.iStatusSconto & 0x00FF0000) >> 16) / 100.0d;

            // totale scontrino corrente
            for (i = 0; i < MAX_NUM_ARTICOLI; i++)
            {
                if ((DB_Data.Articolo[i].iGruppoStampa < NUM_EDIT_GROUPS) && _bScontoGruppo[DB_Data.Articolo[i].iGruppoStampa])
                    iTotaleScontatoCurrTicket += (int)Math.Round(DB_Data.Articolo[i].iQuantitaOrdine * DB_Data.Articolo[i].iPrezzoUnitario * fPerc);
            }

            return (iTotaleScontatoCurrTicket > 0);
        }

        private void ComboCashPos_SelectedIndexChanged(object sender, EventArgs e)
        {
#if STANDFACILE
            int iNewStatus;
            DialogResult dResult;
            string sPagamento;

            iNewStatus = DB_Data.iStatusReceipt;

            switch (comboCashPos.SelectedIndex)
            {
                case 0:
                    iNewStatus = ClearBit(iNewStatus ,BIT_PAGAM_CARD);
                    iNewStatus = ClearBit(iNewStatus, BIT_PAGAM_SATISPAY);
                    break;
                case 1:
                    iNewStatus = SetBit(iNewStatus , BIT_PAGAM_CARD);
                    iNewStatus = ClearBit(iNewStatus, BIT_PAGAM_SATISPAY);
                    break;
                case 2:
                    iNewStatus = ClearBit(iNewStatus , BIT_PAGAM_CARD);
                    iNewStatus = SetBit(iNewStatus, BIT_PAGAM_SATISPAY);
                    break;
                default:
                    break;
            }

            if (iNewStatus == DB_Data.iStatusReceipt)
                return;

            if (IsBitSet(DB_Data.iStatusReceipt, BIT_PAGAM_CARD))
                sPagamento = "Pagammento corrente di tipo Card\n\nSei sicuro di voler cambiare il tipo di pagamento ?";

            else if (IsBitSet(DB_Data.iStatusReceipt, BIT_PAGAM_SATISPAY))
                sPagamento = "Pagammento corrente di tipo Satispay\n\nSei sicuro di voler cambiare il tipo di pagamento ?";

            else
                sPagamento = "Pagamento corrente di tipo Contanti\n\nSei sicuro di voler cambiare il tipo di pagamento ?";

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

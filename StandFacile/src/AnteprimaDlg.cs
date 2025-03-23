/********************************************************************
  	NomeFile : StandFacile/AnteprimaDlg.cs
	Data	 : 06.12.2024
  	Autore   : Mauro Artuso

  Classe di visualizzazione dell'anteprima dello scontrino.
 ********************************************************************/

using System;
using System.Drawing;
using System.Windows.Forms;

using static StandCommonFiles.ComDef;
using static StandCommonFiles.CommonCl;
using static StandCommonFiles.LogServer;
using static StandCommonFiles.ReceiptAndCopies;

using static StandFacile.glb;

namespace StandFacile
{
    /// <summary>
    /// classe per una anteprima dello scontrino, rispetto a Receipt()
    /// ha uno spzio in più come margine sx
    /// </summary>
    public partial class AnteprimaDlg : Form
    {
        #pragma warning disable IDE0044

        const float _fTM_T88_IV_PAPER_WIDTH = (50 + 521.0f);

        /// <summary>riferimento a AnteprimaDlg</summary>
        public static AnteprimaDlg rAnteprimaDlg;

        static bool _bInit = false;

        bool[] _bScontoGruppo = new bool[NUM_EDIT_GROUPS];

        static bool[] _bSomethingInto_GrpToPrint = new bool[NUM_COPIES_GRPS];
        static bool[] _bSomethingInto_ClrToPrint = new bool[NUM_COPIES_GRPS];

        static int _iTotaleTicket, _iTotaleDovutoTicket;

        float _fHZoom, _fVZoom;
        float _fLeftMargin, fLogo_LeftMargin;

        /// <summary>
        /// _fCanvasVertPos cursore verticale per il disegno nel canvas<br/>
        /// _fCanvasVertNumPos memorizza la posizione verticale del numero dell'ordine
        /// </summary>
        float _fCanvasVertPos, _fCanvasVertNumPos;

        Font _printFont;
        Bitmap bmpCanvas;
        Graphics pg;

        /// <summary>ottiene il TC = Totale Corrente</summary>
        public static int GetTotaleReceipt() { return _iTotaleDovutoTicket; }

        /// <summary>costruttore</summary>
        public AnteprimaDlg()
        {
            InitializeComponent();

            MinimumSize = new Size(380, 600);
            MaximumSize = new Size(600, 1200);

            Height = 550;

            rAnteprimaDlg = this;

            _printFont = new Font("Lucida Console", 12);

            picBox.Left = 0;
            picBox.Top = 0;

            picBox.Width = rAnteprimaDlg.Width - 48; // spazio per scrollbar
            picBox.Height = panel.Height + 1;
            Height = FrmMain.rFrmMain.Height;

            _bInit = true;
            _fCanvasVertNumPos = 0;
        }

        /// <summary>aggiornamento anteprima</summary>
        public void RedrawReceipt()
        {
            int i, j;
            int iIncassoParz, iScontoStdTicket;
            String sTmp,sIncassoParz;

            _iTotaleTicket = 0;
            _iTotaleDovutoTicket = 0;

            iScontoStdTicket = 0;

            if (!_bInit) return;

            if (Visible)
            {
                picBox.Width = rAnteprimaDlg.Width - 48; // spazio per scrollbar
                picBox.Height = panel.Height + 1;

                bmpCanvas = new Bitmap(picBox.Width, 4000);
                pg = Graphics.FromImage(bmpCanvas);

                pg.FillRectangle(new SolidBrush(Color.White), new Rectangle(0, 0, picBox.Width, 4000));

                // Zoom da applicare ai pixels, 1 = stessa risoluzione della TM-T88IV
                _fHZoom = picBox.Width / _fTM_T88_IV_PAPER_WIDTH;
                _fVZoom = _fHZoom;

                pg.Clear(Color.White);
                PrintCanvas(pg, "");
            }

            //if (!Visible)                       // controllo
            //    return;

            // i = SF_Data.iNumOfLastReceipt + 1; // prossimo probabile scontrino

            #pragma warning disable IDE0059
            TOrdineStrings sOrdineStrings = new TOrdineStrings();

            sOrdineStrings = SetupHeaderStrings(SF_Data, 0, pg);

            _fLeftMargin = (10 + sGlbWinPrinterParams.iRepLeftMargin) * _fHZoom;

            _fCanvasVertPos = 0;

            // inizio scrittura
            PrintCanvas(pg, "");

            /*************************************
             * 		   Stampa del Logo
             *************************************/
            if (Visible && checkBoxLogo.Checked && !String.IsNullOrEmpty(sGlbWinPrinterParams.sLogoName) && PrintReceiptConfigDlg.GetPrinterTypeIsWinwows())
            {
                Image img = WinPrinterDlg._rWinPrinterDlg.GetWinPrinterLogo();

                if (img != null)
                {
                    fLogo_LeftMargin = ((sGlbWinPrinterParams.iRepLeftMargin + _fTM_T88_IV_PAPER_WIDTH - img.Size.Width) / 2.0f) * _fHZoom;

                    if (fLogo_LeftMargin < 0)
                        fLogo_LeftMargin = _fLeftMargin;

                    RectangleF imageRect = new RectangleF(fLogo_LeftMargin, _fCanvasVertPos, img.Size.Width * _fHZoom, img.Size.Height * _fVZoom);

                    pg.DrawImage(img, imageRect);

                    _fCanvasVertPos += img.Size.Height * _fVZoom;

                    PrintCanvas(pg, "");
                }
            }

            /*************************************
             * 		   Stampa del testo
             *************************************/
            //PrintCanvas( pg, "#########_#########_########");

            // se non c'è il logo stampa sHeaders[0]
            if (((iSysPrinterType == (int)PRINTER_SEL.STAMPANTE_WINDOWS) && string.IsNullOrEmpty(sGlbWinPrinterParams.sLogoName)) ||
                ((iSysPrinterType == (int)PRINTER_SEL.STAMPANTE_LEGACY) && (sGlbLegacyPrinterParams.iLogoBmp == 0)))
            {
                if (!String.IsNullOrEmpty(SF_Data.sHeaders[0]))
                {
                    sTmp = CenterJustify(SF_Data.sHeaders[0], iMAX_RECEIPT_CHARS);
                    PrintCanvas(pg, sTmp);
                    PrintCanvas(pg, "");
                }
            }

            if (!String.IsNullOrEmpty(SF_Data.sHeaders[1]))
            {
                sTmp = CenterJustify(SF_Data.sHeaders[1], iMAX_RECEIPT_CHARS);
                PrintCanvas(pg, sTmp);
                PrintCanvas(pg, "");
            }

            // meglio fare vedere sempre la composizione es. Esportazione
            // anche con Scontrino di valore nullo
            //
            //if (!DataManager.TicketIsGood())   // controllo
            //{
            //    PrintCanvas(pg, "");
            //    sTmp = CenterJustify("anteprima scontrino", iMAX_RECEIPT_CHARS);
            //    PrintCanvas(pg, sTmp);

            //    PrintCanvas(pg, "");
            //    PrintCanvas(pg, "");
            //    PrintCanvas(pg, "");
            //    sTmp = CenterJustify(URL_SITO, iMAX_RECEIPT_CHARS);
            //    PrintCanvas(pg, sTmp);

            //    picBox.Refresh();

            //    return;
            //}

            sTmp = String.Format("{0,-22}C.{1}", GetDateTimeString(), SF_Data.iNumCassa);
            sTmp = CenterJustify(sTmp, iMAX_RECEIPT_CHARS);
            PrintCanvas(pg, sTmp);
            PrintCanvas(pg, "");

            _fCanvasVertNumPos = _fCanvasVertPos;

            if (!String.IsNullOrEmpty(sOrdineStrings.sTavolo) && !String.IsNullOrEmpty(sOrdineStrings.sNome))
            {
                PrintCanvas(pg, 1.20f, 1.50f, sOrdineStrings.sOrdineNum); PrintCanvas(pg, "");
                PrintCanvas(pg, sOrdineStrings.sTavolo);
                PrintCanvas(pg, sOrdineStrings.sNome);
            }
            else if (!String.IsNullOrEmpty(sOrdineStrings.sTavolo))
            {
                PrintCanvas(pg, 1.20f, 1.50f, sOrdineStrings.sOrdineNum); PrintCanvas(pg, "");
                PrintCanvas(pg, sOrdineStrings.sTavolo);
            }
            else if (!String.IsNullOrEmpty(sOrdineStrings.sNome))
            {
                PrintCanvas(pg, 1.20f, 1.50f, sOrdineStrings.sOrdineNum); PrintCanvas(pg, "");
                PrintCanvas(pg, sOrdineStrings.sNome);
            }
            else
            {
                PrintCanvas(pg, 1.20f, 1.50f, sOrdineStrings.sOrdineNum);
            }

            PrintCanvas(pg, "");

            if (IsBitSet(SF_Data.iStatusReceipt, BIT_CARICATO_DA_WEB))
            {
                PrintCanvas(pg, sOrdineStrings.sOrdNumWeb);
                PrintCanvas(pg, "");
            }

            if (IsBitSet(SF_Data.iStatusReceipt, BIT_CARICATO_DA_PREVENDITA))
            {
                PrintCanvas(pg, sOrdineStrings.sOrdNumPrev);
                PrintCanvas(pg, "");
            }

            CheckSomethingToPrint(_bSomethingInto_GrpToPrint, _bSomethingInto_ClrToPrint, SF_Data);

            double fPerc = ((SF_Data.iStatusSconto & 0x00FF0000) >> 16) / 100.0f;

            for (i = 0; i < NUM_EDIT_GROUPS; i++)
                _bScontoGruppo[i] = IsBitSet(SF_Data.iStatusSconto, 8 + i);

            SF_Data.iScontoStdReceipt = 0;   // richiede calcolo
            SF_Data.iScontoGratisReceipt = 0; // richiede calcolo

            // stampa CONTATORI
            if (_bSomethingInto_GrpToPrint[(int)DEST_TYPE.DEST_COUNTER])
            {
                for (j = 0; j < MAX_NUM_ARTICOLI; j++)
                {
                    if ((SF_Data.Articolo[j].iQuantitaOrdine > 0) && (SF_Data.Articolo[j].iGruppoStampa == (int)DEST_TYPE.DEST_COUNTER))
                    {
                        if (SF_Data.Articolo[j].sTipo == _COPERTO)
                        {
                            iIncassoParz = SF_Data.Articolo[j].iQuantitaOrdine * SF_Data.Articolo[j].iPrezzoUnitario;
                            _iTotaleTicket += iIncassoParz;
                            sIncassoParz = IntToEuro(iIncassoParz);
                        }
                        else
                            sIncassoParz = "";

                        sTmp = String.Format(sRCP_FMT_RCPT, SF_Data.Articolo[j].iQuantitaOrdine, SF_Data.Articolo[j].sTipo, sIncassoParz);
                        PrintCanvas(pg, sTmp);

                        if (!String.IsNullOrEmpty(SF_Data.Articolo[j].sNotaArt))
                        {
                            sTmp = String.Format(sRCP_FMT_NOTE, SF_Data.Articolo[j].sNotaArt);
                            PrintCanvas(pg, sTmp);
                        }

                    }
                }

                PrintCanvas(pg, "");
            }

            for (i = 0; i < NUM_EDIT_GROUPS; i++)
            {
                if (_bSomethingInto_GrpToPrint[i])
                {
                    for (j = 0; j < MAX_NUM_ARTICOLI; j++)
                    {
                        if ((SF_Data.Articolo[j].iQuantitaOrdine > 0) && (SF_Data.Articolo[j].iGruppoStampa == i))
                        {
                            if (IsBitSet(SF_Data.iStatusSconto, BIT_SCONTO_STD) && _bScontoGruppo[i])
                                iScontoStdTicket += (int)Math.Round(SF_Data.Articolo[j].iQuantitaOrdine * SF_Data.Articolo[j].iPrezzoUnitario * fPerc);

                            iIncassoParz = SF_Data.Articolo[j].iQuantitaOrdine * SF_Data.Articolo[j].iPrezzoUnitario;
                            _iTotaleTicket += iIncassoParz;
                            sIncassoParz = IntToEuro(iIncassoParz);

                            // larghezza 28
                            sTmp = String.Format(sRCP_FMT_RCPT, SF_Data.Articolo[j].iQuantitaOrdine, SF_Data.Articolo[j].sTipo, sIncassoParz);
                            PrintCanvas(pg, sTmp);

                            if (!String.IsNullOrEmpty(SF_Data.Articolo[j].sNotaArt))
                            {
                                sTmp = String.Format(sRCP_FMT_NOTE, SF_Data.Articolo[j].sNotaArt);
                                PrintCanvas(pg, sTmp);
                                PrintCanvas(pg, "");
                            }
                        }
                    }

                    if (!CheckLastGroup(_bSomethingInto_GrpToPrint, i))
                        PrintCanvas(pg, "");
                }
            }

            sTmp = String.Format(sRCP_FMT_DSH, "------");
            PrintCanvas(pg, sTmp);

            // punto doppio
            iScontoStdTicket = Arrotonda(iScontoStdTicket);

            if (IsBitSet(SF_Data.iStatusSconto, BIT_SCONTO_STD) && TicketScontatoStdIsGood())
            {
                sTmp = String.Format(sRCP_FMT_DSC, "SCONTO", IntToEuro(iScontoStdTicket), "TOTALE", IntToEuro(_iTotaleTicket));
                PrintCanvas(pg, sTmp);

                _iTotaleDovutoTicket = _iTotaleTicket - iScontoStdTicket;

                if (_iTotaleDovutoTicket < 0)
                    _iTotaleDovutoTicket = 0;

                sTmp = String.Format(sRCP_FMT_DIF + "\r\n", "DIFF. DOVUTA", IntToEuro(_iTotaleDovutoTicket));
                PrintCanvas(pg, sTmp);

                PrintCanvas(pg, "");

            }
            else if (IsBitSet(SF_Data.iStatusSconto, BIT_SCONTO_FISSO))
            {
                _iTotaleDovutoTicket = _iTotaleTicket - SF_Data.iScontoFissoReceipt;

                if (_iTotaleDovutoTicket < 0)
                {
                    _iTotaleDovutoTicket = 0;

                    sTmp = String.Format(sRCP_FMT_DSC, "SCONTO", IntToEuro(_iTotaleTicket),
                                             "TOTALE", IntToEuro(_iTotaleTicket));
                }
                else
                    sTmp = String.Format(sRCP_FMT_DSC, "SCONTO", IntToEuro(SF_Data.iScontoFissoReceipt),
                                             "TOTALE", IntToEuro(_iTotaleTicket));
                PrintCanvas(pg, sTmp);

                sTmp = String.Format(sRCP_FMT_DIF + "\r\n", "DIFF. DOVUTA", IntToEuro(_iTotaleDovutoTicket));
                PrintCanvas(pg, sTmp);

                PrintCanvas(pg, "");
            }
            else if (IsBitSet(SF_Data.iStatusSconto, BIT_SCONTO_GRATIS))
            {
                sTmp = String.Format(sRCP_FMT_TOT, "TOTALE", IntToEuro(_iTotaleTicket));
                PrintCanvas(pg, sTmp);

                _iTotaleDovutoTicket = 0;
                sTmp = String.Format(sRCP_FMT_TOT, "DOVUTO", IntToEuro(_iTotaleDovutoTicket));
                PrintCanvas(pg, sTmp);

                PrintCanvas(pg, "");
            }
            else
            {
                _iTotaleDovutoTicket = _iTotaleTicket;

                sTmp = String.Format(sRCP_FMT_TOT, "TOTALE", IntToEuro(_iTotaleTicket));
                PrintCanvas(pg, sTmp);
                PrintCanvas(pg, "");
            }

            // inserimento indicazione di sconto
            if (IsBitSet(SF_Data.iStatusSconto, BIT_SCONTO_STD) && TicketScontatoStdIsGood())
            {
                sTmp = CenterJustify(sConst_Sconti[0], iMAX_RECEIPT_CHARS);
                PrintCanvas(pg, sTmp);

                sTmp = CenterJustify(SF_Data.sScontoReceipt, iMAX_RECEIPT_CHARS);
                if (!String.IsNullOrEmpty(sTmp))
                    PrintCanvas(pg, sTmp);

                sTmp = CenterJustify(sConst_Sconti[3], iMAX_RECEIPT_CHARS);
                PrintCanvas(pg, sTmp);
                PrintCanvas(pg, "");
            }
            else if (IsBitSet(SF_Data.iStatusSconto, BIT_SCONTO_FISSO))
            {
                sTmp = CenterJustify(sConst_Sconti[1], iMAX_RECEIPT_CHARS);
                PrintCanvas(pg, sTmp);

                sTmp = CenterJustify(SF_Data.sScontoReceipt, iMAX_RECEIPT_CHARS);
                PrintCanvas(pg, sTmp);

                sTmp = CenterJustify(sConst_Sconti[3], iMAX_RECEIPT_CHARS);
                PrintCanvas(pg, sTmp);
                PrintCanvas(pg, "");
            }

            else if (IsBitSet(SF_Data.iStatusSconto, BIT_SCONTO_GRATIS))
            {
                sTmp = CenterJustify(sConst_Sconti[2], iMAX_RECEIPT_CHARS);
                PrintCanvas(pg, sTmp);
                sTmp = CenterJustify(SF_Data.sScontoReceipt, iMAX_RECEIPT_CHARS);
                PrintCanvas(pg, sTmp);
                sTmp = CenterJustify(sConst_Sconti[3], iMAX_RECEIPT_CHARS);
                PrintCanvas(pg, sTmp);
                PrintCanvas(pg, "");
            }

            if (IsBitSet(SF_Data.iStatusReceipt, BIT_PAGAM_CASH))
            {
                // non scrivere nulla
            }
            else if (IsBitSet(SF_Data.iStatusReceipt, BIT_PAGAM_CARD))
            {
                sTmp = CenterJustify(sConst_Pagamento_CARD, iMAX_RECEIPT_CHARS);
                PrintCanvas(pg, sTmp);
                PrintCanvas(pg, "");
            }
            else if (IsBitSet(SF_Data.iStatusReceipt, BIT_PAGAM_SATISPAY))
            {
                sTmp = CenterJustify(sConst_Pagamento_Satispay, iMAX_RECEIPT_CHARS);
                PrintCanvas(pg, sTmp);
                PrintCanvas(pg, "");
            }
            else
            {
                sTmp = CenterJustify(sConst_Pagamento_da_EFFETTUARE, iMAX_RECEIPT_CHARS);
                PrintCanvas(pg, sTmp);
                PrintCanvas(pg, "");
            }

            if (!String.IsNullOrEmpty(SF_Data.sNota))
            {
                sTmp = CenterJustify(sConst_Nota[0], iMAX_RECEIPT_CHARS);
                PrintCanvas(pg, sTmp);

                sTmp = CenterJustify(SF_Data.sNota, iMAX_RECEIPT_CHARS);
                PrintCanvas(pg, sTmp);

                sTmp = CenterJustify(sConst_Nota[1], iMAX_RECEIPT_CHARS);
                PrintCanvas(pg, sTmp);
                PrintCanvas(pg, "");
            }

            if (IsBitSet(SF_Data.iStatusReceipt, BIT_ESPORTAZIONE))
            {
                sTmp = CenterJustify(sConst_Esportazione[0], iMAX_RECEIPT_CHARS);
                PrintCanvas(pg, sTmp);

                sTmp = CenterJustify(sConst_Esportazione[1], iMAX_RECEIPT_CHARS);
                PrintCanvas(pg, sTmp);

                sTmp = CenterJustify(sConst_Esportazione[2], iMAX_RECEIPT_CHARS);
                PrintCanvas(pg, sTmp);
                PrintCanvas(pg, "");
            }

            if (!String.IsNullOrEmpty(SF_Data.sHeaders[2]))
            {
                sTmp = CenterJustify(SF_Data.sHeaders[2], iMAX_RECEIPT_CHARS);
                PrintCanvas(pg, sTmp);
                PrintCanvas(pg, "");
            }

            if (!String.IsNullOrEmpty(SF_Data.sHeaders[3]))
            {
                sTmp = CenterJustify(SF_Data.sHeaders[3], iMAX_RECEIPT_CHARS);
                PrintCanvas(pg, sTmp);
                PrintCanvas(pg, "");
            }

            // quando picBox.Height > panel.Height appaiono scrollbar
            if (panel.Height < (int)_fCanvasVertPos)
                picBox.Height = (int)_fCanvasVertPos;
            else
                picBox.Height = panel.Height + 1; // mantiene presenza scrollbars

            LogToFile("AnteprimaDlg: RedrawReceipt");
            picBox.Refresh();
        }

        /// <summary>aggiornamento del numero dello scontrino con quello reale</summary>
        public void RedrawTicketNum()
        {
            float fCanvasNumHeight;
            String sNum;

            if (dBaseIntf.bUSA_NDB())
                sNum = String.Format("{0}  {1}", _TICK_NUM, SF_Data.iNumOfLastReceipt);
            else
                return;

            sNum = CenterJustify(sNum, iCenterOrderNum);

            // cancellazione TicketNum
            if (pg != null)
            {
                _fCanvasVertPos = _fCanvasVertNumPos;
                fCanvasNumHeight = _printFont.GetHeight(pg) * 1.50f;

                RectangleF imageRect = new RectangleF(0, _fCanvasVertPos, picBox.Width, fCanvasNumHeight);

                pg.FillRectangle(Brushes.White, imageRect);
            }

            _fCanvasVertPos = _fCanvasVertNumPos;

            PrintCanvas(pg, 1.20f, 1.50f, sNum);
        }

        private void AnteprimaDlg_Resize(object sender, EventArgs e)
        {
            RedrawReceipt();
            RedrawReceipt();
        }

        /// <summary>funzione per la scrittura nel Canvas per l'Anteprima</summary>
		void PrintCanvas(Graphics pgParam, float fSize, float fVertSep, String sStrParam)
        {
            if (pgParam == null)
                return;

            float fFontSize = fSize * 1.04f * picBox.Width / 28.0f; // dimensione ricalibrata
            //float fFontSize = sGlbWinPrinterParams.iTckFontSize * (picBox.Width / (12*28.0f));

            _printFont = new Font(sGlbWinPrinterParams.sTckFontType, fFontSize);

            pgParam.DrawString(sStrParam, _printFont, Brushes.Black, _fLeftMargin, _fCanvasVertPos);

            _fCanvasVertPos += _printFont.GetHeight(pgParam) * fVertSep;
            picBox.Image = bmpCanvas;
        }

        /// <summary>funzione per la scrittura nel Canvas per l'Anteprima</summary>
        void PrintCanvas(Graphics pgParam, String sStrParam)
        {
            if (pgParam == null)
                return;

            // dimensione ricalibrata
            float fFontSize = sGlbWinPrinterParams.bChars33 ? 1.06f * picBox.Width / 32.0f : 1.04f * picBox.Width / 28.0f;

            _printFont = new Font(sGlbWinPrinterParams.sTckFontType, fFontSize);

            pgParam.DrawString(sStrParam, _printFont, Brushes.Black, _fLeftMargin, _fCanvasVertPos);

            _fCanvasVertPos += _printFont.GetHeight(pgParam);
            picBox.Image = bmpCanvas;
        }

        private void CheckBoxLogo_CheckedChanged(object sender, EventArgs e)
        {
            RedrawReceipt();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            pg.Clear(Color.White);
            picBox.Height = panel.Height - 2;

            picBox.Refresh();
        }

        /// <summary>
        /// da utilizzare solo per le verifiche di scontrino scontato significativo, <br/>
        /// utilizzato da DataManager per inserire nota nello Scontrino
        /// </summary>
        public bool TicketScontatoStdIsGood()
        {
            int i;
            int iTotaleScontatoCurrTicket = 0;

            double fPerc = (double)((SF_Data.iStatusSconto & 0x00FF0000) >> 16) / 100.0f;

            // totale scontrino corrente
            for (i = 0; i < MAX_NUM_ARTICOLI; i++)
            {
                if ((SF_Data.Articolo[i].iGruppoStampa < NUM_EDIT_GROUPS) && _bScontoGruppo[SF_Data.Articolo[i].iGruppoStampa])
                    iTotaleScontatoCurrTicket += (int)Math.Round(SF_Data.Articolo[i].iQuantitaOrdine * SF_Data.Articolo[i].iPrezzoUnitario * fPerc);
            }

            return (iTotaleScontatoCurrTicket > 0);
        }


        private void AnteprimaDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            LogToFile("AnteprimaDlg: FormClosing");

            Hide();
            // evita la distruzione della form creando una eccezione alla successiva apertura
            e.Cancel = true;
        }

    }
}

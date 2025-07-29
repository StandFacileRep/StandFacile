﻿/********************************************************************
    NomeFile : StandCommonSrc/VisDatiDlg.cs
    Data	 : 29.07.2025
    Autore : Mauro Artuso

    Classe di visualizzazione dei files Dati o Prezzi .
 ********************************************************************/
using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

using static StandFacile.glb;
using static StandCommonFiles.ComDef;
using static StandCommonFiles.CommonCl;
using static StandCommonFiles.LogServer;

using static StandFacile.dBaseIntf;

namespace StandFacile
{
    /// <summary>
    /// classe di visualizzazione dei dati, utilizza quasi sempre DB_Data,
    /// tranne dove è richiesto SF_Data.iNumCassa
    /// </summary>
    public partial class VisDatiDlg : Form
    {
#pragma warning disable IDE0060

        const String NUMERIC_CELL_FORMAT = "0.00";

        /// <summary>offset di inizio articoli del listino</summary>
        const int XLS_VOFFSET = 13;

        // facilita la chiusura della form che altrimenti potrebbe aprirsi più volte
        bool _bVisCambioData, _InitCompletato;
        int _iTipoFile, _iNumCassa;
        int _iLastArticoloIndexP1;

        static int _iReportIndex;

        static String _sNomeTabella;
        static DateTime _SelDate;
        static SelectionRange _SelRange;

        /// <summary>riferimento a VisDatiDlg</summary>
        public static VisDatiDlg rVisDatiDlg;

        /// <summary>ottiene la selezione del tipo di report</summary>
        public static int GetReport()
        {
            return _iReportIndex;
        }

        /// <summary>ottiene il bit corrispondente alla selezione del tipo di report in base allo sconto</summary>
        public static int GetDiscountReportBit()
        {
            switch (_iReportIndex)
            {
                case 1:
                    return BIT_SCONTO_GRATIS;
                case 2:
                    return BIT_SCONTO_FISSO;
                case 3:
                    return BIT_SCONTO_STD;
                default:
                    return 100; // valore oltre i 32bit
            }
        }

        /// <summary>ottiene la selezione del tipo di report in base al pagamento</summary>
        public static int GetPaymentReportBit()
        {
            switch (_iReportIndex)
            {
                case 4:
                    return (int)STATUS_FLAGS.BIT_PAGAM_CASH;
                case 5:
                    return (int)STATUS_FLAGS.BIT_PAGAM_CARD;
                case 6:
                    return (int)STATUS_FLAGS.BIT_PAGAM_SATISPAY;
                default:
                    return 100; // valore oltre i 32bit
            }
        }

        /// <summary>verifica se è richiesto un reporto per sconto</summary>
        public static bool DicountReportIsRequested()
        {
            switch (_iReportIndex)
            {
                case 1:
                case 2:
                case 3:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>verifica se è richiesto un reporto per pagamento</summary>
        public static bool PaymentReportIsRequested()
        {
            switch (_iReportIndex)
            {
                case 4:
                case 5:
                case 6:
                    return true;
                default:
                    return false;
            }
        }

#pragma warning disable IDE0044
        TArticolo[] _ArticoliPrimaColonna = new TArticolo[MAX_NUM_ARTICOLI + EXTRA_NUM_ARTICOLI];
        TErrMsg _WrnMsg;

        /// <summary>costruttore</summary>
        public VisDatiDlg()
        {
            InitializeComponent();

            int i;

            rVisDatiDlg = this;

            _InitCompletato = false;
            _iReportIndex = 0;

            // configura il ComboNumCassa
            Combo_NumCassa.Items.Clear();
            for (i = 0; i < (MAX_CASSE_SECONDARIE + 1); i++)
            {
                Combo_NumCassa.Items.Add(sConstCassaType[i]);
            }

            Combo_NumCassa.SelectedIndex = 0;

            comboReport.Items.Clear();
            comboReport.Items.Add("Normale");
            comboReport.Items.Add("Vis. sconto gratuiti");
            comboReport.Items.Add("Vis. sconto fisso");
            comboReport.Items.Add("Vis. sconto precentuale");
            comboReport.Items.Add("Vis. pagam. contanti");
            comboReport.Items.Add("Vis. pagam. carta");
            comboReport.Items.Add("Vis. pagam. Satispay");

            comboReport.SelectedIndex = 0;

            if (bUSA_NDB())
                CkBoxUnioneCasse.Enabled = true;
            else
            {
                CkBoxUnioneCasse.Visible = false;
                CkBoxUnioneCasse.Enabled = false;
                CkBoxUnioneCasse.Checked = false;

                Combo_NumCassa.Visible = false;
                Combo_NumCassa.Enabled = false;
                LblCassa.Visible = false;
            }

#if STANDFACILE
            if (SF_Data.bPrevendita)
                CkBoxUnioneCasse.Checked = true;
            else
                CkBoxUnioneCasse.Checked = false;
#else
            CkBoxUnioneCasse.Checked = true;
#endif

            _InitCompletato = true;
        }

        /// <summary>overload che consente di specificare un intervallo di date</summary>
        public void VisualizzaDati(int iTipoFileParam, SelectionRange datesParam, int iNumCassaParam, bool bVisCambioDataParam, String sNomeTabellaParam = "", int iReportParam = 0)
        {
            // usato per produrre il file Excel
            _SelRange = datesParam;

            // visualizza il primo giorno dell'intervallo
            VisualizzaDati(iTipoFileParam, datesParam.Start, iNumCassaParam, bVisCambioDataParam, sNomeTabellaParam, iReportParam);
        }

        /// <summary>
        /// Parametro iTipoFile : se == FILE_DATI o FILE_PREZZI si seleziona la tabella con il calendario <br/>
        /// se == FILE_AUTO si determina il tipo in base alla stringa sNomeTabella passata <br/> <br/>
        /// 
        /// bVisCambioDataParam : abilita il bottone di cambio data <br/>
        /// sNomeTabella : nome della tabella
        /// </summary>
        public void VisualizzaDati(int iTipoFileParam, DateTime dateParam, int iNumCassaParam, bool bVisCambioDataParam, String sNomeTabellaParam = "", int iReportParam = 0)
        {
            bool bErrors = false;
            int i, iPos;
            String sTmp, sDisp, sInStr, sNomeFile = "", sDir = "";
            String sLineOfText, sCaption, sFormat;

            _InitCompletato = false;

            List<string> sInputStrings = new List<string>();

            StreamWriter fData = null;
            StreamReader fPrezzi = null;

            // abilitazione di default
            BtnPrt.Enabled = true;
            BtnDate.Enabled = true;
            BtnXls.Enabled = true;
            CheckBoxRidColonne.Enabled = true;
            CheckBoxXls.Enabled = true;
            LblCassa.Enabled = true;

            if (sNomeTabellaParam.Contains(_dbPreDataTablePrefix))
                CkBoxUnioneCasse.Checked = true;

            LogToFile("FrmVisDati : entra in VisDati");

            // ripristina il corretto tipo di file
            if (iTipoFileParam == (int)FILE_TO_SHOW.FILE_AUTO)
            {
                if (sNomeTabellaParam.Contains(_dbDataTablePrefix) || sNomeTabellaParam.Contains(_dbPreDataTablePrefix))
                    _iTipoFile = (int)FILE_TO_SHOW.FILE_DATI;
                else if (sNomeTabellaParam.Contains(dBaseIntf.NOME_LISTINO_DBTBL)) // listino
                    _iTipoFile = (int)FILE_TO_SHOW.FILE_PREZZI;
                else
                    return;
            }
            else
                _iTipoFile = iTipoFileParam;

            _bVisCambioData = bVisCambioDataParam;

            if (!_bVisCambioData)
                BtnDate.Enabled = false;

            _iNumCassa = iNumCassaParam;

            // memorizza data scelta e sNomeFileDati per CheckBoxRidColonneClick()
            _SelDate = dateParam;

            _sNomeTabella = sNomeTabellaParam;

            /***********************************************
             ***               FILE_DATI                 ***
             ***********************************************/
            if (_iTipoFile == (int)FILE_TO_SHOW.FILE_DATI)
            {
                this.Width = 640;

                //	visualizzazione
                if (bUSA_NDB() && String.IsNullOrEmpty(_sNomeTabella))
                {
                    CkBoxUnioneCasse.Enabled = true;

                    if (CkBoxUnioneCasse.Checked)
                    {
                        LblCassa.Enabled = false;
                        Combo_NumCassa.Enabled = false;
                    }
                    else
                    {
                        LblCassa.Enabled = true;
                        Combo_NumCassa.Enabled = true;

                        Combo_NumCassa.SelectedIndex = _iNumCassa - 1;
                    }
                }
                else
                {
                    CkBoxUnioneCasse.Enabled = false;
                    CkBoxUnioneCasse.Checked = false;

                    Combo_NumCassa.Enabled = false;
                }

                if (String.IsNullOrEmpty(_sNomeTabella))
                {
                    sNomeFile = GetNomeDatiDBTable(_iNumCassa, _SelDate);
                }
                else
                {
                    if (_sNomeTabella.Contains(_dbPreDataTablePrefix))
                        iPos = _dbPreDataTablePrefix.Length + 2;
                    else
                        iPos = _dbDataTablePrefix.Length + 2;

                    sNomeFile = _sNomeTabella;
                    _iNumCassa = _sNomeTabella[iPos] - '0';
                }

                /***************** inizio punto di caricamento dati DB ****************/

                if (CkBoxUnioneCasse.Checked)
                    // iNumCassaParam == 0 legge senza filtro per cassa
                    _rdBaseIntf.dbCaricaDatidaOrdini(_SelDate, 0, false, _sNomeTabella, iReportParam);
                else
                    _rdBaseIntf.dbCaricaDatidaOrdini(_SelDate, _iNumCassa, false, _sNomeTabella, iReportParam);

                InitFormatStrings(dbGetLengthArticoli());

                /*************** fine punto di caricamento dati DB ******************/

                //textEditDati.ScrollBars = ScrollBars.None;
                textEditDati.Clear();

                if (DB_Data.iActualNumOfReceipts > 0)
                {
                    sTmp = String.Format(" {0}\r\n", DB_Data.sVersione);
                    textEditDati.AppendText(sTmp);

                    if (CkBoxUnioneCasse.Checked)
                        sCaption = " Dati di unione casse\r\n";
                    else
                        sCaption = String.Format(" Dati della {0}\r\n", sConstCassaType[_iNumCassa - 1]);

                    Text = sCaption;

                    textEditDati.AppendText(sCaption);

                    sTmp = String.Format(" {0}\r\n", DB_Data.sDateTime);
                    textEditDati.AppendText(sTmp);

                    textEditDati.AppendText("\r\n");

                    if (!String.IsNullOrEmpty(DB_Data.sHeaders[0]))
                    {
                        sTmp = String.Format(" {0}\r\n", DB_Data.sHeaders[0]);
                        textEditDati.AppendText(sTmp);
                    }

                    if (!String.IsNullOrEmpty(DB_Data.sHeaders[1]))
                    {
                        sTmp = String.Format(" {0}\r\n", DB_Data.sHeaders[1]);
                        textEditDati.AppendText(sTmp);
                    }

                    // dati correnti o da VisDati che filtra in base a bPrevendita || dati da Esplora_DB
                    if (SF_Data.bPrevendita && String.IsNullOrEmpty(_sNomeTabella) || _sNomeTabella.Contains(_dbPreDataTablePrefix))
                    {
                        sTmp = CenterJustify(sConst_Prevendita[0], iMAX_RECEIPT_CHARS);
                        textEditDati.AppendText(String.Format("\r\n{0}\r\n", sTmp));
                        sTmp = CenterJustify(sConst_Prevendita[1], iMAX_RECEIPT_CHARS);
                        textEditDati.AppendText(String.Format("{0}\r\n", sTmp));
                        sTmp = CenterJustify(sConst_Prevendita[2], iMAX_RECEIPT_CHARS);
                        textEditDati.AppendText(String.Format("{0}\r\n", sTmp));
                    }

                    textEditDati.AppendText("\r\n");

                    if (CheckBoxRidColonne.Checked)
                        sFormat = sGlbWinPrinterParams.bChars33 ? "{0,31}" : "{0,26}";
                    else
                        sFormat = sGlbWinPrinterParams.bChars33 ? "{0,33}" : "{0,28}";

                    sTmp = String.Format(sFormat + "{1,4}\r\n", "Numero Scontrini emessi = ", DB_Data.iActualNumOfReceipts);
                    textEditDati.AppendText(sTmp);

                    sTmp = String.Format(sFormat + "{1,4}\r\n", "Numero Scontrini Web = ", DB_Data.iNumOfWebReceipts);
                    textEditDati.AppendText(sTmp);

                    sTmp = String.Format(sFormat + "{1,4}\r\n", "Numero    \"   annullati = ", DB_Data.iNumAnnullati);
                    textEditDati.AppendText(sTmp);

                    if (DB_Data.iNumAnnullati > 0)
                    {
                        sTmp = String.Format(sFormat + "{1,7}\r\n", "valore = ", IntToEuro(DB_Data.iTotaleAnnullato));
                        textEditDati.AppendText(sTmp);
                    }

                    textEditDati.AppendText("\r\n");

                    /*******************************************************************
                     *	1/4 visualizzazione normale del file dati con incasso singolo
                     *******************************************************************/
                    if (!CheckBoxRidColonne.Checked && !CkBoxUnioneCasse.Checked && (_iTipoFile == (int)FILE_TO_SHOW.FILE_DATI))
                    {
                        sNomeFile = NOME_FILE_STAMPA_LOC_TMP;

                        textEditDati.AppendText("    Articolo       quant.venduta       dispon.\r\n");
                        textEditDati.AppendText("             prz. unitario       parziale\r\n");

                        for (i = 0; i < MAX_NUM_ARTICOLI + EXTRA_NUM_ARTICOLI; i++)
                        {
                            // separa voci aggiuntive
                            if ((i == MAX_NUM_ARTICOLI) && ((DB_Data.Articolo[i].iPrezzoUnitario > 0)
#if STANDFACILE
                                || (!String.IsNullOrEmpty(DB_Data.Articolo[i].sTipo) && OptionsDlg._rOptionsDlg.GetZeroPriceEnabled())
#endif
                                ))
                                textEditDati.AppendText("\r\n");

                            if (DB_Data.Articolo[i].iDisponibilita == DISP_OK)
                                sDisp = "OK";
                            else
                                sDisp = DB_Data.Articolo[i].iDisponibilita.ToString();

                            if (((DB_Data.Articolo[i].iPrezzoUnitario > 0) ||
#if STANDFACILE
                                (!String.IsNullOrEmpty(DB_Data.Articolo[i].sTipo) && OptionsDlg._rOptionsDlg.GetZeroPriceEnabled()) ||
#endif
                                (DB_Data.Articolo[i].iGruppoStampa == (int)DEST_TYPE.DEST_COUNTER)) &&

                                (!CkBoxSkipZero.Checked || (DB_Data.Articolo[i].iQuantitaVenduta > 0)))
                            {
                                sInStr = String.Format(sDAT_FMT_DAT + "\r\n", DB_Data.Articolo[i].sTipo,
                                    IntToEuro(DB_Data.Articolo[i].iPrezzoUnitario),
                                    DB_Data.Articolo[i].iQuantitaVenduta,
                                    IntToEuro(DB_Data.Articolo[i].iPrezzoUnitario * DB_Data.Articolo[i].iQuantitaVenduta), sDisp);

                                textEditDati.AppendText(sInStr);
                            }
                        }

                        if (GetReport() == 0)
                        {
                            sTmp = String.Format(sDAT_FMT_DSH + "\r\n", "--------");
                            textEditDati.AppendText(sTmp);

                            sTmp = String.Format(sDAT_FMT_TOT + "\r\n", "TOTALE", IntToEuro(DB_Data.iTotaleIncasso));
                            textEditDati.AppendText(sTmp);
                        }
                    }

                    /******************************************************************
                     *	2/4 visualizzazione normale del file dati con unione incassi
                     ******************************************************************/
                    else if (!CheckBoxRidColonne.Checked && CkBoxUnioneCasse.Checked && (_iTipoFile == (int)FILE_TO_SHOW.FILE_DATI))
                    {

                        sNomeFile = NOME_FILE_STAMPA_LOC_TMP;

                        textEditDati.AppendText("    Articolo       quant.venduta       dispon.\r\n");
                        textEditDati.AppendText("             prz. unitario       parziale\r\n");

                        for (i = 0; i < MAX_NUM_ARTICOLI + EXTRA_NUM_ARTICOLI; i++)
                        {
                            // separa voci aggiuntive
                            if ((i == MAX_NUM_ARTICOLI) && ((DB_Data.Articolo[i].iPrezzoUnitario > 0)
#if STANDFACILE
                                || (!String.IsNullOrEmpty(DB_Data.Articolo[i].sTipo) && OptionsDlg._rOptionsDlg.GetZeroPriceEnabled())
#endif
                                ))
                                textEditDati.AppendText("\r\n");

                            if (DB_Data.Articolo[i].iDisponibilita == DISP_OK)
                                sDisp = "OK";
                            else
                                sDisp = DB_Data.Articolo[i].iDisponibilita.ToString();

                            if (((DB_Data.Articolo[i].iPrezzoUnitario > 0) ||
#if STANDFACILE
                                (!String.IsNullOrEmpty(DB_Data.Articolo[i].sTipo) && OptionsDlg._rOptionsDlg.GetZeroPriceEnabled()) ||
#endif
                                (DB_Data.Articolo[i].iGruppoStampa == (int)DEST_TYPE.DEST_COUNTER)) &&

                                (!CkBoxSkipZero.Checked || (DB_Data.Articolo[i].iQuantitaVenduta > 0)))
                            {
                                sInStr = String.Format(sDAT_FMT_DAT + "\r\n", DB_Data.Articolo[i].sTipo,
                                    IntToEuro(DB_Data.Articolo[i].iPrezzoUnitario),
                                    DB_Data.Articolo[i].iQuantitaVenduta,
                                    IntToEuro(DB_Data.Articolo[i].iPrezzoUnitario * DB_Data.Articolo[i].iQuantitaVenduta), sDisp);

                                textEditDati.AppendText(sInStr);
                            }
                        }

                        if (GetReport() == 0)
                        {
                            sTmp = String.Format(sDAT_FMT_DSH + "\r\n", "--------");
                            textEditDati.AppendText(sTmp);

                            sTmp = String.Format(sDAT_FMT_TOT + "\r\n", "TOTALE", IntToEuro(DB_Data.iTotaleIncasso));
                            textEditDati.AppendText(sTmp);
                        }
                    }

                    /********************************************************************
                     *	3/4 visualizzazione ridotta del file dati con incasso singolo
                     ********************************************************************/
                    else if (CheckBoxRidColonne.Checked && !CkBoxUnioneCasse.Checked && (_iTipoFile == (int)FILE_TO_SHOW.FILE_DATI))
                    {
                        sNomeFile = NOME_FILE_STAMPA_LOC_RID_TMP;

                        textEditDati.AppendText("Articolo   quant.venduta parziale\r\n\r\n");

                        for (i = 0; i < MAX_NUM_ARTICOLI + EXTRA_NUM_ARTICOLI; i++)
                        {
                            // separa voci aggiuntive
                            if ((i == MAX_NUM_ARTICOLI) && ((DB_Data.Articolo[i].iPrezzoUnitario > 0)
#if STANDFACILE
                                || (!String.IsNullOrEmpty(DB_Data.Articolo[i].sTipo) && OptionsDlg._rOptionsDlg.GetZeroPriceEnabled())
#endif
                                ))
                                textEditDati.AppendText("\r\n");

                            if (DB_Data.Articolo[i].iDisponibilita == DISP_OK)
                                sDisp = "OK";
                            else
                                sDisp = DB_Data.Articolo[i].iDisponibilita.ToString();

                            if (((DB_Data.Articolo[i].iPrezzoUnitario > 0) ||
#if STANDFACILE
                                (!String.IsNullOrEmpty(DB_Data.Articolo[i].sTipo) && OptionsDlg._rOptionsDlg.GetZeroPriceEnabled()) ||
#endif
                                (DB_Data.Articolo[i].iGruppoStampa == (int)DEST_TYPE.DEST_COUNTER)) &&

                                (!CkBoxSkipZero.Checked || (DB_Data.Articolo[i].iQuantitaVenduta > 0)))
                            {
                                sInStr = String.Format(sDAT_FMT_REP_RED + "\r\n", DB_Data.Articolo[i].sTipo,
                                    DB_Data.Articolo[i].iQuantitaVenduta,
                                    IntToEuro(DB_Data.Articolo[i].iPrezzoUnitario * DB_Data.Articolo[i].iQuantitaVenduta));

                                textEditDati.AppendText(sInStr);
                            }
                        }

                        if (GetReport() == 0)
                        {
                            sTmp = String.Format(sDAT_FMT_DSH_RED + "\r\n", "--------");
                            textEditDati.AppendText(sTmp);

                            sTmp = String.Format(sDAT_FMT_TOT_RED + "\r\n", "TOTALE", IntToEuro(DB_Data.iTotaleIncasso));
                            textEditDati.AppendText(sTmp);
                        }
                    }

                    /*******************************************************************
                     *	4/4 visualizzazione ridotta del file dati con unione incassi
                     *******************************************************************/
                    else if (CheckBoxRidColonne.Checked && CkBoxUnioneCasse.Checked && (_iTipoFile == (int)FILE_TO_SHOW.FILE_DATI))
                    {
                        sNomeFile = NOME_FILE_STAMPA_LOC_RID_TMP;

                        textEditDati.AppendText("Articolo   quant.venduta parziale\r\n\r\n");

                        for (i = 0; i < MAX_NUM_ARTICOLI + EXTRA_NUM_ARTICOLI; i++)
                        {
                            // separa voci aggiuntive
                            if ((i == MAX_NUM_ARTICOLI) && ((DB_Data.Articolo[i].iPrezzoUnitario > 0)
#if STANDFACILE
                                || (!String.IsNullOrEmpty(DB_Data.Articolo[i].sTipo) && OptionsDlg._rOptionsDlg.GetZeroPriceEnabled())
#endif
                                ))
                                textEditDati.AppendText("\r\n");

                            if (DB_Data.Articolo[i].iDisponibilita == DISP_OK)
                                sDisp = "OK";
                            else
                                sDisp = DB_Data.Articolo[i].iDisponibilita.ToString();

                            if (((DB_Data.Articolo[i].iPrezzoUnitario > 0) ||
#if STANDFACILE
                                (!String.IsNullOrEmpty(DB_Data.Articolo[i].sTipo) && OptionsDlg._rOptionsDlg.GetZeroPriceEnabled()) ||
#endif
                               (DB_Data.Articolo[i].iGruppoStampa == (int)DEST_TYPE.DEST_COUNTER)) &&

                                (!CkBoxSkipZero.Checked || (DB_Data.Articolo[i].iQuantitaVenduta > 0)))
                            {
                                sInStr = String.Format(sDAT_FMT_REP_RED + "\r\n", DB_Data.Articolo[i].sTipo,
                                    DB_Data.Articolo[i].iQuantitaVenduta,
                                    IntToEuro(DB_Data.Articolo[i].iPrezzoUnitario * DB_Data.Articolo[i].iQuantitaVenduta));

                                textEditDati.AppendText(sInStr);
                            }
                        }

                        if (GetReport() == 0)
                        {
                            sTmp = String.Format(sDAT_FMT_DSH_RED + "\r\n", "--------");
                            textEditDati.AppendText(sTmp);

                            sTmp = String.Format(sDAT_FMT_TOT_RED + "\r\n", "TOTALE", IntToEuro(DB_Data.iTotaleIncasso));
                            textEditDati.AppendText(sTmp);
                        }
                    }

                    if (CheckBoxRidColonne.Checked)
                        sFormat = sGlbWinPrinterParams.bChars33 ? "{0,28}" : "{0,23}";
                    else
                        sFormat = sGlbWinPrinterParams.bChars33 ? "{0,36}" : "{0,31}";

                    textEditDati.AppendText("\r\n");

                    LogToFile(String.Format("VisDatiDlg : iTotaleScontatoGratis = {0}", DB_Data.iTotaleScontatoGratis), true);
                    LogToFile(String.Format("VisDatiDlg : iTotaleScontatoFisso = {0}", DB_Data.iTotaleScontatoFisso), true);
                    LogToFile(String.Format("VisDatiDlg : iTotaleScontatoStd = {0}", DB_Data.iTotaleScontatoStd), true);

                    // ***************************************** SCONTI **********************************************

                    // con PaymentReportIsRequested() seve vedere gli sconti per capire meglio
                    if ((GetReport() == 0) || (GetDiscountReportBit() == BIT_SCONTO_GRATIS) || PaymentReportIsRequested())
                    {
                        sTmp = String.Format(sFormat + "{1,10}\r\n", "valore gratuiti", IntToEuro(DB_Data.iTotaleScontatoGratis));
                        textEditDati.AppendText(sTmp);
                    }

                    if ((GetReport() == 0) || (GetDiscountReportBit() == BIT_SCONTO_FISSO) || PaymentReportIsRequested())
                    {
                        sTmp = String.Format(sFormat + "{1,10}\r\n", "valore sconto fisso", IntToEuro(DB_Data.iTotaleScontatoFisso));
                        textEditDati.AppendText(sTmp);
                    }

                    if ((GetReport() == 0) || (GetDiscountReportBit() == BIT_SCONTO_STD) || PaymentReportIsRequested())
                    {
                        sTmp = String.Format(sFormat + "{1,10}\r\n", "valore sconto articoli", IntToEuro(DB_Data.iTotaleScontatoStd));
                        textEditDati.AppendText(sTmp);
                    }

                    if (((DB_Data.iTotaleScontatoStd > 0) || (DB_Data.iTotaleScontatoFisso > 0) || (DB_Data.iTotaleScontatoGratis > 0)) && GetReport() == 0)
                    {
                        sTmp = String.Format(sFormat + "{1,10}\r\n", "", "--------");
                        textEditDati.AppendText(sTmp);

                        sTmp = String.Format(sFormat + "{1,10}\r\n", "TOTALE NETTO", IntToEuro(DB_Data.iTotaleIncasso - DB_Data.iTotaleScontatoStd -
                            DB_Data.iTotaleScontatoFisso - DB_Data.iTotaleScontatoGratis));
                        textEditDati.AppendText(sTmp);
                    }

                    textEditDati.AppendText("\r\n");

                    // ************************************* PAGAMENTI ***********************************************

                    if ((GetReport() == 0) || (GetPaymentReportBit() == (int)STATUS_FLAGS.BIT_PAGAM_CARD))
                    {
                        sTmp = String.Format(sFormat + "{1,10}\r\n", "PAGAM. CARD    ", IntToEuro(DB_Data.iTotaleIncassoCard));
                        textEditDati.AppendText(sTmp);
                    }

                    if ((GetReport() == 0) || (GetPaymentReportBit() == (int)STATUS_FLAGS.BIT_PAGAM_SATISPAY))
                    {
                        sTmp = String.Format(sFormat + "{1,10}\r\n", "PAGAM. SATISPAY", IntToEuro(DB_Data.iTotaleIncassoSatispay));
                        textEditDati.AppendText(sTmp);
                    }

                    if ((GetReport() == 0) || (GetPaymentReportBit() == (int)STATUS_FLAGS.BIT_PAGAM_CASH))
                    {
                        sTmp = String.Format(sFormat + "{1,10}\r\n", "PAGAM. CONT.   ", IntToEuro(DB_Data.iTotaleIncasso - DB_Data.iTotaleScontatoStd -
                                                       DB_Data.iTotaleScontatoFisso - DB_Data.iTotaleScontatoGratis - DB_Data.iTotaleIncassoCard - DB_Data.iTotaleIncassoSatispay));
                        textEditDati.AppendText(sTmp);
                    }
                }
                else
                {
                    bErrors = true;

                    _WrnMsg.sMsg = sNomeFile;
                    _WrnMsg.iErrID = WRN_TNP;
                    WarningManager(_WrnMsg);
                }

                //textEditDati.ScrollBars = ScrollBars.Vertical;
                //textEditDati.Text = textEditDati.Text.Replace("\n", "\r\n");

                /*********************************************************
                 *  costruzione del file dati temporaneo per stampa
                 *  e per visualizzazione ridotta
                 *********************************************************/
#if STANDFACILE
                sDir = DataManager.GetExeDir() + "\\";
#endif

                fData = File.CreateText(sDir + sNomeFile);
                if (fData == null)
                {
                    BtnXls.Enabled = false;
                    BtnPrt.Enabled = false;

                    _WrnMsg.sNomeFile = sNomeFile;
                    _WrnMsg.iErrID = WRN_FNO;
                    WarningManager(_WrnMsg);
                }
                else
                {
                    for (i = 0; i < textEditDati.Lines.Length; i++)
                    {
                        sLineOfText = textEditDati.Lines[i];
                        fData.WriteLine(sLineOfText);
                    }
                }

                // fprintf(fData, "\n\n");
                fData.Close();
            }

            /****************************************************
             *  				FILE_PREZZI
             ****************************************************/
            else if (_iTipoFile == (int)FILE_TO_SHOW.FILE_PREZZI)
            {
                // visualizzazione
                CheckBoxRidColonne.Enabled = false;
                CheckBoxXls.Enabled = false;
                BtnXls.Enabled = false;
                CkBoxUnioneCasse.Enabled = false;
                LblCassa.Enabled = false;
                Combo_NumCassa.Enabled = false;
                CkBoxSkipZero.Enabled = false;
                LblReport.Enabled = false;
                comboReport.Enabled = false;

                // visualizzazione
                CheckBoxRidColonne.Visible = false;
                CheckBoxXls.Visible = false;
                BtnXls.Visible = false;
                CkBoxUnioneCasse.Visible = false;
                LblCassa.Visible = false;
                Combo_NumCassa.Visible = false;
                CkBoxSkipZero.Visible = false;
                LblReport.Visible = false;
                comboReport.Visible = false;
                BtnDate.Visible = false;

                // setup dimensioni
                this.Width = 700;

                textEditDati.Height = 416;
                BtnPrt.Left -= 40;    


#if STANDFACILE
                sNomeFile = NOME_FILE_LISTINO;
                sDir = DataManager.GetExeDir() + "\\";
#else
                sNomeFile = "";
                sTmp = "";
#endif

                Text = "Listino Prezzi";

                sInputStrings.Clear();
                textEditDati.Clear();

                // ******* inizio caricamento stringhe dal DB o file *******

#if STANDFACILE
                if (DataManager.CheckIf_CassaSec_and_NDB()) // cassa secondaria e DB
                {
                    if (_rdBaseIntf.dbCaricaListino(sInputStrings) > 0) // carica dal DB
                        LogToFile("FrmVisDati : dbCaricaListino()");
                    else
                        LogToFile("FrmVisDati : errore dbCaricaListino()");
                }
                else
#endif
                {
                    LogToFile("FrmVisDati : Carica Listino da File");

#if STANDFACILE
#endif

                    if (File.Exists(sDir + sNomeFile))
                        fPrezzi = File.OpenText(sDir + sNomeFile);

                    if (fPrezzi == null)
                    {
                        bErrors = true;

                        _WrnMsg.sNomeFile = sNomeFile;
                        _WrnMsg.iErrID = WRN_FNO;
                        WarningManager(_WrnMsg);
                    }
                    else
                    {
                        // ***** caricamento stringhe dal file Prezzi *****
                        while (((sInStr = fPrezzi.ReadLine()) != null) && (sInputStrings.Count < 1000))
                        {
                            sInputStrings.Add(sInStr);
                        }

                        fPrezzi.Close();
                    }
                }

                // ******* fine caricamento stringhe Listino dal DB o file *******

                // ***** scrittura di textEditDati *****
                for (i = 0; i < sInputStrings.Count;)
                {
                    sInStr = sInputStrings[i];
                    sInStr = sInStr.Trim();
                    i++;

                    // riduzione delle righe non necessarie
                    if (String.IsNullOrEmpty(sInStr))
                        continue;

                    if (sInStr.Contains("#LF"))
                        continue;

                    textEditDati.AppendText(sInStr + "\r\n");

                } // while

                /*****************************************************************
                 *  costruzione del file prezzi temporaneo e ridotto per stampa
                 *****************************************************************/
#if STANDFACILE
                sDir = DataManager.GetExeDir() + "\\";
#endif
                sNomeFile = NOME_FILE_STAMPA_LOC_RID_TMP;

                fData = File.CreateText(sDir + sNomeFile);
                if (fData == null)
                {
                    _WrnMsg.sNomeFile = sNomeFile;
                    _WrnMsg.iErrID = WRN_FNO;
                    WarningManager(_WrnMsg);
                }
                else
                {
                    for (i = 0; i < textEditDati.Lines.Length; i++)
                    {
                        sLineOfText = textEditDati.Lines[i];
                        fData.WriteLine(sLineOfText);
                    }

                    fData.WriteLine("\n");
                    fData.Close();
                }
            }
            else
            {
                _WrnMsg.sNomeFile = sNomeFile;
                _WrnMsg.iErrID = WRN_FNF;
                WarningManager(_WrnMsg);

                return;
            }

            _InitCompletato = true;

            if (!this.Visible && !bErrors)
                ShowDialog();

        } // end VisualizzaDati

        private void BtnPrt_Click(object sender, EventArgs e)
        {
#if STANDFACILE
            String sDir = DataManager.GetExeDir() + "\\";

            if ((_iTipoFile == (int)FILE_TO_SHOW.FILE_DATI) && (CheckBoxRidColonne.Checked) || (_iTipoFile == (int)FILE_TO_SHOW.FILE_PREZZI))
                GenPrintFile(sDir + NOME_FILE_STAMPA_LOC_RID_TMP);
            else
                GenPrintFile(sDir + NOME_FILE_STAMPA_LOC_TMP);
#else
            if ((_iTipoFile == (int)FILE_TO_SHOW.FILE_DATI) && (CheckBoxRidColonne.Checked) || (_iTipoFile == (int)FILE_TO_SHOW.FILE_PREZZI))
                GenPrintFile(sRootDir + "\\" + NOME_FILE_STAMPA_LOC_RID_TMP);
            else
                GenPrintFile(sRootDir + "\\" + NOME_FILE_STAMPA_LOC_TMP);
#endif
        }

        private void OKBtn_KeyPress(object sender, KeyPressEventArgs e)
        {
            int iKey = (int)e.KeyChar;

            if (iKey == KEY_ESC)
                DialogResult = DialogResult.OK;

            LogToFile("FrmVisDati : esce da VisDati");
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            LogToFile("FrmVisDati : esce da VisDati");
        }

        private void CheckBoxRidColonne_Click(object sender, EventArgs e)
        {
            VisualizzaDati(_iTipoFile, _SelDate, _iNumCassa, _bVisCambioData, _sNomeTabella, comboReport.SelectedIndex);
        }

        /// <summary>
        /// Esportazione dei dati di riepilogo giornaliero
        /// </summary>
        private void BtnXls_Click(object sender, EventArgs e)
        {
            bool bMatch;
            int i, j, iRow, iLastItemRow, iColumn, iUpperLimit;
            int iDebugPrezzo, iDebugQty;
            String sNomeFile, sDataDir, sTmp, sDisp;
            String sDebugTipo;
            DateTime columnDay;

            BtnXls.Enabled = false;

            Excel.Application xlApp = null;
            Excel.Workbook xlsWorkBook = null;
            Excel.Worksheet xlsWorkSheet = null;
            Excel.Range xlsRange, xlsRangeCell;

            object misValue = System.Reflection.Missing.Value;

            for (i = 0; i < MAX_NUM_ARTICOLI + EXTRA_NUM_ARTICOLI; i++)
            {
                _ArticoliPrimaColonna[i].sTipo = "";
                _ArticoliPrimaColonna[i].iPrezzoUnitario = 0;
                _ArticoliPrimaColonna[i].iQuantita_Scaricata = 0;
                _ArticoliPrimaColonna[i].iQuantitaOrdine = 0;
                _ArticoliPrimaColonna[i].iIndexListino = 0;
                _ArticoliPrimaColonna[i].iGruppoStampa = 0;
                _ArticoliPrimaColonna[i].iQuantitaVenduta = 0;
                _ArticoliPrimaColonna[i].iQtaEsportata = 0;
                _ArticoliPrimaColonna[i].iDisponibilita = DISP_OK;
            }

            LogToFile("FrmVisDati : Xls prima di Excel.Application()");

            try
            {
                xlApp = new Excel.Application();
                xlsWorkBook = xlApp.Workbooks.Add(misValue);
                xlsWorkSheet = (Excel.Worksheet)xlsWorkBook.Worksheets.get_Item(1); // xls tab1

#if STANDFACILE
                sDataDir = DataManager.GetDataDir() + "\\";
#else
                sDataDir = sRootDir + "\\";
#endif

                if (CkBoxUnioneCasse.Checked)
                {
                    sNomeFile = "Dati_";
                    xlsWorkSheet.Name = "Dati_";
                }
                else
                {
                    sNomeFile = String.Format("Dati_C{0}_", _iNumCassa);
                    xlsWorkSheet.Name = String.Format("Dati_C{0}", _iNumCassa);
                }

                if (_SelRange == null)
                    _SelRange = new SelectionRange(GetActualDate(), GetActualDate());

                // se _sNomeTabella == "" selezionato un range di date, altrimenti si arriva da EsploraDB_Dlg
                if (String.IsNullOrEmpty(_sNomeTabella))
                {
                    if (_SelRange.Start == _SelRange.End)
                    {
                        _SelRange = new SelectionRange(_SelDate, _SelDate);
                        sNomeFile += _SelDate.ToString("yyMMdd'.xlsx'");
                    }
                    else
                        sNomeFile += (_SelRange.Start.ToString("yyMMdd'.xlsx'") + _SelRange.End.ToString("_yyMMdd'.xlsx'"));
                }
                else
                    sNomeFile = _sNomeTabella + ".xlsx";

                if (File.Exists(sDataDir + sNomeFile))
                    File.Delete(sDataDir + sNomeFile);

                _iLastArticoloIndexP1 = MAX_NUM_ARTICOLI; // successivamente potrebbe incrementare

                iLastItemRow = 0;

                for (int iRepeatForLayout = 0; iRepeatForLayout < 2; iRepeatForLayout++)
                {
                    iColumn = 0;
                    iUpperLimit = 0;
                    columnDay = _SelRange.Start;

                    while (columnDay <= _SelRange.End)
                    {
                        /***************** punto di caricamento dati DB ****************/

                        //  _sNomeTabella è usato solo da EsploraDB_Dlg
                        if (String.IsNullOrEmpty(_sNomeTabella))
                        {
                            if (CkBoxUnioneCasse.Checked)
                                // iNumCassaParam == 0 legge senza filtro per cassa
                                _rdBaseIntf.dbCaricaDatidaOrdini(columnDay, 0);
                            else
                                _rdBaseIntf.dbCaricaDatidaOrdini(columnDay, _iNumCassa);
                        }

                        InitFormatStrings(dbGetLengthArticoli());

                        if (DB_Data.iActualNumOfReceipts <= 0)
                        {
                            columnDay = columnDay.AddDays(+1);
                            continue;
                        }

                        // Headers
                        if (columnDay == _SelRange.Start)
                        {
                            iRow = 2;
                            j = 0;

                            // compattazione
                            for (i = 0; (i < MAX_NUM_ARTICOLI); i++) // COPERTI inclusi
                                if (!(String.IsNullOrEmpty(DB_Data.Articolo[i].sTipo)))
                                {
                                    _ArticoliPrimaColonna[j] = DB_Data.Articolo[i];
                                    j++;
                                }

                            // calcolo ultimo indice anche in date passate, no DataManager.CheckLastArticoloIndexP1()
                            iUpperLimit = CheckLastArticoloIndexP1();

                            sTmp = String.Format("{0}", DB_Data.sVersione);
                            xlsWorkSheet.Cells[iRow++, iColumn + 2] = sTmp;

                            xlsWorkSheet.Cells[iRow++, iColumn + 2] = Text.Trim(); // Caption

                            iRow++;

                            xlsWorkSheet.Cells[iRow++, iColumn + 2] = DB_Data.sHeaders[0];
                            xlsWorkSheet.Cells[iRow++, iColumn + 2] = DB_Data.sHeaders[1];

                            xlsWorkSheet.Cells[iRow++, iColumn + 2] = "Num. Scontrini emessi";

                            xlsWorkSheet.Cells[iRow++, iColumn + 2] = "Num. Scontrini Web";

                            //if (DB_Data.iNumAnnullati > 0)
                            xlsWorkSheet.Cells[iRow++, iColumn + 2] = "Num. Scontr. annullati e valore";
                            //else
                            //    xlsWorkSheet.Cells[iRow++, iColumn + 2] = "Num. Scontr. annullati";

                            // recupera l'ora dalla CASSA_PRINCIPALE
                            xlsWorkSheet.Cells[iRow++, iColumn + 2] = DB_Data.sDateTime;
                            xlsWorkSheet.Cells[iRow, iColumn + 2] = "articolo";
                            xlsWorkSheet.Cells[iRow + 1, iColumn + 3] = "prz. unitario";

                            // imposta Larghezza colonne
                            xlsRange = xlsWorkSheet.get_Range("B:B", Type.Missing);
                            xlsRange.EntireColumn.ColumnWidth = 30;

                            xlsRange = xlsWorkSheet.get_Range("C12:C500", Type.Missing);
                            xlsRange.EntireColumn.ColumnWidth = 12;
                            xlsRange.NumberFormat = NUMERIC_CELL_FORMAT;

                        } // end if
                        else
                            iRow = XLS_VOFFSET - 2; // rimpicciolisce se si aggiungono righe di informazioni

                        xlsWorkSheet.Cells[iRow - 4, iColumn + 4] = DB_Data.iActualNumOfReceipts;

                        xlsWorkSheet.Cells[iRow - 3, iColumn + 4] = DB_Data.iNumOfWebReceipts;

                        if (DB_Data.iNumAnnullati > 0)
                        {
                            xlsWorkSheet.Cells[iRow - 2, iColumn + 4] = DB_Data.iNumAnnullati;
                            xlsWorkSheet.Cells[iRow - 2, iColumn + 5] = (DB_Data.iTotaleAnnullato) / 100.0f;

                            xlsRangeCell = xlsWorkSheet.Cells[iRow - 2, iColumn + 5];
                            xlsRangeCell.NumberFormat = NUMERIC_CELL_FORMAT;
                        }
                        else
                        {
                            xlsWorkSheet.Cells[iRow - 2, iColumn + 4] = DB_Data.iNumAnnullati;
                        }

                        xlsWorkSheet.Cells[iRow - 1, iColumn + 4] = DB_Data.sDateTime.Trim().Substring(4, 8);

                        xlsWorkSheet.Cells[iRow, iColumn + 4] = "quant. venduta";

                        if (_SelRange.Start == _SelRange.End)
                            xlsWorkSheet.Cells[iRow, iColumn + 6] = "dispon.";

                        iRow++;

                        xlsWorkSheet.Cells[iRow, iColumn + 5] = "parziale";

                        if (!CheckBoxRidColonne.Checked && (_SelRange.Start == _SelRange.End))
                            xlsWorkSheet.Cells[iRow, iColumn + 7] = "quant.esportazione";

                        // loop principale
                        for (i = 0; i < MAX_NUM_ARTICOLI + EXTRA_NUM_ARTICOLI; i++)
                        {
                            sDebugTipo = DB_Data.Articolo[i].sTipo;
                            iDebugPrezzo = DB_Data.Articolo[i].iPrezzoUnitario;
                            iDebugQty = DB_Data.Articolo[i].iQuantitaVenduta;

                            if (String.IsNullOrEmpty(DB_Data.Articolo[i].sTipo))
                                continue;

                            /************************************
                             *     controllo di sicurezza
                             ************************************/
                            bMatch = false;

                            for (j = 0; j < _iLastArticoloIndexP1; j++)
                            {
                                if (_ArticoliPrimaColonna[j].sTipo == DB_Data.Articolo[i].sTipo)
                                {
                                    bMatch = true;
                                    break;
                                } //
                            } // end for j

                            // aggiunge alla fine la voce altrimenti non trovata
                            if (!bMatch)
                            {
                                _ArticoliPrimaColonna[_iLastArticoloIndexP1] = DB_Data.Articolo[i];

                                if (_iLastArticoloIndexP1 < (MAX_NUM_ARTICOLI + EXTRA_NUM_ARTICOLI - 1))
                                {
                                    j = _iLastArticoloIndexP1;

                                    _iLastArticoloIndexP1++; // può arrivare a (MAX_NUM_ARTICOLI + EXTRA_NUM_ARTICOLI - 1) per non sforare
                                }
                                else
                                {
                                    _WrnMsg.sMsg = String.Format("{0}", DB_Data.Articolo[i].sTipo);
                                    _WrnMsg.iErrID = WRN_NVD;
                                    WarningManager(_WrnMsg);
                                }
                            }

                            if ((DB_Data.Articolo[i].iPrezzoUnitario > 0) ||
#if STANDFACILE
                                (!String.IsNullOrEmpty(DB_Data.Articolo[i].sTipo) && OptionsDlg._rOptionsDlg.GetZeroPriceEnabled()) ||
#endif
                                (DB_Data.Articolo[i].iGruppoStampa == (int)DEST_TYPE.DEST_COUNTER))
                            {
                                if (DB_Data.Articolo[i].iDisponibilita == DISP_OK)
                                    sDisp = "OK";
                                else
                                    sDisp = DB_Data.Articolo[i].iDisponibilita.ToString();

                                if ((columnDay == _SelRange.Start) || (iUpperLimit < _iLastArticoloIndexP1))
                                {
                                    iUpperLimit = _iLastArticoloIndexP1;

                                    xlsWorkSheet.Cells[XLS_VOFFSET + j, 2] = _ArticoliPrimaColonna[j].sTipo;
                                    xlsWorkSheet.Cells[XLS_VOFFSET + j, 3] = (_ArticoliPrimaColonna[j].iPrezzoUnitario) / 100.0f;
                                }

                                xlsWorkSheet.Cells[XLS_VOFFSET + j, iColumn + 4] = DB_Data.Articolo[i].iQuantitaVenduta;

                                if (DB_Data.Articolo[i].iGruppoStampa == (int)DEST_TYPE.DEST_COUNTER)
                                {
                                    if (DB_Data.Articolo[i].sTipo == _COPERTO)
                                        xlsWorkSheet.Cells[XLS_VOFFSET + j, iColumn + 5] = (DB_Data.Articolo[i].iPrezzoUnitario * DB_Data.Articolo[i].iQuantitaVenduta) / 100.0f;
                                    else
                                        xlsWorkSheet.Cells[XLS_VOFFSET + j, iColumn + 5] = "0,00";

                                    if (_SelRange.Start == _SelRange.End)
                                        xlsWorkSheet.Cells[XLS_VOFFSET + j, iColumn + 6] = sDisp;
                                }
                                else
                                {
                                    xlsWorkSheet.Cells[XLS_VOFFSET + j, iColumn + 5] = (DB_Data.Articolo[i].iPrezzoUnitario * DB_Data.Articolo[i].iQuantitaVenduta) / 100.0f;

                                    if (_SelRange.Start == _SelRange.End)
                                        xlsWorkSheet.Cells[XLS_VOFFSET + j, iColumn + 6] = sDisp;

                                    if (!CheckBoxRidColonne.Checked && (_SelRange.Start == _SelRange.End))
                                        xlsWorkSheet.Cells[XLS_VOFFSET + j, iColumn + 7] = DB_Data.Articolo[i].iQtaEsportata;
                                }
                            }
                        }

                        // verifica lo storico e prosegue con iRow
                        if (iLastItemRow < (XLS_VOFFSET + _iLastArticoloIndexP1))
                            iLastItemRow = XLS_VOFFSET + _iLastArticoloIndexP1;

                        iRow = iLastItemRow;

                        // questa parte va scritta solo al secondo passaggio
                        if (iRepeatForLayout == 1)
                        {
                            xlsWorkSheet.Cells[iRow++, iColumn + 5] = "---------";

                            xlsWorkSheet.Cells[iRow, 4] = "TOTALE";
                            xlsWorkSheet.Cells[iRow++, iColumn + 5] = (DB_Data.iTotaleIncasso) / 100.0f;

                            if ((DB_Data.iTotaleScontatoStd > 0) || (DB_Data.iTotaleScontatoFisso > 0) || (DB_Data.iTotaleScontatoGratis > 0))
                            {
                                iRow++;

                                xlsWorkSheet.Cells[iRow, 4] = "valore gratuiti";
                                xlsWorkSheet.Cells[iRow++, iColumn + 5] = (DB_Data.iTotaleScontatoGratis) / 100.0f;

                                xlsWorkSheet.Cells[iRow, 4] = "valore sconto fisso";
                                xlsWorkSheet.Cells[iRow++, iColumn + 5] = (DB_Data.iTotaleScontatoFisso) / 100.0f;

                                xlsWorkSheet.Cells[iRow, 4] = "valore sconto articoli";
                                xlsWorkSheet.Cells[iRow++, iColumn + 5] = (DB_Data.iTotaleScontatoStd) / 100.0f;

                                xlsWorkSheet.Cells[iRow++, iColumn + 5] = "---------";
                                xlsWorkSheet.Cells[iRow, 4] = "TOTALE NETTO";
                                xlsWorkSheet.Cells[iRow++, iColumn + 5] = (DB_Data.iTotaleIncasso - DB_Data.iTotaleScontatoStd -
                                                                            DB_Data.iTotaleScontatoFisso - DB_Data.iTotaleScontatoGratis) / 100.0f;
                            }

                            if ((DB_Data.iTotaleIncassoCard > 0) || (DB_Data.iTotaleIncassoSatispay > 0))
                            {
                                iRow++;

                                xlsWorkSheet.Cells[iRow, 4] = "PAGAM. CARD";
                                xlsWorkSheet.Cells[iRow++, iColumn + 5] = (DB_Data.iTotaleIncassoCard) / 100.0f;

                                xlsWorkSheet.Cells[iRow, 4] = "PAGAM. SATISPAY";
                                xlsWorkSheet.Cells[iRow++, iColumn + 5] = (DB_Data.iTotaleIncassoSatispay) / 100.0f;

                                xlsWorkSheet.Cells[iRow, 4] = "PAGAM. CONT.";
                                xlsWorkSheet.Cells[iRow++, iColumn + 5] = (DB_Data.iTotaleIncasso - DB_Data.iTotaleScontatoStd - DB_Data.iTotaleScontatoFisso -
                                                                            DB_Data.iTotaleScontatoGratis - DB_Data.iTotaleIncassoCard - DB_Data.iTotaleIncassoSatispay) / 100.0f;
                            }

                            // imposta stringa di formato per Larghezza colonne
                            String sFormat = String.Format("{0}12:{0}500", Convert.ToChar(0X44 + iColumn)); // 'D'

                            xlsRange = xlsWorkSheet.get_Range(sFormat, Type.Missing);
                            xlsRange.EntireColumn.ColumnWidth = 12;
                            xlsRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;

                            sFormat = String.Format("{0}12:{0}500", Convert.ToChar(0X45 + iColumn)); // 'E'

                            xlsRange = xlsWorkSheet.get_Range(sFormat, Type.Missing);
                            xlsRange.EntireColumn.ColumnWidth = 10;
                            xlsRange.NumberFormat = NUMERIC_CELL_FORMAT;
                            xlsRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;

                            if (_SelRange.Start == _SelRange.End)
                            {
                                xlsRange = xlsWorkSheet.get_Range("F12:F500", Type.Missing);
                                xlsRange.EntireColumn.ColumnWidth = 10;
                                xlsRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                            }
                        }

                        // aggiornamento indici
                        columnDay = columnDay.AddDays(+1);
                        iColumn += 2;

                    } // end while (columnDay <= _SelRange.End)
                } // for ... iRepeatForLayout

                LogToFile("VisDatiDlg : EsportaDati");

                xlsWorkBook.SaveAs(sDataDir + sNomeFile);
                xlsWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();

                if (CheckBoxXls.Checked)
                    System.Diagnostics.Process.Start(sDataDir + sNomeFile);

                Marshal.ReleaseComObject(xlsWorkSheet);
                Marshal.ReleaseComObject(xlsWorkBook);
                Marshal.ReleaseComObject(xlApp);

                BtnXls.Enabled = true;
            }
            catch (Exception)
            {
                BtnXls.Enabled = true;
                _WrnMsg.iErrID = WRN_EXN;
                WarningManager(_WrnMsg);
            }
        }

        private void BtnDate_Click(object sender, EventArgs e)
        {
            SelectionRange selDates;

            SelDataDlg.rSelDataDlg.ShowDialog();
            selDates = SelDataDlg.rSelDataDlg.GetDateFromPicker();

            if (selDates != null) // non sono uscito con Cancel ...
            {
                DialogResult = DialogResult.None;
                VisualizzaDati((int)FILE_TO_SHOW.FILE_DATI, selDates, SF_Data.iNumCassa, _bVisCambioData, "", comboReport.SelectedIndex);
            }
        }

        private void Combo_NumCassa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_InitCompletato)
                VisualizzaDati((int)FILE_TO_SHOW.FILE_DATI, _SelDate, Combo_NumCassa.SelectedIndex + 1, _bVisCambioData, "", comboReport.SelectedIndex);
        }

        private void ComboReport_SelectedIndexChanged(object sender, EventArgs e)
        {
            _iReportIndex = comboReport.SelectedIndex;

            if (_InitCompletato)
                VisualizzaDati((int)FILE_TO_SHOW.FILE_DATI, _SelDate, Combo_NumCassa.SelectedIndex + 1, _bVisCambioData, "", comboReport.SelectedIndex);
        }

        /// <summary>
        /// verifica e ricalcola l'indice dell'ultima voce presente nella griglia +1 <br/>
        /// serve per agevolare i controlli sui range, <br/> <br/>
        /// attenzione che il limite _iLastArticoloIndexP1 vale solo per SF_Data.Articolo[] e non per DB_Data.Articolo[]
        /// </summary>
        private int CheckLastArticoloIndexP1()
        {
            int i, iUltimoArticolo_NE;

            iUltimoArticolo_NE = 0;

            for (i = 0; (i < MAX_NUM_ARTICOLI - 1); i++) // COPERTI esclusi
                if (!String.IsNullOrEmpty(_ArticoliPrimaColonna[i].sTipo))
                    iUltimoArticolo_NE = i;

            // iUltimoArticolo_NE è un indice di vettore e quindi parte da 0
            _iLastArticoloIndexP1 = iUltimoArticolo_NE + 1;

            return _iLastArticoloIndexP1;
        }

        private void VisDatiDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            _iReportIndex = 0;
        }

    }
}

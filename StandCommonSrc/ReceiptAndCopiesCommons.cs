/*********************************************************************************
 	NomeFile : StandCommonSrc/ReceiptAndCopies.cs
    Data	 : 28.01.2025
 	Autore	 : Mauro Artuso

	Classi di uso comune a DataManager.Receipt(), VisOrdiniDlg.ReceiptRebuild()<br/>
    consente una più agevole manutenzione evitando duplicazioni
 *********************************************************************************/

using System;
using System.IO;
using System.Drawing;


using static StandCommonFiles.ComDef;
using static StandCommonFiles.CommonCl;
using static StandCommonFiles.LogServer;

using StandFacile;
using static StandFacile.glb;
using static StandFacile.Define;

namespace StandCommonFiles
{
#pragma warning disable IDE0044

    /// <summary>
    /// definizione dei codici di errore e di warning
    /// </summary>
    public static class ReceiptAndCopies
    {
#pragma warning disable CS0649

        static TErrMsg _ErrMsg;

        static bool _bCtrlS_UnitQtyItems = false;

#if STANDFACILE
        static bool[] _bSelectedGroups = new bool[NUM_COPIES_GRPS];
        static bool _bPrintSelectedOnly, _bAvoidPrintOtherGroups;
#endif

        /// <summary>
        /// ottiene _bCtrlS_UnitQtyItems per impedire stampa del Logo,<br/>
        /// impostato solo nel DataManager non da VisOrdini
        /// </summary>
        public static bool GetCtrlS_UnitQtyItems() { return _bCtrlS_UnitQtyItems; }

        /// <summary>
        /// verifica nell'ordine caricato se c'è qualcosa da stampare in ogni gruppo di stampa, imposta _bSomethingToPrintGrpParam<br/>
        /// verifica nell'ordine caricato se c'è qualcosa da stampare per ogni colore di stampa,imposta _bSomethingToPrintClrParam<br/>
        /// se bIgnorePrinted == false, tiene conto solo degli Articoli non ancora stampati
        /// deve stare dopo l'acquisizione dei Coperti
        /// </summary>
        public static void CheckSomethingToPrint(bool[] bSomethingToPrintGrpParam, bool[] bSomethingToPrintClrParam, TData dataIdParam, bool bIgnorePrinted = true)
        {
            // deve stare fuori dal loop
            for (int i = 0; i < NUM_COPIES_GRPS; i++)
            {
                bSomethingToPrintGrpParam[i] = false;
                bSomethingToPrintClrParam[i] = false;
            }

            for (int i = 0; i < NUM_COPIES_GRPS; i++)
            {
                for (int j = 0; j < MAX_NUM_ARTICOLI; j++)
                {
                    if (!dataIdParam.Articolo[j].bLocalPrinted || bIgnorePrinted)
                    {
                        if ((dataIdParam.Articolo[j].iQuantitaOrdine > 0) && (dataIdParam.Articolo[j].iGruppoStampa == i))
                        {
                            // il gruppo i-esimo ha qualcosa da stampare
                            bSomethingToPrintGrpParam[i] = true;

                            // questo colore va stampato ma potrebbe essere anche grigio
                            bSomethingToPrintClrParam[i] = true;
                            break;
                        }
                    }
                }

                // devo estendere a tutti i gruppi dello stesso colore > 0 che tanto verrà stampato una sola volta
                if ((i < NUM_EDIT_GROUPS) && bSomethingToPrintClrParam[i] && (dataIdParam.iGroupsColor[i] > 0))
                {
                    for (int k = 0; k < NUM_EDIT_GROUPS; k++)
                    {
                        if (dataIdParam.iGroupsColor[k] == dataIdParam.iGroupsColor[i])
                            bSomethingToPrintClrParam[k] = true;
                    }
                }
            }
        }

        /// <summary>
        /// funzione che resetta la stampa delle copie accorpate in base al colore
        /// </summary>
        public static void ResetCopies_ToBePrintedOnce(bool[] bGroupsColorPrintedParam)
        {
            for (int i = 0; i < NUM_EDIT_GROUPS; i++)
                bGroupsColorPrintedParam[i] = false;
        }

        /// <summary>
        /// funzione per verificare se la copia è da stampare: in particolare verifica <br/>
        /// che le copie con uno stesso colore > 0 associato vengano stampate solo una volta
        /// </summary>
        public static bool CheckCopy_ToBePrintedOnce(int iColorParam, bool[] bGroupsColorPrintedParam, TData dataIdParam)
        {
            if ((iColorParam >= 0) && (iColorParam < NUM_EDIT_GROUPS))
            {
                // il colore trasparente consente sempre la stampa
                if (dataIdParam.iGroupsColor[iColorParam] == 0)
                    return true;

                // il colore > 0 non è ancora stato stampato ritorna true ma
                // contrassegna true i gruppi dello stesso colore
                if (!bGroupsColorPrintedParam[iColorParam])
                {
                    for (int k = 0; k < NUM_EDIT_GROUPS; k++)
                    {
                        // devo saltare anche tutte le copie dello stesso colore
                        if (dataIdParam.iGroupsColor[k] == dataIdParam.iGroupsColor[iColorParam])
                            bGroupsColorPrintedParam[k] = true;
                    }

                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        /// <summary>
        /// verifica se è l'ultimo gruppo da stampare, i contatori vanno esclusi
        /// </summary>
        public static bool CheckLastGroup(bool[] bSomethingToPrintParam, int iParam)
        {
            bool bLast = true;

            for (int i = iParam + 1; i < NUM_EDIT_GROUPS; i++)
            {
                if (bSomethingToPrintParam[i] == true)
                {
                    bLast = false;
                    break;
                }
            }

            return bLast;
        }

#if STANDFACILE

        /// <summary>
        /// verifica se è l'ultimo Articolo significativo da stampare utilizzando<br/>
        /// dataIdParam.Articolo[j].bLocalPrinted = true per gli Articoli già stampati
        /// </summary>
        public static bool CheckLastItemAndGroupToCut(TData dataIdParam, bool[] bSomethingToPrintParam)
        {
            bool bLast;
            int i, j, iTotaleQuantity = 0;

            for (i = 0; i < NUM_COPIES_GRPS; i++)
            {
                if (bSomethingToPrintParam[i] && (!_bPrintSelectedOnly || _bPrintSelectedOnly && (!_bAvoidPrintOtherGroups || _bSelectedGroups[i])))
                {
                    for (j = 0; j < MAX_NUM_ARTICOLI; j++)
                    {
                        if (!dataIdParam.Articolo[j].bLocalPrinted && (dataIdParam.Articolo[j].iGruppoStampa == i))
                            iTotaleQuantity += dataIdParam.Articolo[j].iQuantitaOrdine;

                        if (iTotaleQuantity > 0)
                            break;
                    }
                }

                if (iTotaleQuantity > 0)
                    break;
            }

            bLast = (iTotaleQuantity == 0);

            return bLast;
        }
#endif

        /// <summary>verifica se è l'ultimo Articolo significativo da stampare per unita nello stesso gruppo</summary>
        public static bool CheckLastItemToCut_OnSameGroup(TData dataIdParam, int iGroupParam, int iArtParam)
        {
            bool bLast;
            int j, iTotaleQuantity = 0;

            for (j = iArtParam + 1; j < MAX_NUM_ARTICOLI; j++)
            {
                if (!dataIdParam.Articolo[j].bLocalPrinted)
                {
                    if (!dataIdParam.Articolo[j].bLocalPrinted && (dataIdParam.Articolo[j].iGruppoStampa == iGroupParam))
                        iTotaleQuantity += dataIdParam.Articolo[j].iQuantitaOrdine * 100;
                }

                if (iTotaleQuantity > 0)
                    break;
            }

            bLast = (iTotaleQuantity == 0);

            return bLast;
        }


        /// <summary>
        /// da utilizzare solo per le verifiche di scontrino scontato significativo, <br/>
        /// utilizzato da DataManager per inserire nota nello Scontrino
        /// </summary>
        public static bool TicketScontatoStdIsGood(TData dataIdParam, bool[] bScontoGruppoParam)
        {
            int i;
            int iTotaleScontatoCurrTicket = 0;

            double fPerc = (double)((dataIdParam.iStatusSconto & 0x00FF0000) >> 16) / 100.0f;

            // totale scontrino corrente
            for (i = 0; i < MAX_NUM_ARTICOLI; i++)
            {
                if ((dataIdParam.Articolo[i].iGruppoStampa < NUM_EDIT_GROUPS) && bScontoGruppoParam[dataIdParam.Articolo[i].iGruppoStampa])
                    iTotaleScontatoCurrTicket += (int)Math.Round(dataIdParam.Articolo[i].iQuantitaOrdine * dataIdParam.Articolo[i].iPrezzoUnitario * fPerc);
            }
            return (iTotaleScontatoCurrTicket > 0);
        }

        /// <summary>
        /// funzione preparazione stringhe Header<br/>
        /// prelevando i dati dalla struct dataIdParam
        /// </summary>
        public static TOrdineStrings SetupHeaderStrings(TData dataIdParam, int iNumOfReceiptsParam, Graphics pgParam = null)
        {
            int i;
            string sFmt_QM;

            TOrdineStrings sOrdineStringsTmp = new TOrdineStrings();

            if(string.IsNullOrEmpty(dataIdParam.sWebDateTime))
                dataIdParam.sWebDateTime = GetDateTimeString();

            if (string.IsNullOrEmpty(dataIdParam.sPrevDateTime))
                dataIdParam.sPrevDateTime = GetDateTimeString();

            if (iNumOfReceiptsParam == 0)
                iNumOfReceiptsParam = dataIdParam.iNumOfLastReceipt;

            // *** inizio preparazione stringhe ***

            if (pgParam == null) // generazione files
            {
                i = iNumOfReceiptsParam;

                sFmt_QM = "{0} {1}";
            }
            else // solo per finestra Anteprima
            {
                i = iNumOfReceiptsParam + 1;

                if (dBaseIntf.bUSA_NDB())
                    sFmt_QM = "{0}  {1}"; // stringa più spaziata per allineamento con '?'
                else
                    sFmt_QM = "{0} {1}";
            }

            if ((pgParam != null) && dBaseIntf.bUSA_NDB())
                sOrdineStringsTmp.sOrdineNum = String.Format("{0} ?{1}", _TICK_NUM, i); // Anteprima con ndb
            else
                sOrdineStringsTmp.sOrdineNum = String.Format(sFmt_QM, _TICK_NUM, i);

            if (IsBitSet(dataIdParam.iStatusReceipt, BIT_CARICATO_DA_WEB))
                sOrdineStringsTmp.sOrdNumWeb = String.Format("* {0} {1}, {2} *", _WEB_NUM, dataIdParam.iNumOrdineWeb, dataIdParam.sWebDateTime.Substring(4, 8));
            else
                sOrdineStringsTmp.sOrdNumWeb = "";

            if (!String.IsNullOrEmpty(dataIdParam.sPrevDateTime))
                sOrdineStringsTmp.sOrdNumPrev = String.Format("* {0} {1}, {2} *", _PREV_NUM, dataIdParam.iNumOrdinePrev, dataIdParam.sPrevDateTime.Substring(4, 8));
            else
                sOrdineStringsTmp.sOrdNumPrev = "";

            if (String.IsNullOrEmpty(dataIdParam.sTavolo))
                sOrdineStringsTmp.sTavolo = String.Format("");
            else
                sOrdineStringsTmp.sTavolo = String.Format("Tavolo = {0}", dataIdParam.sTavolo);

            if (String.IsNullOrEmpty(dataIdParam.sNome))
                sOrdineStringsTmp.sNome = String.Format("");
            else
                sOrdineStringsTmp.sNome = String.Format("Nome = {0}", dataIdParam.sNome);

            //sNome = CenterJustify(sNome, iMAX_RECEIPT_CHARS);
            //sTavolo = CenterJustify(sTavolo, iMAX_RECEIPT_CHARS);
            sOrdineStringsTmp.sOrdineNum = CenterJustify(sOrdineStringsTmp.sOrdineNum, iCenterOrderNum);
            sOrdineStringsTmp.sOrdNumWeb = CenterJustify(sOrdineStringsTmp.sOrdNumWeb, iMAX_RECEIPT_CHARS);
            sOrdineStringsTmp.sOrdNumPrev = CenterJustify(sOrdineStringsTmp.sOrdNumPrev, iMAX_RECEIPT_CHARS);

            return sOrdineStringsTmp;
        }

        /// <summary>
        /// funzione che scrive lo scontrino nel file fPrintParam,<br/>
        /// prelevando i dati dalla struct dataIdParam
        /// </summary>
        public static void WriteReceipt(ref TData dataIdParam, int iNumOfReceiptsParam, StreamWriter fPrintParam, String sDirParam, TOrdineStrings sOrdineStringsParam)
        {
            int i, j, iIncassoParz;

#if STANDFACILE
            ulong ulStart, ulStop, ulPingTime;
#endif
            String sTmp, sIncassoParz;
            String sNomeFileTicketPrt;

            bool[] bSomethingInto_GrpToPrint = new bool[NUM_COPIES_GRPS];
            bool[] bSomethingInto_ClrToPrint = new bool[NUM_COPIES_GRPS]; // OK
            bool[] bScontoGruppo = new bool[NUM_EDIT_GROUPS];

            if (iNumOfReceiptsParam == 0)
                iNumOfReceiptsParam = dataIdParam.iNumOfLastReceipt;

            CheckSomethingToPrint(bSomethingInto_GrpToPrint, bSomethingInto_ClrToPrint, dataIdParam);

            dataIdParam.iTotaleReceipt = 0;

            double fPerc = ((dataIdParam.iStatusSconto & 0x00FF0000) >> 16) / 100.0f;

            for (i = 0; i < NUM_EDIT_GROUPS; i++)
                bScontoGruppo[i] = IsBitSet(dataIdParam.iStatusSconto, 8 + i);

            dataIdParam.iScontoStdReceipt = 0;   // richiede calcolo
            dataIdParam.iScontoGratisReceipt = 0; // richiede calcolo

            sNomeFileTicketPrt = String.Format(NOME_FILE_RECEIPT, dataIdParam.iNumCassa, iNumOfReceiptsParam);
            _ErrMsg.sNomeFile = sNomeFileTicketPrt;

            fPrintParam = File.CreateText(sDirParam + sNomeFileTicketPrt);
            if (fPrintParam == null)
            {
                _ErrMsg.iErrID = ERR_FNO;
                ErrorManager(_ErrMsg);
            }
            else
            {
                // se non c'è il logo stampa sHeaders[0]
                if (((iSysPrinterType == (int)PRINTER_SEL.STAMPANTE_WINDOWS) && !string.IsNullOrEmpty(sGlbWinPrinterParams.sLogoName)) ||
                    ((iSysPrinterType == (int)PRINTER_SEL.STAMPANTE_LEGACY) && (sGlbLegacyPrinterParams.iLogoBmp != 0)))
                {
                    sTmp = CenterJustify(_LOGO, iMAX_RECEIPT_CHARS);
                    fPrintParam.WriteLine("{0}", sTmp); fPrintParam.WriteLine();
                }
                else
                {
                    if (!String.IsNullOrEmpty(dataIdParam.sHeaders[0]))
                    {
                        sTmp = CenterJustify(dataIdParam.sHeaders[0], iMAX_RECEIPT_CHARS);
                        fPrintParam.WriteLine("{0}", sTmp); fPrintParam.WriteLine();
                    }
                }

                if (!String.IsNullOrEmpty(dataIdParam.sHeaders[1]))
                {
                    sTmp = CenterJustify(dataIdParam.sHeaders[1], iMAX_RECEIPT_CHARS);
                    fPrintParam.WriteLine("{0}", sTmp); fPrintParam.WriteLine();
                }

                sTmp = String.Format("{0,-22}C.{1}", dataIdParam.sDateTime, dataIdParam.iNumCassa);
                sTmp = CenterJustify(sTmp, iMAX_RECEIPT_CHARS);
                fPrintParam.WriteLine("{0}", sTmp); fPrintParam.WriteLine();

                if (dataIdParam.bAnnullato) // ha senso solo nelle ricostruzioni, quindi con DB_Data
                {
                    sTmp = CenterJustify(sConst_Annullo[0], iMAX_RECEIPT_CHARS);
                    fPrintParam.WriteLine("{0}", sTmp);

                    sTmp = CenterJustify(sConst_Annullo[1], iMAX_RECEIPT_CHARS);
                    fPrintParam.WriteLine("{0}", sTmp);

                    sTmp = CenterJustify(sConst_Annullo[2], iMAX_RECEIPT_CHARS);
                    fPrintParam.WriteLine("{0}", sTmp); fPrintParam.WriteLine();
                }

                if (!String.IsNullOrEmpty(sOrdineStringsParam.sTavolo) && !String.IsNullOrEmpty(sOrdineStringsParam.sNome))
                {
                    fPrintParam.WriteLine("{0}", sOrdineStringsParam.sOrdineNum); fPrintParam.WriteLine();
                    fPrintParam.WriteLine(" {0}", sOrdineStringsParam.sTavolo);
                    fPrintParam.WriteLine(" {0}", sOrdineStringsParam.sNome);
                }
                else if (!String.IsNullOrEmpty(sOrdineStringsParam.sTavolo))
                {
                    fPrintParam.WriteLine("{0}", sOrdineStringsParam.sOrdineNum); fPrintParam.WriteLine();
                    fPrintParam.WriteLine(" {0}", sOrdineStringsParam.sTavolo);
                }
                else if (!String.IsNullOrEmpty(sOrdineStringsParam.sNome))
                {
                    fPrintParam.WriteLine("{0}", sOrdineStringsParam.sOrdineNum); fPrintParam.WriteLine();
                    fPrintParam.WriteLine(" {0}", sOrdineStringsParam.sNome);
                }
                else
                {
                    fPrintParam.WriteLine("{0}", sOrdineStringsParam.sOrdineNum);
                }

                fPrintParam.WriteLine();

                if (IsBitSet(dataIdParam.iStatusReceipt, BIT_CARICATO_DA_WEB))
                {
                    fPrintParam.WriteLine("{0}", sOrdineStringsParam.sOrdNumWeb); fPrintParam.WriteLine();
                }

                if (IsBitSet(dataIdParam.iStatusReceipt, BIT_CARICATO_DA_PREVENDITA))
                {
                    fPrintParam.WriteLine("{0}", sOrdineStringsParam.sOrdNumPrev); fPrintParam.WriteLine();
                }

                // stampa CONTATORI
                if (bSomethingInto_GrpToPrint[(int)DEST_TYPE.DEST_COUNTER])
                {
                    for (j = 0; j < MAX_NUM_ARTICOLI; j++)
                    {
                        if ((dataIdParam.Articolo[j].iQuantitaOrdine > 0) && (dataIdParam.Articolo[j].iGruppoStampa == (int)DEST_TYPE.DEST_COUNTER))
                        {
                            if (dataIdParam.Articolo[j].sTipo == _COPERTO)
                            {
                                iIncassoParz = dataIdParam.Articolo[j].iQuantitaOrdine * dataIdParam.Articolo[j].iPrezzoUnitario;
                                dataIdParam.iTotaleReceipt += iIncassoParz;
                                sIncassoParz = IntToEuro(iIncassoParz);
                            }
                            else
                                sIncassoParz = "";

                            sTmp = String.Format(sRCP_FMT_RCPT, dataIdParam.Articolo[j].iQuantitaOrdine, dataIdParam.Articolo[j].sTipo, sIncassoParz);
                            fPrintParam.WriteLine("{0}", sTmp);

                            if (!String.IsNullOrEmpty(dataIdParam.Articolo[j].sNotaArt))
                            {
                                sTmp = String.Format(sRCP_FMT_NOTE + "\r\n", dataIdParam.Articolo[j].sNotaArt);
                                fPrintParam.WriteLine("{0}", sTmp);
                            }
                        }
                    }

                    fPrintParam.WriteLine();
                } // end if

                // stampa CONTATORI esclusi
                for (i = 0; i < NUM_EDIT_GROUPS; i++)
                {
                    if (bSomethingInto_GrpToPrint[i])
                    {
                        for (j = 0; j < MAX_NUM_ARTICOLI; j++)
                        {
                            if ((dataIdParam.Articolo[j].iQuantitaOrdine > 0) && (dataIdParam.Articolo[j].iGruppoStampa == i))
                            {
                                if (IsBitSet(dataIdParam.iStatusSconto, BIT_SCONTO_STD) && bScontoGruppo[i])
                                    dataIdParam.iScontoStdReceipt += (int)Math.Round(dataIdParam.Articolo[j].iQuantitaOrdine * dataIdParam.Articolo[j].iPrezzoUnitario * fPerc);

                                iIncassoParz = dataIdParam.Articolo[j].iQuantitaOrdine * dataIdParam.Articolo[j].iPrezzoUnitario;
                                dataIdParam.iTotaleReceipt += iIncassoParz;
                                sIncassoParz = IntToEuro(iIncassoParz);

                                // 89 123456789012345678 9876.00  width=28
                                sTmp = String.Format(sRCP_FMT_RCPT, dataIdParam.Articolo[j].iQuantitaOrdine, dataIdParam.Articolo[j].sTipo, sIncassoParz);
                                fPrintParam.WriteLine("{0}", sTmp);

                                if (!String.IsNullOrEmpty(dataIdParam.Articolo[j].sNotaArt))
                                {
                                    sTmp = String.Format(sRCP_FMT_NOTE + "\r\n", dataIdParam.Articolo[j].sNotaArt);
                                    fPrintParam.WriteLine("{0}", sTmp);
                                }
                            }
                        }

                        if (!CheckLastGroup(bSomethingInto_GrpToPrint, i))
                            fPrintParam.WriteLine();
                    } // end if
                }

                fPrintParam.WriteLine(sRCP_FMT_DSH, "------");

                // punto doppio
                dataIdParam.iScontoStdReceipt = Arrotonda(dataIdParam.iScontoStdReceipt);

                if (IsBitSet(dataIdParam.iStatusSconto, BIT_SCONTO_STD) && TicketScontatoStdIsGood(dataIdParam, bScontoGruppo))
                {
                    fPrintParam.WriteLine(sRCP_FMT_DSC, "SCONTO", IntToEuro(dataIdParam.iScontoStdReceipt), "TOTALE", IntToEuro(dataIdParam.iTotaleReceipt));

                    dataIdParam.iTotaleReceiptDovuto = dataIdParam.iTotaleReceipt - dataIdParam.iScontoStdReceipt;

                    fPrintParam.WriteLine(sRCP_FMT_DIF + "\r\n", "DIFF. DOVUTA", IntToEuro(dataIdParam.iTotaleReceiptDovuto));

                    dataIdParam.iTotaleScontatoStd += dataIdParam.iScontoStdReceipt;

                    if (IsBitSet(dataIdParam.iStatusReceipt, BIT_PAGAM_CARD))
                        dataIdParam.iTotaleIncassoCard += dataIdParam.iTotaleReceipt - dataIdParam.iScontoStdReceipt;

                    if (IsBitSet(dataIdParam.iStatusReceipt, BIT_PAGAM_SATISPAY))
                        dataIdParam.iTotaleIncassoSatispay += dataIdParam.iTotaleReceipt - dataIdParam.iScontoStdReceipt;
                }
                else if (IsBitSet(dataIdParam.iStatusSconto, BIT_SCONTO_FISSO))
                {
                    dataIdParam.iTotaleReceiptDovuto = dataIdParam.iTotaleReceipt - dataIdParam.iScontoFissoReceipt;

                    if (dataIdParam.iTotaleReceiptDovuto < 0)
                    {
                        dataIdParam.iTotaleReceiptDovuto = 0;
                        dataIdParam.iScontoFissoReceipt = dataIdParam.iTotaleReceipt;

                        fPrintParam.WriteLine(sRCP_FMT_DSC, "SCONTO", IntToEuro(dataIdParam.iTotaleReceipt),
                                                "TOTALE", IntToEuro(dataIdParam.iTotaleReceipt));
                    }
                    else
                        fPrintParam.WriteLine(sRCP_FMT_DSC, "SCONTO", IntToEuro(dataIdParam.iScontoFissoReceipt),
                                                "TOTALE", IntToEuro(dataIdParam.iTotaleReceipt));

                    fPrintParam.WriteLine(sRCP_FMT_DIF + "\r\n", "DIFF. DOVUTA", IntToEuro(dataIdParam.iTotaleReceiptDovuto));

                    dataIdParam.iTotaleScontatoFisso += dataIdParam.iScontoFissoReceipt;

                    if (IsBitSet(dataIdParam.iStatusReceipt, BIT_PAGAM_CARD))
                        dataIdParam.iTotaleIncassoCard += dataIdParam.iTotaleReceipt - dataIdParam.iScontoFissoReceipt;

                    if (IsBitSet(dataIdParam.iStatusReceipt, BIT_PAGAM_SATISPAY))
                        dataIdParam.iTotaleIncassoSatispay += dataIdParam.iTotaleReceipt - dataIdParam.iScontoFissoReceipt;
                }
                else if (IsBitSet(dataIdParam.iStatusSconto, BIT_SCONTO_GRATIS))
                {
                    fPrintParam.WriteLine(sRCP_FMT_TOT, "TOTALE", IntToEuro(dataIdParam.iTotaleReceipt));

                    fPrintParam.WriteLine(sRCP_FMT_TOT + "\r\n", "DOVUTO", IntToEuro(0));

                    dataIdParam.iTotaleReceiptDovuto = 0;
                    dataIdParam.iScontoGratisReceipt = dataIdParam.iTotaleReceipt;
                    dataIdParam.iTotaleScontatoGratis += dataIdParam.iTotaleReceipt;
                }
                else
                {
                    dataIdParam.iTotaleReceiptDovuto = dataIdParam.iTotaleReceipt;

                    if (IsBitSet(dataIdParam.iStatusReceipt, BIT_PAGAM_CARD))
                        dataIdParam.iTotaleIncassoCard += dataIdParam.iTotaleReceipt;

                    if (IsBitSet(dataIdParam.iStatusReceipt, BIT_PAGAM_SATISPAY))
                        dataIdParam.iTotaleIncassoSatispay += dataIdParam.iTotaleReceipt;

                    fPrintParam.WriteLine(sRCP_FMT_TOT + "\r\n", "TOTALE", IntToEuro(dataIdParam.iTotaleReceipt));
                }

                dataIdParam.iTotaleIncasso += dataIdParam.iTotaleReceipt;

                // inserimento indicazione di sconto
                if (IsBitSet(dataIdParam.iStatusSconto, BIT_SCONTO_STD) && TicketScontatoStdIsGood(dataIdParam, bScontoGruppo))
                {
                    sTmp = CenterJustify(sConst_Sconti[0], iMAX_RECEIPT_CHARS);
                    fPrintParam.WriteLine("{0}", sTmp);

                    sTmp = CenterJustify(dataIdParam.sScontoReceipt, iMAX_RECEIPT_CHARS);
                    if (!String.IsNullOrEmpty(sTmp))
                        fPrintParam.WriteLine("{0}", sTmp);

                    sTmp = CenterJustify(sConst_Sconti[3], iMAX_RECEIPT_CHARS);
                    fPrintParam.WriteLine("{0}", sTmp); fPrintParam.WriteLine();
                }
                else if (IsBitSet(dataIdParam.iStatusSconto, BIT_SCONTO_FISSO))
                {
                    sTmp = CenterJustify(sConst_Sconti[1], iMAX_RECEIPT_CHARS);
                    fPrintParam.WriteLine("{0}", sTmp);
                    sTmp = CenterJustify(dataIdParam.sScontoReceipt, iMAX_RECEIPT_CHARS);
                    fPrintParam.WriteLine("{0}", sTmp);
                    sTmp = CenterJustify(sConst_Sconti[3], iMAX_RECEIPT_CHARS);
                    fPrintParam.WriteLine("{0}", sTmp);
                    fPrintParam.WriteLine();
                }
                else if (IsBitSet(dataIdParam.iStatusSconto, BIT_SCONTO_GRATIS))
                {
                    sTmp = CenterJustify(sConst_Sconti[2], iMAX_RECEIPT_CHARS);
                    fPrintParam.WriteLine("{0}", sTmp);
                    sTmp = CenterJustify(dataIdParam.sScontoReceipt, iMAX_RECEIPT_CHARS);
                    fPrintParam.WriteLine("{0}", sTmp);
                    sTmp = CenterJustify(sConst_Sconti[3], iMAX_RECEIPT_CHARS);
                    fPrintParam.WriteLine("{0}", sTmp);
                    fPrintParam.WriteLine();
                }

                if (IsBitSet(dataIdParam.iStatusReceipt, BIT_PAGAM_CASH))
                {
                    // non scrivere nulla
                }
                else if (IsBitSet(dataIdParam.iStatusReceipt, BIT_PAGAM_CARD))
                {
                    sTmp = CenterJustify(sConst_Pagamento_CARD, iMAX_RECEIPT_CHARS);
                    fPrintParam.WriteLine("{0}", sTmp);
                    fPrintParam.WriteLine();
                }
                else if (IsBitSet(dataIdParam.iStatusReceipt, BIT_PAGAM_SATISPAY))
                {
                    sTmp = CenterJustify(sConst_Pagamento_Satispay, iMAX_RECEIPT_CHARS);
                    fPrintParam.WriteLine("{0}", sTmp);
                    fPrintParam.WriteLine();
                }
                else
                {
                    sTmp = CenterJustify(sConst_Pagamento_da_EFFETTUARE, iMAX_RECEIPT_CHARS);
                    fPrintParam.WriteLine("{0}", sTmp);
                    fPrintParam.WriteLine();
                }

                if (!String.IsNullOrEmpty(dataIdParam.sNota))
                {
                    sTmp = CenterJustify(sConst_Nota[0], iMAX_RECEIPT_CHARS);
                    fPrintParam.WriteLine("{0}", sTmp);

                    sTmp = CenterJustify(dataIdParam.sNota, iMAX_RECEIPT_CHARS);
                    fPrintParam.WriteLine("{0}", sTmp);

                    sTmp = CenterJustify(sConst_Nota[1], iMAX_RECEIPT_CHARS);
                    fPrintParam.WriteLine("{0}", sTmp); fPrintParam.WriteLine();
                }

                if (IsBitSet(dataIdParam.iStatusReceipt, BIT_ESPORTAZIONE))
                {
                    sTmp = CenterJustify(sConst_Esportazione[0], iMAX_RECEIPT_CHARS);
                    fPrintParam.WriteLine("{0}", sTmp);
                    sTmp = CenterJustify(sConst_Esportazione[1], iMAX_RECEIPT_CHARS);
                    fPrintParam.WriteLine("{0}", sTmp);
                    sTmp = CenterJustify(sConst_Esportazione[2], iMAX_RECEIPT_CHARS);
                    fPrintParam.WriteLine("{0}\n", sTmp);
                }

                if (dataIdParam.bPrevendita)
                {
                    sTmp = CenterJustify(sConst_Prevendita[0], iMAX_RECEIPT_CHARS);
                    fPrintParam.WriteLine("{0}", sTmp);
                    sTmp = CenterJustify(sConst_Prevendita[1], iMAX_RECEIPT_CHARS);
                    fPrintParam.WriteLine("{0}", sTmp);
                    sTmp = CenterJustify(sConst_Prevendita[2], iMAX_RECEIPT_CHARS);
                    fPrintParam.WriteLine("{0}\n", sTmp);
                }

                if (!String.IsNullOrEmpty(dataIdParam.sHeaders[2]))
                {
                    sTmp = CenterJustify(dataIdParam.sHeaders[2], iMAX_RECEIPT_CHARS);
                    fPrintParam.WriteLine("{0}", sTmp); fPrintParam.WriteLine();
                }

                if (!String.IsNullOrEmpty(dataIdParam.sHeaders[3]))
                {
                    sTmp = CenterJustify(dataIdParam.sHeaders[3], iMAX_RECEIPT_CHARS);
                    fPrintParam.WriteLine("{0}", sTmp); fPrintParam.WriteLine();
                }

                fPrintParam.Close();
            }

#if STANDFACILE
            /*************************************
             *	salvataggio nel Database Ordini
             *************************************/

            if (Equals(dataIdParam, SF_Data))
            {
                ulStart = (ulong)Environment.TickCount;

                dBaseIntf._rdBaseIntf.dbSalvaOrdine();

                // misura del tempo in ms per eseguire dbSalvaOrdine
                ulStop = (ulong)Environment.TickCount;
                ulPingTime = ulStop - ulStart;
                sTmp = String.Format("{0} ms", ulPingTime);

                LogToFile("DataManager dbSalvaOrdine : dbFuncTime = " + sTmp);

                if (PrintReceiptConfigDlg.GetPrinterTypeIsWinwows())
                {
                    Printer_Windows.PrintFile(sDirParam + sNomeFileTicketPrt, sGlbWinPrinterParams, NUM_EDIT_GROUPS);
                }
                else
                {
                    Printer_Legacy.PrintFile(sDirParam + sNomeFileTicketPrt, sGlbLegacyPrinterParams, (int)PRINT_QUEUE_ACTION.PRINT_ENQUEUE);
                }
            }
#endif
        }

#if STANDFACILE
        /// <summary>
        /// funzione che scrive la copie locali dello scontrino nel file fPrintParam,<br/>
        /// prelevando i dati dalla struct dataIdParam
        /// </summary>
        public static void WriteLocalCopy(TData dataIdParam, int iNumOfReceiptsParam, String sDirParam, TOrdineStrings sOrdineStringsParam)
        {
            bool bLocalCopyRequested, bSingleRowItems, bUnitQtyItems;
            bool bHeaderToBePrinted, bGroupsTextToPrint, bTicketCopies_CutRequired;

            int i, j, k;
            int iNumCoperti = 0;

            String sTmp, sDebug, sNomeFileTicketNpPrt;
            String sHeader1_ToPrintBeforeCut, sHeader2_ToPrintBeforeCut;
            StreamWriter fPrintParam;

            bool[] bSomethingInto_GrpToPrint = new bool[NUM_COPIES_GRPS];
            bool[] bSomethingInto_ClrToPrint = new bool[NUM_COPIES_GRPS]; // OK

            int[] iGrpReorderPtr = new int[NUM_COPIES_GRPS];

            bHeaderToBePrinted = true;

            _bCtrlS_UnitQtyItems = false;

            sHeader1_ToPrintBeforeCut = "";
            sHeader2_ToPrintBeforeCut = "";

            if (iNumOfReceiptsParam == 0)
                iNumOfReceiptsParam = dataIdParam.iNumOfLastReceipt;

            CheckSomethingToPrint(bSomethingInto_GrpToPrint, bSomethingInto_ClrToPrint, dataIdParam);

            sNomeFileTicketNpPrt = String.Format(NOME_FILE_RECEIPT_NP, dataIdParam.iNumCassa, iNumOfReceiptsParam);
            _ErrMsg.sNomeFile = sNomeFileTicketNpPrt;

            _bAvoidPrintOtherGroups = IsBitSet(SF_Data.iReceiptCopyOptions, BIT_AVOIDPRINTGROUPS_PRINT_REQUIRED);

            bLocalCopyRequested = IsBitSet(SF_Data.iReceiptCopyOptions, BIT_RECEIPT_LOCAL_COPY_REQUIRED);

            _bPrintSelectedOnly = IsBitSet(SF_Data.iReceiptCopyOptions, BIT_SELECTEDONLY_PRINT_REQUIRED);

            bSingleRowItems = IsBitSet(SF_Data.iReceiptCopyOptions, BIT_SINGLEROWITEMS_PRINT_REQUIRED);

            bUnitQtyItems = IsBitSet(SF_Data.iReceiptCopyOptions, BIT_QUANTITYONE_PRINT_REQUIRED);

            bTicketCopies_CutRequired = IsBitSet(SF_Data.iReceiptCopyOptions, BIT_PRINT_GROUPS_CUT_REQUIRED);

            // conferma dalle altre dipendenze
            _bAvoidPrintOtherGroups |= !(_bPrintSelectedOnly && (bSingleRowItems || bUnitQtyItems));

            for (i = 0; i < NUM_EDIT_GROUPS; i++)
                _bSelectedGroups[i] = IsBitSet(SF_Data.iReceiptCopyOptions, i);

            // CONTATORI mai selezionati
            _bSelectedGroups[(int)DEST_TYPE.DEST_COUNTER] = true;

            // puntatore di riordino per consentire per primi DEST_TYPE.DEST_COUNTER
            iGrpReorderPtr[0] = (int)DEST_TYPE.DEST_COUNTER;

            for (i = 0; i < NUM_EDIT_GROUPS; i++)
                iGrpReorderPtr[i + 1] = i;


            if (bLocalCopyRequested && !(dataIdParam.bPrevendita || IsBitSet(dataIdParam.iStatusReceipt, BIT_EMESSO_IN_PREVENDITA) && !IsBitSet(dataIdParam.iStatusReceipt, BIT_CARICATO_DA_PREVENDITA)))
            {

                fPrintParam = File.CreateText(sDirParam + sNomeFileTicketNpPrt);
                if (fPrintParam == null)
                {
                    _ErrMsg.iErrID = ERR_FNO;
                    ErrorManager(_ErrMsg);
                }
                else
                {
                    // se non c'è il logo stampa sHeaders[0]
                    if ((((iSysPrinterType == (int)PRINTER_SEL.STAMPANTE_WINDOWS) && !string.IsNullOrEmpty(sGlbWinPrinterParams.sLogoName)) ||
                        ((iSysPrinterType == (int)PRINTER_SEL.STAMPANTE_LEGACY) && (sGlbLegacyPrinterParams.iLogoBmp != 0))) &&
                        WinPrinterDlg.GetCopies_LogoToBePrinted())
                    {
                        sTmp = CenterJustify(_LOGO, MAX_RECEIPT_CHARS_CPY);
                        sHeader1_ToPrintBeforeCut += String.Format("{0}\r\n\r\n", sTmp);
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(dataIdParam.sHeaders[0]))
                        {
                            sTmp = CenterJustify(dataIdParam.sHeaders[0], MAX_RECEIPT_CHARS_CPY);
                            sHeader1_ToPrintBeforeCut += String.Format("{0}\r\n\r\n", sTmp);
                        }
                    }

                    if (!(bUnitQtyItems || bSingleRowItems) && !String.IsNullOrEmpty(dataIdParam.sHeaders[1]))
                    {
                        sTmp = CenterJustify(dataIdParam.sHeaders[1], MAX_RECEIPT_CHARS_CPY);
                        sHeader1_ToPrintBeforeCut += String.Format("{0}\r\n\r\n", sTmp);
                    }

                    sTmp = String.Format("{0,-22}C.{1}", dataIdParam.sDateTime, dataIdParam.iNumCassa);
                    sTmp = CenterJustify(sTmp, MAX_RECEIPT_CHARS_CPY);
                    sHeader1_ToPrintBeforeCut += String.Format("{0}\r\n", sTmp);

                    if (dataIdParam.bAnnullato) // ha senso solo nelle ricostruzioni, quindi con DB_Data
                    {
                        sTmp = CenterJustify(sConst_Annullo[0], MAX_RECEIPT_CHARS_CPY);
                        sHeader1_ToPrintBeforeCut += "\r\n" + sTmp + "\r\n";

                        sTmp = CenterJustify(sConst_Annullo[1], MAX_RECEIPT_CHARS_CPY);
                        sHeader1_ToPrintBeforeCut += sTmp + "\r\n";

                        sTmp = CenterJustify(sConst_Annullo[2], MAX_RECEIPT_CHARS_CPY);
                        sHeader1_ToPrintBeforeCut += sTmp + "\r\n";
                    }

                    if (!String.IsNullOrEmpty(sOrdineStringsParam.sTavolo) && !String.IsNullOrEmpty(sOrdineStringsParam.sNome))
                    {
                        sHeader2_ToPrintBeforeCut += String.Format("  {0}\r\n  {1}\r\n", sOrdineStringsParam.sTavolo, sOrdineStringsParam.sNome);
                    }
                    else if (!String.IsNullOrEmpty(sOrdineStringsParam.sTavolo))
                    {
                        sHeader2_ToPrintBeforeCut += String.Format("  {0}\r\n", sOrdineStringsParam.sTavolo);
                    }
                    else if (!String.IsNullOrEmpty(sOrdineStringsParam.sNome))
                    {
                        sHeader2_ToPrintBeforeCut += String.Format("  {0}\r\n", sOrdineStringsParam.sNome);
                    }
                    else
                    {
                        sHeader2_ToPrintBeforeCut += ""; //  String.Format("{0}", sNum);
                    }

                    if (IsBitSet(dataIdParam.iStatusReceipt, BIT_CARICATO_DA_WEB))
                    {
                        sHeader2_ToPrintBeforeCut += String.Format("{0}\r\n", sOrdineStringsParam.sOrdNumWeb);
                    }

                    if (IsBitSet(dataIdParam.iStatusReceipt, BIT_CARICATO_DA_PREVENDITA))
                    {
                        sHeader2_ToPrintBeforeCut += String.Format("{0}\r\n", sOrdineStringsParam.sOrdNumPrev);
                    }

                    // stampa #1: quantità UNO mediante Crtl+S
                    for (i = 0; i < NUM_COPIES_GRPS; i++)
                    {
                        for (j = 0; (j < MAX_NUM_ARTICOLI) && bSomethingInto_GrpToPrint[iGrpReorderPtr[i]]; j++)
                        {
                            if (IsBitSet(dataIdParam.Articolo[j].iOptionsFlags, BIT_STAMPA_SINGOLA_NELLA_COPIA_RECEIPT) && (dataIdParam.Articolo[j].iGruppoStampa == iGrpReorderPtr[i]))
                            {
                                _bCtrlS_UnitQtyItems = true;

                                for (k = 0; k < dataIdParam.Articolo[j].iQuantitaOrdine; k++)
                                {
                                    if (!String.IsNullOrEmpty(sHeader1_ToPrintBeforeCut))
                                        fPrintParam.WriteLine("{0}", sHeader1_ToPrintBeforeCut);

                                    sTmp = String.Format("  $ {0} {1}, {2} di {3}", _TICK_NUM_NZ, iNumOfReceiptsParam, k + 1, dataIdParam.Articolo[j].iQuantitaOrdine);
                                    sTmp = CenterJustify(sTmp, MAX_RECEIPT_CHARS_CPY);

                                    fPrintParam.WriteLine("{0}\r\n", sTmp);

                                    if (!String.IsNullOrEmpty(sHeader2_ToPrintBeforeCut))
                                        fPrintParam.WriteLine("{0}", sHeader2_ToPrintBeforeCut);

                                    // larghezza 28 "{0,2} {1,-18}{2,7}" :89 123456789012345678 9876.00
                                    sTmp = String.Format(sRCP_FMT_CPY, 1, dataIdParam.Articolo[j].sTipo);
                                    fPrintParam.WriteLine("{0}\r\n", sTmp);

                                    if (!String.IsNullOrEmpty(dataIdParam.Articolo[j].sNotaArt))
                                    {
                                        sTmp = String.Format(" " + sRCP_FMT_NOTE + "\r\n", dataIdParam.Articolo[j].sNotaArt);
                                        fPrintParam.WriteLine("{0}", sTmp);
                                    }

                                    if (k < dataIdParam.Articolo[j].iQuantitaOrdine - 1)
                                        fPrintParam.WriteLine(_CUT_FMT, CenterJustify(_CUT, MAX_RECEIPT_CHARS_CPY));
                                }

                                dataIdParam.Articolo[j].bLocalPrinted = true;

                                // un solo footer
                                WriteFooter(dataIdParam, fPrintParam);

                                if (!CheckLastItemAndGroupToCut(dataIdParam, bSomethingInto_GrpToPrint))
                                    fPrintParam.WriteLine(_CUT_FMT, CenterJustify(_CUT, MAX_RECEIPT_CHARS_CPY));
                            }
                        }
                    }


                    // stampa #2: quantità UNO
                    for (i = 0; i < NUM_COPIES_GRPS; i++)
                    {
                        for (j = 0; (j < MAX_NUM_ARTICOLI) && bSomethingInto_GrpToPrint[iGrpReorderPtr[i]]; j++)
                        {
                            if (dataIdParam.Articolo[j].bLocalPrinted == true)
                                continue;

                            if ((!_bPrintSelectedOnly || _bSelectedGroups[iGrpReorderPtr[i]]) && bUnitQtyItems && (dataIdParam.Articolo[j].iQuantitaOrdine > 0) && (dataIdParam.Articolo[j].iGruppoStampa == iGrpReorderPtr[i]))
                            {
                                //_bCtrlS_UnitQtyItems = true;

                                for (k = 0; k < dataIdParam.Articolo[j].iQuantitaOrdine; k++)
                                {
                                    if (!String.IsNullOrEmpty(sHeader1_ToPrintBeforeCut))
                                        fPrintParam.WriteLine("{0}", sHeader1_ToPrintBeforeCut);

                                    sTmp = String.Format("  {0} {1}, {2} di {3}", _TICK_NUM_NZ, iNumOfReceiptsParam, k + 1, dataIdParam.Articolo[j].iQuantitaOrdine);
                                    sTmp = CenterJustify(sTmp, MAX_RECEIPT_CHARS_CPY);

                                    fPrintParam.WriteLine("{0}\r\n", sTmp);

                                    if (!String.IsNullOrEmpty(sHeader2_ToPrintBeforeCut))
                                        fPrintParam.WriteLine("{0}", sHeader2_ToPrintBeforeCut);

                                    // larghezza 28 "{0,2} {1,-18}{2,7}" :89 123456789012345678 9876.00
                                    sTmp = String.Format(sRCP_FMT_CPY, 1, dataIdParam.Articolo[j].sTipo);
                                    fPrintParam.WriteLine("{0}\r\n", sTmp);

                                    if (!String.IsNullOrEmpty(dataIdParam.Articolo[j].sNotaArt))
                                    {
                                        sTmp = String.Format(" " + sRCP_FMT_NOTE + "\r\n", dataIdParam.Articolo[j].sNotaArt);
                                        fPrintParam.WriteLine("{0}", sTmp);
                                    }

                                    if (k < dataIdParam.Articolo[j].iQuantitaOrdine - 1)
                                        fPrintParam.WriteLine(_CUT_FMT, CenterJustify(_CUT, MAX_RECEIPT_CHARS_CPY));
                                }

                                dataIdParam.Articolo[j].bLocalPrinted = true;

                                // un solo footer
                                WriteFooter(dataIdParam, fPrintParam);

                                if (!CheckLastItemAndGroupToCut(dataIdParam, bSomethingInto_GrpToPrint))
                                    fPrintParam.WriteLine(_CUT_FMT, CenterJustify(_CUT, MAX_RECEIPT_CHARS_CPY));
                            }
                        }
                    }

                    // stampa #3: bSingleRowItems
                    for (i = 0; i < NUM_COPIES_GRPS; i++)
                    {
                        for (j = 0; (j < MAX_NUM_ARTICOLI) && bSomethingInto_GrpToPrint[iGrpReorderPtr[i]]; j++)
                        {
                            if (dataIdParam.Articolo[j].bLocalPrinted == true)
                                continue;

                            if ((!_bPrintSelectedOnly || _bSelectedGroups[iGrpReorderPtr[i]]) && bSingleRowItems && (dataIdParam.Articolo[j].iQuantitaOrdine > 0) && (dataIdParam.Articolo[j].iGruppoStampa == iGrpReorderPtr[i]))
                            {
                                if (!String.IsNullOrEmpty(sHeader1_ToPrintBeforeCut))
                                    fPrintParam.WriteLine("{0}", sHeader1_ToPrintBeforeCut);

                                sTmp = String.Format("    {0} {1}", _TICK_NUM_NZ, iNumOfReceiptsParam);
                                fPrintParam.WriteLine("{0}\r\n", sTmp);

                                if (!String.IsNullOrEmpty(sHeader2_ToPrintBeforeCut))
                                    fPrintParam.WriteLine("{0}", sHeader2_ToPrintBeforeCut);

                                fPrintParam.WriteLine("    {0}\r\n", dataIdParam.sCopiesGroupsText[iGrpReorderPtr[i]]);

                                // larghezza 28 "{0,2} {1,-18}{2,7}" :89 123456789012345678 9876.00
                                sTmp = String.Format(sRCP_FMT_CPY, dataIdParam.Articolo[j].iQuantitaOrdine, dataIdParam.Articolo[j].sTipo);
                                fPrintParam.WriteLine("{0}\r\n", sTmp);

                                if (!String.IsNullOrEmpty(dataIdParam.Articolo[j].sNotaArt))
                                {
                                    sTmp = String.Format(" " + sRCP_FMT_NOTE + "\r\n", dataIdParam.Articolo[j].sNotaArt);
                                    fPrintParam.WriteLine("{0}", sTmp);
                                }

                                dataIdParam.Articolo[j].bLocalPrinted = true;

                                WriteFooter(dataIdParam, fPrintParam);

                                if (!CheckLastItemAndGroupToCut(dataIdParam, bSomethingInto_GrpToPrint))
                                    fPrintParam.WriteLine(_CUT_FMT, CenterJustify(_CUT, MAX_RECEIPT_CHARS_CPY));
                            }
                        }
                    }

                    // aggiornamento che tiene conto delle stampe effettuate
                    CheckSomethingToPrint(bSomethingInto_GrpToPrint, bSomethingInto_ClrToPrint, dataIdParam, false);

                    // stampa #4: raggruppamento per iGruppoStampa
                    for (i = 0; i < NUM_COPIES_GRPS; i++)
                    {
                        // per ogni Gruppo
                        bGroupsTextToPrint = true;

                        // evita il taglio con CONTATORI e DEST_TIPO1
                        if (bTicketCopies_CutRequired && !((iGrpReorderPtr[i] == (int)DEST_TYPE.DEST_TIPO1) && bSomethingInto_GrpToPrint[(int)DEST_TYPE.DEST_COUNTER]))
                            bHeaderToBePrinted = true; // consente più Headers

                        for (j = 0; (j < MAX_NUM_ARTICOLI) && bSomethingInto_GrpToPrint[iGrpReorderPtr[i]]; j++)
                        {
                            if (dataIdParam.Articolo[j].iIndexListino == MAX_NUM_ARTICOLI - 1)
                                iNumCoperti = dataIdParam.Articolo[j].iQuantitaOrdine;

                            if (dataIdParam.Articolo[j].bLocalPrinted == true)
                                continue;

                            if ((!_bPrintSelectedOnly || _bPrintSelectedOnly && (!_bAvoidPrintOtherGroups || _bSelectedGroups[iGrpReorderPtr[i]])) &&
                                (dataIdParam.Articolo[j].iQuantitaOrdine > 0) && (dataIdParam.Articolo[j].iGruppoStampa == iGrpReorderPtr[i]))
                            {
                                // raggruppamento per iGruppoStampa
                                if (bHeaderToBePrinted)
                                {
                                    if (!String.IsNullOrEmpty(sHeader1_ToPrintBeforeCut))
                                        fPrintParam.WriteLine("{0}", sHeader1_ToPrintBeforeCut);

                                    sTmp = String.Format("{0}", sOrdineStringsParam.sOrdineNum); // consente Zoom numero scontrino
                                    fPrintParam.WriteLine("{0}\r\n", sTmp);

                                    if (!String.IsNullOrEmpty(sHeader2_ToPrintBeforeCut))
                                        fPrintParam.WriteLine("{0}", sHeader2_ToPrintBeforeCut);

                                }

                                if (bTicketCopies_CutRequired && bGroupsTextToPrint)
                                {
                                    bGroupsTextToPrint = false;

#pragma warning disable IDE0059
                                    sDebug = dataIdParam.sCopiesGroupsText[iGrpReorderPtr[i]];
                                    fPrintParam.WriteLine("    {0}\r\n", dataIdParam.sCopiesGroupsText[iGrpReorderPtr[i]]);
                                }

                                if (bHeaderToBePrinted)
                                {
                                    bHeaderToBePrinted = false;

                                    if ((iNumCoperti > 0) && (IsBitSet(SF_Data.iReceiptCopyOptions, BIT_EXTEND_PLACESETTINGS_PRINT_REQUIRED) ||
                                        (iGrpReorderPtr[i] == (int)DEST_TYPE.DEST_COUNTER)))
                                    {
                                        sTmp = String.Format(sRCP_FMT_CPY, iNumCoperti, _COPERTO);
                                        fPrintParam.WriteLine("{0}", sTmp);

                                        // spazio aggiuntivo
                                        if (iGrpReorderPtr[i] != (int)DEST_TYPE.DEST_COUNTER) // && (iGrpReorderPtr[i] != (int)DEST_TYPE.DEST_TIPO1))
                                            fPrintParam.WriteLine();
                                    }
                                }

                                if ((dataIdParam.Articolo[j].iIndexListino != MAX_NUM_ARTICOLI - 1) || (dataIdParam.Articolo[j].iGruppoStampa != (int)DEST_TYPE.DEST_COUNTER))
                                {
                                    sTmp = String.Format(sRCP_FMT_CPY, dataIdParam.Articolo[j].iQuantitaOrdine, dataIdParam.Articolo[j].sTipo);
                                    fPrintParam.WriteLine("{0}", sTmp);
                                }

                                if (!String.IsNullOrEmpty(dataIdParam.Articolo[j].sNotaArt))
                                {
                                    sTmp = String.Format(" " + sRCP_FMT_NOTE + "\r\n", dataIdParam.Articolo[j].sNotaArt);
                                    fPrintParam.WriteLine("{0}", sTmp);
                                }

                                // segna come stampato
                                dataIdParam.Articolo[j].bLocalPrinted = true;

                                // se è l'ultimo del gruppo vediamo ...
                                if (CheckLastItemToCut_OnSameGroup(dataIdParam, iGrpReorderPtr[i], j))
                                {
                                    // se c'è il taglio
                                    if (bTicketCopies_CutRequired && !((iGrpReorderPtr[i] == (int)DEST_TYPE.DEST_COUNTER) && bSomethingInto_GrpToPrint[(int)DEST_TYPE.DEST_TIPO1]))
                                    {
                                        WriteFooter(dataIdParam, fPrintParam);

                                        // se non è l'ultimo articolo in generale fai il taglio
                                        if (!CheckLastItemAndGroupToCut(dataIdParam, bSomethingInto_GrpToPrint))
                                            fPrintParam.WriteLine(_CUT_FMT, CenterJustify(_CUT, MAX_RECEIPT_CHARS_CPY));
                                    }
                                    else // se non c'è il taglio vediamo ...
                                    {
                                        // se è l'ultimo articolo in generale, scrivi il footer altrimenti metti il separatore
                                        if (CheckLastItemAndGroupToCut(dataIdParam, bSomethingInto_GrpToPrint))
                                            WriteFooter(dataIdParam, fPrintParam);
                                        else
                                            fPrintParam.WriteLine();
                                    }
                                }
                            }

                        } // end _bSomethingInto_GrpToPrint
                    }   //  for (i

                    fPrintParam.Close();
                }

                /*****************************************************************************
                 *  MESSA IN CODA DI STAMPA
                 *  iQueueAction == PRINT_ENQUEUE mette in coda senza iniziare
                 *  la stampa ; serve ad evitare problemi di Shared data
                 *  durante _AUTO_SEQ_TEST richiede tempo e provoca disallineamenti
                 *****************************************************************************/
                if (!CheckService(Define._AUTO_SEQ_TEST) && Equals(dataIdParam, SF_Data))
                {
                    if (PrintReceiptConfigDlg.GetPrinterTypeIsWinwows())
                        Printer_Windows.PrintFile(sDirParam + sNomeFileTicketNpPrt, sGlbWinPrinterParams, NUM_EDIT_GROUPS);
                    else
                        Printer_Legacy.PrintFile(sDirParam + sNomeFileTicketNpPrt, sGlbLegacyPrinterParams, (int)PRINT_QUEUE_ACTION.PRINT_ENQUEUE);
                }
            }
        }

#endif

        /// <summary>
        /// funzione che scrive la copie di rete dello scontrino nel file fPrintParam,<br/>
        /// prelevando i dati dalla struct dataIdParam
        /// </summary>
        public static void WriteNetworkCopy(TData dataIdParam, int iNumOfReceiptsParam, StreamWriter fPrintParam, String sDirParam, TOrdineStrings sOrdineStringsParam, bool bOrdineAnnullatoParam)
        {
            bool bColorLoop;

            int i, j, k;
            int iEqRowsNumber, iColorLoop, iNumCoperti, iNumCopertiBackupCopy = 0;

            String sTmp, sNomeFileCopiePrt = "";

            bool[] bSomethingInto_GrpToPrint = new bool[NUM_COPIES_GRPS]; // OK
            bool[] bSomethingInto_ClrToPrint = new bool[NUM_COPIES_GRPS]; // OK
            bool[] bGroupsColorPrinted = new bool[NUM_EDIT_GROUPS];

            if (iNumOfReceiptsParam == 0)
                iNumOfReceiptsParam = dataIdParam.iNumOfLastReceipt;

            if (Equals(dataIdParam, SF_Data))
                Console.WriteLine("WriteNetworkCopy: --- SF_Data ---");
            else
                Console.WriteLine("WriteNetworkCopy: *** DB_Data ***");

            CheckSomethingToPrint(bSomethingInto_GrpToPrint, bSomethingInto_ClrToPrint, dataIdParam);

            ResetCopies_ToBePrintedOnce(bGroupsColorPrinted);

            for (i = 0; (i < NUM_EDIT_GROUPS) && !(dataIdParam.bPrevendita || IsBitSet(dataIdParam.iStatusReceipt, BIT_EMESSO_IN_PREVENDITA) && !IsBitSet(dataIdParam.iStatusReceipt, BIT_CARICATO_DA_PREVENDITA)); i++)
            {
                // evita di stampare più di una volta gruppi dello stesso colore
                if (!CheckCopy_ToBePrintedOnce(i, bGroupsColorPrinted, dataIdParam))
                    continue;

                // evita le stampe attualmente non richieste
#if STANDFACILE
                if (!dataIdParam.bCopiesGroupsFlag[i])
                    continue;
#elif STAND_CUCINA
                    if (!NetConfigLightDlg.GetCopiaGroup(i))
                        continue;
#endif

                iEqRowsNumber = 1; // riga di partenza
                iNumCoperti = iNumCopertiBackupCopy;

                if (bSomethingInto_ClrToPrint[i] || ((i == (int)DEST_TYPE.DEST_TIPO1) && bSomethingInto_GrpToPrint[(int)DEST_TYPE.DEST_COUNTER]))
                {
#if STANDFACILE
                    sNomeFileCopiePrt = String.Format(NOME_FILE_COPIE, dataIdParam.iNumCassa, iNumOfReceiptsParam, i);

                    // verifica se serve ricostruire, commentare per debug
                    if (!bOrdineAnnullatoParam && !CheckService(Define._AUTO_SEQ_TEST) && File.Exists(sDirParam + sNomeFileCopiePrt))
                        continue;
#elif STAND_CUCINA
                    sNomeFileCopiePrt = String.Format(NOME_FILE_COPIE, i);

#endif

                    _ErrMsg.sNomeFile = sNomeFileCopiePrt;

                    fPrintParam = File.CreateText(sDirParam + sNomeFileCopiePrt);
                    if (fPrintParam == null)
                    {
                        _ErrMsg.iErrID = ERR_FNO;
                        ErrorManager(_ErrMsg);
                    }
                    else
                    {
                        // se non c'è il logo stampa sHeaders[0]
                        if ((((iSysPrinterType == (int)PRINTER_SEL.STAMPANTE_WINDOWS) && !string.IsNullOrEmpty(sGlbWinPrinterParams.sLogoName)) ||
                            ((iSysPrinterType == (int)PRINTER_SEL.STAMPANTE_LEGACY) && (sGlbLegacyPrinterParams.iLogoBmp != 0)))
#if STANDFACILE
                            && WinPrinterDlg.GetCopies_LogoToBePrinted()
#endif
                            )
                        {
                            sTmp = CenterJustify(_LOGO, MAX_RECEIPT_CHARS_CPY);
                            fPrintParam.WriteLine("{0}", sTmp); fPrintParam.WriteLine();
                            iEqRowsNumber += 2;
                        }
                        else
                        {
                            if (!String.IsNullOrEmpty(dataIdParam.sHeaders[0]))
                            {
                                sTmp = CenterJustify(dataIdParam.sHeaders[0], MAX_RECEIPT_CHARS_CPY);
                                fPrintParam.WriteLine("{0}", sTmp); fPrintParam.WriteLine();
                                iEqRowsNumber += 2;
                            }
                        }

                        //if (!String.IsNullOrEmpty(dataIdParam.sHeaders[1]))
                        //{
                        //    sTmp = CenterJustify(dataIdParam.sHeaders[1], MAX_RECEIPT_CHARS_CPY);
                        //    fPrintParam.WriteLine("{0}", sTmp); fPrintParam.WriteLine();
                        //    iEqRowsNumber += 2;
                        //}

                        sTmp = String.Format("{0,-22}C.{1}", dataIdParam.sDateTime, dataIdParam.iNumCassa);
                        sTmp = CenterJustify(sTmp, MAX_RECEIPT_CHARS_CPY);
                        fPrintParam.WriteLine("{0}", sTmp); fPrintParam.WriteLine();
                        iEqRowsNumber += 2;

                        if (dataIdParam.bAnnullato) // ha senso solo nelle ricostruzioni, quindi con DB_Data
                        {
                            sTmp = CenterJustify(sConst_Annullo[0], MAX_RECEIPT_CHARS_CPY);
                            fPrintParam.WriteLine("{0}", sTmp);

                            sTmp = CenterJustify(sConst_Annullo[1], MAX_RECEIPT_CHARS_CPY);
                            fPrintParam.WriteLine("{0}", sTmp);

                            sTmp = CenterJustify(sConst_Annullo[2], MAX_RECEIPT_CHARS_CPY);
                            fPrintParam.WriteLine("{0}", sTmp); fPrintParam.WriteLine();
                            iEqRowsNumber += 4;
                        }

                        // posto qui per valutare il tipo di stampa gruppo
                        bColorLoop = (dataIdParam.iGroupsColor[i] > 0);
                        iColorLoop = dataIdParam.iGroupsColor[i];

                        // sicurezza indice iExtLoopColor > 0
                        if (bColorLoop)
                            sTmp = CenterJustify(dataIdParam.sColorGroupsText[iColorLoop - 1], MAX_RECEIPT_CHARS_CPY);
                        else
                            sTmp = CenterJustifyStars(dataIdParam.sCopiesGroupsText[i], MAX_RECEIPT_CHARS_CPY, '#');

                        if (!String.IsNullOrEmpty(sTmp))
                        {
                            fPrintParam.WriteLine("{0}", sTmp); fPrintParam.WriteLine();
                            iEqRowsNumber += 2;
                        }

                        if (!String.IsNullOrEmpty(sOrdineStringsParam.sTavolo) && !String.IsNullOrEmpty(sOrdineStringsParam.sNome))
                        {
                            fPrintParam.WriteLine("{0}", sOrdineStringsParam.sOrdineNum); fPrintParam.WriteLine();
                            fPrintParam.WriteLine("  {0}", sOrdineStringsParam.sTavolo);
                            fPrintParam.WriteLine("  {0}", sOrdineStringsParam.sNome);
                            iEqRowsNumber += 4;
                        }
                        else if (!String.IsNullOrEmpty(sOrdineStringsParam.sTavolo))
                        {
                            fPrintParam.WriteLine("{0}", sOrdineStringsParam.sOrdineNum); fPrintParam.WriteLine();
                            fPrintParam.WriteLine("  {0}", sOrdineStringsParam.sTavolo);
                            iEqRowsNumber += 3;
                        }
                        else if (!String.IsNullOrEmpty(sOrdineStringsParam.sNome))
                        {
                            fPrintParam.WriteLine("{0}", sOrdineStringsParam.sOrdineNum); fPrintParam.WriteLine();
                            fPrintParam.WriteLine("  {0}", sOrdineStringsParam.sNome);
                            iEqRowsNumber += 3;
                        }
                        else
                        {
                            fPrintParam.WriteLine("{0}", sOrdineStringsParam.sOrdineNum);
                            iEqRowsNumber += 1;
                        }

                        fPrintParam.WriteLine();
                        iEqRowsNumber += 1;

                        if (IsBitSet(dataIdParam.iStatusReceipt, BIT_CARICATO_DA_WEB))
                        {
                            fPrintParam.WriteLine("{0}", sOrdineStringsParam.sOrdNumWeb); fPrintParam.WriteLine();
                            iEqRowsNumber += 2;
                        }

                        if (IsBitSet(dataIdParam.iStatusReceipt, BIT_CARICATO_DA_PREVENDITA))
                        {
                            fPrintParam.WriteLine("{0}", sOrdineStringsParam.sOrdNumPrev); fPrintParam.WriteLine();
                            iEqRowsNumber += 2;
                        }

                        for (k = i; k < NUM_EDIT_GROUPS; k++)
                        {
                            // se non siamo nel colorLoop stampa i gruppi con colore trasparente solo una volta quando k == i, oppure
                            // quelli con lo stesso colore dataIdParam.iGroupsColor[k] non trasparente vengono stampati tutti ma solo una volta
                            // grazie a CheckCopy_ToBePrintedOnce()
                            if ((!bColorLoop && (k == i)) || (bColorLoop && (dataIdParam.iGroupsColor[k] == iColorLoop)))
                            {
                                // STAMPA CONTATORI NELLO SCONTRINO DELLE PIETANZE
                                if ((k == (int)DEST_TYPE.DEST_TIPO1) && (bSomethingInto_GrpToPrint[(int)DEST_TYPE.DEST_COUNTER]))
                                {
                                    // aggiunge nel file stampa dei soli contatori
                                    for (j = 0; j < MAX_NUM_ARTICOLI; j++)
                                    {
                                        if ((dataIdParam.Articolo[j].iGruppoStampa == (int)DEST_TYPE.DEST_COUNTER) && (dataIdParam.Articolo[j].iIndexListino == MAX_NUM_ARTICOLI - 1))
                                        {
                                            iNumCoperti = dataIdParam.Articolo[j].iQuantitaOrdine;
                                            iNumCopertiBackupCopy = dataIdParam.Articolo[j].iQuantitaOrdine;
                                        }

                                        else if ((dataIdParam.Articolo[j].iQuantitaOrdine > 0) && (dataIdParam.Articolo[j].iGruppoStampa == (int)DEST_TYPE.DEST_COUNTER) &&
                                            (dataIdParam.Articolo[j].iIndexListino != MAX_NUM_ARTICOLI - 1))
                                        {
                                            fPrintParam.WriteLine(sRCP_FMT_CPY, dataIdParam.Articolo[j].iQuantitaOrdine, dataIdParam.Articolo[j].sTipo);
                                            iEqRowsNumber++;

                                            if (!String.IsNullOrEmpty(dataIdParam.Articolo[j].sNotaArt))
                                            {
                                                sTmp = String.Format(" " + sRCP_FMT_NOTE + "\r\n", dataIdParam.Articolo[j].sNotaArt);
                                                fPrintParam.WriteLine("{0}", sTmp);
                                                iEqRowsNumber++;
                                            }
                                        }
                                    }
                                }

                                // STAMPA SENZA CONTATORI ma se richiesto si ripetono i coperti
                                if ((bSomethingInto_GrpToPrint[k]) || (iNumCoperti >= 0))
                                {
                                    // stampa COPERTI
                                    if ((iNumCoperti > 0) && (IsBitSet(SF_Data.iReceiptCopyOptions, BIT_EXTEND_PLACESETTINGS_PRINT_REQUIRED) ||
                                        (k == (int)DEST_TYPE.DEST_TIPO1))) // COPERTI sis tampano assieme a DEST_TYPE.DEST_TIPO1
                                    {
                                        sTmp = String.Format(sRCP_FMT_CPY, iNumCoperti, _COPERTO);
                                        fPrintParam.WriteLine("{0}", sTmp);
                                        iEqRowsNumber++;

                                        fPrintParam.WriteLine();
                                        iEqRowsNumber++;

                                        iNumCoperti = 0; // fa percorre questo blocco solo una volta per stampara i COPERTI
                                    }

                                    for (j = 0; j < MAX_NUM_ARTICOLI; j++)
                                    {
                                        if ((dataIdParam.Articolo[j].iQuantitaOrdine > 0) && (dataIdParam.Articolo[j].iGruppoStampa == k))
                                        {
                                            fPrintParam.WriteLine(sRCP_FMT_CPY, dataIdParam.Articolo[j].iQuantitaOrdine, dataIdParam.Articolo[j].sTipo);
                                            iEqRowsNumber++;

                                            if (!String.IsNullOrEmpty(dataIdParam.Articolo[j].sNotaArt))
                                            {
                                                sTmp = String.Format(" " + sRCP_FMT_NOTE + "\r\n", dataIdParam.Articolo[j].sNotaArt);
                                                fPrintParam.WriteLine("{0}", sTmp);
                                                iEqRowsNumber++;
                                            }
                                        }
                                    }

                                    if (bSomethingInto_GrpToPrint[k])
                                    {
                                        fPrintParam.WriteLine();
                                        iEqRowsNumber++;
                                    }
                                }
                            }
                        }

                        if (!String.IsNullOrEmpty(dataIdParam.sNota))
                        {
                            sTmp = CenterJustify(sConst_Nota[0], MAX_RECEIPT_CHARS_CPY);
                            fPrintParam.WriteLine("{0}", sTmp);

                            sTmp = CenterJustify(dataIdParam.sNota, MAX_RECEIPT_CHARS_CPY);
                            fPrintParam.WriteLine("{0}", sTmp);

                            sTmp = CenterJustify(sConst_Nota[1], MAX_RECEIPT_CHARS_CPY);
                            fPrintParam.WriteLine("{0}", sTmp); fPrintParam.WriteLine();
                            iEqRowsNumber += 4;
                        }

                        if (IsBitSet(dataIdParam.iStatusReceipt, BIT_ESPORTAZIONE))
                        {
                            sTmp = CenterJustify(sConst_Esportazione[0], MAX_RECEIPT_CHARS_CPY);
                            fPrintParam.WriteLine("{0}", sTmp);

                            sTmp = CenterJustify(sConst_Esportazione[1], MAX_RECEIPT_CHARS_CPY);
                            fPrintParam.WriteLine("{0}", sTmp);

                            sTmp = CenterJustify(sConst_Esportazione[2], MAX_RECEIPT_CHARS_CPY);
                            fPrintParam.WriteLine("{0}", sTmp); fPrintParam.WriteLine();
                            iEqRowsNumber += 4;
                        }
                        else
                        {
                            sTmp = CenterJustify("---", MAX_RECEIPT_CHARS_CPY);
                            fPrintParam.WriteLine("{0}", sTmp);
                            iEqRowsNumber++;
                        }

                        // inserimento numero minimo di righe per garantire lunghezza minima
                        do
                        {
                            fPrintParam.WriteLine();
                            iEqRowsNumber++;
                        }
                        while (iEqRowsNumber < MIN_RECEIPT_ROWS_NUMBER);

                        fPrintParam.Close();
                    }

#if STANDFACILE

                    /*****************************************************************************
                     *  MESSA IN CODA DI STAMPA
                     *  iQueueAction == PRINT_ENQUEUE mette in coda senza iniziare
                     *  la stampa ; serve ad evitare problemi di Shared data
                     *  durante _AUTO_SEQ_TEST richiede tempo e provoca disallineamenti
                     *****************************************************************************/
                    if (dataIdParam.bCopiesGroupsFlag[i] && !CheckService(Define._AUTO_SEQ_TEST) && Equals(dataIdParam, SF_Data))
                    {
                        if (PrintNetCopiesConfigDlg.GetPrinterTypeIsWinwows(i))
                            Printer_Windows.PrintFile(sDirParam + sNomeFileCopiePrt, sGlbWinPrinterParams, i);
                        else
                            Printer_Legacy.PrintFile(sDirParam + sNomeFileCopiePrt, sGlbLegacyPrinterParams, (int)PRINT_QUEUE_ACTION.PRINT_ENQUEUE);
                    }
#endif
                } // end if
            }
        }

        /// <summary>
        /// funzione che scrive il Footer nel file fPrintParam,<br/>
        /// prelevando i dati dalla struct dataIdParam
        /// </summary>
        public static void WriteFooter(TData dataIdParam, StreamWriter fPrintParam)
        {
            String sTmp;

            fPrintParam.WriteLine();

            if (!String.IsNullOrEmpty(dataIdParam.sNota))
            {
                sTmp = CenterJustify(sConst_Nota[0], MAX_RECEIPT_CHARS_CPY);
                fPrintParam.WriteLine("{0}", sTmp);

                sTmp = CenterJustify(dataIdParam.sNota, MAX_RECEIPT_CHARS_CPY);
                fPrintParam.WriteLine("{0}", sTmp);

                sTmp = CenterJustify(sConst_Nota[1], MAX_RECEIPT_CHARS_CPY);
                fPrintParam.WriteLine("{0}", sTmp); fPrintParam.WriteLine();
            }

            if (IsBitSet(dataIdParam.iStatusReceipt, BIT_ESPORTAZIONE))
            {
                sTmp = CenterJustify(sConst_Esportazione[0], MAX_RECEIPT_CHARS_CPY);
                fPrintParam.WriteLine("{0}", sTmp);
                sTmp = CenterJustify(sConst_Esportazione[1], MAX_RECEIPT_CHARS_CPY);
                fPrintParam.WriteLine("{0}", sTmp);
                sTmp = CenterJustify(sConst_Esportazione[2], MAX_RECEIPT_CHARS_CPY);
                fPrintParam.WriteLine("{0}\r\n", sTmp);
            }

            if (dataIdParam.bPrevendita)
            {
                sTmp = CenterJustify(sConst_Prevendita[0], MAX_RECEIPT_CHARS_CPY);
                fPrintParam.WriteLine("{0}", sTmp);
                sTmp = CenterJustify(sConst_Prevendita[1], MAX_RECEIPT_CHARS_CPY);
                fPrintParam.WriteLine("{0}", sTmp);
                sTmp = CenterJustify(sConst_Prevendita[2], MAX_RECEIPT_CHARS_CPY);
                fPrintParam.WriteLine("{0}\r\n", sTmp);
            }

            if (!String.IsNullOrEmpty(dataIdParam.sHeaders[2]))
            {
                sTmp = CenterJustify(dataIdParam.sHeaders[2], MAX_RECEIPT_CHARS_CPY);
                fPrintParam.WriteLine("{0}", sTmp); fPrintParam.WriteLine();
            }

            if (!String.IsNullOrEmpty(dataIdParam.sHeaders[3]))
            {
                sTmp = CenterJustify(dataIdParam.sHeaders[3], MAX_RECEIPT_CHARS_CPY);
                fPrintParam.WriteLine("{0}", sTmp); fPrintParam.WriteLine();
            }

            return;
        }

    } // end class
} // end namespace

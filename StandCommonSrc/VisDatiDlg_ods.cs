/********************************************************************
    NomeFile : StandCommonSrc/VisDatiDlg_ods.cs
    Data	 : 30.08.2025
    Autore : Mauro Artuso

    https://www.nuget.org/packages/FreeDataExports

    Classe di visualizzazione dei files Dati o Prezzi
 ********************************************************************/
using System;
using System.IO;
using System.Windows.Forms;

using static StandCommonFiles.ComDef;
using static StandCommonFiles.CommonCl;
using static StandCommonFiles.LogServer;

using static StandFacile.glb;
using static StandFacile.dBaseIntf;

using FreeDataExports;

namespace StandFacile
{
    /// <summary>
    /// classe di visualizzazione dei dati, utilizza quasi sempre DB_Data,
    /// tranne dove è richiesto SF_Data.iNumCassa
    /// </summary>
    public partial class VisDatiDlg : Form
    {
#pragma warning disable IDE0060

        /// <summary>
        /// Esportazione dei dati di riepilogo giornaliero
        /// </summary>
        private void freeExport(bool bIsXlsParam)
        {
            bool bMatch;
            int i, j, iRow, iLastItemRow, iColumn, iUpperLimit;
            int iDebugPrezzo, iDebugQty;
            String sNomeFile, sDataDir, sTmp, sDisp;
            String sDebugTipo;
            DateTime columnDay;

            BtnExport.Enabled = false;
            IDataWorkbook ods_WorkBook = new DataExport().CreateODSv1_3();
            IDataWorkbook xls_WorkBook = new DataExport().CreateXLSX2019();

            IDataWorksheet odsWorkSheet, xlsWorkSheet;

            String[,] sDataArray = new string[420, 100];

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

            LogToFile("FrmVisDati : ods", true);

            try
            {
#if STANDFACILE
                sDataDir = DataManager.GetDataDir() + "\\";
#else
                sDataDir = sRootDir + "\\";
#endif
                xls_WorkBook.CreatedBy = "StandFacile.org";
                ods_WorkBook.CreatedBy = "StandFacile.org";

                if (CkBoxUnioneCasse.Checked)
                {
                    sNomeFile = "Dati_";
                    xlsWorkSheet = xls_WorkBook.AddWorksheet("Dati_");
                    odsWorkSheet = ods_WorkBook.AddWorksheet("Dati_");
                }
                else
                {
                    sNomeFile = String.Format("Dati_FreeExp_C{0}_", _iNumCassa);
                    xlsWorkSheet = xls_WorkBook.AddWorksheet(String.Format("Dati_C{0}", _iNumCassa));
                    odsWorkSheet = ods_WorkBook.AddWorksheet(String.Format("Dati_C{0}", _iNumCassa));
                }

                if (_SelRange == null)
                    _SelRange = new SelectionRange(GetActualDate(), GetActualDate());

                // se _sNomeTabella == "" selezionato un range di date, altrimenti si arriva da EsploraDB_Dlg
                if (String.IsNullOrEmpty(_sNomeTabella))
                {
                    if (_SelRange.Start == _SelRange.End)
                    {
                        _SelRange = new SelectionRange(_SelDate, _SelDate);

                        if (bIsXlsParam)
                            sNomeFile += _SelDate.ToString("yyMMdd'.xlsx'");
                        else
                            sNomeFile += _SelDate.ToString("yyMMdd'.ods'");
                    }
                    else
                    {
                        if (bIsXlsParam)
                            sNomeFile += (_SelRange.Start.ToString("yyMMdd") + _SelRange.End.ToString("_yyMMdd'.xlsx'"));
                        else
                            sNomeFile += (_SelRange.Start.ToString("yyMMdd") + _SelRange.End.ToString("_yyMMdd'.ods'"));
                    }
                }
                else
                {
                    if (bIsXlsParam)
                        sNomeFile = _sNomeTabella + ".xlsx";
                    else
                        sNomeFile = _sNomeTabella + ".ods";
                }

                if (File.Exists(sDataDir + sNomeFile))
                    File.Delete(sDataDir + sNomeFile);

                _iLastArticoloIndexP1 = MAX_NUM_ARTICOLI; // successivamente potrebbe incrementare

                iRow = 0;
                iLastItemRow = 0;

                j = 0;
                iColumn = 1;

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
                            sDataArray[iRow++, iColumn + 1] = sTmp;

                            sDataArray[iRow++, iColumn + 1] = Text.Trim(); // Caption

                            iRow++;

                            sDataArray[iRow++, iColumn + 1] = DB_Data.sHeaders[0];
                            sDataArray[iRow++, iColumn + 1] = DB_Data.sHeaders[1];

                            sDataArray[iRow++, iColumn + 1] = "Num. Scontrini emessi";

                            sDataArray[iRow++, iColumn + 1] = "Num. Scontrini Web";

                            //if (DB_Data.iNumAnnullati > 0)
                            sDataArray[iRow++, iColumn + 1] = "Num. Scontr. annullati e valore";
                            //else
                            //    sDataArray[iRow++, iColumn + 1] = "Num. Scontr. annullati";

                            // recupera l'ora dalla CASSA_PRINCIPALE
                            sDataArray[iRow++, iColumn + 1] = DB_Data.sDateTime;
                            sDataArray[iRow, iColumn + 1] = "articolo";
                            sDataArray[iRow + 1, iColumn + 2] = "prz. unitario";

                        } // end if
                        else
                            iRow = XLS_VOFFSET - 2; // rimpicciolisce se si aggiungono righe di informazioni

                        sDataArray[iRow - 4, iColumn + 3] = DB_Data.iActualNumOfReceipts.ToString();

                        sDataArray[iRow - 3, iColumn + 3] = DB_Data.iNumOfWebReceipts.ToString();

                        if (DB_Data.iNumAnnullati > 0)
                        {
                            sDataArray[iRow - 2, iColumn + 3] = DB_Data.iNumAnnullati.ToString();
                            sDataArray[iRow - 2, iColumn + 4] = IntToEuro(DB_Data.iTotaleAnnullato);
                        }
                        else
                        {
                            sDataArray[iRow - 2, iColumn + 3] = DB_Data.iNumAnnullati.ToString();
                        }

                        sDataArray[iRow - 1, iColumn + 3] = DB_Data.sDateTime.Trim().Substring(4, 8);

                        sDataArray[iRow, iColumn + 3] = "quant. venduta";

                        if (_SelRange.Start == _SelRange.End)
                            sDataArray[iRow, iColumn + 5] = "dispon.";

                        iRow++;

                        sDataArray[iRow, iColumn + 4] = "parziale";

                        if (!CheckBoxRidColonne.Checked && (_SelRange.Start == _SelRange.End))
                            sDataArray[iRow, iColumn + 6] = "quant.esportazione";

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

                                    if (DB_Data.Articolo[i].iGruppoStampa == (int)DEST_TYPE.DEST_BUONI)
                                        sDataArray[XLS_VOFFSET + j, 1] = _ArticoliPrimaColonna[j].sTipo + " (*)";
                                    else
                                        sDataArray[XLS_VOFFSET + j, 1] = _ArticoliPrimaColonna[j].sTipo;

                                    sDataArray[XLS_VOFFSET + j, 2] = IntToEuro(_ArticoliPrimaColonna[j].iPrezzoUnitario);
                                }

                                sDataArray[XLS_VOFFSET + j, iColumn + 3] = DB_Data.Articolo[i].iQuantitaVenduta.ToString();

                                if (DB_Data.Articolo[i].iGruppoStampa == (int)DEST_TYPE.DEST_COUNTER)
                                {
                                    if (DB_Data.Articolo[i].sTipo == _COPERTO)
                                        sDataArray[XLS_VOFFSET + j, iColumn + 4] = IntToEuro(DB_Data.Articolo[i].iPrezzoUnitario * DB_Data.Articolo[i].iQuantitaVenduta);
                                    else
                                        sDataArray[XLS_VOFFSET + j, iColumn + 4] = "0,00";
                                }
                                else
                                {
                                    if (DB_Data.Articolo[i].iGruppoStampa == (int)DEST_TYPE.DEST_BUONI)
                                        sDataArray[XLS_VOFFSET + j, iColumn + 4] = IntToEuro(-DB_Data.Articolo[i].iPrezzoUnitario * DB_Data.Articolo[i].iQuantitaVenduta);
                                    else
                                        sDataArray[XLS_VOFFSET + j, iColumn + 4] = IntToEuro(DB_Data.Articolo[i].iPrezzoUnitario * DB_Data.Articolo[i].iQuantitaVenduta);
                                }

                                if (_SelRange.Start == _SelRange.End)
                                    sDataArray[XLS_VOFFSET + j, iColumn + 5] = sDisp;

                                if (!CheckBoxRidColonne.Checked && (_SelRange.Start == _SelRange.End))
                                    sDataArray[XLS_VOFFSET + j, iColumn + 6] = DB_Data.Articolo[i].iQtaEsportata.ToString();
                            }
                        }

                        // verifica lo storico e prosegue con iRow
                        if (iLastItemRow < (XLS_VOFFSET + _iLastArticoloIndexP1))
                            iLastItemRow = XLS_VOFFSET + _iLastArticoloIndexP1;

                        iRow = iLastItemRow;

                        // questa parte va scritta solo al secondo passaggio
                        if (iRepeatForLayout == 1)
                        {
                            sDataArray[iRow++, iColumn + 4] = "---------";

                            sDataArray[iRow, 3] = "TOTALE";
                            sDataArray[iRow++, iColumn + 4] = IntToEuro(DB_Data.iTotaleIncasso - DB_Data.iTotaleBuoniApplicati);

                            //if (DB_Data.iTotaleBuoniApplicati > 0)
                            {
                                iRow++;

                                sDataArray[iRow, 3] = "valore effettivo buoni applicati (*)";
                                sDataArray[iRow++, iColumn + 4] = IntToEuro(DB_Data.iTotaleBuoniApplicati);
                            }

                            //if ((DB_Data.iTotaleScontatoStd > 0) || (DB_Data.iTotaleScontatoFisso > 0) || (DB_Data.iTotaleScontatoGratis > 0))
                            {
                                iRow++;

                                sDataArray[iRow, 3] = "valore gratuiti";
                                sDataArray[iRow++, iColumn + 4] = IntToEuro(DB_Data.iTotaleScontatoGratis);

                                sDataArray[iRow, 3] = "valore sconto fisso";
                                sDataArray[iRow++, iColumn + 4] = IntToEuro(DB_Data.iTotaleScontatoFisso);

                                sDataArray[iRow, 3] = "valore sconto articoli";
                                sDataArray[iRow++, iColumn + 4] = IntToEuro(DB_Data.iTotaleScontatoStd);

                                sDataArray[iRow++, iColumn + 4] = "---------";
                                sDataArray[iRow, 3] = "TOTALE NETTO";
                                sDataArray[iRow++, iColumn + 4] = IntToEuro(DB_Data.iTotaleIncasso - DB_Data.iTotaleBuoniApplicati - DB_Data.iTotaleScontatoStd -
                                                                            DB_Data.iTotaleScontatoFisso - DB_Data.iTotaleScontatoGratis);
                            }

                            //if ((DB_Data.iTotaleIncassoCard > 0) || (DB_Data.iTotaleIncassoSatispay > 0))
                            {
                                iRow++;

                                sDataArray[iRow, 3] = "PAGAM. CARD";
                                sDataArray[iRow++, iColumn + 4] = IntToEuro(DB_Data.iTotaleIncassoCard);

                                sDataArray[iRow, 3] = "PAGAM. SATISPAY";
                                sDataArray[iRow++, iColumn + 4] = IntToEuro(DB_Data.iTotaleIncassoSatispay);

                                sDataArray[iRow, 3] = "PAGAM. CONT.";
                                sDataArray[iRow++, iColumn + 4] = IntToEuro(DB_Data.iTotaleIncasso - DB_Data.iTotaleBuoniApplicati - DB_Data.iTotaleScontatoStd - DB_Data.iTotaleScontatoFisso -
                                                                            DB_Data.iTotaleScontatoGratis - DB_Data.iTotaleIncassoCard - DB_Data.iTotaleIncassoSatispay);
                            }
                        }

                        // aggiornamento indici
                        columnDay = columnDay.AddDays(+1);
                        iColumn += 2;

                    } // end while (columnDay <= _SelRange.End)
                } // for ... iRepeatForLayout


                // predisposizione tabella esportazione e formattazione

                const int iPRE_COL_TEXT = 12; // posizione di inizio dati fascia centrale
                const int iPOST_COL_TEXT = 20;

                String sCellContent = "";

                // Array per formattazione larghezza delle colonne ma solo per xls
                String[] sXlsColumnWidth = new String[100];

                for (i = 0; i < 100; i += 2)
                {
                    sXlsColumnWidth[i] = "10";
                    sXlsColumnWidth[i + 1] = "20";
                }

                sXlsColumnWidth[1] = "36"; // Articoli
                sXlsColumnWidth[2] = "14"; // prz. unitario

                // Array per formattazione larghezza delle colonne ma solo per ODS
                String[] sOdsColumnWidth = new String[100];

                for (i = 0; i < 100; i += 2)
                {
                    sOdsColumnWidth[i] = "16mm";
                    sOdsColumnWidth[i + 1] = "26mm";
                }

                sOdsColumnWidth[1] = "60mm"; // Articoli
                sOdsColumnWidth[2] = "24mm"; // prz. unitario
                sOdsColumnWidth[3] = "55mm"; // quant. venduta

                try
                {
                    for (i = 0; i < _iLastArticoloIndexP1 + iPRE_COL_TEXT + iPOST_COL_TEXT; i++)
                    {
                        for (j = 0; j < 100; j++)
                        {
                            sCellContent = sDataArray[i, j];

                            // dati prima di iPRE_COL_TEXT
                            if ((i < iPRE_COL_TEXT - 2) && (j > 2))
                            {
                                xlsWorkSheet.AddCell(sCellContent, DataType.Number);
                                odsWorkSheet.AddCell(sCellContent, DataType.Number);
                            }

                            // dati fascia centrale esportazione
                            else if ((i > iPRE_COL_TEXT) && (i <= _iLastArticoloIndexP1 + iPRE_COL_TEXT))
                            {
                                if ((j == 0) || (j == 1) || ((j == 5) && sCellContent == "OK"))
                                {
                                    xlsWorkSheet.AddCell(sCellContent + "", DataType.String);
                                    odsWorkSheet.AddCell(sCellContent + "", DataType.String);
                                }
                                else
                                {
                                    xlsWorkSheet.AddCell(sCellContent, DataType.Number);
                                    odsWorkSheet.AddCell(sCellContent, DataType.Number);
                                }
                            }

                            // dati finali esportazione
                            else if ((i > _iLastArticoloIndexP1 + iPRE_COL_TEXT) && (j >= 4))
                            {
                                if ((sCellContent == null) || sCellContent.Contains("---"))
                                {
                                    xlsWorkSheet.AddCell(sCellContent + "", DataType.String);
                                    odsWorkSheet.AddCell(sCellContent + "", DataType.String);
                                }
                                else
                                {
                                    xlsWorkSheet.AddCell(sCellContent, DataType.Number);
                                    odsWorkSheet.AddCell(sCellContent, DataType.Number);
                                }
                            }
                            else
                            {
                                xlsWorkSheet.AddCell(sCellContent + "", DataType.String);
                                odsWorkSheet.AddCell(sCellContent + "", DataType.String);
                            }
                        }

                        xlsWorkSheet.ColumnWidths(sXlsColumnWidth);
                        xlsWorkSheet.AddRow();

                        odsWorkSheet.ColumnWidths(sOdsColumnWidth);
                        odsWorkSheet.AddRow();
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("i,j,sDataArray[i, j]: {0}, {1}, {2}", i, j, sDataArray[i, j]);
                }

                sTmp = ods_WorkBook.GetErrors();

                LogToFile("VisDatiDlg : EsportaDati");

                if (bIsXlsParam)
                    xls_WorkBook.Save(sDataDir + sNomeFile);
                else
                    ods_WorkBook.Save(sDataDir + sNomeFile);

                if (CheckBoxExport.Checked)
                    System.Diagnostics.Process.Start(sDataDir + sNomeFile);

                BtnExport.Enabled = true;
            }
            catch (Exception)
            {
                BtnExport.Enabled = true;
                _WrnMsg.iErrID = WRN_EXN;
                WarningManager(_WrnMsg);
            }
        }

    }
}

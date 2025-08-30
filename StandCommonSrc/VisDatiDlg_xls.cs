/********************************************************************
    NomeFile : StandCommonSrc/VisDatiDlg_xls.cs
    Data	 : 30.08.2025
    Autore : Mauro Artuso

    Classe di visualizzazione dei files Dati o Prezzi
 ********************************************************************/
using System;
using System.IO;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Forms;
using static StandCommonFiles.ComDef;
using static StandCommonFiles.CommonCl;
using static StandCommonFiles.LogServer;
using static StandFacile.dBaseIntf;
using static StandFacile.glb;

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
        private void xls_Export()
        {
            bool bMatch;
            int i, j, iRow, iLastItemRow, iColumn, iUpperLimit;
            int iDebugPrezzo, iDebugQty;
            String sNomeFile, sDataDir, sTmp, sDisp;
            String sDebugTipo;
            DateTime columnDay;

            BtnExport.Enabled = false;

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

                                    if (DB_Data.Articolo[i].iGruppoStampa == (int)DEST_TYPE.DEST_BUONI)
                                        xlsWorkSheet.Cells[XLS_VOFFSET + j, 2] = _ArticoliPrimaColonna[j].sTipo + " (*)";
                                    else
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
                                }
                                else
                                {
                                    if (DB_Data.Articolo[i].iGruppoStampa == (int)DEST_TYPE.DEST_BUONI)
                                        xlsWorkSheet.Cells[XLS_VOFFSET + j, iColumn + 5] = (-DB_Data.Articolo[i].iPrezzoUnitario * DB_Data.Articolo[i].iQuantitaVenduta) / 100.0f;
                                    else
                                        xlsWorkSheet.Cells[XLS_VOFFSET + j, iColumn + 5] = (DB_Data.Articolo[i].iPrezzoUnitario * DB_Data.Articolo[i].iQuantitaVenduta) / 100.0f;

                                }

                                if (_SelRange.Start == _SelRange.End)
                                    xlsWorkSheet.Cells[XLS_VOFFSET + j, iColumn + 6] = sDisp;

                                if (!CheckBoxRidColonne.Checked && (_SelRange.Start == _SelRange.End))
                                    xlsWorkSheet.Cells[XLS_VOFFSET + j, iColumn + 7] = DB_Data.Articolo[i].iQtaEsportata;
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
                            xlsWorkSheet.Cells[iRow++, iColumn + 5] = (DB_Data.iTotaleIncasso - DB_Data.iTotaleBuoniApplicati) / 100.0f;

                            //if (DB_Data.iTotaleBuoniApplicati > 0)
                            {
                                iRow++;

                                xlsWorkSheet.Cells[iRow, 4] = "valore effettivo buoni applicati (*)";
                                xlsWorkSheet.Cells[iRow++, iColumn + 5] = (DB_Data.iTotaleBuoniApplicati) / 100.0f;
                            }

                            //if ((DB_Data.iTotaleScontatoStd > 0) || (DB_Data.iTotaleScontatoFisso > 0) || (DB_Data.iTotaleScontatoGratis > 0))
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
                                xlsWorkSheet.Cells[iRow++, iColumn + 5] = (DB_Data.iTotaleIncasso - DB_Data.iTotaleBuoniApplicati - DB_Data.iTotaleScontatoStd -
                                                                            DB_Data.iTotaleScontatoFisso - DB_Data.iTotaleScontatoGratis) / 100.0f;
                            }

                            //if ((DB_Data.iTotaleIncassoCard > 0) || (DB_Data.iTotaleIncassoSatispay > 0))
                            {
                                iRow++;

                                xlsWorkSheet.Cells[iRow, 4] = "PAGAM. CARD";
                                xlsWorkSheet.Cells[iRow++, iColumn + 5] = (DB_Data.iTotaleIncassoCard) / 100.0f;

                                xlsWorkSheet.Cells[iRow, 4] = "PAGAM. SATISPAY";
                                xlsWorkSheet.Cells[iRow++, iColumn + 5] = (DB_Data.iTotaleIncassoSatispay) / 100.0f;

                                xlsWorkSheet.Cells[iRow, 4] = "PAGAM. CONT.";
                                xlsWorkSheet.Cells[iRow++, iColumn + 5] = (DB_Data.iTotaleIncasso - DB_Data.iTotaleBuoniApplicati - DB_Data.iTotaleScontatoStd - DB_Data.iTotaleScontatoFisso -
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


                if (CheckBoxExport.Checked)
                    System.Diagnostics.Process.Start(sDataDir + sNomeFile);

                Marshal.ReleaseComObject(xlsWorkSheet);
                Marshal.ReleaseComObject(xlsWorkBook);
                Marshal.ReleaseComObject(xlApp);

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

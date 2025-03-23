/***********************************************
  	NomeFile : StandFacile/MainForm.cs
    Data	 : 06.12.2024
  	Autore   : Mauro Artuso
 ***********************************************/

// #define FONT_CHECK

using System;
using System.Drawing;
using System.Windows.Forms;

using static StandFacile.glb;
using static StandFacile.Define;
using static StandFacile.dBaseIntf;

using static StandCommonFiles.ComDef;
using static StandCommonFiles.CommonCl;
using static StandCommonFiles.LogServer;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace StandFacile
{

    /// <summary>
    /// form principale
    /// </summary>
    public partial class FrmMain : Form
    {
        /// <summary>
        /// abilita/disabilita le varie voci del Menù Principale
        /// </summary>
        void CheckMenuItems()
        {
            // bottoni e/o menù rilevanti
            if (MnuModDispArticoli.Checked)
            {
                // Menù File e Stampa
                MnuStampaDiProva.Enabled = false;
                MnuStampaFile.Enabled = false;
                MnuEsportaListino.Enabled = false;
                MnuImportaListino.Enabled = false;
                MnuChiudiIncasso.Enabled = false;

                // Menù Modifica
                MnuModifica.Enabled = true;
                MnuModDispArticoli.Enabled = true;
                MnuAnnulloOrdine.Enabled = false;
                MnuChangePayment.Enabled = false;

                // Menù Visualizza
                MnuVisualizza.Enabled = false;

                // Menù Impostazioni
                MnuImpostazioni.Enabled = false;

                // Bottoni
                BtnVisListino.Enabled = false;
                BtnSendMsg.Enabled = false;
                BtnX10.Enabled = false;
                BtnEsportazione.Enabled = false;
                BtnSconto.Enabled = false;
                BtnScontrino.Enabled = false;
                BtnDB.Enabled = false;

                // Bottoni Touch
                btnPlus.Enabled = false;
                btnMinus.Enabled = false;
                btnCanc.Enabled = false;

                lblNome.Enabled = false;
                EditNome.Enabled = false;
                lblTavolo.Enabled = false;
                EditTavolo.Enabled = false;
                lblCoperti.Enabled = false;
                EditCoperti.Enabled = false;
                lblNota.Enabled = false;
                EditNota.Enabled = false;
                lblResto.Enabled = false;
                EditResto.Enabled = false;
                lblPagato.Enabled = false;
                EditContante.Enabled = false;

                comboCashPos.Enabled = false;
                EditStatus_QRC.Enabled = false;
                lblRead_bcd.Enabled = false;
            }
            else if (MnuImpListino.Checked)
            {
                // Menù File e Stampa
                MnuStampaDiProva.Enabled = false;
                MnuStampaFile.Enabled = false;
                MnuEsportaListino.Enabled = false;
                MnuImportaListino.Enabled = false;
                MnuChiudiIncasso.Enabled = false;

                // Menù Modifica
                MnuModifica.Enabled = false;
                MnuModDispArticoli.Enabled = false;

                // Menù Visualizza
                MnuVisualizza.Enabled = false;

                // Menù Impostazioni
                MnuImpostazioni.Enabled = true;
                MnuEsperto.Enabled = false;
                MnuImpostaRete.Enabled = false;
                MnuImpostaStampanteWin.Enabled = false;
                MnuImpostaStampanteLegacy.Enabled = false;
                MnuImpostaCopieLocali.Enabled = false;
                MnuImpostaCopieInRete.Enabled = false;
                MnuImpOpzioni.Enabled = false;
                MnuCambiaPassword.Enabled = false;
                MnuImpHeader.Enabled = false;
                MnuImpTabsGrid.Enabled = false;
                MnuImpListino.Enabled = true;

                // Bottoni
                BtnVisListino.Enabled = false;
                //VisListino.Checked = false;

                BtnSendMsg.Enabled = false;
                BtnX10.Enabled = false;
                BtnEsportazione.Enabled = false;
                BtnSconto.Enabled = false;
                BtnScontrino.Enabled = false;
                BtnDB.Enabled = false;

                sStatusText = "Modifica Listino : Esc per uscire";

                // Bottoni Touch
                btnPlus.Enabled = false;
                btnMinus.Enabled = false;
                btnCanc.Enabled = false;

                lblNome.Enabled = false;
                EditNome.Enabled = false;
                lblTavolo.Enabled = false;
                EditTavolo.Enabled = false;
                lblCoperti.Enabled = false;
                EditCoperti.Enabled = false;
                lblNota.Enabled = false;
                EditNota.Enabled = false;
                lblResto.Enabled = false;
                EditResto.Enabled = false;
                lblPagato.Enabled = false;
                EditContante.Enabled = false;

                comboCashPos.Enabled = false;
                EditStatus_QRC.Enabled = false;
                lblRead_bcd.Enabled = false;
            }
            // con BtnVisListino->Enabled = false -->  BtnVisListino->Down == true sempre
            else if (BtnVisListino.Checked && BtnVisListino.Enabled)
            {
                // Menù File e Stampa
                MnuStampaDiProva.Enabled = false;
                MnuStampaFile.Enabled = false;
                MnuEsportaListino.Enabled = false;
                MnuImportaListino.Enabled = false;
                MnuChiudiIncasso.Enabled = false;

                // Menù Modifica
                MnuModifica.Enabled = false;

                // Menù Visualizza
                MnuVisualizza.Enabled = false;

                // Menù Impostazioni
                MnuImpostazioni.Enabled = false;

                // Bottoni
                // BtnVisListino.Enabled = false;
                BtnSendMsg.Enabled = false;
                BtnX10.Enabled = false;
                BtnEsportazione.Enabled = false;
                BtnSconto.Enabled = false;
                BtnScontrino.Enabled = false;
                BtnDB.Enabled = false;

                // Bottoni Touch
                btnPlus.Enabled = false;
                btnMinus.Enabled = false;
                btnCanc.Enabled = false;

                lblNome.Enabled = false;
                EditNome.Enabled = false;
                lblTavolo.Enabled = false;
                EditTavolo.Enabled = false;
                lblCoperti.Enabled = false;
                EditCoperti.Enabled = false;
                lblNota.Enabled = false;
                EditNota.Enabled = false;
                lblResto.Enabled = false;
                EditResto.Enabled = false;
                lblPagato.Enabled = false;
                EditContante.Enabled = false;

                comboCashPos.Enabled = false;
                EditStatus_QRC.Enabled = false;
                lblRead_bcd.Enabled = false;
            }
            else
            {
                /*******************************************
                        nessun bottone o menù rilevante            
                 *******************************************/

                // più sopra poteva essere disabilitato
                MnuEsperto.Enabled = true;

                // Bottoni Touch
                btnPlus.Enabled = true;
                btnMinus.Enabled = true;
                btnCanc.Enabled = true;

                MainGrid.Enabled = true; // sicurezza

                lblNome.Enabled = true;
                EditNome.Enabled = true;
                lblTavolo.Enabled = true;
                EditTavolo.Enabled = true;
                lblCoperti.Enabled = true;
                EditCoperti.Enabled = true;
                lblNota.Enabled = true;
                EditNota.Enabled = true;
                lblResto.Enabled = true;
                EditResto.Enabled = true;
                lblPagato.Enabled = true;
                EditContante.Enabled = true;

                comboCashPos.Enabled = true;

                // per sicurezza accettazione BarCode non con la prevendita in corso
                if (SF_Data.bPrevendita)
                {
                    EditStatus_QRC.Enabled = false;
                    lblRead_bcd.Enabled = false;
                }
                else
                {
                    EditStatus_QRC.Enabled = true;
                    lblRead_bcd.Enabled = true;
                }

                if (MnuEsperto.Checked)
                {
                    // Menù File e Stampa

                    if (DataManager.CheckIf_CassaSec_and_NDB()) // cassa secondaria e DB
                    {
                        MnuEsportaListino.Enabled = false;
                        MnuImportaListino.Enabled = false;
                        MnuChiudiIncasso.Enabled = false;
                        MnuImpHeader.Enabled = false;
                        MnuImpTabsGrid.Enabled = false;

                        if (SF_Data.bPrevendita)
                            MnuImpOpzioni.Enabled = false;
                        else
                            // per poter eliminare il Flag
                            MnuImpOpzioni.Enabled = true;

                        MnuImpListino.Enabled = false;
                    }
                    else
                    {
                        MnuEsportaListino.Enabled = true;
                        MnuImportaListino.Enabled = true;
                        // altrimenti con la Prevendita è troppo complessa la gestione dei nomi delle tabelle
                        MnuChiudiIncasso.Enabled = !SF_Data.bPrevendita;
                        MnuImpHeader.Enabled = true;
                        MnuImpTabsGrid.Enabled = true;
                        MnuImpOpzioni.Enabled = true;
                        MnuImpListino.Enabled = true;
                    }

                    // Menù Modifica
                    // Menù Visualizza
                    MnuVisIncassoOggi.Enabled = true;
                    MnuVisIncassoAltraData.Enabled = true;
                    MnuEsploraDB.Enabled = true;

                    // Menù Impostazioni
                    MnuCambiaPassword.Enabled = true;
                    MnuImpostaRete.Enabled = true;
                    MnuImpostaStampanteWin.Enabled = true;
                    MnuImpostaStampanteLegacy.Enabled = true;
                    MnuImpostaCopieLocali.Enabled = true;
                    MnuImpostaCopieInRete.Enabled = true;

                }
                else // non Esperto
                {
                    // Menù File e Stampa
                    MnuEsportaListino.Enabled = false;
                    MnuImportaListino.Enabled = false;
                    MnuChiudiIncasso.Enabled = false;

                    // Menù Modifica
                    // Menù Visualizza
                    if (SF_Data.bRiservatezzaRichiesta)
                    {
                        MnuVisIncassoOggi.Enabled = false;
                        MnuVisIncassoAltraData.Enabled = false;
                        MnuEsploraDB.Enabled = false;
                        MnuVisOrdiniAltraData.Enabled = false;
                    }
                    else
                    {
                        MnuVisIncassoOggi.Enabled = true;
                        MnuVisIncassoAltraData.Enabled = true;
                        MnuEsploraDB.Enabled = true;
                        MnuVisOrdiniAltraData.Enabled = true;
                    }

                    // Menù Impostazioni
                    MnuImpostaRete.Enabled = false;
                    MnuImpostaStampanteWin.Enabled = false;
                    MnuImpostaStampanteLegacy.Enabled = false;
                    MnuImpostaCopieLocali.Enabled = false;
                    MnuImpostaCopieInRete.Enabled = false;
                    MnuCambiaPassword.Enabled = false;
                    MnuImpOpzioni.Enabled = false;
                    MnuImpHeader.Enabled = false;
                    MnuImpTabsGrid.Enabled = false;
                    MnuImpListino.Enabled = false;
                }

                // cassa secondaria e DB
                if (DataManager.CheckIf_CassaSec_and_NDB())
                    MnuModDispArticoli.Enabled = false;
                else
                    MnuModDispArticoli.Enabled = true;


                // tutti i Menù abilitati
                MnuModifica.Enabled = true;
                MnuVisualizza.Enabled = true;
                MnuImpostazioni.Enabled = true;
                MnuQuickHelp.Enabled = true;

                // Menù File e Stampa
                MnuStampaDiProva.Enabled = true;
                MnuStampaFile.Enabled = true;

                // Menù Modifica
                MnuAnnulloOrdine.Enabled = true;
                MnuChangePayment.Enabled = true;

                // Menù Visualizza
                MnuVisOrdiniOggi.Enabled = true;

                MnuEsploraOrdiniWeb.Enabled = NetConfigDlg.rNetConfigDlg.GetWebOrderEnabled();
                MnuVisListino.Enabled = true;
                MnuReceiptPreview.Enabled = true;

                // più sopra poteva essere disabilitato
                MnuEsperto.Enabled = true;

                // Bottoni
                BtnVisListino.Enabled = true;
                //    altrimenti non si visualizza il listino
                //    BtnVisListino.Checked = false;

                BtnSendMsg.Enabled = true;
                BtnSendMsg.Checked = false;

                BtnX10.Enabled = true;
                BtnX10.Checked = false;

                BtnEsportazione.Enabled = true;

                //BtnEsportazione.Checked = false;
                if (SF_Data.bPrevendita)
                    BtnSconto.Enabled = false;
                else
                    BtnSconto.Enabled = true;

                // ripristina lo stato dopo che si sono consultati i prezzi
                if ((SF_Data.iStatusSconto & 0x000000FF) != 0)
                {
                    BtnSconto.Checked = true;
                    BtnSconto.Image = Properties.Resources.sconto_si;
                }
                else
                {
                    BtnSconto.Checked = false;
                    BtnSconto.Image = Properties.Resources.sconto_no;
                }

                BtnScontrino.Enabled = true;
                BtnScontrino.Checked = false;

                BtnDB.Enabled = true;

                if (_bListinoModificato)
                {
                    DataManager.SalvaListino();

                    DataManager.SalvaDati(); // così si visualizzano prezzi e dati aggiornati
                    SetTabsAppearance();
                }

                sStatusText = "Pronto";
            }

            FormResize(null, null);
            MainGrid_Redraw(this, null);
        }

        /*****************************************
         *       gestione pressione tasti
         *****************************************/
        private void MainGrid_KeyDown(object sender, KeyEventArgs e)
        {
            int i, iD, iU;
            int iKey = (int)e.KeyValue;
            int iPageNumTmp;

            String sTmp;

            if (SF_Data.bTouchMode)
                iPageNumTmp = PAGES_NUM_TABM;
            else
                iPageNumTmp = PAGES_NUM_TXTM;

            if (!MnuImpListino.Checked && !BtnVisListino.Checked)
            {
                // tentativo di gestione qrcode sparato sulla griglia
                scannerInputQueue.Enqueue(iKey);

                // EAN13 contains 13 characters
                if (scannerInputQueue.Count >= 14)
                {
                    EditStatus_QRC.Text = "";
                    DataManager.ClearGrid();

                    while (scannerInputQueue.Count > 0)
                    {
                        i = (int)scannerInputQueue.Dequeue();
                        EditStatus_QRC.Text += Convert.ToChar(i);
                    }

                    // elimina quantità spurie
                    DataManager.ClearGrid();

                    KeyPressEventArgs ek = new KeyPressEventArgs('\r');
                    EditStatus_QRC_KeyPress(EditStatus_QRC, ek);
                }
            }

            ClearBC_FocusTimer();

            // punto unico _iCellPt =
            _iCellPt = MainGrid.CurrentCell.ColumnIndex * MainGrid.RowCount + MainGrid.CurrentCell.RowIndex + iArrayOffset;

            //Console.WriteLine("iKey = {0}, e.Modifiers = {1}, Ctrl = {2}", e.KeyValue, e.Modifiers, e.Control);

            if (e.Modifiers == Keys.Control)
                _bCrtlIsPressed = true;
            else
                _bCrtlIsPressed = false;

            switch (iKey)
            {
                case KEY_HOME:
                    MainGrid.CurrentCell = MainGrid.Rows[0].Cells[0];

                    if (e.Modifiers == Keys.Control)
                        TabSet.SelectedIndex = 0;

                    iKey = KEY_NONE;
                    MainGrid.Focus();
                    break;

                case KEY_END:
                    MainGrid.CurrentCell = MainGrid.Rows[MainGrid.RowCount - 1].Cells[MainGrid.ColumnCount - 1];

                    if (e.Modifiers == Keys.Control)
                        TabSet.SelectedIndex = iPageNumTmp - 1;

                    iKey = KEY_NONE;
                    MainGrid.Focus();
                    break;

                case KEY_ESC:

                    // reset selezioni Menù
                    if (MnuModDispArticoli.Checked)
                    {
                        MnuModDispArticoli.Checked = false;
                        MnuModDispArticoli_Click(this, null);
                    }

                    if (MnuImpListino.Checked)
                    {
                        EditCoperti.Text = _sCopertiPrev;
                        MnuImpListino.Checked = false;
                        MnuImpListino_Click(this, null);
                    }

                    if (BtnVisListino.Checked && BtnVisListino.Enabled)
                    {
                        EditCoperti.Text = _sCopertiPrev;
                        BtnVisListino.Checked = false;
                    }

                    // ci passa se si modifica l'header altrimenti
                    // ci pensa CheckMenuItems();
                    if (_bListinoModificato)
                        DataManager.SalvaListino();

                    // default TAB
                    TabSet.SelectedIndex = 0;
                    MainGrid.Focus();
                    CheckMenuItems();

                    break;

                case KEY_INS:
                case KEY_PLUS:
                case KEY_PLUS_NUM:
                    if (e.Modifiers == Keys.Control)
                    {
                        // inserisce oltre la Pagina corrente, entro l'ultima pagina
                        if (String.IsNullOrEmpty(SF_Data.Articolo[iLastGridIndex * iPageNumTmp - 1].sTipo) && (MnuImpListino.Checked == true))
                        {
                            for (i = iLastGridIndex * iPageNumTmp - 1; i > _iCellPt; i--)
                                SF_Data.Articolo[i] = SF_Data.Articolo[i - 1];

                            SF_Data.Articolo[_iCellPt] = new TArticolo(0);

                            _bListinoModificato = true;
                        }
                    }
                    else
                    {
                        // inserisce entro la Pagina corrente
                        if (String.IsNullOrEmpty(SF_Data.Articolo[iArrayOffset + iLastGridIndex - 1].sTipo) && (MnuImpListino.Checked == true))
                        {
                            for (i = iArrayOffset + iLastGridIndex - 1; i > _iCellPt; i--)
                                SF_Data.Articolo[i] = SF_Data.Articolo[i - 1];

                            SF_Data.Articolo[_iCellPt] = new TArticolo(0);

                            _bListinoModificato = true;
                        }
                    }

                    iKey = KEY_NONE;
                    break;

                case KEY_DEL:
                case KEY_MINUS:
                case KEY_MIN_NUM:

                    if (String.IsNullOrEmpty(SF_Data.Articolo[_iCellPt].sTipo) && (MnuImpListino.Checked == true))
                    {
                        if (e.Modifiers == Keys.Control)
                        {
                            // cancella spostando da oltre la Pagina corrente
                            for (i = _iCellPt; i < iLastGridIndex * iPageNumTmp - 1; i++)
                                SF_Data.Articolo[i] = SF_Data.Articolo[i + 1];

                            SF_Data.Articolo[iLastGridIndex * iPageNumTmp - 1] = new TArticolo(0);
                        }
                        else
                        {
                            // cancella entro la Pagina corrente
                            for (i = _iCellPt; i < iArrayOffset + iLastGridIndex - 1; i++)
                                SF_Data.Articolo[i] = SF_Data.Articolo[i + 1];

                            SF_Data.Articolo[iArrayOffset + iLastGridIndex - 1] = new TArticolo(0);
                        }

                        _bListinoModificato = true;
                        iKey = KEY_NONE;
                    }
                    else if ((!MnuImpListino.Checked) && (!MnuModDispArticoli.Checked) && (iKey != KEY_MIN_NUM))
                    {
                        // così poi aggiorna il TP Totale Parziale
                        iKey = '0';
                    }

                    break;

                case KEY_F1:
                    EditTavolo.Focus();
                    break;
                case KEY_F2:
                    EditCoperti.Focus();
                    break;
                case KEY_F3:
                    EditNota.Focus();
                    break;
                case KEY_F4:
                    EditContante.Focus();
                    break;

                case KEY_F5:
                    BtnVisListino_Click(this, null);
                    break;
                case KEY_F6:
                    BtnSendMsg_Click(this, null);
                    break;
                case KEY_F7:
                    BtnX10.Checked = !BtnX10.Checked;
                    break;
                case KEY_F8:
                    BtnEsportazione.Checked = !BtnEsportazione.Checked;
                    BtnEsportazione_Click(this, null);
                    break;
                case KEY_F9:
                    BtnSconto_Click(this, null);
                    break;
                case KEY_F10:
                    BtnScontrino_Click(this, null);
                    break;
                case KEY_ENTER:
                    // viene processato come Keys.Enter per evitare Key_Down
                    //if (OptionsDlg._rOptionsDlg.GetEnterPrintReceipt())
                    //{
                    //    BtnScontrino_Click(this, null);
                    //}
                    break;

                case '0':
                case KEYNUMPAD0:
                    if (e.Modifiers == Keys.Control)
                    {
                        sTmp = String.Format("FrmMain : premuto 0 + = {0}", e.Modifiers);
                        LogToFile(sTmp);

                        // Ctrl + 0
                        SetEditStatus_BC("");

                        DataManager.ClearGrid();
                        AnteprimaDlg.rAnteprimaDlg.RedrawReceipt();
                    }
                    break;

                // stampa copia locale scontrino normale
                case 'A':
                    if (e.Modifiers == Keys.Control)
                        SF_Data.Articolo[_iCellPt].iOptionsFlags = ClearBit(SF_Data.Articolo[_iCellPt].iOptionsFlags, BIT_STAMPA_SINGOLA_NELLA_COPIA_RECEIPT);
                    break;

                case 'E':
                    if (MnuEsperto.Checked && (e.Modifiers == (Keys.Alt | Keys.Control)))
                    {
                        sTmp = String.Format("FrmMain : premuto E + {0}", e.Modifiers);
                        LogToFile(sTmp);

                        // Ctrl + Alt + E
                        EraseAllaData();
                        return; //  altrimenti il successivo FormResize() genera eccezioni
                    }
                    break;

                case 'C':
                    if (e.Modifiers == Keys.Control)
                    {
                        comboCashPos.SelectedIndex = (comboCashPos.SelectedIndex + 1) % (sConst_PaymentType.Length - 1);
                        scannerInputQueue.Clear();
                    }
                    AnteprimaDlg.rAnteprimaDlg.RedrawReceipt();
                    break;

                case 'P':
                    if (e.Modifiers == Keys.Control)
                    {
                        MnuAnteprimaScontrino_Click(null, null);
                        scannerInputQueue.Clear();
                    }
                    break;

                // stampa copia locale scontrino singola
                case 'S':
                    if ((e.Modifiers == Keys.Control) && PrintReceiptConfigDlg.GetTicketNoPriceCopy())
                        SF_Data.Articolo[_iCellPt].iOptionsFlags = SetBit(SF_Data.Articolo[_iCellPt].iOptionsFlags, BIT_STAMPA_SINGOLA_NELLA_COPIA_RECEIPT);
                    break;

                default:
                    break;
            } // end switch

            // gestione Keypad numerico
            if ((iKey >= KEYNUMPAD0) && (iKey <= KEYNUMPAD9))
            {
                iKey -= 48;
            }

            /************************************************************************************
             *  non si è in modo modifica disponibilità, non si è in modo modifica del Listino,
             *  non si stanno consultando i prezzi
             ************************************************************************************/
            if ((iKey >= '0') && (iKey <= '9') && !String.IsNullOrEmpty(SF_Data.Articolo[_iCellPt].sTipo) &&
                !MnuImpListino.Checked &&       // non si è in modo modifica del Listino
                !BtnVisListino.Checked &&       // non si stanno consultando i prezzi
                !MnuModDispArticoli.Checked)    // non si sta modificando la disponibilità
            {              
                if (SF_Data.Articolo[_iCellPt].iDisponibilita != 0) //
                {
                    if (BtnX10.Checked) // Q.tà ordinazioni a 2 cifre
                    {
                        iD = (SF_Data.Articolo[_iCellPt].iQuantitaOrdine) % 10;
                        iU = (iKey - '0');
                        SF_Data.Articolo[_iCellPt].iQuantitaOrdine = iD * 10 + iU;

                        BtnX10.Checked = false;
                        sStatusText = "Pronto";
                    }
                    else
                    {
                        iU = (iKey - '0');
                        SF_Data.Articolo[_iCellPt].iQuantitaOrdine = iU;
                    }
                }
                else // azzeramento consentito per ordini web non possibili
                {
                    iU = (iKey - '0');

                    if (iU == 0)
                        SF_Data.Articolo[_iCellPt].iQuantitaOrdine = 0;
                }

                AnteprimaDlg.rAnteprimaDlg.RedrawReceipt();
                _iAnteprimaTotParziale = AnteprimaDlg.GetTotaleReceipt();
            }
            // per sicurezza si azzerano in caso numeri di fuori griglia
            else if ((iKey == '0') && String.IsNullOrEmpty(SF_Data.Articolo[_iCellPt].sTipo) &&
                !MnuImpListino.Checked &&      // non si è in modo modifica del Listino
                !BtnVisListino.Checked &&      // non si stanno consultando i prezzi
                !MnuModDispArticoli.Checked)   // non si sta modificando la disponibilità
            {
                SF_Data.Articolo[_iCellPt].iQuantitaOrdine = 0;

                AnteprimaDlg.rAnteprimaDlg.RedrawReceipt();
                _iAnteprimaTotParziale = AnteprimaDlg.GetTotaleReceipt();
            }

            if (MnuImpListino.Checked)
                FormResize(this, null);

            if (SF_Data.Articolo[_iCellPt].iQuantitaOrdine == 0)
            {
                // reset EditNota
                EditNota.BackColor = Color.Gainsboro;
                SF_Data.Articolo[_iCellPt].sNotaArt = "";

                MainGrid_Redraw(this, null);
            }

            MainGrid_Redraw(this, null);
        }

        private void MainGrid_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control)
                _bCrtlIsPressed = true;
            else
                _bCrtlIsPressed = false;

            if ((EditNota.BackColor == Color.LightBlue) && !_bCrtlIsPressed)
            {
                // reset EditNota
                EditNota.BackColor = Color.Gainsboro;
                EditNota.Text = _sEditNotaCopy;
                EditNota.MaxLength = 28;
                lblNota.Text = "Nota:";

                MainGrid_Redraw(this, null);
            }

            Console.WriteLine("MainGrid_KeyUp: {0}", _bCrtlIsPressed);
        }

        private void MainGrid_Resize(object sender, EventArgs e)
        {
            Console.WriteLine("MainGrid_Resize");
            FormResize(sender, null);
        }

        /********************************************************
         *	chiama il dialogo di modifica del record TArticolo
         ********************************************************/
        private void MainGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // punto unico _iPt_DoubleClick =
            _iCellPt = e.ColumnIndex * MainGrid.RowCount + e.RowIndex + iArrayOffset;

            if (MnuModDispArticoli.Checked)
            {
                EditDispArticoliDlg rDispDlg = new EditDispArticoliDlg();
                rDispDlg.Init(_iCellPt);

                FormResize(null, null);
                MainGrid_Redraw(this, null);
                rDispDlg.Dispose();
            }
            else if (MnuImpListino.Checked)
            {
                EditArticoloDlg rModificaArticoloDlg = new EditArticoloDlg();
                rModificaArticoloDlg.Init(_iCellPt, true);

                _bListinoModificato |= EditArticoloDlg.GetListinoModificato();

                EditCoperti.Text = String.Format("{0,4:0.00}", SF_Data.Articolo[MAX_NUM_ARTICOLI - 1].iPrezzoUnitario / 100.0f);

                CheckMenuItems();
                rModificaArticoloDlg.Dispose();
            }

            bSkipDrag = true; // evitare un BeginDrag sul doppio click
        }

        /*****************************
         *     Inizio di un Drag
         *****************************/
        private void MainGrid_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            // il click del mouse annulla il conteggio per barcode scanner detect
            scannerInputQueue.Clear();

            iSwapStartIndex = e.ColumnIndex * MainGrid.RowCount + e.RowIndex + iArrayOffset;

            if (MnuImpListino.Checked && (e.Button == MouseButtons.Left) && !bSkipDrag && !bMouseWrongPos)
            {
                bStartDrag = true;
                Cursor = Cursors.Hand;
            }

            bSkipDrag = false;
        }

        /*******************************************************
         *	Fine di un Drag, si molla il tasto Left del mouse
         *******************************************************/
        private void MainGrid_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            int m, n, iLastArticoloPageIndex;
            int ToCellX, ToCellY;
            TArticolo tmpArticolo;

            // altrimenti può rimanere true
            bPageDxProximity = false;
            bPageSxProximity = false;

            if (MnuImpListino.Checked && (e.Button == MouseButtons.Left) && bStartDrag)
            {
                ToCellX = e.ColumnIndex;
                ToCellY = e.RowIndex;

                m = ToCellX * MainGrid.RowCount + ToCellY + iArrayOffset;
                n = iSwapStartIndex;

                if (SF_Data.bTouchMode)
                    iLastArticoloPageIndex = SF_Data.iGridRows * SF_Data.iGridCols * PAGES_NUM_TABM;
                else
                    iLastArticoloPageIndex = SF_Data.iGridRows * SF_Data.iGridCols * PAGES_NUM_TXTM;

                // controllo per evitare sforamenti del vettore
                if ((m < iLastArticoloPageIndex) && (n < iLastArticoloPageIndex) && (n != m))
                {
                    tmpArticolo = SF_Data.Articolo[m];

                    SF_Data.Articolo[m] = SF_Data.Articolo[n];
                    SF_Data.Articolo[n] = tmpArticolo;
                    _bListinoModificato = true;
                }

                bStartDrag = false;
                Cursor = Cursors.Default;

                DataManager.CheckLastArticoloIndexP1();

                FormResize(null, null);
                MainGrid_Redraw(this, null);
            }
        }

        // il mouse pointer è dentro MainGrid
        private void MainGrid_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ((e.ColumnIndex >= 0) && (e.ColumnIndex < MainGrid.ColumnCount) && (e.RowIndex >= 0) && (e.RowIndex < MainGrid.RowCount) &&
                (e.Button != MouseButtons.Left))
            {
                Cursor = Cursors.Default;
            }

            bMouseWrongPos = false; // il mouse pointer è dentro MainGrid
        }

        // il mouse pointer è fuori MainGrid, non funziona
        private void MainGrid_DragLeave(object sender, EventArgs e)
        {
            if (MnuImpListino.Checked)
            {
                Cursor = Cursors.No;
                bMouseWrongPos = true; // il mouse pointer non è in MainGrid
            }
        }

        // simula MainGridDragOver verifica la posizione rispetto ai bordi
        private void MainGrid_MouseMove(object sender, MouseEventArgs e)
        {
            bool bModArtVisibile;

            if (EditArticoloDlg.rModificaArticoloDlg != null)
                bModArtVisibile = EditArticoloDlg.rModificaArticoloDlg.Visible;
            else
                bModArtVisibile = false;

            if (((e.Y < 5) || ((MainGrid.Height - e.Y) < 5)) && MnuImpListino.Checked && !bModArtVisibile && bStartDrag)
            {
                // esce da MainGrid
                Cursor = Cursors.No;
                bPageDxProximity = false;
                bPageSxProximity = false;
            }
            else if (((MainGrid.Width - e.X) < 50) && MnuImpListino.Checked && !bModArtVisibile && bStartDrag)
            {
                // si sposta a destra
                MainGrid.Cursor = Cursors.AppStarting;
                bPageDxProximity = true;
            }
            else if ((e.X < 50) && MnuImpListino.Checked && !bModArtVisibile && bStartDrag)
            {
                // si sposta a sinistra
                MainGrid.Cursor = Cursors.AppStarting;
                bPageSxProximity = true;
            }
            else if (bStartDrag && !bMouseWrongPos)
            {
                Cursor = Cursors.Hand;
                bPageDxProximity = false;
                bPageSxProximity = false;
            }
            else if (bMouseWrongPos)
            {
                Cursor = Cursors.No;
                bPageDxProximity = false;
                bPageSxProximity = false;
            }
            else
            {
                Cursor = Cursors.Default;
                bPageDxProximity = false;
                bPageSxProximity = false;
            }

            if (BtnVisListino.Checked && BtnVisListino.Enabled)
                _iVisPrzTimeout = TIMEOUT_VIS_PREZZI;
        }

        /// <summary>
        /// special processing for KEY_TAB, KEY_UP, KEY_DOWN, KEY_LEFT, KEY_RIGHT
        /// </summary>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            int iPageNumTmp, iRow, iCol;

            if (SF_Data.bTouchMode)
                iPageNumTmp = PAGES_NUM_TABM;
            else
                iPageNumTmp = PAGES_NUM_TXTM;

            // gestione TABS
            if ((MainGrid.Focused || TabSet.Focused) && (keyData == (Keys.Tab | Keys.Control))) // indietro
            {
                TabSet.SelectedIndex = (TabSet.SelectedIndex + iPageNumTmp - 1) % iPageNumTmp;
                return true;
            }
            else if ((MainGrid.Focused || TabSet.Focused) && (keyData == Keys.Tab)) // avanti
            {
                TabSet.SelectedIndex = (TabSet.SelectedIndex + 1) % iPageNumTmp;
                return true;
            }
            else if ((MainGrid.Focused || TabSet.Focused) && (
                (keyData == Keys.Up) || (keyData == Keys.Down) || (keyData == Keys.Left) || (keyData == Keys.Right) ||
                (keyData == Keys.Home) || (keyData == Keys.End) || (keyData == Keys.Escape)))
            {
                // il movimento con frecce annulla il conteggio per barcode scanner detect
                scannerInputQueue.Clear();

                MainGrid.Focus();

                if (VerificaQuantita())
                    return base.ProcessCmdKey(ref msg, keyData);
                else
                    return true; // cursore rimane sul posto
            }
            else if ((MainGrid.Focused || TabSet.Focused) && (keyData == Keys.Enter))
            {
                if (OptionsDlg._rOptionsDlg.GetEnterPrintReceipt())
                    BtnScontrino_Click(this, null);

                return true; // cursore rimane sul posto
            }
            else if (EditNota.Focused && (keyData == Keys.Enter))
            {
                if (EditNota.BackColor == Color.LightBlue)
                {
                    // acquisisce Nota Articolo
                    SF_Data.Articolo[_iCellPt].sNotaArt = EditNota.Text;

                    EditNota.BackColor = Color.Gainsboro;
                    EditNota.Text = _sEditNotaCopy;
                    EditNota.MaxLength = 28;
                    lblNota.Text = "Nota:";

                    MainGrid_Redraw(this, null);
                }

                return true; // cursore rimane sul posto
            }
            // gestione scorrimento UP nella matrice
            else if (MainGrid.Focused && MnuImpListino.Checked && (keyData == (Keys.Up | Keys.Control)))
            {
                _iCellPt = MainGrid.CurrentCell.ColumnIndex * MainGrid.RowCount + MainGrid.CurrentCell.RowIndex + iArrayOffset;
                String sTime = DateTime.Now.ToString("HH.mm.ss");

                iRow = _iCellPt % MainGrid.RowCount;
                iCol = (_iCellPt % (MainGrid.RowCount * MainGrid.ColumnCount)) / MainGrid.RowCount;

                Console.WriteLine("ProcessCmdKey1: KEY_UP _iCellPt = {0}, {1}, {2}, {3}", _iCellPt, iRow, iCol, sTime);

                if (_iCellPt > 0)
                {
                    Console.WriteLine("ProcessCmdKey2: KEY_UP");

                    TArticolo tmpArticolo = new TArticolo(SF_Data.Articolo[_iCellPt - 1]);

                    SF_Data.Articolo[_iCellPt - 1] = SF_Data.Articolo[_iCellPt];
                    SF_Data.Articolo[_iCellPt] = tmpArticolo;

                    // siamo lungo il bordo superiore, cambio colonna
                    if (iRow == 0)
                    {
                        Console.WriteLine("ProcessCmdKey3: KEY_UP cambio colonna");

                        if (iCol > 0)
                            MainGrid.CurrentCell = MainGrid.Rows[MainGrid.RowCount - 1].Cells[iCol - 1];
                        else
                            MainGrid.CurrentCell = MainGrid.Rows[MainGrid.RowCount - 1].Cells[MainGrid.ColumnCount - 1];

                        SelectTab(_iCellPt - 1);
                        MainGrid.Focus();
                    }
                    else
                        MainGrid.CurrentCell = MainGrid.Rows[iRow - 1].Cells[iCol];

                    _bListinoModificato = true;

                    FormResize(null, null);
                    MainGrid_Redraw(this, null);

                    return true;
                }
            }
            // gestione scorrimento DOWN nella matrice
            else if (MainGrid.Focused && MnuImpListino.Checked && (keyData == (Keys.Down | Keys.Control)))
            {
                _iCellPt = MainGrid.CurrentCell.ColumnIndex * MainGrid.RowCount + MainGrid.CurrentCell.RowIndex + iArrayOffset;
                String sTime = DateTime.Now.ToString("HH.mm.ss");

                iRow = _iCellPt % MainGrid.RowCount;
                iCol = (_iCellPt % (MainGrid.RowCount * MainGrid.ColumnCount)) / MainGrid.RowCount;

                Console.WriteLine("ProcessCmdKey1: KEY_DOWN _iCellPt = {0}, {1}, {2}, {3}", _iCellPt, iRow, iCol, sTime);

                if (_iCellPt < (iLastGridIndex * iPageNumTmp - 1))
                {
                    Console.WriteLine("ProcessCmdKey2: KEY_DOWN");

                    TArticolo tmpArticolo = new TArticolo(SF_Data.Articolo[_iCellPt + 1]);

                    SF_Data.Articolo[_iCellPt + 1] = SF_Data.Articolo[_iCellPt];
                    SF_Data.Articolo[_iCellPt] = tmpArticolo;

                    // siamo lungo il bordo superiore, cambio colonna
                    if (iRow == MainGrid.RowCount - 1)
                    {
                        Console.WriteLine("ProcessCmdKey3: KEY_DOWN cambio colonna");

                        if (iCol < (MainGrid.ColumnCount - 1))
                            MainGrid.CurrentCell = MainGrid.Rows[0].Cells[iCol + 1];
                        else
                            MainGrid.CurrentCell = MainGrid.Rows[0].Cells[0];

                        SelectTab(_iCellPt + 1);
                        MainGrid.Focus();
                    }
                    else
                        MainGrid.CurrentCell = MainGrid.Rows[iRow + 1].Cells[iCol];

                    _bListinoModificato = true;

                    FormResize(null, null);
                    MainGrid_Redraw(this, null);

                    return true;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void EditStatus_QRC_KeyPress(object sender, KeyPressEventArgs e)
        {
            bool bResult;
            int iNumScontrino, iGruppo, iLength;
            String sLog, sStrBarcode, sStrNum, sStrDay, sStrGruppo;
            String JSON_Type = "";

            //Dictionary<int, object> dict = null;

            iFocus_BC_Timeout = BC_FOCUS_TIMEOUT;

            // EditStatus_BC.UseSystemPasswordChar = true; si perde il carattere successivo

            /*************************
                      ENTER
             *************************/
            if (e.KeyChar == '\r')
            {
                // scarta edit vuoti
                if (String.IsNullOrEmpty(EditStatus_QRC.Text))
                    return;
                else
                {
                    sStrBarcode = EditStatus_QRC.Text;
                    sStrBarcode = sStrBarcode.Trim();

                    iLength = sStrBarcode.Length;

                    try
                    {
                        Dictionary<String, object> dict = null;

                        try
                        {
                            // ottiene la tabella dell'output JSON
                            var jss = new JavaScriptSerializer();
                            string sTmpJson = "";

                            // correzione tastiera Italiana 142, QRcode non produce le parentesi !
                            if ((sStrBarcode[0] != '{') && sStrBarcode.Contains(_JS_ORDER_V5))
                                sTmpJson = '{' + sStrBarcode + '}';
                            else
                                sTmpJson = sStrBarcode;

                            dict = jss.Deserialize<dynamic>(sTmpJson);

                            JSON_Type = dict["-10"].ToString();
                        }
                        catch (Exception)
                        {
                        }

                        if (JSON_Type == _JS_ORDER_V5)
                        {
                            dbAzzeraDatiOrdine(ref DB_Data);

                            DB_Data.iNumOrdineWeb = Convert.ToInt32(dict["-9"]);

                            if (dict.ContainsKey("-8"))
                                DB_Data.bAnnullato = (Convert.ToInt32(dict["-8"]) > 0);

                            if (dict.ContainsKey("-7"))
                                DB_Data.bStampato = (Convert.ToInt32(dict["-7"]) > 0);

                            DB_Data.iNumCassa = Convert.ToInt32(dict["-6"]);
                            DB_Data.iStatusReceipt = Convert.ToInt32(dict["-5"]);

                            if (dict.ContainsKey("-4"))
                                DB_Data.sNome = dict["-4"].ToString();

                            if (dict.ContainsKey("-3"))
                                DB_Data.sTavolo = dict["-3"].ToString();

                            if (dict.ContainsKey("-2"))
                                DB_Data.sNota = dict["-2"].ToString();

                            DB_Data.sPL_Checksum = dict["-1"].ToString();

                            // sparato il QRCode di test
                            if (DB_Data.sPL_Checksum == QRC_TEST_CKS)
                            {
                                if (sStrBarcode == QRC_TEST)
                                {
                                    WrnMsg.sMsg = QRC_TEST;
                                    WrnMsg.iErrID = WRN_TQR;

                                    WarningManager(WrnMsg);

                                    bResult = false;
                                    EditStatus_QRC.Text = ""; // pulizia
                                    return;
                                }
                            }

                            int iIndex, iQuantitaOrdine;

                            foreach (var item in dict)
                            {
                                if (Convert.ToInt32(item.Key) >= 0)
                                {
                                    iIndex = Convert.ToInt32(item.Key);
                                    iQuantitaOrdine = Convert.ToInt32(item.Value);

                                    DB_Data.Articolo[iIndex].iQuantitaOrdine = iQuantitaOrdine;
                                }
                            }

                            if (DB_Data.bAnnullato || DB_Data.bStampato)
                                bResult = false;
                            else
                            {
                                sLog = String.Format("Mainform : CKW = {0}, {1}", DataManager.GetWebListinoChecksum(), sStrBarcode);
                                LogToFile(sLog);

                                // ************* caricamento in SF_Data e MainForm *************
                                bResult = DataManager.CaricaOrdine_QR_code();
                            }

                            if (!bResult)
                                EditStatus_QRC.Text = ""; // pulizia

                            sLog = String.Format("Mainform : QR_code = {0}, {1}", DB_Data.iNumOrdineWeb, bResult);
                            LogToFile(sLog);

                            if (CheckService(Define._AUTO_QRCODE_TEST))
                            {
                                // emissione scontrino
                                BtnScontrino_Click(this, null);
                                MainGrid_Redraw(this, null);
                            }
                        }

                        // sparato barcode con checksum corretto
                        else if (StandCommonFiles.Barcode_EAN13.VerifyChecksum(sStrBarcode))
                        {
                            // lo spazio è solo per allineamento
                            sStrNum = sStrBarcode.Substring(8, 4); // numero Scontrino

                            // ribaltamento formato data
                            sStrDay = sStrBarcode.Substring(6, 2) + sStrBarcode.Substring(4, 2) + sStrBarcode.Substring(2, 2);

                            sStrGruppo = sStrBarcode.Substring(0, 2);       // Gruppo

                            iNumScontrino = Convert.ToInt32(sStrNum);
                            iGruppo = Convert.ToInt32(sStrGruppo);

                            if (iGruppo == NUM_PRE_SALE_GRP) // sicurezza
                            {
                                _sOrdiniPrevDBTable = String.Format("{0}_{1}", _dbPreOrdersTablePrefix, sStrDay);
                                bResult = DataManager.CaricaOrdinePrev(iNumScontrino, _sOrdiniPrevDBTable);

                                if (!bResult)
                                    EditStatus_QRC.Text = ""; // pulizia

                                sLog = String.Format("Mainform : barcode prevendita = {0}, {1}, {2}", _sOrdiniPrevDBTable, sStrNum, bResult);
                                LogToFile(sLog);
                            }

                            if (CheckService(Define._AUTO_QRCODE_TEST))
                            {
                                // emissione scontrino
                                BtnScontrino_Click(this, null);
                                MainGrid_Redraw(this, null);
                            }
                        }
                        else
                        {
                            EditStatus_QRC.Text = "";
                            return;
                        }

                    }
                    catch (Exception)
                    {
                        EditStatus_QRC.Text = "";
                        WarningManager(WRN_QRE);
                    }

                    // EditStatus_BC.Text = ""; // è la stampa scontrino che farà pulizia !
                    return;
                }
            }
        }

        /// <summary>
        /// calcola la dimensione delle celle della griglia
        /// e ridisegna il contenuto della griglia
        /// </summary>
        public void FormResize(object sender, EventArgs e)
        {
            int i, j, k, h;
            int iRowsHeight, iPrimoGruppoStampa, iGruppoStampa;
            String sText;

            float fColumnsWidth, fTextSize;

            // Toolbar
            TabSet.Width = lblNome.Location.X - 10;

            // tutto dipende dal topPanel
            topPanel.Width = this.Size.Width - 22;

            if (SF_Data.bTouchMode) // priorità
            {
                TabSet.Height = 34;
                TabSet.ItemSize = new Size(67, TabSet.Height - 3);

                EditCoperti.Font = new Font("Microsoft Sans Serif", 15);

                BtnScontrino.Size = new Size(80, 60);

                topPanel.Height = 60;

                toolStripR.Visible = true;
                toolStripR.Enabled = true;
                toolStripR.Width = 82;

                EditCoperti.Left = topPanel.Width - toolStripR.Width / 2 - EditCoperti.Width / 2 - 2;

                btnSep_R1.Height = toolStripR.Height / 10;
                btnSep_R2.Height = toolStripR.Height / 12;
                btnSep_R3.Height = btnSep_R2.Height;
            }
            else if (OptionsDlg._rOptionsDlg.Get_VButtons())
            {
                TabSet.Height = 26;
                TabSet.ItemSize = new Size(67, TabSet.Height - 3);

                EditCoperti.Font = new Font("Microsoft Sans Serif", 12);

                BtnScontrino.Size = new Size(45, 38);

                topPanel.Height = 42;

                toolStripR.Visible = true;
                toolStripR.Enabled = true;
                toolStripR.Width = 82;

                EditCoperti.Left = topPanel.Width - toolStripR.Width / 2 - EditCoperti.Width / 2 - 2;

                btnSep_R1.Height = toolStripR.Height / 10;
                btnSep_R2.Height = toolStripR.Height / 12;
                btnSep_R3.Height = btnSep_R2.Height;
            }
            else
            {
                TabSet.Height = 26;
                TabSet.ItemSize = new Size(67, TabSet.Height - 3);

                EditCoperti.Font = new Font("Microsoft Sans Serif", 12);

                BtnScontrino.Size = new Size(45, 38);

                topPanel.Height = 42;

                toolStripR.Visible = false;
                toolStripR.Enabled = false;
                toolStripR.Width = 2;

                EditCoperti.Left = topPanel.Width - 75;
            }

            // tutto dipende dal topPanel
            toolStrip.Width = topPanel.Width - toolStrip.Left - 80; // era 150
            toolStrip.Height = topPanel.Height - 2;

            // posizionamento verticale elementi
            TabSet.Top = MainMenu.Height + topPanel.Height + topPanel.Margin.Top + topPanel.Margin.Bottom;

            EditTavolo.Top = TabSet.Top;
            EditCoperti.Top = TabSet.Top;
            EditNome.Top = TabSet.Top;

            EditTavolo.Font = EditCoperti.Font;
            EditNome.Font = EditCoperti.Font;

            lblCoperti.Top = TabSet.Top + 2;
            lblTavolo.Top = lblCoperti.Top;
            lblNome.Top = lblCoperti.Top;

            MainGrid.Top = TabSet.Top + TabSet.Height;
            toolStripR.Top = MainGrid.Top;
            //lblPagato.Top = lblResto.Top; provoca modifica Anchor !

            toolStripR.Left = topPanel.Width - toolStripR.Width - MainGrid.Location.X;

            lblCoperti.Left = EditCoperti.Left - lblCoperti.Width - 2;

            EditTavolo.Left = EditCoperti.Left - EditTavolo.Width - 80;
            lblTavolo.Left = EditTavolo.Left - lblTavolo.Width - 2;

            EditNome.Left = EditTavolo.Left - EditNome.Width - 80;
            lblNome.Left = EditNome.Left - lblNome.Width - 2;

            //lblStatusResto.Left = EditStatusResto.Left - lblStatusResto.Width-2;
            //lblStatusPagato.Left = EditStatusContante.Left - lblStatusPagato.Width-2;

            BtnVisListino.Size = BtnScontrino.Size;
            BtnSendMsg.Size = BtnScontrino.Size;
            BtnX10.Size = BtnScontrino.Size;
            BtnEsportazione.Size = BtnScontrino.Size;
            BtnSconto.Size = BtnScontrino.Size;
            BtnDB.Size = BtnScontrino.Size;

            // imposta la larghezza della griglia in base alla larghezza della form principale
            MainGrid.Width = topPanel.Width - toolStripR.Width - MainGrid.Location.X * 2;

            // imposta l'altezza della griglia in base all'altezza della form principale
            MainGrid.Height = Height - MainMenu.Size.Height - toolStrip.Size.Height - TabSet.Height -
                                StatusBar_Upper.Size.Height - StatusBar.Size.Height - 52;

            // imposta altezza delle righe sulla base della altezza della finestra
            if ((WindowState != FormWindowState.Minimized) && (MainGrid.RowCount > 0))
            {
                iPrimoGruppoStampa = CheckFirstGroupIndex();
                _prevStyle.ForeColor = _gridStyle[_iColorTheme, iPrimoGruppoStampa].ForeColor;
                _prevStyle.BackColor = _gridStyle[_iColorTheme, iPrimoGruppoStampa].BackColor;

                if (MnuImpListino.Checked || BtnVisListino.Checked)
                {
                    if (SF_Data.bTouchMode)
                        sText = sGlbWinPrinterParams.bChars33 ? "12345678901234567890123" : "123456789012345678";
                    else
                        // String.Format(" {0,2} {1,-18} {2,5:0.00}", // width=28
                        sText = sGlbWinPrinterParams.bChars33 ? "123456789012345678901234567809123" : "1234567890123456789012345678";
                }
                else
                {
                    if (SF_Data.bTouchMode)
                        sText = sGlbWinPrinterParams.bChars33 ? "12345678901234567809123" : "123456789012345678";
                    else
                        // String.Format("{0,3} {1,-18} {2,2}" : // width=25
                        sText = sGlbWinPrinterParams.bChars33 ? "123456789012345678901234567890" : "1234567890123456789012345";
                }

                if ((MainGrid.Height % MainGrid.RowCount) < 3)
                    iRowsHeight = (MainGrid.Height / MainGrid.RowCount) - 1; // evita scroll verticale griglia
                else
                    iRowsHeight = MainGrid.Height / MainGrid.RowCount;


                for (i = 0; i < MainGrid.RowCount; i++)
                    MainGrid.Rows[i].Height = iRowsHeight;

                fColumnsWidth = MainGrid.Columns[0].Width;

                fTextSize = TextRenderer.MeasureText(sText, MainGrid.DefaultCellStyle.Font).Width;
                _fFontWidth = fColumnsWidth / sText.Length + 0.5f;

                // avvio form
                if (_fFontWidth < 5.0)
                    _fFontWidth = ((float)iRowsHeight * 0.45f);

                //Console.WriteLine("_fFontWidth = {0}, {1}, {2}, {3}", _fFontWidth, fColumnsWidth, fTextSize, sText.Length);

                MainGrid.Font = new Font(MainGrid.DefaultCellStyle.Font.Name, _fFontWidth);

                // Grid appearance
                MainGrid.GridColor = _gridCrossStyle[_iColorTheme].ForeColor;
                MainGrid.DefaultCellStyle.SelectionForeColor = _selectedCellStyle[_iColorTheme].ForeColor;
                MainGrid.DefaultCellStyle.SelectionBackColor = _selectedCellStyle[_iColorTheme].BackColor;
                MainGrid.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                //sText += (Environment.NewLine + "\r\n --------------");
            }

            // prepara l'aspetto della stringa visualizzata nelle celle
            for (k = 0; ((k < iLastGridIndex) && (MainGrid.RowCount > 0)); k++)
            {
                i = k % MainGrid.RowCount;
                j = k / MainGrid.RowCount;

                h = k + iArrayOffset;

                iGruppoStampa = SF_Data.Articolo[h].iGruppoStampa;

                if (MnuImpListino.Checked || BtnVisListino.Checked)
                {
                    if (String.IsNullOrEmpty(SF_Data.Articolo[h].sTipo))
                    {
                        MainGrid.Rows[i].Cells[j].Style = new DataGridViewCellStyle(_prevStyle);
                    }
                    else
                    {
                        MainGrid.Rows[i].Cells[j].Style.ForeColor = _gridStyle[_iColorTheme, iGruppoStampa].ForeColor;
                        MainGrid.Rows[i].Cells[j].Style.BackColor = _gridStyle[_iColorTheme, iGruppoStampa].BackColor;

                        _prevStyle = new DataGridViewCellStyle(MainGrid.Rows[i].Cells[j].Style);
                        MainGrid.Rows[i].Cells[j].Style.Font = new Font(MainGrid.DefaultCellStyle.Font.Name, _fFontWidth, FontStyle.Regular);
                    }
                }
                else
                {
                    // Disponibilità zero colore rosso
                    if (SF_Data.Articolo[h].iDisponibilita == 0)
                    {
                        MainGrid.Rows[i].Cells[j].Style.ForeColor = _zeroAvailabilityStyle[_iColorTheme].ForeColor;
                        MainGrid.Rows[i].Cells[j].Style.BackColor = _gridStyle[_iColorTheme, iGruppoStampa].BackColor;

                        MainGrid.Rows[i].Cells[j].Style.Font = new Font(MainGrid.DefaultCellStyle.Font.Name, _fFontWidth, FontStyle.Bold);
                    }
                    else if (String.IsNullOrEmpty(SF_Data.Articolo[h].sTipo))
                    {
                        MainGrid.Rows[i].Cells[j].Style = new DataGridViewCellStyle(_prevStyle);
                    }
                    else
                    {
                        MainGrid.Rows[i].Cells[j].Style.ForeColor = _gridStyle[_iColorTheme, iGruppoStampa].ForeColor;
                        MainGrid.Rows[i].Cells[j].Style.BackColor = _gridStyle[_iColorTheme, iGruppoStampa].BackColor;

                        _prevStyle = new DataGridViewCellStyle(MainGrid.Rows[i].Cells[j].Style);

                        MainGrid.Rows[i].Cells[j].Style.Font = new Font(MainGrid.DefaultCellStyle.Font.Name, _fFontWidth, FontStyle.Regular);
                    }
                }

                // Grid appearance: estende il colore dell'angolo in basso a destra a tutta la griglia (in particolare al bordo inferiore)
                if (h == MainGrid.RowCount * MainGrid.ColumnCount + iArrayOffset - 1)
                    MainGrid.BackgroundColor = MainGrid.Rows[i].Cells[j].Style.BackColor;
            }

            //topPanel.Refresh();
            //TabSet.Refresh();
            toolStripR.Refresh();

            MainGrid.Refresh();
        } // end FormResize

        /// <summary>
        /// ridisegna il contenuto della griglia principale
        /// </summary>
        public void MainGrid_Redraw(object sender, EventArgs e)
        {
            int i, j, k, h;
            String sTipoTmp, sTickQty, sDispQty, sPrzTmp, sCellText;
            String sToolTip;

            // prepara la stringa visualizzata nelle celle
            for (k = 0; ((k < iLastGridIndex) && (MainGrid.RowCount > 0)); k++)
            {
                i = k % MainGrid.RowCount;
                j = k / MainGrid.RowCount;

                h = k + iArrayOffset;

                if (!String.IsNullOrEmpty(SF_Data.Articolo[h].sTipo) && IsBitSet(SF_Data.Articolo[h].iOptionsFlags, BIT_STAMPA_SINGOLA_NELLA_COPIA_RECEIPT))
                    sTipoTmp = SF_Data.Articolo[h].sTipo.Insert(0, "$ ");
                else
                    sTipoTmp = SF_Data.Articolo[h].sTipo;

                if (SF_Data.Articolo[h].iQuantitaOrdine != 0)
                    sTickQty = SF_Data.Articolo[h].iQuantitaOrdine.ToString();
                else
                    sTickQty = "";

                if (SF_Data.Articolo[h].iDisponibilita != DISP_OK)
                    sDispQty = SF_Data.Articolo[h].iDisponibilita.ToString();
                else
                    sDispQty = "";

                sCellText = "";

                if ((MnuImpListino.Checked || BtnVisListino.Checked) && !String.IsNullOrEmpty(SF_Data.Articolo[h].sTipo))
                {

                    sPrzTmp = IntToEuro(SF_Data.Articolo[h].iPrezzoUnitario);

                    switch (SF_Data.Articolo[h].iGruppoStampa)
                    {
                        // ******** allineamento *******
                        // 89 123456789012345678 9876.00
                        // fPrint.WriteLine("{0,2} {1,-18} {2,5}", // width=28
                        case (int)DEST_TYPE.DEST_TIPO1:
                            if (SF_Data.bTouchMode)
                                sCellText = String.Format(sGRD_FMT_TCH, "G1", sPrzTmp, Environment.NewLine, CenterJustify(SF_Data.Articolo[h].sTipo, 18));
                            else
                                sCellText = String.Format(sGRD_FMT_STD, "G1", SF_Data.Articolo[h].sTipo, sPrzTmp);
                            break;
                        case (int)DEST_TYPE.DEST_TIPO2:
                            if (SF_Data.bTouchMode)
                                sCellText = String.Format(sGRD_FMT_TCH, "G2", sPrzTmp, Environment.NewLine, CenterJustify(SF_Data.Articolo[h].sTipo, 18));
                            else
                                sCellText = String.Format(sGRD_FMT_STD, "G2", SF_Data.Articolo[h].sTipo, sPrzTmp);
                            break;
                        case (int)DEST_TYPE.DEST_TIPO3:
                            if (SF_Data.bTouchMode)
                                sCellText = String.Format(sGRD_FMT_TCH, "G3", sPrzTmp, Environment.NewLine, CenterJustify(SF_Data.Articolo[h].sTipo, 18));
                            else
                                sCellText = String.Format(sGRD_FMT_STD, "G3", SF_Data.Articolo[h].sTipo, sPrzTmp);
                            break;
                        case (int)DEST_TYPE.DEST_TIPO4:
                            if (SF_Data.bTouchMode)
                                sCellText = String.Format(sGRD_FMT_TCH, "G4", sPrzTmp, Environment.NewLine, CenterJustify(SF_Data.Articolo[h].sTipo, 18));
                            else
                                sCellText = String.Format(sGRD_FMT_STD, "G4", SF_Data.Articolo[h].sTipo, sPrzTmp);
                            break;
                        case (int)DEST_TYPE.DEST_TIPO5:
                            if (SF_Data.bTouchMode)
                                sCellText = String.Format(sGRD_FMT_TCH, "G5", sPrzTmp, Environment.NewLine, CenterJustify(SF_Data.Articolo[h].sTipo, 18));
                            else
                                sCellText = String.Format(sGRD_FMT_STD, "G5", SF_Data.Articolo[h].sTipo, sPrzTmp);
                            break;
                        case (int)DEST_TYPE.DEST_TIPO6:
                            if (SF_Data.bTouchMode)
                                sCellText = String.Format(sGRD_FMT_TCH, "G6", sPrzTmp, Environment.NewLine, CenterJustify(SF_Data.Articolo[h].sTipo, 18));
                            else
                                sCellText = String.Format(sGRD_FMT_STD, "G6", SF_Data.Articolo[h].sTipo, sPrzTmp);
                            break;
                        case (int)DEST_TYPE.DEST_TIPO7:
                            if (SF_Data.bTouchMode)
                                sCellText = String.Format(sGRD_FMT_TCH, "G7", sPrzTmp, Environment.NewLine, CenterJustify(SF_Data.Articolo[h].sTipo, 18));
                            else
                                sCellText = String.Format(sGRD_FMT_STD, "G7", SF_Data.Articolo[h].sTipo, sPrzTmp);
                            break;
                        case (int)DEST_TYPE.DEST_TIPO8:
                            if (SF_Data.bTouchMode)
                                sCellText = String.Format(sGRD_FMT_TCH, "G8", sPrzTmp, Environment.NewLine, CenterJustify(SF_Data.Articolo[h].sTipo, 18));
                            else
                                sCellText = String.Format(sGRD_FMT_STD, "G8", SF_Data.Articolo[h].sTipo, sPrzTmp);
                            break;
                        case (int)DEST_TYPE.DEST_COUNTER:
                            if (SF_Data.bTouchMode)
                                sCellText = String.Format(sGRD_FMT_TCH, "CN", sPrzTmp, Environment.NewLine, CenterJustify(SF_Data.Articolo[h].sTipo, 18));
                            else
                                sCellText = String.Format(sGRD_FMT_STD, "CN", SF_Data.Articolo[h].sTipo, sPrzTmp);
                            break;
                    }

                    // impostazione dei ToolTip di cella
                    if (String.IsNullOrEmpty(SF_Data.Articolo[h].sTipo))
                        sToolTip = "";
                    else
                    {
                        sToolTip = string.Format("Articolo: {0}\n", SF_Data.Articolo[h].sTipo);

                        sToolTip += string.Format("   prezzo: {0}\n", IntToEuro(SF_Data.Articolo[h].iPrezzoUnitario));

                        sToolTip += string.Format("   gruppo stampa: {0}\n", SF_Data.sCopiesGroupsText[SF_Data.Articolo[h].iGruppoStampa]);

                        sToolTip += string.Format("   {0}: {1}\n", "gruppo breve", sConstGruppiShort[SF_Data.Articolo[h].iGruppoStampa]);

                    }

                    MainGrid.Rows[i].Cells[j].ToolTipText = sToolTip;
                }
                else
                {
                    // solo se disponibilità zero colore rosso
                    if (SF_Data.Articolo[h].iDisponibilita == 0)
                    {
                        MainGrid.Rows[i].Cells[j].Style.ForeColor = _zeroAvailabilityStyle[_iColorTheme].ForeColor;
                        MainGrid.Rows[i].Cells[j].Style.BackColor = _gridStyle[_iColorTheme, SF_Data.Articolo[h].iGruppoStampa].BackColor;

                        MainGrid.Rows[i].Cells[j].Style.Font = new Font(MainGrid.DefaultCellStyle.Font.Name, _fFontWidth, FontStyle.Bold);
                    }

                    if (SF_Data.bTouchMode)
                    {
                        if (string.IsNullOrEmpty(sDispQty))
                            sCellText = String.Format(sGRDZ_FMT_TCH, sTickQty, Environment.NewLine, CenterJustify(sTipoTmp, 18));
                        else
                            sCellText = String.Format(sGRDW_FMT_TCH, sDispQty, sTickQty, Environment.NewLine, CenterJustify(sTipoTmp, 18));
                    }
                    else
                        sCellText = String.Format(sGRDW_FMT_STD, sDispQty, sTipoTmp, sTickQty);

                    // impostazione dei ToolTip di cella
                    if (String.IsNullOrEmpty(SF_Data.Articolo[h].sTipo) || String.IsNullOrEmpty(SF_Data.Articolo[h].sNotaArt))
                    {
                        MainGrid.Rows[i].Cells[j].ToolTipText = "";
                    }
                    else
                    {
                        sToolTip = string.Format("Nota: {0}", SF_Data.Articolo[h].sNotaArt);
                        MainGrid.Rows[i].Cells[j].ToolTipText = sToolTip;
                    }
                }

#if FONT_CHECK
                if (!_bPrimaEsecuzione && (((i + j + iArrayOffset) % 6) == 0))
                    MainGrid.Rows[i].Cells[j].Value = sText;
                else
                    MainGrid.Rows[i].Cells[j].Value = sCellText;
#else
                if (!_bPrimaEsecuzione)
                    MainGrid.Rows[i].Cells[j].Value = sCellText;
#endif
            }

            // aggiorna il web checksum in caso di modifica Listino
            Text = String.Format("{0} {1} {2} {1} webcks Listino = {3}   {4}", TITLE, "   ", RELEASE_SW, DataManager.GetWebListinoChecksum(), _sShortDBType);

            MainGrid.Refresh();
        } // end MainGrid_Redraw

    } // end class
} // end namespace



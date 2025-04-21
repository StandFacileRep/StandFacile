/**********************************************************************
    NomeFile : StandFacile/DataManager.cs
	Data	 : 18.04.2025
    Autore   : Mauro Artuso
 **********************************************************************/

using System;
using System.IO;

using static StandFacile.glb;
using static StandFacile.Define;
using static StandFacile.dBaseIntf;
using static StandFacile.ScontoDlg;
using static StandFacile.FrmMain;

using static StandCommonFiles.ComDef;
using static StandCommonFiles.CommonCl;
using static StandCommonFiles.LogServer;

namespace StandFacile
{
    /// <summary>
    ///  Classe da modificare con accuratezza per assicurare la correttezza dei conti economici,<br/>
    ///  contiene i metodi per la gestione dei seguenti files :<br/><br/>
    ///  
    ///  Listino.txt File prezzi<br/>
    ///  Cx_Dati'mmdd'.txt File dati di riepilogo giornaliero<br/>
    ///  Cx_TTyyyy.txt scontrino completo<br/>
    ///  Cx_CTyyyy_Gz.txt copia di riepilogo per gruppi: pietanze, bibite, etc<br/>
    ///  legenda: x è il numero della cassa, mm il mese, dd il giorno,<br/>
    ///          yyyy è un numero progressivo, z il gruppo di articoli<br/>
    /// </summary>
    public partial class DataManager
    {

#pragma warning disable IDE0079
#pragma warning disable IDE0059

        /// <summary>
        /// solo la CASSA_PRINCIPALE salva il file prezzi e la tabella Listino del DB,<br/>
        /// e se non si è in modo _AUTO_SEQ_TEST, resetta il flag _bListinoModificato di MainForm
        /// </summary>
        public static bool SalvaListino()
        {
            bool bListinoIsGood;
            int i, iRowIndex, iEmptyLines;
            uint uWebHashCode = 0, uLocHashCode = 0;
            String sPrzRow, sDebug, sDir, sGroupName;
            StreamWriter fPrz;
            String[] sQueue_Object = new String[2];

            // sicurezza e coerenza con CaricaListino(), altra App lo salva
            if (CheckIf_CassaSec_and_NDB() || CheckService(Define.CFG_SERVICE_STRINGS._AUTO_SEQ_TEST)) // cassa secondaria e DB, seqTest
                return false;

            sDir = _sExeDir + "\\";

            _ErrMsg.sNomeFile = NOME_FILE_LISTINO;

            // esegue il backup del file Prezzi, evita di salvare files incompleti
            if (File.Exists(sDir + NOME_FILE_LISTINO) && _bListinoCaricatoConSuccesso)
            {
                // esegue il backup del file Prezzi
                File.Delete(sDir + NOME_FILE_LISTINO_BK);

                try
                {
                    File.Move(sDir + NOME_FILE_LISTINO, sDir + NOME_FILE_LISTINO_BK);
                }
                catch (IOException)
                {
                    _ErrMsg.iErrID = ERR_FNR;
                    ErrorManager(_ErrMsg);
                }
            }

            // aggiorna per sicurezza il limite Articolo[]
            CheckLastArticoloIndexP1();

            // verifica presenza di almeno un Articolo significativo
            bListinoIsGood = false;
            for (i = 0; i < MAX_NUM_ARTICOLI; i++)
                if (!String.IsNullOrEmpty(SF_Data.Articolo[i].sTipo) && ((SF_Data.Articolo[i].iPrezzoUnitario > 0) || OptionsDlg._rOptionsDlg.GetZeroPriceEnabled()))
                {
                    bListinoIsGood = true;
                    break;
                }

            if (!bListinoIsGood) // ****** inutile proseguire ******
            {
                LogToFile("DataManager : non esegue SalvaListino");
                return false;
            }

            iRowIndex = 1;

            fPrz = File.CreateText(sDir + NOME_FILE_LISTINO);

            if (fPrz == null)
            {
                _ErrMsg.iErrID = ERR_FNO;
                ErrorManager(_ErrMsg);

                return false;
            }
            else
            {
                fPrz.WriteLine(";   StandFacile {0}\n;", RELEASE_SW);

                sPrzRow = String.Format("#DT {0}", GetDateTimeString());

                // sPrzRow = "#DT gio 03/08/24 18.24.00"; // debug

                fPrz.WriteLine(sPrzRow);
                uLocHashCode += Hash(sPrzRow);
                iRowIndex += 3;

                // salva gli Headers
                fPrz.WriteLine(";\n; Intestazioni e pié pagina");
                iRowIndex += 2;

                for (i = 0; i < MAX_NUM_HEADERS; i++)
                {
                    sPrzRow = String.Format("#HD{0} {1}", i, SF_Data.sHeaders[i]);
                    fPrz.WriteLine(sPrzRow);

                    iRowIndex++;
                    uLocHashCode += Hash(sPrzRow);
                }

                // salva tutti i testi delle copie
                fPrz.WriteLine(";\n; descrizioni gruppi copie");
                iRowIndex += 2;

                for (i = 0; i < NUM_EDIT_GROUPS; i++)
                {
                    /**************************************************
                     *   Copie/Text con Flag attivo per StandFacile
                     *   e non per copie stampate da StandCucina
                     *   devono essere salvate tutte
                     **************************************************/

                    if (SF_Data.bCopiesGroupsFlag[i])
                        sPrzRow = String.Format("#GS{0}{1} {2}", i, SF_Data.iGroupsColor[i], SF_Data.sCopiesGroupsText[i]); // Set
                    else
                        sPrzRow = String.Format("#GC{0}{1} {2}", i, SF_Data.iGroupsColor[i], SF_Data.sCopiesGroupsText[i]); // Clear

                    fPrz.WriteLine(sPrzRow);
                    iRowIndex++;
                    uLocHashCode += Hash(sPrzRow);

                    if (i != (int)DEST_TYPE.DEST_TIPO9_NOWEB)
                    {
                        // ritaglia la parte di interesse web
                        sGroupName = sPrzRow[3] + sPrzRow.Substring(5);
                        uWebHashCode += Hash(sGroupName);
                    }

                    Console.WriteLine("DataManager : uLocHashCode = {0}, uWebHashCode = {1}, sPrzRow = {2}", uLocHashCode, uWebHashCode, sPrzRow);
                }

                if (SF_Data.bCopiesGroupsFlag[NUM_EDIT_GROUPS])
                    sPrzRow = String.Format("#GS{0}", NUM_EDIT_GROUPS); // Set
                else
                    sPrzRow = String.Format("#GC{0}", NUM_EDIT_GROUPS); // Clear

                fPrz.WriteLine(sPrzRow);
                iRowIndex++;
                uLocHashCode += Hash(sPrzRow); // no uWebHashCode

                Console.WriteLine("DataManager : uLocHashCode = {0}, uWebHashCode = {1}, sPrzRow = {2}", uLocHashCode, uWebHashCode, sPrzRow);

                // salva tutti i testi dei colori
                fPrz.WriteLine(";\n; descrizioni gruppi per colore");
                iRowIndex += 2;

                for (i = 0; i < NUM_GROUPS_COLORS - 1; i++)
                {
                    sPrzRow = String.Format("#CT{0} {1}", i, SF_Data.sColorGroupsText[i]);

                    fPrz.WriteLine(sPrzRow);
                    iRowIndex++;
                    uLocHashCode += Hash(sPrzRow);
                }

                // sconti
                int iDebug = GetSconto().iStatusSconto & 0x0000FFF0;

                fPrz.WriteLine(";\n; sconti");
                iRowIndex += 2;

                sPrzRow = String.Format("#SC0 {0}; {1:X3}; {2}", GetSconto().iScontoValPerc, (GetSconto().iStatusSconto & 0x0000FFF0) >> 4, GetSconto().sScontoText[(int)DISC_TYPE.DISC_STD]);
                fPrz.WriteLine(sPrzRow);
                uLocHashCode += Hash(sPrzRow);
                //uWebHashCode += Hash(sPrzRow);

                sPrzRow = String.Format("#SC1 {0}; {1}", IntToEuro(GetSconto().iScontoValFisso), GetSconto().sScontoText[(int)DISC_TYPE.DISC_FIXED]);
                fPrz.WriteLine(sPrzRow);
                uLocHashCode += Hash(sPrzRow);
                //uWebHashCode += Hash(sPrzRow);

                sPrzRow = String.Format("#SC2 {0}", GetSconto().sScontoText[(int)DISC_TYPE.DISC_GRATIS]);
                fPrz.WriteLine(sPrzRow);
                uLocHashCode += Hash(sPrzRow);
                //uWebHashCode += Hash(sPrzRow);

                iRowIndex += 3;

                // salva tutte le pagine
                fPrz.WriteLine(";\n; testi TABS della griglia principale");
                iRowIndex += 2;

                for (i = 0; i < PAGES_NUM_TABM; i++)
                {
                    sPrzRow = String.Format("#PN{0} {1}", i, SF_Data.sPageTabs[i]);
                    fPrz.WriteLine(sPrzRow);
                    iRowIndex++;
                    uLocHashCode += Hash(sPrzRow);
                    // sTmpHash.sprintf("0:X8", uLocHashCode);
                    // LogServer.LogToFile("DataManager SalvaListino Hash" + sPrzRow + sTmpHash);
                }

                fPrz.WriteLine(";\n; dimensioni griglia principale");
                iRowIndex += 2;

                sPrzRow = String.Format("#NR{0}", SF_Data.iGridRows);
                fPrz.WriteLine(sPrzRow);
                iRowIndex++;
                uLocHashCode += Hash(sPrzRow);
                // sTmpHash.sprintf("0:X8", uLocHashCode);
                // LogServer.LogToFile("DataManager SalvaListino Hash" + sPrzRow + sTmpHash);

                sPrzRow = String.Format("#NC{0}", SF_Data.iGridCols);
                fPrz.WriteLine(sPrzRow);
                iRowIndex++;
                uLocHashCode += Hash(sPrzRow);
                // sTmpHash.sprintf("0:X8", uLocHashCode);
                // LogServer.LogToFile("DataManager SalvaListino Hash" + sPrzRow + sTmpHash);

                // flag vari
                fPrz.WriteLine(";\n; flag vari");
                iRowIndex += 2;

                // Flag Prevendita richiesto Set/Clear
                if (SF_Data.bPrevendita)
                    sPrzRow = "#PVS"; // Set
                else
                    sPrzRow = "#PVC"; // Clear

                fPrz.WriteLine(sPrzRow);
                iRowIndex++;
                uLocHashCode += Hash(sPrzRow);

                //numero per gestione barcode nelle copie
                sPrzRow = String.Format("{0}{1:X5}", "#BC", SF_Data.iBarcodeRichiesto);

                fPrz.WriteLine(sPrzRow);
                iRowIndex++;
                uLocHashCode += Hash(sPrzRow);

                //numero hex per gestione della opzioni generali
                sPrzRow = String.Format("{0}{1:X5}", "#GO", SF_Data.iGeneralOptions);

                fPrz.WriteLine(sPrzRow);
                iRowIndex++;
                uLocHashCode += Hash(sPrzRow);

                //numero hex per gestione delle opzioni di stampa
                sPrzRow = String.Format("{0}{1:X5}", "#LC", SF_Data.iReceiptCopyOptions);

                fPrz.WriteLine(sPrzRow);
                iRowIndex++;
                uLocHashCode += Hash(sPrzRow);

                fPrz.WriteLine(";");
                fPrz.WriteLine("; Articolo ; prezzo unitario ; Gruppo stampa");
                fPrz.WriteLine(";");
                iRowIndex += 3;

                // salva importo coperti
                sPrzRow = String.Format(sDAT_FMT_PRL, "#PCP COPERTI", IntToEuro(SF_Data.Articolo[MAX_NUM_ARTICOLI - 1].iPrezzoUnitario),
                                    (int)DEST_TYPE.DEST_COUNTER);

                fPrz.WriteLine(sPrzRow);
                iRowIndex++;
                uLocHashCode += Hash(sPrzRow);
                uWebHashCode += Hash(sPrzRow);

                for (i = 0; i < _iLastArticoloIndexP1; i++)
                {
                    if (String.IsNullOrEmpty(SF_Data.Articolo[i].sTipo))
                    {
                        // verifica possibilità di compattazione #LF
                        for (iEmptyLines = 1; ((i + iEmptyLines) < _iLastArticoloIndexP1); iEmptyLines++)
                        {
                            // esce alla prima riga non vuota, ma potrebbe uscire anche a fine array
                            if (!String.IsNullOrEmpty(SF_Data.Articolo[i + iEmptyLines].sTipo))
                                break;
                        }

                        if (iEmptyLines > 2) // compattazione
                        {
                            if ((i + iEmptyLines) < _iLastArticoloIndexP1) // sicurezza
                            {
                                fPrz.WriteLine("");
                                uLocHashCode += Hash(iRowIndex++);
                                // sTmpHash.sprintf("DataManager SalvaListino Hash {0} {1:X8}", sPrzRow.c_str(), uLocHashCode);
                                // LogServer.LogToFile(sTmpHash);

                                sPrzRow = String.Format("#LF{0}", iEmptyLines - 2);
                                fPrz.WriteLine(sPrzRow);
                                iRowIndex++;
                                uLocHashCode += Hash(sPrzRow);
                                // sTmpHash.sprintf("DataManager SalvaListino Hash {0} {1:X8}", sPrzRow.c_str(), uLocHashCode);
                                // LogServer.LogToFile(sTmpHash);

                                fPrz.WriteLine("");
                                uLocHashCode += Hash(iRowIndex++);
                                // sTmpHash.sprintf("DataManager SalvaListino Hash {0} {1:X8}", sPrzRow.c_str(), uLocHashCode);
                                // LogServer.LogToFile(sTmpHash);

                                i += iEmptyLines;
                            }
                        }
                    }

                    if ((SF_Data.Articolo[i].iPrezzoUnitario > 0) ||
                        (!String.IsNullOrEmpty(SF_Data.Articolo[i].sTipo) && OptionsDlg._rOptionsDlg.GetZeroPriceEnabled()) ||
                        (SF_Data.Articolo[i].iGruppoStampa == (int)DEST_TYPE.DEST_COUNTER))
                    {
                        sPrzRow = String.Format(sDAT_FMT_PRL, SF_Data.Articolo[i].sTipo, IntToEuro(SF_Data.Articolo[i].iPrezzoUnitario),
                                    SF_Data.Articolo[i].iGruppoStampa);

                        uLocHashCode += Hash(sPrzRow);

                        if (SF_Data.Articolo[i].iGruppoStampa != (int)DEST_TYPE.DEST_TIPO9_NOWEB)
                            uWebHashCode += Hash(sPrzRow, i); // i per evitare inversioni di righe a parità di uWebHashCode
                        else
                            sDebug = SF_Data.Articolo[i].sTipo;

                        iRowIndex++;
                    }
                    else
                    {
                        uLocHashCode += Hash(iRowIndex++);
                        sPrzRow = String.Format("");
                    }

                    fPrz.WriteLine(sPrzRow);
                }

                fPrz.WriteLine(";\n; checksum locale e web");

                fPrz.WriteLine("#CKL {0:X8}", uLocHashCode);
                fPrz.WriteLine("#CKW {0:X8}", uWebHashCode);

                // aggiorna il valore dopo le modifiche al Listino senza aspettare il riavvio
                _sLocListinoChecksum = String.Format("{0:X8}", uLocHashCode);
                _sWebListinoChecksum = String.Format("{0:X8}", uWebHashCode);

                fPrz.Close();
            }

            LogToFile("DataManager : SalvaListino");

            /****************************************************************
             *  solo la CASSA_PRINCIPALE può riscrivere la tabella Listino
             ****************************************************************/
            if (CheckIf_CassaPri_and_NDB())
            {
                _rdBaseIntf.dbSalvaListino();
            }

            // va chiamata sempre
            _rdBaseIntf.dbUpdateHeadOrdine();

            ClearListinoModificato();
            _bListinoModificato = false;
            _bChecksumListinoCoerente = true;

            if (dBaseTunnel_my.GetWebServiceReq() && dBaseTunnel_my.rdbPing())
            {
                if ((SF_Data.iNumCassa == CASSA_PRINCIPALE) && (_sRemDBChecksum != _sWebListinoChecksum) && dBaseTunnel_my.GetWebServiceReq())
                {
                    // avvia rdbSalvaListino() e ritorna subito
                    sQueue_Object[0] = WEB_PRICELIST_LOAD_START;
                    sQueue_Object[1] = "";

                    dBaseTunnel_my.EventEnqueue(sQueue_Object);
                }
            }

            return true;
        } // end SalvaListino


        /// <summary>Funzione di salvataggio dei dati di riepilogo giornaliero</summary>
        public static void SalvaDati()
        {
            int i, iIncassoParz;
            String sDir, sNomeFileDati, sNomeFileDatiBak;
            String sTmp, sDataRow, sDisp;
            StreamWriter fData;

            ulong iTotaleTeorico = 0;

            sDir = _sDataDir + "\\";

            sNomeFileDati = GetNomeFileDati(SF_Data.iNumCassa, GetActualDate());
            sNomeFileDatiBak = GetNomeFileDatiBak(SF_Data.iNumCassa);

            _ErrMsg.sNomeFile = sNomeFileDati;

            // esegue il backup del file Prezzi
            if (File.Exists(sDir + sNomeFileDati))
            {
                try
                {
                    File.Delete(sDir + sNomeFileDatiBak);
                    File.Move(sDir + sNomeFileDati, sDir + sNomeFileDatiBak);
                }
                catch (Exception)
                {
                    _ErrMsg.iErrID = ERR_FNR;
                    ErrorManager(_ErrMsg);
                }
            }

            fData = File.CreateText(sDir + sNomeFileDati);
            if (fData == null)
            {
                _ErrMsg.iErrID = ERR_FNO;
                ErrorManager(_ErrMsg);
            }
            else
            {
                fData.WriteLine("  StandFacile {0}", RELEASE_SW);

                fData.WriteLine("  Cassa n.{0}", SF_Data.iNumCassa);
                fData.WriteLine("  {0};", GetDateTimeString());

                if (!String.IsNullOrEmpty(SF_Data.sHeaders[0]))
                    fData.WriteLine("  {0}", SF_Data.sHeaders[0]);

                if (!String.IsNullOrEmpty(SF_Data.sHeaders[1]))
                    fData.WriteLine("  {0}", SF_Data.sHeaders[1]);

                fData.WriteLine("");

                /***************************************************************
                 *   Salvataggio del Numero di Scontrini e di Messaggi emessi
                 ***************************************************************/
                SF_Data.iNumOfLastReceipt = GetNumOfOrders();

                fData.WriteLine(sDAT_FMT_HED, "Numero Scontrini emessi = ", SF_Data.iNumOfLastReceipt - SF_Data.iStartingNumOfReceipts);

                fData.WriteLine(sDAT_FMT_HED, "Numero Scontrini QRcode = ", SF_Data.iNumOfWebReceipts);

                fData.WriteLine(sDAT_FMT_HED, "Numero    \"   annullati = ", SF_Data.iNumAnnullati);

                if (SF_Data.iNumAnnullati > 0)
                    fData.WriteLine("{0,28}{1,7}", "valore = ", IntToEuro(SF_Data.iTotaleAnnullato));

                fData.WriteLine("");

                fData.WriteLine("    Articolo       quant.venduta       dispon.");
                fData.WriteLine("             prz. unitario       parziale");

                /****************************************************
                 *   Salvataggio dei dati di riepilogo giornaliero
                 ****************************************************/

                for (i = 0; i < MAX_NUM_ARTICOLI + EXTRA_NUM_ARTICOLI; i++)
                {
                    // separa voci aggiuntive
                    if ((i == MAX_NUM_ARTICOLI) && ((SF_Data.Articolo[i].iPrezzoUnitario > 0) ||
                        (!String.IsNullOrEmpty(SF_Data.Articolo[i].sTipo) && OptionsDlg._rOptionsDlg.GetZeroPriceEnabled())))
                        fData.WriteLine();

                    if ((SF_Data.Articolo[i].iPrezzoUnitario > 0) ||
                        (!String.IsNullOrEmpty(SF_Data.Articolo[i].sTipo) && OptionsDlg._rOptionsDlg.GetZeroPriceEnabled()) ||
                        (SF_Data.Articolo[i].iGruppoStampa == (int)DEST_TYPE.DEST_COUNTER))
                    {
                        if (SF_Data.Articolo[i].iDisponibilita == DISP_OK)
                            sDisp = "OK";
                        else
                            sDisp = SF_Data.Articolo[i].iDisponibilita.ToString();

                        // 123456789012345678 999.00 8888 9876.00 OK
                        // eventuali superamenti del formato non precludono i conti ma solo l'impaginazione
                        // fData.WriteLine("{0,-18}{1,6+2}{2,4+2}{3,7+2}{4,3+2}",
                        if ((SF_Data.Articolo[i].iGruppoStampa == (int)DEST_TYPE.DEST_COUNTER) && (SF_Data.Articolo[i].iIndexListino != MAX_NUM_ARTICOLI - 1))
                        {
                            sDataRow = String.Format(sDAT_FMT_DAT,
                                SF_Data.Articolo[i].sTipo,                              // 18
                                IntToEuro(SF_Data.Articolo[i].iPrezzoUnitario),         //  6
                                SF_Data.Articolo[i].iQuantitaVenduta,                   //  4
                                0,                                                      //  7
                                sDisp);                                                 //  3

                            fData.WriteLine(sDataRow);
                        }
                        else
                        {
                            // vendita normale
                            iIncassoParz = SF_Data.Articolo[i].iQuantitaVenduta * SF_Data.Articolo[i].iPrezzoUnitario;

                            sDataRow = String.Format(sDAT_FMT_DAT,
                                SF_Data.Articolo[i].sTipo,                              // 18
                                IntToEuro(SF_Data.Articolo[i].iPrezzoUnitario),         //  6
                                SF_Data.Articolo[i].iQuantitaVenduta,                   //  4
                                IntToEuro(iIncassoParz),                                //  7
                                sDisp);                                                 //  3

                            iTotaleTeorico += (ulong)iIncassoParz;

                            fData.WriteLine(sDataRow);
                        }
                    }
                }

                fData.WriteLine(sDAT_FMT_DSH, "--------");
                fData.WriteLine(sDAT_FMT_TOT, "TOTALE", IntToEuro(SF_Data.iTotaleIncasso));

                if ((SF_Data.iTotaleScontatoStd > 0) || (SF_Data.iTotaleScontatoFisso > 0) || (SF_Data.iTotaleScontatoGratis > 0))
                {
                    fData.WriteLine("");

                    if (SF_Data.iTotaleScontatoGratis > 0)
                        fData.WriteLine(sDAT_FMT_TOT, "valore gratuiti", IntToEuro(SF_Data.iTotaleScontatoGratis));

                    if (SF_Data.iTotaleScontatoFisso > 0)
                        fData.WriteLine(sDAT_FMT_TOT, "valore sconto fisso", IntToEuro(SF_Data.iTotaleScontatoFisso));

                    if (SF_Data.iTotaleScontatoStd > 0)
                        fData.WriteLine(sDAT_FMT_TOT, "valore sconto articoli", IntToEuro(SF_Data.iTotaleScontatoStd));

                    fData.WriteLine(sDAT_FMT_DSH, "--------");
                    fData.WriteLine(sDAT_FMT_TOT, "TOTALE NETTO", IntToEuro(SF_Data.iTotaleIncasso - SF_Data.iTotaleScontatoStd -
                        SF_Data.iTotaleScontatoFisso - SF_Data.iTotaleScontatoGratis));

                    if ((SF_Data.iTotaleIncassoCard > 0) || (SF_Data.iTotaleIncassoSatispay > 0))
                    {
                        fData.WriteLine("");

                        sTmp = String.Format(sDAT_FMT_TOT, "PAGAM. CARD    ", IntToEuro(SF_Data.iTotaleIncassoCard));
                        fData.WriteLine(sTmp);

                        sTmp = String.Format(sDAT_FMT_TOT, "PAGAM. SATISPAY", IntToEuro(SF_Data.iTotaleIncassoSatispay));
                        fData.WriteLine(sTmp);

                        sTmp = String.Format(sDAT_FMT_TOT, "PAGAM. CONT.   ", IntToEuro(SF_Data.iTotaleIncasso - SF_Data.iTotaleScontatoStd -
                                                       SF_Data.iTotaleScontatoFisso - SF_Data.iTotaleScontatoGratis - SF_Data.iTotaleIncassoCard - SF_Data.iTotaleIncassoSatispay));
                        fData.WriteLine(sTmp);
                    }
                }

                fData.Close();
            } // end else

            LogToFile("DataManager : SalvaDati");

            sTmp = String.Format("DataManager SD: NT={0,3}, TTeor={1,8}, TInc={2,8}, T_SF={3,8}, T_SS={4,8}, T_SG={5,8}",
                    SF_Data.iNumOfLastReceipt, iTotaleTeorico, SF_Data.iTotaleIncasso, SF_Data.iTotaleScontatoFisso, SF_Data.iTotaleScontatoStd, SF_Data.iTotaleScontatoGratis);

            LogTestToFile(sTmp);

            /*********************************************
             *  salvataggio nel database Dati Riepilogo
             *********************************************/
            _rdBaseIntf.dbSalvaDati();
        }

    } // end class
} // end namespace

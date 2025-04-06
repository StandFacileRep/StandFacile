/***************************************************************************************************
    NomeFile : StandFacile/TestManager.cs
    Data	 : 06.12.2024
    Autore   : Mauro Artuso
     - se il registry contiene seqTest si avvia una emissione automatica di scontrini di test
     - durante _AUTO_SEQ_TEST la stampa delle copie richiede tempo e provoca disallineamenti
 **************************************************************************************************/


using System;
using System.IO;
using System.Collections.Generic;
using static System.Convert;

using static StandCommonFiles.ComDef;
using static StandCommonFiles.CommonCl;
using static StandCommonFiles.LogServer;

using static StandFacile.glb;
using static StandFacile.dBaseIntf;
using static StandFacile.DataManager;
using static StandFacile.ScontoDlg;
using static StandFacile.FrmMain;
using System.Threading;

namespace StandFacile
{
    /// <summary>Classe che legge un file per i Test Automatici sequenziali <br/>
    /// di generazione verificabile scontrini</summary>
    public class TestManager
    {

        static bool _bIgnoraFile = false;
        static bool _bPrimaVolta = true;

        static int _iNoteIndex;
        static int _iCounter, _iNumCassa;
        static List<string> _sInputStrings;
        static TErrMsg _WrnMsg;

        static StreamReader _fTest;
        static StreamWriter _fRecording;

        /// <summary>Struttura per Articoli di Test</summary>
        public struct TTestItem
        {
            /// <summary>stringa per la descrizione dell'Articolo di test</summary>
            public string sItem;
            /// <summary>int per la quantità dell'Articolo di test</summary>
            public int iQuantity;
            /// <summary>int per il Prezzo dell'Articolo di test</summary>
            public int iUnitPrice;
        }

        /// <summary>Lista di Articoli di TestSequence</summary>
        public static List<TTestItem> sFakeItem = new List<TTestItem>();

        /// <summary>Costruttore</summary>
        public TestManager()
        {
            String sInStr, sDir;

            _iNoteIndex = 0;
            _sInputStrings = new List<string>();

            if (!CheckService(Define._AUTO_SEQ_TEST))
                return;

            sDir = DataManager.GetExeDir() + "\\";

            if (CheckService(Define._AUTO_SEQ_TEST + "_C1"))
                _iNumCassa = 1;
            else
                _iNumCassa = 0;

            _sInputStrings.Clear();

            _iCounter = 0;
            _WrnMsg.iRiga = 0;
            _WrnMsg.iErrID = ERR_ECE;

            // cassa secondaria e DB e non si forza a caricare il listino locale
            if (CheckIf_CassaSec_and_NDB())
            {
                if (_rdBaseIntf.dbCaricaTest(_sInputStrings) > 0)
                    LogToFile("TestManager : dbCaricaTest()");
            }
            else
            {
                // ******* caricamento sInputStrings da file *******

                LogToFile("TestManager : Carica da File");

                _WrnMsg.sNomeFile = NOME_FILE_TEST;

                if (File.Exists(sDir + NOME_FILE_TEST))
                {
                    _fTest = File.OpenText(sDir + NOME_FILE_TEST);

                    while (((sInStr = _fTest.ReadLine()) != null) && (_sInputStrings.Count < 1000))
                        _sInputStrings.Add(sInStr);

                    _fTest.Close();

                    rFrmMain.SetStatus("File: " + NOME_FILE_TEST + "aperto");

                    _rdBaseIntf.dbSalvaTest();
                }
                else
                {
                    rFrmMain.SetStatus("File: " + NOME_FILE_TEST + "non apribile!");
                }

                // ******* fine caricamento stringhe dal DB o file *******

                if (Directory.Exists(sDir) && CheckService(Define._REC_TEST))
                {
                    if (File.Exists(sDir + NOME_FILE_REC))
                        File.Delete(sDir + NOME_FILE_REC);

                    _fRecording = File.CreateText(sDir + NOME_FILE_REC);
                }
            }
        }

        /// <summary>funzione per l'esecuzione di Test Automatici</summary>
        public static bool CaricaTest()
        {
            bool bCMD_OK = false;
            String sInStr, sTmp;
            String sDebug1, sDebug2;

            int iTagStart_IA, iTagStart_ET, iTagStart_ES, iTagStart_IF, iTagStart_CS, iTagStart_PH, iTagStart_PC, iTagStart_PS, iTagStart_AO;
            int iTagStop_IA, iTagStop_ET, iTagStop_ES, iTagStop_IF, iTagStop_CS, iTagStop_PH, iTagStop_PC, iTagStop_PS, iTagStop_AO;

            int iTagStart_CN, iTagStart_NT, iTagStart_NA, iTagStart_TV, iTagStart_NM, iTagStart_DS;
            int iTagStop_CN, iTagStop_NT, iTagStop_NA, iTagStop_TV, iTagStop_NM, iTagStop_DS;

            int iTagStart_AN;
            int iTagStop_AN;

            int iTagStart_qa, iTagStop_qa; // quantità Articolo
            int iTagStart_da, iTagStop_da; // disponibilità Articolo

            int iTagStart_bv, iTagStop_bv;
            int iTagStart_iv, iTagStop_iv;
            int iTagStart_tv, iTagStop_tv;

            int iIndex, iquantArticolo, idispArticolo, iPrezzoUnitario;
            int iFlagSconto;

            String sTipoArticolo;

            // punto di attesa sincronizzazione con il primo scontrino emesso dalla CASSA_PRINCIPALE
            if ((DataManager.GetNumOfOrders() == 0) && (SF_Data.iNumCassa > CASSA_PRINCIPALE) && _bPrimaVolta)
            {
                LogToFile("TestManager : attesa sync");
                return false;
            }

            if (_iCounter > 0)
            {
                _iCounter--;
                return true;
            }
            else
                _iCounter = 4; // x 0.25s

            if (_WrnMsg.iRiga < _sInputStrings.Count)
            {
                sInStr = _sInputStrings[_WrnMsg.iRiga].Trim();
                _WrnMsg.iRiga++;
            }
            else
                return false;

            // la Cassa Secondaria recupera lo svantaggio iniziale fino al primo scontrino, processando però <CP>
            while (!sInStr.Contains("<ET>") && !sInStr.Contains("<CP>") && (SF_Data.iNumCassa > CASSA_PRINCIPALE) && _bPrimaVolta)
            {
                sInStr = _sInputStrings[_WrnMsg.iRiga].Trim();
                _WrnMsg.iRiga++;
                _iCounter = 0;
            }

            iquantArticolo = 0;
            idispArticolo = 0;
            sTipoArticolo = "";

            // ogni secondo
            LogToFile("TestManager : I CaricaTest");

            /*****************************************
             *		   parsing delle stringhe
             *****************************************/

            if (String.IsNullOrEmpty(sInStr))
            {
                _iCounter = 1; // 0.25s
                return true;
            }
            else if (sInStr.StartsWith(";")) // la riga è di commento
            {
                _iCounter = 1; // 0.25s
                return true;
            }
            else if (_bIgnoraFile) // comodità
                return false;

            // feebdack visivo
            rFrmMain.SetStatus("Test: " + sInStr);

            iTagStart_IA = sInStr.IndexOf("<IA>"); // Imposta Articolo
            iTagStart_ET = sInStr.IndexOf("<ET>"); // Emissione scontrino
            iTagStart_ES = sInStr.IndexOf("<ES>"); // Esportazione
            iTagStart_IF = sInStr.IndexOf("<IF>"); // Ignora file
            iTagStart_DS = sInStr.IndexOf("<DS>"); // Sconto
            iTagStart_CS = sInStr.IndexOf("<CS>"); // Cassa
            iTagStart_CN = sInStr.IndexOf("<CN>"); // Coperti numero
            iTagStart_NT = sInStr.IndexOf("<NT>"); // Nota
            iTagStart_NA = sInStr.IndexOf("<NA>"); // Nota Articolo
            iTagStart_TV = sInStr.IndexOf("<TV>"); // Tavolo
            iTagStart_NM = sInStr.IndexOf("<NM>"); // Nome
            iTagStart_PH = sInStr.IndexOf("<PH>"); // Pagamento CASH
            iTagStart_PC = sInStr.IndexOf("<PC>"); // Pagamento CARD
            iTagStart_PS = sInStr.IndexOf("<PS>"); // Pagamento SATISPAY
            iTagStart_AO = sInStr.IndexOf("<AO>"); // Annullo Ordine

            iTagStart_AN = sInStr.IndexOf("<AN>"); // Imposta Articolo che non esiste nel Listino

            iTagStop_IA = sInStr.IndexOf("</IA>");
            iTagStop_ET = sInStr.IndexOf("</ET>");
            iTagStop_ES = sInStr.IndexOf("</ES>");
            iTagStop_IF = sInStr.IndexOf("</IF>");
            iTagStop_DS = sInStr.IndexOf("</DS>");
            iTagStop_CS = sInStr.IndexOf("</CS>");
            iTagStop_CN = sInStr.IndexOf("</CN>");
            iTagStop_NT = sInStr.IndexOf("</NT>");
            iTagStop_NA = sInStr.IndexOf("</NA>");
            iTagStop_TV = sInStr.IndexOf("</TV>");
            iTagStop_NM = sInStr.IndexOf("</NM>");
            iTagStop_AO = sInStr.IndexOf("</AO>");
            iTagStop_PH = sInStr.IndexOf("</PH>");
            iTagStop_PC = sInStr.IndexOf("</PC>");
            iTagStop_PS = sInStr.IndexOf("</PS>");
            iTagStop_AN = sInStr.IndexOf("</AN>");

            iTagStart_qa = sInStr.IndexOf("<qa>"); // quantità articolo
            iTagStart_da = sInStr.IndexOf("<da>"); // disponibilità Articolo
            iTagStop_qa = sInStr.IndexOf("</qa>");
            iTagStop_da = sInStr.IndexOf("</da>");

            iTagStart_bv = sInStr.IndexOf("<bv>"); // binary value
            iTagStart_iv = sInStr.IndexOf("<iv>"); // integer value
            iTagStart_tv = sInStr.IndexOf("<tv>"); // text value

            iTagStop_bv = sInStr.IndexOf("</bv>");
            iTagStop_iv = sInStr.IndexOf("</iv>");
            iTagStop_tv = sInStr.IndexOf("</tv>");

            /********************************* 
             *      INSERIMENTO ARTICOLO
             *********************************/
            if ((iTagStart_IA != -1) && (iTagStop_IA != -1) && (iTagStart_iv != -1) && (iTagStop_iv != -1) &&
                    ((iTagStart_qa != -1) && (iTagStop_qa != -1)) || ((iTagStart_da != -1) && (iTagStop_da != -1)))
            {
                // composizione Ordine
                sTmp = sInStr.Substring(iTagStart_iv + 4, iTagStop_iv - iTagStart_iv - 4).Trim();
                iIndex = ToInt32(sTmp);
                _iNoteIndex = ToInt32(sTmp);

                if (iIndex >= MAX_NUM_ARTICOLI)
                {
                    _WrnMsg.iErrID = ERR_NVE;
                    _WrnMsg.sNomeFile = NOME_FILE_TEST;
                    WarningManager(_WrnMsg);
                    return false;
                }

                // eventuale tipo
                if ((iTagStart_tv != -1) && (iTagStop_tv != -1))
                {
                    sTipoArticolo = sInStr.Substring(iTagStart_tv + 4, iTagStop_tv - iTagStart_tv - 4).Trim();
                }

                // quantità
                if ((iTagStart_qa != -1) && (iTagStop_qa != -1))
                {
                    sTmp = sInStr.Substring(iTagStart_qa + 4, iTagStop_qa - iTagStart_qa - 4).Trim();
                    iquantArticolo = ToInt32(sTmp);
                }

                // disponibilità
                if ((iTagStart_da != -1) && (iTagStop_da != -1))
                {
                    sTmp = sInStr.Substring(iTagStart_da + 4, iTagStop_da - iTagStart_da - 4).Trim();
                    idispArticolo = ToInt32(sTmp);

                    TestRecord_setDispItem(iIndex, sTipoArticolo, idispArticolo);
                }

                if (String.IsNullOrEmpty(SF_Data.Articolo[iIndex].sTipo))
                {
                    _WrnMsg.iErrID = WRN_TPN;
                    _WrnMsg.sNomeFile = NOME_FILE_TEST;
                    WarningManager(_WrnMsg);
                    return false;
                }

                if (!String.IsNullOrEmpty(sTipoArticolo) && String.Compare(sTipoArticolo, SF_Data.Articolo[iIndex].sTipo) != 0)
                {
                    _WrnMsg.iErrID = WRN_TIANF;
                    _WrnMsg.sMsg = sTipoArticolo;
                    WarningManager(_WrnMsg);
                    return false;
                }

                if (_iNumCassa == SF_Data.iNumCassa)
                {
                    if (iquantArticolo > 0)
                        SF_Data.Articolo[iIndex].iQuantitaOrdine = iquantArticolo;

                    rFrmMain.SelectTab(iIndex);
                }

                if (_iNumCassa <= CASSA_PRINCIPALE) // accetta anche lo zero
                {
                    if (idispArticolo > 0)
                        SF_Data.Articolo[iIndex].iDisponibilita = idispArticolo;
                }

                _iCounter = 1; // 0.25s
                bCMD_OK = true;
            }
            else if ((iTagStart_ET != -1) && (iTagStop_ET != -1))
            {
                /***************************************************
                 *              emissione scontrino
                 * *************************************************/
                if (_iNumCassa == SF_Data.iNumCassa)
                {
                    rFrmMain.BtnScontrino_Click(null, null);
                    sDebug2 = String.Format("TestManager: ET glb={0}, loc={1}", DataManager.GetNumOfOrders(), DataManager.GetNumOfLocalOrders());
                    LogToFile(sDebug2);

                    _iCounter = 2 * 4; // 2s è più veloce per compensare la stampa
                }
                else
                {
                    int iCount = 0;

                    // attende che le altre casse emettano eventuali scontrini
                    while (DataManager.GetNumOfOrders() <= DataManager.GetNumOfLocalOrders() && (iCount < 20))
                    {
                        sDebug1 = String.Format("TestManager: i={0}, glb={1}, loc={2}", iCount, DataManager.GetNumOfOrders(), DataManager.GetNumOfLocalOrders());

                        Console.WriteLine(sDebug1);
                        iCount++;

                        Thread.Sleep(1000);
                    }

                    sDebug2 = String.Format("TestManager: i={0}, glb={1}, loc={2}", iCount, DataManager.GetNumOfOrders(), DataManager.GetNumOfLocalOrders());
                    LogToFile(sDebug2);

                    _iCounter = 5 * 4; // 5s

                }

                _bPrimaVolta = false;
                bCMD_OK = true;
            }
            else if ((iTagStart_ES != -1) && (iTagStop_ES != -1))
            {
                // Esportazione
                if (_iNumCassa == SF_Data.iNumCassa)
                    rFrmMain.BtnEsportazione_Click(null, null);

                _iCounter = 1;
                bCMD_OK = true;
            }
            else if ((iTagStart_IF != -1) && (iTagStop_IF != -1))
            {
                // Ignora
                _bIgnoraFile = true;
                bCMD_OK = true;
            }
            else if ((iTagStart_CS != -1) && (iTagStop_CS != -1))
            {
                // Cassa
                sTmp = sInStr.Substring(iTagStart_iv + 4, iTagStop_iv - iTagStart_iv - 4).Trim();

                // Sovrascrive per test con sola cassa 1
                if (CheckService(Define._AUTO_SEQ_TEST + "_C1"))
                    _iNumCassa = 1;
                else
                    _iNumCassa = ToInt32(sTmp);

                _iCounter = 1;
                bCMD_OK = true;
            }
            else if ((iTagStart_DS != -1) && (iTagStop_DS != -1))
            {
                iFlagSconto = 0;

                // tipo di Sconto
                if ((iTagStart_iv != -1) && (iTagStop_iv != -1))
                {
                    sTmp = sInStr.Substring(iTagStart_iv + 4, iTagStop_iv - iTagStart_iv - 4).Trim();
                    iFlagSconto = ToInt32(sTmp);
                }

                if ((_iNumCassa == SF_Data.iNumCassa) && !SF_Data.bPrevendita)
                    
                    SetSconto(iFlagSconto);

                _iCounter = 1;
                bCMD_OK = true;
            }
            else if ((iTagStart_CN != -1) && (iTagStop_CN != -1))
            {
                // coperti numero
                sTmp = sInStr.Substring(iTagStart_iv + 4, iTagStop_iv - iTagStart_iv - 4).Trim();

                if (_iNumCassa == SF_Data.iNumCassa)
                    rFrmMain.SetEditCoperto(sTmp);

                _iCounter = 1;
                bCMD_OK = true;
            }
            else if ((iTagStart_NT != -1) && (iTagStop_NT != -1))
            {
                // nota
                sTmp = sInStr.Substring(iTagStart_tv + 4, iTagStop_tv - iTagStart_tv - 4).Trim();

                if (_iNumCassa == SF_Data.iNumCassa)
                    rFrmMain.SetEditNota(sTmp);

                _iCounter = 1;
                bCMD_OK = true;
            }
            else if ((iTagStart_NA != -1) && (iTagStop_NA != -1))
            {
                // nota Articolo
                sTmp = sInStr.Substring(iTagStart_tv + 4, iTagStop_tv - iTagStart_tv - 4).Trim();

                if (_iNumCassa == SF_Data.iNumCassa)
                    rFrmMain.SetEditNotaArticolo(_iNoteIndex, sTmp);

                Thread.Sleep(250);
                _iCounter = 1;
                bCMD_OK = true;
            }
            else if ((iTagStart_TV != -1) && (iTagStop_TV != -1))
            {
                // tavolo
                sTmp = sInStr.Substring(iTagStart_tv + 4, iTagStop_tv - iTagStart_tv - 4).Trim();

                if (_iNumCassa == SF_Data.iNumCassa)
                    rFrmMain.SetEditTavolo(sTmp);

                _iCounter = 1;
                bCMD_OK = true;
            }
            else if ((iTagStart_NM != -1) && (iTagStop_NM != -1))
            {
                // nome
                sTmp = sInStr.Substring(iTagStart_tv + 4, iTagStop_tv - iTagStart_tv - 4).Trim();

                if (_iNumCassa == SF_Data.iNumCassa)
                    rFrmMain.SetEditNome(sTmp);

                _iCounter = 1;
                bCMD_OK = true;
            }
            else if ((iTagStart_PH != -1) && (iTagStop_PH != -1))
            {
                if (_iNumCassa == SF_Data.iNumCassa)
                    rFrmMain.SetPagamento_CASH();

                _iCounter = 1;
                bCMD_OK = true;
            }
            else if ((iTagStart_PC != -1) && (iTagStop_PC != -1))
            {
                if (_iNumCassa == SF_Data.iNumCassa)
                    rFrmMain.SetPagamento_CARD();

                _iCounter = 1;
                bCMD_OK = true;
            }
            else if ((iTagStart_PS != -1) && (iTagStop_PS != -1))
            {
                if (_iNumCassa == SF_Data.iNumCassa)
                    rFrmMain.SetPagamento_SATISPAY();

                _iCounter = 1;
                bCMD_OK = true;
            }
            else if ((iTagStart_AO != -1) && (iTagStop_AO != -1))
            {
                // Annullo
                sTmp = sInStr.Substring(iTagStart_iv + 4, iTagStop_iv - iTagStart_iv - 4).Trim();

                if (_iNumCassa == SF_Data.iNumCassa)
                    DataManager.AnnulloOrdine(ToInt32(sTmp) + sConfig.iReceiptStartNumber - 1);

                _iCounter = 1;
                bCMD_OK = true;
            }
            else if ((iTagStart_AN != -1) && (iTagStop_AN != -1))
            {
                // fake Articolo
                sTipoArticolo = sInStr.Substring(iTagStart_tv + 4, iTagStop_tv - iTagStart_tv - 4).Trim();

                sTmp = sInStr.Substring(iTagStart_qa + 4, iTagStop_qa - iTagStart_qa - 4).Trim();
                iquantArticolo = ToInt32(sTmp);

                sTmp = sInStr.Substring(iTagStart_iv + 4, iTagStop_iv - iTagStart_iv - 4).Trim();
                iPrezzoUnitario = ToInt32(sTmp);


                TTestItem sFakeItemTmp = new TTestItem
                {
                    sItem = sTipoArticolo,
                    iQuantity = iquantArticolo,
                    iUnitPrice = iPrezzoUnitario
                };

                if (_iNumCassa == SF_Data.iNumCassa)
                    sFakeItem.Add(sFakeItemTmp);

                _iCounter = 1;
                bCMD_OK = true;
            }

            // evita refresh di pulizia su emissione scontrino o da altra cassa
            if (((iTagStart_ET != -1) && (iTagStop_ET != -1)) || (_iNumCassa != SF_Data.iNumCassa))
            {
                return true;
            }
            else if (bCMD_OK)
            {
                AnteprimaDlg.rAnteprimaDlg.RedrawReceipt();

                rFrmMain.SetAnteprima_TP();
                rFrmMain.MainGrid_Redraw(null, null);
                return true;
            }
            else
            {
                _WrnMsg.iErrID = WRN_CNF;
                _WrnMsg.sNomeFile = NOME_FILE_TEST;
                WarningManager(_WrnMsg);
                return false;
            }
        }

        /// <summary>funzione per l'append di Test Automatici ordine</summary>
        public static void TestRecord_order()
        {
            string sTmpRec;

            if (!CheckService(Define._REC_TEST))
                return;

            // cassa
            sTmpRec = String.Format("<CS><iv>{0}</iv></CS>", SF_Data.iNumCassa);
            _fRecording.WriteLine(sTmpRec);

            // sconti
            if (IsBitSet(SF_Data.iStatusSconto, BIT_SCONTO_STD))
            {
                sTmpRec = String.Format("<DS><iv>{0}</iv></DS>", (SF_Data.iStatusSconto & 0x000000FF));
                _fRecording.WriteLine(sTmpRec);
            }

            if (!String.IsNullOrEmpty(rFrmMain.GetEditCoperto()))
            {
                sTmpRec = String.Format("<CN><iv>{0}</iv></CN>", rFrmMain.GetEditCoperto());
                _fRecording.WriteLine(sTmpRec);
            }


            if (!String.IsNullOrEmpty(rFrmMain.GetEditTavolo()))
            {
                sTmpRec = String.Format("<TV><iv>{0}</iv></TV>", rFrmMain.GetEditTavolo());
                _fRecording.WriteLine(sTmpRec);
            }

            if (!String.IsNullOrEmpty(rFrmMain.GetEditNota()))
            {
                sTmpRec = String.Format("<NT><iv>{0}</iv></NT>", rFrmMain.GetEditNota());
                _fRecording.WriteLine(sTmpRec);
            }

            // esportazione
            if (IsBitSet(SF_Data.iStatusReceipt, BIT_ESPORTAZIONE))
                _fRecording.WriteLine("<ES><bv>1</bv></ES>");

            // ordine
            for (int i = 0; i < MAX_NUM_ARTICOLI - 1; i++)
            {
                if (SF_Data.Articolo[i].iQuantitaOrdine > 0)
                {
                    sTmpRec = String.Format("<IA><iv>{0,3}</iv> <tv>{1,-20}</tv> <qa>{2}</qa></IA>",
                        i, SF_Data.Articolo[i].sTipo, SF_Data.Articolo[i].iQuantitaOrdine);

                    _fRecording.WriteLine(sTmpRec);
                }
            }

            _fRecording.WriteLine("");

            sTmpRec = String.Format("<ET> ***emissione scontrino n. {0} </ET>", GetNumOfLocalOrders());
            _fRecording.WriteLine(sTmpRec + "\n");

            _fRecording.Flush();
        }

        /// <summary>funzione per l'append di Test Automatici disponibilità Articoli</summary>
        public static void TestRecord_setDispItem(int iParam, String sParam, int iDispParam)
        {
            string sTmpRec;

            if (!CheckService(Define._REC_TEST))
                return;

            sTmpRec = String.Format("<IA><iv>{0,3}</iv> <tv>{1,-20}</tv> <da>{2}</da></IA>", iParam, sParam, iDispParam);
            _fRecording.WriteLine(sTmpRec);
            _fRecording.WriteLine("");

            _fRecording.Flush();
        }

        static bool bPrimaVoltaComp = true;

        /// <summary>funzione per l'append di Test Automatici disponibilità Componenti</summary>
        public static void TestRecord_setDispComp(int iParam, String sParam, int iDispParam)
        {
            string sTmpRec;

            if (!CheckService(Define._REC_TEST))
                return;

            if (bPrimaVoltaComp)
            {
                bPrimaVoltaComp = false;
                _fRecording.WriteLine("");
            }

            sTmpRec = String.Format("<IC><iv>{0,3}</iv> <tv>{1,-20}</tv> <dc>{2}</dc></IC>", iParam, sParam, iDispParam);
            _fRecording.WriteLine(sTmpRec);
            _fRecording.WriteLine("");

            _fRecording.Flush();
        }

    } // end class
} // end namespace

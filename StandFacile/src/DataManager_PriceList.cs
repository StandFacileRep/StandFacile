/**********************************************************************
     NomeFile : StandFacile/DataManager.cs
     Data	  : 19.09.2024
     Autore   : Mauro Artuso

     nb: DB_Data compare sempre a destra nelle assegnazioni
 **********************************************************************/

using System;
using System.IO;
using static System.Convert;
using System.Collections.Generic;

using static StandFacile.glb;
using static StandFacile.Define;
using static StandFacile.dBaseIntf;
using static StandFacile.ScontoDlg;
using static StandFacile.FrmMain;

using StandCommonFiles;
using static StandCommonFiles.ComDef;
using static StandCommonFiles.CommonCl;
using static StandCommonFiles.ReceiptAndCopies;
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

        /***************************************************************************
            info generali CaricaListino :

            se la cassa è di tipo CASSA_PRINCIPALE || non si usa NDB si carica il listino dal file Prezzi,

            altrimenti se è di tipo CASSA_SECONDARIA && si usa NDB allora
            si carica il listino dalla tabella "Listino" del database

            ritorna true se ha successo, false altrimenti

            *****  formati :  *****

            Inizializza la struct TArticolo[] con i tipi di pietanza, prezzi, gruppi
            di stampa. Questi dati vengono caricati dal file 'Listino.TXT',

             Formato del File:
             le righe che iniziano con ';' in colonna 1 sono di commento

             alcuni "TAGS" contengono informazioni speciali per il listino,
             ecco il significato delle Linee che iniziano con i TAGS :

            # HD0..HD3 sono 2 stringhe di Header e 2 di Footer

            # GS0..#GS5 testo specifico per i diversi gruppi di stampa a cui gli
             Articoli possono appartenere che verrà stampato nelle copie, ad esempio :

            "##### COPIA BIBITE #####",  "##### COPIA CUCINA #####",
            "##### COPIA GRUPPO1 #####", "##### COPIA GRUPPO2 #####",
            "##### COPIA GRUPPO3 #####", "##### COPIA GRUPPO4 #####"
            "##### CONTATORI #####"

            # GS indica Gruppo SET
            # GC indica Gruppo CLEAR

             #TMS, #TMC, #TVS, #TVC, #CPS, #CPC, #BCS, #BCC, #MPS, #MPC

            sono i 6 flag rispettivamente Set o Clear che indicano il Touch mode, il modo Prevendita e
            se l'inserimento del Tavolo, Coperti, Barcode, Modo pagamento è richiesto o meno

            # PN0..PN3 sono 4 intestazioni dei TABS della griglia principale

            # NRxx è il numero di righe della griglia principale : 16, 20, 25 es. #NR20
            # NCx  è il numero di colonne della griglia principale : 3, 4 es. #NC4
            # LFxx raggruppa un dato numero di righe vuote "Line Feed"  es. #LF12
            # CK codice di checksum per controllare l'integrità del file

             Le altre sono costituite da:
             'descrizione Articolo ; prezzo_unitario ;[ sconto ;] DestinazioneStampaCucina' ,
             oppure sono linee vuote (in aggiunta al TAG #LF) che appariranno
             nel menu' principale del programma come linee di separazione.

             - I caratteri di 'tipo di Articolo' sono al massimo 18 (iMAX_ART_CHAR),

             - 'prezzo_unitario' deve essere espresso in Euro con il punto o
             la virgola come separatore.

             - 'Prezzo Scontato' deve essere espresso in Euro con il punto o
             la virgola come separatore, o in percentuale es. 10%
             iPrezzoScontato viene sempre calcolato,
             iScontoPerc viene letto solo se presente e non calcolato

             - 'iGruppoStampa' può valere da 0 a NUM_EDIT_GROUPS-1 in modo da
             distinguere diversi ragruppamenti di pietanze omogenee nella stampa
             delle copie

             Il numero massimo di voci, comprese le righe vuote, ed escluse quelle di
             commento e quelle vuote in coda al file, e' inferiore a MAX_NUM_ARTICOLI .

             Non sono ammesse ripetizioni dello stesso 'tipo di Articolo'
         ***************************************************************************/
        private bool CaricaListino(bool bGetDataParam = false)
        {
            bool bChecksumTrovato = false;
            int s_pos; // posizione della semicolon
            int i, j, iPrzTmp, iDest;
            int iDiscType, iScontoFlag, iScontoVal, iCopertoVal;

            int iRiga = 0;  // indice per caricamento dati in Articolo[]
            int hIndex;     // scandisce le righe di Header
            int iPageNum;
            int iLineFeed;

            uint uWebHashCode = 0, uLocHashCode = 0;

            String sPL_LocChecksum, sPL_WebChecksum, sDBChecksum;
            String sInStr, sInStrCopy, sTmp, sDir;
            String sArticoloStr, sEuro; // stringa in Euro
            String sGroupName, sScontoText;
            String[] sQueue_Object = new String[2];

            StreamReader fprz;
            List<string> sInputStrings = new List<string>();

            try
            {
                LogToFile("DataManager : I CaricaListino");

                sDir = _sExeDir + "\\";

                sInputStrings.Clear();
                _bChecksumListinoCoerente = false;

                // cassa secondaria e DB e non si forza a caricare il listino locale
                if (CheckIf_CassaSec_and_NDB())
                {

                    // ******* caricamento sInputStrings dal DB *******

                    // segnala che il File Prezzi locale verrà ignorato
                    if (File.Exists(sDir + NOME_FILE_LISTINO) && !CheckService(Define._AUTO_SEQ_TEST))
                        WarningManager(WRN_FPI);

                    _ErrMsg.sNomeFile = NOME_LISTINO_DBTBL;

                    if (_rdBaseIntf.dbCaricaListino(sInputStrings) > 0)
                        LogToFile("DataManager : dbCaricaListino()");
                    else
                        return false; // tanto vale non proseguire
                }
                else
                {
                    // ******* caricamento sInputStrings da file *******
                    LogToFile("DataManager : Carica da File");

                    _ErrMsg.sNomeFile = NOME_FILE_LISTINO;

                    if (File.Exists(sDir + NOME_FILE_LISTINO))
                        fprz = File.OpenText(sDir + NOME_FILE_LISTINO);
                    else
                    {
                        _ErrMsg.iErrID = WRN_FNF;
                        WarningManager(_ErrMsg);
                        return false;
                    }

                    while (((sInStr = fprz.ReadLine()) != null) && (sInputStrings.Count < 1000))
                        sInputStrings.Add(sInStr);

                    fprz.Close();
                }

                // ******* fine caricamento stringhe dal DB o file *******

                /*****************************************
                 *		   parsing delle stringhe
                 *****************************************/

                for (_ErrMsg.iRiga = 0; _ErrMsg.iRiga < sInputStrings.Count;)
                {
                    sInStr = sInputStrings[_ErrMsg.iRiga].Trim();
                    _ErrMsg.iRiga++;

                    if (sInStr.StartsWith(";"))
                        continue;   // la riga è di commento

                    else if (sInStr.StartsWith("//"))
                        continue;   // la riga è di commento

                    else if (sInStr.StartsWith("#CKW"))
                    {
                        _sWebListinoChecksum = String.Format("{0:X8}", uWebHashCode); // valore calcolato

                        // valore letto dal file
                        sPL_WebChecksum = sInStr.Substring(5);                        // valore letto dal file

                        if (!String.Equals(_sWebListinoChecksum, sPL_WebChecksum))
                        {
                            _ErrMsg.iErrID = WRN_CKPW;
                            WarningManager(_ErrMsg);
                        }

                        continue;   // la riga è di commento
                    }
                    /***********************************************
                     *			verifica del checksum 			   *
                     ***********************************************/
                    else if (sInStr.StartsWith("#CKL"))
                    {
                        _sLocListinoChecksum = String.Format("{0:X8}", uLocHashCode); // valore calcolato

                        // valore letto dal file
                        sPL_LocChecksum = sInStr.Substring(5);                        // valore letto dal file

                        if (!String.Equals(_sLocListinoChecksum, sPL_LocChecksum))
                        {
                            _ErrMsg.iErrID = WRN_CKPL;
                            WarningManager(_ErrMsg);
                        }
                        else
                            _bChecksumListinoCoerente = true;

                        bChecksumTrovato = true; // altrimenti dà 2 avvisi

                        _bListinoCaricatoConSuccesso = true;

                        continue;
                    }
                    else if (sInStr.StartsWith("#CK"))
                    {
                        _sLocListinoChecksum = String.Format("{0:X8}", uLocHashCode); // valore calcolato
                        sPL_LocChecksum = sInStr.Substring(3);                        // valore letto dal file

                        if (!String.Equals(_sLocListinoChecksum, sPL_LocChecksum))
                        {
                            _ErrMsg.iErrID = WRN_CKPL;
                            WarningManager(_ErrMsg);
                        }
                        else
                            _bChecksumListinoCoerente = true;

                        bChecksumTrovato = true; // altrimenti dà 2 avvisi

                        _bListinoCaricatoConSuccesso = true;

                        continue;
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(sInStr))   // riga vuota
                        {
                            if ((iRiga < MAX_NUM_ARTICOLI) && !bChecksumTrovato)  // non considera le righe vuote dopo il checksum
                            {
                                iRiga++;
                                uLocHashCode += Hash(_ErrMsg.iRiga); // Righe vuote calcola il checksum del numero per rivelare le inversioni
                                continue;
                            }
                            else
                                continue;
                        }
                        else
                        {
                            // riga normale
                            uLocHashCode += Hash(sInStr);
                        }
                    }

                    /*********************************
                     *	verifica TAG formattazione
                     *********************************/

                    if (sInStr.StartsWith("#DT"))
                    {
                        // ****	Data ed ora ****

                        SF_Data.sListinoDateTime = sInStr.Substring(4);

                        if (bGetDataParam)
                            return true;

                        continue;
                    }
                    else if (sInStr.StartsWith("#HD"))
                    {
                        // ****	Header/Footer ****

                        sTmp = sInStr.Substring(3, 1);

                        hIndex = ToInt32(sTmp);

                        if ((hIndex >= 0) && (hIndex < MAX_NUM_HEADERS) && (sInStr.Length > 5))
                        {
                            SF_Data.sHeaders[hIndex] = sInStr.Substring(5);

                            if (SF_Data.sHeaders[hIndex].Length > iMAX_RECEIPT_CHARS)
                            {
                                _ErrMsg.iErrID = ERR_STL;
                                ErrorManager(_ErrMsg);  // stringa troppo lunga
                            }

                            hIndex++;
                        }

                        continue;
                    }
                    else if (sInStr.StartsWith("#GS"))
                    {
                        /****************************************
                         *	Copie/Text and Flag attivo, SET
                         *	per copie stampate da StandFacile
                         ****************************************/

                        sTmp = sInStr.Substring(3, 1);

                        i = ToInt32(sTmp);

                        if ((i >= 0) && (i < NUM_EDIT_GROUPS) && (sInStr.Length > 5))
                        {
                            if (i != (int)DEST_TYPE.DEST_TIPO8)
                            {
                                // ritaglia la parte di interesse web
                                sGroupName = sInStr[3] + sInStr.Substring(5);
                                uWebHashCode += Hash(sGroupName);
                            }

                            SF_Data.bCopiesGroupsFlag[i] = true;

                            // per gli altri indici il testo è una costante
                            if (i < NUM_EDIT_GROUPS)
                                SF_Data.sCopiesGroupsText[i] = sInStr.Substring(6);

                            if (SF_Data.sCopiesGroupsText[i].Length > MAX_COPIES_TEXT_CHARS)
                            {
                                _ErrMsg.iErrID = ERR_STL;
                                ErrorManager(_ErrMsg);  // stringa troppo lunga
                            }

                            // ricava il colore del gruppo
                            sTmp = sInStr.Substring(4, 1).Trim();
                            if (!String.IsNullOrEmpty(sTmp))
                            {
                                j = ToInt32(sTmp);

                                if ((j >= 0) && (j < NUM_GROUPS_COLORS))
                                    SF_Data.iGroupsColor[i] = j;
                            }
                        }

                        continue;
                    }
                    else if (sInStr.StartsWith("#GC"))
                    {
                        /*******************************************
                         *	Copie/Text and Flag non attivo, CLEAR
                         *	per copie stampate da StandCucina
                         *******************************************/

                        sTmp = sInStr.Substring(3, 1);

                        i = ToInt32(sTmp);

                        if ((i >= 0) && (i < NUM_EDIT_GROUPS) && (sInStr.Length > 5))
                        {
                            if (i != (int)DEST_TYPE.DEST_TIPO8)
                            {
                                // ritaglia la parte di interesse web
                                sGroupName = sInStr[3] + sInStr.Substring(5);
                                uWebHashCode += Hash(sGroupName);
                            }

                            SF_Data.bCopiesGroupsFlag[i] = false;

                            // per gli altri indici il testo è una costante
                            if (i < NUM_EDIT_GROUPS)
                                SF_Data.sCopiesGroupsText[i] = sInStr.Substring(6);

                            if (SF_Data.sCopiesGroupsText[i].Length > MAX_COPIES_TEXT_CHARS)
                            {
                                _ErrMsg.iErrID = ERR_STL;
                                ErrorManager(_ErrMsg);  // stringa troppo lunga
                            }

                            // ricava il colore del gruppo
                            sTmp = sInStr.Substring(4, 1).Trim();
                            if (!String.IsNullOrEmpty(sTmp))
                            {
                                j = ToInt32(sTmp);

                                if ((j >= 0) && (j < NUM_GROUPS_COLORS))
                                    SF_Data.iGroupsColor[i] = j;
                            }
                        }

                        continue;
                    }
                    else if (sInStr.StartsWith("#CT"))
                    {
                        /**************************************************
                         *	Colori/Text per copie stampate da StandFacile
                         **************************************************/

                        sTmp = sInStr.Substring(3, 1);

                        i = ToInt32(sTmp);

                        if ((i >= 0) && (i < NUM_GROUPS_COLORS - 1) && (sInStr.Length > 5))
                        {
                            SF_Data.sColorGroupsText[i] = sInStr.Substring(5);

                            if (SF_Data.sColorGroupsText[i].Length > MAX_COPIES_TEXT_CHARS)
                            {
                                _ErrMsg.iErrID = ERR_STL;
                                ErrorManager(_ErrMsg);  // stringa troppo lunga
                            }
                        }

                        continue;
                    }
                    else if (sInStr.StartsWith("#SC"))
                    {
                        /*******************************************
                         *	gestione sconti: 
                         *	0 percentuale, 1 fisso, 2 gratis
                         *******************************************/

                        iScontoFlag = 0;
                        iScontoVal = 0;
                        sScontoText = "";
                        _ErrMsg.iErrID = ERR_SNF;

                        sTmp = sInStr.Substring(3, 1);

                        iDiscType = ToInt32(sTmp) + 1;

                        switch (iDiscType)
                        {
                            case (int)DISC_TYPE.DISC_STD:
                                s_pos = sInStr.IndexOf(';');

                                if (s_pos == -1)
                                    ErrorManager(_ErrMsg);

                                sTmp = sInStr.Substring(4, s_pos - 4);
                                iScontoVal = Convert.ToInt32(sTmp.Trim());

                                sInStr = sInStr.Remove(0, s_pos + 1);

                                s_pos = sInStr.IndexOf(';');

                                if (s_pos == -1)
                                    ErrorManager(_ErrMsg);

                                sTmp = sInStr.Substring(0, s_pos);
                                iScontoFlag = Convert.ToInt32(sTmp.Trim());

                                sTmp = sInStr.Substring(s_pos + 1);
                                sScontoText = sTmp.Trim();
                                break;

                            case (int)DISC_TYPE.DISC_FIXED:
                                s_pos = sInStr.IndexOf(';');

                                if (s_pos == -1)
                                    ErrorManager(_ErrMsg);

                                sTmp = sInStr.Substring(4, s_pos - 4);
                                iScontoVal = EuroToInt(sTmp, EURO_CONVERSION.EUROCONV_Z_ERROR, _ErrMsg);

                                sTmp = sInStr.Substring(s_pos + 1);
                                sScontoText = sTmp.Trim();
                                break;

                            case (int)DISC_TYPE.DISC_GRATIS:
                                sTmp = sInStr.Substring(4);
                                sScontoText = sTmp.Trim();
                                break;
                        }

                        if (sScontoText.Length > MAX_COPIES_TEXT_CHARS)
                        {
                            _ErrMsg.iErrID = ERR_STL;
                            ErrorManager(_ErrMsg);  // stringa troppo lunga
                        }

                        if (!CheckService(Define._AUTO_SEQ_TEST))
                            SetSconto((DISC_TYPE)iDiscType, iScontoFlag, iScontoVal, sScontoText);

                        continue;
                    }
                    else if (sInStr.StartsWith("#PN"))
                    {
                        // ****	Page TABS ****

                        sTmp = sInStr.Substring(3, 1);

                        iPageNum = ToInt32(sTmp);

                        if ((iPageNum >= 0) && (iPageNum < PAGES_NUM_TABM) && (sInStr.Length > 5))
                        {
                            sTmp = sInStr.Substring(5);
                            SF_Data.sPageTabs[iPageNum] = sTmp.Trim();

                            if (sTmp.Length > MAX_PAGES_CHAR)
                            {
                                _ErrMsg.iErrID = ERR_STL;
                                ErrorManager(_ErrMsg);  // stringa troppo lunga
                            }
                        }

                        continue;
                    }
                    else if (sInStr.StartsWith("#NR"))
                    {
                        // ****	Numero di righe griglia ****

                        sTmp = sInStr.Remove(0, 3);
                        SF_Data.iGridRows = ToInt32(sTmp);

                        continue;
                    }
                    else if (sInStr.StartsWith("#NC"))
                    {
                        // ****	Numero di colonne griglia ****

                        sTmp = sInStr.Substring(3, 1);
                        SF_Data.iGridCols = ToInt32(sTmp);

                        continue;
                    }
                    else if (sInStr.StartsWith("#TMS") || sInStr.StartsWith("#MS")) // compatibilità ver. prec.
                    {
                        /*****************************
                         *	Touch mode Flag, SET
                         *****************************/

                        SF_Data.bTouchMode = true;

                        continue;
                    }
                    else if (sInStr.StartsWith("#TMC") || sInStr.StartsWith("#MC")) // compatibilità ver. prec.
                    {
                        /*******************************
                         *	Touch mode Flag, CLEAR
                         *******************************/

                        SF_Data.bTouchMode = false;

                        continue;
                    }
                    else if (sInStr.StartsWith("#TVS") || sInStr.StartsWith("#TS")) // compatibilità ver. prec.
                    {
                        /***************************************************
                         *	inserimento Tavolo richiesto Flag SET
                         ***************************************************/

                        SF_Data.bTavoloRichiesto = true;

                        continue;
                    }
                    else if (sInStr.StartsWith("#TVC") || sInStr.StartsWith("#TC")) // compatibilità ver. prec.
                    {
                        /*****************************************************
                         *	inserimento Tavolo non richiesto Flag CLEAR
                         *****************************************************/

                        SF_Data.bTavoloRichiesto = false;

                        continue;
                    }
                    else if (sInStr.StartsWith("#CPS") || sInStr.StartsWith("#CS")) // compatibilità ver. prec.
                    {
                        /*************************************************
                         *	inserimento Coperti richiesto Flag, SET
                         *************************************************/

                        SF_Data.bCopertoRichiesto = true;

                        continue;
                    }
                    else if (sInStr.StartsWith("#CPC") || sInStr.StartsWith("#CC")) // compatibilità ver. prec.
                    {
                        /***************************************************
                         *	inserimento Coperti non richiesto Flag, CLEAR
                         ***************************************************/

                        SF_Data.bCopertoRichiesto = false;

                        continue;
                    }
                    else if (sInStr.StartsWith("#RSS"))
                    {
                        /*************************************************
                         *	riservatezza richiesta Flag, SET
                         *************************************************/

                        SF_Data.bRiservatezzaRichiesta = true;

                        continue;
                    }
                    else if (sInStr.StartsWith("#RSC"))
                    {
                        /***************************************************
                         *	riservatezza non richiesta Flag, CLEAR
                         ***************************************************/

                        SF_Data.bRiservatezzaRichiesta = false;

                        continue;
                    }
                    else if (sInStr.StartsWith("#PVS") || sInStr.StartsWith("#PS")) // compatibilità ver. prec.
                    {
                        /*************************************************
                         *	inserimento Prevendita richiesto Flag, SET
                         *************************************************/
                        SF_Data.bPrevendita = true;

                        continue;
                    }
                    else if (sInStr.StartsWith("#PVC") || (sInStr.StartsWith("#PC") && !sInStr.StartsWith("#PCP"))) // compatibilità ver. prec.
                    {
                        /******************************************************
                         *	inserimento Prevendita non richiesto Flag, CLEAR
                         ******************************************************/

                        SF_Data.bPrevendita = false;

                        continue;
                    }
                    else if (sInStr.StartsWith("#BCS") || sInStr.StartsWith("#BCC")) // compatibilità ver. prec.
                    {
                        /***************************************************
                         *	inserimento Barcode non richiesto Flag, CLEAR
                         ***************************************************/

                        SF_Data.iBarcodeRichiesto = 0;

                        continue;
                    }
                    else if (sInStr.StartsWith("#BC"))
                    {
                        /******************************************************
                         *	caricamento per gestione barcode nelle copie
                         ******************************************************/
                        if (sInStr.Length > 3)
                        {
                            sTmp = sInStr.Substring(3);

                            i = ToInt32(sTmp);

                            SF_Data.iBarcodeRichiesto = i;
                        }
                        else
                            SF_Data.iBarcodeRichiesto = 0;

                        continue;
                    }
                    else if (sInStr.StartsWith("#TN"))
                    {
                        /******************************************************************
                         *	caricamento hex per gestione della stampa copie e QuantitàUno 
                         *	nella copia Scontrino senza prezzo
                         ******************************************************************/
                        sTmp = sInStr.Substring(3);

                        i = ToInt32(sTmp, 16);

                        SF_Data.iReceiptCopyOptions = i;

                        sGlbWinPrinterParams.bChars33 = IsBitSet(SF_Data.iReceiptCopyOptions, BIT_CHARS33_PRINT_REQUIRED);

                        iMAX_RECEIPT_CHARS = sGlbWinPrinterParams.bChars33 ? MAX_ABS_RECEIPT_CHARS : MAX_LEG_RECEIPT_CHARS;
                        iMAX_ART_CHAR = sGlbWinPrinterParams.bChars33 ? MAX_ABS_ART_CHAR : MAX_LEG_ART_CHAR;

                        continue;
                    }
                    else if (sInStr.StartsWith("#MPS"))
                    {
                        /****************************************************
                         *	inserimento Modo pagamento richiesto Flag, SET
                         ****************************************************/

                        SF_Data.bModoPagamRichiesto = true;

                        continue;
                    }
                    else if (sInStr.StartsWith("#MPC"))
                    {
                        /******************************************************
                         *	inserimento Modo pagam. non richiesto Flag, CLEAR
                         ******************************************************/

                        SF_Data.bModoPagamRichiesto = false;

                        continue;
                    }
                    else if (sInStr.StartsWith("#LF"))
                    {
                        // ****	Line feed ****

                        sTmp = sInStr.Substring(3);

                        iLineFeed = ToInt32(sTmp);

                        for (i = 0; i < iLineFeed; i++)
                            iRiga++;

                        continue;
                    }
                    else if (sInStr.StartsWith("#PCP"))
                    {
                        // ****	importo dei coperti ****

                        uWebHashCode += Hash(sInStr);

                        s_pos = sInStr.IndexOf(';');    // ricerca prima semicolon

                        if (s_pos == -1)
                        {
                            _ErrMsg.iErrID = ERR_SNF;
                            ErrorManager(_ErrMsg);
                        }

                        sInStr = sInStr.Remove(0, s_pos + 1);
                        sInStr = sInStr.Trim();

                        /***************************************
                         *      ricerca del prezzo in Euro
                         ***************************************/
                        s_pos = sInStr.IndexOf(';');    // ricerca seconda semicolon
                        sEuro = sInStr.Substring(0, s_pos);

                        iCopertoVal = EuroToInt(sEuro, EURO_CONVERSION.EUROCONV_Z_ERROR, _ErrMsg);

                        SF_Data.Articolo[MAX_NUM_ARTICOLI - 1].iPrezzoUnitario = iCopertoVal;

                        continue;
                    }
                    else if (sInStr.StartsWith("#FS")) // Pandemic form
                        continue;
                    else if (sInStr.StartsWith("#FC")) // Pandemic form
                        continue;

                    // copia prima della modifica di sInStr
                    sInStrCopy = sInStr;

                    /*********************************************************
                     *	imposta numero di possibili Articoli nelle pagine
                     *********************************************************/
                    if (SF_Data.bTouchMode)
                        _iLastArticoloIndex = SF_Data.iGridRows * SF_Data.iGridCols * PAGES_NUM_TABM;
                    else
                        _iLastArticoloIndex = SF_Data.iGridRows * SF_Data.iGridCols * PAGES_NUM_TXTM;

                    // se la griglia di default è troppo piccola si porta alle dimensioni massime
                    if ((iRiga >= _iLastArticoloIndex) && !String.IsNullOrEmpty(sInStr))
                    {
                        LogToFile("Datamanager : aumento dimensioni griglia");

                        if (SF_Data.bTouchMode)
                        {
                            SF_Data.iGridRows = MAX_GRID_NROWS_TABM;
                            SF_Data.iGridCols = MAX_GRID_NCOLS_TABM;
                            _iLastArticoloIndex = SF_Data.iGridRows * SF_Data.iGridCols * PAGES_NUM_TABM;
                        }
                        else
                        {
                            SF_Data.iGridRows = MAX_GRID_NROWS_TXTM;
                            SF_Data.iGridCols = MAX_GRID_NCOLS_TXTM;
                            _iLastArticoloIndex = SF_Data.iGridRows * SF_Data.iGridCols * PAGES_NUM_TXTM;
                        }

                        _bListinoModificato = true;

                        if ((iRiga >= MAX_NUM_ARTICOLI) && !String.IsNullOrEmpty(sInStr))
                        {
                            _ErrMsg.iErrID = ERR_NVE;
                            ErrorManager(_ErrMsg); // riga non vuota oltre il limite
                        }
                    }

                    /*****************************************
                     *		ricerca del tipo di articolo
                     *****************************************/
                    s_pos = sInStr.IndexOf(';');    // ricerca prima semicolon

                    if (s_pos == -1)
                    {
                        _ErrMsg.iErrID = ERR_SNF;
                        ErrorManager(_ErrMsg);
                    }

                    sArticoloStr = sInStr.Substring(0, s_pos);
                    sArticoloStr = sArticoloStr.Trim();

                    if (sArticoloStr.Length > MAX_ABS_ART_CHAR)
                    {
                        _ErrMsg.iErrID = ERR_STL;
                        ErrorManager(_ErrMsg);  // stringa troppo lunga con errore
                    }
                    else if ((sArticoloStr.Length > iMAX_ART_CHAR) && (sArticoloStr.Length <= MAX_ABS_ART_CHAR))
                    {
                        _ErrMsg.iErrID = WRN_STL;
                        _ErrMsg.sMsg = sArticoloStr;
                        WarningManager(_ErrMsg);  // stringa troppo lunga con warning
                    }

                    SF_Data.Articolo[iRiga].sTipo = sArticoloStr;
                    sInStr = sInStr.Remove(0, s_pos + 1);
                    sInStr = sInStr.Trim();

                    /***************************************
                     *      ricerca del prezzo in Euro
                     ***************************************/
                    s_pos = sInStr.IndexOf(';');    // ricerca seconda semicolon
                    if (s_pos == -1)
                    {
                        _ErrMsg.iErrID = ERR_SNF;
                        ErrorManager(_ErrMsg);
                    }

                    sEuro = sInStr.Substring(0, s_pos);
                    sEuro = sEuro.Trim();

                    iPrzTmp = EuroToInt(sEuro, EURO_CONVERSION.EUROCONV_Z_ERROR, _ErrMsg);

                    // Tipo vuoto con prezzo non nullo
                    if (String.IsNullOrEmpty(SF_Data.Articolo[iRiga].sTipo) && (iPrzTmp > 0))
                    {
                        _ErrMsg.iErrID = ERR_PVP;
                        ErrorManager(_ErrMsg);
                    }

                    sInStr = sInStr.Remove(0, s_pos + 1);
                    sInStr = sInStr.Trim();

                    /******************************************
                     * compatibilità 4.x
                     * ricerca del prezzo scontato in Euro
                     ******************************************/
                    s_pos = sInStr.IndexOf(';');
                    if (s_pos > 0)
                    {
                        sInStr = sInStr.Remove(0, s_pos + 1);
                        sInStr = sInStr.Trim();
                    }

                    /****************************************
                     *	   ricerca del Gruppo di stampa
                     ****************************************/
                    iDest = ToInt32(sInStr);
                    if ((iDest >= 0) && (iDest < NUM_COPIES_GRPS))
                    {
                        SF_Data.Articolo[iRiga].iGruppoStampa = iDest;
                        SF_Data.Articolo[iRiga].iPrezzoUnitario = iPrzTmp;

                        if (iDest != (int)DEST_TYPE.DEST_TIPO8)
                            uWebHashCode += Hash(sInStrCopy, iRiga);
                        else
                            sTmp = sInStrCopy;
                    }
                    else if (iDest == NUM_COPIES_GRPS)
                    {
                        // compatibilità Listino 5.9.6
                        SF_Data.Articolo[iRiga].iGruppoStampa = NUM_COPIES_GRPS - 1;
                        SF_Data.Articolo[iRiga].iPrezzoUnitario = iPrzTmp;
                    }
                    else
                    {
                        _ErrMsg.iErrID = ERR_EDS;
                        ErrorManager(_ErrMsg);
                    }

                    if (CheckService(Define._AUTO_SEQ_TEST))
                        LogToFile(String.Format("DataManager : CaricaListino {0,-20} {1}", sArticoloStr, iRiga));

                    iRiga++; // ********* riga successiva *********
                } // end for

                if (!bChecksumTrovato)
                {
                    _ErrMsg.iErrID = WRN_CKD;
                    WarningManager(_ErrMsg);
                }

                // ricerca tipi uguali che non siano stringhe vuote
                for (i = 0; i < MAX_NUM_ARTICOLI; i++)
                    for (j = i + 1; j < MAX_NUM_ARTICOLI; j++)
                        if ((SF_Data.Articolo[i].sTipo == SF_Data.Articolo[j].sTipo) && !String.IsNullOrEmpty(SF_Data.Articolo[i].sTipo))
                        {
                            _ErrMsg.sMsg = SF_Data.Articolo[i].sTipo;
                            _ErrMsg.iErrID = ERR_VRP;
                            ErrorManager(_ErrMsg);
                        }

                // pubblicità al sito
                if (String.IsNullOrEmpty(SF_Data.sHeaders[3]))
                    SF_Data.sHeaders[3] = URL_SITO;

                LogToFile("DataManager : F CaricaListino");

                /************************************************************************
                 *	verifica coerenza DB tabella "Listino" : solo la CASSA_PRINCIPALE
                 *	e con Database DB può riscrivere la tabella quando il checksum
                 *  è coerente ma non corrisponde
                 ************************************************************************/

                // if (_bDB_InitialConnectionOk && _bChecksumListinoCoerente && bUSA_NDB())
                if (_bDB_InitialConnectionOk && bUSA_NDB()) // meglio salvare anche con listino incoerente per evitare disallineamenti !
                {
                    sDBChecksum = _rdBaseIntf.dbCheckListino(); // se il listino non esiste ritorna una stringa vuota

                    if ((sDBChecksum != _sLocListinoChecksum) && (SF_Data.iNumCassa == CASSA_PRINCIPALE))
                    {
                        LogToFile("DataManager : dbSalvaListino() per checksum non corrispondente");
                        _rdBaseIntf.dbSalvaListino();
                    }
                }

                //rdBaseTunnel_my.rdbSalvaListino(); // debug

                if (_bDB_InitialConnectionOk && _bChecksumListinoCoerente && dBaseTunnel_my.GetWebServiceReq())
                {
                    if (dBaseTunnel_my.rdbPing())
                    {
                        _sRemDBChecksum = dBaseTunnel_my.rdbCheckListino(2000); // se il listino non esiste ritorna una stringa vuota

                        if (!String.IsNullOrEmpty(_sRemDBChecksum) && (_sRemDBChecksum != _sWebListinoChecksum) && (SF_Data.iNumCassa == CASSA_PRINCIPALE))
                        {
                            LogToFile(String.Format("DataManager: _sRemDBChecksum={0}, _sWebListinoChecksum={1}", _sRemDBChecksum, _sWebListinoChecksum));
                            LogToFile("DataManager: rdbSalvaListino() per checksum non corrispondente");

                            // avvia rdbSalvaListino()
                            sQueue_Object[0] = WEB_PRICELIST_LOAD_START;
                            sQueue_Object[1] = "";

                            dBaseTunnel_my.EventEnqueue(sQueue_Object);
                        }
                    }
                }
            }

            catch (Exception)
            {
                // Errore di conversione
                _ErrMsg.iErrID = ERR_CNV;
                _ErrMsg.sNomeFile = NOME_FILE_LISTINO;
                ErrorManager(_ErrMsg);
                return false;
            }

            return true;
        } // end CaricaListino()

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
            if (CheckIf_CassaSec_and_NDB() || CheckService(Define._AUTO_SEQ_TEST)) // cassa secondaria e DB, seqTest
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
                if (!String.IsNullOrEmpty(SF_Data.Articolo[i].sTipo) && (SF_Data.Articolo[i].iPrezzoUnitario > 0))
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

                // sPrzRow = "#DT gio 03/08/23 18.24.00"; // debug

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

                    if (i != (int)DEST_TYPE.DEST_TIPO8)
                    {
                        // ritaglia la parte di interesse web
                        sGroupName = sPrzRow[3] + sPrzRow.Substring(5);
                        uWebHashCode += Hash(sGroupName);
                    }

                    Console.WriteLine("DataManager : uLocHashCode = {0}, uWebHashCode = {1}, sPrzRow = {2}", uLocHashCode, uWebHashCode, sPrzRow);
                }

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
                int iDebug = GetSconto().iStatusSconto & 0x0000FF00;

                fPrz.WriteLine(";\n; sconti");
                iRowIndex += 2;

                sPrzRow = String.Format("#SC0 {0}; {1}; {2}", GetSconto().iScontoValPerc, (GetSconto().iStatusSconto & 0x0000FF00) >> 8, GetSconto().sScontoText[(int)DISC_TYPE.DISC_STD]);
                fPrz.WriteLine(sPrzRow);
                uLocHashCode += Hash(sPrzRow);

                sPrzRow = String.Format("#SC1 {0}; {1}", IntToEuro(GetSconto().iScontoValFisso), GetSconto().sScontoText[(int)DISC_TYPE.DISC_FIXED]);
                fPrz.WriteLine(sPrzRow);
                uLocHashCode += Hash(sPrzRow);

                sPrzRow = String.Format("#SC2 {0}", GetSconto().sScontoText[(int)DISC_TYPE.DISC_GRATIS]);
                fPrz.WriteLine(sPrzRow);
                uLocHashCode += Hash(sPrzRow);
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

                // Flag modo Touch
                if (SF_Data.bTouchMode)
                    sPrzRow = "#TMS"; // Set
                else
                    sPrzRow = "#TMC"; // Clear

                fPrz.WriteLine(sPrzRow);
                iRowIndex++;
                uLocHashCode += Hash(sPrzRow);

                // Flag Tavolo richiesto Set/Clear
                if (SF_Data.bTavoloRichiesto)
                    sPrzRow = "#TVS"; // Set
                else
                    sPrzRow = "#TVC"; // Clear

                fPrz.WriteLine(sPrzRow);
                iRowIndex++;
                uLocHashCode += Hash(sPrzRow);

                // Flag Coperti richiesto Set/Clear
                if (SF_Data.bCopertoRichiesto)
                    sPrzRow = "#CPS"; // Set
                else
                    sPrzRow = "#CPC"; // Clear

                fPrz.WriteLine(sPrzRow);
                iRowIndex++;
                uLocHashCode += Hash(sPrzRow);

                // Flag Riservatezza richiesta Set/Clear
                if (SF_Data.bRiservatezzaRichiesta)
                    sPrzRow = "#RSS"; // Set
                else
                    sPrzRow = "#RSC"; // Clear

                fPrz.WriteLine(sPrzRow);
                iRowIndex++;
                uLocHashCode += Hash(sPrzRow);

                // Flag Prevendita richiesto Set/Clear
                if (SF_Data.bPrevendita)
                    sPrzRow = "#PVS"; // Set
                else
                    sPrzRow = "#PVC"; // Clear

                fPrz.WriteLine(sPrzRow);
                iRowIndex++;
                uLocHashCode += Hash(sPrzRow);

                // Flag Modo pagamento richiesto Set/Clear
                if (SF_Data.bModoPagamRichiesto)
                    sPrzRow = "#MPS"; // Set
                else
                    sPrzRow = "#MPC"; // Clear

                fPrz.WriteLine(sPrzRow);
                iRowIndex++;
                uLocHashCode += Hash(sPrzRow);

                //numero per gestione barcode nelle copie
                sPrzRow = String.Format("{0}{1}", "#BC", SF_Data.iBarcodeRichiesto);

                fPrz.WriteLine(sPrzRow);
                iRowIndex++;
                uLocHashCode += Hash(sPrzRow);

                //numero hex per gestione della stampa QuantitàUno nella copia senza prezzo
                sPrzRow = String.Format("{0}{1:X5}", "#TN", SF_Data.iReceiptCopyOptions);

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

                    if ((SF_Data.Articolo[i].iPrezzoUnitario > 0) || (SF_Data.Articolo[i].iGruppoStampa == (int)DEST_TYPE.DEST_COUNTER))
                    {
                        sPrzRow = String.Format(sDAT_FMT_PRL, SF_Data.Articolo[i].sTipo, IntToEuro(SF_Data.Articolo[i].iPrezzoUnitario),
                                    SF_Data.Articolo[i].iGruppoStampa);

                        uLocHashCode += Hash(sPrzRow);

                        if (SF_Data.Articolo[i].iGruppoStampa != (int)DEST_TYPE.DEST_TIPO8)
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
                    if ((i == MAX_NUM_ARTICOLI) && (SF_Data.Articolo[i].iPrezzoUnitario > 0))
                        fData.WriteLine();

                    if ((SF_Data.Articolo[i].iPrezzoUnitario > 0) || (SF_Data.Articolo[i].iGruppoStampa == (int)DEST_TYPE.DEST_COUNTER))
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

/**********************************************************************
    NomeFile : StandFacile/DataManager.cs
	Data	 : 18.08.2025
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

             - 'iGruppoStampa' può valere da 0 a NUM_SEP_PRINT_GROUPS-1 in modo da
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
            int iMaxWrnCount = 3;

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
                    if (File.Exists(sDir + NOME_FILE_LISTINO) && !CheckService(Define.CFG_SERVICE_STRINGS._AUTO_SEQ_TEST))
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

                            // iMAX_RECEIPT_CHARS viene caricato successivamente !
                            if (SF_Data.sHeaders[hIndex].Length > MAX_ABS_RECEIPT_CHARS)
                            {
                                _ErrMsg.iErrID = WRN_STL;
                                WarningManager(_ErrMsg);  // stringa troppo lunga
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
                            if ((i != (int)DEST_TYPE.DEST_TIPO9_NOWEB) && (i != (int)DEST_TYPE.DEST_BUONI))
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
                        else if (i == NUM_EDIT_GROUPS)
                        {
                            SF_Data.bCopiesGroupsFlag[i] = true;
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
                            if ((i != (int)DEST_TYPE.DEST_TIPO9_NOWEB) && (i != (int)DEST_TYPE.DEST_BUONI))
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
                        else if (i == NUM_EDIT_GROUPS)
                        {
                            SF_Data.bCopiesGroupsFlag[i] = false;
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
                        //uWebHashCode += Hash(sInStr);

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
                                iScontoFlag = Convert.ToInt32(sTmp.Trim(), 16); // conversione hex

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
                        continue;
                    }
                    else if (sInStr.StartsWith("#TMC") || sInStr.StartsWith("#MC")) // compatibilità ver. prec.
                    {

                        continue;
                    }
                    else if (sInStr.StartsWith("#TVS") || sInStr.StartsWith("#TS")) // compatibilità ver. prec.
                    {
                        continue;
                    }
                    else if (sInStr.StartsWith("#TVC") || sInStr.StartsWith("#TC")) // compatibilità ver. prec.
                    {
                        continue;
                    }
                    else if (sInStr.StartsWith("#CPS") || sInStr.StartsWith("#CS")) // compatibilità ver. prec.
                    {
                        continue;
                    }
                    else if (sInStr.StartsWith("#CPC") || sInStr.StartsWith("#CC")) // compatibilità ver. prec.
                    {
                        continue;
                    }
                    else if (sInStr.StartsWith("#MPS") || sInStr.StartsWith("#MPC")) // compatibilità ver. prec.
                    {
                        continue;
                    }
                    else if (sInStr.StartsWith("#RSS") || sInStr.StartsWith("#RSC")) // compatibilità ver. prec.
                    {
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

                            i = ToInt32(sTmp, 16);

                            SF_Data.iBarcodeRichiesto = i;
                        }
                        else
                            SF_Data.iBarcodeRichiesto = 0;

                        continue;
                    }
                    else if (sInStr.StartsWith("#GO"))
                    {
                        /****************************************************
                         *	caricamento hex per gestione opzioni generali
                         ****************************************************/
                        sTmp = sInStr.Substring(3);

                        i = ToInt32(sTmp, 16);

                        SF_Data.iGeneralOptions = i;

                        continue;
                    }
                    else if (sInStr.StartsWith("#GP")) // GP == General Print
                    {
                        /***********************************************************
                         *	caricamento hex per gestione opzioni stampa generica
                         ***********************************************************/
                        sTmp = sInStr.Substring(3);

                        i = ToInt32(sTmp, 16);

                        SF_Data.iGenericPrinterOptions = i;

                        continue;
                    }
                    else if (sInStr.StartsWith("#TN") || sInStr.StartsWith("#LC")) // compatibilità
                    {
                        /******************************************************************
                         *	caricamento hex per gestione della stampa copie Locali
                         ******************************************************************/
                        sTmp = sInStr.Substring(3);

                        i = ToInt32(sTmp, 16);

                        SF_Data.iReceiptCopyOptions = i;

                        sGlbWinPrinterParams.bChars33 = IsBitSet(SF_Data.iReceiptCopyOptions, (int)LOCAL_COPIES_OPTS.BIT_CHARS33_PRINT_REQUIRED);

                        iMAX_RECEIPT_CHARS = sGlbWinPrinterParams.bChars33 ? MAX_ABS_RECEIPT_CHARS : MAX_LEG_RECEIPT_CHARS;
                        iMAX_ART_CHAR = sGlbWinPrinterParams.bChars33 ? MAX_ABS_ART_CHAR : MAX_LEG_ART_CHAR;

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
                    if (IsBitSet(SF_Data.iGeneralOptions, (int)GEN_OPTS.BIT_TOUCH_MODE_REQUIRED))
                        _iLastArticoloIndex = SF_Data.iGridRows * SF_Data.iGridCols * PAGES_NUM_TABM;
                    else
                        _iLastArticoloIndex = SF_Data.iGridRows * SF_Data.iGridCols * PAGES_NUM_TXTM;

                    // se la griglia di default è troppo piccola si porta alle dimensioni massime
                    if ((iRiga >= _iLastArticoloIndex) && !String.IsNullOrEmpty(sInStr))
                    {
                        LogToFile("Datamanager : aumento dimensioni griglia");

                        if (IsBitSet(SF_Data.iGeneralOptions, (int)GEN_OPTS.BIT_TOUCH_MODE_REQUIRED))
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
                        if (iMaxWrnCount > 0)
                        {
                            iMaxWrnCount--;

                            _ErrMsg.iErrID = WRN_STL;
                            _ErrMsg.sMsg = sArticoloStr;
                            WarningManager(_ErrMsg);  // stringa troppo lunga con warning
                        }
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

                        if ((iDest != (int)DEST_TYPE.DEST_TIPO9_NOWEB) && (iDest != (int)DEST_TYPE.DEST_BUONI))
                            uWebHashCode += Hash(sInStrCopy, iRiga);
                        else
                            sTmp = sInStrCopy;
                    }
                    else if (iDest == NUM_COPIES_GRPS)
                    {
                        // compatibilità Listino 5.9.6
                        SF_Data.Articolo[iRiga].iGruppoStampa = NUM_SEP_PRINT_GROUPS;
                        SF_Data.Articolo[iRiga].iPrezzoUnitario = iPrzTmp;
                    }
                    else
                    {
                        _ErrMsg.iErrID = ERR_EDS;
                        ErrorManager(_ErrMsg);
                    }

                    if (CheckService(Define.CFG_SERVICE_STRINGS._AUTO_SEQ_TEST))
                        LogToFile(String.Format("DataManager : CaricaListino {0,-20} {1}", sArticoloStr, iRiga));

                    iRiga++; // ********* riga successiva *********
                } // end for

                /*********************************************************
                 *	controllo coerenza TouchMode e numero di righe
                 *	perchè c'è il rischio di non vedere gli Articoli
                 *********************************************************/
                LogToFile(String.Format("DataManager : CaricaListino prima di Check iGridRows = {0}, iGridCols = {1}", SF_Data.iGridRows, SF_Data.iGridCols));

                SF_Data.iGridRows = EditGridDlg.CheckGridRows(SF_Data.iGridRows, IsBitSet(SF_Data.iGeneralOptions, (int)GEN_OPTS.BIT_TOUCH_MODE_REQUIRED));
                SF_Data.iGridCols = EditGridDlg.CheckGridCols(SF_Data.iGridCols, IsBitSet(SF_Data.iGeneralOptions, (int)GEN_OPTS.BIT_TOUCH_MODE_REQUIRED));

                LogToFile(String.Format("DataManager : CaricaListino dopo di Check iGridRows = {0}, iGridCols = {1}", SF_Data.iGridRows, SF_Data.iGridCols));


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

    } // end class
} // end namespace

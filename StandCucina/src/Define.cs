/********************************************************************
	 NomeFile : StandCucina/Define.cs
	 Data	  : 14.08.2023
	 Autore	  : Mauro Artuso

 ********************************************************************/

using System;

namespace StandFacile
{
    /// <summary>define specifici a StandCucina</summary>
    public struct Define
    {
        /// <summary>titolo</summary>
        public const string TITLE = "Stand Cucina 2025";

        /// <summary>numero minimo di righe visualizzate</summary>
        public const int MIN_ROWS_NUMBER = 15;

        /// <summary>File temporaneo di stampa Scontrino</summary>
        public const string NOME_FILE_STAMPA_RECEIPT = "StampaTckTmp.txt";
        /// <summary>File temporaneo di stampa messaggio</summary>
        public const string NOME_FILE_STAMPA_MSG    = "StampaMsgTmp.txt";

        /// <summary>nome dell'eseguibile</summary>
        public const string THE_APP = "StandCucina.exe";

        /// <summary>nome del manuale pdf</summary>
        public const string _NOME_MANUALE = "Manuale_StandCucina.pdf";

        /// <summary>mnemonici per percorso chiave di registro</summary>
        public const string KEY_STAND_CUCINA = "HKEY_CURRENT_USER\\Software\\StandCucina";
        /// <summary>copia selezionata per la stampa</summary>
        public const string SEL_TCP_COPY_KEY = "iTcpSelectCopy";

        /// <summary>chiave che descrive se è richiesta la stampa del barcode</summary>
        public const String STAMPA_MANUALE_KEY = "iStampaManuale";

        /// <summary> 
        /// refresh di sService * 250ms circa in StandFacile, 200 in StandClient
        /// </summary>
        public const int REFRESH_SSERVICE = 5 * 60;

        /// <summary>timer del client</summary>
        public const int DB_CLIENT_TIMER        = 16 * 1000;  // 16s
        /// <summary>timer breve del client</summary>
        public const int DB_CLIENT_TIMER_SHORT  = 4 * 1000;

        /// <summary>timer per lampeggio LED con stampante windows</summary>
        public const int LED_TIMER = 3 * 10;

        //mnemonici per i nomi dei files provvisori
        /// <summary>nome locale per il file dello scontrino</summary>
        public const string NOME_FILE_RECEIPT = "TT_tmp.txt";
        /// <summary>formato nome dello scontrino senza prezzi</summary>
        public const string NOME_FILE_RECEIPT_NP = "TN_tmp.txt";

        /// <summary>nome locale per il file delle copie</summary>
        public const string NOME_FILE_COPIE     = "CT_G{0}.txt";

       /// <summary>nome locale per il file del messaggio</summary>
        public const string NOME_FILE_MESSAGGIO = "Msg_tmp.txt";

        // eventi
        /// <summary>evento accensione led stampa</summary>
        public const string UPDATE_COM_LED_EVENT = "updateCOMLedEvent";
        /// <summary>evento aggiornamento nome del DB</summary>
        public const string UPDATE_DB_LABEL_EVENT = "updateDBLabelEvent";

    } // end class
}

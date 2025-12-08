/********************************************************************
	NomeFile : StandMonitor/Define.h
	Data	 : 05.12.2025
	Autore	 : Mauro Artuso
 ********************************************************************/

using System;

namespace StandFacile
{
    /// <summary>
    /// classe di define specifici di StandMonitor
    /// </summary>
    public struct Define
    {
        /// <summary>titolo</summary>
        public const string TITLE = "Stand Monitor 2026";

        /// <summary>nome dell'eseguibile</summary>
        public const string THE_APP = "StandMonitor.exe";

        /// <summary>nome del pdf del manuale</summary>
        public const string _NOME_MANUALE = "Manuale_StandMonitor.pdf";

        /// <summary>chiave di registro</summary>
        public const string KEY_STAND_MONITOR = "HKEY_CURRENT_USER\\Software\\StandMonitor";

        /// <summary>chiave che descrive dove memorizzare le opzioni di stampa generica</summary>
        public const String GEN_PRINT_LOC_STORE_KEY = "iGenPrintLocStoreSettings";

        /// <summary>chiave che descrive le opzioni locali di stampa generica</summary>
        public const String GEN_PRINT_OPT_KEY = "iGenPrintSettings";

        /// <summary>AuxRefresh tabella ogni 30s</summary>
        public static int REFRESH_TIMER = 30 * 4;  // timer da 250ms

        /// <summary>AuxRefresh di sService * 250ms circa in StandFacile, 200 in StandClient</summary>
        public const int REFRESH_SSERVICE = 4 * 20;

        /// <summary>dimensioni larghezza della Main Window</summary>
        public const int MAINWD_WIDTH = 800;

        /// <summary>dimensioni altezza della Main Window</summary>
        public const int MAINWD_HEIGHT = 600;

        /// <summary>nome del file di filtro</summary>
        public const String FILE_FILTRO = "Filtro.txt";

        /// <summary>numero di righe minimo del file filtro</summary>
        public const int FILTRO_MIN_LENGTH = 3;

        /// <summary>formato nome dello scontrino</summary>
        public const String NOME_FILE_RECEIPT = "tmpTicket.txt";

        /// <summary>formato nome dello scontrino senza prezzi</summary>
        public const string NOME_FILE_RECEIPT_NP = "tmpTicket_NP.txt";

        /**********************************************************************************
           mnemonici chiavi del file config.ini:
           queste stringhe consentono l'attivazione di alcune funzioni speciali
         **********************************************************************************/

        /// <summary>
        ///  mnemonici chiavi del file config.ini:<br/>
        /// queste stringhe consentono l'attivazione di alcune funzioni di debug
        /// </summary>
        public struct CFG_SERVICE_STRINGS
        {
            /// <summary>abilita la modalità superUser</summary>
            public const String _SUPER_USER = "superUser";

            /// <summary>viene nascosta la colonna "Q.ta venduta"</summary>
            public const String _REDUCE_COLUMNS = "reducedColumns";

            /// <summary>ordinata secondo la colonna "da consegnare"</summary>
            public const String _SORT_DELIVER = "sortByDeliver";
        }

    } // end class
}

/********************************************************************
	NomeFile : StandMonitor/Define.h
	Data	 : 06.12.2024
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

    } // end class
}

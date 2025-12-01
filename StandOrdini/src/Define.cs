/********************************************************************
  	NomeFile  : StandOrdini/Define.h
	Data	 : 06.12.2024
  	Autore	: Mauro Artuso

 ********************************************************************/

using System;

namespace StandFacile
{
    /// <summary>
    /// classe di define specifici di StandOrdini
    /// </summary>
    public struct Define
    {
        /// <summary>titolo</summary>
        public const string TITLE = "Stand Ordini 2026";

        /// <summary>nome dell'eseguibile</summary>
        public const string THE_APP = "StandOrdini.exe";

        /// <summary>nome del pdf del manuale</summary>
        public const string _NOME_MANUALE = "Manuale_StandOrdini.pdf";

        /// <summary>chiave di registro</summary>
        public const string KEY_STAND_ORDINI = "HKEY_CURRENT_USER\\Software\\StandOrdini";

        /// <summary>chiave di registro</summary>
        public const string KEY_FUNC_MODE = "funcMode";

        /// <summary>refresh di sService * 250ms circa in StandFacile, 200 in StandClient</summary>
        public const int REFRESH_SSERVICE = 4 * 60;

        /// <summary>aspetto pannelli, max numero di righe</summary>
        public const int MAX_RIGHE = 3;
        /// <summary>aspetto pannelli, max numero di blink</summary>
        public const int MAX_BLINK = 60 * 4; // timer da 250ms

        /// <summary>tentativi di scarico ordine</summary>
        public const int MAX_RETRY = 15 * 4; // timer da 250ms

        /// <summary>dimensioni larghezza della Main Window</summary>
        public const int MAINWD_WIDTH = 640;
        /// <summary>dimensioni altezza della Main Window</summary>
        public const int MAINWD_HEIGHT = 480;

        // eventi
        /// <summary>evento di scarico ordine dal database</summary>
        public const string SCARICO_DB_EVENT = "scaricoDB_Event";

        /// <summary>costruttore struct per la gestione degli eventi</summary>
        public struct TQueue_Obj
        {
            /// <summary>TQueue_Obj: stringa identificativa dell'evento</summary>
            public String sEvent;
            /// <summary>TQueue_Obj: numero dello scontrino</summary>
            public int iNumTicket;
            /// <summary>TQueue_Obj: numero del Gruppo</summary>
            public int iGruppo;

            /// <summary>costruttore struct per la gestione degli eventi</summary>
            public TQueue_Obj(String sMsgPrm, int iNumTicketPrm, int iGruppoPrm)
            {
                sEvent = sMsgPrm;
                iNumTicket = iNumTicketPrm;
                iGruppo = iGruppoPrm;
            }
        };

    /// <summary>
    /// enum per distinguere il tipo di database
    /// </summary>
    public enum FUNC_MODE
        {
            /// <summary>FUNC_MODE: SCARICO_DB - DEFAULT</summary>
            SCARICO_DB = 0,
            /// <summary>FUNC_MODE: SOLO LETTURA BARCODE</summary>
            SOLO_LETTURA_BC,
            /// <summary>FUNC_MODE: VISUALIZZA ULTIMI ORDINI DEL DB</summary>
            DUPLICAZIONE_MONITOR
        };

    } // end class
}

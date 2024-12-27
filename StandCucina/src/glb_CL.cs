/*****************************************
 	NomeFile : StandCucina/glb_CL.cs
	Data	 : 06.12.2024
 	Autore   : Mauro Artuso
 *****************************************/

using System;
using static StandCommonFiles.ComDef;

namespace StandFacile
{
#pragma warning disable IDE1006

    /// <summary>
    /// classe per la gestione delle variabili globali
    /// </summary>
    public struct glb
    {
        /// <summary>numero massimo di caratteri per riga</summary>
        public static int iMAX_RECEIPT_CHARS;

        /// <summary>numero massimo di caratteri per articolo</summary>
        public static int iMAX_ART_CHAR;

        /// <summary>numero totali dei scontrini</summary>
        public static int iGlbNumOfTickets;

        /// <summary>numero totali dei messaggi</summary>
        public static int iGlbNumOfMessages;

        /// <summary>Struct fondamentale</summary>
        public static TData SF_Data = new TData(0);

        /// <summary>tipo di stampante Windows o Legacy</summary>
        public static int iSysPrinterType;

        /// <summary>Struct per la gestione della stampa Windows</summary>
        public static TWinPrinterParams sGlbWinPrinterParams = new TWinPrinterParams(0);
        /// <summary>Struct per la gestione della stampa Legacy</summary>
        public static TLegacyPrinterParams sGlbLegacyPrinterParams = new TLegacyPrinterParams();

        /// <summary>stringa di root path</summary>
        public static String sRootDir;

    }
}

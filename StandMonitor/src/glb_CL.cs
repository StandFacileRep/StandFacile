/*****************************************
  NomeFile : StandMonitor/glb_CL.cs
  Data	   : 23.11.2023
  Autore   : Mauro Artuso
 *****************************************/

using System;
using System.Collections.Generic;
using static StandCommonFiles.ComDef;

namespace StandFacile
{
    /// <summary>
    /// classe che contitne le variabili globali
    /// </summary>
    public struct glb
    {
        /// <summary>ottiene il flag di superuser</summary>
        public static bool bSuperUser;

        /// <summary>numero massimo di caratteri per riga</summary>
        public static int iMAX_RECEIPT_CHARS;

        /// <summary>Struct fondamentale</summary>
        public static TData SF_Data = new TData(0);

        /// <summary>descrive se Windows o Legacy</summary>
        public static int iSysPrinterType; // 

        /// <summary>Struct per la gestione della stampa Windows</summary>
        public static TWinPrinterParams sGlbWinPrinterParams = new TWinPrinterParams(0);
        /// <summary>Struct per la gestione della stampa Legacy</summary>
        public static TLegacyPrinterParams sGlbLegacyPrinterParams = new TLegacyPrinterParams();

        /// <summary>lista stringhe di filtro per il monitor 1</summary>
        public static List<string> sFiltroMon_0;

        /// <summary>lista stringhe di filtro per il monitor 2</summary>
        public static List<string> sFiltroMon_1;

        /// <summary>stringa di root path</summary>
        public static String sRootDir;

    }
}

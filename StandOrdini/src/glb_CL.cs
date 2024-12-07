/*****************************************
 NomeFile : StandOrdini/glb_CL.cs
 Data	  : 24.08.2024
 Autore   : Mauro Artuso
 *****************************************/

using System;
using System.Collections;
using static StandCommonFiles.ComDef;

namespace StandFacile
{
    /// <summary>
    /// classe che contitne le variabili globali
    /// </summary>
    public struct glb
    {
        /// <summary>numero massimo di caratteri per riga</summary>
        public static int iMAX_RECEIPT_CHARS;

        /// <summary>flag di avviso singolo</summary>
        public static bool bWarnOnce;

        /// <summary>Struct fondamentale</summary>
        public static TData SF_Data = new TData(0);
       
        /// <summary>coda per il Log</summary>
        public static Queue logQueue = new Queue();

        /// <summary>coda ordini da scaricare</summary>
        public static Queue ordiniQueue = new Queue();

        /// <summary>stringa di root path</summary>
        public static String sRootDir;
    }
}

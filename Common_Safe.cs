/************************************************************
    NomeFile : StandCommonSrc/StandCommonSafe.cs, versione utente
    Data	 : 30.03.2026
    Autore	 : Mauro Artuso
 ************************************************************/

using System;

namespace StandCommonFiles
{
    /// <summary>
    /// classe di mnemonici di sicurezza per accesso a database
    /// </summary>
    public static class ComSafe
    {
        /**************************************************************** 
        * attenzione: 
        * le funzioni di crittografia App e WebService sono diverse 
        * quindi la stessa stringa viene crittografata in modo diverso
        ****************************************************************/

        /// <summary>password di default</summary>
        public const String DBASE_LAN_PASSWORD = "yTb02JWPxJzKvD5691qi4g=u";

        /// <summary>password crittografata per HTTP</summary>
        public const String WEB_SERVICE_PASSWORD = "9Pv9ZUNNQHjKRgEoz8b1Mg=u";

        /// <summary>chiave per la crittografia della password per http_tunnel</summary>
        public const String CIPHER_KEY = "inserisci_la_chiave_http_tunnel";
      
        /// <summary>prefisso tabelle del db server di test locale</summary>
        public const String PREFIX_DB_LOCAL = "standfacile_rdb";

        /// <summary>sito internet #1 del Programma</summary>
        public const String URL_WEBAPP1 = "https://www.standfacile.org";

        /// <summary>indirizzo del db server remoto #1</summary>
        public const String URL_DB_SERVER1 = "inserisci_indirizzo_db_web_server1";

        /// <summary>prefisso tabelle del db server remoto #1</summary>
        public const String PREFIX_DB_SERVER1 = "inserisci_db_prefix1";

        /// <summary>sito internet #2 del Programma</summary>
        public const String URL_WEBAPP2 = "inserisci_url_tuo_sito/wa"; // wa indica subdir WebApp

        /// <summary>indirizzo del db server remoto #2</summary>
        public const String URL_DB_SERVER2 = "inserisci_indirizzo_db_web_server2";

        /// <summary>prefisso tabelle del db server remoto #2</summary>
        public const String PREFIX_DB_SERVER2 = "inserisci_db_prefix2";


    } // end struct
} // end namespace


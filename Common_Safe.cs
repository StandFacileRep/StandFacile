/************************************************************
    NomeFile : StandCommonSrc/StandCommonSafe.cs, versione utente
    Data	 : 03.04.2024
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

        /// <summary>indirizzo del db web_server remoto</summary>
        public const String DB_WEB_SERVER = "inserisci_indirizzo_db_web_server";

    } // end struct
} // end namespace


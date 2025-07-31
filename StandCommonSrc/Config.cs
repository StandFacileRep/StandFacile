/*****************************************************
  	NomeFile : StandFacile/Config.cs
	Data	 : 16.07.2025
  	Autore	 : Mauro Artuso
 *****************************************************/

using System;
using System.IO;
using static StandCommonFiles.ComDef;
using static StandCommonFiles.CommonCl;
using static StandCommonFiles.LogServer;

namespace StandFacile
{
    /// <summary>classe per caricamento della configurazione</summary>
    public class Config
    {
#pragma warning disable IDE1006

        /// <summary>nome del file di filtro</summary>
        const String CONFIG_FILE = "config.ini";

        const String sRCP_CS_HEADER = "receipt_CS_AltHeader_AH";

        /// <summary>riferimento a Config</summary>
        public static Config rConfig { get; private set; }

        TErrMsg _ErrMsg;

        /// <summary>costruttore</summary>
        public Config()
        {
            rConfig = this;

            sConfig.iReceiptStartNumber = 1; // giusto 1 e non 0
            sConfig.sService = "";

            sConfig.bRcpCopyRequired = false;
            sConfig.sRcpCopyHeader = "";

            LoadConfig();
        }

        /// <summary>
        /// lettura del file dei configurazione
        /// </summary>
        public void LoadConfig()
        {
            int iPos, iCount, iVal, hIndex;
            String sTmp, sExeDir, sNomeConfigFile, sInStr;
            StreamReader fFiltro;

            iCount = 0;

            // impostazione della directory per il file Prezzi (la stessa dell'eseguibile)
            sExeDir = Directory.GetCurrentDirectory();

            // Directory.SetCurrentDirectory(sExeDir);

#if STANDFACILE
            sNomeConfigFile = DataManager.GetExeDir() + "\\" + CONFIG_FILE;
#else
            sNomeConfigFile = sExeDir + "\\" + CONFIG_FILE;
#endif

            if (File.Exists(sNomeConfigFile))
            {

                fFiltro = File.OpenText(sNomeConfigFile);
                LogToFile("LoadConfig : caricamento");

                // ***** caricamento stringhe dal file Filtro *****
                while (((sInStr = fFiltro.ReadLine()) != null) && (iCount < 100))
                {
                    sInStr = sInStr.Trim();

                    iCount++;

                    // deve stare nel primo if
                    if (sInStr.StartsWith(";"))
                        continue;

                    else if (String.IsNullOrEmpty(sInStr))
                        continue;

                    else if (sInStr.Contains("receiptStartNumber"))
                    {
                        iPos = sInStr.IndexOf('=');    // ricerca prima semicolon
                        sInStr = sInStr.Remove(0, iPos + 1);
                        try
                        {
                            iVal = Convert.ToInt32(sInStr.Trim());

                            if (iVal > 0)
                                sConfig.iReceiptStartNumber = iVal;
                        }
                        catch (Exception)
                        {
                        }

                        continue;
                    }
                    else if (sInStr.Contains("refreshTimer"))
                    {
                        iPos = sInStr.IndexOf('=');    // ricerca prima semicolon
                        sInStr = sInStr.Remove(0, iPos + 1);
                        try
                        {
                            iVal = Convert.ToInt32(sInStr.Trim());

                            if (iVal > 0)
                                sConfig.iRefreshTimer = iVal;
                        }
                        catch (Exception)
                        {
                        }

                        continue;
                    }

                    // stringhe per stampa copia Receipt() completa dei prezzi
                    else if (sInStr.Contains("receiptCopyRequired_HeaderIs"))
                    {
                        iPos = sInStr.IndexOf('=');    // ricerca prima semicolon
                        sInStr = sInStr.Remove(0, iPos + 1).Trim();

                        sConfig.bRcpCopyRequired = true;
                        sConfig.sRcpCopyHeader = sInStr;

                        continue;
                    }

                    // stringhe per stampa Header/Footer Alternativo
                    else if (sInStr.Contains(sRCP_CS_HEADER))
                    {
                        // ha senso solo per la CASSA_SECONDARIA
                        if (glb.SF_Data.iNumCassa == CASSA_PRINCIPALE)
                            continue;

                        try
                        {
                            sInStr = sInStr.Remove(0, sRCP_CS_HEADER.Length).Trim();

                            // ****	Header/Footer ****

                            sTmp = sInStr.Substring(0, 1); // acquisisce indice

                            hIndex = Convert.ToInt32(sTmp);

                            if ((hIndex >= 0) && (hIndex < MAX_NUM_HEADERS) && (sInStr.Length > 3))
                            {
                                sTmp = sInStr.Substring(3).Trim();

                                // iMAX_RECEIPT_CHARS viene caricato successivamente !
                                if (sTmp.Length > MAX_ABS_RECEIPT_CHARS)
                                {
                                    _ErrMsg.iErrID = WRN_CFSTL;
                                    WarningManager(_ErrMsg);  // stringa troppo lunga
                                }
                                else
                                    sConfig.sRcp_CS_Header[hIndex] = sTmp;
                            }
                        }
                        catch (Exception e)
                        { 
                            Console.WriteLine(e.Message);
                        }

                        continue;
                    }

                    // questo if va per ultimo
                    else if (sInStr.Contains("serviceStrings"))
                    {
                        iPos = sInStr.IndexOf('=');    // ricerca prima semicolon
                        sInStr = sInStr.Remove(0, iPos + 1);
                        try
                        {
                            sConfig.sService = sInStr.Trim();
                        }
                        catch (Exception)
                        {
                        }

                        continue;
                    }
                }

                fFiltro.Close();
            }
            else
                LogToFile("LoadConfig : " + CONFIG_FILE + " non trovato");
        }

    }
}

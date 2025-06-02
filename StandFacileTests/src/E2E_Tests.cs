/*****************************************************
 	NomeFile : StandFacileTests/E2E_Tests.cs
	Data	 : 06.12.2024
 	Autore	 : Mauro Artuso

	Classe per Unit Tests
 *****************************************************/

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;

using static StandFacile.Define;
using static StandCommonFiles.ComDef;
using static StandCommonFiles.CommonCl;


namespace StandFacileTests
{

    [TestClass]
    public class StandFacile_E2ETests
    {
#pragma warning disable CS0162

        const int _DAYS_IN_ADVANCE = 0;

        /// <summary>
        /// true: compara ricevute e dati nella cartella di debug,</br>
        /// false: prende i dati da C:/StandFacile
        /// </summary>
        const bool _DEBUG_VERIFY_DIR = true;

        [TestMethod]
        [DataRow(NOME_DIR_RECEIPTS, "")]
        [DataRow(NOME_DIR_COPIES, "")]
        [DataRow(NOME_DIR_RECEIPTS, "100")]
        [DataRow(NOME_DIR_COPIES, "100")]
        public void ReceiptGenerationCompare_IsTrue(string sDirParam, string sStartNumParam)
        {
            InitActualDate();

            // recupera i dati generati in data precedente
            SetActualDate(DateTime.Now.AddDays(_DAYS_IN_ADVANCE));

            bool bResult = true;
            bool bSkipTotale;

            int iCurrentRow;
            int dateCharCountRef, dateCharCountCurrent;
            string sCurrStringTrimmed;
            string sDirRefReceiptData, sDirCurrentReceiptData;

            List<string> sRefStringsList = new List<string>();
            List<string> sCurrStringsList = new List<string>();
            StreamReader fReference;
            StreamReader fCurrent;

            // serve per avere uscita su Console
            Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));

            if (String.IsNullOrEmpty(sStartNumParam))
                sDirRefReceiptData = Directory.GetCurrentDirectory() + "\\..\\..\\refData\\" + sDirParam + ("0420");
            else
                sDirRefReceiptData = Directory.GetCurrentDirectory() + "\\..\\..\\refData\\" + sDirParam + sStartNumParam + ("_0420");

            string[] sAllRefReceiptFiles = Directory.GetFiles(sDirRefReceiptData, "*.*", SearchOption.AllDirectories);

            if (_DEBUG_VERIFY_DIR)
                sDirCurrentReceiptData = Directory.GetCurrentDirectory() + "\\..\\..\\..\\StandFacile\\exe\\StandDati" + "\\" + ANNO_DIR + GetActualDate().ToString("yyyy") + "\\" +
                                                sDirParam + GetActualDate().ToString("MMdd");
            else
            {
                if (String.IsNullOrEmpty(sStartNumParam))
                    sDirCurrentReceiptData = "C:\\StandFacile\\StandDati_0\\" + ANNO_DIR + GetActualDate().ToString("yyyy") + "\\" + sDirParam + GetActualDate().ToString("MMdd");
                else
                    sDirCurrentReceiptData = "C:\\StandFacile\\StandDati_" + sStartNumParam + "\\" + ANNO_DIR + GetActualDate().ToString("yyyy") + "\\" + sDirParam + GetActualDate().ToString("MMdd");
            }
            try
            {
                string[] sAllCurrentReceiptFiles = Directory.GetFiles(sDirCurrentReceiptData, "*.*", SearchOption.AllDirectories);

                // loop esterno con i files di riferimento
                foreach (var referenceFile in sAllRefReceiptFiles)
                {
                    bool bCurrentFoud = false;
                    string sTmp;
                    string sInRefStr, sRefStringTrimmed;

                    fReference = File.OpenText(referenceFile);

                    // loop esterno con i files prodotti correntemente
                    foreach (var currentReceiptFile in sAllCurrentReceiptFiles)
                    {
                        if (Path.GetFileName(currentReceiptFile) != Path.GetFileName(referenceFile))
                            continue;

                        bCurrentFoud = true;

                        string sInCurrentStr;
                        fCurrent = File.OpenText(currentReceiptFile);

                        Trace.WriteLine("****************************");
                        sTmp = string.Format($"*** file: {Path.GetFileName(currentReceiptFile)}");
                        Trace.WriteLine(sTmp);
                        Trace.WriteLine("****************************");

                        sRefStringsList.Clear();
                        sCurrStringsList.Clear();

                        // legge i files e prepara le 2 liste 
                        while (((sInRefStr = fReference.ReadLine()) != null) && (sRefStringsList.Count < 1000))
                            sRefStringsList.Add(sInRefStr);

                        while (((sInCurrentStr = fCurrent.ReadLine()) != null) && (sCurrStringsList.Count < 1000))
                            sCurrStringsList.Add(sInCurrentStr);

                        fReference.Close();
                        fCurrent.Close();

                        iCurrentRow = 0;
                        bSkipTotale = false;

                        foreach (string sRef in sRefStringsList)
                        {
                            if (iCurrentRow >= sCurrStringsList.Count)
                            {
                                sTmp = string.Format($"***  fail *** iCurrentRow > sCurrStringsList.Count");
                                Trace.WriteLine(sTmp);

                                bResult = false;
                                break;
                            }

                            sRefStringTrimmed = sRef.Trim();
                            sCurrStringTrimmed = sCurrStringsList[iCurrentRow].Trim();

                            dateCharCountRef = 0;
                            dateCharCountRef += sRefStringTrimmed.Split('/').Length - 1;
                            dateCharCountRef += sRefStringTrimmed.Split('.').Length - 1;

                            dateCharCountCurrent = 0;
                            dateCharCountCurrent += sCurrStringTrimmed.Split('/').Length - 1;
                            dateCharCountCurrent += sCurrStringTrimmed.Split('.').Length - 1;

                            // contronfo stringhe
                            if (string.Compare(sRefStringTrimmed, sCurrStringTrimmed) == 0)
                                Trace.WriteLine(sCurrStringTrimmed);

                            // contronfo stringhe contenenti la data
                            else if ((dateCharCountRef == dateCharCountCurrent) && (dateCharCountCurrent == 5))
                                Trace.WriteLine(sCurrStringTrimmed);

                            // contronfo stringhe contenenti Annullo
                            else if (sCurrStringTrimmed == sConst_Annullo[0])
                            {
                                Trace.WriteLine(sCurrStringTrimmed);
                                iCurrentRow++;

                                sCurrStringTrimmed = sCurrStringsList[iCurrentRow].Trim();
                                if (sCurrStringTrimmed == sConst_Annullo[1])
                                {
                                    Trace.WriteLine(sCurrStringTrimmed);
                                    iCurrentRow++;

                                    sCurrStringTrimmed = sCurrStringsList[iCurrentRow].Trim();
                                    if (sCurrStringTrimmed == sConst_Annullo[2])
                                    {
                                        Trace.WriteLine(sCurrStringTrimmed);
                                        iCurrentRow++;
                                        iCurrentRow++;
                                    }
                                }
                            }

                            // contronfo stringhe contenenti Articoli non a Listino
                            else if (sCurrStringTrimmed.Contains("PARMESAN_1"))
                            {
                                Trace.WriteLine(sCurrStringTrimmed);
                                iCurrentRow++;

                                sCurrStringTrimmed = sCurrStringsList[iCurrentRow].Trim();
                                if (sCurrStringTrimmed.Contains("PARMESAN_2"))
                                {
                                    Trace.WriteLine(sCurrStringTrimmed);
                                    iCurrentRow++;

                                    sCurrStringTrimmed = sCurrStringsList[iCurrentRow].Trim();
                                    if (sCurrStringTrimmed.Contains("PARMESAN_3"))
                                    {
                                        Trace.WriteLine(sCurrStringTrimmed);
                                        iCurrentRow++;
                                        iCurrentRow++;

                                        bSkipTotale = true;
                                    }
                                }
                            }

                            // contronfo stringhe contenenti Articoli non a Listino ... segue
                            else if (sCurrStringTrimmed.Contains("TOTALE") && bSkipTotale)
                            {
                                // eccezzione in presenza di scontrino con PARMESAN_x
                                bSkipTotale = false;
                            }

                            // errore
                            else
                            {
                                sTmp = string.Format($"***  fail *** {sCurrStringTrimmed}");
                                Trace.WriteLine(sTmp);

                                bResult = false;
                                break;
                            }

                            iCurrentRow++;
                        }

                        // termina il confronto al primo errore
                        if (!bResult)
                            break;

                        // esaurite le stringhe dello scontrino corrente
                        if (iCurrentRow == sCurrStringsList.Count)
                            break;
                    }

                    if (!bCurrentFoud)
                    {
                        Trace.WriteLine(string.Format($"ReceiptGenerationCompare: {referenceFile} not found !"));
                    }

                    if (!bResult)
                        break;
                }

            }
            catch (Exception e)
            {
                Trace.WriteLine(string.Format($"ReceiptGenerationCompare: {e.Message}"));
                bResult = false;
            }

            Assert.IsTrue(bResult);
        }

        [TestMethod]
        [DataRow("refData")]
        [DataRow("StampaTmp.txt")]
        [DataRow("StampaRidTmp.txt")]
        public void DataReportGenerationCompare_IsTrue(string sFileParam)
        {
            InitActualDate();

            // recupera i dati generati in data precedente
            SetActualDate(DateTime.Now.AddDays(_DAYS_IN_ADVANCE));

            bool bResult = true;
            bool bSkipTotale;

            int iCurrentRow;
            int dateCharCountRef, dateCharCountCurrent;
            string sNomeFileDati, sCurrStringTrimmed;

            string sCurrentReceiptDataFile, sReferenceReceiptDataFile;

            List<string> sRefStringsList = new List<string>();
            List<string> sCurrStringsList = new List<string>();

            StreamReader fReference;
            StreamReader fCurrent;

            // serve per avere uscita su Console
            Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));

            sNomeFileDati = GetNomeFileDati(1, GetActualDate());

            if (sFileParam == "refData")
            {
                sReferenceReceiptDataFile = Directory.GetCurrentDirectory() + "\\..\\..\\refData\\" + "C1_Dati_0322.txt";

                if (_DEBUG_VERIFY_DIR)
                    sCurrentReceiptDataFile = Directory.GetCurrentDirectory() + "\\..\\..\\..\\StandFacile\\exe\\StandDati" + "\\" + ANNO_DIR + GetActualDate().ToString("yyyy") + "\\" + sNomeFileDati;
                else
                    sCurrentReceiptDataFile = "C:\\StandFacile\\StandDati\\" + ANNO_DIR + GetActualDate().ToString("yyyy") + "\\" + sNomeFileDati;
            }
            else
            {
                sReferenceReceiptDataFile = Directory.GetCurrentDirectory() + "\\..\\..\\refData\\" + sFileParam;

                if (_DEBUG_VERIFY_DIR)
                    sCurrentReceiptDataFile = Directory.GetCurrentDirectory() + "\\..\\..\\..\\StandFacile\\exe\\Debug" + "\\" + sFileParam;
                else
                    sCurrentReceiptDataFile = "C:\\StandFacile\\StandFacile_513x\\" + sFileParam;
            }

            string sTmp;
            string sInRefStr, sRefStringTrimmed;

            fReference = File.OpenText(sReferenceReceiptDataFile);

            string sInCurrentStr;

            fCurrent = File.OpenText(sCurrentReceiptDataFile);

            Trace.WriteLine("****************************");
            sTmp = string.Format($"*** file: {Path.GetFileName(sCurrentReceiptDataFile)}");
            Trace.WriteLine(sTmp);
            Trace.WriteLine("****************************");

            sRefStringsList.Clear();
            sCurrStringsList.Clear();

            // legge i files e prepara le 2 liste 
            while (((sInRefStr = fReference.ReadLine()) != null) && (sRefStringsList.Count < 1000))
                sRefStringsList.Add(sInRefStr);

            while (((sInCurrentStr = fCurrent.ReadLine()) != null) && (sCurrStringsList.Count < 1000))
                sCurrStringsList.Add(sInCurrentStr);

            fReference.Close();
            fCurrent.Close();

            iCurrentRow = 0;
            bSkipTotale = false;

            foreach (string sRef in sRefStringsList)
            {
                if (iCurrentRow >= sCurrStringsList.Count)
                {
                    sTmp = string.Format($"***  fail *** iCurrentRow > sCurrStringsList.Count");
                    Trace.WriteLine(sTmp);

                    bResult = false;
                    break;
                }

                sRefStringTrimmed = sRef.Trim();
                sCurrStringTrimmed = sCurrStringsList[iCurrentRow].Trim();

                dateCharCountRef = 0;
                dateCharCountRef += sRefStringTrimmed.Split('/').Length - 1;
                dateCharCountRef += sRefStringTrimmed.Split('.').Length - 1;

                dateCharCountCurrent = 0;
                dateCharCountCurrent += sCurrStringTrimmed.Split('/').Length - 1;
                dateCharCountCurrent += sCurrStringTrimmed.Split('.').Length - 1;

                // contronfo stringhe
                if (string.Compare(sRefStringTrimmed, sCurrStringTrimmed) == 0)
                    Trace.WriteLine(sCurrStringTrimmed);

                // contronfo stringhe contenenti la versione
                else if ((dateCharCountRef == dateCharCountCurrent) && sCurrStringTrimmed.Contains("StandFacile v5"))
                    Trace.WriteLine(sCurrStringTrimmed);

                // contronfo stringhe contenenti la data
                else if ((dateCharCountRef == dateCharCountCurrent) && (dateCharCountCurrent == 4))
                    Trace.WriteLine(sCurrStringTrimmed);

                // contronfo stringhe contenenti Annullo
                else if (sCurrStringTrimmed == sConst_Annullo[0])
                {
                    Trace.WriteLine(sCurrStringTrimmed);
                    iCurrentRow++;

                    sCurrStringTrimmed = sCurrStringsList[iCurrentRow].Trim();
                    if (sCurrStringTrimmed == sConst_Annullo[1])
                    {
                        Trace.WriteLine(sCurrStringTrimmed);
                        iCurrentRow++;

                        sCurrStringTrimmed = sCurrStringsList[iCurrentRow].Trim();
                        if (sCurrStringTrimmed == sConst_Annullo[2])
                        {
                            Trace.WriteLine(sCurrStringTrimmed);
                            iCurrentRow++;
                            iCurrentRow++;
                        }
                    }
                }

                // contronfo stringhe contenenti Articoli non a Listino
                else if (sCurrStringTrimmed.Contains("PARMESAN_1"))
                {
                    Trace.WriteLine(sCurrStringTrimmed);
                    iCurrentRow++;

                    sCurrStringTrimmed = sCurrStringsList[iCurrentRow].Trim();
                    if (sCurrStringTrimmed.Contains("PARMESAN_2"))
                    {
                        Trace.WriteLine(sCurrStringTrimmed);
                        iCurrentRow++;

                        sCurrStringTrimmed = sCurrStringsList[iCurrentRow].Trim();
                        if (sCurrStringTrimmed.Contains("PARMESAN_3"))
                        {
                            Trace.WriteLine(sCurrStringTrimmed);
                            iCurrentRow++;
                            iCurrentRow++;

                            bSkipTotale = true;
                        }
                    }
                }

                // contronfo stringhe contenenti Articoli non a Listino ... segue
                else if (sCurrStringTrimmed.Contains("TOTALE") && bSkipTotale)
                {
                    // eccezzione in presenza di scontrino con PARMESAN_x
                    bSkipTotale = false;
                }

                // errore
                else
                {
                    Trace.WriteLine(string.Format($"DataReportGenerationCompare: ***  fail *** {sCurrentReceiptDataFile}"));
                    Trace.WriteLine(string.Format($"DataReportGenerationCompare: {sCurrStringTrimmed}"));

                    bResult = false;
                    break;
                }

                iCurrentRow++;
            }

            Assert.IsTrue(bResult);
        }
    }

}

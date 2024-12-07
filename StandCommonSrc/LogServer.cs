/****************************************************************
    NomeFile : StandCommonSrc/LogServer.cs
    Data	 : 23.06.2023
 	Autore	 : Mauro Artuso
 ****************************************************************/

using System;
using System.IO;
using System.Threading;
using System.Collections;
using System.Windows.Forms;

using static StandCommonFiles.ComDef;
using static StandCommonFiles.commonCl;

using StandFacile;
using static StandFacile.glb;

namespace StandCommonFiles
{
    /// <summary>
    /// classe per i Log su file delle attività utente <br/>
    /// mette i messaggi in coda e poi utilizza un Thread 
    /// a bassa priorità per il Log su file
    /// </summary>
    public class LogServer
    {
        static bool bStarted = true;

        /// <summary>Thread signal</summary>
        public static ManualResetEvent allDone = new ManualResetEvent(false);

        // serve per uscire dal loop infinito del server
        private static bool bRunning;

        static String sCaptionBuf;
        static StreamWriter fLog;

#if STANDFACILE
        static StreamWriter fLogTest;
#endif

        // coda delle stringhe di cui fare il Log
        static Queue logQueue = new Queue();
        static Queue logTestQueue = new Queue();

        /// <summary>
        /// costruttore ed avvio del LogThread
        /// </summary>
        public LogServer()
        {
            String sTmp;

            ThreadStart LogThreadMethod = new ThreadStart(LogServerThread);
            Thread LogThread = new Thread(LogThreadMethod);
            LogThread.Name = "LogServer";
            LogThread.Priority = ThreadPriority.BelowNormal;

            // avvio
            bRunning = true;
            LogThread.Start();

            sTmp = "\n\n";
            sTmp += "******************************\n";
#if STANDFACILE
            sTmp += "**  AVVIO LOG StandFacile   **\n";
#elif STAND_CUCINA
            sTmp += "**  AVVIO LOG StandCucina   **\n";
#elif STAND_ORDINI
            sTmp += "**  AVVIO LOG StandOrdini   **\n";
#elif STAND_MONITOR
            sTmp += "**  AVVIO STAND_MONITOR LOG **\n";
#endif

            sTmp += "**  ";
            sTmp += RELEASE_SW;
            sTmp += "         **\n";
            sTmp += "******************************";

            LogToFile(sTmp);

#if STANDFACILE
            LogTestToFile(sTmp);
#endif
        }


        /************************************
                   Thread di Log
        ************************************/
        private static void LogServerThread()
        {
            String sLog, sLogDir;
            String sNomeLogFile;

            try
            {

#if STANDFACILE
                String sNomeLogTestFile;

                sLogDir = DataManager.sGetLogDir() + "\\";
                sNomeLogFile = "Log" + getActualDate().ToString("yyMMdd") + ".txt";

                sNomeLogTestFile = "LogTest" + getActualDate().ToString("yyMMdd") + ".txt";
#else
                sLogDir = Directory.GetCurrentDirectory() + "\\";
                sNomeLogFile = "Log" + DateTime.Now.ToString("yyMMdd") + ".txt";
#endif


                while (bRunning)
                {
                    // Set the event to nonsignaled state.
                    allDone.Reset();

                    // punto di attesa del server
                    allDone.WaitOne();


                    lock (logQueue.SyncRoot)
                    {
                        fLog = File.AppendText(sLogDir + sNomeLogFile);

                        while (bRunning && (logQueue.Count > 0))
                        {
                            sLog = (String)logQueue.Dequeue();

                            fLog.WriteLine(sLog);
                        }

                        fLog.Close();
                    }

#if STANDFACILE

                    if (bCheckService(Define._AUTO_SEQ_TEST))
                    {
                        // registrazione del LogTest
                        lock (logTestQueue.SyncRoot)
                        {
                            fLogTest = File.AppendText(sLogDir + sNomeLogTestFile);

                            while (bRunning && (logTestQueue.Count > 0))
                            {
                                sLog = (String)logTestQueue.Dequeue();

                                fLogTest.WriteLine(sLog);
                            }

                            fLogTest.Close();
                        }
                    }
#endif
                } // while
            }
            catch
            {
                sCaptionBuf = String.Format("Avviso {0} : {1}", WRN_LFE, Define.TITLE);
                MessageBox.Show("Errore nell'apertura del Logfile !", sCaptionBuf, MessageBoxButtons.OK);
            }
        }

        /// <summary>Funzione scrittura su LogFile</summary>
        public static void LogToFile(String sLogMsg)
        {

            String sTime, sDate;
            String sTmpMsg;

            sDate = DateTime.Now.ToString("yy/MM/dd");
            sTime = DateTime.Now.ToString("HH.mm.ss.ff");

            if (bStarted)
            {
                sTmpMsg = sLogMsg;
                bStarted = false;
            }
            else
                sTmpMsg = String.Format("{0} {1,9} {2}", sDate, sTime, sLogMsg);

            // il lock() non dovrebbe servire qui !
            lock (logQueue.SyncRoot)
            {
                logQueue.Enqueue(sTmpMsg);
            }

            /*******************************
             *    avanzamento del thread
             *******************************/
            allDone.Set();
        }

#if STANDFACILE
        /// <summary>Funzione scrittura su Log di Test su File</summary>
        public static void LogTestToFile(String sLogMsg)
        {

            String sTime, sDate;
            String sTmpMsg;

            if (!bCheckService(Define._AUTO_SEQ_TEST))
                return;

            sDate = DateTime.Now.ToString("yy/MM/dd");
            sTime = DateTime.Now.ToString("HH.mm.ss.ff");

            if (bStarted)
            {
                sTmpMsg = sLogMsg;
                bStarted = false;
            }
            else
                sTmpMsg = String.Format("{0} {1,9} {2}", sDate, sTime, sLogMsg);

            // il lock() non dovrebbe servire qui !
            lock (logTestQueue.SyncRoot)
            {
                logTestQueue.Enqueue(sTmpMsg);
            }

            /*******************************
             *    avanzamento del thread
             *******************************/
            allDone.Set();
        }
#endif

        /// <summary>arresto del server di Log</summary>
        public static void StopLogServer()
        {
            bRunning = false;

            allDone.Set();
        }

    }
}

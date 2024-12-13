/*****************************************************************************
     NomeFile : StandCommonSrc/Printer_Legacy.cs
     Data	  : 23.06.2023
     Autore   : Mauro Artuso
 ******************************************************************************/

using System;
using System.IO;
using System.IO.Ports;
using System.Collections;
using System.Threading;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

using static StandCommonFiles.ComDef;
using static StandCommonFiles.CommonCl;
using static StandCommonFiles.LogServer;

using StandFacile;
using static StandFacile.glb;
using static StandFacile.Define;

namespace StandCommonFiles
{
    #pragma warning disable IDE0044

    /// <summary>
    /// classe per la gestione delle stampanti seriali mediante coda di stampa
    /// </summary>
    public static class Printer_Legacy
    {
        /// <summary>enum per il modello di stampante su porta seriale e/o parallela</summary>
        public enum LEGACY_PRINTER_MODELS
        {
            /// <summary>modello TM-T88 seriale</summary>
            STAMPANTE_TM_T88_SER = 0,
            /// <summary>modello TM-T88 parallela</summary>
            STAMPANTE_TM_L90_LPT,
            /// <summary>modello LP2844 seriale</summary>
            STAMPANTE_LP2844_PAGEMODE_SER,
            /// <summary>modello LP2844 parallela</summary>
            STAMPANTE_LP2844_PAGEMODE_LPT
        };

        /// <summary>num,ero max dei modelli</summary>
        // public static int MAX_LEGACY_MODELS = Enum.GetNames(typeof(LEGACY_PRINTER_MODELS)).Length;
        public static int MAX_LEGACY_MODELS = 2;

        /// <summary>elenco dei modelli</summary>
        public static readonly String[] sConstLegacyModels =
        {
            "Epson TM-T88 RS232",
            "Epson TM-L90 LPT",
            "ZEBRA LP 2844 RS232",
            "ZEBRA LP 2844 LPT"
        };

        /// <summary>modi di controllo di flusso</summary>
        public static readonly String[] sConstFlow =
        {
            "NONE",
            "RTS/CTS",
            "XON/XOFF"
        };

        // X 1000ms
        const int SER_OPEN_TIMEOUT = 8;
        // X 100ms
        const int CLOSE_DELAY = 16;

        private static int iCloseDelay;

        /// <summary>Struct per la gestione della stampa Legacy</summary>
        public static TLegacyPrinterParams _LegacyPrinterParams;

        // Thread signal
        private static ManualResetEvent allDone = new ManualResetEvent(false);

        // serve per uscire dal loop infinito del server
        private static bool bRunning = true;

        // coda dei nomi di files di cui fare la stampa
        static Queue printQueue = new Queue();

        static TErrMsg _ErrMsg;

        static ThreadStart PrintThreadMethod = new ThreadStart(PrintServerThread);
        static Thread PrintThread = new Thread(PrintThreadMethod);

        static ThreadStart RxThreadMethod = new ThreadStart(RxServerThread);
        static Thread RxThread = new Thread(RxThreadMethod);

        /// <summary>
        /// gestione COM con libreria .NET
        /// </summary>
        public static SerialPort serialPort = new SerialPort();

        // gestione LPT non c'è una libreria .NET quindi si usa la "CreateFile()"
        static FileStream lptStream = null;

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern SafeFileHandle CreateFile(string lpFileName, FileAccess dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, FileMode dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

        static bool LPT_IsOpen()
        {
            if (lptStream == null)
                return false;
            else
                return true;
        }

         /// <summary>
         /// avvio dei server Thread di stampa
         /// </summary>
        public static void Init()
        {
            PrintThread.Name = "PrintServer";
            PrintThread.Priority = ThreadPriority.Normal;
            PrintThread.Start();

            RxThread.Name = "RxServer";
            RxThread.Priority = ThreadPriority.Normal;

            // RxThread.Start(); da valutare se serve

            LogToFile("Printer_Legacy : StartPrintServer");
        }

         /// <summary>
         /// funzione di stampa stringa con ritardo variabile richiesto da alcune stampanti
         /// </summary>
        public static void PrintLine(String sTextToPrint, int iDelay = 0)
        {

            if (_LegacyPrinterParams.sPort.Contains("COM"))
            {
                if (serialPort.IsOpen)
                    serialPort.WriteLine(sTextToPrint);

                if (_LegacyPrinterParams.iFlowCtrl == (int)LegacyPrinterDlg.FLOW_CONTROL.FLOW_NONE)
                {
                    if (iDelay > 0)
                        Thread.Sleep(iDelay);
                    else
                        Thread.Sleep(50); // 50 = delay di default
                }
            }
            else // LPT
            {
                Byte[] buffer = System.Text.Encoding.UTF8.GetBytes(sTextToPrint + "\n");

                if (LPT_IsOpen())
                    lptStream.Write(buffer, 0, buffer.Length);
            }

            iCloseDelay = CLOSE_DELAY;
        }

         /// <summary>
         /// ATTENZIONE : PrintLine(char *pChTextToPrint, int iDelay, int iCount)
         /// non può essere fatta con AnsiString a causa dei bytes 0x00 inviati
         /// </summary>
        public static void PrintBuffer(char[] textToPrint, int iCount, int iDelay = 0)
        {
            if (_LegacyPrinterParams.sPort.Contains("COM"))
            {
                if (serialPort.IsOpen)
                    serialPort.Write(textToPrint, 0, iCount);

                if (_LegacyPrinterParams.iFlowCtrl == (int)LegacyPrinterDlg.FLOW_CONTROL.FLOW_NONE)
                {
                    if (iDelay > 0)
                        Thread.Sleep(iDelay);
                    else
                        Thread.Sleep(50); // 50 = delay di default
                }
            }
            else
            {
                Byte[] buffer = System.Text.Encoding.UTF8.GetBytes(textToPrint);

                if (LPT_IsOpen())
                    lptStream.Write(buffer, 0, iCount);
            }

            iCloseDelay = CLOSE_DELAY;
        }

        /************************************************
         *   Thread di stampa per le stampanti Legacy   *
         ************************************************/
        private static void PrintServerThread()
        {
            String sFileToPrint, sTmp;
#if STANDFACILE
            String[] sQueue_Object;
#endif
            sTmp = String.Format("Printer_Legacy : avviato il PrintServerThread");
            LogToFile(sTmp);

            // punto iniziale di attesa del Thread
            allDone.WaitOne();

            while (bRunning)
            {
                // verifica se la coda è vuota
                if (printQueue.Count > 0)
                {

                    if (!((String.IsNullOrEmpty(_LegacyPrinterParams.sPort) || _LegacyPrinterParams.sPort.Contains("COM")) ? COM_PortIsOpen() : LPT_PortISOpen()))
                    {
                        // Open fallita!
                        _ErrMsg.iErrID = WRN_STF;
                        _ErrMsg.sMsg = ReadRegistry("sLegacyPort", "");
                        WarningManager(_ErrMsg);

                        sTmp = "Printer_Legacy : open fallita! Queue.pop";
                        LogToFile(sTmp);

                        while (!(printQueue.Count > 0))
                            printQueue.Dequeue();

                        iCloseDelay = 0;
                    }
                    else
                    {

                        // altrimenti lo switch viene percorso durante la chiusura
                        while (bRunning && (printQueue.Count > 0))
                        {
                            sFileToPrint = (String)printQueue.Dequeue();

                            LogToFile("Printer_Legacy : stampa di " + sFileToPrint);

#if STANDFACILE
                            /***********************************************
                             *  riattiva il bottone di emissione scontrino
                             *  ed aggiorna il prezzo
                             ***********************************************/

                            sQueue_Object = new String[2] { RESET_RECEIPT_BTN_EVENT, "" };
                            FrmMain.EventEnqueue(sQueue_Object);
#endif

                            switch (_LegacyPrinterParams.iPrinterModel)
                            {
                                case (int)LEGACY_PRINTER_MODELS.STAMPANTE_TM_T88_SER:
                                case (int)LEGACY_PRINTER_MODELS.STAMPANTE_TM_L90_LPT:
                                    TPrinter_TM_POS.PrintFile(sFileToPrint);
                                    break;

                                case (int)LEGACY_PRINTER_MODELS.STAMPANTE_LP2844_PAGEMODE_SER:
                                case (int)LEGACY_PRINTER_MODELS.STAMPANTE_LP2844_PAGEMODE_LPT:
                                    TPrinter_LP2844.PrintFile(sFileToPrint);
                                    break;
                            }

                            Thread.Sleep(200);
                        }

                        sFileToPrint = "";

                    }
                }

                // delay in chiusura per consentire la ricezione Xon, Xoff, etc
                if (iCloseDelay > 0)
                {
                    iCloseDelay--;
                    Thread.Sleep(10);
                }
                else
                {
                    // libera la seriale/LPT
                    PortClose();

                    // Set the event to nonsignaled state.
                    allDone.Reset();

                    if (bRunning) // serve per evitare blocchi
                    {
                        // punto di attesa del server
                        allDone.WaitOne();
                    }
                }
            } // end while (bRunning)
        }

        /***************************************************
         *   ritorna : true se tutto OK,
         *             false se non si può aprire la COM
         ***************************************************/
        static bool COM_PortIsOpen()
        {
            // esegue più tentativi
            int iTimeout = SER_OPEN_TIMEOUT;

#if !STAND_MONITOR
            String[] sQueue_Object;
#endif
            if (serialPort.IsOpen)
                return true;

            LogToFile("Printer_Legacy : inizio portOpen");

            if (String.IsNullOrEmpty(_LegacyPrinterParams.sPort))
                return false;
            else
                serialPort.PortName = _LegacyPrinterParams.sPort;

            serialPort.BaudRate = 19200;
            serialPort.Parity = Parity.None;
            serialPort.DataBits = 8;
            serialPort.StopBits = StopBits.One;

            serialPort.WriteTimeout = SerialPort.InfiniteTimeout;
            serialPort.ReadTimeout = SerialPort.InfiniteTimeout;

            switch (_LegacyPrinterParams.iFlowCtrl)
            {
                case (int)LegacyPrinterDlg.FLOW_CONTROL.RTS_CTS:
                    serialPort.Handshake = Handshake.RequestToSend;
                    break;
                case (int)LegacyPrinterDlg.FLOW_CONTROL.XON_XOFF:
                    serialPort.Handshake = Handshake.XOnXOff;
                    break;
                default:
                    serialPort.Handshake = Handshake.None;
                    break;
            }

            do
            {
                try
                {
                    serialPort.Open();
                    LogToFile(String.Format("Printer_Legacy : Serial Port Opened Successfully {0}", serialPort.PortName));
                }
                catch (Exception)
                {
                    iTimeout--;

                    LogToFile(String.Format("Printer_Legacy : Serial Port Open iTimeout = {0:d}", iTimeout));
                    Thread.Sleep(1000);

#if STANDFACILE
                    sQueue_Object = new String[2];
                    sQueue_Object[0] = UPDATE_COM_STATUS_EVENT;
                    sQueue_Object[1] = String.Format("{0:d1}", (int)COM_STATUS.COM_NOT_FREE);
                    FrmMain.EventEnqueue(sQueue_Object);
#elif STAND_CUCINA
                    sQueue_Object = new String[2];
                    sQueue_Object[0] = UPDATE_COM_LED_EVENT;
                    sQueue_Object[1] = String.Format("{0:d1}", (int)COM_STATUS.COM_NOT_FREE);
                    FrmMain.QueueUpdate(sQueue_Object);
#endif
                }
            }
            while ((!serialPort.IsOpen) && (iTimeout != 0));

            if (serialPort.IsOpen)
            {

#if STANDFACILE
                sQueue_Object = new String[2];
                sQueue_Object[0] = UPDATE_COM_STATUS_EVENT;
                sQueue_Object[1] = String.Format("{0:d1}", (int)COM_STATUS.COM_BUSY);
                FrmMain.EventEnqueue(sQueue_Object);
#elif STAND_CUCINA
                sQueue_Object = new String[2];
                sQueue_Object[0] = UPDATE_COM_LED_EVENT;
                sQueue_Object[1] = String.Format("{0:d1}", (int)COM_STATUS.COM_BUSY);
                FrmMain.QueueUpdate(sQueue_Object);
#endif
                //Thread.Sleep(250); // per consentire le grafiche
                return true;
            }
            else
                return false;
        }

        /*************************************************
         *   ritorna : 1 se tutto OK,
         *             0 se non si può aprire la LPT
         *************************************************/
        static bool LPT_PortISOpen()
        {
            // esegue più tentativi
            int iTimeout = SER_OPEN_TIMEOUT;
            String sLPT_PortName;

#if !STAND_MONITOR
            String[] sQueue_Object;
#endif

            if (lptStream != null)
                return true;

            LogToFile("Printer_Legacy : inizio LPT portOpen");

            if (!String.IsNullOrEmpty(_LegacyPrinterParams.sPort))
                sLPT_PortName = _LegacyPrinterParams.sPort;
            else
            {
                sLPT_PortName = "LPT1";
                iTimeout = SER_OPEN_TIMEOUT / 4;
            }

            do
            {
                try
                {
                        lptStream?.Close();

                    SafeFileHandle hLPT = CreateFile(sLPT_PortName, FileAccess.Write, 0, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);

                    if (hLPT.IsInvalid)
                    {
                        lptStream = null;
                        iTimeout--;

                        LogToFile(String.Format("Printer_Legacy : LPT Port Open INVALID Timeout = {0}", iTimeout));
                        Thread.Sleep(1000);

#if STANDFACILE
                        sQueue_Object = new String[2];
                        sQueue_Object[0] = UPDATE_COM_STATUS_EVENT;
                        sQueue_Object[1] = String.Format("{0:d1}", (int)COM_STATUS.COM_NOT_FREE);
                        FrmMain.EventEnqueue(sQueue_Object);
#elif STAND_CUCINA
                        sQueue_Object = new String[2];
                        sQueue_Object[0] = UPDATE_COM_LED_EVENT;
                        sQueue_Object[1] = String.Format("{0:d1}", (int)COM_STATUS.COM_NOT_FREE);
                        FrmMain.QueueUpdate(sQueue_Object);
#endif
                    }
                    else
                    {
                            lptStream?.Close();

                        lptStream = new FileStream(hLPT, FileAccess.ReadWrite);

                        LogToFile(String.Format("Printer_Legacy : LPT Port Open VALID Timeout = {0}", iTimeout));
                    }

                }
                catch (Exception)
                {
                }
            }
            while (!LPT_IsOpen() && (iTimeout != 0));

            if (LPT_IsOpen())
            {

#if STANDFACILE
                sQueue_Object = new String[2];
                sQueue_Object[0] = UPDATE_COM_STATUS_EVENT;
                sQueue_Object[1] = String.Format("{0:d1}", (int)COM_STATUS.COM_BUSY);
                FrmMain.EventEnqueue(sQueue_Object);
#elif STAND_CUCINA
                sQueue_Object = new String[2];
                sQueue_Object[0] = UPDATE_COM_LED_EVENT;
                sQueue_Object[1] = String.Format("{0:d1}", (int)COM_STATUS.COM_BUSY);
                FrmMain.QueueUpdate(sQueue_Object);
#endif
                //Thread.Sleep(250); // per consentire le grafiche
                return true;
            }
            else
                return false;
        }

        // ******************************
        private static void PortClose()
        {
#if !STAND_MONITOR
            String[] sQueue_Object;
#endif

            LogToFile("Printer_Legacy : PortClose");

            if (serialPort.IsOpen)
            {
                serialPort.Close();

#if STANDFACILE
                sQueue_Object = new String[2];
                sQueue_Object[0] = UPDATE_COM_STATUS_EVENT;
                sQueue_Object[1] = String.Format("{0:d1}", (int)COM_STATUS.COM_FREE);
                FrmMain.EventEnqueue(sQueue_Object);
#elif STAND_CUCINA
                sQueue_Object = new String[2];
                sQueue_Object[0] = UPDATE_COM_LED_EVENT;
                sQueue_Object[1] = String.Format("{0:d1}", (int)COM_STATUS.COM_FREE);
                FrmMain.QueueUpdate(sQueue_Object);
#endif

                LogToFile("Printer_Legacy : PortClose serialPort.IsOpen");
                Thread.Sleep(250); // per consentire le grafiche
            }
            else
                LogToFile("Printer_Legacy : !serialPort.IsOpen");

            if (LPT_IsOpen())
            {
                lptStream.Close();
                lptStream = null;

#if STANDFACILE
                sQueue_Object = new String[2];
                sQueue_Object[0] = UPDATE_COM_STATUS_EVENT;
                sQueue_Object[1] = String.Format("{0:d1}", (int)COM_STATUS.COM_FREE);
                FrmMain.EventEnqueue(sQueue_Object);
#elif STAND_CUCINA
                sQueue_Object = new String[2];
                sQueue_Object[0] = UPDATE_COM_LED_EVENT;
                sQueue_Object[1] = String.Format("{0:d1}", (int)COM_STATUS.COM_FREE);
                FrmMain.QueueUpdate(sQueue_Object);
#endif

                LogToFile("Printer_Legacy : PortClose lptStream != null");
                Thread.Sleep(250); // per consentire le grafiche
            }
            else
                LogToFile("Printer_Legacy : !LPT_IsOpen");
        }

        /// <summary>
        /// verifica se la porta è libera aspettando qualche secondo se serve,
        /// ritorna : 0 se non è libera, 1 se è libera
        /// </summary>
        public static bool PortVerify(TLegacyPrinterParams sLegacyPrinterParams)
        {
            // esegue più tentativi
            int iTimeout = SER_OPEN_TIMEOUT / 4;
            String sLogText;

            _LegacyPrinterParams = sLegacyPrinterParams;

            LogToFile("Printer_Legacy : Verify() prima di loop");

            // porta di default
            if (String.IsNullOrEmpty(_LegacyPrinterParams.sPort) || _LegacyPrinterParams.sPort.Contains("COM"))
            {
                if (!String.IsNullOrEmpty(_LegacyPrinterParams.sPort))
                    serialPort.PortName = _LegacyPrinterParams.sPort;

                serialPort.WriteTimeout = SerialPort.InfiniteTimeout;
                serialPort.ReadTimeout = SerialPort.InfiniteTimeout;

                serialPort.Handshake = Handshake.XOnXOff;

                do
                {
                    try
                    {
                        serialPort.Open();
                        sLogText = String.Format("Printer_Legacy : Verify VALID Timeout = {0}", iTimeout);
                        LogToFile(sLogText);
                    }

                    catch (Exception)
                    {
                        iTimeout--;

                        sLogText = String.Format("Printer_Legacy : Verify INVALID Timeout = {0}", iTimeout);
                        LogToFile(sLogText);

                        Thread.Sleep(1000);
                    }
                }
                while ((!serialPort.IsOpen) && (iTimeout != 0));

                if (serialPort.IsOpen)
                {
                    PortClose();
                    return true;
                }
                else
                    return false;
            }
            else
            {
                do
                {
                    try
                    {
                            lptStream?.Close();

                        SafeFileHandle hLPT = CreateFile(_LegacyPrinterParams.sPort, FileAccess.Write, 0, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);

                        if (!hLPT.IsInvalid)
                        {
                            lptStream = new FileStream(hLPT, FileAccess.ReadWrite);
                            LogToFile("Printer_Legacy : LPT Port Opened Successfully");
                        }
                        else
                        {
                            lptStream = null;
                            iTimeout--;

                            LogToFile(String.Format("Printer_Legacy : LPT Port Open iTimeout = {0:d}", iTimeout));
                            Thread.Sleep(1000);
                        }

                    }
                    catch (Exception)
                    {
                    }
                }
                while (!LPT_IsOpen() && (iTimeout != 0));

                if (LPT_IsOpen())
                {
                    PortClose();
                    return true;
                }
                else
                    return false;
            }
        }


         /// <summary>Autotest</summary>
        public static void PrintAutoTest()
        {

            if (!(_LegacyPrinterParams.sPort.Contains("COM") ? COM_PortIsOpen() : LPT_PortISOpen()))
            {
                WarningManager(WRN_TSF);
                return;
            }

            switch (_LegacyPrinterParams.iPrinterModel)
            {
                case (int)LEGACY_PRINTER_MODELS.STAMPANTE_TM_T88_SER:
                case (int)LEGACY_PRINTER_MODELS.STAMPANTE_TM_L90_LPT:
                    TPrinter_TM_POS.PrintAutoTest();
                    break;
                case (int)LEGACY_PRINTER_MODELS.STAMPANTE_LP2844_PAGEMODE_SER:
                case (int)LEGACY_PRINTER_MODELS.STAMPANTE_LP2844_PAGEMODE_LPT:
                    TPrinter_LP2844.PrintAutoTest();
                    break;
            }

            PortClose();
        }

        /// <summary>Info</summary>
        public static void PrintInfo()
        {
            if (!(_LegacyPrinterParams.sPort.Contains("COM") ? COM_PortIsOpen() : LPT_PortISOpen()))
            {
                WarningManager(WRN_TSF);
                return;
            }

            switch (_LegacyPrinterParams.iPrinterModel)
            {
                case (int)LEGACY_PRINTER_MODELS.STAMPANTE_TM_T88_SER:
                case (int)LEGACY_PRINTER_MODELS.STAMPANTE_TM_L90_LPT:
                    break;

                case (int)LEGACY_PRINTER_MODELS.STAMPANTE_LP2844_PAGEMODE_SER:
                case (int)LEGACY_PRINTER_MODELS.STAMPANTE_LP2844_PAGEMODE_LPT:
                    TPrinter_LP2844.PrintInfo();
                    break;
            }

            PortClose();
        }

         /// <summary>
         /// test di esempio di stampa
         /// </summary>
        public static void PrintSampleText(TLegacyPrinterParams sLegacyPrinterParams)
        {
            String sTmp, sFileToPrint;

            _LegacyPrinterParams = sLegacyPrinterParams;

            sFileToPrint = BuildSampleText();

            sTmp = String.Format("Printer_Legacy : PrintSampleText() {0}", sFileToPrint);
            LogToFile(sTmp);

            PortClose();

            if (!(_LegacyPrinterParams.sPort.Contains("COM") ? COM_PortIsOpen() : LPT_PortISOpen()))
            {
                WarningManager(WRN_TSF);
                return;
            }

            PrintFile(sFileToPrint, sLegacyPrinterParams, (int)PRINT_QUEUE_ACTION.PRINT_NOW);
        }

        /// <summary>
        /// overload PrintFile
        /// </summary>
        public static void PrintFile(String sFileToPrintParams)
        {
            PrintFile(sFileToPrintParams, sGlbLegacyPrinterParams, (int)PRINT_QUEUE_ACTION.PRINT_NOW);
        }

        /// <summary>
        /// Stampa di un file mediante inserimento in coda <br/>
        /// iQueueAction == PRINT_ENQUEUE mette in coda senza iniziare la stampa: serve ad evitare problemi di Shared data<br/>
        /// PRINT_START si usa quando non si passa alcun file alla coda, si avvia il thread di stampa<br/>
        /// PRINT_NOW si inserisce nella coda il file da stampare e si avvia il thread di stampa
        /// </summary>
        public static void PrintFile(String sFileToPrintParm, TLegacyPrinterParams sLegacyPrinterParams, int iQueueAction = (int)PRINT_QUEUE_ACTION.PRINT_NOW)
        {
            String sTmp;

            _LegacyPrinterParams = sLegacyPrinterParams;

            try
            {
                // PRINT_START riavvia la stampa
                if (iQueueAction == (int)PRINT_QUEUE_ACTION.PRINT_START)
                {
                    if (PrintThread.IsAlive)
                        allDone.Set();
                    return;
                }

                if (String.IsNullOrEmpty(sFileToPrintParm) || !File.Exists(sFileToPrintParm))
                    return;

                if (!CheckService(_SKIP_STAMPA))
                {
                    // PRINT_NOW, PRINT_ENQUEUE
                    // importante : al thread bisogna passare parametri per valore,
                    // altrimenti nel tempo i valori indicati dai puntatori possono variare
                    printQueue.Enqueue(sFileToPrintParm);

                    sTmp = String.Format("PrintServer : Queue.push size={0} file = {1}", printQueue.Count, sFileToPrintParm);
                    LogToFile(sTmp);

                    if (iQueueAction == (int)PRINT_QUEUE_ACTION.PRINT_ENQUEUE)
                        return;
                    else
                    {
                        // PRINT_NOW
                        /*****************************************
                         *   avvio del thread di stampa seriale
                         *****************************************/

                        LogToFile("Printer_Legacy : avvio del thread di stampa");
                        iCloseDelay = CLOSE_DELAY;

                        if (PrintThread.IsAlive)
                            allDone.Set();

                        return;
                    }
                }
            }

            catch (Exception)
            {
                _ErrMsg.sMsg = "printQueue";
                _ErrMsg.iErrID = WRN_QEX;
                WarningManager(_ErrMsg);
            }
        } // end try

         /// <summary>
         /// Thread di ricezione da seriale
         /// </summary>
        private static void RxServerThread()
        {
            int iPos;
            String sReceivedData, sTmp;

            sTmp = String.Format("RxServerThread : avviato RxThread");
            LogToFile(sTmp);
            sReceivedData = "";

            while (bRunning)
            {
                if (serialPort.IsOpen)
                {
                    /*************************************************
                        ricezione dalla stampante, solo a COM aperta
                     *************************************************/
                    try
                    {
                        sReceivedData = serialPort.ReadExisting();
                    }
                    catch (Exception ex)
                    {
                        LogToFile(String.Format("RxThread : {0}", ex.Message));
                    }

                    if (sReceivedData.Length > 0)
                    {
                        LogToFile("RxThread : RX " + sReceivedData);

                        iPos = sReceivedData.IndexOf("\x13");
                        if (iPos != -1)
                            LogToFile("RxThread : Ricevuto Xoff !");

                        iPos = sReceivedData.IndexOf("\x11");
                        if (iPos != -1)
                            LogToFile("RxThread : Ricevuto Xon !");

                        // mantiene aperta la seriale
                        iCloseDelay = CLOSE_DELAY;
                    }
                }
            }
        }

         /// <summary>
         /// arresto dei server di stampa e di ricezione
         /// </summary>
        public static void StopPrintServer()
        {
            bRunning = false;

            allDone.Set();

            LogToFile("Printer_Legacy : StopPrintServer !");
        }

        /// <summary> reset del ritardo di chiusura della porta</summary>
        public static void ResetClosedelay()
        {
            iCloseDelay = CLOSE_DELAY;
        }

    } // end class
} // end namespace

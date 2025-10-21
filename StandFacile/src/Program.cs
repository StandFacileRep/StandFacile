/*********************************************************
    NomeFile : StandFacile
    Data	 : 21.08.2025
    Autore   : Mauro Artuso

    Avvia le classi visuali e non, nell'ordine corretto
 *********************************************************/

using System;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Threading;

using StandCommonFiles;
using static StandCommonFiles.ComDef;
using static StandCommonFiles.CommonCl;
using static StandCommonFiles.LogServer;

using static System.Convert;

namespace StandFacile
{
    static class Program
    {
        public static StandFacile_DB.dBaseIntf_ql _rBdBaseIntf_ql;
        public static StandFacile_DB.dBaseIntf_my _rBdBaseIntf_my;
        public static StandFacile_DB.dBaseIntf_pg _rBdBaseIntf_pg;

        public static dBaseTunnel_my _rdBaseTunnel_my;

        static Mutex _mx;

        [STAThread]
        public static void Main(string[] args)
        {
            int iNDbMode, iLoop = 5;
            bool bStandIsRunning = false;

            try
            {
                Printer_Windows.SetSkipNumeroScontrino(false); // init

                for (int i=0; i < args.Length; i++)
                {
                    // acquisisce riga di comando per intervallo tre le stampe
                    if (args[i].Contains("-pw"))
                        Printer_Windows.iPrint_WaitInterval = ToInt32(args[i+1]);

                    if (args[i].Contains("-nn"))
                        Printer_Windows.SetSkipNumeroScontrino(true);

                    // eventuale skip della stampa scontrino
                    if (args[i].Contains("-skipTicket"))
                        Printer_Windows.SetSkipTicketPrint(true);
                }
            }
            catch (Exception)
            {
                Printer_Windows.iPrint_WaitInterval = 500;
            }


            do
            {
                _mx = new Mutex(true, Application.ProductName.ToString(), out bStandIsRunning);

                if (!bStandIsRunning)
                {
                    if (iLoop > 0)
                    {
                        Thread.Sleep(1000); // attendi
                        iLoop--;
                        _mx.Close();
                    }
                    else
                        return; // esci dal programma
                }
                else
                    break;
            }
            while (true);


            // verifica l'esistenza della chiave KEY_STAND_FACILE eventualmente la crea
            if (Registry.GetValue(Define.KEY_STAND_FACILE, "iGridRows", DEF_GRID_NROWS) == null)
                Registry.SetValue(Define.KEY_STAND_FACILE, "iGridRows", DEF_GRID_NROWS);

            // Inizializza le directories dopo che è definita la data
            DataManager rDataManager = new DataManager();

            // Caricamento configurazione
            Config rConfig = new Config();

            // LogServer và creato dopo TDataManager in quanto è utilizzato dalle altre classi
            LogServer rLogServer = new LogServer();

            LogToFile("Program : Start");

            iNDbMode = ReadRegistry(DB_MODE_KEY, (int)DB_MODE.SQLITE);

            if (iNDbMode == (int)DB_MODE.SQLITE)
                _rBdBaseIntf_ql = new StandFacile_DB.dBaseIntf_ql();
            else
            if (iNDbMode == (int)DB_MODE.MYSQL)
                _rBdBaseIntf_my = new StandFacile_DB.dBaseIntf_my();
            else
            if (iNDbMode == (int)DB_MODE.POSTGRES)
                _rBdBaseIntf_pg = new StandFacile_DB.dBaseIntf_pg();

            dBaseIntf rBdBaseIntf = new dBaseIntf();

            _rdBaseTunnel_my = new dBaseTunnel_my();

            Barcode_EAN13 rBarcode_EAN13 = new Barcode_EAN13();

            //Barcode_EAN13.BuildBarcodeID("111308221001");

            if (!CheckService(CFG_COMMON_STRINGS._HIDE_LEGACY_PRINTER))
            {
                // Avvio del server di stampa
                Printer_Legacy.Init();
            }

            // Precede tutte le classi visuali
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            // inizializza i parametri di stampa
            LegacyPrinterDlg rLegacyPrinterDlg = new LegacyPrinterDlg();

            // Avvio della Form di gestione Sconti prima di leggere dal Listino
            ScontoDlg rScontoDlg = new ScontoDlg();

            // gestisce la tipologia di Cassa memorizzando i server con
            // cui è possibile comunicare
            NetConfigDlg rTipoCassaDlg = new NetConfigDlg();

            // Caricamento Listino e Dati
            rDataManager.Init();

            // dopo caricamento Listino
            OptionsDlg rOptionsDlg = new OptionsDlg();

            // inizializza la form di selezione data dei reports
            SelDataDlg rSelDataDlg = new SelDataDlg();

            // inizializza la form di selezione data e ora per l'eliminazione
            GenericPrinterDlg rGenericPrinterDlg = new GenericPrinterDlg();

            // inizializza i parametri di stampa, va messo dopo rDataManager.Init()
            WinPrinterDlg rWinPrinterDlg = new WinPrinterDlg();

            // Avvio della Form di verifica iniziale della data, va messo dopo WinPrinterDlg
            startDlg rStartDlg = new startDlg();

            // inizializza le impostazioni di stampa copie locali
            PrintLocalCopiesConfigDlg rPrintConfigReceiptDlg = new PrintLocalCopiesConfigDlg();

            // inizializza i parametri di stampa
            PrintNetCopiesConfigDlg rFrmPrintConfig = new PrintNetCopiesConfigDlg();

            // avvia la form principale
            FrmMain rFrmMain = new FrmMain();

            // Avvio della Form di anteprima scontrino
            AnteprimaDlg rAnteprimaDlg = new AnteprimaDlg();

            // Avvio della funzione di esecuzione Test Automatici
            TestManager rTestManager = new TestManager();

            rFrmMain.Init();
            rFrmMain.Show();

            bApplicationRuns = true;

            // avvia la form principale
            Application.Run(rFrmMain);

            _mx.ReleaseMutex();
        }

        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message, "Eccezione non gestita nel Thread");
            LogToFile(e.Exception.Message);
            //StopLogServer();
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show((e.ExceptionObject as Exception).Message, "Eccezione non gestita nell'UI");
            LogToFile((e.ExceptionObject as Exception).Message); // non funziona
        }

    }
}

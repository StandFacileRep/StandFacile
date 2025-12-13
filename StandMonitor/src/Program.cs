/***********************************************
  	NomeFile : Program.cs
	Data	 : 08.12.2025
  	Autore   : Mauro Artuso
 ***********************************************/

using System;
using System.Windows.Forms;
using static StandCommonFiles.ComDef;
using static StandCommonFiles.CommonCl;
using static StandFacile.glb;

using StandCommonFiles;

namespace StandFacile
{
    static class Program
    {
#pragma warning disable IDE0059

        /// <summary>
        /// Punto di ingresso principale dell'applicazione.
        /// </summary>

        public static StandFacile_DB.dBaseIntf_my _rBdBaseIntf_my;
        public static StandFacile_DB.dBaseIntf_pg _rBdBaseIntf_pg;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Caricamento configurazione
            Config rConfig = new Config();

            LogServer rLogServer = new LogServer();

            // deve stare qui subito dopo il LogServer !
            MessageDlg rMessageDlg = new MessageDlg();

            // Avvio del server di stampa
            Printer_Legacy.Init();

            FrmMain rFrmMain = new FrmMain();

            VisOrdiniTableFrm rVisOrdiniFrm = new VisOrdiniTableFrm();

            Barcode_EAN13 rBarcode_EAN13 = new Barcode_EAN13();

            // inizializza i parametri di stampa
            LegacyPrinterDlg rThermPrinterDlg = new LegacyPrinterDlg();

            // inizializza i parametri di stampa
            WinPrinterDlg rWinPrinterDlg = new WinPrinterDlg();

            // inizializza la data
            SelDataDlg rSelDataDlg = new SelDataDlg();

            // inizializza i parametri di stampa
            PrintConfigLightDlg rPrintConfigLightDlg = new PrintConfigLightDlg();

            int iNDbMode = ReadRegistry(DB_MODE_KEY, (int)DB_MODE.MYSQL);

            if (iNDbMode == (int)DB_MODE.MYSQL)
                _rBdBaseIntf_my = new StandFacile_DB.dBaseIntf_my();
            else
            if (iNDbMode == (int)DB_MODE.POSTGRES)
                _rBdBaseIntf_pg = new StandFacile_DB.dBaseIntf_pg();

            dBaseIntf rdBaseIntf = new dBaseIntf();

            NetConfigLightDlg rNetConfigLightDlg = new NetConfigLightDlg();

            GenPrinterDlg rGenericPrintDlg = new GenPrinterDlg();

            FiltroDlg rFiltroDlg = new FiltroDlg();

            rFrmMain.Init();

            bApplicationRuns = true;

            // avvia la form principale
            Application.Run(rFrmMain);
        }
    }
}

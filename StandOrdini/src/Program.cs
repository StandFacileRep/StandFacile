/***********************************************
  	NomeFile : StandOrdini.cs
	Data	 : 06.12.2024
  	Autore   : Mauro Artuso
 ***********************************************/

using System;
using System.Windows.Forms;
using System.Threading;

using StandCommonFiles;
using static StandCommonFiles.ComDef;
using static StandCommonFiles.CommonCl;

namespace StandFacile
{
#pragma warning disable IDE0059

    static class Program
    {
#pragma warning disable IDE0060

        /// <summary>
        /// Punto di ingresso principale dell'applicazione. 
        /// </summary>

        static Mutex _mx;
        public static StandFacile_DB.dBaseIntf_my _rBdBaseIntf_my;
        public static StandFacile_DB.dBaseIntf_pg _rBdBaseIntf_pg;

        [STAThread]
        static void Main(string[] args)
        {
            int iLoop = 5;
            bool bStandIsRunning = false;

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

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            LogServer rLogServer = new LogServer();

            // deve stare qui subito dopo il LogServer !
            MessageDlg rMessageDlg = new MessageDlg();

            FrmMain rFrmMain = new FrmMain();

            int iNDbMode = ReadRegistry(DB_MODE_KEY, (int)DB_MODE.MYSQL);

            if (iNDbMode == (int)DB_MODE.MYSQL)
                _rBdBaseIntf_my = new StandFacile_DB.dBaseIntf_my();
            else
            if (iNDbMode == (int)DB_MODE.POSTGRES)
                _rBdBaseIntf_pg = new StandFacile_DB.dBaseIntf_pg();

            dBaseIntf rdBaseIntf = new dBaseIntf();

            NetConfigLightDlg rNetConfigLightDlg = new NetConfigLightDlg();

            rFrmMain.Init(); // ultimo

            bApplicationRuns = true;

            // avvia la form principale
            Application.Run(rFrmMain);

            _mx.ReleaseMutex();
        }
    }
}

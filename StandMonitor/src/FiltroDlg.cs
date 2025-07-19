/*****************************************************
  	NomeFile  : StandMonitor/FiltroDlg.cs
	Data	 : 06.12.2024
  	Autore	: Mauro Artuso
 *****************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

using static StandCommonFiles.ComDef;
using static StandCommonFiles.LogServer;
using static StandCommonFiles.CommonCl;

using static StandFacile.Define;
using static StandFacile.glb;

namespace StandFacile
{
    /// <summary>classe di filtro delle voci di interesse</summary>
    public partial class FiltroDlg : Form
    {
        static String sNomeFileFiltro;
        static TErrMsg _WrnMsg;

        /// <summary>riferimento a FiltroDlg</summary>
        public static FiltroDlg rFiltroDlg;

        /// <summary>costruttore</summary>
        public FiltroDlg()
        {
            InitializeComponent();

            rFiltroDlg = this;

            sFiltroMon_0 = new List<string>();
            sFiltroMon_1 = new List<string>();
        }

        /// <summary>
        /// lettura del file del filtro
        /// </summary>
        public void CaricaFiltro()
        {
            bool bFiltroMonitor0, bFiltroMonitor1;
            int iCount;
            String sInStr;
            StreamReader fFiltro = null;

            iCount = 0;

            bFiltroMonitor0 = false;
            bFiltroMonitor1 = false;

            sFiltroMon_0.Clear();
            sFiltroMon_1.Clear();
            textBox_Filtro.Clear();

            LogToFile("FrmMain : I CaricaFiltro");

            sNomeFileFiltro = sRootDir + "\\" + FILE_FILTRO;

            if (File.Exists(sNomeFileFiltro))
                fFiltro = File.OpenText(sNomeFileFiltro);
            else
            {
                textBox_Filtro.AppendText("#FM1\r\n\r\n");
                textBox_Filtro.AppendText("#FM2\r\n\r\n");
            }


            if (fFiltro == null)
                LogToFile("FrmMain : Filtro.txt non esiste");
            else
            {

                LogToFile("FrmMain : carica Filtro.txt");

                // ***** caricamento stringhe dal file Filtro *****
                while (((sInStr = fFiltro.ReadLine()) != null) && (iCount < 100))
                {
                    sInStr = sInStr.Trim();

                    iCount++;

                    if (sInStr.Contains("#FM1"))
                    {
                        textBox_Filtro.AppendText(sInStr + "\r\n");
                        bFiltroMonitor0 = true;
                        bFiltroMonitor1 = false;
                        continue;
                    }
                    else if (sInStr.Contains("#FM2"))
                    {
                        textBox_Filtro.AppendText("\r\n");
                        textBox_Filtro.AppendText(sInStr + "\r\n");
                        bFiltroMonitor0 = false;
                        bFiltroMonitor1 = true;
                        continue;
                    }
                    else if (String.IsNullOrEmpty(sInStr))
                        continue;


                    if (bFiltroMonitor0)
                    {
                        if (sInStr.Length >= FILTRO_MIN_LENGTH)
                        {
                            sFiltroMon_0.Add(sInStr);
                            textBox_Filtro.AppendText(sInStr + "\r\n");
                        }
                    }

                    if (bFiltroMonitor1)
                    {
                        if (sInStr.Length >= FILTRO_MIN_LENGTH)
                        {
                            sFiltroMon_1.Add(sInStr);
                            textBox_Filtro.AppendText(sInStr + "\r\n");
                        }
                    }
                }

                fFiltro.Close();
            }
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            StreamWriter fFiltro;

            LogToFile("FiltroFrm : salva Filtro");

            fFiltro = File.CreateText(sNomeFileFiltro);

            _WrnMsg.sNomeFile = FILE_FILTRO;
            _WrnMsg.iErrID = WRN_FNO;
            if (fFiltro == null)
                WarningManager(_WrnMsg);
            else
            {
                fFiltro.WriteLine(textBox_Filtro.Text);
                fFiltro.Close();
            }

            // aggiorna all'uscita
            CaricaFiltro();
            Hide();

            DialogResult = DialogResult.OK;
        }

        private void BtnCanc_Click(object sender, EventArgs e)
        {
            textBox_Filtro.Clear();
            textBox_Filtro.AppendText("#FM1\r\n\r\n");
            textBox_Filtro.AppendText("#FM2\r\n\r\n");
        }

        private void BtnAnnulla_Click(object sender, EventArgs e)
        {
            // aggiorna in caso di uscita con Annulla
            // per ovviare ad eventuali modifiche
            CaricaFiltro();
        }
    }
}

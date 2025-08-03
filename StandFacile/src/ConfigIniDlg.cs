/*****************************************************
  	NomeFile : StandFacile/ConfigIniDlg.cs
	Data	 : 03.08.2025
  	Autore	 : Mauro Artuso
 *****************************************************/
using System;
using System.IO;
using System.Windows.Forms;

using static StandCommonFiles.ComDef;
using static StandCommonFiles.LogServer;
using static StandCommonFiles.CommonCl;

namespace StandFacile
{
    /// <summary>classe di visualizzazione del file config.ini</summary>
    public partial class ConfigIniDlg : Form
    {
        bool _bModificato = false;
        static String _sNomeFileConfig;
        static TErrMsg _WrnMsg;

        /// <summary>riferimento a ConfigIniDlg</summary>
        public static ConfigIniDlg rConfigIniDlg;

        /// <summary>costruttore</summary>
        public ConfigIniDlg()
        {
            InitializeComponent();

            btnEdit.Enabled = CheckService(CFG_COMMON_STRINGS._ESPERTO);
            BtnCanc.Enabled = btnEdit.Enabled;

            CaricaFileConfig();
            ShowDialog();
        }

        /// <summary>
        /// lettura del file del filtro
        /// </summary>
        public void CaricaFileConfig()
        {
            int iCount;
            String sInStr;
            StreamReader fConfig = null;

            iCount = 0;

            textBox_FileConfig.Clear();

            LogToFile("ConfigIniDlg : I CaricaFileConfig");

            _sNomeFileConfig = DataManager.GetExeDir() + "\\" + CONFIG_FILE;

            if (File.Exists(_sNomeFileConfig))
                fConfig = File.OpenText(_sNomeFileConfig);

            if (fConfig == null)
                LogToFile("ConfigIniDlg : config.ini non esiste");
            else
            {

                LogToFile("ConfigIniDlg : carica config.ini");

                // ***** caricamento stringhe dal file Filtro *****
                while (((sInStr = fConfig.ReadLine()) != null) && (iCount < 100))
                {
                    sInStr = sInStr.Trim();

                    iCount++;

                    textBox_FileConfig.AppendText(sInStr + "\r\n");
                    continue;
                }

                fConfig.Close();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            textBox_FileConfig.ReadOnly = false;
        }

        private void textBox_FileConfig_KeyUp(object sender, KeyEventArgs e)
        {
            if (textBox_FileConfig.ReadOnly)
                return;

            _bModificato = true;
        }

        private void BtnAnnulla_Click(object sender, EventArgs e)
        {
            // aggiorna in caso di uscita con Annulla
        }

        private void BtnCanc_Click(object sender, EventArgs e)
        {
            textBox_FileConfig.Clear();
            textBox_FileConfig.AppendText(@"
; file di configurazione di StandFacile
; le righe che iniziano per ';' sono di commento

; stringhe multiple per attivazione modalità speciali
; serviceStrings = ""Esperto noData noLegacyPrinters noStampaRcp""

; parametri interi per attivazione funzioni speciali

; valore di inizio numerazione
; receiptStartNumber = 101

; parametri stringa per attivazione funzioni speciali

; receiptCopyRequired_HeaderIs = COPIA PER SEGRETERIA
; receipt_CS_AltHeader_AH2 = CASSA_2");

            _bModificato = true;
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            StreamWriter fConfig;
            DialogResult dResult = DialogResult.None;

            if (_bModificato)
                dResult = MessageBox.Show("File di configurazione modificato,\r\n\r\nvuoi salvare le modifiche fatte ?", "Attenzione !", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dResult == DialogResult.No)
            {
                DialogResult = DialogResult.Cancel;
                return;
            }

            LogToFile("ConfigIniDlg : salva FileConfig");

            fConfig = File.CreateText(_sNomeFileConfig);

            _WrnMsg.sNomeFile = CONFIG_FILE;
            _WrnMsg.iErrID = WRN_FNO;
            if (fConfig == null)
                WarningManager(_WrnMsg);
            else
            {
                fConfig.WriteLine(textBox_FileConfig.Text);
                fConfig.Close();
            }

            DialogResult = DialogResult.OK;
        }
    }
}

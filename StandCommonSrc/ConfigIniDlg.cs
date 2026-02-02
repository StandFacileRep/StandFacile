/*****************************************************
  	NomeFile : StandFacile/ConfigIniDlg.cs
	Data	 : 03.08.2025
  	Autore	 : Mauro Artuso
 *****************************************************/
using System;
using System.IO;
using System.Drawing;
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

        readonly ToolTip _tt = new ToolTip
        {
            InitialDelay = 50,
            ShowAlways = true
        };

        /// <summary>riferimento a ConfigIniDlg</summary>
        public static ConfigIniDlg rConfigIniDlg;

        /// <summary>costruttore</summary>
        public ConfigIniDlg()
        {
            InitializeComponent();

            _tt.SetToolTip(BtnCanc, "reimposta il contenuto del file config.ini ad un valore standard");
            _tt.SetToolTip(BtnEdit, "abilita la modifica del contenuto");
            _tt.SetToolTip(BtnCancel, "esce senza salvare");
            _tt.SetToolTip(BtnSalva, "salve le modifiche effettuate sul file config.ini");

            BtnEdit.Enabled = FrmMain.rFrmMain.GetEsperto();

            BtnCanc.Enabled = false;
            BtnSalva.Enabled = false;

            textBox.ForeColor = Color.Silver;
            CaricaFileConfig();
            ShowDialog();
        }

        /// <summary>
        /// lettura del file del filtro
        /// </summary>
        public void CaricaFileConfig()
        {
            int iCount;
            String sInStr, sExeDir;
            StreamReader fConfig = null;

            iCount = 0;

            textBox.Clear();

            LogToFile("ConfigIniDlg : I CaricaFileConfig");

            // impostazione della directory per il file Prezzi (la stessa dell'eseguibile)
            sExeDir = Directory.GetCurrentDirectory();


#if STANDFACILE
            _sNomeFileConfig = DataManager.GetExeDir() + "\\" + CONFIG_FILE;
#else
            _sNomeFileConfig = sExeDir + "\\" + CONFIG_FILE;
#endif

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

                    textBox.AppendText(sInStr + "\r\n");
                    continue;
                }

                fConfig.Close();
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            textBox.ForeColor = Color.White;
            textBox.ReadOnly = false;

            BtnEdit.Enabled = false;
            BtnCanc.Enabled = true;
            Refresh();
        }

        private void TextBox_FileConfig_KeyUp(object sender, KeyEventArgs e)
        {
            if (textBox.ReadOnly)
                return;

            _bModificato = true;
            textBox.ForeColor = Color.White;
            BtnEdit.Enabled = false;
            BtnSalva.Enabled = true;
        }

        private void BtnAnnulla_Click(object sender, EventArgs e)
        {
            // aggiorna in caso di uscita con Annulla
        }

        private void BtnCanc_Click(object sender, EventArgs e)
        {
            textBox.Clear();
#if STANDFACILE

            textBox.AppendText(@"
; file di configurazione di StandFacile
; le righe che iniziano per ';' sono di commento

; stringhe multiple per attivazione modalità speciali , vedi manuale ""StandFacile.pdf""
; serviceStrings = ""Esperto noData noLegacyPrinters noStampaRcp skipRcpNum printOnA4Paper printOnA5Paper""

; parametri interi per attivazione funzioni speciali

; valore di inizio numerazione
; receiptStartNumber = 101

; intervallo in ms tra stampe successive
; iPrintInterval = 1000

; indice del gruppo di Articoli per il redirect della stampa copie locali
; localCopyPrinterNumRedirect = 0

; parametri stringa per attivazione funzioni speciali

; receiptCopyRequired_HeaderIs = COPIA PER SEGRETERIA

; receipt_CS_AltHeader_AH2 = CASSA_2");

#elif STAND_CUCINA

            textBox.AppendText(@"
; file di configurazione di StandCucina
; le righe che iniziano per ';' sono di commento

; parametri stringa per attivazione funzioni speciali

; serviceStrings = ""Esperto noLegacyPrinters""
");

#else
            textBox.AppendText(@"
; file di configurazione di StandMonitor
; le righe che iniziano per ';' sono di commento

; parametri interi per attivazione funzioni speciali
; normalmente questo valore è 30, accettabile solo >= 5
; refreshTimer=10

; parametri stringa per attivazione funzioni speciali
; serviceStrings = ""Esperto noLegacyPrinters sortByDeliver reducedColumns superUser""

");

#endif

            _bModificato = true;
            textBox.ForeColor = Color.White;
            BtnEdit.Enabled = false;
            BtnSalva.Enabled = true;

            Refresh();
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            StreamWriter fConfig;
            DialogResult dResult = DialogResult.None;

            if (_bModificato)
                dResult = MessageBox.Show(@"File di configurazione modificato,

vuoi salvare le modifiche fatte ?

Poi bisogna riavviare perchè abbia effetto.", "Attenzione !", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

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
                fConfig.WriteLine(textBox.Text);
                fConfig.Close();
            }

            DialogResult = DialogResult.OK;
        }
    }
}

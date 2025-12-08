/******************************************************
  	NomeFile : StandCommonSrc/NetConfigLightDlg.cs
    Data	 : 18.04.2025
  	Autore   : Mauro Artuso

 ******************************************************/

using System;
using System.Net;
using System.Windows.Forms;
using static StandCommonFiles.ComDef;
using static StandCommonFiles.ComSafe;
using static StandCommonFiles.CommonCl;
using static StandCommonFiles.LogServer;

using static StandFacile.glb;
using static StandFacile.dBaseIntf;

#if !STAND_ORDINI
using static StandCommonFiles.ReceiptAndCopies;
#endif

namespace StandFacile
{
    /// <summary>
    /// classe per la configurazione di rete
    /// </summary>
    public partial class NetConfigLightDlg : Form
    {
#pragma warning disable IDE0044

        static bool _bBarcodeRichiesto = true, _bStampaSoloManuale = true;

        /// <summary>riferimento a NetConfigDlg</summary>
        public static NetConfigLightDlg rNetConfigLightDlg;

        /// <summary>ottiene la richiesta di stampa barcode</summary>
        public static bool GetBarcodeRichiesto() { return _bBarcodeRichiesto; }

        /// <summary>ottiene la spunta del gruppo di copie impostate in NetConfigDlg</summary>
        public static bool GetCopiaGroup(int iParam)
        {
            if ((iParam >= 0) && (iParam < NUM_EDIT_GROUPS))
                return _pCheckBoxCopia[iParam].Checked;
            else
                return false;
        }

        /// <summary>ottiene la flag di stampa solo manuale</summary>
        public static bool GetStampaSoloManuale() { return _bStampaSoloManuale; }

        /// <summary>imposta il puntatore ai checkBox delle copie</summary>
        static CheckBox[] _pCheckBoxCopia = new CheckBox[NUM_EDIT_GROUPS];

        TextBox[] _pTextBoxColor;

        readonly int _iNDbMode;

        /// <summary>costruttore</summary>
        public NetConfigLightDlg()
        {
            int i, iVShift = 0;
            String sTmp;
            _pTextBoxColor = new TextBox[NUM_GROUPS_COLORS];

            InitializeComponent();

            rNetConfigLightDlg = this;

            _pCheckBoxCopia[0] = checkBoxCopia_0;
            _pCheckBoxCopia[1] = checkBoxCopia_1;
            _pCheckBoxCopia[2] = checkBoxCopia_2;
            _pCheckBoxCopia[3] = checkBoxCopia_3;
            _pCheckBoxCopia[4] = checkBoxCopia_4;
            _pCheckBoxCopia[5] = checkBoxCopia_5;
            _pCheckBoxCopia[6] = checkBoxCopia_6;
            _pCheckBoxCopia[7] = checkBoxCopia_7;
            _pCheckBoxCopia[8] = checkBoxCopia_8;

            _pTextBoxColor[0] = textBoxColor_0;
            _pTextBoxColor[1] = textBoxColor_1;
            _pTextBoxColor[2] = textBoxColor_2;
            _pTextBoxColor[3] = textBoxColor_3;

            for (i = 0; i < NUM_GROUPS_COLORS - 1; i++)
                _pTextBoxColor[i].Text = sConstColorsGroupsText[i];

            // configura il combo_TipoDBase
            combo_TipoDBase.Items.Clear();
            combo_TipoDBase.Items.Add("MariaDB, MySQL");
            combo_TipoDBase.Items.Add("PostgreSQL");

            // Inizializza controlli
            _iNDbMode = ReadRegistry(DB_MODE_KEY, (int)DB_MODE.MYSQL) - 1;
            combo_TipoDBase.SelectedIndex = _iNDbMode;

            // configura il Combo_DBServerName
            Combo_DBServerName.Items.Clear();

            for (i = 0; i < MAX_COMBO_ITEMS; i++)
            {
                sTmp = String.Format(SEL_DB_SERVER_KEY, i);
                sTmp = ReadRegistry(sTmp, "");
                if (!String.IsNullOrEmpty(sTmp))
                    Combo_DBServerName.Items.Add(sTmp);
            }

            Combo_DBServerName.Text = ReadRegistry(DBASE_SERVER_NAME_KEY, Dns.GetHostName());

            dbPasswordEdit.Text = Decrypt(ReadRegistry(DBASE_PASSWORD_KEY, DBASE_LAN_PASSWORD));

            if (String.IsNullOrEmpty(Combo_DBServerName.Text))
                Combo_DBServerName.Text = GetDB_ServerName();

            _bBarcodeRichiesto = (ReadRegistry(STAMPA_BARCODE_KEY, 0) == 1);
            checkBoxBarcode.Checked = _bBarcodeRichiesto;

            for (i = 0; i < NUM_GROUPS_COLORS - 1; i++)
                _pTextBoxColor[i].Enabled = false;

            labelColors.Enabled = false;

#if STAND_CUCINA

            _bStampaSoloManuale = (ReadRegistry(Define.STAMPA_MANUALE_KEY, 0) == 1);
            checkBox_StampaManuale.Checked = _bStampaSoloManuale;
#else

            iVShift = 26;

            checkBoxBarcode.Enabled = false;
            checkBoxBarcode.Visible = false;

            checkBox_StampaManuale.Enabled = false;
            checkBox_StampaManuale.Visible = false;
#endif

#if STAND_ORDINI

            labelGroups.Text = "Elenco gruppi di stampa: spunta e colore sono determinati dal Listino:";
            labelGroups.Enabled = false;
            panelCopies.Enabled = false; // disabilita anche tutti i _pCheckBoxCopia[i]

#endif

            labelGroups.Top -= iVShift;
            panelCopies.Top -= iVShift;
            textBoxWarn.Top -= iVShift;
            
            labelColors.Top -= iVShift;

			btnOK.Top -= iVShift;
            btnCancel.Top -= iVShift;

            rNetConfigLightDlg.Height -= iVShift;

            for (i = 0; i < NUM_GROUPS_COLORS - 1; i++)
                _pTextBoxColor[i].Top -= iVShift;

            LogToFile("TFrmNetConfig : TFrmNetConfig");
        }

        /// <summary>Inizializzazione per visualizzazione</summary>
        public void Init(bool bShow)
        {
            // controllo per prima esecuzione, in questo caso la chiamata è inopportuna
            if (!String.IsNullOrEmpty(ReadRegistry(DBASE_SERVER_NAME_KEY, "")))
            {
                _rdBaseIntf.dbCheckStatus();

                // inizializza DB_Data.iGroupsColor[i], _pCheckBoxCopia[]
                // ma prima bisogna chiamare dbCheckStatus();
                NetConfigDlg_Shown(this, null);
            }

            if (bShow)
                ShowDialog();
        }

        // verifica il collegamento con il DB Server e ne salva il nome
        private void BtnDB_ServerTest_Click(object sender, EventArgs e)
        {
            if (_rdBaseIntf.dbCheck(Combo_DBServerName.Text, dbPasswordEdit.Text, combo_TipoDBase.SelectedIndex + 1, false))
            {
                AddTo_ComboList(Combo_DBServerName, SEL_DB_SERVER_KEY);

                SetDbMode(Combo_DBServerName.Text, dbPasswordEdit.Text, combo_TipoDBase.SelectedIndex + 1); // imposta il tipo di DB per dBaseIntf
                _rdBaseIntf.dbCheckStatus();

                // solo se è la prima esecuzione
                if (String.IsNullOrEmpty(ReadRegistry(DBASE_SERVER_NAME_KEY, "")))
                {
                    WriteRegistry(DBASE_SERVER_NAME_KEY, Combo_DBServerName.Text);
                    WriteRegistry(DBASE_PASSWORD_KEY, Encrypt(dbPasswordEdit.Text));
                    WriteRegistry(DB_MODE_KEY, combo_TipoDBase.SelectedIndex + 1);
                }

                NetConfigDlg_Shown(this, null);
                btnOK.Enabled = true;
            }
            else
                btnOK.Enabled = false;

            LogToFile("FrmNetConfig : BtnDB_ServerTestClick");
        }

        void AggiornaAspettoControlli()
        {
            // se è MySQL, oppure è se attivo il corrispondente DB
            if ((combo_TipoDBase.SelectedIndex + 1) == iUSA_NDB() && _rdBaseIntf.GetDB_Connected())
                btnOK.Enabled = true;
            else
                btnOK.Enabled = false;
        }


        /// <summary>
        /// si chiama in caso di problemi di connessione per leggere HeadOrdine, ed inizializzare _pTextBoxColor[i].Text =
        /// </summary>
        public void NetConfig_ReadParams()
        {
            NetConfigDlg_Shown(this, null);
        }

        /// <summary>
        /// questa inizializzazione viene fatta appositamente prima 
        /// di visualizzare la form in modo da essere sicuro
        /// di aver letto HeadOrdine
        /// </summary>
        private void NetConfigDlg_Shown(object sender, EventArgs e)
        {
            int i;
            String sTmp;

            btnOK.Enabled = false;

            // carica iGlbNumOfTickets, iGlbNumOfMessages, _Versione, _Header, _HeaderText
            // solo se non è la prima esecuzione
            if (!String.IsNullOrEmpty(ReadRegistry(DBASE_SERVER_NAME_KEY, "")))
                _rdBaseIntf.dbCaricaOrdine(GetActualDate(), 0, false);

            for (i = 0; i < NUM_EDIT_GROUPS; i++)
            {
                _pCheckBoxCopia[i].BackColor = GetColor(DB_Data.iGroupsColor[i])[0];
                _pCheckBoxCopia[i].ForeColor = GetColor(DB_Data.iGroupsColor[i])[1];

                if (!String.IsNullOrEmpty(DB_Data.sCopiesGroupsText[i]))
                {
                    _pCheckBoxCopia[i].Text = DB_Data.sCopiesGroupsText[i];
                }
            }

            for (i = 0; i < NUM_EDIT_GROUPS; i++)
            {
                sTmp = String.Format(SEL_DB_COPY_KEY, i);

#if STAND_ORDINI
                // per scaricare tutti i gruppi di Articoli
                _pCheckBoxCopia[i].Checked = DB_Data.bCopiesGroupsFlag[i];
#else
                if ((i == (int)DEST_TYPE.DEST_TIPO1) || (i == (int)DEST_TYPE.DEST_TIPO2) || (i == (int)DEST_TYPE.DEST_TIPO3))
                    _pCheckBoxCopia[i].Checked = ((ReadRegistry(sTmp, 1) & 0x01) == 1);
                else
                    _pCheckBoxCopia[i].Checked = ((ReadRegistry(sTmp, 0) & 0x01) == 1);
#endif
            }

            for (i = 0; i < NUM_GROUPS_COLORS - 1; i++)
                _pTextBoxColor[i].Text = DB_Data.sColorGroupsText[i];
        }

        private void CheckBoxCopia_MouseClick(object sender, MouseEventArgs e)
        {
#if STAND_CUCINA || STAND_MONITOR

            int i, iActualIndex = 0;

            for (i = 0; i < NUM_EDIT_GROUPS; i++)
            {
                if (sender == _pCheckBoxCopia[i])
                {
                    _pCheckBoxCopia[i].Checked = !_pCheckBoxCopia[i].Checked;
                    iActualIndex = i;
                    break;
                }
            }

            // estende selezione a tutti i gruppi dello stesso colore
            for (int j = 0; j < NUM_EDIT_GROUPS; j++)
                if ((DB_Data.iGroupsColor[i] == DB_Data.iGroupsColor[j]) && (DB_Data.iGroupsColor[i] > 0) && (i != j))
                    _pCheckBoxCopia[j].Checked = _pCheckBoxCopia[i].Checked;

#endif
            // se è se attivo il corrispondente DB
            if ((combo_TipoDBase.SelectedIndex + 1) == iUSA_NDB() && _rdBaseIntf.GetDB_Connected())
                btnOK.Enabled = true;
        }

        private void Combo_TipoDBase_SelectedIndexChanged(object sender, EventArgs e)
        {
            AggiornaAspettoControlli();
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            // verifica il collegamento con il DB Server e ne salva il nome
            if (_rdBaseIntf.dbCheck(Combo_DBServerName.Text, dbPasswordEdit.Text, combo_TipoDBase.SelectedIndex + 1, true))
            {
                dBaseIntf.SetDbMode(Combo_DBServerName.Text, dbPasswordEdit.Text, combo_TipoDBase.SelectedIndex + 1); // imposta il tipo di DB per dBaseIntf

                if (Combo_DBServerName.Text != ReadRegistry(DBASE_SERVER_NAME_KEY, "") && !String.IsNullOrEmpty(Combo_DBServerName.Text))
                    WriteRegistry(DBASE_SERVER_NAME_KEY, Combo_DBServerName.Text);

                if ((dbPasswordEdit.Text != Decrypt(ReadRegistry(DBASE_PASSWORD_KEY, DBASE_LAN_PASSWORD))) && !String.IsNullOrEmpty(dbPasswordEdit.Text))
                    WriteRegistry(DBASE_PASSWORD_KEY, Encrypt(dbPasswordEdit.Text));

                WriteRegistry(DB_MODE_KEY, combo_TipoDBase.SelectedIndex + 1);

                FrmMain.rFrmMain.Init();
            }

            _bBarcodeRichiesto = checkBoxBarcode.Checked;
            WriteRegistry(STAMPA_BARCODE_KEY, _bBarcodeRichiesto);

#if STAND_CUCINA || STAND_MONITOR
            int i;
            String sTmpKey;

            for (i = 0; i < NUM_EDIT_GROUPS; i++)
            {
                sTmpKey = String.Format(SEL_DB_COPY_KEY, i);
                WriteRegistry(sTmpKey, _pCheckBoxCopia[i].Checked);
            }
#endif

            LogToFile("FrmNetConfig : OKBtnClick");

#if STAND_CUCINA

            _bStampaSoloManuale = checkBox_StampaManuale.Checked;
            WriteRegistry(Define.STAMPA_MANUALE_KEY, _bStampaSoloManuale);

            FrmMain.rFrmMain.VisualizzaTicket(iGlbNumOfTickets);
#endif
            Hide();
        }

        private void NetConfigLightDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            // evita la distruzione della form creando una eccezione alla successiva apertura
            e.Cancel = true;
        }

    }
}

/******************************************************************
 	NomeFile : StandFacile/NetConfigDlg.cs
    Data	 : 06.12.2024
 	Autore   : Mauro Artuso

 ******************************************************************/

using System;
using System.Net;
using System.Drawing;
using System.Windows.Forms;

using static StandCommonFiles.ComDef;
using static StandCommonFiles.ComSafe;
using static StandCommonFiles.CommonCl;
using static StandCommonFiles.LogServer;

using static StandFacile.glb;
using static StandFacile.Define;
using static StandFacile.dBaseIntf;
using static StandFacile.dBaseTunnel_my;

using QRCoder;

// QRcode generation: https://www.youtube.com/watch?v=RCdRprTDMLE

namespace StandFacile
{
    /// <summary>dialogo di configurazione delle connessione alla rete locale</summary>
    public partial class NetConfigDlg : Form
    {
#pragma warning disable IDE0044

        readonly String _sHostName;

        static bool _bOK_EnableLoc, _bOK_EnableRem;
        bool _bInitComplete;

        QRCodeGenerator qrGenerator = new QRCodeGenerator();

        /// <summary>riferimento a NetConfigDlg</summary>
        public static NetConfigDlg rNetConfigDlg;

        /// <summary>
        /// ottiene il flag di abilitazione alla gestione ordini web
        /// </summary>
        public bool GetWebOrderEnabled()
        {
            return WO_ckBox.Checked;
        }

        /// <summary>ottiene il nome del server web</summary>
        public String GetWebServerName()
        {
            return Edit_WebServiceDBaseName.Text;
        }

        /// <summary>costruttore</summary>
        public NetConfigDlg()
        {
            InitializeComponent();

            rNetConfigDlg = this;

            _sHostName = Dns.GetHostName();

            _bOK_EnableLoc = true;
            _bOK_EnableRem = true;

            btnOK.Enabled = false;

            // anche la CASSA_SECONDARIA puo collegarsi per lo scarico degli ordini
            WO_ckBox.Enabled = true;
            link_QRcode.Enabled = true;
            link_QRcode.BorderStyle = BorderStyle.None;

            Init(false);
        }

        /// <summary>Init</summary>
        public void Init(bool bShow)
        {
            int i, iTipoDBItem;
            String sStrTmp;

            _bInitComplete = false;

            // configura il Combo_DBServerName
            Combo_DBServerName.Items.Clear();
            for (i = MAX_COMBO_ITEMS - 1; (i >= 0); i--)
            {
                sStrTmp = String.Format(SEL_DB_SERVER_KEY, i);
                sStrTmp = ReadRegistry(sStrTmp, "");

                if (!String.IsNullOrEmpty(sStrTmp))
                    Combo_DBServerName.Items.Insert(0, sStrTmp);
            }

            // configura il combo_TipoDBase
            combo_TipoDBase.Items.Clear();
            combo_TipoDBase.Items.Add("locale SQLite");
            combo_TipoDBase.Items.Add("di rete MariaDB, MySQL");
            combo_TipoDBase.Items.Add("di rete PostgreSQL");

            // configura il ComboNumCassa
            Combo_NumCassa.Items.Clear();
            for (i = MAX_CASSE_SECONDARIE; (i >= 0); i--)
                Combo_NumCassa.Items.Insert(0, sConstCassaType[i]);

            // Inizializza controlli
            iTipoDBItem = (int)(iUSA_NDB());
            combo_TipoDBase.SelectedIndex = iTipoDBItem;

            Combo_DBServerName.Text = ReadRegistry(DBASE_SERVER_NAME_KEY, _sHostName);
            dbPasswordEdit.Text = Decrypt(ReadRegistry(DBASE_PASSWORD_KEY, DBASE_LAN_PASSWORD));

            // impostate da dBaseTunnel_my in base a SF_Data.iNumCassa
            Edit_WebServiceDBaseName.Text = _sWebServerParams.sWeb_DBase;
            Edit_WebServiceDBasePwd.Text = Decrypt_WS(_sWebServerParams.sWebEncryptedPwd);
            Edit_WebService_Name.Text = _sWebServerParams.sWebTablePrefix;

            // se è MySQL, oppure è se attivo il corrispondente DB
            if ((combo_TipoDBase.SelectedIndex == 0) || combo_TipoDBase.SelectedIndex == iUSA_NDB())
                _bOK_EnableLoc = true;
            else
                _bOK_EnableLoc = false;

            Combo_NumCassa.SelectedIndex = ReadRegistry(NUM_CASSA_KEY, 1) - 1;

            // anche la CASSA_SECONDARIA puo collegarsi per lo scarico degli ordini
            WO_ckBox.Checked = (ReadRegistry(WEB_SERVICE_MODE_KEY, 0) == 1);

            if (Edit_WebServiceDBaseName.Text == "standfacile_rdb")
                link_QRcode.Text = String.Format("https://localhost/standfacile_{0}_php/{1}/index.php", sConfig.sWebUrlVersion, Edit_WebService_Name.Text);
            else
            {
                if (!String.IsNullOrEmpty(Edit_WebService_Name.Text))
                    link_QRcode.Text = String.Format("https://{0}/standfacile_{1}_php/{2}/index.php", URL_SITO, sConfig.sWebUrlVersion, Edit_WebService_Name.Text);
                else
                    link_QRcode.Text = String.Format("https://{0}/{1}", URL_SITO, "controlla");
            }
            _bInitComplete = true; // per non scatenare l'evento onClick

            AggiornaAspettoControlli();

            if (bShow)
                ShowDialog();
        }

        void AggiornaAspettoControlli()
        {
            if (!_bInitComplete)
                return;

            Combo_DBServerName.Enabled = true;
            Btn_DBServerTest.Enabled = true;

            dbPasswordEdit.Enabled = true;

            if ((Combo_NumCassa.SelectedIndex + 1) == CASSA_PRINCIPALE)
                textBoxCassa.Text = "La cassa principale può modificare Listino, Disponibilità, impostazioni di stampa delle copie.\r\n" +
                    "Le impostazioni verranno passate tramite database alle casse secondarie.";
            else
                textBoxCassa.Text = "La cassa secondaria riceve dal database Listino, Disponibilità ed impostazioni di stampa delle copie.";


            if (combo_TipoDBase.SelectedIndex > 0)
            {
                Combo_NumCassa.Enabled = true;

                Combo_DBServerName.Enabled = true;
                Btn_DBServerTest.Enabled = true;

                dbPasswordEdit.Enabled = true;

                Panel_Tipo_Cassa.Enabled = true;
            }
            else
            {
                Combo_NumCassa.Enabled = false;
                Combo_NumCassa.SelectedIndex = 0;

                Combo_DBServerName.Enabled = false;
                Btn_DBServerTest.Enabled = false;

                dbPasswordEdit.Enabled = false;

                Panel_Tipo_Cassa.Enabled = false;
            }

            if (WO_ckBox.Checked)
            {
                if (SF_Data.iNumCassa == CASSA_PRINCIPALE)
                {
                    NC_lbl5.Enabled = true;
                    NC_lbl6.Enabled = true;
                    NC_lbl7.Enabled = true;
                    NC_lbl8.Enabled = true;
                    Edit_WebServiceDBaseName.Enabled = true;
                    Edit_WebServiceDBasePwd.Enabled = true;
                    Edit_WebService_Name.Enabled = true;
                }
                else
                {
                    NC_lbl5.Enabled = false;
                    NC_lbl6.Enabled = false;
                    NC_lbl7.Enabled = false;
                    NC_lbl8.Enabled = false;
                    Edit_WebServiceDBaseName.Enabled = false;
                    Edit_WebServiceDBasePwd.Enabled = false;
                    Edit_WebService_Name.Enabled = false;
                }

                NC_btn_webSiteTest.Enabled = true;
                //link_QRcode.Enabled = true;
                pictureBox_QRCode.Enabled = true;

                QRCodeData qrCodeData = qrGenerator.CreateQrCode(link_QRcode.Text, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                Bitmap qrCodeImage = qrCode.GetGraphic(20, Color.Black, Color.White, true);
                pictureBox_QRCode.Image = qrCodeImage;
            }
            else
            {
                NC_lbl5.Enabled = false;
                NC_lbl6.Enabled = false;
                NC_lbl7.Enabled = false;
                NC_lbl8.Enabled = false;
                Edit_WebServiceDBaseName.Enabled = false;
                Edit_WebServiceDBasePwd.Enabled = false;
                Edit_WebService_Name.Enabled = false;
                NC_btn_webSiteTest.Enabled = false;
                //link_QRcode.Enabled = false;
                pictureBox_QRCode.Enabled = false;

                QRCodeData qrCodeData = qrGenerator.CreateQrCode(link_QRcode.Text, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                Bitmap qrCodeImage = qrCode.GetGraphic(20, Color.LightGray, Color.White, true);
                pictureBox_QRCode.Image = qrCodeImage;
            }

            if (WO_ckBox.Checked)
                // necessari 2 consensi
                btnOK.Enabled = _bOK_EnableLoc && _bOK_EnableRem;
            else
                // necessario 1 consenso
                btnOK.Enabled = _bOK_EnableLoc;
        }

        private void Combo_NumCassa_SelectedIndexChanged(object sender, EventArgs e)
        {
            // se è MySQL, oppure è se attivo il corrispondente DB
            if ((combo_TipoDBase.SelectedIndex == 0) || combo_TipoDBase.SelectedIndex == iUSA_NDB())
                _bOK_EnableLoc = true;
            else
                _bOK_EnableLoc = false;

            AggiornaAspettoControlli();
        }

        private void Btn_DBServerTest_Click(object sender, EventArgs e)
        {
            if (_rdBaseIntf.dbCheck(Combo_DBServerName.Text, dbPasswordEdit.Text, combo_TipoDBase.SelectedIndex))
            {
                _bOK_EnableLoc = true;
                AddTo_ComboList(Combo_DBServerName, SEL_DB_SERVER_KEY);
            }
            else
                _bOK_EnableLoc = false;

            AggiornaAspettoControlli();

            String sTmp = String.Format("TFrmNetConfig, _bOK_EnableLoc = {0}, _bOK_EnableRem = {1}", _bOK_EnableLoc, _bOK_EnableRem);
            LogToFile(sTmp);

        }

        private void WO_ckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (WO_ckBox.Checked)
                // se era già attivo non è cambiato
                _bOK_EnableRem = (ReadRegistry(WEB_SERVICE_MODE_KEY, 0) == 1);
            else
                _bOK_EnableRem = true;

            AggiornaAspettoControlli();
        }

        private void NC_btn_webSiteTest_Click(object sender, EventArgs e)
        {
            if (rdbCheckConnection(Edit_WebService_Name.Text, Edit_WebServiceDBaseName.Text, Edit_WebServiceDBasePwd.Text, false))
                _bOK_EnableRem = true;
            else
                _bOK_EnableRem = false;

            AggiornaAspettoControlli();

            String sTmp = String.Format("TFrmNetConfig, _bOK_EnableLoc = {0}, _bOK_EnableRem = {1}", _bOK_EnableLoc, _bOK_EnableRem);
            LogToFile(sTmp);
        }

        private void Link_QRcodeClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.link_QRcode.LinkVisited = true;

            System.Diagnostics.Process.Start(link_QRcode.Text);
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            bool bRiavvio = false;
            String sTmp;

            if (combo_TipoDBase.SelectedIndex != iUSA_NDB())
            {
                WriteRegistry(DB_MODE_KEY, combo_TipoDBase.SelectedIndex);

                bRiavvio = true;
            }

            if ((Combo_DBServerName.Text != ReadRegistry(DBASE_SERVER_NAME_KEY, "")) && (!String.IsNullOrEmpty(Combo_DBServerName.Text))) // 1, 2
            {
                WriteRegistry(DBASE_SERVER_NAME_KEY, Combo_DBServerName.Text);

                bRiavvio = true;
            }

            if ((ReadRegistry(DB_MODE_KEY, 0) > 0) && (dbPasswordEdit.Text != Decrypt(ReadRegistry(DBASE_PASSWORD_KEY, DBASE_LAN_PASSWORD))) &&
                (!String.IsNullOrEmpty(dbPasswordEdit.Text)))
            {
                WriteRegistry(DBASE_PASSWORD_KEY, Encrypt(dbPasswordEdit.Text));

                bRiavvio = true;
            }

            if ((Combo_NumCassa.SelectedIndex + 1) != SF_Data.iNumCassa) // 3
            {
                WriteRegistry(NUM_CASSA_KEY, Combo_NumCassa.SelectedIndex + 1);
                bRiavvio = true;
            }

            if (WO_ckBox.Checked != (ReadRegistry(WEB_SERVICE_MODE_KEY, 0) == 1))
            {
                WriteRegistry(WEB_SERVICE_MODE_KEY, WO_ckBox.Checked ? 1 : 0);
                bRiavvio = true; // il riavvio serve a dBaseTunnel_my
            }

            // Web params
            if (WO_ckBox.Checked && (SF_Data.iNumCassa == CASSA_PRINCIPALE))
            {
                if ((Edit_WebServiceDBaseName.Text != _sWebServerParams.sWeb_DBase) && (!String.IsNullOrEmpty(Edit_WebServiceDBaseName.Text)))
                {
                    _sWebServerParams.sWeb_DBase = Edit_WebServiceDBaseName.Text;
                    WriteRegistry(WEB_DBASE_NAME_KEY, Edit_WebServiceDBaseName.Text);

                    bRiavvio = true;
                }

                if ((Edit_WebServiceDBasePwd.Text != Decrypt_WS(_sWebServerParams.sWebEncryptedPwd)) &&
                    (!String.IsNullOrEmpty(Edit_WebServiceDBasePwd.Text)))
                {
                    _sWebServerParams.sWebEncryptedPwd = Encrypt_WS(Edit_WebServiceDBasePwd.Text);
                    WriteRegistry(WEB_DBASE_PWD_KEY, Encrypt_WS(Edit_WebServiceDBasePwd.Text));

                    bRiavvio = true;
                }

                if ((Edit_WebService_Name.Text != _sWebServerParams.sWebTablePrefix) && (!String.IsNullOrEmpty(Edit_WebService_Name.Text))) // 1, 2
                {
                    _sWebServerParams.sWebTablePrefix = Edit_WebService_Name.Text;
                    WriteRegistry(WEB_SERVER_NAME_KEY, Edit_WebService_Name.Text);

                    bRiavvio = true;
                }

                _rdBaseIntf.dbSetWebServerParams(_sWebServerParams, combo_TipoDBase.SelectedIndex);
            }

            if (bRiavvio)
            {
                MessageBox.Show("Il tipo di Cassa e/o le impostazioni Database sono cambiate,\nil programma verrà riavviato !", "Attenzione !", MessageBoxButtons.OK);

                sTmp = String.Format("TFrmNetConfig, Tipo di database = {0}, DB Server = {1}", (ReadRegistry(DB_MODE_KEY, 0) > 0), Combo_DBServerName.Text);
                LogToFile(sTmp);

                ErrorManager(ERR_CDB);
            }

            LogToFile("TFrmNetConfig : OKBtnClick");
        }

    }
}

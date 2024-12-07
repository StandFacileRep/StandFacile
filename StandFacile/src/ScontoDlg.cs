/***********************************************
	NomeFile : StandFacile/ScontoDlg.cs
	Data	 : 28.08.2024
	Autore	 : Mauro Artuso
 ***********************************************/

using System;
using System.Windows.Forms;

using static StandCommonFiles.ComDef;
using static StandCommonFiles.commonCl;

using static StandFacile.glb;
using static StandFacile.Define;
using static StandCommonFiles.LogServer;

namespace StandFacile
{
    /// <summary>
    /// classe per la scelta dello sconto
    /// </summary>
    public partial class ScontoDlg : Form
    {
        /// <summary>numero minimo di caratteri della motivazione</summary>
        const int MOTIVAZIONE_MIN_CAR = 8;

        TErrMsg _WrnMsg = new TErrMsg();

        readonly ToolTip _tt = new ToolTip
        {
            InitialDelay = 50,
            ShowAlways = true
        };

        /// <summary>struct per gestione sconti</summary>
        static TSconto _Sconto = new TSconto(0);

        /// <summary>struct temp per gestione sconti</summary>
        static TSconto _scontoTmp = new TSconto(0);

        /// <summary>riferimento a ScontoDlg</summary>
        public static ScontoDlg rScontoDlg;

        static CheckBox[] _pCheckBoxGrp;

        /// <summary>ottiene la struct di sconto applicato</summary>
        static public TSconto _getSconto() { return _Sconto; }

        /// <summary>resetta solo il tipo di sconto applicato</summary>
        static public void _resetSconto()
        {
            // meglio impostare solo lo stato con 0x7FFFFF00
            _Sconto.iStatusSconto &= 0x7FFFFF00;
        }

        /// <summary>costruttore</summary>
        public ScontoDlg()
        {
            InitializeComponent();

            _pCheckBoxGrp = new CheckBox[NUM_EDIT_GROUPS];

            _pCheckBoxGrp[0] = ckBox_0;
            _pCheckBoxGrp[1] = ckBox_1;
            _pCheckBoxGrp[2] = ckBox_2;
            _pCheckBoxGrp[3] = ckBox_3;
            _pCheckBoxGrp[4] = ckBox_4;
            _pCheckBoxGrp[5] = ckBox_5;
            _pCheckBoxGrp[6] = ckBox_6;
            _pCheckBoxGrp[7] = ckBox_7;

            _tt.SetToolTip(DS_btnSave, "salve le impostazioni nel Listino");
            _tt.SetToolTip(radioBtn100, "clicca per impostare il 100%, cioè gratis per i gruppi selezionati");
            _tt.SetToolTip(textBoxPercVal, "imposta lo sconto nel range 1%-99%, per impostare il 100% clicca sul pulsante accanto");
            _tt.SetToolTip(textBoxFixVal, "imposta lo sconto fisso");

            rScontoDlg = this;

            _resetSconto();
        }

        /// <summary>Init</summary>
        public void Init()
        {
            // copia locale per valore
            _scontoTmp = deepCopy(_Sconto);

            // provoca inizializzazione grafica
            if (IsBitSet(_scontoTmp.iStatusSconto, BIT_SCONTO_STD))
            {
                DS_rdBtnDiscountStd.Checked = true;
                DS_rdBtnDiscountStd_Click(null, null);
            }
            else if (IsBitSet(_scontoTmp.iStatusSconto, BIT_SCONTO_FISSO))
            {
                DS_rdBtnDiscountFixed.Checked = true;
                DS_rdBtnDiscountFixed_Click(null, null);
            }
            else if (IsBitSet(_scontoTmp.iStatusSconto, BIT_SCONTO_GRATIS))
            {
                DS_rdBtnDiscountGratis.Checked = true;
                DS_rdBtnDiscountGratis_Click(null, null);
            }
            else
            {
                DS_rdBtnDiscountNone.Checked = true;
                DS_rdBtnDiscountNone_Click(null, null);
            }

            // controlli correttezza ed inizializzazione grafica
            check_allItems();

            if (!Visible)
                ShowDialog();
        }

        /// <summary>
        /// Verifica se ci sono le condizioni per abilitare i pulsanti di Ok, Save
        /// ed il colore di sfondo di textBoxMotivazione
        /// </summary>
        void check_allItems()
        {
            // per abilitare il pulsante di Save devono essere OK tutti i controlli, indipendentemente dallo stato
            if ((SF_Data.iNumCassa == CASSA_PRINCIPALE) && FrmMain.rFrmMain.bGetEsperto() &&
                ((String.IsNullOrEmpty(_scontoTmp.sScontoText[(int)DISC_TYPE.DISC_STD]) && (_scontoTmp.iScontoValPerc == 0)) || ((_scontoTmp.sScontoText[(int)DISC_TYPE.DISC_STD].Length >= MOTIVAZIONE_MIN_CAR) && (_scontoTmp.iScontoValPerc > 0))) &&
                ((String.IsNullOrEmpty(_scontoTmp.sScontoText[(int)DISC_TYPE.DISC_FIXED]) && (_scontoTmp.iScontoValFisso == 0)) || ((_scontoTmp.sScontoText[(int)DISC_TYPE.DISC_FIXED].Length >= MOTIVAZIONE_MIN_CAR) && (_scontoTmp.iScontoValFisso > 0))) &&
                 (String.IsNullOrEmpty(_scontoTmp.sScontoText[(int)DISC_TYPE.DISC_GRATIS]) || (_scontoTmp.sScontoText[(int)DISC_TYPE.DISC_GRATIS].Length >= MOTIVAZIONE_MIN_CAR))
               )
            {
                DS_btnSave.Enabled = true;
            }
            else
                DS_btnSave.Enabled = false;

            // pulsante OK
            if (DS_rdBtnDiscountStd.Checked)
            {
                // deve essere ok l'inserimento importo fisso e la motivazione
                if ((textBoxMotivazione.Text.Length >= MOTIVAZIONE_MIN_CAR) && (_scontoTmp.iScontoValPerc > 0))
                    btnOK.Enabled = true;
                else
                    btnOK.Enabled = false;
            }
            else if (DS_rdBtnDiscountFixed.Checked)
            {
                // deve essere ok l'inserimento importo fisso e la motivazione
                if ((textBoxMotivazione.Text.Length >= MOTIVAZIONE_MIN_CAR) && (_scontoTmp.iScontoValFisso > 0))
                    btnOK.Enabled = true;
                else
                    btnOK.Enabled = false;
            }
            else if (DS_rdBtnDiscountGratis.Checked)
            {
                // deve essere ok l'inserimento percentuale e la motivazione
                if (textBoxMotivazione.Text.Length >= MOTIVAZIONE_MIN_CAR)
                    btnOK.Enabled = true;
                else
                    btnOK.Enabled = false;
            }
            else // altri casi
            {
                btnOK.Enabled = true;
            }

            // colore textBox motivazione
            if ((textBoxMotivazione.Text.Length >= MOTIVAZIONE_MIN_CAR) || DS_rdBtnDiscountNone.Checked)
            {
                textBoxMotivazione.BackColor = System.Drawing.SystemColors.Window;
                textBoxMotivazione.ForeColor = System.Drawing.SystemColors.WindowText;
            }
            else
            {
                textBoxMotivazione.BackColor = System.Drawing.Color.Red;
                textBoxMotivazione.ForeColor = System.Drawing.SystemColors.HighlightText;
            }
        }

        private void DS_rdBtnDiscountNone_Click(object sender, EventArgs e)
        {
            _scontoTmp.iStatusSconto &= 0x7FFFFF00;

            textBoxPercVal.Enabled = false;
            textBoxPercVal.Text = "";
            textBoxPercVal.BackColor = System.Drawing.SystemColors.Window;

            DS_lblDiscountPerc.Enabled = false;
            radioBtn100.Enabled = false;

            for (int i = 0; i < NUM_EDIT_GROUPS; i++)
            {
                _pCheckBoxGrp[i].Enabled = false;
                _pCheckBoxGrp[i].Checked = false;
            }

            DS_lblDiscountVal.Enabled = false;
            textBoxFixVal.Enabled = false;
            textBoxFixVal.Text = "";
            textBoxFixVal.BackColor = System.Drawing.SystemColors.Window;

            DS_lblDiscountTxt.Enabled = false;
            textBoxMotivazione.Enabled = false;
            textBoxMotivazione.Text = "";

            check_allItems();
        }

        private void DS_rdBtnDiscountStd_Click(object sender, EventArgs e)
        {
            _scontoTmp.iStatusSconto &= 0x7FFFFF00;
            _scontoTmp.iStatusSconto = SetBit(_scontoTmp.iStatusSconto, BIT_SCONTO_STD);

            DS_lblDiscountPerc.Enabled = true;
            DS_lblDiscountTxt.Enabled = true;

            textBoxPercVal.Text = _scontoTmp.iScontoValPerc.ToString();
            textBoxPercVal.Enabled = true;
            TextBoxPerc_KeyUp(null, null);

            // setup Flags
            for (int i = 0; i < NUM_EDIT_GROUPS; i++)
            {
                if ((SF_Data.iNumCassa == CASSA_PRINCIPALE) && FrmMain.rFrmMain.bGetEsperto())
                    _pCheckBoxGrp[i].Enabled = true;

                _pCheckBoxGrp[i].Checked = IsBitSet(_scontoTmp.iStatusSconto, 8 + i);
            }

            radioBtn100.Enabled = true;

            DS_lblDiscountVal.Enabled = false;
            textBoxFixVal.Text = "";
            textBoxFixVal.Enabled = false;
            textBoxFixVal.BackColor = System.Drawing.SystemColors.Window;

            textBoxMotivazione.Text = _scontoTmp.sScontoText[(int)DISC_TYPE.DISC_STD];
            textBoxMotivazione.Enabled = true;

            check_allItems();
        }

        private void DS_rdBtnDiscountFixed_Click(object sender, EventArgs e)
        {
            _scontoTmp.iStatusSconto &= 0x7FFFFF00;
            _scontoTmp.iStatusSconto = SetBit(_scontoTmp.iStatusSconto, BIT_SCONTO_FISSO);

            DS_lblDiscountPerc.Enabled = false;
            DS_lblDiscountTxt.Enabled = true;
            DS_lblDiscountVal.Enabled = true;

            textBoxPercVal.Text = "";
            textBoxPercVal.Enabled = false;
            textBoxPercVal.BackColor = System.Drawing.SystemColors.Window;
            radioBtn100.Enabled = false;

            for (int i = 0; i < NUM_EDIT_GROUPS; i++)
            {
                _pCheckBoxGrp[i].Enabled = false;
                _pCheckBoxGrp[i].Checked = false;
            }

            textBoxFixVal.Text = IntToEuro(_scontoTmp.iScontoValFisso);
            textBoxFixVal.Enabled = true;
            TextBoxFixVal_KeyUp(null, null);

            textBoxMotivazione.Text = _scontoTmp.sScontoText[(int)DISC_TYPE.DISC_FIXED];
            textBoxMotivazione.Enabled = true;

            check_allItems();
        }

        private void DS_rdBtnDiscountGratis_Click(object sender, EventArgs e)
        {
            _scontoTmp.iStatusSconto &= 0x7FFFFF00;
            _scontoTmp.iStatusSconto = SetBit(_scontoTmp.iStatusSconto, BIT_SCONTO_GRATIS);

            DS_lblDiscountPerc.Enabled = false;
            DS_lblDiscountVal.Enabled = false;
            DS_lblDiscountTxt.Enabled = true;

            textBoxPercVal.Enabled = false;
            textBoxPercVal.Text = "";
            textBoxPercVal.BackColor = System.Drawing.SystemColors.Window;
            radioBtn100.Enabled = false;

            for (int i = 0; i < NUM_EDIT_GROUPS; i++)
            {
                _pCheckBoxGrp[i].Enabled = false;
                _pCheckBoxGrp[i].Checked = false;
            }

            textBoxFixVal.Text = "";
            textBoxFixVal.Enabled = false;
            textBoxFixVal.BackColor = System.Drawing.SystemColors.Window;

            textBoxMotivazione.Text = _scontoTmp.sScontoText[(int)DISC_TYPE.DISC_GRATIS];
            textBoxMotivazione.Enabled = true;

            check_allItems();
        }

        private void radioBtn100_Click(object sender, EventArgs e)
        {
            textBoxPercVal.Text = "100";
            TextBoxPerc_KeyUp(this, null);
        }

        // ammessi numeri e backspace per cancellazione
        private void TextBoxPerc_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || (e.KeyChar == '\b')))
                e.Handled = true;
        }

        private void TextBoxPerc_KeyUp(object sender, KeyEventArgs e)
        {
            textBoxPercVal.Text = textBoxPercVal.Text.Trim();

            try
            {
                if (!String.IsNullOrEmpty(textBoxPercVal.Text))
                {
                    _scontoTmp.iScontoValPerc = Convert.ToInt32(textBoxPercVal.Text);

                    _scontoTmp.iStatusSconto &= 0x7F00FFFF;
                    _scontoTmp.iStatusSconto += (_scontoTmp.iScontoValPerc << 16);    // 0x00FF0000

                    if (_scontoTmp.iScontoValPerc == 0)
                    {
                        textBoxPercVal.BackColor = System.Drawing.Color.Red;
                        textBoxPercVal.ForeColor = System.Drawing.SystemColors.HighlightText;
                    }
                    else
                    {
                        textBoxPercVal.BackColor = System.Drawing.SystemColors.Window;
                        textBoxPercVal.ForeColor = System.Drawing.SystemColors.WindowText;
                    }
                }
                else
                {
                    _scontoTmp.iScontoValPerc = 0;
                    textBoxPercVal.BackColor = System.Drawing.Color.Red;
                    textBoxPercVal.ForeColor = System.Drawing.SystemColors.HighlightText;
                }
            }
            catch (Exception)
            {
                _scontoTmp.iScontoValPerc = 0;
                textBoxPercVal.BackColor = System.Drawing.Color.Red;
                textBoxPercVal.ForeColor = System.Drawing.SystemColors.HighlightText;
            }

            if (sender != null)
                check_allItems();
        }

        // attenzione che sia un click altrimenti viene chiamata in modo inopportuno on "changed"
        private void ckBox_Click(object sender, EventArgs e)
        {
            // per compatibilità dimensione vettore
            for (int i = 0; i < NUM_EDIT_GROUPS; i++)
                _scontoTmp.bScontoGruppo[i] = false;

            _scontoTmp.iStatusSconto &= 0x7FFF00FF;

            for (int i = 0; i < NUM_EDIT_GROUPS; i++)
            {
                if (_pCheckBoxGrp[i].Checked)
                {
                    _scontoTmp.iStatusSconto += (int)Math.Pow(2, 8 + i); // 0x0000FF00
                    _scontoTmp.bScontoGruppo[i] = true;
                }
                else
                    _scontoTmp.bScontoGruppo[i] = false;
            }

        }

        private void TextBoxFixVal_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || (e.KeyChar == '.') || (e.KeyChar == ',') || (e.KeyChar == '\b')))
                e.Handled = true;
        }

        private void TextBoxFixVal_KeyUp(object sender, KeyEventArgs e)
        {
            int iPrz = 0;

            textBoxFixVal.Text = textBoxFixVal.Text.Trim();

            iPrz = EuroToInt(textBoxFixVal.Text, EURO_CONVERSION.EUROCONV_NZ, _WrnMsg);

            if (iPrz > 0) // conversione Ok
            {
                textBoxFixVal.BackColor = System.Drawing.SystemColors.Window;
                textBoxFixVal.ForeColor = System.Drawing.SystemColors.WindowText;

                _scontoTmp.iScontoValFisso = iPrz;
            }
            else
            {
                // Errore di formato del Prezzo in Euro !
                textBoxFixVal.BackColor = System.Drawing.Color.Red;
                textBoxFixVal.ForeColor = System.Drawing.SystemColors.HighlightText;
                _scontoTmp.iScontoValFisso = 0;
            }

            if (sender != null)
                check_allItems();
        }

        private void TextBoxMotivazione_KeyUp(object sender, KeyEventArgs e)
        {
            if (DS_rdBtnDiscountStd.Checked)
                _scontoTmp.sScontoText[(int)DISC_TYPE.DISC_STD] = textBoxMotivazione.Text;
            else if (DS_rdBtnDiscountFixed.Checked)
                _scontoTmp.sScontoText[(int)DISC_TYPE.DISC_FIXED] = textBoxMotivazione.Text;
            else if (DS_rdBtnDiscountGratis.Checked)
                _scontoTmp.sScontoText[(int)DISC_TYPE.DISC_GRATIS] = textBoxMotivazione.Text;

            check_allItems();
        }

        /// <summary>salva tutti i dati Sconto nel Listino</summary>
        private void DS_btnSalva_Click(object sender, EventArgs e)
        {
            if (!((SF_Data.iNumCassa == CASSA_PRINCIPALE) && FrmMain.rFrmMain.bGetEsperto()))
                return;

            _Sconto = deepCopy(_scontoTmp);

            DataManager.bSalvaListino();

            MessageBox.Show("Impostazione Salvate !", "Attenzione !", MessageBoxButtons.OK);
        }

        private void BT_Cancel_Click(object sender, EventArgs e)
        {
            //_resetSconto();
        }

        /// <summary>salva solo i dati selezionati nella Struct TSconto</summary>
        private void BtnOK_Click(object sender, EventArgs e)
        {
            bool bListinoModificato = false;
            String sTmp;

            DialogResult dResult = DialogResult.No;

            if ((_Sconto.iScontoValFisso != _scontoTmp.iScontoValFisso) || (_Sconto.iScontoValPerc != _scontoTmp.iScontoValPerc))
                bListinoModificato = true;

            for (int i = 0; i < NUM_EDIT_GROUPS; i++)
            {
                if (_Sconto.bScontoGruppo[i] != _scontoTmp.bScontoGruppo[i])
                    bListinoModificato = true;
            }

            for (int i = 1; i < NUM_DISC_TYPE; i++)
            {
                if ((_Sconto.sScontoText[i] != _scontoTmp.sScontoText[i]))
                    bListinoModificato = true;
            }

            if (bListinoModificato)
            {
                dResult = MessageBox.Show("Sei sicuro di modificare lo sconto ?", "Attenzione !", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }
            else if (!DS_rdBtnDiscountNone.Checked)
            {
                dResult = MessageBox.Show("Sei sicuro di applicare lo sconto ?", "Attenzione !", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }

            if (dResult == DialogResult.Yes)
            {
                _Sconto = deepCopy(_scontoTmp);

                SF_Data.iStatusSconto = _scontoTmp.iStatusSconto;

                // SF_Data.iScontoStdReceipt = _Sconto.iStatusSconto non serve
                // dato che viene ricalcolato a partire dalla % dentro a SF_Data.iStatusSconto
                SF_Data.iScontoFissoReceipt = _scontoTmp.iScontoValFisso;

                if (DS_rdBtnDiscountStd.Checked)
                    SF_Data.sScontoReceipt = _scontoTmp.sScontoText[(int)DISC_TYPE.DISC_STD];
                else if (DS_rdBtnDiscountFixed.Checked)
                    SF_Data.sScontoReceipt = _scontoTmp.sScontoText[(int)DISC_TYPE.DISC_FIXED];
                else if (DS_rdBtnDiscountGratis.Checked)
                    SF_Data.sScontoReceipt = _scontoTmp.sScontoText[(int)DISC_TYPE.DISC_GRATIS];

                bListinoModificato = false;
            }
            else
            {
                _resetSconto();

                // così si resetta il pulsante del carrello
                SF_Data.iStatusSconto = _scontoTmp.iStatusSconto;
            }

            sTmp = String.Format("ScontoDlg : SF_Data.iStatusSconto = {0:X8}", SF_Data.iStatusSconto);
            LogToFile(sTmp);
        }

        /// <summary>imposta il tipo di sconto applicato e tutta la struct</summary>
        public static void setSconto(DISC_TYPE eDiscParam, int iScontoFlagParam, int iScontoValParam, string sScontoTextParam)
        {
            switch (eDiscParam)
            {
                case DISC_TYPE.DISC_STD:
                    _Sconto.iStatusSconto = SetBit(0, BIT_SCONTO_STD);             // 0x000000FF
                    _Sconto.iStatusSconto += (iScontoFlagParam << 8);   // 0x0000FF00
                    _Sconto.iStatusSconto += (iScontoValParam << 16);   // 0x00FF0000

                    // per compatibilità dimensione vettore
                    for (int i = 0; i < NUM_EDIT_GROUPS; i++)
                        _Sconto.bScontoGruppo[i] = false;

                    for (int h = 0; h < NUM_EDIT_GROUPS; h++)
                    {
                        if (IsBitSet(iScontoFlagParam, h))
                            _Sconto.bScontoGruppo[h] = true;
                    }

                    _Sconto.iScontoValPerc = iScontoValParam;
                    _Sconto.sScontoText[(int)DISC_TYPE.DISC_STD] = sScontoTextParam;
                    break;

                case DISC_TYPE.DISC_FIXED:
                    _Sconto.iStatusSconto &= 0x7FFFFF00;
                    _Sconto.iStatusSconto = SetBit(_Sconto.iStatusSconto, BIT_SCONTO_FISSO);

                    _Sconto.iScontoValFisso = iScontoValParam;
                    _Sconto.sScontoText[(int)DISC_TYPE.DISC_FIXED] = sScontoTextParam;
                    break;

                case DISC_TYPE.DISC_GRATIS:
                    _Sconto.iStatusSconto &= 0x7FFFFF00;
                    _Sconto.iStatusSconto = SetBit(_Sconto.iStatusSconto, BIT_SCONTO_GRATIS);

                    _Sconto.sScontoText[(int)DISC_TYPE.DISC_GRATIS] = sScontoTextParam;
                    break;

                default:
                    _resetSconto();
                    break;
            }

            if (bCheckService(Define._AUTO_SEQ_TEST))
            {
                SF_Data.iStatusSconto = _Sconto.iStatusSconto;

                // SF_Data.iScontoStdReceipt = _Sconto.iStatusSconto non serve
                // dato che viene ricalcolato a partire dalla % dentro a SF_Data.iStatusSconto
                SF_Data.iScontoFissoReceipt = _Sconto.iScontoValFisso;

                SF_Data.sScontoReceipt = sScontoTextParam;
            }
        }

        private void ScontoDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();

            // evita la distruzione della form creando una eccezione alla successiva apertura
            e.Cancel = true;
        }
    }
}

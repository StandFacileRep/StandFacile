﻿/***************************************************
    NomeFile : StandFacile/EditArticoloDlg.cs
	Data	 : 26.07.2025
    Autore   : Mauro Artuso
 ***************************************************/
using System;
using System.Drawing;
using System.Windows.Forms;

using static StandFacile.glb;
using static StandFacile.Define;

using static StandCommonFiles.ComDef;
using static StandCommonFiles.CommonCl;
using static StandCommonFiles.LogServer;

namespace StandFacile
{
    /// <summary>
    /// classe per l'impostazione del singolo articolo
    /// </summary>
    public partial class EditArticoloDlg : Form
    {
        bool _bTipoEdit_OK, _bPrzEdit_OK;
        int _iPt, _iPrevTab;

        static bool _bListinoModificato;    // Flag per la gestione del salvataggio Listino

        /// <summary>riferimento ModificaArticoloDlg</summary>
        public static EditArticoloDlg rModificaArticoloDlg;

        const string NOTA_BUONI = "nota : \"Buoni\" verrà sottratto al totale senza andare in negativo !";
        const string NOTA_CONTATORI = "nota : \"Contatori\" ha il Prezzo = 0 quindi è gratuito !";

        TArticolo _Articolo, _Coperto;

        TErrMsg _WrnMsg = new TErrMsg();

        enum TAB_ITEM
        {
            ARTICOLO = 0,
            BUONI,
            COPERTI
        };

        readonly ToolTip _tt = new ToolTip
        {
            InitialDelay = 50,
            ShowAlways = true
        };

        /// <summary>ottiene flag di modifica listino necessaria</summary>
        public static bool GetListinoModificato() { return _bListinoModificato; }

        /// <summary>costruttore</summary>
        public EditArticoloDlg()
        {
            InitializeComponent();

            rModificaArticoloDlg = this;

            int i;
            string sTmp;

            _WrnMsg.iErrID = 0;

            _bPrzEdit_OK = true;
            _bTipoEdit_OK = true;
            _iPrevTab = 0;

            TipoEdit.MaxLength = iMAX_ART_CHAR; // 18

            //groupsText.ReadOnly = true;
            groupsText.Top = groupsCombo.Top;

            groupsCombo.Items.Clear();

            for (i = NUM_COPIES_GRPS - 1; (i >= 0); i--) // OK
            {
                sTmp = string.Format("{0}, {1}", sConstGruppi[i], SF_Data.sCopiesGroupsText[i]);

                groupsCombo.Items.Insert(0, sTmp);
            }

            _tt.SetToolTip(btnNavRight, "vai all'Articolo successivo");
            _tt.SetToolTip(btnNavLeft, "vai all'Articolo precedente");
            _tt.SetToolTip(btnElimina, "elimina il contenuto dell'Articolo");
            _tt.SetToolTip(btnSalva, "salva il contenuto dell'Articolo");
        }

        /// <summary>
        /// iPt: indice dell'array fondamentale,<br/>
        /// da invocare quando non si conosce il gruppo
        /// </summary>
        public void Init(int iPt)
        {
            String sTmp;

            Text = String.Format("Imposta Articolo: {0,3}", iPt);

            // *** copia struct Articolo ***
            _Articolo = SF_Data.Articolo[iPt];
            _Coperto = SF_Data.Articolo[MAX_NUM_ARTICOLI - 1];
            _iPt = iPt;

            switch (SF_Data.Articolo[iPt].iGruppoStampa)
            {
                case (int)DEST_TYPE.DEST_BUONI: // qui non si arriva con DEST_COUNTER

                    tabEditArticolo.SelectedIndex = (int)TAB_ITEM.BUONI;

                    TipoEdit.Text = _Articolo.sTipo;
                    groupsCombo.SelectedIndex = (int)DEST_TYPE.DEST_BUONI;

                    labelNota.Text = NOTA_BUONI;

                    groupsText.Text = sConstGruppi[(int)DEST_TYPE.DEST_BUONI];
                    groupsText.Visible = true;
                    groupsCombo.Visible = false;

                    if (_Articolo.iPrezzoUnitario > 0)
                        PrzEdit.Text = IntToEuro(_Articolo.iPrezzoUnitario);
                    else
                        PrzEdit.Text = "";
                    break;

                default: // Articolo default

                    tabEditArticolo.SelectedIndex = (int)TAB_ITEM.ARTICOLO;

                    TipoEdit.Text = _Articolo.sTipo;
                    groupsCombo.SelectedIndex = _Articolo.iGruppoStampa;

                    labelNota.Text = NOTA_CONTATORI;

                    groupsText.Visible = false;
                    groupsCombo.Visible = true;

                    if (_Articolo.iPrezzoUnitario > 0)
                        PrzEdit.Text = IntToEuro(_Articolo.iPrezzoUnitario);
                    else
                        PrzEdit.Text = "";
                    break;
            }

            OkBtn.Enabled = true;

            _bListinoModificato = false;

            sTmp = String.Format("EditArticoloDlg Init, articolo {0}", _iPt);
            LogToFile(sTmp);

            if (!Visible)
                ShowDialog();
        }

        /// <summary>
        /// iPt: indice dell'array fondamentale,<br/>
        /// da invocare quando si conosce il gruppo
        /// </summary>
        public void Init(int iPt, int tabIndexParam)
        {
            String sTmp;

            Text = String.Format("Imposta Articolo: {0,3}", iPt);

            // *** copia struct Articolo ***
            _Articolo = SF_Data.Articolo[iPt];
            _Coperto = SF_Data.Articolo[MAX_NUM_ARTICOLI - 1];

            switch (tabIndexParam)
            {
                case (int)TAB_ITEM.COPERTI:

                    TipoEdit.Text = _COPERTO;
                    groupsCombo.SelectedIndex = (int)DEST_TYPE.DEST_COUNTER;

                    groupsText.Text = sConstGruppi[(int)DEST_TYPE.DEST_COUNTER];
                    groupsText.Visible = true;
                    groupsCombo.Visible = false;

                    if (_Coperto.iPrezzoUnitario > 0)
                        PrzEdit.Text = IntToEuro(_Coperto.iPrezzoUnitario);
                    else
                        PrzEdit.Text = "";
                    break;

                case (int)TAB_ITEM.BUONI:
                    _iPt = iPt;

                    TipoEdit.Text = _Articolo.sTipo;
                    groupsCombo.SelectedIndex = (int)DEST_TYPE.DEST_BUONI;

                    groupsText.Text = sConstGruppi[(int)DEST_TYPE.DEST_BUONI];
                    groupsText.Visible = true;
                    groupsCombo.Visible = false;

                    if (_Articolo.iPrezzoUnitario > 0)
                        PrzEdit.Text = IntToEuro(_Articolo.iPrezzoUnitario);
                    else
                        PrzEdit.Text = "";
                    break;

                default: // Articolo default
                    _iPt = iPt;

                    TipoEdit.Text = _Articolo.sTipo;
                    groupsCombo.SelectedIndex = _Articolo.iGruppoStampa;

                    groupsText.Visible = false;
                    groupsCombo.Visible = true;

                    if (_Articolo.iPrezzoUnitario > 0)
                        PrzEdit.Text = IntToEuro(_Articolo.iPrezzoUnitario);
                    else
                        PrzEdit.Text = "";
                    break;
            }

            OkBtn.Enabled = true;

            _bListinoModificato = false;

            sTmp = String.Format("EditArticoloDlg Init, articolo {0} {1}", _iPt, tabIndexParam);
            LogToFile(sTmp);

            if (!Visible)
                ShowDialog();
        }

        private void TabEditArt_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabEditArticolo.SelectedIndex == (int)TAB_ITEM.COPERTI)
            {
                btnNavLeft.Enabled = false;
                btnNavRight.Enabled = false;
                ckBoxSkipEmpty.Enabled = false;
                groupsCombo.Enabled = false;
                labelNota.Enabled = false;
                TipoEdit.ReadOnly = true;
                btnElimina.Text = "Azzera";
                labelNota.Text = "nota : \"Contatori\" ha il Prezzo = 0 quindi è gratuito !";

                Init(MAX_NUM_ARTICOLI - 1, tabEditArticolo.SelectedIndex);
            }
            else if (tabEditArticolo.SelectedIndex == (int)TAB_ITEM.BUONI)
            {
                btnNavLeft.Enabled = true;
                btnNavRight.Enabled = true;
                ckBoxSkipEmpty.Enabled = true;
                groupsCombo.Enabled = false;
                labelNota.Enabled = true;
                TipoEdit.ReadOnly = false;
                btnElimina.Text = "Elimina";
                labelNota.Text = NOTA_BUONI;
                Init(_iPt, tabEditArticolo.SelectedIndex);
            }
            else // Articolo
            {
                btnNavLeft.Enabled = true;
                btnNavRight.Enabled = true;
                ckBoxSkipEmpty.Enabled = true;
                groupsCombo.Enabled = true;
                labelNota.Enabled = true;
                TipoEdit.ReadOnly = false;
                btnElimina.Text = "Elimina";
                labelNota.Text = NOTA_CONTATORI;

                Init(_iPt, tabEditArticolo.SelectedIndex);
            }

            _iPrevTab = tabEditArticolo.SelectedIndex;

            PrzEdit_KeyUp(this, null);

            groupsCombo_SelectedIndexChanged(this, null);
        }

        private void groupsCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((tabEditArticolo.SelectedIndex == (int)TAB_ITEM.ARTICOLO) && (groupsCombo.SelectedIndex == (int)DEST_TYPE.DEST_COUNTER))
            {
                PrzEdit.ReadOnly = true;
                PrzEdit.Text = "0";
            }
            else
            {
                PrzEdit.ReadOnly = false;
            }
        }

        private void BtnElimina_Click(object sender, EventArgs e)
        {
            DialogResult dResult;

            PrzEdit.Text = PrzEdit.Text.Trim();
            TipoEdit.Text = TipoEdit.Text.Trim();

            if (!String.IsNullOrEmpty(PrzEdit.Text) || !String.IsNullOrEmpty(TipoEdit.Text)) // se è compilato per intero
                dResult = MessageBox.Show("Sei sicuro di voler cancellare la voce ?", "Attenzione !", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            else
                return;

            if (dResult == DialogResult.No)
                return;
            else  // azzeramento
            {
                if ((tabEditArticolo.SelectedIndex == (int)TAB_ITEM.ARTICOLO) || (tabEditArticolo.SelectedIndex == (int)TAB_ITEM.BUONI))
                {
                    SF_Data.Articolo[_iPt].sTipo = "";
                    SF_Data.Articolo[_iPt].iPrezzoUnitario = 0;
                    SF_Data.Articolo[_iPt].iGruppoStampa = 0;

                    Init(_iPt, tabEditArticolo.SelectedIndex);
                }
                else
                {
                    SF_Data.Articolo[MAX_NUM_ARTICOLI - 1].iPrezzoUnitario = 0;

                    //if (_iPt < MAX_NUM_ARTICOLI - 1)
                    //    SF_Data.Articolo[_iPt].iGruppoStampa = 0;

                    Init(MAX_NUM_ARTICOLI - 1, tabEditArticolo.SelectedIndex);
                }
            }

            _bListinoModificato = true;

            LogToFile("EditArticoloDlg EliminaBtnClick");
        }

        private void TipoEdit_TextChanged(object sender, EventArgs e)
        {
            int i;
            LblMaxChar.Text = "(max " + iMAX_ART_CHAR.ToString() + " caratteri)";

            // no Trim() altrimenti non si possono più gestire gli spazi tra stringhe
            // TipoEdit.Text = TipoEdit.Text.Trim();

            i = iMAX_ART_CHAR - TipoEdit.Text.Length;
            if (i < 0)
                i = 0;

            LbRimanenti.Text = "car. rimanenti = " + i.ToString();

            if (TipoEdit.Text.Length > 2)
            {
                TipoEdit.BackColor = SystemColors.Window;
                TipoEdit.ForeColor = SystemColors.WindowText;
                _bTipoEdit_OK = true;
            }
            else if (String.IsNullOrEmpty(TipoEdit.Text) && _bPrzEdit_OK)
            {
                TipoEdit.BackColor = SystemColors.Window;
                TipoEdit.ForeColor = SystemColors.WindowText;
                _bTipoEdit_OK = true;
            }
            else
            {
                TipoEdit.BackColor = Color.Red;
                TipoEdit.ForeColor = SystemColors.HighlightText;
                _bTipoEdit_OK = false;
            }

            Check_allItems();
        }

        // accetta solo numeri, backspace, punti e virgole
        private void PrzEdit_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || (e.KeyChar == '.') || (e.KeyChar == ',') || (e.KeyChar == '\b')))
                e.Handled = true;
        }

        private void PrzEdit_KeyUp(object sender, KeyEventArgs e)
        {
            PrzEdit.Text = PrzEdit.Text.Trim();

            _Articolo.iPrezzoUnitario = EuroToInt(PrzEdit.Text, EURO_CONVERSION.EUROCONV_DONT_CARE, _WrnMsg);

            if ((_Articolo.iPrezzoUnitario == -1) || ( // Errore di formato del Prezzo in Euro !
                 ((tabEditArticolo.SelectedIndex == (int)TAB_ITEM.ARTICOLO) && (_Articolo.iPrezzoUnitario == 0) && !OptionsDlg._rOptionsDlg.GetZeroPriceEnabled() ||
                  (tabEditArticolo.SelectedIndex == (int)TAB_ITEM.BUONI) && (_Articolo.iPrezzoUnitario == 0)) &&
                  (groupsCombo.SelectedIndex != (int)DEST_TYPE.DEST_COUNTER)
               ))
            {
                PrzEdit.BackColor = Color.Red;
                PrzEdit.ForeColor = SystemColors.HighlightText;
                _bPrzEdit_OK = false;
            }
            else
            {
                PrzEdit.BackColor = SystemColors.Window;
                PrzEdit.ForeColor = SystemColors.WindowText;
                _bPrzEdit_OK = true;
            }

            Check_allItems();
        }

        /// <summary>click freccia sx</summary>
        private void BtnNavLeft_Click(object sender, EventArgs e)
        {
            if ((_iPt > 0) && CheckModifiche())
            {
                do
                {
                    _iPt--;
                }
                while (ckBoxSkipEmpty.Checked && String.IsNullOrEmpty(SF_Data.Articolo[_iPt].sTipo) && (_iPt > 0));

                Init(_iPt);
            }
        }

        /// <summary>click freccia dx</summary>
        private void BtnNavRight_Click(object sender, EventArgs e)
        {
            if ((_iPt < MAX_NUM_ARTICOLI - 2) && CheckModifiche())
            {
                do
                {
                    _iPt++;
                }
                while (ckBoxSkipEmpty.Checked && String.IsNullOrEmpty(SF_Data.Articolo[_iPt].sTipo) && (_iPt < MAX_NUM_ARTICOLI - 2));

                Init(_iPt);
            }
        }

        private void tabEditArticolo_Selecting(object sender, TabControlCancelEventArgs e)
        {
            // l'esito negativo evita il cambio TAB
            if (!CheckModifiche(sender))
                e.Cancel = true;
        }

        /// <summary>verifica se ci sono modifiche pendenti prima del cambio Articolo</summary>
        private bool CheckModifiche(object sender = null)
        {
            DialogResult dResult = DialogResult.None;

            bool bPricesEqual;
            int iPrzEdit;

            int iDebug1, iGruppoStampa;
            string sArticolo, sEditText;

            sEditText = TipoEdit.Text.ToUpper().Trim();

#pragma warning disable IDE0059
            iDebug1 = groupsCombo.SelectedIndex;
            iGruppoStampa = SF_Data.Articolo[_iPt].iGruppoStampa;

            if ((sender != null) && (sender == tabEditArticolo) && (_iPrevTab == (int)TAB_ITEM.COPERTI))
            {
                iPrzEdit = EuroToInt(PrzEdit.Text, EURO_CONVERSION.EUROCONV_DONT_CARE, _WrnMsg);

                sArticolo = SF_Data.Articolo[MAX_NUM_ARTICOLI - 1].sTipo.ToUpper().Trim();
                bPricesEqual = (iPrzEdit == SF_Data.Articolo[MAX_NUM_ARTICOLI - 1].iPrezzoUnitario);

                if (!bPricesEqual || (sArticolo != sEditText))
                    dResult = MessageBox.Show("Sei sicuro di abbandonare le modifiche fatte ?", "Attenzione !", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }
            else
            {
                if (iGruppoStampa == (int)DEST_TYPE.DEST_COUNTER)
                    iPrzEdit = 0;
                else
                    iPrzEdit = EuroToInt(PrzEdit.Text, EURO_CONVERSION.EUROCONV_DONT_CARE, _WrnMsg);

                sArticolo = SF_Data.Articolo[_iPt].sTipo.ToUpper().Trim();
                bPricesEqual = (String.IsNullOrEmpty(PrzEdit.Text) && (SF_Data.Articolo[_iPt].iPrezzoUnitario == 0)) || (iPrzEdit == SF_Data.Articolo[_iPt].iPrezzoUnitario);

                if (!bPricesEqual || (sArticolo != sEditText) || (groupsCombo.SelectedIndex != SF_Data.Articolo[_iPt].iGruppoStampa))
                    dResult = MessageBox.Show("Sei sicuro di abbandonare le modifiche fatte ?", "Attenzione !", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }

            if (dResult == DialogResult.No)
                return false;
            else
                return true;
        }

        /// <summary>special processing for KEY_UP, KEY_DOWN, KEY_LEFT, KEY_RIGHT</summary>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if ((btnNavRight.Focused || btnNavLeft.Focused) && ((keyData == Keys.Up) || (keyData == Keys.Down) || (keyData == Keys.Left) || (keyData == Keys.Right)))
            {
                switch (keyData)
                {
                    case Keys.Down:
                    case Keys.Left:
                        if (_iPt > 0)
                        {
                            do
                            {
                                _iPt--;
                            }
                            while (ckBoxSkipEmpty.Checked && String.IsNullOrEmpty(SF_Data.Articolo[_iPt].sTipo) && (_iPt > 0));
                        }
                        break;

                    case Keys.Up:
                    case Keys.Right:
                        if (_iPt < MAX_NUM_ARTICOLI - 2)
                        {
                            do
                            {
                                _iPt++;
                            }
                            while (ckBoxSkipEmpty.Checked && String.IsNullOrEmpty(SF_Data.Articolo[_iPt].sTipo) && (_iPt < MAX_NUM_ARTICOLI - 2));
                        }
                        break;

                    default:
                        break;
                } // END switch

                Init(_iPt, tabEditArticolo.SelectedIndex);
                return true;
            }
            else
            {
                return base.ProcessCmdKey(ref msg, keyData);
            }
        }

        private void EditArticoloDlg_KeyDown(object sender, KeyEventArgs e)
        {
            int iKey;

            CheckModifiche();

            if (e == null)
                iKey = (int)KEY_NONE;
            else
                iKey = (int)e.KeyValue;

            switch (iKey)
            {
                case KEY_HOME:
                    _iPt = 0;
                    break;
                case KEY_END:
                    _iPt = MAX_NUM_ARTICOLI - 2;
                    break;

                case KEY_MINUS:
                case KEY_MIN_NUM:
                    if (_iPt > 0)
                    {
                        _iPt--;
                    }
                    break;
                case KEY_PLUS:
                case KEY_PLUS_NUM:
                    if (_iPt < MAX_NUM_ARTICOLI - 2)
                    {
                        _iPt++;
                    }
                    break;
                case KEY_PAGEDOWN:
                    if (_iPt > 9)
                    {
                        _iPt -= 10;
                    }
                    break;
                case KEY_PAGEUP:
                    if (_iPt < MAX_NUM_ARTICOLI - 11)
                    {
                        _iPt += 10;
                    }
                    break;

                case KEY_ESC:
                    Close();
                    break;

                default:
                    break;
            } // END switch

            Init(_iPt);
        }

        private void AnnullaBtn_Click(object sender, EventArgs e)
        {
            if (CheckModifiche())
                Close();
        }

        private void BtnSalva_Click(object sender, EventArgs e)
        {
            String[] sQueue_Object = new String[2];

            SalvaArticolo();

            // avvia il refresh della griglia principale
            sQueue_Object[0] = MAIN_GRID_UPDATE_EVENT;
            sQueue_Object[1] = "";

            FrmMain.EventEnqueue(sQueue_Object);
        }

        /// <summary>
        /// funzione di verifica della correttezza dei dati <br/>
        /// immessi, modifica direttamente l'array globale
        /// </summary>
        private bool SalvaArticolo()
        {
            int i, iPrz = 0;
            String sTmp, sInStr;

            try
            {
                PrzEdit.Text = PrzEdit.Text.Trim();
                TipoEdit.Text = TipoEdit.Text.Trim();
                sInStr = TipoEdit.Text;

                // prezzo e tipoArticolo entrambi nulli devono essere processati normalmente

                if (sInStr.Contains("#") || sInStr.Contains(";")) //  || sInStr.Contains("\"")
                {
                    WarningManager(WRN_TKP);  // Token presenti
                    iPrz = -1;
                }
                // stringa troppo lunga tipoArticolo
                else if (TipoEdit.Text.Length > iMAX_ART_CHAR)
                {
                    iPrz = -1;

                    _WrnMsg.iRiga = -1;
                    _WrnMsg.iErrID = WRN_STL;
                    _WrnMsg.sMsg = TipoEdit.Text;
                    WarningManager(_WrnMsg);
                }
                // tipoArticolo e prezzo (salvo GetZeroPriceEnabled()) entrambi presenti
                else if (!String.IsNullOrEmpty(TipoEdit.Text) &&
                    ((!String.IsNullOrEmpty(PrzEdit.Text) || OptionsDlg._rOptionsDlg.GetZeroPriceEnabled()) || (groupsCombo.SelectedIndex == (int)DEST_TYPE.DEST_COUNTER))
                    )
                {
                    // conversione
                    if (groupsCombo.SelectedIndex == (int)DEST_TYPE.DEST_COUNTER)
                    {
                        if ((tabEditArticolo.SelectedIndex == (int)TAB_ITEM.ARTICOLO) || (tabEditArticolo.SelectedIndex == (int)TAB_ITEM.BUONI))
                        {
                            iPrz = 0;
                            PrzEdit.Text = "";
                        }
                        else // TAB CONTATORI
                            iPrz = EuroToInt(PrzEdit.Text, EURO_CONVERSION.EUROCONV_Z_WARN, _WrnMsg);
                    }
                    else
                    {
                        if (OptionsDlg._rOptionsDlg.GetZeroPriceEnabled())
                        {
                            iPrz = EuroToInt(PrzEdit.Text, EURO_CONVERSION.EUROCONV_DONT_CARE, _WrnMsg);

                            if (iPrz == 0)
                                PrzEdit.Text = "0,00";
                        }
                        else
                            iPrz = EuroToInt(PrzEdit.Text, EURO_CONVERSION.EUROCONV_WARN, _WrnMsg);

                    }

                    if ((tabEditArticolo.SelectedIndex == (int)TAB_ITEM.ARTICOLO) || (tabEditArticolo.SelectedIndex == (int)TAB_ITEM.BUONI))
                    {
                        // ricerca duplicati
                        for (i = 0; i < MAX_NUM_ARTICOLI; i++)
                            if ((i != _iPt) && (SF_Data.Articolo[i].sTipo.Trim().ToUpper() == TipoEdit.Text.Trim().ToUpper()))
                            {
                                iPrz = -1;
                                WarningManager(WRN_TVD);
                            }
                    }
                }
                // una sola voce presente o prezzo o tipoArticolo che non sia un contatore
                else if (!String.IsNullOrEmpty(TipoEdit.Text) ||
                        (!String.IsNullOrEmpty(PrzEdit.Text) && (groupsCombo.SelectedIndex != (int)DEST_TYPE.DEST_COUNTER) && !OptionsDlg._rOptionsDlg.GetZeroPriceEnabled()))
                {
                    if ((tabEditArticolo.SelectedIndex == (int)TAB_ITEM.ARTICOLO) || (tabEditArticolo.SelectedIndex == (int)TAB_ITEM.BUONI))
                    {
                        iPrz = -1;
                        WarningManager(WRN_TPV);
                    }
                }
                // else
                // nessuna voce presente : si prosegue per l'uscita regolare con valori nulli

            }

            catch (Exception)
            {
                WarningManager(WRN_ECE);
                iPrz = -1;
            }

            // se non ci sono errori uscita con chiusura Form
            if (iPrz != -1)
            {
                if ((tabEditArticolo.SelectedIndex == (int)TAB_ITEM.ARTICOLO) || (tabEditArticolo.SelectedIndex == (int)TAB_ITEM.BUONI))
                {
                    SF_Data.Articolo[_iPt].sTipo = TipoEdit.Text;
                    SF_Data.Articolo[_iPt].iPrezzoUnitario = iPrz;
                    SF_Data.Articolo[_iPt].iGruppoStampa = groupsCombo.SelectedIndex;

                    sTmp = String.Format("EditArticoloDlg BtnSalva_Click, Articolo {0}, iPrezzoUnitario {1}, iGruppoStampa {2}, tabEdit.SelIndex {3}",
                        SF_Data.Articolo[_iPt].sTipo, iPrz, groupsCombo.SelectedIndex, tabEditArticolo.SelectedIndex);
                }
                else
                {
                    // SF_Data.Articolo[_iPt].sTipo = TipoEdit.Text;                        // non cambia
                    SF_Data.Articolo[MAX_NUM_ARTICOLI - 1].iPrezzoUnitario = iPrz;          // cambia
                                                                                            // SF_Data.Articolo[_iPt].iGruppoStampa = groupsCombo.SelectedIndex;    // non cambia
                    sTmp = String.Format("EditArticoloDlg BtnSalva_Click, Articolo {0}, iPrezzoUnitario {1}, iGruppoStampa {2}",
                        SF_Data.Articolo[MAX_NUM_ARTICOLI - 1].sTipo, iPrz, groupsCombo.SelectedIndex);
                }

                _bListinoModificato = true;

                LogToFile(sTmp);

                return true;
            }
            else
            {
                return false;
            }
        }

        void Check_allItems()
        {
            OkBtn.Enabled = _bTipoEdit_OK && _bPrzEdit_OK;
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            if (SalvaArticolo())
                Close();
        }

    }
}

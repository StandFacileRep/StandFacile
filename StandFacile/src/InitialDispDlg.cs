/***************************************************
	NomeFile : StandFacile\InitialDispDlg.cs
	Data	 : 10.07.2025
	Autore   : Mauro Artuso
 ***************************************************/

using System;
using System.Windows.Forms;

using static StandCommonFiles.ComDef;
using static StandCommonFiles.CommonCl;
using static StandCommonFiles.LogServer;

using static StandFacile.Define;
using static StandFacile.dBaseIntf;

namespace StandFacile
{
    /// <summary>
    /// classe per il mantenimento della disponibilità:
    /// carica la disponibilità precedente in DB_Data[] 
    /// </summary>
    public partial class InitialDispDlg : Form
    {
#pragma warning disable IDE0044

        // flag di conferma
        static bool _bApplicaDisponibilita, _bCheckSingle;

        DateTime _date;

        static CheckBox[] _pCheckBoxCopia = new CheckBox[NUM_COPIES_GRPS];

        /// <summary>ritorna se è richiesto l'uso del dialogo InitDispDlg</summary>
        public static bool GetApplicaDisponibilita() { return _bApplicaDisponibilita; }

        /// <summary>
        /// costruttore con parametri: dateParam, bShow<br/>
        /// carica la disponibilità precedente in DB_DataArticolo[] 
        /// </summary>
        public InitialDispDlg(DateTime dateParam, bool bShow)
        {
            int iDispGroups;

            InitializeComponent();

            _date = dateParam;

            _bApplicaDisponibilita = false;
            _bCheckSingle = false;

            _pCheckBoxCopia[0] = checkBoxCopia_0;
            _pCheckBoxCopia[1] = checkBoxCopia_1;
            _pCheckBoxCopia[2] = checkBoxCopia_2;
            _pCheckBoxCopia[3] = checkBoxCopia_3;
            _pCheckBoxCopia[4] = checkBoxCopia_4;
            _pCheckBoxCopia[5] = checkBoxCopia_5;
            _pCheckBoxCopia[6] = checkBoxCopia_6;
            _pCheckBoxCopia[7] = checkBoxCopia_7;
            _pCheckBoxCopia[8] = checkBoxCopia_8;
            _pCheckBoxCopia[9] = checkBoxCopia_9;
            _pCheckBoxCopia[10] = checkBoxCopia_10;
            _pCheckBoxCopia[11] = checkBoxCopia_11;

            iDispGroups = ReadRegistry(DISP_GROUP_KEY, 0);

            for (int h = 0; h < NUM_COPIES_GRPS; h++)
            {
                if (IsBitSet(iDispGroups, h))
                    _pCheckBoxCopia[h].Checked = true;
            }

            InitFormatStrings(true);

            if (bShow)
                ShowDialog();
            else
                BtnOK_Click(this, null);
        }

        private void RadioBtnAll_CheckedChanged(object sender, EventArgs e)
        {
            if (_bCheckSingle) return;

            for (int i = 0; i < NUM_COPIES_GRPS; i++)
                _pCheckBoxCopia[i].Checked = true;
        }

        private void RadioBtnNone_CheckedChanged(object sender, EventArgs e)
        {
            if (_bCheckSingle) return;

            for (int i = 0; i < NUM_COPIES_GRPS; i++)
                _pCheckBoxCopia[i].Checked = false;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult dResult;

            dResult = MessageBox.Show("Sei sicuro di voler uscire ed utilizzare\n\nla piena disponibilità di magazzino ?", "Attenzione !",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dResult == DialogResult.Yes)
                Close();

            LogToFile("InitDispDlg : btnCancel");
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            #pragma warning disable IDE0059

            int i, iGrp, iDebug, iDispGroups;

            iDispGroups = 0;

            for (int h = 0; h < NUM_COPIES_GRPS; h++)
            {
                if (_pCheckBoxCopia[h].Checked)
                    iDispGroups += (1 << h);
            }

            WriteRegistry(DISP_GROUP_KEY, iDispGroups);

            _rdBaseIntf.dbCaricaDisponibilità(_date);

            for (i = 0; i < MAX_NUM_ARTICOLI; i++)
            {
                if (String.IsNullOrEmpty(DB_Data.Articolo[i].sTipo))
                    continue;
                else
                {
                    iGrp = DB_Data.Articolo[i].iGruppoStampa;

                    if (!_pCheckBoxCopia[iGrp].Checked)
                        DB_Data.Articolo[i].iDisponibilita = DISP_OK;
                    else if (DB_Data.Articolo[i].iDisponibilita != DISP_OK) // debug
                        iDebug = DB_Data.Articolo[i].iDisponibilita;        // articoli di cui si mantiene la disponibilità precedente
                }
            }

            _bApplicaDisponibilita = true;

            LogToFile("InitDispDlg : btnOK");
            Close();
        }


        private void CheckBoxCopia_Click(object sender, EventArgs e)
        {
            _bCheckSingle = true;

            for (int h = 0; h < NUM_COPIES_GRPS; h++)
            {
                if (_pCheckBoxCopia[h].Checked == false)
                {
                    rdButtonNone.Enabled = false;
                    rdButtonAll.Enabled = false;

                    rdButtonNone.Checked = false;
                    rdButtonAll.Checked = false;

                    rdButtonNone.Enabled = true;
                    rdButtonAll.Enabled = true;
                    break;
                }
            }

            _bCheckSingle = false;
        }

    }
}

/********************************************************************
  	NomeFile : StandCommonSrc/SelDataDlg.cs
    Data	 : 31.12.2025
  	Autore   : Mauro Artuso
 ********************************************************************/

using System;
using System.Windows.Forms;
using System.Collections.Generic;

using static StandCommonFiles.CommonCl;
using static StandCommonFiles.ComDef;
using static StandCommonFiles.LogServer;
using static StandFacile.dBaseIntf;

using static StandFacile.glb;

namespace StandFacile
{
    /// <summary>
    /// Questo dialogo consente di selezionare dal calendario la data
    /// che sarà poi usata per definire i nomi di vari files.
    /// </summary>
    public partial class SelDataDlg : Form
    {
        /// <summary>riferimento a SelDataDlg</summary>
        public static SelDataDlg rSelDataDlg;

        static SelectionRange _selDatesFromCalendar;
        static DialogResult result = DialogResult.None;

        int _lastClickTick;

        /// <summary>costruttore</summary>
        public SelDataDlg()
        {
            InitializeComponent();
            rSelDataDlg = this;

            SetTodayDate();
        }

        /// <summary>imposta il warning di avvio calendario e l'aspetto</summary>
        public void SetWarningAndLook(bool bLongWarn)
        {
            if (bLongWarn)
            {
                textBox.Text = "Selezionare la data di interesse: ai fini dell'esportazione Excel/ODS è anche possibile selezionare un intervallo di giorni cliccando sull'inizio desiderato, e poi muovendosi con le frecce tenendo premuto il tasto Shift";
                textBox.Height = 74;
                textBox.BorderStyle = BorderStyle.Fixed3D;
            }
            else
            {
                textBox.Text = "                         " + "Selezionare la data di interesse:";
                textBox.Height = 16;
                textBox.BorderStyle = BorderStyle.None;
            }

            this.Height = mCalendar.Height + textBox.Height + 146;
        }


        /// <summary>inizializza il calendario evidenziando le date che contengono dati</summary>
        public void SetTodayDate()
        {
            int i, iDB_StringsCount = 0;
            String sCell, sPrefix;

            List<string> sElencoStrings = new List<string>();
            List<DateTime> list = new List<DateTime>();

            _selDatesFromCalendar = new SelectionRange(GetActualDate(), GetActualDate());

            mCalendar.TodayDate = GetActualDate();

            sElencoStrings.Clear();

            if (_rdBaseIntf != null) // per StandMonitor
                iDB_StringsCount = _rdBaseIntf.dbElencoTabelle(sElencoStrings);

            if (SF_Data.bPrevendita)
                sPrefix = _dbPreDataTablePrefix;
            else
                sPrefix = _dbDataTablePrefix;

            for (i = 0; i < iDB_StringsCount; i++)
            {
                sCell = sElencoStrings[i];

                if (sCell.Contains(NOME_STATUS_DBTBL) || sCell.Contains(NOME_NSC_DBTBL) ||
                    sCell.Contains(NOME_DISP_DBTBL) || sCell.Contains(NOME_NMSG_DBTBL) ||
                    sCell.Contains("_unionetmp1") || sCell.Contains("_unionetmp2") ||
                    sCell.Contains("images") || sCell.Contains("indexes") ||
                    sCell.Contains("status") || sCell.Contains("num_orders") ||
                    sCell.Contains("orders_csec") || sCell.Contains("num_messages") ||
                    sCell.Contains("price_list") || sCell.Contains("testsequence") ||
                    !sCell.Contains(RELEASE_DB_TBLS) ||
                    String.IsNullOrEmpty(sCell))
                    continue;

                if (sCell.Contains(sPrefix))
                {
                    // aggiunge date alla lista
                    int iPos, iAnno, iMese, iGiorno;
                    string sTmp;

                    // protezione quando _dbDataTablePrefix è contenuta in _dbPreDataTablePrefix
                    if (sCell.Contains(_dbPreDataTablePrefix) && !SF_Data.bPrevendita)
                        continue;

                    iPos = sPrefix.Length + 4;

                    sTmp = sCell.Substring(iPos, 2);
                    iAnno = 2000 + Convert.ToInt32(sTmp);

                    sTmp = sCell.Substring(iPos + 2, 2);
                    iMese = Convert.ToInt32(sTmp);

                    sTmp = sCell.Substring(iPos + 4, 2);
                    iGiorno = Convert.ToInt32(sTmp);

                    DateTime tmpDate = new DateTime(iAnno, iMese, iGiorno);

                    list.Add(tmpDate);
                }
            }

            mCalendar.BoldedDates = list.ToArray();
        }

        /// <summary>ritorna 0 se non si esce con l'Ok</summary>
        public SelectionRange GetDateFromPicker()
        {
            String sDateTmp1, sDateTmp2;

            if (result == DialogResult.OK)
            {
                sDateTmp1 = _selDatesFromCalendar.Start.ToString("dd/MM/yy");
                sDateTmp2 = _selDatesFromCalendar.End.ToString("dd/MM/yy");
                LogToFile(String.Format("SelDataDlg, scelta la data : {0} {1}", sDateTmp1, sDateTmp2));

                return _selDatesFromCalendar;
            }
            else
            {
                LogToFile("SelDataDlg : esce");
                return null;
            }
        }

        /// <summary>click su Ok</summary>
        private void OKBtn_Click(object sender, EventArgs e)
        {
            _selDatesFromCalendar = mCalendar.SelectionRange;
            result = DialogResult.OK;
        }

        /// <summary>esce premendo ESC</summary>
        private void SelDataDlg_KeyPress(object sender, KeyPressEventArgs e)
        {
            int iKey = (int)e.KeyChar;

            if (iKey == KEY_ESC)
                result = DialogResult.Cancel;
        }

        // aggiunge la gestione dell'evento DoubleClick
        private void MCalendar_MouseDown(object sender, MouseEventArgs e)
        {
#pragma warning disable IDE1005

            int tick = Environment.TickCount;

            if (tick - _lastClickTick <= SystemInformation.DoubleClickTime - 80)
            {
                EventHandler handler = MCalendar_DoubleClick;
                if (handler != null) handler(this, EventArgs.Empty);
            }
            else
            {
                base.OnMouseDown(e);

                _lastClickTick = tick;
            }
        }

        // evento DoubleClick
        private void MCalendar_DoubleClick(object sender, EventArgs a)
        {
            OKBtn_Click(mCalendar, null);
            Close();
        }

    }
}

/********************************************************************
  	NomeFile : StandCommonSrc/EsploraDB_Dlg.cs
    Data	 : 06.12.2024
  	Autore   : Mauro Artuso

 Classe di esplorazione del database, ha senso chiamarla solo se
 il database è selezionato
 ********************************************************************/

using System;
using System.Collections.Generic;
using System.Windows.Forms;

using static StandCommonFiles.ComDef;
using static StandCommonFiles.CommonCl;

using static StandFacile.FrmMain;
using static StandFacile.dBaseIntf;

namespace StandFacile
{
    /// <summary>
    /// classe per l'esplorazione del database <br/>
    /// il bottone RENAME viene nascosto e non usato
    /// </summary>
    public partial class EsploraDB_Dlg : Form
    {

#pragma warning disable IDE0044
#pragma warning disable IDE1006

        int _iDB_StringsCount, _iGridStringsCount, _iDBGridRowIndex;
        List<string> _sElencoStrings = new List<string>();

        /// <summary>
        /// costruttore EsploraDB_Dlg
        /// </summary>
        public EsploraDB_Dlg()
        {
            InitializeComponent();

            dbGrid.Focus();
            _iDBGridRowIndex = 0;
            RenameBtn.Enabled = false;
        }

        private void EsploraDB_Dlg_Shown(object sender, EventArgs e)
        {
            int i;
            String sCell;

            _sElencoStrings.Clear();
            _iDB_StringsCount = _rdBaseIntf.dbElencoTabelle(_sElencoStrings);

            dbGrid.ColumnCount = 1;
            dbGrid.RowCount = 1;

            _iGridStringsCount = 0;

            for (i = 0; i < _iDB_StringsCount; i++)
            {
                sCell = _sElencoStrings[i];

                if (sCell.Contains(NOME_STATO_DBTBL) || sCell.Contains(NOME_NSC_DBTBL) ||
                    sCell.Contains(NOME_DISP_DBTBL) || sCell.Contains(NOME_NMSG_DBTBL) ||
                    sCell.Contains(NOME_LISTINO_DBTBL) || sCell.Contains(NOME_TEST_DBTBL) ||
                    sCell.Contains("images") || sCell.Contains("indexes") ||
                    sCell.Contains("_unionetmp1") || sCell.Contains("_unionetmp2") ||
                    sCell.Contains(NOME_WEBORD_DBTBL) || sCell.Contains(NOME_NSC_DBTBL) ||
                    !sCell.Contains(RELEASE_TBL) || String.IsNullOrEmpty(sCell))
                    continue;

                dbGrid.RowCount = _iGridStringsCount + 1;

                dbGrid.Rows[_iGridStringsCount].Cells[0].Value = sCell;
                dbGrid.Rows[_iGridStringsCount].Height = 26;

                _iGridStringsCount++;
            }

            if ((_iDBGridRowIndex > 0) && (_iDBGridRowIndex < dbGrid.Rows.Count))
                dbGrid.CurrentCell = dbGrid.Rows[_iDBGridRowIndex].Cells[0];
            else if ((_iDBGridRowIndex > 0) && (_iDBGridRowIndex == dbGrid.Rows.Count))
                dbGrid.CurrentCell = dbGrid.Rows[_iDBGridRowIndex - 1].Cells[0]; // cancellata l'ultima riga

            // inizializza l'aspetto dei pulsanti
            dbGrid_KeyDown(this, new KeyEventArgs(Keys.None));
        }

        private void dbGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            String sTabellaTmp;

            if (_iGridStringsCount > 0)
            {
                sTabellaTmp = dbGrid.CurrentRow.Cells[0].Value.ToString();
                DateTime tmpDate = new DateTime(2024, 01, 01);

                if (sTabellaTmp.Contains(_dbOrdersTablePrefix))
                {

                    // il parametro di data qui serve sempre per facilitare l'accesso alle tabelle dei Dati
                    VisOrdiniDlg rVisOrdiniDlg = new VisOrdiniDlg(tmpDate, VisOrdiniDlg.MAX_NUM_TICKET, sTabellaTmp);

                    rVisOrdiniDlg.Dispose();
                }
                else
                {
                    VisDatiDlg rVisDatiDlg = new VisDatiDlg();

                    // il parametro di cassa viene ignorato essendo presente sTabellaTmp
                    rVisDatiDlg.VisualizzaDati((int)FILE_TO_SHOW.FILE_AUTO, tmpDate, 0, false, sTabellaTmp);
                    rVisDatiDlg.Dispose();
                }
            }
        }

        private void EliminaBtn_Click(object sender, EventArgs e)
        {
            String sTmp;
            DialogResult dResult;

            dResult = MessageBox.Show("Sei sicuro di voler cancellare la tabella ?", "Attenzione !", MessageBoxButtons.YesNo);

            _iDBGridRowIndex = 0;

            if (dResult == DialogResult.Yes)
            {
                _iDBGridRowIndex = dbGrid.CurrentRow.Index;
                sTmp = dbGrid.CurrentRow.Cells[0].Value.ToString();

                if (String.IsNullOrEmpty(sTmp))
                    return;

#if STANDFACILE // sicurezza
                _rdBaseIntf.dbDropTable(sTmp);
#endif
                EsploraDB_Dlg_Shown(this, null);

            }
        }

        private void RenameBtn_Click(object sender, EventArgs e)
        {

#if STANDFACILE // sicurezza
            String sOldTabella, sNewTabella;
            DialogResult dResult = DialogResult.None;

            sOldTabella = dbGrid.CurrentRow.Cells[0].Value.ToString();

            if (String.IsNullOrEmpty(sOldTabella))
                return;

            // apre form per postfisso
            RenameDlg rRenameDlg = new RenameDlg();

            rRenameDlg.Init("Inserisci il nuovo nome :", sOldTabella);
            dResult = rRenameDlg.ShowDialog();

            if (dResult == DialogResult.OK)
            {
                sNewTabella = rRenameDlg.GetEdit();

                _rdBaseIntf.dbRenameTable(sOldTabella, sNewTabella);

                EsploraDB_Dlg_Shown(this, null);
            }
#endif
        }

        /*************************************************
	        evita di cancellare i files in uso,
            chiamata dopo gli spostamenti con le frecce
         *************************************************/
        private void dbGrid_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            dbGrid_KeyDown(this, new KeyEventArgs(Keys.None));
        }

        /*************************************************
            inizializza l'aspetto dei pulsanti
         *************************************************/
        private void dbGrid_KeyDown(object sender, KeyEventArgs e)
        {
#if STANDFACILE // sicurezza

            int iKey = (int)e.KeyValue;
            String sRow = "", sDate;

            sDate = GetActualDate().ToString("yyMMdd");

            if ((_iDB_StringsCount > 0) && (dbGrid.CurrentRow != null) && (dbGrid.CurrentRow.Cells[0].Value != null))
                sRow = dbGrid.CurrentRow.Cells[0].Value.ToString();

            // sicurezza
            if (rFrmMain.GetEsperto() && !sRow.Contains(sDate) && (_iDB_StringsCount > 0) &&
                !DataManager.CheckIf_CassaSec_and_NDB() && !sRow.Contains(NOME_LISTINO_DBTBL))
            {
                EliminaBtn.Enabled = true;
                //RenameBtn.Enabled = true;

                switch (iKey)
                {
                    case KEY_DEL:
                        EliminaBtn_Click(this, null);
                        break;
                    case KEY_F2:
                        RenameBtn_Click(this, null);
                        break;
                    case KEY_ESC:
                        Close();
                        break;
                    default:
                        break;
                } // END switch

            }
            else
#endif
            {
                EliminaBtn.Enabled = false;
                RenameBtn.Enabled = false;
            }
        }

    }
}

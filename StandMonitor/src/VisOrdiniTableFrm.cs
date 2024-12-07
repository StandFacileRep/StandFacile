/***************************************************
  NomeFile : StandMonitor/VisOrdiniTableFrm.cs
  Data	   : 01.05.2024
  Autore   : Mauro Artuso
 ***************************************************/

using System;
using System.Windows.Forms;

using static StandFacile.dBaseIntf;

using static StandCommonFiles.ComDef;
using static StandCommonFiles.LogServer;

namespace StandFacile
{
    /// <summary> classe di visualizzazione degli ordini emessi </summary>
    public partial class VisOrdiniTableFrm : Form
    {
        /// <summary>riferimento a VisOrdiniFrm</summary>
        public static VisOrdiniTableFrm rVisOrdiniFrm;

        bool bPrimaVolta;

        /// <summary>costruttore</summary>
        public VisOrdiniTableFrm()
        {
            InitializeComponent();

            rVisOrdiniFrm = this;
            Width = 800;

            bPrimaVolta = true;
            LogToFile("FrmVisOrdini : costruttore avvio");
        }
        /// <summary> aggiorna la tabella ordini </summary>
        public void Aggiorna()
        {
            try
            {
                _rdBaseIntf.dbCheckStatus();

                if (_rdBaseIntf.bGetDB_Connected())
                {
                    LogToFile("FrmVisOrdini : Aggiorna Connected");

                    if (VisOrdiniTableFrm.rVisOrdiniFrm.Visible)
                    {
                        OrdiniGrid.DataSource = _rdBaseIntf.dbOrdiniMonitorList(checkBox_nonevasi.Checked, true).Tables[0];

                        FormResize(this, null);
                    }
                    else
                        _rdBaseIntf.dbOrdiniMonitorList(checkBox_nonevasi.Checked, false);
                }
                else
                {
                    LogToFile("FrmVisOrdini : Aggiorna not Connected");
                }
            }

            catch (Exception)
            {
                LogToFile("FrmVisOrdini dbException: Aggiorna2");
            }
        }

        private void FormResize(object sender, EventArgs e)
        {
            int iRowsHeight;

            float fWidth;
            float fFontHeaderHeight, fFontHeight;

            // Posizione griglia
            OrdiniGrid.Height = this.Height - 100;
            //OrdiniGrid.Width = this.Width - 50;

            if (OrdiniGrid.ColumnCount > 0) // altrimenti genera eccezione
            {
                fWidth = OrdiniGrid.Width;
                OrdiniGrid.Columns[0].Width = (int)(fWidth * 0.10f);
                OrdiniGrid.Columns[1].Width = (int)(fWidth * 0.34f);
                OrdiniGrid.Columns[2].Width = (int)(fWidth * 0.10f);
                OrdiniGrid.Columns[3].Width = (int)(fWidth * 0.10f);
                OrdiniGrid.Columns[4].Width = (int)(fWidth * 0.16f);
                OrdiniGrid.Columns[5].Width = (int)(fWidth * 0.10f);
                OrdiniGrid.Columns[6].Width = (int)(fWidth * 0.10f);
            }


            if ((OrdiniGrid.ColumnCount > 0) && (OrdiniGrid.Width > 0))// altrimenti genera eccezione
            {
                // imposta Font sulla base della larghezza della finestra
                fFontHeight = ((float)OrdiniGrid.Width) / 50;
                OrdiniGrid.Font = new System.Drawing.Font(OrdiniGrid.DefaultCellStyle.Font.Name, fFontHeight);

                fFontHeaderHeight = ((float)OrdiniGrid.Width) / 80;

                OrdiniGrid.Columns[0].HeaderCell.Style.Font = new System.Drawing.Font(OrdiniGrid.DefaultCellStyle.Font.Name, fFontHeaderHeight);
                OrdiniGrid.Columns[1].HeaderCell.Style.Font = OrdiniGrid.Columns[0].HeaderCell.Style.Font;
                OrdiniGrid.Columns[2].HeaderCell.Style.Font = OrdiniGrid.Columns[0].HeaderCell.Style.Font;
                OrdiniGrid.Columns[3].HeaderCell.Style.Font = OrdiniGrid.Columns[0].HeaderCell.Style.Font;
                OrdiniGrid.Columns[4].HeaderCell.Style.Font = OrdiniGrid.Columns[0].HeaderCell.Style.Font;
                OrdiniGrid.Columns[5].HeaderCell.Style.Font = OrdiniGrid.Columns[0].HeaderCell.Style.Font;
                OrdiniGrid.Columns[6].HeaderCell.Style.Font = OrdiniGrid.Columns[0].HeaderCell.Style.Font;

                if (bPrimaVolta)
                {
                    OrdiniGrid.Columns[0].HeaderText = "Num. Sc";
                    OrdiniGrid.Columns[1].HeaderText = "Articolo";
                    OrdiniGrid.Columns[2].HeaderText = "Quantità";
                    OrdiniGrid.Columns[3].HeaderText = "Gruppo";
                    OrdiniGrid.Columns[4].HeaderText = "Evaso";
                    OrdiniGrid.Columns[5].HeaderText = "Cassa";
                    OrdiniGrid.Columns[6].HeaderText = "Stato";

                    OrdiniGrid.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

                    bPrimaVolta = false;
                }
            }

            if (OrdiniGrid.Height > 0)
            {
                iRowsHeight = OrdiniGrid.Height / 12;
                OrdiniGrid.RowTemplate.Height = iRowsHeight;
                OrdiniGrid.ColumnHeadersHeight = (int)(iRowsHeight * 0.8f);

                OrdiniGrid.AutoResizeRows();
            }
        }

        private void checkBox_nonevasi_Click(object sender, EventArgs e)
        {
            Aggiorna();
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void VisOrdiniFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // cambia il comportamento del bottone di Close altrimenti distrugge l'oggetto 
            // invece di nasconderlo soltanto

            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }
    }
}
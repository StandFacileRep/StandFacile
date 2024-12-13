/**********************************************
  NomeFile : StandMonitor/AuxMonitor.cs
  Data	   : 23.06.2023
  Autore   : Mauro Artuso
 **********************************************/
using System;
using System.Windows.Forms;

using static StandCommonFiles.CommonCl;

namespace StandFacile
{
    /// <summary>
    /// classe finestra per secondo monitor
    /// </summary>
    public partial class AuxForm : Form
    {
        /// <summary>riferimento a AuxForm</summary>
        public static AuxForm rAuxForm;

        bool bPrimaVolta = true;

        /// <summary>costruttore</summary>
        public AuxForm(int iMon)
        {
            InitializeComponent();

                rAuxForm?.Close();

            rAuxForm = this;
            DBGrid.MultiSelect = false;

            Screen[] screens = Screen.AllScreens;
            Screen thisScreen = Screen.FromControl(FrmMain.rFrmMain);

            if (iMon == 2)
            {
                for (int screenNumber = 0; screenNumber < screens.Length; screenNumber++)
                {
                    if (screens[screenNumber].DeviceName != thisScreen.DeviceName)
                    {
                        rAuxForm.Location = screens[screenNumber].WorkingArea.Location;

                        rAuxForm.WindowState = FormWindowState.Maximized;
                        break;
                    }
                }
            }
            else
            {
                // solo se c'è il terzo monitor
                if (screens.Length > 2)
                {
                    rAuxForm.Location = screens[screens.Length - 1].WorkingArea.Location;
                    rAuxForm.WindowState = FormWindowState.Maximized;
                }
            }
        }

        /// <summary>
        /// funzione di aggiornamento dei dati
        /// </summary>
        public void AuxRefresh()
        {
            String sTime, sData, sTmpStr;

            LabelNumScontrino.Text = FrmMain.rFrmMain.GetNumScontrinoLabel();

            sData = GetActualDate().ToString("ddd  dd/MM/yy");
            sTmpStr = sData.ToUpper();
            sTmpStr = sTmpStr.Substring(0, 1);

            sData = sData.Substring(1);
            sData = sTmpStr + sData;

            sTime = DateTime.Now.ToString("HH:mm");

            LabelClock.Text = sData + " " + sTime;

            DBGrid.DataSource = FrmMain.rFrmMain.DS.Tables[1];

            // il sort deve avvenire solo alla fine della fusione delle tabelle
            DBGrid.Sort(DBGrid.Columns[1], System.ComponentModel.ListSortDirection.Descending);

            AuxForm_Resize(this, null);
        }

        private void AuxForm_Resize(object sender, EventArgs e)
        {
            int iRowsHeight;
            float fWidth;
            float fFontHeaderHeight, fFontHeight;

            // altrimenti da errore
            if (DBGrid.ColumnCount == 0)
                return;

            if (bPrimaVolta)
            {
                bPrimaVolta = false;

                DBGrid.Columns[0].HeaderText = "Articolo";
                DBGrid.Columns[1].HeaderText = "Q.tà Venduta";
                DBGrid.Columns[2].HeaderText = "da consegnare";
                DBGrid.Columns[3].HeaderText = "Disponibilità";
                DBGrid.Columns[4].HeaderText = "Gruppo";

                DBGrid.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                DBGrid.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                DBGrid.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                DBGrid.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                DBGrid.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            DBGrid.Width = this.Width - 40;

            // imposta altezza sulla base della altezza della finestra
            if (WindowState != FormWindowState.Minimized)
            {
                DBGrid.Height = this.Height - LabelClock.Height - LabelNumScontrino.Height - 80;

                iRowsHeight = DBGrid.Height / 12;
                DBGrid.RowTemplate.Height = iRowsHeight;
                DBGrid.ColumnHeadersHeight = (int)(iRowsHeight * 0.8f);
            }

            fWidth = DBGrid.Width;

            DBGrid.Columns[0].Width = (int)(fWidth * 0.48f);
            DBGrid.Columns[1].Width = (int)(fWidth * 0.18f);
            DBGrid.Columns[2].Width = (int)(fWidth * 0.18f);
            DBGrid.Columns[3].Width = (int)(fWidth * 0.14f);
            DBGrid.Columns[4].Width = (int)(fWidth * 0.12f);

            // imposta Font sulla base della larghezza della finestra
            fFontHeight = ((float)DBGrid.Width) / 50;
            DBGrid.Font = new System.Drawing.Font(DBGrid.DefaultCellStyle.Font.Name, fFontHeight);

            fFontHeaderHeight = ((float)DBGrid.Width) / 80;
            DBGrid.Columns[0].HeaderCell.Style.Font = new System.Drawing.Font(DBGrid.DefaultCellStyle.Font.Name, fFontHeaderHeight);
            DBGrid.Columns[1].HeaderCell.Style.Font = DBGrid.Columns[0].HeaderCell.Style.Font;
            DBGrid.Columns[2].HeaderCell.Style.Font = DBGrid.Columns[0].HeaderCell.Style.Font;
            DBGrid.Columns[3].HeaderCell.Style.Font = DBGrid.Columns[0].HeaderCell.Style.Font;
            DBGrid.Columns[4].HeaderCell.Style.Font = DBGrid.Columns[0].HeaderCell.Style.Font;

            if (DBGrid.Rows.Count > 0)
            {
                DBGrid.Rows[0].Selected = true;
                DBGrid.FirstDisplayedScrollingRowIndex = 0;
            }

            DBGrid.AutoResizeRows();
        }

        private void AuxForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
    }
}

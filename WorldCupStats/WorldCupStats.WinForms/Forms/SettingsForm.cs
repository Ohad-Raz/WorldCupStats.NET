using System;
using System.Windows.Forms;
using WorldCupStats.Data.Models;

namespace WorldCupStats.WinForms.Forms
{
    public partial class SettingsForm : Form
    {
        public AppSettings SelectedSettings { get; set; } = null!;

        // Settings dialog. Combos fill in Load.
        public SettingsForm()
        {
            InitializeComponent();
        }

        // Empty hook for championship combo changes.
        private void CbChampionship_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        // Stores choices from the two combos when user hits Confirm.
        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            if (CbChampionship.SelectedItem is null || CbDataSource.SelectedItem is null)
            {
                return;
            }

            SelectedSettings = new AppSettings
            {
                Championship = (ChampionshipType)CbChampionship.SelectedItem,
                DataSource = (DataSourceMode)CbDataSource.SelectedItem
            };
        }

        // Fills combos and maps Enter and Esc to the buttons.
        private void SettingsForm_Load(object sender, EventArgs e)
        {
            CbChampionship.DataSource = Enum.GetValues(typeof(ChampionshipType));
            CbDataSource.DataSource = Enum.GetValues(typeof(DataSourceMode));

            BtnConfirm.DialogResult = DialogResult.OK;
            BtnCancel.DialogResult = DialogResult.Cancel;

            AcceptButton = BtnConfirm;
            CancelButton = BtnCancel;
        }
    }
}

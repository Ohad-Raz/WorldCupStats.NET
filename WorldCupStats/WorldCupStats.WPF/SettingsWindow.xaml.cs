using System.Windows;
using WorldCupStats.Data.Models;

/// <summary>
/// Interaction logic for SettingsWindow.xaml
/// </summary>
namespace WorldCupStats.WPF
{
    public partial class SettingsWindow : Window
    {
        public AppSettings? SelectedSettings { get; private set; }

        public SettingsWindow(AppSettings? currentSettings = null)
        {
            InitializeComponent();

            LoadComboBoxes(currentSettings);
        }

        // Loads enum values into the settings ComboBoxes.
        private void LoadComboBoxes(AppSettings? currentSettings)
        {
            cmbChampionship.ItemsSource = Enum.GetValues(typeof(ChampionshipType));
            cmbDataSource.ItemsSource = Enum.GetValues(typeof(DataSourceMode));

            if (currentSettings != null)
            {
                cmbChampionship.SelectedItem = currentSettings.Championship;
                cmbDataSource.SelectedItem = currentSettings.DataSource;
            }
            else
            {
                cmbChampionship.SelectedItem = ChampionshipType.Men;
                cmbDataSource.SelectedItem = DataSourceMode.Json;
            }
        }

        // Saves selected settings and closes the window.
        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (cmbChampionship.SelectedItem is not ChampionshipType championship ||
                cmbDataSource.SelectedItem is not DataSourceMode dataSource)
            {
                MessageBox.Show("Please choose all settings.");
                return;
            }

            SelectedSettings = new AppSettings
            {
                Championship = championship,
                DataSource = dataSource
            };

            DialogResult = true;
            Close();
        }

        // Closes the window without saving settings.
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
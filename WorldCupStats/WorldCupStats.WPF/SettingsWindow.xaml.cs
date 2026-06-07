using System.Windows;
using WorldCupStats.Data.Models;

namespace WorldCupStats.WPF
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public AppSettings? SelectedSettings { get; private set; }

        public SettingsWindow(AppSettings? currentSettings = null)
        {
            InitializeComponent();

            LoadComboBoxes(currentSettings);
        }

        // Loads enum values and display options into the settings ComboBoxes.
        private void LoadComboBoxes(AppSettings? currentSettings)
        {
            // 1. Load all ComboBox options first.
            cmbChampionship.ItemsSource = Enum.GetValues(typeof(ChampionshipType));
            cmbDataSource.ItemsSource = Enum.GetValues(typeof(DataSourceMode));

            cmbWindowMode.ItemsSource = new List<string>
            {
                "Windowed",
                "Fullscreen"
            };

            cmbResolution.ItemsSource = new List<string>
            {
                "800x450",
                "1024x768",
                "1280x720"
            };

            // 2. Select current settings or default values.
            if (currentSettings != null)
            {
                cmbChampionship.SelectedItem = currentSettings.Championship;
                cmbDataSource.SelectedItem = currentSettings.DataSource;
                cmbWindowMode.SelectedItem = currentSettings.IsFullScreen ? "Fullscreen" : "Windowed";
                cmbResolution.SelectedItem = currentSettings.Resolution;
            }
            else
            {
                cmbChampionship.SelectedItem = ChampionshipType.Men;
                cmbDataSource.SelectedItem = DataSourceMode.Json;
                cmbWindowMode.SelectedItem = "Windowed";
                cmbResolution.SelectedItem = "800x450";
            }
        }

        // Saves selected settings and closes the window.
        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            string? selectedWindowMode = cmbWindowMode.SelectedItem as string;
            string? selectedResolution = cmbResolution.SelectedItem as string;
            if (cmbChampionship.SelectedItem is not ChampionshipType championship ||
                cmbDataSource.SelectedItem is not DataSourceMode dataSource|| selectedWindowMode is null || selectedResolution is null)
            {
                MessageBox.Show("Please choose all settings.");
                return;
            }

            SelectedSettings = new AppSettings
            {
                Championship = championship,
                DataSource = dataSource,
                IsFullScreen = selectedWindowMode == "Fullscreen",
                Resolution = selectedResolution
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
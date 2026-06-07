using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WorldCupStats.Data.Models;
using WorldCupStats.Data.Services;

namespace WorldCupStats.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private readonly SettingsService _settingsService = new SettingsService();
        private readonly FavoriteTeamService _favoriteTeamService = new FavoriteTeamService();
        private readonly WorldCupDataService _worldCupDataService = new WorldCupDataService();

        private AppSettings? _settings;
        private List<Team> _teams = new List<Team>();
        private List<Match> _favoriteTeamMatches = new List<Match>();
        public MainWindow()
        {
            InitializeComponent();
        }
       // Loads settings, teams, favorite team, and opponents when the WPF window opens.
private async void Window_Loaded(object sender, RoutedEventArgs e)
{
    try
    {
        // 1. Load shared settings from the Data layer.
        _settings = _settingsService.Load();

        // 2. If settings are missing, open WPF settings window.
        if (_settings is null)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.Owner = this;

            if (settingsWindow.ShowDialog() != true || settingsWindow.SelectedSettings is null)
            {
                Close();
                return;
            }

            _settings = settingsWindow.SelectedSettings;
            _settingsService.Save(_settings);
        }

        // 3. Load teams, restore favorite team, and load opponents.
        await ReloadTeamsAsync();
    }
    catch (Exception ex)
    {
        MessageBox.Show($"An error occurred: {ex.Message}");
    }
}
        // Opens the settings window and reloads WPF data after changes.
        private async void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow(_settings);
            settingsWindow.Owner = this;

            if (settingsWindow.ShowDialog() != true || settingsWindow.SelectedSettings is null)
            {
                return;
            }

            _settings = settingsWindow.SelectedSettings;
            _settingsService.Save(_settings);

            try
            {
                await ReloadTeamsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }
        // Reloads teams, restores favorite team if possible, and reloads opponents.
        private async Task ReloadTeamsAsync()
        {
            if (_settings is null)
            {
                return;
            }

            // 1. Load teams.
            _teams = await _worldCupDataService.GetTeamsAsync(_settings);

            // 2. Bind favorite team ComboBox.
            cmbFavoriteTeam.ItemsSource = null;
            cmbFavoriteTeam.ItemsSource = _teams;
            cmbFavoriteTeam.DisplayMemberPath = nameof(Team.DisplayName);

            // 3. Try restoring saved favorite team.
            string? favoriteTeamCode = _favoriteTeamService.LoadFavoriteTeam();
            Team? favoriteTeam = null;

            if (!string.IsNullOrWhiteSpace(favoriteTeamCode))
            {
                favoriteTeam = _teams.FirstOrDefault(team => team.FifaCode == favoriteTeamCode);
            }

            cmbFavoriteTeam.SelectedItem = favoriteTeam;

            if (favoriteTeam != null)
            {
                await LoadMatchesAndOpponentsAsync(favoriteTeam);
            }
            else
            {
                cmbOpponentTeam.ItemsSource = null;
                lblMatchResult.Text = "Select favorite team and opponent to show result";
            }
        }
        // Loads matches for the selected favorite team and fills the opponent ComboBox.
        private async Task LoadMatchesAndOpponentsAsync(Team selectedTeam)
        {
            if (_settings is null)
            {
                return;
            }

            // 1. Load matches for the selected team.
            _favoriteTeamMatches = await _worldCupDataService.GetMatchesByFifaCodeAsync(
                _settings,
                selectedTeam.FifaCode);

            // 2. Build opponent list from those matches.
            List<Team> opponents = new List<Team>();

            foreach (Match match in _favoriteTeamMatches)
            {
                Team? opponent = null;

                if (match.HomeTeam?.Code == selectedTeam.FifaCode)
                {
                    opponent = _teams.FirstOrDefault(team => team.FifaCode == match.AwayTeam?.Code);
                }
                else if (match.AwayTeam?.Code == selectedTeam.FifaCode)
                {
                    opponent = _teams.FirstOrDefault(team => team.FifaCode == match.HomeTeam?.Code);
                }

                if (opponent != null && !opponents.Any(team => team.FifaCode == opponent.FifaCode))
                {
                    opponents.Add(opponent);
                }
            }

            // 3. Bind opponents to the opponent ComboBox.
            cmbOpponentTeam.ItemsSource = opponents;
            cmbOpponentTeam.DisplayMemberPath = nameof(Team.DisplayName);

            if (opponents.Count > 0)
            {
                cmbOpponentTeam.SelectedIndex = 0;
            }
        }
        // Reloads matches and opponents when the favorite team changes in WPF.
        private async void cmbFavoriteTeam_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Team? selectedTeam = cmbFavoriteTeam.SelectedItem as Team;

            if (selectedTeam is null)
            {
                return;
            }

            _favoriteTeamService.SaveFavoriteTeam(selectedTeam.FifaCode);

            try
            {
                await LoadMatchesAndOpponentsAsync(selectedTeam);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        // Shows the selected match result when the opponent changes.
        private void cmbOpponentTeam_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // 1. Get selected favorite team and opponent.
            Team? favoriteTeam = cmbFavoriteTeam.SelectedItem as Team;
            Team? opponentTeam = cmbOpponentTeam.SelectedItem as Team;

            if (favoriteTeam is null || opponentTeam is null)
            {
                lblMatchResult.Text = "Select opponent to show result";
                return;
            }

            // 2. Find the match between these two teams.
            Match? selectedMatch = null;

            foreach (Match match in _favoriteTeamMatches)
            {
                bool favoriteHomeOpponentAway =
                    match.HomeTeam?.Code == favoriteTeam.FifaCode &&
                    match.AwayTeam?.Code == opponentTeam.FifaCode;

                bool opponentHomeFavoriteAway =
                    match.HomeTeam?.Code == opponentTeam.FifaCode &&
                    match.AwayTeam?.Code == favoriteTeam.FifaCode;

                if (favoriteHomeOpponentAway || opponentHomeFavoriteAway)
                {
                    selectedMatch = match;
                    break;
                }
            }

            if (selectedMatch is null)
            {
                lblMatchResult.Text = "Match result not found";
                return;
            }

            // 3. Show result in home-away order.
            lblMatchResult.Text =
                $"{selectedMatch.HomeTeam?.Country} {selectedMatch.HomeTeam?.Goals} : " +
                $"{selectedMatch.AwayTeam?.Goals} {selectedMatch.AwayTeam?.Country}";
        }
    }
}
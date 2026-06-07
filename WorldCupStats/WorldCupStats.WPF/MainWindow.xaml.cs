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
        private readonly PlayerImageService _playerImageService = new PlayerImageService();
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
                    ApplyDisplaySettings();
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
                ApplyDisplaySettings();
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
            DrawStartingElevenOnPitch();
        }
        // Opens details for the selected favorite team.
        private void btnFavoriteTeamDetails_Click(object sender, RoutedEventArgs e)
        {
            Team? team = cmbFavoriteTeam.SelectedItem as Team;

            if (team is null)
            {
                MessageBox.Show("Please select a favorite team first.");
                return;
            }

            TeamDetailsWindow window = new TeamDetailsWindow(team);
            window.Owner = this;
            window.ShowDialog();
        }

        // Opens details for the selected opponent team.
        private void btnOpponentTeamDetails_Click(object sender, RoutedEventArgs e)
        {
            Team? team = cmbOpponentTeam.SelectedItem as Team;

            if (team is null)
            {
                MessageBox.Show("Please select an opponent first.");
                return;
            }

            TeamDetailsWindow window = new TeamDetailsWindow(team);
            window.Owner = this;
            window.ShowDialog();
        }
        // Finds the match between the selected favorite team and selected opponent.
        private Match? GetSelectedMatch()
        {
            Team? favoriteTeam = cmbFavoriteTeam.SelectedItem as Team;
            Team? opponentTeam = cmbOpponentTeam.SelectedItem as Team;

            if (favoriteTeam is null || opponentTeam is null)
            {
                return null;
            }

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
                    return match;
                }
            }

            return null;
        }
        // Gets the selected favorite team's statistics from the selected match.
        private TeamStatistics? GetFavoriteTeamStatistics(Match match)
        {
            Team? favoriteTeam = cmbFavoriteTeam.SelectedItem as Team;

            if (favoriteTeam is null)
            {
                return null;
            }

            if (match.HomeTeam?.Code == favoriteTeam.FifaCode)
            {
                return match.HomeTeamStatistics;
            }

            if (match.AwayTeam?.Code == favoriteTeam.FifaCode)
            {
                return match.AwayTeamStatistics;
            }

            return null;
        }
        // Draws the selected favorite team's starting eleven on the pitch.
        private void DrawStartingElevenOnPitch()
        {
            // 1. Clear old player labels.
            pitchCanvas.Children.Clear();

            // 2. Find selected match and team statistics.
            Match? match = GetSelectedMatch();
            if (match is null)
            {
                return;
            }

            TeamStatistics? statistics = GetFavoriteTeamStatistics(match);
            if (statistics is null || statistics.StartingEleven is null)
            {
                return;
            }

            // 3. Draw each player according to position.
            foreach (Player player in statistics.StartingEleven)
            {
                DrawPlayerOnPitch(player);
            }
        }
        // Draws one player on the pitch according to his position.
        private void DrawPlayerOnPitch(Player player)
        {
            double canvasWidth = pitchCanvas.ActualWidth;
            double canvasHeight = pitchCanvas.ActualHeight;

            if (canvasWidth == 0 || canvasHeight == 0)
            {
                canvasWidth = 700;
                canvasHeight = 220;
            }

            double x = 0;
            double y = 0;

            int samePositionIndex = CountPlayersAlreadyDrawnInPosition(player.Position);

            if (player.Position == "Goalie")
            {
                x = canvasWidth * 0.08;
                y = canvasHeight * 0.45;
            }
            else if (player.Position == "Defender")
            {
                x = canvasWidth * 0.28;
                y = 30 + samePositionIndex * 40;
            }
            else if (player.Position == "Midfield")
            {
                x = canvasWidth * 0.52;
                y = 30 + samePositionIndex * 40;
            }
            else
            {
                x = canvasWidth * 0.78;
                y = 45 + samePositionIndex * 45;
            }

            Border playerBox = new Border
            {
                Width = 140,
                Height = 38,
                Background = Brushes.White,
                BorderBrush = Brushes.DarkGreen,
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(6),
                Padding = new Thickness(4),
                Child = new TextBlock
                {
                    Text = $"{player.ShirtNumber} {player.Name}",
                    FontSize = 10,
                    FontWeight = FontWeights.Bold,
                    TextAlignment = TextAlignment.Center,
                    TextWrapping = TextWrapping.Wrap
                }
            };
            playerBox.Tag = player.Position;

            playerBox.Cursor = Cursors.Hand;
            playerBox.MouseLeftButtonUp += (sender, e) =>
            {
                OpenPlayerDetails(player);
            };

            Canvas.SetLeft(playerBox, x);
            Canvas.SetTop(playerBox, y);

            pitchCanvas.Children.Add(playerBox);
        }
        // Counts how many drawn player boxes already belong to the same position group.
        private int CountPlayersAlreadyDrawnInPosition(string position)
        {
            int count = 0;

            foreach (UIElement element in pitchCanvas.Children)
            {
                if (element is Border border && border.Tag is string existingPosition)
                {
                    if (existingPosition == position)
                    {
                        count++;
                    }
                }
            }

            return count;
        }
        // Gets the event list for the selected favorite team in the selected match.
        private List<MatchEvent> GetFavoriteTeamEvents(Match match)
        {
            Team? favoriteTeam = cmbFavoriteTeam.SelectedItem as Team;

            if (favoriteTeam is null)
            {
                return new List<MatchEvent>();
            }

            if (match.HomeTeam?.Code == favoriteTeam.FifaCode)
            {
                return match.HomeTeamEvents ?? new List<MatchEvent>();
            }

            if (match.AwayTeam?.Code == favoriteTeam.FifaCode)
            {
                return match.AwayTeamEvents ?? new List<MatchEvent>();
            }

            return new List<MatchEvent>();
        }

        // Counts one event type for one player in the selected match.
        private int CountPlayerEventsInMatch(Player player, List<MatchEvent> events, string eventNamePart)
        {
            int count = 0;

            foreach (MatchEvent matchEvent in events)
            {
                if (matchEvent.Player == player.Name &&
                    matchEvent.TypeOfEvent.Contains(eventNamePart))
                {
                    count++;
                }
            }

            return count;
        }
        // Opens the player details window for a player shown on the pitch.
        private void OpenPlayerDetails(Player player)
        {
            Match? match = GetSelectedMatch();
            if (match is null)
            {
                return;
            }

            Team? favoriteTeam = cmbFavoriteTeam.SelectedItem as Team;
            if (favoriteTeam is null)
            {
                return;
            }

            List<MatchEvent> events = GetFavoriteTeamEvents(match);

            int goals = CountPlayerEventsInMatch(player, events, "goal");
            int yellowCards = CountPlayerEventsInMatch(player, events, "yellow-card");

            string? imagePath = _playerImageService.GetPlayerImagePath(
                favoriteTeam.FifaCode,
                player);

            PlayerDetailsWindow window = new PlayerDetailsWindow(
                player,
                goals,
                yellowCards,
                imagePath);

            window.Owner = this;
            window.ShowDialog();
        }
        // Applies WPF window mode and resolution settings.
        private void ApplyDisplaySettings()
        {
            if (_settings is null)
            {
                return;
            }

            if (_settings.Resolution == "1024x768")
            {
                Width = 1024;
                Height = 768;
            }
            else if (_settings.Resolution == "1280x720")
            {
                Width = 1280;
                Height = 720;
            }
            else
            {
                Width = 800;
                Height = 450;
            }

            if (_settings.IsFullScreen)
            {
                WindowStyle = WindowStyle.None;
                WindowState = WindowState.Maximized;
            }
            else
            {
                WindowStyle = WindowStyle.SingleBorderWindow;
                WindowState = WindowState.Normal;
            }
        }
    }
}

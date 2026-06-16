using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WorldCupStats.Data.Models;
using WorldCupStats.Data.Services;

namespace WorldCupStats.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private enum PitchSide
        {
            Left,
            Right
        }

        private readonly SettingsService _settingsService = new SettingsService();
        private readonly FavoriteTeamService _favoriteTeamService = new FavoriteTeamService();
        private readonly WorldCupDataService _worldCupDataService = new WorldCupDataService();

        private AppSettings? _settings;
        private List<Team> _teams = new List<Team>();
        private List<Match> _favoriteTeamMatches = new List<Match>();
        private readonly PlayerImageService _playerImageService = new PlayerImageService();
        private bool _isLoading;
        public MainWindow()
        {
            InitializeComponent();
        }
        // Loads settings, teams, favorite team, and opponents when the WPF window opens
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // 1. Load saved settings
                _settings = _settingsService.Load();

                // 2. If settings are missing, open WPF settings window
                if (_settings is null)
                {
                    SettingsWindow settingsWindow = new SettingsWindow();
                    settingsWindow.Owner = this;

                    if (settingsWindow.ShowDialog() != true ||
                        settingsWindow.SelectedSettings is null)
                    {
                        Close();
                        return;
                    }

                    _settings = settingsWindow.SelectedSettings;
                    _settingsService.Save(_settings);
                }

                // 3. Apply display settings before loading application data
                ApplyDisplaySettings();

                // 4. Show loading while data loads
                ShowLoading();

                await ReloadTeamsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
            finally
            {
                HideLoading();
            }
        }
        // Opens the settings window and reloads WPF data after changes
        private async void btnSettings_Click(
            object sender,
            RoutedEventArgs e)
        {
            SettingsWindow settingsWindow =
                new SettingsWindow(_settings);

            settingsWindow.Owner = this;

            if (settingsWindow.ShowDialog() != true ||
                settingsWindow.SelectedSettings is null)
            {
                return;
            }

            _settings = settingsWindow.SelectedSettings;
            _settingsService.Save(_settings);

            try
            {
                ShowLoading();

                ApplyDisplaySettings();
                await ReloadTeamsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"An error occurred: {ex.Message}");
            }
            finally
            {
                HideLoading();
            }
        }
        // Reloads teams, restores favorite team if possible, and reloads opponents
        private async Task ReloadTeamsAsync()
        {
            if (_settings is null)
            {
                return;
            }

            // 1. Load teams
            _teams = await _worldCupDataService.GetTeamsAsync(_settings);

            // 2. Bind favorite team ComboBox
            cmbFavoriteTeam.ItemsSource = null;
            cmbFavoriteTeam.ItemsSource = _teams;
            cmbFavoriteTeam.DisplayMemberPath = nameof(Team.DisplayName);

            // 3. Try restoring saved favorite team
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
        // Loads matches for the selected favorite team and fills the opponent ComboBox
        private async Task LoadMatchesAndOpponentsAsync(Team selectedTeam)
        {
            if (_settings is null)
            {
                return;
            }

            // 1. Load matches for the selected team
            _favoriteTeamMatches = await _worldCupDataService.GetMatchesByFifaCodeAsync(
                _settings,
                selectedTeam.FifaCode);

            // 2. Build opponent list from those matches
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

            // 3. Bind opponents to the opponent ComboBox
            cmbOpponentTeam.ItemsSource = opponents;
            cmbOpponentTeam.DisplayMemberPath = nameof(Team.DisplayName);

            if (opponents.Count > 0)
            {
                cmbOpponentTeam.SelectedIndex = 0;
            }
        }
        // Reloads matches and opponents when favorite team changes
        private async void cmbFavoriteTeam_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            if (_isLoading)
            {
                return;
            }

            Team? selectedTeam =
                cmbFavoriteTeam.SelectedItem as Team;

            if (selectedTeam is null)
            {
                return;
            }

            _favoriteTeamService.SaveFavoriteTeam(
                selectedTeam.FifaCode);
            try
            {
                ShowLoading();

                await LoadMatchesAndOpponentsAsync(
                    selectedTeam);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"An error occurred: {ex.Message}");
            }
            finally
            {
                HideLoading();
            }
        }

        // Shows the selected match result when the opponent changes
        private void cmbOpponentTeam_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // 1. Get selected favorite team and opponent
            Team? favoriteTeam = cmbFavoriteTeam.SelectedItem as Team;
            Team? opponentTeam = cmbOpponentTeam.SelectedItem as Team;

            if (favoriteTeam is null || opponentTeam is null)
            {
                lblMatchResult.Text = "Select opponent to show result";
                return;
            }

            // 2. Find the match between these two teams
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

            // 3. Show result in home-away order
            lblMatchResult.Text =
                $"{selectedMatch.HomeTeam?.Country} {selectedMatch.HomeTeam?.Goals} : " +
                $"{selectedMatch.AwayTeam?.Goals} {selectedMatch.AwayTeam?.Country}";
            DrawStartingElevenOnPitch();
        }
        // Opens details for the selected favorite team
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

        // Opens details for the selected opponent team
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
        // Finds the match between the selected favorite team and selected opponent
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
        // Gets the match statistics for a specific team
        private TeamStatistics? GetTeamStatistics(
            Match match,
            string fifaCode)
        {
            if (match.HomeTeam?.Code == fifaCode)
            {
                return match.HomeTeamStatistics;
            }

            if (match.AwayTeam?.Code == fifaCode)
            {
                return match.AwayTeamStatistics;
            }

            return null;
        }
        // Draws both starting elevens on opposite halves of the pitch
        private void DrawStartingElevenOnPitch()
        {
            // 1. Clear previously drawn player controls
            pitchCanvas.Children.Clear();

            // 2. Get selected teams and match
            Team? favoriteTeam =
                cmbFavoriteTeam.SelectedItem as Team;

            Team? opponentTeam =
                cmbOpponentTeam.SelectedItem as Team;

            Match? match = GetSelectedMatch();

            if (favoriteTeam is null ||
                opponentTeam is null ||
                match is null)
            {
                return;
            }

            // 3. Get statistics for both teams
            TeamStatistics? favoriteStatistics =
                GetTeamStatistics(match, favoriteTeam.FifaCode);

            TeamStatistics? opponentStatistics =
                GetTeamStatistics(match, opponentTeam.FifaCode);

            // 4. Draw favorite team on the left half
            if (favoriteStatistics?.StartingEleven is not null)
            {
                foreach (Player player in favoriteStatistics.StartingEleven)
                {
                    DrawPlayerOnPitch(
                     player,
                     favoriteTeam.FifaCode,
                     PitchSide.Left,
                     favoriteStatistics.StartingEleven);
                }
            }

            // 5. Draw opponent team on the right half
            if (opponentStatistics?.StartingEleven is not null)
            {
                foreach (Player player in opponentStatistics.StartingEleven)
                {
                    DrawPlayerOnPitch(
                        player,
                        opponentTeam.FifaCode,
                        PitchSide.Right,
                        opponentStatistics.StartingEleven);
                }
            }
        }
        // Draws one player control on the selected side of the pitch
        private void DrawPlayerOnPitch(
            Player player,
            string fifaCode,
            PitchSide pitchSide,
            IEnumerable<Player> startingEleven)
        {
            double canvasWidth = pitchCanvas.ActualWidth;
            double canvasHeight = pitchCanvas.ActualHeight;

            if (canvasWidth == 0 || canvasHeight == 0)
            {
                canvasWidth = 900;
                canvasHeight = 500;
            }

            double x;
            double y;

            int samePositionIndex =
                CountPlayersAlreadyDrawnInPosition(
                    player.Position,
                    pitchSide);
            int samePositionCount = CountPlayersInPosition(startingEleven, player.Position);

            double controlHeight = 58;

            double centeredY =
                CalculateCenteredPlayerY(
                    canvasHeight,
                    samePositionIndex,
                    samePositionCount,
                    controlHeight);
            double leftGoalkeeperX = canvasWidth * 0.02;
            double leftDefenderX = canvasWidth * 0.13;
            double leftMidfieldX = canvasWidth * 0.25;
            double leftForwardX = canvasWidth * 0.38;

            double rightGoalkeeperX = canvasWidth * 0.82;
            double rightDefenderX = canvasWidth * 0.70;
            double rightMidfieldX = canvasWidth * 0.58;
            double rightForwardX = canvasWidth * 0.47;

            if (pitchSide == PitchSide.Left)
            {
                if (player.Position == "Goalie")
                {
                    x = leftGoalkeeperX;
                    y = (canvasHeight - controlHeight) / 2;
                }
                else if (player.Position == "Defender")
                {
                    x = leftDefenderX;
                    y = centeredY;
                }
                else if (player.Position == "Midfield")
                {
                    x = leftMidfieldX;
                    y = centeredY;
                }
                else
                {
                    x = leftForwardX;
                    y = centeredY;
                }
            }
            else
            {
                if (player.Position == "Goalie")
                {
                    x = rightGoalkeeperX;
                    y = (canvasHeight - controlHeight) / 2;
                }
                else if (player.Position == "Defender")
                {
                    x = rightDefenderX;
                    y = centeredY;
                }
                else if (player.Position == "Midfield")
                {
                    x = rightMidfieldX;
                    y = centeredY;
                }
                else
                {
                    x = rightForwardX;
                    y = centeredY;
                }

            }

            string imagePath =
                _playerImageService.GetPlayerImagePathOrDefault(
                    fifaCode,
                    player);

            PlayerPitchUserControl playerControl =
                new PlayerPitchUserControl();

            playerControl.SetPlayer(
                player,
                imagePath);

            playerControl.Tag = new PitchPlayerTag
            {
                Position = player.Position,
                PitchSide = pitchSide,
                FifaCode = fifaCode
            };

            playerControl.MouseLeftButtonUp +=
                PlayerPitchControl_MouseLeftButtonUp;

            Canvas.SetLeft(playerControl, x);
            Canvas.SetTop(playerControl, y);

            pitchCanvas.Children.Add(playerControl);
        }
        // Extra data stored on each pitch player control
        private class PitchPlayerTag
        {
            public string Position { get; set; } = string.Empty;

            public PitchSide PitchSide { get; set; }

            public string FifaCode { get; set; } = string.Empty;
        }
        // Opens player details when a player control on the pitch is clicked
        private void PlayerPitchControl_MouseLeftButtonUp(
            object? sender,
            MouseButtonEventArgs e)
        {
            PlayerPitchUserControl? playerControl =
                sender as PlayerPitchUserControl;

            if (playerControl?.BoundPlayer is null ||
                playerControl.Tag is not PitchPlayerTag pitchTag)
            {
                return;
            }

            OpenPlayerDetails(
                playerControl.BoundPlayer,
                pitchTag.FifaCode);
        }

        // Counts already drawn players with the same position on one pitch side
        private int CountPlayersAlreadyDrawnInPosition(
            string position,
            PitchSide pitchSide)
        {
            int count = 0;

            foreach (UIElement element in pitchCanvas.Children)
            {
                if (element is PlayerPitchUserControl playerControl &&
                    playerControl.Tag is PitchPlayerTag pitchTag &&
                    pitchTag.Position == position &&
                    pitchTag.PitchSide == pitchSide)
                {
                    count++;
                }
            }

            return count;
        }
        // Gets the event list for a specific team in the selected match
        private List<MatchEvent> GetTeamEvents(
            Match match,
            string fifaCode)
        {
            if (match.HomeTeam?.Code == fifaCode)
            {
                return match.HomeTeamEvents ?? new List<MatchEvent>();
            }

            if (match.AwayTeam?.Code == fifaCode)
            {
                return match.AwayTeamEvents ?? new List<MatchEvent>();
            }

            return new List<MatchEvent>();
        }

        // Counts one event type for one player in the selected match
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
        // Opens the details window for the selected player
        private void OpenPlayerDetails(
            Player player,
            string fifaCode)
        {
            Match? match = GetSelectedMatch();

            if (match is null)
            {
                return;
            }

            List<MatchEvent> events =
                GetTeamEvents(match, fifaCode);

            int goals =
                CountPlayerEventsInMatch(
                    player,
                    events,
                    "goal");

            int yellowCards =
                CountPlayerEventsInMatch(
                    player,
                    events,
                    "yellow-card");

            string imagePath =
                _playerImageService.GetPlayerImagePathOrDefault(
                    fifaCode,
                    player);

            PlayerDetailsWindow window =
                new PlayerDetailsWindow(
                    player,
                    goals,
                    yellowCards,
                    imagePath);

            window.Owner = this;
            window.ShowDialog();
        }
        // Applies WPF window mode and resolution settings
        private void ApplyDisplaySettings()
        {
            if (_settings is null)
            {
                return;
            }

            if (_settings.Resolution == "800x450")
            {
                Width = 800;
                Height = 450;
            }
            else if (_settings.Resolution == "1280x720")
            {
                Width = 1280;
                Height = 720;
            }
            else
            {
                Width = 1024;
                Height = 768;
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

                CenterWindow();
            }
        }

        // Places the window in the center of the usable screen area
        private void CenterWindow()
        {
            Rect workArea = SystemParameters.WorkArea;

            Left = workArea.Left + (workArea.Width - Width) / 2;
            Top = workArea.Top + (workArea.Height - Height) / 2;
        }
        // Redraws both starting elevens when the pitch changes size
        private void pitchCanvas_SizeChanged(object? sender, SizeChangedEventArgs e)
        {
            if (!IsLoaded)
            {
                return;
            }

            DrawStartingElevenOnPitch();
        }
        // Calculates a vertically centered position for one player group
        private double CalculateCenteredPlayerY(
            double canvasHeight,
            int playerIndex,
            int playerCount,
            double controlHeight)
        {
            if (playerCount <= 1)
            {
                return (canvasHeight - controlHeight) / 2;
            }

            double availableHeight = canvasHeight - controlHeight - 40;
            double spacing = availableHeight / (playerCount - 1);

            return 20 + playerIndex * spacing;
        }
        // Counts players with one position in a starting eleven
        private static int CountPlayersInPosition(
            IEnumerable<Player> players,
            string position)
        {
            return players.Count(player =>
                player.Position == position);
        }

        // Confirms whether the user wants to close the WPF application
        private void Window_Closing(
            object? sender,
            System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "Are you sure you want to exit?",
                "Exit confirmation",
                MessageBoxButton.OKCancel,
                MessageBoxImage.Question,
                MessageBoxResult.OK);

            if (result != MessageBoxResult.OK)
            {
                e.Cancel = true;
            }
        }
        // Shows loading and sets _isLoading so events do not fire twice
        private void ShowLoading()
        {
            _isLoading = true;
            loadingOverlay.Visibility = Visibility.Visible;
        }

        // Hides loading and clears _isLoading
        private void HideLoading()
        {
            loadingOverlay.Visibility = Visibility.Collapsed;
            _isLoading = false;
        }

        // Returns the application to windowed mode when Esc is pressed in fullscreen
        private void Window_KeyDown(
            object? sender,
            KeyEventArgs e)
        {
            if (e.Key != Key.Escape ||
                WindowStyle != WindowStyle.None)
            {
                return;
            }

            WindowStyle = WindowStyle.SingleBorderWindow;
            WindowState = WindowState.Normal;

            if (_settings is not null)
            {
                _settings.IsFullScreen = false;
                _settingsService.Save(_settings);
            }

            e.Handled = true;
        }
    }
}

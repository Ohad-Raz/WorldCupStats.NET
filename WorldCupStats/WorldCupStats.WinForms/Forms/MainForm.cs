using WorldCupStats.Data.Models;
using WorldCupStats.Data.Services;
using WorldCupStats.WinForms.Forms;

namespace WorldCupStats.WinForms
{
    public partial class MainForm : Form
    {
        // Later course work: drag and drop, multi-select moves, default player image, rankings, print.

        private readonly FavoriteTeamService _favoriteTeamService = new();
        private readonly WorldCupDataService _worldCupData = new();
        private readonly MatchPlayerService _matchPlayerService = new();
        private readonly FavoritePlayerService _favoritePlayerService = new();

        private AppSettings? _settings;
        private List<Team>? _teams;
        private List<Match>? _matches;
        private bool _isLoading;
        private readonly PlayerImageService _playerImageService = new();

        // Designer builds the form layout.
        public MainForm()
        {
            InitializeComponent();
        }

        // First launch can show settings then loads teams and fills the player panels.
        private async void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                // 1. Load settings or ask the user on first run
                SettingsService settingsService = new SettingsService();
                AppSettings? settings = settingsService.Load();
                if (settings == null)
                {
                    using SettingsForm formSettings = new SettingsForm();

                    if (formSettings.ShowDialog() != DialogResult.OK || formSettings.SelectedSettings is null)
                    {
                        Close();
                        return;
                    }

                    settings = formSettings.SelectedSettings;

                    settingsService.Save(settings);
                }

                _settings = settings;

                // 2. Load team list into the combo box
                _isLoading = true;
                ShowLoading();

                _teams = await _worldCupData.GetTeamsAsync(settings);
                cbFavoriteTeam.DataSource = _teams;
                cbFavoriteTeam.DisplayMember = nameof(Team.DisplayName);

                // 3. Restore last favorite team selection if we have a saved code
                string? favTeamCode = LoadFavoriteTeam();
                if (favTeamCode is not null)
                {
                    cbFavoriteTeam.SelectedItem = _teams.FirstOrDefault(t => t.FifaCode == favTeamCode);
                }

                // 4. Load all matches for that team (empty list when no code yet)
                if (!string.IsNullOrWhiteSpace(favTeamCode))
                {
                    _matches = await _worldCupData.GetMatchesByFifaCodeAsync(settings, favTeamCode);
                }
                else
                {
                    _matches = new List<Match>();
                }

                // 5. Fill the two player flow panels from the first match roster
                RebuildPlayerPanels();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
            finally
            {
                _isLoading = false;
                HideLoading();

            }
        }

        // Reads favorite FIFA code from disk. Null when none saved.
        private string? LoadFavoriteTeam()
        {
            return _favoriteTeamService.LoadFavoriteTeam();
        }

        // Saves the new favorite team code and reloads matches plus player tiles.
        private async void CbFavoriteTeam_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isLoading || _settings is null)
            {
                return;
            }

            Team? selectedTeam = (Team?)cbFavoriteTeam.SelectedItem;
            if (selectedTeam is null)
            {
                return;
            }

            // 1. Persist the new favorite team code
            _favoriteTeamService.SaveFavoriteTeam(selectedTeam.FifaCode);

            try
            {
                // 2. Show loading while matches are being fetched.
                ShowLoading();

                // 3. Fetch matches for this team then refresh the UI.
                _matches = await _worldCupData.GetMatchesByFifaCodeAsync(_settings, selectedTeam.FifaCode);
                RebuildPlayerPanels();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
            finally
            {
                // 4. Hide loading after success or error.
                HideLoading();
            }
        }

        // Removes and disposes every control inside one flow panel.
        private static void ClearFlowPanel(FlowLayoutPanel panel)
        {
            while (panel.Controls.Count > 0)
            {
                Control child = panel.Controls[0];
                panel.Controls.Remove(child);
                child.Dispose();
            }
        }
        // Prepares the two player panels to receive dragged player tiles.



        // Loads favorite ids and rebuilds favorited and other player tiles from the current roster.
        private void RebuildPlayerPanels()
        {
            // 1. Clear old tiles so we do not stack duplicates.
            ClearFlowPanel(flowFavoritePlayers);
            ClearFlowPanel(flowOtherPlayers);

            Team? team = (Team?)cbFavoriteTeam.SelectedItem;
            if (team is null || _matches is null)
            {
                return;
            }

            // 2. Build roster from the first match and load saved favorite ids for this team.
            List<Player> roster = _matchPlayerService.GetPlayersFromFirstMatch(_matches, team.FifaCode);
            List<string> favoriteIds = _favoritePlayerService.LoadFavoritePlayerIds(team.FifaCode);

            // 3. Create one tile per player and put it in the correct panel.
            foreach (Player player in roster)
            {
                bool isFavorite = _favoritePlayerService.IsFavoritePlayer(team.FifaCode, player, favoriteIds);
                PlayerUserControl tile = CreatePlayerTile(player, team.FifaCode, isFavorite);

                FlowLayoutPanel host = isFavorite ? flowFavoritePlayers : flowOtherPlayers;
                host.Controls.Add(tile);
            }
        }

        // Creates one PlayerUserControl tile and loads its saved image if it exists.
        private PlayerUserControl CreatePlayerTile(Player player, string fifaCode, bool isFavorite)
        {
            // 1. Create the tile and bind player text/star data.
            PlayerUserControl tile = new PlayerUserControl();
            tile.SetPlayer(player, isFavorite);

            // 2. Load saved player image if it exists.
            string? imagePath = _playerImageService.GetPlayerImagePath(fifaCode, player);
            if (imagePath != null)
            {
                tile.SetImage(imagePath);
            }

            // 3. Attach the context menu used for favorites and pictures.
            tile.ContextMenuStrip = contextMenuPlayers;
            // 4. Start drag/drop from the generated player tile.
            tile.MouseDown += PlayerTile_MouseDown;

            foreach (Control child in tile.Controls)
            {
                child.MouseDown += PlayerTile_MouseDown;
            }
            return tile;
        }

        // Finds which player tile the context menu was opened on, if any.
        private PlayerUserControl? GetSelectedPlayerControlFromContextMenu()
        {
            // 1. Read which control was under the pointer when the user right-clicked to open the menu.
            Control? source = contextMenuPlayers.SourceControl;

            // 2. If the click hit a child control inside the tile, walk Parent until we reach PlayerUserControl.
            while (source != null && source is not PlayerUserControl)
            {
                source = source.Parent;
            }
            // 3. Return the tile, or null if the user clicked empty panel space.
            PlayerUserControl? tile = source as PlayerUserControl;
            return tile;
        }

        // Returns true when the user is still allowed to add another favorite player (max three).
        private bool CanAddMoreFavoritePlayers()
        {
            // 1. Count how many PlayerUserControl tiles are in flowFavoritePlayers right now.
            int count = 0;

            foreach (Control control in flowFavoritePlayers.Controls)
            {
                if (control is PlayerUserControl)
                {
                    count++;
                }
            }
            // 2. Allow adding only while the count is below three.
            return count < 3;
        }

        // Collects the Player objects for every tile that currently sits in the favorite flow panel.
        private List<Player> GetCurrentFavoritePlayersFromPanel()
        {
            List<Player> favorites = new List<Player>();

            // 1. Loop through each control in the favorites panel.
            foreach (Control control in flowFavoritePlayers.Controls)
            {
                // 2. Use only PlayerUserControl tiles.
                if (control is PlayerUserControl tile)
                {
                    // 3. Add only tiles that actually have a Player bound.
                    if (tile.BoundPlayer != null)
                    {
                        favorites.Add(tile.BoundPlayer);
                    }
                }
            }
            return favorites;
        }

        // Removes the tile from its current panel, adds it to the target panel, and updates the star.
        private void MovePlayerControl(PlayerUserControl playerControl, FlowLayoutPanel targetPanel, bool isFavorite)
        {
            // 1. Detach from the old FlowLayoutPanel without disposing the tile.
            Control? currentParent = playerControl.Parent;
            if (currentParent != null)
            {
                currentParent.Controls.Remove(playerControl);
            }

            // 2. Attach to the new panel. The FlowLayoutPanel will lay out the tile automatically.
            targetPanel.Controls.Add(playerControl);

            // 3. Match the star to the new favorite state.
            playerControl.SetFavorite(isFavorite);
        }

        // Writes whatever tiles sit in the favorite panel to DataFiles/favoritePlayers.txt.
        private void SaveCurrentFavoritePlayers()
        {
            Team? team = (Team?)cbFavoriteTeam.SelectedItem;
            if (team is null)
            {
                return;
            }

            List<Player> favorites = GetCurrentFavoritePlayersFromPanel();
            _favoritePlayerService.SaveFavoritePlayers(team.FifaCode, favorites);
        }

        // Syncs each tile star with FavoritePlayerService using the ids on disk.
        private void RefreshPlayerControlStars()
        {
            Team? team = (Team?)cbFavoriteTeam.SelectedItem;
            if (team is null)
            {
                return;
            }

            List<string> favoriteIds = _favoritePlayerService.LoadFavoritePlayerIds(team.FifaCode);

            foreach (Control control in flowFavoritePlayers.Controls)
            {
                if (control is PlayerUserControl tile && tile.BoundPlayer != null)
                {
                    bool isFavorite = _favoritePlayerService.IsFavoritePlayer(team.FifaCode, tile.BoundPlayer, favoriteIds);
                    tile.SetFavorite(isFavorite);
                }
            }

            foreach (Control control in flowOtherPlayers.Controls)
            {
                if (control is PlayerUserControl tile && tile.BoundPlayer != null)
                {
                    bool isFavorite = _favoritePlayerService.IsFavoritePlayer(team.FifaCode, tile.BoundPlayer, favoriteIds);
                    tile.SetFavorite(isFavorite);
                }
            }
        }

        // Moves the right-clicked tile into the favorite panel if rules allow, then saves and refreshes stars.
        private void AddSelectedPlayerToFavorites()
        {
            // 1. Resolve the tile under the context menu, if any.
            PlayerUserControl? tile = GetSelectedPlayerControlFromContextMenu();
            if (tile is null)
            {
                return;
            }

            if (tile.Parent == flowFavoritePlayers)
            {
                return;
            }

            // 2. Enforce at most three tiles in the favorite panel.
            if (!CanAddMoreFavoritePlayers())
            {
                MessageBox.Show("You can choose only three favorite players.");
                return;
            }

            // 3. Move into the favorite column and turn the star on.
            MovePlayerControl(tile, flowFavoritePlayers, true);

            // 4. Persist ids so the next run matches the panels.
            SaveCurrentFavoritePlayers();

            // 5. Re-read the file and align every star with the saved ids.
            RefreshPlayerControlStars();
        }

        // Moves the right-clicked tile out of favorites only when it already lives in that panel.
        private void RemoveSelectedPlayerFromFavorites()
        {
            // 1. Resolve the tile under the context menu, if any.
            PlayerUserControl? tile = GetSelectedPlayerControlFromContextMenu();
            if (tile is null)
            {
                return;
            }

            // 2. Ignore remove when the tile is not in the favorite panel.
            if (tile.Parent != flowFavoritePlayers)
            {
                return;
            }

            // 3. Move to the other column and turn the star off.
            MovePlayerControl(tile, flowOtherPlayers, false);

            // 4. Persist ids after the move.
            SaveCurrentFavoritePlayers();

            // 5. Align every star with the file again.
            RefreshPlayerControlStars();
        }

        // User chose Add to favorites on the panel context menu.
        private void MenuItemAddToFavorites_Click(object sender, EventArgs e)
        {
            AddSelectedPlayerToFavorites();
        }

        // User chose Remove from favorites on the panel context menu.
        private void MenuItemRemoveFromFavorites_Click(object sender, EventArgs e)
        {
            RemoveSelectedPlayerFromFavorites();
        }

        private async void BtnSettings_Click(object sender, EventArgs e)
        {
            // 1. Open SettingsForm as a dialog.
            using SettingsForm formSettings = new SettingsForm();

            // 2. If user cancels, do nothing.
            if (formSettings.ShowDialog() != DialogResult.OK || formSettings.SelectedSettings is null)
            {
                return;
            }

            // 3. Save selected settings.
            SettingsService settingsService = new SettingsService();
            AppSettings newSettings = formSettings.SelectedSettings;
            settingsService.Save(newSettings);

            // 4. Update current settings in MainForm.
            _settings = newSettings;
            //prevents error 400 

            // 5. Reload teams and players.
            try
            {
                _isLoading = true;

                // 1. Reload teams for the new settings.
                _teams = await _worldCupData.GetTeamsAsync(_settings);

                // 2. Rebind the ComboBox.
                cbFavoriteTeam.DataSource = null;
                cbFavoriteTeam.DataSource = _teams;
                cbFavoriteTeam.DisplayMember = nameof(Team.DisplayName);

                // 3. Clear matches and player panels for now.
                _matches = new List<Match>();
                ClearFlowPanel(flowFavoritePlayers);
                ClearFlowPanel(flowOtherPlayers);

                // 4. Do not reuse old favorite team automatically after settings change.
                cbFavoriteTeam.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
            finally
            {
                _isLoading = false;
            }

        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 1. Ask the user to confirm closing the application.
            DialogResult result = MessageBox.Show(
                "Are you sure you want to exit?",
                "Exit confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            // 2. If the user chooses No, cancel the closing event.
            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
        }
        // Shows a simple loading message while async work is running.
        private void ShowLoading()
        {
            lblLoading.Visible = true;
            lblLoading.BringToFront();
        }

        // Hides the loading message after async work is finished.
        private void HideLoading()
        {
            lblLoading.Visible = false;
        }

        private void menuItemSetPicture_Click(object sender, EventArgs e)
        {
            // 1. Find the player tile that was right-clicked.
            PlayerUserControl? tile = GetSelectedPlayerControlFromContextMenu();
            if (tile is null)
            {
                return;
            }

            // 2. Open file dialog for image selection.
            using OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Pictures|*.jpg;*.jpeg;*.png;*.bmp|All files|*.*";
            dialog.InitialDirectory = Application.StartupPath;


            // 3. If user selected an image, show it in the tile.
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Team? team = (Team?)cbFavoriteTeam.SelectedItem;
                if (team is null || tile.BoundPlayer is null)
                {
                    return;
                }

                string savedImagePath = _playerImageService.SavePlayerImage(
                    team.FifaCode,
                    tile.BoundPlayer,
                    dialog.FileName);

                tile.SetImage(savedImagePath);
            }
        }

        // Allows dropping only when the dragged data is a PlayerUserControl.
        private void PlayerPanel_DragEnter(object sender, DragEventArgs e)
        {
            // 1. Check whether the dragged object is a player tile.
            if (e.Data != null && e.Data.GetDataPresent(typeof(PlayerUserControl)))
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        // Moves a dragged player tile into the panel where it was dropped.
        private void PlayerPanel_DragDrop(object sender, DragEventArgs e)
        {
            // 1. Make sure the target is one of the player panels.
            FlowLayoutPanel? targetPanel = sender as FlowLayoutPanel;
            if (targetPanel is null)
            { return; }

            // 2. Get the dragged player tile.
            PlayerUserControl? tile = e.Data?.GetData(typeof(PlayerUserControl)) as PlayerUserControl;
            if (tile is null)
            { return; }

            // 3. If the tile was dropped on the same panel, do nothing.
            if (tile.Parent == targetPanel)
            { return; }

            // 4. If dropping into favorites, enforce maximum 3 favorites.
            if (targetPanel == flowFavoritePlayers && !CanAddMoreFavoritePlayers())
            {
                MessageBox.Show("You can choose only three favorite players.");
                return;
            }

            // 5. Move the tile and update favorite state.
            bool isFavorite = targetPanel == flowFavoritePlayers;
            MovePlayerControl(tile, targetPanel, isFavorite);

            // 6. Save current favorite players and refresh stars.
            SaveCurrentFavoritePlayers();
            RefreshPlayerControlStars();
        }

        // Starts dragging a player tile when the user holds the left mouse button.
        private void PlayerTile_MouseDown(object sender, MouseEventArgs e)
        {
            // 1. Only start drag with left mouse button.
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            // 2. Make sure the sender is a player tile.
            PlayerUserControl? tile = sender as PlayerUserControl;
            if (tile is null)
            {
                return;
            }

            // 3. Start dragging the tile.
            tile.DoDragDrop(tile, DragDropEffects.Move);
        }

        //private void PlayerTile_MouseDown(object sender, MouseEventArgs e)
        //{
        //    // 1. Only left mouse button starts drag.
        //    if (e.Button != MouseButtons.Left)
        //    {
        //        return;
        //    }

        //    // 2. Sender can be the tile itself or a child control inside it.
        //    Control? source = sender as Control;
        //    while (source != null && source is not PlayerUserControl)
        //    {
        //        source = source.Parent;
        //    }

        //    // 3. Drag the whole PlayerUserControl, similar to dragging PictureBox in the exercise.
        //    PlayerUserControl? tile = source as PlayerUserControl;
        //    if (tile != null)
        //    {
        //        tile.DoDragDrop(tile, DragDropEffects.Move);
        //    }
        //}
    }
}

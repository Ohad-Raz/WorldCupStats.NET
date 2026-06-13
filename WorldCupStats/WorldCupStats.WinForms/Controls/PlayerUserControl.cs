using WorldCupStats.Data.Models;
using WorldCupStats.Data.Persistence;

namespace WorldCupStats.WinForms
{
    public partial class PlayerUserControl : UserControl
    {
        private Player? _player;
        public bool IsSelectedForMove { get; private set; }

        public PlayerUserControl()
        {
            InitializeComponent();
        }

        // Last player passed to SetPlayer. Used for context menus and later for drag and drop.
        public Player? BoundPlayer => _player;

        // Fills the labels from the player and shows a star when marked favorite.
        public void SetPlayer(Player player, bool isFavorite)
        {
            // 1. Remember the model for later features such as context menus
            _player = player;

            // 2. Copy simple fields onto the labels
            lblName.Text = player.Name ?? string.Empty;
            lblShirtNumber.Text = player.ShirtNumber.ToString();
            lblPosition.Text = player.Position ?? string.Empty;
            lblCaptain.Text = player.Captain ? "Captain" : string.Empty;

            // 3. Star on or off from the favorite flag
            SetFavorite(isFavorite);
        }

        // Updates only the favorite star label.
        public void SetFavorite(bool isFavorite)
        {
            lblFavoriteStar.Text = isFavorite ? "\u2605" : string.Empty;
        }
        // Shows the given image file, or the shared default player image when missing.
        public void SetImage(string? imagePath)
        {
            string resolvedPath = imagePath ?? string.Empty;

            if (string.IsNullOrWhiteSpace(resolvedPath) || !File.Exists(resolvedPath))
            {
                resolvedPath = AppPaths.DefaultPlayerImagePath;
            }

            if (File.Exists(resolvedPath))
            {
                pbPlayerImage.ImageLocation = resolvedPath;
            }
        }
        // Changes whether this player tile is visually selected for multi-move.
        public void SetSelectedForMove(bool isSelected)
        {
            // 1. Save selected state.
            IsSelectedForMove = isSelected;

            // 2. Show selected state visually.
            if (isSelected)
            {
                BackColor = Color.LightBlue;
            }
            else
            {
                BackColor = SystemColors.Control;
            }
        }
    }
}

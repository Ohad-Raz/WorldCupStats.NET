using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WorldCupStats.Data.Models;

namespace WorldCupStats.WPF
{
    public partial class PlayerPitchUserControl : UserControl
    {
        public Player? BoundPlayer { get; private set; }

        public PlayerPitchUserControl()
        {
            InitializeComponent();
        }

        // Displays one player inside the pitch control
        public void SetPlayer(Player player, string imagePath)
        {
            BoundPlayer = player;

            txtPlayerName.Text = player.Name;
            txtShirtNumber.Text = $"#{player.ShirtNumber}";

            LoadPlayerImage(imagePath);
        }

        // Loads the player image without permanently locking the file
        private void LoadPlayerImage(string imagePath)
        {
            if (!File.Exists(imagePath))
            {
                imgPlayer.Source = null;
                return;
            }

            BitmapImage image = new BitmapImage();

            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.UriSource = new Uri(imagePath, UriKind.Absolute);
            image.EndInit();
            image.Freeze();

            imgPlayer.Source = image;
        }
    }
}
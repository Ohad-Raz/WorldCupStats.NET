using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using WorldCupStats.Data.Models;
using WorldCupStats.Data.Persistence;

namespace WorldCupStats.WPF
{
    /// <summary>
    /// Interaction logic for PlayerDetailsWindow.xaml
    /// </summary>
    public partial class PlayerDetailsWindow : Window
    {
        public PlayerDetailsWindow(
            Player player,
            int goals,
            int yellowCards,
            string? imagePath)
        {
            InitializeComponent();

            ShowPlayerDetails(player, goals, yellowCards, imagePath);
        }

        // Shows player information in the details window
        private void ShowPlayerDetails(
            Player player,
            int goals,
            int yellowCards,
            string? imagePath)
        {
            lblPlayerName.Text = player.Name;
            lblShirtNumber.Text = $"Shirt number: {player.ShirtNumber}";
            lblPosition.Text = $"Position: {player.Position}";
            lblCaptain.Text = player.Captain ? "Captain: Yes" : "Captain: No";
            lblGoals.Text = $"Goals in this match: {goals}";
            lblYellowCards.Text = $"Yellow cards in this match: {yellowCards}";

            if (!string.IsNullOrWhiteSpace(imagePath) && File.Exists(imagePath))
            {
                imgPlayer.Source = new BitmapImage(new Uri(imagePath));
            }
            else if (File.Exists(AppPaths.DefaultPlayerImagePath))
            {
                imgPlayer.Source = new BitmapImage(new Uri(AppPaths.DefaultPlayerImagePath));
            }
        }

        // Plays a short scale animation when the player details window opens
        private void Window_Loaded(
            object sender,
            RoutedEventArgs e)
        {
            contentScale.ScaleX = 0.75;
            contentScale.ScaleY = 0.75;

            DoubleAnimation scaleAnimation =
                new DoubleAnimation
                {
                    From = 0.75,
                    To = 1,
                    Duration = TimeSpan.FromSeconds(0.3),
                    EasingFunction = new QuadraticEase
                    {
                        EasingMode = EasingMode.EaseOut
                    }
                };

            contentScale.BeginAnimation(
                ScaleTransform.ScaleXProperty,
                scaleAnimation);

            contentScale.BeginAnimation(
                ScaleTransform.ScaleYProperty,
                scaleAnimation);
        }

        // Closes the player details window
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
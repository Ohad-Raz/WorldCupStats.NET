using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WorldCupStats.Data.Models;

namespace WorldCupStats.WPF
{
    /// <summary>
    /// Interaction logic for TeamDetailsWindow.xaml
    /// </summary>

    public partial class TeamDetailsWindow : Window
    {
        public TeamDetailsWindow(Team team)
        {
            InitializeComponent();

            ShowTeamDetails(team);
        }
        // Shows team statistics in the details window
        private void ShowTeamDetails(Team team)
        {
            lblTeamName.Text = team.DisplayName;
            lblWins.Text = team.NoWins.ToString();
            lblLosses.Text = team.NoLosses.ToString();
            lblDraws.Text = team.Draws.ToString();
            lblGamesPlayed.Text = team.GamesPlayed.ToString();
            lblGoalsFor.Text = team.GoalsFor.ToString();
            lblGoalsAgainst.Text = team.GoalsAgainst.ToString();
            lblGoalDifference.Text = team.GoalDifferential.ToString();
        }
        // Plays a short fade-in animation when the team details window opens
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Starts at opacity 0 so the fade-in animation can play
            Opacity = 0;
            DoubleAnimation fadeAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.5)
            };

            BeginAnimation(Window.OpacityProperty, fadeAnimation);
        }
    }
}
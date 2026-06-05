using System.Text.Json.Serialization;

namespace WorldCupStats.Data.Models
{
    public class Team : IComparable<Team>
    {
        [JsonPropertyName("fifa_code")]
        public string FifaCode { get; set; } = string.Empty;

        [JsonPropertyName("country")]
        public string Country { get; set; } = string.Empty;

        [JsonPropertyName("wins")]
        public int NoWins { get; set; }

        [JsonPropertyName("losses")]
        public int NoLosses { get; set; }

        [JsonPropertyName("draws")]
        public int Draws { get; set; }

        [JsonPropertyName("games_played")]
        public int GamesPlayed { get; set; }

        [JsonPropertyName("goals_for")]
        public int GoalsFor { get; set; }

        [JsonPropertyName("goals_against")]
        public int GoalsAgainst { get; set; }

        [JsonPropertyName("goal_differential")]
        public int GoalDifferential { get; set; }

        [JsonPropertyName("points")]
        public int Points { get; set; }

        public string DisplayName => $"{Country} ({FifaCode})";

        public override string ToString() => DisplayName;

        public int CompareTo(Team? other) =>
            FifaCode.CompareTo(other?.FifaCode);

        public override bool Equals(object? obj) =>
            obj is Team team && FifaCode == team.FifaCode;

        public override int GetHashCode() =>
            HashCode.Combine(FifaCode);
    }
}

using System.Text.Json.Serialization;

namespace WorldCupStats.Data.Models
{
    public class Match
    {
        [JsonPropertyName("datetime")]
        public DateTime Datetime { get; set; }

        [JsonPropertyName("location")]
        public string Location { get; set; } = string.Empty;

        [JsonPropertyName("attendance")]
        public string Attendance { get; set; } = string.Empty;

        [JsonPropertyName("home_team")]
        public MatchTeam HomeTeam { get; set; } = new();

        [JsonPropertyName("away_team")]
        public MatchTeam AwayTeam { get; set; } = new();

        [JsonPropertyName("home_team_statistics")]
        public TeamStatistics HomeTeamStatistics { get; set; } = new();

        [JsonPropertyName("away_team_statistics")]
        public TeamStatistics AwayTeamStatistics { get; set; } = new();

        [JsonPropertyName("home_team_events")]
        public List<MatchEvent> HomeTeamEvents { get; set; } = new();

        [JsonPropertyName("away_team_events")]
        public List<MatchEvent> AwayTeamEvents { get; set; } = new();

        public override string? ToString() =>
            $"{Datetime}: {HomeTeam?.Country} vs {AwayTeam?.Country}";
    }
}

using System.Text.Json.Serialization;

namespace WorldCupStats.Data.Models
{
    public class TeamStatistics
    {
        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("starting_eleven")]
        public List<Player> StartingEleven { get; set; }

        [JsonPropertyName("substitutes")]
        public List<Player> Substitutes { get; set; }
    }
}
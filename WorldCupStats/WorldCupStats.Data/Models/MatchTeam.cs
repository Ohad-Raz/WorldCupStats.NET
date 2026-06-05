using System.Text.Json.Serialization;

namespace WorldCupStats.Data.Models
{
    public class MatchTeam
    {
        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("goals")]
        public int Goals { get; set; }
    }
}
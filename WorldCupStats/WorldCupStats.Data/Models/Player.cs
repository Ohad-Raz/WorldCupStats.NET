using System.Text.Json.Serialization;

namespace WorldCupStats.Data.Models
{
    public class Player : IComparable<Player>
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("captain")]
        public bool Captain { get; set; }

        [JsonPropertyName("shirt_number")]
        public int ShirtNumber { get; set; }

        [JsonPropertyName("position")]
        public string Position { get; set; }

        public int CompareTo(Player? other)
        {
            if (other is null)
                return 1;

            return ShirtNumber.CompareTo(other.ShirtNumber);
        }

        public override string ToString()
        {
            return $"{Name} #{ShirtNumber}";
        }

        public override bool Equals(object? obj)
        {
            return obj is Player player &&
                   Name == player.Name &&
                   ShirtNumber == player.ShirtNumber;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, ShirtNumber);
        }
    }
}
namespace WorldCupStats.Data.Models
{
    public class MatchRanking
    {
        public string Location { get; set; } = string.Empty;
        public int Attendance { get; set; }
        public string HomeTeam { get; set; } = string.Empty;
        public string AwayTeam { get; set; } = string.Empty;
    }
}
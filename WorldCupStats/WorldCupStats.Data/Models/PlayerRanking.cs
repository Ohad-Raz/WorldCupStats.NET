namespace WorldCupStats.Data.Models;

public class PlayerRanking
{
    public string Name { get; set; } = string.Empty;
    public int ShirtNumber { get; set; }
    public string Position { get; set; } = string.Empty;
    public int Appearances { get; set; }
    public int Goals { get; set; }
    public int YellowCards { get; set; }
}

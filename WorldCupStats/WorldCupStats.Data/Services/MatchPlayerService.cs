using WorldCupStats.Data.Models;

namespace WorldCupStats.Data.Services
{
    // Gets roster players from the team's first match by date
    public class MatchPlayerService
    {
        // Gets players from the earliest match and combines starters with substitutes
        public List<Player> GetPlayersFromFirstMatch(List<Match>? matches, string fifaCode)
        {
            if (matches == null || matches.Count == 0 || string.IsNullOrWhiteSpace(fifaCode))
            {
                return new List<Player>();
            }

            // 1. Remove extra spaces from the FIFA code
            string code = fifaCode.Trim();

            // 2. Sort matches by date and take the first one
            Match? first = matches.OrderBy((Match m) => m.Datetime).FirstOrDefault();
            if (first is null)
            {
                return new List<Player>();
            }

            // 3. Get home or away statistics for our FIFA code
            TeamStatistics? stats = null;
            if (first.HomeTeam is not null &&
                string.Equals(first.HomeTeam.Code, code, StringComparison.OrdinalIgnoreCase))
            {
                stats = first.HomeTeamStatistics;
            }
            else if (first.AwayTeam is not null &&
                     string.Equals(first.AwayTeam.Code, code, StringComparison.OrdinalIgnoreCase))
            {
                stats = first.AwayTeamStatistics;
            }

            if (stats is null)
            {
                return new List<Player>();
            }

            // 4. Combines starters and substitutes into one roster
            List<Player> starters = stats.StartingEleven ?? new List<Player>();
            List<Player> substitutes = stats.Substitutes ?? new List<Player>();
            return starters.Concat(substitutes).ToList();
        }
    }
}

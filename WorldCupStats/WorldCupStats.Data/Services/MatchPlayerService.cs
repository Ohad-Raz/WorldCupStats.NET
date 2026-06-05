using WorldCupStats.Data.Models;

namespace WorldCupStats.Data.Services
{
    // Picks roster players from the first match in time for the given FIFA code.
    public class MatchPlayerService
    {
        // Sorts matches by Datetime, takes the first, finds home or away stats for the code, then merges starters and subs.
        public List<Player> GetPlayersFromFirstMatch(List<Match>? matches, string fifaCode)
        {
            if (matches == null || matches.Count == 0 || string.IsNullOrWhiteSpace(fifaCode))
            {
                return new List<Player>();
            }

            // 1. Normalize the FIFA code we compare against home and away
            string code = fifaCode.Trim();

            // 2. Earliest match in the list is treated as the first played match here
            Match? first = matches.OrderBy((Match m) => m.Datetime).FirstOrDefault();
            if (first is null)
            {
                return new List<Player>();
            }

            // 3. Pick the statistics block that belongs to our national team
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

            // 4. Merge starters and substitutes into one list for the UI
            List<Player> starters = stats.StartingEleven ?? new List<Player>();
            List<Player> substitutes = stats.Substitutes ?? new List<Player>();
            return starters.Concat(substitutes).ToList();
        }
    }
}

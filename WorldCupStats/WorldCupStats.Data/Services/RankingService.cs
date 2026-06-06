using WorldCupStats.Data.Models;

namespace WorldCupStats.Data.Services
{
    public class RankingService
    {
        // Builds match rankings ordered by attendance from highest to lowest.
        public List<MatchRanking> GetMatchRankings(List<Match> matches)
        {
            List<MatchRanking> rankings = new List<MatchRanking>();

            // 1. Create one ranking row for each match.
            foreach (Match match in matches)
            {
                int attendance = 0;
                int.TryParse(match.Attendance, out attendance);

                MatchRanking ranking = new MatchRanking
                {
                    Location = match.Location ?? string.Empty,
                    Attendance = attendance,
                    HomeTeam = match.HomeTeam?.Country ?? string.Empty,
                    AwayTeam = match.AwayTeam?.Country ?? string.Empty
                };

                rankings.Add(ranking);
            }

            // 2. Return matches ordered by attendance, highest first.
            return rankings
                .OrderByDescending(ranking => ranking.Attendance)
                .ToList();
        }

        // Builds player rankings for the selected team.
        public List<PlayerRanking> GetPlayerRankings(List<Match> matches, string fifaCode)
        {
            List<PlayerRanking> rankings = new List<PlayerRanking>();

            // 1. Go through each match of the selected team.
            foreach (Match match in matches)
            {
                TeamStatistics? statistics = GetTeamStatisticsForMatch(match, fifaCode);
                if (statistics is null)
                {
                    continue;
                }

                // 2. Combine starting eleven and substitutes.
                List<Player> players = new List<Player>();

                if (statistics.StartingEleven != null)
                {
                    players.AddRange(statistics.StartingEleven);
                }

                if (statistics.Substitutes != null)
                {
                    players.AddRange(statistics.Substitutes);
                }

                // 3. Get events for this team in this match.
                List<MatchEvent> events = GetTeamEventsForMatch(match, fifaCode);

                // 4. Update ranking row for each player.
                foreach (Player player in players)
                {
                    PlayerRanking? ranking = rankings.FirstOrDefault(
                        r => r.Name == player.Name && r.ShirtNumber == player.ShirtNumber);

                    if (ranking is null)
                    {
                        ranking = new PlayerRanking
                        {
                            Name = player.Name,
                            ShirtNumber = player.ShirtNumber,
                            Position = player.Position,
                            Appearances = 0,
                            Goals = 0,
                            YellowCards = 0
                        };

                        rankings.Add(ranking);
                    }

                    ranking.Appearances++;

                    ranking.Goals += CountPlayerEvents(events, player.Name, "goal");
                    ranking.YellowCards += CountPlayerEvents(events, player.Name, "yellow-card");
                }
            }

            // 5. Return ordered rankings: goals first, then yellow cards, then appearances.
            return rankings
                .OrderByDescending(ranking => ranking.Goals)
                .ThenByDescending(ranking => ranking.YellowCards)
                .ThenByDescending(ranking => ranking.Appearances)
                .ToList();
        }
        // Counts how many matching events belong to one player.
        private int CountPlayerEvents(List<MatchEvent> events, string playerName, string eventNamePart)
        {
            int count = 0;

            // 1. Loop through team events.
            foreach (MatchEvent matchEvent in events)
            {
                // 2. Count event only when player name and event type match.
                if (matchEvent.Player == playerName &&
                    matchEvent.TypeOfEvent.Contains(eventNamePart))
                {
                    count++;
                }
            }

            return count;
        }
        // Gets the statistics object for the selected team in one match.
        private TeamStatistics? GetTeamStatisticsForMatch(Match match, string fifaCode)
        {
            // 1. Check if the selected team is the home team.
            if (match.HomeTeam?.Code == fifaCode)
            {
                return match.HomeTeamStatistics;
            }

            // 2. Check if the selected team is the away team.
            if (match.AwayTeam?.Code == fifaCode)
            {
                return match.AwayTeamStatistics;
            }

            // 3. Return null if this match does not belong to the selected team.
            return null;
        }
        // Gets the event list for the selected team in one match.
        private List<MatchEvent> GetTeamEventsForMatch(Match match, string fifaCode)
        {
            // 1. If the selected team is home, return home events.
            if (match.HomeTeam?.Code == fifaCode)
            {
                return match.HomeTeamEvents ?? new List<MatchEvent>();
            }

            // 2. If the selected team is away, return away events.
            if (match.AwayTeam?.Code == fifaCode)
            {
                return match.AwayTeamEvents ?? new List<MatchEvent>();
            }

            // 3. If the match is unrelated, return an empty list.
            return new List<MatchEvent>();
        }
    }
}
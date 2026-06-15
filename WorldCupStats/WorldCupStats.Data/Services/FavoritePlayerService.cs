using WorldCupStats.Data.Models;
using WorldCupStats.Data.Persistence;

namespace WorldCupStats.Data.Services
{
    public class FavoritePlayerService
    {
        private readonly IRepository _repo = RepositoryFactory.GetInstance();

        private static char Separator => '|';

        // Creates an ID using the team, player name and shirt number
        public static string MakeFavoriteId(string fifaCode, Player player)
        {
            string code = fifaCode.Trim();
            string name = player.Name?.Trim() ?? string.Empty;
            return $"{code}{Separator}{name}{Separator}{player.ShirtNumber}";
        }

        public bool IsFavoritePlayer(string fifaCode, Player player, IEnumerable<string> favoriteIds)
        {
            string id = MakeFavoriteId(fifaCode, player);
            return favoriteIds.Contains(id, StringComparer.OrdinalIgnoreCase);
        }

        // Loads favorite player ids for one team
        public List<string> LoadFavoritePlayerIds(string fifaCode)
        {
            string prefix = $"{fifaCode.Trim()}{Separator}";

            return LoadAllFavoriteIds()
                .Where(id => id.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        // Saves this team's favorites without removing favorites from other teams
        public void SaveFavoritePlayers(string fifaCode, IEnumerable<Player> players)
        {
            string prefix = $"{fifaCode.Trim()}{Separator}";

            IEnumerable<string> keptFromOtherTeams = LoadAllFavoriteIds()
                .Where(id => !id.StartsWith(prefix, StringComparison.OrdinalIgnoreCase));

            HashSet<string> idsForTeam = players
                .Select(player => MakeFavoriteId(fifaCode, player))
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            IEnumerable<string> allIds = keptFromOtherTeams.Concat(idsForTeam);
            _repo.WriteAllText(AppPaths.FavoritePlayersFilePath, string.Join(Environment.NewLine, allIds));
        }

        private List<string> LoadAllFavoriteIds()
        {
            if (!_repo.Exists(AppPaths.FavoritePlayersFilePath))
            {
                return new List<string>();
            }

            string text = _repo.ReadAllText(AppPaths.FavoritePlayersFilePath);

            return text
                .Split(new[] { "\r\n", "\n" }, StringSplitOptions.None)
                .Select(line => line.Trim())
                .Where(line => line.Length > 0)
                .Where(IsValidFavoriteId)
                .ToList();
        }

        // Checks that the favorite ID has the expected format
        private static bool IsValidFavoriteId(string line)
        {
            string[] parts = line.Split(Separator);
            return parts.Length >= 3
                && !string.IsNullOrWhiteSpace(parts[0])
                && int.TryParse(parts[^1], out _); // Uses the last part as the shirt number
        }
    }
}

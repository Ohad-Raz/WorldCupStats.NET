using WorldCupStats.Data.Models;
using WorldCupStats.Data.Persistence;

namespace WorldCupStats.Data.Services
{
    public class FavoritePlayerService
    {
        private readonly IRepository _repo = RepositoryFactory.GetInstance();

        private static char Separator => '|';

        // Stable id: FIFA_CODE|PlayerName|ShirtNumber
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

        // Favorite ids for one national team only.
        public List<string> LoadFavoritePlayerIds(string fifaCode)
        {
            string prefix = $"{fifaCode.Trim()}{Separator}";

            return LoadAllFavoriteIds()
                .Where(id => id.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        // Keeps favorites for other teams and replaces the set for the given FIFA code.
        public void SaveFavoritePlayers(string fifaCode, IEnumerable<Player> players)
        {
            string prefix = $"{fifaCode.Trim()}{Separator}";

            IEnumerable<string> keptFromOtherTeams = LoadAllFavoriteIds()
                .Where(id => !id.StartsWith(prefix, StringComparison.OrdinalIgnoreCase));

            HashSet<string> idsForTeam = players
                .Select(player => MakeFavoriteId(fifaCode, player))
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            IEnumerable<string> allIds = keptFromOtherTeams.Concat(idsForTeam);
            _repo.WriteAllText(AppPaths.FavoritePlayersFile, string.Join(Environment.NewLine, allIds));
        }

        public void ClearFavoritePlayers(string fifaCode)
        {
            SaveFavoritePlayers(fifaCode, Array.Empty<Player>());
        }

        private List<string> LoadAllFavoriteIds()
        {
            if (!_repo.Exists(AppPaths.FavoritePlayersFile))
            {
                return new List<string>();
            }

            string text = _repo.ReadAllText(AppPaths.FavoritePlayersFile);

            return text
                .Split(new[] { "\r\n", "\n" }, StringSplitOptions.None)
                .Select(line => line.Trim())
                .Where(line => line.Length > 0)
                .Where(IsValidFavoriteId)
                .ToList();
        }

        // Accepts FIFA|Name|Number. Ignores legacy Name|Number lines from older builds.
        private static bool IsValidFavoriteId(string line)
        {
            string[] parts = line.Split(Separator);
            return parts.Length >= 3
                && !string.IsNullOrWhiteSpace(parts[0])
                && int.TryParse(parts[^1], out _);
        }
    }
}

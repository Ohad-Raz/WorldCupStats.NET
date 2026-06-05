using WorldCupStats.Data.Models;

namespace WorldCupStats.Data.Persistence
{
    /// <summary>
    /// Central relative-path rules rooted at <see cref="AppContext.BaseDirectory"/>.
    /// </summary>
    public static class AppPaths
    {
        public const string DataRootFolder = "DataFiles";
        public const string AssetsFolder = "Assets";

        public static string BaseDirectory => AppContext.BaseDirectory;

        public static string Resolve(string relativePath) =>
            Path.Combine(BaseDirectory, relativePath);

        public static string DataRoot => Resolve(DataRootFolder);

        public static string SettingsFile => Path.Combine(DataRootFolder, "settings.json");

        public static string FavoriteTeamFile => Path.Combine(DataRootFolder, "favoriteTeam.txt");

        public static string FavoritePlayersFile => Path.Combine(DataRootFolder, "favoritePlayers.txt");

        public static string TeamsJsonRelative(ChampionshipType championship) =>
            Path.Combine(DataRootFolder, GenderFolder(championship), "teams", "results.json");

        public static string CountryMatchesJsonRelative(ChampionshipType championship, string fifaCode) =>
            Path.Combine(DataRootFolder, GenderFolder(championship), "matches", $"country_{fifaCode}.json");

        /// <summary>Future player images: Assets/Players/{fifaCode}/...</summary>
        public static string PlayerImagesRelative(string fifaCode) =>
            Path.Combine(AssetsFolder, "Players", fifaCode);

        public static string TeamsJsonPath(ChampionshipType championship) =>
            Resolve(TeamsJsonRelative(championship));

        public static string CountryMatchesJsonPath(ChampionshipType championship, string fifaCode) =>
            Resolve(CountryMatchesJsonRelative(championship, fifaCode));

        public static void EnsureDataRootExists() =>
            Directory.CreateDirectory(DataRoot);

        private static string GenderFolder(ChampionshipType championship) =>
            championship == ChampionshipType.Women ? "women" : "men";
    }
}

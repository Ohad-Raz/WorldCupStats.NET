using WorldCupStats.Data.Models;

namespace WorldCupStats.Data.Persistence
{
    /// <summary>
    /// Bundled application content lives under <see cref="BundledBaseDirectory"/>.
    /// User-generated persistence lives under <see cref="SharedUserDataRoot"/>.
    /// </summary>
    public static class AppPaths
    {
        public const string DataRootFolder = "DataFiles";
        public const string AssetsFolder = "Assets";
        public const string DefaultPlayerImageFileName = "default-player.png";
        private const string SharedAppFolderName = "WorldCupStats";

        // --- Bundled application data (AppContext.BaseDirectory) ---

        public static string BundledBaseDirectory => AppContext.BaseDirectory;

        public static string ResolveBundled(string relativePath) =>
            Path.Combine(BundledBaseDirectory, relativePath);

        public static string BundledDefaultPlayerImagePath =>
            ResolveBundled(Path.Combine(AssetsFolder, DefaultPlayerImageFileName));

        public static string TeamsJsonPath(ChampionshipType championship) =>
            ResolveBundled(TeamsJsonRelative(championship));

        public static string CountryMatchesJsonPath(ChampionshipType championship, string fifaCode) =>
            ResolveBundled(CountryMatchesJsonRelative(championship, fifaCode));

        public static string TeamsJsonRelative(ChampionshipType championship) =>
            Path.Combine(DataRootFolder, GenderFolder(championship), "teams", "results.json");

        public static string CountryMatchesJsonRelative(ChampionshipType championship, string fifaCode) =>
            Path.Combine(DataRootFolder, GenderFolder(championship), "matches", $"country_{fifaCode}.json");

        // --- Shared user data (%LocalAppData%\WorldCupStats) ---

        public static string SharedUserDataRoot =>
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                SharedAppFolderName);

        public static string SharedDataRoot =>
            Path.Combine(SharedUserDataRoot, DataRootFolder);

        public static string SharedAssetsRoot =>
            Path.Combine(SharedUserDataRoot, AssetsFolder);

        public static string SharedPlayerImagesRoot =>
            Path.Combine(SharedAssetsRoot, "Players");

        public static string SettingsFilePath =>
            Path.Combine(SharedDataRoot, "settings.json");

        public static string FavoriteTeamFilePath =>
            Path.Combine(SharedDataRoot, "favoriteTeam.txt");

        public static string FavoritePlayersFilePath =>
            Path.Combine(SharedDataRoot, "favoritePlayers.txt");

        public static string SharedDefaultPlayerImagePath =>
            Path.Combine(SharedAssetsRoot, DefaultPlayerImageFileName);

        public static string SharedPlayerImagesFolder(string fifaCode) =>
            Path.Combine(SharedPlayerImagesRoot, fifaCode);

        /// <summary>
        /// Shared default image used by both clients after first-use bootstrap.
        /// </summary>
        public static string DefaultPlayerImagePath
        {
            get
            {
                EnsureSharedDefaultPlayerImage();
                return SharedDefaultPlayerImagePath;
            }
        }

        public static void EnsureSharedUserDataRootExists() =>
            Directory.CreateDirectory(SharedDataRoot);

        public static void EnsureSharedDefaultPlayerImage()
        {
            if (File.Exists(SharedDefaultPlayerImagePath))
            {
                return;
            }

            Directory.CreateDirectory(SharedAssetsRoot);

            if (File.Exists(BundledDefaultPlayerImagePath))
            {
                File.Copy(BundledDefaultPlayerImagePath, SharedDefaultPlayerImagePath, overwrite: false);
            }
        }

        private static string GenderFolder(ChampionshipType championship) =>
            championship == ChampionshipType.Women ? "women" : "men";
    }
}

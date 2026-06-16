using WorldCupStats.Data.Models;

namespace WorldCupStats.Data.Persistence
{
    /// <summary>
    /// Bundled files use the application folder, while saved user data uses LocalAppData
    /// </summary>
    public static class AppPaths
    {
        public const string DataRootFolder = "DataFiles";
        public const string AssetsFolder = "Assets";
        public const string DefaultPlayerImageFileName = "default-player.png";
        private const string SharedAppFolderName = "WorldCupStats";

        // Bundled application data

        // Returns the folder where the application is running
        public static string BundledBaseDirectory => AppContext.BaseDirectory;

        // Combines the application folder with a relative path
        public static string ResolveBundled(string relativePath) =>
            Path.Combine(BundledBaseDirectory, relativePath);

        // Returns the bundled default player image path
        public static string BundledDefaultPlayerImagePath =>
            ResolveBundled(Path.Combine(AssetsFolder, DefaultPlayerImageFileName));

        // Returns the full path to the teams JSON file
        public static string TeamsJsonPath(ChampionshipType championship) =>
            ResolveBundled(TeamsJsonRelative(championship));

        // Returns the full path to one country's matches JSON file
        public static string CountryMatchesJsonPath(ChampionshipType championship, string fifaCode) =>
            ResolveBundled(CountryMatchesJsonRelative(championship, fifaCode));

        // Builds the relative path to the teams JSON file
        public static string TeamsJsonRelative(ChampionshipType championship) =>
            Path.Combine(DataRootFolder, GenderFolder(championship), "teams", "results.json");

        // Builds the relative path to one country's matches JSON file
        public static string CountryMatchesJsonRelative(ChampionshipType championship, string fifaCode) =>
            Path.Combine(DataRootFolder, GenderFolder(championship), "matches", $"country_{fifaCode}.json");

        // Shared user data

        // Returns the WorldCupStats folder in LocalAppData
        public static string SharedUserDataRoot =>
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                SharedAppFolderName);

        // Returns the folder used for settings and favorite files
        public static string SharedDataRoot =>
            Path.Combine(SharedUserDataRoot, DataRootFolder);

        // Returns the shared Assets folder
        public static string SharedAssetsRoot =>
            Path.Combine(SharedUserDataRoot, AssetsFolder);

        // Returns the main folder used for custom player images
        public static string SharedPlayerImagesRoot =>
            Path.Combine(SharedAssetsRoot, "Players");

        // Returns the settings file path
        public static string SettingsFilePath =>
            Path.Combine(SharedDataRoot, "settings.json");

        // Returns the favorite team file path
        public static string FavoriteTeamFilePath =>
            Path.Combine(SharedDataRoot, "favoriteTeam.txt");

        // Returns the favorite players file path
        public static string FavoritePlayersFilePath =>
            Path.Combine(SharedDataRoot, "favoritePlayers.txt");

        // Returns the shared default player image path
        public static string SharedDefaultPlayerImagePath =>
            Path.Combine(SharedAssetsRoot, DefaultPlayerImageFileName);

        // Returns the custom image folder for one team
        public static string SharedPlayerImagesFolder(string fifaCode) =>
            Path.Combine(SharedPlayerImagesRoot, fifaCode);

        // Makes sure the default image exists and returns its shared path
        public static string DefaultPlayerImagePath
        {
            get
            {
                EnsureSharedDefaultPlayerImage();
                return SharedDefaultPlayerImagePath;
            }
        }

        // Copies the bundled default image to LocalAppData when needed
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

        // Returns the folder name for the selected championship
        private static string GenderFolder(ChampionshipType championship) =>
            championship == ChampionshipType.Women ? "women" : "men";
    }
}
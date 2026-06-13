using WorldCupStats.Data.Models;
using WorldCupStats.Data.Persistence;

namespace WorldCupStats.Data.Services
{
    public class PlayerImageService
    {
        // Builds a safe file name for a player's image.
        private string BuildPlayerImageFileName(Player player, string extension)
        {
            string safeName = player.Name.Replace(" ", "_");
            return $"{safeName}_{player.ShirtNumber}{extension}";
        }

        // Builds the full saved image path for a player in shared user storage.
        private string BuildPlayerImagePath(string fifaCode, Player player, string extension)
        {
            string folderFullPath = AppPaths.SharedPlayerImagesFolder(fifaCode);
            string fileName = BuildPlayerImageFileName(player, extension);
            return Path.Combine(folderFullPath, fileName);
        }

        // Copies the selected image into shared Assets and returns the saved full path.
        public string SavePlayerImage(string fifaCode, Player player, string sourceImagePath)
        {
            string extension = Path.GetExtension(sourceImagePath);
            string fullPath = BuildPlayerImagePath(fifaCode, player, extension);

            string? directory = Path.GetDirectoryName(fullPath);
            if (directory != null)
            {
                Directory.CreateDirectory(directory);
            }

            File.Copy(sourceImagePath, fullPath, true);

            return fullPath;
        }

        // Finds an existing player image in shared storage if one was already saved.
        public string? GetPlayerImagePath(string fifaCode, Player player)
        {
            string folderFullPath = AppPaths.SharedPlayerImagesFolder(fifaCode);

            if (!Directory.Exists(folderFullPath))
            {
                return null;
            }

            string safeName = player.Name.Replace(" ", "_");
            string searchPattern = $"{safeName}_{player.ShirtNumber}.*";

            string[] files = Directory.GetFiles(folderFullPath, searchPattern);

            if (files.Length == 0)
            {
                return null;
            }

            return files[0];
        }

        // Returns the shared default player image path (bootstrapped from bundled asset on first use).
        public string GetDefaultPlayerImagePath()
        {
            return AppPaths.DefaultPlayerImagePath;
        }

        // Returns a saved player image path, or the shared default when none exists.
        public string GetPlayerImagePathOrDefault(string fifaCode, Player player)
        {
            string? savedImagePath = GetPlayerImagePath(fifaCode, player);

            if (!string.IsNullOrWhiteSpace(savedImagePath) && File.Exists(savedImagePath))
            {
                return savedImagePath;
            }

            return GetDefaultPlayerImagePath();
        }
    }
}

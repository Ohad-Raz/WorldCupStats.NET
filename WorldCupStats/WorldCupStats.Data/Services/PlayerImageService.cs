using WorldCupStats.Data.Models;
using WorldCupStats.Data.Persistence;

namespace WorldCupStats.Data.Services
{
    public class PlayerImageService
    {
        // Builds the image file name using the name and shirt number
        private string BuildPlayerImageFileName(Player player, string extension)
        {
            string safeName = player.Name.Replace(" ", "_");
            return $"{safeName}_{player.ShirtNumber}{extension}";
        }

        // Builds the full saved image path for a player in LocalAppData
        private string BuildPlayerImagePath(string fifaCode, Player player, string extension)
        {
            string folderFullPath = AppPaths.SharedPlayerImagesFolder(fifaCode);
            string fileName = BuildPlayerImageFileName(player, extension);
            return Path.Combine(folderFullPath, fileName);
        }

        // Copies the selected image into the shared folder and returns the saved path
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

        // Finds a saved image for the player
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

        // Returns the path to the default player image
        public string GetDefaultPlayerImagePath()
        {
            return AppPaths.DefaultPlayerImagePath;
        }

        // Returns the saved image path, or the default image when none exists
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

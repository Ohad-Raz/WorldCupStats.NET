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

        // Builds the full saved image path for a player.
        private string BuildPlayerImagePath(string fifaCode, Player player, string extension)
        {
            string folderRelativePath = AppPaths.PlayerImagesRelative(fifaCode);
            string fileName = BuildPlayerImageFileName(player, extension);
            string relativePath = Path.Combine(folderRelativePath, fileName);

            return AppPaths.Resolve(relativePath);
        }

        // Copies the selected image into the app Assets folder and returns the saved full path.
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

        // Finds an existing player image if one was already saved.
        public string? GetPlayerImagePath(string fifaCode, Player player)
        {
            string folderRelativePath = AppPaths.PlayerImagesRelative(fifaCode);
            string folderFullPath = AppPaths.Resolve(folderRelativePath);

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
    }
}
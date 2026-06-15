namespace WorldCupStats.Data.Persistence
{
    internal class FileRepository : IRepository
    {
        // Checks whether the file exists
        public bool Exists(string path) =>
            File.Exists(NormalizePath(path));

        // Reads all text from the file
        public string ReadAllText(string path) =>
            File.ReadAllText(NormalizePath(path));

        // Creates the folder if needed and writes the text to the file
        public void WriteAllText(string path, string content)
        {
            string fullPath = NormalizePath(path);
            string? directory = Path.GetDirectoryName(fullPath);

            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(fullPath, content);
        }

        // Uses full paths directly and places relative paths under LocalAppData
        private static string NormalizePath(string path)
        {
            if (Path.IsPathRooted(path))
            {
                return path;
            }

            return Path.Combine(AppPaths.SharedUserDataRoot, path);
        }
    }
}
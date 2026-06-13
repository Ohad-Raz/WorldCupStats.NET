namespace WorldCupStats.Data.Persistence
{
    internal class FileRepository : IRepository
    {
        public bool Exists(string path) =>
            File.Exists(NormalizePath(path));

        public string ReadAllText(string path) =>
            File.ReadAllText(NormalizePath(path));

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

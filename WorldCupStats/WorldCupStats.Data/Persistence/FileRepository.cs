namespace WorldCupStats.Data.Persistence
{
    internal class FileRepository : IRepository
    {
        public FileRepository()
        {
            AppPaths.EnsureDataRootExists();
        }

        public bool Exists(string relativePath) =>
            File.Exists(AppPaths.Resolve(relativePath));

        public string ReadAllText(string relativePath) =>
            File.ReadAllText(AppPaths.Resolve(relativePath));

        public void WriteAllText(string relativePath, string content)
        {
            string fullPath = AppPaths.Resolve(relativePath);
            string? directory = Path.GetDirectoryName(fullPath);
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(fullPath, content);
        }
    }
}

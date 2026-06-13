using System.Text.Json;
using WorldCupStats.Data.Models;
using WorldCupStats.Data.Persistence;

namespace WorldCupStats.Data.Services
{
    public class SettingsService
    {
        private readonly IRepository _repo = RepositoryFactory.GetInstance();

        public void Save(AppSettings settings)
        {
            string json = JsonSerializer.Serialize(settings);
            _repo.WriteAllText(AppPaths.SettingsFilePath, json);
        }

        public AppSettings? Load()
        {
            if (!_repo.Exists(AppPaths.SettingsFilePath))
            {
                return null;
            }

            string json = _repo.ReadAllText(AppPaths.SettingsFilePath);
            return JsonSerializer.Deserialize<AppSettings>(json);
        }
    }
}

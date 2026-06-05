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
            _repo.WriteAllText(AppPaths.SettingsFile, json);
        }

        public AppSettings? Load()
        {
            if (!_repo.Exists(AppPaths.SettingsFile))
            {
                return null;
            }

            string json = _repo.ReadAllText(AppPaths.SettingsFile);
            return JsonSerializer.Deserialize<AppSettings>(json);
        }
    }
}

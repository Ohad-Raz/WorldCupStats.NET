using WorldCupStats.Data.Persistence;

namespace WorldCupStats.Data.Services
{
    public class FavoriteTeamService
    {
        private readonly IRepository _repo = RepositoryFactory.GetInstance();

        public void SaveFavoriteTeam(string fifaCode)
        {
            _repo.WriteAllText(AppPaths.FavoriteTeamFilePath, fifaCode);
        }

        public string? LoadFavoriteTeam()
        {
            if (!_repo.Exists(AppPaths.FavoriteTeamFilePath))
            {
                return null;
            }

            return _repo.ReadAllText(AppPaths.FavoriteTeamFilePath).Trim();
        }
    }
}

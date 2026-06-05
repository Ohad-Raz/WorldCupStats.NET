using WorldCupStats.Data.Persistence;

namespace WorldCupStats.Data.Services
{
    public class FavoriteTeamService
    {
        private readonly IRepository _repo = RepositoryFactory.GetInstance();

        public void SaveFavoriteTeam(string fifaCode)
        {
            _repo.WriteAllText(AppPaths.FavoriteTeamFile, fifaCode);
        }

        public string? LoadFavoriteTeam()
        {
            if (!_repo.Exists(AppPaths.FavoriteTeamFile))
            {
                return null;
            }

            return _repo.ReadAllText(AppPaths.FavoriteTeamFile).Trim();
        }
    }
}

using System.Text.Json;
using WorldCupStats.Data.Models;
using WorldCupStats.Data.Persistence;

namespace WorldCupStats.Data.Services
{
    public class WorldCupDataService
    {
        private readonly WorldCupApiService _api = new();

        public Task<List<Team>> GetTeamsAsync(AppSettings settings)
        {
            return settings.DataSource == DataSourceMode.Json
                ? LoadTeamsFromFileAsync(settings.Championship)
                : _api.GetTeamsAsync(settings.Championship);
        }

        public Task<List<Match>> GetMatchesByFifaCodeAsync(AppSettings settings, string fifaCode)
        {
            return settings.DataSource == DataSourceMode.Json
                ? LoadMatchesFromFileAsync(settings.Championship, fifaCode)
                : _api.GetMatchesByFifaCodeAsync(settings.Championship, fifaCode);
        }

        private static async Task<List<Team>> LoadTeamsFromFileAsync(ChampionshipType championship)
        {
            string path = AppPaths.TeamsJsonPath(championship);

            if (!File.Exists(path))
            {
                throw new FileNotFoundException(
                    $"JSON teams file not found. Expected: {path}. Ensure offline DataFiles are copied to the application output folder.");
            }

            string json = await File.ReadAllTextAsync(path).ConfigureAwait(false);
            List<Team>? teams = JsonSerializer.Deserialize<List<Team>>(json);
            if (teams is null)
            {
                throw new InvalidOperationException("Failed to deserialize teams from JSON file.");
            }

            return teams;
        }

        private static async Task<List<Match>> LoadMatchesFromFileAsync(ChampionshipType championship, string fifaCode)
        {
            string path = AppPaths.CountryMatchesJsonPath(championship, fifaCode);

            if (!File.Exists(path))
            {
                throw new FileNotFoundException(
                    $"JSON matches file not found. Expected: {path}. Use one file per country (API-shaped array).");
            }

            string json = await File.ReadAllTextAsync(path).ConfigureAwait(false);
            List<Match>? matches = JsonSerializer.Deserialize<List<Match>>(json);
            if (matches is null)
            {
                throw new InvalidOperationException("Failed to deserialize matches from JSON file.");
            }

            return matches;
        }
    }
}

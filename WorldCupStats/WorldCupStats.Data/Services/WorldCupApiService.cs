using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WorldCupStats.Data.Models;

namespace WorldCupStats.Data.Services
{
    public class WorldCupApiService
    {
        private readonly HttpClient _httpClient;

        // One HttpClient for all requests to the World Cup site.
        public WorldCupApiService()
        {
            _httpClient = new HttpClient();
        }
        private const string _base = @"https://worldcup-vua.nullbit.hr/";
        private const string _teamResSuffix = @"/teams/results";
        private const string _matchesSuffix = @"/matches/";
        private const string _countrySuffix = "country?fifa_code=";




        // Full URL for teams/results for men or women.
        private string GetTeamsEndpoint(ChampionshipType championship)
        {
            string gender = championship == ChampionshipType.Women ? nameof(ChampionshipType.Women) : nameof(ChampionshipType.Men);
            string endpoint = $"{_base}{gender.ToLower()}{_teamResSuffix}";
            return endpoint;
        }

        // Fetch teams JSON and deserialize to a list of Team.
        public async Task<List<Team>> GetTeamsAsync(ChampionshipType championship)
        {
            string endpoint = GetTeamsEndpoint(championship);
            string json = await _httpClient.GetStringAsync(endpoint);

            List<Team>? teams = JsonSerializer.Deserialize<List<Team>>(json);
            if (teams == null)
            {
                throw new InvalidOperationException("Failed to deserialize teams.");
            }
            return teams;
        }

        // Base URL for the matches path before adding country query.
        private string GetMatchesByFifaCodeEndpoint(ChampionshipType championship)
        {
            string gender = championship == ChampionshipType.Women ? nameof(ChampionshipType.Women) : nameof(ChampionshipType.Men);
            string endpoint = $"{_base}{gender.ToLower()}{_matchesSuffix}";
            return endpoint;
        }

        // Fetch matches JSON for one country and deserialize to a list of Match.
        public async Task<List<Match>> GetMatchesByFifaCodeAsync(ChampionshipType championship, string fifaCode)
        {
            string endpoint = GetMatchesByFifaCodeEndpoint(championship);
            string endPointAndCode = $"{endpoint}{_countrySuffix}{fifaCode}";

            string json = await _httpClient.GetStringAsync(endPointAndCode);

            List<Match>? matches = JsonSerializer.Deserialize<List<Match>>(json);
            if (matches == null)
            {
                throw new InvalidOperationException("Failed to deserialize matches.");
            }
            return matches;
        }
    }
}

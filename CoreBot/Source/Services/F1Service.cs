using System;
using System.Net.Http;
using System.Threading.Tasks;
using CoreBot.Models.F1.Races;
using CoreBot.Settings;
using Newtonsoft.Json;
using Serilog;

namespace CoreBot.Services
{
    public class F1Service
    {
        private static readonly HttpClient _http = new HttpClient();

        public async Task<RaceRoot> GetRaceSchedule(int? season)
        {
            if (season == null) season = DateTime.Now.Year;
            Log.Information($"Getting schedule for the given season: {season}.");
            var result = await _http.GetStringAsync(string.Format(DefaultValues.F1_API_SCHEDULE_URL, season));
            Console.WriteLine(result);
            var response = JsonConvert.DeserializeObject<RaceRoot>(result);
            return response;
        }
    }
}

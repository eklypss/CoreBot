using System.Net.Http;
using System.Threading.Tasks;
using CoreBot.Settings;
using CoreBot.Models.NHL;
using Newtonsoft.Json;

namespace CoreBot.Services
{
    public class NHLService
    {
        private static readonly HttpClient _http = new HttpClient();

        public async Task<NHLRoot> GetStandingsAsync()
        {
            var result = await _http.GetStringAsync(DefaultValues.NHL_STANDINGS_URL);
            var standings = JsonConvert.DeserializeObject<NHLRoot>(result);
            return standings;
        }
    }
}

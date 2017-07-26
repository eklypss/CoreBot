using System.Net.Http;
using System.Threading.Tasks;
using CoreBot.Models.Weather;
using CoreBot.Settings;
using Newtonsoft.Json;

namespace CoreBot.Services
{
    public class WeatherService
    {
        public async Task<WeatherResponse.TopLevel> GetWeatherDataAsync(string location)
        {
            using (var http = new HttpClient())
            {
                var result = await http.GetAsync($"http://api.openweathermap.org/data/2.5/weather?q={location}&APPID={BotSettings.Instance.WeatherAPIKey}");
                return JsonConvert.DeserializeObject<WeatherResponse.TopLevel>(await result.Content.ReadAsStringAsync());
            }
        }
    }
}
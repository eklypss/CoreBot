using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CoreBot.Helpers;
using CoreBot.Models.Weather;
using CoreBot.Settings;
using Discord.Commands;
using Newtonsoft.Json;

namespace CoreBot.Modules
{
    public class WeatherModule : ModuleBase
    {
        [Command("weather")]
        public async Task Weather([Remainder] string location)
        {
            using (var http = new HttpClient())
            {
                var result = await http.GetAsync($"http://api.openweathermap.org/data/2.5/weather?q={location}&APPID={BotSettings.Instance.WeatherAPIKey}");
                var weather = JsonConvert.DeserializeObject<WeatherResponse.TopLevel>(await result.Content.ReadAsStringAsync());
                await ReplyAsync($"[**{weather.Name}, {weather.Sys.Country}**], **temp:** {Math.Round(weather.Main.Temp - 273, 1)}°C, {weather.Weather.FirstOrDefault().Description}, **wind:** {weather.Wind.Speed} m/s, **updated:** {HelperMethods.FromUnixTime((long)weather.Dt).AddHours(3).ToString()}");
            }
        }
    }
}
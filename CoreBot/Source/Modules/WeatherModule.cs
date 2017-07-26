using System;
using System.Linq;
using System.Threading.Tasks;
using CoreBot.Helpers;
using CoreBot.Services;
using Discord.Commands;
using Serilog;

namespace CoreBot.Modules
{
    public class WeatherModule : ModuleBase
    {
        private readonly WeatherService _weatherService;

        public WeatherModule(WeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [Command("weather")]
        public async Task Weather([Remainder] string location)
        {
            Log.Information($"Getting weather data for the given location: {location}.");
            var weather = await _weatherService.GetWeatherDataAsync(location);
            await ReplyAsync($"[**{weather.Name}, {weather.Sys.Country}**], **temp:** {Math.Round(weather.Main.Temp - 273, 1)}°C, {weather.Weather.FirstOrDefault().Description}, **wind:** {weather.Wind.Speed} m/s, **updated:** {HelperMethods.FromUnixTime((long)weather.Dt).AddHours(3).ToString()}");
        }
    }
}
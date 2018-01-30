using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp.Parser.Html;
using CoreBot.Helpers;
using CoreBot.Interfaces;
using CoreBot.Models.Weather;
using CoreBot.Settings;
using Humanizer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using ServiceStack;
using static CoreBot.Helpers.HelperMethods;

namespace CoreBot.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly HtmlParser _parser;
        private static readonly HttpClient _http = new HttpClient();

        public WeatherService()
        {
            _parser = new HtmlParser();
        }

        public async Task<string> GetWeatherDataAsync(string location)
        {
            string openWeatherUrl = string.Format(DefaultValues.OPEN_WEATHER_URL, location, BotSettings.Instance.WeatherAPIKey);
            // Don't use await for parallel page loading
            var openWeatherQuery = _http.GetAsync(openWeatherUrl);
            var fmiQuery = FmiAsync(location);

            await Task.WhenAll(openWeatherQuery, fmiQuery);
            if (fmiQuery.Result != null)
            {
                return fmiQuery.Result;
            }
            var result = JsonConvert.DeserializeObject<WeatherResponse.TopLevel>(await openWeatherQuery.Result.Content.ReadAsStringAsync());
            if (result.Code == 404)
            {
                Log.Warning($"Weather data not found for {location}.");
                return "Weather data not found.";
            }
            var date = result.Dt.ToDateTime().ToLocalTime();
            return CreateWeatherMessage(result.Name, Math.Round(result.Main.Temp - 273, 1), result.Sys.Country, result.Weather.FirstOrDefault().Description, result.Wind.Speed, date);
        }

        public async Task<string> FmiAsync(string location)
        {
            try
            {
                Log.Information($"Getting FMI weather data for the given location: {location}.");
                var idResponse = await _http.GetAsync(string.Format(DefaultValues.FMI_URL, location));

                // Parse initial response
                var document = await _parser.ParseAsync(await idResponse.Content.ReadAsStringAsync());
                var id = document.QuerySelector("#observation-station-menu option").GetAttribute("value");
                var statusCss = ".first-mobile-forecast-time-step-content div.weather-symbol";
                var weatherStatus = document.QuerySelector(statusCss).GetAttribute("title");

                var weatherResponse = await _http.GetAsync(DefaultValues.FMI_TEMP_URL + id);

                // Parse weather response
                dynamic weatherInfo = JObject.Parse(await weatherResponse.Content.ReadAsStringAsync());
                var temperature = weatherInfo.t2m.Last[1];
                var wind = weatherInfo.WindSpeedMS != null ? weatherInfo.WindSpeedMS.Last[1] : "??";
                long timeStamp = weatherInfo.latestObservationTime / 1000;
                var date = timeStamp.ToDateTime();

                return CreateWeatherMessage(location.ToTitleCase(), temperature, "FI", weatherStatus, wind, date);
            }
            catch (Exception)
            {
                Log.Warning("Could not parse using FMI.");
                return null;
            }
        }

        public string CreateWeatherMessage(string location, object temp, string country, string status, object wind, DateTime timestamp)
        {
            string ago = (DateTime.Now - timestamp).Humanize(maxUnit: BotSettings.Instance.HumanizerMaxUnit, precision: BotSettings.Instance.HumanizerPrecision);
            return $"[**{location}, {country}**], **temp:** {temp}°C, {status}, **wind:** {wind} m/s, **updated:** {ago} ago";
        }
    }
}

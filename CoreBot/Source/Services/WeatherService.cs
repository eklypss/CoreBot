using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp.Parser.Html;
using CoreBot.Helpers;
using CoreBot.Models.Weather;
using CoreBot.Settings;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using static CoreBot.Helpers.HelperMethods;

namespace CoreBot.Services
{
    public class WeatherService
    {
        private readonly HtmlParser _parser;

        public WeatherService()
        {
            _parser = new HtmlParser();
        }

        public async Task<string> GetWeatherDataAsync(string location)
        {
            using (var http = new HttpClient())
            {
                string openWeatherUrl = string.Format(DefaultValues.OPEN_WEATHER_URL, location, BotSettings.Instance.WeatherAPIKey);
                // Don't use await for parallel page loading
                var openWeatherQuery = http.GetAsync(openWeatherUrl);
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
                var date = result.Dt.ToDateTime().AddHours(3).ToString();
                return CreateWeatherMessage(result.Name, Math.Round(result.Main.Temp - 273, 1), result.Sys.Country, result.Weather.FirstOrDefault().Description, result.Wind.Speed, date);
            }
        }

        private async Task<string> FmiAsync(string location)
        {
            try
            {
                Log.Information($"Getting FMI weather data for location: {location}.");
                var url = string.Format(DefaultValues.FMI_URL, location);
                var document = await _parser.ParseAsync(await GetAsync(url));

                var id = document.QuerySelector("#observation-station-menu option").GetAttribute("value");

                var statusCss = ".first-mobile-forecast-time-step-content div.weather-symbol";
                var weatherStatus = document.QuerySelector(statusCss).GetAttribute("title");

                dynamic weatherInfo = JObject.Parse(await GetAsync(DefaultValues.FMI_TEMP_URL + id));
                var temperature = weatherInfo.t2m.Last[1];
                var wind = weatherInfo.WindSpeedMS != null ? weatherInfo.WindSpeedMS.Last[1] : "??";
                long timeStamp = weatherInfo.latestObservationTime / 1000;

                var dateString = timeStamp.ToDateTime().ToString();
                return CreateWeatherMessage(location, temperature, "FI", weatherStatus, wind, dateString);
            }
            catch (Exception)
            {
                Log.Warning("Could not parse using FMI.");
                return null;
            }
        }

        private string CreateWeatherMessage(string location, object temp, string country,
            string status, object wind, string timestamp)
        {
            return $"[**{location}, {country}**], **temp:** {temp}°C, {status}, **wind:** {wind} m/s, **updated:** {timestamp}";
        }
    }
}
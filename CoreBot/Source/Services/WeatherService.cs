using System;
using System.Linq;
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
        private const string FMI_URL = "http://ilmatieteenlaitos.fi/saa/{0}?forecast=short";
        private const string FMI_TEMP_URL = "http://ilmatieteenlaitos.fi/observation-data?station=";
        private const string OPEN_WEATHER_URL = "http://api.openweathermap.org/data/2.5/weather?q={0}&APPID={1}";

        private HtmlParser _parser;

        public WeatherService()
        {
            _parser = new HtmlParser();
        }

        public async Task<string> GetWeatherDataAsync(string location)
        {
            var openWeatherUrl = String.Format(OPEN_WEATHER_URL, location, BotSettings.Instance.WeatherAPIKey);

            // Don't use await for parallel page loading
            var openWeatherQuery = GetAsync(openWeatherUrl);
            var fmiQuery = FmiAsync(location);

            await Task.WhenAll(openWeatherQuery, fmiQuery);
            if (fmiQuery.Result != null)
            {
                return fmiQuery.Result;
            }
            var result = JsonConvert.DeserializeObject<WeatherResponse.TopLevel>(openWeatherQuery.Result);
            if (result.Cod == 404)
            {
                return "404";
            }
            var date = result.Dt.ToDateTime().AddHours(3).ToString();
            return CreateWeatherMessage(result.Name, Math.Round(result.Main.Temp - 273, 1), result.Sys.Country, result.Weather.FirstOrDefault().Description, result.Wind.Speed, date);
        }

        private async Task<string> FmiAsync(string location)
        {
            try
            {
                return await FetchFmiAsync(location);
            }
            catch (Exception)
            {
                Log.Information($"Could not parse using FMI");
                return null;
            }
        }

        private async Task<string> FetchFmiAsync(string location)
        {
            var url = String.Format(FMI_URL, location);
            var document = await _parser.ParseAsync(await GetAsync(url));

            var id = document.QuerySelector("#observation-station-menu option").GetAttribute("value");

            var statusCss = ".first-mobile-forecast-time-step-content div.weather-symbol";
            var weatherStatus = document.QuerySelector(statusCss).GetAttribute("title");

            dynamic weatherInfo = JObject.Parse(await GetAsync(FMI_TEMP_URL + id));
            var temperature = weatherInfo.t2m.Last[1];
            var wind = weatherInfo.WindSpeedMS != null ? weatherInfo.WindSpeedMS.Last[1] : "??";
            long timeStamp = weatherInfo.latestObservationTime / 1000;

            var dateString = timeStamp.ToDateTime().ToString();
            return CreateWeatherMessage(location, temperature, "FI", weatherStatus, wind, dateString);
        }

        private string CreateWeatherMessage(string location, object temp, string country,
            string status, object wind, string timestamp)
        {
            return $"[**{location}, {country}**], **temp:** {temp}°C, {status}, **wind:** {wind} m/s, **updated:** {timestamp}";
        }
    }
}
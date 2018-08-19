using System;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AngleSharp.Parser.Html;
using CoreBot.Helpers;
using CoreBot.Models.Weather;
using CoreBot.Settings;
using Discord;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;

namespace CoreBot.Services
{
    public class WeatherService
    {
        private readonly HtmlParser _parser;
        private static readonly HttpClient _http = new HttpClient();

        public WeatherService()
        {
            _parser = new HtmlParser();
        }

        public async Task<Embed> GetWeatherDataAsync(string location)
        {
            string openWeatherUrl = string.Format(DefaultValues.OPEN_WEATHER_URL, location, BotSettings.Instance.WeatherAPIKey);
            // Don't use await for parallel page loading
            var openWeatherQuery = _http.GetAsync(openWeatherUrl);
            var fmiQuery = GetDataFmiAsync(location);

            await Task.WhenAll(openWeatherQuery, fmiQuery);
            if (fmiQuery.Result != null)
            {
                return fmiQuery.Result;
            }
            var result = JsonConvert.DeserializeObject<WeatherResponse.TopLevel>(await openWeatherQuery.Result.Content.ReadAsStringAsync());
            if (result.Code != 200)
            {
                Log.Warning($"Weather data not found for {location}.");
                return new EmbedBuilder().WithTitle("Weather data not found.").WithColor(BotSettings.Instance.EmbeddedColor).Build();
            }

            var date = result.Dt.ToDateTime();
            return CreateEmbedWeatherMessage(result.Name, Math.Round(result.Main.Temp - 273, 1), result.Sys.Country, result.Weather.FirstOrDefault().Description, result.Wind.Speed, date);
        }

        public async Task<string> SearchForPlace(string location)
        {
            var json = await _http.GetStringAsync(DefaultValues.FMI_SEARCH_API + location);
            dynamic searchResponse = JArray.Parse(json);

            return searchResponse[0].id;
        }

        public async Task<Embed> GetDataFmiAsync(string location)
        {
            try
            {
                var searchLocation = await SearchForPlace(location);
                return await FetchWeather(searchLocation);
            }
            catch (Exception e)
            {
                Log.Warning("Could not parse using FMI." + e);
                return null;
            }
        }

        public async Task<Embed> FetchWeather(string searchLocation)
        {
            Log.Information($"Getting FMI weather data for the given location: {searchLocation}.");
            var idResponse = await _http.GetAsync(string.Format(DefaultValues.FMI_URL, searchLocation));

            // Parse initial response
            var document = await _parser.ParseAsync(await idResponse.Content.ReadAsStringAsync());

            var daytimeCss = ".first-mobile-forecast-time-step-content .celestial-status-text";
            var daytime = document.QuerySelector(daytimeCss)?.TextContent.Trim();

            var statusCss = ".first-mobile-forecast-time-step-content div.smartsymbol";
            var statusElement = document.QuerySelector(statusCss);
            var iconId = Regex.Match(statusElement.OuterHtml, @"code-(\d+)").Groups[1].Value;
            var weatherStatus = statusElement.GetAttribute("title");

            var id = document.QuerySelector("#observation-station-menu option").GetAttribute("value");
            var weatherResponse = await _http.GetAsync(DefaultValues.FMI_TEMP_URL + id);

            // Parse weather response
            dynamic weatherInfo = JObject.Parse(await weatherResponse.Content.ReadAsStringAsync());
            var temperature = weatherInfo.t2m.Last[1];
            var wind = weatherInfo.WindSpeedMS?.Last[1];
            int? humidity = weatherInfo.Humidity?.Last[1];
            long timeStamp = weatherInfo.latestObservationTime / 1000;
            float? minTemp = weatherInfo.MinimumTemperature?.Last[1];
            float? maxTemp = weatherInfo.MaximumTemperature?.Last[1];
            int? snowDepth = weatherInfo.SnowDepth?.Last[1];
            var date = timeStamp.ToDateTime(TimeZoneInfo.Local);

            return CreateEmbedWeatherMessage(searchLocation, temperature, "FI", weatherStatus, wind, date,
                string.Format(DefaultValues.FMI_WEATHER_ICON_URL, iconId), daytime, humidity, minTemp, maxTemp,
                snowDepth);
        }

        public Embed CreateEmbedWeatherMessage(string location, object temp, string country, string status,
            object wind, DateTime timestamp, string iconUrl = null, string daytime = null, int? humidity = null,
            float? minTemp = null, float? maxTemp = null, int? snowDepth = null)
        {
            var embed = new EmbedBuilder();

            embed.AddField("🌡️ Temperature", $"{temp}°C", inline: true);
            embed.AddField("🌤️ Status", status, inline: true);

            if (wind != null) embed.AddField("💨 Wind", $"{wind} m/s", inline: true);
            if (humidity != null) embed.AddField("💦 Humidity", $"{humidity} %", inline: true);
            if (minTemp != null && maxTemp != null) embed.AddField("🌡️ Min/Max", $"{minTemp}°C - {maxTemp} °C", inline: true);
            if (snowDepth != null && snowDepth != -1) embed.AddField("⛄ Snow depth", $"{snowDepth} cm", inline: true);
            if (daytime != null) embed.AddField("🌅 Daytime", daytime, inline: true);

            return embed.WithTitle($"{location}, {country}")
                .WithColor(BotSettings.Instance.EmbeddedColor)
                .WithTimestamp(timestamp)
                .WithThumbnailUrl(iconUrl)
                .Build();
        }
    }
}

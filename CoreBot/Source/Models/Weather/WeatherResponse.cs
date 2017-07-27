using Newtonsoft.Json;

namespace CoreBot.Models.Weather
{
    /// <summary>
    /// Base class for deserializing JSON, used by <see cref="CoreBot.Services.WeatherService"/>.
    /// Root object is <see cref="TopLevel"/>.
    /// </summary>
    public class WeatherResponse
    {
        public class Coord
        {
            [JsonProperty("lat")]
            public double Lat { get; set; }

            [JsonProperty("lon")]
            public double Lon { get; set; }
        }

        public class Weather
        {
            [JsonProperty("icon")]
            public string Icon { get; set; }

            [JsonProperty("description")]
            public string Description { get; set; }

            [JsonProperty("id")]
            public double Id { get; set; }

            [JsonProperty("main")]
            public string Main { get; set; }
        }

        public class Main
        {
            [JsonProperty("pressure")]
            public double Pressure { get; set; }

            [JsonProperty("temp_max")]
            public double TempMax { get; set; }

            [JsonProperty("humidity")]
            public double Humidity { get; set; }

            [JsonProperty("temp")]
            public double Temp { get; set; }

            [JsonProperty("temp_min")]
            public double TempMin { get; set; }
        }

        public class Wind
        {
            [JsonProperty("deg")]
            public double Deg { get; set; }

            [JsonProperty("speed")]
            public double Speed { get; set; }
        }

        public class Clouds
        {
            [JsonProperty("all")]
            public double All { get; set; }
        }

        public class Sys
        {
            [JsonProperty("message")]
            public double Message { get; set; }

            [JsonProperty("sunset")]
            public double Sunset { get; set; }

            [JsonProperty("country")]
            public string Country { get; set; }

            [JsonProperty("id")]
            public double Id { get; set; }

            [JsonProperty("sunrise")]
            public double Sunrise { get; set; }

            [JsonProperty("type")]
            public double Type { get; set; }
        }

        public class TopLevel
        {
            [JsonProperty("coord")]
            public Coord Coord { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("clouds")]
            public Clouds Clouds { get; set; }

            [JsonProperty("base")]
            public string Base { get; set; }

            [JsonProperty("cod")]
            public double Cod { get; set; }

            [JsonProperty("id")]
            public double Id { get; set; }

            [JsonProperty("dt")]
            public long Dt { get; set; }

            [JsonProperty("main")]
            public Main Main { get; set; }

            [JsonProperty("visibility")]
            public double Visibility { get; set; }

            [JsonProperty("sys")]
            public Sys Sys { get; set; }

            [JsonProperty("weather")]
            public Weather[] Weather { get; set; }

            [JsonProperty("wind")]
            public Wind Wind { get; set; }
        }
    }
}
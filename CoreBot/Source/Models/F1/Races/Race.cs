using System;
using Newtonsoft.Json;

namespace CoreBot.Models.F1.Races
{
    public class Race
    {
        [JsonProperty("season")]
        public string Season { get; set; }

        [JsonProperty("round")]
        public string Round { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("raceName")]
        public string RaceName { get; set; }

        [JsonProperty("Circuit")]
        public Circuit Circuit { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("time")]
        public DateTime Time { get; set; }
    }
}

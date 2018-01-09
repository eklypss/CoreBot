using Newtonsoft.Json;

namespace CoreBot.Models.NHL
{
    public class LeagueRecord
    {
        [JsonProperty("wins")]
        public long Wins { get; set; }

        [JsonProperty("losses")]
        public long Losses { get; set; }

        [JsonProperty("ot")]
        public long Ot { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}

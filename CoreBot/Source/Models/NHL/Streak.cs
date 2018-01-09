using Newtonsoft.Json;

namespace CoreBot.Models.NHL
{
    public class Streak
    {
        [JsonProperty("streakType")]
        public string StreakType { get; set; }

        [JsonProperty("streakNumber")]
        public long StreakNumber { get; set; }

        [JsonProperty("streakCode")]
        public string StreakCode { get; set; }
    }
}

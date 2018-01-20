using Newtonsoft.Json;

namespace CoreBot.Models.F1.Races
{
    public class RaceRoot
    {
        [JsonProperty("MRData")]
        public RaceData RaceData { get; set; }
    }
}

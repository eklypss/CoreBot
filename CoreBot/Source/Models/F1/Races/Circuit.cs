using Newtonsoft.Json;

namespace CoreBot.Models.F1.Races
{
    public class Circuit
    {
        [JsonProperty("circuitId")]
        public string CircuitId { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("circuitName")]
        public string CircuitName { get; set; }

        [JsonProperty("Location")]
        public Location Location { get; set; }
    }
}

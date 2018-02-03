using Newtonsoft.Json;

namespace CoreBot.Models.Wolfram
{
    public class State
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("input")]
        public string Input { get; set; }
    }
}

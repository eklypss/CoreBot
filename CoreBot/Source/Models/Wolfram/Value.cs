using Newtonsoft.Json;

namespace CoreBot.Models.Wolfram
{
    public class Value
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("desc")]
        public string Desc { get; set; }

        [JsonProperty("input")]
        public string Input { get; set; }
    }
}

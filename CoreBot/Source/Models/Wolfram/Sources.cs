using Newtonsoft.Json;

namespace CoreBot.Models.Wolfram
{
    public class Sources
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }
}

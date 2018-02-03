using Newtonsoft.Json;

namespace CoreBot.Models.Wolfram
{
    public class Subpod
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("img")]
        public Img Img { get; set; }

        [JsonProperty("plaintext")]
        public string PlainText { get; set; }
    }
}

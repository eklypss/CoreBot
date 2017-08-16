using Newtonsoft.Json;

namespace CoreBot.Models.Urban
{
    public class Definition
    {
        [JsonProperty("definition")]
        public string Description { get; set; }

        [JsonProperty("current_vote")]
        public string CurrentVote { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("defid")]
        public double Defid { get; set; }

        [JsonProperty("permalink")]
        public string Permalink { get; set; }

        [JsonProperty("thumbs_up")]
        public double ThumbsUp { get; set; }

        [JsonProperty("example")]
        public string Example { get; set; }

        [JsonProperty("thumbs_down")]
        public double ThumbsDown { get; set; }

        [JsonProperty("word")]
        public string Word { get; set; }
    }
}
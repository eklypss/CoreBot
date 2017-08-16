using System.Collections.Generic;
using Newtonsoft.Json;

namespace CoreBot.Models.Urban
{
    public class UrbanResponse
    {
        [JsonProperty("result_type")]
        public string ResultType { get; set; }

        [JsonProperty("list")]
        public List<Definition> Definitions { get; set; }

        [JsonProperty("sounds")]
        public List<string> Sounds { get; set; }

        [JsonProperty("tags")]
        public List<string> Tags { get; set; }
    }
}
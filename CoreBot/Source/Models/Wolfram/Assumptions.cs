using System.Collections.Generic;
using Newtonsoft.Json;

namespace CoreBot.Models.Wolfram
{
    public class Assumptions
    {
        [JsonProperty("type")]
        public string PurpleType { get; set; }

        [JsonProperty("template")]
        public string Template { get; set; }

        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("values")]
        public List<Value> Values { get; set; }
    }
}

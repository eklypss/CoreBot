using System.Collections.Generic;
using Newtonsoft.Json;

namespace CoreBot.Models.Wolfram
{
    public class Pod
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("scanner")]
        public string Scanner { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("position")]
        public long Position { get; set; }

        [JsonProperty("error")]
        public bool Error { get; set; }

        [JsonProperty("numsubpods")]
        public long Numsubpods { get; set; }

        [JsonProperty("subpods")]
        public List<Subpod> Subpods { get; set; }

        [JsonProperty("states")]
        public List<State> States { get; set; }
    }
}

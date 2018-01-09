using System.Collections.Generic;
using Newtonsoft.Json;

namespace CoreBot.Models.NHL
{
    public class NHLRoot
    {
        [JsonProperty("copyright")]
        public string Copyright { get; set; }

        [JsonProperty("records")]
        public List<Record> Records { get; set; }
    }
}

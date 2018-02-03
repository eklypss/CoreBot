using System.Collections.Generic;
using Newtonsoft.Json;

namespace CoreBot.Models.Wolfram
{
    public class QueryResult
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("error")]
        public bool Error { get; set; }

        [JsonProperty("numpods")]
        public int Numpods { get; set; }

        [JsonProperty("datatypes")]
        public string DataTypes { get; set; }

        [JsonProperty("timedout")]
        public string Timedout { get; set; }

        [JsonProperty("timedoutpods")]
        public string TimedoutPods { get; set; }

        [JsonProperty("timing")]
        public double Timing { get; set; }

        [JsonProperty("parsetiming")]
        public double ParseTiming { get; set; }

        [JsonProperty("parsetimedout")]
        public bool ParseTimedout { get; set; }

        [JsonProperty("recalculate")]
        public string Recalculate { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("host")]
        public string Host { get; set; }

        [JsonProperty("server")]
        public string Server { get; set; }

        [JsonProperty("related")]
        public string Related { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("pods")]
        public IList<Pod> Pods { get; set; }

        [JsonProperty("sources")]
        public Sources Sources { get; set; }

        [JsonProperty("source")]
        public Sources Sources2 { set { Sources = value; } }
    }
}

using Newtonsoft.Json;

namespace CoreBot.Models.Wolfram
{
    public class WolframData
    {
        [JsonProperty("queryresult")]
        public QueryResult QueryResult { get; set; }
    }
}

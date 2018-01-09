using System.Collections.Generic;
using Newtonsoft.Json;

namespace CoreBot.Models.NHL
{
    public class Record
    {
        [JsonProperty("standingsType")]
        public string StandingsType { get; set; }

        [JsonProperty("league")]
        public Conference League { get; set; }

        [JsonProperty("division")]
        public Conference Division { get; set; }

        [JsonProperty("conference")]
        public Conference Conference { get; set; }

        [JsonProperty("teamRecords")]
        public List<TeamRecord> TeamRecords { get; set; }
    }
}

using System.Collections.Generic;
using Newtonsoft.Json;

namespace CoreBot.Models.F1.Races
{
    public class RaceTable
    {
        [JsonProperty("season")]
        public string Season { get; set; }

        [JsonProperty("Races")]
        public List<Race> Races { get; set; }
    }
}

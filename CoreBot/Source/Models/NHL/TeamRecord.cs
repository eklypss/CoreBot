using System;
using Newtonsoft.Json;

namespace CoreBot.Models.NHL
{
    public class TeamRecord
    {
        [JsonProperty("team")]
        public Conference Team { get; set; }

        [JsonProperty("leagueRecord")]
        public LeagueRecord LeagueRecord { get; set; }

        [JsonProperty("goalsAgainst")]
        public long GoalsAgainst { get; set; }

        [JsonProperty("goalsScored")]
        public long GoalsScored { get; set; }

        [JsonProperty("points")]
        public long Points { get; set; }

        [JsonProperty("divisionRank")]
        public string DivisionRank { get; set; }

        [JsonProperty("conferenceRank")]
        public string ConferenceRank { get; set; }

        [JsonProperty("leagueRank")]
        public string LeagueRank { get; set; }

        [JsonProperty("wildCardRank")]
        public string WildCardRank { get; set; }

        [JsonProperty("row")]
        public long Row { get; set; }

        [JsonProperty("gamesPlayed")]
        public long GamesPlayed { get; set; }

        [JsonProperty("streak")]
        public Streak Streak { get; set; }

        [JsonProperty("lastUpdated")]
        public DateTime LastUpdated { get; set; }
    }
}

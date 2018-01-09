using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreBot.Services;
using Discord.Commands;

namespace CoreBot.Source.Modules
{
    public class NHLModule : ModuleBase
    {
        private readonly NHLService _nhlService;

        public NHLModule(NHLService nhlService)
        {
            _nhlService = nhlService;
        }

        [Command("standings")]
        public async Task GetStandingsAsync()
        {
            var standings = await _nhlService.GetStandingsAsync();
            var standing = new List<string>();

            foreach (var conference in standings.Records)
            {
                conference.TeamRecords.Sort((a, b) => b.Points.CompareTo(a.Points));
                standing.Add($"**{conference.Conference.Name} {conference.Division.Name}**");

                foreach (var team in conference.TeamRecords)
                {
                    standing.Add($"{team.Team.Name} **GP**: {team.GamesPlayed} **W:** {team.LeagueRecord.Wins} **L:** {team.LeagueRecord.Losses} **OT:** {team.LeagueRecord.Ot} **GF:** {team.GoalsScored} **GA:** {team.GoalsAgainst} **DIFF:** {team.GoalsScored - team.GoalsAgainst} **TP:** {team.Points}");
                }
                await ReplyAsync(string.Join(Environment.NewLine, standing));
            }
        }
    }
}

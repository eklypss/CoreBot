using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreBot.Services;
using CoreBot.Settings;
using Discord;
using Discord.Commands;

namespace CoreBot.Modules
{
    public class NHLModule : ModuleBase
    {
        private readonly NHLService _nhlService;

        public NHLModule(NHLService nhlService)
        {
            _nhlService = nhlService;
        }

        [Command("standings"), Summary("Gets the current standings of the NHL.")]
        public async Task GetStandingsAsync()
        {
            var standings = await _nhlService.GetStandingsAsync();
            foreach (var conference in standings.Records)
            {
                var embed = new EmbedBuilder().WithColor(BotSettings.Instance.EmbeddedColor);
                var standingStringList = new List<string>();
                conference.TeamRecords.Sort((a, b) => b.Points.CompareTo(a.Points));
                foreach (var team in conference.TeamRecords)
                {
                    standingStringList.Add($"{team.Team.Name} **GP**: {team.GamesPlayed} **W:** {team.LeagueRecord.Wins} **L:** {team.LeagueRecord.Losses} **OT:** {team.LeagueRecord.Ot} **GF:** {team.GoalsScored} **GA:** {team.GoalsAgainst} **DIFF:** {team.GoalsScored - team.GoalsAgainst} **TP:** {team.Points}");
                }
                embed.AddField($"**{conference.Conference.Name} {conference.Division.Name}**", string.Join(Environment.NewLine, standingStringList));
                await ReplyAsync(string.Empty, embed: embed.Build());
            }
        }
    }
}

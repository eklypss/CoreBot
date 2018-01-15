using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using epnetcore;
using epnetcore.Helpers;

namespace CoreBot.Modules
{
    public class EPStatsModule : ModuleBase
    {
        private readonly EPClient _client;

        public EPStatsModule(EPClient client)
        {
            _client = client;
        }

        [Command("contract"), Summary("Displays contract summary of the given player.")]
        public async Task GetPlayerContractInfoAsync([Remainder] string playerName)
        {
            int id;

            try
            {
                id = await _client.GetPlayerIdAsync(playerName);
            }
            catch (NullReferenceException)
            {
                await ReplyAsync("no contract found for " + playerName);
                return;
            }

            var stats = await _client.GetPlayerStatsAsync(id);
            var data = stats.Data.Find(x => x.Season.EndYear == 2017 || x.Season.EndYear == 2018);

            if (data != null)
            {
                if (data.Player.Caphit == null || string.IsNullOrEmpty(data.Player.Caphit)) await ReplyAsync($"**Contract:** {data.Player.Contract} **Team:** {data.Team.Name}");
                else await ReplyAsync($"**Contract:** {data.Player.Contract} **Cap hit:** {data.Player.Caphit} **Team:** {data.Team.Name}");
            }
            else await ReplyAsync("no contract found for current season");
        }

        [Command("scoring")]
        public async Task GetPlayerStatsAsync()
        {
            var scoring = await _client.GetTopScorers(7);
            var scoringList = new List<string>();
            foreach (var scorer in scoring.Data)
            {
                scoringList.Add($"**{scorer.Player.FirstName} {scorer.Player.LastName}** ({scorer.Team.Name}) **GP:** {scorer.GP} **G:** {scorer.G} **A:** {scorer.A} **TP:** {scorer.TP} **PPG:** {scorer.PPG} **+/-:** {scorer.PM} **PIM:** {scorer.PIM}");
            }
            await ReplyAsync(string.Join(Environment.NewLine, scoringList));
        }

        [Command("stats"), Summary("Displays stats of the given player.")]
        public async Task GetPlayerStatsAsync([Remainder] string playerName)
        {
            int id;

            try
            {
                id = await _client.GetPlayerIdAsync(playerName);
            }
            catch (NullReferenceException)
            {
                await ReplyAsync("no stats found for " + playerName);
                return;
            }

            var stats = await _client.GetPlayerStatsAsync(id);
            var data = stats.Data.FindAll(x => x.Season.EndYear == 2018);

            if (data.Count < 1)
            {
                await ReplyAsync("no stats for this season for " + playerName);
                return;
            }

            var statsList = new List<string>();
            var season = data.FirstOrDefault();
            var player = season.Player;
            statsList.Add($"**Player:** {player.FirstName} {player.LastName}  **Season:** {season.Season.StartYear}-{season.Season.EndYear} **DoB:** {player.DateOfBirth} **Country:** {player.Country.Name} **Height:** {player.Height} cm **Weight:** {player.Weight} kg");
            foreach (var team in data)
            {
                string gameType = string.Empty;
                switch (team.GameType)
                {
                    case "REGULAR_SEASON": gameType = "Regular season"; break;
                    case "PLAYOFFS": gameType = "Playoffs"; break;
                    default: gameType = team.GameType; break;
                }
                if (player.PlayerPosition == "GOALIE")
                {
                    statsList.Add($"**Team:** {team.Team.Name} ({gameType}) **GP:** {team.GP} **SVP:** {team.SVP} **GAA:** {team.GAA}");
                }
                else
                {
                    statsList.Add($"**Team:** {team.Team.Name} ({gameType}) **GP:** {team.GP} **G:** {team.G} **A:** {team.A} **TP:** {team.TP} **PPG:** {team.PPG} **+/-:** {team.PM} **PIM:** {team.PIM}");
                }
            }
            await ReplyAsync($"{string.Join(Environment.NewLine, statsList)}");
        }
    }
}

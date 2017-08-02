using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using epnetcore;

namespace CoreBot.Modules
{
    public class EPStatsModule : ModuleBase
    {
        private readonly EPClient _client;

        public EPStatsModule(EPClient client)
        {
            _client = client;
        }

        [Command("contract")]
        public async Task GetPlayerContract([Remainder] string playerName)
        {
            var id = await _client.GetPlayerIdAsync(playerName);
            var stats = await _client.GetPlayerStats(id);
            var data = stats.Data.Find(x => x.Season.EndYear == 2017 || x.Season.EndYear == 2018);

            if (data != null)
            {
                if (data.Player.Caphit == null || string.IsNullOrEmpty(data.Player.Caphit)) await ReplyAsync($"**Contract:** {data.Player.Contract} **Team:** {data.Team.Name}");
                else await ReplyAsync($"**Contract:** {data.Player.Contract} **Cap hit:** {data.Player.Caphit} **Team:** {data.Team.Name}");
            }
            else await ReplyAsync("Player not found.");
        }

        [Command("stats")]
        public async Task GetPlayerStats([Remainder] string playerName)
        {
            var id = await _client.GetPlayerIdAsync(playerName);
            var stats = await _client.GetPlayerStats(id);
            var data = stats.Data.FindAll(x => x.Season.EndYear == 2017);

            if (data != null)
            {
                var season = data.FirstOrDefault();
                var player = season.Player;
                await ReplyAsync($"**Player:** {player.FirstName} {player.LastName}  **Season:** {season.Season.StartYear}-{season.Season.EndYear} **DoB:** {player.DateOfBirth} **Country:** {player.Country.Name} **Height:** {player.Height} cm **Weight:** {player.Weight} kg");
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
                        await ReplyAsync($"**Team:** {team.Team.Name} ({gameType}) **GP:** {team.GP} **SVP:** {team.SVP} **GAA:** {team.GAA}");
                    }
                    else
                    {
                        await ReplyAsync($"**Team:** {team.Team.Name} ({gameType}) **GP:** {team.GP} **G:** {team.G} **A:** {team.A} **TP:** {team.TP} **PPG:** {team.PPG} **+/-:** {team.PM} **PIM:** {team.PIM}");
                    }
                }
            }
        }
    }
}
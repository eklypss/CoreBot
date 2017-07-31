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
            var data = stats.Data.Find(x => x.Season.EndYear == 2017);

            if (stats != null)
            {
                if (data.Player.Caphit == null || string.IsNullOrEmpty(data.Player.Caphit)) await ReplyAsync($"**Contract:** {data.Player.Contract} **Team:** {data.Team.Name}");
                else await ReplyAsync($"**Contract:** {data.Player.Contract} **Cap hit:** {data.Player.Caphit} **Team:** {data.Team.Name}");
            }
            else await ReplyAsync("Player not found.");
        }
    }
}
using System;
using System.Linq;
using System.Threading.Tasks;
using CoreBot.Settings;
using Discord;
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
                await ReplyAsync($"No contract found for {playerName}.");
                return;
            }

            var stats = await _client.GetPlayerStatsAsync(id);
            var data = stats.Data.Find(x => x.Season.EndYear == (DateTime.Now.Year - 1) || x.Season.EndYear == DateTime.Now.Year);

            if (data != null)
            {
                if (data.Player.Caphit == null || string.IsNullOrEmpty(data.Player.Caphit)) await ReplyAsync($"**Contract:** {data.Player.Contract} **Team:** {data.Team.Name}");
                else await ReplyAsync($"**Contract:** {data.Player.Contract} **Cap hit:** {data.Player.Caphit} **Team:** {data.Team.Name}");
            }
            else await ReplyAsync("No contract found for current season.");
        }

        [Command("scoring"), Summary("Displays current top 10 scoring in total points.")]
        public async Task GetTopScoringAsync()
        {
            var scoring = await _client.GetTopScorersAsync(7);
            var embed = new EmbedBuilder().WithColor(BotSettings.Instance.EmbeddedColor);
            int rank = 1;
            foreach (var scorer in scoring.Data)
            {
                embed.AddField($"#{rank} {scorer.Player.FirstName} {scorer.Player.LastName} ({scorer.Team.Name})", $"**GP:** {scorer.GP} **G:** {scorer.G} **A:** {scorer.A} **TP:** {scorer.TP} **PPG:** {scorer.PPG} **+/-:** {scorer.PM} **PIM:** {scorer.PIM}");
                rank++;
            }
            await ReplyAsync(string.Empty, embed: embed.Build());
        }

        [Command("goals"), Summary("Displays current top 10 scoring in goals.")]
        [Alias("goal")]
        public async Task Get()
        {
            var scoring = await _client.GetTopGoalsAsync(7);
            var embed = new EmbedBuilder().WithColor(BotSettings.Instance.EmbeddedColor);
            int rank = 1;
            foreach (var scorer in scoring.Data)
            {
                embed.AddField($"#{rank} {scorer.Player.FirstName} {scorer.Player.LastName} ({scorer.Team.Name})", $"**GP:** {scorer.GP} **G:** {scorer.G} **A:** {scorer.A} **TP:** {scorer.TP} **PPG:** {scorer.PPG} **+/-:** {scorer.PM} **PIM:** {scorer.PIM}");
                rank++;
            }
            await ReplyAsync(string.Empty, embed: embed.Build());
        }

        [Command("svp"), Summary("Displays current top 10 scoring in SVP.")]
        [Alias("svs", "save", "saves")]
        public async Task GetPlayerStatsAsync()
        {
            var scoring = await _client.GetTopSVPAsync(7);
            var embed = new EmbedBuilder().WithColor(BotSettings.Instance.EmbeddedColor);
            int rank = 1;
            foreach (var scorer in scoring.Data)
            {
                embed.AddField($"#{rank} {scorer.Player.FirstName} {scorer.Player.LastName} ({scorer.Team.Name})", $"**GP:** {scorer.GP} **SVP:** {scorer.SVP} **GAA:** {scorer.GAA}");
                rank++;
            }
            await ReplyAsync(string.Empty, embed: embed.Build());
        }

        [Command("stats"), Summary("Displays stats of the given player.")]
        [Alias("sts")]
        public async Task GetPlayerStatsAsync([Remainder] string playerName)
        {
            int id;

            try
            {
                id = await _client.GetPlayerIdAsync(playerName);
            }
            catch (NullReferenceException)
            {
                await ReplyAsync($"No stats found for {playerName}.");
                return;
            }

            var stats = await _client.GetPlayerStatsAsync(id);
            stats.Data.Sort((a, b) => b.Season.EndYear.CompareTo(a.Season.EndYear));
            var data = stats.Data.FindAll(x => x.Season.EndYear == stats.Data.FirstOrDefault().Season.EndYear);
            if (data.Count < 1)
            {
                await ReplyAsync($"No stats found for {playerName}.");
                return;
            }

            var embed = new EmbedBuilder();
            var season = data.FirstOrDefault();
            var player = season.Player;
            embed.WithTitle($"{player.FirstName} {player.LastName}");
            embed.WithDescription($"**DoB:** {player.DateOfBirth} **Country:** {player.Country.Name} **Height:** {player.Height} cm **Weight:** {player.Weight} kg");
            embed.AddField("Latest season", $"{season.Season.StartYear}-{season.Season.EndYear}");
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
                    embed.AddField($"{team.Team.Name} ({gameType})", $"**GP:** {team.GP} **SVP:** {team.SVP} **GAA:** {team.GAA}");
                }
                else
                {
                    embed.AddField($"{team.Team.Name} ({gameType})", $"**GP:** {team.GP} **G:** {team.G} **A:** {team.A} **TP:** {team.TP} **PPG:** {team.PPG} **+/-:** {team.PM} **PIM:** {team.PIM}");
                }
                embed.WithTimestamp(DateTime.Parse(season.Updated));
                embed.WithUrl(string.Format(DefaultValues.EP_PLAYERSTATS_URL, player.Id));
                embed.WithColor(BotSettings.Instance.EmbeddedColor);
                if (!string.IsNullOrEmpty(player.ImageUrl)) embed.WithThumbnailUrl(string.Format(DefaultValues.EP_PLAYERIMAGE_URL, player.ImageUrl));
            }
            await ReplyAsync(string.Empty, embed: embed.Build());
        }
    }
}

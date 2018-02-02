using System;
using System.Linq;
using System.Threading.Tasks;
using CoreBot.Settings;
using Discord;
using Discord.Commands;
using Humanizer;

namespace CoreBot.Modules
{
    [Group("server")]
    public class ServerModule : ModuleBase
    {
        [Command("info")]
        public async Task GetServerInfo()
        {
            var users = await Context.Guild.GetUsersAsync();
            var onlineUsers = users.Where(x => x.Status != UserStatus.Offline).ToList();
            var embed = new EmbedBuilder()
            .AddField("Server name", Context.Guild.Name)
            .AddField("# of emotes", Context.Guild.Emotes.Count)
            .AddField("# of users", users.Count)
            .AddField("Users online", onlineUsers.Count)
            .AddField("Created", $"{DateTime.Now.Subtract(Context.Guild.CreatedAt.DateTime).Humanize(maxUnit: BotSettings.Instance.HumanizerMaxUnit, precision: BotSettings.Instance.HumanizerPrecision)} ago")
            .WithColor(BotSettings.Instance.EmbeddedColor)
            .WithThumbnailUrl(Context.Guild.IconUrl)
            .Build();
            await ReplyAsync(string.Empty, embed: embed);
        }
    }
}

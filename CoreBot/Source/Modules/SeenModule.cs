using System;
using System.Linq;
using System.Threading.Tasks;
using CoreBot.Settings;
using Discord;
using Discord.Commands;
using Humanizer;

namespace CoreBot.Modules
{
    public class SeenModule : ModuleBase
    {
        [Command("seen"), Summary("Shows latest activity of the specified user.")]
        [Alias("s")]
        public async Task GetUserLastSeenInfoAsync(string userName)
        {
            var found = await Context.Channel.GetMessagesAsync(2000)
                .FirstOrDefault(batch => batch
                .Any(message => message.Author.Username.Equals(userName, StringComparison.InvariantCultureIgnoreCase)));
            if (found != null)
            {
                var msg = found.First(m => m.Author.Username.Equals(userName, StringComparison.InvariantCultureIgnoreCase));

                var embed = new EmbedBuilder()
               .WithDescription(msg.Content)
               .WithTimestamp(msg.Timestamp)
               .WithColor(BotSettings.Instance.EmbeddedColor)
               .WithAuthor(msg.Author)
               .Build();
                await ReplyAsync(string.Empty, embed: embed);
            }
            else
            {
                await ReplyAsync($"No messages from \"{userName}\"");
            }
        }
    }
}

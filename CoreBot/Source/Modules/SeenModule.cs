using System;
using System.Linq;
using System.Threading.Tasks;
using CoreBot.Settings;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace CoreBot.Modules
{
    public class SeenModule : ModuleBase
    {
        [Command("seen"), Summary("Shows latest activity of the specified user.")]
        [Alias("s")]
        public async Task GetUserLastSeenInfoAsync([Remainder] string userName)
        {
            var msg = await Context.Channel.GetMessagesAsync(10000)
                .Flatten()
                .FirstOrDefaultAsync(message =>
                {
                    var author = (SocketGuildUser)message.Author;
                    return author.Username.Equals(userName, StringComparison.InvariantCultureIgnoreCase)
                           || author.Nickname != null
                           && author.Nickname.Equals(userName, StringComparison.InvariantCultureIgnoreCase);
                });
            if (msg != null)
            {
                var sender = (SocketGuildUser)msg.Author;

                var embed = new EmbedBuilder()
                   .WithDescription(msg.Content)
                   .WithTimestamp(msg.Timestamp)
                   .WithColor(BotSettings.Instance.EmbeddedColor)
                   .WithAuthor($"{sender.Nickname} ({sender})")
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

using System;
using System.Linq;
using System.Threading.Tasks;
using CoreBot.Settings;
using Discord.Commands;
using Humanizer;

namespace CoreBot.Modules
{
    public class SeenModule : ModuleBase
    {
        [Command("seen"), Summary("Shows latest activity of the specified user.")]
        public async Task GetUserLastSeenInfoAsync(string userName)
        {
            var found = await Context.Channel.GetMessagesAsync(2000)
                .FirstOrDefault(batch => batch
                .Any(message => message.Author.Username.Equals(userName, StringComparison.InvariantCultureIgnoreCase)));
            if (found != null)
            {
                var msg = found.First(m => m.Author.Username.Equals(userName, StringComparison.InvariantCultureIgnoreCase));
                await ReplyAsync($"{msg.Author.Username} was last seen {DateTime.Now.Subtract(msg.Timestamp.DateTime).Humanize(BotSettings.Instance.HumanizerPrecision)} ago saying: `{msg.Content}`");
            }
            else
            {
                await ReplyAsync($"No messages from \"{userName}\"");
            }
        }
    }
}
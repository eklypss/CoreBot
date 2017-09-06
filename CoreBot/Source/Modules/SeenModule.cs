using System;
using System.Linq;
using System.Threading.Tasks;
using CoreBot.Helpers;
using CoreBot.Settings;
using Discord;
using Discord.Commands;
using Humanizer;

namespace CoreBot.Modules
{
    public class SeenModule : ModuleBase
    {
        [Command("seen"), Summary("Shows latest activity of the specified user.")]
        public async Task GetUserLastSeenInfoAsync(string name)
        {
            IUser user;

            try
            {
                user = await Context.Channel.FindUserByNameAsync(name);
            }
            catch (InvalidOperationException)
            {
                await ReplyAsync($"No user with nick or username '{name}'");
                return;
            }

            IMessage msg;

            try
            {
                msg = await LatestMessageAsync(user);
            }
            catch (InvalidOperationException)
            {
                await ReplyAsync($"No message from '{name}' in 2000 messages");
                return;
            }

            var timeDiff = DateTime.UtcNow - msg.Timestamp;

            await ReplyAsync($"{name} was last seen " +
                $"{timeDiff.Humanize(BotSettings.Instance.HumanizerPrecision)} " +
                $"ago saying: `{msg.Content}`");
        }

        /// <summary>
        /// Find last message for user
        /// </summary>
        /// <param name="user">user to search for</param>
        /// <exception cref="InvalidOperationException">
        /// User hasn't sent message in last 2000 messages
        /// </exception>
        private async Task<IMessage> LatestMessageAsync(IUser user)
        {
            var foundBatch = await Context.Channel.GetMessagesAsync(2000)
                .First(batch => batch.Any(message =>
                (
                    message.Author.Id == user.Id
                )));

            return foundBatch.First(message => message.Author.Id == user.Id);
        }
    }
}
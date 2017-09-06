using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace CoreBot.Helpers
{
    public static class HelperMethods
    {
        /// <summary>
        /// Converts Unix timestamp to DateTime.
        /// </summary>
        /// <param name="unixTime">unix timestamp in seconds</param>
        public static DateTime ToDateTime(this long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }

        /// <summary>
        /// Find user by nickname with username fallback. Case-insensitive.
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="nick"></param>
        /// <exception cref="InvalidOperationException">No user found</exception>
        public async static Task<IUser> FindUserByNameAsync(this IMessageChannel channel,
            string nick)
        {
            var u = await channel.GetUsersAsync().Flatten();
            var users = u.Cast<SocketGuildUser>();

            var lowerNick = nick.ToLower();

            try
            {
                return users.First(sender => sender.Nickname.ToLower() == lowerNick);
            }
            catch (InvalidOperationException)
            {
                // Throw if not found even with username
                return users.First(sender => sender.Username.ToLower() == lowerNick);
            }
        }
    }
}
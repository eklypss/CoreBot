using System.Net.Http;
using System.Threading.Tasks;
using CoreBot.Settings;
using Discord.Commands;

namespace CoreBot.Modules
{
    public class FollowAgeModule : ModuleBase
    {
        private static readonly HttpClient _http = new HttpClient();

        [Command("followage"), Summary("Displays how long the specified user has followed the specified Twitch channel.")]
        public async Task GetFollowAgeAsync(string user, string channel)
        {
            var result = await _http.GetAsync(string.Format(DefaultValues.FOLLOWAGE_URL, user, channel));
            await ReplyAsync(await result.Content.ReadAsStringAsync());
        }
    }
}

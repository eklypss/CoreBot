using System.Net.Http;
using System.Threading.Tasks;
using CoreBot.Settings;
using Discord.Commands;

namespace CoreBot.Modules
{
    public class FollowAgeModule : ModuleBase
    {
        [Command("followage"), Summary("Displays how long the specified user has followed the specified Twitch channel.")]
        public async Task GetFollowAge(string user, string channel)
        {
            using (var http = new HttpClient())
            {
                var result = await http.GetAsync(string.Format(DefaultValues.FOLLOWAGE_URL, user, channel));
                await ReplyAsync(await result.Content.ReadAsStringAsync());
            }
        }
    }
}
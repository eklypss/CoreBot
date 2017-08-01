using System.Net.Http;
using System.Threading.Tasks;
using CoreBot.Settings;
using Discord.Commands;

namespace CoreBot.Modules
{
    public class FollowAgeModule : ModuleBase
    {
        [Command("followage")]
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
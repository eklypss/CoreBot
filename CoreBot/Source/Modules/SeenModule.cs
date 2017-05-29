using System.Linq;
using System.Threading.Tasks;
using CoreBot.Collections;
using Discord.Commands;
using Serilog;

namespace CoreBot.Modules
{
    public class SeenModule : ModuleBase
    {
        [Command("seen"), Summary("Shows latest activity of the specified user.")]
        public async Task Seen(string userName)
        {
            var message = UserMessages.Instance.Messages.Where(x => x.User == userName).FirstOrDefault();
            if (message == null)
            {
                Log.Warning($"No actiivty was found for the user: {userName}.");
                await ReplyAsync($"I haven't seen {userName} here.");
            }
            else
            {
                await ReplyAsync($"{message.User} was last seen on {message.DateTime.ToString()} saying: {message.Message}");
            }
        }
    }
}
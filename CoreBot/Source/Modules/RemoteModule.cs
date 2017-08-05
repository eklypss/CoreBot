using System.Linq;
using System.Threading.Tasks;
using CoreBot.Settings;
using Discord.Commands;

namespace CoreBot.Modules
{
    public class RemoteModule : ModuleBase
    {
        [Command("remote")]
        public async Task SendRemoteMessage([Remainder] string message)
        {
            var guilds = await Context.Client.GetGuildsAsync();
            var textChannels = await guilds.FirstOrDefault(x => x.Name == BotSettings.Instance.DefaultGuild).GetTextChannelsAsync();
            var channel = textChannels.FirstOrDefault(x => x.Name == BotSettings.Instance.DefaultChannel);
            await channel.SendMessageAsync(message);
        }
    }
}
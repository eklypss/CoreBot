using System.Linq;
using System.Threading.Tasks;
using CoreBot.Interfaces;
using CoreBot.Settings;
using Discord;

namespace CoreBot.Services
{
    public class MessageService : IMessageService
    {
        private readonly IDiscordClient _client;

        public MessageService(IDiscordClient client)
        {
            _client = client;
        }

        public async Task SendMessageToDefaultChannelAsync(string message)
        {
            var guilds = await _client.GetGuildsAsync();
            var chans = await guilds.FirstOrDefault(x => x.Name == BotSettings.Instance.DefaultGuild).GetTextChannelsAsync();
            var channel = chans.FirstOrDefault(x => x.Name == BotSettings.Instance.DefaultChannel);
            await channel.SendMessageAsync(message);
        }
    }
}
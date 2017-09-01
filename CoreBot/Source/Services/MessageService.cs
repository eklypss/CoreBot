using System;
using System.Linq;
using System.Threading.Tasks;
using CoreBot.Interfaces;
using CoreBot.Settings;
using Discord;
using Serilog;

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
            try
            {
                var guilds = await _client.GetGuildsAsync();
                var chans = await guilds
                    .First(x => x.Name == BotSettings.Instance.DefaultGuild)
                    .GetTextChannelsAsync();

                var channel = chans.First(x => x.Name == BotSettings.Instance.DefaultChannel);
                await channel.SendMessageAsync(message);
            }
            catch (NullReferenceException)
            {
                Log.Error($"Could not send message to default channel since it has not been set or was not found.");
            }
            catch (Exception)
            {
                Log.Error($"couldn't send message because channel '{BotSettings.Instance.DefaultChannel}' at '{BotSettings.Instance.DefaultGuild}' not found");
            }
        }
    }
}
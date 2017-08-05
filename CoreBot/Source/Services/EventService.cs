using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoreBot.Collections;
using CoreBot.Managers;
using CoreBot.Settings;
using Discord.WebSocket;
using Serilog;

namespace CoreBot.Services
{
    public class EventService
    {
        private readonly Timer _timer;

        public EventService(DiscordSocketClient client, EventManager eventManager)
        {
            _timer = new Timer(_ =>
            {
                foreach (var eve in Events.Instance.EventsList)
                {
                    if (eve.DateTime.Subtract(DateTime.Now).TotalSeconds <= 0)
                    {
                        Log.Information($"Event ID {eve.Id} completed.");
                        Task.Run(async () => await client.Guilds.FirstOrDefault(x => x.Name == BotSettings.Instance.DefaultGuild).TextChannels.FirstOrDefault(x => x.Name == BotSettings.Instance.DefaultChannel).
                        SendMessageAsync($"@everyone **Event:** {eve.Description}"));
                        Task.Run(async () => await eventManager.DeleteEventAsync(eve));
                    }
                }
            },
            null,
            TimeSpan.FromSeconds(5),
            TimeSpan.FromSeconds(5));
        }
    }
}
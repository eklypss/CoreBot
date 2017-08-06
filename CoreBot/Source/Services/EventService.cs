using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoreBot.Collections;
using CoreBot.Managers;
using CoreBot.Models;
using CoreBot.Settings;
using Discord.WebSocket;
using Humanizer;
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
                if (DateTime.Now.Hour == 00 && DateTime.Now.Minute == 00 && DateTime.Now.Second == 00)
                {
                    var eventList = new List<Event>();
                    var messageList = new List<string>();
                    messageList.Add($"**Day changed to {DateTime.Now.ToString("dd-MM-yyyy")}**");
                    foreach (var ev in Events.Instance.EventsList)
                    {
                        if (ev.DateTime.Subtract(DateTime.Now).TotalHours < 24)
                        {
                            eventList.Add(ev);
                        }
                    }

                    if (eventList.Count > 0)
                    {
                        messageList.Add("**Events today:**");
                        foreach (var eve in eventList)
                        {
                            var remainder = eve.DateTime.Subtract(DateTime.Now);
                            messageList.Add($"{eve.Description} (id: {eve.Id}), **time left:** {remainder.Humanize(2)}");
                        }
                        Task.Run(async () => await client.Guilds.FirstOrDefault(x => x.Name == BotSettings.Instance.DefaultGuild).TextChannels.FirstOrDefault(x => x.Name == BotSettings.Instance.DefaultChannel).
                        SendMessageAsync($"{string.Join(Environment.NewLine, messageList)}"));
                    }
                }
                foreach (var eve in Events.Instance.EventsList)
                {
                    if (eve.DateTime.Subtract(DateTime.Now).TotalSeconds <= 0)
                    {
                        Log.Information($"Event ID {eve.Id} completed.");
                        Task.Run(async () => await client.Guilds.FirstOrDefault(x => x.Name == BotSettings.Instance.DefaultGuild).TextChannels.FirstOrDefault(x => x.Name == BotSettings.Instance.DefaultChannel).
                        SendMessageAsync($"@everyone **{eve.Description}**"));
                        Task.Run(async () => await eventManager.DeleteEventAsync(eve));
                    }
                }
            },
            null,
            TimeSpan.FromSeconds(5),
            TimeSpan.FromSeconds(1));
        }
    }
}
using System;
using System.Linq;
using System.Threading.Tasks;
using CoreBot.Collections;
using CoreBot.Managers;
using CoreBot.Models;
using CoreBot.Settings;
using Discord;
using FluentScheduler;
using Serilog;

namespace CoreBot.Services
{
    public class EventService : Registry
    {
        private readonly IDiscordClient _client;
        private readonly EventManager _eventManager;

        public EventService(IDiscordClient client, EventManager eventManager)
        {
            _client = client;
            _eventManager = eventManager;

            Task.Run(async () => await InitAsync());
        }

        public async Task InitAsync()
        {
            await CompleteOutdatedEventsAsync();
            await SchedulePreviousEventsAsync();
        }

        public async Task CreateEventAsync(string msg, DateTime date)
        {
            Log.Information($"Creating event: {msg}, to happen at {date.ToString()}.");
            var eve = new Event() { Message = msg, Date = date, Completed = false };
            await ScheduleEventAsync(eve);
            await _eventManager.SaveEventAsync(eve);
        }

        public async Task ScheduleEventAsync(Event eve)
        {
            Log.Information($"Scheduling event:  {eve.Message}.");
            JobManager.AddJob(async () => await CompleteEventAsync(eve), (s) => s.ToRunOnceAt(eve.Date));
        }

        public async Task CompleteEventAsync(Event eve)
        {
            var guilds = await _client.GetGuildsAsync();
            var chans = await guilds.FirstOrDefault(x => x.Name == BotSettings.Instance.DefaultGuild).GetTextChannelsAsync();
            var channel = chans.FirstOrDefault(x => x.Name == BotSettings.Instance.DefaultChannel);
            await channel.SendMessageAsync(eve.Message);
            await _eventManager.CompleteEventAsync(eve);
        }

        public async Task CompleteOutdatedEventsAsync()
        {
            foreach (var eve in Events.Instance.EventsList.FindAll(x => !x.Completed))
            {
                var remainder = eve.Date.Subtract(DateTime.Now);
                if (remainder.TotalSeconds < 0)
                {
                    Log.Information($"Completing outdated event: {eve.Message} (id: {eve.ID}).");
                    await _eventManager.CompleteEventAsync(eve);
                }
            }
        }

        public async Task SchedulePreviousEventsAsync()
        {
            foreach (var eve in Events.Instance.EventsList.FindAll(x => !x.Completed))
            {
                await ScheduleEventAsync(eve);
            }
            Log.Information($"Scheduled {Events.Instance.EventsList.FindAll(x => !x.Completed).Count} previous events.");
        }
    }
}
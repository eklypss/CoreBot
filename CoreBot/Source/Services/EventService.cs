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

        public async Task ScheduleEventAsync(string msg, DateTime date)
        {
            var eve = new Event() { Message = msg, Date = date, Completed = false };
            Log.Information($"Scheduling job:  {eve.Message}");
            JobManager.AddJob(async () => await CompleteEventAsync(eve), (s) => s.ToRunOnceAt(date));
            await _eventManager.SaveEventAsync(eve);
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
            foreach (var eve in Events.Instance.EventsList.Where(x => !x.Completed).ToList())
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
            foreach (var eve in Events.Instance.EventsList.Where(x => !x.Completed).ToList())
            {
                await ScheduleEventAsync(eve.Message, eve.Date);
            }
        }
    }
}
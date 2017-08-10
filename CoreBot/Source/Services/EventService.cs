using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreBot.Collections;
using CoreBot.Database.Dao;
using CoreBot.Models;
using FluentScheduler;
using Humanizer;
using Serilog;

namespace CoreBot.Services
{
    public class EventService : Registry
    {
        private readonly MessageService _messageService;
        private readonly EventDao _eventDao;

        public EventService(MessageService messageService, EventDao eventDao)
        {
            _messageService = messageService;
            _eventDao = eventDao;

            Task.Run(async () => await InitAsync());
        }

        public async Task InitAsync()
        {
            await CompleteOutdatedEventsAsync();
            await SchedulePreviousEventsAsync();
            JobManager.AddJob(async () => await DisplayEventsDailyAsync(), (s) => s.ToRunEvery(1).Days().At(00, 00));
        }

        private async Task DisplayEventsDailyAsync()
        {
            var eventList = new List<string>();
            if (Events.Instance.EventsList.FindAll(x => !x.Completed).Count > 0) eventList.Add($"Day changed to {DateTime.Now.DayOfWeek.ToString()}, {DateTime.Now.Date.ToString("dd-MM-yyyy")}. Events today:");
            else eventList.Add($"Day changed to {DateTime.Now.DayOfWeek.ToString()}, {DateTime.Now.Date.ToString("dd-MM-yyyy")}. No events today.");
            foreach (var eve in Events.Instance.EventsList.FindAll(x => !x.Completed))
            {
                var remainder = eve.Date.Subtract(DateTime.Now);
                if (remainder.TotalHours < 24)
                {
                    eventList.Add($"{eve.Message}, **time left:** {remainder.Humanize(2)}.");
                }
            }
            await _messageService.SendMessageToDefaultChannelAsync(string.Join(Environment.NewLine, eventList));
        }

        public async Task CreateEventAsync(string msg, DateTime date)
        {
            Log.Information($"Creating event: {msg}, to happen at {date.ToString()}.");
            var eve = new Event { Message = msg, Date = date, Completed = false };
            await ScheduleEventAsync(eve);
            await _eventDao.SaveEventAsync(eve);
        }

        public async Task ScheduleEventAsync(Event eve)
        {
            Log.Information($"Scheduling event:  {eve.Message}.");
            JobManager.AddJob(async () => await CompleteEventAsync(eve), (s) => s.ToRunOnceAt(eve.Date));
        }

        public async Task CompleteEventAsync(Event eve)
        {
            await _messageService.SendMessageToDefaultChannelAsync(eve.Message);
            await _eventDao.CompleteEventAsync(eve);
        }

        public async Task CompleteOutdatedEventsAsync()
        {
            foreach (var eve in Events.Instance.EventsList.FindAll(x => !x.Completed))
            {
                var remainder = eve.Date.Subtract(DateTime.Now);
                if (remainder.TotalSeconds < 0)
                {
                    Log.Information($"Completing outdated event: {eve.Message} (id: {eve.Id}).");
                    await _eventDao.CompleteEventAsync(eve);
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
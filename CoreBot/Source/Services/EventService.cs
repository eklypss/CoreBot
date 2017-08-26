using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using CoreBot.Collections;
using CoreBot.Database.Dao;
using CoreBot.Interfaces;
using CoreBot.Models;
using CoreBot.Settings;
using FluentScheduler;
using Humanizer;
using Serilog;

namespace CoreBot.Services
{
    public class EventService : Registry, IEventService
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
            SchedulePreviousEvents();
            JobManager.AddJob(async () => await DisplayEventsDailyAsync(), (s) => s.ToRunEvery(1).Days().At(00, 00));
        }

        public async Task DisplayEventsDailyAsync()
        {
            var eventList = new List<string>();
            if (Events.Instance.EventsList.FindAll(x => !x.Completed && x.Date.Date == DateTime.Today.Date).Count > 0) eventList.Add($"Day changed to {DateTime.Now.DayOfWeek.ToString()}, {DateTime.Now.Date.ToString("dd-MM-yyyy")}. Events today:");
            else eventList.Add($"Day changed to {DateTime.Now.DayOfWeek.ToString()}, {DateTime.Now.Date.ToString(BotSettings.Instance.DateFormat)}. No events today.");
            foreach (var eve in Events.Instance.EventsList.FindAll(x => !x.Completed))
            {
                var remainder = eve.Date.Subtract(DateTime.Now);
                if (remainder.TotalHours < 24)
                {
                    eventList.Add($"{eve.Message}, **time left:** {remainder.Humanize(3)}.");
                }
            }
            await _messageService.SendMessageToDefaultChannelAsync(string.Join(Environment.NewLine, eventList));
        }

        public async Task CreateEventAsync(string msg, DateTime date)
        {
            Log.Information($"Creating event: {msg}, to happen at {date.ToString(BotSettings.Instance.DateTimeFormat, new CultureInfo(BotSettings.Instance.DateTimeCulture))}.");
            var eve = new Event { Message = msg, Date = date, Completed = false };
            ScheduleEvent(eve);
            await _eventDao.SaveEventAsync(eve);
        }

        public void ScheduleEvent(Event eve)
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

        public void SchedulePreviousEvents()
        {
            foreach (var eve in Events.Instance.EventsList.FindAll(x => !x.Completed))
            {
                ScheduleEvent(eve);
            }
            Log.Information($"Scheduled {Events.Instance.EventsList.FindAll(x => !x.Completed).Count} previous events.");
        }
    }
}
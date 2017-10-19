using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CoreBot.Collections;
using CoreBot.Services;
using CoreBot.Settings;
using Discord.Commands;
using Humanizer;
using Serilog;

namespace CoreBot.Modules
{
    [Group("event"), Summary("Module for adding, viewing and editing events.")]
    [Alias("events")]
    public class EventModule : ModuleBase
    {
        private readonly EventService _eventService;

        public EventModule(EventService eventService)
        {
            _eventService = eventService;
        }

        [Command("add"), Summary("Adds a new event.")]
        public async Task AddEventAsync(string date, string time, [Remainder] string message)
        {
            if (Context.User.IsBot)
            {
                Log.Warning($"Bot '{Context.User.Username}' tried to add an event, prevented.");
                await ReplyAsync("Bots cannot add events.");
            }
            else
            {
                DateTime eventDate;
                if (date == "today") date = DateTime.Now.ToString(BotSettings.Instance.DateFormat);
                if (date == "tomorrow") date = DateTime.Now.AddDays(1).ToString(BotSettings.Instance.DateFormat);
                if (DateTime.TryParse(string.Format("{0} {1}", date, time), new CultureInfo(BotSettings.Instance.DateTimeCulture), DateTimeStyles.AssumeLocal, out eventDate))
                {
                    var remainder = eventDate.Subtract(DateTime.Now);
                    if (remainder.TotalSeconds > 0)
                    {
                        await _eventService.CreateEventAsync(message, eventDate);
                        Log.Information($"Event added: {message}, **time left:** {remainder.Humanize(BotSettings.Instance.HumanizerPrecision)}.");
                        await ReplyAsync($"Event added: {message}, **time left:** {remainder.Humanize(BotSettings.Instance.HumanizerPrecision)}.");
                    }
                    else await ReplyAsync("Invalid date.");
                }
                else await ReplyAsync("Invalid date.");
            }
        }

        [Command("list"), Summary("Lists all uncompleted events.")]
        public async Task ListEventsAsync()
        {
            if (Events.Instance.EventsList.Any(x => !x.Completed))
            {
                var list = new List<string>();
                Events.Instance.EventsList.Sort((a, b) => a.Date.CompareTo(b.Date));
                foreach (var eve in Events.Instance.EventsList.Where(x => !x.Completed))
                {
                    var remainder = eve.Date.Subtract(DateTime.Now);
                    list.Add($"{eve.Message} (id: {eve.Id}), **time left:** {remainder.Humanize(BotSettings.Instance.HumanizerPrecision)}.");
                }
                await ReplyAsync($"{string.Join(Environment.NewLine, list)}");
            }
            else await ReplyAsync("No events to list.");
        }

        [Command("today"), Summary("Lists all uncompleted events that are occuring today.")]
        public async Task GetTodaysEventsAsync()
        {
            if (Events.Instance.EventsList.FindAll(x => !x.Completed && x.Date.Date == DateTime.Now.Date).Count > 0)
            {
                var list = new List<string>();
                Events.Instance.EventsList.Sort((a, b) => a.Date.CompareTo(b.Date));
                foreach (var eve in Events.Instance.EventsList.FindAll(x => !x.Completed && x.Date.Date == DateTime.Now.Date))
                {
                    var remainder = eve.Date.Subtract(DateTime.Now);
                    list.Add($"{eve.Message} at **{eve.Date.TimeOfDay}** (id: {eve.Id}), **time left:** {remainder.Humanize(BotSettings.Instance.HumanizerPrecision)}.");
                }
                await ReplyAsync($"{string.Join(Environment.NewLine, list)}");
            }
            else await ReplyAsync("No events today.");
        }

        [Command("tomorrow"), Summary("Lists all uncompleted events that are occuring tomorrow.")]
        public async Task GetTomorrowsEventsAsync()
        {
            if (Events.Instance.EventsList.FindAll(x => !x.Completed && x.Date.Date == DateTime.Now.AddDays(1).Date).Count > 0)
            {
                var list = new List<string>();
                Events.Instance.EventsList.Sort((a, b) => a.Date.CompareTo(b.Date));
                foreach (var eve in Events.Instance.EventsList.FindAll(x => !x.Completed && x.Date.Date == DateTime.Now.AddDays(1).Date))
                {
                    var remainder = eve.Date.Subtract(DateTime.Now);
                    list.Add($"{eve.Message} at **{eve.Date.TimeOfDay}** (id: {eve.Id}), **time left:** {remainder.Humanize(BotSettings.Instance.HumanizerPrecision)}.");
                }
                await ReplyAsync($"{string.Join(Environment.NewLine, list)}");
            }
            else await ReplyAsync("No events tomorrow.");
        }

        [Command("next"), Summary("Displays the next event.")]
        public async Task GetNextEventAsync()
        {
            if (Events.Instance.EventsList.FindAll(x => !x.Completed).Count > 0)
            {
                Events.Instance.EventsList.Sort((a, b) => a.Date.CompareTo(b.Date));
                var nextEvent = Events.Instance.EventsList.FirstOrDefault(x => !x.Completed);
                var remainder = nextEvent.Date.Subtract(DateTime.Now);
                await ReplyAsync($"Next event: {nextEvent.Message} (id: {nextEvent.Id}), **time left:** {remainder.Humanize(BotSettings.Instance.HumanizerPrecision)}.");
            }
            else await ReplyAsync("No events to list.");
        }

        [Command("find"), Summary("Lists all uncompleted events that contain the given string.")]
        public async Task FindEventsAsync([Remainder] string searchTerm)
        {
            if (Events.Instance.EventsList.FindAll(x => !x.Completed && x.Message.ToLower().Contains(searchTerm.ToLower())).Count > 0)
            {
                var list = new List<string>();
                Events.Instance.EventsList.Sort((a, b) => a.Date.CompareTo(b.Date));
                foreach (var eve in Events.Instance.EventsList.FindAll(x => !x.Completed && x.Message.ToLower().Contains(searchTerm.ToLower())))
                {
                    var remainder = eve.Date.Subtract(DateTime.Now);
                    list.Add($"{eve.Message} (id: {eve.Id}), **time left:** {remainder.Humanize(BotSettings.Instance.HumanizerPrecision)}.");
                }
                await ReplyAsync($"{string.Join(Environment.NewLine, list)}");
            }
            else await ReplyAsync("No events found.");
        }

        [Command("complete"), Summary("Completes/removes an uncompleted event, marking it as completed.")]
        [Alias("delete", "del")]
        public async Task CompleteEventAsync(int id)
        {
            var eve = Events.Instance.EventsList.FirstOrDefault(x => x.Id == id);
            if (eve != null)
            {
                await _eventService.CompleteEventAsync(eve);
            }
            else await ReplyAsync("Event not found.");
        }

        [Command("info"), Summary("Shows basic information about the specified event.")]
        [Alias("details", "id")]
        public async Task ShowEventDetailsAsync(int id)
        {
            var eve = Events.Instance.EventsList.FirstOrDefault(x => x.Id == id);
            if (eve != null)
            {
                if (eve.Completed) await ReplyAsync($"**Event ID:** {eve.Id} {Environment.NewLine}**Message:** {eve.Message} {Environment.NewLine}**Date:** {eve.Date.ToString(BotSettings.Instance.DateTimeFormat, new CultureInfo(BotSettings.Instance.DateTimeCulture))} {Environment.NewLine}**Completed:** {eve.Completed} ({eve.Date.Subtract(DateTime.Now).Humanize(BotSettings.Instance.HumanizerPrecision)} ago)");
                else await ReplyAsync($"**Event ID:** {eve.Id} {Environment.NewLine}**Message:** {eve.Message} {Environment.NewLine}**Date:** {eve.Date.ToString(BotSettings.Instance.DateTimeFormat, new CultureInfo(BotSettings.Instance.DateTimeCulture))} {Environment.NewLine}**Completed:** {eve.Completed} {Environment.NewLine}**Time left:** {eve.Date.Subtract(DateTime.Now).Humanize(BotSettings.Instance.HumanizerPrecision)}.");
            }
            else await ReplyAsync("Event not found.");
        }
    }
}

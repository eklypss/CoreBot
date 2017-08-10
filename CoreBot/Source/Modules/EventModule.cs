using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreBot.Collections;
using CoreBot.Services;
using Discord.Commands;
using Humanizer;

namespace CoreBot.Modules
{
    [Group("event")]
    [Alias("events")]
    public class EventModule : ModuleBase
    {
        private readonly EventService _eventService;

        public EventModule(EventService eventService)
        {
            _eventService = eventService;
        }

        [Command("add")]
        public async Task AddEvent(string date, string time, [Remainder] string message)
        {
            DateTime eventDate;
            if (date == "today") date = DateTime.Now.ToString("dd-MM-yyyy");
            if (date == "tomorrow") date = DateTime.Now.AddDays(1).ToString("dd-MM-yyyy");
            if (DateTime.TryParse(string.Format("{0} {1}", date, time), out eventDate))
            {
                var remainder = eventDate.Subtract(DateTime.Now);
                if (remainder.TotalSeconds > 0)
                {
                    await ReplyAsync($"Event added: {message}, **time left:** {remainder.Humanize(2)}.");
                    await _eventService.CreateEventAsync(message, eventDate);
                }
                else await ReplyAsync("Invalid date.");
            }
            else await ReplyAsync("Invalid date.");
        }

        [Command("list")]
        public async Task ListEvents()
        {
            if (Events.Instance.EventsList.Any(x => !x.Completed))
            {
                var list = new List<string>();
                Events.Instance.EventsList.Sort((a, b) => a.Date.CompareTo(b.Date));
                foreach (var eve in Events.Instance.EventsList.Where(x => !x.Completed))
                {
                    var remainder = eve.Date.Subtract(DateTime.Now);
                    list.Add($"{eve.Message} (id: {eve.Id}), **time left:** {remainder.Humanize(2)}.");
                }
                await ReplyAsync($"{string.Join(Environment.NewLine, list)}");
            }
            else await ReplyAsync("No events to list.");
        }

        [Command("today")]
        public async Task GetTodaysEvents()
        {
            if (Events.Instance.EventsList.FindAll(x => x.Date.Date == DateTime.Now.Date).Count > 0)
            {
                var list = new List<string>();
                Events.Instance.EventsList.Sort((a, b) => a.Date.CompareTo(b.Date));
                foreach (var eve in Events.Instance.EventsList.FindAll(x => x.Date.Date == DateTime.Now.Date))
                {
                    var remainder = eve.Date.Subtract(DateTime.Now);
                    list.Add($"{eve.Message} (id: {eve.Id}), **time left:** {remainder.Humanize(2)}.");
                }
                await ReplyAsync($"{string.Join(Environment.NewLine, list)}");
            }
            else await ReplyAsync("No events to list.");
        }

        [Command("tomorrow")]
        public async Task GetTomorrowsEvents()
        {
            if (Events.Instance.EventsList.FindAll(x => x.Date.Date == DateTime.Now.AddDays(1).Date).Count > 0)
            {
                var list = new List<string>();
                Events.Instance.EventsList.Sort((a, b) => a.Date.CompareTo(b.Date));
                foreach (var eve in Events.Instance.EventsList.FindAll(x => x.Date.Date == DateTime.Now.AddDays(1).Date))
                {
                    var remainder = eve.Date.Subtract(DateTime.Now);
                    list.Add($"{eve.Message} (id: {eve.Id}), **time left:** {remainder.Humanize(2)}.");
                }
                await ReplyAsync($"{string.Join(Environment.NewLine, list)}");
            }
            else await ReplyAsync("No events to list.");
        }

        [Command("complete")]
        [Alias("delete")]
        public async Task CompleteEvent(int id)
        {
            var eve = Events.Instance.EventsList.FirstOrDefault(x => x.Id == id);
            if (eve != null)
            {
                await _eventService.CompleteEventAsync(eve);
            }
            else await ReplyAsync("Event not found.");
        }
    }
}
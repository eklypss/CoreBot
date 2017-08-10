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
            if (DateTime.TryParse(string.Format("{0} {1}", date, time), out eventDate))
            {
                var remainder = eventDate.Subtract(DateTime.Now);
                if (remainder.TotalSeconds > 0)
                {
                    await ReplyAsync($"Event added: {message}, **time left:** {remainder.Humanize(2)}.");
                    await _eventService.ScheduleEvent(message, eventDate);
                }
                else await ReplyAsync("Invalid date.");
            }
            else await ReplyAsync("Invalid date.");
        }

        [Command("list")]
        public async Task ListEvents()
        {
            var list = new List<string>();

            Events.Instance.EventsList.Sort((a, b) => a.Date.CompareTo(b.Date));
            foreach (var eve in Events.Instance.EventsList.Where(x => !x.Completed))
            {
                var remainder = eve.Date.Subtract(DateTime.Now);
                list.Add($"{eve.Message} (id: {eve.ID}), **time left:** {remainder.Humanize(2)}.");
            }
            await ReplyAsync($"{string.Join(Environment.NewLine, list)}");
        }
    }
}
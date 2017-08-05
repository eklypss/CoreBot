using System;
using System.Threading.Tasks;
using CoreBot.Collections;
using CoreBot.Managers;
using CoreBot.Models;
using Discord.Commands;

namespace CoreBot.Modules
{
    [Group("event")]
    public class EventModule : ModuleBase
    {
        private readonly EventManager _eventManager;

        public EventModule(EventManager eventManager)
        {
            _eventManager = eventManager;
        }

        [Command("add")]
        public async Task AddEvent(string date, string time, [Remainder] string description)
        {
            DateTime eventDate;
            DateTime.TryParse(string.Format("{0} {1}", date, time), out eventDate);
            var remainder = eventDate.Subtract(DateTime.Now);
            if (remainder.TotalSeconds > 0)
            {
                await _eventManager.AddEventAsync(new Event(description, eventDate));
                await ReplyAsync($"Event **{description}** added, **time left:** {remainder.Days} days, {remainder.Hours} hours, {remainder.Minutes} minutes, {remainder.Seconds} seconds.");
            }
            else await ReplyAsync("Invalid date.");
        }

        [Command("list")]
        public async Task ListEvents()
        {
            foreach (var eve in Events.Instance.EventsList)
            {
                var remainder = eve.DateTime.Subtract(DateTime.Now);
                await ReplyAsync($"**Event:** {eve.Description} (id: {eve.Id}), **time left:** {remainder.Days} days, {remainder.Hours} hours, {remainder.Minutes} minutes, {remainder.Seconds} seconds.");
            }
        }

        [Command("delete")]
        [Alias("del")]
        public async Task DeleteEvent(int id)
        {
            var eve = Events.Instance.EventsList.Find(x => x.Id == id);
            if (eve != null)
            {
                await _eventManager.DeleteEventAsync(eve);
                await ReplyAsync($"Event **{eve.Description}** deleted.");
            }
            else await ReplyAsync("Event not found.");
        }
    }
}
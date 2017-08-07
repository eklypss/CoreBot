using System;
using System.Threading.Tasks;
using CoreBot.Collections;
using CoreBot.Helpers;
using CoreBot.Models;
using Serilog;
using ServiceStack.OrmLite;

namespace CoreBot.Managers
{
    public class EventManager
    {
        public EventManager()
        {
            Task.Run(async () => await CheckEvents());
        }

        public async Task AddEventAsync(Event eve)
        {
            using (var connection = Database.Open())
            {
                await connection.InsertAsync(eve);
                Events.Instance.EventsList.Clear();
                Events.Instance.EventsList = await connection.SelectAsync<Event>();
            }
        }

        public async Task DeleteEventAsync(Event eve)
        {
            using (var connection = Database.Open())
            {
                await connection.DeleteAsync(eve);
            }
        }

        public async Task UpdateEventAsync(Event eve, bool completed)
        {
            using (var connection = Database.Open())
            {
                eve.Completed = completed;
                await connection.UpdateAsync(eve);
            }
        }

        public async Task CheckEvents()
        {
            Log.Information("Checking events for expired events.");
            foreach (var eve in Events.Instance.EventsList)
            {
                if (eve.DateTime.Subtract(DateTime.Now).TotalSeconds <= 0)
                {
                    Log.Warning($"Removed event: {eve.Description} because it was expired.");
                    await DeleteEventAsync(eve);
                }
            }
        }
    }
}
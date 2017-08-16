using System;
using System.Threading.Tasks;
using CoreBot.Models;

namespace CoreBot.Interfaces
{
    public interface IEventService
    {
        Task InitAsync();

        Task DisplayEventsDailyAsync();

        Task CreateEventAsync(string msg, DateTime date);

        Task ScheduleEventAsync(Event eve);

        Task CompleteEventAsync(Event eve);

        Task CompleteOutdatedEventsAsync();

        Task SchedulePreviousEventsAsync();
    }
}
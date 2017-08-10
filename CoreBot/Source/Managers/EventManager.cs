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
        public async Task SaveEvent(Event eve)
        {
            using (var connection = Database.Open())
            {
                eve.ID = (int)await connection.InsertAsync(eve, selectIdentity: true);
                Events.Instance.EventsList.Add(eve);
                Log.Information($"Event: {eve.Message} (id: {eve.ID}) saved.");
            }
        }

        public async Task CompleteEvent(Event eve)
        {
            using (var connection = Database.Open())
            {
                eve.Completed = true;
                await connection.UpdateAsync(eve);
                Log.Information($"Event: {eve.Message} (id: {eve.ID}) completed.");
            }
        }
    }
}
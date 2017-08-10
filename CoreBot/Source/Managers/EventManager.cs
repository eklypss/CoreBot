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
        public async Task SaveEventAsync(Event eve)
        {
            using (var connection = Database.Open())
            {
                eve.Id = (int)await connection.InsertAsync(eve, selectIdentity: true);
                Events.Instance.EventsList.Add(eve);
                Log.Information($"Event: {eve.Message} (id: {eve.Id}) saved.");
            }
        }

        public async Task CompleteEventAsync(Event eve)
        {
            using (var connection = Database.Open())
            {
                eve.Completed = true;
                await connection.UpdateAsync(eve);
                Log.Information($"Event: {eve.Message} (id: {eve.Id}) completed.");
            }
        }
    }
}
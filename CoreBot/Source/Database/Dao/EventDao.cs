using System.Threading.Tasks;
using CoreBot.Collections;
using CoreBot.Models;
using Serilog;
using ServiceStack.OrmLite;

namespace CoreBot.Database.Dao
{
    public class EventDao
    {
        public async Task SaveEventAsync(Event eve)
        {
            using (var connection = DbConnection.Open())
            {
                eve.Id = (int)await connection.InsertAsync(eve, selectIdentity: true);
                Events.Instance.EventsList.Add(eve);
                Log.Information($"Event: {eve.Message} (id: {eve.Id}) saved.");
            }
        }

        public async Task CompleteEventAsync(Event eve)
        {
            using (var connection = DbConnection.Open())
            {
                eve.Completed = true;
                await connection.UpdateAsync(eve);
                Log.Information($"Event: {eve.Message} (id: {eve.Id}) completed.");
            }
        }
    }
}
using System.Threading.Tasks;
using CoreBot.Collections;
using CoreBot.Models;
using Serilog;
using ServiceStack.OrmLite;

namespace CoreBot.Database.Dao
{
    public class EventDao
    {
        /// <summary>
        /// Saves the given <see cref="Event"/> to the database.
        /// </summary>
        /// <param name="eve">Event to save.</param>
        public async Task SaveEventAsync(Event eve)
        {
            using (var connection = DbConnection.Open())
            {
                eve.Id = (int)await connection.InsertAsync(eve, selectIdentity: true);
                Events.Instance.EventsList.Add(eve);
                Log.Information($"Event: {eve.Message} (id: {eve.Id}) saved.");
            }
        }

        /// <summary>
        /// Completes the given <see cref="Event"/>.
        /// </summary>
        /// <param name="eve">Event to complete.</param>
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
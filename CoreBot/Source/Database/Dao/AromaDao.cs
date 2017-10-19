using System.Collections.Generic;
using System.Threading.Tasks;
using CoreBot.Models;
using ServiceStack.OrmLite;

namespace CoreBot.Database.Dao
{
    public class AromaDao
    {
        public async Task<List<Aroma>> RandomAromas(int count)
        {
            using (var conn = DbConnection.Open())
            {
                var query = conn.From<Aroma>()
                    .OrderByRandom()
                    .Limit(count)
                    .Select();

                return await conn.SelectAsync(query);
            }
        }
    }
}

using System.Diagnostics;
using System.Threading.Tasks;
using CoreBot.Models;
using Serilog;
using ServiceStack.OrmLite;

namespace CoreBot.Database.Dao
{
    public class CommentDao
    {

        public async Task<IltasanomatComment> RandomCommentAsync()
        {
            using (var conn = DbConnection.Open())
            {
                var bench = Stopwatch.StartNew();

                var query = conn.From<IltasanomatComment>().OrderByRandom();
                var result = await conn.SingleAsync(query);

                Log.Debug($"comment fetch took {bench.ElapsedMilliseconds} ms");
                return result;
            }
        }
    }
}

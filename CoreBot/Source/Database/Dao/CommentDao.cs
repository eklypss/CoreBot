using System;
using System.Diagnostics;
using System.Threading.Tasks;
using CoreBot.Models;
using Serilog;
using ServiceStack.OrmLite;

namespace CoreBot.Database.Dao
{
    public class CommentDao
    {
        private readonly int _totalCount;
        private readonly Random _random;

        private CommentDao(int totalCount)
        {
            _totalCount = totalCount;
            _random = new Random();
        }

        public static async Task<CommentDao> Create()
        {
            using (var conn = DbConnection.Open())
            {
                var count = await conn.CountAsync<IltasanomatComment>();
                Log.Information($"Loaded {count} Ilta-Sanomat comments.");

                var maxQuery = conn.From<IltasanomatComment>()
                    .Select(c => c.Id)
                    .OrderByDescending(x => x.Id);

                int maxId = await conn.SingleAsync<int>(maxQuery);

                if (maxId != count - 1)
                {
                    Log.Error($"Comment consistency error, maxId: {maxId}, count: {count}.");
                }

                return new CommentDao((int)count);
            }
        }

        public async Task<IltasanomatComment> RandomCommentAsync()
        {
            using (var conn = DbConnection.Open())
            {
                var bench = Stopwatch.StartNew();
                int index = _random.Next(_totalCount);

                var randomComment = await conn.SingleAsync<IltasanomatComment>(x => x.Id == index);

                Log.Debug($"Comment fetch took {bench.ElapsedMilliseconds} milliseconds.");
                return randomComment;
            }
        }
    }
}

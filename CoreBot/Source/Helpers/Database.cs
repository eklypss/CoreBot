using System.Data;
using System.Threading.Tasks;
using CoreBot.Collections;
using CoreBot.Models;
using CoreBot.Settings;
using Serilog;
using ServiceStack.OrmLite;

namespace CoreBot.Helpers
{
    internal static class Database
    {
        private static IDbConnection connection;

        public async static Task Init()
        {
            var connectionFactory = new OrmLiteConnectionFactory(BotSettings.Instance.DatabaseString, SqliteDialect.Provider);
            using (var connection = connectionFactory.Open())
            {
                connection.CreateTableIfNotExists<Command>();
                Commands.Instance.CommandsList = await connection.SelectAsync<Command>();
                Log.Information($"Loaded {Commands.Instance.CommandsList.Count} commands from {BotSettings.Instance.DatabaseString}.");
            }
        }

        public static IDbConnection Run()
        {
            return connection;
        }
    }
}
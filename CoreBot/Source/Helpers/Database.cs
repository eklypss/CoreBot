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
        private static OrmLiteConnectionFactory _factory;

        public async static Task InitAsync()
        {
            _factory = new OrmLiteConnectionFactory(BotSettings.Instance.DatabaseString, SqliteDialect.Provider);
            using (var connection = Open())
            {
                connection.CreateTableIfNotExists<Command>();
                connection.CreateTableIfNotExists<Event>();
                connection.CreateTableIfNotExists<Link>();
                Commands.Instance.CommandsList = await connection.SelectAsync<Command>();
                Events.Instance.EventsList = await connection.SelectAsync<Event>();
                Log.Information($"Loaded {Commands.Instance.CommandsList.Count} commands from {BotSettings.Instance.DatabaseString}.");
                Log.Information($"Loaded {Events.Instance.EventsList.Count} events from {BotSettings.Instance.DatabaseString}.");

                long linkCount = await connection.CountAsync<Link>();
                Log.Information($"Loaded {linkCount} links from {BotSettings.Instance.DatabaseString}.");
            }
        }

        public static IDbConnection Open()
        {
            return _factory.Open();
        }
    }
}
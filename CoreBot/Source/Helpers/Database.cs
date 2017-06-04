using System.Data;
using CoreBot.Collections;
using CoreBot.Models;
using CoreBot.Settings;
using Serilog;
using ServiceStack.OrmLite;

namespace CoreBot.Source.Helpers
{
    internal static class Database
    {
        private static IDbConnection connection;

        public static void Init()
        {
            var connectionFactory = new OrmLiteConnectionFactory(BotSettings.Instance.DatabaseString, SqliteDialect.Provider);
            connection = connectionFactory.Open();
            connection.CreateTableIfNotExists<Command>();
            Commands.Instance.CommandsList = connection.Select<Command>();
            Log.Information($"Loaded {Commands.Instance.CommandsList.Count} commands from {BotSettings.Instance.DatabaseString}.");
        }

        public static IDbConnection Run()
        {
            return connection;
        }
    }
}
using CoreBot.Collections;
using CoreBot.Models;
using Serilog;
using ServiceStack.OrmLite;
using System.Data;

namespace CoreBot.Source.Helpers
{
    static class Database
    {
        private static string dbString = "kanta.db";

        private static IDbConnection connection;

        public static void Init()
        {
            var connectionFactory = new OrmLiteConnectionFactory(dbString, SqliteDialect.Provider);
            connection = connectionFactory.Open();
            connection.CreateTableIfNotExists<Command>();
            Commands.Instance.CommandsList = connection.Select<Command>();
            Log.Information($"Loaded {Commands.Instance.CommandsList.Count} commands from {dbString}.");
        }
        
        public static IDbConnection Run()
        {
            return connection;
        }
    }
}

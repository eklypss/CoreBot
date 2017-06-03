using CoreBot.Models;
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
        }
        
        public static IDbConnection Run()
        {
            return connection;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreBot.Helpers;
using CoreBot.Models;
using CoreBot.Settings;
using Microsoft.Data.Sqlite;
using Serilog;
using ServiceStack.OrmLite;

namespace CoreBot.Modules
{
    public class DrinkManager
    {
        private readonly List<int> drinkIds;
        private readonly Random random;

        public static async Task<DrinkManager> CreateAsync()
        {
            try
            {
                using (var connection = Database.Open())
                {
                    var drinks = await connection.SelectAsync<Drink>();
                    var ids = drinks.Select(d => d.Id).ToList();
                    Log.Information($"Loaded {ids.Count} drinks from the database.");
                    return new DrinkManager(ids);
                }
            }
            catch (SqliteException ex)
            {
                Log.Error($"Could not retrive drinks from the database: {ex.Source} {ex.Message}.");
                return null;
            }
        }

        private DrinkManager(List<int> drinkIds)
        {
            random = new Random();
            this.drinkIds = drinkIds;
        }

        public string RandomLink()
        {
            int randomId = drinkIds[random.Next(drinkIds.Count)];
            return string.Format(DefaultValues.ALKO_URL, randomId);
        }
    }
}
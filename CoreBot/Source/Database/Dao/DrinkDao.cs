using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreBot.Models;
using CoreBot.Settings;
using Microsoft.Data.Sqlite;
using Serilog;
using ServiceStack.OrmLite;

namespace CoreBot.Database.Dao
{
    public class DrinkDao
    {
        private readonly List<int> _drinkIds;
        private readonly Random _random;

        public static async Task<DrinkDao> CreateAsync()
        {
            try
            {
                using (var connection = DbConnection.Open())
                {
                    if (connection.TableExists<Drink>())
                    {
                        var drinks = await connection.SelectAsync<Drink>();
                        var ids = drinks.Select(d => d.Id).ToList();
                        Log.Information($"Loaded {ids.Count} drinks from the database.");
                        return new DrinkDao(ids);
                    }
                    else
                    {
                        connection.CreateTable<Drink>();
                        Log.Warning("Created drinks table because it didn't exist.");
                        return null;
                    }
                }
            }
            catch (SqliteException ex)
            {
                Log.Error($"Could not retrive drinks from the database: {ex.Source} {ex.Message}.");
                return null;
            }
        }

        private DrinkDao(List<int> drinkIds)
        {
            _random = new Random();
            _drinkIds = drinkIds;
        }

        public string RandomLink()
        {
            int randomId = _drinkIds[_random.Next(_drinkIds.Count)];
            return string.Format(DefaultValues.ALKO_URL, randomId);
        }
    }
}
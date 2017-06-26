﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreBot.Helpers;
using CoreBot.Source.Models;
using ServiceStack.OrmLite;

namespace CoreBot.Modules
{
    public class DrinkManager
    {
        private const string URL = "https://www.alko.fi/tuotteet/{0:D6}/";
        private List<int> drinkIds;
        private Random random;

        public static async Task<DrinkManager> Create()
        {
            var drinks = await Database.Run().SelectAsync<Drink>();
            var ids = drinks.Select(d => d.Id).ToList();
            return new DrinkManager(ids);
        }

        private DrinkManager(List<int> drinkIds)
        {
            random = new Random();
            this.drinkIds = drinkIds;
        }

        public string RandomLink()
        {
            int randomId = drinkIds[random.Next(drinkIds.Count)];
            return string.Format(URL, randomId);
        }
    }
}
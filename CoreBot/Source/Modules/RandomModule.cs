using System;
using System.Threading.Tasks;
using Discord.Commands;
using CoreBot.Settings;
using Humanizer;
using Humanizer.Localisation;
using Serilog;
using Discord;
using System.Collections.Generic;

namespace CoreBot.Modules
{
    public class RandomModule : ModuleBase
    {
        [Command("random"), Summary("Generates a random number between two given values.")]
        [Alias("rng", "rand", "rdm")]
        public async Task GetRandomNumberAsync(int firstNumber, int secondNumber)
        {
            var random = new Random();
            await ReplyAsync($"{random.Next(firstNumber, secondNumber + 1)}");
        }

        [Command("randomdate"), Summary("Returns a random date.")]
        public async Task GetRandomDate(int years = 10)
        {
            var currentDate = DateTime.Now;
            var random = new Random();
            int hours = years * 365 * 24;
            var randomDate = currentDate.AddHours(random.Next(0, hours));
            var remainder = randomDate.Subtract(DateTime.Now);
            await ReplyAsync($"Random date: {randomDate.ToString(BotSettings.Instance.DateFormat)} (in {remainder.Humanize(BotSettings.Instance.HumanizerPrecision, maxUnit: TimeUnit.Year)})");
        }

        [Command("randomuser"), Summary("Returns a random user from the guild.")]
        public async Task GetRandomUser()
        {
            var users = await Context.Guild.GetUsersAsync();
            // Create new list of users because the method above doesn't do it(?)
            var list = new List<IUser>();
            foreach (var user in users)
            {
                list.Add(user);
            }
            await ReplyAsync($"Random user: {list[new Random().Next(0, list.Count)]}");
        }
    }
}

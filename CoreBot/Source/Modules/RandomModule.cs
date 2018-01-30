using System;
using System.Threading.Tasks;
using Discord.Commands;
using CoreBot.Settings;
using Humanizer;
using Humanizer.Localisation;
using Discord;
using System.Collections.Generic;
using System.Linq;

namespace CoreBot.Modules
{
    public class RandomModule : ModuleBase
    {
        private readonly Random _random;

        public RandomModule()
        {
            _random = new Random();
        }

        [Command("random"), Summary("Generates a random number between two given values.")]
        [Alias("rng", "rand", "rdm")]
        public async Task GetRandomNumberAsync(int firstNumber, int secondNumber)
        {
            await ReplyAsync($"{_random.Next(firstNumber, secondNumber + 1)}");
        }

        [Command("randomdate"), Summary("Returns a random date.")]
        public async Task GetRandomDateAsync(int years = 10)
        {
            var currentDate = DateTime.Now;
            int hours = years * 365 * 24;
            var randomDate = currentDate.AddHours(_random.Next(hours));
            var remainder = randomDate.Subtract(DateTime.Now);
            await ReplyAsync($"Random date: {randomDate.ToString(BotSettings.Instance.DateFormat)} (in {remainder.Humanize(BotSettings.Instance.HumanizerPrecision, maxUnit: TimeUnit.Year)})");
        }

        [Command("randomuser"), Summary("Returns a random user from the guild.")]
        public async Task GetRandomUserAsync()
        {
            var users = await Context.Guild.GetUsersAsync();
            // Create new list of users because the method above doesn't do it(?)
            var list = new List<IUser>();
            foreach (var user in users)
            {
                list.Add(user);
            }
            await ReplyAsync($"Random user: {list[_random.Next(list.Count)]}");
        }

        [Command("randomchoice"), Summary("Return random choice from space-separated list")]
        [Alias("choice", "rngchoice")]
        public async Task GetRandomChoiceAsync([Remainder] string message)
        {
            var components = message.Split(" ");
            var selection = components[_random.Next(components.Length)];

            await ReplyAsync(selection);
        }

        [Command("multichoice"), Summary("Return multiple random choices from space-separated list")]
        public async Task GetRandomMultiChoiceAsync(int choices, [Remainder] string message)
        {
            var components = message.Split(" ");
            if (components.Length < choices || choices <= 0)
            {
                await ReplyAsync("Not enough choices.");
                return;
            }
            List<string> selections = new List<string>();
            foreach (var i in Enumerable.Range(0, choices))
            {
                selections.Add(components[_random.Next(components.Length)]);
            }
            await ReplyAsync(string.Join(" ", selections));
        }
    }
}

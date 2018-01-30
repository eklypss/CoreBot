using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreBot.Settings;
using Discord;
using Discord.Commands;
using Humanizer;
using Humanizer.Localisation;

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

        [Command("randomchoice"), Summary("Return random choice from space-separated list.")]
        [Alias("choice", "rngchoice")]
        public async Task GetRandomChoiceAsync([Remainder] string message)
        {
            var components = message.Split(" ");
            var selection = components[_random.Next(components.Length)];

            await ReplyAsync(selection);
        }

        [Command("multichoice"), Summary("Return multiple random choices from space-separated list.")]
        public async Task GetRandomMultiChoiceAsync(int choices, [Remainder] string message)
        {
            var components = message.Split(" ").ToList();
            if (components.Count < choices || choices <= 0)
            {
                await ReplyAsync("Not enough choices.");
                return;
            }
            List<string> selections = new List<string>();
            var randomChoices = Enumerable.Range(0, components.Count).OrderBy(x => _random.Next()).Take(choices).ToList();
            randomChoices.ForEach(x => selections.Add(components[x]));
            await ReplyAsync(string.Join(" ", selections));
        }
    }
}

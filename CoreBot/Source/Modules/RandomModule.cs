using System;
using System.Threading.Tasks;
using Discord.Commands;
using CoreBot.Settings;
using Humanizer;
using Humanizer.Localisation;

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
        public async Task GetRandomDate()
        {
            var currentDate = DateTime.Now;
            var random = new Random();
            var randomDate = currentDate.AddHours(random.Next(0, 87600)); // 10 years
            var remainder = randomDate.Subtract(DateTime.Now);
            await ReplyAsync($"Random date: {randomDate.ToString(BotSettings.Instance.DateFormat)} (in {remainder.Humanize(BotSettings.Instance.HumanizerPrecision, maxUnit: TimeUnit.Year)})");
        }
    }
}

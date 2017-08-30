using System;
using System.Threading.Tasks;
using Discord.Commands;

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
    }
}
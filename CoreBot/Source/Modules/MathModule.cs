using System.Threading.Tasks;
using Discord.Commands;

namespace CoreBot.Modules
{
    [Group("math"), Summary("Basic math commands.")]
    public class MathModule : ModuleBase
    {
        [Command("add")]
        public async Task Add(int i, int i2)
        {
            await ReplyAsync($"{i} + {i2} = {i + i2}");
        }

        [Command("sub")]
        public async Task Substract(int i, int i2)
        {
            await ReplyAsync($"{i} - {i2} = {i - i2}");
        }

        [Command("multiply")]
        public async Task Multiply(int i, int i2)
        {
            await ReplyAsync($"{i} * {i2} = {i * i2}");
        }
    }
}

using System.Data;
using System.Threading.Tasks;
using Discord.Commands;

namespace CoreBot.Modules
{
    public class MathModule : ModuleBase
    {
        [Command("math"), Summary("Calculates basic math operations.")]
        public async Task Calculate(string input)
        {
            DataTable dataTable = new DataTable();
            var result = dataTable.Compute(input, string.Empty);
            await ReplyAsync($"{result}");
        }
    }
}
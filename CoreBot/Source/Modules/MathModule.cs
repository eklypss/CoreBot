using System.Data;
using System.Globalization;
using System.Threading.Tasks;
using Discord.Commands;

namespace CoreBot.Modules
{
    public class MathModule : ModuleBase
    {
        [Command("math"), Summary("Calculates basic math operations.")]
        public async Task Calculate([Remainder] string input)
        {
            var dataTable = new DataTable() { CaseSensitive = false, Locale = CultureInfo.CurrentCulture };
            var result = dataTable.Compute(input, string.Empty);
            await ReplyAsync($"`{input} = {result}`");
        }
    }
}
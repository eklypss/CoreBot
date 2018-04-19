using System.Data;
using System.Globalization;
using System.Threading.Tasks;
using CoreBot.Settings;
using Discord;
using Discord.Commands;

namespace CoreBot.Modules
{
    public class MathModule : ModuleBase
    {
        [Command("math"), Summary("Calculates basic math operations.")]
        [Alias("m")]
        public async Task CalculateAsync([Remainder] string input)
        {
            using (var dataTable = new DataTable { CaseSensitive = false, Locale = CultureInfo.CurrentCulture })
            {
                var result = dataTable.Compute(input, string.Empty);
                var embed = new EmbedBuilder()
                    .WithColor(BotSettings.Instance.EmbeddedColor)
                    .WithTitle(input)
                    .WithDescription(result.ToString())
                    .Build();

                await ReplyAsync(string.Empty, embed: embed);
            }
        }
    }
}

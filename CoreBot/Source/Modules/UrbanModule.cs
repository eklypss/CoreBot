using System.Threading.Tasks;
using CoreBot.Services;
using Discord.Commands;

namespace CoreBot.Modules
{
    public class UrbanModule : ModuleBase
    {
        private readonly UrbanService _urbanService;

        public UrbanModule(UrbanService urbanService)
        {
            _urbanService = urbanService;
        }

        [Command("urban"), Summary("Gets definition for the given input from the Urban Dictionary.")]
        public async Task GetUrbanQuoteAsync([Remainder] string searchTerm)
        {
            var quotes = await _urbanService.GetUrbanQuotesAsync(searchTerm);
            string definitions = _urbanService.ParseQuotes(quotes);
            if (definitions.Length > 0) await ReplyAsync(definitions);
            else await ReplyAsync("No definitions found.");
        }
    }
}

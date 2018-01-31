using System.Threading.Tasks;
using CoreBot.Services;
using Discord.Commands;

namespace CoreBot.Modules
{
    public class QuoteModule : ModuleBase
    {
        private readonly QuoteService _quoteService;

        public QuoteModule(QuoteService quoteService)
        {
            _quoteService = quoteService;
        }

        [Command("quote"), Summary("Displays a quote linked to the search term.")]
        [Alias("wiisaus", "viisaus")]
        public async Task GetWisdomAsync([Remainder] string searchTerm = "a")
        {
            // Use "a" as default value because its much faster than "*", and still matches 16/26k rows
            await ReplyAsync(await _quoteService.GetQuoteAsync(searchTerm));
        }
    }
}

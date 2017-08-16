using System;
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

        [Command("urban")]
        public async Task GetUrbanQuote([Remainder] string searchTerm)
        {
            var quotes = await _urbanService.GetUrbanQuotesAsync(searchTerm);
            var definitions = await _urbanService.ParseQuotesAsync(quotes);
            await ReplyAsync($"{string.Join(Environment.NewLine, definitions)}");
        }
    }
}
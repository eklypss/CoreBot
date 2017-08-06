using System.Threading.Tasks;
using CoreBot.Source.Services;
using Discord.Commands;

namespace CoreBot.Source.Modules
{
    public class QuoteModule : ModuleBase
    {
        private QuoteService _service;

        public QuoteModule(QuoteService s)
        {
            _service = s;
        }

        [Command("viisaus")]
        [Alias("wiisaus")]
        public async Task Wisdom([Remainder] string searchTerm = "a")
        {
            // Use "a" as default value because its much faster than "*", and still matches 16/26k rows

            await ReplyAsync(await _service.Quote(searchTerm));
        }
    }
}
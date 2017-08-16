using System;
using System.Linq;
using System.Threading.Tasks;
using CoreBot.Services;
using Discord.Commands;
using Serilog;

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
            if (definitions.Count > 0)
            {
                string response = string.Join(Environment.NewLine, definitions);
                Log.Information($"Lenght: {response.Length}");
                if (response.Length > 2000)
                {
                    // If the message length for all definitions is too long, just display the first definition.
                    // TODO: Change it shows as many definitions as possible (max length is 2000 characters).
                    await ReplyAsync(definitions.FirstOrDefault());
                }
                else await ReplyAsync(response);
            }
            else await ReplyAsync("No definitions found.");
        }
    }
}
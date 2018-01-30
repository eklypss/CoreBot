using System.Threading.Tasks;
using CoreBot.Services;
using Discord.Commands;

namespace CoreBot.Modules
{
    public class AromaModule : ModuleBase
    {
        private readonly AromaService _aromaService;

        public AromaModule(AromaService aromaService)
        {
            _aromaService = aromaService;
        }

        [Command("aromas"), Summary("Returns random aromas")]
        [Alias("vivahteet")]
        public async Task GetRandomAromasAsync()
        {
            await ReplyAsync(await _aromaService.RandomAromas());
        }
    }
}

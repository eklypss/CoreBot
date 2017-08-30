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

        [Command("vivahteet")]
        public async Task GetRandomAromasAsync()
        {
            await ReplyAsync(await _aromaService.RandomAromas());
        }
    }
}
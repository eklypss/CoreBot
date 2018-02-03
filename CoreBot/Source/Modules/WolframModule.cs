using System.Linq;
using System.Threading.Tasks;
using CoreBot.Services;
using Discord;
using Discord.Commands;

namespace CoreBot.Modules
{
    public class WolframModule : ModuleBase
    {
        private readonly WolframService _wolframService;

        public WolframModule(WolframService wolframService)
        {
            _wolframService = wolframService;
        }

        [Command("ask")]
        public async Task GetAnswerAsync([Remainder] string question)
        {
            var answer = await _wolframService.GetWolframAnswerAsync(question);
            if (answer == null)
            {
                await ReplyAsync("No answer found.");
                return;
            }
            var resultPod = answer.Pods.FirstOrDefault(x => x.Title.ToLower().Contains("response") || x.Title.ToLower().Contains("result"));
            var inputPod = answer.Pods.FirstOrDefault(x => x.Title.ToLower().Contains("input"));
            var embed = new EmbedBuilder()
                .AddField(inputPod.Subpods.FirstOrDefault().PlainText, resultPod.Subpods.FirstOrDefault().PlainText);
            await ReplyAsync(string.Empty, embed: embed);
        }
    }
}

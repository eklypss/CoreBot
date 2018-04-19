using System.Linq;
using System.Threading.Tasks;
using CoreBot.Services;
using CoreBot.Settings;
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

            var inputPod = answer.Pods[0];
            var resultPod = answer.Pods[1];
            var embed = new EmbedBuilder()
                .AddField(inputPod.Subpods.FirstOrDefault().PlainText, resultPod.Subpods.FirstOrDefault().PlainText)
                .WithColor(BotSettings.Instance.EmbeddedColor)
                .Build();

            await ReplyAsync(string.Empty, embed: embed);
        }
    }
}

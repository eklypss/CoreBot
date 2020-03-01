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
            if (!(bool) answer.success || answer.pods.Count < 1)
            {
                var message = "No results.";

                if (answer.tips != null)
                {
                    var node = answer.tips.Count == null ? answer.tips : answer.tips[0];
                    message += $" {(string) node.text}";
                }

                if (answer.didyoumeans != null)
                {
                    var node = answer.didyoumeans.Count == null ? answer.didyoumeans.val : answer.didyoumeans[0].val;
                    var nextAns = await _wolframService.GetWolframAnswerAsync((string)node);

                    var suggestedQuery = (string)nextAns.pods[0].subpods[0].plaintext;
                    var suggestedExplanation = (string)nextAns.pods[1].subpods[0].plaintext;

                    var em = new EmbedBuilder()
                        .AddField($"Did you mean \"{node}\"?\n{suggestedQuery}", suggestedExplanation)
                        .WithColor(BotSettings.Instance.EmbeddedColor)
                        .Build();

                    await ReplyAsync(string.Empty, embed: em);
                    return;
                }
                await ReplyAsync(message);
                return;
            }

            var query = (string)answer.pods[0].subpods[0].plaintext;
            var explanation = (string)answer.pods[1].subpods[0].plaintext;

            var embed = new EmbedBuilder()
                .AddField(query, explanation)
                .WithColor(BotSettings.Instance.EmbeddedColor)
                .Build();

            await ReplyAsync(string.Empty, embed: embed);
        }
    }
}

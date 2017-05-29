using System.Threading.Tasks;
using CoreBot.Enum;
using CoreBot.Services;
using CoreBot.Settings;
using Discord.Commands;

namespace CoreBot.Modules
{
    namespace CoreBot.Modules
    {
        public class PrefixModule : ModuleBase
        {
            [Command("prefix"), Summary("Sets the command prefix used by the bot.")]
            public async Task SetPrefix(char prefix)
            {
                BotSettings.Instance.BotPrefix = prefix;
                await FileManager.SaveFile(FileType.SettingsFile);
                await ReplyAsync($"Prefix was changed to: **{prefix}*'");
            }
        }
    }
}
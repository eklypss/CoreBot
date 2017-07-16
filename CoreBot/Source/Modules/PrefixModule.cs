using CoreBot.Enum;
using CoreBot.Helpers;
using CoreBot.Settings;
using Discord.Commands;
using System.Threading.Tasks;

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
                await FileHelper.SaveFile(FileType.SettingsFile);
                await ReplyAsync($"Prefix was changed to: **{prefix}**");
            }
        }
    }
}
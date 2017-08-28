using System.Threading.Tasks;
using CoreBot.Enum;
using CoreBot.Helpers;
using CoreBot.Settings;
using Discord.Commands;

namespace CoreBot.Modules
{
    /// <summary>
    /// Module for commands that are used to configure the bot, or provide information about it.
    /// </summary>
    public class ConfigurationModule : ModuleBase
    {
        [Command("prefix"), Summary("Sets the command prefix used by the bot.")]
        public async Task SetPrefix(char prefix)
        {
            BotSettings.Instance.BotPrefix = prefix;
            await FileHelper.SaveFileAsync(FileType.SettingsFile);
            await ReplyAsync($"Prefix was changed to: **{prefix}**");
        }
    }
}
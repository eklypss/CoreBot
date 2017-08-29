using System;
using System.Globalization;
using System.Threading.Tasks;
using CoreBot.Enum;
using CoreBot.Helpers;
using CoreBot.Models;
using CoreBot.Settings;
using Discord.Commands;
using Humanizer;

namespace CoreBot.Modules
{
    /// <summary>
    /// Module for commands that are used to configure the bot, or provide information about it.
    /// </summary>
    public class ConfigurationModule : ModuleBase
    {
        private readonly StartupTime _startupTime;

        public ConfigurationModule(StartupTime startupTime)
        {
            _startupTime = startupTime;
        }

        [Command("prefix"), Summary("Sets the command prefix used by the bot.")]
        public async Task SetPrefix(char prefix)
        {
            BotSettings.Instance.BotPrefix = prefix;
            await FileHelper.SaveFileAsync(FileType.SettingsFile);
            await ReplyAsync($"Prefix was changed to: **{prefix}**");
        }

        [Command("uptime"), Summary("Gets the uptime of the bot (aka time since bot was started)")]
        public async Task GetUptime()
        {
            await ReplyAsync($"Bot started {_startupTime.StartTime.Subtract(DateTime.Now).Humanize(BotSettings.Instance.HumanizerPrecision)} ago (at *{_startupTime.StartTime.ToString(BotSettings.Instance.DateTimeFormat, new CultureInfo(BotSettings.Instance.DateTimeCulture))}*)");
        }
    }
}
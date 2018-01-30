using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CoreBot.Enum;
using CoreBot.Helpers;
using CoreBot.Models;
using CoreBot.Settings;
using Discord.Commands;
using Humanizer;
using Humanizer.Localisation;

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
        public async Task SetPrefixAsync(char prefix)
        {
            BotSettings.Instance.BotPrefix = prefix;
            await FileHelper.SaveFileAsync(FileType.SettingsFile);
            await ReplyAsync($"Prefix was changed to: **{prefix}**");
        }

        [Command("uptime"), Summary("Returns the uptime of the bot.")]
        public async Task ReplyWithUptimeAsync()
        {
            await ReplyAsync($"Bot started {_startupTime.StartTime.Subtract(DateTime.Now).Humanize(maxUnit: BotSettings.Instance.HumanizerMaxUnit, precision: BotSettings.Instance.HumanizerPrecision)} ago (at *{_startupTime.StartTime.ToString(BotSettings.Instance.DateTimeFormat, new CultureInfo(BotSettings.Instance.DateTimeCulture))}*)");
        }

        [Command("precision"), Summary("Sets the precision used by Humanizer.")]
        public async Task SetPrecisionAsync(int precision)
        {
            // Only values between 1 and 5 seem to work.
            if (Enumerable.Range(1, 5).Contains(precision))
            {
                var previousPrecision = BotSettings.Instance.HumanizerPrecision;
                BotSettings.Instance.HumanizerPrecision = precision;
                await FileHelper.SaveFileAsync(FileType.SettingsFile);
                await ReplyAsync($"Humanizer precision was changed to: {precision}. Previous precision: {previousPrecision}.");
            }
            else await ReplyAsync("Invalid value.");
        }

        [Command("maxunit"), Summary("Sets the max unit used by Humanizer.")]
        public async Task SetMaxUnitAsync(string unit)
        {
            string currentUnit = BotSettings.Instance.HumanizerMaxUnit.ToString();
            switch (unit.ToLower())
            {
                case "hour":
                {
                    BotSettings.Instance.HumanizerMaxUnit = TimeUnit.Hour;
                    await FileHelper.SaveFileAsync(FileType.SettingsFile);
                    break;
                }
                case "day":
                {
                    BotSettings.Instance.HumanizerMaxUnit = TimeUnit.Day;
                    await FileHelper.SaveFileAsync(FileType.SettingsFile);
                    break;
                }
                case "week":
                {
                    BotSettings.Instance.HumanizerMaxUnit = TimeUnit.Week;
                    await FileHelper.SaveFileAsync(FileType.SettingsFile);
                    break;
                }
                case "month":
                {
                    BotSettings.Instance.HumanizerMaxUnit = TimeUnit.Month;
                    await FileHelper.SaveFileAsync(FileType.SettingsFile);
                    break;
                }
                case "year":
                {
                    BotSettings.Instance.HumanizerMaxUnit = TimeUnit.Year;
                    await FileHelper.SaveFileAsync(FileType.SettingsFile);
                    break;
                }
                default:
                {
                    await ReplyAsync("Invalid unit specified, available units: hour, day, week, month, year.");
                    return;
                }
            }
            await ReplyAsync($"Humanizer max unit was changed to: {BotSettings.Instance.HumanizerMaxUnit.ToString()}. Previous precision: {currentUnit}.");
        }
    }
}

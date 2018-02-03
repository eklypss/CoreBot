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
                    break;
                }
                case "day":
                {
                    BotSettings.Instance.HumanizerMaxUnit = TimeUnit.Day;
                    break;
                }
                case "week":
                {
                    BotSettings.Instance.HumanizerMaxUnit = TimeUnit.Week;
                    break;
                }
                case "month":
                {
                    BotSettings.Instance.HumanizerMaxUnit = TimeUnit.Month;
                    break;
                }
                case "year":
                {
                    BotSettings.Instance.HumanizerMaxUnit = TimeUnit.Year;
                    break;
                }
                default:
                {
                    await ReplyAsync("Invalid unit specified, available units: hour, day, week, month, year.");
                    return;
                }
            }

            await FileHelper.SaveFileAsync(FileType.SettingsFile);
            await ReplyAsync($"Humanizer max unit was changed to: {BotSettings.Instance.HumanizerMaxUnit.ToString()}. Previous precision: {currentUnit}.");
        }

        [Command("cmdlimit"), Summary("Sets the max dynamic commands displayed per line in command listing.")]
        public async Task SetMaxLimitAsync(int limit)
        {
            // Only values between 1 and 5 seem to work.
            if (Enumerable.Range(1, 30).Contains(limit))
            {
                var previousLimit = BotSettings.Instance.DynamicCommandsPerLine;
                BotSettings.Instance.DynamicCommandsPerLine = limit;
                await FileHelper.SaveFileAsync(FileType.SettingsFile);
                await ReplyAsync($"Dynamic commands per line was changed to: {limit}. Previous limit: {previousLimit}.");
            }
            else await ReplyAsync("Invalid value, valid values: 1-30.");
        }

        [Command("setapikey"), Summary("Sets/updates the selected API key in bot settings.")]
        public async Task SetAPIKeyAsync(string api, string key)
        {
            switch (api.ToLower())
            {
                case "weather":
                {
                    BotSettings.Instance.WeatherAPIKey = key;
                    break;
                }
                case "ep":
                {
                    BotSettings.Instance.EPAPIKey = key;
                    break;
                }
                case "urban":
                {
                    BotSettings.Instance.UrbanMashapeKey = key;
                    break;
                }
                case "wolfram":
                {
                    BotSettings.Instance.WolframAppID = key;
                    break;
                }
                default:
                {
                    await ReplyAsync("Invalid value, valid values: ``weather`` ``ep`` ``urban`` ``wolfram``");
                    return;
                }
            }
            await FileHelper.SaveFileAsync(FileType.SettingsFile);
            await ReplyAsync("API key set.");
        }
    }
}

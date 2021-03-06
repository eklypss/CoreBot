﻿using System.Collections.Generic;
using System.IO;
using System.Reflection;
using CoreBot.Helpers;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Humanizer.Localisation;

namespace CoreBot.Settings
{
    public class BotSettings
    {
        private static BotSettings _instance;

        public static BotSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new BotSettings();
                }
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        public string BotToken { get; set; } = string.Empty;
        public char BotPrefix { get; set; } = DefaultValues.DEFAULT_PREFIX;

        public ulong DefaultChannel { get; set; }
        public ulong DefaultGuild { get; set; }

        public string DatabaseString { get; set; } = string.Empty;
        public bool LogToFile { get; set; } = true;

        public string WeatherAPIKey { get; set; } = string.Empty;
        public string EPAPIKey { get; set; } = string.Empty;
        public string UrbanMashapeKey { get; set; } = string.Empty;
        public string WolframAppID { get; set; } = string.Empty;
        public string AzureVisionKey { get; set; } = string.Empty;

        public string DateTimeFormat { get; set; }
        public string DateFormat { get; set; }
        public string DateTimeCulture { get; set; }

        public List<string> OldLinkBlacklist { get; set; }

        public int HumanizerPrecision { get; set; } = DefaultValues.DEFAULT_HUMANIZER_PRECISION;
        public TimeUnit HumanizerMaxUnit { get; set; } = DefaultValues.DEFAULT_HUMANIZER_MAXUNIT;
        public int GrapevineServerPort { get; set; } = DefaultValues.DEFAULT_GRAPEVINE_SERVER_PORT;
        public string DiscordnetLoglevel { get; set; } = DefaultValues.DEFAULT_LOGLEVEL;
        public string SelfHotstring { get; set; } = DefaultValues.DEFAULT_SELF_HOTSTRING;
        public int DynamicCommandsPerLine { get; set; } = DefaultValues.DEFAULT_MAX_DYNAMIC_COMMANDS_PER_LINE;
        public Color EmbeddedColor { get; set; } = DefaultValues.DEFAULT_EMBEDDED_COLOR;
        public char SeparatorChar { get; set; } = DefaultValues.DEFAULT_SEPARATOR_CHAR;

        public static DiscordSocketConfig CreateDiscordConfig(string loglevel)
        {
            return new DiscordSocketConfig
            {
                LogLevel = LogHelper.ParseLoglevel(loglevel)
            };
        }

        public static CommandServiceConfig CreateCommandConfig(string loglevel)
        {
            return new CommandServiceConfig
            {
                DefaultRunMode = RunMode.Async,
                LogLevel = LogHelper.ParseLoglevel(loglevel)
            };
        }
    }
}

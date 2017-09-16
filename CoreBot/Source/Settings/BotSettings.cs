using System.Collections.Generic;
using System.IO;
using System.Reflection;
using CoreBot.Interfaces;

namespace CoreBot.Settings
{
    public class BotSettings : IBotSettings
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
        public string SettingsFolder { get; set; }
        public string SettingsFile { get; set; }
        public string WeatherAPIKey { get; set; } = string.Empty;
        public string EPAPIKey { get; set; } = string.Empty;
        public List<string> OldLinkBlacklist { get; set; }
        public string UrbanMashapeKey { get; set; } = string.Empty;
        public string DateTimeFormat { get; set; }
        public string DateFormat { get; set; }
        public string DateTimeCulture { get; set; }
        public int HumanizerPrecision { get; set; } = DefaultValues.DEFAULT_HUMANIZER_PRECISION;
        public int GrapevineServerPort { get; set; } = DefaultValues.DEFAULT_GRAPEVINE_SERVER_PORT;

        private BotSettings()
        {
            SettingsFolder = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Settings");
            SettingsFile = Path.Combine(SettingsFolder, "BotSettings.json");
        }
    }
}
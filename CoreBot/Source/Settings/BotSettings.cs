using System.IO;
using System.Reflection;

namespace CoreBot.Settings
{
    public class BotSettings
    {
        private static BotSettings instance;

        public static BotSettings Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BotSettings();
                }
                return instance;
            }
            set
            {
                instance = value;
            }
        }

        public string BotToken { get; set; } = string.Empty;
        public char BotPrefix { get; set; } = '!';
        public bool LogToFile { get; set; } = true;
        public string SettingsFolder { get; set; }
        public string SettingsFile { get; set; }

        private BotSettings()
        {
            SettingsFolder = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Settings");
            SettingsFile = Path.Combine(SettingsFolder, "BotSettings.json");
        }
    }
}
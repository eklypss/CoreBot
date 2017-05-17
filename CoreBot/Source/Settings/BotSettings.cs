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
        }

        public string BotToken { get; set; }
        public string BotPrefix { get; set; } = "!";
        public bool LogToFile { get; set; } = true;
        public string SettingsFolder { get; set; }
        public string SettingsFile { get; set; }

        private BotSettings()
        {
            SettingsFolder = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Settings");
            SettingsFile = Path.Combine(SettingsFolder, "BotSettings.config");
        }
    }
}
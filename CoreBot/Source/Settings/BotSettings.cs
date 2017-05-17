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
        public string BotPrefix { get; set; } = "!";
        public bool LogToFile { get; set; } = true;
        public string SettingsFolder { get; set; }
        public string SettingsFile { get; set; }
        public string CommandsFolder { get; set; }
        public string CommandsFile { get; set; }

        private BotSettings()
        {
            SettingsFolder = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Settings");
            CommandsFolder = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Commands");
            SettingsFile = Path.Combine(SettingsFolder, "BotSettings.json");
            CommandsFile = Path.Combine(CommandsFolder, "Commands.json");
        }
    }
}
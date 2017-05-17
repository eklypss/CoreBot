using CoreBot.Settings;
using Serilog;
using System.Configuration;
using System.IO;

namespace CoreBot.Services
{
    public static class FileManager
    {
        public static void CheckFiles()
        {
            Log.Information("Trying to load configuration files.");
            if (!Directory.Exists(BotSettings.Instance.SettingsFolder))
            {
                Log.Warning("Settings directory does not exist. Trying to create it.");
                Directory.CreateDirectory(BotSettings.Instance.SettingsFolder);
                BotSettings.Instance.SettingsFile = Path.Combine(BotSettings.Instance.SettingsFolder, "/BotSettings.config");
                Log.Information($"Settings folder created at: {BotSettings.Instance.SettingsFolder}.");
            }

            if (!File.Exists(BotSettings.Instance.SettingsFile))
            {
                Log.Warning("Settings file does not exist. Trying to create it.");
                File.Create(BotSettings.Instance.SettingsFile);
                Configuration config = ConfigurationManager.OpenExeConfiguration(BotSettings.Instance.SettingsFile);
                config.AppSettings.Settings.Add("Prefix", "!");
                config.AppSettings.Settings.Add("Token", "Insert bot token here.");
                config.Save();
                Log.Information($"Settings file created at: {BotSettings.Instance.SettingsFile}.");
                Log.Error($"Change your bot token in the configuration file at {BotSettings.Instance.SettingsFile} and restart the program.");
            }
            else
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(BotSettings.Instance.SettingsFile);
                BotSettings.Instance.BotPrefix = config.AppSettings.Settings["Prefix"].Value;
                BotSettings.Instance.BotToken = config.AppSettings.Settings["Token"].Value;
                Log.Information("Configuration file loaded successfully.");
            }
        }
    }
}
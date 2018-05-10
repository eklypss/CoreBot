using System;
using System.IO;
using System.Threading.Tasks;
using CoreBot.Database;
using CoreBot.Settings;
using Newtonsoft.Json;
using Serilog;

namespace CoreBot.Helpers
{
    public static class FileHelper
    {

        public static string settingsPath;

        public async static Task CheckFilesAsync(string settingsPath)
        {
            Log.Information("Checking bot files.");

            if (!File.Exists(settingsPath))
            {
                Log.Fatal("settings file not found at " + settingsPath);
                Environment.Exit(1);
            }

            FileHelper.settingsPath = settingsPath;

            await LoadSettingsAsync(settingsPath);
            await DbConnection.InitAsync();
        }

        public async static Task SaveSettingsAsync()
        {
            try
            {
                using (StreamWriter writer = File.CreateText(settingsPath))
                {
                    await writer.WriteAsync(JsonConvert.SerializeObject(BotSettings.Instance, Formatting.Indented));
                    Log.Information($"Successfully saved settings to {settingsPath}.");
                }
            }
            catch (Exception e)
            {
                Log.Error("Error occurred while trying to save settings. " + e);
                throw;
            }
        }

        public async static Task LoadSettingsAsync(string path)
        {
            try
            {
                Log.Information("Trying to load configuration files.");
                BotSettings.Instance = JsonConvert.DeserializeObject<BotSettings>(File.ReadAllText(path));

                Log.Information("Successfully loaded the configuration file.");

                // If values have not been set, use default values instead.
                if (BotSettings.Instance.BotPrefix == '\0') BotSettings.Instance.BotPrefix = DefaultValues.DEFAULT_PREFIX;
                if (string.IsNullOrEmpty(BotSettings.Instance.DatabaseString)) BotSettings.Instance.DatabaseString = DefaultValues.DEFAULT_DATABASE_STRING;
                if (string.IsNullOrEmpty(BotSettings.Instance.DateTimeFormat)) BotSettings.Instance.DateTimeFormat = DefaultValues.DEFAULT_DATETIME_FORMAT;
                if (string.IsNullOrEmpty(BotSettings.Instance.DateFormat)) BotSettings.Instance.DateFormat = DefaultValues.DEFAULT_DATE_FORMAT;
                if (string.IsNullOrEmpty(BotSettings.Instance.DateTimeCulture)) BotSettings.Instance.DateTimeCulture = DefaultValues.DEFAULT_CULTURE;

                // Used to sync new settings to old settings file without having to re-create it.
                await SaveSettingsAsync();
            }
            catch (Exception e)
            {
                Log.Error("Failed to load the configuration file. " + e);
                throw;
            }
        }
    }
}

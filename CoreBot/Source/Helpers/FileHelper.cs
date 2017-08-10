using System;
using System.IO;
using System.Threading.Tasks;
using CoreBot.Database;
using CoreBot.Enum;
using CoreBot.Settings;
using Newtonsoft.Json;
using Serilog;

namespace CoreBot.Helpers
{
    public static class FileHelper
    {
        public async static Task CheckFilesAsync()
        {
            if (!Directory.Exists(BotSettings.Instance.SettingsFolder)) await CreateFileAsync(FileType.SettingsFolder);

            if (!File.Exists(BotSettings.Instance.SettingsFile)) await CreateFileAsync(FileType.SettingsFile);
            else await LoadFileAsync(FileType.SettingsFile);
            await DbConnection.InitAsync();
        }

        public async static Task CreateFileAsync(FileType fileType)
        {
            switch (fileType)
            {
                case FileType.SettingsFile:
                {
                    try
                    {
                        Log.Warning("Settings file does not exist. Trying to create it.");
                        using (StreamWriter writer = File.CreateText(BotSettings.Instance.SettingsFile))
                        {
                            await writer.WriteAsync(JsonConvert.SerializeObject(BotSettings.Instance, Formatting.Indented));
                            Log.Information($"Settings file created at: {BotSettings.Instance.SettingsFile}.");
                        }
                        Log.Error($"Change your bot token in the configuration file at {BotSettings.Instance.SettingsFile} and restart the program.");
                        break;
                    }
                    catch (Exception)
                    {
                        Log.Error("Failed to create the configuration file.");
                        throw;
                    }
                }
                case FileType.SettingsFolder:
                {
                    Log.Warning("Settings folder does not exist. Trying to create it.");
                    Directory.CreateDirectory(BotSettings.Instance.SettingsFolder);
                    BotSettings.Instance.SettingsFile = Path.Combine(BotSettings.Instance.SettingsFolder, "BotSettings.json");
                    Log.Information($"Settings folder created at: {BotSettings.Instance.SettingsFolder}.");
                    break;
                }
            }
        }

        public async static Task SaveFileAsync(FileType fileType)
        {
            switch (fileType)
            {
                case FileType.SettingsFile:
                {
                    try
                    {
                        using (StreamWriter writer = File.CreateText(BotSettings.Instance.SettingsFile))
                        {
                            await writer.WriteAsync(JsonConvert.SerializeObject(BotSettings.Instance, Formatting.Indented));
                            Log.Information($"Successfully saved settings to {BotSettings.Instance.SettingsFile}.");
                        }
                    }
                    catch (Exception)
                    {
                        Log.Error("Error occurred while trying to save settings.");
                        throw;
                    }
                    break;
                }
            }
        }

        public async static Task LoadFileAsync(FileType fileType)
        {
            switch (fileType)
            {
                case FileType.SettingsFile:
                {
                    try
                    {
                        Log.Information("Trying to load configuration files.");
                        BotSettings.Instance = JsonConvert.DeserializeObject<BotSettings>(File.ReadAllText(BotSettings.Instance.SettingsFile));
                        Log.Information("Successfully loaded the configuration file.");
                        if (BotSettings.Instance.DatabaseString == null) BotSettings.Instance.DatabaseString = DefaultValues.DEFAULT_DATABASE_STRING;
                        if (BotSettings.Instance.BotPrefix == '\0') BotSettings.Instance.BotPrefix = DefaultValues.DEFAULT_PREFIX;
                        // Used to sync new settings to old settings file without having to re-create it.
                        await SaveFileAsync(FileType.SettingsFile);
                    }
                    catch (Exception)
                    {
                        Log.Error("Failed to load the configuration file.");
                        throw;
                    }
                    break;
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CoreBot.Collections;
using CoreBot.Enum;
using CoreBot.Models;
using CoreBot.Settings;
using Newtonsoft.Json;
using Serilog;

namespace CoreBot.Helpers
{
    public static class FileHelper
    {
        public async static Task CheckFiles()
        {
            if (!Directory.Exists(BotSettings.Instance.SettingsFolder)) await CreateFile(FileType.SettingsFolder);

            if (!File.Exists(BotSettings.Instance.SettingsFile)) await CreateFile(FileType.SettingsFile);
            else await LoadFile(FileType.SettingsFile);

            if (File.Exists(BotSettings.Instance.MessagesFile)) await LoadFile(FileType.MessagesFile);

            if (!Directory.Exists(BotSettings.Instance.CommandsFolder)) await CreateFile(FileType.CommandsFolder);

            if (File.Exists(BotSettings.Instance.CommandsFile)) await LoadFile(FileType.CommandsFile);
            else Log.Information("Commands file does not exist. No dynamic commands were loaded.");
        }

        public async static Task CreateFile(FileType fileType)
        {
            switch (fileType)
            {
                case FileType.CommandsFolder:
                {
                    Log.Warning("Commands folder does not exist. Trying to create it.");
                    Directory.CreateDirectory(BotSettings.Instance.CommandsFolder);
                    Log.Information($"Commands folder created at: {BotSettings.Instance.CommandsFolder}.");
                    break;
                }
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

        public async static Task SaveFile(FileType fileType)
        {
            switch (fileType)
            {
                case FileType.MessagesFile:
                {
                    try
                    {
                        using (StreamWriter writer = File.CreateText(BotSettings.Instance.MessagesFile))
                        {
                            await writer.WriteAsync(JsonConvert.SerializeObject(UserMessages.Instance.Messages, Formatting.Indented));
                        }
                    }
                    catch (Exception)
                    {
                        Log.Error("Error occurred while trying to save messages.");
                        throw;
                    }
                    break;
                }
                case FileType.CommandsFile:
                {
                    try
                    {
                        using (StreamWriter writer = File.CreateText(BotSettings.Instance.CommandsFile))
                        {
                            await writer.WriteAsync(JsonConvert.SerializeObject(Commands.Instance.CommandsList, Formatting.Indented));
                            Log.Information($"Successfully saved commands to {BotSettings.Instance.CommandsFile}.");
                        }
                    }
                    catch (Exception)
                    {
                        Log.Error("Error occurred while trying to save commands.");
                        throw;
                    }
                    break;
                }
                case FileType.SettingsFile:
                {
                    try
                    {
                        using (StreamWriter writer = File.CreateText(BotSettings.Instance.SettingsFile))
                        {
                            await writer.WriteAsync(JsonConvert.SerializeObject(BotSettings.Instance, Formatting.Indented));
                            Log.Information($"Successfully saved settings to {BotSettings.Instance.CommandsFile}.");
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

        public async static Task LoadFile(FileType fileType)
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
                        // Used to sync new settings to old settings file without having to re-create it.
                        await SaveFile(FileType.SettingsFile);
                    }
                    catch (Exception)
                    {
                        Log.Error("Failed to load the configuration file.");
                        throw;
                    }
                    break;
                }
                case FileType.MessagesFile:
                {
                    try
                    {
                        Log.Information("Trying to load messages.");
                        UserMessages.Instance.Messages = JsonConvert.DeserializeObject<List<UserMessage>>(await File.ReadAllTextAsync(BotSettings.Instance.MessagesFile));
                        Log.Information($"Loaded {UserMessages.Instance.Messages.Count} messages from {BotSettings.Instance.MessagesFile}.");
                    }
                    catch (Exception)
                    {
                        Log.Error("Failed to load the messages file.");
                        throw;
                    }
                    break;
                }
                case FileType.CommandsFile:
                {
                    try
                    {
                        Log.Information("Trying to load dynamic commands.");
                        Commands.Instance.CommandsList = JsonConvert.DeserializeObject<List<Command>>(await File.ReadAllTextAsync(BotSettings.Instance.CommandsFile));
                        Log.Information($"Loaded {Commands.Instance.CommandsList.Count} commands from {BotSettings.Instance.CommandsFile}.");
                    }
                    catch (Exception)
                    {
                        Log.Error("Failed to load the commands file.");
                        throw;
                    }
                    break;
                }
            }
        }
    }
}
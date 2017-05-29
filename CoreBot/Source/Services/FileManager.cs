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

namespace CoreBot.Services
{
    public static class FileManager
    {
        public async static Task CheckFiles()
        {
            Log.Information("Trying to load configuration files.");
            if (!Directory.Exists(BotSettings.Instance.SettingsFolder))
            {
                await CreateFile(FileType.SettingsFolder);
            }

            if (!File.Exists(BotSettings.Instance.SettingsFile))
            {
                await CreateFile(FileType.SettingsFile);
            }
            else
            {
                try
                {
                    BotSettings.Instance = JsonConvert.DeserializeObject<BotSettings>(File.ReadAllText(BotSettings.Instance.SettingsFile));
                    Log.Information("Successfully loaded the configuration file.");
                }
                catch (Exception)
                {
                    Log.Error("Failed to load the configuration file.");
                    throw;
                }
            }

            if (File.Exists(BotSettings.Instance.MessagesFile))
            {
                UserMessages.Instance.Messages = JsonConvert.DeserializeObject<List<UserMessage>>(await File.ReadAllTextAsync(BotSettings.Instance.MessagesFile));
                Log.Information($"Loaded {UserMessages.Instance.Messages.Count} messages from {BotSettings.Instance.MessagesFile}.");
            }

            Log.Information("Trying to load dynamic commands.");
            if (!Directory.Exists(BotSettings.Instance.CommandsFolder))
            {
                await CreateFile(FileType.CommandsFolder);
            }
            if (File.Exists(BotSettings.Instance.CommandsFile))
            {
                Commands.Instance.CommandsList = JsonConvert.DeserializeObject<List<Command>>(await File.ReadAllTextAsync(BotSettings.Instance.CommandsFile));
                Log.Information($"Loaded {Commands.Instance.CommandsList.Count} commands from {BotSettings.Instance.CommandsFile}.");
            }
            else Log.Information("Commands file does not exist. No dynamic commands were loaded.");
        }

        public async static Task CreateFile(FileType createType)
        {
            switch (createType)
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
                        var json = JsonConvert.SerializeObject(UserMessages.Instance.Messages, Formatting.Indented);
                        if (File.Exists(BotSettings.Instance.MessagesFile))
                        {
                            File.Delete(BotSettings.Instance.MessagesFile);
                            Log.Information("Deleted old messages file.");
                        }

                        using (StreamWriter writer = File.CreateText(BotSettings.Instance.MessagesFile))
                        {
                            await writer.WriteAsync(json);
                            Log.Information($"Successfully saved messages to {BotSettings.Instance.MessagesFile}.");
                        }
                    }
                    catch (Exception)
                    {
                        Log.Error("Error occurred while trying to save messages.");
                        throw;
                    }
                    break;
                }
            }
        }
    }
}
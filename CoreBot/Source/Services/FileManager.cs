using CoreBot.Collections;
using CoreBot.Models;
using CoreBot.Settings;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
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
                Log.Warning("Settings folder does not exist. Trying to create it.");
                Directory.CreateDirectory(BotSettings.Instance.SettingsFolder);
                BotSettings.Instance.SettingsFile = Path.Combine(BotSettings.Instance.SettingsFolder, "/BotSettings.config");
                Log.Information($"Settings folder created at: {BotSettings.Instance.SettingsFolder}.");
            }

            if (!File.Exists(BotSettings.Instance.SettingsFile))
            {
                try
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
                catch (TypeLoadException ex)
                {
                    Log.Error(ex.Message);
                    throw;
                }
                finally
                {
                    Log.Information("Successfully created bot settings file.");
                }
            }
            else
            {
                try
                {
                    Configuration config = ConfigurationManager.OpenExeConfiguration(BotSettings.Instance.SettingsFile);
                    BotSettings.Instance.BotPrefix = config.AppSettings.Settings["Prefix"].Value;
                    BotSettings.Instance.BotToken = config.AppSettings.Settings["Token"].Value;
                    Log.Information("Configuration file loaded successfully.");
                }
                catch (TypeLoadException ex)
                {
                    Log.Error(ex.Message);
                    throw;
                }
                finally
                {
                    Log.Information("Successfully loaded bot settings.");
                }
            }

            Log.Information("Trying to load dynamic commands.");
            if (!Directory.Exists(BotSettings.Instance.CommandsFolder))
            {
                Log.Warning("Commands folder does not exist. Trying to create it.");
                Directory.CreateDirectory(BotSettings.Instance.SettingsFolder);
                Log.Information($"Commands folder created at: {BotSettings.Instance.CommandsFolder}.");
            }
            if (File.Exists(BotSettings.Instance.CommandsFile))
            {
                Commands.Instance.CommandsList = JsonConvert.DeserializeObject<List<Command>>(File.ReadAllText(BotSettings.Instance.CommandsFile));
                Log.Information($"Loaded {Commands.Instance.CommandsList.Count} commands from {BotSettings.Instance.CommandsFile}.");
            }
            else Log.Information("Commands file does not exist. No dynamic commands were loaded.");
        }
    }
}
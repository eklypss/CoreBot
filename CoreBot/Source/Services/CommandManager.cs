﻿using System;
using System.IO;
using System.Threading.Tasks;
using CoreBot.Collections;
using CoreBot.Models;
using CoreBot.Settings;
using Newtonsoft.Json;
using Serilog;

namespace CoreBot.Services
{
    /// <summary>
    /// Class for managing dynamic commands.
    /// </summary>
    public class CommandManager
    {
        public async Task SaveCommands()
        {
            try
            {
                var json = JsonConvert.SerializeObject(Commands.Instance.CommandsList, Formatting.Indented);
                if (File.Exists(BotSettings.Instance.CommandsFile))
                {
                    File.Delete(BotSettings.Instance.CommandsFile);
                    Log.Information("Deleted old commands file.");
                }

                using (StreamWriter writer = File.CreateText(BotSettings.Instance.CommandsFile))
                {
                    await writer.WriteAsync(json);
                    Log.Information($"Successfully saved commands to {BotSettings.Instance.CommandsFile}.");
                }
            }
            catch (Exception)
            {
                Log.Error("Error occurred while trying to save commands.");
                throw;
            }
        }

        public async Task AddCommand(Command command)
        {
            Log.Information($"Trying to add command: {BotSettings.Instance.BotPrefix}{command.Name}, action: {command.Action}.");
            bool commandExists = false;
            foreach (Command cmd in Commands.Instance.CommandsList)
            {
                if (cmd.Action == command.Action)
                {
                    commandExists = true;
                    break;
                }
            }

            if (!commandExists)
            {
                Log.Information($"Command added: {BotSettings.Instance.BotPrefix}{command.Name}, action: {command.Action}");
                Commands.Instance.CommandsList.Add(command);
                await SaveCommands();
            }
            else
            {
                Log.Warning($"Could not add command: {BotSettings.Instance.BotPrefix}{command.Name} since it already exists.");
            }
        }

        public async Task DeleteCommand(Command command)
        {
            if (Commands.Instance.CommandsList.Contains(command))
            {
                Commands.Instance.CommandsList.Remove(command);
                await SaveCommands();
                Log.Information($"Command {command.Name} was deleted.");
            }
            else Log.Warning($"Command does not exist: {command.Name}");
        }
    }
}
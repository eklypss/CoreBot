﻿using CoreBot.Collections;
using CoreBot.Models;
using CoreBot.Settings;
using Newtonsoft.Json;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CoreBot.Services
{
    public class CommandManager
    {
        public async Task SaveCommands()
        {
            try
            {
                var json = JsonConvert.SerializeObject(Commands.Instance.CommandsList);
                if (File.Exists(BotSettings.Instance.CommandsFile))
                {
                    File.Delete(BotSettings.Instance.CommandsFile);
                    Log.Information("Deleted old CommandsFile.");
                }

                using (StreamWriter writer = File.CreateText(BotSettings.Instance.CommandsFile))
                {
                    await writer.WriteAsync(json);
                }
            }
            catch (Exception)
            {
                Log.Error("Error occured while trying to save commands.");
                throw;
            }
            finally
            {
                Log.Information($"Successfully saved commands to {BotSettings.Instance.CommandsFile}.");
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
                Log.Information($"Command added: {BotSettings.Instance.BotPrefix}{command.Name}, action: {command.Action}, added by: {command.AddedBy} on {command.DateAdded}.");
                Commands.Instance.CommandsList.Add(command);
                await SaveCommands();
            }
            else
            {
                Log.Warning($"Could not add command: {BotSettings.Instance.BotPrefix}{command.Name} since it already exists.");
            }
        }
    }
}
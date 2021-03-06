﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreBot.Collections;
using CoreBot.Database.Dao;
using CoreBot.Models;
using CoreBot.Settings;
using Discord.Commands;
using Serilog;

namespace CoreBot.Modules
{
    [Group("command"), Summary("Module for modifying and listing commands.")]
    [Alias("commands", "cmd", "cmds")]
    public class CommandModule : ModuleBase
    {
        private readonly CommandDao _commandDao;
        private readonly CommandService _commandService;

        public CommandModule(CommandDao commandDao, CommandService commandService)
        {
            _commandDao = commandDao;
            _commandService = commandService;
        }

        [Command("add"), Summary("Adds a new dynamic command.")]
        [Alias("new")]
        public async Task AddCommandAsync(string commandName, [Remainder] string commandAction)
        {
            if (commandAction.Length > 0 || !string.IsNullOrEmpty(commandAction))
            {
                var found = Commands.Instance.CommandsList.Find(x => x.Name.Equals(commandName.Replace(BotSettings.Instance.BotPrefix.ToString(), string.Empty), StringComparison.InvariantCultureIgnoreCase));
                if (found == null)
                {
                    await _commandDao.AddCommandAsync(new Command(commandName.Replace(BotSettings.Instance.BotPrefix.ToString(), string.Empty), commandAction));
                    await ReplyAsync($"Command ``{commandName}`` added, action: ``{commandAction}``");
                    Log.Information($"Command {commandName} added, action: {commandAction}");
                }
                else
                {
                    Log.Warning($"Could not add command {commandName} since it already exists.");
                    await ReplyAsync($"Command ``{commandName}`` already exists. Use ``{BotSettings.Instance.BotPrefix}command update {commandName}`` to edit the existing command.");
                }
            }
            else
            {
                Log.Warning($"Could not add command {commandName} because the action was invalid.");
            }
        }

        [Command("info"), Summary("Displays info about the command.")]
        [Alias("i", "details")]
        public async Task DisplayCommandInfoAsync(string commandName)
        {
            var command = Commands.Instance.CommandsList.Find(x => x.Name.Equals(commandName.Replace(BotSettings.Instance.BotPrefix.ToString(), string.Empty), StringComparison.InvariantCultureIgnoreCase));
            if (command != null)
            {
                await ReplyAsync($"Command: ``{commandName}`` ID: ``{command.Id}`` Action: ``{command.Action}``");
            }
            else
            {
                await ReplyAsync($"Command ``{commandName}`` does not exist.");
            }
        }

        [Command("delete"), Summary("Deletes a dynamic command.")]
        [Alias("del", "remove")]
        public async Task DeleteCommandAsync(string commandName)
        {
            var command = Commands.Instance.CommandsList.Find(x => x.Name.Equals(commandName.Replace(BotSettings.Instance.BotPrefix.ToString(), string.Empty), StringComparison.InvariantCultureIgnoreCase));
            if (command != null)
            {
                await _commandDao.DeleteCommandAsync(command);
                await ReplyAsync($"Command deleted: ``{commandName}``");
                Log.Information($"Command deleted: {commandName}");
            }
            else
            {
                Log.Warning($"Failed to delete command {commandName} as it doesn't exist.");
                await ReplyAsync($"Command ``{commandName}`` does not exist.");
            }
        }

        [Command("update"), Summary("Updates the action of a dynamic command.")]
        [Alias("upd")]
        public async Task UpdateCommandAsync(string commandName, [Remainder] string newAction)
        {
            var command = Commands.Instance.CommandsList.Find(x => x.Name.Equals(commandName.Replace(BotSettings.Instance.BotPrefix.ToString(), string.Empty), StringComparison.InvariantCultureIgnoreCase));
            if (command != null)
            {
                await _commandDao.UpdateCommandAsync(command, newAction);
                await ReplyAsync($"Command ``{commandName}`` updated, new action: ``{newAction}``");
                Log.Information($"Command {commandName} updated, new action: ``{newAction}``");
            }
            else
            {
                Log.Warning($"Failed to update command {commandName} as it doesn't exist.");
                await ReplyAsync($"Command ``{commandName}`` does not exist.");
            }
        }

        [Command("list"), Summary("Lists all available commands, both dynamic and module based.")]
        [Alias("l", "all")]
        public async Task ListCommandsAsync()
        {
            var staticCommandNames = _commandService.Modules.Select(module =>
            {
                var moduleCommandNames = module.Commands.Select(c => c.Name);
                return $"{module.Name} *({string.Join(", ", moduleCommandNames)})*";
            });

            var dynamicCommandNames = new List<string>();
            var sortedNames = Commands.Instance.CommandsList
                .Select(command => $"{BotSettings.Instance.BotPrefix}{command.Name}")
                .OrderBy(a => a, StringComparer.Ordinal);

            var i = 0;
            foreach (var command in sortedNames)
            {
                if (i == BotSettings.Instance.DynamicCommandsPerLine)
                {
                    i = 0;
                    dynamicCommandNames.Add(Environment.NewLine);
                }
                dynamicCommandNames.Add(command);
                i++;
            }
            await ReplyAsync($"**Available static commands:**{Environment.NewLine}{string.Join(Environment.NewLine, staticCommandNames)}");
            await ReplyAsync($"**Available dynamic commands:**{Environment.NewLine}{string.Join(" ", dynamicCommandNames)}");
        }
    }
}

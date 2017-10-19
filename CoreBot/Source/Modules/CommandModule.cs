using System;
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
        public async Task AddCommandAsync(string commandName, [Remainder] string commandAction)
        {
            if (commandAction.Length > 0 || !string.IsNullOrEmpty(commandAction))
            {
                var found = Commands.Instance.CommandsList.Find(x => x.Name.Equals(commandName.Replace(BotSettings.Instance.BotPrefix.ToString(), string.Empty), StringComparison.InvariantCultureIgnoreCase));
                if (found == null)
                {
                    await _commandDao.AddCommandAsync(new Command(commandName.Replace(BotSettings.Instance.BotPrefix.ToString(), string.Empty), commandAction));
                    await ReplyAsync($"Command {commandName} added, action: {commandAction}");
                    Log.Information($"Command {commandName} added, action: {commandAction}");
                }
                else
                {
                    Log.Warning($"Could not add command {commandName} since it already exists.");
                    await ReplyAsync($"Command {commandName} already exists. Use {BotSettings.Instance.BotPrefix}command update {commandName} to edit the existing command.");
                }
            }
            else
            {
                Log.Warning($"Could not add command {commandName} because the action was invalid.");
            }
        }

        [Command("delete"), Summary("Deletes a dynamic command.")]
        public async Task DeleteCommandAsync(string commandName)
        {
            var command = Commands.Instance.CommandsList.Find(x => x.Name.Equals(commandName.Replace(BotSettings.Instance.BotPrefix.ToString(), string.Empty), StringComparison.InvariantCultureIgnoreCase));
            if (command != null)
            {
                await _commandDao.DeleteCommandAsync(command);
                await ReplyAsync($"Command deleted: {commandName}");
                Log.Information($"Command deleted: {commandName}");
            }
            else
            {
                Log.Warning($"Failed to delete command {commandName} as it doesn't exist.");
                await ReplyAsync($"Command {commandName} does not exist.");
            }
        }

        [Command("update"), Summary("Updates the action of a dynamic command.")]
        public async Task UpdateCommandAsync(string commandName, [Remainder] string newAction)
        {
            var command = Commands.Instance.CommandsList.Find(x => x.Name.Equals(commandName.Replace(BotSettings.Instance.BotPrefix.ToString(), string.Empty), StringComparison.InvariantCultureIgnoreCase));
            if (command != null)
            {
                await _commandDao.UpdateCommandAsync(command, newAction);
                await ReplyAsync($"Command {commandName} updated, new action: {newAction}");
                Log.Information($"Command {commandName} updated, new action: {newAction}");
            }
            else
            {
                Log.Warning($"Failed to update command {commandName} as it doesn't exist.");
                await ReplyAsync($"Command {commandName} does not exist.");
            }
        }

        [Command("list"), Summary("Lists all available commands, both dynamic and module based.")]
        public async Task ListCommandsAsync()
        {
            var staticCommandNames = _commandService.Modules.Select(module =>
            {
                var moduleCommandNames = module.Commands.Select(c => c.Name);
                return $"{module.Name} *({string.Join(", ", moduleCommandNames)})*";
            });

            var dynamicCommandNames = Commands.Instance.CommandsList
                .Select(x => x.Name)
                .Select(name => $"{BotSettings.Instance.BotPrefix}{name}");

            await ReplyAsync($"**Available static commands:**{Environment.NewLine}{string.Join(Environment.NewLine, staticCommandNames)}");
            await ReplyAsync($"**Available dynamic commands:**{Environment.NewLine}{string.Join(Environment.NewLine, dynamicCommandNames)}");
        }
    }
}

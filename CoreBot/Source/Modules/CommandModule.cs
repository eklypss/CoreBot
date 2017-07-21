using System.Linq;
using System.Threading.Tasks;
using CoreBot.Collections;
using CoreBot.Managers;
using CoreBot.Models;
using CoreBot.Settings;
using Discord.Commands;
using Serilog;

namespace CoreBot.Modules
{
    [Group("command"), Summary("Module for modifying and listing commands.")]
    public class CommandModule : ModuleBase
    {
        private readonly CommandManager commandManager;
        private readonly CommandService commandService;

        public CommandModule(CommandManager cm, CommandService commands)
        {
            commandManager = cm;
            commandService = commands;
        }

        [Command("add"), Summary("Adds a new dynamic command.")]
        public async Task AddCommand(string commandName, [Remainder] string commandAction)
        {
            if (commandAction.Length > 0 || !string.IsNullOrEmpty(commandAction))
            {
                var found = Commands.Instance.CommandsList.Find(x => x.Name == commandName.Replace(BotSettings.Instance.BotPrefix.ToString(), string.Empty));
                if (found == null)
                {
                    await commandManager.AddCommand(new Command(commandName.Replace(BotSettings.Instance.BotPrefix.ToString(), string.Empty), commandAction));
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
        public async Task DeleteCommand(string commandName)
        {
            var command = Commands.Instance.CommandsList.Find(x => x.Name == commandName.Replace(BotSettings.Instance.BotPrefix.ToString(), string.Empty));
            if (command != null)
            {
                await commandManager.DeleteCommand(command);
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
        public async Task UpdateCommand(string commandName, string newAction)
        {
            var command = Commands.Instance.CommandsList.Find(x => x.Name == commandName.Replace(BotSettings.Instance.BotPrefix.ToString(), string.Empty));
            if (command != null)
            {
                await commandManager.UpdateCommand(command, newAction);
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
        public async Task ListCommands()
        {
            var staticCommandNames = commandService.Modules.Select(module =>
            {
                var moduleCommandNames = module.Commands.Select(c => c.Name);
                return $"{module.Name} *({string.Join(", ", moduleCommandNames)})*";
            });
            var dynamicCommandNames = Commands.Instance.CommandsList.Select(x => x.Name);
            await ReplyAsync($"**Available static commands:** {string.Join(", ", staticCommandNames)}");
            await ReplyAsync($"**Available dynamic commands:** {string.Join(", ", dynamicCommandNames)}");
        }
    }
}
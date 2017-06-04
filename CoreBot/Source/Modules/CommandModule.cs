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
        private CommandManager commandManager;
        private CommandService commandService;

        public CommandModule(CommandManager cm, CommandService commands)
        {
            commandManager = cm;
            commandService = commands;
        }

        [Command("add"), Summary("Adds a new dynamic command.")]
        public async void AddCommand(string commandName, [Remainder] string commandAction)
        {
            if (commandAction.Length > 0 || !string.IsNullOrEmpty(commandAction)) await commandManager.AddCommand(new Command(commandName.Replace(BotSettings.Instance.BotPrefix.ToString(), string.Empty), commandAction));
            else Log.Warning($"Could not add command {commandName} because the action was invalid.");
        }

        [Command("delete"), Summary("Deletes a dynamic command.")]
        public async void DeleteCommand(string commandName)
        {
            var command = Commands.Instance.CommandsList.Find(x => x.Name == commandName.Replace(BotSettings.Instance.BotPrefix.ToString(), string.Empty));
            if (command != null) await commandManager.DeleteCommand(command);
            else Log.Warning($"Failed to delete command {commandName} as it doesn't exist.");
        }

        [Command("update"), Summary("Updates the action of a dynamic command.")]
        public async void UpdateCommand(string commandName, string newAction)
        {
            var command = Commands.Instance.CommandsList.Find(x => x.Name == commandName.Replace(BotSettings.Instance.BotPrefix.ToString(), string.Empty));
            if (command != null) await commandManager.UpdateCommand(command, newAction);
            else Log.Warning($"Failed to update command {commandName} as it doesn't exist.");
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
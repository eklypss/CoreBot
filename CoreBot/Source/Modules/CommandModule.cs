using System.Collections.Generic;
using System.Threading.Tasks;
using CoreBot.Collections;
using CoreBot.Models;
using CoreBot.Services;
using CoreBot.Settings;
using Discord.Commands;
using System.Linq;

namespace CoreBot.Modules
{
    [Group("command"), Summary("Module for adding, deleting and listing dynamic commands.")]
    public class CommandModule : ModuleBase
    {

        private CommandManager commandManager;
        private CommandService comService;

        public CommandModule(CommandManager cm, CommandService commands)
        {
            this.commandManager = cm;
            this.comService = commands;
        }
        [Command("add"), Summary("Adds a new dynamic command.")]
        public async Task AddCommand(string commandName, [Remainder] string commandAction)
        {
            await commandManager.AddCommand(new Command(commandName.Replace(BotSettings.Instance.BotPrefix.ToString(), string.Empty), commandAction));
        }

        [Command("delete"), Summary("Deletes a dynamic command.")]
        public async Task DeleteCommand(string commandName)
        {
            var command = Commands.Instance.CommandsList.Find(x => x.Name == commandName.Replace(BotSettings.Instance.BotPrefix.ToString(), string.Empty));
            await commandManager.DeleteCommand(command);
        }

        [Command("list"), Summary("Lists all available dynamic commands.")]
        public async Task ListCommands()
        {
            var commandNames = new List<string>();
            foreach (var module in comService.Modules)
            {
                var moduleCommands = from c in module.Commands select c.Name;
                commandNames.Add($"{module.Name}({string.Join(", ", moduleCommands)})");
            }
            Commands.Instance.CommandsList.ForEach(x => commandNames.Add(x.Name));
            await ReplyAsync($"Available dynamic commands: {string.Join(", ", commandNames)}");
        }
    }
}
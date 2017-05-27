using System.Collections.Generic;
using System.Threading.Tasks;
using CoreBot.Collections;
using CoreBot.Models;
using CoreBot.Services;
using CoreBot.Settings;
using Discord.Commands;

namespace CoreBot.Modules
{
    [Group("command"), Summary("Module for adding, deleting and listing dynamic commands.")]
    public class CommandModule : ModuleBase
    {
        [Command("add"), Summary("Adds a new dynamic command.")]
        public async Task AddCommand(string commandName, [RemainderAttribute] string commandAction)
        {
            CommandManager commandManager = new CommandManager();
            await commandManager.AddCommand(new Command(commandName.Replace(BotSettings.Instance.BotPrefix.ToString(), string.Empty), commandAction));
        }

        [Command("delete"), Summary("Deletes a dynamic command.")]
        public async Task DeleteCommand(string commandName)
        {
            CommandManager commandManager = new CommandManager();
            var command = Commands.Instance.CommandsList.Find(x => x.Name == commandName.Replace(BotSettings.Instance.BotPrefix.ToString(), string.Empty));
            await commandManager.DeleteCommand(command);
        }

        [Command("list"), Summary("Lists all available dynamic commands.")]
        public async Task ListCommands()
        {
            var commandNames = new List<string>();
            Commands.Instance.CommandsList.ForEach(x => commandNames.Add(x.Name));
            await ReplyAsync($"Available dynamic commands: {string.Join(", ", commandNames)}");
        }
    }
}
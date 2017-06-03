using System.Linq;
using System.Threading.Tasks;
using CoreBot.Collections;
using CoreBot.Managers;
using CoreBot.Models;
using CoreBot.Settings;
using Discord.Commands;

namespace CoreBot.Modules
{
    [Group("command"), Summary("Module for adding, deleting and listing commands.")]
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
        public void AddCommand(string commandName, [Remainder] string commandAction)
        {
            commandManager.AddCommand(new Command(commandName.Replace(BotSettings.Instance.BotPrefix.ToString(), string.Empty), commandAction));
        }

        [Command("delete"), Summary("Deletes a dynamic command.")]
        public void DeleteCommand(string commandName)
        {
            var command = Commands.Instance.CommandsList.Find(x => x.Name == commandName.Replace(BotSettings.Instance.BotPrefix.ToString(), string.Empty));
            commandManager.DeleteCommand(command);
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
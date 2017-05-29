﻿using System.Collections.Generic;
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

        [Command("list"), Summary("Lists all available commands, both dynamic and module based.")]
        public async Task ListCommands()
        {
            var dynamicCommandNames = new List<string>();
            var staticCommandNames = new List<string>();
            foreach (var module in commandService.Modules)
            {
                var moduleCommandNames = new List<string>();
                module.Commands.ToList().ForEach(x => moduleCommandNames.Add(x.Name));
                staticCommandNames.Add($"{module.Name} *({string.Join(", ", moduleCommandNames)})*");
            }
            Commands.Instance.CommandsList.ForEach(x => dynamicCommandNames.Add(x.Name));
            await ReplyAsync($"**Available static commands:** {string.Join(", ", staticCommandNames)}");
            await ReplyAsync($"**Available dynamic commands:** {string.Join(", ", dynamicCommandNames)}");
        }
    }
}
using CoreBot.Collections;
using CoreBot.Managers;
using CoreBot.Modules;
using CoreBot.Settings;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace CoreBot.Handlers
{
    public class CommandHandler
    {
        private DiscordSocketClient client;
        private CommandService commandService;
        private IServiceCollection services;
        private IServiceProvider serviceProvider;

        public async Task InstallCommands(DiscordSocketClient discordClient)
        {
            client = discordClient;
            commandService = new CommandService();
            var drinkManager = await DrinkManager.Create();
            services = new ServiceCollection();
            services.AddSingleton(new CommandManager());
            if (drinkManager != null) services.AddSingleton(drinkManager);
            serviceProvider = services.BuildServiceProvider();
            await commandService.AddModulesAsync(Assembly.GetEntryAssembly());
            client.MessageReceived += HandleCommandAsync;
        }

        private async Task HandleCommandAsync(SocketMessage message)
        {
            Log.Information($"{message.Author.Username} ({message.Author}): {message.Content}");
            if (message == null)
            {
                Log.Warning($"Message {message.Id} is not from a valid user.");
                return;
            }
            else
            {
                var userMessage = (SocketUserMessage)message;
                var context = new SocketCommandContext(client, userMessage);
                int argPos = 0;
                if (userMessage.HasCharPrefix(BotSettings.Instance.BotPrefix, ref argPos))
                {
                    var result = await commandService.ExecuteAsync(context, argPos, serviceProvider);
                    if (!result.IsSuccess) // Module was not found, check for dynamic commands.
                    {
                        Log.Information(result.ToString());
                        bool matchFound = false;
                        foreach (var command in Commands.Instance.CommandsList)
                        {
                            if (userMessage.Content == $"{BotSettings.Instance.BotPrefix}{command.Name}")
                            {
                                await userMessage.Channel.SendMessageAsync(command.Action);
                                matchFound = true;
                                break;
                            }
                        }
                        if (!matchFound)
                        {
                            // If command was found but failed to execute, send error message.
                            if (result.Error != CommandError.UnknownCommand) await userMessage.Channel.SendMessageAsync(result.ToString());
                        }
                    }
                }
            }
        }
    }
}
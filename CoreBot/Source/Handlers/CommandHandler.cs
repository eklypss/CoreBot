using System;
using System.Reflection;
using System.Threading.Tasks;
using CoreBot.Collections;
using CoreBot.Managers;
using CoreBot.Modules;
using CoreBot.Services;
using CoreBot.Settings;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace CoreBot.Handlers
{
    public class CommandHandler
    {
        private DiscordSocketClient client;
        private CommandService commandService;
        private IServiceCollection services;
        private IServiceProvider serviceProvider;

        public async Task InstallAsync(DiscordSocketClient discordClient)
        {
            client = discordClient;
            commandService = new CommandService();
            var drinkManager = await DrinkManager.CreateAsync();
            services = new ServiceCollection();

            // Add services to the ServiceCollection
            services.AddSingleton(new CommandManager());
            services.AddSingleton(new WeatherService());
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
                        var matchFound = false;
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
                            if (result.Error != CommandError.UnknownCommand)
                            {
                                await userMessage.Channel.SendMessageAsync(result.ToString());
                            }
                        }
                    }
                }
            }
        }
    }
}
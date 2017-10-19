using System;
using System.Reflection;
using System.Threading.Tasks;
using CoreBot.Api;
using CoreBot.Collections;
using CoreBot.Database.Dao;
using CoreBot.Interfaces;
using CoreBot.Models;
using CoreBot.Services;
using CoreBot.Settings;
using Discord.Commands;
using Discord.WebSocket;
using epnetcore;
using FluentScheduler;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace CoreBot.Handlers
{
    public class CommandHandler : ICommandHandler
    {
        private DiscordSocketClient _client;
        private CommandService _commandService;
        private IServiceCollection _services;
        private IServiceProvider _serviceProvider;
        private OldLinkService _oldLinkService;
        private Spammer _spammer;

        public async Task InstallAsync(DiscordSocketClient discordClient,
            CommandService commandService)
        {
            _client = discordClient;
            _commandService = commandService;
            var server = new Server(_client);
            server.Start();

            var drinkDao = await DrinkDao.CreateAsync();
            _services = new ServiceCollection();

            var messageService = new MessageService(_client);
            var eventService = new EventService(messageService, new EventDao());

            // Add services to the ServiceCollection
            _services.AddSingleton(new CommandDao());
            _services.AddSingleton(new QuoteService());
            _services.AddSingleton(new WeatherService());
            _services.AddSingleton(new UrbanService());
            _services.AddSingleton(new AromaService(new AromaDao()));
            _services.AddSingleton(new EPClient(BotSettings.Instance.EPAPIKey));
            _services.AddSingleton(messageService);
            _services.AddSingleton(eventService);
            _services.AddSingleton(new StartupTime());
            if (drinkDao != null) _services.AddSingleton(drinkDao);
            _oldLinkService = new OldLinkService();
            _spammer = new Spammer();

            JobManager.Initialize(eventService);

            // Build ServiceProvider and add modules
            _serviceProvider = _services.BuildServiceProvider();
            await _commandService.AddModulesAsync(Assembly.GetEntryAssembly());
            _client.MessageReceived += HandleCommandAsync;
            Log.Debug("CommandHandler installed.");
        }

        public async Task HandleCommandAsync(SocketMessage message)
        {
            Log.Information($"{message.Author.Username} ({message.Author}): {message.Content}");
            if (message.Author == null) // This should never happen, but just in case.
            {
                Log.Warning($"Message {message.Id} is not from a valid user.");
            }
            else
            {
                var userMessage = (SocketUserMessage)message;
                await _oldLinkService.CheckAsync(userMessage);
                await _spammer.CheckSpamAsync(userMessage);
                var context = new SocketCommandContext(_client, userMessage);
                int argPos = 0;
                if (userMessage.HasCharPrefix(BotSettings.Instance.BotPrefix, ref argPos))
                {
                    var result = await _commandService.ExecuteAsync(context, argPos, _serviceProvider);
                    if (!result.IsSuccess) // Module was not found, check for dynamic commands.
                    {
                        Log.Information(result.ToString());
                        var matchFound = false;
                        foreach (var command in Commands.Instance.CommandsList)
                        {
                            if (userMessage.Content.Equals($"{BotSettings.Instance.BotPrefix}{command.Name}", StringComparison.InvariantCultureIgnoreCase))
                            {
                                await userMessage.Channel.SendMessageAsync(command.Action);
                                matchFound = true;
                                break;
                            }
                        }
                        if (!matchFound && result.Error != CommandError.UnknownCommand)
                        {
                            // If command was found but failed to execute, send error message.
                            await userMessage.Channel.SendMessageAsync(result.ToString());
                            Log.Error($"Additional information: {result.ErrorReason}, {result.Error.Value}");
                        }
                    }
                }
            }
        }
    }
}

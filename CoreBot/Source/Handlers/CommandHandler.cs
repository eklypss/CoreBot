﻿using System;
using System.Linq;
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
        private ForbiddenMsgService _forbiddenMsgService;
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
            _services.AddSingleton(await CommentService.Create());
            _services.AddSingleton(new CommandDao());
            _services.AddSingleton(new QuoteService());
            _services.AddSingleton(new WeatherService());
            _services.AddSingleton(new UrbanService());
            _services.AddSingleton(new NHLService());
            _services.AddSingleton(new AromaService(new AromaDao()));
            _services.AddSingleton(new EPClient(BotSettings.Instance.EPAPIKey));
            _services.AddSingleton(messageService);
            _services.AddSingleton(eventService);
            _services.AddSingleton(new WolframService());
            _services.AddSingleton(new StartupTime());
            _services.AddSingleton(new F1Service());
            _services.AddSingleton(drinkDao);
            _oldLinkService = new OldLinkService();
            _forbiddenMsgService = new ForbiddenMsgService();
            _spammer = new Spammer();

            JobManager.Initialize(eventService);

            // Build ServiceProvider and add modules
            _serviceProvider = _services.BuildServiceProvider();
            await _commandService.AddModulesAsync(Assembly.GetEntryAssembly(), _serviceProvider);
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
                await _forbiddenMsgService.CheckMsgAsync(userMessage);
                var context = new SocketCommandContext(_client, userMessage);

                if (userMessage.Content.Length > 0 && userMessage.Content[0] == BotSettings.Instance.BotPrefix)
                {
                    if (userMessage.Author.IsBot) await userMessage.DeleteAsync();
                    await userMessage.Channel.TriggerTypingAsync();
                    await ExecuteCommand(userMessage, context, userMessage.Content);
                }
                else
                {
                    await _spammer.CheckSpamAsync(userMessage);
                }
            }
        }

        private async Task ExecuteCommand(SocketMessage userMessage, ICommandContext context, string contents)
        {
            var result = await _commandService.ExecuteAsync(context, contents.Substring(1), _serviceProvider);

            // if command throws exception, result is still successfull
            if (result.IsSuccess) return;

            Log.Information(result.ToString());

            var dynamicCommand = Commands.Instance.CommandsList
                .Find(command => contents.Equals($"{BotSettings.Instance.BotPrefix}{command.Name}",
                    StringComparison.InvariantCultureIgnoreCase));

            if (dynamicCommand != null)
            {
                string formattedAction = dynamicCommand.Action.Replace(BotSettings.Instance.SelfHotstring,
                    string.Format("<@!{0}>", userMessage.Author.Id));

                if (formattedAction.StartsWith(BotSettings.Instance.BotPrefix))
                {
                    await ExecuteCommand(userMessage, context, formattedAction);
                }
                else
                {
                    await userMessage.Channel.SendMessageAsync(formattedAction);
                }
            }
            else if (result.Error == CommandError.UnknownCommand)
            {
                await userMessage.Channel.SendMessageAsync("no command " + contents);
            }
            else
            {
                // If command was found but failed to execute, send error message.
                await userMessage.Channel.SendMessageAsync(result.ToString());
                Log.Error($"Additional information: {result.ErrorReason} {result.Error.Value}");
            }
        }
    }
}

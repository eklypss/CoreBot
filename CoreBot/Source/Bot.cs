﻿using System.Threading.Tasks;
using CoreBot.Exceptions;
using CoreBot.Helpers;
using CoreBot.Services;
using CoreBot.Settings;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Serilog;

namespace CoreBot
{
    internal class Bot
    {
        private readonly HandlerService _handler;
        private DiscordSocketClient _client;

        public static void Main() => new Bot().MainAsync().GetAwaiter().GetResult();

        private Bot()
        {
            _handler = new HandlerService();
        }

        private async Task MainAsync()
        {
            LogHelper.CreateLogger(BotSettings.Instance.LogToFile);
            await FileHelper.CheckFilesAsync();

            if (!string.IsNullOrWhiteSpace(BotSettings.Instance.BotToken))
            {
                _client = new DiscordSocketClient
                (
                    BotSettings.CreateDiscordConfig(BotSettings.Instance.DiscordnetLoglevel)
                );

                await _client.LoginAsync(TokenType.Bot, BotSettings.Instance.BotToken);
                await _client.StartAsync();

                var commandService = new CommandService
                (
                    BotSettings.CreateCommandConfig(BotSettings.Instance.DiscordnetLoglevel)
                );

                // Install handlers
                _handler.LogHandler.Install(_client, commandService);
                await _handler.CommandHandler.InstallAsync(_client, commandService);
            }
            else
            {
                Log.Error("Bot token is invalid, cannot connect.");
                Log.Error($"Change your bot token in the following .json file: {BotSettings.Instance.SettingsFile}.");
                throw new CoreBotException("Bot token is null or empty.");
            }
            await Task.Delay(-1);
        }
    }
}

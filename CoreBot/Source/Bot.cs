using System.Threading.Tasks;
using CoreBot.Collections;
using CoreBot.Handlers;
using CoreBot.Models;
using CoreBot.Services;
using CoreBot.Settings;
using Discord;
using Discord.WebSocket;
using Serilog;

namespace CoreBot
{
    internal class Bot
    {
        private CommandHandler commandHandler;
        private DiscordSocketClient client;

        public static void Main(string[] args) => new Bot().MainAsync().GetAwaiter().GetResult();

        private Bot()
        {
            LogManager.CreateLogger(BotSettings.Instance.LogToFile);
            commandHandler = new CommandHandler();
        }

        private async Task MainAsync()
        {
            await FileManager.CheckFiles();
            if (!string.IsNullOrWhiteSpace(BotSettings.Instance.BotToken))
            {
                client = new DiscordSocketClient();
                await client.LoginAsync(TokenType.Bot, BotSettings.Instance.BotToken);
                await client.StartAsync();

                client.Log += MessageLogger;
                await commandHandler.InstallCommands(client);
            }
            else
            {
                Log.Error("Bot token is invalid, cannot connect.");
                Log.Error($"Change your bot token in the following .config file: {BotSettings.Instance.SettingsFile}.");
            }

            await Task.Delay(-1);
        }

        /// <summary>
        /// Output messages by the main <see cref="IDiscordClient"/>. Separated from all other log
        /// messages on purpose.
        /// </summary>
        private async Task MessageLogger(LogMessage message)
        {
            switch (message.Severity)
            {
                case LogSeverity.Debug:
                case LogSeverity.Info:
                case LogSeverity.Verbose:
                {
                    Log.Information($"[Discord] {message.Message}");
                    break;
                }
                case LogSeverity.Critical:
                case LogSeverity.Error:
                case LogSeverity.Warning:
                {
                    Log.Error($"[Discord] [{message.Severity}] {message.Message} {message.Exception} {message.Source}");
                    break;
                }
            }
        }
    }
}
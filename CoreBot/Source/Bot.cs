using CoreBot.Collections;
using CoreBot.Models;
using CoreBot.Services;
using CoreBot.Settings;
using Discord;
using Discord.WebSocket;
using Serilog;
using System.Threading.Tasks;

namespace CoreBot
{
    internal class Bot
    {
        private CommandManager commandManager;

        public static void Main(string[] args) => new Bot().MainAsync().GetAwaiter().GetResult();

        private Bot()
        {
            LogManager.CreateLogger(BotSettings.Instance.LogToFile);
            commandManager = new CommandManager();
        }

        private async Task MainAsync()
        {
            await FileManager.CheckFiles();
            if (!string.IsNullOrWhiteSpace(BotSettings.Instance.BotToken))
            {
                await Clients.Instance.MainClient.LoginAsync(TokenType.Bot, BotSettings.Instance.BotToken);
                await Clients.Instance.MainClient.StartAsync();

                Clients.Instance.MainClient.MessageReceived += MessageReceived;
                Clients.Instance.MainClient.Log += MessageLogger;
            }
            else
            {
                Log.Error("Bot token is ínvalid, cannot connect.");
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

        private async Task MessageReceived(SocketMessage message)
        {
            Log.Information($"[{message.Channel}] {message.Author.Username}: {message.Content}");
            if (message.Content.StartsWith(BotSettings.Instance.BotPrefix) && !message.Author.IsBot)
            {
                var commandParameters = message.Content.Split(' ');
                string commandName = commandParameters[0].Replace(BotSettings.Instance.BotPrefix, string.Empty);
                foreach (var command in Commands.Instance.CommandsList)
                {
                    if (commandName == command.Name)
                    {
                        await message.Channel.SendMessageAsync(command.Action);
                        break;
                    }
                }
            }

            // Temporary !addcom for debugging purposes.
            if (message.Content.StartsWith($"{BotSettings.Instance.BotPrefix}addcom "))
            {
                var commandParameters = message.Content.Split(' ');
                if (commandParameters.Length >= 2)
                {
                    string commandName = commandParameters[1].Replace(BotSettings.Instance.BotPrefix, string.Empty);
                    string commandAction = message.Content.Substring(message.Content.LastIndexOf(commandParameters[1]) + commandParameters[1].Length + 1);
                    Log.Debug($"Name: {commandName}, action: {commandAction}");
                    await commandManager.AddCommand(new Command(commandName, commandAction, message.Author.Username));
                }
                else await message.Channel.SendMessageAsync($"Usage: !test {BotSettings.Instance.BotPrefix}[command] [action]");
            }
        }
    }
}
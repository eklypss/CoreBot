using System.Threading.Tasks;
using CoreBot.Handlers;
using CoreBot.Helpers;
using CoreBot.Settings;
using Discord;
using Discord.WebSocket;
using Serilog;

namespace CoreBot
{
    internal class Bot
    {
        private CommandHandler commandHandler;
        private MessageHandler messageHandler;
        private LogHandler logHandler;
        private DiscordSocketClient client;

        public static void Main(string[] args) => new Bot().MainAsync().GetAwaiter().GetResult();

        private Bot()
        {
            LogHelper.CreateLogger(BotSettings.Instance.LogToFile);
            commandHandler = new CommandHandler();
            messageHandler = new MessageHandler();
            logHandler = new LogHandler();
        }

        private async Task MainAsync()
        {
            await FileHelper.CheckFiles();
            if (!string.IsNullOrWhiteSpace(BotSettings.Instance.BotToken))
            {
                client = new DiscordSocketClient();
                await client.LoginAsync(TokenType.Bot, BotSettings.Instance.BotToken);
                await client.StartAsync();

                await logHandler.Install(client);
                await commandHandler.InstallCommands(client);
                await messageHandler.Install(client);
            }
            else
            {
                Log.Error("Bot token is invalid, cannot connect.");
                Log.Error($"Change your bot token in the following .config file: {BotSettings.Instance.SettingsFile}.");
            }

            await Task.Delay(-1);
        }
    }
}
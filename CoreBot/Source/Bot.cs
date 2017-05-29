using System.Threading.Tasks;
using CoreBot.Helpers;
using CoreBot.Service;
using CoreBot.Settings;
using Discord;
using Discord.WebSocket;
using Serilog;

namespace CoreBot
{
    internal class Bot
    {
        private HandlerService handler;
        private DiscordSocketClient client;

        public static void Main(string[] args) => new Bot().MainAsync().GetAwaiter().GetResult();

        private Bot()
        {
            LogHelper.CreateLogger(BotSettings.Instance.LogToFile);
            handler = new HandlerService();
        }

        private async Task MainAsync()
        {
            await handler.CreateHandlers();
            await FileHelper.CheckFiles();
            if (!string.IsNullOrWhiteSpace(BotSettings.Instance.BotToken))
            {
                client = new DiscordSocketClient();
                await client.LoginAsync(TokenType.Bot, BotSettings.Instance.BotToken);
                await client.StartAsync();

                // Install handlers
                await handler.LogHandler.Install(client);
                await handler.CommandHandler.InstallCommands(client);
                await handler.MessageHandler.Install(client);
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
using System.Threading.Tasks;
using CoreBot.Helpers;
using CoreBot.Service;
using CoreBot.Settings;
using Discord;
using Discord.WebSocket;
using Serilog;
using CoreBot.Source.Helpers;

namespace CoreBot
{
    internal class Bot
    {
        private HandlerService handler;
        private DiscordSocketClient client;

        public static void Main(string[] args) => new Bot().MainAsync().GetAwaiter().GetResult();

        private Bot()
        {
            handler = new HandlerService();
        }

        private async Task MainAsync()
        {
            await LogHelper.CreateLogger(BotSettings.Instance.LogToFile);
            await FileHelper.CheckFiles();

            if (!string.IsNullOrWhiteSpace(BotSettings.Instance.BotToken))
            {
                client = new DiscordSocketClient();
                await handler.CreateHandlers();
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
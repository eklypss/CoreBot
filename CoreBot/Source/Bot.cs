using System.Threading.Tasks;
using CoreBot.Exceptions;
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
        private readonly HandlerService handler;
        private DiscordSocketClient client;

        public static void Main(string[] args) => new Bot().MainAsync().GetAwaiter().GetResult();

        private Bot()
        {
            handler = new HandlerService();
        }

        private async Task MainAsync()
        {
            await LogHelper.CreateLoggerAsync(BotSettings.Instance.LogToFile);
            await FileHelper.CheckFilesAsync();

            if (!string.IsNullOrWhiteSpace(BotSettings.Instance.BotToken))
            {
                client = new DiscordSocketClient();
                await client.LoginAsync(TokenType.Bot, BotSettings.Instance.BotToken);
                await client.StartAsync();

                // Install handlers
                await handler.LogHandler.InstallAsync(client);
                await handler.CommandHandler.InstallAsync(client);
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
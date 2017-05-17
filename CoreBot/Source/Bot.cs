using CoreBot.Collections;
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
        public static void Main(string[] args) => new Bot().MainAsync().GetAwaiter().GetResult();

        private Bot()
        {
            LogManager.CreateLogger(BotSettings.Instance.LogToFile);
            FileManager.CheckFiles();
        }

        private async Task MainAsync()
        {
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
            }

            await Task.Delay(-1);
        }

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
                foreach (var command in Commands.Instance.CommandsList)
                {
                    if (message.Content.StartsWith($"{BotSettings.Instance.BotPrefix}{command.Name}"))
                    {
                        await message.Channel.SendMessageAsync(command.Action);
                    }
                }
                if (message.Content.StartsWith($"{BotSettings.Instance.BotPrefix}ew"))
                {
                    await message.Channel.SendMessageAsync("Yeah, fuck off buddy we absolutely need more <:ew:230406264385961986> duos. Fuckin every time this kid steps in the battleground someone dies. kids fuckin dirt nasty man. Does fuckin jone have 14 kills this season I dont fuckin think so bud. I'm fuckin tellin ya Evil 'Military Man' Walrus is pottin 50 in '17 fuckin callin it right now. Clap bombs, fuck moms, wheel, snipe, and fuckin celly boys fuck");
                }
            }
        }
    }
}
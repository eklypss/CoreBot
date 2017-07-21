using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Serilog;

namespace CoreBot.Handlers
{
    public class LogHandler
    {
        private DiscordSocketClient client;

        public async Task Install(DiscordSocketClient discordClient)
        {
            client = discordClient;
            client.Log += HandleLoggingAsync;
            await Task.CompletedTask;
        }

        /// <summary>
        /// Output messages by the main <see cref="IDiscordClient"/>. Separated from all other log
        /// messages on purpose.
        /// </summary>
        private async Task HandleLoggingAsync(LogMessage message)
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
            await Task.CompletedTask;
        }
    }
}
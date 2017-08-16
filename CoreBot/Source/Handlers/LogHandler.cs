using System.Threading.Tasks;
using CoreBot.Interfaces;
using Discord;
using Discord.WebSocket;
using Serilog;

namespace CoreBot.Handlers
{
    public class LogHandler : ILogHandler
    {
        private DiscordSocketClient _client;

        public async Task InstallAsync(DiscordSocketClient client)
        {
            _client = client;
            _client.Log += HandleLoggingAsync;
            Log.Debug("LogHandler installed.");
            await Task.CompletedTask;
        }

        /// <summary>
        /// Output messages by the main <see cref="IDiscordClient"/>. Separated from all other log
        /// messages on purpose.
        /// </summary>
        /// <param name="message">todo: describe message parameter on HandleLoggingAsync</param>
        public async Task HandleLoggingAsync(LogMessage message)
        {
            switch (message.Severity)
            {
                case LogSeverity.Debug:
                case LogSeverity.Verbose:
                case LogSeverity.Info:
                {
                    Log.Information($"[Discord] {message.Message}");
                    break;
                }
                case LogSeverity.Critical:
                case LogSeverity.Error:
                {
                    Log.Error($"[Discord] [{message.Severity}] {message.Message} {message.Exception} {message.Source}");
                    break;
                }
                case LogSeverity.Warning:
                {
                    Log.Warning($"[Discord] [{message.Severity}] {message.Message} {message.Exception} {message.Source}");
                    break;
                }
            }
            await Task.CompletedTask;
        }
    }
}
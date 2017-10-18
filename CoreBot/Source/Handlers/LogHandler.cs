using System.Threading.Tasks;
using CoreBot.Interfaces;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Serilog;

namespace CoreBot.Handlers
{
    public class LogHandler : ILogHandler
    {
        private DiscordSocketClient _client;

        public void Install(DiscordSocketClient client, CommandService commandService)
        {
            _client = client;
            _client.Log += PrintDiscordMessage;
            commandService.Log += PrintDiscordMessage;
            Log.Debug("LogHandler installed.");
        }

        /// <summary>
        /// Output messages by the main <see cref="IDiscordClient"/>. Separated from all other log
        /// messages on purpose. Logging to file is blocking so it should be executed on another thread.
        /// </summary>
        /// <param name="message">todo: describe message parameter on HandleLoggingAsync</param>
        public async Task PrintDiscordMessage(LogMessage message)
        {
            switch (message.Severity)
            {
                case LogSeverity.Debug:
                case LogSeverity.Verbose:
                case LogSeverity.Info:
                {
                    await Task.Run(() => Log.Information($"[Discord] {message.Message}"));
                    break;
                }
                case LogSeverity.Critical:
                case LogSeverity.Error:
                {
                    await Task.Run(() =>
                    {
                        Log.Error($"[Discord] [{message.Severity}] {message.Message} " +
                            $"{message.Exception} {message.Source}");
                    });
                    break;
                }
                case LogSeverity.Warning:
                {
                    await Task.Run(() =>
                    {
                        Log.Warning($"[Discord] [{message.Severity}] {message.Message} " +
                            $"{message.Exception} {message.Source}");
                    });
                    break;
                }
            }
        }
    }
}
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace CoreBot.Interfaces
{
    public interface ILogHandler
    {
        Task InstallAsync(DiscordSocketClient client);

        Task HandleLoggingAsync(LogMessage message);
    }
}
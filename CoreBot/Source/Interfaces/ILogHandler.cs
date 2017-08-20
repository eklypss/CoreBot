using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace CoreBot.Interfaces
{
    public interface ILogHandler
    {
        void Install(DiscordSocketClient client);

        Task PrintDiscordMessage(LogMessage message);
    }
}
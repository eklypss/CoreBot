using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace CoreBot.Interfaces
{
    public interface ILogHandler
    {
        void Install(DiscordSocketClient client, CommandService commandService);

        Task PrintDiscordMessage(LogMessage message);
    }
}
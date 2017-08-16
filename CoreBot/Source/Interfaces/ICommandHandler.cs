using System.Threading.Tasks;
using Discord.WebSocket;

namespace CoreBot.Interfaces
{
    public interface ICommandHandler
    {
        Task InstallAsync(DiscordSocketClient discordClient);

        Task HandleCommandAsync(SocketMessage message);
    }
}
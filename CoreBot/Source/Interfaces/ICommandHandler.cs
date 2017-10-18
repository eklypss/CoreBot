using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;

namespace CoreBot.Interfaces
{
    public interface ICommandHandler
    {
        Task InstallAsync(DiscordSocketClient discordClient, CommandService commandService);

        Task HandleCommandAsync(SocketMessage message);
    }
}
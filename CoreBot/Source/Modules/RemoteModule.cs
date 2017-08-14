using System.Threading.Tasks;
using CoreBot.Services;
using Discord.Commands;

namespace CoreBot.Modules
{
    public class RemoteModule : ModuleBase
    {
        private readonly MessageService _messageService;

        public RemoteModule(MessageService messageService)
        {
            _messageService = messageService;
        }

        [Command("remote"), Summary("Allows users to send messages to the main channel in the main guild, specified in the BotSettings from other guilds or channels.")]
        public async Task SendRemoteMessage([Remainder] string message)
        {
            await _messageService.SendMessageToDefaultChannelAsync(message);
        }
    }
}
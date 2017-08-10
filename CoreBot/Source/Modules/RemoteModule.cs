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

        [Command("remote")]
        public async Task SendRemoteMessage([Remainder] string message)
        {
            await _messageService.SendMessageToDefaultChannelAsync(message);
        }
    }
}
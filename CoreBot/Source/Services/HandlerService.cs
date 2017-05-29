using System.Threading.Tasks;
using CoreBot.Handlers;
using CoreBot.Interface;

namespace CoreBot.Service
{
    public class HandlerService : IHandlerService
    {
        public CommandHandler CommandHandler { get; set; }
        public LogHandler LogHandler { get; set; }
        public MessageHandler MessageHandler { get; set; }

        public async Task CreateHandlers()
        {
            CommandHandler = new CommandHandler();
            LogHandler = new LogHandler();
            MessageHandler = new MessageHandler();
            await Task.CompletedTask;
        }
    }
}
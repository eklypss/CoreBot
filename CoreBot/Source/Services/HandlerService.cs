using CoreBot.Handlers;
using CoreBot.Interfaces;
using System.Threading.Tasks;

namespace CoreBot.Service
{
    public class HandlerService : IHandlerService
    {
        public CommandHandler CommandHandler { get; set; }
        public LogHandler LogHandler { get; set; }

        public async Task CreateHandlers()
        {
            CommandHandler = new CommandHandler();
            LogHandler = new LogHandler();
        }
    }
}
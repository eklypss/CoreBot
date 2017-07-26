using CoreBot.Handlers;
using CoreBot.Interfaces;

namespace CoreBot.Services
{
    public class HandlerService : IHandlerService
    {
        public CommandHandler CommandHandler { get; set; }
        public LogHandler LogHandler { get; set; }

        public HandlerService()
        {
            CommandHandler = new CommandHandler();
            LogHandler = new LogHandler();
        }
    }
}
using CoreBot.Handlers;
using CoreBot.Interfaces;

namespace CoreBot.Service
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
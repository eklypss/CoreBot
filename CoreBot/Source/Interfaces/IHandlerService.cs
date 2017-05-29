using System.Threading.Tasks;
using CoreBot.Handlers;

namespace CoreBot.Interfaces
{
    public interface IHandlerService
    {
        CommandHandler CommandHandler { get; set; }
        LogHandler LogHandler { get; set; }
        MessageHandler MessageHandler { get; set; }

        Task CreateHandlers();
    }
}
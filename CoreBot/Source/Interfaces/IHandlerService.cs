using CoreBot.Handlers;
using System.Threading.Tasks;

namespace CoreBot.Interfaces
{
    public interface IHandlerService
    {
        CommandHandler CommandHandler { get; set; }
        LogHandler LogHandler { get; set; }

        Task CreateHandlers();
    }
}
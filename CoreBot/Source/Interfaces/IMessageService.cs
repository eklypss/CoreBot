using System.Threading.Tasks;

namespace CoreBot.Interfaces
{
    public interface IMessageService
    {
        Task SendMessageToDefaultChannelAsync(string message);
    }
}

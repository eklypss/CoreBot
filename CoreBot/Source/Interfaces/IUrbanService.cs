using System.Threading.Tasks;
using CoreBot.Models.Urban;

namespace CoreBot.Interfaces
{
    public interface IUrbanService
    {
        Task<UrbanResponse> GetUrbanQuotesAsync(string searchTerm);

        string ParseQuotes(UrbanResponse response);
    }
}
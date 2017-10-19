using System;
using System.Threading.Tasks;

namespace CoreBot.Interfaces
{
    public interface IWeatherService
    {
        Task<string> GetWeatherDataAsync(string location);

        Task<string> FmiAsync(string location);

        string CreateWeatherMessage(string location, object temp, string country, string status, object wind, DateTime timestamp);
    }
}

using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CoreBot.Interfaces;
using CoreBot.Models.Urban;
using CoreBot.Settings;
using Newtonsoft.Json;
using Serilog;

namespace CoreBot.Services
{
    public class UrbanService : IUrbanService
    {
        public async Task<UrbanResponse> GetUrbanQuotesAsync(string searchTerm)
        {
            using (var http = new HttpClient())
            {
                Log.Information($"Getting definitions for the given term: {searchTerm}.");
                http.DefaultRequestHeaders.Add("X-Mashape-Key", BotSettings.Instance.UrbanMashapeKey);
                http.DefaultRequestHeaders.Add("Accept", "text/plain");

                var result = await http.GetStringAsync(string.Format(DefaultValues.URBAN_API_URL, searchTerm));
                var response = JsonConvert.DeserializeObject<UrbanResponse>(result);
                Log.Information($"{response.Definitions.Count} definition(s) found for the term: {searchTerm}.");
                return response;
            }
        }

        public string ParseQuotes(UrbanResponse response)
        {
            return response.Definitions.Aggregate(new StringBuilder(), (sb, cur) =>
            {
                string row = $"**[{cur.Word}]** *{cur.Description}*\n";
                if (sb.Length + row.Length <= 2000)
                    sb.Append(row);
                return sb;
            }).ToString();
        }
    }
}
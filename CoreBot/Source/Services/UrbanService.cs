using System.Collections.Generic;
using System.Net.Http;
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

                var result = await http.GetAsync($"https://mashape-community-urban-dictionary.p.mashape.com/define?term={searchTerm}");
                var response = JsonConvert.DeserializeObject<UrbanResponse>(await result.Content.ReadAsStringAsync());
                Log.Information($"{response.Definitions.Count} definition(s) found for the term: {searchTerm}.");
                return response;
            }
        }

        public async Task<IEnumerable<string>> ParseQuotesAsync(UrbanResponse response)
        {
            var list = new List<string>();
            foreach (var definition in response.Definitions)
            {
                list.Add($"**[{definition.Word}]** *{definition.Description}*");
            }
            return list;
        }
    }
}
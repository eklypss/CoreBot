﻿using System.Net.Http;
using System.Linq;
using System.Threading.Tasks;
using CoreBot.Interfaces;
using CoreBot.Models.Urban;
using CoreBot.Settings;
using Newtonsoft.Json;
using Serilog;
using System.Text;

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

                var result = await http.GetStringAsync($"https://mashape-community-urban-dictionary.p.mashape.com/define?term={searchTerm}");
                var response = JsonConvert.DeserializeObject<UrbanResponse>(result);
                Log.Information($"{response.Definitions.Count} definition(s) found for the term: {searchTerm}.");
                return response;
            }
        }

        public string ParseQuotesAsync(UrbanResponse response)
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
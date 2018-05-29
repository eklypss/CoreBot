using System;
using System.Net.Http;
using System.Threading.Tasks;
using CoreBot.Models.Wolfram;
using CoreBot.Settings;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;

namespace CoreBot.Services
{
    public class WolframService
    {
        private static readonly HttpClient _http = new HttpClient();

        public async Task<QueryResult> GetWolframAnswerAsync(string question)
        {
            if (string.IsNullOrEmpty(question)) throw new ArgumentException("Question cannot be null or empty.");
            Log.Information($"Getting answer for the given question: {question}.");

            var json = await _http.GetStringAsync(string.Format(DefaultValues.WOLFRAM_API_URL, BotSettings.Instance.WolframAppID, question));
            var result = json.Replace(@"\\:", @"\u");
            WolframData response;

            try
            {
                response = JsonConvert.DeserializeObject<WolframData>(result);
            }
            catch (Exception e)
            {
                Log.Error($"failed to decode json '{result}': {e.ToString()}");
                return null;
            }

            if (response.QueryResult.Success && response.QueryResult.Pods.Count > 0)
            {
                Log.Information($"{response.QueryResult.Pods.Count} pods(s) found for the question: {question}.");
                return response.QueryResult;
            }
            else
            {
                Log.Error($"Failed to get answer to the question: {question}.");
                return null;
            }
        }
    }
}

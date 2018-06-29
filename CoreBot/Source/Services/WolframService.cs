using System;
using System.Net.Http;
using System.Threading.Tasks;
using CoreBot.Settings;
using Newtonsoft.Json.Linq;
using Serilog;

namespace CoreBot.Services
{
    public class WolframService
    {
        private static readonly HttpClient _http = new HttpClient();

        public async Task<dynamic> GetWolframAnswerAsync(string question)
        {
            if (string.IsNullOrEmpty(question)) throw new ArgumentException("Question cannot be null or empty.");
            Log.Information($"Getting answer for the given question: {question}.");

            var json = await _http.GetStringAsync(string.Format(DefaultValues.WOLFRAM_API_URL, BotSettings.Instance.WolframAppID, question));
            Log.Debug("wolfram result json: " + json);
            var result = json.Replace(@"\\:", @"\u");
            dynamic response = JObject.Parse(result);

            return response.queryresult;
        }
    }
}

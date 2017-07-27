using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CoreBot.Helpers
{
    public static class HelperMethods
    {
        public static DateTime ToDateTime(this long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }

        public async static Task<string> GetAsync(string url)
        {
            using (var http = new HttpClient())
            {
                var globalResult = await http.GetAsync(url);
                return await globalResult.Content.ReadAsStringAsync();
            }
        }
    }
}
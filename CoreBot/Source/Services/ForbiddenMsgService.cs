using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Internal;
using CoreBot.Settings;
using Discord;
using Discord.WebSocket;
using Newtonsoft.Json.Linq;
using Serilog;

namespace CoreBot.Services
{
    public class ForbiddenMsgService
    {

        private readonly HttpClient _httpClient;
        private readonly ICollection<string> _drivingTags = new List<string>
        {
            "driving", "track", "road", "traffic", "street", "highway", "bridge"
        };

        public ForbiddenMsgService()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", BotSettings.Instance.AzureVisionKey);

            if (BotSettings.Instance.AzureVisionKey.IsNullOrEmpty())
            {
                Log.Warning("AzureVisionKey missing, ForbiddenMsgService will fail");
            }
        }

        public async Task CheckMsgAsync(SocketUserMessage message)
        {
            foreach (var attachment in message.Attachments)
            {
                Log.Information("Processing attachment " + attachment.Url);

                try
                {
                    if (HasTimestampForToday(attachment.Filename) && await IsDrivingPhoto(attachment))
                    {
                        await message.Channel.SendMessageAsync("ei kuvia ajaessa <:ewMad:393124041071919104> ! ! ! !");
                    }
                }
                catch (Exception e)
                {
                    Log.Error("failed to check attachment " + e);
                }

            }
        }

        private bool HasTimestampForToday(string filename)
        {
            var thisDay = DateTime.Now.ToString("yyyyMMdd");

            return filename.Contains(thisDay);
        }

        private async Task<bool> IsDrivingPhoto(Attachment attachment)
        {
            dynamic payload = new JObject();
            payload.url = attachment.Url;
            var stringPayload = new StringContent(payload.ToString(), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(DefaultValues.AZURE_VISION_URL, stringPayload);

            response.EnsureSuccessStatusCode();

            JObject contents = JObject.Parse(await response.Content.ReadAsStringAsync());
            var tagJson = (JArray) contents["description"]["tags"];
            var tags = tagJson.ToObject<IList<string>>();

            Log.Information("tags " + string.Join(", ", tags));
            return tags.Intersect(_drivingTags).Any();
        }
    }
}

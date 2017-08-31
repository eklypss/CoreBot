using Discord.WebSocket;
using Grapevine.Interfaces.Server;
using Grapevine.Server;
using Grapevine.Server.Attributes;

namespace CoreBot.Api
{
    [RestResource]
    public class Latency
    {
        [RestRoute(PathInfo = "/ping")]
        public IHttpContext DiscordPing(IHttpContext context)
        {
            var client = (DiscordSocketClient)context.Server.Properties["client"];
            context.Response.SendResponse(client.Latency.ToString());
            return context;
        }
    }
}
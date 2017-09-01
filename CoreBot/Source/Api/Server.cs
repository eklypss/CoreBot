using CoreBot.Settings;
using Discord.WebSocket;
using Grapevine.Server;

namespace CoreBot.Api
{
    public class Server
    {
        public Server(DiscordSocketClient discordClient)
        {
            var server = new RestServer
            {
                Port = DefaultValues.GRAPEVINE_SERVER_PORT.ToString()
            };

            server.Properties["client"] = discordClient;
            server.Start();
        }
    }
}
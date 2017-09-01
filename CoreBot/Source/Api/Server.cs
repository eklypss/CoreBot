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
                Port = BotSettings.Instance.GrapevineServerPort.ToString()
            };

            server.Properties["client"] = discordClient;
            server.Start();
        }
    }
}
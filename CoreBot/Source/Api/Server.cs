using CoreBot.Settings;
using Discord.WebSocket;
using Grapevine.Server;

namespace CoreBot.Api
{
    public class Server
    {
        private readonly RestServer _restServer;

        public Server(DiscordSocketClient discordClient)
        {
            _restServer = new RestServer
            {
                Port = BotSettings.Instance.GrapevineServerPort.ToString(),
            };

            _restServer.Properties["client"] = discordClient;
        }

        public void Start()
        {
            _restServer.Start();
        }

        public void Stop()
        {
            _restServer.Stop();
        }
    }
}

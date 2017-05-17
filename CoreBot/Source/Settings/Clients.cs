using Discord.WebSocket;
using Serilog;

namespace CoreBot.Settings
{
    public class Clients
    {
        private static Clients instance;

        public static Clients Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Clients();
                }
                return instance;
            }
        }

        public DiscordSocketClient MainClient { get; set; }

        private Clients()
        {
            Log.Information($"Creating a new instance of {this.GetType().ToString()}.");
            MainClient = new DiscordSocketClient();
        }
    }
}
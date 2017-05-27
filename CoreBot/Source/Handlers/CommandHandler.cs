using System.Reflection;
using System.Threading.Tasks;
using CoreBot.Collections;
using CoreBot.Settings;
using Discord.Commands;
using Discord.WebSocket;
using Serilog;

namespace CoreBot.Handlers
{
    public class CommandHandler
    {
        private DiscordSocketClient client;
        private CommandService commandService;

        public async Task InstallCommands(DiscordSocketClient discordClient)
        {
            client = discordClient;
            commandService = new CommandService();
            await commandService.AddModulesAsync(Assembly.GetEntryAssembly());
            client.MessageReceived += HandleCommandAsync;
        }

        private async Task HandleCommandAsync(SocketMessage message)
        {
            Log.Information($"{message.Author.Username} ({message.Author}): {message.Content}");
            if (message == null)
            {
                Log.Warning($"Message {message.Id} is not from a valid user.");
                return;
            }
            else
            {
                var userMessage = (SocketUserMessage)message;
                var context = new SocketCommandContext(client, userMessage);
                int argPos = 0;
                if (userMessage.HasCharPrefix(BotSettings.Instance.BotPrefix, ref argPos))
                {
                    var result = await commandService.ExecuteAsync(context, argPos);
                    if (!result.IsSuccess) // Module was not found, check for dynamic commands.
                    {
                        Log.Information(result.ToString());
                        bool matchFound = false;
                        foreach (var command in Commands.Instance.CommandsList)
                        {
                            if (userMessage.Content == $"{BotSettings.Instance.BotPrefix}{command.Name}")
                            {
                                await userMessage.Channel.SendMessageAsync(command.Action);
                                matchFound = true;
                                break;
                            }
                        }
                        if (!matchFound) await userMessage.Channel.SendMessageAsync(result.ToString());
                    }
                }
            }
        }
    }
}
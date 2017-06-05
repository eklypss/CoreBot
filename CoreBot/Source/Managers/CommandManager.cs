using System.Threading.Tasks;
using CoreBot.Collections;
using CoreBot.Models;
using CoreBot.Settings;
using CoreBot.Source.Helpers;
using Serilog;
using ServiceStack.OrmLite;

namespace CoreBot.Managers
{
    /// <summary>
    /// Class for managing dynamic commands.
    /// </summary>
    public class CommandManager
    {
        public async Task AddCommand(Command command)
        {
            Log.Information($"Trying to add command: {BotSettings.Instance.BotPrefix}{command.Name}, action: {command.Action}.");
            bool commandExists = false;
            foreach (Command cmd in Commands.Instance.CommandsList)
            {
                if (cmd.Name == command.Name)
                {
                    commandExists = true;
                    break;
                }
            }

            if (!commandExists)
            {
                Log.Information($"Command added: {BotSettings.Instance.BotPrefix}{command.Name}, action: {command.Action}");
                Commands.Instance.CommandsList.Add(command);
                await Database.Run().InsertAsync(command);
            }
            else
            {
                Log.Warning($"Could not add command: {BotSettings.Instance.BotPrefix}{command.Name} since it already exists.");
            }
        }

        public async Task DeleteCommand(Command command)
        {
            if (Commands.Instance.CommandsList.Contains(command))
            {
                Commands.Instance.CommandsList.Remove(command);
                await Database.Run().DeleteAsync(command);
                Log.Information($"Command {command.Name} was deleted.");
            }
            else Log.Warning($"Command does not exist: {command.Name}");
        }

        /// <summary>
        /// Updates the Action property of the command.
        /// </summary>
        public async Task UpdateCommand(Command command, string newAction)
        {
            command.Action = newAction;
            // TODO: Test this.
            await Database.Run().UpdateAsync(command);
            Log.Information($"Command {command.Name} was updated. New action: {newAction}");
        }
    }
}
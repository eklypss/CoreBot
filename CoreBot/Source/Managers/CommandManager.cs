using CoreBot.Collections;
using CoreBot.Models;
using CoreBot.Settings;
using Serilog;
using ServiceStack.OrmLite;
using CoreBot.Source.Helpers;

namespace CoreBot.Managers
{
    /// <summary>
    /// Class for managing dynamic commands.
    /// </summary>
    public class CommandManager
    {
        public void AddCommand(Command command)
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
                Database.Run().Insert(command);
            }
            else
            {
                Log.Warning($"Could not add command: {BotSettings.Instance.BotPrefix}{command.Name} since it already exists.");
            }
        }

        public void DeleteCommand(Command command)
        {
            if (Commands.Instance.CommandsList.Contains(command))
            {
                Commands.Instance.CommandsList.Remove(command);
                Database.Run().Delete(command);
                Log.Information($"Command {command.Name} was deleted.");
            }
            else Log.Warning($"Command does not exist: {command.Name}");
        }
    }
}
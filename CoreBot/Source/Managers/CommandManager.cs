using System.Threading.Tasks;
using CoreBot.Collections;
using CoreBot.Enum;
using CoreBot.Helpers;
using CoreBot.Models;
using CoreBot.Settings;
using Serilog;

namespace CoreBot.Managers
{
    /// <summary>
    /// Class for managing dynamic commands.
    /// </summary>
    public class CommandManager
    {
        public async Task SaveCommands()
        {
            await FileHelper.SaveFile(FileType.CommandsFile);
        }

        public async Task AddCommand(Command command)
        {
            Log.Information($"Trying to add command: {BotSettings.Instance.BotPrefix}{command.Name}, action: {command.Action}.");
            bool commandExists = false;
            foreach (Command cmd in Commands.Instance.CommandsList)
            {
                if (cmd.Action == command.Action)
                {
                    commandExists = true;
                    break;
                }
            }

            if (!commandExists)
            {
                Log.Information($"Command added: {BotSettings.Instance.BotPrefix}{command.Name}, action: {command.Action}");
                Commands.Instance.CommandsList.Add(command);
                await SaveCommands();
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
                await SaveCommands();
                Log.Information($"Command {command.Name} was deleted.");
            }
            else Log.Warning($"Command does not exist: {command.Name}");
        }
    }
}
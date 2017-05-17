using CoreBot.Collections;
using CoreBot.Settings;
using Newtonsoft.Json;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CoreBot.Source.Services
{
    public class CommandManager
    {
        public async Task SaveCommands()
        {
            try
            {
                var json = JsonConvert.SerializeObject(Commands.Instance.CommandsList);
                if (File.Exists(BotSettings.Instance.CommandsFile))
                {
                    File.Delete(BotSettings.Instance.CommandsFile);
                    Log.Information("Deleted old CommandsFile.");
                }

                using (StreamWriter writer = File.CreateText(BotSettings.Instance.CommandsFile))
                {
                    await writer.WriteAsync(json);
                }
            }
            catch (Exception)
            {
                Log.Error("Error occured while trying to save commands.");
                throw;
            }
            finally
            {
                Log.Information($"Successfully saved commands to {BotSettings.Instance.CommandsFile}.");
            }
        }
    }
}
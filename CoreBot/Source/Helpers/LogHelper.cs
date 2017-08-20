using System.IO;
using System.Reflection;
using Serilog;

namespace CoreBot.Helpers
{
    public static class LogHelper
    {
        public static void CreateLogger(bool logToFile)
        {
            if (logToFile)
            {
                string logsFolder = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Logs");
                if (!Directory.Exists(logsFolder)) Directory.CreateDirectory(logsFolder);
                string logFile = Path.Combine(logsFolder, "CoreBot.txt");
                Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.LiterateConsole().WriteTo.RollingFile(logFile).CreateLogger();
                Log.Information($"Log file created at {logFile}.");
            }
            else
            {
                Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.LiterateConsole().CreateLogger();
            }
        }
    }
}
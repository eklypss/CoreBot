using Serilog;
using System.IO;
using System.Reflection;

namespace CoreBot.Services
{
    public static class LogManager
    {
        public static void CreateLogger(bool logToFile)
        {
            if (logToFile)
            {
                string logsFolder = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Logs");
                if (!Directory.Exists(logsFolder)) Directory.CreateDirectory(logsFolder);
                string logFile = string.Format("{0}\\CoreBot.txt", logsFolder);
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
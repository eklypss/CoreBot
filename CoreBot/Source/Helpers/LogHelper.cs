﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Discord;
using Serilog;

namespace CoreBot.Helpers
{
    public static class LogHelper
    {
        public static Dictionary<string, LogSeverity> logLevels
            = new Dictionary<string, LogSeverity>
            {
                { "Debug", LogSeverity.Debug },
                { "Verbose", LogSeverity.Verbose },
                { "Info", LogSeverity.Info },
                { "Warning", LogSeverity.Warning },
                { "Error", LogSeverity.Error },
                { "Critical", LogSeverity.Critical },
            };

        public static LogSeverity ParseLoglevel(string loglevel)
        {
            if (!logLevels.ContainsKey(loglevel))
            {
                var levels = string.Join(", ", LogHelper.logLevels.Keys);

                Console.Error.WriteLine("invalid loglevel: " + loglevel);
                Console.Error.WriteLine("valid loglevels: " + levels);
                Environment.Exit(1);
            }

            return logLevels[loglevel];
        }

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

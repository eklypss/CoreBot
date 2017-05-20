using System;

namespace CoreBot.Enum
{
    [Flags]
    public enum CreateType
    {
        CommandsFolder,
        CommandsFile,
        SettingsFolder,
        SettingsFile
    }
}
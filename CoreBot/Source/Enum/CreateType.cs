using System;

namespace CoreBot.Enum
{
    [Flags]
    public enum FileType
    {
        CommandsFolder,
        CommandsFile,
        SettingsFolder,
        SettingsFile,
        MessagesFile
    }
}
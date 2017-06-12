namespace CoreBot.Interfaces
{
    public interface IBotSettings
    {
        string BotToken { get; set; }
        char BotPrefix { get; set; }
        string DatabaseString { get; set; }
        bool LogToFile { get; set; };
        string SettingsFolder { get; set; }
        string SettingsFile { get; set; }
    }
}
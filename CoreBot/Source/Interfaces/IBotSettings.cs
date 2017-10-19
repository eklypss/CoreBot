using System.Collections.Generic;

namespace CoreBot.Interfaces
{
    public interface IBotSettings
    {
        string BotToken { get; set; }
        char BotPrefix { get; set; }
        string DatabaseString { get; set; }
        bool LogToFile { get; set; }
        string SettingsFolder { get; set; }
        string SettingsFile { get; set; }
        ulong DefaultChannel { get; set; }
        ulong DefaultGuild { get; set; }
        string WeatherAPIKey { get; set; }
        string EPAPIKey { get; set; }
        string UrbanMashapeKey { get; set; }
        List<string> OldLinkBlacklist { get; set; }
        string DateTimeFormat { get; set; }
        string DateFormat { get; set; }
        string DateTimeCulture { get; set; }
        int HumanizerPrecision { get; set; }
        int GrapevineServerPort { get; set; }
    }
}

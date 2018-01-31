using System;
using Humanizer.Localisation;

namespace CoreBot.Settings
{
    /// <summary>
    /// Default bot settings to be used if not defined in the configuration file and constant URL
    /// strings to be used with modules/services.
    /// </summary>
    public static class DefaultValues
    {
        public const char DEFAULT_PREFIX = '!';
        public const string DEFAULT_DATABASE_STRING = "Database.db";
        public const string DEFAULT_LOGLEVEL = "Warning";

        public const string NHL_STANDINGS_URL = "https://statsapi.web.nhl.com/api/v1/standings";
        public const string ALKO_URL = "https://www.alko.fi/tuotteet/{0:D6}/";
        public const string FMI_URL = "http://ilmatieteenlaitos.fi/saa/{0}?forecast=short";
        public const string FMI_TEMP_URL = "http://ilmatieteenlaitos.fi/observation-data?station=";
        public const string OPEN_WEATHER_URL = "http://api.openweathermap.org/data/2.5/weather?q={0}&APPID={1}";
        public const string FOLLOWAGE_URL = "https://2g.be/twitch/following.php?user={0}&channel={1}&format=mwdhms";
        public const string URBAN_API_URL = "https://mashape-community-urban-dictionary.p.mashape.com/define?term={0}";
        public const string VKS_URL = "http://kaino.kotus.fi/vks/";
        public const string VKS_PAGING = VKS_URL + "?p=searchresults";
        public const string TWITCH_URL = "https://www.twitch.tv/{0}";
        public const string TWITTER_URL = "https://twitter.com/{0}";

        public const string F1_API_SCHEDULE_URL = "http://ergast.com/api/f1/{0}.json";

        public const string DEFAULT_DATETIME_FORMAT = "dd/MM/yyyy H:mm:ss";
        public const string DEFAULT_DATE_FORMAT = "dd/MM/yyyy";
        public const string DEFAULT_CULTURE = "fi-FI";

        public const int DEFAULT_SPAM_TRIGGER = 3;
        public const double DEFAULT_SPAM_PROB = 0.2;
        public const int DEFAULT_HUMANIZER_PRECISION = 2;
        public const TimeUnit DEFAULT_HUMANIZER_MAXUNIT = TimeUnit.Year;
        public const int DEFAULT_GRAPEVINE_SERVER_PORT = 5700;
        public const int DEFAULT_MAX_DYNAMIC_COMMANDS_PER_LINE = 5;
        public const string DEFAULT_SELF_HOTSTRING = "$me$";
    }
}

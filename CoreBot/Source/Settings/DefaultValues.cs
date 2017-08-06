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

        public const string ALKO_URL = "https://www.alko.fi/tuotteet/{0:D6}/";
        public const string FMI_URL = "http://ilmatieteenlaitos.fi/saa/{0}?forecast=short";
        public const string FMI_TEMP_URL = "http://ilmatieteenlaitos.fi/observation-data?station=";
        public const string OPEN_WEATHER_URL = "http://api.openweathermap.org/data/2.5/weather?q={0}&APPID={1}";
        public const string FOLLOWAGE_URL = "https://2g.be/twitch/following.php?user={0}&channel={1}&format=mwdhms";
        public const string VKS_URL = "http://kaino.kotus.fi/vks/";
        public const string VKS_PAGING = VKS_URL + "?p=searchresults";
    }
}
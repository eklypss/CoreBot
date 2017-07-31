namespace CoreBot.Settings
{
    public static class DefaultValues
    {
        public const char DEFAULT_PREFIX = '!';
        public const string DEFAULT_DATABASE_STRING = "Database.db";
        public const string ALKO_URL = "https://www.alko.fi/tuotteet/{0:D6}/";
        public const string FMI_URL = "http://ilmatieteenlaitos.fi/saa/{0}?forecast=short";
        public const string FMI_TEMP_URL = "http://ilmatieteenlaitos.fi/observation-data?station=";
        public const string OPEN_WEATHER_URL = "http://api.openweathermap.org/data/2.5/weather?q={0}&APPID={1}";
    }
}
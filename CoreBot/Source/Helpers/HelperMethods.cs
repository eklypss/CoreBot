using System;

namespace CoreBot.Helpers
{
    public static class HelperMethods
    {
        /// <summary>
        /// Converts unix timestamp in specific timezone to DateTime.
        /// </summary>
        /// <param name="unixTime">unix timestamp in seconds</param>
        /// <param name="timezone">timezone of the input, default is utc</param>
        public static DateTime ToDateTime(this long unixTime, TimeZoneInfo timezone)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(unixTime);
            return TimeZoneInfo.ConvertTimeToUtc(epoch, timezone);
        }

        /// <summary>
        /// Converts unix timestamp to DateTime.
        /// </summary>
        /// <param name="unixTime">unix timestamp in seconds</param>
        public static DateTime ToDateTime(this long unixTime)
        {
            return ToDateTime(unixTime, TimeZoneInfo.Utc);
        }
    }
}

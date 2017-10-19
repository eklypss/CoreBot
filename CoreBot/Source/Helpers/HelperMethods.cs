using System;

namespace CoreBot.Helpers
{
    public static class HelperMethods
    {
        /// <summary>
        /// Converts Unix timestamp to DateTime.
        /// </summary>
        /// <param name="unixTime">unix timestamp in seconds</param>
        public static DateTime ToDateTime(this long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }
    }
}

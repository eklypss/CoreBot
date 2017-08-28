using System;

namespace CoreBot.Models
{
    public class StartupTime
    {
        public DateTime StartTime { get; set; }

        public StartupTime()
        {
            StartTime = DateTime.Now;
        }
    }
}
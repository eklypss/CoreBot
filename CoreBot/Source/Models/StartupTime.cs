using System;

namespace CoreBot.Models
{
    public class StartupTime
    {
        public DateTime StartTime { get; set; }

        // Workaround class since DI doesn't like to inject DateTime
        public StartupTime()
        {
            StartTime = DateTime.Now;
        }
    }
}
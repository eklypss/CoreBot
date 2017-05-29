using System;

namespace CoreBot.Models
{
    public class UserMessage
    {
        public string User { get; set; }
        public string Message { get; set; }
        public DateTime DateTime { get; set; }

        public UserMessage(string user, string message)
        {
            User = user;
            Message = message;
            DateTime = DateTime.Now;
        }
    }
}
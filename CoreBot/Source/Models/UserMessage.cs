using System;

namespace CoreBot.Models
{
    public class UserMessage
    {
        public int Id { get; set; }

        public string User { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }

        public UserMessage(string user, string message)
        {
            User = user;
            Message = message;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
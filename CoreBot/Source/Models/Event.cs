using System;
using ServiceStack.DataAnnotations;

namespace CoreBot.Models
{
    public class Event
    {
        [Required]
        public string Message { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public bool Completed { get; set; }

        [AutoIncrement]
        public int ID { get; set; }
    }
}
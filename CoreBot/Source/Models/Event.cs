using System;
using ServiceStack.DataAnnotations;

namespace CoreBot.Models
{
    public class Event
    {
        [AutoIncrement]
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        public Event(string desc, DateTime date)
        {
            DateTime = date;
            Description = desc;
        }
    }
}
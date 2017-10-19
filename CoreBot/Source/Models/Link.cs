using System;
using ServiceStack.DataAnnotations;

namespace CoreBot.Models
{
    public class Link
    {
        [AutoIncrement]
        public int Id { get; set; }

        [Required]
        [Index(Unique = true)]
        public string Url { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        [Required]
        public ulong AuthorId { get; set; }

        public Link(string url, ulong authorId)
        {
            Timestamp = DateTime.Now;
            Url = url;
            AuthorId = authorId;
        }
    }
}

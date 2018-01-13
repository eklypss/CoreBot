using ServiceStack.DataAnnotations;

namespace CoreBot.Models
{
    public class IltasanomatComment
    {
        [AutoIncrement]
        [Index(Unique = true)]
        public int Id { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        public int Upvotes { get; set; }
    }
}

using ServiceStack.DataAnnotations;

namespace CoreBot.Models
{
    public class Aroma
    {
        [AutoIncrement]
        public int Id { get; set; }

        [Required]
        [Index(Unique = true)]
        public string Name { get; set; }

        [Required]
        public int Count { get; set; }
    }
}

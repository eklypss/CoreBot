using ServiceStack.DataAnnotations;

namespace CoreBot.Models
{
    public class Command
    {
        [AutoIncrement]
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }

        [Required]
        public string Action { get; set; }

        public Command(string name, string action)
        {
            Name = name;
            Action = action;
        }
    }
}
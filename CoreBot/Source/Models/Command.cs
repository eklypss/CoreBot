namespace CoreBot.Models
{
    public class Command
    {
        public string Name { get; set; }
        public string Action { get; set; }

        public Command(string name, string action)
        {
            Name = name;
            Action = action;
        }
    }
}
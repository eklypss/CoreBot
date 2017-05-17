using System;

namespace CoreBot.Models
{
    public class Command
    {
        public string Name { get; set; }
        public string Action { get; set; }
        public DateTime DateAdded { get; set; }
        public string AddedBy { get; set; }
        public DateTime DateEdited { get; set; }
        public string EditedBy { get; set; }
        public bool IsEnabled { get; set; }

        public Command(string name, string action, string addedBy)
        {
            Name = name;
            Action = action;
            DateAdded = DateTime.Now;
            AddedBy = addedBy;
            DateEdited = DateTime.Now;
            EditedBy = addedBy;
            IsEnabled = true;
        }
    }
}
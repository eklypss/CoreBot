using System.Collections.Generic;
using CoreBot.Models;

namespace CoreBot.Collections
{
    public class Commands
    {
        private static readonly Commands _instance = new Commands();

        public static Commands Instance
        {
            get { return _instance; }
        }

        public List<Command> CommandsList { get; set; }

        private Commands()
        {
            CommandsList = new List<Command>();
        }
    }
}
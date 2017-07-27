using System.Collections.Generic;
using CoreBot.Models;

namespace CoreBot.Collections
{
    public class Commands
    {
        private static Commands _instance;

        public static Commands Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Commands();
                }
                return _instance;
            }
        }

        public List<Command> CommandsList { get; set; }

        private Commands()
        {
            CommandsList = new List<Command>();
        }
    }
}
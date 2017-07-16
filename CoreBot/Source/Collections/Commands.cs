using CoreBot.Models;
using System.Collections.Generic;

namespace CoreBot.Collections
{
    public class Commands
    {
        private static Commands instance;

        public static Commands Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Commands();
                }
                return instance;
            }
        }

        public List<Command> CommandsList { get; set; }

        private Commands()
        {
            CommandsList = new List<Command>();
        }
    }
}
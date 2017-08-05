using System.Collections.Generic;
using CoreBot.Models;

namespace CoreBot.Collections
{
    public class Events
    {
        private static Events _instance;

        public static Events Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Events();
                }
                return _instance;
            }
        }

        public List<Event> EventsList { get; set; }

        private Events()
        {
            EventsList = new List<Event>();
        }
    }
}
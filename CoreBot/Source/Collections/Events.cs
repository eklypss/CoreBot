using System.Collections.Generic;
using CoreBot.Models;

namespace CoreBot.Collections
{
    public class Events
    {
        private static readonly Events _instance = new Events();

        public static Events Instance
        {
            get { return _instance; }
        }

        public List<Event> EventsList { get; set; }

        private Events()
        {
            EventsList = new List<Event>();
        }
    }
}

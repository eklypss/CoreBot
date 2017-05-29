using System.Collections.Generic;
using CoreBot.Models;

namespace CoreBot.Collections
{
    public class UserMessages
    {
        private static UserMessages instance;

        public static UserMessages Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UserMessages();
                }
                return instance;
            }
        }

        public List<UserMessage> Messages;

        private UserMessages()
        {
            Messages = new List<UserMessage>();
        }
    }
}
using System;
using System.Runtime.Serialization;

namespace CoreBot.Exceptions
{
    [Serializable]
    public class CoreBotException : Exception
    {
        public CoreBotException() : base()
        {
        }

        public CoreBotException(string message) : base(message)
        {
        }

        public CoreBotException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CoreBotException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
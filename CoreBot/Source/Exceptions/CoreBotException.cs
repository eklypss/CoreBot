using System;

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

        protected CoreBotException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
        }
    }
}
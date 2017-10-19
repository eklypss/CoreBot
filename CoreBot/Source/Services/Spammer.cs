using System;
using System.Threading.Tasks;
using CoreBot.Settings;
using Discord;

namespace CoreBot.Services
{
    /// <summary>
    /// Respond randomly to spam by echoing the message
    /// </summary>
    public class Spammer
    {
        private string _lastMessage;
        private int _count;
        private readonly Random _random;

        private readonly int _triggerCount;
        private readonly double _probability;

        public Spammer(int triggerCount = DefaultValues.DEFAULT_SPAM_TRIGGER,
            double probability = DefaultValues.DEFAULT_SPAM_PROB)
        {
            _count = 0;
            _triggerCount = triggerCount;
            _probability = probability;

            _random = new Random();
        }

        private void RegisterMessage(string message)
        {
            if (_lastMessage == null) _lastMessage = message;

            if (_lastMessage == message)
            {
                _count++;
                return;
            }

            _count = 1;
            _lastMessage = message;
        }

        private bool RandomTrigger()
        {
            double roll = _random.NextDouble();

            if (_count >= _triggerCount && roll < _probability)
            {
                _count = 0;
                return true;
            }

            return false;
        }

        public async Task CheckSpamAsync(IMessage msg)
        {
            if (msg.Author.IsBot)
            {
                return;
            }

            RegisterMessage(msg.Content);

            if (RandomTrigger())
            {
                await msg.Channel.SendMessageAsync(msg.Content);
            }
        }
    }
}
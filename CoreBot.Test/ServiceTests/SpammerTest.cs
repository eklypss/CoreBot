using System.Threading.Tasks;
using CoreBot.Services;
using Discord;
using Moq;
using NUnit.Framework;

namespace CoreBot.Test.ServiceTests
{
    [TestFixture]
    internal class SpammerTest
    {
        private static Mock<IMessage> CreateMessage(IMessageChannel channel,
            string message, bool isBot = false)
        {
            var msg = new Mock<IMessage>();
            msg.Setup(m => m.Content).Returns(message);
            msg.Setup(m => m.Author.IsBot).Returns(isBot);
            msg.Setup(m => m.Channel).Returns(channel);

            return msg;
        }

        [Test]
        public async Task RespondAlwaysToFullProbability()
        {
            var spammer = new Spammer(1, 1);
            var channel = new Mock<IMessageChannel>();
            var msg = CreateMessage(channel.Object, "123");

            await spammer.CheckSpamAsync(msg.Object);

            channel.Verify(c => c.SendMessageAsync("123", false, null, null),
                Times.Once());
        }

        [Test]
        public async Task RespondForTwoTrigger()
        {
            var spammer = new Spammer(2, 1);
            var channel = new Mock<IMessageChannel>();

            var msg = CreateMessage(channel.Object, "123");

            await spammer.CheckSpamAsync(msg.Object);
            await spammer.CheckSpamAsync(msg.Object);

            channel.Verify(c => c.SendMessageAsync("123", false, null, null),
                Times.Once());
        }

        [Test]
        public async Task NoResponseWhenLargerTriggerCount()
        {
            var spammer = new Spammer(2, 1);
            var channel = new Mock<IMessageChannel>();
            var msg = CreateMessage(channel.Object, "123");

            await spammer.CheckSpamAsync(msg.Object);

            channel.Verify(c => c.SendMessageAsync("123", false, null, null),
                Times.Never());
        }
    }
}

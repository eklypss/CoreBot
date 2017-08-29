using System;
using CoreBot.Models.Urban;
using CoreBot.Services;
using NUnit.Framework;

namespace CoreBot.Test.ServiceTests
{
    [TestFixture]
    public class UrbanServiceTests
    {
        [Test]
        public void Service_ShouldThrowException_WhenNoSearchTermGiven()
        {
            var service = new UrbanService();
            Assert.That(async () => await service.GetUrbanQuotesAsync(null), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void Parser_ShouldThrowException_WhenInputIsEmpty()
        {
            var service = new UrbanService();
            Assert.That(() => service.ParseQuotes(new UrbanResponse()), Throws.ArgumentNullException);
        }
    }
}
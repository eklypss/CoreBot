using System;
using CoreBot.Models.Urban;
using CoreBot.Services;
using NUnit.Framework;

namespace CoreBot.Test.ServiceTests
{
    [TestFixture]
    public class UrbanServiceTests
    {
        private UrbanService _urbanService;

        [SetUp]
        public void Setup()
        {
            _urbanService = new UrbanService();
        }

        [Test]
        public void Service_ShouldThrowException_WhenNoSearchTermGiven()
        {
            Assert.That(async () => await _urbanService.GetUrbanQuotesAsync(null), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void Parser_ShouldThrowException_WhenInputIsEmpty()
        {
            Assert.That(() => _urbanService.ParseQuotes(new UrbanResponse()), Throws.ArgumentNullException);
        }
    }
}
using Model;
using Service;
using NUnit.Framework;
using Moq;

namespace ServiceTests
{
    [TestFixture]
    public class CreamServiceTests
    {
        private ICondimentService _service;
        private Mock<IApplicationSettingsService> _appSettingsMock;

        [SetUp]
        public void SetUp()
        {
            _appSettingsMock = new Mock<IApplicationSettingsService>();
            _service = new CreamService(_appSettingsMock.Object);
        }

        [TestCase(0, 3, true)]
        [TestCase(1, 3, true)]
        [TestCase(3, 3, true)]
        [TestCase(5, 3, false)]
        [TestCase(-1, 3, false)]
        public void IsCondimentValid_ReturnsCorrectResult(int cream, int max, bool expected)
        {
            _appSettingsMock.SetupGet(s => s.MaxCream).Returns(max);
            bool result = _service.IsCondimentValid(cream);
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(5)]
        public void OrderCondiment_ReturnsCream(int cream)
        {
            decimal creamPrice = 1.5m;
            _appSettingsMock.SetupGet(s => s.CreamPrice).Returns(creamPrice);

            var result = _service.OrderCondiment(cream);
            Assert.That(result.CondimentType, Is.EqualTo(CondimentType.Cream));
            Assert.That(result.Quantity, Is.EqualTo(cream));
            Assert.That(result.Price, Is.EqualTo(cream * creamPrice));
        }
    }
}

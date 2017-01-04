using NUnit.Framework;
using Service;
using Model;
using Moq;

namespace ServiceTests
{
    [TestFixture]
    public class SugarServiceTests
    {
        private ICondimentService _service;
        private Mock<IApplicationSettingsService> _appSettingsMock;

        [SetUp]
        public void SetUp()
        {
            _appSettingsMock = new Mock<IApplicationSettingsService>();
            _service = new SugarService(_appSettingsMock.Object);
        }

        [TestCase(0, 3, true)]
        [TestCase(1, 3, true)]
        [TestCase(3, 3, true)]
        [TestCase(5, 3, false)]
        [TestCase(-1, 3, false)]
        public void IsCondimentValid_ReturnsCorrectResult(int sugar, int max, bool expected)
        {
            _appSettingsMock.SetupGet(s => s.MaxSugar).Returns(max);
            bool result = _service.IsCondimentValid(sugar);
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(5)]
        public void OrderCondiment_ReturnsSugar(int sugar)
        {
            decimal sugarPrice = 2.5m;
            _appSettingsMock.SetupGet(s => s.SugarPrice).Returns(sugarPrice);

            var result = _service.OrderCondiment(sugar);
            Assert.That(result.CondimentType, Is.EqualTo(CondimentType.Sugar));
            Assert.That(result.Quantity, Is.EqualTo(sugar));
            Assert.That(result.Price, Is.EqualTo(sugar * sugarPrice));
        }
    }
}

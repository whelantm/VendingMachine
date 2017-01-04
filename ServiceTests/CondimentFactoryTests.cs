using Service;
using Model;
using NUnit.Framework;
using Moq;

namespace ServiceTests
{
    [TestFixture]
    public class CondimentFactoryTests
    {
        private Mock<IApplicationSettingsService> _appSettingsMock;
        private ICondimentServiceFactory _factory;

        [SetUp]
        public void SetUp()
        {
            _appSettingsMock = new Mock<IApplicationSettingsService>();
            _factory = new CondimentServiceFactory(_appSettingsMock.Object);
        }

        [Test]
        public void Create_ReturnsSugarService()
        {
            var result = _factory.Create(CondimentType.Sugar);
            Assert.That(result.GetType(), Is.EqualTo(typeof(SugarService)));
        }

        [Test]
        public void Create_ReturnsCreamService()
        {
            var result = _factory.Create(CondimentType.Cream);
            Assert.That(result.GetType(), Is.EqualTo(typeof(CreamService)));
        }
    }
}

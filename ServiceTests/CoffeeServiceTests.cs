using System.Linq;
using Model;
using Service;
using NUnit.Framework;
using Moq;

namespace ServiceTests
{
    [TestFixture]
    public class CoffeeServiceTests
    {
        private Mock<IApplicationSettingsService> _appSettingsMock;
        private Mock<IPaymentService> _paymentServiceMock;
        private Mock<ICondimentServiceFactory> _condimentServiceFactoryMock;

        private ICoffeeService _service;

        private const decimal SmallPrice = 0.50m;
        private const decimal MedPrice = 1.50m;
        private const decimal LargePrice = 2.50m;

        [SetUp]
        public void SetUp()
        {
            _appSettingsMock = new Mock<IApplicationSettingsService>();
            _paymentServiceMock = new Mock<IPaymentService>();
            _condimentServiceFactoryMock = new Mock<ICondimentServiceFactory>();

            _appSettingsMock.SetupGet(s => s.SmallCoffeePrice).Returns(SmallPrice);
            _appSettingsMock.SetupGet(s => s.MediumCoffeePrice).Returns(MedPrice);
            _appSettingsMock.SetupGet(s => s.LargeCoffeePrice).Returns(LargePrice);

            _service = new CoffeeService(_condimentServiceFactoryMock.Object, _paymentServiceMock.Object, _appSettingsMock.Object);
        }

        [Test]
        public void WhenCancelCurrentCup_CurrentCupSetToNull()
        {
            _service.OrderCoffee(CupSize.Medium);
            Assert.That(_service.ReviewCurrentCup(), Is.Not.Null);
            _service.CancelCurrentCup();
            Assert.That(_service.ReviewCurrentCup(), Is.Null);
            
        }

        [TestCase(CupSize.Small)]
        [TestCase(CupSize.Medium)]
        [TestCase(CupSize.Large)]
        public void WhenOrderingCoffee_CurrentCupIsRightSize_NoCondimentsAdded(CupSize cupSize)
        {
            _service.OrderCoffee(cupSize);
            CoffeeOrder order = _service.ReviewCurrentCup();
            Assert.That(order.Coffee, Is.Not.Null);
            Assert.That(order.Condiments.Count, Is.EqualTo(0));
            Assert.That(order.Coffee.Size, Is.EqualTo(cupSize));
        }

        [Test]
        public void WhenCompletingCup_CurrentCupIsNull_OrderListIncremented()
        {
            Assert.That(_service.CurrentOrder().Count(), Is.EqualTo(0));
            _service.OrderCoffee(CupSize.Small);
            _service.CompleteCup();
            Assert.That(_service.ReviewCurrentCup(), Is.Null);
            Assert.That(_service.CurrentOrder().Count(), Is.EqualTo(1));
        }

        [Test]
        public void WhenOrderingCoffee_PriceIsCorrect()
        {
           

            _service.OrderCoffee(CupSize.Small);
            var cup = _service.ReviewCurrentCup();
            Assert.That(cup.Coffee.Price, Is.EqualTo(SmallPrice));
            _service.CancelCurrentCup();

            _service.OrderCoffee(CupSize.Medium);
            cup = _service.ReviewCurrentCup();
            Assert.That(cup.Coffee.Price, Is.EqualTo(MedPrice));
            _service.CancelCurrentCup();

            _service.OrderCoffee(CupSize.Large);
            cup = _service.ReviewCurrentCup();
            Assert.That(cup.Coffee.Price, Is.EqualTo(LargePrice));
            _service.CancelCurrentCup();
        }

        [Test]
        public void WhenNoOrder_TotalReturnsZero()
        {
            Assert.That(_service.CurrentOrder().Count(), Is.EqualTo(0));
            Assert.That(_service.Total(), Is.EqualTo(0));
        }

        [Test]
        public void Total_SumsPricesOfAllCupsInOrder()
        {
            _service.OrderCoffee(CupSize.Small);
            _service.CompleteCup();

            _service.OrderCoffee(CupSize.Medium);
            _service.CompleteCup();

            _service.OrderCoffee(CupSize.Large);
            _service.CompleteCup();

            var expectedTotal = SmallPrice + MedPrice + LargePrice;
            Assert.That(_service.Total(), Is.EqualTo(expectedTotal));
        }

        [Test]
        public void WhenOrderCondiment_AlreadyExists_OrderIsReplaced()
        {
            _service.OrderCoffee(CupSize.Small);
            
            Mock<ICondimentService> condimentService = new Mock<ICondimentService>();
            _condimentServiceFactoryMock.Setup(f => f.Create(CondimentType.Cream)).Returns(condimentService.Object);

            condimentService.Setup(s => s.IsCondimentValid(It.IsAny<int>())).Returns(true);

            Condiment setupFour = new Condiment() {Quantity = 4};
            condimentService.Setup(s => s.OrderCondiment(4)).Returns(setupFour);

            Condiment setupTwo = new Condiment() { Quantity = 2 };
            condimentService.Setup(s => s.OrderCondiment(2)).Returns(setupTwo);


            var result = _service.OrderCondiment(CondimentType.Cream, 4);
            var condiment = _service.ReviewCurrentCup().Condiments[CondimentType.Cream];
            Assert.That(condiment.Quantity, Is.EqualTo(4));
            Assert.That(result, Is.EqualTo(true));

            result = _service.OrderCondiment(CondimentType.Cream, 2);
            condiment = _service.ReviewCurrentCup().Condiments[CondimentType.Cream];
            Assert.That(condiment.Quantity, Is.EqualTo(2));
            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public void OrderCondiment_ReturnsFalseIfNoCupOrdered()
        {
            Mock<ICondimentService> condimentService = new Mock<ICondimentService>();
            _condimentServiceFactoryMock.Setup(f => f.Create(CondimentType.Cream)).Returns(condimentService.Object);

            condimentService.Setup(s => s.IsCondimentValid(It.IsAny<int>())).Returns(true);
            
            var result = _service.OrderCondiment(CondimentType.Cream, 4);
            Assert.That(result, Is.EqualTo(false));
        }


        [Test]
        public void OrderCondiment_ReturnsFalseIfInvalidAmount()
        {
            Mock<ICondimentService> condimentService = new Mock<ICondimentService>();
            _condimentServiceFactoryMock.Setup(f => f.Create(CondimentType.Cream)).Returns(condimentService.Object);

            condimentService.Setup(s => s.IsCondimentValid(It.IsAny<int>())).Returns(false);
            
            var result = _service.OrderCondiment(CondimentType.Cream, 4);
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void WhenCompleteAndPay_Successful_CurrentOrderIsCleared()
        {
            _paymentServiceMock.Setup(p => p.RemitPayment(It.IsAny<decimal>(), It.IsAny<decimal>()))
                .Returns(new PaymentResult() {PaymentStatus = PaymentStatus.Success});

            _service.OrderCoffee(CupSize.Small);
            _service.CompleteCup();

            _service.OrderCoffee(CupSize.Medium);
            _service.CompleteCup();

            _service.OrderCoffee(CupSize.Large);
            _service.CompleteCup();

            Assert.That(_service.CurrentOrder().Count(), Is.Not.EqualTo(0));

            _service.CompleteOrderAndPay(10m);
            Assert.That(_service.CurrentOrder().Count(), Is.EqualTo(0));
            Assert.That(_service.ReviewCurrentCup(), Is.Null);
        }

        [Test]
        public void WhenCompleteAndPay_InsufficientFunds_CurrentOrderIsNotCleared()
        {
            _paymentServiceMock.Setup(p => p.RemitPayment(It.IsAny<decimal>(), It.IsAny<decimal>()))
                .Returns(new PaymentResult() { PaymentStatus = PaymentStatus.InsufficientFunds });

            _service.OrderCoffee(CupSize.Small);
            _service.CompleteCup();

            _service.OrderCoffee(CupSize.Medium);
            _service.CompleteCup();

            _service.OrderCoffee(CupSize.Large);
            _service.CompleteCup();

            Assert.That(_service.CurrentOrder().Count(), Is.Not.EqualTo(0));

            _service.CompleteOrderAndPay(10m);
            Assert.That(_service.CurrentOrder().Count(), Is.Not.EqualTo(0));            
        }

        [Test]
        public void WhenCompleteAndPay_InvalidAmount_CurrentOrderIsNotCleared()
        {
            _paymentServiceMock.Setup(p => p.RemitPayment(It.IsAny<decimal>(), It.IsAny<decimal>()))
                .Returns(new PaymentResult() { PaymentStatus = PaymentStatus.InvalidAmount });

            _service.OrderCoffee(CupSize.Small);
            _service.CompleteCup();

            _service.OrderCoffee(CupSize.Medium);
            _service.CompleteCup();

            _service.OrderCoffee(CupSize.Large);
            _service.CompleteCup();

            Assert.That(_service.CurrentOrder().Count(), Is.Not.EqualTo(0));

            _service.CompleteOrderAndPay(10m);
            Assert.That(_service.CurrentOrder().Count(), Is.Not.EqualTo(0));
        }
    }
}

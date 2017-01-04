using NUnit.Framework;
using Service;
using Model;

namespace ServiceTests
{
    [TestFixture]
    public class PaymentServiceTests
    {
        private IPaymentService _service;

        [SetUp]
        public void SetUp()
        {
            _service = new PaymentService();
        }

        [TestCase(10.50, 10.50, PaymentStatus.Success)]
        [TestCase(10.50, 11, PaymentStatus.Success)]
        [TestCase(1, 2, PaymentStatus.Success)]
        [TestCase(0, 10, PaymentStatus.Success)]
        public void WhenPaymentIsValid_SuccessReturned(decimal due, decimal payment, PaymentStatus expected)
        {
            var result = _service.RemitPayment(due, payment);
            Assert.That(result.PaymentStatus, Is.EqualTo(expected));
        }

        [TestCase(0, 0, 0)]
        [TestCase(0, 1, 1)]
        [TestCase(0.50, 10, 9.5)]
        public void WhenPaymentIsValid_ChangeIsSet(decimal due, decimal payment, decimal expectedChange)
        {
            var result = _service.RemitPayment(due, payment);
            Assert.That(result.Change, Is.EqualTo(expectedChange));
        }

        [TestCase(10, 5)]
        [TestCase(10, 10.01)]
        public void WhenPaymentIsInvalid_ChangeIsZero(decimal due, decimal payment)
        {
            var result = _service.RemitPayment(due, payment);
            Assert.That(result.Change, Is.EqualTo(0));
        }

        [TestCase(10, -1)]
        [TestCase(10, 10.01)]
        public void WhenPaymentIsInvalid_StatusIsInvalid(decimal due, decimal payment)
        {
            var result = _service.RemitPayment(due, payment);
            Assert.That(result.PaymentStatus, Is.EqualTo(PaymentStatus.InvalidAmount));
        }

        [TestCase(10, 5)]
        [TestCase(10, 9.95)]
        public void WhenPaymentIsInsufficient_StatusIsInsufficient(decimal due, decimal payment)
        {
            var result = _service.RemitPayment(due, payment);
            Assert.That(result.PaymentStatus, Is.EqualTo(PaymentStatus.InsufficientFunds));
        }
    }
}

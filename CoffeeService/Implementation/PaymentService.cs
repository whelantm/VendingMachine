using Model;

namespace Service
{
    public class PaymentService : IPaymentService
    {
        public PaymentResult RemitPayment(decimal due, decimal payment)
        {
            PaymentResult result = new PaymentResult();
            if (!ValidatePayment(payment))
            {
                result.PaymentStatus = PaymentStatus.InvalidAmount;
                return result;
            }
            if (payment < due)
            {
                result.PaymentStatus = PaymentStatus.InsufficientFunds;
                return result;
            }

            result.PaymentStatus = PaymentStatus.Success;
            result.Change = payment - due;
            return result;
        }

        private bool ValidatePayment(decimal payment)
        {
            if (payment < 0)
                return false;
            if ((payment * 100) % 5 != 0)
                return false;

            return true;
        }
    }
}

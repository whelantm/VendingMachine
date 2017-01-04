using Model;

namespace Service
{
    public interface IPaymentService
    {
        PaymentResult RemitPayment(decimal due, decimal payment);
    }
}

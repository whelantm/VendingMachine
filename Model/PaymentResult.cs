namespace Model
{
    public enum PaymentStatus
    {
        Success,
        InvalidAmount,
        InsufficientFunds
    }
    public class PaymentResult
    {
        public PaymentStatus PaymentStatus { get; set; }
        public decimal Change { get; set; }
    }
}

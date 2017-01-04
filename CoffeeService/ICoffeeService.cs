using Model;
using System.Collections.Generic;

namespace Service
{
    public interface ICoffeeService
    {
        void OrderCoffee(CupSize size);
        bool OrderCondiment(CondimentType condimentType, int quantity);
        void CancelCurrentCup();
        void CancelOrder();
        bool CompleteCup();
        CoffeeOrder ReviewCurrentCup();
        IEnumerable<CoffeeOrder> CurrentOrder();
        decimal Total();
        PaymentResult CompleteOrderAndPay(decimal payment);
    }
}

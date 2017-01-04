using System;
using System.Collections.Generic;
using Model;

namespace Service
{
    public class CoffeeService : ICoffeeService
    {
        private readonly IApplicationSettingsService _appSettings;
        private readonly ICondimentServiceFactory _condimentServiceFactory;
        private IList<CoffeeOrder> _orders;
        private CoffeeOrder _currentCup;
        private IPaymentService _paymentService;

        public CoffeeService(ICondimentServiceFactory condimentServiceFactory, IPaymentService paymentService, IApplicationSettingsService appSettings)
        {
            _condimentServiceFactory = condimentServiceFactory;
            _paymentService = paymentService;
            _appSettings = appSettings;
            _orders = new List<CoffeeOrder>();
        }
        public void CancelOrder()
        {
            _orders = new List<CoffeeOrder>();
        }
        public void CancelCurrentCup()
        {
            _currentCup = null;
        }

        public bool CompleteCup()
        {
            if (_currentCup == null)
                return false;

            _orders.Add(_currentCup);
            _currentCup = null;

            return true;
        }

        public CoffeeOrder ReviewCurrentCup()
        {
            return _currentCup;            
        }

        public IEnumerable<CoffeeOrder> CurrentOrder()
        {
            return _orders;
        }

        public void OrderCoffee(CupSize size)
        {
            if (_currentCup == null)
            {
                _currentCup = new CoffeeOrder();
            }

            _currentCup.Coffee = new Coffee()
            {
                Size = size,
                Price = CoffeePrice(size)
            };
        }

        public bool OrderCondiment(CondimentType condimentType, int quantity)
        {
            if (_currentCup == null)
                return false;

            ICondimentService service = _condimentServiceFactory.Create(condimentType);
            if (!service.IsCondimentValid(quantity))
            {
                return false;
            }

            if (_currentCup.Condiments.ContainsKey(condimentType))
            {
                _currentCup.Condiments.Remove(condimentType);
            }

            var condiment = service.OrderCondiment(quantity);
            _currentCup.Condiments.Add(condimentType, condiment);

            return true;
        }

        public decimal Total()
        {
            decimal price = 0;
            foreach(var order in _orders)
            {
                price = price + order.Price();
            }
            return price;
        }

        private decimal CoffeePrice(CupSize size)
        {
            switch(size)
            {
                case CupSize.Small:
                    return _appSettings.SmallCoffeePrice;
                case CupSize.Medium:
                    return _appSettings.MediumCoffeePrice;
                case CupSize.Large:
                    return _appSettings.LargeCoffeePrice;
                default:
                    throw new ArgumentOutOfRangeException("Invalid coffee size");
            }
        }

        public PaymentResult CompleteOrderAndPay(decimal payment)
        {
            var result = _paymentService.RemitPayment(Total(), payment);
            if (result.PaymentStatus == PaymentStatus.Success)
            {
                _orders = new List<CoffeeOrder>();
                _currentCup = null;
            }
            return result;
        }
    }
}

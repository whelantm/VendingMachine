using System;
using Service;
using Model;

namespace VendingMachine
{
    class Program
    {
        private static IApplicationSettingsService _appSettings;
        private static ICoffeeService _coffeeService;
        private static IPaymentService _paymentService;
        private static ICondimentServiceFactory _condimentServiceFactory;

        static void Main(string[] args)
        {
            // Ideally this would be handled via an IOC container for dependency injection 
            // in the interest of time I'm handling it manually here
            _appSettings = new ApplicationSettingsService();
            _condimentServiceFactory = new CondimentServiceFactory(_appSettings);
            _paymentService = new PaymentService();
            _coffeeService = new CoffeeService(_condimentServiceFactory, _paymentService, _appSettings);

            while(true)
            {
                string input = MainMenu();
                switch(input.ToLowerInvariant())
                {
                    case "1":
                        OrderCoffee();
                        break;
                    case "2":
                        ReviewOrder();
                        break;
                    case "3":
                        CancelOrder();
                        break;
                    case "4":
                        Pay();
                        break;
                    case "q":
                        return;
                }
            }
        }

        static string MainMenu()
        {
            Console.WriteLine("Enter a choice from the following options");
            Console.WriteLine("1. Order Coffee");
            Console.WriteLine("2. Review Order");
            Console.WriteLine("3. Cancel Order");
            Console.WriteLine("4. Complete Order and Pay");
            Console.WriteLine();
            Console.WriteLine("Q - Quit");
            Console.Write(">>");
            return Console.ReadLine();
        }

        static void OrderCoffee()
        {
            while(true)
            {
                Console.WriteLine("1. Select Size");
                Console.WriteLine("2. Order Cream");
                Console.WriteLine("3. Order Sugar");
                Console.WriteLine("4. Cancel Cup");
                Console.WriteLine("5. Complete Order");
                Console.Write(">>");
                string input = Console.ReadLine();
                switch(input)
                {
                    case "1":
                        SelectSize();
                        break;
                    case "2":
                        OrderCream();
                        break;
                    case "3":
                        OrderSugar();
                        break;
                    case "4":
                        _coffeeService.CancelCurrentCup();
                        return;
                    case "5":
                        _coffeeService.CompleteCup();
                        return;
                }
            }
        }

        static void SelectSize()
        {
            string input = "";
            while (!string.Equals(input, "S", StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(input, "M", StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(input, "L", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Specify size: (S)mall (M)edium (L)arge");
                Console.Write(">>");
                input = Console.ReadLine();
            }

            CupSize size;
            switch(input.ToLowerInvariant())
            {
                case "s":
                    size = CupSize.Small;
                    break;
                case "m":
                    size = CupSize.Medium;
                    break;
                case "l":
                    size = CupSize.Large;
                    break;
                default:
                    Console.WriteLine("Invalid cup size");
                    return;
            }
            _coffeeService.OrderCoffee(size);
        }

        static void OrderCream()
        {
            string input = "";
            int quantity = 0;
            while(!int.TryParse(input, out quantity))
            {
                Console.WriteLine("Please enter the amount of cream");
                Console.Write(">>");
                input = Console.ReadLine();
            }

            if (!_coffeeService.OrderCondiment(CondimentType.Cream, quantity))
            {
                Console.WriteLine("You have entered an invalid amount of cream");
                return;
            }
        }

        static void OrderSugar()
        {
            string input = "";
            int quantity = 0;
            while (!int.TryParse(input, out quantity))
            {
                Console.WriteLine("Please enter the amount of sugar");
                Console.Write(">>");
                input = Console.ReadLine();
            }

            if (!_coffeeService.OrderCondiment(CondimentType.Sugar, quantity))
            {
                Console.WriteLine("You have entered an invalid amount of sugar");
                return;
            }
        }

        static void ReviewOrder()
        {
            var orders = _coffeeService.CurrentOrder();
            foreach(var order in orders)
            {
                Console.WriteLine(order.ToString());
            }
        }

        static void CancelOrder()
        {
            _coffeeService.CancelOrder();
        }

        static void Pay()
        {
            while (true)
            {
                Console.WriteLine("Please Pay {0:C}", _coffeeService.Total());
                Console.Write(">>");
                string input = Console.ReadLine();
                decimal payment = 0;
                if (decimal.TryParse(input, out payment))
                {
                    var result = _coffeeService.CompleteOrderAndPay(payment);
                    if (result.PaymentStatus == PaymentStatus.Success)
                    {
                        Console.WriteLine("Thank you for your payment, your change is {0:C}", result.Change);
                        return;
                    }
                    else if (result.PaymentStatus == PaymentStatus.InsufficientFunds)
                    {
                        Console.WriteLine("You have not paid enough to cover your bill.");
                    }
                    else if (result.PaymentStatus == PaymentStatus.InvalidAmount)
                    {
                        Console.WriteLine("The amount you entered was invalid.");
                    }
                }
            }
        }
    }
}

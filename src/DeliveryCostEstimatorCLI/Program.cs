using CourierService;
using CourierService.Contracts;
using CourierService.Messages;
using CourierService.Models;
using CourierService.Translator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;

namespace DeliveryCostEstimatorCLI
{
    public class Program
    {
        private static IConfigurationRoot Configuration;
        private static IServiceProvider ServiceProvider;
        private static double _baseDeliveryCost;

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Kiki's Courier Service");
            try
            {
                Initialize();
                var deliveryCostCalculator = GetDeliveryCostCalculator();

                var deliveryTimeCalculatorRQ = GetDeliveryTimeCalculatorRQ();
                var deliveryTimeCalculatorRS = DeliveryTimeCalculator.Calculate(deliveryTimeCalculatorRQ);

                if (deliveryTimeCalculatorRS != null)
                {
                    foreach (var orderWithDeliveryTime in deliveryTimeCalculatorRS.OrdersWithDeliveryTime)
                    {
                        var request = DeliveryCostCalculatorTranslator.GetDeliveryCostCalculatorRQ(orderWithDeliveryTime.Order, _baseDeliveryCost);
                        var deliveryCostCalculatorRS = deliveryCostCalculator.Calculate(request);
                        DisplayDetails(deliveryCostCalculatorRS, orderWithDeliveryTime.DeliveryTime);
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Got Exception : {ex.Message} \n {ex.StackTrace}");
            }
            Console.ReadKey();
        }

        public static void Initialize()
        {
            #region Build configuration
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();
            #endregion

            #region Dependency Injection
            ServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IConfigurationRoot>(Configuration);
            serviceCollection.AddSingleton<IOfferStore, FileOfferStore>();

            ServiceProvider = serviceCollection.BuildServiceProvider();
            #endregion
        }

        public static DeliveryCostCalculator GetDeliveryCostCalculator()
        {
            double costPerKGWeight = Configuration.GetSection(Constants.ConfigurationSections.CostPerKGWeight).Get<double>();
            double costPerKM = Configuration.GetSection(Constants.ConfigurationSections.CostPerKM).Get<double>();
            var offerStore = ServiceProvider.GetService<IOfferStore>();

            return new DeliveryCostCalculator(offerStore, costPerKGWeight, costPerKM);
        }

        private static DeliveryTimeCalculatorRQ GetDeliveryTimeCalculatorRQ()
        {
            DeliveryTimeCalculatorRQ deliveryTimeCalculatorRQ = null;
            Console.WriteLine("Enter the base_delivery_cost and no_of_packges as space seperated values.");
            var line1 = Console.ReadLine();
            var line1Values = line1.Split(' ');
            if (line1Values.Length == 2)
            {
                _baseDeliveryCost = double.Parse(line1Values[0]);
                var numberOfPackages = int.Parse(line1Values[1]);

                List<Order> orders = ReadOrders(numberOfPackages);

                Console.WriteLine("Enter the no_of_vehicles max_speed max_carriable_weight as space seperated values.");
                var valuesForTimeCalculation = Console.ReadLine().Split(' ');
                if (valuesForTimeCalculation.Length == 3)
                {
                    deliveryTimeCalculatorRQ = DeliveryTimeCalculatorTranslator.GetDeliveryTimeCalculatorRQ(orders, valuesForTimeCalculation);
                }
                else
                {
                    Console.WriteLine("Please enter Valid Input : no_of_vehicles max_speed max_carriable_weight as space seperated values.");
                }
            }
            else
            {
                Console.WriteLine("Please enter Valid Input : base_delivery_cost and no_of_packges as space seperated values.");
            }
            return deliveryTimeCalculatorRQ;
        }

        private static List<Order> ReadOrders(int numberOfPackages)
        {
            List<Order> orders = new List<Order>();
            Console.WriteLine("Enter the package info (pkg_id pkg_weight_in_kg distance_in_km offer_code) as space seperated values.");
            for (int i = 0; i < numberOfPackages; i++)
            {
                Console.WriteLine($"Enter info of Package number : {i}");
                Order order = OrderTranslator.GetOrder(Console.ReadLine());
                orders.Add(order);
            }
            return orders;
        }

        private static void DisplayDetails(DeliveryCostCalculatorRS response, double deliveryTime)
        {
            Console.WriteLine($"{response.Order.Package.Id} {response.DiscountAmmount} {response.FinalDeliveryCost} {deliveryTime}");
        }
    }
}

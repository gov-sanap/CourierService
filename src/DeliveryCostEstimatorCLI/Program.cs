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
        public static IConfigurationRoot ConfigurationRoot;
        public static IServiceProvider ServiceProvider;
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Initialize();
            var deliveryCostCalculator = GetDeliveryCostCalculator();

            Console.WriteLine("Enter the base_delivery_cost and no_of_packges as space seperated values.");
            var line1 = Console.ReadLine();
            var line1Values = line1.Split(' ');
            if(line1Values.Length == 2)
            {
                double baseDeliveryCost = double.Parse(line1Values[0]);
                int numberOfPackages = int.Parse(line1Values[1]);

                List<Order> orders = GetOrders(numberOfPackages);

                Console.WriteLine("Enter the no_of_vehicles max_speed max_carriable_weight as space seperated values.");
                var valuesForTimeCalculation = Console.ReadLine().Split(' ');
                if(valuesForTimeCalculation.Length == 3)
                {
                    var deliveryTimeCalculatorRQ = new DeliveryTimeCalculatorRQ
                    {
                        NumberOfVehicles = int.Parse(valuesForTimeCalculation[0]),
                        MaxSpeed = double.Parse(valuesForTimeCalculation[1]),
                        MaxCarriableWeight = double.Parse(valuesForTimeCalculation[2])
                    };

                    var deliveryTimeCalculatorRS = new DeliveryTimeCalculator().Calculate(deliveryTimeCalculatorRQ);

                    foreach (var orderWithDeliveryTime in deliveryTimeCalculatorRS.OrdersWithDeliveryTime)
                    {
                        var request = DeliveryCostCalculatorTranslator.GetDeliveryCostCalculatorRQ(orderWithDeliveryTime.Order, baseDeliveryCost);
                        var deliveryCostCalculatorRS = deliveryCostCalculator.Calculate(request);
                        DisplayDetails(deliveryCostCalculatorRS, orderWithDeliveryTime.DeliveryTime);
                    }
                }
            }
        }

        private static void DisplayDetails(DeliveryCostCalculatorRS response, double deliveryTime)
        {
            Console.WriteLine($"{response.Order.Package.Id} {response.DiscountAmmount} {response.TotalAmmount} {deliveryTime}");
        }

        private static List<Order> GetOrders(int numberOfPackages)
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

        public static void Initialize()
        {
            #region Build configuration
            ConfigurationRoot = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();
            #endregion

            #region Dependency Injection
            ServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IConfigurationRoot>(ConfigurationRoot);
            serviceCollection.AddSingleton<IOfferStore, FileOfferStore>();

            ServiceProvider = serviceCollection.BuildServiceProvider();
            #endregion
        }

        public static DeliveryCostCalculator GetDeliveryCostCalculator()
        {
            double costPerKGWeight = ConfigurationRoot.GetSection(Constants.ConfigurationSections.CostPerKGWeight).Get<double>();
            double costPerKM = ConfigurationRoot.GetSection(Constants.ConfigurationSections.CostPerKM).Get<double>();
            var offerStore = ServiceProvider.GetService<IOfferStore>();

            return new DeliveryCostCalculator(offerStore, costPerKGWeight, costPerKM);
        }
    }
}

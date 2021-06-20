using CourierService.Exceptions;
using CourierService.Helpers;
using CourierService.Messages;
using CourierService.Models;
using CourierService.Translator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourierService
{
    public static class DeliveryTimeCalculator
    {
        public static DeliveryTimeCalculatorRS Calculate(DeliveryTimeCalculatorRQ request)
        {
            var response = new DeliveryTimeCalculatorRS();

            if (request.IsValid())
            {
                var allRemainingOrders = GetAllOrders(request.Orders);
                var vehiclesReturningTime = new List<double>();
                for (int i = 0; i < request.NumberOfVehicles; i++)
                {
                    vehiclesReturningTime.Add(0);
                }

                while (allRemainingOrders.Any())
                {
                    var index = vehiclesReturningTime.IndexOf(vehiclesReturningTime.Min());
                    List<double> totalTimes = new List<double>();

                    Shipment qualifiedShipment = ShipmentHelper.GetQualifiedShipment(allRemainingOrders, request.MaxCarriableWeight);

                    foreach (var order in qualifiedShipment.Orders)
                    {
                        var orderWithEstimatedTime = new OrderWithDeliveryTime
                        {
                            Order = order,
                            DeliveryTime = vehiclesReturningTime[index] + Math.Round(order.DistanceInKM / request.MaxSpeed, 2)
                        };
                        response.OrdersWithDeliveryTime.Add(orderWithEstimatedTime);
                        allRemainingOrders.Remove(order);
                    }
                    qualifiedShipment.Orders.ForEach(order => allRemainingOrders.Remove(order));
                    vehiclesReturningTime[index] += 2 * Math.Round(qualifiedShipment.LongestDistance / request.MaxSpeed, 2);
                }

                response.OrdersWithDeliveryTime = (from orderWithEstimatedTime in response.OrdersWithDeliveryTime
                                                    orderby orderWithEstimatedTime.Order.Package.Id
                                                    select orderWithEstimatedTime).ToList();
            }
            else
            {
                throw new InvalidRequestException("Check whether orders are provided and weight of no package is greater than maxCarriableWeight.");
            }
            return response;
        }

        private static List<Order> GetAllOrders(List<Order> orders)
        {
            var allOrders = new List<Order>();
            foreach(var order in orders)
            {
                allOrders.Add(new Order {
                    DistanceInKM = order.DistanceInKM,
                    OfferCode = order.OfferCode,
                    Package = new Package
                    {
                        Id = order.Package.Id,
                        WeightInKG = order.Package.WeightInKG
                    }
                });
            }
            return allOrders;
        }
    }
}

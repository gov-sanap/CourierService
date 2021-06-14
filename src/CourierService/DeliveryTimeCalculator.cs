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
    public class DeliveryTimeCalculator
    {
        public DeliveryTimeCalculatorRS Calculate(DeliveryTimeCalculatorRQ request)
        {
            if(request?.Orders != null)
            {
                var response = new DeliveryTimeCalculatorRS
                {
                    OrdersWithDeliveryTime = new List<OrderWithDeliveryTime>()
                };
                
                var vehiclesReturningTime = new List<double>();
                for (int i = 0; i < request.NumberOfVehicles; i++)
                {
                    vehiclesReturningTime.Add(0);
                }

                while (request.Orders.Any())
                {
                    var index = vehiclesReturningTime.IndexOf(vehiclesReturningTime.Min());
                    List<double> totalTimes = new List<double>();

                    Shipment qualifiedShipment = ShipmentHelper.GetQualifiedShipment(request.Orders, request.MaxCarriableWeight);

                    foreach (var order in qualifiedShipment.Orders)
                    {
                        var orderWithEstimatedTime = new OrderWithDeliveryTime
                        {
                            Order = order,
                            DeliveryTime = vehiclesReturningTime[index] + Math.Round(order.DistanceInKM / request.MaxSpeed, 2)
                        };
                        response.OrdersWithDeliveryTime.Add(orderWithEstimatedTime);
                        request.Orders.Remove(order);
                    }
                    qualifiedShipment.Orders.ForEach(order => request.Orders.Remove(order));
                    vehiclesReturningTime[index] += 2 * Math.Round(qualifiedShipment.LongestDistance / request.MaxSpeed, 2);
                }

                response.OrdersWithDeliveryTime = (from orderWithEstimatedTime in response.OrdersWithDeliveryTime
                                                    orderby orderWithEstimatedTime.Order.Package.Id
                                                    select orderWithEstimatedTime).ToList();
                return response;
            }
            return null;
        }
    }
}

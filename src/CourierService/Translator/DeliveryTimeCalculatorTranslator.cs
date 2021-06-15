using CourierService.Messages;
using CourierService.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourierService.Translator
{
    public class DeliveryTimeCalculatorTranslator
    {
        public static DeliveryTimeCalculatorRQ GetDeliveryTimeCalculatorRQ(List<Order> orders, int numberOfVehicles, double maxSpeed, double maxCarriableWeight)
        {
            return new DeliveryTimeCalculatorRQ
            {
                Orders = orders,
                NumberOfVehicles = numberOfVehicles,
                MaxSpeed = maxSpeed,
                MaxCarriableWeight = maxCarriableWeight
            };
        }

        public static DeliveryTimeCalculatorRS GetDeliveryTimeCalculatorRS(List<OrderWithDeliveryTime> orderWithEstimatedTimes)
        {
            return new DeliveryTimeCalculatorRS
            {
                OrdersWithDeliveryTime = orderWithEstimatedTimes
            };
        }

        public static OrderWithDeliveryTime GetOrderWithEstimatedTime(Order order, double totalTime)
        {
            return new OrderWithDeliveryTime
            {
                Order = order,
                DeliveryTime = totalTime
            };
        }

        public static DeliveryTimeCalculatorRQ GetDeliveryTimeCalculatorRQ(List<Order> orders, string[] valuesForTimeCalculation)
        {
            var deliveryTimeCalculatorRQ = new DeliveryTimeCalculatorRQ
            {
                NumberOfVehicles = int.Parse(valuesForTimeCalculation[0]),
                MaxSpeed = double.Parse(valuesForTimeCalculation[1]),
                MaxCarriableWeight = double.Parse(valuesForTimeCalculation[2]),
                Orders = orders
            };
            return deliveryTimeCalculatorRQ;
        }
    }
}

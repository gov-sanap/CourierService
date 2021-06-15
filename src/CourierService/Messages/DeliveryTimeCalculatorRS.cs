using CourierService.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourierService.Messages
{
    public class DeliveryTimeCalculatorRS
    {
        public List<OrderWithDeliveryTime> OrdersWithDeliveryTime { get; set; } = new List<OrderWithDeliveryTime>();
    }

    public class OrderWithDeliveryTime
    {
        public Order Order { get; set; }
        public double DeliveryTime { get; set; }
    }
}

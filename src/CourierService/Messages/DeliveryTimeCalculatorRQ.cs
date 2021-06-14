using CourierService.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourierService.Messages
{
    public class DeliveryTimeCalculatorRQ
    {
        public List<Order> Orders { get; set; }
        public int NumberOfVehicles { get; set; }
        public double MaxSpeed { get; set; }
        public double MaxCarriableWeight { get; set; }
    }
}

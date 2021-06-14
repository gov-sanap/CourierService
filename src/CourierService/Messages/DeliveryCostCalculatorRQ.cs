using CourierService.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourierService.Messages
{
    public class DeliveryCostCalculatorRQ
    {
        public Order Order { get; set; }
        public double BaseDeliveryCost { get; set; }
    }
}

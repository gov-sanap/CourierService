using CourierService.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourierService.Messages
{
    public class DeliveryCostCalculatorRS
    {
        public Order Order { get; set; }
        public double DiscountAmmount { get; set; }
        public double FinalDeliveryCost { get; set; }
    }
}

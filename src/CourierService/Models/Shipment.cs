using System;
using System.Collections.Generic;
using System.Text;

namespace CourierService.Models
{
    public class Shipment
    {
        public int Length { get; set; }
        public double TotalWeight { get; set; }
        public List<Order> Orders { get; set; }
        public double LongestDistance { get; internal set; }
    }
}

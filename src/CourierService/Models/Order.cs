using System;
using System.Collections.Generic;
using System.Text;

namespace CourierService.Models
{
    public class Order
    {
        public Package Package { get; set; }
        public double DistanceInKM { get; set; }
        public string OfferCode { get; set; }
    }
}

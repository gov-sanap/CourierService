using CourierService.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourierService.Translator
{
    public class OrderTranslator
    {
        public static Order GetOrder(string orderString)
        {
            var orderValues = orderString.Split(' ');
            return new Order
            {
                Package = new Package
                {
                    Id = orderValues[0],
                    WeightInKG = double.Parse(orderValues[1])
                },
                DistanceInKM = double.Parse(orderValues[2]),
                OfferCode = orderValues[3]
            };
        }
    }
}

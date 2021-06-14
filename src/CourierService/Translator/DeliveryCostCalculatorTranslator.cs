using CourierService.Messages;
using CourierService.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourierService.Translator
{
    public static class DeliveryCostCalculatorTranslator
    {
        public static DeliveryCostCalculatorRQ GetDeliveryCostCalculatorRQ(Order order, double baseDeliveryCost)
        {
            return new DeliveryCostCalculatorRQ
            {
                Order = order,
                BaseDeliveryCost = baseDeliveryCost
            };
        }

        public static DeliveryCostCalculatorRS GetDeliveryCostCalculatorRS(Order order, double discountAmmount, double totalAmmount)
        {
            return new DeliveryCostCalculatorRS
            {
                Order = order,
                DiscountAmmount = discountAmmount,
                TotalAmmount = totalAmmount
            };
        }
    }
}

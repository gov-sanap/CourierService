using CourierService.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourierService.Models
{
    public class Offer
    {
        public string Code { get; set; }
        public List<Rule> Rules { get; set; }
        public double Discount { get; set; }
        public DiscountType DiscountType { get; set; }

        public bool IsApplicable(Order order)
        {
            return Rules.TrueForAll(rule => rule.Qualify(order));
        }

        public double GetDiscountAmmount(double deliveryCost)
        {
            if (DiscountType.Equals(DiscountType.Percentage))
            {
                return deliveryCost * Discount / 100;
            }
            else
            {
                return Discount;
            }
        }
    }
}

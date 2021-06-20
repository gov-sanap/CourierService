using CourierService.Enums;
using CourierService.Exceptions;
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
            if(Discount < 0)
            {
                throw new NegativeDiscountException("Discount can not be negative");
            }

            if (DiscountType.Equals(DiscountType.Percentage))
            {
                return Discount > 0 ? deliveryCost * Discount / 100 : 0;
            }
            else
            {
                return Discount;
            }
        }
    }
}

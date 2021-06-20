using CourierService.Contracts;
using CourierService.Messages;
using CourierService.Models;
using CourierService.Translator;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourierService
{
    public class DeliveryCostCalculator
    {
        private double _costPerKGWeight;
        private double _costPerKM;
        private IOfferStore _offerStore;

        public DeliveryCostCalculator(IOfferStore offerStore, double costPerKGWeight, double costPerKM)
        {
            _offerStore = offerStore;
            _costPerKGWeight = costPerKGWeight;
            _costPerKM = costPerKM;
        }

        public DeliveryCostCalculatorRS Calculate(DeliveryCostCalculatorRQ request)
        {
            if(request?.Order == null)
            {
                return DeliveryCostCalculatorTranslator.GetDeliveryCostCalculatorRS(request.Order, 0, 0);
            }

            var totalDeliveryCost = GetTotalDeliveryCost(request.Order, request.BaseDeliveryCost);
            
            var discountAmmount = GetDiscountAmmount(totalDeliveryCost, request.Order);

            var finalAmmount = totalDeliveryCost - discountAmmount;

            return DeliveryCostCalculatorTranslator.GetDeliveryCostCalculatorRS(request.Order, discountAmmount, finalAmmount);
        }

        private double GetTotalDeliveryCost(Order order, double baseDeliveryCost)
        {
            var deliveryCost = baseDeliveryCost + (order.Package.WeightInKG * _costPerKGWeight)
                        + (order.DistanceInKM * _costPerKM);
            return deliveryCost;
        }

        private double GetDiscountAmmount(double deliveryCost, Order order)
        {
            if (!string.IsNullOrEmpty(order.OfferCode))
            {
                Offer offer = _offerStore.GetOffer(order.OfferCode);
                if (offer != null && offer.IsApplicable(order))
                {
                    return offer.GetDiscountAmmount(deliveryCost);
                }
            }
            return 0;
        }
    }
}

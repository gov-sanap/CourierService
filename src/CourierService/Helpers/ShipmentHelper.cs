using CourierService.Models;
using System.Collections.Generic;
using System.Linq;

namespace CourierService.Helpers
{
    public static class ShipmentHelper
    {
        private static List<Shipment> _shipments = new List<Shipment>();

        public static Shipment GetQualifiedShipment(List<Order> orders, double maxCarriableWeight)
        {
            _shipments = new List<Shipment>();
            var useStatuses = new List<bool>();
            orders.ForEach(_ => useStatuses.Add(false));

            int largestSubsetSize = GetMaxSubsetLengthWithTotalWeightLessThanMaxCarriableWeight(orders, maxCarriableWeight);

            SubsetsOfGivenLengthWithTotalWeightsLessThanMaxWeight(orders, maxCarriableWeight, largestSubsetSize, 0, 0, useStatuses);

            Shipment optimalShipment = GetShipmentWithMaxTotalWeightAndCanBeDeliveredFirst(_shipments);
            return optimalShipment;
        }

        private static int GetMaxSubsetLengthWithTotalWeightLessThanMaxCarriableWeight(List<Order> orders, double maxCarriableWeight)
        {
            int subsetSize = 0;
            double totalWeight = 0;
            var sortedOrders = (from order in orders
                                orderby order.Package.WeightInKG
                                select order).ToList();

            foreach (var order in sortedOrders)
            {
                if ((totalWeight + order.Package.WeightInKG) <= maxCarriableWeight)
                {
                    totalWeight += order.Package.WeightInKG;
                    subsetSize++;
                }
            }
            return subsetSize;
        }

        private static Shipment GetShipmentWithMaxTotalWeightAndCanBeDeliveredFirst(List<Shipment> shipments)
        {
            var sortedShipments = (from shipment in _shipments
                                   orderby shipment.TotalWeight descending
                                   select shipment).ToList();

            var maxTotalWeight = sortedShipments.First().TotalWeight;
            double longestDistance = LongestDistance(sortedShipments.First().Orders);
            var optimalShipment = sortedShipments.First();
            foreach (var shipment in sortedShipments)
            {
                if (shipment.TotalWeight < maxTotalWeight)
                {
                    break;
                }
                else
                {
                    var longestDistanceFromCurrentShipment = LongestDistance(shipment.Orders);
                    if (longestDistance > longestDistanceFromCurrentShipment)
                    {
                        longestDistance = longestDistanceFromCurrentShipment;
                        optimalShipment = shipment;
                    }
                }
            }
            optimalShipment.LongestDistance = longestDistance;
            return optimalShipment;
        }

        private static double LongestDistance(List<Order> orders)
        {
            var longestDistance = orders.Max(y => y.DistanceInKM);
            return longestDistance;
        }

        private static void SubsetsOfGivenLengthWithTotalWeightsLessThanMaxWeight(List<Order> orders, double maxWeight, int sizeOfSubset, int start, int currentLength, List<bool> used)
        {
            if (currentLength == sizeOfSubset)
            {
                var orderSubset = new List<Order>();
                double totalWeight = 0;
                for (int i = 0; i < orders.Count(); i++)
                {
                    if (used[i] == true)
                    {
                        orderSubset.Add(orders[i]);
                        totalWeight += orders[i].Package.WeightInKG;
                    }
                }
                if (totalWeight <= maxWeight)
                    _shipments.Add(new Shipment
                    {
                        Orders = orderSubset,
                        TotalWeight = totalWeight,
                        Length = sizeOfSubset
                    });
                return;
            }
            if (start == orders.Count())
            {
                return;
            }

            used[start] = true;
            SubsetsOfGivenLengthWithTotalWeightsLessThanMaxWeight(orders, maxWeight, sizeOfSubset, start + 1, currentLength + 1, used);

            used[start] = false;
            SubsetsOfGivenLengthWithTotalWeightsLessThanMaxWeight(orders, maxWeight, sizeOfSubset, start + 1, currentLength, used);
        }
    }
}

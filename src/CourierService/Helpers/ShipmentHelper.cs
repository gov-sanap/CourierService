using CourierService.Models;
using System.Collections.Generic;
using System.Linq;

namespace CourierService.Helpers
{
    internal static class ShipmentHelper
    {
        private static List<Shipment> _shipments = new List<Shipment>();

        internal static Shipment GetQualifiedShipment(List<Order> orders, double maxCarriableWeight)
        {
            _shipments = new List<Shipment>();
            var useStatuses = new List<bool>();
            orders.ForEach(_ => useStatuses.Add(false));
            
            //Get Length of largest subset With TotalWeight LessThan MaxCarriableWeight
            int largestSubsetSize = GetLargestSubsetSize(orders, maxCarriableWeight);

            //Add all possible shipments with orders count as largestSubsetSize 
            //and totalWeight is less than maxcarriableWeight to _shipments.
            AddAllShipments(orders, maxCarriableWeight, largestSubsetSize, 0, 0, useStatuses);

            Shipment optimalShipment = GetOptimalShipment(_shipments);
            return optimalShipment;
        }

        private static int GetLargestSubsetSize(List<Order> orders, double maxCarriableWeight)
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

        private static Shipment GetOptimalShipment(List<Shipment> shipments)
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

        private static void AddAllShipments(List<Order> orders, double maxWeight, int sizeOfSubset, int start, int currentLength, List<bool> areOrdersUsed)
        {
            if (currentLength == sizeOfSubset)
            {
                var orderSubset = new List<Order>();
                double totalWeight = 0;
                for (int i = 0; i < orders.Count(); i++)
                {
                    if (areOrdersUsed[i] == true)
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

            areOrdersUsed[start] = true;
            AddAllShipments(orders, maxWeight, sizeOfSubset, start + 1, currentLength + 1, areOrdersUsed);

            areOrdersUsed[start] = false;
            AddAllShipments(orders, maxWeight, sizeOfSubset, start + 1, currentLength, areOrdersUsed);
        }
    }
}

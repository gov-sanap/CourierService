using CourierService.Messages;
using CourierService.Models;
using CourierService.Translator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourierService
{
    public class DeliveryTimeCalculator
    {
        private List<Shipment> _shipments = new List<Shipment>();

        public DeliveryTimeCalculatorRS Calculate(DeliveryTimeCalculatorRQ request)
        {
            if(request?.Orders != null)
            {
                var response = new DeliveryTimeCalculatorRS
                {
                    OrdersWithDeliveryTime = new List<OrderWithDeliveryTime>()
                };
                
                var vehiclesReturningTime = new List<double>();
                for (int i = 0; i < request.NumberOfVehicles; i++)
                {
                    vehiclesReturningTime.Add(0);
                }
                while (request.Orders.Any())
                {
                    var index = vehiclesReturningTime.IndexOf(vehiclesReturningTime.Min());
                    List<double> totalTimes = new List<double>();

                    Shipment qualifiedShipment = GetQualifiedShipment(request.Orders, request.MaxCarriableWeight);
                    foreach (var order in qualifiedShipment.Orders)
                    {
                        var orderWithEstimatedTime = new OrderWithDeliveryTime
                        {
                            Order = order,
                            DeliveryTime = vehiclesReturningTime[index] + Math.Round(order.DistanceInKM / request.MaxSpeed, 2)
                        };
                        response.OrdersWithDeliveryTime.Add(orderWithEstimatedTime);
                        request.Orders.Remove(order);
                    }
                    qualifiedShipment.Orders.ForEach(order => request.Orders.Remove(order));
                    vehiclesReturningTime[index] += 2 * Math.Round(qualifiedShipment.LongestDistance / request.MaxSpeed, 2);
                }

                response.OrdersWithDeliveryTime = (from orderWithEstimatedTime in response.OrdersWithDeliveryTime
                                                    orderby orderWithEstimatedTime.Order.Package.Id
                                                    select orderWithEstimatedTime).ToList();
                return response;
            }
            return null;
        }

        public Shipment GetQualifiedShipment(List<Order> orders, double maxCarriableWeight)
        {
            _shipments = new List<Shipment>();
            var useStatuses = new List<bool>();
            orders.ForEach(_ => useStatuses.Add(false));

            int largestSubsetSize = GetMaxSubsetLengthWithTotalWeightLessThanMaxCarriableWeight(orders, maxCarriableWeight);

            SubsetsOfGivenLengthWithTotalWeightsLessThanMaxWeight(orders, maxCarriableWeight, largestSubsetSize, 0, 0, useStatuses);

            Shipment optimalShipment = GetShipmentWithMaxTotalWeightAndCanBeDeliveredFirst(_shipments);
            return optimalShipment;
        }

        private int GetMaxSubsetLengthWithTotalWeightLessThanMaxCarriableWeight(List<Order> orders, double maxCarriableWeight)
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

        private Shipment GetShipmentWithMaxTotalWeightAndCanBeDeliveredFirst(List<Shipment> shipments)
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

        private double LongestDistance(List<Order> orders)
        {
            var longestDistance = orders.Max(y => y.DistanceInKM);
            return longestDistance;
        }

        public void SubsetsOfGivenLengthWithTotalWeightsLessThanMaxWeight(List<Order> orders, double maxWeight, int sizeOfSubset, int start, int currentLength, List<bool> used)
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
            // For every index we have two options,
            // 1.. Either we select it, means put true in used[] and make currLen+1
            used[start] = true;
            SubsetsOfGivenLengthWithTotalWeightsLessThanMaxWeight(orders, maxWeight, sizeOfSubset, start + 1, currentLength + 1, used);
            // 2.. OR we dont select it, means put false in used[] and dont increase
            used[start] = false;
            SubsetsOfGivenLengthWithTotalWeightsLessThanMaxWeight(orders, maxWeight, sizeOfSubset, start + 1, currentLength, used);
        }
    }
}

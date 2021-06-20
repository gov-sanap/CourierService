using CourierService.Helpers;
using CourierService.Models;
using CourierService.Translator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace CourierService.Fixture
{
    public class ShipmentHelperFixture
    {
        //QualifiedShipment: Shipment with max no. of orders with totalWeight less than maxCarriableWeight and can be delivered first.
        [Fact]
        public void GetQualifiedShipment_Should_Return_Shipment_1()
        {
            //This is normal senario where there is only one order subset with total weight less than maxcarriable weight.
            var orders = new List<Order>
            {
                OrderTranslator.GetOrder("PKG1 50 30 OFR001"),
                OrderTranslator.GetOrder("PKG2 140 50 OFFR003"),
                OrderTranslator.GetOrder("PKG3 75 125 OFFR0008"),
                OrderTranslator.GetOrder("PKG4 110 60 OFFR002"),
            };
            var qualifiedShipment = ShipmentHelper.GetQualifiedShipment(orders, 150);
            Assert.Equal(2, qualifiedShipment.Orders.Count());
            Assert.Contains<Order>(orders[0], qualifiedShipment.Orders);
            Assert.Contains<Order>(orders[2], qualifiedShipment.Orders);
        }

        [Fact]
        public void GetQualifiedShipment_Should_Return_Shipment_2()
        {
            //Here there are multiple order subset with total weight less than maxcarriable weight.
            //So we will pick longest subset.
            var orders = new List<Order>
            {
                OrderTranslator.GetOrder("PKG1 50 30 OFR001"),
                OrderTranslator.GetOrder("PKG2 30 125 OFFR0008"),
                OrderTranslator.GetOrder("PKG3 90 120 OFFR002"),
                OrderTranslator.GetOrder("PKG4 95 110 NA"),
                OrderTranslator.GetOrder("PKG5 60 92 NA"),
            };
            var qualifiedShipment = ShipmentHelper.GetQualifiedShipment(orders, 150);
            Assert.Equal(3, qualifiedShipment.Orders.Count());
            Assert.Contains<Order>(orders[0], qualifiedShipment.Orders);
            Assert.Contains<Order>(orders[1], qualifiedShipment.Orders);
            Assert.Contains<Order>(orders[4], qualifiedShipment.Orders);
        }

        [Fact]
        public void GetQualifiedShipment_Should_Return_Shipment_3()
        {
            //Here there are multiple order subset with total weight less than maxcarriable weight.
            //Also there are more than 1 subset with longest length.
            //We will pick one with maximum total sum.
            var orders = new List<Order>
            {
                OrderTranslator.GetOrder("PKG1 10 30 OFR001"),
                OrderTranslator.GetOrder("PKG2 40 125 OFFR0008"),
                OrderTranslator.GetOrder("PKG3 65 92 NA"),
                OrderTranslator.GetOrder("PKG4 90 120 OFFR002"),
                OrderTranslator.GetOrder("PKG5 60 95 NA"),
            };
            var qualifiedShipment = ShipmentHelper.GetQualifiedShipment(orders, 150);
            Assert.Equal(3, qualifiedShipment.Orders.Count());
            Assert.Contains<Order>(orders[0], qualifiedShipment.Orders);
            Assert.Contains<Order>(orders[1], qualifiedShipment.Orders);
            Assert.Contains<Order>(orders[3], qualifiedShipment.Orders);
        }

        [Fact]
        public void GetQualifiedShipment_Should_Return_Shipment_4()
        {
            //Here there are multiple order subset with total weight less than maxcarriable weight.
            //Also there are more than 1 subset with longest length.
            //Also there are multiple subset with total sum = maximum total sum. 
            //We will one which can be delivered first. i.e. one with smallest longestDistance value.
            //In following example there are subsets (0,3),(4,5) out of which we will select (4,5)
            var orders = new List<Order>
            {
                OrderTranslator.GetOrder("PKG1 35 30 OFR001"),
                OrderTranslator.GetOrder("PKG2 75 92 NA"),
                OrderTranslator.GetOrder("PKG3 90 120 OFFR002"),
                OrderTranslator.GetOrder("PKG4 60 95 NA"),
                OrderTranslator.GetOrder("PKG5 50 30 OFR001"),
                OrderTranslator.GetOrder("PKG6 45 65 OFFR0008"),
            };
            var qualifiedShipment = ShipmentHelper.GetQualifiedShipment(orders, 100);
            Assert.Equal(2, qualifiedShipment.Orders.Count());
            Assert.Contains<Order>(orders[4], qualifiedShipment.Orders);
            Assert.Contains<Order>(orders[5], qualifiedShipment.Orders);
        }
    }
}

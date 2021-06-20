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
            var orders = new List<Order>
            {
                OrderTranslator.GetOrder("PKG1 50 30 OFR001"),
                OrderTranslator.GetOrder("PKG2 75 125 OFFR0008"),
                OrderTranslator.GetOrder("PKG3 175 100 OFFR003"),
                OrderTranslator.GetOrder("PKG4 110 60 OFFR002"),
                OrderTranslator.GetOrder("PKG5 155 95 NA"),
            };
            var qualifiedShipment = ShipmentHelper.GetQualifiedShipment(orders, 200);
            Assert.Equal(2, qualifiedShipment.Orders.Count());
            Assert.Contains<Order>(orders[1], qualifiedShipment.Orders);
            Assert.Contains<Order>(orders[3], qualifiedShipment.Orders);
        }

        [Fact]
        public void GetQualifiedShipment_Should_Return_Shipment_2()
        {
            var orders = new List<Order>
            {
                OrderTranslator.GetOrder("PKG1 10 30 OFR001"),
                OrderTranslator.GetOrder("PKG2 40 125 OFFR0008"),
                OrderTranslator.GetOrder("PKG3 65 120 OFFR002"),
                OrderTranslator.GetOrder("PKG4 60 95 NA"),
                OrderTranslator.GetOrder("PKG5 90 92 NA"),
            };
            var qualifiedShipment = ShipmentHelper.GetQualifiedShipment(orders, 150);
            Assert.Equal(3, qualifiedShipment.Orders.Count());
            Assert.Contains<Order>(orders[0], qualifiedShipment.Orders);
            Assert.Contains<Order>(orders[1], qualifiedShipment.Orders);
            Assert.Contains<Order>(orders[4], qualifiedShipment.Orders);
        }
    }
}

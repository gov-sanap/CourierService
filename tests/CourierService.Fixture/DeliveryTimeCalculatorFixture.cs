using CourierService.Messages;
using CourierService.Models;
using CourierService.Translator;
using DeliveryCostEstimatorCLI;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace CourierService.Fixture
{
    public class DeliveryTimeCalculatorFixture
    {
        string _examplePackageId = "sampleId";
        DeliveryTimeCalculator _deliveryTimeCalculator;
        DeliveryTimeCalculatorRQ _deliveryTimeCalculatorRQ;
        public DeliveryTimeCalculatorFixture()
        {
            _deliveryTimeCalculator = new DeliveryTimeCalculator();
            var order = new Order
            {
                Package = new Package
                {
                    Id = _examplePackageId,
                    WeightInKG = 40
                },
                DistanceInKM = 10,
                OfferCode = "OFR001"
            };
            var orders = new List<Order> { order };
            _deliveryTimeCalculatorRQ = DeliveryTimeCalculatorTranslator.GetDeliveryTimeCalculatorRQ(orders, 2, 70, 200);
        }

        [Fact]
        public void Calculate_Should_Return_OrdersWithDeliveryTime_When_List_Of_Orders_Is_Passed()
        {
            _deliveryTimeCalculatorRQ.Orders = new List<Order>
            {
                OrderTranslator.GetOrder("PKG1 50 30 OFR001"),
                OrderTranslator.GetOrder("PKG2 75 125 OFFR0008"),
                OrderTranslator.GetOrder("PKG3 175 100 OFFR003"),
                OrderTranslator.GetOrder("PKG4 110 60 OFFR002"),
                OrderTranslator.GetOrder("PKG5 155 95 NA"),
            };
            List<double> expectedDeliveryTimes = new List<double> { 4.01, 1.79, 1.43, 0.86, 4.22 };
            var n = _deliveryTimeCalculatorRQ.Orders.Count;
            _deliveryTimeCalculatorRQ.MaxCarriableWeight = 200;
            _deliveryTimeCalculatorRQ.MaxSpeed = 70;
            _deliveryTimeCalculatorRQ.NumberOfVehicles = 2;

            var res = _deliveryTimeCalculator.Calculate(_deliveryTimeCalculatorRQ);

            for(int i=0;i< n; i++)
            {
                Assert.Equal(expectedDeliveryTimes[i], res.OrdersWithDeliveryTime[i].DeliveryTime);
            }
        }

        [Fact]
        public void Calculate_Should_Return_OrdersWithDeliveryTime_When_List_Of_Orders_Is_Passed_SecondTest()
        {
            _deliveryTimeCalculatorRQ.Orders = new List<Order>
            {
                OrderTranslator.GetOrder("PKG1 10 30 OFR001"),
                OrderTranslator.GetOrder("PKG2 40 125 OFFR0008"),
                OrderTranslator.GetOrder("PKG3 90 92 NA"),
                OrderTranslator.GetOrder("PKG4 65 120 OFFR002"),
                OrderTranslator.GetOrder("PKG5 60 95 NA"),
            };
            List<double> expectedDeliveryTimes = new List<double> { 0.43, 1.79, 1.31, 1.71, 1.36 };
            var n = _deliveryTimeCalculatorRQ.Orders.Count;
            _deliveryTimeCalculatorRQ.MaxCarriableWeight = 150;
            _deliveryTimeCalculatorRQ.MaxSpeed = 70;
            _deliveryTimeCalculatorRQ.NumberOfVehicles = 2;

            var res = _deliveryTimeCalculator.Calculate(_deliveryTimeCalculatorRQ);

            for (int i = 0; i < n; i++)
            {
                Assert.Equal(expectedDeliveryTimes[i], res.OrdersWithDeliveryTime[i].DeliveryTime);
            }
        }
    }
}

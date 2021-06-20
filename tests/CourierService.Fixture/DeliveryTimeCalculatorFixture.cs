using CourierService.Exceptions;
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
        [Fact]
        public void Calculate_Should_Throw_InvalidRequestException_If_Null_Orders_Are_Provided()
        {
            var deliveryTimeCalculatorRQ = DeliveryTimeCalculatorTranslator.GetDeliveryTimeCalculatorRQ(null, 2, 70, 200);
            Assert.Throws<InvalidRequestException>(() => DeliveryTimeCalculator.Calculate(deliveryTimeCalculatorRQ));
        }

        [Fact]
        public void Calculate_Should_Throw_InvalidRequestException_If_No_Orders_Are_Provided()
        {
            var orders = new List<Order>();
            var deliveryTimeCalculatorRQ = DeliveryTimeCalculatorTranslator.GetDeliveryTimeCalculatorRQ(orders, 2, 70, 200);
            Assert.Throws<InvalidRequestException>(() => DeliveryTimeCalculator.Calculate(deliveryTimeCalculatorRQ));
        }

        [Fact]
        public void Calculate_Should_Throw_InvalidRequestException_If_Order_With_Weight_GreaterThan_MaxCarriable_Weight_Provided()
        {
            var orders = new List<Order>
            {
                OrderTranslator.GetOrder("PKG1 250 30 OFR001")
            };
            var deliveryTimeCalculatorRQ = DeliveryTimeCalculatorTranslator.GetDeliveryTimeCalculatorRQ(orders, 2, 70, 200);
            Assert.Throws<InvalidRequestException>(() => DeliveryTimeCalculator.Calculate(deliveryTimeCalculatorRQ));
        }

        [Fact]
        public void Calculate_Should_Return_OrdersWithDeliveryTime_When_List_Of_Orders_Is_Passed_1()
        {
            var orders = new List<Order>
            {
                OrderTranslator.GetOrder("PKG1 50 30 OFR001"),
                OrderTranslator.GetOrder("PKG2 75 125 OFFR0008"),
                OrderTranslator.GetOrder("PKG3 175 100 OFFR003"),
                OrderTranslator.GetOrder("PKG4 110 60 OFFR002"),
                OrderTranslator.GetOrder("PKG5 155 95 NA"),
            };
            var deliveryTimeCalculatorRQ = DeliveryTimeCalculatorTranslator.GetDeliveryTimeCalculatorRQ(orders, 2, 70, 200);
            var res = DeliveryTimeCalculator.Calculate(deliveryTimeCalculatorRQ);

            List<double> expectedDeliveryTimes = new List<double> { 4.01, 1.79, 1.43, 0.86, 4.22 };
            var n = deliveryTimeCalculatorRQ.Orders.Count;
            for (int i=0;i< n; i++)
            {
                Assert.Equal(expectedDeliveryTimes[i], res.OrdersWithDeliveryTime[i].DeliveryTime);
            }
        }

        [Fact]
        public void Calculate_Should_Return_OrdersWithDeliveryTime_When_List_Of_Orders_Is_Passed_2()
        {
            var orders = new List<Order>
            {
                OrderTranslator.GetOrder("PKG1 10 30 OFR001"),
                OrderTranslator.GetOrder("PKG2 40 125 OFFR0008"),
                OrderTranslator.GetOrder("PKG3 90 92 NA"),
                OrderTranslator.GetOrder("PKG4 65 120 OFFR002"),
                OrderTranslator.GetOrder("PKG5 60 95 NA"),
            };
            var deliveryTimeCalculatorRQ = DeliveryTimeCalculatorTranslator.GetDeliveryTimeCalculatorRQ(orders, 2, 70, 150);
            var res = DeliveryTimeCalculator.Calculate(deliveryTimeCalculatorRQ);

            List<double> expectedDeliveryTimes = new List<double> { 0.43, 1.79, 1.31, 1.71, 1.36 };
            var n = deliveryTimeCalculatorRQ.Orders.Count;
            for (int i = 0; i < n; i++)
            {
                Assert.Equal(expectedDeliveryTimes[i], res.OrdersWithDeliveryTime[i].DeliveryTime);
            }
        }
    }
}

using CourierService.Messages;
using CourierService.Models;
using CourierService.Translator;
using DeliveryCostEstimatorCLI;
using System;
using System.Collections.Generic;
using Xunit;

namespace CourierService.Fixture
{
    public class DeliveryCostCalculatorFixture
    {
        string _examplePackageId = "sampleId";
        DeliveryCostCalculator _deliveryCostCalculator;

        public DeliveryCostCalculatorFixture()
        {
            Program.Initialize();
            _deliveryCostCalculator = Program.GetDeliveryCostCalculator();
        }

        [Fact]
        public void Calculate_Should_Return_Zero_When_Null_Request_Is_Passed()
        {
            var deliveryCostCalculatorRQ = DeliveryCostCalculatorTranslator.GetDeliveryCostCalculatorRQ(null, 100);
            var response = _deliveryCostCalculator.Calculate(deliveryCostCalculatorRQ);

            Assert.Equal(0, response.FinalDeliveryCost);
            Assert.Equal(0, response.DiscountAmmount);
        }

        [Theory]
        [InlineData("PKG1 5 5 OFR001",      0,    175)]
        [InlineData("PKG2 15 5 OFR002",     0,      275)]
        [InlineData("PKG3 10 100 OFR003",   35,    665)]
        [InlineData("PKG1 100 100 ",        0,     1600)]
        [InlineData("PKG1 100 100 OFR001",  160,     1440)]
        [InlineData("PKG2 100 100 OFR002",  112,     1488)]
        [InlineData("PKG3 100 100 OFR003",  80,     1520)]
        public void Calculate_Should_Return_TotalCost_When_offerCode_Is_Given_In_Order(string orderString, double expectedDiscountAmount, double expectedFinalCost)
        {
            var order = OrderTranslator.GetOrder(orderString);
            var deliveryCostCalculatorRQ = DeliveryCostCalculatorTranslator.GetDeliveryCostCalculatorRQ(order, 100);

            var response = _deliveryCostCalculator.Calculate(deliveryCostCalculatorRQ);
            
            Assert.Equal(expectedFinalCost, response.FinalDeliveryCost);
            Assert.Equal(expectedDiscountAmount, response.DiscountAmmount);
        }
    }
}

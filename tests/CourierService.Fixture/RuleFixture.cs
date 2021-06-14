using CourierService.Enums;
using CourierService.Models;
using CourierService.Translator;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace CourierService.Fixture
{
    public class RuleFixture
    {
        [Theory]
        [InlineData("DistanceInKM", OperatorType.Equal, "123", false)]
        [InlineData("DistanceInKM", OperatorType.LessThan, "123", true)]
        [InlineData("DistanceInKM", OperatorType.LessThanEqual, "100", true)]
        [InlineData("DistanceInKM", OperatorType.GreaterThanEqual, "100", true)]
        [InlineData("Package.WeightInKG", OperatorType.Equal, "123", false)]
        [InlineData("Package.WeightInKG", OperatorType.LessThan, "123", false)]
        [InlineData("Package.WeightInKG", OperatorType.LessThanEqual, "175", true)]
        [InlineData("Package.WeightInKG", OperatorType.GreaterThanEqual, "100", true)]
        public void Qualify_Should_Return_True_If_Rule_Is_True(string key, OperatorType operatorType, string value, bool result)
        {
            Rule rule = new Rule()
            {
                Key = key,
                Operator = operatorType,
                Value = value
            };
            var order = OrderTranslator.GetOrder("PKG3 175 100 OFFR003");

            Assert.Equal(result, rule.Qualify(order));
        }
    }
}

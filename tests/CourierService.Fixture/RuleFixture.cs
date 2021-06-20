using CourierService.Enums;
using CourierService.Exceptions;
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
        private Order _order;
        public RuleFixture()
        {
            _order = OrderTranslator.GetOrder("PKG3 175 100 OFFR003");
        }

        [Fact]
        public void Qualify_Should_Throw_InvalidKeyException_When_Rule_Has_Invalid_Key()
        {
            Rule rule = new Rule()
            {
                Key = "Length",
                Operator = OperatorType.LessThanEqual,
                Value = "100"
            };

            Assert.Throws<InvalidKeyException>(()=> rule.Qualify(_order));
        }


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

            Assert.Equal(result, rule.Qualify(_order));
        }
    }
}

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
    public class OfferFixture
    {
        private Order _order;
        private Offer _offer;
        public OfferFixture()
        {
            _order = OrderTranslator.GetOrder("PKG3 175 100 OFFR003");
            _offer = new Offer()
            {
                Code = "Offer001",
                Discount = 10,
                DiscountType = DiscountType.Percentage,
                Rules = new List<Rule>()
            };
        }

        [Fact]
        public void IsApplicable_Should_Return_False_If_Any_Rule_Fails_To_Qualify()
        {
            var rules = new List<Rule>
            {
                new Rule()
                {
                    Key = "DistanceInKM",
                    Operator = OperatorType.LessThanEqual,
                    Value = "100"
                },
                new Rule()
                {
                    Key = "Package.WeightInKG",
                    Operator = OperatorType.LessThan,
                    Value = "123"
                }
            };
            _offer.Rules = rules;
            Assert.False(_offer.IsApplicable(_order));
        }

        [Fact]
        public void IsApplicable_Should_Return_True_If_All_Rules_Qualifies()
        {
            var rules = new List<Rule>
            {
                new Rule()
                {
                    Key = "DistanceInKM",
                    Operator = OperatorType.LessThanEqual,
                    Value = "100"
                },
                new Rule()
                {
                    Key = "Package.WeightInKG",
                    Operator = OperatorType.LessThanEqual,
                    Value = "175"
                }
            };
            _offer.Rules = rules;
            Assert.True(_offer.IsApplicable(_order));
        }

        [Fact]
        public void GetDiscountAmmount_Should_Throw_NegativeDiscountException_When_Offer_Has_Negative_Discount()
        {
            double totalDeliveryCost = 120;
            _offer.Discount = -5;
            Assert.Throws<NegativeDiscountException>(() => _offer.GetDiscountAmmount(totalDeliveryCost));
        }

        [Theory]
        [InlineData(DiscountType.Percentage, 10, 150, 15)]
        [InlineData(DiscountType.Percentage, 20, 150, 30)]
        [InlineData(DiscountType.Fixed, 20, 150, 20)]
        [InlineData(DiscountType.Fixed, 120, 150, 120)]
        public void GetDiscountAmmount_Should_Give_DiscountAmount_When_TotalCost_Given(DiscountType discountType, double discount, double totalCost, double expectedDiscountAmount)
        {
            _offer.Discount = discount;
            _offer.DiscountType = discountType;
            var actualDiscountAmount = _offer.GetDiscountAmmount(totalCost);
            Assert.Equal(expectedDiscountAmount, actualDiscountAmount);
        }
    }
}

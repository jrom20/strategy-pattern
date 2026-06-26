using System.Collections.Generic;
using Moq;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Strategies;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests
{
    public class RebateServiceTests
    {
        private readonly Mock<IRebateDataStore> _rebateDataStore;
        private readonly Mock<IProductDataStore> _productDataStore;

        public RebateServiceTests()
        {
            _rebateDataStore = new Mock<IRebateDataStore>();
            _productDataStore = new Mock<IProductDataStore>();
        }


        [Fact]
        public void Calculate_FixedCashAmount_WhenSupportedAndAmountIsPositive_ReturnsSuccessAndStoresAmount()
        {
            var rebate = CreateRebate(IncentiveType.FixedCashAmount, amount: 55m);
            var product = CreateProduct(SupportedIncentiveType.FixedCashAmount);

            SetupDataStore(rebate, product);

            var service = CreateService();
            var request = CreateRequest(volume: 10m);

            var result = service.Calculate(request);

            Assert.True(result.Success);
            VerifyCalculationWasStored(rebate, 55m);
        }

        [Fact]
        public void Calculate_FixedCashAmount_WhenProductDoesNotSupportIt_ReturnsFailed()
        {
            var rebate = CreateRebate(IncentiveType.FixedCashAmount, amount: 55m);
            var product = CreateProduct(SupportedIncentiveType.AmountPerUom);

            SetupDataStore(rebate, product);

            var service = CreateService();
            var request = CreateRequest(volume: 10m);

            var result = service.Calculate(request);

            Assert.False(result.Success);
            VerifyCalculationWasNotStored();
        }

        [Fact]
        public void Calculate_FixedCashAmount_WhenAmountIsZero_ReturnsFailed()
        {
            var rebate = CreateRebate(IncentiveType.FixedCashAmount, amount: 0m);
            var product = CreateProduct(SupportedIncentiveType.FixedCashAmount);

            SetupDataStore(rebate, product);

            var service = CreateService();
            var request = CreateRequest(volume: 10m);

            var result = service.Calculate(request);

            Assert.False(result.Success);
            VerifyCalculationWasNotStored();
        }

        [Fact]
        public void Calculate_FixedRateRebate_WhenValid_ReturnsSuccessAndStoresCalculatedAmount()
        {
            var rebate = CreateRebate(IncentiveType.FixedRateRebate, percentage: 0.1m);
            var product = CreateProduct(SupportedIncentiveType.FixedRateRebate, price: 200m);

            SetupDataStore(rebate, product);

            var service = CreateService();
            var request = CreateRequest(volume: 5m);

            var result = service.Calculate(request);

            Assert.True(result.Success);
            VerifyCalculationWasStored(rebate, 100m);
        }

        [Fact]
        public void Calculate_FixedRateRebate_WhenProductDoesNotSupportIt_ReturnsFailed()
        {
            var rebate = CreateRebate(IncentiveType.FixedRateRebate, percentage: 0.1m);
            var product = CreateProduct(SupportedIncentiveType.FixedCashAmount, price: 200m);

            SetupDataStore(rebate, product);

            var service = CreateService();
            var request = CreateRequest(volume: 5m);

            var result = service.Calculate(request);

            Assert.False(result.Success);
            VerifyCalculationWasNotStored();
        }

        [Fact]
        public void Calculate_FixedRateRebate_WhenPercentageIsZero_ReturnsFailed()
        {
            var rebate = CreateRebate(IncentiveType.FixedRateRebate, percentage: 0m);
            var product = CreateProduct(SupportedIncentiveType.FixedRateRebate, price: 200m);

            SetupDataStore(rebate, product);

            var service = CreateService();
            var request = CreateRequest(volume: 5m);

            var result = service.Calculate(request);

            Assert.False(result.Success);
            VerifyCalculationWasNotStored();
        }

        [Fact]
        public void Calculate_FixedRateRebate_WhenProductPriceIsZero_ReturnsFailed()
        {
            var rebate = CreateRebate(IncentiveType.FixedRateRebate, percentage: 0.1m);
            var product = CreateProduct(SupportedIncentiveType.FixedRateRebate, price: 0m);

            SetupDataStore(rebate, product);

            var service = CreateService();
            var request = CreateRequest(volume: 5m);

            var result = service.Calculate(request);

            Assert.False(result.Success);
            VerifyCalculationWasNotStored();
        }

        [Fact]
        public void Calculate_FixedRateRebate_WhenVolumeIsZero_ReturnsFailed()
        {
            var rebate = CreateRebate(IncentiveType.FixedRateRebate, percentage: 0.1m);
            var product = CreateProduct(SupportedIncentiveType.FixedRateRebate, price: 200m);

            SetupDataStore(rebate, product);

            var service = CreateService();
            var request = CreateRequest(volume: 0m);

            var result = service.Calculate(request);

            Assert.False(result.Success);
            VerifyCalculationWasNotStored();
        }

        [Fact]
        public void Calculate_AmountPerUom_WhenValid_ReturnsSuccessAndStoresCalculatedAmount()
        {
            var rebate = CreateRebate(IncentiveType.AmountPerUom, amount: 12m);
            var product = CreateProduct(SupportedIncentiveType.AmountPerUom);

            SetupDataStore(rebate, product);

            var service = CreateService();
            var request = CreateRequest(volume: 4m);

            var result = service.Calculate(request);

            Assert.True(result.Success);
            VerifyCalculationWasStored(rebate, 48m);
        }

        [Fact]
        public void Calculate_AmountPerUom_WhenProductDoesNotSupportIt_ReturnsFailed()
        {
            var rebate = CreateRebate(IncentiveType.AmountPerUom, amount: 12m);
            var product = CreateProduct(SupportedIncentiveType.FixedRateRebate);

            SetupDataStore(rebate, product);

            var service = CreateService();
            var request = CreateRequest(volume: 4m);

            var result = service.Calculate(request);

            Assert.False(result.Success);
            VerifyCalculationWasNotStored();
        }

        [Fact]
        public void Calculate_AmountPerUom_WhenAmountIsZero_ReturnsFailed()
        {
            var rebate = CreateRebate(IncentiveType.AmountPerUom, amount: 0m);
            var product = CreateProduct(SupportedIncentiveType.AmountPerUom);

            SetupDataStore(rebate, product);

            var service = CreateService();
            var request = CreateRequest(volume: 4m);

            var result = service.Calculate(request);

            Assert.False(result.Success);
            VerifyCalculationWasNotStored();
        }

        [Fact]
        public void Calculate_AmountPerUom_WhenVolumeIsZero_ReturnsFailed()
        {
            var rebate = CreateRebate(IncentiveType.AmountPerUom, amount: 12m);
            var product = CreateProduct(SupportedIncentiveType.AmountPerUom);

            SetupDataStore(rebate, product);

            var service = CreateService();
            var request = CreateRequest(volume: 0m);

            var result = service.Calculate(request);

            Assert.False(result.Success);
            VerifyCalculationWasNotStored();
        }

        [Fact]
        public void Calculate_WhenNoStrategyIsRegisteredForTheIncentive_ReturnsFailed()
        {
            var rebate = CreateRebate(IncentiveType.FixedCashAmount, amount: 55m);
            var product = CreateProduct(SupportedIncentiveType.FixedCashAmount);

            SetupDataStore(rebate, product);

            var strategies = new IRebateCalculationStrategy[]
            {
                new AmountPerUomRebateStrategy()
            };

            var service = CreateService(strategies);
            var request = CreateRequest(volume: 10m);

            var result = service.Calculate(request);

            Assert.False(result.Success);
            VerifyCalculationWasNotStored();
        }


        private static IEnumerable<IRebateCalculationStrategy> CreateAllStrategies()
        {
            return new IRebateCalculationStrategy[]
            {
                new FixedCashAmountRebateStrategy(),
                new FixedRateRebateStrategy(),
                new AmountPerUomRebateStrategy()
            };
        }

        private RebateService CreateService(IEnumerable<IRebateCalculationStrategy> strategies = null)
        {
            var registeredStrategies = strategies ?? CreateAllStrategies();

            return new RebateService(
                _rebateDataStore.Object,
                _productDataStore.Object,
                registeredStrategies);
        }

        private void SetupDataStore(Rebate rebate, Product product)
        {
            _rebateDataStore
                .Setup(x => x.GetRebate(It.IsAny<string>()))
                .Returns(rebate);

            _productDataStore
                .Setup(x => x.GetProduct(It.IsAny<string>()))
                .Returns(product);
        }

        private void VerifyCalculationWasStored(Rebate rebate, decimal expectedAmount)
        {
            _rebateDataStore.Verify(
                x => x.StoreCalculationResult(rebate, expectedAmount),
                Times.Once());
        }

        private void VerifyCalculationWasNotStored()
        {
            _rebateDataStore.Verify(
                x => x.StoreCalculationResult(It.IsAny<Rebate>(), It.IsAny<decimal>()),
                Times.Never());
        }

        private static Rebate CreateRebate(IncentiveType incentive, decimal amount = 0m, decimal percentage = 0m)
        {
            return new Rebate
            {
                Identifier = incentive.ToString(),
                Amount = amount,
                Percentage = percentage
            };
        }

        private static Product CreateProduct(SupportedIncentiveType supportedIncentives, decimal price = 0m)
        {
            return new Product
            {
                Identifier = "product-1",
                Price = price,
                SupportedIncentives = supportedIncentives
            };
        }

        private static CalculateRebateRequest CreateRequest(decimal volume)
        {
            return new CalculateRebateRequest
            {
                RebateIdentifier = "rebate-1",
                ProductIdentifier = "product-1",
                Volume = volume
            };
        }
    }
}
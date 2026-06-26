using Microsoft.Extensions.DependencyInjection;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Strategies;

namespace Smartwyre.DeveloperTest.Runner
{
    public static class Registrations
    {
        public static IServiceCollection AddRebateCalculationStrategies(this IServiceCollection services)
        {
            services.AddSingleton<IRebateCalculationStrategy, AmountPerUomRebateStrategy>();
            services.AddSingleton<IRebateCalculationStrategy, FixedCashAmountRebateStrategy>();
            services.AddSingleton<IRebateCalculationStrategy, FixedRateRebateStrategy>();

            services.AddSingleton<IRebateService, RebateService>();
            services.AddSingleton<IProductDataStore, ProductDataStore>();
            services.AddSingleton<IRebateDataStore, RebateDataStore>();

            return services;
        }
    }
}

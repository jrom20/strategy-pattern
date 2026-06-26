using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Strategies;

public interface IRebateCalculationStrategy
{
    IncentiveType IncentiveType { get; }

    decimal? Calculate(Rebate rebate, Product product, CalculateRebateRequest request);
}

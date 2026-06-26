using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Strategies;

public class AmountPerUomRebateStrategy : IRebateCalculationStrategy
{
    public IncentiveType IncentiveType => IncentiveType.AmountPerUom;

    public decimal? Calculate(Rebate rebate, Product product, CalculateRebateRequest request)
    {
        if (rebate is null || product is null)
            return null;

        if (!product.SupportedIncentives.HasFlag(SupportedIncentiveType.AmountPerUom))
            return null;

        if (rebate.Amount <= 0 || request.Volume <= 0)
            return null;

        var amount = rebate.Amount * request.Volume;

        return amount;
    }
}
using Smartwyre.DeveloperTest.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Smartwyre.DeveloperTest.Strategies;

public class FixedCashAmountRebateStrategy : IRebateCalculationStrategy
{
    public IncentiveType IncentiveType => IncentiveType.FixedCashAmount;

    public decimal? Calculate(Rebate rebate, Product product, CalculateRebateRequest request)
    {
        if (rebate is null || product is null)
            return null;

        if (!product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedCashAmount))
            return null;

        if (rebate.Amount <= 0)
            return null;

        return rebate.Amount;
    }
}
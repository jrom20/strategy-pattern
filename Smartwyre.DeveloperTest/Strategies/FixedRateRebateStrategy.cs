using Smartwyre.DeveloperTest.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Smartwyre.DeveloperTest.Strategies;

public class FixedRateRebateStrategy : IRebateCalculationStrategy
{
    public IncentiveType IncentiveType => IncentiveType.FixedRateRebate;

    public decimal? Calculate(Rebate rebate, Product product, CalculateRebateRequest request)
    {
        if (rebate is null || product is null)
            return null;

        if (!product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedRateRebate))
            return null;

        if (rebate.Percentage <= 0 || product.Price <= 0 || request.Volume <= 0)
            return null;

        var amount = product.Price * rebate.Percentage * request.Volume;

        return amount;
    }
}
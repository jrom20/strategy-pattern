using System;

namespace Smartwyre.DeveloperTest.Types;

public class CalculateRebateResult
{
    public bool Success { get; set; }

    internal static CalculateRebateResult Failed()
    {
        return new CalculateRebateResult { Success = false };
    }

    internal static CalculateRebateResult Successful(decimal amount)
    {
        return new CalculateRebateResult { Success = true };
    }

}

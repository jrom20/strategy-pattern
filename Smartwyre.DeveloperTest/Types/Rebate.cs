using System;

namespace Smartwyre.DeveloperTest.Types;

public class Rebate
{
    public string Identifier { get; set; }

    // Only for demonstration purposes, we are using Enum.TryParse to convert the string Identifier to IncentiveType.
    // In a real-world scenario, you might want to have a more robust mapping or validation mechanism.
    public IncentiveType Incentive => Enum.TryParse<IncentiveType>(Identifier, out var incentiveType) ? incentiveType : throw new InvalidOperationException($"Invalid IncentiveType: {Identifier}");
    public decimal Amount { get; set; }
    public decimal Percentage { get; set; }

}

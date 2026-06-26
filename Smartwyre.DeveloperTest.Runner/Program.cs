using Microsoft.Extensions.DependencyInjection;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using System;
using System.Collections.Generic;
namespace Smartwyre.DeveloperTest.Runner;

class Program
{
    static void Main(string[] args)
    {
        var services = new ServiceCollection();
        services.AddRebateCalculationStrategies();

        using var provider = services.BuildServiceProvider();
        var rebateService = provider.GetRequiredService<IRebateService>();

        //Allowed rebateIds: AmountPerUom, FixedCashAmount, FixedRateRebate
        Console.Write("Enter rebate id (AmountPerUom, FixedCashAmount, FixedRateRebate): ");
        var rebateId = Console.ReadLine();

        Console.Write("Enter product id: ");
        var productId = Console.ReadLine();

        Console.Write("Enter volume: ");
        var volumeInput = Console.ReadLine();

        if (!decimal.TryParse(volumeInput, out decimal volume))
        {
            Console.WriteLine("Invalid volume. Please enter a valid number.");
            return;
        }

        var request = new CalculateRebateRequest
        {
            RebateIdentifier = rebateId,
            ProductIdentifier = productId,
            Volume = volume
        };

        var result = rebateService.Calculate(request);
        Console.WriteLine($"Success: {result.Success} (No database implementation / Product and Rebate data are hardcoded.)");
    }
}

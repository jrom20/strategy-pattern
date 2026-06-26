using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Strategies;
using Smartwyre.DeveloperTest.Types;
using System.Collections.Generic;
using System.Linq;

namespace Smartwyre.DeveloperTest.Services;

public class RebateService : IRebateService
{
    private readonly IRebateDataStore _rebateDataStore;
    private readonly IProductDataStore _productDataStore;
    private readonly IReadOnlyDictionary<IncentiveType, IRebateCalculationStrategy> _strategies;

    public RebateService(
           IRebateDataStore rebateDataStore,
           IProductDataStore productDataStore,
           IEnumerable<IRebateCalculationStrategy> strategies)
    {
        _rebateDataStore = rebateDataStore;
        _productDataStore = productDataStore;
        _strategies = strategies.ToDictionary(x => x.IncentiveType);
    }

    public CalculateRebateResult Calculate(CalculateRebateRequest request)
    {
        // reworked to avoid breaking open-close principle, by using strategy pattern to handle different incentive types

        // Rebate.cs class now class the rebate identifier ONLY for dev purposes
        var rebate = _rebateDataStore.GetRebate(request.RebateIdentifier);
        var product = _productDataStore.GetProduct(request.ProductIdentifier);

        if (!_strategies.TryGetValue(rebate.Incentive, out var strategy))
            return CalculateRebateResult.Failed();

        var result = strategy.Calculate(rebate, product, request);

        if (result.HasValue)
        {
            _rebateDataStore.StoreCalculationResult(rebate, result.Value);
        }

        return result.HasValue ? CalculateRebateResult.Successful(result.Value) : CalculateRebateResult.Failed();
    }
}

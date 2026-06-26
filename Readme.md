# Rebate Calculator Refactor

## Overview

This project calculates rebates for different incentive types.

The original implementation had all the calculation logic inside one method using a `switch` statement. This made the code harder to maintain because every new incentive type required changing the same method.

The code was refactored to follow the **Open/Closed Principle**.

This means the code is open for extension, but closed for modification.

## Main Changes

### 1. Removed the large switch statement

The previous code used a `switch` statement to decide how to calculate each rebate.

Now, each rebate calculation has its own strategy class.

Example strategies:

* `FixedCashAmountRebateStrategy`
* `FixedRateRebateStrategy`
* `AmountPerUomRebateStrategy`

This makes the code easier to read, test, and extend.

## 2. Added the Strategy Pattern

The Strategy Pattern was used to separate each calculation rule.

Each strategy implements the same interface:

```csharp
public interface IRebateCalculationStrategy
{
    IncentiveType IncentiveType { get; }

    CalculateRebateResult Calculate(
        Rebate rebate,
        Product product,
        CalculateRebateRequest request);
}
```

The main service does not need to know the details of each calculation.

It only selects the correct strategy and executes it.
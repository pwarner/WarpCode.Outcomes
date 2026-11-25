# 🧯 What is a Problem?

The Outcomes library defines a problem as:

```csharp
public record Problem(string Detail)
{
    /// <summary>
    /// Human-readable detail of the problem that occurred.
    /// </summary>
    public string Detail { get; init; } = Detail ?? throw new ArgumentNullException(nameof(Detail));
}
```

This contract's single `Detail` property mirrors the `Message` property of an exception.

Problems are record types, so they are immutable by design. They *implicitly* convert to an Outcome, and so do values, which keeps methods that return outcomes refreshingly free of ceremony:

```csharp
public static Outcome<int> RedeemStamps(Customer customer, int stamps)
{
    if (stamps > customer.LoyaltyStamps)
        return new Problem($"{customer.Name} only has {customer.LoyaltyStamps} stamps.");

    return customer.LoyaltyStamps - stamps;
}
```

## 🏷️ Typed problems

Typically, you will want to define your own hierarchy of strongly-typed problems that reflect real-life domain problems. Since `Problem` is a record, derive your problems as records too:

```csharp
public abstract record NotFoundProblem(string Detail) : Problem(Detail);

public sealed record NotFoundProblem<T>(string Key)
    : NotFoundProblem($"No {typeof(T).Name.ToLowerInvariant()} found for '{Key}'.");

public sealed record OutOfStockProblem(string DrinkName)
    : Problem($"Sorry, we're all out of {DrinkName}.");
```

Extra properties like `Key` and `DrinkName` carry the details you'll want later. When you [resolve an outcome](resolving-outcomes.md), the *type* of the problem is what decides the response, so a base type like `NotFoundProblem` lets you handle a whole family of problems in one place.

## 🛡️ What is not a Problem

Not every case where you throw an exception is a candidate for replacing with a problem.

An obvious example is guards, i.e. exceptions thrown when parameters are invalid. Here, throwing an exception is correct because your method was misused, and the guard protects your API from violation.

```csharp
public static Outcome<Customer> FindCustomerGuarded(string customerId)
{
    ArgumentException.ThrowIfNullOrWhiteSpace(customerId);

    return CoffeeShop.FindCustomer(customerId);
}
```

Passing in a blank id is a bug in the caller 🐛, so we throw. Passing in an id that doesn't match a customer is an everyday situation 🤷, so we return a problem.

📂 Sample code: [WhatIsAProblem.cs](../samples/Outcomes.Samples/WhatIsAProblem.cs), [CoffeeShop.cs](../samples/Outcomes.Samples/CoffeeShop.cs)<br>
▶️ `dotnet run --project samples/Outcomes.Samples -- what-is-a-problem`

---
### 📚 Index
- [Why Outcomes?](why-outcomes.md)
- this: What is a Problem?
- [Creating Outcomes](creating-outcomes.md)
- [Composing Outcomes](composing-outcomes.md)
- [Composing Async Outcomes](composing-async-outcomes.md)
- [Resolving Outcomes](resolving-outcomes.md)

### further reading / miscellaneous
- [Outcome Aggregation](outcome-aggregation.md)
- [Adapting to Outcomes](outcome-adaptation.md)
- [Outcomes as Monads](outcomes-as-monads.md)
- [Runnable samples](../samples/Outcomes.Samples/) ☕

# What is a problem?

The Results library defines a problem as:

```csharp
public record Problem(string Detail)
{
    /// <summary>
    /// Human-readable detail of the problem that occured.
    /// </summary>
    public string Detail { get; init; } = Detail ?? throw new ArgumentNullException(nameof(Detail));
}
```

This contract's single `Detail` property mirrors the `Message` property of an exception.

Problems are record types, so they are immutable by design. They *implicitly* convert to a Result, as this very silly example shows:

```csharp
Result<Tint> ProcessWithdrawal(Withdrawal withdrawl)
{
    if(withdrawal.Amount > _account.Balance)
        return new Problem("Insufficient funds in account to process transaction");
    
    int remaningBalance = _account.Debit(withdrawl.Amount);

    return remainingBalance;
}
```

Typically, you will want to define your own hierarchy of strongly-typed problems that reflect real-life domain problems. 

```csharp
public class EntityNotFoundProblem<T>: Problem
{
    public EntityNotFoundProblem(string id): 
        base($"A entity of type {typeof(T)} with an id of {id} was not found.") =>
        MissingId = id;
    
    public string MissingId {get;}
} 
```
Later, when you [resolve a result](resolving-results.md), the type of the problem will be useful for determining the resolution value. 

## What is not a Problem
Not every case where you throw an exception is a candidate for replacing with a problem. 

An obvious example are Guards, i.e. exceptions thrown when paramaters are invalid.

In each of these cases, throwing an exception is correct because your method was mis-used and they protect your API from violation.

```csharp
throw new AgrumentNullException(nameof(id));
throw new ArgumentException("Invalid name", nameof(name));
throw new ArgumentOutOfRange(nameof(date), "The date is not in range");
```

---
### Index
- [Why Results?](why-results.md)
- this: What is a Problem?
- [Creating Results](creating-results.md)
- [Composing Results](composing-results.md)
- [Resolving Results](resolving-results.md)

### further reading / miscellaneous
- [Result Aggregation](result-extensions.md)
- [Adapting to Results](result-adaptation.md)
- [Results as Monads](results-as-monads.md)

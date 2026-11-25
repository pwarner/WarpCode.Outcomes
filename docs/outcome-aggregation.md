# 🧺 Outcome Aggregation

Sometimes you have a whole sequence of outcomes, and you want a single outcome out of them: either *all* the values, or the problem(s) that got in the way.

There are two aggregation overloads, plus two async overloads that work on an `IAsyncEnumerable<Outcome<T>>`.

> [!TIP]
> 📂 Full sample: [OutcomeAggregation.cs](../samples/Outcomes.Samples/OutcomeAggregation.cs)<br>
> ▶️ `dotnet run --project samples/Outcomes.Samples -- outcome-aggregation`

## 📦 Aggregate on `IEnumerable<Outcome<T>>`

`Aggregate` returns an `Outcome<T[]>`.
- ✅ If every outcome in the sequence was a success, you get a success outcome holding an array of all the values.
- ❌ If one or more held a problem, and you pass `true` for `bailEarly`, you get the **first** problem found and the rest of the sequence isn't enumerated.
- ❌ If you pass `false` (the default), you get a `ProblemAggregate` holding **all** the problems found.

Ordering a round of drinks for the table: every drink must be available, or the whole round fails.

```csharp
public static Outcome<Drink[]> OrderRound(IEnumerable<string> drinkNames, bool bailEarly = false) =>
    drinkNames
        .Select(name => CoffeeShop.FindDrink(name).Ensure(CoffeeShop.CheckStock))
        .Aggregate(bailEarly);
```

Order `["Latte", "Mocha", "Babyccino"]` and you'll hear about the sold-out mocha *and* the babyccino we've never heard of. Pass `bailEarly: true` and you'll only hear about the mocha.

## 🫙 Aggregate on `IEnumerable<Outcome<None>>`

The same behaviour, except it returns an `Outcome<None>`, since none of the outcomes in the sequence hold a useful value. Perfect for a checklist:

```csharp
public static Outcome<None> OpeningChecks() =>
    new[]
    {
        Outcome.Ok,                                    // machine is hot
        Outcome.OfProblem("The milk fridge is warm"),
        Outcome.OfProblem("We're low on oat milk")
    }.Aggregate();
```

## 🔍 Inspecting a `ProblemAggregate`

`ProblemAggregate` is a `Problem` defined by the library. It exposes the problems it collected as a `Problem[] Problems` property. Its own `Detail` only says how many there were (e.g. "2 problems occurred…"), so look inside for the real story:

```csharp
private static string Describe(Problem problem) => problem switch
{
    ProblemAggregate aggregate => string.Join(" | ", aggregate.Problems.Select(p => p.Detail)),
    _ => problem.Detail
};
```

## ⚡ Aggregating async streams

`AggregateAsync` does the same for an `IAsyncEnumerable<Outcome<T>>` (or `IAsyncEnumerable<Outcome<None>>`), returning a `Task` of the aggregated outcome.

```csharp
public static Task<Outcome<Drink[]>> OrderRoundAsync(IEnumerable<string> drinkNames) =>
    IncomingOrders(drinkNames).AggregateAsync();
```

---
### 📚 Index
- [Why Outcomes?](why-outcomes.md)
- [What is a Problem?](what-is-a-problem.md)
- [Creating Outcomes](creating-outcomes.md)
- [Composing Outcomes](composing-outcomes.md)
- [Composing Async Outcomes](composing-async-outcomes.md)
- [Resolving Outcomes](resolving-outcomes.md)

### further reading / miscellaneous
- this: Outcome Aggregation
- [Adapting to Outcomes](outcome-adaptation.md)
- [Outcomes as Monads](outcomes-as-monads.md)
- [Runnable samples](../samples/Outcomes.Samples/) ☕

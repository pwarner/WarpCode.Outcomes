# 🔌 Adapting to Outcomes

You've started using Outcomes in your code, because you're a talented and discerning developer 😎.

But there's a lot of other code you have to work with that doesn't use Outcomes. It's not at all viable to change that code, and often not possible, because *it's not your code*.

Happily, you can still work with methods that don't return Outcomes:
- via [composition](composing-outcomes.md): the map overloads of `Then` (`Func<T, TNext>`) accept functions that return plain values.
- with `ToOutcome()`, which turns thrown exceptions into problems (this document).

> [!NOTE]
> Composition with functions that don't return outcomes does **not** catch exceptions.
> When you want exceptions from a function turned into problems, use the `ToOutcome()` extension method.

> [!TIP]
> 📂 Full sample: [OutcomeAdaptation.cs](../samples/Outcomes.Samples/OutcomeAdaptation.cs)<br>
> ▶️ `dotnet run --project samples/Outcomes.Samples -- outcome-adaptation`

Our coffee shop uses a third-party card-terminal SDK 🧓 that we can't change. Its documentation tells us that it throws:
- an `ArgumentException` if the `cardNumber` is null, empty or whitespace.
- a `CardDeclinedException` if the card is declined.

```csharp
public static string Charge(string cardNumber, decimal amount)
public static async Task<string> ChargeAsync(string cardNumber, decimal amount, CancellationToken cancellationToken)
```

To adapt this SDK to give us Problems instead of Exceptions, we need to do two things:

1. Use the `ToOutcome()` extension method.
2. Define an `ExceptionMap`, either globally or as a parameter.

## 🗺️ Exception mapping

An `ExceptionMap` is just a delegate with the following signature:

```csharp
/// <summary>
/// A function called when exceptions are thrown by code instead of returning outcomes.
/// If the function returns a <see cref="Problem"/> then a new <see cref="Outcome{T}"/>
/// will be returned by the adaptive methods. If null is returned, the exception will be re-thrown.
/// </summary>
public delegate Problem? ExceptionMap(Exception exception);
```

### Exception map as a parameter

We can pass a map just for the `CardDeclinedException` case. Returning `null` for anything else lets those exceptions keep throwing:

```csharp
public static Outcome<string> Charge(string cardNumber, decimal amount)
{
    Func<string> charge = () => LegacyCardTerminal.Charge(cardNumber, amount);

    return charge.ToOutcome(exception => exception switch
    {
        CardDeclinedException declined => new PaymentDeclinedProblem(declined.Amount),
        _ => null
    });
}
```

As it's very common to need to map just a single exception type, there's a convenient shortcut that uses the generic `ExceptionMap<TException>` delegate. It catches and maps only exceptions of type `TException`, and lets everything else throw.

Name the exception type on the lambda's parameter, and C# infers everything else for you:

```csharp
public static Task<Outcome<string>> ChargeAsync(string cardNumber, decimal amount, CancellationToken cancellationToken) =>
    LegacyCardTerminal.ChargeAsync(cardNumber, amount, cancellationToken)
        .ToOutcome((CardDeclinedException declined) => new PaymentDeclinedProblem(declined.Amount));
```

> [!IMPORTANT]
> **Don't pass the exception type as a lone type argument.** On `Func<T>`, `Task<T>`, `ValueTask<T>` and `Task`, a call like `.ToOutcome<CardDeclinedException>(map)` **won't compile**; the compiler reports `CS1929: '...' does not contain a definition for 'ToOutcome'`. When you write explicit type arguments, C# fills the *receiver's* type parameter first (the `T` of `Task<T>`), so your one type argument would be read as the value type, not the exception type.
>
> That's deliberate. In version 1 the same call on a `Task<T>` compiled, quietly bound to the value-less `Task` overload instead (a `Task<T>` *is* a `Task`), and threw away the task's result. Version 2 turns that silent bug into a compile error.
>
> If you do need explicit type arguments (C# can't infer `TException` from a *method group*, only from a typed lambda), give both, receiver type first:
> - `.ToOutcome<string, CardDeclinedException>(MapDeclined)` on a `Task<string>`, `ValueTask<string>` or `Func<string>`
> - `.ToOutcome<Task, CardDeclinedException>(MapDeclined)` on a plain `Task`
>
> `Action` and plain `ValueTask` have no value type, so on those a single type argument, `.ToOutcome<CardDeclinedException>(MapDeclined)`, is fine.

### 🌍 Global, application-level exception mapping

Instead of passing an exception map to every call of `ToOutcome()`, it's far more convenient to write one function responsible for mapping every exception that logically represents a problem.

To do that, set the static `Adapt.MapExceptions` property during your application's startup:

```csharp
public static void ConfigureExceptionMapping() =>
    Adapt.MapExceptions = exception => exception switch
    {
        CardDeclinedException declined => new PaymentDeclinedProblem(declined.Amount),
        // other exception types that represent expected situations go here
        _ => null // everything else keeps on throwing
    };
```

After that, a plain `ToOutcome()` with no arguments uses the global map.

> [!CAUTION]
> It's tempting to catch every `Exception` and return some catch-all wrapper problem. This is a bad idea 🙈. Mapping all exceptions would swallow guard exceptions like `ArgumentNullException` and `ArgumentException`, which are thrown when an API is misused.
> Outcomes replace throwing exceptions to represent violations of *business* state, but they are most definitely **not** intended to replace throwing exceptions *when exceptional conditions occur*.

## 🧰 What can be adapted?

The `ToOutcome()` extension method is available for:

|target type|adapts to|
|--|--|
| `Func<T>` | `Outcome<T>` |
| `Action` | `Outcome<None>` |
| `Task<T>` | `Task<Outcome<T>>` |
| `Task` | `Task<Outcome<None>>` |
| `ValueTask<T>` | `ValueTask<Outcome<T>>` |
| `ValueTask` | `ValueTask<Outcome<None>>` |

<!-- TODO: Adapt for async should return AsyncOutcome<T> -->

---
### 📚 Index
- [Why Outcomes?](why-outcomes.md)
- [What is a Problem?](what-is-a-problem.md)
- [Creating Outcomes](creating-outcomes.md)
- [Composing Outcomes](composing-outcomes.md)
- [Composing Async Outcomes](composing-async-outcomes.md)
- [Resolving Outcomes](resolving-outcomes.md)

### further reading / miscellaneous
- [Outcome Aggregation](outcome-aggregation.md)
- this: Adapting to Outcomes
- [Outcomes as Monads](outcomes-as-monads.md)
- [Runnable samples](../samples/Outcomes.Samples/) ☕

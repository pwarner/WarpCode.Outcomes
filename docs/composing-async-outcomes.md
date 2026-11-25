# ⚡ Composing Async Outcomes

Real workflows rarely stay synchronous for long. Sooner or later you're waiting on a database, an HTTP API or (in our coffee shop) a card terminal that takes its sweet time.

Everything you learned in [Composing Outcomes](composing-outcomes.md) carries straight over to async code. The only new thing to meet is `AsyncOutcome<T>`.

> [!TIP]
> All the code on this page comes from a runnable sample, 📂 [ComposingAsyncOutcomes.cs](../samples/Outcomes.Samples/ComposingAsyncOutcomes.cs).
>
> ▶️ `dotnet run --project samples/Outcomes.Samples -- composing-async-outcomes`

## 🤔 What is an `AsyncOutcome<T>`?

An `AsyncOutcome<T>` is an outcome that hasn't finished arriving yet. It's a lightweight `readonly struct` that holds two things:
- a `ValueTask<Outcome<T>>`: the eventual outcome.
- a `CancellationToken`: captured **once**, when you go async, and handed to every async step after that.

It plays nicely with the rest of .NET async:
- `await` it, and you get the `Outcome<T>` inside.
- It converts implicitly to both `ValueTask<Outcome<T>>` and `Task<Outcome<T>>`, so you can return it from methods with either signature.

## 🚀 Going async

There are two ways onto the async rails.

|from|use|gives you|
|---|---|---|
| an existing `Outcome<T>` | `outcome.AsAsync(cancellationToken)` | `AsyncOutcome<T>` |
| a value | `AsyncOutcome.Of(value, cancellationToken)` | `AsyncOutcome<T>` |
| a problem | `AsyncOutcome.OfProblem<T>(problem, cancellationToken)` | `AsyncOutcome<T>` |
| nothing at all | `AsyncOutcome.Ok(cancellationToken)` | `AsyncOutcome<None>` |

```csharp
// From an outcome you already have...
Outcome<OrderRequest> validated = Outcome.Of(request).Ensure(CoffeeShop.Validate);
AsyncOutcome<OrderRequest> fromOutcome = validated.AsAsync(cancellationToken);

// ...or straight from a value, a problem, or nothing at all.
AsyncOutcome<OrderRequest> fromValue = AsyncOutcome.Of(request, cancellationToken);
AsyncOutcome<Drink> fromProblem = AsyncOutcome.OfProblem<Drink>(new OutOfStockProblem("Mocha"), cancellationToken);
AsyncOutcome<None> fromNothing = AsyncOutcome.Ok(cancellationToken);
```

The `CancellationToken` parameter is optional everywhere, but you'll almost always want to pass one in.

## 🔗 Composing

### ✅ Synchronous steps still work

All five operators (`Then`, `Ensure`, `OnValue`, `Rescue` and `OnProblem`) are available on `AsyncOutcome<T>` with **exactly the same signatures** as on `Outcome<T>`, including the value-less overloads and tuple parameter spreading from [Composing Outcomes](composing-outcomes.md).

So a step that doesn't need to `await` anything doesn't have to pretend it does.

### ⏳ Asynchronous steps

Each operator that takes a delegate also comes in two async flavours: one for delegates returning a `ValueTask`, and one for delegates returning a `Task`.

|operator|`ValueTask` delegate|`Task` delegate|
|---|---|---|
| `Then` | `ThenAsync` | `ThenTask` |
| `Ensure` | `EnsureAsync` | `EnsureTask` |
| `OnValue` | `OnValueAsync` | `OnValueTask` |
| `Rescue` | `RescueAsync` | `RescueTask` |
| `OnProblem` | `OnProblemAsync` | `OnProblemTask` |

The async delegates take the same parameters as their synchronous twins, **plus a `CancellationToken` as the last parameter**, and return a `ValueTask`/`Task` of what the synchronous version returns. For example, on an `AsyncOutcome<T>`:

|operation|signature|
|---|---|
| `ThenAsync<TNext>` | `Func<T, CancellationToken, ValueTask<Outcome<TNext>>>` |
| `ThenAsync<TNext>` | `Func<T, CancellationToken, ValueTask<TNext>>` |
| `ThenTask<TNext>` | `Func<T, CancellationToken, Task<Outcome<TNext>>>` |
| `ThenTask<TNext>` | `Func<T, CancellationToken, Task<TNext>>` |
| `OnValueAsync` | `Func<T, CancellationToken, ValueTask>` |
| `RescueTask` | `Func<Problem, CancellationToken, Task<Outcome<T>>>` |

> [!TIP]
> **Which flavour do I pick?** Reach for `…Async` when you write an inline `async` lambda, or when calling a method that returns a `ValueTask`. Reach for `…Task` when you're passing in an existing method that returns a `Task`, which is what most libraries hand you.

<details>
<summary>Why not just one overloaded <code>ThenAsync</code>? 🧐</summary>

An `async` lambda can be converted to a delegate returning *either* `Task<T>` or `ValueTask<T>`, and the C# compiler considers neither a better match. If both were overloads of a single method name, every inline `async` lambda would fail to compile with an ambiguous-call error (`CS0121`). That's the most common way to write an async step, so the two families get different names instead.

</details>

Here's a whole order going through the async rails:

```csharp
public static AsyncOutcome<Receipt> PlaceOrderAsync(OrderRequest request, CancellationToken cancellationToken) =>
    Outcome.Of(request)
        .Ensure(CoffeeShop.Validate)                 // plain sync composition...
        .Then(r => r.CustomerId)
        .AsAsync(cancellationToken)                  // ...until we go async: the token is captured once, here
        .ThenTask(CoffeeShop.FindCustomerAsync)      // an existing Task-returning method ➜ ThenTask
        .ThenAsync(async (customer, ct) =>           // an inline async lambda ➜ ThenAsync (ValueTask)
        {
            Outcome<Drink> drink = await CoffeeShop.FindDrinkAsync(request.DrinkName, ct);
            return drink.Then(d => (customer, d));
        })
        .EnsureAsync((_, drink, ct) => CoffeeShop.CheckStockAsync(drink, ct))      // tuples spread, token comes last
        .Then((customer, drink) => CoffeeShop.PriceOrder(customer, drink, request.Size)) // sync delegates still work
        .ThenTask(CoffeeShop.ChargeCardAsync);
```

A few things worth noticing:
- We stay synchronous for as long as we can, and only call `AsAsync` once the first `await` is needed.
- `CoffeeShop.FindCustomerAsync(string, CancellationToken)` and `CoffeeShop.ChargeCardAsync(Order, CancellationToken)` are passed as plain method groups. Their signatures already fit, token and all.
- The lambda in `ThenAsync` returns an `Outcome<(Customer, Drink)>`, and the steps after it receive `customer` and `drink` as separate parameters.
- If any step returns a problem, every following happy-path step is skipped, just like in synchronous code.

### 🛟 Async side effects and the sad path

The sad-path and side-effect operators work just the same. Here, if the drink is out of stock, we rescue the order by placing it again with a filter coffee, then write to an audit log:

```csharp
public static AsyncOutcome<Receipt> PlaceOrderOrSubstituteAsync(OrderRequest request, CancellationToken cancellationToken) =>
    PlaceOrderAsync(request, cancellationToken)
        .RescueAsync((problem, ct) => problem is OutOfStockProblem
            ? PlaceOrderAsync(request with { DrinkName = "Filter Coffee" }, ct) // an AsyncOutcome is a ValueTask<Outcome<T>>
            : AsyncOutcome.OfProblem<Receipt>(problem, ct))
        .OnValueTask(AuditLog.WriteAsync)            // Task-returning side effect
        .OnProblemAsync(async (problem, ct) =>       // ValueTask-returning side effect
        {
            await Task.Delay(5, ct);
            AuditLog.Write($"Order failed: {problem.Detail}");
        });
```

Because an `AsyncOutcome<Receipt>` converts implicitly to a `ValueTask<Outcome<Receipt>>`, a whole async pipeline can be returned straight from a `RescueAsync` (or `ThenAsync`) delegate.

## 🛑 Cancellation

You never pass the token to each step by hand. The one you supplied when going async is handed to every async delegate as its last parameter, so it reaches all the way down to your database calls and HTTP clients.

When that token is cancelled, the `OperationCanceledException` is **not** turned into a problem. Cancellation is an exceptional event, not an expected business outcome, so it propagates as normal:

```csharp
try
{
    await PlaceOrderAsync(requests[0], cancelled.Token);
}
catch (OperationCanceledException)
{
    Console.WriteLine("   🛑 Cancelled: cancellation is exceptional, so it throws rather than becoming a problem");
}
```

## 🏁 Resolving and returning

`await` an `AsyncOutcome<T>` to get the `Outcome<T>` inside, or [resolve](resolving-outcomes.md) it in one go with `Match`, which returns a `ValueTask<TFinal>`:

```csharp
public static async Task<string> DescribeOrderAsync(OrderRequest request, CancellationToken cancellationToken)
{
    // await an AsyncOutcome to get the Outcome inside...
    Outcome<Receipt> outcome = await PlaceOrderAsync(request, cancellationToken);
    Console.WriteLine($"   awaited ➜ {outcome}");

    // ...or resolve it directly: Match returns a ValueTask<TFinal>
    return await PlaceOrderAsync(request, cancellationToken).Match(
        receipt => $"✅ {receipt.Description} for {receipt.CustomerName}: ${receipt.Charged:F2}",
        problem => $"❌ {problem.Detail}");
}
```

Already have an interface or endpoint that returns `Task<Outcome<T>>` or `ValueTask<Outcome<T>>`? Just return the `AsyncOutcome<T>`:

```csharp
// An AsyncOutcome<T> converts implicitly to both ValueTask<Outcome<T>> and Task<Outcome<T>>.
public static Task<Outcome<Receipt>> HandleAsync(OrderRequest request, CancellationToken cancellationToken) =>
    PlaceOrderAsync(request, cancellationToken);
```

> [!IMPORTANT]
> An `AsyncOutcome<T>` wraps a `ValueTask`, so the same rules apply: **await it (or `Match` it) exactly once**, and don't hold on to it to await again later. Pipelines are also *hot*, like most `Task`-returning methods: steps start running as soon as you build the pipeline, not when you first `await` it.

## 📋 Summary
- Go async with `Outcome<T>.AsAsync`, `AsyncOutcome.Of`, `AsyncOutcome.OfProblem` or `AsyncOutcome.Ok`, passing in your `CancellationToken` once.
- Synchronous steps keep their familiar signatures on `AsyncOutcome<T>`.
- Async steps come in two flavours: `…Async` takes `ValueTask` delegates (use it for inline `async` lambdas), and `…Task` takes `Task` delegates (use it for existing `Task`-returning methods).
- Async delegates receive the `CancellationToken` as their last parameter. Cancellation throws rather than becoming a problem.
- `await` an `AsyncOutcome<T>` or `Match` it, but only once. It converts implicitly to `ValueTask<Outcome<T>>` and `Task<Outcome<T>>`.

---
### 📚 Index
- [Why Outcomes?](why-outcomes.md)
- [What is a Problem?](what-is-a-problem.md)
- [Creating Outcomes](creating-outcomes.md)
- [Composing Outcomes](composing-outcomes.md)
- this: Composing Async Outcomes
- [Resolving Outcomes](resolving-outcomes.md)

### further reading / miscellaneous
- [Outcome Aggregation](outcome-aggregation.md)
- [Adapting to Outcomes](outcome-adaptation.md)
- [Outcomes as Monads](outcomes-as-monads.md)
- [Runnable samples](../samples/Outcomes.Samples/) ☕

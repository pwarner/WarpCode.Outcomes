# 🏁 Resolving Outcomes

At the end of a pipeline of operations returning Outcomes, you'll need to leave the rails 🚉 and return a final value that makes sense for your application. In a Web API, for example, that might be an `IActionResult` or `IResult`.

To keep the samples free of dependencies, we resolve to a tiny stand-in for ASP.NET Core's `IResult`:

```csharp
public sealed record HttpResult(int StatusCode, string Body)
```

> [!TIP]
> 📂 Full sample: [ResolvingOutcomes.cs](../samples/Outcomes.Samples/ResolvingOutcomes.cs)<br>
> ▶️ `dotnet run --project samples/Outcomes.Samples -- resolving-outcomes`

## 🔀 Match

The `Match` method resolves your outcome to a final return value. It takes two functions: one to resolve the ✅ success state, and one to resolve the ❌ problem state.

```csharp
public static HttpResult GetTheUsual(string customerId) =>
    ComposingOutcomes.TheUsualInStock(customerId).Match(
        drink => HttpResult.Ok(drink.Name),
        problem => problem switch
        {
            NotFoundProblem notFound => HttpResult.NotFound(notFound.Detail),
            OutOfStockProblem outOfStock => HttpResult.Conflict(outOfStock.Detail),
            _ => HttpResult.Error(problem.Detail)
        });
```

A switch expression over the problem's *type* maps each of your [typed problems](what-is-a-problem.md) to the right response. Notice how matching on the base `NotFoundProblem` handles both unknown customers and unknown drinks.

## ♻️ A shared resolver

Most likely, you'll want a single function that maps all your application's problems to responses, used everywhere you resolve an outcome. Write it as extension methods, and resolution becomes a one-liner:

```csharp
public static class OutcomeHttpExtensions
{
    public static HttpResult ToHttpResult<T>(this Outcome<T> outcome) =>
        outcome.Match(HttpResult.Ok, ToHttpResult);

    public static ValueTask<HttpResult> ToHttpResultAsync<T>(this AsyncOutcome<T> outcome) =>
        outcome.Match(HttpResult.Ok, ToHttpResult);

    public static HttpResult ToHttpResult(this Problem problem) => problem switch
    {
        ValidationProblem invalid => HttpResult.BadRequest(invalid.Detail),
        PaymentDeclinedProblem declined => HttpResult.PaymentRequired(declined.Detail),
        NotFoundProblem notFound => HttpResult.NotFound(notFound.Detail),
        OutOfStockProblem outOfStock => HttpResult.Conflict(outOfStock.Detail),
        _ => HttpResult.Error(problem.Detail)
    };
}
```

```csharp
public static HttpResult GetTheUsualResolved(string customerId) =>
    ComposingOutcomes.TheUsualInStock(customerId).ToHttpResult();
```

## ⚡ Resolving async outcomes

`Match` on an [`AsyncOutcome<T>`](composing-async-outcomes.md) returns a `ValueTask<TFinal>`, so you can resolve an async pipeline without `await`ing it first:

```csharp
public static ValueTask<HttpResult> PostOrderAsync(OrderRequest request, CancellationToken cancellationToken) =>
    ComposingAsyncOutcomes.PlaceOrderAsync(request, cancellationToken)
        .Match(HttpResult.Ok, OutcomeHttpExtensions.ToHttpResult);
```

## 📋 Summary
- `Match` takes one function for the value and one for the problem, and returns whatever they return.
- Switch over the problem's type to choose a response, and put that switch in one shared resolver.
- On an `AsyncOutcome<T>`, `Match` returns a `ValueTask<TFinal>`.

---
### 📚 Index
- [Why Outcomes?](why-outcomes.md)
- [What is a Problem?](what-is-a-problem.md)
- [Creating Outcomes](creating-outcomes.md)
- [Composing Outcomes](composing-outcomes.md)
- [Composing Async Outcomes](composing-async-outcomes.md)
- this: Resolving Outcomes

### further reading / miscellaneous
- [Outcome Aggregation](outcome-aggregation.md)
- [Adapting to Outcomes](outcome-adaptation.md)
- [Outcomes as Monads](outcomes-as-monads.md)
- [Runnable samples](../samples/Outcomes.Samples/) ☕

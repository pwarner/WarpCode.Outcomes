# Resolving Outcomes

At the end of a pipeline of operations returning Outomes, you'll need to leave the 'rails' and return a final value that makes sense for your application, for example in a Web API that resolves to an `IActionResult` or `IResult`.

The Match method resolves your outcomes to a final return value. 
It takes two delegates - one to resolve the `Success Outcome` state,
and one to resolve the `Problem Outcome` state.

```csharp
ValueTask<Outcome<TOrderDetailDto>> Pipeline(OrderDetailRequest request) =>
    Validate(request)
    .ThenAsync(FetchOrderDetailsAsync)
    .Then(MapToDto);

public async ValueTask<IResult> GetOrderDetail(OrderDetailRequest request)
{
    Outcome<TOrderDetailDto> outcome = await Pipeline(request);

    return outcome.Match(
        value=> Outcomes.Of(value),
        problem => problem switch
        {
            NotFoundProblem notFound => Results.NotFound(),
            ValidationProblem invalid => Results.ValidationProblem(invalid.Message)
            _ => Outcomes.Error(problem.Detail)
        }
    );
}
```

There is a `MatchAsync()` extension that allows you to resolve an asynchronous Outcome without `await`ing it first.

Using this, the GetOrderDetail method above can be simplified a little:

```csharp

public ValueTask<IResult> GetOrderDetail(OrderDetailRequest request) =>
    Pipeline(request)
        .MatchAsync(
            value=> Outcomes.Of(value),
            problem => problem switch
            {
                NotFoundProblem notFound => Results.NotFound(),
                ValidationProblem invalid => Results.ValidationProblem(invalid.Message)
                _ => Outcomes.Error(problem.Detail)
            }
        );
```

In the examples above, a switch expression is used to handle multiple problems defined in our application.

Most likely, you'll want to create a single function that resolves multiple problem cases to appropriate response types,
that can be used anywhere you resolve an outcome. If you write this as an extension method, outcome resolution looks much simpler.

```csharp

public static class OutcomeResolverExtensions
{
    public static IResult ToApiResult<T>(this Outcome<T> outcome, Func<T, IResult> valueResolver) =>
        outcome.Match(valueResolver, ProblemResolver);

    public static IResult ToApiResult<T>(this Outcome<T> outcome) =>
        outcome.Match(DefaultValueResolver<T>, ProblemResolver);

    private static IResult DefaultValueResolver<T>(T value) => 
        Results.Ok(value);

    private static IResult ProblemResolver(IProblem problem) =>
        problem switch 
        {
            NotFoundProblem notFound => Results.NotFound(),
            ValidationProblem invalid => Results.ValidationProblem(invalid.Message)
            _ => Results.Error(problem.Detail)
        };
}
```

---
### Index
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
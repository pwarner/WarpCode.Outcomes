# Resolving Outcomes

At the end of a pipeline of operations returning Outomes, you'll need to leave the 'rails' and return a final value that makes sense for your application, for example in a Web API that resolves to an `IActionResult` or `IResult`.

The Match method resolves your outcomes to a final return value. It takes two delegates - one for the success outcome that holds a value,
and one for the failure outcome when it holds a problem.

```csharp
Task<Outcome<OrderDetailDto>> Pipeline(OrderDetailRequest request) =>
    from isValid in Validate(request)
    from orderDetail in FetchOrderDetailsAsync(request.OrderId)
    select MapToDto(orderDetail);

public async Task<IResult> GetOrderDetail(OrderDetailRequest request)
{
    Outcome<OrderDetailDto> outcome = await Pipline(request);

    return outcome.Match(
        value=> Results.Of(value),
        problem => problem switch
        {
            NotFoundProblem notFound => Results.NotFound(),
            ValidationProblem invalid => Results.ValidationProblem(invalid.Message)
            _ => Results.Error(problem.Detail)
        }
    );
}
```

There is a `MatchAsync()` extension that allows you to resolve an asynchronous Outcome without `await`ing it first.

Using this, the GetOrderDetail method above can be simplified a little:

```csharp

public Task<IResult> GetOrderDetail(OrderDetailRequest request) =>
    Pipeline(request)
        .MatchAsync(
            value=> Results.Of(value),
            problem => problem switch
            {
                NotFoundProblem notFound => Results.NotFound(),
                ValidationProblem invalid => Results.ValidationProblem(invalid.Message)
                _ => Results.Error(problem.Detail)
            }
        );
```

In the examples above, a switch expression is used to handle multiple problems defined in our application.

Most likely, you'll want to create a single function that resolves multiple problem cases to appropriate response types,
that can be used anywhere you resolve an outcome. If you write this as an extension method, outcome resolution looks much simpler.

```csharp

public static class OutcomeResolverExtensions
{
    public static IResult ToResult<T>(this Outcome<T> outcome, Func<T, IResult> valueResolver) =>
        outcome.Match(valueResolver, ProblemResolver);

    public static IResult ToResult<T>(this Outcome<T> outcome) =>
        outcome.Match(DefaultValueResolver<T>, ProblemResolver);

    private static IResult DefaultValueResolver<T>(T value) => 
        Results.Of(value);

    private static IResult ProblemResolver(IProblem problem) =>
        problem switch 
        {
            NotFoundProblem notFound => Results.NotFound(),
            ValidationProblem invalid => Results.ValidationProblem(invalid.Message)
            _ => Results.Error(problem.Detail)
        };
}
```

### Index
- [Why Outcomes?](../readme.md)
- [What is a Problem?](what-is-a-problem.md)
- [Creating Outcomes](creating-outcomes.md)
- [Composing Outcomes](composing-outcomes.md)
- this: Resolving Outcomes

### further reading / miscellaneous
- [Outcome Extensions](outcome-extensions.md)
- [Adapting to Outcomes](outcome-adaptation.md)
- [Outcomes as Monads](outcomes-as-monads.md)
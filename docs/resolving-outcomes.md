# Resolving Outcomes

At the end of a pipeline of operations returning Outomes, you'll need to leave the 'rails' and return a final value that makes sense for your application, for example in a Web API that resolves to an `IActionResult` or `IResult`.

A switch expression with pattern matching is perfect for mapping your outcomes to a final return value.

```csharp
Task<Outcome<OrderDetailDto>> Pipeline(OrderDetailRequest request) =>
    from isValid in Validate(request)
    from orderDetail in FetchOrderDetailsAsync(request.OrderId)
    select MapToDto(orderDetail);

public async Task<IResult> GetOrderDetail(OrderDetailRequest request)
{
    Outcome<OrderDetailDto> outcome = await Pipline(request);

    return outcome.Problem switch
    {
        null => Results.Ok(outcome.Value),
        NotFoundProblem notFound => Results.NotFound(),
        ValidationProblem invalid => Results.ValidationProblem(invalid.Message)
        {} other => Results.Error(other.Detail)
    };
}
```

### Index
- [Why Outcomes?](../readme.md)
- [What is a Problem?](what-is-a-problem.md)
- [Creating Outcomes](creating-outcomes.md)
- [Composing Outcomes](composing-outcomes.md)
- [Adapting to Outcomes](outcome-adaptation.md)
- this: Resolving Outcomes

### further reading / miscellaneous
- [Outcomes as Monads](./docs/outcomes-as-monads.md)
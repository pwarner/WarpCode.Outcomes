# Resolving Outcomes

At the end of a pipeline of operations returning Outomes, you'll need to leave the 'rails' and return a final value that makes sense for your application, for example in a Web API that resolves to an `IActionResult` or `IResult`.

When you've stuffed a value or a problem into an` Outcome<T>`, you might be surprised to find you can't directly access them because the `Outcome<T>` type does not expose either a Value or Problem directly. 

Nor does it expose any boolean indicators such as `IsSuccess` or `HasProblem`.

You use the `Match` method to obtain a final non-outcome based value.

 The default `Match` method takes two resolver functions, one for each state of `value` or `problem`.

```csharp
Task<Outcome<OrderDetailDto>> Pipeline(OrderDetailRequest request) =>
    from isValid in Validate(request)
    from orderDetail in FetchOrderDetailsAsync(request.OrderId)
    select MapToDto(orderDetail);

public async Task<IResult> GetOrderDetail(OrderDetailRequest request)
{
    Outcome<OrderDetailDto> outcome = await Pipline(request);

    return outcome.Match
    (
        dto => Results.Ok(dto),
        problem => problem switch
        {
            NotFoundProblem notFound => Results.NotFound(),
            ValidationProblem invalid => Results.ValidationProblem(...)
        }
    );    
}
```

### Index
- [Why Outcomes?](../readme.md)
- [What is a Problem?](what-is-a-problem.md)
- [Creating Outcomes](creating-outcomes.md)
- [Composing Outcomes](composing-outcomes.md)
- [Adapting to Outcomes](outcome-adaptation.md)
- this: Resolving Outcomes
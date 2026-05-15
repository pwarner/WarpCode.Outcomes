# Composing Results

You can compose (or chain) expressions that return Results (and Tasks of Results and ValueTasks of Results)
together with one of two approaches:
1. With Then and ThenAsync
2. With the LINQ comprehension known as the natural query style.

## Compose an `Result<T>` with `Then` and a map function.

The `map` function parameter is a `Func<T,TNext>` delegate that's executed if the result **does not** carry a problem.

```csharp
Result<Tstring> FormatDate(Result<TDateTime> input) =>
    input.Then(dt => dt.ToString("yyyy/MM/dd"));
```

A map function can also be used with a `Task<Result<T>>` or `ValueTask<Result<T>>` by using `ThenAsync`.

```csharp
Task<Result<Tstring>> FormatDateAsyc(Task<Result<TDateTime>> input) =>
    input.ThenAsync(dt => dt.ToString("yyyy/MM/dd"));
```

## Transform an `Result<T>` with `from` and `select`

We can achieve the same thing using a LINQ comprehension.

```csharp
Result<Tstring> FormatDate(Result<TDateTime> input) =>
    from dt in input
    select dt.ToString("yyyy/MM/dd");
```

As before, this works in asynchronous scenarios, with both `Task<Result<T>>` and `ValueTask<Result<T>>`:

```csharp
ValueTask<Result<Tstring>> FormatDate(ValueTask<Result<TDateTime>> input) =>
    from dt in input
    select dt.ToString("yyyy/MM/dd");
```

> Notice how there was no need to `await` anything. 

A line in the form `from x in y` means:
> `x` represents the value contained in an Outcome returned by the expression `y`.

## Important
If any Outcome in a composition step holds an `IProblem`, *none* of the subsequent Outcome expressions (the `from` or `select` clauses) are evaluated. 
The flow is short-circuited, and a new problem Outcome of the desired return type is immediately returned.

In the example above, if the `input` Outcome holds an instance of `IProblem` instead of a `DateTime` value, the `select` clause is never evaluated.

## Further composition with factory functions

The `Then` composition accepts a factory delegate that is passed the current value to create an result.

```csharp
    // Composing with a map function
    public static Result<TTNext> Then<T, TNext>(
        this Result<T> map,
        Func<T, TNext> map) { ... }

    // Composing with a factory function
    public static Result<TTNext> Then<T, TNext>(
        this Result<T> self,
        Func<T, Result<TTNext>> factory) { ... }
```

Composition with factory functions is the *glue* that allows you to compose multiple logical steps that each return an Outcome.

Here's an entirely contrived example. I know you were hoping for another to-do list, but I'm afraid you're
going to have to settle for the traditional *Fetch Order from the Database* routine.

```csharp
Task<Result<TGetOrderResult>> FetchOrderAsync(GetOrderRequest request, CancellationToken ct) =>
    _validator.Validate(request)
    .ThenAsync(_ =>_FetchOrderAsync(request, ct)) // factory
    .ThenAsync(EnsureOrderExists) // factory
    .ThenAsync(order => new GetOrderResult(order)); // map
```

Notice the return type is `Task<Result<TGetOrderRequest>>` because we compose with an async expression. (`ValueTask<Result<TGetOrderRequest>>` is also supported.)

- `validator.Validate()` validates the incoming request and returns an `Result<TNone>`. 
Remember, that's an Outcome that doesn't carry a value we're interested in, but it could carry a Problem.

If the request was invalid, this will hold a problem representing all of the validation errors.

The rest of the composition will not be evaluated and will immediately return an result holding this problem.

- Next, an Entity Framework DBContext is invoked asychronously to return an `Task<Order?>`. 

The implementation of `FetchOrderAsync` looks like this:
```csharp
private async Task<Result<TOrder?>> FetchOrderAsnc(GetOrderRequest request, CancellationToken ct) =>
    await _dbContext.Orders.FindAsync(request.OrderId, cancellationToken: ct);
```

Although `FindAsync` returns a `Task<Order?>`, thanks to implicit coversion, the awaited `Order?` value becomes an `Result<TOrder?>`.

- Next, if there no order was fetched with this ID, the Outcome returned by `EnsureOrderExists` will hold some kind of `EntityNotFoundProblem`. 
The composition will halt immediately, and this problem is used to create the return value.

The implementation of `EnsureOrderExists` looks like this:

```csharp
private static Result<TOrder> EnsureOrderExists(Order? maybeOrder) =>
    maybeOrder switch
    {
        null => new EntityNotFoundProblem<Order>(),
        not null => maybeOrder
    };
```

- Finally, and only when an order was successfully fetched, the last `ThenAsync` clause is evaluated with a map function, and a DTO is created to hold relevant properties of the Order. 
This becomes the value held by the final Outcome.

What does this look like as a LINQ comprehension?

## LINQ-Style composition with multiple `from` clauses

```csharp
Task<Result<TGetOrderResult>> FetchOrderAsync(GetOrderRequest request) =>
    from _ in _validator.Validate(request)
    from maybeOrder in FetchOrderAsync(request)
    from order in EnsureOrderExists(maybeOrder)
    select new GetOrderResult(order);
```

> It isn't obvious from this example, but LINQ style composition offers an advantage over the Then/ThenAsync direct style.
The value of the Outcome in each step is **in scope** to all subsequent clauses. 
`GetOrderResult` here could take `maybeOrder` as a parameter because it is still in scope, even though it was not on the immediately preceding line.

## Special case: Composing Then/ThenAsync with `Result<TNone>`

```csharp
var result = Outcome.Of(1).Then(x=> Outcome.Ok);
```
What is the generic type of the result stored in `result`? 

In the code above, the factory function used in `Then` returns an `Result<TNone>`.

The method signature looks like this:

```csharp
public static Result<TTNext> Then<T, TNext>(
        this Result<T> self,
        Func<T, Result<TTNext>> factory)
```
Without intervention, the generic type `TNext` will be of type `None`, so result will be of type `Result<TNone>`.

That's going to be annoying when composing logic because we've lost the integer type and value that we composed on.

What we really want when we compose with `Result<TNone>` is to pick up any problem it holds, but as it is value-less, 
we'd like to hold on to the value we start with.

Luckily for us, the library treats `Then/ThenAsync` with a delegate that returns `None` or `Result<TNone>` as a special case thanks to this overload:
```csharp
public static Result<T> Then<T>(
        this Result<T> self,
        Func<T, Result<TNone>> factory)
```

Revealing the answer to the fiendish puzzle above:

```csharp
Result<Tint> result = Outcome.Of(1).Then(x=> Outcome.Ok);
```
The return type is an `Result<Tint>` which will hold 1 because the result returned by the factory delegate doesn't hold a problem.

> This special case only applies to `Then/ThenAsync` method of compisition.
> With `from x in y` LINQ style composition, all values of results in a composition chain are in scope so we never see the case of 'losing' a previous composition value.


## Outcome-compatible expressions
With `from value in expression` or the `Then`/`ThenAsync` syntax, you can compose expressions that evaluate to any of the following:

- `Result<T>`
- `Task<Result<T>>`
- `ValueTask<Result<T>>`

Additionally, the following types are also available thanks to Outcome adaptation.

- `Task<T>`(adapts to `Task<Result<T>>`)
- `ValueTask<T>` (adapts to `ValueTask<Result<T>>`)
- `Task`(adapts to `Task<Result<TNone>>`)
- `ValueTask` (adapts to `ValueTask<Result<TNone>>`)

---
### Index
- [Why Results?](why-results.md)
- [What is a Problem?](what-is-a-problem.md)
- [Creating Results](creating-results.md)
- this: Composing Results
- [Resolving Results](resolving-results.md)

### further reading / miscellaneous
- [Result Aggregation](result-extensions.md)
- [Adapting to Results](result-adaptation.md)
- [Results as Monads](results-as-monads.md)
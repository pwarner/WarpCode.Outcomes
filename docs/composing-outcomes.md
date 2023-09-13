# Composing Outcomes

You can compose (or chain) expressions that return Outcomes (and Tasks of Outcomes and ValueTasks of Outcomes)
together with one of two approaches:
1. With Map/MapAsync and Bind/BindAsync
2. With the LINQ comprehension known as the natural query style.

In these examples, we want to transform the `System.DateTime` value potentially held by the input outcome, to a formatted string.

## Transform an `Outcome<T>` with `Map`

```csharp
Outcome<string> FormatDate(Outcome<DateTime> input) =>
    input.Map(dt => dt.ToString("yyyy/MM/dd"));
```

The `Map` function parameter is a delegate that will be executed if the Outcome **does not** carry a problem.

`MapAsync` is the async equivalent.

```csharp
Task<Outcome<string>> FormatDateAsyc(Task<Outcome<DateTime>> input) =>
    input.MapAsync(dt => dt.ToString("yyyy/MM/dd"));
```

We can achieve the same thing using a LINQ comprehension, as we'll see next.

## Transform an `Outcome<T>` with `from` and `select`

```csharp
Outcome<string> FormatDate(Outcome<DateTime> input) =>
    from dt in input
    select dt.ToString("yyyy/MM/dd");
```

As before, this works if in asynchronous scenarios, with both `Task<Outcome<T>>` and `ValueTask<Outcome<T>>`:

```csharp
Task<Outcome<string>> FormatDate(Task<Outcome<DateTime>> input) =>
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

## Composition with Bind/BindAsync

`Bind` is similar to `Map` but where `Map` has a delegate parameter that maps an Outcome value to another non-Outcome value, 
`Bind` has a delegate parameter that maps an Outcome value to another Outcome.

```csharp
    public static Outcome<TNext> Map<T, TNext>(
        this Outcome<T> self,
        Func<T, TNext> selector) { ... }

    public static Outcome<TNext> Bind<T, TNext>(
        this Outcome<T> self,
        Func<T, Outcome<TNext>> selector) { ... }
```

`Bind` is the *glue* that allows you to compose multiple logical steps that each return an Outcome.

Here's an entirely contrived example. I know you were hoping for another to-do list, but I'm afraid you're
going to have to settle for the tried and trusted *Fetch Order from the Database* routine.

```csharp
Task<Outcome<GetOrderResult>> FetchOrderAsync(GetOrderRequest request) =>
    _validator.Validate(request)
    .BindAsync(_ => _dbContext.Orders
                    .FindAsync(request.OrderId).ToOutcome())
    .BindAsync(EnsureOrderExists)
    .MapAsync(order=> new GetOrderResult(order));
```

Pretty straight forward stuff.

Notice the return type is `Task<Outcome<GetOrderRequest>>` because we compose an async expression. (`ValueTask<Outcome<GetOrderRequest>>` is also supported.)

- `validator.Validate()` validates the incoming request and returns an `Outcome<None>`. 
Remember, that's an Outcome that doesn't carry a value we're interested in, but it could carry a Problem.

If the request was invalid, this will hold a problem representing all of the validation errors. 
The rest of the composition will not be evaluated and will immediately return an outcome holding this problem.

- Next, an Entity Framework DBContext is invoked asychronously to return an `Task<Order?>`. 
(Because the previous step yielded a `None` value, we discard the input parameter with `_`)

This is obviously not a Task that resolves to an Outcome, but we can easily adapt it using the `ToOutcome()` 
extension availble (see [Adapting to Outcomes](outcome-adaptation.md) )

- If there was no order with this ID, the Outcome returned by `EnsureOrderExists` will hold some kind of `EntityNotFoundProblem`. 
The composition will halt immediately, and this problem is used to create the return value.

The implementation of `EnsureOrderExists` might look like this:

```csharp
private static Outcome<Order> EnsureOrderExists(Order? maybeOrder) =>
    maybeOrder switch
    {
        null => new EntityNotFoundProblem<Order>(),
        not null => maybeOrder
    };
```

- If an order was successfully fetched, the final `MapAsync` clause is evaluated, and a DTO is created to hold relevant properties of the Order. 
This becomes the value held by the final Outcome.

What does this look like as a LINQ comprehension?

## LINQ-Style composition with multiple `from` clauses

```csharp
Task<Outcome<GetOrderResult>> FetchOrderAsync(GetOrderRequest request) =>
    from _ in _validator.Validate(request)
    from maybeOrder in _dbContext.Orders
            .FindAsync(request.OrderId).ToOutcome()
    from order in EnsureOrderExists(maybeOrder)
    select new GetOrderResult(order);
```

> It isn't obvious from this example, but LINQ style composition offers an advantage over the Map & Bind direct style.
The value of the Outcome in each step is **in scope** to all subsequent clauses. 
`GetOrderResult` here could take `maybeOrder` as a parameter because it is still in scope, even though it was not on the immediately preceding line.


## Outcome-compatible expressions
With `from value in expression` or the `Bind`/`BindAsync` syntax, you can compose with expressions that evaluate to any of the following:

- `Outcome<T>`
- `Task<Outcome<T>>`
- `ValueTask<Outcome<T>>`

Additionally, the following types are also available thanks to Outcome adaptation.

- `Task<T>`(adapts to `Task<Outcome<T>>`)
- `ValueTask<T>` (adapts to `ValueTask<Outcome<T>>`)
- `Task`(adapts to `Task<Outcome<None>>`)
- `ValueTask` (adapts to `ValueTask<Outcome<None>>`)

### Index
- [Why Outcomes?](../readme.md)
- [What is a Problem?](what-is-a-problem.md)
- [Creating Outcomes](creating-outcomes.md)
- this: Composing Outcomes
- [Resolving Outcomes](resolving-outcomes.md)

### further reading / miscellaneous
- [Outcome Extensions](outcome-extensions.md)
- [Adapting to Outcomes](outcome-adaptation.md)
- [Outcomes as Monads](outcomes-as-monads.md)
# Composing Outcomes

You can compose (or chain) expressions that return Outcomes (and Tasks of Outcomes and ValueTasks of Outcomes)
together with the LINQ comprehension known as the natutal query style.

In these examples, we want to transform the `System.DateTime` value potentially held by the input outcome, to a formatted string.

## Transform an `Outcome<T>` with `from` and `select`

```csharp
Outcome<string> FormatDate(Outcome<DateTime> input) =>
    from dt in input
    select dt.ToString("yyyy/MM/dd");
```

A line in the form `from x in y` means:
> `x` represents the value contained in an Outcome returned by the expression `y`.

## Important
If any Outcome in a composition (the `y` in `from x in y`) holds an `IProblem`, *none* of the subsequent Outcome expressions (the `from` or `select` clauses) are evaluated. 
The flow is short-circuited, and a new problem Outcome of the desired return type is immediately returned.

In the example above, if the `input` Outcome holds an instance of `IProblem` instead of a `DateTime` value, the `select` clause is never evaluated.

## Composition with multiple `from` clauses

```csharp
Task<Outcome<GetProductResult>> GetProduct(GetProductRequest request) =>
    from _ in _validator.Validate(request)
    from maybeProduct in _dbContext.Products
            .FindAsync(request.ProductId).ToOutcome()
    from product in EnsureProductExists(maybeProduct)
    select new GetProductResult(product);
```
Let's unpick what's going on here:

First, notice the return type is `Task<Outcome<GetProductRequest>>` because we compose an async expression. (`ValueTask<Outcome<GetProductRequest>>` is also supported.)

- `validator.Validate()` is a method that validates the incoming request and returns an `Outcome<None>`. 

If the request was invalid, this will hold a problem representing all of the validation errors. 

The rest of the composition will not be evaluated and will immediately return an outcome holding this problem.

- An Entity Framework DBContext is invoked asychronously to return an `Task<Product?>`.

This is obviously not a Task that resolves to an Outcome, but we can easily adapt it using the `ToOutcome()` 
extension availble (see [Adapting to Outcomes](outcome-adaptation.md) )

- If there was no product with this ID, the Outcome returned by `EnsureProductExists` will hold some kind of `EntityNotFoundProblem`. 
The composition will halt immediately, and this problem is used to create the return value.

- If a product was successfully fetched, the final `select` clause is evaluated, and a DTO is created to hold relevant properties of the product. 
This becomes the value held by the final Outcome.

## Outcome-compatible expressions
With `from value in expression` syntax, you can compose with expressions that evaluate to any of the following:

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
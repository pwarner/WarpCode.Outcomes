# Composing Outcomes

You can compose (or chain) operations on Outcomes together either directly using the `Then` method for simple scenarios, else via a LINQ comprehension style.

In these examples, we want to transform the `System.DateTime` value potentially held by the input outcome, to a formatted string.
## Transform an `Outcome<T>` with `Then`

```csharp
Outcome<string> FormatDate(Outcome<DateTime> input) =>
    input.Then<string>(dt=> dt.ToString("yyyy/MM/dd"));
```
The parameter for `Then` is a function that takes the value of type T1 in an `Outcome<T1>` and returns a new `Outcome<T2>`. 

It wll only be invoked if the Outcome on which it is called does not hold a problem value.
But if the Outome holds a problem value, a `new Outcome<T2>(problem)` will be returned.

The code above specifies the generic type `string` in the signature so it can benefit from implict conversion to return an `Outcome<string>`. It's functionally equivalent to this:

```csharp
Outcome<string> FormatDate(Outcome<DateTime> input) =>
    input.Then(dt=> Outcome.Ok(dt.ToString("yyyy/MM/dd")));
```

Composing with `Then` has some limitations.
- The scope is limited to the value of the previous Outcome and any variables in scope (unless you want to create complicated Tuples)
- You can't use `Then` with asynchronouse outcomes.

For more flexibility, use the LINQ comprehension style explained next.

## Transform an `Outcome<T>` with `from` and `select`

```csharp
Outcome<string> FormatDate(Outcome<DateTime> input) =>
    from dt in input
    select dt.ToString("yyyy/MM/dd");
```

A line in the form `from x in y` means:
> `x` is the value contained in an Outcome - or something that can be converted to an Outcome - returned by the expression `y`.

## Important
If any Outcome in a composition (the `y` in `from x in y`) holds an `IProblem`, *none* of the subsequent Outcome expressions (the `from` or `select` clauses) are evaluated. The flow is short-circuited, and a new problem Outcome of the desired return type is immediately returned.

In the example above, if the `input` Outcome holds an instance of `IProblem` instead of a `DateTime` value, the `select` clause is never evaluated.

## Composition with multiple `from` clauses

```csharp
Task<Outcome<GetProductResult>> GetProduct(GetProductRequest request) =>
    from validRequest in _validator.Validate(request)
    from product in _repository.GetByIdAsync(validRequest.ProductId)
    select new GetProductResult(product);
```
Let's unpick what's going on here:

- First, notice the return type is `Task<Outcome<GetProductRequest>>` because we compose an async expression. (`ValueTask<Outcome<GetProductRequest>>` is also supported.)

- `validator.Validate()` is a method that validates the incoming request and returns an `Outcome<GetProductRequest>`. If the request was invalid, this will hold a problem representing all of the validation errors. The rest of the composition will not be evaluated and will immediately return an outcome holding this problem.

- next, the repository is invoked asychronously to return an `ValueTask<Outcome<Product>>`.

- The variable `validRequest` from the first line is in scope here, and used to supply the productId parameter for invocation. With each successive line in composition, all previous variables (and any input parameters of course) are in scope.

- If there was no product with this ID, the Outcome returned by `GetByIdAsync` will hold some kind of `EntityNotFoundProblem`. The composition will halt immediately, and this problem is used to create the return value.

- If a product was fetched, the final `select` clause is evaluated, and a DTO is created to hold relevant properties of the product. This becomes the value held by the final Outcome.

## Outcome-compatble expressions
With `from value in expression` syntax, you can compose with expressions that evaluate to any of the following:

- `Outcome<T>`
- `Task<Outcome<T>>`
- `ValueTask<Outcome<T>>`

Additionally, the following types are also available thanks to [Outcome Adaptation](outcome-adaptation.md).

- `Task<T>`(adapts to `Task<Outcome<T>>`)
- `ValueTask<T>` (adapts to `ValueTask<Outcome<T>>`)
- `Task`(adapts to `Task<Outcome<None>>`)
- `ValueTask` (adapts to `ValueTask<Outcome<None>>`)

### Index
- [Why Outcomes?](../readme.md)
- [What is a Problem?](what-is-a-problem.md)
- [Creating Outcomes](creating-outcomes.md)
- this: Composing Outcomes
- [Adapting to Outcomes](outcome-adaptation.md)
- [Resolving Outcomes](resolving-outcomes.md)
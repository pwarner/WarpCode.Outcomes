# Composing Outcomes

When you've stuffed a value or a problem into an` Outcome<T>`, you might be surprised to find you can't directly access them because the `Outcome<T>` type does not expose either a Value or Problem directly. 

Nor does it expose any boolean indicators such as `IsSuccess` or `HasProblem`.

Once you're using Outcomes, it's like being on a direct train hurtling down a rail-track to its destination. The train is either on the 'success/value' track, or the 'problem' track. And just like when you're on a moving train and the doors are locked, there's no way of jumping off until the train safely arrives in the station.

This is a deliberate design decision to adhere to a `Monad` design pattern from the world of functional programming. A Monad usually appears in the form of a sort of wrapper that augments an inner value with additional logic, but it's also a type of contract.

The contract requires that a Monad exposes two functions that take function/delegates as parameters and create new Monads from the existing one.

## Map
A map function takes as a parameter a function that transforms the Monad's inner value to another value.

```csharp
class Monad<T1>
{
    Monad<T2> Map<T1, T1>(Func<T1,T2> selector)
}
```
## Bind/Flat-map
The bind function takes as a parameter a function that takes the Monad's inner value and returns a new Monad. 
```csharp
class Monad<T1>
{
    Monad<T2> Bind<T1, T1>(Func<T1, Monad<T2>> selector);
}
```
If you provided this parameter to the Map function, you'd get a Monad whose inner type was also a Monad (here, a `Monad<Monad<T2>>`). The bind function knows how to 'flatten' this *Monad-within-a-Monad* and produce a single `Monad<T2>`, hence why it is sometimes known as a Flat-Map.

The monadic contract doesn't require that these functions are strictly named `Map` and `Bind`, just that functions are present that represent these behaviours.

## The `IEnumerable<T>` Monad
Did you know that `IEnumerable<T>` was a monad?
- It presents the `Map` operation as the `Select` function.
- It presents the `Bind/Flat-map` operation as the `SelectMany` function.

It's not a particularly useful observation on it's own. But consider that the C# compiler has long recognised these method signatures to offer the **synactic sugar** of the LINQ comprehension also known as the **LINQ natural query form** 

```csharp
// natural query equivalent of 
// products.Select(p => new{ p.Id,p.Name});
var qry = from p in producs
          select new{ p.Id, p.Name};

// natural query equivalent of 
// products.SelectMany(p=> p.Categories, (p,c) => new{ ProductName: p.Name, Category: c.Name});
var qry = from p in producs
          from c in p.Categories
          select new{ ProductName: p.Name, Category: c.Name};
```
The smeantic clarity and readability of this language form is valuable and powerful. So `Outcome<T>` too fulfils it's monadic contract using `Select` and `Selectmany` extensions, allowing you to compose flow logic in a LINQ comprehension style.

## Map an `Outcome<T1>` to an `Outcome<T2>`
In this sample, we want to transform the `System.DateTime` value potentially held by the input outcome, to a formatted string.

```csharp
Outcome<string> FormatDate(Outcome<DateTime> input) =>
    from dt in input
    select dt.ToString("yyyy/MM/dd");
```

A line in the form `from x in y` means:
> `x` is the value contained in the Outcome (or something that can be converted to an Outcome) returned by the expression `y`.

A single `from` clause before the final `select` means this a *map* operation, the equivalent of `input.Select(dt=> dt.ToString("yyyy/MM/dd"));`

## Important
If any Outcome in a composition (the `y` in `from x in y`) holds an `IProblem`, *none* of the following Outcome expressions (`from` or `select`) are evaluated, and a new Outcome of the desired return type is returned that holds that problem.

In the example above, if the `input` Outcome holds an instance of `IProblem` instead of a `DateTime` value, the `select` clause is never evaluated.

## Bind an `Outcome<T1>` to an `Outcome<T2>`

In this example, we'll see more than one 'from` clause, meaning it's a *bind/flat-map* operation.

```csharp
AsyncOutcome<GetProductResult> GetProduct(GetProductRequest request) =>
    from validRequest in _validator.Validate(request)
    from product in _repository.GetByIdAsync(validRequest.ProductId)
    select new GetProductResult(product);
```
Let's unpick what's going on here:
- First, notice the return type is `AsyncOutcome<GetProductRequest>`. The return type of compositions that involve async operations changes to `AsyncOutcome<>` from `Outcome<>`, which is simply a wrapper for a `Task`/`ValueTask` of an `Outcome<>`.
- `validator.Validate()` is a method that validates the incoming request and returns an `Outcome<GetProductRequest>`. If the request was invalid, this will hold a problem representing all of the validation errors. The rest of the composition will not be evaluated and will immediately return an outcome holding this problem.
- next, the repository is invoked asychronously to return an `ValueTask<Outcome<Product>>`. 
- The variable `validRequest` from the first line is in scope here, and used to supply the productId parameter for invocation. With each successive line in composition, all previous variables (and any input parameters of course) are in scope.
- If there was no product with this ID, the Outcome returned by `GetByIdAsync` will hold some kind of `EntityNotFoundProblem`. Again, the composition immediately halts, and this problem is used to create the return value.  
- If a product was fetched, the final `select` clause is evaluated, and a DTO is created to hold relevant properties of the product. This becomes the value held by the Outcome that completes the `ValueTask<Outcome<GetProductResult>>` hiding inside the `AsyncOutcome`.
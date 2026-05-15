# Monadic design of Results 

Results are a `discriminated union` type, and also adhere to a `Monad` design pattern from the world of functional programming. 

A Monad usually appears in the form of a sort of wrapper that augments an inner value with additional logic, but it's also a type of contract.

The contract requires that a Monad exposes two functions that take function/delegates as parameters and create new Monads from the existing one.

## Map
A map function takes as a parameter a function that transforms the Monad's inner value to another value.

```csharp
class Monad<T>
{
    Monad<TNext> Map<TNext>(Func<T,TNext> selector)
}
```
## Bind/Flat-map
The bind function takes as a parameter a function that takes the Monad's inner value and returns a new Monad. 
```csharp
class Monad<T>
{
    Monad<TNext> Bind<TNext>(Func<T, Monad<TNext>> selector);
}
```

If you provided this parameter to the Map function, you'd get a Monad whose inner type was also a Monad (here, a `Monad<Monad<TNext>>`). 
The bind function knows how to 'flatten' this *Monad-within-a-Monad* and produce a single `Monad<TNext>`, hence why it is sometimes known as a Flat-Map.

The monadic contract doesn't require that these functions are strictly named `Map` and `Bind`, just that functions are present that represent these behaviours.

## The `IEnumerable<T>` Monad
Did you know that `IEnumerable<T>` was a monad?
- It presents the `Map` operation as the `Select` function.
- It presents the `Bind/Flat-map` operation as the `SelectMany` function.

## Results as Monads

Since bind and map are compositional operations, `Result<T>` fulfils its monadic contract via the `|` operator as it does for every compisitional operator in the library.

They are `happy path` operators, meaning that they only execute when the result does not hold a problem. 
Which makes sense, as there is no meaningful value when a problem is present.

Bind:
```csharp
Result<int> myResult = Result.Of(5) | (x => Result.Of(x * 2));
```
Map:
```csharp
static int DoubleIt(int x) => x * 2;
Result<int> myResult = Result.Of(5) | DoubleIt;
```

---
### Index
- [Why Results?](why-results.md)
- [What is a Problem?](what-is-a-problem.md)
- [Creating Results](creating-results.md)
- [Composing Results](composing-results.md)
- [Resolving Results](resolving-results.md)

### further reading / miscellaneous
- [Result Aggregation](result-extensions.md)
- [Adapting to Results](result-adaptation.md)
- this: Results as Monads
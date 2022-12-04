# Monadic design of Outcomes 

Once you're using Outcomes, it's like being on a direct train hurtling down a rail-track to its destination. The train is either on the 'success/value' track, or the 'problem' track. And just like when you're on a moving train and the doors are locked, there's no way of jumping off until the train safely arrives in the station.

This is a deliberate design decision to adhere to a `Monad` design pattern from the world of functional programming. 

A Monad usually appears in the form of a sort of wrapper that augments an inner value with additional logic, but it's also a type of contract.

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

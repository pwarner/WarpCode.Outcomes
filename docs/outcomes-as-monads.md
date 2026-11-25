# 🧙 Monadic design of Outcomes

Outcomes are a `discriminated union` type, and also follow the `Monad` design pattern from the world of functional programming.

Don't let the word scare you off 👻. A Monad usually appears as a sort of wrapper that augments an inner value with some extra logic, but it's also a kind of contract.

The contract requires that a Monad exposes two functions that take functions as parameters and create new Monads from the existing one.

## 🗺️ Map

A map function takes a function that transforms the Monad's inner value into another value.

```csharp
class Monad<T>
{
    Monad<TNext> Map<TNext>(Func<T, TNext> selector);
}
```

## 🔗 Bind / Flat-map

A bind function takes a function that receives the Monad's inner value and returns a whole new Monad.

```csharp
class Monad<T>
{
    Monad<TNext> Bind<TNext>(Func<T, Monad<TNext>> selector);
}
```

If you passed that function to `Map` instead, you'd get a Monad whose inner value was *also* a Monad (here, a `Monad<Monad<TNext>>`) 🪆. The bind function knows how to 'flatten' this *Monad-within-a-Monad* into a single `Monad<TNext>`, which is why it's sometimes called flat-map.

The monadic contract doesn't require that these functions are literally named `Map` and `Bind`, just that functions with these behaviours exist.

## 📜 The `IEnumerable<T>` Monad

Did you know that `IEnumerable<T>` is a monad?
- It presents the map operation as the `Select` function.
- It presents the bind/flat-map operation as the `SelectMany` function.

```csharp
public static (IEnumerable<decimal> Prices, IEnumerable<string> Words) MenuMonads(IEnumerable<Drink> menu) =>
    (menu.Select(drink => drink.Price), menu.SelectMany(drink => drink.Name.Split(' ')));
```

`Select` turns each drink into one price. `SelectMany` turns each drink into a *sequence* of words, then flattens those sequences into one.

## ✅ Outcomes as Monads

`Outcome<T>` fulfils its monadic contract via the [`Then`](composing-outcomes.md) operator, which is overloaded for both behaviours.

They are happy-path operators, meaning they only execute when the outcome does not hold a problem. Which makes sense, as there's no meaningful value to work with when a problem is present.

Map:
```csharp
public static Outcome<decimal> LargePrice(Outcome<Drink> drink) =>
    drink.Then(d => decimal.Round(d.Price * 1.25m, 2));
```

Bind:
```csharp
public static Outcome<Drink> Favourite(Outcome<Customer> customer) =>
    customer.Then(c => CoffeeShop.FindDrink(c.Favourite));
```

No `Outcome<Outcome<Drink>>` in sight: `Then` flattens it for you.

📂 Sample code: [OutcomesAsMonads.cs](../samples/Outcomes.Samples/OutcomesAsMonads.cs)<br>
▶️ `dotnet run --project samples/Outcomes.Samples -- outcomes-as-monads`

---
### 📚 Index
- [Why Outcomes?](why-outcomes.md)
- [What is a Problem?](what-is-a-problem.md)
- [Creating Outcomes](creating-outcomes.md)
- [Composing Outcomes](composing-outcomes.md)
- [Composing Async Outcomes](composing-async-outcomes.md)
- [Resolving Outcomes](resolving-outcomes.md)

### further reading / miscellaneous
- [Outcome Aggregation](outcome-aggregation.md)
- [Adapting to Outcomes](outcome-adaptation.md)
- this: Outcomes as Monads
- [Runnable samples](../samples/Outcomes.Samples/) ☕

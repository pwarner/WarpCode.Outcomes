# 🧩 Composing Outcomes

There are three core composition operations:
- ✅ `Then` and `Ensure` are executed when your outcome has a value (aka the happy path).
- ❌ `Rescue` is executed when your outcome holds a problem (aka the sad path).

Each of those operations can cause an outcome to switch between the happy and sad paths.

Together with `OnValue` and `OnProblem` (which never change the path), these form the five canonical operations that let you compose a sequence of steps while handling both success and failure.

Throughout this page, a regular walks into our coffee shop ☕ and asks for "the usual". Each section adds one more operator to that pipeline.

> [!TIP]
> 📂 Full sample: [ComposingOutcomes.cs](../samples/Outcomes.Samples/ComposingOutcomes.cs)<br>
> ▶️ `dotnet run --project samples/Outcomes.Samples -- composing-outcomes`

## ✅ Then

[Monadic](outcomes-as-monads.md) "happy path" operators that transform an Outcome's value into the next value, possibly of a different type.

|operation|description|signature|
|---|---|---|
| `Then<TNext>` | Invoke the function with the current value to obtain an `Outcome<TNext>`. | `Func<T, Outcome<TNext>>` |
| `Then<TNext>` | Invoke the function with the current value to obtain a `TNext`, and wrap it in a new `Outcome<TNext>`. | `Func<T, TNext>` |

```csharp
public static Outcome<string> TheUsual(string customerId) =>
    CoffeeShop.FindCustomer(customerId)                              // Outcome<Customer>
        .Then(customer => CoffeeShop.FindDrink(customer.Favourite))   // bind: Customer ➜ Outcome<Drink>
        .Then(drink => $"One {drink.Name}, coming up!");              // map:  Drink ➜ string
```

If we don't know the customer, `FindDrink` never runs and the `NotFoundProblem<Customer>` comes straight out the end. Likewise if their favourite drink isn't on the menu.

## ✔️ Ensure

A "happy path" operator for validating the value of an outcome.

|operation|description|signature|
|---|---|---|
| `Ensure` | Invoke the validation function with the current value to get an `Outcome<None>`. If the returned outcome carries a problem, the original outcome is replaced with that problem. Otherwise the original outcome is returned. | `Func<T, Outcome<None>>` |

You *could* validate with `Then`, but then the validator bears the responsibility of round-tripping the value it was given:

```csharp
public static Outcome<OrderRequest> ValidateForThen(OrderRequest request) =>
    string.IsNullOrWhiteSpace(request.DrinkName)
        ? new ValidationProblem(nameof(request.DrinkName), "is required")
        : request;
```

`Ensure` makes a subtle signature change: the validator returns an `Outcome<None>`, so it only has to say yes (`Outcome.Ok`) or no (a problem).

```csharp
public static Outcome<None> CheckStock(Drink drink) =>
    Database.SoldOut.Contains(drink.Name)
        ? new OutOfStockProblem(drink.Name)
        : Outcome.Ok;
```

…and the value carries on to the next step untouched:

```csharp
public static Outcome<Drink> TheUsualInStock(string customerId) =>
    CoffeeShop.FindCustomer(customerId)
        .Then(customer => CoffeeShop.FindDrink(customer.Favourite))
        .Ensure(CoffeeShop.CheckStock);
```

## 👀 OnValue

A "happy path" operator for performing side effects with the value of an outcome.

|operation|description|signature|
|---|---|---|
| `OnValue` | Invoke the action with the current value, and return the original outcome. | `Action<T>` |

Let's tell the barista what to make:

```csharp
public static Outcome<Drink> TheUsualWithBarista(string customerId) =>
    CoffeeShop.FindCustomer(customerId)
        .Then(customer => CoffeeShop.FindDrink(customer.Favourite))
        .Ensure(CoffeeShop.CheckStock)
        .OnValue(drink => AuditLog.Write($"Grinding beans for a {drink.Name}"));
```

## 🫙 Value-less overloads for `Outcome<None>`

The three happy path operators also have overloads for `Outcome<None>`, which holds no value.

They exist to save you (or your agentic self 🤖) from typing the discard `_` in lambda expressions, and make it clear that the step doesn't use a value from the outcome.

|operation|signature|note|
|---|---|---|
| `Then<TNext>` | `Func<Outcome<TNext>>` | no input parameter |
| `Then<TNext>` | `Func<TNext>` | no input parameter |
| `Then<TNext>` | `Outcome<TNext>` | returns an existing `Outcome<TNext>` without invoking a function |
| `Then<TNext>` | `TNext` | returns an existing `TNext` value without invoking a function |
| `Ensure` | `Func<Outcome<None>>` | no input parameter |
| `Ensure` | `Outcome<None>` | evaluates an existing outcome; handy for chaining a sequence of value-less checks |
| `OnValue` | `Action` | no input parameter |

```csharp
public static Outcome<string> OpenShop() =>
    HeatMachine()                                       // Outcome<None>
        .Ensure(CheckMilk)                              // Func<Outcome<None>>: no discard needed
        .OnValue(() => AuditLog.Write("Machine hot, milk cold"))
        .Then("Open for business!");                    // a constant value
```

## 🛟 Rescue

A "sad path" operator for handling problems in an outcome.

|operation|description|signature|
|---|---|---|
| `Rescue` | Invoke the rescue function with the problem. It can pass the problem back, return a different one, or return a new success outcome. | `Func<Problem, Outcome<T>>` |

`Rescue` is the sad path's mirror image of `Ensure`. It lets you inspect a problem and decide whether you can recover from it, or whether it should stay a problem.

It's only useful if the problem carries enough information to decide (or you can find a good value by other means). Here, when a customer's favourite drink is sold out, we cheerfully offer them a filter coffee instead 😅:

```csharp
public static Outcome<Drink> TheUsualOrFilter(string customerId) =>
    CoffeeShop.FindCustomer(customerId)
        .Then(customer => CoffeeShop.FindDrink(customer.Favourite))
        .Ensure(CoffeeShop.CheckStock)
        .Rescue(OfferFilterCoffee)
        .OnValue(drink => AuditLog.Write($"Grinding beans for a {drink.Name}"));

private static Outcome<Drink> OfferFilterCoffee(Problem problem) => problem switch
{
    OutOfStockProblem => CoffeeShop.FindDrink("Filter Coffee"), // back on the happy path
    _ => problem                                                // anything else stays a problem
};
```

An unknown customer is still a problem; we only rescue the out-of-stock case.

## 🚨 OnProblem

A "sad path" operator for performing side effects with the problem of an outcome.

|operation|description|signature|
|---|---|---|
| `OnProblem` | Invoke the action with the current problem, and return the original outcome. | `Action<Problem>` |

We'd like to know whenever the usual couldn't be made, *before* the rescue papers over it:

```csharp
public static Outcome<Drink> TheUsualFullService(string customerId) =>
    CoffeeShop.FindCustomer(customerId)
        .Then(customer => CoffeeShop.FindDrink(customer.Favourite))
        .Ensure(CoffeeShop.CheckStock)
        .OnProblem(problem => AuditLog.Write($"Couldn't make the usual: {problem.Detail}"))
        .Rescue(OfferFilterCoffee)
        .OnValue(drink => AuditLog.Write($"Grinding beans for a {drink.Name}"));
```

Order matters: side effects see the outcome *as it is at that point* in the pipeline.

## 🧵 Parameter spreading with tuples

You might have noticed how readable composition becomes when you pass methods rather than lambdas. In real workflows, though, a step often needs more than one earlier value. Since we can't use output parameters, we return tuples instead.

For example, to price an order we need the customer, the drink *and* the size. `FindCustomerAndDrink` gathers all three into a tuple:

```csharp
public static Outcome<(Customer, Drink, Size)> FindCustomerAndDrink(OrderRequest request) =>
    FindCustomer(request.CustomerId)
        .Then(customer => FindDrink(request.DrinkName)
            .Ensure(CheckStock)
            .Then(drink => (customer, drink, request.Size)));
```

Overloads of all the happy-path operators automatically spread the members of tuples (of up to 4 items) into separate parameters, so `PriceOrder` can take three ordinary parameters:

```csharp
public static Order PriceOrder(Customer customer, Drink drink, Size size)
```

```csharp
public static Outcome<Order> TakeOrder(OrderRequest request) =>
    CoffeeShop.FindCustomerAndDrink(request)
        .Then(CoffeeShop.PriceOrder);
```

Without spreading, every step would have to accept the tuple and unpack it itself:

```csharp
public static Order PriceOrderWithoutSpreading((Customer, Drink, Size) order)
{
    var (customer, drink, size) = order;
    return CoffeeShop.PriceOrder(customer, drink, size);
}
```

> [!TIP]
> Talking to a database or an HTTP API? Every operator here also works with async code, and tuples still spread. See [Composing Async Outcomes](composing-async-outcomes.md). ⚡

## 📋 Summary
- Three operators can switch you from the happy path to the sad path or back: `Then`, `Ensure` and `Rescue`.
- `OnValue` and `OnProblem` are for performing side effects, and never change the outcome.
- `Outcome<None>` has value-less overloads, so you don't need discards.
- Tuples of up to four items are spread into separate parameters.

---
### 📚 Index
- [Why Outcomes?](why-outcomes.md)
- [What is a Problem?](what-is-a-problem.md)
- [Creating Outcomes](creating-outcomes.md)
- this: Composing Outcomes
- [Composing Async Outcomes](composing-async-outcomes.md)
- [Resolving Outcomes](resolving-outcomes.md)

### further reading / miscellaneous
- [Outcome Aggregation](outcome-aggregation.md)
- [Adapting to Outcomes](outcome-adaptation.md)
- [Outcomes as Monads](outcomes-as-monads.md)
- [Runnable samples](../samples/Outcomes.Samples/) ☕

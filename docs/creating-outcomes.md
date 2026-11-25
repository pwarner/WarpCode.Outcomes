# 🏗️ Creating Outcomes

As mentioned, Outcomes have exactly two states: either they hold a value (`Success Outcomes`), or they hold a Problem (`Problem Outcomes`).

You can consider success outcomes as the ✅ *'happy path'* of your code, and problem outcomes as the ❌ *'unhappy path'*.

Or if you really enjoy the railway-orientated concept 🚂, you can call them the 'hale rail' and the 'fail rail'.

`Success Outcomes` can also hold an absence of meaningful value. Use `None` as the generic type, which plays the role of `void` (known as `Unit` in functional languages).

## ✅ Creating success outcomes

```csharp
var byConstructor = new Outcome<string>("Flat White");
Outcome<string> byConversion = "Flat White";
Outcome<string> byFactory = Outcome.Of("Flat White");
```

Value-less success outcomes, with `Outcome.Ok` (or `default`, which is the same thing):

```csharp
Outcome<None> ok = Outcome.Ok;
Outcome<None> alsoOk = default;
```

## ❌ Creating problem outcomes

```csharp
var problemByConstructor = new Outcome<Drink>(new OutOfStockProblem("Mocha"));
Outcome<Drink> problemByConversion = new OutOfStockProblem("Mocha");
Outcome<Drink> problemByFactory = Outcome.OfProblem<Drink>(new OutOfStockProblem("Mocha"));
```

Implicit conversion works for any type deriving from `WarpCode.Outcomes.Problem`.

Value-less problem outcomes, with `Outcome.OfProblem(...)`:

```csharp
Outcome<None> valuelessProblem = Outcome.OfProblem(new OutOfStockProblem("Mocha"));
```

Need a problem in a hurry? Create the basic `Problem` from just a detail message:

```csharp
Outcome<Drink> fromDetail = Outcome.OfProblem<Drink>("The grinder is jammed");
Outcome<None> valuelessFromDetail = Outcome.OfProblem("The grinder is jammed");
```

> [!NOTE]
> Outcomes have value equality: two outcomes are equal when they hold equal values, or equal problems.

> [!TIP]
> Need an *async* outcome from the start? See [Going async](composing-async-outcomes.md) for `AsyncOutcome.Of`, `AsyncOutcome.Ok` and friends.

📂 Sample code: [CreatingOutcomes.cs](../samples/Outcomes.Samples/CreatingOutcomes.cs)<br>
▶️ `dotnet run --project samples/Outcomes.Samples -- creating-outcomes`

---
### 📚 Index
- [Why Outcomes?](why-outcomes.md)
- [What is a Problem?](what-is-a-problem.md)
- this: Creating Outcomes
- [Composing Outcomes](composing-outcomes.md)
- [Composing Async Outcomes](composing-async-outcomes.md)
- [Resolving Outcomes](resolving-outcomes.md)

### further reading / miscellaneous
- [Outcome Aggregation](outcome-aggregation.md)
- [Adapting to Outcomes](outcome-adaptation.md)
- [Outcomes as Monads](outcomes-as-monads.md)
- [Runnable samples](../samples/Outcomes.Samples/) ☕

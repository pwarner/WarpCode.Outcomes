# ☕ WarpCode.Outcomes samples

Runnable, dependency-free samples behind every code excerpt in the [docs](../../docs/why-outcomes.md).

They're all set in one small coffee shop, so you'll meet the same customers, drinks and problems on every page.

## ▶️ Running them

```shell
dotnet run --project samples/Outcomes.Samples                             # run every sample
dotnet run --project samples/Outcomes.Samples -- composing-async-outcomes # run one, named after its doc
```

## 🗂️ What's where

|doc|sample|
|---|---|
| [Why Outcomes?](../../docs/why-outcomes.md) | [WhyOutcomes.cs](WhyOutcomes.cs) |
| [What is a Problem?](../../docs/what-is-a-problem.md) | [WhatIsAProblem.cs](WhatIsAProblem.cs) |
| [Creating Outcomes](../../docs/creating-outcomes.md) | [CreatingOutcomes.cs](CreatingOutcomes.cs) |
| [Composing Outcomes](../../docs/composing-outcomes.md) | [ComposingOutcomes.cs](ComposingOutcomes.cs) |
| [Composing Async Outcomes](../../docs/composing-async-outcomes.md) | [ComposingAsyncOutcomes.cs](ComposingAsyncOutcomes.cs) |
| [Resolving Outcomes](../../docs/resolving-outcomes.md) | [ResolvingOutcomes.cs](ResolvingOutcomes.cs) |
| [Outcome Aggregation](../../docs/outcome-aggregation.md) | [OutcomeAggregation.cs](OutcomeAggregation.cs) |
| [Adapting to Outcomes](../../docs/outcome-adaptation.md) | [OutcomeAdaptation.cs](OutcomeAdaptation.cs) |
| [Outcomes as Monads](../../docs/outcomes-as-monads.md) | [OutcomesAsMonads.cs](OutcomesAsMonads.cs) |

The shared domain lives in [CoffeeShop.cs](CoffeeShop.cs): the records, the problems, an in-memory `Database`, and the `CoffeeShop` operations that return outcomes.

## 👥 The regulars

|customer|favourite|why they're interesting|
|---|---|---|
| Ada | Flat White | 9 loyalty stamps, so her next coffee is free 🎉 |
| Grace | Mocha | the Mocha is sold out, and her $3.00 balance won't cover a large latte |
| Linus | Turmeric Latte | not on the menu |

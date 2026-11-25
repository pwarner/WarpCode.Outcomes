# 🤔 Why Outcomes?

Throwing exceptions in your code to enforce business logic is:
- 💸 **expensive**: the entire call-stack is captured for the exception.
- 🙅 **incorrect**: exceptions should represent unexpected, *exceptional* states and events, not expected ones.

An Outcome is a `discriminated union` that represents a value or a problem, but never both.

It is implemented as a generic, immutable value-type.

We can use outcomes to control our logical workflows without resorting to throwing exceptions to halt the flow.

> [!TIP]
> Every example in these docs comes from a small, runnable coffee-shop ☕ sample: 📂 [samples/Outcomes.Samples](../samples/Outcomes.Samples/).
>
> ▶️ `dotnet run --project samples/Outcomes.Samples -- why-outcomes`

Instead of doing this 👇

```csharp
public static Customer FindCustomerOrThrow(string customerId)
{
    Customer? customer = Database.Customers.Find(customerId);

    if (customer is null)
        throw new CustomerNotFoundException(customerId);

    return customer;
}
```

we do this 👇

```csharp
using WarpCode.Outcomes;

public static Outcome<Customer> FindCustomer(string customerId)
{
    Customer? customer = Database.Customers.Find(customerId);

    if (customer is null)
        return new NotFoundProblem<Customer>(customerId);

    return customer;
}
```

By returning a Problem instead of throwing an exception:
- ✋ we retain the convenience of halting execution.
- ⚡ we skip the cost of capturing the call-stack.
- 📣 we state explicitly that a missing customer is not an exceptional circumstance, but one we anticipated.
- 🧩 we can compose readable workflows from small logical steps.

That last bullet-point means you can write code like this:

```csharp
public static Outcome<Receipt> PlaceOrder(OrderRequest request) =>
    Outcome.Of(request)
        .Ensure(CoffeeShop.Validate)
        .Then(CoffeeShop.FindCustomerAndDrink)
        .Then(CoffeeShop.PriceOrder)
        .Then(CoffeeShop.ChargeCard);
```

Each step runs only if the previous one succeeded. The first problem (an invalid request, an unknown customer, a sold-out drink, a declined card) skips the rest of the workflow and comes out the other end.

📂 Sample code: [WhyOutcomes.cs](../samples/Outcomes.Samples/WhyOutcomes.cs), [CoffeeShop.cs](../samples/Outcomes.Samples/CoffeeShop.cs)

---
### 📚 Index
- this: Why Outcomes?
- [What is a Problem?](what-is-a-problem.md)
- [Creating Outcomes](creating-outcomes.md)
- [Composing Outcomes](composing-outcomes.md)
- [Composing Async Outcomes](composing-async-outcomes.md)
- [Resolving Outcomes](resolving-outcomes.md)

### further reading / miscellaneous
- [Outcome Aggregation](outcome-aggregation.md)
- [Adapting to Outcomes](outcome-adaptation.md)
- [Outcomes as Monads](outcomes-as-monads.md)
- [Runnable samples](../samples/Outcomes.Samples/) ☕

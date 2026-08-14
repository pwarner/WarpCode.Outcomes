# Why Outcomes?

Throwing exceptions in your code to enforce business logic is:
- expensive: the entire call-stack is captured for the exception.
- incorrect: exceptions should represent unexpected/exceptional states and events, not expected ones.

An Outcome is a `discriminated union` that represents a value or a problem, but not both. 

It is implemented as a generic immutable value-type.

We can use outcomes to control our logical workflows without resorting to throwing exceptions to halt the flow.

Instead of doing this 👇

```csharp
public async Task<Customer> FetchCustomerAsync(string customerId)
{
    Customer? customer = await _dbContext.Customers.FindAsync(customerId);

    if(customer is null)
        throw new CustomerNotFoundException(
            "Customer was not found", 
            customerId);

    return customer;
}
```

we do this 👇

```csharp
using WarpCode.Outcomes;


public async Task<Outcome<TCustomer>> FetchCustomerAsync(string customerId)
{
    Customer? customer = await _dbContext.Customers.FindAsync(customerId);

    if(customer is null)
        return new CustomerNotFoundProblem(
            "Customer was not found",
            customerId);

    return customer;
}
```
By returning a Problem instead of throwing an exception:
- we retain the convenience of halting execution
- we skip the need to capture the call-stack
- we explicitly indicate that a customer without the supplied id is not an exceptional circumstance but one we anticipated.
- we can compose readable workflows from logical steps.

That last bullet-point means you can write code like this:

```csharp
private Task<Outcome<CustomerUpdateResult>> UpdateCustomerNameFlow(UpdateCustomerNameCommand cmd) =>
    ValidateCommand(cmd)
    | LoadCustomerAync
    | UpdateCustomer<UpdateCustomerNameCommand>
    | SaveCustomerAsync
    | ToUpdateCommandResult;
```

---

### Index
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
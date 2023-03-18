# Why Outcomes?

Throwing exceptions in your code to enforce business logic is:
- expensive (the call-stack is captured for the exception)
- incorrect (exceptions should represent unexpected/unwanted states and events, not expected ones)

In place of throwing exceptions, we can use Outcomes to control our program flow.

An Outcome is a type that can represent a value or a problem, but not both.

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

we can do this 👇
```csharp
public async Task<Outcome<Customer>> FetchCustomerAsync(string customerId)
{
    Customer? customer = await _dbContext.Customers.FindAsync(customerId);

    if(customer is null)
        return new CustomerNotFoundProblem(
            "Customer was not found",
            customerId);

    return customer;
```

By returning a Problem instead of throwing an exception:
- we retain the convenience of halting execution
- we skip the need to capture the call-stack
- we explicitly indicate that a customer without the supplied id is not an exceptional circumstance but one we anticipated.
- allows us to use composition to compose workflows from logical steps.

That intriguing last bullet-point means you can write code like this:
```csharp
private Task<Outcome<CustomerUpdateResult>> UpdateCustomerNameFlow(UpdateCustomerNameCommand cmd) =>
    from _ in ValidateCommand(cmd)
    from customer in LoadCustomerAync(cmd.CustomerId)
    from modified in customer.UpdateName(cmd.NewName)
    from result in SaveCustomerAsync(modified)
    select new CustomerUpdateResult(cmd.Id, modified.Name, result.LastUpdated);
```

### Index
- this: Why Outcomes?
- [What is a Problem?](./docs/what-is-a-problem.md)
- [Creating Outcomes](./docs/creating-outcomes.md)
- [Composing Outcomes](./docs/composing-outcomes.md)
- [Adapting to Outcomes](./docs/outcome-adaptation.md)
- [Resolving Outcomes](./docs/resolving-outcomes.md)

### further reading / miscellaneous
- [Outcomes as Monads](./docs/outcomes-as-monads.md)

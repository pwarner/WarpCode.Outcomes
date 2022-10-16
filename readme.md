# Why Outcomes?

Throwing exceptions in your code to express business logic is:
- expensive (the call-stack is captured for the exception)
- incorrect (exceptions should represent unexpected states and events, not expected ones)

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

we shall do this 👇
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
private AsyncOutcome<CustomerUpdateResult> UpdateCustomerNameFlow(UpdateCustomerNameCommand cmd) =>
    from _ in ValidateCommand(cmd)
    from customer in LoadCustomerAync(cmd.CustomerId)
    from modified in customer.UpdateName(cmd.NewName)
    from result in SaveCustomerAsync(modified)
    select new CustomerUpdateResult(cmd.Id, modified.Name, result.LastUpdated);
```

# Creating Outcomes
As mentioned, Outcomes have exactly two states: either they carry a value (Successful Outcomes), or they carry a Problem (Problem Outcomes) 

Successful Outcomes can also carry an absence of value (The `None` type, which is like `System.Void` or `Unit`)

## Creating Successful Outcomes
By constructor
```csharp
var outcome = new Outcome<string>("success!");
var outcome2 = new Outcome<int>(13);
```

With `Outcome.Ok`
```csharp
Outcome<string> outcome = Outcome.Ok("success!");
Outcome<int> outcome2 = Outcome.Ok(13);
```

By implicit coversion
```csharp
Outcome<string> outcome = "foo";
Outcome<int> outcome2 = 13;
```

Create value-less Outcomes with `Outcome.NoProblem`
```csharp
Outcome<None> outcome = Outcome.NoProblem;
```

`Outcome<None>` also implicitly converts to other `Outcome<T>` types, so you can write:
```csharp
Outcome<string?> outcome = Outcome.NoProblem; // value is default(string), or null
Outcome<int> outcome = Outcome.NoProblem; // value is default(int), or 0
```

## Creating Problem Outcomes
By constructor
```csharp
var outcome = new Outcome<string>(new SomeProblem());
```

With `Outcome.Problem<T>`
```csharp
Outcome<string> outcome = Outcome.Problem<string>(new SomeProblem());
```

By implicit coversion
```csharp
Outcome<string> outcome = new SomeProblem();
```

Create value-less Outcomes with `Outcome.Problem`
```csharp
Outcome<None> outcome = Outcome.Problem(new SomeProblem());
```

Again, since `Outcome<None>` implicitly converts to other `Outcome<T>` types, you can write:
```csharp
Outcome<string?> outcome = Outcome.Problem(new SomeProblem());
```
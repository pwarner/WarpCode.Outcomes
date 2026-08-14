# Composing Outcomes
There are three core composition operations:
- `Then` and `Ensure` are executed when your outcome has a value (aka the happy path).
- `Rescue` is executed when your outcome holds a problem (aka the sad path).

Each of those operations can cause an outcome to switch between the happy and sad paths.

Together with `OnValue` and `OnProblem` (which never affect the path, these form the five canonical operators that allow you to compose a sequence of operations while handling both success and failure cases.
  

## Then

[Monadic](outcomes-as-monads.md) "happy path" operators that transform an Outcome value to the next value and possibly different type.

|operation|description|signature|
|---|---|---|
| `Then<TNext>` | Invoke the function with the current value to obtain a `Outcome<TNext>`. | `Func<T,Outcome<TNext>>` |
| `Then<TNext>` | Invoke the function with the current value to obtain a `TNext`, and produce a new `Outcome<TNext>` with it. | `Func<T,TNext>` |


```csharp
public Outcome<Customer> UpdateCachedCustomer(UpdateCustomer command)
{
    CacheEntry<Customer> customer = _cacheService<Customer>.FetchCachedEntity(command.CustomerId);

    return Outcome.Of(customer)
            .Then(EnsureCachedEntity // using method name
            .Then(customer => customer with { Name = command.Name, Status = command.Status }); // using lambda, command is captured by closure
}

private static Outcome<T> EnsureCachedEntity<T>(CacheEntry<T> maybeEntity) =>
    maybeEntity switch
    {
        { IsCached: false } => new CacheMissProblem<T>(maybeEntity.Id),
        { Entity: var entity } => entity
    };

```

## Ensure
A "happy path" operator useful for validating the value of an outcome.

|operation|description|signature|
|---|---|---|
| `Ensure` | invoke the validation function with the current value to get a `Outcome{TNone}`. If the returned outcome carries a problem, the original outcome is replaced with the problem outcome. Otherwise the original outcome is returned. | `Func<T,Outcome<TNone>>` |

The following example (using a `Then` signature) works fine, but the ValidateCommand method bears the responsibility for round-tripping the the input parameter value.
```csharp
public Outcome<UpdateCustomer> ValidateCommand(UpdateCustomer command)
{
    bool isValid = // your validation logic here

    return isValid 
        ? Outcome.Of(command) 
        : new ValidationProblem("Invalid command");
}

```
The **Ensure** operation makes a subtle signature change, returning `Outcome<None>` and only returns a problem, or an ok outcome. 
The original outcome only changes to the sad path if a problem is returned.

```csharp
public Outcome<None> ValidateCommand(UpdateCustomer command)
{
    bool isValid = // your validation logic here

    return isValid 
        ? Outcome.Ok 
        : new ValidationProblem("Invalid command");
}
```

## OnValue
A "happy path" operator for performing side effects with the value of an outcome.

|operation|description|signature|
|---|---|---|
| `OnValue` | invoke the provided action with the current value, and return the original outcome | `Action<T>` |


Let's revisit an example above to see the **OnValue** operator in action. We just want to log a message when we have a cache hit.
```csharp
public Outcome<Customer> UpdateCachedCustomer(UpdateCustomer command)
{
    CacheEntry<Customer> customer = _cacheService<Customer>.FetchCachedEntity(command.CustomerId);

    return Outcome.Of(customer)
            .Then(EnsureCachedEntity)
            .OnValue(LogCacheHit)
            .Then(customer => customer with { Name = command.Name, Status = command.Status });
    
    static void LogCacheHit(Customer c) => _logger.LogInformation("Successfully fetched customer {CustomerId} from cache", c.Id);
}
```

## Value-less overloads for `Result<None>`
The three happy path operators also have overloads for `Outcome<None>` which holds no value. 

They exist to save you (or your agentic self) from having to type the discard operator `_` in lambda expressions, and make it more clear that you are not using a value from the outcome in the body of the operator.

|operation|signature|note|
|---|---|---
| `Then<TNext>` | `Func<Outcome<TNext>>` | no input parameter |
| `Then<TNext>` | `Func<TNext>` | no input parameter |
| `Then<TNext>` | `Outcome<TNext>` | returns an existing Outcome<TNext> without invoking a function |
| `Then<TNext>` | `TNext` | returns an existing TNext value without invoking a function |
| `OnSuccess` | `Action` | no input parameter |

## Rescue
A "sad path" operator for handling problems in an outcome.

|operation|description|signature|
|---|---|---|
| `Rescue` | invoke the rescue function with the problem, which returns a `Outcome<T>`. That function can just return the original problem outcome, or it can pass back a new success outcome. | `Func<Problem,Outcome<T>>` |

**Rescue** is the sad path equivalent of **Ensure**. It allows you to inspect a problem and decide whether to return a new success outcome, or just pass the original problem back.

It's only useful if the problem contains enough information to be able to determine a valid success outcome, unless you can produce one by other means. 

In this contrived example, the business realised that returning a problem on a customer cache miss was not ideal, and decided to perform an UPSERT instead, creating a default customer instead. 

The problem contains the customer id, so we can use that to create a default customer.

```csharp
public Outcome<Customer> UpdateCachedCustomer(UpdateCustomer command)
{
    CacheEntry<Customer> customer = _cacheService<Customer>.FetchCachedEntity(command.CustomerId);

    return Outcome.Of(customer)
            .Then(EnsureCachedEntity)
            .Rescue(CreateDefaultCustomerOnCacheMiss)
            .Then(customer => customer with { Name = command.Name, Status = command.Status });
    
    static Outcome<Customer> CreateDefaultCustomerOnCacheMiss(Problem p) => p switch 
    {
        CacheMissProblem<Customer> cacheMiss => new Customer { Id = cacheMiss.Id, Name = "Default Customer", Status = "Unknown" },
        _ => p // pass the original problem back if it's not a cache miss
    };
}

```

## OnProblem
A "sad path" operator for performing side effects with the problem of an outcome.

|operation|description|signature|
|---|---|---|
| `OnProblem` | invoke the provided action with the current problem, and return the original outcome | `Action<Problem>` |


**OnProblem** example
Let's add to our **OnValue** example to see the **OnProblem** operator in action. We also want to log a message when we have a cache miss problem.
```csharp
public Outcome<Customer> UpdateCachedCustomer(UpdateCustomer command)
{
    CacheEntry<Customer> customer = _cacheService<Customer>.FetchCachedEntity(command.CustomerId);

    return Outcome.Of(customer)
            .Then(EnsureCachedEntity)
            .OnValue(LogCacheHit)
            .OnProblem(LogCacheMiss)
            .Then(customer => customer with { Name = command.Name, Status = command.Status });
    
    static void LogCacheHit(Customer c) => _logger.LogInformation("Successfully fetched customer {CustomerId} from cache", c.Id);
    static void LogCacheMiss(Problem p) 
    {
        if(p is CacheMissProblem<Customer> cacheMiss)
            _logger.LogInformation("Cache miss for customer {CustomerId}", cacheMiss.Id;
    }
}
```
// TODO: we could make a strongly typed OnProblem operator that uses a generic TProblem type parameter which is only invoked if the problem is of the expected type. 
// This would save us from having to do a type check and cast in the body of the LogCacheMiss method.


## Parameter spreading with Tuple

In the examples above, you might have noticed how readable composition is when you work with methods over lambda functions. 

These are deeply contrived examples where the methods take a single input value.

In real world scenarios, the inputs that you need to keep in scope for later operations require functions that return multiple values.

Since we can't use output parameters, we'll want to return tuples instead.

For example, let's say we want to fetch the state of an entity with a given Id, but our state doesn't hold its own identity. We don't want to lose the id, so we'll return a tuple of the id and the entity state.

```csharp
public async Task<Outcome<(int, EntityState?)>> FetchEntityState(int id, CancellationToken cancellationToken)
{
    EntityState? entity = await _persister.LoadAsync(id, cancellationToken); 
    return (id, entity);
}
```

Later, after validating and modifying the entity state, we want to pass the id and the modified state to a method that will persist it.

```csharp
public async Task<Outcome<None>> PersistEntityState(int id, EntityState state, CancellationToken cancellationToken)
{
    await _persister.SaveAsync(id, state, cancellationToken);
    return Outcome.Ok;
}
```

If we didn't allow for parameter spreading, the signarure of the `PersistEntityState` method would require us to take a tuple as input, and then unpack it in the body of the method. 
This would make the method less readable, and would require us to write more code.

```csharp
public async Task<Outcome<None>> PersistEntityState((int, EntityState) tuple, CancellationToken cancellationToken)
{
    var (id, state) = tuple;
    await _persister.SaveAsync(id, state, cancellationToken);
    return Outcome.Ok;
}
```

`WarpCode.Outcomes` supports async composition.

Overloads of all composition operators automatically spread the member values in those tuples to multiple parameters (support for up to 4 parameters).

```csharp
public static Outcome<int> AnotherSillyExample(int firstInput) =>
    Outcome.Of(firstInput)
    .Then(NextInput(13))
    .Then(AndAnotherInput(42))
    .Then(AddThem);
    
private static Func<int, (int, int)> NextInput(int b) => a => (a, b);
private static Func<int, int, (int, int, int)> AndAnotherInput(int c) => (a, b) => (a, b, c);
private static int AddThem(int a, int b, int c) => a + b + c;
```

## Summary
- Three of our operators can switch you from the happy path to the sad path or back: `Then`, `Ensure` and `Rescue`.
- `OnSuccess` and `OnProblem` are for performing side effects, and don't change the outcome.
- Tuples of up to four items are spread to input parameters.

---
### Index
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
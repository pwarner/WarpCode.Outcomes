# Adapting to Outcomes

You've started using Outcomes in your code, because you're a talented and discerning developer.

But there's a lot of other code you have to work with that doesn't use Outcomes. 
It's not at all viable to change that code to use them, and often not possible because *it's not your code*.

Happily, you can still work with methods that don't return Outcomes via a simple adaptation wrapper.

For example, in the code below, the method `GetReservationAsync` doesn't return a `Task<Outcome<Reservation>>`. 
Any problems that occur during invocation of that method result in an exception being thrown.
Specifically, the method documentation tells you that it throws: 
- An `ArgumentNullException` if the `bookingId` parameter is null.
- An `ArgumentException` if the `bookingId` parameter is empty or just whitespace.
- an `BookingNotFoundException` if a seemingly valid `bookingId` is provided but a matching reservation with this id is not found.

```csharp
Task<Reservation> GetReservationAsync(string bookingId);
```

To adapt this method for use, we need to do two things:

1. Use the `ToOutcome()` extension method
2. Define an `ExceptionMap` either globally, or provide it as a parameter.

### Exception Mapping
An `ExceptionMap` is just a delegate with the following signature:
```csharp
/// <summary>
/// A function called when exceptions are thrown by code instead of returning outcomes.
/// If the function returns a <see cref="IProblem"/> then a new <see cref="Outcome{T}"/>
/// will be returned by the adaptive methods. If null is returned, the exception will be re-thrown.
/// </summary>
/// <param name="exception">A caught <see cref="Exception"/> instance to try to map to a <see cref="IProblem"/>.</param>
/// <returns>A <see cref="IProblem"/> if one could be created from the exception, or null.</returns>
public delegate IProblem? ExceptionMap(Exception exception);
```

### Exception map as a parameter

We can provide a delegate as a parameter just to handle the `BookingNotFound` exception case.

```csharp
await GetReservationAsync(bookingId).ToOutcome(e=> 
	e switch {
		BookingNotFoundException => new NotFoundProblem($"Could not find booking with Id {bookingId}",
		_ => null
	});
```

As it's quite a common use case to only need to map a single exception type to a problem, there's a convenient short-cut syntax:
```csharp
await GetReservationAsync(bookingId)
.ToOutcome<Reservation, BookingNotFoundException>(e=> 
	new NotFoundProblem($"Could not find booking with Id {bookingId}");
```
This approach makes use of the generic `ExceptionMap<TException>` delegate, 
which catches and maps only exceptions of type `TException` to a problem, 
allowing all other exceptions to throw.


### Global/Application-level exception mapping
Instead of providing an exception map delegate as a method parameter to every call of `ToOutcome()`, 
it is far more convenient to create a single exception mapper function responsible for mapping any exceptions that logically map to problems.

To achieve this, set `Adapt.MapExections` to this function in your application startup.

```csharp

Adapt.MapExceptions = e => 
	e switch 
	{
		BookingNotFoundException bnf => 
			new NotFoundProblem($"Could not find booking with Id {bnf.BookingId}",
		
		BalanceUnpaidException bup => 
			new OverdueProblem("$An overdue amount of {bup.Amount} is due for booking {bup.BookingId}"),

		// other exception types mapped here
		
		_ => null // let all other exceptions be thrown
	};
```

### What can be adapted?
The `ToOutcome()` extension method is available for:

|Target type|Adapts to|
|--|--|
|`System.Func<T>`|`Outcome<T>`|
|`System.Action`|`Outcome<None>`|
| `Task<T>` | `Task<Outcome<T>>` |
| `Task` | `Task<Outcome<None>>` |
| `ValueTask<T>` | `ValueTask<Outcome<T>>` |
| `ValueTask` | `ValueTask<Outcome<None>>` |



### Don't map `System.Exception`
It might be tempting to try and catch all exceptions of type `Exception` and return some catch-all ExceptionWrapper problem.

This is a bad idea. Mapping all exceptions would swallow up guard exceptions like `ArgumentNullException` and `AgumentException`, thrown when you misuse an API. 

Outcomes help to replace throwing Exceptions to represent violations of business state, but they are most definitely **not** intended to replace throwing Exceptions *when exceptional conditions occur*.

---
### Index
- [Why Outcomes?](why-outcomes.md)
- [What is a Problem?](what-is-a-problem.md)
- [Creating Outcomes](creating-outcomes.md)
- [Composing Outcomes](composing-outcomes.md)
- [Resolving Outcomes](resolving-outcomes.md)

### further reading / miscellaneous
- [Outcome Extensions](outcome-extensions.md)
- this: Adapting to Outcomes
- [Outcomes as Monads](outcomes-as-monads.md)
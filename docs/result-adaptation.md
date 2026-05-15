# Adapting to Results

You've started using Results in your code, because you're a talented and discerning developer.

But there's a lot of other code you have to work with that doesn't use Results. 
It's not at all viable to change that code to use them, and often not possible because *it's not your code*.

Happily, you can still work with methods that don't return Results.
- via [composition](composing-results.md) which overloads the `|` operator for function signatures that don't return results.
- with `ToResult()` via the Adaptation wrapper (this document).

> [!NOTE]
> Composition with functions that don't return results do not catch exceptions. 
> In cases where you want to adapt a function that doesn't return a result, you need to use the `ToResult()` extension method.


For example, in the code below, the method `GetReservationAsync` doesn't return a `Task<Result<TReservation>>`. 
Any problems that occur during invocation of that method result in an exception being thrown.
Specifically, the method documentation tells you that it throws: 
- An `ArgumentNullException` if the `bookingId` parameter is null.
- An `ArgumentException` if the `bookingId` parameter is empty or just whitespace.
- an `BookingNotFoundException` if a seemingly valid `bookingId` is provided but a matching reservation with this id is not found.

```csharp
Task<Reservation> GetReservationAsync(string bookingId);
```

To adapt this method to give us Problems instead of Exceptions, we need to do two things:

1. Use the `ToResult()` extension method (or in non-extension form`Adapt.ToResult(...)`)
2. Define an `ExceptionMap` either globally, or provide it as a parameter.

### Exception Mapping
An `ExceptionMap` is just a delegate with the following signature:
```csharp
/// <summary>
/// A function called when exceptions are thrown by code instead of returning results.
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
	.ToResult<TReservation, BookingNotFoundException>(e=> 
		new NotFoundProblem($"Could not find booking with Id {bookingId}");
```
This approach makes use of the generic `ExceptionMap<TException>` delegate, 
which catches and maps only exceptions of type `TException` to a problem, 
allowing all other exceptions to throw.


### Global/Application-level exception mapping
Instead of providing an exception map delegate as a method parameter to every call of `ToResult()`, 
it is far more convenient to create a single exception mapper function responsible for mapping any exceptions that logically map to problems.

To achieve this, set the static `Adapt.MapExceptions` property to an instance of this delegate in your application startup.

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

> [!CAUTION]
> It's tempting to try and catch all exceptions of type `Exception` and return some catch-all ExceptionWrapper problem.
> This is a bad idea. Mapping all exceptions would swallow up guard exceptions like `ArgumentNullException` and `AgumentException`, thrown when you misuse an API. 
> Results help to replace throwing Exceptions to represent violations of business state, but they are most definitely **not** intended to replace throwing Exceptions *when exceptional conditions occur*.


### What can be adapted?
The `ToResult()` extension method is available for:

|Target type|Adapts to|
|--|--|
|`System.Func<T>`|`Result<T>`|
|`System.Action`|`Result<TNone>`|
| `Task<T>` | `Task<Result<T>>` |
| `Task` | `Task<Result<TNone>>` |
| `ValueTask<T>` | `ValueTask<Result<T>>` |
| `ValueTask` | `ValueTask<Result<TNone>>` |

// TO DO: Adapt for async should return AsyncResult<T>

---
### Index
- [Why Results?](why-results.md)
- [What is a Problem?](what-is-a-problem.md)
- [Creating Results](creating-results.md)
- [Composing Results](composing-results.md)
- [Resolving Results](resolving-results.md)

### further reading / miscellaneous
- [Result Aggregation](result-extensions.md)
- this: Adapting to Results
- [Results as Monads](results-as-monads.md)
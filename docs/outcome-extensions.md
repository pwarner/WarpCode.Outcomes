# Outcome Extensions

Each of these extensions have an async equivalent that extends `Task<Outcome<T>>` and `ValueTask<Outcome<T>>`.

## OnSuccess
Invokes an action with the value of a success outcome as the parameter, and the outcome is returned for further operations.

If the outcome is a problem outcome, the method is not invoked.
```csharp
	Outcome<string> outcome = GetMyOutcome();
	outcome.OnSuccess(value=> Console.WriteLine($"success: {value}"));
```

## OnProblem
Invokes an action with the problem of a problem outcome as the parameter, and the outcome is returned for further operations.

If the outcome is a success outcome, the method is not invoked.
```csharp
	Outcome<string> outcome = GetMyOutcome();
	outcome.OnProblem(problem=> Console.WriteLine($"problem: {problem.Detail}"));
```

## Ensure
Invokes a predicate with the value of a success outcome as the parameter.

If the predicate returns false, a factory delegate is invoked that returns an `IProblem`.

If the outcome is a problem outcome, the method is not invoked.
```csharp
Outcome<int> outcome = GetInputNumber();
outcome.Ensure(
	value=> value > 0 && value <= 10,
	value=> new ValidationProblem($"Value must be from 1 to 10, but was {value}")); 
```

## Rescue
Invokes a predicate with the value of a problem outcome as the parameter.

If the predicate returns true, a factory delegate is invoked that returns a replacement value for the outcome.

If the outcome is a success outcome, the method is not invoked.
```csharp
Outcome<User> outcome = GetUser();
outcome.Rescue(
	problem => problem is AnonUserProblem,
	problem=> new User(ClaimsPrincipal.Current))); 
```

## Aggregation
There are two aggregation overloads, and two async overloads that work on an `IAsyncEnumerable<Outcome<T>>`

### Aggregate on `IEnumerable<Outcome<T>>`
Aggregate will return an `Outcome<List<T>>`. 

It will be a success outcome containing a `List<T>` if all of the outcomes in the sequence were success outcomes.

If there was 1 or more problems and you pass true as the `bailEarly` parameter, 
it will return a problem outcome holding the first problem found in the sequence.

If you pass false (or omit the parameter which defaults to false), you will get a ProblemAggregate that contains a list of all the problems found.

```chsarp
IEnumerable<Outcome<Temperature>> temps = GetTemperatures();

// bail early and return the first problem if one of the outcomes has a problem
Outcome<List<Temperature>> tempAggregate = temps.Aggregate(true); 

```

### Aggregate on `IEnumerable<Outcome<None>>`
Aggregate will return an `Outcome<None>`. 

Exhibits the same behaviour as before, except returns an `Outcome<None>` as none of the outcomes in the sequence hold a useful value.

```chsarp
IEnumerable<Outcome<Temperature>> temps = GetTemperatures();

// use default bail early setting of false, collect all the problems in a ProblemAggregate instance.
Outcome<None> tempAggregate = temps.Aggregate(); 
```

> `ProblemAggregate` is a type defined in the library that exposes a `Problems` property of type `IReadOnlyList<IProblem>`.

### Index
- [Why Outcomes?](../readme.md)
- [What is a Problem?](what-is-a-problem.md)
- [Creating Outcomes](creating-outcomes.md)
- [Composing Outcomes](composing-outcomes.md)
- [Resolving Outcomes](resolving-outcomes.md)

### further reading / miscellaneous
- this: Outcome Extensions
- [Adapting to Outcomes](outcome-adaptation.md)
- [Outcomes as Monads](outcomes-as-monads.md)
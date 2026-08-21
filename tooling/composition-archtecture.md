---
Purpose: agents read this when they need to understand the composition extensions
---

## Extended types
Two structs representing an Outcome are the target extension points for composition functions.
- Outcome<T>
- AsyncOutcome<T>
Extensions use the extension members syntax introduced in C# 14

## Composition functions
### Happy path
Happy path means functions executed when an outcome (or result of `await AsyncOutcome`) does not hold a Problem value.
- `Then<TNext>` calls `Func<T, TNext>` to get value of next Outcome (monadic Map).
- `Then<TNext>` calls `Func<T, Outcome<TNext>>` to get next Outcome (monadic Bind).
- `Ensure` calls `Func<T, Outcome<None>>` and returns original Outcome if delegate returns a problem-free outcome, else an Outcome<T> with the found problem. Used for validating the value.
- `OnValue` calls an `Action<T>` and returns the original outcome (wraps a call to `Then`).
 
 info: Monadic Map and Bind are both overloads of `Then` function due to feedback from early users that found those terms confusing. 

### Sad path
Sad path means functions executed when an outcome (or result of `await AsyncOutcome`) does not hold a Problem value.
- `Rescue` calls `Func<Problem,Outcome<T>>` which can recover from a problem and switch the outcome to the happy path.
- `OnProblem` calls an `Action<Problem>` and returns the original outcome (wraps a call to `Rescue`).

## Value cardinalty on the happy path
Overloads supports outcomes that hold 0, 1, 2, 3, or 4 values. This determines the number of inputs to the function delegates for `Then`, `Ensure`, and `OnValue`
- 1 value: Outcome<T> or AsyncOutcome<T>
- 0 values: Outcome<None> or AsyncOutcome<None>
- 2 values: Outcome<(T, T1)> or AsyncOutcome<(T, T1)>  
- 3 values: Outcome<(T, T1, T2)> or AsyncOutcome<(T, T1, T2)>
- 4 values: Outcome<(T, T1, T2, T3)> or AsyncOutcome<(T, T1, T2, T3)>

For the Outcomes that hold a tuple value, the overloads allow spreading the input parameters. Example:
- Map format: Outcome<(T,T1)>.Then<TNext>(Func<T, T1, TNext> function)
- Bind format: Outcome<(T,T1)>.Then<TNext>(Func<T, T1, Outcome<TNext>> function)

The sad path `Rescue` and `OnProblem` functions are not overloaded the same way, as they only ever take a single parameter of type `Problem`.

### Special case for no-value (value-less) Outcomes
Outcome<None> inherits extensions for Outcome<T> since `None` is a concrete type closing generic Outcome<T>.
A lambda like `(None value => myNewValue)` while available, is also inelegant. The user would typically use a discard like `(_ => myNewValue)`.
So we always provide a parameterless overload `() => myNewValue`
And for the Then and Ensure function, also provide non-delegate overloads:
- Then<TNext>(TNext constantValue)
- Then<TNext>(Outcome<TNext> constantValue)
- Ensure(Outcome<None> constantValue)
which are the only composition overloads that don't take a delegate.

 Ensure function on a value-less Outcome doesn't validate a value since there isn't one, but it's provided for the sake of completion (it can also be used to chain together and short-circuit on the first problem in  a sequence of `Outcome<None>`).

## Sync composition checklist
Outcome extensions file name format: `{xxx}Extensions.cs where {xxx}` is the composition method name like `Then`, `Ensure` etc.
AsyncOutcome extensions filename format: `Async{xxx}Extensions.cs`. The Async prefix denotes that the AsyncOutcome is being extended, but the delegate is NOT asynchronous (does not returns an async type like Task or ValueTask).

- (Async)Outcome<None>.Then<TNext>(TNext)
- (Async)Outcome<None>.Then<TNext>(Outcome<TNext>)
- (Async)Outcome<None>.Then<TNext>(Func<TNext>)
- (Async)Outcome<None>.Then<TNext>(Func<Outcome<TNext>>)
- (Async)Outcome<T>.Then<TNext>(Func<T, TNext>)
- (Async)Outcome<T>.Then<TNext>(Func<T, Outcome<TNext>>)
- (Async)Outcome<(T,T1)>.Then<TNext>(Func<T, T1, TNext>)
- (Async)Outcome<(T,T1)>.Then<TNext>(Func<T, T1, Outcome<TNext>>)
- (Async)Outcome<(T,T1,T2)>.Then<TNext>(Func<T, T1, T2, TNext>)
- (Async)Outcome<(T,T1,T2)>.Then<TNext>(Func<T, T1, T2, Outcome<TNext>>)
- (Async)Outcome<(T,T1,T2,T3)>.Then<TNext>(Func<T, T1, T2, T3, TNext>)
- (Async)Outcome<(T,T1,T2,T3)>.Then<TNext>(Func<T, T1, T2, T3, Outcome<TNext>>)
- (Async)Outcome<None>.Ensure(Outcome<None>)
- (Async)Outcome<None>.Ensure(Func<Outcome<None>>)
- (Async)Outcome<T>.Ensure(Func<T, Outcome<None>>)
- (Async)Outcome<(T,T1)>.Ensure(Func<T, T1, Outcome<None>>)
- (Async)Outcome<(T,T1,T2)>.Ensure(Func<T, T1, T2, Outcome<None>>)
- (Async)Outcome<(T,T1,T2,T3)>.Ensure(Func<T, T1, T2, T3, Outcome<None>>)
- (Async)Outcome<None>.OnValue(Action)
- (Async)Outcome<T>.OnValue(Action<T>)
- (Async)Outcome<(T,T1)>.OnValue(Action<T, T1>)
- (Async)Outcome<(T,T1,T2)>.OnValue(Action<T, T1, T2>)
- (Async)Outcome<(T,T1,T2,T3)>.OnValue(Action<T, T1, T2, T3>)
- (Async)Outcome<T>.Rescue(Func<Problem, Outcome<T>>)
- (Async)Outcome<T>.OnProblem(Action<Problem>)

Total: 25 sync extensions.

## Async extensions
- Async versions of the sync composition extensions extend ONLY the AsyncOutcome type.
- Async delegate extensions filename format: `{xxx}AsyncExtensions.cs` where `xxx` is the composition method name like `Then`, `Ensure` etc.
- For each sync composition operator that takes a Func or Action delegate (22 out of 25), two async versions are provided:
  - delegate returning a Task or Task<T> (extension name is `{zzz}Async` where `zzz` is the sync composition method name like `Then`, `Ensure` etc.)
  - delegate returning a ValueTask or ValueTask<T>.
- In both cases, a CancellationToken is provided as the last input parameter.
- An AsyncOutcome is a struct containing the `CancellationToken` that gets passed, and a `ValueTask<Outcome<T>>>`

Total: 44 async extensions (22 ValueTask versions + 22 Task versions)

### async version examples (shows ValueTask variants, non-exhaustive)
| operation | original sync version | async verson |
|--|--|--|
| AsyncOutcome<T>.ThenAsync<TNext> | Func<T, TNext> | Func<T, CancellationToken, ValueTask<TNext>> |
| AsyncOutcome<T>.ThenAsync<TNext> | Func<T, Outcome<TNext>> | Func<T, CancellationToken, ValueTask<Outcome<TNext>>> |
| AsyncOutcome<T>.OnValueAsync | Action<T> | Func<T, CancellationToken, ValueTask> |
| AsyncOutcome<None>.OnValueAsync | Action | Func<CancellationToken, ValueTask> |

## Extension documentation
The structured function comments that drive the Xml documentation are carefully worded to be reused across overloads where possible. Almost all overloads just take a delegate. The delegate changes across overloads but the description of the function behavour is consistent, so overloads use <inheritdoc ... /> and keep the files smaller.
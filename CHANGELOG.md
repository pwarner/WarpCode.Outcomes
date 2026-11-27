# Changelog

All notable changes to WarpCode.Outcomes are documented here.
The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/), and the project follows [Semantic Versioning](https://semver.org/).

## [Unreleased]

## [2.0.0] - 2026-11-27

A ground-up redesign of composition. Async pipelines get a first-class `AsyncOutcome<T>` type with cancellation, every operator gets consistent, symmetrical overloads, and problems become records.
This is a breaking release: see [Migrating from 1.x](#migrating-from-1x) below.

### Added
- **`AsyncOutcome<T>`**: an awaitable wrapper around a `ValueTask<Outcome<T>>` that carries a `CancellationToken` through the whole pipeline.
  - Start one with `outcome.AsAsync(cancellationToken)`, `AsyncOutcome.Of`, `AsyncOutcome.OfProblem` or `AsyncOutcome.Ok`.
  - `await` it to get the `Outcome<T>`, or resolve it with `Match`.
  - It converts implicitly to `ValueTask<Outcome<T>>` and `Task<Outcome<T>>`, so it can be returned from methods with either signature.
- **Five composition operators** on both `Outcome<T>` and `AsyncOutcome<T>`, with the same signatures on each:
  - `Then`: map (`Func<T, TNext>`) and bind (`Func<T, Outcome<TNext>>`).
  - `Ensure`: validate with a `Func<T, Outcome<None>>` and keep the value.
  - `OnValue`: side effects on the happy path.
  - `Rescue`: recover from a problem with a `Func<Problem, Outcome<T>>`.
  - `OnProblem`: side effects on the sad path.
- **Async delegates** for every operator on `AsyncOutcome<T>`, each in two flavours that receive the pipeline's `CancellationToken` as their last parameter:
  - `ThenAsync`, `EnsureAsync`, `OnValueAsync`, `RescueAsync` and `OnProblemAsync` take `ValueTask`-returning delegates.
  - `ThenTask`, `EnsureTask`, `OnValueTask`, `RescueTask` and `OnProblemTask` take `Task`-returning delegates.
- **Tuple parameter spreading**: on an outcome of a tuple of up to four items, the happy-path operators accept delegates that take the items as separate parameters.
- **Value-less overloads** for `Outcome<None>`, so steps don't need a `_` discard. These include `Then(constantValue)`, `Then(existingOutcome)` and `Ensure(existingOutcome)`.
- `Outcome.OfProblem(string detail)` and `Outcome.OfProblem<T>(string detail)` helpers, with matching `AsyncOutcome` factories.
- `Outcome<T>.ToString()` and `AsyncOutcome<T>.ToString()` for readable debugging. The async version reports whether the outcome is pending, completed, faulted or cancelled.
- A symbol package (`.snupkg`) and SourceLink, so you can step into the library's source while debugging.
- Rewritten documentation and runnable samples (`samples/Outcomes.Samples`), all set in one small coffee shop.

### Changed
- **Targets .NET 10 and .NET 11.** .NET 8 is no longer supported.
- **`Problem` is now a `record`**, and `IProblem` is gone. Derive your own problem types from `Problem`, e.g. `public sealed record OutOfStockProblem(string Drink) : Problem($"We're out of {Drink}.")`.
  - Problems now compare equal when their type and all their properties are equal. In 1.x only `Detail` was compared.
- **`ProblemAggregate` is now a sealed record** holding a `Problem[]` (was an `IReadOnlyList<IProblem>`).
- `Match` takes `onValue` and `onProblem` (was `onSuccess`), and passes a `Problem` (was `IProblem`).
- **`Ensure` takes a single `Func<T, Outcome<None>>`** validator, replacing the `Predicate<T>` plus problem-factory pair.
- **`Rescue` takes a single `Func<Problem, Outcome<T>>`**, replacing the `Predicate<IProblem>` plus value-factory pair. It can pass the problem on, swap it for another, or recover.
- `OnSuccess` is renamed to `OnValue`.
- `Aggregate` and `AggregateAsync` produce an array (`Outcome<T[]>`) instead of a `List<T>`.
- `ExceptionMap` and `ExceptionMap<TException>` return a `Problem` (was `IProblem`).
- The typed-map `ToOutcome` overloads throw `ArgumentNullException` for a `null` map; previously it was ignored. On `Func<T>` the typed map is no longer optional.
- On a `Task<T>`, `task.ToOutcome<SomeException>(map)` is now a compile error, where 1.x silently bound it to the `Task` overload and discarded the result. Give the lambda's parameter a type instead: `task.ToOutcome((SomeException e) => ...)`.

### Removed
- `IProblem`. Use `Problem` and records derived from it.
- The async extensions on `Task<Outcome<T>>` and `ValueTask<Outcome<T>>` (`ThenAsync`, `EnsureAsync`, `RescueAsync`, `OnSuccessAsync`, `OnProblemAsync` and `MatchAsync`). Compose on `AsyncOutcome<T>` instead.
- LINQ query syntax support (`Select` and `SelectMany`) on outcomes and async outcomes. Use `Then`.
- `Outcome.Problem(...)` and `Outcome.Problem<T>(...)`, and the `problem.ToOutcome()` and `problem.ToOutcome<T>()` extensions. Use `Outcome.OfProblem(...)`, or rely on the implicit conversion from a `Problem`.

### Migrating from 1.x

| 1.x | 2.0 |
|---|---|
| `IProblem` | `Problem`, a record you can derive from |
| `Outcome.Problem<T>(problem)` or `problem.ToOutcome<T>()` | `Outcome.OfProblem<T>(problem)`, or return the `Problem` directly |
| `outcome.Match(onSuccess: …, onProblem: …)` | `outcome.Match(onValue: …, onProblem: …)` |
| `outcome.OnSuccess(v => …)` | `outcome.OnValue(v => …)` |
| `outcome.Ensure(v => v.IsValid, v => new Problem("…"))` | `outcome.Ensure(v => v.IsValid ? Outcome.Ok : new Problem("…"))` |
| `outcome.Rescue(p => p is NotFound, p => fallback)` | `outcome.Rescue(p => p is NotFound ? fallback : p)` |
| `outcome.Then(validate)` where `validate` returns `Outcome<None>` | `outcome.Ensure(validate)`. See the warning below. |
| `from x in outcome select …` | `outcome.Then(x => …)` |
| `Outcome<List<T>>` from `Aggregate` | `Outcome<T[]>` |
| `await outcome.ThenAsync(LoadAsync)` (on `Outcome` or `Task<Outcome<T>>`) | `outcome.AsAsync(ct).ThenTask((v, ct) => LoadAsync(v, ct))` |
| a method returning `Task<Outcome<T>>` as the first async step | `AsyncOutcome.Ok(ct).ThenTask(ct => LoadAsync(ct))` |
| `await task.MatchAsync(…)` | `await asyncOutcome.Match(…)`, or `(await task).Match(…)` |

> [!WARNING]
> In 1.x, `Then` with a `Func<T, Outcome<None>>` validated and **kept** the value. In 2.0 that call is an ordinary bind, so the result becomes an `Outcome<None>` and the value is lost. Usually the compiler will catch this, because the type no longer fits the next step, but not always. Search for validators passed to `Then` and switch them to `Ensure`.

## [1.0.0] - 2025-12-05

The first stable release.

- `Outcome<T>`, a value-or-problem union type, with `Outcome<None>` for operations that produce no value.
- Problems as `IProblem`, with `Problem` and `ProblemAggregate` implementations.
- Composition with `Then`, `Ensure`, `Rescue`, `OnSuccess` and `OnProblem`, plus async versions on `Task<Outcome<T>>` and `ValueTask<Outcome<T>>`, and LINQ query syntax support.
- `Aggregate` and `AggregateAsync` for combining many outcomes, with an option to stop at the first problem.
- `Adapt`: wraps exception-throwing functions, actions and tasks, turning mapped exceptions into problems.
- Targets .NET 8.

[Unreleased]: https://github.com/pwarner/WarpCode.Outcomes/compare/2.0.0...HEAD
[2.0.0]: https://github.com/pwarner/WarpCode.Outcomes/compare/1.0.0...2.0.0
[1.0.0]: https://github.com/pwarner/WarpCode.Outcomes/releases/tag/1.0.0

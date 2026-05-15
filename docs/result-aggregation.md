## Result Aggregation
There are two aggregation overloads, and two async overloads that work on an `IAsyncEnumerable<Result<T>>`

### Aggregate on `IEnumerable<Result<T>>`
Aggregate will return an `Result<TList<T>>`. 

It will be a success result containing a `List<T>` if all of the results in the sequence were success results.

If there was 1 or more problems and you pass true as the `bailEarly` parameter, 
it will return a problem result holding the first problem found in the sequence.

If you pass false (or omit the parameter which defaults to false), you will get a ProblemAggregate that contains a list of all the problems found.

```chsarp
IEnumerable<Result<TTemperature>> temps = GetTemperatures();

// bail early and return the first problem if one of the results has a problem
Result<TList<Temperature>> tempAggregate = temps.Aggregate(true); 

```

### Aggregate on `IEnumerable<Result<TNone>>`
Aggregate will return an `Result<TNone>`. 

Exhibits the same behaviour as before, except returns an `Result<TNone>` as none of the results in the sequence hold a useful value.

```chsarp
IEnumerable<Result<TTemperature>> temps = GetTemperatures();

// use default bail early setting of false, collect all the problems in a ProblemAggregate instance.
Result<TNone> tempAggregate = temps.Aggregate(); 
```

> `ProblemAggregate` is a type defined in the library that exposes a `Problems` property of type `IReadOnlyList<IProblem>`.

---
### Index
- [Why Results?](why-results.md)
- [What is a Problem?](what-is-a-problem.md)
- [Creating Results](creating-results.md)
- [Composing Results](composing-results.md)
- [Resolving Results](resolving-results.md)

### further reading / miscellaneous
- this: Result Aggregation
- [Adapting to Results](result-adaptation.md)
- [Results as Monads](results-as-monads.md)
## Outcome Aggregation
There are two aggregation overloads, and two async overloads that work on an `IAsyncEnumerable<Outcome<T>>`

### Aggregate on `IEnumerable<Outcome<T>>`
Aggregate will return an `Outcome<TList<T>>`. 

It will be a success outcome containing a `List<T>` if all of the outcomes in the sequence were success outcomes.

If there was 1 or more problems and you pass true as the `bailEarly` parameter, 
it will return a problem outcome holding the first problem found in the sequence.

If you pass false (or omit the parameter which defaults to false), you will get a ProblemAggregate that contains a list of all the problems found.

```chsarp
IEnumerable<Outcome<TTemperature>> temps = GetTemperatures();

// bail early and return the first problem if one of the outcomes has a problem
Outcome<TList<Temperature>> tempAggregate = temps.Aggregate(true); 

```

### Aggregate on `IEnumerable<Outcome<TNone>>`
Aggregate will return an `Outcome<TNone>`. 

Exhibits the same behaviour as before, except returns an `Outcome<TNone>` as none of the outcomes in the sequence hold a useful value.

```chsarp
IEnumerable<Outcome<TTemperature>> temps = GetTemperatures();

// use default bail early setting of false, collect all the problems in a ProblemAggregate instance.
Outcome<TNone> tempAggregate = temps.Aggregate(); 
```

> `ProblemAggregate` is a type defined in the library that exposes a `Problems` property of type `IReadOnlyList<IProblem>`.

---
### Index
- [Why Outcomes?](why-outcomes.md)
- [What is a Problem?](what-is-a-problem.md)
- [Creating Outcomes](creating-outcomes.md)
- [Composing Outcomes](composing-outcomes.md)
- [Resolving Outcomes](resolving-outcomes.md)

### further reading / miscellaneous
- this: Outcome Aggregation
- [Adapting to Outcomes](outcome-adaptation.md)
- [Outcomes as Monads](outcomes-as-monads.md)
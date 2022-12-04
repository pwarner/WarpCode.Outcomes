
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
Outcome<string> outcome = "success!";
Outcome<int> outcome2 = 13;
```

Create value-less Outcomes with `Outcome.NoProblem`
```csharp
Outcome<None> outcome = Outcome.NoProblem;
```

`Outcome<None>` also implicitly converts to other `Outcome<T>` types, so you can write:
```csharp
Outcome<string> outcome = Outcome.NoProblem; // value is default(string), or null
Outcome<int> outcome = Outcome.NoProblem; // value is default(int), or 0
```

## Creating Problem Outcomes

By constructor
```csharp
Outcome<string> outcome = new Outcome<string>(new SomeProblem());
```

By implicit coversion
```csharp
Outcome<string> outcome = new SomeProblem();
```

With `ToOutcome<T> extensions`
```csharp
Outcome<string> outcome = new SomeProblem().ToOutcome<Customer>();
```

Create value-less Outcomes with `ToOutcome()`
```csharp
Outcome<None> outcome = new SomeProblem().ToOutcome();
```

Again, since `Outcome<None>` implicitly converts to other `Outcome<T>` types, you can write:
```csharp
Outcome<bool> outcome = new SomeProblem().ToOutcome();
```
### Index
- [Why Outcomes?](../readme.md)
- [What is a Problem?](what-is-a-problem.md)
- this: Creating Outcomes
- [Composing Outcomes](composing-outcomes.md)
- [Adapting to Outcomes](outcome-adaptation.md)
- [Resolving Outcomes](resolving-outcomes.md)

# Creating Outcomes
As mentioned, Outcomes have exactly two states: either they hold a value (Successful Outcomes), or they hold a Problem (Problem Outcomes) 

Successful Outcomes can also hold an absence of value (The `None` type, which is like `System.Void` or `Unit`)

## Creating Successful Outcomes
By constructor
```csharp
var outcomeStr = new Outcome<string>("success!");
var outcomeInt = new Outcome<int>(13);
```

With `Outcome.Ok`
```csharp
Outcome<string> outcomeStr = Outcome.Ok("success!");
Outcome<int> outcomeInt = Outcome.Ok(13);

// create value-less Outcome
Outcome<None> outcomeVoid = Outcome.Ok();
```

By implicit coversion
```csharp
Outcome<string> outcomeStr = "success!";
Outcome<int> outcomeInt = 13;
Outcome<None> outcomeVoid = default;
```

`Outcome<None>` also implicitly converts to other `Outcome<T>` types, so you can write:
```csharp
Outcome<string> outcome = Outcome.Ok(); // value is default(string), or null
Outcome<int> outcome = Outcome.Ok(); // value is default(int), or 0
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
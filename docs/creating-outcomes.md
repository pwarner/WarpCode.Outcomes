
# Creating Outcomes
As mentioned, Outcomes have exactly two states: either they hold a value (`Success Outcomes`), or they hold a Problem (`Problem Outcomes`).

`Success Outcomes` can also hold an absence of meaningful value. Use `None` as the generic type, which is like `System.Void` or `Unit`

## Creating Successful Outcomes
By constructor
```csharp
var outcomeStr = new Outcome<string>("success!");
var outcomeInt = new Outcome<int>(13);
```

By implicit coversion
```csharp
Outcome<string> outcomeStr = "success!";
Outcome<int> outcomeInt = 13;
Outcome<None> outcomeVoid = default;
```

With `Outcome.Of(..)`
```csharp
Outcome<string> outcomeStr = Outcome.Of("success!");
Outcome<int> outcomeInt = Outcome.Of(13);
```

Value-less Outcome<None> with `Outcome.Ok`
```csharp
Outcome<None> outcomeVoid = Outcome.Ok;
```

## Creating Problem Outcomes

By constructor
```csharp
Outcome<string> outcome = new Outcome<string>(new SomeProblem());
```

By implicit coversion
```csharp
// available if the SomeProblem type derives from the base Outcomes.Problem type
Outcome<string> outcome = new SomeProblem();
```

With the generic `ToOutcome<T>()` extension.
```csharp
Outcome<string> outcome = new SomeProblem().ToOutcome<string>();
```

Value-less problem outcomes with the non-generic `ToOutcome()` extension.
```csharp
Outcome<None> outcome = new SomeProblem().ToOutcome();
```

---
### Index
- [Why Outcomes?](../readme.md)
- [What is a Problem?](what-is-a-problem.md)
- this: Creating Outcomes
- [Composing Outcomes](composing-outcomes.md)
- [Resolving Outcomes](resolving-outcomes.md)

### further reading / miscellaneous
- [Outcome Extensions](outcome-extensions.md)
- [Adapting to Outcomes](outcome-adaptation.md)
- [Outcomes as Monads](outcomes-as-monads.md)
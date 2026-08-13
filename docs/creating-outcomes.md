
# Creating Outcomes
As mentioned, Outcomes have exactly two states: either they hold a value (`Success Outcomes`), or they hold a Problem (`Problem Outcomes`).

You can consider succcess outcomes as the *'happy path'* of your code, and Problem Outcomes as the *'unhappy path'*.

Or if you really enjoy the railway orientated concept, you can call them the 'hale rail' and 'fail rail'.

`Success Outcomes` can also hold an absence of meaningful value. Use `None` as the generic type, which is like `System.Void` or `Unit`

## Creating Successful Outcomes
By constructor
```csharp
var outcomeStr = new Outcome<Tstring>("success!");
var outcomeInt = new Outcome<Tint>(13);
```

By implicit coversion
```csharp
Outcome<Tstring> outcomeStr = "success!";
Outcome<Tint> outcomeInt = 13;
Outcome<TNone> outcomeVoid = default;
```

With `Outcome.Of(..)`
```csharp
Outcome<Tstring> outcomeStr = Outcome.Of("success!");
Outcome<Tint> outcomeInt = Outcome.Of(13);
```

Value-less Outcome<TNone> with `Outcome.Ok`
```csharp
Outcome<TNone> outcomeVoid = Outcome.Ok;
```

## Creating Problem Outcomes

By constructor
```csharp
Outcome<Tstring> outcome = new Outcome<Tstring>(new SomeProblem());
```

By implicit coversion
```csharp
// available if the SomeProblem type derives from the base Outcomes.Problem type
Outcome<Tstring> outcome = new SomeProblem();
```
With `Outcome.OfProblem<T>(...)`
```csharp
Outcome<Tstring> outcome = Outcome.OfProblem<string>(new SomeProblem());
```

Valueless Problem Outcome with `Outcome.OfProblem(...)`
```csharp
Outcome<TNone> outcome = Outcome.OfProblem(new SomeProblem());
```

Create the basic `WarpCode.Outcomes.Problem` with just a string description
```csharp
Outcome<Customer> outcome = Outcome.OfProblem<Customer>("Something went wrong");
Outcome<TNone> outcome = Outcome.OfProblem("Something went wrong");
```

---
### Index
- [Why Outcomes?](why-outcomes.md)
- [What is a Problem?](what-is-a-problem.md)
- this: Creating Outcomes
- [Composing Outcomes](composing-outcomes.md)
- [Resolving Outcomes](resolving-outcomes.md)

### further reading / miscellaneous
- [Outcome Aggregation](outcome-aggregation.md)
- [Adapting to Outcomes](outcome-adaptation.md)
- [Outcomes as Monads](outcomes-as-monads.md)
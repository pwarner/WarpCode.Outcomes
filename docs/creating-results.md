
# Creating Results
As mentioned, Results have exactly two states: either they hold a value (`Success Results`), or they hold a Problem (`Problem Results`).

You can consider succcess results as the *'happy path'* of your code, and Problem Results as the *'unhappy path'*.

Or if you really enjoy the railway orientated concept, you can call them the 'hale rail' and 'fail rail'.

`Success Results` can also hold an absence of meaningful value. Use `None` as the generic type, which is like `System.Void` or `Unit`

## Creating Successful Results
By constructor
```csharp
var resultStr = new Result<Tstring>("success!");
var resultInt = new Result<Tint>(13);
```

By implicit coversion
```csharp
Result<Tstring> resultStr = "success!";
Result<Tint> resultInt = 13;
Result<TNone> resultVoid = default;
```

With `Result.Of(..)`
```csharp
Result<Tstring> resultStr = Result.Of("success!");
Result<Tint> resultInt = Result.Of(13);
```

Value-less Result<TNone> with `Outcome.Ok`
```csharp
Result<TNone> resultVoid = Outcome.Ok;
```

## Creating Problem Results

By constructor
```csharp
Result<Tstring> result = new Result<Tstring>(new SomeProblem());
```

By implicit coversion
```csharp
// available if the SomeProblem type derives from the base Results.Problem type
Result<Tstring> result = new SomeProblem();
```
With `Result.OfProblem<T>(...)`
```csharp
Result<Tstring> result = Result.OfProblem<string>(new SomeProblem());
```

Valueless Problem Outcome with `Result.OfProblem(...)`
```csharp
Result<TNone> result = Result.OfProblem(new SomeProblem());
```

Create the basic `WarpCode.Outcomes.Problem` with just a string description
```csharp
Result<Customer> result = Result.OfProblem<Customer>("Something went wrong");
Result<TNone> result = Result.OfProblem("Something went wrong");
```

---
### Index
- [Why Results?](why-results.md)
- [What is a Problem?](what-is-a-problem.md)
- this: Creating Results
- [Composing Results](composing-results.md)
- [Resolving Results](resolving-results.md)

### further reading / miscellaneous
- [Result Aggregation](result-extensions.md)
- [Adapting to Results](result-adaptation.md)
- [Results as Monads](results-as-monads.md)
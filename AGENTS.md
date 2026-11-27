## What this project is
- An open-source library, providing an efficient monadic Result-type called Outcome, currently targeting .NET 10 and .NET 11
- Outcomes are a fluent, compositional discriminated union type representing a value or a problem.
- Outcomes support R.O.P. flow control (Railway Orientated Programming) via composition with extension methods and **full async support**.
- Only types and methods that need to be public are public. Public types and methods must be documented. Classes are sealed until there is a reason not to.

## General
- Code style comes from .editorconfig, and its enforced rules are build errors (`EnforceCodeStyleInBuild` + `TreatWarningsAsErrors`). `dotnet format` fixes most of them.
- use latest compatible language syntax and a functional style where appropriate (for example: ternary expressions, switch expressions, lambda style method bodies)
- Performance is paramount. Where possible always minimise allocations. When using readonly structs (Outcome and AsyncOutcome are structs) be wary of scenarios where defensive copies will be made by the compiler.
- Public API documentation for consumers are markdown files stored in the docs folder. They are first-class citizens and should be kept up to date with any changes.
- Before adding or changing a composition operator, read `tooling/composition-architecture.md`.

## Docs and samples
- Every code excerpt in `docs/` is copied verbatim from a compiling sample in `samples/Outcomes.Samples`. When you change one, change the other, and run `dotnet run --project samples/Outcomes.Samples` to check the output still makes sense.
- Samples share one coffee-shop domain (`CoffeeShop.cs`) and have no dependencies beyond the library; `Console.WriteLine` shows their results.
- Docs use a light touch of emoji: one per heading, plus GitHub alerts (`> [!TIP]`, `> [!NOTE]`, `> [!WARNING]`, `> [!CAUTION]`) for callouts.

## Tests
- Aim for 100% coverage of all public facing methods and types. Measure it with `dotnet test --coverage --coverage-output-format cobertura` (reports land in `TestResults/`, one per target framework).
- Tests are found in `.\tests\Outcomes.Tests\Outcomes.Tests.csproj`. Test files are named after the class they are testing, with a suffix of `Tests.cs`. 
- Where appropriate, use `[Theory]` attributes over `[Fact]` to test multiple scenarios with a single test method where only the test inputs and outputs vary but behaviour is the same.
- Tests run on the Microsoft Testing Platform, so the VSTest `--filter` syntax doesn't work. To run one test class: `dotnet test --project tests/Outcomes.Tests --filter-class "WarpCode.Outcomes.Tests.ThenExtensionsTests"` (add `-f net10.0` for a single target framework).

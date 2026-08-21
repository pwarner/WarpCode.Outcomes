## What this project is
- An open-source library, providing an efficient monadic Result-type called Outcome, currently targeting .Net 8 and .NET 10
- Outcomes are a fluent, compositonal discriminated union type representing a value or a problem.
- Outcomes support R.O.P. flow control (Railway Orientated Programming) via composition with extension methods and **full async support**.
- Only types and methods that need to be public are public. Public types and methods must be documented. Classes are sealed until there is a reason not to.

## General
- General code style rules can be cleaned from .editorconfig
- use latest compatible language syntax and a functional style where appropriate (for example: ternary expressions, switch expressions, lambda syle method bodies)
- Performance is paramount. Where possible always minimise allocations. When using readonly structs (Outcome and AsyncOutcome are structs) be wary of scenarios where defensive copies will be made by the compiler.
- File structure is flat to refect a simple namespace of `Warpcode.Outcomes`. 
- Public API documentation for consumers are markdown files stored in the docs folder. They are first-class citizens and should be kept up to date with any changes.

## Map
| When you need to ... | Go to ... |
|--|--|
| understand user documentation | docs/why-outcomes.md |
| understand composition design | tooling/composition-architecture.md |

## Tests
- Using xUnit.v3 and the Microsoft Test Platform for performance. 
- Aim for 100% coverage of all public facing methods and types. 
- Tests are found in `.\tests\Outcomes.Tests\Outcomes.Tests.csproj`. Test files are named after the class they are testing, with a suffix of `Tests.cs`. 
- Where appropriate, use `[Theory]` attributes over `[Fact]` to test multiple scenarios with a single test method where only the test inputs and outputs vary but behaviour is the same.

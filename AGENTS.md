## What this project is
- An open-source library, providing an efficient monadic Result-type called Outcome, currently targeting .Net 8 and .NET 10
- Outcomes are a fluent, compositonal discriminated union type representing a value or a problem.
- Outcomes support R.O.P. flow control (Railway Orientated Programming) via composition with extension methods and **full async support**.
- Only types and methods that need to be public are public. Public types and methods must be documented. Classes are sealed until there is a reason not to.

## Code style and rules
- General code style rules can be cleaned from .editorconfig
- Only use braces for code blocks.
- Namespaces are file-scoped. When a using directive appears in more than one file it's added to a global usings file.
- Always prefer latest language syntax and a functional style where appropriate (for example: ternary expressions, switch expressions, lambda syle method bodies)
- Performance is important. Where possible always minimise allocations by using readonly structs (Outcome and AsyncOutcome are structs) and we avoid scenarios where defensive copies could be made.
- AsyncOutcome composition methods support both Task and ValueTask variants.
- File structure is flat to refect a simple namespace of `Warpcode.Outcomes`. Filenames always clearly confer the meaning of the file.

## Documentation
Docs are stored in the docs folder. Public facing markdown files for consumers of the package. They are first-class citizens and should be kept up to date with any changes to the library.

## Tests
- Using xUnit.v3 and the Microsoft Test Platform for performance. 
- We want 100% coverage of all public facing methods and types. 
- Tests are stored in the tests folder. Test files are named after the class they are testing, with a suffix of `.Tests.cs`. 
- Where appropriate, use `[Theory]` attributes over `[Fact]` to test multiple scenarios with a single test method where only the test inputs and outputs vary but behaviour is the same.
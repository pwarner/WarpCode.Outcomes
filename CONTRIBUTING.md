# Contributing to Outcomes

Thank you for considering contributing to the Outcomes project! Here are some guidelines to help you get started.

## Getting set up
You'll need the **.NET 11 SDK**, which builds and tests both targets (.NET 10 and .NET 11). If you also want to run the .NET 10 tests locally, you'll need the .NET 10 runtime too.

```shell
dotnet build                                   # warnings are errors, and code style is enforced
dotnet test                                    # runs the tests on both target frameworks
dotnet run --project samples/Outcomes.Samples  # runs the samples behind the docs
```

The tests use xUnit on the Microsoft Testing Platform, so filter with `--filter-class` / `--filter-method` rather than VSTest's `--filter`:

```shell
dotnet test --project tests/Outcomes.Tests --filter-class "WarpCode.Outcomes.Tests.ThenExtensionsTests"
```

## How to Contribute
1. Create a new branch from `main` for your feature or bug fix.
2. Keep the build clean. Code style comes from `.editorconfig`, and its rules fail the build, so run `dotnet format` to fix most of them automatically.
3. Write tests for your changes. All features and fixed bugs must have tests, and coverage of the library is kept at 100% (`dotnet test --coverage --coverage-output-format cobertura` writes a report to `TestResults/`).
4. Keep the docs in step. The [docs](docs) are first-class: every code excerpt in them is copied verbatim from a compiling sample in [samples/Outcomes.Samples](samples/Outcomes.Samples), so if you change one, change the other.
5. Add a line describing your change under **[Unreleased]** in [CHANGELOG.md](CHANGELOG.md).
6. Write clear, concise commit messages, and submit a pull request with a detailed description of your changes. CI builds, tests and checks formatting on every pull request.

If you're changing or adding a composition operator (`Then`, `Ensure`, `OnValue`, `Rescue`, `OnProblem`), read [tooling/composition-architecture.md](tooling/composition-architecture.md) first. It explains the overload conventions and a couple of C# 14 extension-member pitfalls.

## Code of Conduct
Follow the [Contributor Covenant](https://www.contributor-covenant.org), version 2.1.
By participating in this project, you agree to abide by its terms.

## Reporting Issues
If you encounter a bug or have a feature request, please open an issue on GitHub.

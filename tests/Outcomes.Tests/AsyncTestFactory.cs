namespace WarpCode.Outcomes.Tests;

/// <summary>
/// Test factory that mirrors <see cref="AsyncOutcome"/> entry points but supplies the ambient
/// <c>TestContext.Current.CancellationToken</c>, keeping async test bodies free of token noise
/// while satisfying xUnit1051.
/// </summary>
internal static class Async
{
    private static CancellationToken Ct => TestContext.Current.CancellationToken;

    public static AsyncOutcome<None> Ok() => AsyncOutcome.Ok(Ct);

    public static AsyncOutcome<T> Of<T>(T value) => AsyncOutcome.Of(value, Ct);

    public static AsyncOutcome<T> OfProblem<T>(Problem problem) => AsyncOutcome.OfProblem<T>(problem, Ct);

    public static AsyncOutcome<None> OfProblem(Problem problem) => AsyncOutcome.OfProblem(problem, Ct);
}

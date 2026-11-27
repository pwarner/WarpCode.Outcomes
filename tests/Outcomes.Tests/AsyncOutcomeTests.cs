namespace WarpCode.Outcomes.Tests;

public class AsyncOutcomeTests
{
    private static readonly Problem TestProblem = new(nameof(TestProblem));

    private static CancellationToken Ct => TestContext.Current.CancellationToken;

    [Fact]
    public void ParameterlessConstructor_ShouldThrow()
        => Assert.Throws<InvalidOperationException>(() => { _ = new AsyncOutcome<int>(); });

    [Fact]
    public async Task OfProblem_ShouldCreateProblemOutcome_FromDetail()
        => Assert.Equal(Outcome.OfProblem<int>(TestProblem.Detail), await AsyncOutcome.OfProblem<int>(TestProblem.Detail, Ct));

    [Fact]
    public async Task OfProblem_ShouldCreateValuelessProblemOutcome_FromDetail()
        => Assert.Equal(Outcome.OfProblem(TestProblem.Detail), await AsyncOutcome.OfProblem(TestProblem.Detail, Ct));

    [Fact]
    public async Task Match_ShouldResolveWithOnValueFunction_WhenValue()
        => Assert.Equal("value 10", await Async.Of(10).Match(v => $"value {v}", p => p.Detail));

    [Fact]
    public async Task Match_ShouldResolveWithOnProblemFunction_WhenProblem()
        => Assert.Equal(TestProblem.Detail, await Async.OfProblem<int>(TestProblem).Match(v => $"value {v}", p => p.Detail));

    [Fact]
    public async Task ImplicitConversion_ShouldExposeTheWrappedValueTask()
    {
        ValueTask<Outcome<int>> task = Async.Of(10);

        Assert.Equal(Outcome.Of(10), await task);
    }

    [Fact]
    public async Task ImplicitConversion_ShouldExposeTheWrappedOutcomeAsTask()
    {
        Task<Outcome<int>> task = Async.Of(10);

        Assert.Equal(Outcome.Of(10), await task);
    }

    // ----- ToString reflects the state of the underlying task -----

    [Fact]
    public void ToString_ShouldDescribeCompletedOutcome()
        => Assert.Equal("Completed Outcome<System.Int32>: Value: 10", Async.Of(10).ToString());

    [Fact]
    public void ToString_ShouldDescribeCancelledOutcome_WhenTokenIsCancelled()
    {
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        Assert.Equal("Cancelled AsyncOutcome<System.Int32>", AsyncOutcome.Of(10, cts.Token).ToString());
    }

    [Fact]
    public async Task ToString_ShouldDescribePendingOutcome_UntilItCompletes()
    {
        var pending = new TaskCompletionSource<int>();
        var outcome = Async.Of(10).ThenTask((_, _) => pending.Task);

        Assert.Equal("Pending AsyncOutcome<System.Int32>", outcome.ToString());

        pending.SetResult(20);
        Assert.Equal(Outcome.Of(20), await outcome);
    }

    [Fact]
    public async Task ToString_ShouldDescribeFaultedOutcome_WhenDelegateThrows()
    {
        static Task<int> Throw(int value, CancellationToken cancellationToken) => throw new InvalidOperationException();
        var outcome = Async.Of(10).ThenTask(Throw);

        Assert.Equal("Faulted Outcome<System.Int32>", outcome.ToString());
        await Assert.ThrowsAsync<InvalidOperationException>(async () => await outcome);
    }
}

namespace WarpCode.Outcomes.Tests;

public class RescueAsyncExtensionsTests
{
    private static readonly Problem TestProblem = new(nameof(TestProblem));
    private static readonly Problem RescueProblem = new(nameof(RescueProblem));

    private static Task<Outcome<int>> RecoverTask(Problem p, CancellationToken ct) => Task.FromResult(Outcome.Of(10));

    // ----- RescueAsync (ValueTask) -----

    [Fact]
    public async Task RescueAsync_ShouldReturnOriginal_WhenSuccess()
        => Assert.Equal(Outcome.Of(10), await Async.Of(10).RescueAsync((_, _) => ValueTask.FromResult(Outcome.Of(99))));

    [Fact]
    public async Task RescueAsync_ShouldRecoverToValue_WhenProblem()
        => Assert.Equal(Outcome.Of(10), await Async.OfProblem<int>(TestProblem).RescueAsync((_, _) => ValueTask.FromResult(Outcome.Of(10))));

    [Fact]
    public async Task RescueAsync_ShouldPropagateNewProblem_WhenRescueFails()
        => Assert.Equal(Outcome.OfProblem<int>(RescueProblem), await Async.OfProblem<int>(TestProblem).RescueAsync((_, _) => ValueTask.FromResult(Outcome.OfProblem<int>(RescueProblem))));

    [Fact]
    public async Task RescueAsync_ShouldPropagateSameProblem_WhenProblemIsUnrecoverable()
        => Assert.Equal(Outcome.OfProblem<int>(TestProblem), await Async.OfProblem<int>(TestProblem).RescueAsync((p, _) => ValueTask.FromResult(Outcome.OfProblem<int>(p))));

    // ----- RescueTask (Task) -----

    [Fact]
    public async Task RescueTask_ShouldReturnOriginal_WhenSuccess()
        => Assert.Equal(Outcome.Of(10), await Async.Of(10).RescueTask((_, _) => Task.FromResult(Outcome.Of(99))));

    [Fact]
    public async Task RescueTask_ShouldRecoverToValue_WhenProblem()
        => Assert.Equal(Outcome.Of(10), await Async.OfProblem<int>(TestProblem).RescueTask(RecoverTask));

    [Fact]
    public async Task RescueTask_ShouldPropagateNewProblem_WhenRescueFails()
        => Assert.Equal(Outcome.OfProblem<int>(RescueProblem), await Async.OfProblem<int>(TestProblem).RescueTask((_, _) => Task.FromResult(Outcome.OfProblem<int>(RescueProblem))));

    [Fact]
    public async Task RescueTask_ShouldPropagateSameProblem_WhenProblemIsUnrecoverable()
        => Assert.Equal(Outcome.OfProblem<int>(TestProblem), await Async.OfProblem<int>(TestProblem).RescueTask((p, _) => Task.FromResult(Outcome.OfProblem<int>(p))));

    // ----- CancellationToken flow -----

    [Fact]
    public async Task RescueAsync_ShouldPassOutcomeToken_ToDelegate()
    {
        using var cts = new CancellationTokenSource();
        CancellationToken seen = default;

        await AsyncOutcome.OfProblem<int>(TestProblem, cts.Token).RescueAsync((_, ct) => { seen = ct; return ValueTask.FromResult(Outcome.Of(10)); });

        Assert.Equal(cts.Token, seen);
    }

    [Fact]
    public async Task RescueTask_ShouldPassOutcomeToken_ToDelegate()
    {
        using var cts = new CancellationTokenSource();
        CancellationToken seen = default;

        await AsyncOutcome.OfProblem<int>(TestProblem, cts.Token).RescueTask((_, ct) => { seen = ct; return Task.FromResult(Outcome.Of(10)); });

        Assert.Equal(cts.Token, seen);
    }
}

namespace WarpCode.Outcomes.Tests;

public class RescueAsyncExtensionsTests
{
    private static readonly Problem TestProblem = new(nameof(TestProblem));
    private static readonly Problem RescueProblem = new(nameof(RescueProblem));

    private static Task<Outcome<int>> RecoverTask(Problem p, CancellationToken ct) => Task.FromResult(Outcome.Of(10));

    [Fact]
    public async Task RescueAsync_ShouldRecoverToValue_WhenProblem()
        => Assert.Equal(Outcome.Of(10), await Async.OfProblem<int>(TestProblem).RescueAsync((_, _) => ValueTask.FromResult(Outcome.Of(10))));

    [Fact]
    public async Task RescueTask_ShouldRecoverToValue_WhenProblem()
        => Assert.Equal(Outcome.Of(10), await Async.OfProblem<int>(TestProblem).RescueTask(RecoverTask));

    [Fact]
    public async Task RescueAsync_ShouldReceiveProblem_WhenProblem()
    {
        Problem? seen = null;

        await Async.OfProblem<int>(TestProblem).RescueAsync((p, _) => { seen = p; return ValueTask.FromResult(Outcome.Of(10)); });

        Assert.Equal(TestProblem, seen);
    }

    [Fact]
    public async Task RescueAsync_ShouldPropagateNewProblem_WhenRescueFails()
        => Assert.Equal(Outcome.OfProblem<int>(RescueProblem), await Async.OfProblem<int>(TestProblem).RescueAsync((_, _) => ValueTask.FromResult(Outcome.OfProblem<int>(RescueProblem))));

    [Fact]
    public async Task RescueAsync_ShouldPropagateSameProblem_WhenProblemIsUnrecoverable()
        => Assert.Equal(Outcome.OfProblem<int>(TestProblem), await Async.OfProblem<int>(TestProblem).RescueAsync((p, _) => ValueTask.FromResult(Outcome.OfProblem<int>(p))));

    [Fact]
    public async Task RescueAsync_ShouldNotInvokeAndReturnOriginal_WhenSuccess()
    {
        var invoked = false;

        var actual = await Async.Of(10).RescueAsync((_, _) => { invoked = true; return ValueTask.FromResult(Outcome.Of(99)); });

        Assert.Equal(Outcome.Of(10), actual);
        Assert.False(invoked);
    }

    [Fact]
    public async Task RescueTask_ShouldNotInvokeAndReturnOriginal_WhenSuccess()
    {
        var invoked = false;

        var actual = await Async.Of(10).RescueTask((_, _) => { invoked = true; return Task.FromResult(Outcome.Of(99)); });

        Assert.Equal(Outcome.Of(10), actual);
        Assert.False(invoked);
    }

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

namespace WarpCode.Outcomes.Tests;

public class OnProblemAsyncExtensionsTests
{
    private static readonly Problem TestProblem = new(nameof(TestProblem));

    [Fact]
    public async Task OnProblemAsync_ShouldInvokeWithProblemAndReturnOriginal_WhenProblem()
    {
        Problem? seen = null;

        var actual = await Async.OfProblem<int>(TestProblem).OnProblemAsync((p, _) => { seen = p; return ValueTask.CompletedTask; });

        Assert.Equal(Outcome.OfProblem<int>(TestProblem), actual);
        Assert.Equal(TestProblem, seen);
    }

    [Fact]
    public async Task OnProblemTask_ShouldInvokeWithProblemAndReturnOriginal_WhenProblem()
    {
        Problem? seen = null;

        var actual = await Async.OfProblem<int>(TestProblem).OnProblemTask((p, _) => { seen = p; return Task.CompletedTask; });

        Assert.Equal(Outcome.OfProblem<int>(TestProblem), actual);
        Assert.Equal(TestProblem, seen);
    }

    [Fact]
    public async Task OnProblemAsync_ShouldNotInvokeAndReturnOriginal_WhenSuccess()
    {
        var invoked = false;

        var actual = await Async.Of(10).OnProblemAsync((_, _) => { invoked = true; return ValueTask.CompletedTask; });

        Assert.Equal(Outcome.Of(10), actual);
        Assert.False(invoked);
    }

    [Fact]
    public async Task OnProblemTask_ShouldNotInvokeAndReturnOriginal_WhenSuccess()
    {
        var invoked = false;

        var actual = await Async.Of(10).OnProblemTask((_, _) => { invoked = true; return Task.CompletedTask; });

        Assert.Equal(Outcome.Of(10), actual);
        Assert.False(invoked);
    }

    // ----- CancellationToken flow -----

    [Fact]
    public async Task OnProblemAsync_ShouldPassOutcomeToken_ToDelegate()
    {
        using var cts = new CancellationTokenSource();
        CancellationToken seen = default;

        await AsyncOutcome.OfProblem<int>(TestProblem, cts.Token).OnProblemAsync((_, ct) => { seen = ct; return ValueTask.CompletedTask; });

        Assert.Equal(cts.Token, seen);
    }

    [Fact]
    public async Task OnProblemTask_ShouldPassOutcomeToken_ToDelegate()
    {
        using var cts = new CancellationTokenSource();
        CancellationToken seen = default;

        await AsyncOutcome.OfProblem<int>(TestProblem, cts.Token).OnProblemTask((_, ct) => { seen = ct; return Task.CompletedTask; });

        Assert.Equal(cts.Token, seen);
    }
}

namespace WarpCode.Outcomes.Tests;

public class ThenAsyncExtensionsTests
{
    private static readonly Problem TestProblem = new(nameof(TestProblem));

    private static Task<int> DoubleTask(int v, CancellationToken ct) => Task.FromResult(v * 2);

    // ----- AsyncOutcome<None> — happy path, all four delegate shapes -----

    [Fact]
    public async Task ThenAsync_None_ValueTaskValue_ShouldProjectValue_WhenSuccess()
        => Assert.Equal(Outcome.Of(5), await Async.Ok().ThenAsync((_) => ValueTask.FromResult(5)));

    [Fact]
    public async Task ThenTask_None_TaskValue_ShouldProjectValue_WhenSuccess()
        => Assert.Equal(Outcome.Of(5), await Async.Ok().ThenTask((_) => Task.FromResult(5)));

    [Fact]
    public async Task ThenAsync_None_ValueTaskOutcome_ShouldProduceOutcome_WhenSuccess()
        => Assert.Equal(Outcome.Of(5), await Async.Ok().ThenAsync((_) => ValueTask.FromResult(Outcome.Of(5))));

    [Fact]
    public async Task ThenTask_None_TaskOutcome_ShouldProduceOutcome_WhenSuccess()
        => Assert.Equal(Outcome.Of(5), await Async.Ok().ThenTask((_) => Task.FromResult(Outcome.Of(5))));

    [Fact]
    public async Task ThenAsync_None_ShouldPropagateInnerProblem_WhenDelegateReturnsProblem()
        => Assert.Equal(Outcome.OfProblem<int>(TestProblem), await Async.Ok().ThenAsync((_) => ValueTask.FromResult(Outcome.OfProblem<int>(TestProblem))));

    [Fact]
    public async Task ThenAsync_None_ShouldNotInvoke_WhenProblem()
    {
        var invoked = false;

        var actual = await Async.OfProblem(TestProblem).ThenAsync((_) => { invoked = true; return ValueTask.FromResult(5); });

        Assert.Equal(Outcome.OfProblem<int>(TestProblem), actual);
        Assert.False(invoked);
    }

    // ----- AsyncOutcome<T> — happy path, all four delegate shapes -----

    [Fact]
    public async Task ThenAsync_ValueTaskValue_ShouldProjectValue_WhenSuccess()
        => Assert.Equal(Outcome.Of(20), await Async.Of(10).ThenAsync((v, _) => ValueTask.FromResult(v * 2)));

    [Fact]
    public async Task ThenTask_TaskValue_ShouldProjectValue_WhenSuccess()
        => Assert.Equal(Outcome.Of(20), await Async.Of(10).ThenTask((v, _) => Task.FromResult(v * 2)));

    [Fact]
    public async Task ThenAsync_ValueTaskOutcome_ShouldProduceOutcome_WhenSuccess()
        => Assert.Equal(Outcome.Of(20), await Async.Of(10).ThenAsync((v, _) => ValueTask.FromResult(Outcome.Of(v * 2))));

    [Fact]
    public async Task ThenTask_TaskOutcome_ShouldProduceOutcome_WhenSuccess()
        => Assert.Equal(Outcome.Of(20), await Async.Of(10).ThenTask((v, _) => Task.FromResult(Outcome.Of(v * 2))));

    [Fact]
    public async Task ThenAsync_ShouldPropagateInnerProblem_WhenDelegateReturnsProblem()
        => Assert.Equal(Outcome.OfProblem<int>(TestProblem), await Async.Of(10).ThenAsync((_, _) => ValueTask.FromResult(Outcome.OfProblem<int>(TestProblem))));

    [Fact]
    public async Task ThenAsync_ShouldAwaitGenuinelyAsyncContinuation_WhenSuccess()
        // With ThenAsync bound to ValueTask only, an inline `async` lambda is no longer ambiguous.
        => Assert.Equal(Outcome.Of(20), await Async.Of(10).ThenAsync(async (v, _) => { await Task.Yield(); return v * 2; }));

    [Fact]
    public async Task ThenTask_ShouldAcceptTaskReturningMethodGroup_WhenSuccess()
        // The reason ThenTask exists: a Task-returning method can be passed directly, no ValueTask wrap.
        => Assert.Equal(Outcome.Of(20), await Async.Of(10).ThenTask(DoubleTask));

    [Fact]
    public async Task ThenAsync_ShouldNotInvoke_WhenProblem()
    {
        var invoked = false;

        var actual = await Async.OfProblem<int>(TestProblem).ThenAsync((v, _) => { invoked = true; return ValueTask.FromResult(v * 2); });

        Assert.Equal(Outcome.OfProblem<int>(TestProblem), actual);
        Assert.False(invoked);
    }

    // ----- AsyncOutcome<(T, T1)> — happy path, all four delegate shapes -----

    [Fact]
    public async Task ThenAsync_TwoValues_ValueTaskValue_ShouldSpreadAndProject_WhenSuccess()
        => Assert.Equal(Outcome.Of(3), await Async.Of((1, 2)).ThenAsync((a, b, _) => ValueTask.FromResult(a + b)));

    [Fact]
    public async Task ThenTask_TwoValues_TaskValue_ShouldSpreadAndProject_WhenSuccess()
        => Assert.Equal(Outcome.Of(3), await Async.Of((1, 2)).ThenTask((a, b, _) => Task.FromResult(a + b)));

    [Fact]
    public async Task ThenAsync_TwoValues_ValueTaskOutcome_ShouldSpreadAndProject_WhenSuccess()
        => Assert.Equal(Outcome.Of(3), await Async.Of((1, 2)).ThenAsync((a, b, _) => ValueTask.FromResult(Outcome.Of(a + b))));

    [Fact]
    public async Task ThenTask_TwoValues_TaskOutcome_ShouldSpreadAndProject_WhenSuccess()
        => Assert.Equal(Outcome.Of(3), await Async.Of((1, 2)).ThenTask((a, b, _) => Task.FromResult(Outcome.Of(a + b))));

    [Fact]
    public async Task ThenAsync_TwoValues_ShouldNotInvoke_WhenProblem()
    {
        var invoked = false;

        var actual = await Async.OfProblem<(int, int)>(TestProblem).ThenAsync((a, b, _) => { invoked = true; return ValueTask.FromResult(a + b); });

        Assert.Equal(Outcome.OfProblem<int>(TestProblem), actual);
        Assert.False(invoked);
    }

    // ----- AsyncOutcome<(T, T1, T2)> — happy path, all four delegate shapes -----

    [Fact]
    public async Task ThenAsync_ThreeValues_ValueTaskValue_ShouldSpreadAndProject_WhenSuccess()
        => Assert.Equal(Outcome.Of(6), await Async.Of((1, 2, 3)).ThenAsync((a, b, c, _) => ValueTask.FromResult(a + b + c)));

    [Fact]
    public async Task ThenTask_ThreeValues_TaskValue_ShouldSpreadAndProject_WhenSuccess()
        => Assert.Equal(Outcome.Of(6), await Async.Of((1, 2, 3)).ThenTask((a, b, c, _) => Task.FromResult(a + b + c)));

    [Fact]
    public async Task ThenAsync_ThreeValues_ValueTaskOutcome_ShouldSpreadAndProject_WhenSuccess()
        => Assert.Equal(Outcome.Of(6), await Async.Of((1, 2, 3)).ThenAsync((a, b, c, _) => ValueTask.FromResult(Outcome.Of(a + b + c))));

    [Fact]
    public async Task ThenTask_ThreeValues_TaskOutcome_ShouldSpreadAndProject_WhenSuccess()
        => Assert.Equal(Outcome.Of(6), await Async.Of((1, 2, 3)).ThenTask((a, b, c, _) => Task.FromResult(Outcome.Of(a + b + c))));

    [Fact]
    public async Task ThenAsync_ThreeValues_ShouldNotInvoke_WhenProblem()
    {
        var invoked = false;

        var actual = await Async.OfProblem<(int, int, int)>(TestProblem).ThenAsync((a, b, c, _) => { invoked = true; return ValueTask.FromResult(a + b + c); });

        Assert.Equal(Outcome.OfProblem<int>(TestProblem), actual);
        Assert.False(invoked);
    }

    // ----- AsyncOutcome<(T, T1, T2, T3)> — happy path, all four delegate shapes -----

    [Fact]
    public async Task ThenAsync_FourValues_ValueTaskValue_ShouldSpreadAndProject_WhenSuccess()
        => Assert.Equal(Outcome.Of(10), await Async.Of((1, 2, 3, 4)).ThenAsync((a, b, c, d, _) => ValueTask.FromResult(a + b + c + d)));

    [Fact]
    public async Task ThenTask_FourValues_TaskValue_ShouldSpreadAndProject_WhenSuccess()
        => Assert.Equal(Outcome.Of(10), await Async.Of((1, 2, 3, 4)).ThenTask((a, b, c, d, _) => Task.FromResult(a + b + c + d)));

    [Fact]
    public async Task ThenAsync_FourValues_ValueTaskOutcome_ShouldSpreadAndProject_WhenSuccess()
        => Assert.Equal(Outcome.Of(10), await Async.Of((1, 2, 3, 4)).ThenAsync((a, b, c, d, _) => ValueTask.FromResult(Outcome.Of(a + b + c + d))));

    [Fact]
    public async Task ThenTask_FourValues_TaskOutcome_ShouldSpreadAndProject_WhenSuccess()
        => Assert.Equal(Outcome.Of(10), await Async.Of((1, 2, 3, 4)).ThenTask((a, b, c, d, _) => Task.FromResult(Outcome.Of(a + b + c + d))));

    [Fact]
    public async Task ThenAsync_FourValues_ShouldNotInvoke_WhenProblem()
    {
        var invoked = false;

        var actual = await Async.OfProblem<(int, int, int, int)>(TestProblem).ThenAsync((a, b, c, d, _) => { invoked = true; return ValueTask.FromResult(a + b + c + d); });

        Assert.Equal(Outcome.OfProblem<int>(TestProblem), actual);
        Assert.False(invoked);
    }

    // ----- CancellationToken flow -----
    // The token carried by the AsyncOutcome must be handed to the delegate for every cardinality,
    // on both the ValueTask (ThenAsync) and Task (ThenTask) paths.

    [Fact]
    public async Task ThenAsync_None_ShouldPassOutcomeToken_ToDelegate()
    {
        using var cts = new CancellationTokenSource();
        CancellationToken seen = default;

        await AsyncOutcome.Ok(cts.Token).ThenAsync(ct => { seen = ct; return ValueTask.FromResult(0); });

        Assert.Equal(cts.Token, seen);
    }

    [Fact]
    public async Task ThenAsync_ShouldPassOutcomeToken_ToDelegate()
    {
        using var cts = new CancellationTokenSource();
        CancellationToken seen = default;

        await AsyncOutcome.Of(10, cts.Token).ThenAsync((v, ct) => { seen = ct; return ValueTask.FromResult(v); });

        Assert.Equal(cts.Token, seen);
    }

    [Fact]
    public async Task ThenTask_ShouldPassOutcomeToken_ToDelegate()
    {
        using var cts = new CancellationTokenSource();
        CancellationToken seen = default;

        await AsyncOutcome.Of(10, cts.Token).ThenTask((v, ct) => { seen = ct; return Task.FromResult(v); });

        Assert.Equal(cts.Token, seen);
    }

    [Fact]
    public async Task ThenAsync_TwoValues_ShouldPassOutcomeToken_ToDelegate()
    {
        using var cts = new CancellationTokenSource();
        CancellationToken seen = default;

        await AsyncOutcome.Of((1, 2), cts.Token).ThenAsync((a, b, ct) => { seen = ct; return ValueTask.FromResult(a + b); });

        Assert.Equal(cts.Token, seen);
    }

    [Fact]
    public async Task ThenTask_TwoValues_ShouldPassOutcomeToken_ToDelegate()
    {
        using var cts = new CancellationTokenSource();
        CancellationToken seen = default;

        await AsyncOutcome.Of((1, 2), cts.Token).ThenTask((a, b, ct) => { seen = ct; return Task.FromResult(a + b); });

        Assert.Equal(cts.Token, seen);
    }

    [Fact]
    public async Task ThenAsync_ThreeValues_ShouldPassOutcomeToken_ToDelegate()
    {
        using var cts = new CancellationTokenSource();
        CancellationToken seen = default;

        await AsyncOutcome.Of((1, 2, 3), cts.Token).ThenAsync((a, b, c, ct) => { seen = ct; return ValueTask.FromResult(a + b + c); });

        Assert.Equal(cts.Token, seen);
    }

    [Fact]
    public async Task ThenAsync_FourValues_ShouldPassOutcomeToken_ToDelegate()
    {
        using var cts = new CancellationTokenSource();
        CancellationToken seen = default;

        await AsyncOutcome.Of((1, 2, 3, 4), cts.Token).ThenAsync((a, b, c, d, ct) => { seen = ct; return ValueTask.FromResult(a + b + c + d); });

        Assert.Equal(cts.Token, seen);
    }

    [Fact]
    public async Task ThenTask_FourValues_ShouldPassOutcomeToken_ToDelegate()
    {
        using var cts = new CancellationTokenSource();
        CancellationToken seen = default;

        await AsyncOutcome.Of((1, 2, 3, 4), cts.Token).ThenTask((a, b, c, d, ct) => { seen = ct; return Task.FromResult(a + b + c + d); });

        Assert.Equal(cts.Token, seen);
    }
}

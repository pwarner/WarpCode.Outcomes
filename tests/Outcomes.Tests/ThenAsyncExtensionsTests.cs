namespace WarpCode.Outcomes.Tests;

public class ThenAsyncExtensionsTests
{
    private static readonly Problem TestProblem = new(nameof(TestProblem));

    private static Task<int> DoubleTask(int v, CancellationToken ct) => Task.FromResult(v * 2);

    // ----- AsyncOutcome<None> -----

    [Fact]
    public async Task ThenAsync_None_Value_ShouldProjectValue_WhenSuccess()
        => Assert.Equal(Outcome.Of(5), await Async.Ok().ThenAsync(_ => ValueTask.FromResult(5)));

    [Fact]
    public async Task ThenAsync_None_Value_ShouldPropagateProblem_WhenProblem()
        => Assert.Equal(Outcome.OfProblem<int>(TestProblem), await Async.OfProblem(TestProblem).ThenAsync(_ => ValueTask.FromResult(5)));

    [Fact]
    public async Task ThenTask_None_Value_ShouldProjectValue_WhenSuccess()
        => Assert.Equal(Outcome.Of(5), await Async.Ok().ThenTask(_ => Task.FromResult(5)));

    [Fact]
    public async Task ThenTask_None_Value_ShouldPropagateProblem_WhenProblem()
        => Assert.Equal(Outcome.OfProblem<int>(TestProblem), await Async.OfProblem(TestProblem).ThenTask(_ => Task.FromResult(5)));

    [Fact]
    public async Task ThenAsync_None_Outcome_ShouldProduceOutcome_WhenSuccess()
        => Assert.Equal(Outcome.Of(5), await Async.Ok().ThenAsync(_ => ValueTask.FromResult(Outcome.Of(5))));

    [Fact]
    public async Task ThenAsync_None_Outcome_ShouldPropagateProblem_WhenProblem()
        => Assert.Equal(Outcome.OfProblem<int>(TestProblem), await Async.OfProblem(TestProblem).ThenAsync(_ => ValueTask.FromResult(Outcome.Of(5))));

    [Fact]
    public async Task ThenTask_None_Outcome_ShouldProduceOutcome_WhenSuccess()
        => Assert.Equal(Outcome.Of(5), await Async.Ok().ThenTask(_ => Task.FromResult(Outcome.Of(5))));

    [Fact]
    public async Task ThenTask_None_Outcome_ShouldPropagateProblem_WhenProblem()
        => Assert.Equal(Outcome.OfProblem<int>(TestProblem), await Async.OfProblem(TestProblem).ThenTask(_ => Task.FromResult(Outcome.Of(5))));

    // ----- AsyncOutcome<T> -----

    [Fact]
    public async Task ThenAsync_Value_ShouldProjectValue_WhenSuccess()
        => Assert.Equal(Outcome.Of(20), await Async.Of(10).ThenAsync((v, _) => ValueTask.FromResult(v * 2)));

    [Fact]
    public async Task ThenAsync_Value_ShouldPropagateProblem_WhenProblem()
        => Assert.Equal(Outcome.OfProblem<int>(TestProblem), await Async.OfProblem<int>(TestProblem).ThenAsync((v, _) => ValueTask.FromResult(v * 2)));

    [Fact]
    public async Task ThenTask_Value_ShouldProjectValue_WhenSuccess()
        => Assert.Equal(Outcome.Of(20), await Async.Of(10).ThenTask((v, _) => Task.FromResult(v * 2)));

    [Fact]
    public async Task ThenTask_Value_ShouldPropagateProblem_WhenProblem()
        => Assert.Equal(Outcome.OfProblem<int>(TestProblem), await Async.OfProblem<int>(TestProblem).ThenTask((v, _) => Task.FromResult(v * 2)));

    [Fact]
    public async Task ThenAsync_Outcome_ShouldProduceOutcome_WhenSuccess()
        => Assert.Equal(Outcome.Of(20), await Async.Of(10).ThenAsync((v, _) => ValueTask.FromResult(Outcome.Of(v * 2))));

    [Fact]
    public async Task ThenAsync_Outcome_ShouldPropagateProblem_WhenProblem()
        => Assert.Equal(Outcome.OfProblem<int>(TestProblem), await Async.OfProblem<int>(TestProblem).ThenAsync((v, _) => ValueTask.FromResult(Outcome.Of(v * 2))));

    [Fact]
    public async Task ThenTask_Outcome_ShouldProduceOutcome_WhenSuccess()
        => Assert.Equal(Outcome.Of(20), await Async.Of(10).ThenTask((v, _) => Task.FromResult(Outcome.Of(v * 2))));

    [Fact]
    public async Task ThenTask_Outcome_ShouldPropagateProblem_WhenProblem()
        => Assert.Equal(Outcome.OfProblem<int>(TestProblem), await Async.OfProblem<int>(TestProblem).ThenTask((v, _) => Task.FromResult(Outcome.Of(v * 2))));

    // ----- AsyncOutcome<(T, T1)> -----

    [Fact]
    public async Task ThenAsync_TwoValues_Value_ShouldSpreadAndProject_WhenSuccess()
        => Assert.Equal(Outcome.Of(3), await Async.Of((1, 2)).ThenAsync((a, b, _) => ValueTask.FromResult(a + b)));

    [Fact]
    public async Task ThenAsync_TwoValues_Value_ShouldPropagateProblem_WhenProblem()
        => Assert.Equal(Outcome.OfProblem<int>(TestProblem), await Async.OfProblem<(int, int)>(TestProblem).ThenAsync((a, b, _) => ValueTask.FromResult(a + b)));

    [Fact]
    public async Task ThenTask_TwoValues_Value_ShouldSpreadAndProject_WhenSuccess()
        => Assert.Equal(Outcome.Of(3), await Async.Of((1, 2)).ThenTask((a, b, _) => Task.FromResult(a + b)));

    [Fact]
    public async Task ThenTask_TwoValues_Value_ShouldPropagateProblem_WhenProblem()
        => Assert.Equal(Outcome.OfProblem<int>(TestProblem), await Async.OfProblem<(int, int)>(TestProblem).ThenTask((a, b, _) => Task.FromResult(a + b)));

    [Fact]
    public async Task ThenAsync_TwoValues_Outcome_ShouldSpreadAndProject_WhenSuccess()
        => Assert.Equal(Outcome.Of(3), await Async.Of((1, 2)).ThenAsync((a, b, _) => ValueTask.FromResult(Outcome.Of(a + b))));

    [Fact]
    public async Task ThenAsync_TwoValues_Outcome_ShouldPropagateProblem_WhenProblem()
        => Assert.Equal(Outcome.OfProblem<int>(TestProblem), await Async.OfProblem<(int, int)>(TestProblem).ThenAsync((a, b, _) => ValueTask.FromResult(Outcome.Of(a + b))));

    [Fact]
    public async Task ThenTask_TwoValues_Outcome_ShouldSpreadAndProject_WhenSuccess()
        => Assert.Equal(Outcome.Of(3), await Async.Of((1, 2)).ThenTask((a, b, _) => Task.FromResult(Outcome.Of(a + b))));

    [Fact]
    public async Task ThenTask_TwoValues_Outcome_ShouldPropagateProblem_WhenProblem()
        => Assert.Equal(Outcome.OfProblem<int>(TestProblem), await Async.OfProblem<(int, int)>(TestProblem).ThenTask((a, b, _) => Task.FromResult(Outcome.Of(a + b))));

    // ----- AsyncOutcome<(T, T1, T2)> -----

    [Fact]
    public async Task ThenAsync_ThreeValues_Value_ShouldSpreadAndProject_WhenSuccess()
        => Assert.Equal(Outcome.Of(6), await Async.Of((1, 2, 3)).ThenAsync((a, b, c, _) => ValueTask.FromResult(a + b + c)));

    [Fact]
    public async Task ThenAsync_ThreeValues_Value_ShouldPropagateProblem_WhenProblem()
        => Assert.Equal(Outcome.OfProblem<int>(TestProblem), await Async.OfProblem<(int, int, int)>(TestProblem).ThenAsync((a, b, c, _) => ValueTask.FromResult(a + b + c)));

    [Fact]
    public async Task ThenTask_ThreeValues_Value_ShouldSpreadAndProject_WhenSuccess()
        => Assert.Equal(Outcome.Of(6), await Async.Of((1, 2, 3)).ThenTask((a, b, c, _) => Task.FromResult(a + b + c)));

    [Fact]
    public async Task ThenTask_ThreeValues_Value_ShouldPropagateProblem_WhenProblem()
        => Assert.Equal(Outcome.OfProblem<int>(TestProblem), await Async.OfProblem<(int, int, int)>(TestProblem).ThenTask((a, b, c, _) => Task.FromResult(a + b + c)));

    [Fact]
    public async Task ThenAsync_ThreeValues_Outcome_ShouldSpreadAndProject_WhenSuccess()
        => Assert.Equal(Outcome.Of(6), await Async.Of((1, 2, 3)).ThenAsync((a, b, c, _) => ValueTask.FromResult(Outcome.Of(a + b + c))));

    [Fact]
    public async Task ThenAsync_ThreeValues_Outcome_ShouldPropagateProblem_WhenProblem()
        => Assert.Equal(Outcome.OfProblem<int>(TestProblem), await Async.OfProblem<(int, int, int)>(TestProblem).ThenAsync((a, b, c, _) => ValueTask.FromResult(Outcome.Of(a + b + c))));

    [Fact]
    public async Task ThenTask_ThreeValues_Outcome_ShouldSpreadAndProject_WhenSuccess()
        => Assert.Equal(Outcome.Of(6), await Async.Of((1, 2, 3)).ThenTask((a, b, c, _) => Task.FromResult(Outcome.Of(a + b + c))));

    [Fact]
    public async Task ThenTask_ThreeValues_Outcome_ShouldPropagateProblem_WhenProblem()
        => Assert.Equal(Outcome.OfProblem<int>(TestProblem), await Async.OfProblem<(int, int, int)>(TestProblem).ThenTask((a, b, c, _) => Task.FromResult(Outcome.Of(a + b + c))));

    // ----- AsyncOutcome<(T, T1, T2, T3)> -----

    [Fact]
    public async Task ThenAsync_FourValues_Value_ShouldSpreadAndProject_WhenSuccess()
        => Assert.Equal(Outcome.Of(10), await Async.Of((1, 2, 3, 4)).ThenAsync((a, b, c, d, _) => ValueTask.FromResult(a + b + c + d)));

    [Fact]
    public async Task ThenAsync_FourValues_Value_ShouldPropagateProblem_WhenProblem()
        => Assert.Equal(Outcome.OfProblem<int>(TestProblem), await Async.OfProblem<(int, int, int, int)>(TestProblem).ThenAsync((a, b, c, d, _) => ValueTask.FromResult(a + b + c + d)));

    [Fact]
    public async Task ThenTask_FourValues_Value_ShouldSpreadAndProject_WhenSuccess()
        => Assert.Equal(Outcome.Of(10), await Async.Of((1, 2, 3, 4)).ThenTask((a, b, c, d, _) => Task.FromResult(a + b + c + d)));

    [Fact]
    public async Task ThenTask_FourValues_Value_ShouldPropagateProblem_WhenProblem()
        => Assert.Equal(Outcome.OfProblem<int>(TestProblem), await Async.OfProblem<(int, int, int, int)>(TestProblem).ThenTask((a, b, c, d, _) => Task.FromResult(a + b + c + d)));

    [Fact]
    public async Task ThenAsync_FourValues_Outcome_ShouldSpreadAndProject_WhenSuccess()
        => Assert.Equal(Outcome.Of(10), await Async.Of((1, 2, 3, 4)).ThenAsync((a, b, c, d, _) => ValueTask.FromResult(Outcome.Of(a + b + c + d))));

    [Fact]
    public async Task ThenAsync_FourValues_Outcome_ShouldPropagateProblem_WhenProblem()
        => Assert.Equal(Outcome.OfProblem<int>(TestProblem), await Async.OfProblem<(int, int, int, int)>(TestProblem).ThenAsync((a, b, c, d, _) => ValueTask.FromResult(Outcome.Of(a + b + c + d))));

    [Fact]
    public async Task ThenTask_FourValues_Outcome_ShouldSpreadAndProject_WhenSuccess()
        => Assert.Equal(Outcome.Of(10), await Async.Of((1, 2, 3, 4)).ThenTask((a, b, c, d, _) => Task.FromResult(Outcome.Of(a + b + c + d))));

    [Fact]
    public async Task ThenTask_FourValues_Outcome_ShouldPropagateProblem_WhenProblem()
        => Assert.Equal(Outcome.OfProblem<int>(TestProblem), await Async.OfProblem<(int, int, int, int)>(TestProblem).ThenTask((a, b, c, d, _) => Task.FromResult(Outcome.Of(a + b + c + d))));

    // ----- Delegate binding (distinct from happy/sad) -----

    [Fact]
    public async Task ThenAsync_ShouldAwaitGenuinelyAsyncContinuation_WhenSuccess()
        // With ThenAsync bound to ValueTask only, an inline `async` lambda is no longer ambiguous.
        => Assert.Equal(Outcome.Of(20), await Async.Of(10).ThenAsync(async (v, _) => { await Task.Yield(); return v * 2; }));

    [Fact]
    public async Task ThenTask_ShouldAcceptTaskReturningMethodGroup_WhenSuccess()
        // The reason ThenTask exists: a Task-returning method can be passed directly, no ValueTask wrap.
        => Assert.Equal(Outcome.Of(20), await Async.Of(10).ThenTask(DoubleTask));

    // ----- CancellationToken flow -----
    // The token carried by the AsyncOutcome must reach the delegate for every cardinality, on
    // both the ValueTask (ThenAsync) and Task (ThenTask) forwarding chains.

    [Fact]
    public async Task ThenAsync_None_ShouldPassOutcomeToken_ToDelegate()
    {
        using var cts = new CancellationTokenSource();
        CancellationToken seen = default;

        await AsyncOutcome.Ok(cts.Token).ThenAsync(ct => { seen = ct; return ValueTask.FromResult(0); });

        Assert.Equal(cts.Token, seen);
    }

    [Fact]
    public async Task ThenTask_None_ShouldPassOutcomeToken_ToDelegate()
    {
        using var cts = new CancellationTokenSource();
        CancellationToken seen = default;

        await AsyncOutcome.Ok(cts.Token).ThenTask(ct => { seen = ct; return Task.FromResult(0); });

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
    public async Task ThenTask_ThreeValues_ShouldPassOutcomeToken_ToDelegate()
    {
        using var cts = new CancellationTokenSource();
        CancellationToken seen = default;

        await AsyncOutcome.Of((1, 2, 3), cts.Token).ThenTask((a, b, c, ct) => { seen = ct; return Task.FromResult(a + b + c); });

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

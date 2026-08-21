namespace WarpCode.Outcomes.Tests;

public class OnValueAsyncExtensionsTests
{
    private static readonly Problem TestProblem = new(nameof(TestProblem));

    // ----- AsyncOutcome<None> -----

    [Fact]
    public async Task OnValueAsync_None_ShouldInvokeAndReturnOriginal_WhenSuccess()
    {
        var invoked = false;

        var actual = await Async.Ok().OnValueAsync(_ => { invoked = true; return ValueTask.CompletedTask; });

        Assert.Equal(Outcome.Ok, actual);
        Assert.True(invoked);
    }

    [Fact]
    public async Task OnValueTask_None_ShouldInvokeAndReturnOriginal_WhenSuccess()
    {
        var invoked = false;

        var actual = await Async.Ok().OnValueTask(_ => { invoked = true; return Task.CompletedTask; });

        Assert.Equal(Outcome.Ok, actual);
        Assert.True(invoked);
    }

    [Fact]
    public async Task OnValueAsync_None_ShouldNotInvoke_WhenProblem()
    {
        var invoked = false;

        var actual = await Async.OfProblem(TestProblem).OnValueAsync(_ => { invoked = true; return ValueTask.CompletedTask; });

        Assert.Equal(Outcome.OfProblem(TestProblem), actual);
        Assert.False(invoked);
    }

    // ----- AsyncOutcome<T> -----

    [Fact]
    public async Task OnValueAsync_ShouldInvokeWithValueAndReturnOriginal_WhenSuccess()
    {
        int? seen = null;

        var actual = await Async.Of(10).OnValueAsync((v, _) => { seen = v; return ValueTask.CompletedTask; });

        Assert.Equal(Outcome.Of(10), actual);
        Assert.Equal(10, seen);
    }

    [Fact]
    public async Task OnValueTask_ShouldInvokeWithValueAndReturnOriginal_WhenSuccess()
    {
        int? seen = null;

        var actual = await Async.Of(10).OnValueTask((v, _) => { seen = v; return Task.CompletedTask; });

        Assert.Equal(Outcome.Of(10), actual);
        Assert.Equal(10, seen);
    }

    [Fact]
    public async Task OnValueAsync_ShouldNotInvoke_WhenProblem()
    {
        var invoked = false;

        var actual = await Async.OfProblem<int>(TestProblem).OnValueAsync((_, _) => { invoked = true; return ValueTask.CompletedTask; });

        Assert.Equal(Outcome.OfProblem<int>(TestProblem), actual);
        Assert.False(invoked);
    }

    // ----- AsyncOutcome<(T, T1)> -----

    [Fact]
    public async Task OnValueAsync_TwoValues_ShouldSpreadAndReturnOriginal_WhenSuccess()
    {
        (int, int)? seen = null;

        var actual = await Async.Of((1, 2)).OnValueAsync((a, b, _) => { seen = (a, b); return ValueTask.CompletedTask; });

        Assert.Equal(Outcome.Of((1, 2)), actual);
        Assert.Equal((1, 2), seen);
    }

    [Fact]
    public async Task OnValueTask_TwoValues_ShouldSpreadAndReturnOriginal_WhenSuccess()
    {
        (int, int)? seen = null;

        var actual = await Async.Of((1, 2)).OnValueTask((a, b, _) => { seen = (a, b); return Task.CompletedTask; });

        Assert.Equal(Outcome.Of((1, 2)), actual);
        Assert.Equal((1, 2), seen);
    }

    [Fact]
    public async Task OnValueAsync_TwoValues_ShouldNotInvoke_WhenProblem()
    {
        var invoked = false;

        var actual = await Async.OfProblem<(int, int)>(TestProblem).OnValueAsync((_, _, _) => { invoked = true; return ValueTask.CompletedTask; });

        Assert.Equal(Outcome.OfProblem<(int, int)>(TestProblem), actual);
        Assert.False(invoked);
    }

    // ----- AsyncOutcome<(T, T1, T2)> -----

    [Fact]
    public async Task OnValueAsync_ThreeValues_ShouldSpreadAndReturnOriginal_WhenSuccess()
    {
        (int, int, int)? seen = null;

        var actual = await Async.Of((1, 2, 3)).OnValueAsync((a, b, c, _) => { seen = (a, b, c); return ValueTask.CompletedTask; });

        Assert.Equal(Outcome.Of((1, 2, 3)), actual);
        Assert.Equal((1, 2, 3), seen);
    }

    [Fact]
    public async Task OnValueTask_ThreeValues_ShouldSpreadAndReturnOriginal_WhenSuccess()
    {
        (int, int, int)? seen = null;

        var actual = await Async.Of((1, 2, 3)).OnValueTask((a, b, c, _) => { seen = (a, b, c); return Task.CompletedTask; });

        Assert.Equal(Outcome.Of((1, 2, 3)), actual);
        Assert.Equal((1, 2, 3), seen);
    }

    [Fact]
    public async Task OnValueAsync_ThreeValues_ShouldNotInvoke_WhenProblem()
    {
        var invoked = false;

        var actual = await Async.OfProblem<(int, int, int)>(TestProblem).OnValueAsync((_, _, _, _) => { invoked = true; return ValueTask.CompletedTask; });

        Assert.Equal(Outcome.OfProblem<(int, int, int)>(TestProblem), actual);
        Assert.False(invoked);
    }

    // ----- AsyncOutcome<(T, T1, T2, T3)> -----

    [Fact]
    public async Task OnValueAsync_FourValues_ShouldSpreadAndReturnOriginal_WhenSuccess()
    {
        (int, int, int, int)? seen = null;

        var actual = await Async.Of((1, 2, 3, 4)).OnValueAsync((a, b, c, d, _) => { seen = (a, b, c, d); return ValueTask.CompletedTask; });

        Assert.Equal(Outcome.Of((1, 2, 3, 4)), actual);
        Assert.Equal((1, 2, 3, 4), seen);
    }

    [Fact]
    public async Task OnValueTask_FourValues_ShouldSpreadAndReturnOriginal_WhenSuccess()
    {
        (int, int, int, int)? seen = null;

        var actual = await Async.Of((1, 2, 3, 4)).OnValueTask((a, b, c, d, _) => { seen = (a, b, c, d); return Task.CompletedTask; });

        Assert.Equal(Outcome.Of((1, 2, 3, 4)), actual);
        Assert.Equal((1, 2, 3, 4), seen);
    }

    [Fact]
    public async Task OnValueAsync_FourValues_ShouldNotInvoke_WhenProblem()
    {
        var invoked = false;

        var actual = await Async.OfProblem<(int, int, int, int)>(TestProblem).OnValueAsync((_, _, _, _, _) => { invoked = true; return ValueTask.CompletedTask; });

        Assert.Equal(Outcome.OfProblem<(int, int, int, int)>(TestProblem), actual);
        Assert.False(invoked);
    }

    // ----- CancellationToken flow -----

    [Fact]
    public async Task OnValueAsync_ShouldPassOutcomeToken_ToDelegate()
    {
        using var cts = new CancellationTokenSource();
        CancellationToken seen = default;

        await AsyncOutcome.Of(10, cts.Token).OnValueAsync((_, ct) => { seen = ct; return ValueTask.CompletedTask; });

        Assert.Equal(cts.Token, seen);
    }

    [Fact]
    public async Task OnValueTask_ShouldPassOutcomeToken_ToDelegate()
    {
        using var cts = new CancellationTokenSource();
        CancellationToken seen = default;

        await AsyncOutcome.Of(10, cts.Token).OnValueTask((_, ct) => { seen = ct; return Task.CompletedTask; });

        Assert.Equal(cts.Token, seen);
    }
}

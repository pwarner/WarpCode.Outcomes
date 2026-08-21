namespace WarpCode.Outcomes.Tests;

public class AsyncOnValueExtensionsTests
{
    private static readonly Problem TestProblem = new(nameof(TestProblem));

    // ----- AsyncOutcome<None> -----

    [Fact]
    public async Task OnValue_None_ShouldInvokeAndReturnOriginal_WhenSuccess()
    {
        var invoked = false;

        var actual = await Async.Ok().OnValue(() => invoked = true);

        Assert.Equal(Outcome.Ok, actual);
        Assert.True(invoked);
    }

    [Fact]
    public async Task OnValue_None_ShouldNotInvoke_WhenProblem()
    {
        var invoked = false;

        var actual = await Async.OfProblem(TestProblem).OnValue(() => invoked = true);

        Assert.Equal(Outcome.OfProblem(TestProblem), actual);
        Assert.False(invoked);
    }

    // ----- AsyncOutcome<T> -----

    [Fact]
    public async Task OnValue_ShouldInvokeWithValueAndReturnOriginal_WhenSuccess()
    {
        int? seen = null;

        var actual = await Async.Of(10).OnValue(v => seen = v);

        Assert.Equal(Outcome.Of(10), actual);
        Assert.Equal(10, seen);
    }

    [Fact]
    public async Task OnValue_ShouldNotInvoke_WhenProblem()
    {
        var invoked = false;

        var actual = await Async.OfProblem<int>(TestProblem).OnValue(_ => invoked = true);

        Assert.Equal(Outcome.OfProblem<int>(TestProblem), actual);
        Assert.False(invoked);
    }

    // ----- AsyncOutcome<(T, T1)> -----

    [Fact]
    public async Task OnValue_TwoValues_ShouldSpreadAndReturnOriginal_WhenSuccess()
    {
        (int, int)? seen = null;

        var actual = await Async.Of((1, 2)).OnValue((a, b) => seen = (a, b));

        Assert.Equal(Outcome.Of((1, 2)), actual);
        Assert.Equal((1, 2), seen);
    }

    [Fact]
    public async Task OnValue_TwoValues_ShouldNotInvoke_WhenProblem()
    {
        var invoked = false;

        var actual = await Async.OfProblem<(int, int)>(TestProblem).OnValue((_, _) => invoked = true);

        Assert.Equal(Outcome.OfProblem<(int, int)>(TestProblem), actual);
        Assert.False(invoked);
    }

    // ----- AsyncOutcome<(T, T1, T2)> -----

    [Fact]
    public async Task OnValue_ThreeValues_ShouldSpreadAndReturnOriginal_WhenSuccess()
    {
        (int, int, int)? seen = null;

        var actual = await Async.Of((1, 2, 3)).OnValue((a, b, c) => seen = (a, b, c));

        Assert.Equal(Outcome.Of((1, 2, 3)), actual);
        Assert.Equal((1, 2, 3), seen);
    }

    [Fact]
    public async Task OnValue_ThreeValues_ShouldNotInvoke_WhenProblem()
    {
        var invoked = false;

        var actual = await Async.OfProblem<(int, int, int)>(TestProblem).OnValue((_, _, _) => invoked = true);

        Assert.Equal(Outcome.OfProblem<(int, int, int)>(TestProblem), actual);
        Assert.False(invoked);
    }

    // ----- AsyncOutcome<(T, T1, T2, T3)> -----

    [Fact]
    public async Task OnValue_FourValues_ShouldSpreadAndReturnOriginal_WhenSuccess()
    {
        (int, int, int, int)? seen = null;

        var actual = await Async.Of((1, 2, 3, 4)).OnValue((a, b, c, d) => seen = (a, b, c, d));

        Assert.Equal(Outcome.Of((1, 2, 3, 4)), actual);
        Assert.Equal((1, 2, 3, 4), seen);
    }

    [Fact]
    public async Task OnValue_FourValues_ShouldNotInvoke_WhenProblem()
    {
        var invoked = false;

        var actual = await Async.OfProblem<(int, int, int, int)>(TestProblem).OnValue((_, _, _, _) => invoked = true);

        Assert.Equal(Outcome.OfProblem<(int, int, int, int)>(TestProblem), actual);
        Assert.False(invoked);
    }
}

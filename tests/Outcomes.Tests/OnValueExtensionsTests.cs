namespace WarpCode.Outcomes.Tests;

public class OnValueExtensionsTests
{
    private static readonly Problem TestProblem = new(nameof(TestProblem));

    // ----- Outcome<None> -----

    [Fact]
    public void OnValue_None_ShouldInvokeAndReturnOriginal_WhenSuccess()
    {
        var invoked = false;

        var actual = Outcome.Ok.OnValue(() => invoked = true);

        Assert.Equal(Outcome.Ok, actual);
        Assert.True(invoked);
    }

    [Fact]
    public void OnValue_None_ShouldNotInvoke_WhenProblem()
    {
        var invoked = false;

        var actual = Outcome.OfProblem(TestProblem).OnValue(() => invoked = true);

        Assert.Equal(Outcome.OfProblem(TestProblem), actual);
        Assert.False(invoked);
    }

    // ----- Outcome<T> -----

    [Fact]
    public void OnValue_ShouldInvokeWithValueAndReturnOriginal_WhenSuccess()
    {
        int? seen = null;

        var actual = Outcome.Of(10).OnValue(v => seen = v);

        Assert.Equal(Outcome.Of(10), actual);
        Assert.Equal(10, seen);
    }

    [Fact]
    public void OnValue_ShouldNotInvoke_WhenProblem()
    {
        var invoked = false;

        var actual = Outcome.OfProblem<int>(TestProblem).OnValue(_ => invoked = true);

        Assert.Equal(Outcome.OfProblem<int>(TestProblem), actual);
        Assert.False(invoked);
    }

    // ----- Outcome<(T, T1)> -----

    [Fact]
    public void OnValue_TwoValues_ShouldSpreadAndReturnOriginal_WhenSuccess()
    {
        (int, int)? seen = null;

        var actual = Outcome.Of((1, 2)).OnValue((a, b) => seen = (a, b));

        Assert.Equal(Outcome.Of((1, 2)), actual);
        Assert.Equal((1, 2), seen);
    }

    [Fact]
    public void OnValue_TwoValues_ShouldNotInvoke_WhenProblem()
    {
        var invoked = false;

        var actual = Outcome.OfProblem<(int, int)>(TestProblem).OnValue((_, _) => invoked = true);

        Assert.Equal(Outcome.OfProblem<(int, int)>(TestProblem), actual);
        Assert.False(invoked);
    }

    // ----- Outcome<(T, T1, T2)> -----

    [Fact]
    public void OnValue_ThreeValues_ShouldSpreadAndReturnOriginal_WhenSuccess()
    {
        (int, int, int)? seen = null;

        var actual = Outcome.Of((1, 2, 3)).OnValue((a, b, c) => seen = (a, b, c));

        Assert.Equal(Outcome.Of((1, 2, 3)), actual);
        Assert.Equal((1, 2, 3), seen);
    }

    [Fact]
    public void OnValue_ThreeValues_ShouldNotInvoke_WhenProblem()
    {
        var invoked = false;

        var actual = Outcome.OfProblem<(int, int, int)>(TestProblem).OnValue((_, _, _) => invoked = true);

        Assert.Equal(Outcome.OfProblem<(int, int, int)>(TestProblem), actual);
        Assert.False(invoked);
    }

    // ----- Outcome<(T, T1, T2, T3)> -----

    [Fact]
    public void OnValue_FourValues_ShouldSpreadAndReturnOriginal_WhenSuccess()
    {
        (int, int, int, int)? seen = null;

        var actual = Outcome.Of((1, 2, 3, 4)).OnValue((a, b, c, d) => seen = (a, b, c, d));

        Assert.Equal(Outcome.Of((1, 2, 3, 4)), actual);
        Assert.Equal((1, 2, 3, 4), seen);
    }

    [Fact]
    public void OnValue_FourValues_ShouldNotInvoke_WhenProblem()
    {
        var invoked = false;

        var actual = Outcome.OfProblem<(int, int, int, int)>(TestProblem).OnValue((_, _, _, _) => invoked = true);

        Assert.Equal(Outcome.OfProblem<(int, int, int, int)>(TestProblem), actual);
        Assert.False(invoked);
    }
}

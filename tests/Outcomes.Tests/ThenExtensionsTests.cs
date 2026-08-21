namespace WarpCode.Outcomes.Tests;

public class ThenExtensionsTests
{
    private static readonly Problem TestProblem = new(nameof(TestProblem));

    // ----- Outcome<None> -----

    [Fact]
    public void Then_None_ConstantValue_ShouldReturnValue_WhenSuccess()
        => Assert.Equal(Outcome.Of(5), Outcome.Ok.Then(5));

    [Fact]
    public void Then_None_ConstantValue_ShouldPropagateProblem_WhenProblem()
        => Assert.Equal(Outcome.OfProblem<int>(TestProblem), Outcome.OfProblem(TestProblem).Then(5));

    [Fact]
    public void Then_None_ConstantOutcome_ShouldReturnOutcome_WhenSuccess()
        => Assert.Equal(Outcome.Of(5), Outcome.Ok.Then(Outcome.Of(5)));

    [Fact]
    public void Then_None_ConstantOutcome_ShouldPropagateProblem_WhenProblem()
        => Assert.Equal(Outcome.OfProblem<int>(TestProblem), Outcome.OfProblem(TestProblem).Then(Outcome.Of(5)));

    [Fact]
    public void Then_None_FuncValue_ShouldReturnValue_WhenSuccess()
        => Assert.Equal(Outcome.Of(5), Outcome.Ok.Then(() => 5));

    [Fact]
    public void Then_None_FuncValue_ShouldNotInvoke_WhenProblem()
    {
        var invoked = false;

        var actual = Outcome.OfProblem(TestProblem).Then(() => { invoked = true; return 5; });

        Assert.Equal(Outcome.OfProblem<int>(TestProblem), actual);
        Assert.False(invoked);
    }

    [Fact]
    public void Then_None_FuncOutcome_ShouldReturnOutcome_WhenSuccess()
        => Assert.Equal(Outcome.Of(5), Outcome.Ok.Then(() => Outcome.Of(5)));

    [Fact]
    public void Then_None_FuncOutcome_ShouldNotInvoke_WhenProblem()
    {
        var invoked = false;

        var actual = Outcome.OfProblem(TestProblem).Then(() => { invoked = true; return Outcome.Of(5); });

        Assert.Equal(Outcome.OfProblem<int>(TestProblem), actual);
        Assert.False(invoked);
    }

    // ----- Outcome<T> -----

    [Fact]
    public void Then_Map_ShouldProjectValue_WhenSuccess()
        => Assert.Equal(Outcome.Of(20), Outcome.Of(10).Then(v => v * 2));

    [Fact]
    public void Then_Map_ShouldNotInvoke_WhenProblem()
    {
        var invoked = false;

        var actual = Outcome.OfProblem<int>(TestProblem).Then(v => { invoked = true; return v * 2; });

        Assert.Equal(Outcome.OfProblem<int>(TestProblem), actual);
        Assert.False(invoked);
    }

    [Fact]
    public void Then_Bind_ShouldProduceNextOutcome_WhenSuccess()
        => Assert.Equal(Outcome.Of(20), Outcome.Of(10).Then(v => Outcome.Of(v * 2)));

    [Fact]
    public void Then_Bind_ShouldPropagateInnerProblem_WhenSuccess()
        => Assert.Equal(Outcome.OfProblem<int>(TestProblem), Outcome.Of(10).Then(_ => Outcome.OfProblem<int>(TestProblem)));

    [Fact]
    public void Then_Bind_ShouldNotInvoke_WhenProblem()
    {
        var invoked = false;

        var actual = Outcome.OfProblem<int>(TestProblem).Then(v => { invoked = true; return Outcome.Of(v * 2); });

        Assert.Equal(Outcome.OfProblem<int>(TestProblem), actual);
        Assert.False(invoked);
    }

    // ----- Outcome<(T, T1)> -----

    [Fact]
    public void Then_Map_TwoValues_ShouldSpreadAndProject_WhenSuccess()
        => Assert.Equal(Outcome.Of(3), Outcome.Of((1, 2)).Then((a, b) => a + b));

    [Fact]
    public void Then_Bind_TwoValues_ShouldSpreadAndProject_WhenSuccess()
        => Assert.Equal(Outcome.Of(3), Outcome.Of((1, 2)).Then((a, b) => Outcome.Of(a + b)));

    [Fact]
    public void Then_TwoValues_ShouldNotInvoke_WhenProblem()
    {
        var invoked = false;

        var actual = Outcome.OfProblem<(int, int)>(TestProblem).Then((a, b) => { invoked = true; return a + b; });

        Assert.Equal(Outcome.OfProblem<int>(TestProblem), actual);
        Assert.False(invoked);
    }

    // ----- Outcome<(T, T1, T2)> -----

    [Fact]
    public void Then_Map_ThreeValues_ShouldSpreadAndProject_WhenSuccess()
        => Assert.Equal(Outcome.Of(6), Outcome.Of((1, 2, 3)).Then((a, b, c) => a + b + c));

    [Fact]
    public void Then_Bind_ThreeValues_ShouldSpreadAndProject_WhenSuccess()
        => Assert.Equal(Outcome.Of(6), Outcome.Of((1, 2, 3)).Then((a, b, c) => Outcome.Of(a + b + c)));

    [Fact]
    public void Then_ThreeValues_ShouldNotInvoke_WhenProblem()
    {
        var invoked = false;

        var actual = Outcome.OfProblem<(int, int, int)>(TestProblem).Then((a, b, c) => { invoked = true; return a + b + c; });

        Assert.Equal(Outcome.OfProblem<int>(TestProblem), actual);
        Assert.False(invoked);
    }

    // ----- Outcome<(T, T1, T2, T3)> -----

    [Fact]
    public void Then_Map_FourValues_ShouldSpreadAndProject_WhenSuccess()
        => Assert.Equal(Outcome.Of(10), Outcome.Of((1, 2, 3, 4)).Then((a, b, c, d) => a + b + c + d));

    [Fact]
    public void Then_Bind_FourValues_ShouldSpreadAndProject_WhenSuccess()
        => Assert.Equal(Outcome.Of(10), Outcome.Of((1, 2, 3, 4)).Then((a, b, c, d) => Outcome.Of(a + b + c + d)));

    [Fact]
    public void Then_FourValues_ShouldNotInvoke_WhenProblem()
    {
        var invoked = false;

        var actual = Outcome.OfProblem<(int, int, int, int)>(TestProblem).Then((a, b, c, d) => { invoked = true; return a + b + c + d; });

        Assert.Equal(Outcome.OfProblem<int>(TestProblem), actual);
        Assert.False(invoked);
    }
}

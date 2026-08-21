namespace WarpCode.Outcomes.Tests;

public class EnsureExtensionsTests
{
    private static readonly Problem TestProblem = new(nameof(TestProblem));
    private static readonly Problem ValidationProblem = new(nameof(ValidationProblem));

    // ----- Outcome<None>.Ensure(Outcome<None>) -----

    [Fact]
    public void Ensure_None_ConstantOutcome_ShouldReturnOriginal_WhenBothSuccessful()
        => Assert.Equal(Outcome.Ok, Outcome.Ok.Ensure(Outcome.Ok));

    [Fact]
    public void Ensure_None_ConstantOutcome_ShouldPropagateValidationProblem_WhenEnsureFails()
        => Assert.Equal(Outcome.OfProblem(ValidationProblem), Outcome.Ok.Ensure(Outcome.OfProblem(ValidationProblem)));

    [Fact]
    public void Ensure_None_ConstantOutcome_ShouldPropagateOriginalProblem_WhenProblem()
        => Assert.Equal(Outcome.OfProblem(TestProblem), Outcome.OfProblem(TestProblem).Ensure(Outcome.OfProblem(ValidationProblem)));

    // ----- Outcome<None>.Ensure(Func<Outcome<None>>) -----

    [Fact]
    public void Ensure_None_Func_ShouldReturnOriginal_WhenEnsureSucceeds()
        => Assert.Equal(Outcome.Ok, Outcome.Ok.Ensure(() => Outcome.Ok));

    [Fact]
    public void Ensure_None_Func_ShouldPropagateValidationProblem_WhenEnsureFails()
        => Assert.Equal(Outcome.OfProblem(ValidationProblem), Outcome.Ok.Ensure(() => Outcome.OfProblem(ValidationProblem)));

    [Fact]
    public void Ensure_None_Func_ShouldNotInvoke_WhenProblem()
    {
        var invoked = false;

        var actual = Outcome.OfProblem(TestProblem).Ensure(() => { invoked = true; return Outcome.Ok; });

        Assert.Equal(Outcome.OfProblem(TestProblem), actual);
        Assert.False(invoked);
    }

    // ----- Outcome<T> -----

    [Fact]
    public void Ensure_ShouldReturnOriginal_WhenValidationPasses()
        => Assert.Equal(Outcome.Of(10), Outcome.Of(10).Ensure(_ => Outcome.Ok));

    [Fact]
    public void Ensure_ShouldPropagateValidationProblem_WhenValidationFails()
        => Assert.Equal(Outcome.OfProblem<int>(ValidationProblem), Outcome.Of(10).Ensure(_ => Outcome.OfProblem(ValidationProblem)));

    [Fact]
    public void Ensure_ShouldReceiveValue_WhenSuccess()
    {
        int? seen = null;

        Outcome.Of(10).Ensure(v => { seen = v; return Outcome.Ok; });

        Assert.Equal(10, seen);
    }

    [Fact]
    public void Ensure_ShouldNotInvoke_WhenProblem()
    {
        var invoked = false;

        var actual = Outcome.OfProblem<int>(TestProblem).Ensure(_ => { invoked = true; return Outcome.Ok; });

        Assert.Equal(Outcome.OfProblem<int>(TestProblem), actual);
        Assert.False(invoked);
    }

    // ----- Outcome<(T, T1)> -----

    [Fact]
    public void Ensure_TwoValues_ShouldReturnOriginal_WhenValidationPasses()
        => Assert.Equal(Outcome.Of((1, 2)), Outcome.Of((1, 2)).Ensure((a, b) => a + b == 3 ? Outcome.Ok : ValidationProblem));

    [Fact]
    public void Ensure_TwoValues_ShouldPropagateValidationProblem_WhenValidationFails()
        => Assert.Equal(Outcome.OfProblem<(int, int)>(ValidationProblem), Outcome.Of((1, 2)).Ensure((_, _) => Outcome.OfProblem(ValidationProblem)));

    [Fact]
    public void Ensure_TwoValues_ShouldNotInvoke_WhenProblem()
    {
        var invoked = false;

        var actual = Outcome.OfProblem<(int, int)>(TestProblem).Ensure((_, _) => { invoked = true; return Outcome.Ok; });

        Assert.Equal(Outcome.OfProblem<(int, int)>(TestProblem), actual);
        Assert.False(invoked);
    }

    // ----- Outcome<(T, T1, T2)> -----

    [Fact]
    public void Ensure_ThreeValues_ShouldReturnOriginal_WhenValidationPasses()
        => Assert.Equal(Outcome.Of((1, 2, 3)), Outcome.Of((1, 2, 3)).Ensure((_, _, _) => Outcome.Ok));

    [Fact]
    public void Ensure_ThreeValues_ShouldPropagateValidationProblem_WhenValidationFails()
        => Assert.Equal(Outcome.OfProblem<(int, int, int)>(ValidationProblem), Outcome.Of((1, 2, 3)).Ensure((_, _, _) => Outcome.OfProblem(ValidationProblem)));

    [Fact]
    public void Ensure_ThreeValues_ShouldNotInvoke_WhenProblem()
    {
        var invoked = false;

        var actual = Outcome.OfProblem<(int, int, int)>(TestProblem).Ensure((_, _, _) => { invoked = true; return Outcome.Ok; });

        Assert.Equal(Outcome.OfProblem<(int, int, int)>(TestProblem), actual);
        Assert.False(invoked);
    }

    // ----- Outcome<(T, T1, T2, T3)> -----

    [Fact]
    public void Ensure_FourValues_ShouldReturnOriginal_WhenValidationPasses()
        => Assert.Equal(Outcome.Of((1, 2, 3, 4)), Outcome.Of((1, 2, 3, 4)).Ensure((_, _, _, _) => Outcome.Ok));

    [Fact]
    public void Ensure_FourValues_ShouldPropagateValidationProblem_WhenValidationFails()
        => Assert.Equal(Outcome.OfProblem<(int, int, int, int)>(ValidationProblem), Outcome.Of((1, 2, 3, 4)).Ensure((_, _, _, _) => Outcome.OfProblem(ValidationProblem)));

    [Fact]
    public void Ensure_FourValues_ShouldNotInvoke_WhenProblem()
    {
        var invoked = false;

        var actual = Outcome.OfProblem<(int, int, int, int)>(TestProblem).Ensure((_, _, _, _) => { invoked = true; return Outcome.Ok; });

        Assert.Equal(Outcome.OfProblem<(int, int, int, int)>(TestProblem), actual);
        Assert.False(invoked);
    }
}

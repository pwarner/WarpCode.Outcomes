namespace WarpCode.Outcomes.Tests;

public class AsyncEnsureExtensionsTests
{
    private static readonly Problem TestProblem = new(nameof(TestProblem));
    private static readonly Problem ValidationProblem = new(nameof(ValidationProblem));

    // ----- AsyncOutcome<None>.Ensure(Outcome<None>) -----

    [Fact]
    public async Task Ensure_None_ConstantOutcome_ShouldReturnOriginal_WhenEnsureSucceeds()
        => Assert.Equal(Outcome.Ok, await Async.Ok().Ensure(Outcome.Ok));

    [Fact]
    public async Task Ensure_None_ConstantOutcome_ShouldPropagateValidationProblem_WhenEnsureFails()
        => Assert.Equal(Outcome.OfProblem(ValidationProblem), await Async.Ok().Ensure(Outcome.OfProblem(ValidationProblem)));

    [Fact]
    public async Task Ensure_None_ConstantOutcome_ShouldPropagateProblem_WhenProblem()
        => Assert.Equal(Outcome.OfProblem(TestProblem), await Async.OfProblem(TestProblem).Ensure(Outcome.OfProblem(ValidationProblem)));

    // ----- AsyncOutcome<None>.Ensure(Func<Outcome<None>>) -----

    [Fact]
    public async Task Ensure_None_Func_ShouldReturnOriginal_WhenEnsureSucceeds()
        => Assert.Equal(Outcome.Ok, await Async.Ok().Ensure(() => Outcome.Ok));

    [Fact]
    public async Task Ensure_None_Func_ShouldPropagateValidationProblem_WhenEnsureFails()
        => Assert.Equal(Outcome.OfProblem(ValidationProblem), await Async.Ok().Ensure(() => Outcome.OfProblem(ValidationProblem)));

    [Fact]
    public async Task Ensure_None_Func_ShouldPropagateProblem_WhenProblem()
        => Assert.Equal(Outcome.OfProblem(TestProblem), await Async.OfProblem(TestProblem).Ensure(() => Outcome.Ok));

    // ----- AsyncOutcome<T> -----

    [Fact]
    public async Task Ensure_ShouldReturnOriginal_WhenEnsureSucceeds()
        => Assert.Equal(Outcome.Of(10), await Async.Of(10).Ensure(_ => Outcome.Ok));

    [Fact]
    public async Task Ensure_ShouldPropagateValidationProblem_WhenEnsureFails()
        => Assert.Equal(Outcome.OfProblem<int>(ValidationProblem), await Async.Of(10).Ensure(_ => Outcome.OfProblem(ValidationProblem)));

    [Fact]
    public async Task Ensure_ShouldPropagateProblem_WhenProblem()
        => Assert.Equal(Outcome.OfProblem<int>(TestProblem), await Async.OfProblem<int>(TestProblem).Ensure(_ => Outcome.Ok));

    // ----- AsyncOutcome<(T, T1)> -----

    [Fact]
    public async Task Ensure_TwoValues_ShouldReturnOriginal_WhenEnsureSucceeds()
        => Assert.Equal(Outcome.Of((1, 2)), await Async.Of((1, 2)).Ensure((_, _) => Outcome.Ok));

    [Fact]
    public async Task Ensure_TwoValues_ShouldPropagateValidationProblem_WhenEnsureFails()
        => Assert.Equal(Outcome.OfProblem<(int, int)>(ValidationProblem), await Async.Of((1, 2)).Ensure((_, _) => Outcome.OfProblem(ValidationProblem)));

    [Fact]
    public async Task Ensure_TwoValues_ShouldPropagateProblem_WhenProblem()
        => Assert.Equal(Outcome.OfProblem<(int, int)>(TestProblem), await Async.OfProblem<(int, int)>(TestProblem).Ensure((_, _) => Outcome.Ok));

    // ----- AsyncOutcome<(T, T1, T2)> -----

    [Fact]
    public async Task Ensure_ThreeValues_ShouldReturnOriginal_WhenEnsureSucceeds()
        => Assert.Equal(Outcome.Of((1, 2, 3)), await Async.Of((1, 2, 3)).Ensure((_, _, _) => Outcome.Ok));

    [Fact]
    public async Task Ensure_ThreeValues_ShouldPropagateValidationProblem_WhenEnsureFails()
        => Assert.Equal(Outcome.OfProblem<(int, int, int)>(ValidationProblem), await Async.Of((1, 2, 3)).Ensure((_, _, _) => Outcome.OfProblem(ValidationProblem)));

    [Fact]
    public async Task Ensure_ThreeValues_ShouldPropagateProblem_WhenProblem()
        => Assert.Equal(Outcome.OfProblem<(int, int, int)>(TestProblem), await Async.OfProblem<(int, int, int)>(TestProblem).Ensure((_, _, _) => Outcome.Ok));

    // ----- AsyncOutcome<(T, T1, T2, T3)> -----

    [Fact]
    public async Task Ensure_FourValues_ShouldReturnOriginal_WhenEnsureSucceeds()
        => Assert.Equal(Outcome.Of((1, 2, 3, 4)), await Async.Of((1, 2, 3, 4)).Ensure((_, _, _, _) => Outcome.Ok));

    [Fact]
    public async Task Ensure_FourValues_ShouldPropagateValidationProblem_WhenEnsureFails()
        => Assert.Equal(Outcome.OfProblem<(int, int, int, int)>(ValidationProblem), await Async.Of((1, 2, 3, 4)).Ensure((_, _, _, _) => Outcome.OfProblem(ValidationProblem)));

    [Fact]
    public async Task Ensure_FourValues_ShouldPropagateProblem_WhenProblem()
        => Assert.Equal(Outcome.OfProblem<(int, int, int, int)>(TestProblem), await Async.OfProblem<(int, int, int, int)>(TestProblem).Ensure((_, _, _, _) => Outcome.Ok));
}

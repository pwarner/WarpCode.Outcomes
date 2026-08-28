namespace WarpCode.Outcomes.Tests;

public class EnsureAsyncExtensionsTests
{
    private static readonly Problem TestProblem = new(nameof(TestProblem));
    private static readonly Problem ValidationProblem = new(nameof(ValidationProblem));

    // ----- AsyncOutcome<None> -----

    [Fact]
    public async Task EnsureAsync_None_ShouldReturnOriginal_WhenEnsureSucceeds()
        => Assert.Equal(Outcome.Ok, await Async.Ok().EnsureAsync(_ => ValueTask.FromResult(Outcome.Ok)));

    [Fact]
    public async Task EnsureAsync_None_ShouldPropagateValidationProblem_WhenEnsureFails()
        => Assert.Equal(Outcome.OfProblem(ValidationProblem), await Async.Ok().EnsureAsync(_ => ValueTask.FromResult(Outcome.OfProblem(ValidationProblem))));

    [Fact]
    public async Task EnsureAsync_None_ShouldPropagateProblem_WhenProblem()
        => Assert.Equal(Outcome.OfProblem(TestProblem), await Async.OfProblem(TestProblem).EnsureAsync(_ => ValueTask.FromResult(Outcome.Ok)));

    [Fact]
    public async Task EnsureTask_None_ShouldReturnOriginal_WhenEnsureSucceeds()
        => Assert.Equal(Outcome.Ok, await Async.Ok().EnsureTask(_ => Task.FromResult(Outcome.Ok)));

    [Fact]
    public async Task EnsureTask_None_ShouldPropagateValidationProblem_WhenEnsureFails()
        => Assert.Equal(Outcome.OfProblem(ValidationProblem), await Async.Ok().EnsureTask(_ => Task.FromResult(Outcome.OfProblem(ValidationProblem))));

    [Fact]
    public async Task EnsureTask_None_ShouldPropagateProblem_WhenProblem()
        => Assert.Equal(Outcome.OfProblem(TestProblem), await Async.OfProblem(TestProblem).EnsureTask(_ => Task.FromResult(Outcome.Ok)));

    // ----- AsyncOutcome<T> -----

    [Fact]
    public async Task EnsureAsync_ShouldReturnOriginal_WhenEnsureSucceeds()
        => Assert.Equal(Outcome.Of(10), await Async.Of(10).EnsureAsync((_, _) => ValueTask.FromResult(Outcome.Ok)));

    [Fact]
    public async Task EnsureAsync_ShouldPropagateValidationProblem_WhenEnsureFails()
        => Assert.Equal(Outcome.OfProblem<int>(ValidationProblem), await Async.Of(10).EnsureAsync((_, _) => ValueTask.FromResult(Outcome.OfProblem(ValidationProblem))));

    [Fact]
    public async Task EnsureAsync_ShouldPropagateProblem_WhenProblem()
        => Assert.Equal(Outcome.OfProblem<int>(TestProblem), await Async.OfProblem<int>(TestProblem).EnsureAsync((_, _) => ValueTask.FromResult(Outcome.Ok)));

    [Fact]
    public async Task EnsureTask_ShouldReturnOriginal_WhenEnsureSucceeds()
        => Assert.Equal(Outcome.Of(10), await Async.Of(10).EnsureTask((_, _) => Task.FromResult(Outcome.Ok)));

    [Fact]
    public async Task EnsureTask_ShouldPropagateValidationProblem_WhenEnsureFails()
        => Assert.Equal(Outcome.OfProblem<int>(ValidationProblem), await Async.Of(10).EnsureTask((_, _) => Task.FromResult(Outcome.OfProblem(ValidationProblem))));

    [Fact]
    public async Task EnsureTask_ShouldPropagateProblem_WhenProblem()
        => Assert.Equal(Outcome.OfProblem<int>(TestProblem), await Async.OfProblem<int>(TestProblem).EnsureTask((_, _) => Task.FromResult(Outcome.Ok)));

    // ----- AsyncOutcome<(T, T1)> -----

    [Fact]
    public async Task EnsureAsync_TwoValues_ShouldReturnOriginal_WhenEnsureSucceeds()
        => Assert.Equal(Outcome.Of((1, 2)), await Async.Of((1, 2)).EnsureAsync((_, _, _) => ValueTask.FromResult(Outcome.Ok)));

    [Fact]
    public async Task EnsureAsync_TwoValues_ShouldPropagateValidationProblem_WhenEnsureFails()
        => Assert.Equal(Outcome.OfProblem<(int, int)>(ValidationProblem), await Async.Of((1, 2)).EnsureAsync((_, _, _) => ValueTask.FromResult(Outcome.OfProblem(ValidationProblem))));

    [Fact]
    public async Task EnsureAsync_TwoValues_ShouldPropagateProblem_WhenProblem()
        => Assert.Equal(Outcome.OfProblem<(int, int)>(TestProblem), await Async.OfProblem<(int, int)>(TestProblem).EnsureAsync((_, _, _) => ValueTask.FromResult(Outcome.Ok)));

    [Fact]
    public async Task EnsureTask_TwoValues_ShouldReturnOriginal_WhenEnsureSucceeds()
        => Assert.Equal(Outcome.Of((1, 2)), await Async.Of((1, 2)).EnsureTask((_, _, _) => Task.FromResult(Outcome.Ok)));

    [Fact]
    public async Task EnsureTask_TwoValues_ShouldPropagateValidationProblem_WhenEnsureFails()
        => Assert.Equal(Outcome.OfProblem<(int, int)>(ValidationProblem), await Async.Of((1, 2)).EnsureTask((_, _, _) => Task.FromResult(Outcome.OfProblem(ValidationProblem))));

    [Fact]
    public async Task EnsureTask_TwoValues_ShouldPropagateProblem_WhenProblem()
        => Assert.Equal(Outcome.OfProblem<(int, int)>(TestProblem), await Async.OfProblem<(int, int)>(TestProblem).EnsureTask((_, _, _) => Task.FromResult(Outcome.Ok)));

    // ----- AsyncOutcome<(T, T1, T2)> -----

    [Fact]
    public async Task EnsureAsync_ThreeValues_ShouldReturnOriginal_WhenEnsureSucceeds()
        => Assert.Equal(Outcome.Of((1, 2, 3)), await Async.Of((1, 2, 3)).EnsureAsync((_, _, _, _) => ValueTask.FromResult(Outcome.Ok)));

    [Fact]
    public async Task EnsureAsync_ThreeValues_ShouldPropagateValidationProblem_WhenEnsureFails()
        => Assert.Equal(Outcome.OfProblem<(int, int, int)>(ValidationProblem), await Async.Of((1, 2, 3)).EnsureAsync((_, _, _, _) => ValueTask.FromResult(Outcome.OfProblem(ValidationProblem))));

    [Fact]
    public async Task EnsureAsync_ThreeValues_ShouldPropagateProblem_WhenProblem()
        => Assert.Equal(Outcome.OfProblem<(int, int, int)>(TestProblem), await Async.OfProblem<(int, int, int)>(TestProblem).EnsureAsync((_, _, _, _) => ValueTask.FromResult(Outcome.Ok)));

    [Fact]
    public async Task EnsureTask_ThreeValues_ShouldReturnOriginal_WhenEnsureSucceeds()
        => Assert.Equal(Outcome.Of((1, 2, 3)), await Async.Of((1, 2, 3)).EnsureTask((_, _, _, _) => Task.FromResult(Outcome.Ok)));

    [Fact]
    public async Task EnsureTask_ThreeValues_ShouldPropagateValidationProblem_WhenEnsureFails()
        => Assert.Equal(Outcome.OfProblem<(int, int, int)>(ValidationProblem), await Async.Of((1, 2, 3)).EnsureTask((_, _, _, _) => Task.FromResult(Outcome.OfProblem(ValidationProblem))));

    [Fact]
    public async Task EnsureTask_ThreeValues_ShouldPropagateProblem_WhenProblem()
        => Assert.Equal(Outcome.OfProblem<(int, int, int)>(TestProblem), await Async.OfProblem<(int, int, int)>(TestProblem).EnsureTask((_, _, _, _) => Task.FromResult(Outcome.Ok)));

    // ----- AsyncOutcome<(T, T1, T2, T3)> -----

    [Fact]
    public async Task EnsureAsync_FourValues_ShouldReturnOriginal_WhenEnsureSucceeds()
        => Assert.Equal(Outcome.Of((1, 2, 3, 4)), await Async.Of((1, 2, 3, 4)).EnsureAsync((_, _, _, _, _) => ValueTask.FromResult(Outcome.Ok)));

    [Fact]
    public async Task EnsureAsync_FourValues_ShouldPropagateValidationProblem_WhenEnsureFails()
        => Assert.Equal(Outcome.OfProblem<(int, int, int, int)>(ValidationProblem), await Async.Of((1, 2, 3, 4)).EnsureAsync((_, _, _, _, _) => ValueTask.FromResult(Outcome.OfProblem(ValidationProblem))));

    [Fact]
    public async Task EnsureAsync_FourValues_ShouldPropagateProblem_WhenProblem()
        => Assert.Equal(Outcome.OfProblem<(int, int, int, int)>(TestProblem), await Async.OfProblem<(int, int, int, int)>(TestProblem).EnsureAsync((_, _, _, _, _) => ValueTask.FromResult(Outcome.Ok)));

    [Fact]
    public async Task EnsureTask_FourValues_ShouldReturnOriginal_WhenEnsureSucceeds()
        => Assert.Equal(Outcome.Of((1, 2, 3, 4)), await Async.Of((1, 2, 3, 4)).EnsureTask((_, _, _, _, _) => Task.FromResult(Outcome.Ok)));

    [Fact]
    public async Task EnsureTask_FourValues_ShouldPropagateValidationProblem_WhenEnsureFails()
        => Assert.Equal(Outcome.OfProblem<(int, int, int, int)>(ValidationProblem), await Async.Of((1, 2, 3, 4)).EnsureTask((_, _, _, _, _) => Task.FromResult(Outcome.OfProblem(ValidationProblem))));

    [Fact]
    public async Task EnsureTask_FourValues_ShouldPropagateProblem_WhenProblem()
        => Assert.Equal(Outcome.OfProblem<(int, int, int, int)>(TestProblem), await Async.OfProblem<(int, int, int, int)>(TestProblem).EnsureTask((_, _, _, _, _) => Task.FromResult(Outcome.Ok)));

    // ----- CancellationToken flow -----

    [Fact]
    public async Task EnsureAsync_ShouldPassOutcomeToken_ToDelegate()
    {
        using var cts = new CancellationTokenSource();
        CancellationToken seen = default;

        await AsyncOutcome.Of(10, cts.Token).EnsureAsync((_, ct) => { seen = ct; return ValueTask.FromResult(Outcome.Ok); });

        Assert.Equal(cts.Token, seen);
    }

    [Fact]
    public async Task EnsureTask_ShouldPassOutcomeToken_ToDelegate()
    {
        using var cts = new CancellationTokenSource();
        CancellationToken seen = default;

        await AsyncOutcome.Of(10, cts.Token).EnsureTask((_, ct) => { seen = ct; return Task.FromResult(Outcome.Ok); });

        Assert.Equal(cts.Token, seen);
    }
}

namespace WarpCode.Outcomes.Tests;

public class AsyncThenExtensionsTests
{
    private static readonly Problem TestProblem = new(nameof(TestProblem));

    // ----- AsyncOutcome<None> -----

    [Fact]
    public async Task Then_None_ConstantValue_ShouldReturnValue_WhenSuccess()
        => Assert.Equal(Outcome.Of(5), await Async.Ok().Then(5));

    [Fact]
    public async Task Then_None_ConstantValue_ShouldPropagateProblem_WhenProblem()
        => Assert.Equal(Outcome.OfProblem<int>(TestProblem), await Async.OfProblem(TestProblem).Then(5));

    [Fact]
    public async Task Then_None_ConstantOutcome_ShouldReturnOutcome_WhenSuccess()
        => Assert.Equal(Outcome.Of(5), await Async.Ok().Then(Outcome.Of(5)));

    [Fact]
    public async Task Then_None_ConstantOutcome_ShouldPropagateProblem_WhenProblem()
        => Assert.Equal(Outcome.OfProblem<int>(TestProblem), await Async.OfProblem(TestProblem).Then(Outcome.Of(5)));

    [Fact]
    public async Task Then_None_FuncValue_ShouldReturnValue_WhenSuccess()
        => Assert.Equal(Outcome.Of(5), await Async.Ok().Then(() => 5));

    [Fact]
    public async Task Then_None_FuncValue_ShouldPropagateProblem_WhenProblem()
        => Assert.Equal(Outcome.OfProblem<int>(TestProblem), await Async.OfProblem(TestProblem).Then(() => 5));

    [Fact]
    public async Task Then_None_FuncOutcome_ShouldReturnOutcome_WhenSuccess()
        => Assert.Equal(Outcome.Of(5), await Async.Ok().Then(() => Outcome.Of(5)));

    [Fact]
    public async Task Then_None_FuncOutcome_ShouldPropagateProblem_WhenProblem()
        => Assert.Equal(Outcome.OfProblem<int>(TestProblem), await Async.OfProblem(TestProblem).Then(() => Outcome.Of(5)));

    // ----- AsyncOutcome<T> -----

    [Fact]
    public async Task Then_Map_ShouldProjectValue_WhenSuccess()
        => Assert.Equal(Outcome.Of(20), await Async.Of(10).Then(v => v * 2));

    [Fact]
    public async Task Then_Map_ShouldPropagateProblem_WhenProblem()
        => Assert.Equal(Outcome.OfProblem<int>(TestProblem), await Async.OfProblem<int>(TestProblem).Then(v => v * 2));

    [Fact]
    public async Task Then_Bind_ShouldProduceNextOutcome_WhenSuccess()
        => Assert.Equal(Outcome.Of(20), await Async.Of(10).Then(v => Outcome.Of(v * 2)));

    [Fact]
    public async Task Then_Bind_ShouldPropagateProblem_WhenProblem()
        => Assert.Equal(Outcome.OfProblem<int>(TestProblem), await Async.OfProblem<int>(TestProblem).Then(v => Outcome.Of(v * 2)));

    // ----- AsyncOutcome<(T, T1)> -----

    [Fact]
    public async Task Then_Map_TwoValues_ShouldSpreadAndProject_WhenSuccess()
        => Assert.Equal(Outcome.Of(3), await Async.Of((1, 2)).Then((a, b) => a + b));

    [Fact]
    public async Task Then_Map_TwoValues_ShouldPropagateProblem_WhenProblem()
        => Assert.Equal(Outcome.OfProblem<int>(TestProblem), await Async.OfProblem<(int, int)>(TestProblem).Then((a, b) => a + b));

    [Fact]
    public async Task Then_Bind_TwoValues_ShouldSpreadAndProject_WhenSuccess()
        => Assert.Equal(Outcome.Of(3), await Async.Of((1, 2)).Then((a, b) => Outcome.Of(a + b)));

    [Fact]
    public async Task Then_Bind_TwoValues_ShouldPropagateProblem_WhenProblem()
        => Assert.Equal(Outcome.OfProblem<int>(TestProblem), await Async.OfProblem<(int, int)>(TestProblem).Then((a, b) => Outcome.Of(a + b)));

    // ----- AsyncOutcome<(T, T1, T2)> -----

    [Fact]
    public async Task Then_Map_ThreeValues_ShouldSpreadAndProject_WhenSuccess()
        => Assert.Equal(Outcome.Of(6), await Async.Of((1, 2, 3)).Then((a, b, c) => a + b + c));

    [Fact]
    public async Task Then_Map_ThreeValues_ShouldPropagateProblem_WhenProblem()
        => Assert.Equal(Outcome.OfProblem<int>(TestProblem), await Async.OfProblem<(int, int, int)>(TestProblem).Then((a, b, c) => a + b + c));

    [Fact]
    public async Task Then_Bind_ThreeValues_ShouldSpreadAndProject_WhenSuccess()
        => Assert.Equal(Outcome.Of(6), await Async.Of((1, 2, 3)).Then((a, b, c) => Outcome.Of(a + b + c)));

    [Fact]
    public async Task Then_Bind_ThreeValues_ShouldPropagateProblem_WhenProblem()
        => Assert.Equal(Outcome.OfProblem<int>(TestProblem), await Async.OfProblem<(int, int, int)>(TestProblem).Then((a, b, c) => Outcome.Of(a + b + c)));

    // ----- AsyncOutcome<(T, T1, T2, T3)> -----

    [Fact]
    public async Task Then_Map_FourValues_ShouldSpreadAndProject_WhenSuccess()
        => Assert.Equal(Outcome.Of(10), await Async.Of((1, 2, 3, 4)).Then((a, b, c, d) => a + b + c + d));

    [Fact]
    public async Task Then_Map_FourValues_ShouldPropagateProblem_WhenProblem()
        => Assert.Equal(Outcome.OfProblem<int>(TestProblem), await Async.OfProblem<(int, int, int, int)>(TestProblem).Then((a, b, c, d) => a + b + c + d));

    [Fact]
    public async Task Then_Bind_FourValues_ShouldSpreadAndProject_WhenSuccess()
        => Assert.Equal(Outcome.Of(10), await Async.Of((1, 2, 3, 4)).Then((a, b, c, d) => Outcome.Of(a + b + c + d)));

    [Fact]
    public async Task Then_Bind_FourValues_ShouldPropagateProblem_WhenProblem()
        => Assert.Equal(Outcome.OfProblem<int>(TestProblem), await Async.OfProblem<(int, int, int, int)>(TestProblem).Then((a, b, c, d) => Outcome.Of(a + b + c + d)));
}

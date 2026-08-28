namespace WarpCode.Outcomes.Tests;

public class AsyncRescueExtensionsTests
{
    private static readonly Problem TestProblem = new(nameof(TestProblem));
    private static readonly Problem RescueProblem = new(nameof(RescueProblem));

    [Fact]
    public async Task Rescue_ShouldReturnOriginal_WhenSuccess()
        => Assert.Equal(Outcome.Of(10), await Async.Of(10).Rescue(_ => Outcome.Of(99)));

    [Fact]
    public async Task Rescue_ShouldRecoverToValue_WhenProblem()
        => Assert.Equal(Outcome.Of(10), await Async.OfProblem<int>(TestProblem).Rescue(_ => Outcome.Of(10)));

    [Fact]
    public async Task Rescue_ShouldPropagateNewProblem_WhenRescueFails()
        => Assert.Equal(Outcome.OfProblem<int>(RescueProblem), await Async.OfProblem<int>(TestProblem).Rescue(_ => Outcome.OfProblem<int>(RescueProblem)));

    [Fact]
    public async Task Rescue_ShouldPropagateSameProblem_WhenProblemIsUnrecoverable()
        => Assert.Equal(Outcome.OfProblem<int>(TestProblem), await Async.OfProblem<int>(TestProblem).Rescue(p => Outcome.OfProblem<int>(p)));
}

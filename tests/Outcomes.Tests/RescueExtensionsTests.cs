namespace WarpCode.Outcomes.Tests;

public class RescueExtensionsTests
{
    private static readonly Problem TestProblem = new(nameof(TestProblem));
    private static readonly Problem RescueProblem = new(nameof(RescueProblem));

    [Fact]
    public void Rescue_ShouldReturnOriginal_WhenSuccess()
        => Assert.Equal(Outcome.Of(10), Outcome.Of(10).Rescue(_ => Outcome.Of(99)));

    [Fact]
    public void Rescue_ShouldRecoverToValue_WhenProblem()
        => Assert.Equal(Outcome.Of(10), Outcome.OfProblem<int>(TestProblem).Rescue(_ => Outcome.Of(10)));

    [Fact]
    public void Rescue_ShouldPropagateNewProblem_WhenRescueFails()
        => Assert.Equal(Outcome.OfProblem<int>(RescueProblem), Outcome.OfProblem<int>(TestProblem).Rescue(_ => Outcome.OfProblem<int>(RescueProblem)));

    [Fact]
    public void Rescue_ShouldPropagateSameProblem_WhenProblemIsUnrecoverable()
        => Assert.Equal(Outcome.OfProblem<int>(TestProblem), Outcome.OfProblem<int>(TestProblem).Rescue(p => Outcome.OfProblem<int>(p)));
}

namespace WarpCode.Outcomes.Tests;

public class OutcomeTests
{
    private static readonly Problem TestProblem = new(nameof(TestProblem));

    [Fact]
    public void Should_CreateOutcome_FromValueImplicitly() => Assert.Equal(new Outcome<int>(10), 10);

    [Fact]
    public void Should_CreateOutcome_FromOfEntryHelper() => Assert.Equal(new Outcome<int>(10), Outcome.Of(10));

    [Fact]
    public void Should_CreateValuelessOutcome_FromOkEntryHelper() => Assert.Equal(new Outcome<None>(default(None)), Outcome.Ok);

    [Fact]
    public void Should_CreateProblemOutcome_Implicitly() => Assert.Equal(new Outcome<None>(TestProblem), TestProblem);

    [Fact]
    public void Should_CreateProblemOutcome_FromToOutcomeExtension() => Assert.Equal(new Outcome<None>(TestProblem), Outcome.OfProblem(TestProblem));

    [Fact]
    public void Should_CreateStronglyTypedProblemOutcome_FromToOutcomeExtension() => Assert.Equal(new Outcome<int>(TestProblem), Outcome.OfProblem<int>(TestProblem));

    [Fact]
    public void Should_CreateProblemOutcome_FromProblemHelper() => Assert.Equal(new Outcome<None>(TestProblem), Outcome.OfProblem(TestProblem));

    [Fact]
    public void Should_CreateStronglyTypedProblemOutcome_ProblemHelper() => Assert.Equal(new Outcome<int>(TestProblem), Outcome.OfProblem<int>(TestProblem));

    [Fact]
    public void Match_ShouldResolveWithOnSuccessFunction_WhenNoProblem() => Assert.True(Outcome.Ok.Match(_ => true, _ => false));

    [Fact]
    public void Match_ShouldResolveWithOnProblemFunction_WhenProblem() => Assert.True(new Outcome<None>(TestProblem).Match(_ => false, _ => true));
}

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

    [Fact]
    public void ParameterlessConstructor_ShouldThrow() => Assert.Throws<InvalidOperationException>(() => new Outcome<int>());

    [Fact]
    public void Constructor_ShouldThrow_WhenProblemIsNull()
        => Assert.Equal("problem", Assert.Throws<ArgumentNullException>(() => new Outcome<int>((Problem)null!)).ParamName);

    [Fact]
    public async Task AsAsync_ShouldWrapTheOutcome()
        => Assert.Equal(Outcome.Of(10), await Outcome.Of(10).AsAsync(TestContext.Current.CancellationToken));

    [Fact]
    public void ToString_ShouldDescribeValue() => Assert.Equal("Value: 10", Outcome.Of(10).ToString());

    [Fact]
    public void ToString_ShouldDescribeProblem()
        => Assert.Equal("Problem: Problem { Detail = TestProblem }", Outcome.OfProblem<int>(TestProblem).ToString());
}

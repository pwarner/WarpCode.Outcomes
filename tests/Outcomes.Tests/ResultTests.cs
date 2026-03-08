namespace WarpCode.Outcomes.Tests;

public class ResultTests
{
    private static readonly Problem TestProblem = new(nameof(TestProblem));

    [Fact]
    public void Should_CreateResult_FromValueImplicitly() => Assert.Equal(new Result<int>(10), 10);

    [Fact]
    public void Should_CreateResult_FromOfEntryHelper() => Assert.Equal(new Result<int>(10), Result.From(10));

    [Fact]
    public void Should_CreateValuelessResult_FromOkEntryHelper() => Assert.Equal(new Result<None>(default(None)), Result.Ok);

    [Fact]
    public void Should_CreateProblemResult_Implicitly() => Assert.Equal(new Result<None>(TestProblem), TestProblem);

    [Fact]
    public void Should_CreateProblemResult_FromToResultExtension() => Assert.Equal(new Result<None>(TestProblem), Result.FromProblem(TestProblem));

    [Fact]
    public void Should_CreateStronglyTypedProblemResult_FromToResultExtension() => Assert.Equal(new Result<int>(TestProblem), Result.FromProblem<int>(TestProblem));

    [Fact]
    public void Should_CreateProblemResult_FromProblemHelper() => Assert.Equal(new Result<None>(TestProblem), Result.FromProblem(TestProblem));

    [Fact]
    public void Should_CreateStronglyTypedProblemResult_ProblemHelper() => Assert.Equal(new Result<int>(TestProblem), Result.FromProblem<int>(TestProblem));

    [Fact]
    public void Match_ShouldResolveWithOnSuccessFunction_WhenNoProblem() => Assert.True(Result.Ok.Match(_ => true, _ => false));

    [Fact]
    public void Match_ShouldResolveWithOnProblemFunction_WhenProblem() => Assert.True(new Result<None>(TestProblem).Match(_ => false, _ => true));
}

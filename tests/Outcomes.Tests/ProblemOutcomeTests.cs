namespace Outcomes.Tests;

public class ProblemOutcomeTests
{
    private static readonly Problem TestProblem = new(nameof(TestProblem));

    private IProblem? _actual;

    [Fact]
    public void Match_Problem_ShouldCallProblemHandler()
    {
        var testable = new Outcome<None>(TestProblem);

        bool result = testable.Resolve(OnNoProblem, OnProblem);

        AssertProblemWasMatched(result);
    }

    [Fact]
    public void Should_CreateProblemOutcome_Implicitly()
    {
        var expected = new Outcome<None>(TestProblem);

        Outcome<None> actual = TestProblem;

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Should_CreateProblemOutcome_FromToOutcomeExtension()
    {
        var expected = new Outcome<int>(TestProblem);

        Outcome<int> actual = TestProblem.ToOutcome();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Should_CreateProblemOutcome_FromStrongTypedToOutcomeExtension()
    {
        var expected = new Outcome<int>(TestProblem);

        Outcome<int> actual = TestProblem.ToOutcome<int>();

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData('A', "Error_A")]
    [InlineData('B', "Error_B")]
    [InlineData('C', "Error_C")]
    public void ComposedOutcome_ShouldBreakChainWhenProblemOccurs(char code, string message)
    {
        Outcome<None> testable =
            from a in CreateOutcome(code == 'A', code)
            from b in CreateOutcome(code == 'B', code)
            from c in CreateOutcome(code == 'C', code)
            from _ in Outcome.NoProblem
            select default(None);

        bool result = testable.Resolve(OnNoProblem, OnProblem);

        AssertProblemWasMatched(result, message);
    }

    private bool OnProblem(IProblem p)
    {
        _actual = p;
        return true;
    }

    private static bool OnNoProblem(None _) => false;

    private void AssertProblemWasMatched(bool result, string? expectedMessage = null)
    {
        Assert.True(result);
        Assert.NotNull(_actual);
        Assert.Equal(expectedMessage ?? TestProblem.Detail, _actual?.Detail);
    }

    private static Outcome<None> CreateOutcome(bool isProblem, char c) =>
        isProblem ? new Problem($"Error_{c}") : Outcome.NoProblem;
}

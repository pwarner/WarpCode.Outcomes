namespace Matterlab.Rails.Tests;

public class ProblemOutcomeTests
{
    private static readonly Problem TestProblem = new(nameof(TestProblem));

    private Problem? _actual;

    [Fact]
    public void Match_Problem_ShouldCallProblemHandler()
    {
        var testable = new Outcome<None>(TestProblem);

        bool result = testable.Match(OnNoProblem, OnProblem);

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
    public void Should_CreateProblemOutcome_FromEntryHelper()
    {
        var expected = new Outcome<None>(TestProblem);

        Outcome<None> actual = Outcome.Problem(TestProblem);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Should_CreateProblemOutcome_FromStrongTypedEntryHelper()
    {
        var expected = new Outcome<int>(TestProblem);

        Outcome<int> actual = Outcome.Problem<int>(TestProblem);

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

        bool result = testable.Match(OnNoProblem, OnProblem);

        AssertProblemWasMatched(result, message);
    }

    private bool OnProblem(Problem p)
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

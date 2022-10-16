namespace Outcomes.Tests;

public class SuccessOutcomeTests
{
    private string? _actual;

    private static readonly string TestValue = nameof(TestValue);

    [Fact]
    public void Match_Success_ShouldCallSuccessHandler()
    {
        var testable = new Outcome<string>(TestValue);

        bool result = testable.Match(OnNoProblem, OnProblem);

        AssertSuccessWasMatched(result);
    }

    [Fact]
    public void Should_CreateSuccessOutcome_Implicitly()
    {
        var expected = new Outcome<string>(TestValue);

        Outcome<string> actual = TestValue;

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Should_CreateSuccessOutcome_FromEntryHelper()
    {
        var expected = new Outcome<None>();

        Outcome<None> actual = Outcome.NoProblem;

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Should_CreateSuccessOutcome_FromStrongTypedEntryHelper()
    {
        var expected = new Outcome<string>(TestValue);

        Outcome<string> actual = Outcome.Ok(TestValue);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ComposedOutcome_ShouldCompleteChainWhenNoProblemOccurs()
    {
        Outcome<string> testable =
            from a in Outcome.Ok('A')
            from b in Outcome.Ok('B')
            from c in Outcome.Ok('C')
            select $"{a}{b}{c}";

        bool result = testable.Match(OnNoProblem, OnProblem);

        AssertSuccessWasMatched(result, "ABC");
    }

    private static bool OnProblem(IProblem _) => false;

    private bool OnNoProblem(string value)
    {
        _actual = value;
        return true;
    }

    private void AssertSuccessWasMatched(bool result, string? expectedValue = null)
    {
        Assert.True(result);
        Assert.NotNull(_actual);
        Assert.Equal(expectedValue ?? TestValue, _actual);
    }
}

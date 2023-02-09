namespace Outcomes.Tests;

public class OutcomeCreationTests
{
    private static readonly string TestValue = nameof(TestValue);
    private static readonly Problem TestProblem = new(nameof(TestProblem));

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
        var expected = new Outcome<string>();

        Outcome<string> actual = Outcome.Ok();

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
}

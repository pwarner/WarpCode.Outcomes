namespace Outcomes.Tests;

public class OutcomeCreationTests
{
    private const string TestValue = nameof(TestValue);
    private static readonly Problem TestProblem = new(nameof(TestProblem));

    [Fact]
    public void Should_CreateSuccessOutcome_Implicitly()
    {
        Assert.Equal(new Outcome<string>(TestValue), TestValue);
    }

    [Fact]
    public void Should_CreateSuccessOutcome_FromEntryHelper()
    {
        Assert.Equal(new Outcome<string>(), Outcome.Ok());
    }

    [Fact]
    public void Should_CreateSuccessOutcome_FromStrongTypedEntryHelper()
    {
        Assert.Equal(new Outcome<string>(TestValue), Outcome.Ok(TestValue));
    }

    [Fact]
    public void Should_CreateProblemOutcome_Implicitly()
    {
        Assert.Equal(new Outcome<None>(TestProblem), TestProblem);
    }

    [Fact]
    public void Should_CreateProblemOutcome_FromToOutcomeExtension()
    {
        Assert.Equal(new Outcome<int>(TestProblem), TestProblem.ToOutcome());
    }

    [Fact]
    public void Should_CreateProblemOutcome_FromStrongTypedToOutcomeExtension()
    {
        Assert.Equal(new Outcome<int>(TestProblem), TestProblem.ToOutcome<int>());
    }
}

namespace Outcomes.Tests;

public class OutcomeCreationTests
{
    private const string TestValue = nameof(TestValue);
    private static readonly Problem TestProblem = new(nameof(TestProblem));

    [Fact]
    public void Should_CreateSuccessOutcome_Implicitly()
    {
        Assert.Equal(new Outcome<int>(10), 10);
    }

    [Fact]
    public void Should_CreateSuccessOutcome_FromStrongTypedEntryHelper()
    {
        Assert.Equal(new Outcome<int>(10), Outcome.Ok(10));
    }

    [Fact]
    public void Should_CreateSuccessOutcome_FromEntryHelper()
    {
        Assert.Equal(new Outcome<None>(), Outcome.Ok());
    }

    [Fact]
    public void Should_CreateStronglyTypedSuccessOutcome_FromEntryHelper()
    {
        Assert.Equal(new Outcome<int>(), Outcome.Ok());
    }

    [Fact]
    public void Should_CreateProblemOutcome_Implicitly()
    {
        Assert.Equal(new Outcome<None>(TestProblem), TestProblem);
    }

    [Fact]
    public void Should_CreateProblemOutcome_FromEntryHelper()
    {
        Assert.Equal(new Outcome<None>(TestProblem), Outcome.Problem(TestProblem));
    }
}

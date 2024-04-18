namespace Outcomes.Tests;

public abstract class CompositionTestBase
{
    protected const string Success = "success";
    private int _steps;

    protected void AssertExpectedOutcome(int failValue, Outcome<string> composition)
    {
        (int expectedSteps, string? expectedFinal) = failValue switch
        {
            1 => (0, "Problem1"),
            2 => (1, "Problem2"),
            _ => (2, Success)
        };

        string actual = composition.Match(_ => Success, p => p.Detail);
        Assert.Equal(expectedFinal, actual);
        Assert.Equal(expectedSteps, _steps);
    }

    protected void AssertExpectedMapOutcome(int failValue, Outcome<string> composition)
    {
        (int expectedSteps, string? expectedFinal) = failValue switch
        {
            1 => (0, "Problem1"),
            _ => (1, Success)
        };

        string actual = composition.Match(value => value, p => p.Detail);
        Assert.Equal(expectedFinal, actual);
        Assert.Equal(expectedSteps, _steps);
    }

    protected Outcome<string> FirstOutcome(int value)
    {
        if (value == 1)
            return new Problem("Problem1");

        _steps++;
        return Success;
    }

    protected Outcome<None> NextOutcome(int value)
    {
        if (value == 2)
            return new Problem("Problem2");

        _steps++;
        return Outcome.Ok();
    }
}

namespace Outcomes.Tests;

public abstract class CompositionTestBase
{
    protected const string Success = "success";
    private int _steps;

    protected void AssertExpectedBind(int failValue, Outcome<None> composition)
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

    protected void AssertExpectedMap(int failValue, Outcome<string> composition)
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

    protected Outcome<None> OutcomeFactory(int failValue, int step)
    {
        if (failValue == step)
            return new Problem($"Problem{failValue}");

        _steps++;
        return Outcome.Ok();
    }

    private (int expectedSteps, string? expectedFinal) GetExpectations(int failValue) =>
        failValue switch
        {
            1 => (0, "Problem1"),
            2 => (1, "Problem2"),
            _ => (2, Success)
        };
}

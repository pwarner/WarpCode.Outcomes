namespace Outcomes.Tests;

public abstract class CompositionTestBase
{
    protected const string Success = "success";
    protected static readonly Problem TestProblem1 = new("ruh roh!");
    protected static readonly Problem TestProblem2 = new("dammit!");

    protected static void AssertExpectedOutcome(ProblemStep step, Outcome<string> composition)
    {
        string actual = composition.Match(value => value, p => p.Detail);
        string expected = step switch
        {
            ProblemStep.First => TestProblem1.Detail,
            ProblemStep.Second => TestProblem2.Detail,
            _ => Success
        };

        Assert.Equal(expected, actual);
    }

    protected static Outcome<None> FirstOutcome(ProblemStep step) =>
        step is ProblemStep.First ? TestProblem1 : Outcome.Ok;

    protected static Outcome<string> NextOutcome(ProblemStep step) =>
        step is ProblemStep.Second ? TestProblem2 : Success;
}

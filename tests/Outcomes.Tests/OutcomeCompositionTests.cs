namespace Outcomes.Tests;

public class OutcomeCompositionTests
{
    [Theory]
    [InlineData('A', "Problem_A")]
    [InlineData('B', "Problem_BA")]
    [InlineData('C', "Problem_CBA")]
    [InlineData('X', "CBA")]
    public void CompositionWithThen_ShouldCompleteOrShortCircuit(char code, string expected)
    {
        Outcome<string> testable =
            CreateOutcome(code == 'A', "A")
                .Then(a => CreateOutcome(code == 'B', $"B{a}"))
                .Then(ba => CreateOutcome(code == 'C', $"C{ba}"));

        string result = testable.Resolve(x => x, p => p.Detail);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData('A', "Problem_A")]
    [InlineData('B', "Problem_BA")]
    [InlineData('C', "Problem_CBA")]
    [InlineData('X', "CBA")]
    public void CompositionWithLinq_ShouldCompleteOrShortCircuit(char code, string expected)
    {
        Outcome<string> testable =
            from a in CreateOutcome(code == 'A', "A")
            from ba in CreateOutcome(code == 'B', $"B{a}")
            from cba in CreateOutcome(code == 'C', $"C{ba}")
            select cba;

        string result = testable.Resolve(x => x, p => p.Detail);

        Assert.Equal(expected, result);
    }

    private static Outcome<string> CreateOutcome(bool isProblem, string value) =>
        isProblem ? new Problem($"Problem_{value}") : Outcome.Ok(value);
}

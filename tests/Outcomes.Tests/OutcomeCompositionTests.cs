namespace Outcomes.Tests;

public class OutcomeCompositionTests
{
    [Theory]
    [InlineData('A', "Problem_A")]
    [InlineData('B', "Problem_AB")]
    [InlineData('C', "Problem_ABC")]
    [InlineData('X', "ABC")]
    public void CompositionWithThen_ShouldCompleteOrShortCircuit(char code, string expected)
    {
        Outcome<string> testable =
            CreateOutcome(code == 'A', "A")
                .Then(a => CreateOutcome(code == 'B', $"{a}B"))
                .Then(ab => CreateOutcome(code == 'C', $"{ab}C"));

        string result = testable switch
        {
            { Problem: null } => testable.Value,
            { Problem: not null } => testable.Problem.Detail,
        };

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData('A', "Problem_A")]
    [InlineData('B', "Problem_AB")]
    [InlineData('C', "Problem_ABC")]
    [InlineData('X', "ABC")]
    public void CompositionWithLinq_ShouldCompleteOrShortCircuit(char code, string expected)
    {
        Outcome<string> testable =
            from a in CreateOutcome(code == 'A', "A")
            from ab in CreateOutcome(code == 'B', $"{a}B")
            from abc in CreateOutcome(code == 'C', $"{ab}C")
            select abc;

        string result = testable switch
        {
            { Problem: null } => testable.Value,
            { Problem: not null } => testable.Problem.Detail,
        };

        Assert.Equal(expected, result);
    }

    private static Outcome<string> CreateOutcome(bool isProblem, string value) =>
        isProblem ? new Problem($"Problem_{value}") : Outcome.Ok(value);
}

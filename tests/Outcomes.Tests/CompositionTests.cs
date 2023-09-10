namespace Outcomes.Tests;

public class CompositionTests
{
    private const string Success = "success";
    private int _invocations;

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task Should_ComposeStartingWithOutcome(int value)
    {
        Task<Outcome<int>> composition =
            from x in OutcomeFactory(value, 1)
            from y in Task.FromResult(OutcomeFactory(x, 2))
            from z in ValueTask.FromResult(OutcomeFactory(y, 3))
            select z;

        await AssertExpectedComposition(value, composition);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task Should_ComposeStartingWithTaskOfOutcome(int value)
    {
        Task<Outcome<int>> composition =
            from x in Task.FromResult(OutcomeFactory(value, 1))
            from y in ValueTask.FromResult(OutcomeFactory(x, 2))
            from z in OutcomeFactory(y, 3)
            select z;

        await AssertExpectedComposition(value, composition);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task Should_ComposeStartingWithValueTaskOfOutcome(int value)
    {
        Task<Outcome<int>> composition =
            from x in ValueTask.FromResult(OutcomeFactory(value, 1))
            from y in OutcomeFactory(x, 2)
            from z in Task.FromResult(OutcomeFactory(y, 3))
            select z;

        await AssertExpectedComposition(value, composition);
    }

    private async Task AssertExpectedComposition(int value, Task<Outcome<int>> composition)
    {
        string actual = await composition.MatchAsync(_ => Success, p => p.Detail);
        string expected = value == 0 ? Success : $"Problem{value}";
        Assert.Equal(expected, actual);

        int expectedInvocations = value == 0 ? 3 : value;
        Assert.Equal(expectedInvocations, _invocations);
    }

    private Outcome<int> OutcomeFactory(int value, int problemValue)
    {
        _invocations++;
        return value == problemValue ? new Problem($"Problem{value}") : value;
    }
}

namespace Outcomes.Tests;

public class CompositionTests : CompositionTestBase
{
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    public void ShouldMapOutcome(int value)
    {
        Outcome<string> composition =
            FirstOutcome(value)
                .Then(first => first);

        AssertExpectedMapOutcome(value, composition);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    public async Task ShouldMapOutcomeTask(int value)
    {
        Task<Outcome<string>> composition =
            Task.FromResult(FirstOutcome(value))
                .ThenAsync(first => first);

        AssertExpectedMapOutcome(value, await composition);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    public async Task ShouldMapOutcomeValueTask(int value)
    {
        Outcome<string> composition = await
            ValueTask.FromResult(FirstOutcome(value))
                .ThenAsync(first => first);

        AssertExpectedMapOutcome(value, composition);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void ShouldComposeOutcomeAndOutcome(int value)
    {
        Outcome<string> composition =
            FirstOutcome(value)
                .Then(_ => NextOutcome(value));

        AssertExpectedOutcome(value, composition);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public async Task ShouldComposeOutcomeAndOutcomeTask(int value)
    {
        Outcome<string> composition = await
            FirstOutcome(value)
                .ThenAsync(first => Task.FromResult(NextOutcome(value)).ThenAsync(_ => first));

        AssertExpectedOutcome(value, composition);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public async Task ShouldComposeOutcomeAndOutcomeValueTask(int value)
    {
        Outcome<string> composition = await
            FirstOutcome(value)
                .ThenAsync(first => ValueTask.FromResult(NextOutcome(value)).ThenAsync(_ => first));

        AssertExpectedOutcome(value, composition);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public async Task ShouldComposeOutcomeTaskAndOutcome(int value)
    {
        Outcome<string> composition = await
            Task.FromResult(FirstOutcome(value))
                .ThenAsync(first => NextOutcome(value).Then(_ => first));

        AssertExpectedOutcome(value, composition);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public async Task ShouldComposeOutcomeTaskAndOutcomeTask(int value)
    {
        Outcome<string> composition = await
            Task.FromResult(FirstOutcome(value))
                .ThenAsync(first => Task.FromResult(NextOutcome(value)).ThenAsync(_ => first));

        AssertExpectedOutcome(value, composition);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public async Task ShouldComposeOutcomeTaskAndOutcomeValueTask(int value)
    {
        Outcome<string> composition = await
            Task.FromResult(FirstOutcome(value))
                .ThenAsync(first => ValueTask.FromResult(NextOutcome(value)).ThenAsync(_ => first));

        AssertExpectedOutcome(value, composition);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public async Task ShouldComposeOutcomeValueTaskAndOutcome(int value)
    {
        Outcome<string> composition = await
            ValueTask.FromResult(FirstOutcome(value))
                .ThenAsync(first => NextOutcome(value).Then(_ => first));

        AssertExpectedOutcome(value, composition);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public async Task ShouldComposeOutcomeValueTaskAndOutcomeTask(int value)
    {
        Outcome<string> composition = await
            ValueTask.FromResult(FirstOutcome(value))
                .ThenAsync(first => Task.FromResult(NextOutcome(value)).ThenAsync(_ => first));

        AssertExpectedOutcome(value, composition);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public async Task ShouldComposeOutcomeValueTaskAndOutcomeValueTask(int value)
    {
        Outcome<string> composition = await
            ValueTask.FromResult(FirstOutcome(value))
                .ThenAsync(first => ValueTask.FromResult(NextOutcome(value)).ThenAsync(_ => first));

        AssertExpectedOutcome(value, composition);
    }
}

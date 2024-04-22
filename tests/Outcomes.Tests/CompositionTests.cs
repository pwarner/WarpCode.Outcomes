namespace Outcomes.Tests;

public class CompositionTests : CompositionTestBase
{
    [Theory]
    [InlineData(ProblemStep.First)]
    [InlineData(ProblemStep.None)]
    public void ShouldMapOutcome(ProblemStep step)
    {
        Outcome<string> composition =
            FirstOutcome(step)
                .Then(_ => Success);

        AssertExpectedOutcome(step, composition);
    }

    [Theory]
    [InlineData(ProblemStep.First)]
    [InlineData(ProblemStep.None)]
    public async Task ShouldMapOutcomeTask(ProblemStep step)
    {
        Outcome<string> composition = await
            Task.FromResult(FirstOutcome(step))
                .ThenAsync(_ => Success);

        AssertExpectedOutcome(step, composition);
    }

    [Theory]
    [InlineData(ProblemStep.First)]
    [InlineData(ProblemStep.None)]
    public async Task ShouldMapOutcomeValueTask(ProblemStep step)
    {
        Outcome<string> composition = await
            ValueTask.FromResult(FirstOutcome(step))
                .ThenAsync(_ => Success);

        AssertExpectedOutcome(step, composition);
    }

    [Theory]
    [InlineData(ProblemStep.First)]
    [InlineData(ProblemStep.Second)]
    [InlineData(ProblemStep.None)]
    public void ShouldComposeOutcomeAndOutcome(ProblemStep step)
    {
        Outcome<string> composition =
            FirstOutcome(step)
                .Then(_ => NextOutcome(step));

        AssertExpectedOutcome(step, composition);
    }

    [Theory]
    [InlineData(ProblemStep.First)]
    [InlineData(ProblemStep.Second)]
    [InlineData(ProblemStep.None)]
    public async Task ShouldComposeOutcomeAndOutcomeTask(ProblemStep step)
    {
        Outcome<string> composition = await
            FirstOutcome(step)
                .ThenAsync(_ => Task.FromResult(NextOutcome(step)));

        AssertExpectedOutcome(step, composition);
    }

    [Theory]
    [InlineData(ProblemStep.First)]
    [InlineData(ProblemStep.Second)]
    [InlineData(ProblemStep.None)]
    public async Task ShouldComposeOutcomeAndOutcomeValueTask(ProblemStep step)
    {
        Outcome<string> composition = await
            FirstOutcome(step)
                .ThenAsync(_ => ValueTask.FromResult(NextOutcome(step)));

        AssertExpectedOutcome(step, composition);
    }

    [Theory]
    [InlineData(ProblemStep.First)]
    [InlineData(ProblemStep.Second)]
    [InlineData(ProblemStep.None)]
    public async Task ShouldComposeOutcomeTaskAndOutcome(ProblemStep step)
    {
        Outcome<string> composition = await
            Task.FromResult(FirstOutcome(step))
                .ThenAsync(_ => NextOutcome(step));

        AssertExpectedOutcome(step, composition);
    }

    [Theory]
    [InlineData(ProblemStep.First)]
    [InlineData(ProblemStep.Second)]
    [InlineData(ProblemStep.None)]
    public async Task ShouldComposeOutcomeTaskAndOutcomeTask(ProblemStep step)
    {
        Outcome<string> composition = await
            Task.FromResult(FirstOutcome(step))
                .ThenAsync(_ => Task.FromResult(NextOutcome(step)));

        AssertExpectedOutcome(step, composition);
    }

    [Theory]
    [InlineData(ProblemStep.First)]
    [InlineData(ProblemStep.Second)]
    [InlineData(ProblemStep.None)]
    public async Task ShouldComposeOutcomeTaskAndOutcomeValueTask(ProblemStep step)
    {
        Outcome<string> composition = await
            Task.FromResult(FirstOutcome(step))
                .ThenAsync(_ => ValueTask.FromResult(NextOutcome(step)));

        AssertExpectedOutcome(step, composition);
    }

    [Theory]
    [InlineData(ProblemStep.First)]
    [InlineData(ProblemStep.Second)]
    [InlineData(ProblemStep.None)]
    public async Task ShouldComposeOutcomeValueTaskAndOutcome(ProblemStep step)
    {
        Outcome<string> composition = await
            ValueTask.FromResult(FirstOutcome(step))
                .ThenAsync(_ => NextOutcome(step));

        AssertExpectedOutcome(step, composition);
    }

    [Theory]
    [InlineData(ProblemStep.First)]
    [InlineData(ProblemStep.Second)]
    [InlineData(ProblemStep.None)]
    public async Task ShouldComposeOutcomeValueTaskAndOutcomeTask(ProblemStep step)
    {
        Outcome<string> composition = await
            ValueTask.FromResult(FirstOutcome(step))
                .ThenAsync(_ => Task.FromResult(NextOutcome(step)));

        AssertExpectedOutcome(step, composition);
    }

    [Theory]
    [InlineData(ProblemStep.First)]
    [InlineData(ProblemStep.Second)]
    [InlineData(ProblemStep.None)]
    public async Task ShouldComposeOutcomeValueTaskAndOutcomeValueTask(ProblemStep step)
    {
        Outcome<string> composition = await
            ValueTask.FromResult(FirstOutcome(step))
                .ThenAsync(_ => ValueTask.FromResult(NextOutcome(step)));

        AssertExpectedOutcome(step, composition);
    }
}

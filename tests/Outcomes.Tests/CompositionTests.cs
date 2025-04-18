namespace WarpCode.Outcomes.Tests;

public class CompositionTests : CompositionTestBase
{
    [Theory]
    [InlineData(ProblemStep.First)]
    [InlineData(ProblemStep.None)]
    public void Then_ShouldMapOutcome(ProblemStep step)
    {
        Outcome<string> composition =
            EmptyOutcome(step)
                .Then(_ => Success);

        AssertExpectedOutcome(step, composition);
    }

    [Theory]
    [InlineData(ProblemStep.First)]
    [InlineData(ProblemStep.None)]
    public async Task ThenAsync_ShouldMapOutcomeTask(ProblemStep step)
    {
        Outcome<string> composition = await
            Task.FromResult(EmptyOutcome(step))
                .ThenAsync(_ => Success);

        AssertExpectedOutcome(step, composition);
    }

    [Theory]
    [InlineData(ProblemStep.First)]
    [InlineData(ProblemStep.None)]
    public async Task ThenAsync_ShouldMapOutcomeValueTask(ProblemStep step)
    {
        Outcome<string> composition = await
            ValueTask.FromResult(EmptyOutcome(step))
                .ThenAsync(_ => Success);

        AssertExpectedOutcome(step, composition);
    }

    [Theory]
    [InlineData(ProblemStep.First)]
    [InlineData(ProblemStep.Second)]
    [InlineData(ProblemStep.None)]
    public void Then_ShouldComposeOutcomeAndOutcome(ProblemStep step)
    {
        Outcome<string> composition =
            EmptyOutcome(step)
                .Then(_ => StringOutcome(step));

        AssertExpectedOutcome(step, composition);
    }

    [Theory]
    [InlineData(ProblemStep.First)]
    [InlineData(ProblemStep.Second)]
    [InlineData(ProblemStep.None)]
    public async Task ThenAsync_ShouldComposeOutcomeAndOutcomeTask(ProblemStep step)
    {
        Outcome<string> composition = await
            EmptyOutcome(step)
                .ThenAsync(_ => Task.FromResult(StringOutcome(step)));

        AssertExpectedOutcome(step, composition);
    }

    [Theory]
    [InlineData(ProblemStep.First)]
    [InlineData(ProblemStep.Second)]
    [InlineData(ProblemStep.None)]
    public async Task ThenAsync_ShouldComposeOutcomeAndOutcomeValueTask(ProblemStep step)
    {
        Outcome<string> composition = await
            EmptyOutcome(step)
                .ThenAsync(_ => ValueTask.FromResult(StringOutcome(step)));

        AssertExpectedOutcome(step, composition);
    }

    [Theory]
    [InlineData(ProblemStep.First)]
    [InlineData(ProblemStep.Second)]
    [InlineData(ProblemStep.None)]
    public async Task ThenAsync_ShouldComposeOutcomeTaskAndOutcome(ProblemStep step)
    {
        Outcome<string> composition = await
            Task.FromResult(EmptyOutcome(step))
                .ThenAsync(_ => StringOutcome(step));

        AssertExpectedOutcome(step, composition);
    }

    [Theory]
    [InlineData(ProblemStep.First)]
    [InlineData(ProblemStep.Second)]
    [InlineData(ProblemStep.None)]
    public async Task ThenAsync_ShouldComposeOutcomeTaskAndOutcomeTask(ProblemStep step)
    {
        Outcome<string> composition = await
            Task.FromResult(EmptyOutcome(step))
                .ThenAsync(_ => Task.FromResult(StringOutcome(step)));

        AssertExpectedOutcome(step, composition);
    }

    [Theory]
    [InlineData(ProblemStep.First)]
    [InlineData(ProblemStep.Second)]
    [InlineData(ProblemStep.None)]
    public async Task ThenAsync_ShouldComposeOutcomeTaskAndOutcomeValueTask(ProblemStep step)
    {
        Outcome<string> composition = await
            Task.FromResult(EmptyOutcome(step))
                .ThenAsync(_ => ValueTask.FromResult(StringOutcome(step)));

        AssertExpectedOutcome(step, composition);
    }

    [Theory]
    [InlineData(ProblemStep.First)]
    [InlineData(ProblemStep.Second)]
    [InlineData(ProblemStep.None)]
    public async Task ThenAsync_ShouldComposeOutcomeValueTaskAndOutcome(ProblemStep step)
    {
        Outcome<string> composition = await
            ValueTask.FromResult(EmptyOutcome(step))
                .ThenAsync(_ => StringOutcome(step));

        AssertExpectedOutcome(step, composition);
    }

    [Theory]
    [InlineData(ProblemStep.First)]
    [InlineData(ProblemStep.Second)]
    [InlineData(ProblemStep.None)]
    public async Task ThenAsync_ShouldComposeOutcomeValueTaskAndOutcomeTask(ProblemStep step)
    {
        Outcome<string> composition = await
            ValueTask.FromResult(EmptyOutcome(step))
                .ThenAsync(_ => Task.FromResult(StringOutcome(step)));

        AssertExpectedOutcome(step, composition);
    }

    [Theory]
    [InlineData(ProblemStep.First)]
    [InlineData(ProblemStep.Second)]
    [InlineData(ProblemStep.None)]
    public async Task ThenAsync_ShouldComposeOutcomeValueTaskAndOutcomeValueTask(ProblemStep step)
    {
        Outcome<string> composition = await
            ValueTask.FromResult(EmptyOutcome(step))
                .ThenAsync(_ => ValueTask.FromResult(StringOutcome(step)));

        AssertExpectedOutcome(step, composition);
    }
}

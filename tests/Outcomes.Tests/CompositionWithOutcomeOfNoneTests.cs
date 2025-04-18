namespace WarpCode.Outcomes.Tests;

public class CompositionWithOutcomeOfNoneTests : CompositionTestBase
{
    [Theory]
    [InlineData(ProblemStep.First)]
    [InlineData(ProblemStep.Second)]
    [InlineData(ProblemStep.None)]
    public void Then_ShouldComposeOutcomeAndOutcome(ProblemStep step)
    {
        Outcome<string> composition =
            StringOutcome(step)
                .Then(_ => EmptyOutcome(step));

        AssertExpectedOutcome(step, composition);
    }

    [Theory]
    [InlineData(ProblemStep.First)]
    [InlineData(ProblemStep.Second)]
    [InlineData(ProblemStep.None)]
    public async Task ThenAsync_ShouldComposeOutcomeAndOutcomeTask(ProblemStep step)
    {
        Outcome<string> composition = await
            StringOutcome(step)
                .ThenAsync(_ => Task.FromResult(EmptyOutcome(step)));

        AssertExpectedOutcome(step, composition);
    }

    [Theory]
    [InlineData(ProblemStep.First)]
    [InlineData(ProblemStep.Second)]
    [InlineData(ProblemStep.None)]
    public async Task ThenAsync_ShouldComposeOutcomeAndOutcomeValueTask(ProblemStep step)
    {
        Outcome<string> composition = await
            StringOutcome(step)
                .ThenAsync(_ => ValueTask.FromResult(EmptyOutcome(step)));

        AssertExpectedOutcome(step, composition);
    }

    [Theory]
    [InlineData(ProblemStep.First)]
    [InlineData(ProblemStep.Second)]
    [InlineData(ProblemStep.None)]
    public async Task ThenAsync_ShouldComposeOutcomeTaskAndOutcome(ProblemStep step)
    {
        Outcome<string> composition = await
            Task.FromResult(StringOutcome(step))
                .ThenAsync(_ => EmptyOutcome(step));

        AssertExpectedOutcome(step, composition);
    }

    [Theory]
    [InlineData(ProblemStep.First)]
    [InlineData(ProblemStep.Second)]
    [InlineData(ProblemStep.None)]
    public async Task ThenAsync_ShouldComposeOutcomeTaskAndOutcomeTask(ProblemStep step)
    {
        Outcome<string> composition = await
            Task.FromResult(StringOutcome(step))
                .ThenAsync(_ => Task.FromResult(EmptyOutcome(step)));

        AssertExpectedOutcome(step, composition);
    }

    [Theory]
    [InlineData(ProblemStep.First)]
    [InlineData(ProblemStep.Second)]
    [InlineData(ProblemStep.None)]
    public async Task ThenAsync_ShouldComposeOutcomeTaskAndOutcomeValueTask(ProblemStep step)
    {
        Outcome<string> composition = await
            Task.FromResult(StringOutcome(step))
                .ThenAsync(_ => ValueTask.FromResult(EmptyOutcome(step)));

        AssertExpectedOutcome(step, composition);
    }

    [Theory]
    [InlineData(ProblemStep.First)]
    [InlineData(ProblemStep.Second)]
    [InlineData(ProblemStep.None)]
    public async Task ThenAsync_ShouldComposeOutcomeValueTaskAndOutcome(ProblemStep step)
    {
        Outcome<string> composition = await
            ValueTask.FromResult(StringOutcome(step))
                .ThenAsync(_ => EmptyOutcome(step));

        AssertExpectedOutcome(step, composition);
    }

    [Theory]
    [InlineData(ProblemStep.First)]
    [InlineData(ProblemStep.Second)]
    [InlineData(ProblemStep.None)]
    public async Task ThenAsync_ShouldComposeOutcomeValueTaskAndOutcomeTask(ProblemStep step)
    {
        Outcome<string> composition = await
            ValueTask.FromResult(StringOutcome(step))
                .ThenAsync(_ => Task.FromResult(EmptyOutcome(step)));

        AssertExpectedOutcome(step, composition);
    }

    [Theory]
    [InlineData(ProblemStep.First)]
    [InlineData(ProblemStep.Second)]
    [InlineData(ProblemStep.None)]
    public async Task ThenAsync_ShouldComposeOutcomeValueTaskAndOutcomeValueTask(ProblemStep step)
    {
        Outcome<string> composition = await
            ValueTask.FromResult(StringOutcome(step))
                .ThenAsync(_ => ValueTask.FromResult(EmptyOutcome(step)));

        AssertExpectedOutcome(step, composition);
    }
}

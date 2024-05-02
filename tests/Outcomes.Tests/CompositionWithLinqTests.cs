namespace Outcomes.Tests;

public class CompositionWithLinqTests : CompositionTestBase
{
    [Theory]
    [InlineData(ProblemStep.First)]
    [InlineData(ProblemStep.None)]
    public void Select_ShouldMapOutcome(ProblemStep step)
    {
        Outcome<string> composition =
            from _ in EmptyOutcome(step)
            select Success;

        AssertExpectedOutcome(step, composition);
    }

    [Theory]
    [InlineData(ProblemStep.First)]
    [InlineData(ProblemStep.None)]
    public async Task Select_ShouldMapOutcomeTask(ProblemStep step)
    {
        Outcome<string> composition = await (
            from _ in Task.FromResult(EmptyOutcome(step))
            select Success
        );

        AssertExpectedOutcome(step, composition);
    }

    [Theory]
    [InlineData(ProblemStep.First)]
    [InlineData(ProblemStep.None)]
    public async Task Select_ShouldMapOutcomeValueTask(ProblemStep step)
    {
        Outcome<string> composition = await (
            from _ in ValueTask.FromResult(EmptyOutcome(step))
            select Success
        );

        AssertExpectedOutcome(step, composition);
    }

    [Theory]
    [InlineData(ProblemStep.First)]
    [InlineData(ProblemStep.Second)]
    [InlineData(ProblemStep.None)]
    public void SelectMany_ShouldComposeOutcomeAndOutcome(ProblemStep step)
    {
        Outcome<string> composition =
            from _ in EmptyOutcome(step)
            from next in StringOutcome(step)
            select next;

        AssertExpectedOutcome(step, composition);
    }

    [Theory]
    [InlineData(ProblemStep.First)]
    [InlineData(ProblemStep.Second)]
    [InlineData(ProblemStep.None)]
    public async Task SelectMany_ShouldComposeOutcomeAndOutcomeTask(ProblemStep step)
    {
        Outcome<string> composition = await (
            from _ in EmptyOutcome(step)
            from next in Task.FromResult(StringOutcome(step))
            select next
        );

        AssertExpectedOutcome(step, composition);
    }

    [Theory]
    [InlineData(ProblemStep.First)]
    [InlineData(ProblemStep.Second)]
    [InlineData(ProblemStep.None)]
    public async Task SelectMany_ShouldComposeOutcomeAndOutcomeValueTask(ProblemStep step)
    {
        Outcome<string> composition = await (
            from _ in EmptyOutcome(step)
            from next in ValueTask.FromResult(StringOutcome(step))
            select next
        );

        AssertExpectedOutcome(step, composition);
    }

    [Theory]
    [InlineData(ProblemStep.First)]
    [InlineData(ProblemStep.Second)]
    [InlineData(ProblemStep.None)]
    public async Task SelectMany_ShouldComposeOutcomeTaskAndOutcome(ProblemStep step)
    {
        Outcome<string> composition = await (
            from _ in Task.FromResult(EmptyOutcome(step))
            from next in StringOutcome(step)
            select next
        );

        AssertExpectedOutcome(step, composition);
    }

    [Theory]
    [InlineData(ProblemStep.First)]
    [InlineData(ProblemStep.Second)]
    [InlineData(ProblemStep.None)]
    public async Task SelectMany_ShouldComposeOutcomeTaskAndOutcomeTask(ProblemStep step)
    {
        Outcome<string> composition = await (
            from _ in Task.FromResult(EmptyOutcome(step))
            from next in Task.FromResult(StringOutcome(step))
            select next
        );

        AssertExpectedOutcome(step, composition);
    }

    [Theory]
    [InlineData(ProblemStep.First)]
    [InlineData(ProblemStep.Second)]
    [InlineData(ProblemStep.None)]
    public async Task SelectMany_ShouldComposeOutcomeTaskAndOutcomeValueTask(ProblemStep step)
    {
        Outcome<string> composition = await (
            from _ in Task.FromResult(EmptyOutcome(step))
            from next in ValueTask.FromResult(StringOutcome(step))
            select next
        );

        AssertExpectedOutcome(step, composition);
    }

    [Theory]
    [InlineData(ProblemStep.First)]
    [InlineData(ProblemStep.Second)]
    [InlineData(ProblemStep.None)]
    public async Task SelectMany_ShouldComposeOutcomeValueTaskAndOutcome(ProblemStep step)
    {
        Outcome<string> composition = await (
            from _ in ValueTask.FromResult(EmptyOutcome(step))
            from next in StringOutcome(step)
            select next
        );

        AssertExpectedOutcome(step, composition);
    }

    [Theory]
    [InlineData(ProblemStep.First)]
    [InlineData(ProblemStep.Second)]
    [InlineData(ProblemStep.None)]
    public async Task SelectMany_ShouldComposeOutcomeValueTaskAndOutcomeTask(ProblemStep step)
    {
        Outcome<string> composition = await (
            from _ in ValueTask.FromResult(EmptyOutcome(step))
            from next in Task.FromResult(StringOutcome(step))
            select next
        );

        AssertExpectedOutcome(step, composition);
    }

    [Theory]
    [InlineData(ProblemStep.First)]
    [InlineData(ProblemStep.Second)]
    [InlineData(ProblemStep.None)]
    public async Task SelectMany_ShouldComposeOutcomeValueTaskAndOutcomeValueTask(ProblemStep step)
    {
        Outcome<string> composition = await (
            from _ in ValueTask.FromResult(EmptyOutcome(step))
            from next in ValueTask.FromResult(StringOutcome(step))
            select next
        );

        AssertExpectedOutcome(step, composition);
    }
}

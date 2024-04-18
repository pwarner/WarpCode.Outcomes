namespace Outcomes.Tests;

public class CompositionWithLinqTests : CompositionTestBase
{
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    public void Select_ShouldMapOutcome(int value)
    {
        Outcome<string> composition =
            from first in FirstOutcome(value)
            select first;

        AssertExpectedMapOutcome(value, composition);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    public async Task Select_ShouldMapOutcomeTask(int value)
    {
        Outcome<string> composition = await (
            from first in Task.FromResult(FirstOutcome(value))
            select first
        );

        AssertExpectedMapOutcome(value, composition);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    public async Task Select_ShouldMapOutcomeValueTask(int value)
    {
        Outcome<string> composition = await (
            from first in ValueTask.FromResult(FirstOutcome(value))
            select first
        );

        AssertExpectedMapOutcome(value, composition);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void SelectMany_ShouldComposeOutcomeAndOutcome(int value)
    {
        Outcome<string> composition =
            from first in FirstOutcome(value)
            from _ in NextOutcome(value)
            select first;

        AssertExpectedOutcome(value, composition);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public async Task SelectMany_ShouldComposeOutcomeAndOutcomeTask(int value)
    {
        Outcome<string> composition = await (
            from first in FirstOutcome(value)
            from _ in Task.FromResult(NextOutcome(value))
            select first
        );

        AssertExpectedOutcome(value, composition);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public async Task SelectMany_ShouldComposeOutcomeAndOutcomeValueTask(int value)
    {
        Outcome<string> composition = await (
            from first in FirstOutcome(value)
            from _ in ValueTask.FromResult(NextOutcome(value))
            select first
        );

        AssertExpectedOutcome(value, composition);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public async Task SelectMany_ShouldComposeOutcomeTaskAndOutcome(int value)
    {
        Outcome<string> composition = await (
            from first in Task.FromResult(FirstOutcome(value))
            from _ in NextOutcome(value)
            select first
        );

        AssertExpectedOutcome(value, composition);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public async Task SelectMany_ShouldComposeOutcomeTaskAndOutcomeTask(int value)
    {
        Outcome<string> composition = await (
            from first in Task.FromResult(FirstOutcome(value))
            from _ in Task.FromResult(NextOutcome(value))
            select first
        );

        AssertExpectedOutcome(value, composition);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public async Task SelectMany_ShouldComposeOutcomeTaskAndOutcomeValueTask(int value)
    {
        Outcome<string> composition = await (
            from first in Task.FromResult(FirstOutcome(value))
            from _ in ValueTask.FromResult(NextOutcome(value))
            select first
        );

        AssertExpectedOutcome(value, composition);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public async Task SelectMany_ShouldComposeOutcomeValueTaskAndOutcome(int value)
    {
        Outcome<string> composition = await (
            from first in ValueTask.FromResult(FirstOutcome(value))
            from _ in NextOutcome(value)
            select first
        );

        AssertExpectedOutcome(value, composition);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public async Task SelectMany_ShouldComposeOutcomeValueTaskAndOutcomeTask(int value)
    {
        Outcome<string> composition = await (
            from first in ValueTask.FromResult(FirstOutcome(value))
            from _ in Task.FromResult(NextOutcome(value))
            select first
        );

        AssertExpectedOutcome(value, composition);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public async Task SelectMany_ShouldComposeOutcomeValueTaskAndOutcomeValueTask(int value)
    {
        Outcome<string> composition = await (
            from first in ValueTask.FromResult(FirstOutcome(value))
            from _ in ValueTask.FromResult(NextOutcome(value))
            select first
        );

        AssertExpectedOutcome(value, composition);
    }
}

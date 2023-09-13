namespace Outcomes.Tests;

public class CompositionWithLinqTests : CompositionTestBase
{
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    public void Select_ShouldMapOutcome(int failValue)
    {
        Outcome<string> composition =
            from _ in OutcomeFactory(failValue, 1)
            select Success;

        AssertExpectedMap(failValue, composition);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    public async Task Select_ShouldMapOutcomeTask(int failValue)
    {
        Task<Outcome<string>> composition =
            from _ in Task.FromResult(OutcomeFactory(failValue, 1))
            select Success;

        AssertExpectedMap(failValue, await composition);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    public async Task Select_ShouldMapOutcomeValueTask(int failValue)
    {
        Task<Outcome<string>> composition =
            from _ in ValueTask.FromResult(OutcomeFactory(failValue, 1))
            select Success;

        AssertExpectedMap(failValue, await composition);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void SelectMany_ShouldComposeOutcomeAndOutcome(int failValue)
    {
        Outcome<None> composition =
            from first in OutcomeFactory(failValue, 1)
            from next in OutcomeFactory(failValue, 2)
            select next;

        AssertExpectedBind(failValue, composition);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public async Task SelectMany_ShouldComposeOutcomeAndOutcomeTask(int failValue)
    {
        Task<Outcome<None>> composition =
            from first in OutcomeFactory(failValue, 1)
            from next in Task.FromResult(OutcomeFactory(failValue, 2))
            select next;

        AssertExpectedBind(failValue, await composition);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public async Task SelectMany_ShouldComposeOutcomeAndOutcomeValueTask(int failValue)
    {
        Task<Outcome<None>> composition =
            from first in OutcomeFactory(failValue, 1)
            from next in ValueTask.FromResult(OutcomeFactory(failValue, 2))
            select next;

        AssertExpectedBind(failValue, await composition);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public async Task SelectMany_ShouldComposeOutcomeTaskAndOutcome(int failValue)
    {
        Task<Outcome<None>> composition =
            from first in Task.FromResult(OutcomeFactory(failValue, 1))
            from next in OutcomeFactory(failValue, 2)
            select next;

        AssertExpectedBind(failValue, await composition);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public async Task SelectMany_ShouldComposeOutcomeTaskAndOutcomeTask(int failValue)
    {
        Task<Outcome<None>> composition =
            from first in Task.FromResult(OutcomeFactory(failValue, 1))
            from next in Task.FromResult(OutcomeFactory(failValue, 2))
            select next;

        AssertExpectedBind(failValue, await composition);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public async Task SelectMany_ShouldComposeOutcomeTaskAndOutcomeValueTask(int failValue)
    {
        Task<Outcome<None>> composition =
            from first in Task.FromResult(OutcomeFactory(failValue, 1))
            from next in ValueTask.FromResult(OutcomeFactory(failValue, 2))
            select next;

        AssertExpectedBind(failValue, await composition);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public async Task SelectMany_ShouldComposeOutcomeValueTaskAndOutcome(int failValue)
    {
        Task<Outcome<None>> composition =
            from first in ValueTask.FromResult(OutcomeFactory(failValue, 1))
            from next in OutcomeFactory(failValue, 2)
            select next;

        AssertExpectedBind(failValue, await composition);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public async Task SelectMany_ShouldComposeOutcomeValueTaskAndOutcomeTask(int failValue)
    {
        Task<Outcome<None>> composition =
            from first in ValueTask.FromResult(OutcomeFactory(failValue, 1))
            from next in Task.FromResult(OutcomeFactory(failValue, 2))
            select next;

        AssertExpectedBind(failValue, await composition);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public async Task SelectMany_ShouldComposeOutcomeValueTaskAndOutcomeValueTask(int failValue)
    {
        Task<Outcome<None>> composition =
            from first in ValueTask.FromResult(OutcomeFactory(failValue, 1))
            from next in ValueTask.FromResult(OutcomeFactory(failValue, 2))
            select next;

        AssertExpectedBind(failValue, await composition);
    }
}

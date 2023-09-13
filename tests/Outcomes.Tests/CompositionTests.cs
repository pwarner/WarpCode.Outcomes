namespace Outcomes.Tests;

public class CompositionTests : CompositionTestBase
{
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    public void Map_ShouldMapOutcome(int failValue)
    {
        Outcome<string> composition =
            OutcomeFactory(failValue, 1)
                .Map(_ => Success);

        AssertExpectedMap(failValue, composition);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    public async Task Map_ShouldMapOutcomeTask(int failValue)
    {
        Task<Outcome<string>> composition =
            Task.FromResult(OutcomeFactory(failValue, 1))
                .MapAsync(_ => Success);

        AssertExpectedMap(failValue, await composition);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    public async Task Map_ShouldMapOutcomeValueTask(int failValue)
    {
        Task<Outcome<string>> composition =
            ValueTask.FromResult(OutcomeFactory(failValue, 1))
                .MapAsync(_ => Success);

        AssertExpectedMap(failValue, await composition);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void Bind_ShouldComposeOutcomeAndOutcome(int failValue)
    {
        Outcome<None> composition =
            OutcomeFactory(failValue, 1)
                .Bind(_ => OutcomeFactory(failValue, 2));

        AssertExpectedBind(failValue, composition);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public async Task Bind_ShouldComposeOutcomeAndOutcomeTask(int failValue)
    {
        Task<Outcome<None>> composition =
            OutcomeFactory(failValue, 1)
                .BindAsync(_ => Task.FromResult(OutcomeFactory(failValue, 2)));

        AssertExpectedBind(failValue, await composition);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public async Task Bind_ShouldComposeOutcomeAndOutcomeValueTask(int failValue)
    {
        ValueTask<Outcome<None>> composition =
            OutcomeFactory(failValue, 1)
                .BindAsync(_ => ValueTask.FromResult(OutcomeFactory(failValue, 2)));

        AssertExpectedBind(failValue, await composition);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public async Task Bind_ShouldComposeOutcomeTaskAndOutcome(int failValue)
    {
        Task<Outcome<None>> composition =
            Task.FromResult(OutcomeFactory(failValue, 1))
                .BindAsync(_ => OutcomeFactory(failValue, 2));

        AssertExpectedBind(failValue, await composition);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public async Task Bind_ShouldComposeOutcomeTaskAndOutcomeTask(int failValue)
    {
        Task<Outcome<None>> composition =
            Task.FromResult(OutcomeFactory(failValue, 1))
                .BindAsync(_ => Task.FromResult(OutcomeFactory(failValue, 2)));

        AssertExpectedBind(failValue, await composition);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public async Task Bind_ShouldComposeOutcomeTaskAndOutcomeValueTask(int failValue)
    {
        Task<Outcome<None>> composition =
            Task.FromResult(OutcomeFactory(failValue, 1))
                .BindAsync(_ => ValueTask.FromResult(OutcomeFactory(failValue, 2)));

        AssertExpectedBind(failValue, await composition);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public async Task Bind_ShouldComposeOutcomeValueTaskAndOutcome(int failValue)
    {
        Task<Outcome<None>> composition =
            ValueTask.FromResult(OutcomeFactory(failValue, 1))
                .BindAsync(_ => OutcomeFactory(failValue, 2));

        AssertExpectedBind(failValue, await composition);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public async Task Bind_ShouldComposeOutcomeValueTaskAndOutcomeTask(int failValue)
    {
        Task<Outcome<None>> composition =
            ValueTask.FromResult(OutcomeFactory(failValue, 1))
                .BindAsync(_ => Task.FromResult(OutcomeFactory(failValue, 2)));

        AssertExpectedBind(failValue, await composition);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public async Task Bind_ShouldComposeOutcomeValueTaskAndOutcomeValueTask(int failValue)
    {
        Task<Outcome<None>> composition =
            ValueTask.FromResult(OutcomeFactory(failValue, 1))
                .BindAsync(_ => ValueTask.FromResult(OutcomeFactory(failValue, 2)));

        AssertExpectedBind(failValue, await composition);
    }
}

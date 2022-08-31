namespace Matterlab.Rails.Tests;

public class OutcomeCompositionTests
{
    [Fact]
    public void Outcome_ComposesWith_Outcome()
    {
        static Outcome<int> PlusOne(int x) => x + 1;

        Outcome<int> composition =
            from x in Outcome.Ok(2)
            from y in PlusOne(x)
            select x * y;

        int actual = composition.Match(x => x, p => 0);
        Assert.Equal(6, actual);
    }

    [Fact]
    public async Task Outcome_ComposesWith_AsyncOutcome()
    {
        static AsyncOutcome<int> PlusOne(int x) => new(x + 1);

        AsyncOutcome<int> composition =
            from x in Outcome.Ok(2)
            from y in PlusOne(x)
            select x * y;

        int actual = await composition.MatchAsync(x => x, p => 0);
        Assert.Equal(6, actual);
    }

    [Fact]
    public async Task Outcome_ComposesWith_TaskOfOutcome()
    {
        static Task<Outcome<int>> PlusOne(int x) =>
            Task.FromResult(Outcome.Ok(x + 1));

        AsyncOutcome<int> composition =
            from x in Outcome.Ok(2)
            from y in PlusOne(x)
            select x * y;

        int actual = await composition.MatchAsync(x => x, p => 0);
        Assert.Equal(6, actual);
    }

    [Fact]
    public async Task Outcome_ComposesWith_ValueTaskOfOutcome()
    {
        static ValueTask<Outcome<int>> PlusOne(int x) =>
            ValueTask.FromResult(Outcome.Ok(x + 1));

        AsyncOutcome<int> composition =
            from x in Outcome.Ok(2)
            from y in PlusOne(x)
            select x * y;

        int actual = await composition.MatchAsync(x => x, p => 0);
        Assert.Equal(6, actual);
    }

    [Fact]
    public async Task Outcome_ComposesWith_TaskOfT()
    {
        static Task<int> PlusOne(int x) =>
            Task.FromResult(x + 1);

        AsyncOutcome<int> composition =
            from x in Outcome.Ok(2)
            from y in PlusOne(x)
            select x * y;

        int actual = await composition.MatchAsync(x => x, p => 0);
        Assert.Equal(6, actual);
    }

    [Fact]
    public async Task Outcome_ComposesWith_ValueTaskOfT()
    {
        static ValueTask<int> PlusOne(int x) =>
            ValueTask.FromResult(x + 1);

        AsyncOutcome<int> composition =
            from x in Outcome.Ok(2)
            from y in PlusOne(x)
            select x * y;

        int actual = await composition.MatchAsync(x => x, p => 0);
        Assert.Equal(6, actual);
    }

    [Fact]
    public async Task Outcome_ComposesWith_Task()
    {
        static Task SomeAction() => Task.CompletedTask;

        AsyncOutcome<int> composition =
            from x in Outcome.Ok(3)
            from _ in SomeAction()
            select x + 3;

        int actual = await composition.MatchAsync(x => x, p => 0);
        Assert.Equal(6, actual);
    }

    [Fact]
    public async Task Outcome_ComposesWith_ValueTask()
    {
        static ValueTask SomeAction() => ValueTask.CompletedTask;

        AsyncOutcome<int> composition =
            from x in Outcome.Ok(3)
            from _ in SomeAction()
            select x + 3;

        int actual = await composition.MatchAsync(x => x, p => 0);
        Assert.Equal(6, actual);
    }
}

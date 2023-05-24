namespace Outcomes.Tests;

public class AsyncOutcomeCompositionTests
{
    private static readonly AsyncOutcome<int> AsyncOutcome = new(2);

    [Fact]
    public async Task Outcome_ComposesWith_TaskOfOutcome()
    {
        ValueTask<Outcome<int>> composition =
            from x in Outcome.Ok(2)
            from y in Task.FromResult(Outcome.Ok(x + 1))
            select x * y;

        Outcome<int> actual = await composition;
        Assert.Equal(new Outcome<int>(6), actual);
    }

    [Fact]
    public async Task Outcome_ComposesWith_ValueTaskOfOutcome()
    {
        ValueTask<Outcome<int>> composition =
            from x in Outcome.Ok(2)
            from y in ValueTask.FromResult(Outcome.Ok(x + 1))
            select x * y;

        Outcome<int> actual = await composition;
        Assert.Equal(new Outcome<int>(6), actual);
    }

    [Fact]
    public async Task Outcome_ComposesWith_TaskOfT()
    {
        ValueTask<Outcome<int>> composition =
            from x in Outcome.Ok(2)
            from y in Task.FromResult(x + 1)
            select x * y;

        Outcome<int> actual = await composition;
        Assert.Equal(new Outcome<int>(6), actual);
    }

    [Fact]
    public async Task Outcome_ComposesWith_ValueTaskOfT()
    {
        ValueTask<Outcome<int>> composition =
            from x in Outcome.Ok(2)
            from y in ValueTask.FromResult(x + 1)
            select x * y;

        Outcome<int> actual = await composition;
        Assert.Equal(new Outcome<int>(6), actual);
    }

    [Fact]
    public async Task Outcome_ComposesWith_Task()
    {
        ValueTask<Outcome<int>> composition =
            from x in Outcome.Ok(3)
            from _ in Task.CompletedTask
            select x + 3;

        Outcome<int> actual = await composition;
        Assert.Equal(new Outcome<int>(6), actual);
    }

    [Fact]
    public async Task Outcome_ComposesWith_ValueTask()
    {
        ValueTask<Outcome<int>> composition =
            from x in Outcome.Ok(3)
            from _ in ValueTask.CompletedTask
            select x + 3;

        Outcome<int> actual = await composition;
        Assert.Equal(new Outcome<int>(6), actual);
    }

    [Fact]
    public async Task AsyncOutcome_ComposesWith_Outcome()
    {
        ValueTask<Outcome<int>> composition =
            from x in AsyncOutcome
            from y in Outcome.Ok(x + 1)
            select x * y;

        Outcome<int> actual = await composition;
        Assert.Equal(new Outcome<int>(6), actual);
    }

    [Fact]
    public async Task AsyncOutcome_ComposesWith_TaskOfOutcome()
    {
        ValueTask<Outcome<int>> composition =
            from x in AsyncOutcome
            from y in Task.FromResult(Outcome.Ok(x + 1))
            select x * y;

        Outcome<int> actual = await composition;
        Assert.Equal(new Outcome<int>(6), actual);
    }

    [Fact]
    public async Task AsyncOutcome_ComposesWith_ValueTaskOfOutcome()
    {
        ValueTask<Outcome<int>> composition =
            from x in AsyncOutcome
            from y in ValueTask.FromResult(Outcome.Ok(x + 1))
            select x * y;

        Outcome<int> actual = await composition;
        Assert.Equal(new Outcome<int>(6), actual);
    }

    [Fact]
    public async Task AsyncOutcome_ComposesWith_TaskOfT()
    {
        ValueTask<Outcome<int>> composition =
            from x in AsyncOutcome
            from y in Task.FromResult(x + 1)
            select x * y;

        Outcome<int> actual = await composition;
        Assert.Equal(new Outcome<int>(6), actual);
    }

    [Fact]
    public async Task AsyncOutcome_ComposesWith_ValueTaskOfT()
    {
        ValueTask<Outcome<int>> composition =
            from x in AsyncOutcome
            from y in ValueTask.FromResult(x + 1)
            select x * y;

        Outcome<int> actual = await composition;
        Assert.Equal(new Outcome<int>(6), actual);
    }

    [Fact]
    public async Task AsyncOutcome_ComposesWith_Task()
    {
        ValueTask<Outcome<int>> composition =
            from x in AsyncOutcome
            from _ in Task.CompletedTask
            select x * 3;

        Outcome<int> actual = await composition;
        Assert.Equal(new Outcome<int>(6), actual);
    }

    [Fact]
    public async Task AsyncOutcome_ComposesWith_ValueTask()
    {
        ValueTask<Outcome<int>> composition =
            from x in AsyncOutcome
            from _ in ValueTask.CompletedTask
            select x * 3;

        Outcome<int> actual = await composition;
        Assert.Equal(new Outcome<int>(6), actual);
    }
}

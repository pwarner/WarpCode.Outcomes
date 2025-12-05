namespace WarpCode.Outcomes.Tests;

public class OutcomeAggregationTests
{
    private static readonly Problem TestProblem = new(nameof(TestProblem));

    [Fact]
    public void Aggregate_ShouldCreateOutcomeOfListOfValues_WhenNoProblems()
    {
        var expected = new List<int>(3) { 1, 2, 3 };
        List<int> actual = NoIntProblems().Aggregate().Match(l => l, _ => []);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task AggregateAsync_ShouldCreateOutcomeOfListOfValues_WhenNoProblems()
    {
        var expected = new List<int>(3) { 1, 2, 3 };
        List<int> actual = await AsyncOf(NoIntProblems()).AggregateAsync()
            .MatchAsync(l => l, _ => []);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Aggregate_ShouldCreateSuccessOutcome_WhenNoProblemsAndNoValues()
    {
        Outcome<None> expected = Outcome.Ok;
        Outcome<None> actual = NoProblems().Aggregate();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task AggregateAsync_ShouldCreateSuccessOutcome_WhenNoProblemsAndNoValues()
    {
        Outcome<None> expected = Outcome.Ok;
        Outcome<None> actual = await AsyncOf(NoProblems()).AggregateAsync();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Aggregate_ShouldCreateOutcomeOfFirstProblem_WhenBailEarlyNoValues()
    {
        var expected = TestProblem.ToOutcome();
        Outcome<None> actual = SomeProblems().Aggregate(bailEarly: true);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task AggregateAsync_ShouldCreateOutcomeOfFirstProblem_WhenBailEarlyNoValues()
    {
        var expected = TestProblem.ToOutcome();
        Outcome<None> actual = await AsyncOf(SomeProblems()).AggregateAsync(bailEarly: true);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Aggregate_ShouldCreateOutcomeOfFirstProblem_WhenBailEarly()
    {
        var expected = TestProblem.ToOutcome<List<int>>();
        Outcome<List<int>> actual = SomeIntProblems().Aggregate(bailEarly: true);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task AggregateAsync_ShouldCreateOutcomeOfFirstProblem_WhenBailEarly()
    {
        var expected = TestProblem.ToOutcome<List<int>>();
        Outcome<List<int>> actual = await AsyncOf(SomeIntProblems())
            .AggregateAsync(bailEarly: true);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Aggregate_ShouldCreateOutcomeOfAllProblems_WhenNoValuesNoBailEarly()
    {
        Outcome<None> expected = new ProblemAggregate([TestProblem, TestProblem]);
        Outcome<None> actual = SomeProblems().Aggregate();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task AggregateAsync_ShouldCreateOutcomeOfAllProblems_WhenNoValuesNoBailEarly()
    {
        Outcome<None> expected = new ProblemAggregate([TestProblem, TestProblem]);
        Outcome<None> actual = await AsyncOf(SomeProblems()).AggregateAsync();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Aggregate_ShouldCreateOutcomeOfAllProblems_WhenNoBailEarly()
    {
        Outcome<List<int>> expected = new ProblemAggregate([TestProblem, TestProblem]);
        Outcome<List<int>> actual = SomeIntProblems().Aggregate();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task AggregateAsync_ShouldCreateOutcomeOfAllProblems_WhenNoBailEarly()
    {
        Outcome<List<int>> expected = new ProblemAggregate([TestProblem, TestProblem]);
        Outcome<List<int>> actual = await AsyncOf(SomeIntProblems()).AggregateAsync();

        Assert.Equal(expected, actual);
    }

    private static IEnumerable<Outcome<None>> SomeProblems()
    {
        yield return Outcome.Ok;
        yield return TestProblem;
        yield return Outcome.Ok;
        yield return TestProblem;
    }

    private static IEnumerable<Outcome<int>> SomeIntProblems()
    {
        yield return 1;
        yield return TestProblem;
        yield return 2;
        yield return TestProblem;
    }

    private static IEnumerable<Outcome<None>> NoProblems()
    {
        yield return Outcome.Ok;
        yield return Outcome.Ok;
        yield return Outcome.Ok;
    }

    private static IEnumerable<Outcome<int>> NoIntProblems()
    {
        yield return 1;
        yield return 2;
        yield return 3;
    }

    private static async IAsyncEnumerable<T> AsyncOf<T>(IEnumerable<T> items)
    {
        foreach (T item in items)
        {
            await Task.CompletedTask;
            yield return item;
        }
    }
}

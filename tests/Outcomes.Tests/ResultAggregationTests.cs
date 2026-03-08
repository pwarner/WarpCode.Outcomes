namespace WarpCode.Outcomes.Tests;

public class ResultAggregationTests
{
    private static readonly Problem TestProblem = new(nameof(TestProblem));

    [Fact]
    public void Aggregate_ShouldCreateResultOfValueArray_WhenNoProblems()
    {
        var expected = new[] { 1, 2, 3 };
        var actual = NoIntProblems().Aggregate().Match(l => l, _ => []);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task AggregateAsync_ShouldCreateResultOfValueArray_WhenNoProblems()
    {
        var expected = new[] { 1, 2, 3 };
        var actual = (await AsyncOf(NoIntProblems()).AggregateAsync()).Match(l => l, _ => []);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Aggregate_ShouldCreateSuccessResult_WhenNoProblemsAndNoValues()
    {
        Result<None> expected = Result.Ok;
        Result<None> actual = NoProblems().Aggregate();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task AggregateAsync_ShouldCreateSuccessResult_WhenNoProblemsAndNoValues()
    {
        Result<None> expected = Result.Ok;
        Result<None> actual = await AsyncOf(NoProblems()).AggregateAsync();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Aggregate_ShouldCreateResultOfFirstProblem_WhenBailEarlyNoValues()
    {
        var expected = Result.FromProblem(TestProblem);
        Result<None> actual = SomeProblems().Aggregate(bailEarly: true);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task AggregateAsync_ShouldCreateResultOfFirstProblem_WhenBailEarlyNoValues()
    {
        var expected = Result.FromProblem(TestProblem);
        Result<None> actual = await AsyncOf(SomeProblems()).AggregateAsync(bailEarly: true);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Aggregate_ShouldCreateResultOfFirstProblem_WhenBailEarly()
    {
        var expected = Result.FromProblem<int[]>(TestProblem);
        Result<int[]> actual = SomeIntProblems().Aggregate(bailEarly: true);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task AggregateAsync_ShouldCreateResultOfFirstProblem_WhenBailEarly()
    {
        var expected = Result.FromProblem<int[]>(TestProblem);
        Result<int[]> actual = await AsyncOf(SomeIntProblems()).AggregateAsync(bailEarly: true);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Aggregate_ShouldCreateResultOfAllProblems_WhenNoValuesNoBailEarly()
    {
        Result<None> expected = new ProblemAggregate([TestProblem, TestProblem]);
        Result<None> actual = SomeProblems().Aggregate();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task AggregateAsync_ShouldCreateResultOfAllProblems_WhenNoValuesNoBailEarly()
    {
        Result<None> expected = new ProblemAggregate([TestProblem, TestProblem]);
        Result<None> actual = await AsyncOf(SomeProblems()).AggregateAsync();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Aggregate_ShouldCreateResultOfAllProblems_WhenNoBailEarly()
    {
        Result<int[]> expected = new ProblemAggregate([TestProblem, TestProblem]);
        Result<int[]> actual = SomeIntProblems().Aggregate();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task AggregateAsync_ShouldCreateResultOfAllProblems_WhenNoBailEarly()
    {
        Result<int[]> expected = new ProblemAggregate([TestProblem, TestProblem]);
        Result<int[]> actual = await AsyncOf(SomeIntProblems()).AggregateAsync();

        Assert.Equal(expected, actual);
    }

    private static IEnumerable<Result<None>> SomeProblems()
    {
        yield return Result.Ok;
        yield return TestProblem;
        yield return Result.Ok;
        yield return TestProblem;
    }

    private static IEnumerable<Result<int>> SomeIntProblems()
    {
        yield return 1;
        yield return TestProblem;
        yield return 2;
        yield return TestProblem;
    }

    private static IEnumerable<Result<None>> NoProblems()
    {
        yield return Result.Ok;
        yield return Result.Ok;
        yield return Result.Ok;
    }

    private static IEnumerable<Result<int>> NoIntProblems()
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

namespace WarpCode.Outcomes.Tests;

public class ProblemAggregateTests
{
    private static readonly Problem TestProblem = new(nameof(TestProblem));

    [Theory]
    [InlineData(1, "1 problem occurred. ")]
    [InlineData(2, "2 problems occurred. ")]
    [InlineData(3, "3 problems occurred. ")]
    public void Detail_ShouldStateHowManyProblemsOccurred(int count, string expectedStart)
        => Assert.StartsWith(expectedStart, new ProblemAggregate([.. Enumerable.Repeat(TestProblem, count)]).Detail);

    [Fact]
    public void Constructor_ShouldThrow_WhenProblemsIsNull()
        => Assert.Equal("Problems", Assert.Throws<ArgumentNullException>(() => new ProblemAggregate(null!)).ParamName);

    [Fact]
    public void Equals_ShouldBeFalse_WhenOtherIsNull()
        => Assert.False(new ProblemAggregate([TestProblem]).Equals(null));

    [Fact]
    public void Equals_ShouldBeTrue_WhenSameInstance()
    {
        var aggregate = new ProblemAggregate([TestProblem]);

        Assert.True(aggregate.Equals(aggregate));
    }

    [Fact]
    public void Equals_ShouldBeFalse_WhenInnerProblemsDiffer()
        => Assert.False(new ProblemAggregate([TestProblem]).Equals(new ProblemAggregate([new Problem("other")])));
}

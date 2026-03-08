namespace WarpCode.Outcomes.Tests;

public class ResultEqualityTests
{
    [Fact]
    public void Should_BeEqualIfResultsContainSameReferenceTypes()
    {
        const string test = "test";
        Result<string> x = test;
        Result<string> y = test;
        Assert.True(x.Equals(y));
        Assert.Equal(x.GetHashCode(), y.GetHashCode());
    }

    [Fact]
    public void Should_BeEqualIfResultsContainReferenceTypesThatAreEqual()
    {
        Result<TestValue> x = new TestValue(42, "test");
        Result<TestValue> y = new TestValue(42, "test");
        Assert.True(x.Equals(y));
        Assert.Equal(x.GetHashCode(), y.GetHashCode());
    }

    [Fact]
    public void Should_BeEqualIfResultsContainValueTypesThatAreEqual()
    {
        Result<decimal> x = 4.2m;
        Result<decimal> y = 4.2m;
        Assert.True(x.Equals(y));
        Assert.Equal(x.GetHashCode(), y.GetHashCode());
    }

    [Fact]
    public void Should_BeEqualIfResultsContainTheSameProblems()
    {
        var problem = new Problem("test");
        Result<None> x = problem;
        Result<None> y = problem;
        Assert.True(x.Equals(y));
        Assert.Equal(x.GetHashCode(), y.GetHashCode());
    }

    [Fact]
    public void Should_BeEqualIfResultsContainProblemsThatAreEqual()
    {
        Result<None> x = new Problem("test");
        Result<None> y = new Problem("test");
        Assert.True(x.Equals(y));
        Assert.Equal(x.GetHashCode(), y.GetHashCode());
    }

    [Fact]
    public void ProblemAggregateEquality_ShouldBeEqualIfInnerProblemsAreEqual()
    {
        var foo = new Problem("foo");
        var x = new ProblemAggregate(
        [
            foo,
            new Problem("bar")
        ]);
        var y = new ProblemAggregate(
        [
            foo,
            new Problem("bar")
        ]);
        Assert.True(x.Equals(y));
        Assert.Equal(x.GetHashCode(), y.GetHashCode());
    }

    internal record TestValue(int Int, string String);
}

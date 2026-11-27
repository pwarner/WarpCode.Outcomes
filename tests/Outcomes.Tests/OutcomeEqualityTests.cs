namespace WarpCode.Outcomes.Tests;

public class OutcomeEqualityTests
{
    [Fact]
    public void Should_BeEqualIfOutcomesContainSameReferenceTypes()
    {
        const string test = "test";
        Outcome<string> x = test;
        Outcome<string> y = test;
        Assert.True(x.Equals(y));
        Assert.Equal(x.GetHashCode(), y.GetHashCode());
    }

    [Fact]
    public void Should_BeEqualIfOutcomesContainReferenceTypesThatAreEqual()
    {
        Outcome<TestValue> x = new TestValue(42, "test");
        Outcome<TestValue> y = new TestValue(42, "test");
        Assert.True(x.Equals(y));
        Assert.Equal(x.GetHashCode(), y.GetHashCode());
    }

    [Fact]
    public void Should_BeEqualIfOutcomesContainValueTypesThatAreEqual()
    {
        Outcome<decimal> x = 4.2m;
        Outcome<decimal> y = 4.2m;
        Assert.True(x.Equals(y));
        Assert.Equal(x.GetHashCode(), y.GetHashCode());
    }

    [Fact]
    public void Should_BeEqualIfOutcomesContainTheSameProblems()
    {
        var problem = new Problem("test");
        Outcome<None> x = problem;
        Outcome<None> y = problem;
        Assert.True(x.Equals(y));
        Assert.Equal(x.GetHashCode(), y.GetHashCode());
    }

    [Fact]
    public void Should_BeEqualIfOutcomesContainProblemsThatAreEqual()
    {
        Outcome<None> x = new Problem("test");
        Outcome<None> y = new Problem("test");
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

    [Theory]
    [InlineData(1, 2)]
    [InlineData(0, -1)]
    public void Should_NotBeEqualIfOutcomesContainDifferentValues(int left, int right)
    {
        Outcome<int> x = left;
        Outcome<int> y = right;
        Assert.False(x.Equals(y));
        Assert.False(x == y);
        Assert.True(x != y);
    }

    [Fact]
    public void Should_NotBeEqualIfOneOutcomeHoldsAProblem()
    {
        Outcome<int> x = default(int);
        Outcome<int> y = new Problem("foo");
        Assert.False(x.Equals(y));
        Assert.True(x != y);
    }

    [Fact]
    public void EqualityOperator_ShouldBeTrue_WhenOutcomesAreEqual()
    {
        Outcome<int> x = 10;
        Outcome<int> y = 10;
        Assert.True(x == y);
        Assert.False(x != y);
    }

    [Fact]
    public void EqualsObject_ShouldCompareBoxedOutcomes()
    {
        Outcome<int> x = 10;
        Assert.True(x.Equals((object)Outcome.Of(10)));
        Assert.False(x.Equals((object)Outcome.Of(11)));
        Assert.False(x.Equals((object)10));
        Assert.False(x.Equals((object?)null));
    }

    internal sealed record TestValue(int Int, string String);
}

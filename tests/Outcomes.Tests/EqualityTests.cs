namespace WarpCode.Outcomes.Tests;

public class EqualityTests
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
        Outcome<string> x = problem.ToOutcome<string>();
        Outcome<string> y = problem.ToOutcome<string>();
        Assert.True(x.Equals(y));
        Assert.Equal(x.GetHashCode(), y.GetHashCode());
    }

    [Fact]
    public void Should_BeEqualIfOutcomesContainProblemsThatAreEqual()
    {
        Outcome<string> x = new Problem("test").ToOutcome<string>();
        Outcome<string> y = new Problem("test").ToOutcome<string>();
        Assert.True(x.Equals(y));
        Assert.Equal(x.GetHashCode(), y.GetHashCode());
    }

    [Fact]
    public void ProblemAggregateEquality_ShouldBeEqualIfInnerProblemsAreEqual()
    {
        var foo = new Problem("foo");
        var x = new ProblemAggregate(new[]
        {
            foo,
            new Problem("bar")
        });
        var y = new ProblemAggregate(new[]
        {
            foo,
            new Problem("bar")
        });
        Assert.True(x.Equals(y));
        Assert.Equal(x.GetHashCode(), y.GetHashCode());
    }

    internal record TestValue(int Int, string String);
}

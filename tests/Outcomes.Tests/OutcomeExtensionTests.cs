namespace Outcomes.Tests;

public class OutcomeExtensionTests
{
    private static readonly Problem TestProblem = new(nameof(TestProblem));

    [Fact]
    public void Should_CreateProblemOutcome_FromToOutcomeExtension()
    {
        Assert.Equal(new Outcome<None>(TestProblem), TestProblem.ToOutcome());
    }

    [Fact]
    public void Should_CreateStronglyTypesProblemOutcome_FromOutcomeExtension()
    {
        Assert.Equal(new Outcome<int>(TestProblem), TestProblem.ToOutcome());
    }

    [Fact]
    public void OnSuccess_ShouldBeInvokedWhenThereIsNoProblem()
    {
        bool invoked = false;

        Outcome.Ok().OnSuccess(_ => invoked = true);

        Assert.True(invoked);
    }

    [Fact]
    public void OnSuccess_ShouldNotBeInvokedWhenThereIsAProblem()
    {
        bool invoked = false;

        TestProblem.ToOutcome().OnSuccess(_ => invoked = true);

        Assert.False(invoked);
    }

    [Fact]
    public void OnProblem_ShouldBeInvokedWhenThereIsAProblem()
    {
        bool invoked = false;

        TestProblem.ToOutcome().OnProblem(_ => invoked = true);

        Assert.True(invoked);
    }

    [Fact]
    public void OnProblem_ShouldNotBeInvokedWhenThereIsNoProblem()
    {
        bool invoked = false;

        Outcome.Ok().OnProblem(_ => invoked = true);

        Assert.False(invoked);
    }

    [Fact]
    public void Ensure_ShouldCreateProblemOutcomeIfPredicateIsFalse()
    {
        Outcome<None> actual = Outcome.Ok().Ensure(_ => false, _ => TestProblem);

        Assert.Equal(TestProblem.ToOutcome(), actual);
    }

    [Fact]
    public void Ensure_ShouldNotAffectOutcomeIfPredicateIsTrue()
    {
        Outcome<None> actual = Outcome.Ok().Ensure(_ => true, _ => TestProblem);

        Assert.Equal(Outcome.Ok(), actual);
    }

    [Fact]
    public void Rescue_ShouldCreateSuccessOutcomeIfPredicateIsTrue()
    {
        const int expected = 13;

        Outcome<int> actual = new Outcome<int>(TestProblem).Rescue(_ => true, _ => expected);

        Assert.Equal(Outcome.Ok(expected), actual);
    }

    [Fact]
    public void Rescue_ShouldNotAffectOutcomeIfPredicateIsFalse()
    {
        Outcome<int> actual = new Outcome<int>(TestProblem).Rescue(_ => false, _ => 0);

        Assert.Equal(TestProblem.ToOutcome(), actual);
    }

    [Fact]
    public void Aggregate_ShouldCreateProblemAggregateForManyProblems()
    {
        Outcome<None> actual = ProblemOutcomes(3, 1).Aggregate();

        var expected = new ProblemAggregate(new List<IProblem>(2)
        {
            TestProblem,
            TestProblem
        });

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Aggregate_ShouldCreateOutcomeWithFirstOfManyProblemsIfBailEarlyIsTrue()
    {
        Outcome<None> actual = ProblemOutcomes(3, 1).Aggregate(bailEarly: true);

        Assert.Equal(TestProblem.ToOutcome(), actual);
    }

    [Fact]
    public void Aggregate_ShouldCreateOutcomeOfListOfValuesWhenNoProblems()
    {
        Outcome<List<int>?> actual = IntProblemOutcomes(3, 3).Aggregate();

        var expected = new List<int>(3) { 0, 1, 2 };

        List<int>? actualList = actual.Match(v => v, _ => null);

        Assert.Equal(expected, actualList);
    }

    [Fact]
    public void Aggregate_ShouldCreateSuccessOutcomeWhenNoProblemsAndNoValues()
    {
        Outcome<None> actual = ProblemOutcomes(3, 3).Aggregate();

        Assert.Equal(Outcome.Ok(), actual);
    }

    private static IEnumerable<Outcome<int>> IntProblemOutcomes(int total, int totalOk) =>
        Enumerable.Range(0, total)
            .Select(i =>
                i < totalOk
                    ? i
                    : new Outcome<int>(TestProblem)
            );

    private static IEnumerable<Outcome<None>> ProblemOutcomes(int total, int totalOk) =>
        Enumerable.Range(0, total)
            .Select(i =>
                i < totalOk
                    ? Outcome.Ok()
                    : TestProblem.ToOutcome()
            );
}

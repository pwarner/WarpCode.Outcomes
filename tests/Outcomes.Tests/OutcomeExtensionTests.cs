namespace Outcomes.Tests;

public class OutcomeExtensionTests
{
    private static readonly Problem TestProblem = new(nameof(TestProblem));

    [Fact]
    public void Outcome_OnSuccess_ShouldBeInvokedWhenThereIsNoProblem()
    {
        bool invoked = false;

        Outcome.Ok().OnSuccess(_ => invoked = true);

        Assert.True(invoked);
    }

    [Fact]
    public void Outcome_OnSuccess_ShouldNotBeInvokedWhenThereIsAProblem()
    {
        bool invoked = false;

        TestProblem.ToOutcome().OnSuccess(_ => invoked = true);

        Assert.False(invoked);
    }

    [Fact]
    public void Outcome_OnProblem_ShouldBeInvokedWhenThereIsAProblem()
    {
        bool invoked = false;

        TestProblem.ToOutcome().OnFail(_ => invoked = true);

        Assert.True(invoked);
    }

    [Fact]
    public void Outcome_OnProblem_ShouldNotBeInvokedWhenThereIsNoProblem()
    {
        bool invoked = false;

        Outcome.Ok().OnFail(_ => invoked = true);

        Assert.False(invoked);
    }

    [Fact]
    public void Outcome_Ensure_ShouldCreateProblemOutcomeIfPredicateIsFalse()
    {
        Outcome<None> actual = Outcome.Ok().Ensure(_ => false, _ => TestProblem);

        Assert.Equal(TestProblem.ToOutcome(), actual);
    }

    [Fact]
    public void Outcome_Ensure_ShouldNotAffectOutcomeIfPredicateIsTrue()
    {
        Outcome<None> actual = Outcome.Ok().Ensure(_ => true, _ => TestProblem);

        Assert.Equal(Outcome.Ok(), actual);
    }

    [Fact]
    public void Outcome_Rescue_ShouldCreateSuccessOutcomeIfPredicateIsTrue()
    {
        const int expected = 13;

        Outcome<int> actual = TestProblem.ToOutcome<int>().Rescue(_ => true, _ => expected);

        Assert.Equal(Outcome.Ok(expected), actual);
    }

    [Fact]
    public void Outcome_Rescue_ShouldNotAffectOutcomeIfPredicateIsFalse()
    {
        Outcome<int> actual = TestProblem.ToOutcome<int>().Rescue(_ => false, _ => 0);

        Assert.Equal(TestProblem.ToOutcome(), actual);
    }

    [Fact]
    public void Outcome_Aggregate_ShouldCreateProblemAggregateForManyProblems()
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
    public void Outcome_Aggregate_ShouldCreateOutcomeWithFirstOfManyProblemsIfBailEarlyIsTrue()
    {
        Outcome<None> actual = ProblemOutcomes(3, 1).Aggregate(bailEarly: true);

        Assert.Equal(TestProblem.ToOutcome<List<int>?>(), actual);
    }

    [Fact]
    public void Outcome_Aggregate_ShouldCreateOutcomeOfListOfValuesWhenNoProblems()
    {
        Outcome<List<int>?> actual = IntProblemOutcomes(3, 3).Aggregate();

        var expected = new List<int>(3) { 0, 1, 2 };

        var actualList = actual.Match(v => v, _ => new List<int>())!;

        Assert.Equal(expected, actualList);
    }

    [Fact]
    public void Outcome_Aggregate_ShouldCreateSuccessOutcomeWhenNoProblemsAndNoValues()
    {
        Outcome<None> actual = ProblemOutcomes(3, 3).Aggregate();

        Assert.Equal(Outcome.Ok(), actual);
    }

    private static IEnumerable<Outcome<int>> IntProblemOutcomes(int total, int totalOk) =>
        Enumerable.Range(0, total)
            .Select(i =>
                i < totalOk
                    ? i
                    : TestProblem.ToOutcome<int>()
            );

    private static IEnumerable<Outcome<None>> ProblemOutcomes(int total, int totalOk) =>
        Enumerable.Range(0, total)
            .Select(i =>
                i < totalOk
                    ? Outcome.Ok()
                    : TestProblem.ToOutcome()
            );
}

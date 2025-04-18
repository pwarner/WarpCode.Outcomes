namespace WarpCode.Outcomes.Tests;

public class OutcomeTests
{
    private static readonly Problem TestProblem = new(nameof(TestProblem));

    [Fact]
    public void Should_CreateOutcome_FromValueImplicitly()
    {
        Assert.Equal(new Outcome<int>(10), 10);
    }

    [Fact]
    public void Should_CreateOutcome_FromOfEntryHelper()
    {
        Assert.Equal(new Outcome<int>(10), Outcome.Of(10));
    }

    [Fact]
    public void Should_CreateValuelessOutcome_FromOkEntryHelper()
    {
        Assert.Equal(new Outcome<None>(default(None)), Outcome.Ok);
    }

    [Fact]
    public void Should_CreateProblemOutcome_Implicitly()
    {
        Assert.Equal(new Outcome<None>(TestProblem), TestProblem);
    }

    [Fact]
    public void Should_CreateProblemOutcome_FromToOutcomeExtension()
    {
        Assert.Equal(new Outcome<None>(TestProblem), TestProblem.ToOutcome());
    }

    [Fact]
    public void Should_CreateStronglyTypedProblemOutcome_FromToOutcomeExtension()
    {
        Assert.Equal(new Outcome<int>(TestProblem), TestProblem.ToOutcome<int>());
    }

    [Fact]
    public void Should_CreateProblemOutcome_FromProblemHelper()
    {
        Assert.Equal(new Outcome<None>(TestProblem), Outcome.Problem(TestProblem));
    }

    [Fact]
    public void Should_CreateStronglyTypedProblemOutcome_ProblemHelper()
    {
        Assert.Equal(new Outcome<int>(TestProblem), TestProblem.Problem<int>());
    }

    [Fact]
    public void Match_ShouldResolveWithOnSuccessFunction_WhenNoProblem()
    {
        Assert.True(Outcome.Ok.Match(_ => true, _ => false));
    }

    [Fact]
    public void Match_ShouldResolveWithOnProblemFunction_WhenProblem()
    {
        Assert.True(new Outcome<None>(TestProblem).Match(_ => false, _ => true));
    }

    [Fact]
    public async Task MatchAsync_ShouldResolveWithOnSuccessFunction_WhenNoProblem()
    {
        Assert.True(await Task.FromResult(Outcome.Ok).MatchAsync(_ => true, _ => false));
        Assert.True(await ValueTask.FromResult(Outcome.Ok).MatchAsync(_ => true, _ => false));
    }

    [Fact]
    public async Task MatchAsync_ShouldResolveWithOnProblemFunction_WhenProblem()
    {
        Outcome<None> outcome = TestProblem.ToOutcome();
        Assert.True(await Task.FromResult(outcome).MatchAsync(_ => false, _ => true));
        Assert.True(await ValueTask.FromResult(outcome).MatchAsync(_ => false, _ => true));
    }

    [Fact]
    public void OnSuccess_ShouldBeInvoked_WhenThereIsNoProblem()
    {
        var invoked = false;
        Outcome.Ok.OnSuccess(_ => invoked = true);
        Assert.True(invoked);
    }

    [Fact]
    public void OnSuccess_ShouldNotBeInvoked_WhenThereIsAProblem()
    {
        var invoked = false;
        new Outcome<None>(TestProblem).OnSuccess(_ => invoked = true);
        Assert.False(invoked);
    }

    [Fact]
    public async Task OnSuccessAsync_ShouldBeInvoked_WhenThereIsNoProblem()
    {
        var invoked = false;
        await Task.FromResult(Outcome.Ok).OnSuccessAsync(_ => invoked = true);
        Assert.True(invoked);

        invoked = false;
        await ValueTask.FromResult(Outcome.Ok).OnSuccessAsync(_ => invoked = true);
        Assert.True(invoked);
    }

    [Fact]
    public async Task OnSuccessAsync_ShouldNotBeInvoked_WhenThereIsAProblem()
    {
        Outcome<None> problem = TestProblem.ToOutcome();
        var invoked = false;
        await Task.FromResult(problem).OnSuccessAsync(_ => invoked = true);
        Assert.False(invoked);

        await ValueTask.FromResult(problem).OnSuccessAsync(_ => invoked = true);
        Assert.False(invoked);
    }

    [Fact]
    public void OnProblem_ShouldBeInvoked_WhenThereIsAProblem()
    {
        var invoked = false;
        new Outcome<None>(TestProblem).OnProblem(_ => invoked = true);
        Assert.True(invoked);
    }

    [Fact]
    public void OnProblem_ShouldNotBeInvoked_WhenThereIsNoProblem()
    {
        var invoked = false;
        Outcome.Ok.OnProblem(_ => invoked = true);
        Assert.False(invoked);
    }

    [Fact]
    public async Task OnProblemAsync_ShouldBeInvoked_WhenThereIsAProblem()
    {
        Outcome<None> problem = TestProblem.ToOutcome();
        var invoked = false;
        await Task.FromResult(problem).OnProblemAsync(_ => invoked = true);
        Assert.True(invoked);

        invoked = false;
        await ValueTask.FromResult(problem).OnProblemAsync(_ => invoked = true);
        Assert.True(invoked);
    }

    [Fact]
    public async Task OnProblemAsync_ShouldNotBeInvoked_WhenThereIsNoProblem()
    {
        var invoked = false;
        await Task.FromResult(Outcome.Ok).OnProblemAsync(_ => invoked = true);
        Assert.False(invoked);

        await ValueTask.FromResult(Outcome.Ok).OnProblemAsync(_ => invoked = true);
        Assert.False(invoked);
    }

    [Fact]
    public void Ensure_ShouldCreateProblemOutcome_WhenPredicateIsFalse()
    {
        Outcome<None> expected = TestProblem.ToOutcome();

        Outcome<None> actual = Outcome.Ok.Ensure(_ => false, _ => TestProblem);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Ensure_ShouldNotAffectOutcome_WhenPredicateIsTrue()
    {
        Outcome<None> expected = Outcome.Ok;

        Outcome<None> actual = Outcome.Ok.Ensure(_ => true, _ => TestProblem);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task EnsureAsync_ShouldCreateProblemOutcome_WhenPredicateIsFalse()
    {
        Outcome<None> expected = TestProblem.ToOutcome();

        Outcome<None> actual = await Task.FromResult(Outcome.Ok)
            .EnsureAsync(_ => false, _ => TestProblem);

        Assert.Equal(expected, actual);

        actual = await ValueTask.FromResult(Outcome.Ok)
            .EnsureAsync(_ => false, _ => TestProblem);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task EnsureAsync_ShouldNotAffectOutcome_WhenPredicateIsTrue()
    {
        Outcome<None> expected = Outcome.Ok;

        Outcome<None> actual = await Task.FromResult(Outcome.Ok)
            .EnsureAsync(_ => true, _ => TestProblem);

        Assert.Equal(expected, actual);

        actual = await ValueTask.FromResult(Outcome.Ok)
            .EnsureAsync(_ => true, _ => TestProblem);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Rescue_ShouldCreateSuccessOutcome_WhenPredicateIsTrue()
    {
        const int i = 13;
        Outcome<int> expected = Outcome.Of(i);

        Outcome<int> actual = TestProblem.ToOutcome<int>().Rescue(_ => true, _ => i);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Rescue_ShouldNotAffectOutcome_WhenPredicateIsFalse()
    {
        Outcome<int> expected = TestProblem.ToOutcome<int>();

        Outcome<int> actual = expected.Rescue(_ => false, _ => 0);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task RescueAsync_ShouldCreateSuccessOutcome_WhenPredicateIsTrue()
    {
        const int i = 13;
        Outcome<int> expected = Outcome.Of(i);

        Outcome<int> actual = await Task.FromResult(TestProblem.ToOutcome<int>())
            .RescueAsync(_ => true, _ => i);

        Assert.Equal(expected, actual);

        actual = await ValueTask.FromResult(TestProblem.ToOutcome<int>())
            .RescueAsync(_ => true, _ => i);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task RescueAsync_ShouldNotAffectOutcome_WhenPredicateIsFalse()
    {
        Outcome<int> expected = TestProblem.ToOutcome<int>();

        Outcome<int> actual = await Task.FromResult(expected)
            .RescueAsync(_ => false, _ => 0);

        Assert.Equal(expected, actual);

        actual = await ValueTask.FromResult(expected)
            .RescueAsync(_ => false, _ => 0);

        Assert.Equal(expected, actual);
    }
}
